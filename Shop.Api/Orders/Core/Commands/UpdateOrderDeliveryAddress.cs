namespace Shop.Api.Orders.Core.Commands;

using Exceptions;
using Models;
using Repositories;

public record UpdateOrderDeliveryAddress(int OrderId, DeliveryAddress DeliveryAddress) : IRequest;

public class UpdateOrderDeliveryAddressHandler : IRequestHandler<UpdateOrderDeliveryAddress>
{
    private readonly IOrderRepository _repository;

    public UpdateOrderDeliveryAddressHandler(IOrderRepository repository) => _repository = repository;

    public async Task<Unit> Handle(UpdateOrderDeliveryAddress command, CancellationToken cancellationToken)
    {
        var (orderId, deliveryAddress) = command;
        var order = await _repository.Get(orderId);

        if (order is null)
        {
            throw new OrderNotFoundException(orderId);
        }
        
        order.DeliveryAddress = deliveryAddress;
        await _repository.Update(order);
        return Unit.Value;
    }
}

