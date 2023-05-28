namespace HolidayWS.Models
{
    public class GetIsTodayHolidayResonse
    {
        private readonly bool _isHoliday;

        public GetIsTodayHolidayResonse(List<Holiday>? holidays) 
        {
            //if there is element in holidays provided then today is holiday
            _isHoliday = holidays?.Any() ?? false;
        }   
        public bool IsHoliday { get { return _isHoliday; } }
    }
}
