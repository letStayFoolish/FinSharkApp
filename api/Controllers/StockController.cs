using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
  // We do not want to leave context mutable
  private readonly ApplicationDbContext _context;
  private readonly IStockRepository _stockRepository;

  public StockController(ApplicationDbContext context, IStockRepository stockRepository)
  {
    _context = context;
    _stockRepository = stockRepository;
  }

  [HttpGet] // read
  public async Task<IActionResult> GetStocks()
  {
    // without ToList - returning list as an object (Deffer execution);
    var stocks = await _stockRepository.GetAllAsync();
    var stockDto = stocks.Select(s => s.ToStockDto());

    return Ok(stockDto);
  }

  [HttpGet("{id}")] // read by the id (id from query)
  public async Task<IActionResult> GetStockById([FromRoute] int id)
  {
    var stock = await _context.Stocks.FindAsync(id);

    if (stock == null)
    {
      return NotFound();
    }

    return Ok(stock.ToStockDto());
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockRequestDto)
  {
    var stockModel = stockRequestDto.ToStockFromCreateDto();

    await _context.AddAsync(stockModel);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
  }

  [HttpPut]
  [Route("{id}")] // [ Route( "api/stock/{id}" )]
  public async  Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStock)
  {
    var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

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

    await _context.SaveChangesAsync();

    return Ok(stockModel.ToStockDto());
  }

  [HttpDelete]
  [Route("{id}")]
  public async Task<IActionResult> Delete([FromRoute] int id)
  {
    // Find an existing stock object
    var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

    if (stockModel == null)
    {
      return NotFound();
    }

    _context.Remove(stockModel);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}