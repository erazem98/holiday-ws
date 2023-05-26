using HolidayWS.Models;
using HolidayWS.Repositories;
using System.Linq;

namespace HolidayWS.Services
{
    public class HolidayService
    {
        private HolidayRepository _repository = new HolidayRepository("d9ee19d73a1e77044f3c22a94597e52f1ddd06f8");

        private List<Holiday> _holidays = new List<Holiday>();

        private void PopulateHolidays(string? country = null, int? year = null)
        {
            var holidaysResponse = _repository.GetHolidays(country, year)?.GetAwaiter().GetResult();

            //no result or error occured
            if (holidaysResponse?.response?.holidays  == null || !string.IsNullOrEmpty(holidaysResponse.meta.error_type))
            {
                return;
            }

            //convert to type Holiday and populate variable _holidays
            holidaysResponse.response.holidays
                                     .Where(holiday => holiday != null && holiday.primary_type == "National holiday")?.ToList() 
                                     .ForEach(x => 
                                     _holidays.Add(new Holiday { Name = x.name, Date = new DateTime(x.date.datetime.year, x.date.datetime.month, x.date.datetime.day)}));
        }

        public List<Holiday>? GetHolidaysForPeriod(DateTime date, PeriodType periodType) 
        {
            PopulateHolidays(year: date.Year);

            if (periodType == PeriodType.Year) 
            {
                return _holidays?.Where(x => x != null)
                               ?.Where(y => y?.Date.Year == date.Year)?.ToList();
            }

            if (periodType == PeriodType.Day)
            {
                return _holidays?.Where(x => x != null)
                               ?.Where(y => y.Date.Date.Equals(date))?.ToList();
            }

            return null;
        } 
    }
}
