using HolidayWS.Models;

namespace HolidayWS.Repositories
{
    public class HolidayRepository
    {
        private static HttpClient client = new HttpClient();
        private static string? _apiKey;

        public HolidayRepository(string? apiKey) {
            _apiKey = apiKey != null && !string.IsNullOrWhiteSpace(apiKey) ? apiKey : throw new Exception("Error in HolidayRepository: API key is null or empty!");
        }

        /// <summary>
        /// Gets holidays for specific country for specific year from Calendarific API.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<CalendarificHolidayResponse?> GetHolidays(string? country = null, int? year = null)
        {
            if (country == null) country = Constants.DEFAULT_COUNTRY;
            if (year == null) year = Constants.DEFAULT_YEAR;

            CalendarificHolidayResponse? holidays = null;
            HttpResponseMessage response = await client.GetAsync($@"{Constants.CALENDARIFIC_HOLIDAY_URL}
                                                                    &country={country}
                                                                    &year={year}
                                                                    &api_key={_apiKey}");

            if (response?.IsSuccessStatusCode ?? false)
            {
                try
                {
                    holidays = await response.Content.ReadFromJsonAsync<CalendarificHolidayResponse>();
                }
                catch (Exception)
                {
                    //Do nothing, we will just return empty result
                }
            }

            return holidays;
        }
    }
}
