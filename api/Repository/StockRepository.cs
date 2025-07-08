using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
  // Dependency Injection
  private readonly ApplicationDbContext _context;

  public StockRepository(ApplicationDbContext context)
  {
    // DI - this is how we bring the database before actually use any of it 
    _context = context;
    Console.WriteLine("StockRepository instantiated!");
  }

  public async Task<List<Stock>> GetAllAsync(QueryObject query)
  {
    // before adding filtering functionality:
    // return await _context.Stocks.Include(item => item.Comments).ToListAsync();
    // After adding filtering:
    var stocks = _context.Stocks.Include(item => item.Comments).ThenInclude(c => c.AppUser).AsQueryable();

    if (!string.IsNullOrWhiteSpace(query.CompanyName))
    {
      stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
    }

    if (!string.IsNullOrWhiteSpace(query.Symbol))
    {
      stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
    }

    if (!string.IsNullOrWhiteSpace(query.SortBy))
    {
      if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
      {
        stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
      }
    }

    var skipNumber = (query.PageNumber - 1) * query.PageSize;

    return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
  }

  public async Task<Stock?> GetByIdAsync(int id)
  {
    return await _context.Stocks.Include(item => item.Comments).FirstOrDefaultAsync(s => s.Id == id);
  }

  public async Task<Stock?> GetBySymbolAsync(string symbol)
  {
    return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
  }

  public async Task<Stock> CreateAsync(Stock stockModel)
  {
    await _context.AddAsync(stockModel);
    await _context.SaveChangesAsync();

    return stockModel;
  }

  public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
  {
    var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);


    if (existingStock == null)
    {
      return null;
    }

    existingStock.Symbol = stockDto.Symbol;
    existingStock.CompanyName = stockDto.CompanyName;
    existingStock.Purchase = stockDto.Purchase;
    existingStock.LastDiv = stockDto.LastDiv;
    existingStock.Industry = stockDto.Industry;
    existingStock.MarketCap = stockDto.MarketCap;

    await _context.SaveChangesAsync();

    return existingStock;
  }

  public async Task<Stock?> DeleteAsync(int id)
  {
    var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

    if (existingStock == null)
    {
      return null;
    }

    _context.Remove(existingStock);
    await _context.SaveChangesAsync();

    return existingStock;
  }

  public async Task<bool> DeleteAllAsync()
  {
    var allStocks = await _context.Stocks.ToListAsync();
    _context.RemoveRange(allStocks);
    await _context.SaveChangesAsync();

    return true;
  }

  public async Task<bool> StockExistsAsync(int id)
  {
    return await _context.Stocks.AnyAsync(stock => stock.Id == id);
  }
}