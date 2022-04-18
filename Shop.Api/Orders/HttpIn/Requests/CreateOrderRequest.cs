namespace Shop.Api.Orders.HttpIn.Requests;

public record CreateOrderRequest(DeliveryAddressRequest DeliveryAddress, IEnumerable<ItemRequest> Items);
