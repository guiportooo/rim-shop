namespace Shop.Api.Tests.Builders.Inventory.Core.Models;

using Api.Inventory.Core.Models;

public sealed class ProductBuilder : AutoFaker<Product>
{
    public ProductBuilder() : base("en_US")
    {
        RuleFor(x => x.Id, 0);
    }

    public ProductBuilder WithCode(Guid code)
    {
        RuleFor(x => x.Code, code);
        return this;
    }
    
    public ProductBuilder WithStock(int stock)
    {
        RuleFor(x => x.Stock, stock);
        return this;
    }
    
    public Product Build() => Generate();
    
    public IEnumerable<Product> Build(int count) => Generate(count);

}