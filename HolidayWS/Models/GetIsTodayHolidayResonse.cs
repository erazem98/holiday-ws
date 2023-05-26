namespace HolidayWS.Models
{
    public class GetIsTodayHolidayResonse
    {
        private readonly bool _isHoliday = false;

        public GetIsTodayHolidayResonse(List<Holiday>? holidays) 
        {
            //if there is element in holidays provided then today is holiday
            if (holidays?.Any() ?? false) _isHoliday = true;
        }   
        public bool IsHoliday { get { return _isHoliday; } }
    }
}
