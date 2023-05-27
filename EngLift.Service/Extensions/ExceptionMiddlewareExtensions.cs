using EngLift.DTO.Response;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace EngLift.Service.Extensions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var resut = new ResponseData()
                {
                    Data = null,
                    Message = error.Message,
                    StatusCode = (HttpStatusCode)(error.Data["StatusCode"] ?? HttpStatusCode.BadGateway)
                };
                response.StatusCode = (int)resut.StatusCode;
                await response.WriteAsync(JsonSerializer.Serialize(resut));
            }
        }
    }
}
