namespace Shop.Api.Orders.Storage.Repositories;

using Core.Models;
using Core.Repositories;
using Shared.Storage;

public class OrderRepository : IOrderRepository
{
    private readonly ShopDbContext _dbContext;

    public OrderRepository(ShopDbContext dbContext) => _dbContext = dbContext;

    public async Task<Order?> Get(int id) => await _dbContext
        .Orders
        .Where(x => x.Status != OrderStatus.Cancelled)
        .Include(x => x.DeliveryAddress)
        .Include(x => x.Items)
        .FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<IEnumerable<Order>> Get(int pageNumber, int pageSize) => await _dbContext
        .Orders
        .Where(x => x.Status != OrderStatus.Cancelled)
        .Include(x => x.DeliveryAddress)
        .Include(x => x.Items)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    public async Task Add(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public Task Update(Order order)
    {
        _dbContext.Orders.Update(order);
        return _dbContext.SaveChangesAsync();
    }
}