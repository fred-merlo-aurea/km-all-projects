using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.main.blasts.Report;
using ecn.communicator.MasterPages.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;
using EcnReport = ecn.communicator.main.blasts.Report.Fakes;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    /// <summary>
    /// Unit test for <see cref="CampaignStatisticsReport"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignStatisticsReportTest : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string ButtonReportClick = "btnReport_Click";
        private const string DefaultText = "Unit Test";
        private const int DefaultId = 1;
        private const string TextBoxStartDate = "txtstartDate";
        private const string TextBoxEndDate = "txtendDate";
        private const string DropDownListCampaigns = "ddlCampaigns";
        private const string DropDownListExport = "drpExport";
        private const string HiddenFiledSelectGroupId = "hfSelectGroupID";
        private const string ReportPath = "ReportPath";
        private const string Pdf = "PDF";
        private const string Xls = "XLS";
        private const string Xlsdata = "xlsdata";
        private const string InvalidStartDate = "Invalid start date";
        private const string InvalidStopDate = "Invalid end date";
        private const string DateRangeErrorMessage = "Date range cannot be more than 1 year";
        private bool _addHeader;
        private bool _binaryWriteByteArray;
        private bool _responseClear;
        private bool _responseEnd;
        private CampaignStatisticsReport _campaignStatisticsReport;
        private IDisposable _shimsContext;

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            _campaignStatisticsReport = new CampaignStatisticsReport();
            InitializePage(_campaignStatisticsReport);
        }

        [TearDown]
        public void TearDown()
        {
            ShimsContext.Reset();
            _shimsContext.Dispose();
        }

        [TestCase(TestZero)]
        [TestCase(TestOne)]
        public void ButtonReportClick_ExportTypeIsXxsdata_RenderReportResult(string testCase)
        {
            // Arrange
            var exportToCsv = false;
            var parameters = new object[] { this, EventArgs.Empty };
            SetPagePrivateObject(testCase);
            var drpExport = Get<DropDownList>(DropDownListExport);
            drpExport.DataSource = new[] { Xlsdata, Pdf, Xls };
            drpExport.SelectedValue = Xlsdata;
            drpExport.DataBind();
            ShimReportViewerExport.ExportToCSVStringString = (newList, tfile) => exportToCsv = true;
            ShimEmojiFunctions.GetSubjectUTFString = (x) => DefaultText;
            ShimBaseValidator.AllInstances.Validate = (sender) => { };
            CreatePageShimObject();

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            exportToCsv.ShouldBeTrue();
        }

        [TestCase(TestZero, Pdf)]
        [TestCase(TestOne, Xls)]
        public void ButtonReportClick_ExportTypeIsPdf_RenderReportResult(string testCase, string format)
        {
            // Arrange
            var setParametrs = false;
            var refreshReport = false;
            var reportParameters = new List<ReportParameter>();
            var parameters = new object[] { this, EventArgs.Empty };
            SetPagePrivateObject(testCase);
            var drpExport = Get<DropDownList>(DropDownListExport);
            drpExport.DataSource = new[] { Xlsdata, Pdf, Xls };
            drpExport.SelectedValue = format;
            drpExport.DataBind();
            ShimEmojiFunctions.GetSubjectUTFString = (x) => DefaultText;
            ShimBaseValidator.AllInstances.Validate = (sender) => { };
            ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (sender, parameter) =>
            {
                reportParameters = parameter.ToList();
                setParametrs = true;
            };
            ShimLocalReport.AllInstances.Refresh = (sender) => refreshReport = true;
            ShimLocalReport.AllInstances.RenderStringStringPageCountModeStringOutStringOutStringOutStringArrayOutWarningArrayOut =
                (LocalReport sender,
                string fileformat,
                string deviceInfo,
                PageCountMode pageCountMode,
                out string mimeType,
                out string encoding,
                out string fileNameExtension,
                out string[] streams,
                out Warning[] warnings) =>
                {
                    mimeType = string.Empty;
                    encoding = string.Empty;
                    fileNameExtension = string.Empty;
                    streams = new string[3];
                    warnings = new Warning[1];
                    return new byte[10];
                };
            CreatePageShimObject();

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            reportParameters.ShouldSatisfyAllConditions(
                () => _addHeader.ShouldBeTrue(),
                () => _binaryWriteByteArray.ShouldBeTrue(),
                () => _responseClear.ShouldBeTrue(),
                () => _responseEnd.ShouldBeTrue(),
                () => setParametrs.ShouldBeTrue(),
                () => refreshReport.ShouldBeTrue(),
                () => reportParameters.ShouldNotBeNull(),
                () => reportParameters.Any().ShouldBeTrue(),
                () => reportParameters.Count.ShouldBe(3));
        }

        [Test]
        public void ButtonReportClick_StartDateIsInvalid_ThrowECNException()
        {
            // Arrange
            var message = string.Empty;
            var parameters = new object[] { this, EventArgs.Empty };
            ShimBaseValidator.AllInstances.Validate = (sender) => { };
            EcnReport.ShimCampaignStatisticsReport.AllInstances.throwECNExceptionString = (sender, errorMessage) => { message = errorMessage; };
            CreatePageShimObject();
            Get<TextBox>(TextBoxStartDate).Text = DefaultText;

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            message.ShouldBe(InvalidStartDate);

        }

        [Test]
        public void ButtonReportClick_EndDateIsInvalid_ThrowECNException()
        {
            // Arrange
            var message = string.Empty;
            var parameters = new object[] { this, EventArgs.Empty };
            ShimBaseValidator.AllInstances.Validate = (sender) => { };
            EcnReport.ShimCampaignStatisticsReport.AllInstances.throwECNExceptionString = (sender, errorMessage) => { message = errorMessage; };
            CreatePageShimObject();
            Get<TextBox>(TextBoxStartDate).Text = DateTime.Now.ToString();
            Get<TextBox>(TextBoxEndDate).Text = DefaultText;

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            message.ShouldBe(InvalidStopDate);

        }

        [Test]
        public void ButtonReportClick_TotalDaysDiffrenceGreaterThenOnYear_ThrowECNException()
        {
            // Arrange
            var message = string.Empty;
            var parameters = new object[] { this, EventArgs.Empty };
            ShimBaseValidator.AllInstances.Validate = (sender) => { };
            EcnReport.ShimCampaignStatisticsReport.AllInstances.throwECNExceptionString = (sender, errorMessage) => { message = errorMessage; };
            CreatePageShimObject();
            Get<TextBox>(TextBoxStartDate).Text = DateTime.Now.ToString();
            Get<TextBox>(TextBoxEndDate).Text = DateTime.Now.AddYears(2).ToString();

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            message.ShouldBe(DateRangeErrorMessage);

        }

        private void SetPagePrivateObject(string testCase)
        {
            Get<TextBox>(TextBoxStartDate).Text = DateTime.Now.ToString();
            Get<TextBox>(TextBoxEndDate).Text = DateTime.Now.ToString();
            Get<HiddenField>(HiddenFiledSelectGroupId).Value = testCase;
            var ddlCampaigns = Get<DropDownList>(DropDownListCampaigns);
            ddlCampaigns.DataSource = Enumerable.Range(0, 10).ToArray();
            ddlCampaigns.SelectedValue = DefaultId.ToString(); ;
            ddlCampaigns.DataBind();
        }

        private void CreatePageShimObject()
        {
            var shimSession = CreateShimEcnSessionObject();
            shimSession.Instance.CurrentUser = CreateUserObject(shimSession);
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            EcnReport.ShimCampaignStatisticsReport.AllInstances.MasterGet = (x) =>
                new ShimCommunicator
                {
                    UserSessionGet = () => shimSession.Instance,
                };
            var appSettings = new NameValueCollection();
            appSettings.Add(ReportPath, string.Empty);
            ShimConfigurationManager.AppSettingsGet = () => appSettings;
            ShimPage.AllInstances.ResponseGet = (sender) => new ShimHttpResponse
            {
                Clear = () => _responseClear = true,
                BinaryWriteByteArray = (bytes) => _binaryWriteByteArray = true,
                End = () => _responseEnd = true,
                AddHeaderStringString = (x, y) => _addHeader = true,
            };
            ShimPage.AllInstances.ServerGet = (sender) => new ShimHttpServerUtility
            {
                MapPathString = (x) => string.Empty,
            };

            ShimCampaignStatisticsReport.GetInt32DateTimeDateTimeNullableOfInt32 = (x, y, z, m) =>
                new List<FrameworkEntities.CampaignStatisticsReport>
                {
                    new FrameworkEntities.CampaignStatisticsReport
                    {
                        BlastID = DefaultId,
                        BounceTotal = DefaultId,
                        CampaignItemName = DefaultText,
                        FilterName = DefaultText,
                        GroupName = DefaultText,
                        MessageName = DefaultText,
                        Delivered = DefaultId,
                        EmailSubject=DefaultText
                    }
                };
        }

        private static ShimECNSession CreateShimEcnSessionObject()
        {
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession
            {
                ClearSession = () => { },
                ClientIDGet = () => DefaultId,
                UserIDGet = () => DefaultId,
                BaseChannelIDGet = () => DefaultId,
                ClientGroupIDGet = () => DefaultId,
                CustomerIDGet = () => DefaultId,
            };
            return shimSession;
        }

        private static User CreateUserObject(ShimECNSession shimSession)
        {
            return new User
            {
                UserID = DefaultId,
                CustomerID = DefaultId,
                UserName = DefaultText,
                IsActive = true,
                CurrentSecurityGroup = new SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true,
                CurrentClient = new Client
                {
                    ClientTestDBConnectionString = string.Empty,
                    ClientLiveDBConnectionString = string.Empty
                },
            };
        }
    }
}
