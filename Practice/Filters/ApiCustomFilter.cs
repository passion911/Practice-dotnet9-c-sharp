using Microsoft.AspNetCore.Mvc.Filters;

namespace Practice.Filters;

public class ApiCustomFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        Console.WriteLine($"Request: {request.Method} {request.Path}");
        //_logger.LogInformation($"Request: {request.Method} {request.Path}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var response = context.HttpContext.Response;
        Console.WriteLine($"Response {response.StatusCode} ");
        //_logger.LogInformation($"Response: {response.StatusCode}");
    }
}
