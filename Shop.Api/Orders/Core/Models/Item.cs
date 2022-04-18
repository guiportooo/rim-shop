namespace Shop.Api.Orders.Core.Models;

public class Item
{
    public Item(Guid productCode, int quantity)
    {
        ProductCode = productCode;
        Quantity = quantity;
    }

    public int Id { get; set; }
    public Guid ProductCode { get; set; }
    public int Quantity { get; set; }
}