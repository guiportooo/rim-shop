namespace Shop.Api.Tests.UnitTests.Core.Commands;

using System.Threading.Tasks;
using Api.Core.Commands;
using Api.Core.Models;
using Api.Core.Repositories;
using AutoMapper;
using Builders.Core.Commands;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

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