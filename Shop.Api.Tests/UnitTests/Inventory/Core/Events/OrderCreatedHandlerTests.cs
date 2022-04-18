namespace Shop.Api.Tests.UnitTests.Inventory.Core.Events;

using System.Threading;
using Api.Inventory.Core.Events;
using Api.Inventory.Core.Models;
using Api.Inventory.Core.Repositories;
using Builders.Inventory.Core.Models;
using Builders.Shared.Events;
using MediatR;
using Shared.Core.Events;

public class OrderCreatedHandlerTests
{
    private AutoMocker _mocker = null!;
    private OrderCreatedHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<OrderCreatedHandler>();
    }

    [Test]
    public async Task Should_accept_order_when_all_products_have_stock()
    {
        const int orderId = 123;
        var productCode1 = Guid.NewGuid();
        var productCode2 = Guid.NewGuid();

        var items = new[]
        {
            (productCode1, 10),
            (productCode1, 10),
            (productCode2, 100),
        };

        var @event = new OrderCreatedBuilder()
            .WithOrderId(orderId)
            .WithItems(items)
            .Build();

        var products = new[]
        {
            new ProductBuilder()
                .WithCode(productCode1)
                .WithStock(20)
                .Build(),
            new ProductBuilder()
                .WithCode(productCode2)
                .WithStock(100)
                .Build()
        };

        _mocker
            .GetMock<IProductRepository>()
            .Setup(x => x.Get(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(products);

        var orderAccepted = new OrderAccepted(0);

        _mocker
            .GetMock<IMediator>()
            .Setup(x => x.Publish(It.IsAny<OrderAccepted>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((e, _) => orderAccepted = (OrderAccepted) e);

        await _handler.Handle(@event, default);

        var expectedOrderAccepted = new OrderAccepted(orderId);

        orderAccepted.Should().BeEquivalentTo(expectedOrderAccepted);

        _mocker
            .GetMock<IProductRepository>()
            .Verify(x => x.Update(It.IsAny<Product>()), Times.Exactly(2));
    }

    [Test]
    public async Task Should_reject_order_when_at_least_one_of_the_products_does_not_exist()
    {
        const int orderId = 123;
        var existentProductCode = Guid.NewGuid();
        var nonExistentProductCode = Guid.NewGuid();

        var items = new[]
        {
            (existentProductCode, 10),
            (existentProductCode, 10),
            (nonExistentProductCode, 100),
        };

        var @event = new OrderCreatedBuilder()
            .WithOrderId(orderId)
            .WithItems(items)
            .Build();

        var products = new[]
        {
            new ProductBuilder()
                .WithCode(existentProductCode)
                .WithStock(10)
                .Build()
        };

        _mocker
            .GetMock<IProductRepository>()
            .Setup(x => x.Get(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(products);

        var orderRejected = new OrderRejected(0);

        _mocker
            .GetMock<IMediator>()
            .Setup(x => x.Publish(It.IsAny<OrderRejected>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((e, _) => orderRejected = (OrderRejected) e);

        await _handler.Handle(@event, default);

        var expectedOrderRejected = new OrderRejected(orderId);

        orderRejected.Should().BeEquivalentTo(expectedOrderRejected);
    }

    [Test]
    public async Task Should_reject_order_when_at_least_one_of_the_products_does_not_have_stock()
    {
        const int orderId = 123;
        var productCodeWithoutStock = Guid.NewGuid();
        var productCodeWithStock = Guid.NewGuid();

        var items = new[]
        {
            (productCodeWithoutStock, 10),
            (productCodeWithoutStock, 10),
            (productCodeWithStock, 100),
        };

        var @event = new OrderCreatedBuilder()
            .WithOrderId(orderId)
            .WithItems(items)
            .Build();

        var products = new[]
        {
            new ProductBuilder()
                .WithCode(productCodeWithoutStock)
                .WithStock(10)
                .Build(),
            new ProductBuilder()
                .WithCode(productCodeWithStock)
                .WithStock(100)
                .Build()
        };

        _mocker
            .GetMock<IProductRepository>()
            .Setup(x => x.Get(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(products);

        var orderRejected = new OrderRejected(0);

        _mocker
            .GetMock<IMediator>()
            .Setup(x => x.Publish(It.IsAny<OrderRejected>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((e, _) => orderRejected = (OrderRejected) e);

        await _handler.Handle(@event, default);

        var expectedOrderRejected = new OrderRejected(orderId);

        orderRejected.Should().BeEquivalentTo(expectedOrderRejected);
    }
}