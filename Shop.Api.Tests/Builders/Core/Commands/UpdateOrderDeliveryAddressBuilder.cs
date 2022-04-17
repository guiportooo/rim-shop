namespace Shop.Api.Tests.Builders.Core.Commands;

using Api.Core.Commands;
using Api.Core.Models;

public sealed class UpdateOrderDeliveryAddressBuilder : AutoFaker<UpdateOrderDeliveryAddress>
{
    public UpdateOrderDeliveryAddressBuilder() : base("en_US")
    {
    }

    public UpdateOrderDeliveryAddressBuilder WithOrderId(int orderId)
    {
        RuleFor(x => x.OrderId, orderId);
        return this;
    }

    public UpdateOrderDeliveryAddressBuilder WithDeliveryAddress(DeliveryAddress deliveryAddress)
    {
        RuleFor(x => x.DeliveryAddress, deliveryAddress);
        return this;
    }

    public UpdateOrderDeliveryAddress Build() => Generate();

    public IEnumerable<UpdateOrderDeliveryAddress> Build(int count) => Generate(count);
}