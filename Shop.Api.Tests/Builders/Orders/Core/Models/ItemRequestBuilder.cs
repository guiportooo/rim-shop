namespace Shop.Api.Tests.Builders.Orders.Core.Models;

using Api.Orders.Core.Models;

public sealed class ItemBuilder : AutoFaker<Item>
{
    public ItemBuilder() : base("en_US")
    {
        RuleFor(x => x.Id, 0);
        RuleFor(x => x.Quantity, 10);
    }

    public ItemBuilder WithProductCode(Guid productCode)
    {
        RuleFor(x => x.ProductCode, productCode);
        return this;
    }
    
    public ItemBuilder WithQuantity(int quantity)
    {
        RuleFor(x => x.Quantity, quantity);
        return this;
    }
    
    public Item Build() => Generate();
    
    public IEnumerable<Item> Build(int count) => Generate(count);
}