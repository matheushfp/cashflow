using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CashFlowException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknownError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        var cashFlowException = (CashFlowException)context.Exception;
        var errorResponse = new ErrorResponse(cashFlowException.GetErrors());

        context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnknownError(ExceptionContext context)
    {
        var errorResponse = new ErrorResponse(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
