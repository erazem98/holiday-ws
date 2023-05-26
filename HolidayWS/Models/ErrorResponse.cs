namespace HolidayWS.Models
{
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = "Generic error occured";
    }
}
