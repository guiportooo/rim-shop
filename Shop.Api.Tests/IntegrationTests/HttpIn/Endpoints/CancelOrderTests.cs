namespace Shop.Api.Tests.IntegrationTests.HttpIn.Endpoints;

using Builders.Core.Models;
using Core.Models;
using Storage;

public class CancelOrderTests
{
    [Test]
    public async Task Should_cancel_order()
    {
        var order = new OrderBuilder().Build();

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Orders.Add(order);
        await arrangeDbContext.SaveChangesAsync();

        var response = await client.DeleteAsync($"orders/{order.Id}");

        using var assertScope = application.Services.CreateScope();
        await using var assertDbContext = assertScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await assertDbContext.Database.EnsureCreatedAsync();

        var createdOrder = await assertDbContext
            .Orders
            .FirstAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createdOrder.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Test]
    public async Task Should_return_not_found_when_order_is_already_cancelled()
    {
        var order = new OrderBuilder()
            .Cancelled()
            .Build();

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Orders.Add(order);
        await arrangeDbContext.SaveChangesAsync();

        var response = await client.DeleteAsync($"orders/{order.Id}");
        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse(@"{
                ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.4"",
                ""title"": ""Not Found"",
                ""status"": 404,
                ""detail"": ""Order with id 1 not found""
            }");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }

    [Test]
    public async Task Should_return_not_found_when_order_does_not_exists()
    {
        await using var application = new ShopApi();
        var client = application.CreateClient();

        var response = await client.DeleteAsync($"orders/1");
        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse(@"{
                    ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.4"",
                    ""title"": ""Not Found"",
                    ""status"": 404,
                    ""detail"": ""Order with id 1 not found""
                }");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }
}