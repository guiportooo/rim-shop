# rim-shop

## The challenge

### Distributed .Net Engineer Tech Assignment

**Create a production ready API solution implementing the follow actions**:

- Create a new order
- Update the order delivery address
- Update the order items
- Cancel an order
- Retrieve a single order
- Retrieve a paginated list of orders

* **Bonus point**: manage the inventory stock for products

**Additional requirements**:

- The solution has to build and execute without errors
- The solution has to be production ready (deployable)

**Additional notes**

- A product can be just a GUID
- There is no expectation of validating the product, this would be outside the scope of this assignment
- Feel free to use whatever persistence layer you see fit
- Async communication, usually this is done over a messaging bus, but it can be simplified using an in-memory approach

## Decisions

- [.NET Minimal API](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0) as web API hosting
- [EF Core 6](https://github.com/dotnet/efcore) with SQLite provider for persistence
- [MediatR](https://github.com/jbogard/MediatR) as in-memory messaging bus 
- InMemoryDatabase as provider for integration tests

## Dependencies
- [.NET SDK 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [EF Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- [Docker](https://www.docker.com)

## Running on Docker

### Configure dev certificate for Chat API HTTPS redirection

Change the certificate password on the **docker-compose.yaml** file to match you local dev certificate's password:

The password can be configured replacing `[your_password_here]` with your password on the **line 8** of the docker-compose.yaml:

```
ASPNETCORE_Kestrel__Certificates__Default__Password=[your_password_here]
```

If you don't know your local dev certificate's password, you can generate a new one running:

```bash
dotnet dev-certs https --clean
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p [your_password_here]
dotnet dev-certs https --trust
```

### Run with Docker Compose

1. Run the command `docker-compose build`
2. Run the command `docker-compose up`
3. The Shop API will be available at _https://localhost:7250/swagger/index.html_

## Running locally

1. Go to `/Shop.Api` directory
2. Run the command `dotnet run`
3. The Shop API will be available at _https://localhost:7250/swagger/index.html_

Alternatively, you can run the Shop.Api project on your favorite IDE

## Running the tests

1. Go to `/Shop.Api.Tests` director
2. Run the command `dotnet test`

You can also run the tests on your IDE