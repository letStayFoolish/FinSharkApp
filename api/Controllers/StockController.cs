using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
  private readonly IStockRepository _stockRepository;
  public StockController(IStockRepository stockRepository)
  {
    _stockRepository = stockRepository;
  }

  [HttpGet] // read
  [Authorize]
  public async Task<IActionResult> GetAllStocks([FromQuery] QueryObject query)
  {
    // Validation according to what I set within the Model
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    // without ToList - returning list as an object (Deffer execution);
    var stocks = await _stockRepository.GetAllAsync(query);
    // Limit information that we show/send to the end-user
    var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

    return Ok(stockDto);
  }

  [HttpGet("{id:int}")] // read by the id (id from a query)
  public async Task<IActionResult> GetStockById([FromRoute] int id)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var stock = await _stockRepository.GetByIdAsync(id);

    if (stock == null)
    {
      return NotFound();
    }

    return Ok(stock.ToStockDto());
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockRequestDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var stockModel = stockRequestDto.ToStockFromCreateDto();

    await _stockRepository.CreateAsync(stockModel);

    return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
  }

  [HttpPut]
  [Route("{id:int}")] // [ Route( "api/stock/{id}" )]
  public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStock)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var stockModel = await _stockRepository.UpdateAsync(id, updateStock);

    if (stockModel == null)
    {
      return NotFound();
    }

    return Ok(stockModel.ToStockDto());
  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<IActionResult> Delete([FromRoute] int id)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    // Find an existing stock object
    var stockModel = await _stockRepository.DeleteAsync(id);

    if (stockModel == null)
    {
      return NotFound();
    }

    return NoContent();
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteAll()
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var success = await _stockRepository.DeleteAllAsync();

    if (success)
    {
      return NoContent();
    }

    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete records.\n");
  }
}