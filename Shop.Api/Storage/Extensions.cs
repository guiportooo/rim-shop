namespace Shop.Api.Storage;

using Core.Repositories;
using Repositories;

public static class Extensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ShopDbContext.ConnectionString);
        return services
            .AddSqlite<ShopDbContext>(connectionString)
            .AddScoped<IOrderRepository, OrderRepository>();
    }
}