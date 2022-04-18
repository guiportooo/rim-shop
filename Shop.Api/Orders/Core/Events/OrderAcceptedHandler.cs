namespace Shop.Api.Orders.Core.Events;

using Exceptions;
using Models;
using Repositories;
using Shared.Core.Events;

public class OrderAcceptedHandler : INotificationHandler<OrderAccepted>
{
    private readonly IOrderRepository _repository;

    public OrderAcceptedHandler(IOrderRepository repository) => _repository = repository;

    public async Task Handle(OrderAccepted @event, CancellationToken cancellationToken)
    {
        var order = await _repository.Get(@event.OrderId);

        if (order is null)
        {
            throw new OrderNotFoundException(@event.OrderId);
        }

        if (order.Status != OrderStatus.Pending)
        {
            throw new OrderCannotBeAcceptedException(@event.OrderId);
        }
        
        order.Status = OrderStatus.Accepted;
        await _repository.Update(order);
    }
}