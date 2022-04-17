namespace Shop.Api.HttpIn.Validations.Validators;

using Requests;

public class UpdateOrderItemsRequestValidator : AbstractValidator<UpdateOrderItemsRequest>
{
    public UpdateOrderItemsRequestValidator()
    {
        RuleForEach(x => x.Items).NotEmpty().SetValidator(new ItemRequestValidator());
    }
}