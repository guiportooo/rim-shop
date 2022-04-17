namespace Shop.Api.Tests.UnitTests.Core.Commands;

using Api.Core.Commands;
using Api.Core.Exceptions;
using Api.Core.Models;
using Api.Core.Repositories;
using Builders.Core.Commands;
using Builders.Core.Models;

public class CancelOrderTests
{
    private AutoMocker _mocker;
    private CancelOrderHandler _handler;

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