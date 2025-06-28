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

  public CommentController(ICommentRepository commentRepository)
  {
    _commentRepository = commentRepository;
  }

  [HttpGet] // read all comments
  public async Task<IActionResult> GetAllComments()
  {
    var comments = await _commentRepository.GetAllAsync();
    // DTO Comments...
    var commentsDto = comments.Select(c => c.ToCommentDto());
    return Ok(commentsDto);
  }

}