using System;
using System.Threading;

namespace KM.Common.Functions
{
    public static class DateTimeFunctions
    {
        public static DateTime ParseDate(DateFormat dateFormat, string date)
        {
            if (String.IsNullOrWhiteSpace(date))
            {
                throw new ArgumentNullException(nameof(date));
            }

            var dateFormatString = dateFormat.ToString().ToLower();
            var dateFormatParsingInfo = new DateFormatParsingInfo(dateFormatString, date.Length);

            if (date.Length < dateFormatString.Length ||
                date.Length > dateFormatParsingInfo.MaxValidDateLength)
            {
                return DateTime.Now;
            }

            var dayValue = dateFormatParsingInfo.FirstDayOfMonth.ToString();
            if (dateFormatParsingInfo.DayStartIndex != -1)
            {
                dayValue = date.Substring(dateFormatParsingInfo.DayStartIndex, dateFormatParsingInfo.DayLength);
            }

            var monthValue = dateFormatParsingInfo.FirstMonthOfYear.ToString();
            if (dateFormatParsingInfo.MonthStartIndex != -1)
            {
                monthValue = date.Substring(dateFormatParsingInfo.MonthStartIndex, dateFormatParsingInfo.MonthLength);
            }

            var yearValue = date.Substring(dateFormatParsingInfo.YearStartIndex, dateFormatParsingInfo.YearLength);

            var day = 0;
            var month = 0;
            var year = 0;

            Int32.TryParse(dayValue, out day);
            Int32.TryParse(monthValue, out month);
            Int32.TryParse(yearValue, out year);

            if ((dateFormatParsingInfo.DayWithSingleFormatHasTwoDigits && day < 10) ||
                (dateFormatParsingInfo.MonthWithSingleFormatHasTwoDigits && month < 10))
            {
                return DateTime.Now;
            }

            if (year > 0)
            {
                year = ConvertTwoYearToFour(year);
            }

            if (month > 0 && day > 0 && year > 0)
            {
                return new DateTime(year, month, day);
            }
            else if (month > 0 && year > 0)
            {
                return new DateTime(year, month, 1);
            }

            return DateTime.Now;
        }

        public static int ConvertTwoYearToFour(int twoDigitYear)
        {
            return Thread.CurrentThread.CurrentCulture.Calendar.ToFourDigitYear(twoDigitYear);
        }

        public static DateTime GetMinDate()
        {
            return new DateTime(1900, 1, 1);
        }

        public static TimeSpan GetMinTime()
        {
            return new TimeSpan(0);
        }
    }
}
