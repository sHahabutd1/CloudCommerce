using Catalog.API.Endpoints;
using Catalog.API.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;


namespace Catalog.API.Extensions;


public static class ApplicationBuilderExtensions
{


    public static WebApplication UseApiPipeline(
        this WebApplication app)
    {


        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseExceptionHandler();

        app.UseSerilogRequestLogging();


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI();
        }

        app.MapHealthChecks(
            "/health",
            new HealthCheckOptions
            {
                ResponseWriter =
                    UIResponseWriter.WriteHealthCheckUIResponse
            });

        app.MapHealthChecks(
            "/health/live",
            new HealthCheckOptions
            {
                Predicate =
                    check =>
                        check.Tags.Contains("live")
            });


        app.MapHealthChecks(
            "/health/ready",
            new HealthCheckOptions
            {
                Predicate =
                    check =>
                        check.Tags.Contains("ready"),

                ResponseWriter =
                    UIResponseWriter.WriteHealthCheckUIResponse
            });

        app.MapProductEndpoints();


        return app;

    }

}