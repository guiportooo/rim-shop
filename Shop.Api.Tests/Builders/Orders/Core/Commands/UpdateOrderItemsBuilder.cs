namespace Shop.Api.Tests.Builders.Orders.Core.Commands;

using Api.Orders.Core.Commands;
using Api.Orders.Core.Models;

public sealed class UpdateOrderItemsBuilder : AutoFaker<UpdateOrderItems>
{
    public UpdateOrderItemsBuilder() : base("en_US")
    {
    }

    public UpdateOrderItemsBuilder WithOrderId(int orderId)
    {
        RuleFor(x => x.OrderId, orderId);
        return this;
    }

    public UpdateOrderItemsBuilder WithItems(IEnumerable<Item> items)
    {
        RuleFor(x => x.Items, items);
        return this;
    }

    public UpdateOrderItems Build() => Generate();

    public IEnumerable<UpdateOrderItems> Build(int count) => Generate(count);
}