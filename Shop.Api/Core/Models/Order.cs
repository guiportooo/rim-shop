namespace Shop.Api.Core.Models;

public class Order
{
    public int Id { get; set; }
    public DeliveryAddress DeliveryAddress { get; set; } = null!;
    public IEnumerable<Item> Items { get; set; } = null!;
}