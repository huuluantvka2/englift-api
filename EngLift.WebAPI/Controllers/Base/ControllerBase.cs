using EngLift.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EngLift.WebAPI.Controllers.Base
{
    public class ControllerApiBase : Controller
    {
        protected IActionResult MyJsonResponse<T>(HttpStatusCode statusCode, ResponseData<T> result)
        {
            var jsonResult = Json(result);
            jsonResult.StatusCode = (int)statusCode;
            return jsonResult;
        }

        protected IActionResult Success<T>(T result)
        {
            return MyJsonResponse(HttpStatusCode.OK, new ResponseData<T>
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = null,
                Data = result
            });
        }

        protected IActionResult Created<T>(T result)
        {
            return MyJsonResponse(HttpStatusCode.Created, new ResponseData<T>
            {
                StatusCode = HttpStatusCode.Created,
                Success = true,
                Message = null,
                Data = result
            });
        }

        protected IActionResult BadRequest<T>(string? Message = null)
        {
            return MyJsonResponse(HttpStatusCode.BadRequest, new ResponseData<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = Message
            });
        }

        protected IActionResult NotFoundData<T>(string? Message = null)
        {
            return MyJsonResponse(HttpStatusCode.NotFound, new ResponseData<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Success = false,
                Message = Message
            });
        }

        protected IActionResult Conflict<T>(string? Message = null)
        {
            return MyJsonResponse(HttpStatusCode.Conflict, new ResponseData<T>
            {
                StatusCode = HttpStatusCode.Conflict,
                Success = false,
                Message = Message
            });
        }

        protected IActionResult ServerError<T>(string? Message = null)
        {
            return MyJsonResponse(HttpStatusCode.BadGateway, new ResponseData<T>
            {
                StatusCode = HttpStatusCode.BadGateway,
                Success = false,
                Message = Message
            });
        }
    }
}
