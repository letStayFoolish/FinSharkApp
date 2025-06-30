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

    var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
    return Ok(userPortfolio);
  }
}