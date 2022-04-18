namespace Shop.Api.Tests.Builders.Shared.Events;

using Api.Shared.Core.Events;

public sealed class OrderCreatedBuilder : AutoFaker<OrderCreated>
{
    public OrderCreatedBuilder() : base("en_US")
    {
    }

    public OrderCreatedBuilder WithOrderId(int orderId)
    {
        RuleFor(x => x.OrderId, orderId);
        return this;
    }

    public OrderCreatedBuilder WithItems(IEnumerable<(Guid, int)> items)
    {
        RuleFor(x => x.Items, items);
        return this;
    }

    public OrderCreated Build() => Generate();

    public IEnumerable<OrderCreated> Build(int count) => Generate(count);
}