namespace Shop.Api.Tests.Builders.HttpIn.Requests;

using Api.HttpIn.Requests;

public sealed class UpdateOrderItemsRequestBuilder : AutoFaker<UpdateOrderItemsRequest>
{
    public UpdateOrderItemsRequestBuilder() : base("en_US")
    {
    }

    public UpdateOrderItemsRequestBuilder WithItems(IEnumerable<ItemRequest> items)
    {
        RuleFor(x => x.Items, items);
        return this;
    }

    public UpdateOrderItemsRequest Build() => Generate();

    public IEnumerable<UpdateOrderItemsRequest> Build(int count) => Generate(count);
}