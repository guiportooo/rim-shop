namespace Shop.Api.Inventory.Core.Events;

using Repositories;
using Shared.Core.Events;

public class OrderCreatedHandler : INotificationHandler<OrderCreated>
{
    private readonly IProductRepository _repository;
    private readonly IMediator _mediator;

    public OrderCreatedHandler(IProductRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task Handle(OrderCreated @event, CancellationToken cancellationToken)
    {
        var (orderId, orderItems) = @event;
        var items = (from item in orderItems
                group item by item.productCode
                into g
                select new
                {
                    ProductCode = g.Key, 
                    Quantity = g.Sum(x => x.quantity)
                })
            .ToList();

        var products = (await _repository.Get(items.Select(x => x.ProductCode))).ToList();

        if (products.Count < items.Count)
        {
            await _mediator.Publish(new OrderRejected(orderId), cancellationToken);
            return;
        }

        foreach (var product in products)
        {
            var quantity = items.FirstOrDefault(x => x.ProductCode == product.Code)?.Quantity;

            if (quantity is null)
            {
                continue;
            }

            if (product.Stock < quantity)
            {
                await _mediator.Publish(new OrderRejected(orderId), cancellationToken);
                return;
            }

            product.Stock -= quantity.Value;
            await _repository.Update(product);
        }


        await _mediator.Publish(new OrderAccepted(orderId), cancellationToken);
    }
}