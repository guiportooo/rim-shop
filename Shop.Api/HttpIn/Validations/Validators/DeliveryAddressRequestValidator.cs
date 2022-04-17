namespace Shop.Api.HttpIn.Validations.Validators;

using Requests;

public class DeliveryAddressRequestValidator : AbstractValidator<DeliveryAddressRequest>
{
    public DeliveryAddressRequestValidator()
    {
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.PostCode).NotEmpty();
    } 
}