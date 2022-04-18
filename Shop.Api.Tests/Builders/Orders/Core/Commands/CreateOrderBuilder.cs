namespace Shop.Api.Tests.Builders.Orders.Core.Commands;

using Api.Orders.Core.Commands;
using Api.Orders.Core.Models;

public sealed class CreateOrderBuilder : AutoFaker<CreateOrder>
{
    public CreateOrderBuilder() : base("en_US")
    {
    }

    public CreateOrderBuilder WithDeliveryAddress(DeliveryAddress deliveryAddress)
    {
        RuleFor(x => x.DeliveryAddress, deliveryAddress);
        return this;
    }

    public CreateOrderBuilder WithItems(IEnumerable<Item> items)
    {
        RuleFor(x => x.Items, items);
        return this;
    }

    public CreateOrder Build() => Generate();

    public IEnumerable<CreateOrder> Build(int count) => Generate(count);
}