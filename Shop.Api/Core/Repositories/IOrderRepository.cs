namespace Shop.Api.Core.Repositories;

using Models;

public interface IOrderRepository
{
    Task<Order?> Get(int id);
    Task Add(Order order);
    Task Update(Order order);
}