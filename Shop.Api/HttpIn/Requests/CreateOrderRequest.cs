namespace Shop.Api.HttpIn.Requests;

public record CreateOrderRequest(DeliveryAddressRequest DeliveryAddress, IEnumerable<ItemRequest> Items);
