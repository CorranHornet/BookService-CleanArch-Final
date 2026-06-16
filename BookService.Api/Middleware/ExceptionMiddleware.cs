using BookService.Application.Common.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace BookService.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,

                    FluentValidation.ValidationException => StatusCodes.Status400BadRequest,

                    ArgumentException => StatusCodes.Status400BadRequest,

                    InvalidOperationException => StatusCodes.Status409Conflict,

                    _ => StatusCodes.Status500InternalServerError
                };

                var message = ex switch
                {
                    FluentValidation.ValidationException validationEx =>
                        "Validation failed: " + string.Join(" | ",
                            validationEx.Errors.Select(e => e.ErrorMessage)),

                    _ => ex.Message
                };

                var response = new
                {
                    error = message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}