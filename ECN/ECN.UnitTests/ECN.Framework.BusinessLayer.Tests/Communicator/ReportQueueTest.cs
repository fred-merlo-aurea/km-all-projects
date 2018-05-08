using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Globalization;
using System.Linq;
using System.Threading;
using ECN_Framework_BusinessLayer.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    /// <summary>
    /// Unit test for <see cref="ReportQueue"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportQueueTest
    {
        private const string DefaultCulture = "en-US";
        private const string TimeFormat = "h:mm:ss tt";
        private const string ScheduleTypeRecurring = "Recurring";
        private const string ScheduleTypeOneTime = "One-Time";
        private const string RecurrenceTypeDaily = "daily";
        private const string RecurrenceTypeWeekly = "weekly";
        private const string RecurrenceTypeMonthly = "monthly";
        private IDisposable _shimObject;

        [SetUp]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(DefaultCulture);
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _shimObject.Dispose();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetNextDateToRun_ScheduleTypeIsRecurringAndRecurrenceTypeIsDaily_ReturnsNextScheduleValue(bool isNew)
        {
            // Arrange
            var startDate = DateTime.Now.ToString();
            var stopDate = DateTime.Now.AddDays(1).ToString();
            var startTime = DateTime.Now.ToString(TimeFormat);
            var reportSchedule = new ECN_Framework_Entities.Communicator.ReportSchedule
            {
                StartDate = startDate,
                EndDate = stopDate,
                ScheduleType = ScheduleTypeRecurring,
                RecurrenceType = RecurrenceTypeDaily,
                StartTime = startTime,
            };

            // Act
            var dateToSend = ReportQueue.GetNextDateToRun(reportSchedule, isNew);

            // Assert
            dateToSend.ShouldNotBeNull();
        }

        [TestCase(0, true)]
        [TestCase(1, false)]
        public void GetNextDateToRun_ScheduleTypeIsRecurringAndRecurrenceTypeIsMonthly_ReturnsNextScheduleValue(int day, bool monthLastDay)
        {
            // Arrange
            ShimDateTime.NowGet = () => new DateTime(2018, 1, 1);
            var days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var stopDay = days == 31 ? 0 : 1;
            var startDate = DateTime.Now.AddDays(day).ToString();
            var stopDate = DateTime.Now.AddDays(stopDay).ToString();
            var startTime = DateTime.Now.ToString(TimeFormat);
            var reportSchedule = new ECN_Framework_Entities.Communicator.ReportSchedule
            {
                StartDate = startDate,
                EndDate = stopDate,
                ScheduleType = ScheduleTypeRecurring,
                RecurrenceType = RecurrenceTypeMonthly,
                StartTime = startTime,
                MonthLastDay = monthLastDay,
                MonthScheduleDay = 1,
            };

            // Act
            var dateToSend = ReportQueue.GetNextDateToRun(reportSchedule, true);

            // Assert
            dateToSend.ShouldBeNull();
        }

        [TestCase(-5, false)]
        [TestCase(-20, true)]
        public void GetNextDateToRun_AndRecurrenceTypeIsMonthly_ReturnsNextScheduleValue(int day, bool monthLastDay)
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-day).ToString();
            var stopDate = DateTime.Now.AddDays(1).ToString();
            var startTime = DateTime.Now.ToString(TimeFormat);
            var reportSchedule = new ECN_Framework_Entities.Communicator.ReportSchedule
            {
                StartDate = startDate,
                EndDate = stopDate,
                ScheduleType = ScheduleTypeRecurring,
                RecurrenceType = RecurrenceTypeMonthly,
                StartTime = startTime,
                MonthLastDay = monthLastDay,
                MonthScheduleDay = day,
            };

            // Act
            var dateToSend = ReportQueue.GetNextDateToRun(reportSchedule, true);

            // Assert
            dateToSend.ShouldBeNull();
        }

        [Test]
        public void GetNextDateToRun_ScheduleTypeOneTime_ReturnsNextScheduleValue()
        {
            // Arrange
            var startDate = DateTime.Now.ToShortDateString();
            var stopDate = DateTime.Now.AddDays(1).ToShortDateString();
            var startTime = DateTime.Now.ToString(TimeFormat);
            var reportSchedule = new ECN_Framework_Entities.Communicator.ReportSchedule
            {
                StartDate = startDate,
                EndDate = stopDate,
                ScheduleType = ScheduleTypeOneTime,
                StartTime = startTime,
                MonthLastDay = false,
                MonthScheduleDay = 1,
            };

            // Act
            var dateToSend = ReportQueue.GetNextDateToRun(reportSchedule, true);

            // Assert
            dateToSend.ShouldSatisfyAllConditions(
            () => dateToSend.ShouldNotBeNull(),
            () => dateToSend.ShouldBe(Convert.ToDateTime(startDate + " " + startTime))
            );
        }

        [TestCase(true, DayOfWeek.Sunday)]
        [TestCase(false, DayOfWeek.Sunday)]
        [TestCase(true, DayOfWeek.Monday)]
        [TestCase(false, DayOfWeek.Monday)]
        [TestCase(true, DayOfWeek.Tuesday)]
        [TestCase(false, DayOfWeek.Tuesday)]
        [TestCase(true, DayOfWeek.Wednesday)]
        [TestCase(false, DayOfWeek.Wednesday)]
        [TestCase(true, DayOfWeek.Thursday)]
        [TestCase(false, DayOfWeek.Thursday)]
        [TestCase(true, DayOfWeek.Friday)]
        [TestCase(false, DayOfWeek.Friday)]
        [TestCase(true, DayOfWeek.Saturday)]
        [TestCase(false, DayOfWeek.Saturday)]
        public void GetNextDateToRun_ScheduleTypeIsRecurringAndRecurrenceTypeIsWeekly_ReturnsNextScheduleValue(bool isNew, DayOfWeek dayOfweek)
        {
            // Arrange
            var dateList = GetDates();
            var startDate = DateTime.Now.ToString();
            var stopDate = DateTime.Now.AddDays(20).ToString();
            var startTime = DateTime.Now.ToString(TimeFormat);
            ShimDateTime.NowGet =
               () =>
               {
                   return dateList.FirstOrDefault(x => x.DayOfWeek == dayOfweek);
               };
            var reportSchedule = new ECN_Framework_Entities.Communicator.ReportSchedule
            {
                StartDate = startDate,
                EndDate = stopDate,
                ScheduleType = ScheduleTypeRecurring,
                RecurrenceType = RecurrenceTypeWeekly,
                StartTime = startTime,
                RunSunday = true,
                RunMonday = true,
                RunTuesday = true,
                RunWednesday = true,
                RunFriday = true,
                RunSaturday = true,
                RunThursday = true
            };

            // Act
            var dateToSend = ReportQueue.GetNextDateToRun(reportSchedule, isNew);

            // Assert
            dateToSend.ShouldNotBeNull();
        }

        public static List<DateTime> GetDates()
        {
            var now = DateTime.Now;
            var currentDay = now.DayOfWeek;
            var days = (int)currentDay;
            var sunday = now.AddDays(-days);
            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => sunday.AddDays(d))
                .ToList();
            return daysThisWeek;
        }
    }
}
