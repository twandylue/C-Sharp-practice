using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebAPI.Middlewares
{
    public class TestMiddleware
    {
        private readonly ILogger<TestMiddleware> _logger;
        private readonly static JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // _logger.LogInformation("context");
            int errorCode = 500;
            string detailMessage = "Server Error";

            await Response(errorCode, new ErrorResponse()
            {
                ErrorType = "type 1",
                DetailMessage = detailMessage
            });

            async Task Response(int code, ErrorResponse errorResponse)
            {
                context.Response.StatusCode = code;
                context.Response.ContentType = "application/json";
                // context.Request.Headers

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, _options)).ConfigureAwait(false);
            }
        }
    }

    public class ErrorResponse
    {
        public string ErrorType { set; get; }
        public string DetailMessage { set; get; }
    }
}