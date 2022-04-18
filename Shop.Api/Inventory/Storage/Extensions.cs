namespace Shop.Api.Inventory.Storage;

using Core.Repositories;
using Repositories;

public static class Extensions
{
    public static IServiceCollection AddInventoryStorage(this IServiceCollection services) =>
        services
            .AddScoped<IProductRepository, ProductRepository>();
}