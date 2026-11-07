using Microsoft.EntityFrameworkCore;
using FTELMarket.Data.Entities;
namespace FTELMarket.Data;

public class FTELMarketDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public FTELMarketDbContext(DbContextOptions<FTELMarketDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FTELMarketDbContext).Assembly);
    }
}