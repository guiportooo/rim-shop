namespace Shop.Api.HttpIn.Responses;

using Core.Models;

public record OrdersResponse(IEnumerable<Order> Orders);
