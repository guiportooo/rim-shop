namespace Shop.Api.Tests.Builders.Core.Models;

using System.Collections.Generic;
using Api.Core.Models;
using AutoBogus;

public sealed class DeliveryAddressBuilder : AutoFaker<DeliveryAddress>
{
    public DeliveryAddressBuilder() : base("en_US")
    {
        RuleFor(x => x.Id, 0);
    }

    public DeliveryAddress Build() => Generate();

    public IEnumerable<DeliveryAddress> Build(int count) => Generate(count);
}