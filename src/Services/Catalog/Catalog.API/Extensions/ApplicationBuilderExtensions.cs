using Catalog.API.Endpoints;
using Catalog.API.Middlewares;
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


        app.MapProductEndpoints();


        return app;

    }

}