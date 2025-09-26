using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bank.Api.ExceptionMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,                 // HTTP status code
                Title = "An error occurred while processing your request.", // Short title
                Detail = exception.Message,                            // Detailed message
                Type = "https://example.com/probs/internal-server-error", // Type URI (optional, can be a documentation link)
                Instance = context.Request.Path                        // Path of the request that caused error
            };

            return context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
