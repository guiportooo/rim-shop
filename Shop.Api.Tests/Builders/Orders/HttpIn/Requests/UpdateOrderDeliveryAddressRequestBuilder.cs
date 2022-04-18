namespace Shop.Api.Tests.Builders.Orders.HttpIn.Requests;

using Api.Orders.HttpIn.Requests;

public sealed class UpdateOrderDeliveryAddressRequestBuilder : AutoFaker<UpdateOrderDeliveryAddressRequest>
{
    public UpdateOrderDeliveryAddressRequestBuilder() : base("en_US")
    {
    }

    public UpdateOrderDeliveryAddressRequestBuilder WithDeliveryAddress(DeliveryAddressRequest deliveryAddress)
    {
        RuleFor(x => x.DeliveryAddress, deliveryAddress);
        return this;
    }

    public UpdateOrderDeliveryAddressRequest Build() => Generate();

    public IEnumerable<UpdateOrderDeliveryAddressRequest> Build(int count) => Generate(count);
}