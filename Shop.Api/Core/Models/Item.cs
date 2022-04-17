namespace Shop.Api.Core.Models;

public class Item
{
    public Item(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public int Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}