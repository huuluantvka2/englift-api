namespace EngLift.DTO.Base
{
    public class BaseRequest
    {
        public int Limit { get; set; } = 15;
        public int Page { get; set; } = 1;
        public string? Search { get; set; }
        public int? Sort { get; set; }//tự quy định
    }
}
