using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account;

public class LoginDto
{
  [Required]
  public string Username { get; set; } = string.Empty; // avoiding null values
  [Required]
  public string Password { get; set; } = string.Empty;
}