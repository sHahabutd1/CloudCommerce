using Catalog.API.Endpoints;
using Catalog.Application;
using Catalog.Infrastructure;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

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


app.MapProductEndpoints();

app.Run();