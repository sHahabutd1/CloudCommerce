using Catalog.Application;
using Catalog.Application.Features.Products.Commands.CreateProduct;
using Catalog.Infrastructure;
using MediatR;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure(
    builder.Configuration);


builder.Services.AddApplication();


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "CloudCommerce Catalog API",
            Version = "v1",
            Description = "Catalog Microservice API"
        });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.MapGet("/health",
() => Results.Ok("Catalog API is running"))
.WithTags("Health");


app.MapPost(
"/api/products",
async (
CreateProductCommand command,
ISender sender) =>
{

    var id = await sender.Send(command);

    return Results.Created(
        $"/api/products/{id}",
        id);

})
.WithName("CreateProduct")
.WithTags("Products");


app.Run();