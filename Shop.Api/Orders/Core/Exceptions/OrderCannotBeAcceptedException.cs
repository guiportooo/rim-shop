namespace Shop.Api.Orders.Core.Exceptions;

using Shared.Core.Exceptions;

public class OrderCannotBeAcceptedException : CoreException
{
    public OrderCannotBeAcceptedException(int id) : base($"Order with id {id} cannot be accepted")
    {
    }
}