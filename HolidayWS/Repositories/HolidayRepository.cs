using HolidayWS.Models;

namespace HolidayWS.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private static HttpClient client = new HttpClient();
        private static string? _apiKey;

        public HolidayRepository(string? apiKey) {
            _apiKey = !string.IsNullOrWhiteSpace(apiKey) ? apiKey : throw new Exception("Error in HolidayRepository: API key is null or empty!");
        }

        /// <summary>
        /// Gets holidays for specific country for specific year from Calendarific API (https://calendarific.com).
        /// </summary>
        /// <param name="country"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<CalendarificHolidayResponse?> GetHolidays(string country, int year)
        {
            CalendarificHolidayResponse? holidays = null;
            HttpResponseMessage response = await client.GetAsync($@"{Constants.CALENDARIFIC_HOLIDAY_URL}
                                                                    &country={country}
                                                                    &year={year}
                                                                    &api_key={_apiKey}");

            if (response?.IsSuccessStatusCode ?? false)
            {
                holidays = await response.Content.ReadFromJsonAsync<CalendarificHolidayResponse>();
            }

            return holidays;
        }
    }
}
