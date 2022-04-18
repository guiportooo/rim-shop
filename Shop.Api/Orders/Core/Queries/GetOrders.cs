namespace Shop.Api.Orders.Core.Queries;

using Models;
using Repositories;

public record GetOrders(int PageNumber, int PageSize) : IRequest<IEnumerable<Order>>;

public class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<IEnumerable<Order>> Handle(GetOrders query, CancellationToken cancellationToken) =>
        await _orderRepository.Get(query.PageNumber, query.PageSize);
}