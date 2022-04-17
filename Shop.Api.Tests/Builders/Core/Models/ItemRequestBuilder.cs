namespace Shop.Api.Tests.Builders.Core.Models;

using System.Collections.Generic;
using Api.Core.Models;
using AutoBogus;

public sealed class ItemBuilder : AutoFaker<Item>
{
    public ItemBuilder() : base("en_US")
    {
        RuleFor(x => x.Id, 0);
    }
    
    public Item Build() => Generate();
    
    public IEnumerable<Item> Build(int count) => Generate(count);
}