using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
  // We do not want to leave context mutable
  private readonly ApplicationDbContext _context;
  public StockController(ApplicationDbContext context)
  {
    _context = context;
  }

  [HttpGet] // read
  public IActionResult GetStocks()
  {
    // without ToList - returning list as an object (Deffer execution);
    var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());
    
    return Ok(stocks);
  }

  [HttpGet("{id}")]
  public IActionResult GetStockById([FromRoute] int id)
  {
    var stock = _context.Stocks.Find(id);

    if (stock == null)
    {
      return NotFound();
    }
    
    return Ok(stock.ToStockDto());
  }

  [HttpPost]
  public IActionResult Create([FromBody] CreateStockRequestDto stockRequestDto)
  {
    var stockModel = stockRequestDto.ToStockFromCreateDto();
    
    _context.Add(stockModel);
    _context.SaveChanges();
    
    return CreatedAtAction(nameof(GetStockById), new {id = stockModel.Id}, stockModel.ToStockDto());
  }

  [HttpPut]
  [Route("{id}")] // [ Route( "api/stock/{id}" )]
  public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStock)
  {
    var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

    if (stockModel == null)
    {
      return NotFound();
    }
    
    stockModel.Symbol = updateStock.Symbol;
    stockModel.CompanyName = updateStock.CompanyName;
    stockModel.Purchase = updateStock.Purchase;
    stockModel.LastDiv = updateStock.LastDiv;
    stockModel.Industry = updateStock.Industry;
    stockModel.MarketCap = updateStock.MarketCap;
    
    _context.SaveChanges();
    
    return Ok(stockModel.ToStockDto());
  }
}