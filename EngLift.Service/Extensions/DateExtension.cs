namespace EngLift.Service.Extensions
{
    public static class DateExtension
    {
        public static DateTime GetBeginingOfTheDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
        public static DateTime GetEndingOfTheDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
        public static string GetDateMonthYear(this DateTime date)
        {
            var day = date.Day;
            var month = date.Month;
            var year = date.Year;
            return $"{(day < 10 ? $"0{day}" : day)}/{(month < 10 ? $"0{month}" : month)}/{year}";
        }
    }
}
