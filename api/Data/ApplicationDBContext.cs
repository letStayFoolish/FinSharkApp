using api.Models;
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
}