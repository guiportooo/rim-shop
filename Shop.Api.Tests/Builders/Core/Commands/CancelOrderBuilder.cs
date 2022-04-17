namespace Shop.Api.Tests.Builders.Core.Commands;

using Api.Core.Commands;

public sealed class CancelOrderBuilder : AutoFaker<CancelOrder>
{
    public CancelOrderBuilder() : base("en_US")
    {
    }

    public CancelOrderBuilder WithOrderId(int orderId)
    {
        RuleFor(x => x.OrderId, orderId);
        return this;
    }

    public CancelOrder Build() => Generate();

    public IEnumerable<CancelOrder> Build(int count) => Generate(count);
}