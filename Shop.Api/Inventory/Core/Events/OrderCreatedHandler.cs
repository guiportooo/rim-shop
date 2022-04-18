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

    /*
     * This handler is called when an order is created.
     * First, it sums the quantity of possible duplicated products inside the order items.
     * Then it rejects the order if there are products that don't exist in the inventory,
     * or if any of the products don't have enough stock.
     * Otherwise, it updates the stock of the products and accepts the order.
     */
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

        var orderHasMissingProducts = products.Count != items.Count;
        var orderHasProductsOutOfStock = products
            .Any(x => x.Stock < items.Single(y => y.ProductCode == x.Code).Quantity);
        
        if (orderHasMissingProducts || orderHasProductsOutOfStock)
        {
            await _mediator.Publish(new OrderRejected(orderId), cancellationToken);
            return;
        }

        foreach (var product in products)
        {
            var quantity = items.First(x => x.ProductCode == product.Code).Quantity;
            product.Stock -= quantity;
            await _repository.Update(product);
        }

        await _mediator.Publish(new OrderAccepted(orderId), cancellationToken);
    }
}