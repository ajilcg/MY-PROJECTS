namespace BBPSApi.Dto
{
    public class Response
    {
        public string? Status { get; set; }
        public string? ErrorCode { get; set; }
    }
    public class PostResponse
    {
        public string? status { get; set; }
        public string? acknowledgementId { get; set; }
    }
}
