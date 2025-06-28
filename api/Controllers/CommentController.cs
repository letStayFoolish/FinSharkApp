using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
  private readonly ICommentRepository _commentRepository;
  private readonly IStockRepository _stockRepository;

  public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
  {
    _commentRepository = commentRepository;
    _stockRepository = stockRepository;
  }

  [HttpGet] // read all comments
  public async Task<IActionResult> GetAllComments()
  {
    var comments = await _commentRepository.GetAllAsync();
    // DTO Comments...
    var commentsDto = comments.Select(c => c.ToCommentDto());
    return Ok(commentsDto);
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<IActionResult> GetCommentById([FromRoute] int id)
  {
    var existingComment = await _commentRepository.GetByIdAsync(id);

    if (existingComment == null)
    {
      return NotFound();
    }

    return Ok(existingComment.ToCommentDto());
  }

  [HttpPost]
  [Route("{stockId}")]
  public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
  {
    // find existing stock
    var existingStock = await _stockRepository.StockExists(stockId);
    if (!existingStock)
    {
      return BadRequest("Stock does not exist");
    }

    // existing stock update comment property only
    var commentModel = commentDto.ToCommentFromCreate(stockId);

    await _commentRepository.CreateAsync(commentModel);

    return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id }, commentModel.ToCommentDto());
  }
}