using System.Text.Json.Serialization;

namespace HolidayWS.Models
{
    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Date
    {
        public object iso { get; set; }
        public Datetime datetime { get; set; }
        public Timezone timezone { get; set; }
    }

    public class Datetime
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int? hour { get; set; }
        public int? minute { get; set; }
        public int? second { get; set; }
    }

    public class CalendarificHoliday
    {
        public string name { get; set; }
        public string description { get; set; }
        public Country country { get; set; }
        public Date date { get; set; }
        public List<string> type { get; set; }
        public string primary_type { get; set; }
        public string canonical_url { get; set; }
        public string urlid { get; set; }
        public string locations { get; set; }
        public string states { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
        public string error_type { get; set; } = string.Empty;
    }

    public class Response
    {
        public List<CalendarificHoliday> holidays { get; set; }
    }

    public class CalendarificHolidayResponse
    {
        public Meta meta { get; set; }
        public Response response { get; set; }
    }

    public class Timezone
    {
        public string offset { get; set; }
        public string zoneabb { get; set; }
        public int zoneoffset { get; set; }
        public int zonedst { get; set; }
        public int zonetotaloffset { get; set; }
    }


}
