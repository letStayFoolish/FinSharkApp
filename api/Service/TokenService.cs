using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace api.Service;

public class TokenService : ITokenService
{
  private readonly IConfiguration _configuration;
  private readonly SymmetricSecurityKey _key;
  // we need to pull things from `appsettings.json`, so that is why we need IConfiguration
  public TokenService(IConfiguration configuration)
  {
    _configuration = configuration;
    _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
  }
  public string CreateToken(AppUser user)
  {
    // putting claims inside token
    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
    };

    var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddDays(7),
      SigningCredentials = creds,
      Issuer = _configuration["JWT:Issuer"],
      Audience = _configuration["JWT:Audience"]
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token); // return token in a form of a string instead of object!
  }
}