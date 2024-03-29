using Assignment_05_03.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Assignment_05_03.Customization.CustomMiddleWare
{
    public class LoggerMiddleWare
    {
        RequestDelegate _next;
        public LoggerMiddleWare(RequestDelegate next)
        {
            _next = next;

        }


        public async Task InvokeAsync(HttpContext ctx, LoggerDbContext context)
        {
            // Log in output window

            /* var route = ctx.Request.RouteValues;
             route.TryGetValue("controller", out object controller);
             route.TryGetValue("action", out object action);
             var bodyData = await new StreamReader(ctx.Request.Body).ReadToEndAsync();

             Debug.WriteLine($"Current Controller in Request : {controller}, and its action method: {action}");*/

            var requestId = Guid.NewGuid();
            var requestDate = DateOnly.FromDateTime(DateTime.Now);
            var requestTime = TimeOnly.FromDateTime(DateTime.Now);
            var requestedController = ctx.Request.RouteValues["controller"]?.ToString();
            var requestAction = ctx.Request.RouteValues["action"]?.ToString();
            var requestBody = await ReadRequestBody(ctx.Request);
            var requestHeaders = JsonSerializer.Serialize(ctx.Request.Headers);
            var requestUrl = ctx.Request.Path;


            await context.Loggers.AddAsync(new Logger
            {
                RequestID = requestId,
                RequestDate = requestDate,
                RequestTime = requestTime,
                RequestedController = requestedController,
                RequestAction = requestAction,
                RequestBody = requestBody,
                RequestHeaders = requestHeaders,
                RequestUrl = requestUrl
            });
            await context.SaveChangesAsync();
            
            await _next(ctx);


            
        }
        public async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
            var requestBody = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin); // Reset the stream position
            return requestBody;
        }

    }

   
    public static class LoggerMiddlewareExtensions
    {

        public static void UseCustomLogger(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<LoggerMiddleWare>();
        }
    }
}
