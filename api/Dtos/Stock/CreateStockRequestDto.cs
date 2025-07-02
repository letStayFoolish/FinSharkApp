using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock;

public record CreateStockRequestDto
{
  [Required] [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 over characters long.")] public string Symbol { get; init; } = string.Empty;
  [Required] [MaxLength(10, ErrorMessage = "Company Name cannot be over 10 over characters long.")] public string CompanyName { get; init; } = string.Empty;
  [Required] [Range(1, 1000000000)] public decimal Purchase { get; init; }
  [Required] [Range(0.001, 100)] public decimal LastDiv { get; init; }
  [Required] [MaxLength(10, ErrorMessage = "Industry cannot be over 10 over characters long.")] public string Industry { get; init; } = string.Empty;
  [Range(1, 5000000000)] public long MarketCap { get; init;  }
}