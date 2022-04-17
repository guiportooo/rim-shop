namespace Shop.Api.Tests.Builders.HttpIn.Requests;

using System.Collections.Generic;
using Api.HttpIn.Requests;
using AutoBogus;

public sealed class ItemRequestBuilder : AutoFaker<ItemRequest>
{
    public ItemRequestBuilder() : base("en_US")
    {
        RuleFor(x => x.Quantity, f => f.Random.Number(0, 100));
    }
    
    public ItemRequestBuilder WithQuantity(int quantity)
    {
        RuleFor(x => x.Quantity, quantity);
        return this;
    }
    
    public ItemRequest Build() => Generate();
    
    public IEnumerable<ItemRequest> Build(int count) => Generate(count);
}