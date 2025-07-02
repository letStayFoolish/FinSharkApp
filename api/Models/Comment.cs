using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Comments")]
public class Comment
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Content { get; set; }
  public DateTime CreatedOn { get; set; } = DateTime.Now;
  public int? StockId { get; set; } // Navigation property!!!
  public Stock? Stock { get; set; } //
  public string? AppUserId { get; set; }
  public AppUser? AppUser { get; set; }
}