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

                var response = new
                {
                    error = ex is FluentValidation.ValidationException validationEx
                        ? validationEx.Errors.Select(e => e.ErrorMessage)
                        : new[] { ex.Message }
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}