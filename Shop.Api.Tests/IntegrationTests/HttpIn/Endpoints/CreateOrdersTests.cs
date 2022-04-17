namespace Shop.Api.Tests.IntegrationTests.HttpIn.Endpoints;

using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Builders.HttpIn.Requests;
using Core.Models;
using FluentAssertions;
using FluentAssertions.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Storage;

public class CreateOrdersTests
{
    [Test]
    public async Task Should_create_order_and_return_created()
    {
        var deliveryAddress = new DeliveryAddressRequestBuilder().Build();

        var items = new[]
        {
            new ItemRequestBuilder().Build(),
            new ItemRequestBuilder().Build()
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

        using var scope = application.Services.CreateScope();
        var provider = scope.ServiceProvider;
        await using var dbContext = provider.GetRequiredService<ShopDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        var createdOrder = await dbContext
            .Orders
            .Include(x => x.DeliveryAddress)
            .Include(x => x.Items)
            .FirstAsync();

        var expectedCreatedOrder = new Order
        {
            DeliveryAddress = new DeliveryAddress(request.DeliveryAddress.Street,
                request.DeliveryAddress.City,
                request.DeliveryAddress.PostCode),
            Items = new[]
            {
                new Item(items[0].ProductId, items[0].Quantity),
                new Item(items[1].ProductId, items[1].Quantity)
            }
        };

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"http://localhost/orders/{createdOrder.Id}");
        createdOrder.Should().BeEquivalentTo(expectedCreatedOrder, opt => opt
            .Excluding(x => x.Id)
            .Excluding(x => x.DeliveryAddress.Id)
            .Using<Item>(ctx => ctx.Subject.Should().BeEquivalentTo(ctx.Expectation, o => o.Excluding(x => x.Id)))
            .WhenTypeIs<Item>());
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