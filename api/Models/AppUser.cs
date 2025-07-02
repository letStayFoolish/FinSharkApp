using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUser : IdentityUser
{
  // public int Risk { get; set; }
  public List<Portfolio> Portfolios { get; } = [];
}