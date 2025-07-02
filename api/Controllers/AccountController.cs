using api.Dtos.Account;
using api.Models;
using api.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
  private readonly UserManager<AppUser> _userManager;
  private readonly ITokenService _tokenService;
  private readonly SignInManager<AppUser> _signInManager;

  public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
    SignInManager<AppUser> signInManager)
  {
    _userManager = userManager;
    _tokenService = tokenService;
    _signInManager = signInManager;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginDto loginDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var existingUser = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.Username);

    if (existingUser == null)
    {
      return Unauthorized("Invalid username!");
    }

    var result = await _signInManager.CheckPasswordSignInAsync(existingUser, loginDto.Password, false);

    if (!result.Succeeded)
    {
      return Unauthorized("Invalid password!");
    }

    return Ok(
      new NewUserDto
      {
        UserName = existingUser.UserName?? "",
        Email = existingUser.Email ?? "",
        Token = _tokenService.CreateToken(existingUser)
      }
    );
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = new AppUser
      {
        UserName = registerDto.Username,
        Email = registerDto.Email
      };

      var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
      // CreateAsync - will return an object with a lot of stuff we can use for conditions, etc...
      if (createdUser.Succeeded)
      {
        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
        if (roleResult.Succeeded)
        {
          return Ok(
            new NewUserDto
            {
              UserName = appUser.UserName?? "",
              Email = appUser.Email?? "",
              Token = _tokenService.CreateToken(appUser)
            }
          );
        }
        else
        {
          return StatusCode(500, roleResult.Errors);
        }
      }
      else
      {
        return StatusCode(500, createdUser.Errors);
      }
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }
}