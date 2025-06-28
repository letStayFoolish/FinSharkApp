using api.Data;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
  private readonly ApplicationDbContext _context;

  public StockRepository(ApplicationDbContext context)
  {
    // DI - this is how we bring the database before actually use any of it 
    _context = context;
  }

  public async Task<List<Stock>> GetAllAsync()
  {
   return await _context.Stocks.ToListAsync();
    // var stockDto = stocks.Select(s => s.ToStockDto());
  }
}