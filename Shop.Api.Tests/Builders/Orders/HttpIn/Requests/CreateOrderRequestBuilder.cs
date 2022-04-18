namespace Shop.Api.Tests.Builders.Orders.HttpIn.Requests;

using Api.Orders.HttpIn.Requests;

public sealed class CreateOrderRequestBuilder : AutoFaker<CreateOrderRequest>
{
    public CreateOrderRequestBuilder() : base("en_US")
    {
    }

    public CreateOrderRequestBuilder WithDeliveryAddress(DeliveryAddressRequest deliveryAddress)
    {
        RuleFor(x => x.DeliveryAddress, deliveryAddress);
        return this;
    }

    public CreateOrderRequestBuilder WithItems(IEnumerable<ItemRequest> items)
    {
        RuleFor(x => x.Items, items);
        return this;
    }

    public CreateOrderRequest Build() => Generate();

    public IEnumerable<CreateOrderRequest> Build(int count) => Generate(count);
}