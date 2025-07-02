using api.Dtos.Comment;

namespace api.Dtos.Stock;

public record StockDto
{
  public int Id { get; init; }
  public string Symbol { get; init; } = string.Empty;
  public string CompanyName { get; init; } = string.Empty;
  public decimal Purchase { get; init; }
  public decimal LastDiv { get; init; }
  public string Industry { get; init; } = string.Empty;
  public long MarketCap { get; init; }
  // Comments are secret (first part of the tutorial)
  // Now we can show comments
  public List<CommentDto> Comments { get; init; }
}