namespace Shop.Api.Inventory.Core.Models;

public class Product
{
    public int Id { get; set; }
    public Guid Code { get; set; }
    public int Stock { get; set; }
}