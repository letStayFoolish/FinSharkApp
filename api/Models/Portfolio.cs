using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Portfolios")]
public class Portfolio
{
  // IDs - Linking user id and stock id
  public string AppUserId { get; set; }
  public int StockId { get; set; }
  // navigation properties (for us developers)
  public AppUser AppUser { get; set; }
  public Stock Stock { get; set; }
}