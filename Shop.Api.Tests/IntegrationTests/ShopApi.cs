namespace Shop.Api.Tests.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Shared.Storage;

public class ShopApi : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder
            .ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ShopDbContext>));
                services.AddDbContext<ShopDbContext>(options =>
                    options.UseInMemoryDatabase($"ShopDb", root));
            })
            .UseEnvironment("Test");

        return base.CreateHost(builder);
    }
}