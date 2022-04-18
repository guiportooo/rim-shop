namespace Shop.Api.Inventory.Storage.Repositories;

using Core.Models;
using Core.Repositories;
using Shared.Storage;

public class ProductRepository : IProductRepository
{
    private readonly ShopDbContext _dbContext;

    public ProductRepository(ShopDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<Product>> Get(IEnumerable<Guid> productCodes) =>
        await _dbContext.Products.Where(p => productCodes.Contains(p.Code)).ToListAsync();

    public async Task Update(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }
}