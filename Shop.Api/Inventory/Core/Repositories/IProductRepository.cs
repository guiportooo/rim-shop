namespace Shop.Api.Inventory.Core.Repositories;

using Models;

public interface IProductRepository
{
    Task<IEnumerable<Product>> Get(IEnumerable<Guid> productCodes);
    Task Update(Product product);
}