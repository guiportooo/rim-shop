namespace Shop.Api.Orders.Core.Exceptions;

using Shared.Core.Exceptions;

public class OrderNotFoundException : CoreException
{
    public OrderNotFoundException(int id) : base($"Order with id {id} not found")
    {
    }
}