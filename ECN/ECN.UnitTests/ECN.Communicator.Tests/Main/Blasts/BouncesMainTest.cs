using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit;
using ecn.communicator.blastsmanager;
using ecn.communicator.blastsmanager.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN.Common.Fakes;
using ECN.Communicator.Tests.Helpers;
using KM.Platform.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_Entities.Communicator;
using static KMPlatform.Enums;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MasterPage = ecn.communicator.MasterPages.Communicator;
using Shouldly;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit Tests for <see cref="bounces_main"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BouncesMainTest : BaseBlastsTest<bounces_main>
    {
        private const string MethodPageLoad = "Page_Load";
        private const string MethodExportReport = "ExportReport"; 
        private const string MethodDownloadBouncedEmails = "downloadBouncedEmails";
        private const string MethodUnsubscribeBounces = "UnsubscribeBounces"; 
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string One = "1";
        private const string Zero = "0";
        private NameValueCollection _queryString;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            testObject = new bounces_main();
            _privateTestObject = new PrivateObject(testObject);
            InitializeAllControls(testObject);
            _queryString = new NameValueCollection();
            _queryString.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [TestCase(One, "")]
        [TestCase(Zero, "")]
        [TestCase(One, DummyString)]
        [TestCase(Zero, DummyString)]
        public void UnsubscribeBounces_Success_BouncesAreUnsubscribed(string blastId, string ispType)
        {
            // Arrange
            CreateShims();
            _queryString.Add("BlastID", blastId);
            _queryString.Add("isp", ispType); 
            var bouncesUnsubscribed = false;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => true;
            ShimEmailGroup.UnsubscribeBouncesInt32StringUser = (x, y, z) => { };
            ShimBlast.GetByCampaignItemID_NoAccessCheckInt32Boolean = (a, b) => new List<BlastAbstract> { ReflectionHelper.CreateInstance(typeof(BlastRegular)) };
            ShimHttpResponse.AllInstances.RedirectString = (x, y) =>
            {
                bouncesUnsubscribed = true;
            };

            // Act
            _privateTestObject.Invoke(MethodUnsubscribeBounces, null, EventArgs.Empty);

            // Assert
            bouncesUnsubscribed.ShouldBeTrue();
        }

        [TestCase(One, ".xls")]
        [TestCase(One, ".csv")]
        [TestCase(Zero, ".pdf")]
        public void DownloadBouncedEmails_Success_BouncedEmailsAreDownloaded(string blastId, string downloadType)
        {
            // Arrange
            CreateShims();
            _queryString.Add("BlastID", blastId);
            var bouncedEmailsDownloaded = false;
            ShimReportViewerExport.ExportToExcelFromDataTblOf1ListOfM0StringStringString<DataRow>((a, b, c, d) =>new List<DataRow>());
            ShimBlastActivity.DownloadBlastReportDetailsInt32BooleanStringStringStringUserStringStringStringBoolean = (a, b, c, d, e, f, g, h, i, j) =>
            {
                bouncedEmailsDownloaded = true;
                return new DataTable();
            };

            // Act
            _privateTestObject.Invoke(MethodDownloadBouncedEmails, DummyString, downloadType);

            // Assert
            bouncedEmailsDownloaded.ShouldBeTrue();
        }

        [TestCase(One, "PDF")]
        [TestCase(Zero, "CSV")]
        public void ExportReport_Success_ReportsAreExported(string blastId, string reportType)
        {
            // Arrange
            CreateShims();
            _queryString.Add("BlastID", blastId);
            var reportExported = false;
            ShimHttpResponse.AllInstances.End = (x) => 
            {
                reportExported = true;
            };
            var httpResponse = new HttpResponse(new StringWriter());

            // Act
            _privateTestObject.Invoke(MethodExportReport, httpResponse, reportType);

            // Assert
            reportExported.ShouldBeTrue();
        }

        [TestCase(One, true, true)]
        [TestCase(Zero, false, false)]
        public void Page_Load_WithViewAccess_TabPanelsToogleVisibility(string blastId, bool viewAccess, bool tabPanelsVisibility)
        {
            // Arrange
            CreateShims();
            _queryString.Add("BlastID", blastId);
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, accessType) =>
            {
                switch (accessType)
                {
                    case Access.Download:
                        return false;
                    case Access.ViewDetails:
                        return viewAccess;
                    default:
                        return true;
                }
            };
            
            // Act
            _privateTestObject.Invoke(MethodPageLoad, null, EventArgs.Empty);

            // Assert
            var secondTabPanel = ReflectionHelper.GetField(testObject, "TabPanel2") as TabPanel;
            var thirdTabPanel = ReflectionHelper.GetField(testObject, "TabPanel3") as TabPanel;
            var fourthTabPanel = ReflectionHelper.GetField(testObject, "TabPanel4") as TabPanel;
            testObject.ShouldSatisfyAllConditions(
                () => secondTabPanel.ShouldNotBeNull(),
                () => thirdTabPanel.ShouldNotBeNull(),
                () => fourthTabPanel.ShouldNotBeNull(),
                () => secondTabPanel.Visible.ShouldBe(tabPanelsVisibility),
                () => thirdTabPanel.Visible.ShouldBe(tabPanelsVisibility),
                () => fourthTabPanel.Visible.ShouldBe(tabPanelsVisibility));
        }

        [Test]
        public void GetUdfName_NoException_ReturnsQueryStringValue()
        {
            // Arrange
            QueryString.Add(UDFName, UDFNameValue);

            // Act, Assert
            testObject.getUDFName().ShouldBe(UDFNameValue);
        }

        [Test]
        public void GetUdfName_ExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            QueryString = null;

            // Act, Assert
            testObject.getUDFName().ShouldBeEmpty();
        }

        [Test]
        public void GetUdfData_NoException_ReturnsQueryStringValue()
        {
            // Arrange
            QueryString.Add(UDFdata, UDFdataValue);

            // Act, Assert
            testObject.getUDFData().ShouldBe(UDFdataValue);
        }

        [Test]
        public void GetUdfData_ExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            QueryString = null;

            // Act, Assert
            testObject.getUDFData().ShouldBeEmpty();
        }

        private void CreateShims()
        {
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            var ecnSession = ReflectionHelper.CreateInstance(typeof(ECNSession));
            var dummyUser = ReflectionHelper.CreateInstance(typeof(KMPlatform.Entity.User));
            var dummyCustormer = ReflectionHelper.CreateInstance(typeof(ECN_Framework_Entities.Accounts.Customer));
            ecnSession.CurrentUser = dummyUser;
            ecnSession.CurrentCustomer = dummyCustormer;
            ShimECNSession.CurrentSession = () => ecnSession;
            ecn.communicator.MasterPages.Fakes.ShimCommunicator.AllInstances.UserSessionGet = (x) => ecnSession;
            var masterPage = new MasterPage();
            Shimbounces_main.AllInstances.MasterGet = (x) => masterPage;
            ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
            ShimHttpRequest.AllInstances.QueryStringGet = (x) => _queryString;
            var blastReportDataTable = new DataTable();
            blastReportDataTable.Columns.Add("ACTIONVALUE");
            blastReportDataTable.Columns.Add("TOTALBounces");
            blastReportDataTable.Columns.Add(DummyString);
            blastReportDataTable.Rows.Add(One, One, DummyString);
            ShimBlastActivityBounces.BlastReportWithUDFInt32StringString = (x, y, z) => blastReportDataTable;
            ShimBlastActivityBounces.BlastReportInt32 = (x) => blastReportDataTable;
            ShimBlastActivityBounces.BlastReportByCampaignItemIDInt32 = (x) => blastReportDataTable;
            var blastReportDataSet = new DataSet();
            var dummyDataTable = new DataTable();
            dummyDataTable.Columns.Add("RecordCount");
            dummyDataTable.Rows.Add(One);
            blastReportDataSet.Tables.Add(dummyDataTable);
            blastReportDataSet.Tables.Add(blastReportDataTable);
            ShimBlastActivity.GetBlastReportDetailsInt32BooleanStringStringStringInt32Int32StringStringUserBoolean = (a, b, c, d, e, f, g, h, i, j, k) => blastReportDataSet;
            ShimBounceByDomain.GetInt32Int32 = (x, y) => new System.Collections.Generic.List<BounceByDomain> { ReflectionHelper.CreateInstance(typeof(BounceByDomain)) };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, accessType) => true;
            ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (x, y) => { };
            ShimCompositeControl.AllInstances.DataBind = (x) => { };
            var _reportData = new byte[1];
            ShimLocalReport
                    .AllInstances
                    .RenderStringStringPageCountModeStringOutStringOutStringOutStringArrayOutWarningArrayOut = (
                    LocalReport format,
                    string renderers,
                    string info,
                    PageCountMode mode,
                    out string type,
                    out string encoding,
                    out string extension,
                    out string[] streams,
                    out Warning[] warnings) =>
                    {
                        type = string.Empty;
                        encoding = string.Empty;
                        extension = string.Empty;
                        streams = new string[0];
                        warnings = new Warning[0];
                        return _reportData;
                    };
            ShimHttpResponse.AllInstances.OutputStreamGet = (x) => new MemoryStream();
            ShimHttpResponse.AllInstances.End = (x) => { };
            ConfigurationManager.AppSettings["globalBounceCodes"] = DummyString;
            ReflectionHelper.SetField(testObject, "dropdownView", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = One
                    }
                }
            });
            var sharesChart = ReflectionHelper.GetField(testObject, "chartBounceTypes") as Chart;
            sharesChart.Series.Add("Series1");
            sharesChart.ChartAreas.Add("ChartArea1");
        }
    }
}
