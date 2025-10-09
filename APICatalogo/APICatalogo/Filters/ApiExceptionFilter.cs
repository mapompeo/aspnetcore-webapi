using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        _logger.LogError(exception, "Exceção não tratada capturada pelo filtro global.");

        context.Result = new ObjectResult(new
        {
            message = exception.Message,
            stackTrace = exception.StackTrace
        })
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}

