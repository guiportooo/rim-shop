namespace Shop.Api.Orders.Core.Commands;

using Exceptions;
using Models;
using Repositories;

public record UpdateOrderItems(int OrderId, IEnumerable<Item> Items) : IRequest;

public class UpdateOrderItemsHandler : IRequestHandler<UpdateOrderItems>
{
    private readonly IOrderRepository _repository;

    public UpdateOrderItemsHandler(IOrderRepository repository) => _repository = repository;

    public async Task<Unit> Handle(UpdateOrderItems command, CancellationToken cancellationToken)
    {
        var (orderId, items) = command;
        var order = await _repository.Get(orderId);

        if (order is null)
        {
            throw new OrderNotFoundException(orderId);
        }
        
        order.Items = items;
        await _repository.Update(order);
        return Unit.Value;
    }
}

