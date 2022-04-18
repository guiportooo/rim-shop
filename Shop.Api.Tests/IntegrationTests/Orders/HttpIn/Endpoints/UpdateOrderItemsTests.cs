namespace Shop.Api.Tests.IntegrationTests.Orders.HttpIn.Endpoints;

using Builders.Orders.Core.Models;
using Builders.Orders.HttpIn.Requests;
using Shared.Storage;
using Storage;

public class UpdateOrderItemsTests
{
    [Test]
    public async Task Should_update_order_items()
    {
        var items = new[]
        {
            new ItemBuilder().Build()
        };

        var order = new OrderBuilder()
            .WithItems(items)
            .Build();

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Orders.Add(order);
        await arrangeDbContext.SaveChangesAsync();

        var newItems = new[]
        {
            new ItemRequestBuilder().Build(),
            new ItemRequestBuilder().Build()
        };

        var request = new UpdateOrderItemsRequestBuilder()
            .WithItems(newItems)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await client.PutAsync($"orders/{order.Id}/items", requestContent);

        using var assertScope = application.Services.CreateScope();
        await using var assertDbContext = assertScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await assertDbContext.Database.EnsureCreatedAsync();

        var createdOrder = await assertDbContext
            .Orders
            .Include(x => x.Items)
            .FirstAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createdOrder.Items.Should().BeEquivalentTo(newItems);
    }

    [Test]
    public async Task Should_return_bad_request_when_request_is_invalid()
    {
        var items = new[]
        {
            new ItemRequestBuilder()
                .WithQuantity(0)
                .Build()
        };

        var request = new UpdateOrderItemsRequestBuilder()
            .WithItems(items)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        await using var application = new ShopApi();
        var client = application.CreateClient();

        var response = await client.PutAsync("orders/1/items", requestContent);

        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse(@"{
            ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
            ""title"": ""One or more validation errors occurred."",
            ""status"": 400,
            ""errors"": {
                ""Items[0].Quantity"": [
                    ""'Quantity' must be greater than '0'.""]
            }
        }");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }

    [Test]
    public async Task Should_return_not_found_when_order_does_not_exists()
    {
        var items = new[]
        {
            new ItemRequestBuilder().Build()
        };

        var request = new UpdateOrderItemsRequestBuilder()
            .WithItems(items)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        await using var application = new ShopApi();
        var client = application.CreateClient();

        var response = await client.PutAsync("orders/1/items", requestContent);
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