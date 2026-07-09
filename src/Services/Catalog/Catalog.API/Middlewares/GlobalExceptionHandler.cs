using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.API.Middlewares;


public class GlobalExceptionHandler
    : IExceptionHandler
{

    private readonly ILogger<GlobalExceptionHandler> _logger;



    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }



    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {


        _logger.LogError(
            exception,
            "Exception occurred: {Message}",
            exception.Message);



        ProblemDetails problemDetails;


        if (exception is ValidationException validationException)
        {

            problemDetails =
                new ProblemDetails
                {
                    Status =
                        StatusCodes.Status400BadRequest,

                    Title =
                        "Validation Error",

                    Detail =
                        "One or more validation errors occurred"
                };


            problemDetails.Extensions["errors"] =
                validationException.Errors
                    .Select(x => new
                    {
                        x.PropertyName,
                        x.ErrorMessage
                    });


        }
        else
        {

            problemDetails =
                new ProblemDetails
                {
                    Status =
                        StatusCodes.Status500InternalServerError,

                    Title =
                        "Server Error",

                    Detail =
                        "Unexpected error occurred"
                };

        }



        httpContext.Response.StatusCode =
            problemDetails.Status.Value;



        await httpContext.Response
            .WriteAsJsonAsync(
                problemDetails,
                cancellationToken);



        return true;

    }

}