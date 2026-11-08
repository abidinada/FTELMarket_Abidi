using FTELMarket_Abidi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace FTELMarket_Abidi.Infrastructure;

public class FtelMarketDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public FtelMarketDbContext(DbContextOptions<FtelMarketDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FtelMarketDbContext).Assembly);
    }
}