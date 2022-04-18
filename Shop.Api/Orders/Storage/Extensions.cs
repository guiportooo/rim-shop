namespace Shop.Api.Orders.Storage;

using Core.Repositories;
using Repositories;

public static class Extensions
{
    public static IServiceCollection AddOrdersStorage(this IServiceCollection services) =>
        services
            .AddScoped<IOrderRepository, OrderRepository>();
}