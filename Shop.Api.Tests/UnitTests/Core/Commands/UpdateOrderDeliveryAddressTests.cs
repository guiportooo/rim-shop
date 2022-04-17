namespace Shop.Api.Tests.UnitTests.Core.Commands;

using Api.Core.Commands;
using Api.Core.Exceptions;
using Api.Core.Models;
using Api.Core.Repositories;
using Builders.Core.Commands;
using Builders.Core.Models;

public class UpdateOrderDeliveryAddressTests
{
    private AutoMocker _mocker;
    private UpdateOrderDeliveryAddressHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<UpdateOrderDeliveryAddressHandler>();
    }

    [Test]
    public async Task Should_throw_exception_when_order_does_not_exists()
    {
        const int orderId = 123;

        var command = new UpdateOrderDeliveryAddressBuilder()
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
    public async Task Should_update_order_delivery_address()
    {
        const int orderId = 123;
        
        var deliveryAddress = new DeliveryAddressBuilder().Build();

        var command = new UpdateOrderDeliveryAddressBuilder()
            .WithOrderId(orderId)
            .WithDeliveryAddress(deliveryAddress)
            .Build();

        var order = new OrderBuilder().Build();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Get(orderId))
            .ReturnsAsync(order);

        var usedDeliveryAddress = new DeliveryAddress("", "", "");
        
        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Update(order))
            .Callback<Order>(usedOrder => usedDeliveryAddress = usedOrder.DeliveryAddress);

        await _handler.Handle(command, default);

        usedDeliveryAddress.Should().BeEquivalentTo(deliveryAddress);
    }
}