using Newtonsoft.Json;

namespace HolidayWS.Models
{
    public class Holiday
    {
        public string Name { get; set; } = string.Empty;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Date { get; set; }
    }
}
