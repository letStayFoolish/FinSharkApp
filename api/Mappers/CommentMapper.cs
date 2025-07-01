using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
  public static CommentDto ToCommentDto(this Comment commentModel)
  {
    return new CommentDto
    {
      Id = commentModel.Id,
      Title = commentModel.Title,
      Content = commentModel.Content,
      CreatedOn = commentModel.CreatedOn,
      StockId = commentModel.StockId,
      CreatedBy = commentModel.AppUser?.UserName
    };
  }
  
  public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId)
  {
    return new Comment
    {
      Title = commentDto.Title,
      Content = commentDto.Content,
      StockId = stockId,
      // the rest is added automatically by the C# program as we set it in Comment model
    };
  }
  
  public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto)
  {
    return new Comment
    {
      Title = commentDto.Title,
      Content = commentDto.Content,
      // the rest is added automatically by the C# program as we set it in Comment model
    };
  }
}