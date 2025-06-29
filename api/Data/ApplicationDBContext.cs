using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

// Inherit from DbContext
public class ApplicationDbContext : IdentityDbContext<AppUser>
{
  // base allows us to pass up our db context into db context
  public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
  {
  }
  // adding tables
  // DbSet -> grabbing something from database: manipulating the whole Stock table
  public DbSet<Stock> Stocks { get; set; }
  public DbSet<Comment> Comments { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    
    List<IdentityRole> roles = new List<IdentityRole>
    {
      new IdentityRole
      {
        Id = "1", // Static, unique identifier
        Name = "Admin",
        NormalizedName = "ADMIN" // CAPITALIZE
      },
      new IdentityRole
      {
        Id = "2", // Static, unique identifier
        Name = "User",
        NormalizedName = "USER"
      },
    };
    builder.Entity<IdentityRole>().HasData(roles);
  }
}