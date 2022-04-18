namespace Shop.Api.Orders.Core.Models;

public class DeliveryAddress
{
    public DeliveryAddress(string street, string city, string postCode)
    {
        Street = street;
        City = city;
        PostCode = postCode;
    }

    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
}