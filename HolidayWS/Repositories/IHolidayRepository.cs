using HolidayWS.Models;

namespace HolidayWS.Repositories
{
    public interface IHolidayRepository
    {
        public Task<CalendarificHolidayResponse?> GetHolidays(string country, int year);
    }
}
