namespace Shop.Api.Orders.HttpIn.Requests;

public record DeliveryAddressRequest(string Street, string City, string PostCode);
