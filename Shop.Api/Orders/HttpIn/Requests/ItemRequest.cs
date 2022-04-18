namespace Shop.Api.Orders.HttpIn.Requests;

public record ItemRequest(Guid ProductId, int Quantity);

