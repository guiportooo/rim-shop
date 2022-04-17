namespace Shop.Api.HttpIn.Requests;

using Core.Models;

public record UpdateOrderItemsRequest(IEnumerable<ItemRequest> Items);
