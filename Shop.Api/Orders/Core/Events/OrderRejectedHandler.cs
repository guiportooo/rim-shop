namespace Shop.Api.Orders.Core.Events;

using Exceptions;
using Models;
using Repositories;
using Shared.Core.Events;

public class OrderRejectedHandler : INotificationHandler<OrderRejected>
{
    private readonly IOrderRepository _repository;

    public OrderRejectedHandler(IOrderRepository repository) => _repository = repository;

    public async Task Handle(OrderRejected @event, CancellationToken cancellationToken)
    {
        var order = await _repository.Get(@event.OrderId);

        if (order is null)
        {
            throw new OrderNotFoundException(@event.OrderId);
        }

        if (order.Status != OrderStatus.Pending)
        {
            throw new OrderCannotBeRejectedException(@event.OrderId);
        }
        
        order.Status = OrderStatus.Rejected;
        await _repository.Update(order);
    }
}