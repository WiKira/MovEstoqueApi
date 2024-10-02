using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using MovEstApi.TransferModels;

namespace MovEstApi.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, 
                                  ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, 
        Exception ex, 
        ILogger<GlobalExceptionHandler> logger)
    {
        context.Response.ContentType = "application/json";
        logger.LogError(ex, ex.Message);

        if(ex is ValidationException ||
        ex is ArgumentNullException ||
        ex is ArgumentException ||
        ex is ArgumentOutOfRangeException ||
        ex is InvalidOperationException){
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        else if(ex is KeyNotFoundException){
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        else if (ex is AuthenticationException){
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else if (ex is UnauthorizedAccessException){
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else if(ex is NotSupportedException || ex is NotSupportedException){
            context.Response.StatusCode = StatusCodes.Status501NotImplemented;
            return context.Response.WriteAsJsonAsync(new { 
                message = "Unable to process request." 
                });
        }
        else{
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsJsonAsync(new { 
                message = "Unable to process request." 
                });
        }

        return context.Response.WriteAsJsonAsync(new ResponseDto() 
            { Message = ex.Message }
        );
    }
}
