namespace Shop.Api.Tests.UnitTests.Orders.Core.Events;

using Api.Orders.Core.Events;
using Api.Orders.Core.Exceptions;
using Api.Orders.Core.Models;
using Api.Orders.Core.Repositories;
using Builders.Orders.Core.Models;
using Builders.Shared.Events;

public class OrderAcceptedHandlerTests
{
    private AutoMocker _mocker = null!;
    private OrderAcceptedHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<OrderAcceptedHandler>();
    }

    [Test]
    public async Task Should_throw_exception_when_order_does_not_exists()
    {
        const int orderId = 123;

        var @event = new OrderAcceptedBuilder()
            .WithOrderId(orderId)
            .Build();

        var exception = new OrderNotFoundException(orderId);

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Get(orderId))
            .Throws(exception);

        Func<Task> handle = async () => await _handler.Handle(@event, default);

        var expectedMessage = $"Order with id {orderId} not found";

        await handle
            .Should()
            .ThrowAsync<OrderNotFoundException>()
            .WithMessage(expectedMessage);
    }

    [TestCase(OrderStatus.Rejected)]
    [TestCase(OrderStatus.Cancelled)]
    public async Task Should_throw_exception_when_order_cannot_be_accepted(OrderStatus status)
    {
        const int orderId = 123;

        var @event = new OrderAcceptedBuilder()
            .WithOrderId(orderId)
            .Build();

        var order = new OrderBuilder()
            .WithStatus(status)
            .Build();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Get(orderId))
            .ReturnsAsync(order);

        Func<Task> handle = async () => await _handler.Handle(@event, default);

        var expectedMessage = $"Order with id {orderId} cannot be accepted";

        await handle
            .Should()
            .ThrowAsync<OrderCannotBeAcceptedException>()
            .WithMessage(expectedMessage);
    }

    [Test]
    public async Task Should_accept_order()
    {
        const int orderId = 123;

        var @event = new OrderAcceptedBuilder()
            .WithOrderId(orderId)
            .Build();

        var order = new OrderBuilder()
            .Pending()
            .Build();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Get(orderId))
            .ReturnsAsync(order);

        var updatedOrder = new Order();
        
        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Update(It.IsAny<Order>()))
            .Callback<Order>(o => updatedOrder = o);
        
        await _handler.Handle(@event, default);

        updatedOrder.Status.Should().Be(OrderStatus.Accepted);
    }
}