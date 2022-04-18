namespace Shop.Api.Tests.IntegrationTests.Orders.HttpIn.Endpoints;

using System.Linq;
using Builders.Orders.Core.Models;
using Shared.Storage;
using Storage;

public class GetOrdersTests
{
    [Test]
    public async Task Should_return_paginated_orders()
    {
        const int pageNumber = 4;
        const int pageSize = 2;

        var orders = new OrderBuilder()
            .WithOneItem()
            .Build(10)
            .ToList();

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Orders.AddRange(orders);
        await arrangeDbContext.SaveChangesAsync();

        var response = await client.GetAsync($"orders?pageNumber={pageNumber}&pageSize={pageSize}");

        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        using var assertScope = application.Services.CreateScope();
        await using var assertDbContext = assertScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await assertDbContext.Database.EnsureCreatedAsync();

        var createdOrder = await assertDbContext
            .Orders
            .Include(x => x.DeliveryAddress)
            .Include(x => x.Items)
            .FirstAsync();

        var firstOrder = orders[6];
        var firstDeliveryAddress = firstOrder.DeliveryAddress;
        var firstItem = firstOrder.Items.First();

        var secondOrder = orders[7];
        var secondDeliveryAddress = secondOrder.DeliveryAddress;
        var secondItem = secondOrder.Items.First();

        var expectedContent = JToken.Parse($@"{{
           'data': {{
            'orders': [{{
                'id': {firstOrder.Id},
                'status': 'Pending',
                'deliveryAddress': {{
                    'id': {firstDeliveryAddress.Id},
                    'street': '{firstDeliveryAddress.Street}',
                    'city': '{firstDeliveryAddress.City}',
                    'postCode': '{firstDeliveryAddress.PostCode}'
                }},
                'items': [
                    {{
                        'id': {firstItem.Id},
                        'productCode': '{firstItem.ProductCode}',
                        'quantity': {firstItem.Quantity}
                    }}]
                }}, {{
                'id': {secondOrder.Id},
                'status': 'Pending',
                'deliveryAddress': {{
                    'id': {secondDeliveryAddress.Id},
                    'street': '{secondDeliveryAddress.Street}',
                    'city': '{secondDeliveryAddress.City}',
                    'postCode': '{secondDeliveryAddress.PostCode}'
                }},
                'items': [
                    {{
                        'id': {secondItem.Id},
                        'productCode': '{secondItem.ProductCode}',
                        'quantity': {secondItem.Quantity}
                    }}]
                }}]
            }}}}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }

    [Test]
    public async Task Should_return_not_found_when_no_orders_exist()
    {
        const int pageNumber = 4;
        const int pageSize = 2;

        var orders = new OrderBuilder()
            .WithOneItem()
            .Build(2)
            .ToList();

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Orders.AddRange(orders);
        await arrangeDbContext.SaveChangesAsync();

        var response = await client.GetAsync($"orders?pageNumber={pageNumber}&pageSize={pageSize}");

        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse(@"{
            ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.4"",
            ""title"": ""Not Found"",
            ""status"": 404,
            ""detail"": ""No orders found""
        }");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }
}