namespace Shop.Api.Orders.HttpIn.Validations.Validators;

using Requests;

public class UpdateOrderDeliveryAddressRequestValidator : AbstractValidator<UpdateOrderDeliveryAddressRequest>
{
    public UpdateOrderDeliveryAddressRequestValidator()
    {
        RuleFor(x => x.DeliveryAddress).NotEmpty().SetValidator(new DeliveryAddressRequestValidator());
    }
}