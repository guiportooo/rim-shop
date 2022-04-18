namespace Shop.Api.Orders.Core.Commands;

using Models;
using Repositories;

public record CreateOrder(DeliveryAddress DeliveryAddress, IEnumerable<Item> Items) : IRequest<int>;

public class CreateOrderHandler : IRequestHandler<CreateOrder, int>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;

    public CreateOrderHandler(IMapper mapper, IOrderRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<int> Handle(CreateOrder command, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(command);
        order.Status = OrderStatus.Pending;
        await _repository.Add(order);
        return order.Id;
    }
}
