using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ecn.communicator.main.Reports.ReportSettingsControls;
using ecn.communicator.main.Reports.ReportSettingsControls.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Reports
{
    public partial class ScheduledReportSetupTest
    {
        private const string ToEmailControl = "txtToEmail";
        private const string FromEmailControl = "txtFromEmail";
        private const string RecurringStartDateControl = "txtRecurringStartDate";
        private const string DdlRecurringStartTime = "ddlRecurringStartTime";
        private const string DrpReportsControl = "drpReports";
        private const string RecurringEndDateControl = "txtRecurringEndDate";
        private const string PhErrorControl = "phError";
        private const string LblErrorMessageControl = "lblErrorMessage";
        private const string DdlScheduleTypeControl = "ddlScheduleType";
        private const string DdlRecurrenceControl = "ddlRecurrence";
        private const string CbDaysControl = "cbDays";
        private const string TxtStartDateControl = "txtStartDate";
        private const string DdlStartTimeControl = "ddlStartTime";
        private const string DrpDayofMonthControl = "drpDayofMonth";
        private const string DllFormatsControl = "dllFormats";

        private const string InvalidEmailAddress = "test1@test1.com";
        private const string SampleReportParameters = "SampleReportParameters";
        private const string SampleExportFormat = "Pdf";
        private const string RecurringScheduleType = "Recurring";
        private const string NonRecurringScheduleType = "NonRecurring";
        private const string WeeklyScheduleType = "Weekly";
        private const string MontlyScheduleType = "Monthly";
        private const string ctlReportSettingsField = "ctlReportSettings";
        private const string BtnSaveClickMethodName = "btnSave_Click";

        private static readonly List<string> ValidEmailAddress = new List<string>
        {
            "test@test.com",
            "sample@sample.com"
        };
        
        [Test]
        public void btnSave_Click_WithInvalidToEmailAddress_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();
            Get<TextBox>(_privateTestObject, ToEmailControl).Text = InvalidEmailAddress;

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.ShouldContain($"ReportSchedule: Invalid To Email");
        }

        [Test]
        public void btnSave_Click_WithInvalidFromEmailAddress_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();
            Get<TextBox>(_privateTestObject, FromEmailControl).Text = "sample1@sample.com";

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.ShouldContain($"ReportSchedule: Invalid From Email Address");
        }

        [Test]
        public void btnSave_Click_WithInvalidStartDate_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();
            Get<TextBox>(_privateTestObject, RecurringStartDateControl).Text = DateTime.Now.AddDays(-1).ToString();

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.ShouldContain($"ReportSchedule: Start Date cannot be in the past");
        }

        [Test]
        public void btnSave_Click_WithInvalidStartTime_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();
            Get<TextBox>(_privateTestObject, RecurringStartDateControl).Text = DateTime.Now.Date.ToString();
            var ddlRecurringStartTime = Get<DropDownList>(_privateTestObject, DdlRecurringStartTime);
            ddlRecurringStartTime.Items.RemoveAt(0);
            ddlRecurringStartTime.Items.Add(new ListItem
            {
                Value = DateTime.Now.TimeOfDay.Subtract(TimeSpan.FromMinutes(1)).ToString(),
                Selected = true
            });

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.ShouldContain($"ReportSchedule: Send Time cannot be in the past");
        }

        [Test]
        public void btnSave_Click_WithInvalidEndDate_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();
            Get<TextBox>(_privateTestObject, RecurringEndDateControl).Text = DateTime.Now.AddDays(-2).ToString();
            
            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.ShouldContain($"ReportSchedule: End Date cannot be in the past");
        }

        [Test]
        public void btnSave_Click_WithInvalidSelectedReportId_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();
            var drpReports = Get<DropDownList>(_privateTestObject, DrpReportsControl);
            drpReports.SelectedIndex = 0;

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.ShouldContain($"ReportSchedule: Please select a Report");
        }

        [Test]
        public void btnSave_Click_WithInvalidctlReportSettings_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();
            ShimDataDumpReport.AllInstances.IsValid = (d) => false;

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessageControl).Text.ShouldContain($"ReportSchedule: FTP Credentials are not valid");
        }

        [Test]
        public void btnSave_Click_WithNovalidationErrorAndReportScheduleId_SetsErrorLabelException()
        {
            // Arrange
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls();

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            _isSavedReportSchedule.ShouldBeTrue();
            _savedReportSchedule.ShouldNotBeNull();
            _savedReportSchedule.ShouldSatisfyAllConditions(
                () => _savedReportSchedule.CustomerID.ShouldBe(1),
                () => _savedReportSchedule.ReportScheduleID.ShouldBe(1),
                () => _savedReportSchedule.ReportID.ShouldBe(1),
                () => _savedReportSchedule.StartDate.ShouldContain(Get<TextBox>(_privateTestObject, TxtStartDateControl).Text),
                () => _savedReportSchedule.StartTime.ShouldContain(Get<DropDownList>(_privateTestObject, DdlStartTimeControl).SelectedValue),
                () => _savedReportSchedule.RunSunday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunMonday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunThursday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunWednesday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunTuesday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunSaturday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.MonthLastDay.Value.ShouldBeFalse(),
                () => _savedReportSchedule.MonthScheduleDay.ShouldBeNull(),
                () => _savedReportSchedule.FromEmail.ShouldBe(ValidEmailAddress[1]),
                () => _savedReportSchedule.ToEmail.ShouldBe(ValidEmailAddress[0]),
                () => _savedReportSchedule.ReportParameters.ShouldBe(SampleReportParameters),
                () => _savedReportSchedule.ExportFormat.ShouldBe(SampleExportFormat));
        }

        [Test]
        public void btnSave_Click_WithNovalidationErrorAndNoExistingReportScheduleId_SetsErrorLabelException()
        {
            // Arrange
            QueryString.Clear();
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls(scheduleType: NonRecurringScheduleType, recurrenceType: MontlyScheduleType);

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);
            
            // Assert
            _isSavedReportSchedule.ShouldBeTrue();
            _savedReportSchedule.ShouldNotBeNull();
            _savedReportSchedule.ShouldSatisfyAllConditions(
                () => _savedReportSchedule.CustomerID.ShouldBe(1),
                () => _savedReportSchedule.ReportScheduleID.ShouldBe(1),
                () => _savedReportSchedule.ReportID.ShouldBe(1),
                () => _savedReportSchedule.StartDate.ShouldContain(Get<TextBox>(_privateTestObject,TxtStartDateControl).Text),
                () => _savedReportSchedule.StartTime.ShouldContain(Get<DropDownList>(_privateTestObject,DdlStartTimeControl).SelectedValue),
                () => _savedReportSchedule.RunSunday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunMonday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunThursday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunWednesday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunTuesday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunSaturday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.MonthLastDay.Value.ShouldBeTrue(),
                () => _savedReportSchedule.MonthScheduleDay.ShouldBeNull(),
                () =>  _savedReportSchedule.FromEmail.ShouldBe(ValidEmailAddress[1]),
                () => _savedReportSchedule.ToEmail.ShouldBe(ValidEmailAddress[0]),
                () => _savedReportSchedule.ReportParameters.ShouldBe(SampleReportParameters),
                () => _savedReportSchedule.ExportFormat.ShouldBe(SampleExportFormat)); 
        }

        [Test]
        public void btnSave_Click_WithNoExistingReportScheduleIdAndDayOfMonthNotLast_SetsErrorLabelException()
        {
            // Arrange
            QueryString.Clear();
            SetUpFakesForBtnSaveMethodClick();
            SetPageControls(
                scheduleType: NonRecurringScheduleType,
                recurrenceType: MontlyScheduleType,
                dayOfMonth: DateTime.UtcNow.DayOfYear.ToString());

            // Act
            _privateTestObject.Invoke(BtnSaveClickMethodName, this, EventArgs.Empty);

            // Assert
            _isSavedReportSchedule.ShouldBeTrue();
            _savedReportSchedule.ShouldNotBeNull();
            _savedReportSchedule.ShouldSatisfyAllConditions(
                () => _savedReportSchedule.CustomerID.ShouldBe(1),
                () => _savedReportSchedule.ReportScheduleID.ShouldBe(1),
                () => _savedReportSchedule.ReportID.ShouldBe(1),
                () => _savedReportSchedule.StartDate.ShouldContain(Get<TextBox>(_privateTestObject, TxtStartDateControl).Text),
                () => _savedReportSchedule.StartTime.ShouldContain(Get<DropDownList>(_privateTestObject, DdlStartTimeControl).SelectedValue),
                () => _savedReportSchedule.RunSunday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunMonday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunThursday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunWednesday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunTuesday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.RunSaturday.Value.ShouldBeTrue(),
                () => _savedReportSchedule.MonthLastDay.Value.ShouldBeFalse(),
                () => _savedReportSchedule.MonthScheduleDay.ShouldBe(DateTime.UtcNow.DayOfYear),
                () => _savedReportSchedule.FromEmail.ShouldBe(ValidEmailAddress[1]),
                () => _savedReportSchedule.ToEmail.ShouldBe(ValidEmailAddress[0]),
                () => _savedReportSchedule.ReportParameters.ShouldBe(SampleReportParameters),
                () => _savedReportSchedule.ExportFormat.ShouldBe(SampleExportFormat));
        }

        private void SetUpFakesForBtnSaveMethodClick()
        {
            ShimEmail.IsValidEmailAddressString = (email) =>
            {
                return ValidEmailAddress.Any(x => x.Equals(email));
            };

            ShimDataDumpReport.AllInstances.IsValid = (d) => true;
            ShimDataDumpReport.AllInstances.GetParameters = (d) => SampleReportParameters; 
            ShimReportSchedule.GetByReportScheduleIDInt32User = (id, user) => new ReportSchedule { };
            ShimReportSchedule.SaveReportScheduleUser = (rpt, user) => 
            {
                _savedReportSchedule = rpt;
                _savedReportSchedule.ReportScheduleID = 1;
                _isSavedReportSchedule = true;
                return _savedReportSchedule.ReportScheduleID;
            };
        }

        private void SetPageControls(
            string scheduleType = RecurringScheduleType,
            string recurrenceType = WeeklyScheduleType,
            string dayOfMonth = "Last Day")
        {
            Get<TextBox>(_privateTestObject, ToEmailControl).Text = ValidEmailAddress[0];
            Get<TextBox>(_privateTestObject, FromEmailControl).Text = ValidEmailAddress[1];
            Get<TextBox>(_privateTestObject, RecurringStartDateControl).Text = DateTime.Now.AddDays(1).ToString();
            Get<TextBox>(_privateTestObject, RecurringEndDateControl).Text = DateTime.Now.AddDays(10).ToString();
            _privateTestObject.SetFieldOrProperty(ctlReportSettingsField, new DataDumpReport());

            // DropDownList
            var ddlRecurringStartTime = Get<DropDownList>(_privateTestObject, DdlRecurringStartTime);
            ddlRecurringStartTime.Items.Add(new ListItem
            {
                Value = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(2)).ToString(),
                Selected = true
            });

            var drpReports = Get<DropDownList>(_privateTestObject, DrpReportsControl);
            drpReports.Items.Add(new ListItem { Value = "0" });
            drpReports.Items.Add(new ListItem { Value = "1", Selected = true });
            
            var ddlScheduleType = Get<DropDownList>(_privateTestObject, DdlScheduleTypeControl);
            ddlScheduleType.Items.Add(new ListItem { Value = scheduleType });
            
            var ddlRecurrence = Get<DropDownList>(_privateTestObject, DdlRecurrenceControl);
            ddlRecurrence.Items.Add(new ListItem { Value = recurrenceType });

            var dllFormats = Get<DropDownList>(_privateTestObject, DllFormatsControl);
            dllFormats.Items.Add(new ListItem { Value = SampleExportFormat });

            var drpDayofMonth = Get<DropDownList>(_privateTestObject, DrpDayofMonthControl);
            drpDayofMonth.Items.Add(new ListItem { Value = dayOfMonth });

            var cbDays = Get<RadioButtonList>(_privateTestObject, CbDaysControl);
            cbDays.Items.Add(new ListItem { Value = DayOfWeek.Sunday.ToString(), Selected = true });
            cbDays.Items.Add(new ListItem { Value = DayOfWeek.Monday.ToString(), Selected = true });
            cbDays.Items.Add(new ListItem { Value = DayOfWeek.Tuesday.ToString(), Selected = true });
            cbDays.Items.Add(new ListItem { Value = DayOfWeek.Wednesday.ToString(), Selected = true });
            cbDays.Items.Add(new ListItem { Value = DayOfWeek.Thursday.ToString(), Selected = true });
            cbDays.Items.Add(new ListItem { Value = DayOfWeek.Friday.ToString(), Selected = true });
            cbDays.Items.Add(new ListItem { Value = DayOfWeek.Saturday.ToString(), Selected = true });
        }
    }
}
