namespace Shop.Api.Storage;

using Core.Models;

public class ShopDbContext : DbContext
{
    public const string ConnectionString = "ShopDb";
    
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
}