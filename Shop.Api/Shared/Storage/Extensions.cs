namespace Shop.Api.Shared.Storage;

using Inventory.Core.Models;
using Inventory.Storage;
using Orders.Storage;

public static class Extensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ShopDbContext.ConnectionString);
        return services
            .AddSqlite<ShopDbContext>(connectionString)
            .AddOrdersStorage()
            .AddInventoryStorage();
    }

    public static async Task<WebApplication> SeedStorage(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        if (!app.Environment.IsDevelopment() || await dbContext.Products.AnyAsync())
        {
            return app;
        }

        dbContext.Products.AddRange(
            new Product {Code = Guid.Parse("ac7a1adc-0550-4dd3-a3e8-af15f24a280d"), Stock = 10},
            new Product {Code = Guid.Parse("a54b730d-5c3e-449d-b360-6090b6afdb41"), Stock = 15},
            new Product {Code = Guid.Parse("3b5876c0-15ff-45dd-83ef-88c4181f32e9"), Stock = 2});
        await dbContext.SaveChangesAsync();
        return app;
    }
}