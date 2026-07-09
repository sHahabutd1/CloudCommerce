using Catalog.API.Endpoints;
using Catalog.API.Middlewares;
using Catalog.Application;
using Catalog.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;


Log.Logger =
    new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .CreateLogger();

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

builder.Services.AddExceptionHandler
    <GlobalExceptionHandler>();

builder.Host.UseSerilog();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.MapProductEndpoints();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.Run();