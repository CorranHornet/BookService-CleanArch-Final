using BookService.Application.Common.Exceptions; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;

namespace BookService.Api.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        // 1. Handle ValidationException (FluentValidation)
        if (context.Exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage });

            context.Result = new BadRequestObjectResult(new { Errors = errors });
            context.ExceptionHandled = true;
        }
        // 2. Handle NotFoundException (Custom exception)
        else if (context.Exception is NotFoundException notFoundException)
        {
            context.Result = new NotFoundObjectResult(new { error = notFoundException.Message });
            context.ExceptionHandled = true;
        }

        base.OnException(context);
    }
}