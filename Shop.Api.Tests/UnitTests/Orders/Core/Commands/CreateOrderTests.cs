namespace Shop.Api.Tests.UnitTests.Orders.Core.Commands;

using Api.Orders;
using Api.Orders.Core.Commands;
using Api.Orders.Core.Models;
using Api.Orders.Core.Repositories;
using Builders.Orders.Core.Commands;

public class CreateOrderTests
{
    private AutoMocker _mocker;
    private CreateOrderHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();
        
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new Mappings());
        });
        var mapper = config.CreateMapper();
        _mocker.Use(mapper);
        
        _handler = _mocker.CreateInstance<CreateOrderHandler>();
    }
    
    [Test]
    public async Task Should_create_order()
    {
        const int id = 123;
        var command = new CreateOrderBuilder().Build();

        _mocker
            .GetMock<IOrderRepository>()
            .Setup(x => x.Add(It.IsAny<Order>()))
            .Callback<Order>(order => order.Id = id);
        
        var returnedId = await _handler.Handle(command, default);

        returnedId.Should().Be(id);
    }
}