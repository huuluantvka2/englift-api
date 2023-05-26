using System.Net;

namespace EngLift.DTO.Response
{
    public class ResponseData<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool? Success { get; set; } = true;
        public string? Message { get; set; } = null;
        public T? Data { get; set; }
    }
    public class ResponseData
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool? Success { get; set; } = true;
        public string? Message { get; set; } = null;
        public bool? Data = null;
    }
}
