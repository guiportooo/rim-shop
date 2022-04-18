namespace Shop.Api.Shared.Storage;

using Orders.Storage;

public static class Extensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ShopDbContext.ConnectionString);
        return services
            .AddSqlite<ShopDbContext>(connectionString)
            .AddOrdersStorage();
    }
}