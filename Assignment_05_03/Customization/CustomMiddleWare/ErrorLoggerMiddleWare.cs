using Assignment_05_03.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Assignment_05_03.Customization.CustomMiddleWare
{
    public class ErrorLoggerMiddleWare
    {
         RequestDelegate _next;
        

        public ErrorLoggerMiddleWare(RequestDelegate next)
        {
            _next = next;
            
        }

        public async Task InvokeAsync(HttpContext ctx, LoggerDbContext context)
        {
            try
            {
                ctx.Request.EnableBuffering();//This is necessary if you plan to read the request body more than once
                await _next(ctx);
               
            }
            catch (Exception ex)
            {
                // Log the exception message
                var exceptionMessage = ex.Message;

                // Capture request details
                var requestId = Guid.NewGuid();
                var requestDate = DateOnly.FromDateTime(DateTime.Now);
                var requestTime = TimeOnly.FromDateTime(DateTime.Now);
                var requestedController = ctx.Request.RouteValues["controller"]?.ToString();
                var requestAction = ctx.Request.RouteValues["action"]?.ToString();
                var requestBody = await ReadRequestBody(ctx.Request);
                var requestHeaders = JsonSerializer.Serialize(ctx.Request.Headers);
                var requestUrl = ctx.Request.Path;

                // Add error log to the database
                 context.ErrorLoggers.Add(new ErrorLogger
                {
                    RequestID = requestId,
                    RequestDate = requestDate,
                    RequestTime = requestTime,
                    RequestedController = requestedController,
                    RequestAction = requestAction,
                    RequestBody = requestBody,
                    RequestHeaders = requestHeaders,
                    RequestUrl = requestUrl,
                    ExceptionMessage = exceptionMessage 
                });

                 context.SaveChanges();

                
                throw;
            }

        }
         private async Task<string> ReadRequestBody(HttpRequest request)
         {

             using var reader = new StreamReader(request.Body);
             var requestBody = await reader.ReadToEndAsync();
             request.Body.Seek(0, SeekOrigin.Begin); 
             return requestBody;
         }

       /* private async Task<string> ReadRequestBody(HttpResponse response)
        {

            using var reader = new StreamReader(response.Body);
            var requestBody = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return requestBody;
        }*/
    }

    public static class ErrorMiddlewareExtensions
    {
        public static void UseCustomErrorLogger(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorLoggerMiddleWare>();
        }
    }

}
