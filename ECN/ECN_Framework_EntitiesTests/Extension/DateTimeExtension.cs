using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace ECN_Framework_EntitiesTests.Extension
{
    public static class DateTimeExtension
    {
        public static void ShouldBeCloseTo(this DateTime thisDate, DateTime date, int milliseconds)
        {
            thisDate.ShouldBeInRange(date.AddMilliseconds(-milliseconds), date.AddMilliseconds(milliseconds));
        }

        public static void ShouldBeCloseTo(this DateTime? thisDate, DateTime date, int milliseconds)
        {
            thisDate.ShouldNotBeNull();
            ShouldBeCloseTo(thisDate.Value, date, milliseconds);
        }
    }
}
