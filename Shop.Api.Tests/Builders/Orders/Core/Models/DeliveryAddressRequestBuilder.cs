namespace Shop.Api.Tests.Builders.Orders.Core.Models;

using Api.Orders.Core.Models;

public sealed class DeliveryAddressBuilder : AutoFaker<DeliveryAddress>
{
    public DeliveryAddressBuilder() : base("en_US")
    {
        RuleFor(x => x.Id, 0);
    }

    public DeliveryAddress Build() => Generate();

    public IEnumerable<DeliveryAddress> Build(int count) => Generate(count);
}