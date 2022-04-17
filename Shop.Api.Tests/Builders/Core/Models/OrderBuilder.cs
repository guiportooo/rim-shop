namespace Shop.Api.Tests.Builders.Core.Models;

using System.Collections.Generic;
using Api.Core.Models;
using AutoBogus;

public sealed class OrderBuilder : AutoFaker<Order>
{
    public OrderBuilder() : base("en_US")
    {
        RuleFor(x => x.Id, 0);
    }

    public OrderBuilder WithDeliveryAddress(DeliveryAddress deliveryAddress)
    {
        RuleFor(x => x.DeliveryAddress, deliveryAddress);
        return this;
    }

    public OrderBuilder WithItems(IEnumerable<Item> items)
    {
        RuleFor(x => x.Items, items);
        return this;
    }

    public Order Build() => Generate();

    public IEnumerable<Order> Build(int count) => Generate(count);
}