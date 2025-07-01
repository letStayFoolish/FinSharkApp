using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository : IPortfolioRepository
{
  // Dependency Injection
  private readonly ApplicationDbContext _context;

  public PortfolioRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
  {
    return await _context.Portfolios.Where(p => p.AppUserId == user.Id).Select(stock => new Stock
    {
      Id = stock.StockId,
      Symbol = stock.Stock.Symbol,
      CompanyName = stock.Stock.CompanyName,
      Purchase = stock.Stock.Purchase,
      LastDiv = stock.Stock.LastDiv,
      Industry = stock.Stock.Industry,
      MarketCap = stock.Stock.MarketCap,
    }).ToListAsync();
  }

  public async Task<Portfolio?> CreateAsync(Portfolio portfolioModel)
  {
    await _context.Portfolios.AddAsync(portfolioModel);
    await _context.SaveChangesAsync();
    return portfolioModel;
  }

  public async Task<Portfolio?> DeleteAsync(AppUser appuser, string symbol)
  {
    var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == appuser.Id && p.Stock.Symbol == symbol);
    if (portfolioModel == null)
    {
      return null;
    }

    _context.Portfolios.Remove(portfolioModel);
    await _context.SaveChangesAsync();
    return portfolioModel;
  }
}