using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
  // Dependecy Injection in action
  private readonly ApplicationDbContext _context;
  public CommentRepository(ApplicationDbContext context)
  {
    _context = context;
  }
  
  public async Task<List<Comment>> GetAllAsync()
  {
    return await _context.Comments.ToListAsync();
  }

  public async Task<Comment?> GetByIdAsync(int id)
  {
   return await _context.Comments.FindAsync(id);
  }

  public async Task<Comment> CreateAsync(Comment commentModel)
  {
    await _context.Comments.AddAsync(commentModel);
    
    await _context.SaveChangesAsync();
    
    return commentModel;
  }

  public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto commentDto)
  {
    var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

    if (existingComment == null)
    {
      return null;
    }
    
    existingComment.Title = commentDto.Title;
    existingComment.Content = commentDto.Content;

    _context.SaveChangesAsync();
    return existingComment;
  }
}