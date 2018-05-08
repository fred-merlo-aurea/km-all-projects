using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using KMPS.MD.Controls;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Controls
{
    /// <summary>
    /// Unit test for <see cref="Activity"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ActivityTestsLoadControls : BaseControlTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string DropDownOpenActivity = "drpOpenActivity";
        private const string DropDownOpenClickActivity = "drpClickActivity";
        private const string DropDownVisitActivity = "drpVisitActivity";
        private const string DropDownOpenActivityDateRange = "drpOpenActivityDateRange";
        private const string DropDownOpenActivityDays = "drpOpenActivityDays";
        private const string DropDownOpenEmailDays = "drpOpenEmailDays";
        private const string DropDownClickActivityDays = "drpClickActivityDays";
        private const string DropDownVisitActivityDays = "drpVisitActivityDays";
        private const string DropDownClickEmailDays = "drpClickEmailDays";
        private const string DropDownOpenEmailDateRange = "drpOpenEmailDateRange";
        private const string DropDownClickActivityDateRange = "drpClickActivityDateRange";
        private const string DropDownVisitActivityDateRange = "drpVisitActivityDateRange";
        private const string DropDownClickEmailDateRange = "drpClickEmailDateRange";
        private const string XDays = "XDays";
        private const string DateRange = "DateRange";
        private const string Year = "Year";
        private const string Month = "Month";
        private const string Custom = "Custom";
        private const string Custom1 = "Custom1";
        private const string EmptyDefaultStringValue = " ";
        private Activity _activity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _activity = new Activity();
            InitializeUserControl(_activity);
            InitializeAllControls(_activity);
        }

        [TestCase(TestZero, EmptyDefaultStringValue)]
        [TestCase(TestZero, XDays, Custom)]
        [TestCase(TestZero, XDays, Custom1)]
        [TestCase(TestZero, DateRange, Custom1)]
        [TestCase(TestZero, Year)]
        [TestCase(TestZero, Month)]
        [TestCase(TestOne, EmptyDefaultStringValue)]
        [TestCase(TestOne, XDays, Custom)]
        [TestCase(TestOne, XDays, Custom1)]
        [TestCase(TestOne, DateRange, Custom1)]
        [TestCase(TestOne, Year)]
        [TestCase(TestOne, Month)]
        public void LoadControls_OpenActivitySelectedValueIsZero_UpdateControlValues(string selectedValue, string activityRangeValue, string openActivityDays = Custom)
        {
            // Arrange
            BindPageDropDownControl(selectedValue, activityRangeValue, openActivityDays);

            // Act
            _activity.LoadControls();

            // Assert
            GetField<DropDownList>(DropDownOpenActivity).SelectedValue.ShouldBe(selectedValue);
            GetField<DropDownList>(DropDownOpenClickActivity).SelectedValue.ShouldBe(selectedValue);
            GetField<DropDownList>(DropDownVisitActivity).SelectedValue.ShouldBe(selectedValue);
            GetField<DropDownList>(DropDownOpenActivityDateRange).SelectedValue.ShouldBe(activityRangeValue);
            GetField<DropDownList>(DropDownOpenEmailDateRange).SelectedValue.ShouldBe(activityRangeValue);
            GetField<DropDownList>(DropDownClickActivityDateRange).SelectedValue.ShouldBe(activityRangeValue);
            GetField<DropDownList>(DropDownVisitActivityDateRange).SelectedValue.ShouldBe(activityRangeValue);
            GetField<DropDownList>(DropDownClickEmailDateRange).SelectedValue.ShouldBe(activityRangeValue);
            GetField<DropDownList>(DropDownOpenActivityDays).SelectedValue.ShouldBe(openActivityDays);
            GetField<DropDownList>(DropDownOpenEmailDays).SelectedValue.ShouldBe(openActivityDays);
            GetField<DropDownList>(DropDownClickActivityDays).SelectedValue.ShouldBe(openActivityDays);
            GetField<DropDownList>(DropDownVisitActivityDays).SelectedValue.ShouldBe(openActivityDays);
            GetField<DropDownList>(DropDownClickEmailDays).SelectedValue.ShouldBe(openActivityDays);
        }

        private void BindPageDropDownControl(string selectedValue, string activityRangeValue, string openActivityDays)
        {
            var dataSource = new string[] { "0", "1", "2", "3" }; ;
            var dataactivityDateRangeDataSource = new string[] { " ", "DateRange", "XDays", "Year", "Month" };
            var openActivityDaysDataSource = new string[] { "Custom", "Custom1" };

            var drpOpenActivity = GetField<DropDownList>(DropDownOpenActivity);
            drpOpenActivity.DataSource = dataSource;
            drpOpenActivity.SelectedValue = selectedValue;
            drpOpenActivity.DataBind();

            var drpClickActivity = GetField<DropDownList>(DropDownOpenClickActivity);
            drpClickActivity.DataSource = dataSource;
            drpClickActivity.SelectedValue = selectedValue;
            drpClickActivity.DataBind();

            var drpVisitActivity = GetField<DropDownList>(DropDownVisitActivity);
            drpVisitActivity.DataSource = dataSource;
            drpVisitActivity.SelectedValue = selectedValue;
            drpVisitActivity.DataBind();

            var drpOpenActivityDateRange = GetField<DropDownList>(DropDownOpenActivityDateRange);
            drpOpenActivityDateRange.DataSource = dataactivityDateRangeDataSource;
            drpOpenActivityDateRange.SelectedValue = activityRangeValue;
            drpOpenActivityDateRange.DataBind();

            var drpOpenEmailDateRange = GetField<DropDownList>(DropDownOpenEmailDateRange);
            drpOpenEmailDateRange.DataSource = dataactivityDateRangeDataSource;
            drpOpenEmailDateRange.SelectedValue = activityRangeValue;
            drpOpenEmailDateRange.DataBind();

            var drpClickActivityDateRange = GetField<DropDownList>(DropDownClickActivityDateRange);
            drpClickActivityDateRange.DataSource = dataactivityDateRangeDataSource;
            drpClickActivityDateRange.SelectedValue = activityRangeValue;
            drpClickActivityDateRange.DataBind();

            var drpVisitActivityDateRange = GetField<DropDownList>(DropDownVisitActivityDateRange);
            drpVisitActivityDateRange.DataSource = dataactivityDateRangeDataSource;
            drpVisitActivityDateRange.SelectedValue = activityRangeValue;
            drpVisitActivityDateRange.DataBind();

            var drpClickEmailDateRange = GetField<DropDownList>(DropDownClickEmailDateRange);
            drpClickEmailDateRange.DataSource = dataactivityDateRangeDataSource;
            drpClickEmailDateRange.SelectedValue = activityRangeValue;
            drpClickEmailDateRange.DataBind();


            var drpOpenActivityDays = GetField<DropDownList>(DropDownOpenActivityDays);
            drpOpenActivityDays.DataSource = openActivityDaysDataSource;
            drpOpenActivityDays.SelectedValue = openActivityDays;
            drpOpenActivityDays.DataBind();

            var drpOpenEmailDays = GetField<DropDownList>(DropDownOpenEmailDays);
            drpOpenEmailDays.DataSource = openActivityDaysDataSource;
            drpOpenEmailDays.SelectedValue = openActivityDays;
            drpOpenEmailDays.DataBind();

            var drpClickActivityDays = GetField<DropDownList>(DropDownClickActivityDays);
            drpClickActivityDays.DataSource = openActivityDaysDataSource;
            drpClickActivityDays.SelectedValue = openActivityDays;
            drpClickActivityDays.DataBind();

            var drpVisitActivityDays = GetField<DropDownList>(DropDownVisitActivityDays);
            drpVisitActivityDays.DataSource = openActivityDaysDataSource;
            drpVisitActivityDays.SelectedValue = openActivityDays;
            drpVisitActivityDays.DataBind();

            var drpClickEmailDays = GetField<DropDownList>(DropDownClickEmailDays);
            drpClickEmailDays.DataSource = openActivityDaysDataSource;
            drpClickEmailDays.SelectedValue = openActivityDays;
            drpClickEmailDays.DataBind();
        }
    }
}
