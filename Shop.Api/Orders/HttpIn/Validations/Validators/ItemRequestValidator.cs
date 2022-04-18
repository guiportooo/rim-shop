namespace Shop.Api.Orders.HttpIn.Validations.Validators;

using Requests;

public class ItemRequestValidator : AbstractValidator<ItemRequest>
{
    public ItemRequestValidator()
    {
        RuleFor(x => x.ProductCode).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    } 
}