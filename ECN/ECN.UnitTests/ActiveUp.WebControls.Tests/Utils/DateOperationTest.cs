using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.Utils
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DateOperationTest
    {
        private MSTest::PrivateObject _privateObjcet;
        private const string _MethodFormatCustomDate = "FormatCustomDate";
        private DateOperation _dateOperation;

        [SetUp]
        public void SetUp()
        {
            _dateOperation = new DateOperation();
            _privateObjcet = new MSTest::PrivateObject(_dateOperation);
        }

        [Test]
        public void FormatCustomDate_FormatIsEmpty_ReturnString()
        {
            // Arrange
            var date = DateTime.Now;
            var expectedOutput = date.ToShortDateString();
            var format = string.Empty;
            var days = new string[0];
            var months = new string[0];

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_FormatNotEmpty_ReturnString()
        {
            // Arrange
            var date = DateTime.Now;
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year;
            var expectedOutput = $"{day.PadLeft(2, '0')}/{month.PadLeft(2, '0')}/{year}";
            const string format = "dd/mm/yyyy";
            var days = new string[0];
            var months = new string[0];

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_BadFormat_ReturnString()
        {
            // Arrange
            var date = DateTime.Now;
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year;
            const string format = "TestString";
            var expectedOutput = format.ToLower();
            var days = new string[0];
            var months = new string[0];

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_IndexDayAndIndexMonthAreEqual_ReturnString()
        {
            // Arrange
            var date = DateTime.Now;
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year;
            var expectedOutput = $"{day}/{month}/{year}";
            const string format = "d/m/yyyy";
            var days = new string[0];
            var months = new string[0];

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_DayEqual2_ReturnString()
        {
            // Arrange
            var day = 1;
            var month = 1;
            var year = DateTime.Now.Year;
            var date = new DateTime(year, month, day);
            var expectedOutput = $"{date.DayOfWeek.ToString().Substring(0, 3)} {month} {year}";
            const string format = "ddd m yyyy";
            var days = new string[] 
            {
                DayOfWeek.Sunday.ToString(),
                DayOfWeek.Monday.ToString(),
                DayOfWeek.Tuesday.ToString(),
                DayOfWeek.Wednesday.ToString(),
                DayOfWeek.Thursday.ToString(),
                DayOfWeek.Friday.ToString(),
                DayOfWeek.Saturday.ToString()
            };
            var months = new string[] { "January"};

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_DayEqual2LengthIs2_ReturnString()
        {
            // Arrange
            var day = 1;
            var month = 1;
            var year = DateTime.Now.Year;
            var date = new DateTime(year, month, day);
            var expectedOutput = $"{date.DayOfWeek.ToString().Substring(0, 2)} {month} {year}";
            const string format = "ddd m yyyy";
            var days = new string[]
            {
                DayOfWeek.Sunday.ToString().Substring(0, 2),
                DayOfWeek.Monday.ToString().Substring(0, 2),
                DayOfWeek.Tuesday.ToString().Substring(0, 2),
                DayOfWeek.Wednesday.ToString().Substring(0, 2),
                DayOfWeek.Thursday.ToString().Substring(0, 2),
                DayOfWeek.Friday.ToString().Substring(0, 2),
                DayOfWeek.Saturday.ToString().Substring(0, 2)
            };
            var months = new string[] { "January" };

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_MonthFormatEqual2_ReturnString()
        {
            // Arrange
            var day = 1;
            var month = 1;
            var year = DateTime.Now.Year;
            var date = new DateTime(year, month, day);
            var expectedOutput = $"{date.DayOfWeek.ToString().Substring(0, 3)} Jan {year}";
            const string format = "ddd mmm yyyy";
            var days = new string[]
            {
                DayOfWeek.Sunday.ToString(),
                DayOfWeek.Monday.ToString(),
                DayOfWeek.Tuesday.ToString(),
                DayOfWeek.Wednesday.ToString(),
                DayOfWeek.Thursday.ToString(),
                DayOfWeek.Friday.ToString(),
                DayOfWeek.Saturday.ToString()
            };
            var months = new string[] { "January" };

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_DayAndMonthFormatEqual3_ReturnString()
        {
            // Arrange
            var day = 1;
            var month = 1;
            var year = DateTime.Now.Year;
            var date = new DateTime(year, month, day);
            var expectedOutput = $"{date.DayOfWeek.ToString()} January {year}";
            const string format = "dddd mmmm yyyy";
            var days = new string[]
            {
                DayOfWeek.Sunday.ToString(),
                DayOfWeek.Monday.ToString(),
                DayOfWeek.Tuesday.ToString(),
                DayOfWeek.Wednesday.ToString(),
                DayOfWeek.Thursday.ToString(),
                DayOfWeek.Friday.ToString(),
                DayOfWeek.Saturday.ToString()
            };
            var months = new string[] { "January" };

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_DayAndMonthFormatGreaterThan3_ReturnString()
        {
            // Arrange
            var day = 1;
            var month = 1;
            var year = DateTime.Now.Year;
            var date = new DateTime(year, month, day);
            var expectedOutput = $"{day.ToString().PadLeft(2, '0')} {month.ToString().PadLeft(2, '0')} {year}";
            const string format = "ddddd mmmmm yyyy";
            var days = new string[]
            {
                DayOfWeek.Sunday.ToString(),
                DayOfWeek.Monday.ToString(),
                DayOfWeek.Tuesday.ToString(),
                DayOfWeek.Wednesday.ToString(),
                DayOfWeek.Thursday.ToString(),
                DayOfWeek.Friday.ToString(),
                DayOfWeek.Saturday.ToString()
            };
            var months = new string[] { "January" };

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_YearIs2Digits_ReturnString()
        {
            // Arrange
            var date = DateTime.Now;
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year.ToString();
            var expectedOutput = $"{day.PadLeft(2, '0')}/{month.PadLeft(2, '0')}/{string.Format("{0:yy}", date)}";
            const string format = "dd/mm/yy";
            var days = new string[0];
            var months = new string[0];

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }

        [Test]
        public void FormatCustomDate_YearIndexIsEqual_ReturnString()
        {
            // Arrange
            var date = DateTime.Now;
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year.ToString();
            var expectedOutput = $"{day.PadLeft(2, '0')}/{month.PadLeft(2, '0')}/{string.Format("{0:yy}", date)}";
            const string format = "dd/mm/y";
            var days = new string[0];
            var months = new string[0];

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodFormatCustomDate,
                BindingFlags.Static | BindingFlags.NonPublic,
                new object[] { date, format, days, months }) as string;

            // Assert
            actualResult.ShouldBe(expectedOutput);
        }
    }
}
