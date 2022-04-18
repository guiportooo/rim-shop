namespace Shop.Api.Tests.IntegrationTests.Orders.HttpIn.Endpoints;

using Builders.Orders.Core.Models;
using Builders.Orders.HttpIn.Requests;
using Shared.Storage;
using Storage;

public class UpdateOrderDeliveryAddressTests
{
    [Test]
    public async Task Should_update_order_delivery_address()
    {
        var deliveryAddress = new DeliveryAddressBuilder().Build();

        var order = new OrderBuilder()
            .WithDeliveryAddress(deliveryAddress)
            .Build();

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Orders.Add(order);
        await arrangeDbContext.SaveChangesAsync();

        var newDeliveryAddress = new DeliveryAddressRequestBuilder().Build();

        var request = new UpdateOrderDeliveryAddressRequestBuilder()
            .WithDeliveryAddress(newDeliveryAddress)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await client.PutAsync($"orders/{order.Id}/deliveryAddress", requestContent);
        
        using var assertScope = application.Services.CreateScope();
        await using var assertDbContext = assertScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await assertDbContext.Database.EnsureCreatedAsync();

        var createdOrder = await assertDbContext
            .Orders
            .Include(x => x.DeliveryAddress)
            .FirstAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createdOrder.DeliveryAddress.Should().BeEquivalentTo(newDeliveryAddress);
    }

    [Test]
    public async Task Should_return_bad_request_when_request_is_invalid()
    {
        var deliveryAddress = new DeliveryAddressRequestBuilder()
            .WithStreet(string.Empty)
            .Build();

        var request = new UpdateOrderDeliveryAddressRequestBuilder()
            .WithDeliveryAddress(deliveryAddress)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        await using var application = new ShopApi();
        var client = application.CreateClient();

        var response = await client.PutAsync("orders/1/deliveryAddress", requestContent);

        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse(@"{
            ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
            ""title"": ""One or more validation errors occurred."",
            ""status"": 400,
            ""errors"": {
                ""DeliveryAddress.Street"": [
                    ""'Street' must not be empty.""]
            }
        }");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }

    [Test]
    public async Task Should_return_not_found_when_order_does_not_exists()
    {
        var deliveryAddress = new DeliveryAddressRequestBuilder().Build();

        var request = new UpdateOrderDeliveryAddressRequestBuilder()
            .WithDeliveryAddress(deliveryAddress)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        await using var application = new ShopApi();
        var client = application.CreateClient();

        var response = await client.PutAsync("orders/1/deliveryAddress", requestContent);
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