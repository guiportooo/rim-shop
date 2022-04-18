namespace Shop.Api.Tests.UnitTests.Orders.Core.Commands;

using System.Threading;
using Api.Orders;
using Api.Orders.Core.Commands;
using Api.Orders.Core.Models;
using Api.Orders.Core.Repositories;
using Builders.Orders.Core.Commands;
using Builders.Orders.Core.Models;
using MediatR;
using Shared.Core.Events;

public class CreateOrderTests
{
    private AutoMocker _mocker = null!;
    private CreateOrderHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();

        var config = new MapperConfiguration(opts => { opts.AddProfile(new Mappings()); });
        var mapper = config.CreateMapper();
        _mocker.Use(mapper);

        _handler = _mocker.CreateInstance<CreateOrderHandler>();
    }

    [Test]
    public async Task Should_create_order_and_publish_orderCreated_event()
    {
        const int id = 123;
        var productCode = Guid.NewGuid();
        const int quantity = 10;

        var items = new[]
        {
            new ItemBuilder()
                .WithProductCode(productCode)
                .WithQuantity(quantity)
                .Build()
        };

        var command = new CreateOrderBuilder()
            .WithItems(items)
            .Build();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Add(It.IsAny<Order>()))
            .Callback<Order>(order => order.Id = id);

        var orderCreated = new OrderCreated(0, new List<(Guid productCode, int quantity)>());

        _mocker
            .GetMock<IMediator>()
            .Setup(x => x.Publish(It.IsAny<OrderCreated>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((e, _) => orderCreated = (OrderCreated) e);

        var returnedId = await _handler.Handle(command, default);

        var expectedOrderCreated = new OrderCreated(id, new[] {(productCode, quantity)});

        returnedId.Should().Be(id);
        orderCreated.Should().BeEquivalentTo(expectedOrderCreated);
    }
}