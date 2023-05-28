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
    //[ApiController]
    //[Route("[controller]")]
    public class HolidayController : ControllerBase
    {
        private readonly ILogger<HolidayController> _logger;
        private HolidayService _holidayService; 

        public HolidayController(ILogger<HolidayController> logger, IHolidayRepository holidayRepository)
        {
            _logger = logger;
            _holidayService = new HolidayService(holidayRepository); 
        }

        #region HTTP GET
        /// <summary>
        /// Retrieves all holidays with their names and dates for a given year.
        /// </summary>
        /// <param name="year">The year for which to retrieve the holidays (e.g., 2023).</param>
        /// <returns>
        /// List of holidays in JSON format. 
        /// Example: [{ "Date":"2023-01-01","Name":"New Year's Day"}, ...]
        /// </returns>
        [HttpGet("holidays/{year}")]
        [ProducesResponseType(type: typeof(List<Holiday>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status409Conflict)]
        public ActionResult<string> GetHolidaysForYear(int? year)
        {
            try
            {
                var result = _holidayService.GetHolidaysForPeriod(new DateTime(year ?? Constants.DEFAULT_YEAR, 1, 1), PeriodType.Year);
                if (result != null && result.Any()) return Ok(JsonConvert.SerializeObject(result));
                return NotFound(JsonConvert.SerializeObject(new ErrorResponse { ErrorCode = (int)HttpStatusCode.NotFound, ErrorMessage = Constants.NO_HOLIDAYS }));
            }
            catch (Exception e)
            {
                return  Conflict(GenerateExceptionResponse(e));
            }
        }

        /// <summary>
        /// Checks if the current date is a holiday and returns a boolean response.
        /// </summary>
        /// <returns>
        /// Answer of whether today is a holiday or not in JSON format.
        /// Example: { "IsHoliday": false }
        /// </returns>
        [HttpGet("holidays/today")]
        [ProducesResponseType(type: typeof(GetIsTodayHolidayResonse), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status409Conflict)]
        public ActionResult<string> GetIsTodayHoliday()
        {
            try
            {
                var holidaysToday = _holidayService.GetHolidaysForPeriod(DateTime.Now.Date, PeriodType.Day);
                GetIsTodayHolidayResonse response = new GetIsTodayHolidayResonse(holidaysToday);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception e)
            {
                return Conflict(GenerateExceptionResponse(e));
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
