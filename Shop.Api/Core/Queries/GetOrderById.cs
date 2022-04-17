using Shop.Api.Core.Models;
using Shop.Api.Core.Repositories;

namespace Shop.Api.Core.Queries;

public record GetOrderById(int Id) : IRequest<Order?>;

public class GetOrderByIdHandler : IRequestHandler<GetOrderById, Order?>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdHandler(IOrderRepository repository) => _repository = repository;

    public async Task<Order?> Handle(GetOrderById query, CancellationToken cancellationToken) =>
        await _repository.Get(query.Id);
}