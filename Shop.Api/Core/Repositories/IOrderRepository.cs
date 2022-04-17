namespace Shop.Api.Core.Repositories;

using Models;

public interface IOrderRepository
{
    Task<Order?> Get(int id);
    Task<IEnumerable<Order>> Get(int pageNumber, int pageSize);
    Task Add(Order order);
    Task Update(Order order);
}