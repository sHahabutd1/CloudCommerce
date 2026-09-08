using Catalog.API.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;


namespace Catalog.API.Extensions;


public static class ServiceCollectionExtensions
{


    public static WebApplicationBuilder AddApiServices(
        this WebApplicationBuilder builder)
    {


        builder.Services.AddEndpointsApiExplorer();


        builder.Services.AddSwaggerGen(options =>
        {

            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title =
                        "CloudCommerce Catalog API",

                    Version =
                        "v1",

                    Description =
                        "Catalog Microservice API"
                });

        });


        builder.Services.AddExceptionHandler
            <GlobalExceptionHandler>();


        builder.Services.AddProblemDetails();

        builder.Services
            .AddHealthChecks()

            .AddCheck(
                "self",
                () => HealthCheckResult.Healthy(),
                tags: ["live"])

            .AddNpgSql(
                builder.Configuration
                    .GetConnectionString("CatalogDb")!,
                tags: ["ready"]);


        builder.Host.UseSerilog(
            (context, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(
                        context.Configuration);
            });

        builder.Services.AddObservability();

        return builder;

    }

}