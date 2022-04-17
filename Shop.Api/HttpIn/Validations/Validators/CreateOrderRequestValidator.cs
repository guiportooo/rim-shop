namespace Shop.Api.HttpIn.Validations.Validators;

using Requests;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.DeliveryAddress).NotEmpty().SetValidator(new DeliveryAddressRequestValidator());
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).SetValidator(new ItemRequestValidator());
    }
}