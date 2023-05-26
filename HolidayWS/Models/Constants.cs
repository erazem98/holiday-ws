namespace HolidayWS.Models
{
    public static class Constants
    {
        #region Default values
        public static string DEFAULT_COUNTRY = "SI";
        public static int DEFAULT_YEAR = DateTime.Now.Year;
        #endregion

        #region Urls, endpoints
        public static string CALENDARIFIC_HOLIDAY_URL = "https://calendarific.com/api/v2/holidays?";
        #endregion

        #region Messages
        public static string NO_HOLIDAYS = "No holidays found.";
        public static string ERROR = "Error occured.";
        #endregion
    }
}
