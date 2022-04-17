using Shop.Api.HttpIn;
using Shop.Api.Storage;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
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

public partial class Program {}