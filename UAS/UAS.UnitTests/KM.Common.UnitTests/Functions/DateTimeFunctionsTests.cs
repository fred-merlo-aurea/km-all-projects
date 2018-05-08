using System;
using NUnit.Framework;
using KM.Common.Functions;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    public class DateTimeFunctionsTests
    {
        private const string DayIdentifier = "d";

        [Test]
        [TestCase("130118", DateFormat.DDMMYY)]
        [TestCase("13012018", DateFormat.DDMMYYYY)]
        [TestCase("13118", DateFormat.DDMYY)]
        [TestCase("1312018", DateFormat.DDMYYYY)]
        [TestCase("130118", DateFormat.DMMYY)]
        [TestCase("13012018", DateFormat.DMMYYYY)]
        [TestCase("11318", DateFormat.MDDYY)]
        [TestCase("1132018", DateFormat.MDDYYYY)]
        [TestCase("011318", DateFormat.MMDDYY)]
        [TestCase("01132018", DateFormat.MMDDYYYY)]
        [TestCase("011318", DateFormat.MMDYY)]
        [TestCase("01132018", DateFormat.MMDYYYY)]
        [TestCase("0118", DateFormat.MMYY)]
        [TestCase("118", DateFormat.MYY)]
        [TestCase("181", DateFormat.YYM)]
        [TestCase("18113", DateFormat.YYMDD)]
        [TestCase("1801", DateFormat.YYMM)]
        [TestCase("180113", DateFormat.YYMMD)]
        [TestCase("180113", DateFormat.YYMMDD)]
        [TestCase("2018113", DateFormat.YYYYMDD)]
        [TestCase("201801", DateFormat.YYYYMM)]
        [TestCase("20180113", DateFormat.YYYYMMD)]
        [TestCase("20180113", DateFormat.YYYYMMDD)]
        public void ParseDate_ValidDateTimeString_ReturnsDateTime(string date, DateFormat dateFormat)
        {
            // Arrange
            var expectedResult = new DateTime(2018, 1, 13);
            if (!dateFormat.ToString().ToLower().Contains(DayIdentifier))
            {
                expectedResult = new DateTime(2018, 1, 1);
            }

            // Act            
            var actualResult = DateTimeFunctions.ParseDate(dateFormat, date);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase("aa", DateFormat.DDMMYY)]
        [TestCase("00000", DateFormat.DDMYY)]
        [TestCase("1310000", DateFormat.DDMYYYY)]
        [TestCase("130018", DateFormat.DMMYY)]
        public void ParseDate_InvalidDateTimeString_ReturnsCurrentDateTime(string date, DateFormat dateFormat)
        {
            // Arrange
            var expectedResult = DateTime.Now;

            // Act            
            var actualResult = DateTimeFunctions.ParseDate(dateFormat, date);

            // Assert
            Assert.AreEqual(expectedResult.Year, actualResult.Year);
            Assert.AreEqual(expectedResult.Month, actualResult.Month);
            Assert.AreEqual(expectedResult.Day, actualResult.Day);
        }
    }
}
