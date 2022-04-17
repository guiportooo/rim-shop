using System.ComponentModel.DataAnnotations;
using Shop.Api.Core.Commands;
using Shop.Api.HttpIn.Requests;
using Shop.Api.HttpIn.Responses;
using Shop.Api.HttpIn.Validations;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Shop.Api.HttpIn;

using Core.Exceptions;
using Core.Models;
using Core.Queries;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost("/orders", async (CreateOrderRequest request,
                IValidator<CreateOrderRequest> validator,
                IMediator mediator,
                IMapper mapper) =>
            {
                var result = await validator.ValidateAsync(request);

                if (!result.IsValid)
                {
                    return Results.ValidationProblem(result.ToDictionary());
                }

                var command = mapper.Map<CreateOrder>(request);
                var id = await mediator.Send(command);
                return Results.CreatedAtRoute("get-order", new {id});
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        endpoints
            .MapGet("/orders/{id}", async (int id, IMediator mediator) =>
            {
                var query = new GetOrderById(id);
                var order = await mediator.Send(query);
                return order is null
                    ? Results.Problem("Order not found", statusCode: StatusCodes.Status404NotFound)
                    : Results.Ok(new SuccessResponse<OrderResponse>(new OrderResponse(order)));
            })
            .WithName("get-order")
            .Produces(StatusCodes.Status200OK, typeof(Order))
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints
            .MapPut("/orders/{id}/deliveryAddress", async (int id,
                UpdateOrderDeliveryAddressRequest request,
                IValidator<UpdateOrderDeliveryAddressRequest> validator,
                IMediator mediator,
                IMapper mapper) =>
            {
                var result = await validator.ValidateAsync(request);

                if (!result.IsValid)
                {
                    return Results.ValidationProblem(result.ToDictionary());
                }

                var deliveryAddress = mapper.Map<DeliveryAddress>(request.DeliveryAddress);
                var command = new UpdateOrderDeliveryAddress(id, deliveryAddress);

                try
                {
                    await mediator.Send(command);
                    return Results.Ok();
                }
                catch (OrderNotFoundException e)
                {
                    return Results.Problem(e.Message, statusCode: StatusCodes.Status404NotFound);
                }
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        endpoints
            .MapPut("/orders/{id}/items", async (int id,
                UpdateOrderItemsRequest request,
                IValidator<UpdateOrderItemsRequest> validator,
                IMediator mediator,
                IMapper mapper) =>
            {
                var result = await validator.ValidateAsync(request);

                if (!result.IsValid)
                {
                    return Results.ValidationProblem(result.ToDictionary());
                }

                var items = mapper.Map<IEnumerable<Item>>(request.Items);
                var command = new UpdateOrderItems(id, items);

                try
                {
                    await mediator.Send(command);
                    return Results.Ok();
                }
                catch (OrderNotFoundException e)
                {
                    return Results.Problem(e.Message, statusCode: StatusCodes.Status404NotFound);
                }
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}