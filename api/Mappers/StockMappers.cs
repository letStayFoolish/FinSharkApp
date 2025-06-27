using api.Dtos.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
  public static StockDto ToStockDto(this Stock stockModel)
  {
    return new StockDto
    {
      Id = stockModel.Id,
      Symbol = stockModel.Symbol,
      CompanyName = stockModel.CompanyName,
      Purchase = stockModel.Purchase,
      LastDiv = stockModel.LastDiv,
      Industry = stockModel.Industry,
      MarketCap = stockModel.MarketCap,
    };
  }

  public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockRequestDto)
  {
    return new Stock
    {
      // we do not need ID in the body of the POST request
      Symbol = stockRequestDto.Symbol,
      CompanyName = stockRequestDto.CompanyName,
      Purchase = stockRequestDto.Purchase,
      LastDiv = stockRequestDto.LastDiv,
      Industry = stockRequestDto.Industry,
      MarketCap = stockRequestDto.MarketCap,
    };
  }
}