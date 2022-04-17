namespace Shop.Api.Core.Commands;

using Exceptions;
using Models;
using Repositories;

public record CancelOrder(int OrderId) : IRequest;

public class CancelOrderHandler : IRequestHandler<CancelOrder>
{
    private readonly IOrderRepository _repository;

    public CancelOrderHandler(IOrderRepository repository) => _repository = repository;

    public async Task<Unit> Handle(CancelOrder command, CancellationToken cancellationToken)
    {
        var orderId = command.OrderId;
        var order = await _repository.Get(orderId);

        if (order is null)
        {
            throw new OrderNotFoundException(orderId);
        }

        order.Status = OrderStatus.Cancelled;
        await _repository.Update(order);
        return Unit.Value;
    }
}