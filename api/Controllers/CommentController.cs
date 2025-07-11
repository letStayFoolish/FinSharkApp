﻿using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
// CommentController derives from the class ControllerBase.
// in OOP this establishes an inheritance relationship
public class CommentController : ControllerBase
{
  // DI
  private readonly ICommentRepository _commentRepository;
  private readonly IStockRepository _stockRepository;
  private readonly UserManager<AppUser> _userManager;

  // Interfaces are being injected into the CommentController using its constructor
  // The dependencies (ICommentRepository, IStockRepository) are provided by the DI container when creating an instance of CommentController
  // e.g. builder.Services.AddScoped<ICommentRepository, CommentRepository>();
  // `AddScoped()` method registers the dependencies in the DI container and ensures that a new instance of the service is created for each HTTP request.
  public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager)
  {
    _commentRepository = commentRepository;
    _stockRepository = stockRepository;
    _userManager = userManager;
  }

  [HttpGet] // read all comments
  public async Task<IActionResult> GetAllComments()
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var comments = await _commentRepository.GetAllAsync();
    // DTO Comments: Mapping through each of the comments and turn it to DTO comment (we do not want to show all to the end-user)
    var commentsDto = comments.Select(c => c.ToCommentDto());
    return Ok(commentsDto);
  }

  [HttpGet]
  [Route("{id:int}")] // or [HttpGet("{ id }")]
  public async Task<IActionResult> GetCommentById([FromRoute] int id)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var existingComment = await _commentRepository.GetByIdAsync(id);

    if (existingComment == null)
    {
      return NotFound();
    }

    return Ok(existingComment.ToCommentDto());
  }

  [HttpPost]
  [Route("{stockId:int}")]
  public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    // find existing stock
    var existingStock = await _stockRepository.StockExistsAsync(stockId);
    if (!existingStock)
    {
      return BadRequest("Stock does not exist");
    }

    var username = User.GetUsername();
    var appUser = await _userManager.FindByNameAsync(username);
    
    // existing stock update comment property only
    var commentModel = commentDto.ToCommentFromCreate(stockId);
    commentModel.AppUserId = appUser.Id;
    await _commentRepository.CreateAsync(commentModel);

    return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id }, commentModel.ToCommentDto());
  }

  [HttpPut]
  [Route("{id:int}")]
  public async Task<IActionResult> Update([FromRoute] int id,
    [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var existingComment = await _commentRepository.UpdateAsync(id, updateCommentRequestDto.ToCommentFromUpdate());

    if (existingComment == null)
    {
      return NotFound("Comment not found");
    }

    return Ok(existingComment.ToCommentDto());
  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<IActionResult> Delete([FromRoute] int id)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var commentModel = await _commentRepository.DeleteAsync(id);

    if (commentModel == null)
    {
      return NotFound("Comment not found");
    }

    return NoContent();
  }
}