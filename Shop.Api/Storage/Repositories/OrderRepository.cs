namespace Shop.Api.Storage.Repositories;

using Core.Models;
using Core.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShopDbContext _dbContext;

    public OrderRepository(ShopDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Order?> Get(int id) => await _dbContext
        .Orders
        .Include(x => x.DeliveryAddress)
        .Include(x => x.Items)
        .FirstOrDefaultAsync(x => x.Id == id);
}