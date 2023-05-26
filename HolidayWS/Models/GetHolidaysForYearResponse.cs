namespace HolidayWS.Models
{
    public class GetHolidaysForYearResponse
    {
        //[JsonConverter(typeof(string))]
        public List<Holiday> Holidays { get; set; } = new List<Holiday>();
    }
}
