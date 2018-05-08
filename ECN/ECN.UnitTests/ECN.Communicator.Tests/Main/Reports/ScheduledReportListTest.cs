using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using ecn.communicator.main.Reports;
using ecn.communicator.main.Reports.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;
using ReportCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.Reports
{
    /// <summary>
    /// Unit test for <see cref="ScheduledReportList"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ScheduledReportListTest : PageHelper
    {
        private const string GridViewScheduledReportsRowDataBound = "gvScheduledReports_RowDataBound";
        private const string ImageButtonEdit = "imgbtnEdit";
        private const string LabelReportName = "lblReportName";
        private const string LabelScheduleType = "lblScheduleType";
        private const string LabelRecurrenceType = "lblRecurrenceType";
        private const string LabelMonthScheduleDay = "lblMonthScheduleDay";
        private const string LabelNextScheduledDate = "lblNextScheduledDate";
        private const string Monthly = "monthly";
        private const string Daily = "daily";
        private const string Weekly = "weekly";
        private const int MaxIterations = 20;
        private scheduledReportList _scheduledReportList;
        private PrivateObject _privateObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _scheduledReportList = new scheduledReportList();
            InitializeAllControls(_scheduledReportList);
            _privateObject = new PrivateObject(_scheduledReportList);
            ShimCurrentUser();
            CreatePageShimObject();
            ShimReports.GetByReportIDInt32User = (x, y) =>
            {
                return new ReportCommunicator.Reports
                {
                    ReportID = 100,
                    IsExport = true,
                    ReportName = "Unit Test Report",
                    ControlName = "Unit test",
                    ShowInSetup = true
                };
            };
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(8)]
        [TestCase(11)]
        [TestCase(12)]
        public void GridViewScheduledReportsRowDataBound_RecurrenceTypeIsDaily_UpdatedPageControlValue(int reportId)
        {
            // Arrange
            var gridView = (GridView)_privateObject.GetFieldOrProperty("gvScheduledReports");
            var row = CreateGridViewRow(Daily, bool.FalseString.ToLower());
            var gridViewEvent = new GridViewRowEventArgs(row);
            gridViewEvent.Row.DataItem = new ReportSchedule { ReportID = reportId };
            var parameters = new object[] { this, gridViewEvent };

            // Act
            _privateObject.Invoke(GridViewScheduledReportsRowDataBound, parameters);

            // Assert
            var imageButtonEdit = gridViewEvent.Row.FindControl(ImageButtonEdit) as ImageButton;
            var labelReportName = gridViewEvent.Row.FindControl(LabelReportName) as Label;
            var lblScheduleType = gridViewEvent.Row.FindControl(LabelScheduleType) as Label;
            var lblNextScheduledDate = gridViewEvent.Row.FindControl(LabelNextScheduledDate) as Label;
            gridViewEvent.ShouldSatisfyAllConditions
            (
                () => imageButtonEdit.Visible.ShouldBeFalse(),
                () => labelReportName.Text.ShouldBe("Unit Test Report"),
                () => lblScheduleType.Text.ShouldBe(Daily),
                () => lblNextScheduledDate.Text.ShouldNotBeNullOrEmpty()
            );
        }

        [TestCase(1, false)]
        [TestCase(1, true)]
        public void GridViewScheduledReportsRowDataBound_RecurrenceTypeIsMonthly_UpdatedPageControlValue(int reportId, bool monthLastDay)
        {
            // Arrange
            var gridView = (GridView)_privateObject.GetFieldOrProperty("gvScheduledReports");
            var row = CreateGridViewRow(Monthly, monthLastDay.ToString());
            var gridViewEvent = new GridViewRowEventArgs(row);
            gridViewEvent.Row.DataItem = new ReportSchedule { ReportID = reportId };
            var parameters = new object[] { this, gridViewEvent };

            // Act
            _privateObject.Invoke(GridViewScheduledReportsRowDataBound, parameters);

            // Assert
            var imageButtonEdit = gridViewEvent.Row.FindControl(ImageButtonEdit) as ImageButton;
            var labelReportName = gridViewEvent.Row.FindControl(LabelReportName) as Label;
            var lblScheduleType = gridViewEvent.Row.FindControl(LabelScheduleType) as Label;
            var labelMonthScheduleDay = gridViewEvent.Row.FindControl(LabelMonthScheduleDay) as Label;
            var lblNextScheduledDate = gridViewEvent.Row.FindControl(LabelNextScheduledDate) as Label;
            gridViewEvent.ShouldSatisfyAllConditions
            (
                () => imageButtonEdit.Visible.ShouldBeFalse(),
                () => labelReportName.Text.ShouldBe("Unit Test Report"),
                () => lblScheduleType.Text.ShouldBe(Monthly),
                () => labelMonthScheduleDay.Text.ShouldBe("1"),
                () => lblNextScheduledDate.Text.ShouldNotBeNullOrEmpty()
            );
        }

        [TestCase(1, DayOfWeek.Sunday)]
        [TestCase(1, DayOfWeek.Monday)]
        [TestCase(1, DayOfWeek.Tuesday)]
        [TestCase(1, DayOfWeek.Wednesday)]
        [TestCase(1, DayOfWeek.Thursday)]
        [TestCase(1, DayOfWeek.Friday)]
        [TestCase(1, DayOfWeek.Saturday)]
        public void GridViewScheduledReportsRowDataBound_RecurrenceTypeIsWeekly_UpdatedPageControlValue(int reportId, DayOfWeek dayOfweek)
        {
            // Arrange
            var row = CreateGridViewRow(Weekly, bool.FalseString.ToLower());
            var gridViewEvent = new GridViewRowEventArgs(row);
            gridViewEvent.Row.DataItem = new ReportSchedule { ReportID = reportId };
            var parameters = new object[] { this, gridViewEvent };
            var dateList = GetDates();
            ShimDateTime.NowGet =
               () =>
               {
                   return dateList.FirstOrDefault(x => x.DayOfWeek == dayOfweek);
               };

            // Act
            _privateObject.Invoke(GridViewScheduledReportsRowDataBound, parameters);

            // Assert
            var imageButtonEdit = gridViewEvent.Row.FindControl(ImageButtonEdit) as ImageButton;
            var labelReportName = gridViewEvent.Row.FindControl(LabelReportName) as Label;
            var lblScheduleType = gridViewEvent.Row.FindControl(LabelScheduleType) as Label;
            var labelMonthScheduleDay = gridViewEvent.Row.FindControl(LabelMonthScheduleDay) as Label;
            var lblNextScheduledDate = gridViewEvent.Row.FindControl(LabelNextScheduledDate) as Label;
            gridViewEvent.ShouldSatisfyAllConditions
            (
                () => imageButtonEdit.Visible.ShouldBeFalse(),
                () => labelReportName.Text.ShouldBe("Unit Test Report"),
                () => lblScheduleType.Text.ShouldBe(Weekly),
                () => labelMonthScheduleDay.Text.ShouldBe("1"),
                () => lblNextScheduledDate.Text.ShouldNotBeNullOrEmpty()
            );
        }

        private GridViewRow CreateGridViewRow(string recurrenceType, string monthLastDay)
        {
            int rowIndex = 1;
            int dataItemIndex = 1;
            var rowType = DataControlRowType.DataRow;
            var rowState = DataControlRowState.Normal;
            var row = new GridViewRow(rowIndex, dataItemIndex, rowType, rowState);
            for (int i = 0; i < MaxIterations; i++)
            {
                row.Cells.Add(new TableCell());
            }
            var dictionalryLableObject = CreateLableControl(recurrenceType, monthLastDay);
            for (int i = 0; i < dictionalryLableObject.Count; i++)
            {
                var controlResult = dictionalryLableObject.ElementAt(i);
                row.Cells[i].Controls.Add(CreateLabel(controlResult.Key, controlResult.Value));
            }
            var lnkbtnReportDetails = CreateLinkButton("lnkbtnReportDetails");
            row.Cells[16].Controls.Add(lnkbtnReportDetails);
            var imgbtnParamDelete = CreateImageButton("imgbtnParamDelete");
            row.Cells[17].Controls.Add(imgbtnParamDelete);
            var imgbtnEdit = CreateImageButton("imgbtnEdit");
            row.Cells[18].Controls.Add(imgbtnEdit);
            return row;
        }

        private static Dictionary<string, string> CreateLableControl(string recurrenceType, string monthLastDay)
        {
            var dictionalryLableObject = new Dictionary<string, string>();
            dictionalryLableObject.Add("lblScheduleType", "Recurring");
            dictionalryLableObject.Add("lblRecurrenceType", recurrenceType);
            dictionalryLableObject.Add("lblNextScheduledDate", DateTime.Now.AddDays(1).ToString());
            dictionalryLableObject.Add("lblStartTime", DateTime.Now.TimeOfDay.ToString());
            dictionalryLableObject.Add("lblEndDate", DateTime.Now.TimeOfDay.ToString());
            dictionalryLableObject.Add("lblStartDate", DateTime.Now.AddDays(1).ToString());
            dictionalryLableObject.Add("lblReportName", "Unit Test Report");
            dictionalryLableObject.Add("lblMonthLastDay", monthLastDay);
            dictionalryLableObject.Add("lblMonthScheduleDay", "1");
            dictionalryLableObject.Add("lblRunSunday", bool.TrueString.ToLower());
            dictionalryLableObject.Add("lblRunMonday", bool.TrueString.ToLower());
            dictionalryLableObject.Add("lblRunTuesday", bool.TrueString.ToLower());
            dictionalryLableObject.Add("lblRunWednesday", bool.TrueString.ToLower());
            dictionalryLableObject.Add("lblRunThursday", bool.TrueString.ToLower());
            dictionalryLableObject.Add("lblRunFriday", bool.TrueString.ToLower());
            dictionalryLableObject.Add("lblRunSaturday", bool.TrueString.ToLower());
            return dictionalryLableObject;
        }

        private void CreatePageShimObject()
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            ecnSession.CurrentBaseChannel = new BaseChannel { BaseChannelID = 3 };
            ecnSession.CurrentUser = new KMPlatform.Entity.User
            {
                UserID = 1,
                UserName = "TestUser",
                IsActive = true,
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true
            };

            Common.Fakes.ShimMasterPageEx.AllInstances.HeadingSetString = (masterpageEx, inputString) => { };
            Common.Fakes.ShimMasterPageEx.AllInstances.HelpContentSetString = (masterpageEx, inputString) => { };
            Common.Fakes.ShimMasterPageEx.AllInstances.HelpTitleSetString = (masterpageEx, inputString) => { };

            ShimscheduledReportList.AllInstances.MasterGet = (x) =>
            {
                ecn.communicator.MasterPages.Communicator communicator = new ShimCommunicator
                {
                    CurrentMenuCodeGet = () => { return MenuCode.OMNITURE; },
                    CurrentMenuCodeSetEnumsMenuCode = (y) => { },
                    UserSessionGet = () => { return ecnSession; }
                };
                return communicator;
            };
        }

        private void ShimCurrentUser(bool isActive = true)
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new KMPlatform.Entity.User
            {
                UserID = 1,
                UserName = "TestUser",
                IsActive = isActive,
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true
            };
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = 1, CustomerName = "TestUser" };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }

        private static LinkButton CreateLinkButton(string id)
        {
            var linkButton = new LinkButton();
            linkButton.ID = id;
            linkButton.Text = "Approve";
            linkButton.CommandArgument = string.Empty;
            return linkButton;
        }

        private static Label CreateLabel(string id, string value = "")
        {
            var label = new Label();
            label.ID = id;
            label.Text = value;
            return label;
        }

        private static ImageButton CreateImageButton(string id)
        {
            var imageButton = new ImageButton();
            imageButton.ID = id;
            imageButton.Visible = true;
            return imageButton;
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
