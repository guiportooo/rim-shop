namespace Shop.Api.HttpIn.Requests;

public record UpdateOrderItemsRequest(IEnumerable<ItemRequest> Items);
