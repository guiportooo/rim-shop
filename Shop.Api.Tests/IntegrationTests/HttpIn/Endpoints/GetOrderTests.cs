namespace Shop.Api.Tests.IntegrationTests.HttpIn.Endpoints;

using Builders.Core.Models;
using Storage;

public class GetOrderTests
{
    [Test]
    public async Task Should_return_order_with_id()
    {
        var deliveryAddress = new DeliveryAddressBuilder().Build();

        var items = new[]
        {
            new ItemBuilder().Build()
        };

        var order = new OrderBuilder()
            .WithDeliveryAddress(deliveryAddress)
            .WithItems(items)
            .Build();

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Orders.Add(order);
        await arrangeDbContext.SaveChangesAsync();

        var response = await client.GetAsync($"orders/{order.Id}");

        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse($@"{{
           'data': {{
            'order': {{
                'id': {order.Id},
                'deliveryAddress': {{
                    'id': {deliveryAddress.Id},
                    'street': '{deliveryAddress.Street}',
                    'city': '{deliveryAddress.City}',
                    'postCode': '{deliveryAddress.PostCode}'
                }},
                'items': [
                    {{
                        'id': {items[0].Id},
                        'productId': '{items[0].ProductId}',
                        'quantity': {items[0].Quantity}
                    }}]
                }}
            }}}}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }

    [Test]
    public async Task Should_return_not_found_when_order_does_not_exists()
    {
        await using var application = new ShopApi();
        var client = application.CreateClient();
        
        var response = await client.GetAsync("orders/1");
        
        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse(@"{
            ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.4"",
            ""title"": ""Not Found"",
            ""status"": 404,
            ""detail"": ""Order not found""
        }");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }
}