using HolidayWS.Models;
using HolidayWS.Repositories;
using System.Linq;

namespace HolidayWS.Services
{
    public class HolidayService
    {
        private List<Holiday> _holidays = new List<Holiday>();
        private IHolidayRepository _repository;

        public HolidayService(IHolidayRepository repository)
        {
            _repository = repository;
        }

        private void PopulateHolidays(string? country = null, int? year = null)
        {
            if (country == null) country = Constants.DEFAULT_COUNTRY;
            if (year == null) year = Constants.DEFAULT_YEAR;

            //already have data for this year and this country, no need to retrieve it again
            if (_holidays.Any(x => x.Date.Year == (year ?? Constants.DEFAULT_YEAR))) return;

            var holidaysResponse = _repository.GetHolidays(country, (int)year)?.GetAwaiter().GetResult();

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
