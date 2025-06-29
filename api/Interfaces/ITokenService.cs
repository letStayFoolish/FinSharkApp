using api.Models;

namespace api.Service;

public interface ITokenService
{
  public string CreateToken(AppUser user);
}