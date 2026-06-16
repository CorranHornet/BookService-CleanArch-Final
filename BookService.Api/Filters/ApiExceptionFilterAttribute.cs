using BookService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookService.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        // Handle NotFound exceptions specifically
        if (context.Exception is NotFoundException notFoundException)
        {
            context.Result = new NotFoundObjectResult(new { error = notFoundException.Message });
            context.ExceptionHandled = true;
        }
        else
        {
            // Handle all other exceptions as 500 Internal Server Error
            context.Result = new ObjectResult(new { error = "An internal server error occurred." })
            { StatusCode = 500 };
            context.ExceptionHandled = true;
        }

        base.OnException(context);
    }
}