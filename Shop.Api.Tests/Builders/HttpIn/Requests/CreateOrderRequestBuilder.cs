namespace Shop.Api.Tests.Builders.HttpIn.Requests;

using System.Collections.Generic;
using Api.HttpIn.Requests;
using AutoBogus;

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