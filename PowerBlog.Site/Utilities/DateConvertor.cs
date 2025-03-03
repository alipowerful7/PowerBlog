using System.Globalization;

namespace PowerBlog.Site.Utilities
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(value).ToString();
            string month = pc.GetMonth(value).ToString("00");
            string day = pc.GetDayOfMonth(value).ToString("00");
            return $"{year}/{month}/{day}";
        }
    }
}
