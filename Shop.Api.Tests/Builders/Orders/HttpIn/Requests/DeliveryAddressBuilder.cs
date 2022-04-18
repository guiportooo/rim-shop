namespace Shop.Api.Tests.Builders.Orders.HttpIn.Requests;

using Api.Orders.HttpIn.Requests;

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