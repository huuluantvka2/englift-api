using System.Net;

namespace EngLift.Service.Extensions
{
    [Serializable]
    public class ServiceExeption : Exception
    {
        public ServiceExeption(HttpStatusCode statusCode, string? message = "Error") : base(message)
        {
            Data.Add("StatusCode", statusCode);
        }
    }
}
