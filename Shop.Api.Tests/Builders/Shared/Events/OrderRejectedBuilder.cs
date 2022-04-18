namespace Shop.Api.Tests.Builders.Shared.Events;

using Api.Shared.Core.Events;

public sealed class OrderRejectedBuilder : AutoFaker<OrderRejected>
{
    public OrderRejectedBuilder() : base("en_US")
    {
    }

    public OrderRejectedBuilder WithOrderId(int orderId)
    {
        RuleFor(x => x.OrderId, orderId);
        return this;
    }

    public OrderRejected Build() => Generate();

    public IEnumerable<OrderRejected> Build(int count) => Generate(count);
}