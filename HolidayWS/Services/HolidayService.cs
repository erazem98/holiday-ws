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

        /// <summary>
        /// Populates variable _holidays and returns whether ppulation of the variable went as expected
        /// </summary>
        /// <param name="country"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private bool PopulateHolidays(out string errorMessage, string? country = null, int? year = null)
        {
            if (country == null) country = Constants.DEFAULT_COUNTRY;
            if (year == null) year = Constants.DEFAULT_YEAR;

            errorMessage = string.Empty;

            //already have data for this year and this country, no need to retrieve it again
            if (_holidays.Any(x => x.Date.Year == (year ?? Constants.DEFAULT_YEAR))) return true;

            var holidaysResponse = _repository.GetHolidays(country, (int)year)?.GetAwaiter().GetResult();

            //error occured
            if (!string.IsNullOrEmpty(holidaysResponse?.meta?.error_type))
            {
                errorMessage = holidaysResponse.meta.error_type;
                return false;
            }

            //convert to type Holiday and populate variable _holidays
            holidaysResponse?.response?.holidays?
                                     .Where(holiday => holiday != null && holiday.primary_type == "National holiday")?.ToList() 
                                     .ForEach(x => 
                                     _holidays.Add(new Holiday { Name = x.name, Date = new DateTime(x.date.datetime.year, x.date.datetime.month, x.date.datetime.day)}));

            return true;
        }

        public List<Holiday>? GetHolidaysForPeriod(DateTime date, PeriodType periodType) 
        {
            string errorMessage;  

            //populate variable _holidays. if error occured, do not continue
            if (!PopulateHolidays(out errorMessage, year: date.Year))
            {
                throw new Exception(errorMessage);
            }

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
