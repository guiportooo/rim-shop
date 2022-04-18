namespace Shop.Api.Orders.HttpIn.Responses;

using Core.Models;

public record OrdersResponse(IEnumerable<Order> Orders);
