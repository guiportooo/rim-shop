namespace Shop.Api.Orders;

using Core.Commands;
using Core.Models;
using HttpIn.Requests;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<CreateOrderRequest, CreateOrder>();
        CreateMap<DeliveryAddressRequest, DeliveryAddress>();
        CreateMap<ItemRequest, Item>();
        CreateMap<CreateOrder, Order>();
    }
}