using System;
using System.ComponentModel.Design.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ActiveUp.WebControls.Design;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveCalendar.Designer
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CalendarControlDesignerTest
    {
        private const string SampleImagePath = "http://km.com/abc.png";
        private const string CalanderDateFormat = "DAY;MONTH;YEAR;HOUR;MINUTE;SECOND;MILLISECOND;MERIDIEM;";
        private const string CssStyleLeftKey = "LEFT";
        private const string CssStyleLeftValue = "10";
        private const string CssStyleTopKey = "TOP";
        private const string CssStyleTopValue = "20";
        private const string CssStylePositionKey = "POSITION";
        private const string CssStylePositionValue = "center";
        private CalendarControlDesigner _calendarControlDesigner;
        private IDisposable _context;

        [SetUp]
        public void Setup()
        {
            _calendarControlDesigner = new CalendarControlDesigner();
            _context = ShimsContext.Create();
            ShimWebControl.AllInstances.StyleGet = (x) =>
            {
                return CreateCssStyleCollection();
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetDesignTimeHtml_CalenderUseDatePickerOptionEnabled_ReturnsHtmlStringContent()
        {
            // Arrange
            ShimComponentDesigner.AllInstances.ComponentGet = (x) =>
            {
                return CreateCalendarObject();
            };

            // Act
            var result = _calendarControlDesigner.GetDesignTimeHtml();

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetDesignTimeHtml_CalenderUseDatePickerOptionIsDisabled_ReturnsHtmlStringContent()
        {
            // Arrange
            ShimComponentDesigner.AllInstances.ComponentGet = (x) =>
            {
                var calender = CreateCalendarObject(false);
                calender.UseDatePicker = false;
                calender.MultiSelection = true;
                calender.ShowHeaderNavigation = false;
                calender.PrevMonthImage = SampleImagePath;
                calender.PrevYearImage = SampleImagePath;
                calender.NextYearImage = SampleImagePath;
                calender.NextMonthImage = SampleImagePath;
                calender.DayNameFormat = DayNameFormat.FirstLetter;
                calender.PrefixTodayFooter = DateTime.Now.ToShortDateString();
                return calender;
            };
            // Act
            var result = _calendarControlDesigner.GetDesignTimeHtml();

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetDesignTimeHtml_CalenderUseMultiSelectionIsEnabled_ReturnsHtmlStringContent()
        {
            // Arrange
            ShimComponentDesigner.AllInstances.ComponentGet = (x) =>
            {
                var calender = CreateCalendarObject(false, false);
                calender.UseDatePicker = false;
                calender.MultiSelection = true;
                calender.ShowHeaderNavigation = false;
                calender.PrevMonthImage = SampleImagePath;
                calender.PrevYearImage = SampleImagePath;
                calender.NextYearImage = SampleImagePath;
                calender.NextMonthImage = SampleImagePath;
                calender.DayNameFormat = DayNameFormat.FirstTwoLetters;
                calender.PrefixTodayFooter = DateTime.Now.ToShortDateString();
                calender.UseCustomDateFormat = false;
                return calender;
            };
            // Act
            var result = _calendarControlDesigner.GetDesignTimeHtml();

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void GetDesignTimeHtml_CalenderShowMonthAndYearTrue_ReturnsHtmlStringContent(bool useDatePicker, bool useMultiSelection)
        {
            // Arrange
            ShimComponentDesigner.AllInstances.ComponentGet = (x) =>
            {
                var calender = CreateCalendarObject(false, false);
                calender.UseDatePicker = useDatePicker;
                calender.MultiSelection = useMultiSelection;
                calender.ShowHeaderNavigation = false;
                calender.PrevMonthImage = SampleImagePath;
                calender.PrevYearImage = SampleImagePath;
                calender.NextYearImage = SampleImagePath;
                calender.NextMonthImage = SampleImagePath;
                calender.DayNameFormat = DayNameFormat.Full;
                calender.PrefixTodayFooter = DateTime.Now.ToShortDateString();
                calender.UseCustomDateFormat = false;
                calender.ShowMonth = true;
                calender.ShowYear = true;
                return calender;
            };
            // Act
            var result = _calendarControlDesigner.GetDesignTimeHtml();

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetDesignTimeHtml_CalenderUseSelectorsIsEnabled_ReturnsHtmlStringContent()
        {
            // Arrange
            ShimComponentDesigner.AllInstances.ComponentGet = (x) =>
            {
                var calender = CreateCalendarObject();
                calender.UseSelectors = true;
                calender.Format = CalanderDateFormat;
                return calender;
            };
            // Act
            var result = _calendarControlDesigner.GetDesignTimeHtml();

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        private Calendar CreateCalendarObject(bool includeStyleDate = true, bool includeBlockedDates = true)
        {
            var calender = new Calendar
            {
                UseSelectors = false,
                VisibleDate = DateTime.Now,
                ShowHeaderNavigation = true,
                UseDatePicker = true,
                ShowDateOnStart = true,
                SelectedDate = DateTime.Now,
                UseCustomDateFormat = true,
                ShowMonth = false,
                ShowYear = false,
                ShowWeekNumber = true,
                UseTime = true,
                ShowTodayFooter = true,
                SelectedDayStyle = new CalendarDayStyleShort { BackgroundImage = SampleImagePath }
            };
            if (includeStyleDate)
            {
                var dateStyleCollectionItem = new DateStyleCollectionItem();
                dateStyleCollectionItem.BackgroundImage = SampleImagePath;
                calender.StyleDates.Add(dateStyleCollectionItem);
            }
            var dateCollectionItem = new DateCollectionItem();
            if (includeBlockedDates)
            {
                calender.BlockedDates.Add(dateCollectionItem);
            }
            calender.SelectedDates.Add(dateCollectionItem);
            return calender;
        }

        private CssStyleCollection CssStyleCollection()
        {
            return (CssStyleCollection)typeof(CssStyleCollection).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0].Invoke(null);
        }

        private CssStyleCollection CreateCssStyleCollection()
        {
            var cssStyleCollection = CssStyleCollection();
            cssStyleCollection.Add(CssStyleLeftKey, CssStyleLeftValue);
            cssStyleCollection.Add(CssStyleTopKey, CssStyleTopValue);
            cssStyleCollection.Add(CssStylePositionKey, CssStylePositionValue);
            return cssStyleCollection;
        }
    }
}
