using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Stocks")]
public class Stock
{
  public int Id { get; init; }
  public required string Symbol { get; set; }
  public string CompanyName { get; set; } = string.Empty;
  [Column(TypeName = "decimal(18, 2)")] public decimal Purchase { get; set; }
  [Column(TypeName = "decimal(18, 2)")] public decimal LastDiv { get; set; }
  public string Industry { get; set; } = string.Empty;
  public long MarketCap { get; set; }
  public List<Comment> Comments { get; set; } = [];
  public List<Portfolio> Portfolios { get; set; } = [];
}