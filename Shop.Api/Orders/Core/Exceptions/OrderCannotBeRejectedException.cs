namespace Shop.Api.Orders.Core.Exceptions;

using Shared.Core.Exceptions;

public class OrderCannotBeRejectedException : CoreException
{
    public OrderCannotBeRejectedException(int id) : base($"Order with id {id} cannot be rejected")
    {
    }
}