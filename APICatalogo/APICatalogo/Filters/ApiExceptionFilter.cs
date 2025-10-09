using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace APICatalogo.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
    private readonly string _logPath;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IConfiguration configuration)
    {
        _logger = logger;
        _logPath = configuration.GetValue<string>("LogPath") ?? "APICatalogo/log/log.txt";
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var statusCode = StatusCodes.Status500InternalServerError;

        var detailedLog = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Status Code: {statusCode}\n" +
                         $"Exception Message: {exception.Message}\n" +
                         $"Stack Trace: {exception.StackTrace}\n" +
                         $"Source: {exception.Source}\n" +
                         new string('-', 80) + "\n";

        var directory = Path.GetDirectoryName(_logPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.AppendAllText(_logPath, detailedLog);

        _logger.LogError(exception, "Exceção não tratada capturada pelo filtro global.");

        context.Result = new ObjectResult(new
        {
            message = "Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde.",
            statusCode
        })
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}

