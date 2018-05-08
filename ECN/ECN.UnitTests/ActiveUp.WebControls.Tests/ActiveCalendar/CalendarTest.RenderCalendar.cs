using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Web.UI.Fakes;
using System.Web.Fakes;
using System.Web;
using System.Web.Configuration.Fakes;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveCalendar
{
    /// <summary>
    /// Unit test for <see cref="Calendar"/> class
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class CalendarTestRenderCalendar
    {
        private const string SampleImagePath = "http://km.com/abc.png";
        private const string CalanderDateFormat = "DAY;MONTH;YEAR;HOUR;MINUTE;SECOND;MILLISECOND;MERIDIEM;";
        private const string CssStyleLeftKey = "LEFT";
        private const string CssStyleLeftValue = "10";
        private const string CssStyleTopKey = "TOP";
        private const string CssStyleTopValue = "20";
        private const string CssStylePositionKey = "POSITION";
        private const string CssStylePositionValue = "center";
        private const string RenderCalendar = "RenderCalendar";
        private delegate int CalculationHandler(int x, int y);
        private Calendar _calendar;
        private IDisposable _context;
        private PrivateObject _privateObject;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            ShimWebControl.AllInstances.StyleGet = (x) =>
            {
                return CreateCssStyleCollection();
            };
            ShimHttpCapabilitiesBase.AllInstances.BrowserGet = (x) => "IE";
            HttpRequest request = new ShimHttpRequest
            {
                BrowserGet = () => new HttpBrowserCapabilities()
            };
            ShimControl.AllInstances.PageGet = (x) =>
            {
                Page page = new ShimPage
                {
                    RequestGet = () => request,
                    ClientScriptGet = () =>
                    {
                        return new ShimClientScriptManager
                        {
                            GetWebResourceUrlTypeString = (type, ressourceName) => { return SampleImagePath; }
                        };
                    }
                };
                return page;
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void RenderCalendar_RenderLevelIsGreateThenOneAndUserSelectorIsFalse_ReturnsCalenderHtmlContent()
        {
            // Arrange
            _calendar = CreateCalendarObject();
            _calendar.SelectedDate = DateTime.MinValue;
            _calendar.Enabled = false;
            _privateObject = new PrivateObject(_calendar);
            var stringWriter = new StringWriter();
            var output = new HtmlTextWriter(stringWriter);
            _privateObject.SetFieldOrProperty("_autoPostBack", true);
            ShimControl.AllInstances.EventsGet = (x) =>
            {
                var eventHandlerList = new EventHandlerList();
                CalculationHandler sumHandler = new CalculationHandler(Sum);
                eventHandlerList.AddHandler("click", sumHandler);
                return eventHandlerList;
            };

            // Act
            _privateObject.Invoke(RenderCalendar, output);
            // Assert
            output.ShouldSatisfyAllConditions(
             () => output.ShouldNotBeNull(),
             () => output.InnerWriter.ShouldNotBeNull()
            );

        }

        [Test]
        public void RenderCalendar_CalenderUseDatePickerOptionIsDisabled_ReturnsHtmlStringContent()
        {
            // Arrange
            _calendar = CreateCalendarObject(false);
            _calendar.UseDatePicker = false;
            _calendar.MultiSelection = true;
            _calendar.ShowHeaderNavigation = false;
            _calendar.PrevMonthImage = SampleImagePath;
            _calendar.PrevYearImage = SampleImagePath;
            _calendar.NextYearImage = SampleImagePath;
            _calendar.NextMonthImage = SampleImagePath;
            _calendar.DayNameFormat = DayNameFormat.FirstLetter;
            _calendar.PrefixTodayFooter = DateTime.Now.ToShortDateString();
            _privateObject = new PrivateObject(_calendar);
            var stringWriter = new StringWriter();
            var output = new HtmlTextWriter(stringWriter);

            // Act
            _privateObject.Invoke(RenderCalendar, output);

            // Assert
            output.ShouldSatisfyAllConditions(
             () => output.ShouldNotBeNull(),
             () => output.InnerWriter.ShouldNotBeNull()
            );
        }

        [Test]
        public void RenderCalendar_CalenderUseMultiSelectionIsEnabled_ReturnsHtmlStringContent()
        {
            // Arrange
            _calendar = CreateCalendarObject(false, false);
            _calendar.UseDatePicker = false;
            _calendar.MultiSelection = true;
            _calendar.ShowHeaderNavigation = false;
            _calendar.PrevMonthImage = SampleImagePath;
            _calendar.PrevYearImage = SampleImagePath;
            _calendar.NextYearImage = SampleImagePath;
            _calendar.NextMonthImage = SampleImagePath;
            _calendar.DayNameFormat = DayNameFormat.FirstTwoLetters;
            _calendar.PrefixTodayFooter = DateTime.Now.ToShortDateString();
            _calendar.UseCustomDateFormat = false;
            _privateObject = new PrivateObject(_calendar);
            var stringWriter = new StringWriter();
            var output = new HtmlTextWriter(stringWriter);
            // Act
            _privateObject.Invoke(RenderCalendar, output);

            // Assert
            output.ShouldSatisfyAllConditions(
             () => output.ShouldNotBeNull(),
             () => output.InnerWriter.ShouldNotBeNull()
            );
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void RenderCalendar_CalenderShowMonthAndYearTrue_ReturnsHtmlStringContent(bool useDatePicker, bool useMultiSelection)
        {
            // Arrange

            _calendar = CreateCalendarObject(false, false);
            _calendar.UseDatePicker = useDatePicker;
            _calendar.MultiSelection = useMultiSelection;
            _calendar.ShowHeaderNavigation = false;
            _calendar.PrevMonthImage = SampleImagePath;
            _calendar.PrevYearImage = SampleImagePath;
            _calendar.NextYearImage = SampleImagePath;
            _calendar.NextMonthImage = SampleImagePath;
            _calendar.DayNameFormat = DayNameFormat.Full;
            _calendar.PrefixTodayFooter = DateTime.Now.ToShortDateString();
            _calendar.UseCustomDateFormat = false;
            _calendar.ShowMonth = true;
            _calendar.ShowYear = true;
            _privateObject = new PrivateObject(_calendar);
            var stringWriter = new StringWriter();
            var output = new HtmlTextWriter(stringWriter);
            // Act
            _privateObject.Invoke(RenderCalendar, output);

            // Assert
            output.ShouldSatisfyAllConditions(
             () => output.ShouldNotBeNull(),
             () => output.InnerWriter.ShouldNotBeNull()
            );
        }

        [Test]
        public void RenderCalendar_CalenderUseSelectorsIsEnabled_ReturnsHtmlStringContent()
        {
            // Arrange
            _calendar = CreateCalendarObject();
            _calendar.UseSelectors = true;
            _calendar.Format = CalanderDateFormat;
            _privateObject = new PrivateObject(_calendar);
            var stringWriter = new StringWriter();
            var output = new HtmlTextWriter(stringWriter);

            // Act
            _privateObject.Invoke(RenderCalendar, output);

            // Assert
            output.ShouldSatisfyAllConditions(
             () => output.ShouldNotBeNull(),
             () => output.InnerWriter.ShouldNotBeNull()
            );
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

        private Calendar CreateCalendarObject(bool includeStyleDate = true, bool includeBlockedDates = true)
        {
            var calender = new Calendar
            {
                UseSelectors = false,
                VisibleDate = DateTime.Now,
                ShowHeaderNavigation = false,
                UseDatePicker = true,
                ShowDateOnStart = true,
                SelectedDate = DateTime.Now,
                UseCustomDateFormat = true,
                ShowMonth = false,
                ShowYear = false,
                ShowWeekNumber = true,
                UseTime = true,
                ShowTodayFooter = true,
                SelectedDayStyle = new CalendarDayStyleShort { BackgroundImage = SampleImagePath },
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

        private int Sum(int x, int y)
        {
            return x + y;
        }
    }
}
