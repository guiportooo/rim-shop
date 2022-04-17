namespace Shop.Api.Tests.UnitTests.Core.Commands;

using System.Linq;
using Api.Core.Commands;
using Api.Core.Exceptions;
using Api.Core.Models;
using Api.Core.Repositories;
using Builders.Core.Commands;
using Builders.Core.Models;

public class UpdateOrderItemsTests
{
    private AutoMocker _mocker;
    private UpdateOrderItemsHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<UpdateOrderItemsHandler>();
    }

    [Test]
    public async Task Should_throw_exception_when_order_does_not_exists()
    {
        const int orderId = 123;

        var command = new UpdateOrderItemsBuilder()
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
    public async Task Should_update_order_items()
    {
        const int orderId = 123;

        var items = new[]
        {
            new ItemBuilder().Build()
        };

        var command = new UpdateOrderItemsBuilder()
            .WithOrderId(orderId)
            .WithItems(items)
            .Build();

        var order = new OrderBuilder().Build();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Get(orderId))
            .ReturnsAsync(order);

        var usedItems = new List<Item>();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Update(order))
            .Callback<Order>(usedOrder => usedItems = usedOrder.Items.ToList());

        await _handler.Handle(command, default);

        usedItems.Should().BeEquivalentTo(items);
    }
}