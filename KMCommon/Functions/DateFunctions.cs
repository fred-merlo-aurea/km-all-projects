using System;
using KM.Common.Functions;

namespace KM.Common
{
    public class DateFunctions
    {
        public static DateTime GetMinDate()
        {
            return DateTimeFunctions.GetMinDate();
        }
        public static TimeSpan GetMinTime()
        {
            return DateTimeFunctions.GetMinTime();
        }
    }
}
