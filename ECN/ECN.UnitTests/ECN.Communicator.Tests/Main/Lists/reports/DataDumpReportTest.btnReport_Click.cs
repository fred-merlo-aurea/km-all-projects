using System;
using System.Collections.Generic;
using System.Fakes;
using System.Globalization;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists.reports.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using NUnit.Framework;
using Shouldly;
using CommMaster = ecn.communicator.MasterPages;
using ReportEntities = ECN_Framework_Entities.Activity.Report;
using ReportFakes = ECN_Framework_BusinessLayer.Activity.Report.Fakes;

namespace ECN.Communicator.Tests.Main.Lists.reports
{
    public partial class DataDumpReportTest
    {
        private const int BRC_CustomerID = 10;
        private const int BRC_UserId = 200;
        private TextBox _txtstartDate;
        private TextBox _txtendDate;
        private DropDownList _drpExport;
        private ReportViewer _reportViewer1;

        [Test]
        public void btnReport_Click_NoGroupList_Error()
        {
            // Arrange
            InitTest_btnReport_Click(null);
            var expectedError = $"<br/>{Enums.Entity.Page}: Please select at least one group.";

            // Act
            _dataDumpReportPrivateObject.Invoke("btnReport_Click", new object[] { null, null });

            // Assert
            _phError.ShouldSatisfyAllConditions(
             () => _phError.Visible.ShouldBeTrue(),
             () => _lblErrorMessage.Text.ShouldBe(expectedError));
        }

        [Test]
        public void btnReport_Click_InvalidDates_Error()
        {
            // Arrange
            var grpList = CreateGroupList_btnReport_Click();
            InitTest_btnReport_Click(grpList);
            _txtendDate.Text = DateTime.Now.ToString("dd/MM/yyyy invalid");
            _txtstartDate.Text = DateTime.Now.ToString("dd/MM/yyyy invalid");
            var expectedError = $"<br/>{Enums.Entity.Page}: Invalid dates";

            // Act
            _dataDumpReportPrivateObject.Invoke("btnReport_Click", new object[] { null, null });

            // Assert
            _phError.ShouldSatisfyAllConditions(
             () => _phError.Visible.ShouldBeTrue(),
             () => _lblErrorMessage.Text.ShouldBe(expectedError));
        }

        [Test]
        public void btnReport_Click_DaysGreaterThan365_Error()
        {
            // Arrange
            var grpList = CreateGroupList_btnReport_Click();
            InitTest_btnReport_Click(grpList);
            _txtstartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            _txtendDate.Text = DateTime.Now.AddDays(366).ToString("dd/MM/yyyy");
            var expectedError = $"<br/>{Enums.Entity.Page}: Date range cannot be more than 1 year.";

            // Act
            _dataDumpReportPrivateObject.Invoke("btnReport_Click", new object[] { null, null });

            // Assert
            _phError.ShouldSatisfyAllConditions(
             () => _phError.Visible.ShouldBeTrue(),
             () => _lblErrorMessage.Text.ShouldBe(expectedError));
        }

        [TestCase("Field")]
        [TestCase(null)]
        public void btnReport_Click_XlsFormat_NoError(string blastFieldIdString)
        {
            // Arrange
            var grpList = CreateGroupList_btnReport_Click();
            var dumpReports = CreateDataDumpReport_btnReport_Click();
            InitTest_btnReport_Click(grpList, dataDumpReports: dumpReports, selectedFormat: "xls", blastFieldIdString: blastFieldIdString);
            _txtstartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            _txtendDate.Text = DateTime.Now.AddDays(300).ToString("dd/MM/yyyy");

            // Act
            _dataDumpReportPrivateObject.Invoke("btnReport_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeFalse();
            dumpReports.ForEach(ddr =>
            {
                ddr.ShouldSatisfyAllConditions(
                       () => ddr.Delivery.ShouldBe(ddr.usend - ddr.tbounce),
                       () => ddr.SuccessPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.Delivery) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.OpensPercentOfDelivered.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.topen) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uOpensPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.uopen) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.tClickPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.tClick) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uClickPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.uClick) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.tClicksOpensPerc.ShouldBe(ddr.topen > 0 ? decimal.Round(Convert.ToDecimal(ddr.tClick) / Convert.ToDecimal(ddr.topen), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uClicksOpensPerc.ShouldBe(ddr.uopen > 0 ? decimal.Round(Convert.ToDecimal(ddr.uClick) / Convert.ToDecimal(ddr.uopen), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.SuppressedPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.Suppressed) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uAbuseRpt_UnsubPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uAbuseRpt_Unsub) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uFeedBack_UnsubPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uFeedBack_Unsub) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uHardBouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uHardBounce) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uMastSup_UnsubPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uMastSup_Unsub) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uOtherBouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uOtherBounce) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uSoftBouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uSoftBounce) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.uSubscribePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uSubscribe) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.treferPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.trefer) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.tresendPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.tresend) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.tbouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.tbounce) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.sendPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.usend) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                       () => ddr.ClickThroughPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.ClickThrough) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0));
            });
        }

        [Test]
        public void btnReport_Click_XlsdataFormat_NoError()
        {
            // Arrange
            var grpList = CreateGroupList_btnReport_Click();
            var dumpReports = CreateDataDumpReport_btnReport_Click();
            InitTest_btnReport_Click(grpList, dataDumpReports: dumpReports, selectedFormat: "xlsdata");
            _txtstartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            _txtendDate.Text = DateTime.Now.AddDays(300).ToString("dd/MM/yyyy");

            // Act
            _dataDumpReportPrivateObject.Invoke("btnReport_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeFalse();
            dumpReports.ForEach(ddr =>
            {
                ddr.ShouldSatisfyAllConditions(
                            () => ddr.Delivery.ShouldBe(ddr.usend - ddr.tbounce),
                            () => ddr.OpensPercentOfDelivered.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.topen * 100) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.SuccessPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.Delivery * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uOpensPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.uopen * 100) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.tClickPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.tClick * 100) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uClickPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.uClick * 100) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.tClicksOpensPerc.ShouldBe(ddr.topen > 0 ? decimal.Round(Convert.ToDecimal(ddr.tClick * 100) / Convert.ToDecimal(ddr.topen), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uClicksOpensPerc.ShouldBe(ddr.uopen > 0 ? decimal.Round(Convert.ToDecimal(ddr.uClick * 100) / Convert.ToDecimal(ddr.uopen), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.SuppressedPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.Suppressed * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uAbuseRpt_UnsubPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uAbuseRpt_Unsub * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uFeedBack_UnsubPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uFeedBack_Unsub * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uHardBouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uHardBounce * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uMastSup_UnsubPerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uMastSup_Unsub * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uOtherBouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uOtherBounce * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uSoftBouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uSoftBounce * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.uSubscribePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.uSubscribe * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.treferPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.trefer * 100) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.tresendPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.tresend * 100) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.tbouncePerc.ShouldBe(ddr.usend > 0 ? decimal.Round(Convert.ToDecimal(ddr.tbounce * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.sendPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.usend * 100) / Convert.ToDecimal(ddr.usend), 2, MidpointRounding.AwayFromZero) : 0),
                            () => ddr.ClickThroughPerc.ShouldBe(ddr.Delivery > 0 ? decimal.Round(Convert.ToDecimal(ddr.ClickThrough * 100) / Convert.ToDecimal(ddr.Delivery), 2, MidpointRounding.AwayFromZero) : 0));
            });
        }

        private void InitTest_btnReport_Click(List<Group> groups, List<ReportEntities.DataDumpReport> dataDumpReports = null, string selectedFormat = null, string blastFieldIdString = null)
        {
            var shimHttpContext = new ShimHttpContext();
            SetPageProperties_btnReport_Click();
            SetPageControls_btnReport_Click();
            ShimDateTime.TryParseStringDateTimeOut = DateTimeTryParse_btnReport_Click;
            ShimDataDumpReport.AllInstances.groupListGet = (ddr) => groups;
            if (!string.IsNullOrEmpty(selectedFormat))
            {
                _drpExport.SelectedValue = selectedFormat;
            }
            ShimBlastFieldsName.GetByBlastFieldIDInt32User = (id, user) => blastFieldIdString != null ? new BlastFieldsName() { Name = $"{blastFieldIdString}{id}" } : null;
            ShimHttpServerUtility.AllInstances.MapPathString = (su, s) => Assembly.GetExecutingAssembly().Location;
            ReportFakes.ShimDataDumpReport.GetListInt32DateTimeDateTimeString = (cid, endData, startDate, gId) => dataDumpReports;
            ShimHttpContext.AllInstances.ServerGet = (context) => new ShimHttpServerUtility();
            ShimHttpContext.CurrentGet = () => shimHttpContext;
            ShimHttpContext.AllInstances.ResponseGet = (c) => new ShimHttpResponse();
            ShimReport.AllInstances.LoadReportDefinitionStream = (r, s) => { };
            ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (r, parameters) => { };
            ShimLocalReport.AllInstances.Refresh = (r) => { };
            ShimReport.AllInstances.RenderStringStringStringOutStringOutStringOutStringArrayOutWarningArrayOut = ReportRender_btnReport_Click;
        }

        private void SetPageControls_btnReport_Click()
        {
            _txtstartDate = new TextBox();
            _dataDumpReportPrivateObject.SetField("txtstartDate", BindingFlags.Instance | BindingFlags.NonPublic, _txtstartDate);
            _txtendDate = new TextBox();
            _dataDumpReportPrivateObject.SetField("txtendDate", BindingFlags.Instance | BindingFlags.NonPublic, _txtendDate);
            _drpExport = new DropDownList();
            _drpExport.Items.Add(new ListItem("xls", "xls"));
            _drpExport.Items.Add(new ListItem("xlsdata", "xlsdata"));
            _dataDumpReportPrivateObject.SetField("drpExport", BindingFlags.Instance | BindingFlags.NonPublic, _drpExport);
            _reportViewer1 = new ReportViewer();
            _dataDumpReportPrivateObject.SetField("ReportViewer1", BindingFlags.Instance | BindingFlags.NonPublic, _reportViewer1);
        }

        private void SetPageProperties_btnReport_Click()
        {
            var shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new User() { UserID = BRC_UserId };
            shimECNSession.Instance.CurrentCustomer = new Customer() { CustomerID = BRC_CustomerID };
            ShimCommunicator.AllInstances.UserSessionGet = (c) => shimECNSession;
            ShimDataDumpReport.AllInstances.MasterGet = (mt) => new CommMaster.Communicator();
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (h, u, b) => { };
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            ShimPage.AllInstances.RequestGet = (p) => new ShimHttpRequest();
        }

        private List<Group> CreateGroupList_btnReport_Click()
        {
            var grpList = new List<Group>();
            var group = new Group() { GroupID = 10 };
            grpList.Add(group);
            return grpList;
        }

        private bool DateTimeTryParse_btnReport_Click(string dateTimeString, out DateTime dateTime)
        {
            if (dateTimeString.Contains("invalid"))
            {
                dateTime = DateTime.MinValue;
                return false;
            }
            return DateTime.TryParseExact(dateTimeString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
        }

        private List<ReportEntities.DataDumpReport> CreateDataDumpReport_btnReport_Click()
        {
            var dataDumpReports = new List<ReportEntities.DataDumpReport>();
            var report1 = new ReportEntities.DataDumpReport();
            report1.EmailSubject = "EmailSubject1";
            report1.usend = 100; 
            report1.tbounce = 20;
            report1.Delivery = 30;
            report1.topen = 30;
            report1.uopen = 20;
            report1.tClick = 10;
            report1.Suppressed = 5;
            report1.uAbuseRpt_Unsub = 10;
            report1.uFeedBack_Unsub = 20;
            report1.uHardBounce = 30;
            report1.uMastSup_Unsub = 40;
            report1.uOtherBounce = 50;
            report1.uSoftBounce = 60;
            report1.uSubscribe = 70;
            report1.trefer = 80;
            report1.tresend = 90;
            report1.tbounce = 90;
            report1.ClickThrough = 100;
            var report2 = new ReportEntities.DataDumpReport();
            report2.EmailSubject = "EmailSubject2";
            report2.usend = -100;
            report2.tbounce = 20;
            report2.Delivery = -30;
            report2.topen = -30;
            report2.uopen = -20;
            report2.tClick = 10;
            report2.Suppressed = 5;
            report2.uAbuseRpt_Unsub = 10;
            report2.uFeedBack_Unsub = 20;
            report2.uHardBounce = 30;
            report2.uMastSup_Unsub = 40;
            report2.uOtherBounce = 50;
            report2.uSoftBounce = 60;
            report2.uSubscribe = 70;
            report2.trefer = 80;
            report2.tresend = 90;
            report2.tbounce = 90;
            report2.ClickThrough = 100;
            dataDumpReports.Add(report1);
            dataDumpReports.Add(report2);
            return dataDumpReports;
        }

        private byte[] ReportRender_btnReport_Click(Report report, string format, string deviceInfo, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings)
        {
            warnings = null;
            streams = null;
            mimeType = null;
            encoding = null;
            fileNameExtension = null;
            return null;
        }
    }
}
