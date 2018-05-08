using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Fakes;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ActiveUp.WebControls;
using ecn.communicator.blastsmanager;
using ecn.communicator.blastsmanager.Fakes;
using ECN.Common.Fakes;
using Ecn.Communicator.Main.Interfaces;
using ecn.communicator.main.blasts.Interfaces;
using ecn.communicator.MasterPages.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_Entities.Activity;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Moq;
using Microsoft.Reporting.WebForms;
using Microsoft.QualityTools.Testing.Fakes;
using KMPlatform.Entity;
using Panel = System.Web.UI.WebControls.Panel;
using MasterPage = ecn.communicator.MasterPages.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class OpensTest
    {
        private const int One = 1;
        private const int Zero = 0;
        private const int TextWrapThreshold = 0;
        private const int XAxisInterval = 1;
        private const int ReportDataSourceCount = 2;
        private const int LegendsCount = 1;
        private const string ActiveGridlaceHolderName = "phActiveGrid";
        private const string OpensGridPlaceHolderName = "phOpensGrid";
        private const string AllOpensPlaceHolderName = "phAllOpens";
        private const string BrowserStatsPlaceHolderName = "phBrowserStats";
        private const string OpensbyTimeButtonName = "btnOpensbyTime";
        private const string ActiveOpensButtonName = "btnActiveOpens";
        private const string AllOpensButtonName = "btnAllOpens";
        private const string BrowserStatsButtonName = "btnBrowserStats";
        private const string NonEmptyCssClass = "Not Empty";
        private const string SelectedCssClass = "selected";
        private const string ChartAreaName = "ChartArea1";
        private const string SeriesOneName = "Series1";
        private const string OpensCountSeriesName = "OpensCount";
        private const string ChartBrowserStats = "chartBrowserStats";
        private const string ChartOpensbyTime = "chtOpensbyTime";
        private const string ReportViewerOne = "ReportViewer1";
        private const string PDFType = "PDF";
        private const string HourXAxis = "Hour";
        private const string OpensYAxix = "Opens";
        private const string OpensCountYAxis = "Opens Count";
        private const string SeriesToolTip = "#VALY{G}";
        private const string EmailClientName = "EmailClientName";
        private const string Usage = "Usage";
        private const string SeriesLabel = "#VALX";
        private const string PieLabelStyle = "PieLabelStyle";
        private const string Disabled = "Disabled";
        private const string BlastOpensReportName = "main\\blasts\\Report\\rpt_BlastOpens.rdlc";
        private const string BlastOpensByTimeReportName = "main\\blasts\\Report\\rpt_BlastOpensByTime.rdlc";
        private const string PDFMimeType = "application/pdf";
        private const string MethodPageLoad = "Page_Load";
        private const string MethodDownloadOpenEmails = "downloadOpenEmails";
        private const string MethodLoadGrid = "loadGrid";
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string DummyString2 = "dummyString2";
        private const string DummyActiveOpens = "activeopens";
        private const string DummyEntry1 = "DummyEntry1";
        private const string DummyEntry2 = "DummyEntry2";
        private const string DummyEntry3 = "DummyEntry3";
        private const string DummyEntry4 = "DummyEntry4";
        private const string DummyEntry5 = "DummyEntry5";
        private const string DummyEntry6 = "DummyEntry6";
        private const string DummyLayout = "layout";
        private const string DummyCsv = ".csv";
        private const string DummyXls = ".xls";
        private const string SampleHost = "km.com";
        private const string SampleHttpHost = "http://km.com";
        private const string SampleHostPath = "http://km.com/addTemplate";
        private const string SampleUserAgent = "http://km.com/addTemplate";
        private const string CampaignItemTemplateID = "1";
        private const string KMCommon_Application = "1";
        private IDisposable _context;
        private NameValueCollection _queryString;
        private HttpSessionState _sessionState;
        private DataGrid _dataGrid;

        [SetUp]
        protected void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            _queryString = new NameValueCollection();
            _queryString.Clear();
        }

        [TearDown]
        public void TearDwond()
        {
            if (_dataGrid != null)
            {
                _dataGrid.Dispose();
            }
            _context.Dispose();
        }

        [Test]
        public void downloadOpenEmails_Success_OpenEmailAreDownloaded()
        {
            // Arrange 
            CreateShims();
            InitializeSession();
            Mock<HttpResponseBase> response;
            Mock<IReportViewer> reportViewer;
            var open = CreateOpens(out response, out reportViewer);
            Initialize(open);
            var openEmailsDownloaded = false;
            ShimHttpResponse.AllInstances.End = (x) =>
            {
                openEmailsDownloaded = true;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            typeof(opens).CallMethod(MethodDownloadOpenEmails, methodArgs, open);

            // Assert
            openEmailsDownloaded.ShouldBeTrue();
        }

        [Test]
        public void loadGrid_WhenReportTypeIsActiveOptions_PhActiveGridIsVisible()
        {
            // Arrange 
            CreateShims();
            InitializeSession();
            Mock<HttpResponseBase> response;
            Mock<IReportViewer> reportViewer;
            var open = CreateOpens(out response, out reportViewer);
            Initialize(open);
            ReflectionHelper.SetField(open, "ActiveGrid", new DataGrid());
            var methodArgs = new object[] { DummyActiveOpens };

            // Act
            typeof(opens).CallMethod(MethodLoadGrid, methodArgs, open);

            // Assert
            var phActiveGrid = ReflectionHelper.GetField(open, "phActiveGrid") as PlaceHolder;
            open.ShouldSatisfyAllConditions(
               () => phActiveGrid.ShouldNotBeNull(),
               () => phActiveGrid.Visible.ShouldBeTrue());
        }

        [Test]
        public void loadGrid_WhenReportTypeIsNotActiveOptions_PhActiveGridIsNotVisible()
        {
            // Arrange 
            CreateShims();
            InitializeSession();
            Mock<HttpResponseBase> response;
            Mock<IReportViewer> reportViewer;
            var open = CreateOpens(out response, out reportViewer);
            Initialize(open);
            var methodArgs = new object[] { DummyString };

            // Act
            typeof(opens).CallMethod(MethodLoadGrid, methodArgs, open);

            // Assert
            var phActiveGrid = ReflectionHelper.GetField(open, "phActiveGrid") as PlaceHolder;
            open.ShouldSatisfyAllConditions(
              () => phActiveGrid.ShouldNotBeNull(),
              () => phActiveGrid.Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_WhenUserHasAccess_BtnActiveOpensShouldBeVisible()
        {
            // Arrange 
            CreateShims();
            InitializeSession();
            Mock<HttpResponseBase> response;
            Mock<IReportViewer> reportViewer;
            var open = CreateOpens(out response, out reportViewer);
            Initialize(open);
            Shimopens.AllInstances.loadGridString = (x, y) => { };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            typeof(opens).CallMethod(MethodPageLoad, methodArgs, open);

            // Assert
            var btnActiveOpens = ReflectionHelper.GetField(open, "btnActiveOpens") as LinkButton;
            open.ShouldSatisfyAllConditions(
             () => btnActiveOpens.ShouldNotBeNull(),
             () => btnActiveOpens.Visible.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_WhenUserHasNotAccess_BtnActiveOpensShouldNotBeVisible()
        {
            // Arrange 
            CreateShims();
            InitializeSession();
            Mock<HttpResponseBase> response;
            Mock<IReportViewer> reportViewer;
            var open = CreateOpens(out response, out reportViewer);
            Initialize(open);
            Shimopens.AllInstances.getBlastID = (x) => Zero;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => false;
            Shimopens.AllInstances.getCampaignItemID = (x) => One;
            var campaignItem = new CampaignItem();
            campaignItem.CampaignItemType = DummyLayout;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItem;
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            typeof(opens).CallMethod(MethodPageLoad, methodArgs, open);

            // Assert
            var btnActiveOpens = ReflectionHelper.GetField(open, "btnActiveOpens") as LinkButton;
            open.ShouldSatisfyAllConditions(
                () => btnActiveOpens.ShouldNotBeNull(),
                () => btnActiveOpens.Visible.ShouldBeFalse());
        }

        [Test]
        public void BrowserStatsExportReport_PDFType_UpdatesControlProperties()
        {
            // Arrange
            Mock<HttpResponseBase> response;
            Mock<IReportViewer> reportViewer;
            var open = CreateOpens(out response, out reportViewer);
            var placeHolders = new string[]
            {
                ActiveGridlaceHolderName,
                OpensGridPlaceHolderName,
                AllOpensPlaceHolderName
            };
            InitializeControls(open, placeHolders, new PlaceHolder { Visible = true });
            var browserStatsPlaceHolder = new PlaceHolder { Visible = false };
            InitializeControls(open, new string[] { BrowserStatsPlaceHolderName }, browserStatsPlaceHolder);
            var linkButtons = new string[]
            {
                OpensbyTimeButtonName,
                ActiveOpensButtonName,
                AllOpensButtonName,
            };
            InitializeControls(open, linkButtons, new LinkButton { CssClass = NonEmptyCssClass });
            var browserStatsButton = new LinkButton { CssClass = string.Empty };
            InitializeControls(open, new string[] { BrowserStatsButtonName }, browserStatsButton);
            var chart = new Chart();
            chart.Series.Add(new Series(SeriesOneName));
            chart.ChartAreas.Add(ChartAreaName);
            InitializeControls(open, new string[] { ChartBrowserStats }, chart);
            InitializeControls(open, new string[] { ReportViewerOne }, new ReportViewer());

            // Act
            typeof(opens)
                .CallMethod(
                "BrowserStats_ExportReport",
                new object[]
                {
                    response.Object,
                    PDFType
                },
                open);

            // Assert
            AssertThatPlaceHoldersAreNotVisible(open, placeHolders);
            Assert.That(browserStatsPlaceHolder.Visible, Is.True);
            AssertThatLinkButtonsCssClassIsEmpty(open, linkButtons);
            Assert.That(browserStatsButton.CssClass, Is.EqualTo(SelectedCssClass));
            Assert.That(chart.DataSource, Is.Not.Null);
            Assert.That(chart.DataSource, Is.InstanceOf(typeof(IEnumerable)));
            Assert.That(chart.Series[SeriesOneName].ChartType, Is.EqualTo(SeriesChartType.Pie));
            Assert.That(chart.Series[SeriesOneName].XValueMember, Is.EqualTo(EmailClientName));
            Assert.That(chart.Series[SeriesOneName].YValueMembers, Is.EqualTo(Usage));
            Assert.That(chart.Series[0].Label, Is.EqualTo(SeriesLabel));
            Assert.That(chart.Series[0][PieLabelStyle], Is.EqualTo(Disabled));
            AssertLegendProperties(chart, Docking.Right);
            Assert.That(chart.ChartAreas[0].Area3DStyle.Enable3D, Is.True);
            Assert.That(chart.AntiAliasing, Is.EqualTo(AntiAliasingStyles.Graphics));
            AssertReportData(reportViewer, open, BlastOpensReportName);
        }

        [Test]
        public void OpenbyTimeExportReport_PDFType_UpdatesControlProperties()
        {
            // Arrange
            Mock<HttpResponseBase> response;
            Mock<IReportViewer> reportViewer;
            var open = CreateOpens(out response, out reportViewer);
            var placeHolders = new string[]
            {
                ActiveGridlaceHolderName,
                AllOpensPlaceHolderName,
                BrowserStatsPlaceHolderName
            };
            InitializeControls(open, placeHolders, new PlaceHolder { Visible = true });
            var opensGridPlaceHolder = new PlaceHolder { Visible = false };
            InitializeControls(open, new string[] { OpensGridPlaceHolderName }, opensGridPlaceHolder);
            var linkButtons = new string[]
            {
                BrowserStatsButtonName,
                ActiveOpensButtonName,
                AllOpensButtonName,
            };
            InitializeControls(open, linkButtons, new LinkButton { CssClass = NonEmptyCssClass });
            var opensByTimeButton = new LinkButton { CssClass = string.Empty };
            InitializeControls(open, new string[] { OpensbyTimeButtonName }, opensByTimeButton);
            var chart = new Chart();
            var seriesName = OpensCountSeriesName;
            InitializeControls(open, new string[] { ChartOpensbyTime }, chart);
            InitializeControls(open, new string[] { ReportViewerOne }, new ReportViewer());

            // Act
            typeof(opens)
                .CallMethod(
                "OpenbyTime_ExportReport",
                new object[]
                {
                    response.Object,
                    PDFType
                },
                open);

            // Assert
            AssertThatPlaceHoldersAreNotVisible(open, placeHolders);
            Assert.That(opensGridPlaceHolder.Visible, Is.True);
            AssertThatLinkButtonsCssClassIsEmpty(open, linkButtons);
            Assert.That(opensByTimeButton.CssClass, Is.EqualTo(SelectedCssClass));
            Assert.That(chart.DataSource, Is.Not.Null);
            Assert.That(chart.DataSource, Is.InstanceOf(typeof(IEnumerable)));
            Assert.That(chart.Series[seriesName].ChartType, Is.EqualTo(SeriesChartType.Column));
            Assert.That(chart.Series[seriesName].XValueMember, Is.EqualTo(HourXAxis));
            Assert.That(chart.Series[seriesName].YValueMembers, Is.EqualTo(OpensYAxix));
            Assert.That(chart.Series[seriesName].IsVisibleInLegend, Is.True);
            Assert.That(chart.Series[seriesName].ShadowOffset, Is.EqualTo(3));
            Assert.That(chart.Series[seriesName].ToolTip, Is.EqualTo(SeriesToolTip));
            Assert.That(chart.Series[seriesName].BorderWidth, Is.EqualTo(3));
            Assert.That(chart.Series[seriesName].Color, Is.EqualTo(Color.SteelBlue));
            Assert.That(chart.Series[seriesName].IsValueShownAsLabel, Is.True);
            var chartAreaName = "ChartArea1";
            Assert.That(chart.ChartAreas[chartAreaName].AxisX.Title, Is.EqualTo(HourXAxis));
            Assert.That(chart.ChartAreas[chartAreaName].AxisY.Title, Is.EqualTo(OpensCountYAxis));
            Assert.That(chart.ChartAreas[chartAreaName].AxisX.MajorGrid.Enabled, Is.True);
            Assert.That(chart.ChartAreas[chartAreaName].AxisY.MajorGrid.Enabled, Is.True);
            Assert.That(chart.ChartAreas[chartAreaName].AxisX.Interval, Is.EqualTo(XAxisInterval));
            Assert.That(chart.ChartAreas[chartAreaName].AxisX.MajorGrid.LineColor, Is.EqualTo(Color.LightGray));
            Assert.That(chart.ChartAreas[chartAreaName].AxisY.MajorGrid.LineColor, Is.EqualTo(Color.LightGray));
            Assert.That(chart.ChartAreas[chartAreaName].BackColor, Is.EqualTo(Color.Transparent));
            Assert.That(chart.ChartAreas[chartAreaName].ShadowColor, Is.EqualTo(Color.Transparent));
            Assert.That(chart.Height.Value, Is.EqualTo(450));
            Assert.That(chart.Width.Value, Is.EqualTo(800));
            var titleName = "Title1";
            Assert.That(chart.Titles, Is.Not.Null);
            Assert.That(chart.Titles.Count, Is.EqualTo(1));
            Assert.That(chart.Titles[titleName], Is.Not.Null);
            Assert.That(chart.Titles[titleName].Text, Is.Empty);
            var legendName = "Legends1";
            Assert.That(chart.Legends[legendName], Is.Not.Null);
            AssertLegendProperties(chart, Docking.Bottom);
            AssertReportData(reportViewer, open, BlastOpensByTimeReportName);
        }

        private opens CreateOpens(out Mock<HttpResponseBase> response, out Mock<IReportViewer> reportViewer)
        {
            var emailClients = new Mock<IEmailClients>();
            emailClients.Setup(x => x.Get()).Returns(new List<EmailClients>());
            var platforms = new Mock<IPlatforms>();
            platforms.Setup(x => x.Get()).Returns(new List<Platforms>());
            var master = new Mock<IMasterCommunicator>();
            master.Setup(x => x.GetCustomerID()).Returns(0);
            var server = new Mock<IServer>();
            server.Setup(x => x.MapPath(It.IsAny<string>())).Returns(string.Empty);
            var blastReport = new Mock<IBlastReport>();
            blastReport.Setup(x => x.Get(It.IsAny<int>())).Returns(new List<BlastReport>());
            response = new Mock<HttpResponseBase>();
            response.SetupGet(x => x.OutputStream).Returns(new MemoryStream());
            reportViewer = new Mock<IReportViewer>();
            reportViewer.Setup(x => x.DataBind());
            reportViewer.Setup(x => x.SetParameters(It.IsAny<ReportParameter[]>()));
            var outString = string.Empty;
            var outStringArray = new string[] { };
            var outWarningArray = new Warning[] { };
            reportViewer.Setup(x => x.Render(
                It.IsAny<string>(),
                It.IsAny<string>(),
                out outString,
                out outString,
                out outString,
                out outStringArray,
                out outWarningArray)).Returns(new byte[] { });
            return new opens(emailClients.Object, platforms.Object, master.Object, server.Object, blastReport.Object, reportViewer.Object);
        }

        private void AssertReportData(Mock<IReportViewer> reportViewer, opens open, string reportPath)
        {
            var report = ReflectionHelper.GetField(open, "ReportViewer1") as ReportViewer;
            Assert.That(report, Is.Not.Null);
            Assert.That(report.LocalReport.ReportPath, Is.EqualTo(reportPath));
            Assert.That(report.LocalReport.DataSources, Is.Not.Null);
            Assert.That(report.LocalReport.DataSources, Is.InstanceOf(typeof(ReportDataSourceCollection)));
            Assert.That(report.LocalReport.DataSources.Count, Is.EqualTo(ReportDataSourceCount));
            var mimeType = PDFMimeType;
            string encoding = null;
            string extention = null;
            string[] streamids = null;
            Warning[] warnings = null;
            reportViewer.Verify(x => x.Render(It.IsAny<string>(), null, out mimeType, out encoding, out extention, out streamids, out warnings), Times.Once());
        }

        private void AssertLegendProperties(Chart chart, Docking docking)
        {
            Assert.That(chart.Legends, Is.Not.Null);
            Assert.That(chart.Legends.Count, Is.EqualTo(LegendsCount));
            Assert.That(chart.Legends[0].Enabled, Is.True);
            Assert.That(chart.Legends[0].Docking, Is.EqualTo(docking));
            Assert.That(chart.Legends[0].Alignment, Is.EqualTo(StringAlignment.Center));
            Assert.That(chart.Legends[0].IsEquallySpacedItems, Is.True);
            Assert.That(chart.Legends[0].TextWrapThreshold, Is.EqualTo(TextWrapThreshold));
            Assert.That(chart.Legends[0].IsTextAutoFit, Is.True);
            Assert.That(chart.Legends[0].BackColor, Is.EqualTo(Color.Transparent));
            Assert.That(chart.Legends[0].ShadowColor, Is.EqualTo(Color.Transparent));
        }

        private void AssertThatLinkButtonsCssClassIsEmpty(opens open, string[] linkButtons)
        {
            foreach (var name in linkButtons)
            {
                var button = ReflectionHelper.GetField(open, name) as LinkButton;
                Assert.That(button, Is.Not.Null);
                Assert.That(button.CssClass, Is.Empty);
            }
        }

        private void AssertThatPlaceHoldersAreNotVisible(opens open, string[] placeHolders)
        {
            foreach (var name in placeHolders)
            {
                var placeHolder = ReflectionHelper.GetField(open, name) as PlaceHolder;
                Assert.That(placeHolder, Is.Not.Null);
                Assert.That(placeHolder.Visible, Is.False);
            }
        }

        private void InitializeControls(opens open, string[] controlNames, Control control)
        {
            foreach (var name in controlNames)
            {
                ReflectionHelper.SetField(open, name, control);
            }
        }

        private void CreateShims()
        {
            ShimHttpServerUtility.AllInstances.MapPathString = (s, path) => string.Empty;
            ShimPage.AllInstances.ServerGet = (p) => new ShimHttpServerUtility();
            Shimopens.AllInstances.MasterGet = (x) => new MasterPage();
            ShimHttpServerUtility.AllInstances.MapPathString = (x, y) => DummyString;
            ShimDirectory.ExistsString = (x) => false;
            ShimDirectory.CreateDirectoryString = (x) => new ShimDirectoryInfo();
            ShimFile.ExistsString = (x) => true;
            ShimFile.ExistsString = (x) => true;
            ShimFile.DeleteString = (x) => { };
            ShimFile.AppendTextString = (x) => new ShimStreamWriter();
            ShimBlastActivity.DownloadBlastReportDetailsInt32BooleanStringStringStringUserStringStringStringBoolean = (a, b, c, d, e, f, g, h, i, j) => new DataTable();
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (h, u, b) => { };
            ShimPage.AllInstances.ResponseGet = (x) => new ShimHttpResponse();
            ShimHttpResponse.AllInstances.WriteFileString = (x, y) => { };
            ShimHttpResponse.AllInstances.End = (x) => { };
            var table1 = new DataTable(DummyEntry1);
            table1.Columns.Add(DummyString);
            table1.Columns.Add(DummyString2);
            table1.Rows.Add(1);
            table1.Rows.Add(2);
            var table2 = new DataTable(DummyEntry2);
            table2.Columns.Add(DummyEntry3);
            table2.Columns.Add(DummyEntry4);
            table2.Rows.Add(1, DummyEntry5);
            table2.Rows.Add(2, DummyEntry6);
            var dataSet = new DataSet();
            dataSet.Tables.Add(table1);
            dataSet.Tables.Add(table2);
            ShimBlastActivity.GetBlastReportDetailsInt32BooleanStringStringStringInt32Int32StringStringUserBoolean = (a, b, c, d, e, f, g, h, i, j, k) => dataSet;
            ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
            Shimopens.AllInstances.getBlastID = (x) => One;
            var blast = ReflectionHelper.CreateInstance(typeof(BlastRegular));
            blast.BlastType = DummyLayout;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => blast;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            Shimopens.AllInstances.loadOpensGrid = (x) => { };
            Shimopens.AllInstances.openbyTime_CreateChart = (x) => { };
            Shimopens.AllInstances.ShowHideDownload = (x) => { };
        }

        private void Initialize(object open)
        {
            ReflectionHelper.SetField(open, "DownloadType", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = DummyCsv
                    }
                }
            });
            ReflectionHelper.SetField(open, "OpensTypeRBList", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = DummyString
                    }
                }
            });
            _dataGrid = new DataGrid { PageSize = 1 };
            ReflectionHelper.SetField(open, "OpensGrid", _dataGrid);
            ReflectionHelper.SetField(open, "OpensPager", new PagerBuilder { RecordCount = 1 });
            ReflectionHelper.SetField(open, "phActiveGrid", new PlaceHolder());
            ReflectionHelper.SetField(open, "phOpensGrid", new PlaceHolder());
            ReflectionHelper.SetField(open, "phBrowserStats", new PlaceHolder());
            ReflectionHelper.SetField(open, "phAllOpens", new PlaceHolder());
            ReflectionHelper.SetField(open, "btnActiveOpens", new LinkButton());
            ReflectionHelper.SetField(open, "btnOpensbyTime", new LinkButton());
            ReflectionHelper.SetField(open, "btnBrowserStats", new LinkButton());
            ReflectionHelper.SetField(open, "btnAllOpens", new LinkButton());
            ReflectionHelper.SetField(open, "DownloadPanel", new Panel());
        }
        private void InitializeSession()
        {
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            var customerID = 1;
            var userID = 1;
            var config = new NameValueCollection();
            var reqParams = new NameValueCollection();
            var queryString = new NameValueCollection();
            var dummyCustormer = ReflectionHelper.CreateInstance(typeof(Customer));
            var dummyUser = ReflectionHelper.CreateInstance(typeof(User));
            var authTkt = ReflectionHelper.CreateInstance(typeof(ECN_Framework_Entities.Application.AuthenticationTicket));
            var ecnSession = ReflectionHelper.CreateInstance(typeof(ECNSession));
            config.Add("KMCommon_Application", KMCommon_Application);
            queryString.Add("HTTP_HOST", SampleHttpHost);
            queryString.Add("CampaignItemTemplateID", CampaignItemTemplateID);
            dummyCustormer.CustomerID = customerID;
            dummyUser.UserID = userID;
            ReflectionHelper.SetField(authTkt, "CustomerID", customerID);
            ReflectionHelper.SetField(ecnSession, "CurrentUser", dummyUser);
            ReflectionHelper.SetField(ecnSession, "CurrentCustomer", dummyCustormer);
            HttpContext.Current = MockHelpers.FakeHttpContext();
            ShimECNSession.CurrentSession = () => ecnSession;
            ShimAuthenticationTicket.getTicket = () => authTkt;
            ShimUserControl.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimUserControl.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
            ShimConfigurationManager.AppSettingsGet = () => config;
            ShimHttpRequest.AllInstances.UserAgentGet = (h) => SampleUserAgent;
            ShimHttpRequest.AllInstances.QueryStringGet = (h) => queryString;
            ShimHttpRequest.AllInstances.UserHostAddressGet = (h) => SampleHost;
            ShimHttpRequest.AllInstances.UrlReferrerGet = (h) => new Uri(SampleHostPath);
            ShimPage.AllInstances.SessionGet = x => HttpContext.Current.Session;
            ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimHttpRequest.AllInstances.ParamsGet = (x) => reqParams;
            ShimControl.AllInstances.ParentGet = (control) => new Page();
            ShimGridView.AllInstances.DataBind = (x) => { };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            _sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimPage.AllInstances.SessionGet = (p) =>
            {
                return _sessionState;
            };
            ShimUserControl.AllInstances.SessionGet = (c) =>
            {
                return _sessionState;
            };
        }
    }
}