namespace Shop.Api.Shared.Storage;

using Inventory.Core.Models;
using Orders.Core.Models;

public class ShopDbContext : DbContext
{
    public const string ConnectionString = "ShopDb";
    
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();
}