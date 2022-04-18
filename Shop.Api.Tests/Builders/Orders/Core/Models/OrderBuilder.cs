namespace Shop.Api.Tests.Builders.Orders.Core.Models;

using Api.Orders.Core.Models;

public sealed class OrderBuilder : AutoFaker<Order>
{
    public OrderBuilder() : base("en_US")
    {
        RuleFor(x => x.Id, 0);
        RuleFor(x => x.Status, OrderStatus.Pending);
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

    public OrderBuilder WithOneItem()
    {
        RuleFor(x => x.Items, f => new ItemBuilder().Build(1));
        return this;
    }

    public OrderBuilder Cancelled()
    {
        RuleFor(x => x.Status, OrderStatus.Cancelled);
        return this;
    }

    public Order Build() => Generate();

    public IEnumerable<Order> Build(int count) => Generate(count);
}