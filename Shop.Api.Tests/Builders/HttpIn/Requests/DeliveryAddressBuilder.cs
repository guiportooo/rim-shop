namespace Shop.Api.Tests.Builders.HttpIn.Requests;

using System.Collections.Generic;
using Api.HttpIn.Requests;
using AutoBogus;

public sealed class DeliveryAddressRequestBuilder : AutoFaker<DeliveryAddressRequest>
{
    public DeliveryAddressRequestBuilder() : base("en_US")
    {
    }

    public DeliveryAddressRequestBuilder WithStreet(string street)
    {
        RuleFor(x => x.Street, street);
        return this;
    }

    public DeliveryAddressRequest Build() => Generate();

    public IEnumerable<DeliveryAddressRequest> Build(int count) => Generate(count);
}