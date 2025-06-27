using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
  // We do not want to leave context mutabable
  private readonly ApplicationDBContext _context;
  public StockController(ApplicationDBContext context)
  {
    _context = context;
  }

  [HttpGet] // read
  public IActionResult GetStocks()
  {
    // without ToList - returnging list as an object (Deffer execution);
    var stocks = _context.Stocks.ToList();
    
    // condition
    // if ()
    // if not ok
    // if ok
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
    
    return Ok(stock);
  }
}