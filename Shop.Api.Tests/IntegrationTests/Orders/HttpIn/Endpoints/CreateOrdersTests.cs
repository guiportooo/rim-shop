namespace Shop.Api.Tests.IntegrationTests.Orders.HttpIn.Endpoints;

using System.Linq;
using Api.Orders.Core.Models;
using Builders.Inventory.Core.Models;
using Builders.Orders.HttpIn.Requests;
using Shared.Storage;

public class CreateOrdersTests
{
    [Test]
    public async Task Should_create_accepted_order_and_return_created()
    {
        var productCode1 = Guid.NewGuid();
        var productCode2 = Guid.NewGuid();
        const int itemQuantity1 = 15;
        const int itemQuantity2 = 7;
        const int productQuantity1 = 20;
        const int productQuantity2 = 10;

        var deliveryAddress = new DeliveryAddressRequestBuilder().Build();

        var items = new[]
        {
            new ItemRequestBuilder()
                .WithProductCode(productCode1)
                .WithQuantity(itemQuantity1)
                .Build(),
            new ItemRequestBuilder()
                .WithProductCode(productCode2)
                .WithQuantity(itemQuantity2)
                .Build()
        };

        var products = new[]
        {
            new ProductBuilder()
                .WithCode(productCode1)
                .WithStock(productQuantity1)
                .Build(),
            new ProductBuilder()
                .WithCode(productCode2)
                .WithStock(productQuantity2)
                .Build()
        };

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Products.AddRange(products);
        await arrangeDbContext.SaveChangesAsync();

        var request = new CreateOrderRequestBuilder()
            .WithDeliveryAddress(deliveryAddress)
            .WithItems(items)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync("orders", requestContent);

        using var assertScope = application.Services.CreateScope();
        await using var assertDbContext = assertScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await assertDbContext.Database.EnsureCreatedAsync();

        var createdOrder = await assertDbContext
            .Orders
            .Include(x => x.DeliveryAddress)
            .Include(x => x.Items)
            .FirstAsync();

        var expectedCreatedOrder = new Order
        {
            Status = OrderStatus.Accepted,
            DeliveryAddress = new DeliveryAddress(request.DeliveryAddress.Street,
                request.DeliveryAddress.City,
                request.DeliveryAddress.PostCode),
            Items = new[]
            {
                new Item(items[0].ProductCode, items[0].Quantity),
                new Item(items[1].ProductCode, items[1].Quantity)
            }
        };

        var productsAfterOrder = assertDbContext.Products.ToList();

        var expectedProducts = new[]
        {
            new ProductBuilder()
                .WithCode(productCode1)
                .WithStock(5)
                .Build(),
            new ProductBuilder()
                .WithCode(productCode2)
                .WithStock(3)
                .Build()
        };
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"http://localhost/orders/{createdOrder.Id}");
        createdOrder.Should().BeEquivalentTo(expectedCreatedOrder, opt => opt
            .Excluding(x => x.Id)
            .Excluding(x => x.DeliveryAddress.Id)
            .Using<Item>(ctx => ctx.Subject.Should().BeEquivalentTo(ctx.Expectation, o => o.Excluding(x => x.Id)))
            .WhenTypeIs<Item>());
        productsAfterOrder.Should().BeEquivalentTo(expectedProducts, opt => opt.Excluding(x => x.Id));
    }

    [Test]
    public async Task Should_create_rejected_order_and_return_created()
    {
        var productCode1 = Guid.NewGuid();
        var productCode2 = Guid.NewGuid();
        const int itemQuantity1 = 20;
        const int itemQuantity2 = 10;
        const int productQuantity1 = 19;
        const int productQuantity2 = 10;

        var deliveryAddress = new DeliveryAddressRequestBuilder().Build();

        var items = new[]
        {
            new ItemRequestBuilder()
                .WithProductCode(productCode1)
                .WithQuantity(itemQuantity1)
                .Build(),
            new ItemRequestBuilder()
                .WithProductCode(productCode2)
                .WithQuantity(itemQuantity2)
                .Build()
        };

        var products = new[]
        {
            new ProductBuilder()
                .WithCode(productCode1)
                .WithStock(productQuantity1)
                .Build(),
            new ProductBuilder()
                .WithCode(productCode2)
                .WithStock(productQuantity2)
                .Build()
        };

        await using var application = new ShopApi();
        var client = application.CreateClient();

        using var arrangeScope = application.Services.CreateScope();
        await using var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await arrangeDbContext.Database.EnsureCreatedAsync();

        arrangeDbContext.Products.AddRange(products);
        await arrangeDbContext.SaveChangesAsync();

        var request = new CreateOrderRequestBuilder()
            .WithDeliveryAddress(deliveryAddress)
            .WithItems(items)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync("orders", requestContent);

        using var assertScope = application.Services.CreateScope();
        await using var assertDbContext = assertScope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await assertDbContext.Database.EnsureCreatedAsync();

        var createdOrder = await assertDbContext
            .Orders
            .Include(x => x.DeliveryAddress)
            .Include(x => x.Items)
            .FirstAsync();

        var expectedCreatedOrder = new Order
        {
            Status = OrderStatus.Rejected,
            DeliveryAddress = new DeliveryAddress(request.DeliveryAddress.Street,
                request.DeliveryAddress.City,
                request.DeliveryAddress.PostCode),
            Items = new[]
            {
                new Item(productCode1, itemQuantity1),
                new Item(productCode2, itemQuantity2)
            }
        };

        var productsAfterOrder = assertDbContext.Products.ToList();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"http://localhost/orders/{createdOrder.Id}");
        createdOrder.Should().BeEquivalentTo(expectedCreatedOrder, opt => opt
            .Excluding(x => x.Id)
            .Excluding(x => x.DeliveryAddress.Id)
            .Using<Item>(ctx => ctx.Subject.Should().BeEquivalentTo(ctx.Expectation, o => o.Excluding(x => x.Id)))
            .WhenTypeIs<Item>());
        productsAfterOrder.Should().BeEquivalentTo(products, opt => opt.Excluding(x => x.Id));
    }

    [Test]
    public async Task Should_return_bad_request_when_request_is_invalid()
    {
        var deliveryAddress = new DeliveryAddressRequestBuilder()
            .WithStreet(string.Empty)
            .Build();

        var items = new[]
        {
            new ItemRequestBuilder()
                .WithQuantity(0)
                .Build(),
        };

        var request = new CreateOrderRequestBuilder()
            .WithDeliveryAddress(deliveryAddress)
            .WithItems(items)
            .Build();

        var requestContent = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        await using var application = new ShopApi();
        var client = application.CreateClient();

        var response = await client.PostAsync("orders", requestContent);

        var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());

        var expectedContent = JToken.Parse(@"{
            ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
            ""title"": ""One or more validation errors occurred."",
            ""status"": 400,
            ""errors"": {
                ""DeliveryAddress.Street"": [
                    ""'Street' must not be empty.""],
                ""Items[0].Quantity"": [
                    ""'Quantity' must be greater than '0'.""]
            }
        }");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().BeEquivalentTo(expectedContent);
    }
}