namespace Shop.Api;

using Core.Commands;
using Core.Models;
using HttpIn.Requests;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<CreateOrderRequest, CreateOrder>();
        CreateMap<UpdateOrderDeliveryAddressRequest, UpdateOrderDeliveryAddress>();
        CreateMap<DeliveryAddressRequest, DeliveryAddress>();
        CreateMap<ItemRequest, Item>();
        CreateMap<CreateOrder, Order>();
    }
}