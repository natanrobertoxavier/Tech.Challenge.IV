using Contact.Persistence.Communication.Response;
using Contact.Persistence.Exceptions;
using Contact.Persistence.Exceptions.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Contact.Persistence.Api.Filters;

public class ExceptionFilters : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TechChallengeException)
        {
            ProcessTechChallengeException(context);
        }
        else
        {
            ThrowUnknownError(context);
        }
    }

    private static void ProcessTechChallengeException(ExceptionContext context)
    {
        if (context.Exception is ValidationErrorsException)
        {
            HandleValidationError(context);
        }
        else if (context.Exception is InvalidLoginException)
        {
            HandleLoginException(context);
        };
    }

    private static void HandleValidationError(ExceptionContext context)
    {
        var validationErrorException = context.Exception as ValidationErrorsException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ErrorResponseJson(validationErrorException.ErrorMessages));
    }

    private static void HandleLoginException(ExceptionContext context)
    {
        var loginError = context.Exception as InvalidLoginException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new ErrorResponseJson(loginError.Message));
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ErrorResponseJson(ErrorsMessages.UnknowError));
    }
}
