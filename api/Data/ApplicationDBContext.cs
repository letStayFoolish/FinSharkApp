using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

// Inherit from DbContext
public class ApplicationDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
  // base allows us to pass up our db context into db context

  // adding tables
  // DbSet -> grabbing something from database: manipulating the whole Stock table
  public DbSet<Stock> Stocks { get; set; }
  public DbSet<Comment> Comments { get; set; }
}