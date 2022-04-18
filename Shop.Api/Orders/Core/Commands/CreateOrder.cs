namespace Shop.Api.Orders.Core.Commands;

using Models;
using Repositories;
using Shared.Core.Events;

public record CreateOrder(DeliveryAddress DeliveryAddress, IEnumerable<Item> Items) : IRequest<int>;

public class CreateOrderHandler : IRequestHandler<CreateOrder, int>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly IMediator _mediator;

    public CreateOrderHandler(IMapper mapper, IOrderRepository repository, IMediator mediator)
    {
        _mapper = mapper;
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<int> Handle(CreateOrder command, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(command);
        order.Status = OrderStatus.Pending;
        await _repository.Add(order);
        var orderCreated = new OrderCreated(order.Id, order.Items.Select(x => (x.ProductCode, x.Quantity)));
        await _mediator.Publish(orderCreated, cancellationToken);
        return order.Id;
    }
}
