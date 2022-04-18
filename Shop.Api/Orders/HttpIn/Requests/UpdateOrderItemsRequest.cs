namespace Shop.Api.Orders.HttpIn.Requests;

public record UpdateOrderItemsRequest(IEnumerable<ItemRequest> Items);
