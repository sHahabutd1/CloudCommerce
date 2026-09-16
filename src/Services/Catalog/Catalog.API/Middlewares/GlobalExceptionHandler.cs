using Catalog.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Middlewares;

public sealed class GlobalExceptionHandler
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
            "Unhandled exception occurred.");

        var problemDetails = CreateProblemDetails(exception);

        httpContext.Response.StatusCode =
            problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(
        Exception exception)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException =>
                CreateValidationProblem(validationException),

            NotFoundException =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource Not Found",
                    Detail = exception.Message
                },

            _ =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred."
                }
        };

        return problemDetails;
    }

    private static ProblemDetails CreateValidationProblem(
        ValidationException validationException)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Detail = "One or more validation errors occurred."
        };

        problemDetails.Extensions["errors"] =
            validationException.Errors
                .Select(x => new
                {
                    x.PropertyName,
                    x.ErrorMessage
                });

        return problemDetails;
    }
}