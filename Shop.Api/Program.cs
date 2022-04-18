using Shop.Api.Orders.HttpIn;
using Shop.Api.Shared.Storage;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Configure<JsonOptions>(opt => opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMediatR(Assembly.GetExecutingAssembly())
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddStorage(builder.Configuration);

var app = builder.Build();

app.MapOrdersEndpoints();

if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app.Run();

namespace Shop.Api
{
    public partial class Program
    {
    }
}