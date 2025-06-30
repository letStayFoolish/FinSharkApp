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
  public DbSet<Portfolio> Portfolios { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<Portfolio>(p => p.HasKey(p => new {p.AppUserId, p.StockId}));
    // connect it to the table
    builder.Entity<Portfolio>()
      .HasOne(p => p.AppUser)
      .WithMany(u => u.Portfolios)
      .HasForeignKey(p => p.AppUserId);
    
    builder.Entity<Portfolio>()
      .HasOne(p => p.Stock)
      .WithMany(u => u.Portfolios)
      .HasForeignKey(p => p.StockId);
    
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