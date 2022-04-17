namespace Shop.Api.Tests.IntegrationTests.HttpIn.Endpoints;

using System.Net;
using System.Threading.Tasks;
using Builders.Core.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Storage;

public class GetOrdersTests
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

        using var scope = application.Services.CreateScope();
        var provider = scope.ServiceProvider;
        await using var dbContext = provider.GetRequiredService<ShopDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

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
    }
}