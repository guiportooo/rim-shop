namespace Shop.Api.Tests.Builders.Shared.Events;

using Api.Shared.Core.Events;

public sealed class OrderAcceptedBuilder : AutoFaker<OrderAccepted>
{
    public OrderAcceptedBuilder() : base("en_US")
    {
    }

    public OrderAcceptedBuilder WithOrderId(int orderId)
    {
        RuleFor(x => x.OrderId, orderId);
        return this;
    }

    public OrderAccepted Build() => Generate();

    public IEnumerable<OrderAccepted> Build(int count) => Generate(count);
}