using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

// Inherit from DbContext
public class ApplicationDBContext : DbContext
{
  public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
  {
    // base allows us to pass up our dbcontext into db context
  }
  
  // adding tables
  // DbSet -> grabbing something from database: manipulating the whole Stock table
  public DbSet<Stock> Stocks { get; set; }
  public DbSet<Comment> Comments { get; set; }
}