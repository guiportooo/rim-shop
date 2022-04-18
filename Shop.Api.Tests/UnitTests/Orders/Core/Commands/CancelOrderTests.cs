namespace Shop.Api.Tests.UnitTests.Orders.Core.Commands;

using Api.Orders.Core.Commands;
using Api.Orders.Core.Exceptions;
using Api.Orders.Core.Models;
using Api.Orders.Core.Repositories;
using Builders.Orders.Core.Commands;
using Builders.Orders.Core.Models;

public class CancelOrderTests
{
    private AutoMocker _mocker = null!;
    private CancelOrderHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<CancelOrderHandler>();
    }

    [Test]
    public async Task Should_throw_exception_when_order_does_not_exists()
    {
        const int orderId = 123;

        var command = new CancelOrderBuilder()
            .WithOrderId(orderId)
            .Build();

        var exception = new OrderNotFoundException(orderId);

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Get(orderId))
            .Throws(exception);

        Func<Task> handle = async () => await _handler.Handle(command, default);

        var expectedMessage = $"Order with id {orderId} not found";

        await handle
            .Should()
            .ThrowAsync<OrderNotFoundException>()
            .WithMessage(expectedMessage);
    }

    [Test]
    public async Task Should_cancel_order()
    {
        const int orderId = 123;

        var items = new[]
        {
            new ItemBuilder().Build()
        };

        var command = new CancelOrderBuilder()
            .WithOrderId(orderId)
            .Build();

        var order = new OrderBuilder().Build();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Get(orderId))
            .ReturnsAsync(order);

        var usedStatus = 0;

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Update(order))
            .Callback<Order>(usedOrder => usedStatus = (int)usedOrder.Status);

        await _handler.Handle(command, default);

        usedStatus.Should().Be((int)OrderStatus.Cancelled);
    }
}