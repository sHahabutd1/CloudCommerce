using Catalog.API.Extensions;
using Catalog.Application;
using Catalog.Infrastructure;


var builder =
    WebApplication.CreateBuilder(args);


builder.Services.AddApplication();


builder.Services.AddInfrastructure(
    builder.Configuration);


builder.AddApiServices();


var app =
    builder.Build();


app.UseApiPipeline();


app.Run();