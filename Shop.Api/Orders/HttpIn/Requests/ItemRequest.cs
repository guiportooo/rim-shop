namespace Shop.Api.Orders.HttpIn.Requests;

public record ItemRequest(Guid ProductCode, int Quantity);

