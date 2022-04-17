namespace Shop.Api.Core.Repositories;

using Models;

public interface IOrderRepository
{
    Task Add(Order order);
    Task<Order?> Get(int id);
}