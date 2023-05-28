using HolidayWS.Models;
using HolidayWS.Repositories;
using HolidayWS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HolidayWS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HolidayController : ControllerBase
    {
        private readonly ILogger<HolidayController> _logger;
        private HolidayService _holidayService; 

        public HolidayController(ILogger<HolidayController> logger, IHolidayRepository holidayRepository)
        {
            _logger = logger;
            _holidayService = new HolidayService(holidayRepository); 
        }

        #region API GET
        [HttpGet("holidays/{year}")]
        public string GetHolidaysForYear(int? year)
        {
            try
            {
                var result = _holidayService.GetHolidaysForPeriod(new DateTime(year ?? Constants.DEFAULT_YEAR, 1, 1), PeriodType.Year);
                if (result != null && result.Any()) return JsonConvert.SerializeObject(result);
                return JsonConvert.SerializeObject(new ErrorResponse { ErrorCode = (int)HttpStatusCode.NotFound, ErrorMessage = Constants.NO_HOLIDAYS });
            }
            catch (Exception e)
            {
                return GenerateExceptionResponse(e);
            }
        }

        [HttpGet("holidays/today")]
        public string GetIsTodayHoliday()
        {
            try
            {
                var holidaysToday = _holidayService.GetHolidaysForPeriod(DateTime.Now.Date, PeriodType.Day);
                GetIsTodayHolidayResonse response = new GetIsTodayHolidayResonse(holidaysToday);
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception e)
            {
                return GenerateExceptionResponse(e);
            }
        }
        #endregion

        #region Helpers
        private string GenerateExceptionResponse(Exception e)
        {
            return JsonConvert.SerializeObject(new ErrorResponse { ErrorCode = (int)HttpStatusCode.Conflict, ErrorMessage = e?.Message ?? Constants.ERROR });
        }
        #endregion
    }
}
