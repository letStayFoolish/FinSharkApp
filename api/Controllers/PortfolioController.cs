using api.Extensions;
using api.Interfaces;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController : ControllerBase
{
  // Dependency Injection
  private readonly UserManager<AppUser> _appUserManager;
  private readonly IStockRepository _stockRepo;
  private readonly IPortfolioRepository _portfolioRepo;

  public PortfolioController(UserManager<AppUser> appUserManager, IStockRepository stockRepo,
    IPortfolioRepository portfolioRepo)
  {
    _appUserManager = appUserManager;
    _stockRepo = stockRepo;
    _portfolioRepo = portfolioRepo;
  }

  // Defining Endpoints
  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetUserPortfolio()
  {
    // User inherited from ControllerBase
    var username = User.GetUsername();

    var appuser = await _appUserManager.FindByNameAsync(username);

    var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appuser);
    return Ok(userPortfolio);
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> AddPortfolio(string symbol)
  {
    var username = User.GetUsername();
    var appuser = await _appUserManager.FindByNameAsync(username);

    var stock = await _stockRepo.GetBySymbolAsync(symbol);

    if (stock == null)
    {
      return NotFound("Stock not found");
    }

    var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appuser);

    if (userPortfolio.Any(p => string.Equals(p.Symbol, symbol, StringComparison.CurrentCultureIgnoreCase)))
    {
      return BadRequest("Stock already in portfolio");
    }

    var portfolioModel = new Portfolio
    {
      AppUser = appuser,
      Stock = stock
    };

    var newPortfolioObj = await _portfolioRepo.CreateAsync(portfolioModel);

    if (newPortfolioObj == null)
    {
      return StatusCode(500, "Could not create");
    }
    else
    {
      return Created();
    }
  }

  [HttpDelete]
  [Authorize]
  public async Task<IActionResult> DeletePortfolio(string symbol)
  {
    var username = User.GetUsername();
    var appuser = await _appUserManager.FindByNameAsync(username);
    var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appuser);

    var filteredStock =
      userPortfolio.Where(p => string.Equals(p.Symbol, symbol, StringComparison.CurrentCultureIgnoreCase)).ToList();

    if (filteredStock.Count == 1)
    {
      await _portfolioRepo.DeleteAsync(appuser, symbol);
    }
    else
    {
      return BadRequest("Stock not in your portfolio");
    }

    return Ok();
  }
}