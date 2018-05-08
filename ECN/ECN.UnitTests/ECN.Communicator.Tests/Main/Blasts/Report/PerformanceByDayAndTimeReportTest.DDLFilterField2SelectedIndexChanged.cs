using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ecn.communicator.main.blasts.Report.Fakes;
using ecn.communicator.main.ECNWizard.Content.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN.Common.Fakes;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Application;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using ActivityReport = ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport;
using ActivityReportShim = ECN_Framework_BusinessLayer.Activity.Report.Fakes.ShimPerformanceByDayAndTimeReport;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    /// <summary>
    /// Unit Tests for <see cref="PerformanceByDayAndTimeReport"/>
    /// </summary>
    public partial class PerformanceByDayAndTimeReportTest
    {
        private const string MethodDDLFilterField1SelectedIndexChanged = "ddlFilterField1_SelectedIndexChanged"; 
        private const string MethodDDLFilterField2SelectedIndexChanged = "ddlFilterField2_SelectedIndexChanged";
        private const string MethodDDLFilterValue1SelectedIndexChanged = "ddlFilterValue1_SelectedIndexChanged"; 
        private const string MethodButtonReportClick = "btnReport_Click"; 
        private const string MethodOnBubbleEvent = "OnBubbleEvent"; 
        private const string MethodPageLoad = "Page_Load"; 
        private const string TestOne = "1";
        private const string TestTwo = "2";
        private DropDownList ddlFilterField1;
        private DropDownList ddlFilterField2;

        [Test]
        public void Page_Load_Success_ControlsAreInitialized()
        {
            // Arrange
            InitializeFakes();
            ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            ShimlayoutExplorer.AllInstances.enableSelectMode = (x) => { };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodPageLoad, methodArgs, _page);

            // Assert
            var errorMessage = ReflectionHelper.GetFieldValue(_page, "lblErrorMessage") as Label;
            var filterValue = ReflectionHelper.GetFieldValue(_page, "lblFilterValue2") as Label;
            var error = ReflectionHelper.GetFieldValue(_page, "phError") as PlaceHolder;
            _page.ShouldSatisfyAllConditions(
                () => errorMessage.ShouldNotBeNull(),
                () => filterValue.ShouldNotBeNull(),
                () => error.ShouldNotBeNull(),
                () => errorMessage.Text.ShouldBeEmpty(),
                () => filterValue.Visible.ShouldBeFalse(),
                () => error.Visible.ShouldBeFalse());
        }

        [Test]
        public void BtnReport_Click_Success_ChartsAreDrawn()
        {
            // Arrange
            InitializeFakes();
            var chartsDrawn = false;
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown("Campaign"));
            _page.SetField(GetField("txtstartDate"), new TextBox { Text = StartDate });
            _page.SetField(GetField("txtendDate"), new TextBox { Text = EndDate });
            _page.SetField(GetField("ddlOpensOrClicks"), SetDropDown("Campaign"));
            _page.SetField(GetField("ddlDay"), SetDropDown("Campaign"));
            _page.SetField(GetField("ddlFilterValue2"), SetDropDown(TestOne));
            ActivityReportShim.GetInt32DateTimeDateTimeStringInt32StringNullableOfInt32 = (a, b, c, d, e, f, g) => new List<ActivityReport> { ReflectionHelper.CreateInstance(typeof(ActivityReport)) };
            ShimPerformanceByDayAndTimeReport.AllInstances.drawDayChartString = (x, y) => { };
            ShimPerformanceByDayAndTimeReport.AllInstances.drawWeekChartString = (x, y) =>
            {
                chartsDrawn = true;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodButtonReportClick, methodArgs, _page);

            // Assert
            chartsDrawn.ShouldBeTrue();
        }

        [Test]
        public void BtnReport_Click_Failure_ChartsAreaIsCleared()
        {
            // Arrange
            InitializeFakes();
            var chartAreaCleared = false;
            _page.SetField(GetField("txtstartDate"), new TextBox { Text = "3/16/2019" });
            _page.SetField(GetField("txtendDate"), new TextBox { Text = EndDate });
            ShimPerformanceByDayAndTimeReport.AllInstances.setECNErrorECNException = (x, y) => 
            {
                chartAreaCleared = true;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodButtonReportClick, methodArgs, _page);

            // Assert
            chartAreaCleared.ShouldBeTrue();
        }

        [TestCase("Campaign")]
        [TestCase("Message Type")]
        [TestCase("Group")]
        public void DdlFilterField1_SelectedIndexChanged_WhenListContainsItems_FilterValueDropDownIsSet(string selectedFilter)
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown(selectedFilter));
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterField1SelectedIndexChanged, methodArgs, _page);

            // Assert
            var ddlFilterValue1 = ReflectionHelper.GetFieldValue(_page, "ddlFilterValue1") as DropDownList;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue1.ShouldNotBeNull(),
                () => ddlFilterValue1.DataTextField.ShouldNotBeNullOrWhiteSpace(),
                () => ddlFilterValue1.DataValueField.ShouldNotBeNullOrWhiteSpace(),
                () => ddlFilterValue1.SelectedIndex.ShouldBe(0),
                () => ddlFilterValue1.Enabled.ShouldBeTrue(),
                () => ddlFilterValue1.Visible.ShouldBeTrue());
        }

        [Test]
        public void DdlFilterField1_SelectedIndexChanged_WhenSelectedFilterIsMessage_LayoutPanelIsShown()
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown("Message"));
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterField1SelectedIndexChanged, methodArgs, _page);

            // Assert
            var layoutPanel = ReflectionHelper.GetFieldValue(_page, "pnlLayout") as Panel;
            var filterLabel = ReflectionHelper.GetFieldValue(_page, "lblFilterValue1") as Label;
            _page.ShouldSatisfyAllConditions(
                () => layoutPanel.ShouldNotBeNull(),
                () => filterLabel.ShouldNotBeNull(),
                () => layoutPanel.Visible.ShouldBeTrue(),
                () => filterLabel.Visible.ShouldBeTrue());
        }

        [TestCase("Campaign")]
        [TestCase("Message Type")]
        [TestCase("Group")]
        public void DdlFilterField1_SelectedIndexChanged_WhenListIsEmpty_ErrorIsDisplayed(string selectedFilter)
        {
            // Arrange
            InitializeFakes();
            var errorText = "Please make sure that you have";
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown(selectedFilter));
            ShimCampaign.GetByCustomerID_NoAccessCheckInt32Boolean = (x, y) => new List<Campaign>();
            ShimMessageType.GetByBaseChannelIDInt32User = (x, y) => new List<MessageType>();
            ShimGroup.GetByCustomerID_NoAccessCheckInt32String = (x, y) => new List<Group>();
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterField1SelectedIndexChanged, methodArgs, _page);

            // Assert
            var ddlFilterValue1 = ReflectionHelper.GetFieldValue(_page, "ddlFilterValue1") as DropDownList;
            var errorMessage = ReflectionHelper.GetFieldValue(_page, "lblErrorMessage") as Label;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue1.ShouldNotBeNull(),
                () => errorMessage.ShouldNotBeNull(),
                () => errorMessage.Text.ShouldNotBeNullOrWhiteSpace(),
                () => errorMessage.Text.ShouldContain(errorText));
        }

        [TestCase("Campaign")]
        [TestCase("Message Type")]
        [TestCase("Group")] 
        public void DdlFilterField2_SelectedIndexChanged_WhenListContainsItems_FilterValueDropDownIsSet(string selectedFilter)
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField(nameof(ddlFilterField2)), SetDropDown(selectedFilter));
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterField2SelectedIndexChanged, methodArgs, _page);

            // Assert
            var ddlFilterValue2 = ReflectionHelper.GetFieldValue(_page, "ddlFilterValue2") as DropDownList;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue2.ShouldNotBeNull(),
                () => ddlFilterValue2.DataTextField.ShouldNotBeNullOrWhiteSpace(),
                () => ddlFilterValue2.DataValueField.ShouldNotBeNullOrWhiteSpace(),
                () => ddlFilterValue2.SelectedIndex.ShouldBe(0),
                () => ddlFilterValue2.Enabled.ShouldBeTrue(),
                () => ddlFilterValue2.Visible.ShouldBeTrue());
        }

        [Test]
        public void DdlFilterField2_SelectedIndexChanged_WhenSelectedFilterIsMessage_LayoutPanelIsShown()
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField(nameof(ddlFilterField2)), SetDropDown("Message"));
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterField2SelectedIndexChanged, methodArgs, _page);

            // Assert
            var layoutPanel = ReflectionHelper.GetFieldValue(_page, "pnlLayout2") as Panel;
            var filterLabel = ReflectionHelper.GetFieldValue(_page, "lblFilterValue2") as Label;
            _page.ShouldSatisfyAllConditions(
                () => layoutPanel.ShouldNotBeNull(),
                () => filterLabel.ShouldNotBeNull(),
                () => layoutPanel.Visible.ShouldBeTrue(),
                () => filterLabel.Visible.ShouldBeTrue());
        }

        [Test]
        public void DdlFilterField2_SelectedIndexChanged_WhenSelectedFilterIsNone_ReportButtonIsEnabled()
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField(nameof(ddlFilterField2)), SetDropDown("None"));
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterField2SelectedIndexChanged, methodArgs, _page);

            // Assert
            var reportButton = ReflectionHelper.GetFieldValue(_page, "btnReport") as Button;
            _page.ShouldSatisfyAllConditions(
                () => reportButton.ShouldNotBeNull(),
                () => reportButton.Enabled.ShouldBeTrue());
        }

        [TestCase("Campaign")]
        [TestCase("Message Type")]
        [TestCase("Group")]
        public void DdlFilterField2_SelectedIndexChanged_WhenListIsEmpty_ErrorIsDisplayed(string selectedFilter)
        {
            // Arrange
            InitializeFakes();
            var errorText = "Please make sure that you have";
            _page.SetField(GetField(nameof(ddlFilterField2)), SetDropDown(selectedFilter));
            ShimCampaign.GetByCustomerID_NoAccessCheckInt32Boolean = (x, y) => new List<Campaign>();
            ShimMessageType.GetByBaseChannelIDInt32User = (x, y) => new List<MessageType>();
            ShimGroup.GetByCustomerID_NoAccessCheckInt32String = (x, y) => new List<Group>();
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterField2SelectedIndexChanged, methodArgs, _page);

            // Assert
            var ddlFilterValue2 = ReflectionHelper.GetFieldValue(_page, "ddlFilterValue2") as DropDownList;
            var errorMessage = ReflectionHelper.GetFieldValue(_page, "lblErrorMessage") as Label;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue2.ShouldNotBeNull(),
                () => errorMessage.ShouldNotBeNull(),
                () => errorMessage.Text.ShouldNotBeNullOrWhiteSpace(),
                () => errorMessage.Text.ShouldContain(errorText));
        }

        [Test]
        public void DdlFilterValue1_SelectedIndexChanged_WhenFilterOneIsCampaign_InsertsThreeItems()
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField(nameof(ddlFilterField2)), new DropDownList());
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown("Campaign"));
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterValue1SelectedIndexChanged, methodArgs, _page);

            // Assert
            var ddlFilterValue2 = ReflectionHelper.GetFieldValue(_page, "ddlFilterField2") as DropDownList;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue2.ShouldNotBeNull(),
                () => ddlFilterValue2.SelectedIndex.ShouldBe(0),
                () => ddlFilterValue2.Enabled.ShouldBeTrue(),
                () => ddlFilterValue2.Visible.ShouldBeTrue(),
                () => ddlFilterValue2.Items.Count.ShouldBe(3));
        }

        [Test]
        public void DdlFilterValue1_SelectedIndexChanged_WhenFilterOneIsNotCampaign_InsertsFiveItems()
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField(nameof(ddlFilterField2)), new DropDownList());
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown("dummyItem"));
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodDDLFilterValue1SelectedIndexChanged, methodArgs, _page);

            // Assert
            var ddlFilterValue2 = ReflectionHelper.GetFieldValue(_page, "ddlFilterField2") as DropDownList;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue2.ShouldNotBeNull(),
                () => ddlFilterValue2.SelectedIndex.ShouldBe(0),
                () => ddlFilterValue2.Enabled.ShouldBeTrue(),
                () => ddlFilterValue2.Visible.ShouldBeTrue(),
                () => ddlFilterValue2.Items.Count.ShouldBe(5));
        }

        [Test]
        public void OnBubbleEvent_WhenFilterOneIsCampaign_InsertsThreeItems()
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField("hfWhichLayout"), new HiddenField { Value = TestOne });
            _page.SetField(GetField(nameof(ddlFilterField2)), new DropDownList());
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown("Campaign"));
            var methodArgs = new object[] { ReflectionHelper.CreateInstance(typeof(Layout)), EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodOnBubbleEvent, methodArgs, _page);

            // Assert
            var ddlFilterValue2 = ReflectionHelper.GetFieldValue(_page, "ddlFilterField2") as DropDownList;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue2.ShouldNotBeNull(),
                () => ddlFilterValue2.SelectedIndex.ShouldBe(0),
                () => ddlFilterValue2.Enabled.ShouldBeTrue(),
                () => ddlFilterValue2.Visible.ShouldBeTrue(),
                () => ddlFilterValue2.Items.Count.ShouldBe(3));
        }

        [Test]
        public void OnBubbleEvent_SelectedIndexChanged_WhenFilterOneIsNotCampaign_InsertsFiveItems()
        {
            // Arrange
            InitializeFakes();
            _page.SetField(GetField("hfWhichLayout"), new HiddenField { Value = TestTwo });
            _page.SetField(GetField(nameof(ddlFilterField2)), new DropDownList());
            _page.SetField(GetField(nameof(ddlFilterField1)), SetDropDown("dummyItem"));
            var methodArgs = new object[] { ReflectionHelper.CreateInstance(typeof(Layout)), EventArgs.Empty };

            // Act
            _page.GetType().CallMethod(MethodOnBubbleEvent, methodArgs, _page);

            // Assert
            var ddlFilterValue2 = ReflectionHelper.GetFieldValue(_page, "ddlFilterField2") as DropDownList;
            _page.ShouldSatisfyAllConditions(
                () => ddlFilterValue2.ShouldNotBeNull(),
                () => ddlFilterValue2.SelectedIndex.ShouldBe(0),
                () => ddlFilterValue2.Enabled.ShouldBeTrue(),
                () => ddlFilterValue2.Visible.ShouldBeTrue(),
                () => ddlFilterValue2.Items.Count.ShouldBe(5));
        }

        private void InitializeFakes()
        {
            SetupSessionFakes();
            ShimPerformanceByDayAndTimeReport.AllInstances.MasterGet = (x) => new ecn.communicator.MasterPages.Communicator();
            ShimAuthenticationTicket.getTicket = () => ReflectionHelper.CreateInstance(typeof(AuthenticationTicket));
            ShimECNSession.AllInstances.RefreshSession = (x) => { };
            ShimECNSession.AllInstances.ClearSession = (x) => { };
            ShimCampaign.GetByCustomerID_NoAccessCheckInt32Boolean = (x, y) => new List<Campaign> { ReflectionHelper.CreateInstance(typeof(Campaign)) };
            ShimMessageType.GetByBaseChannelIDInt32User = (x, y) => new List<MessageType> { ReflectionHelper.CreateInstance(typeof(MessageType)) };
            ShimGroup.GetByCustomerID_NoAccessCheckInt32String = (x, y) => new List<Group> { ReflectionHelper.CreateInstance(typeof(Group)) };
        }

        private DropDownList SetDropDown(string filterValue)
        {
            return new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = filterValue
                    }
                }
            };
        }
    }
}