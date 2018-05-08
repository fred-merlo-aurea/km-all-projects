using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.blasts.Report;
using ecn.communicator.MasterPages.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using NUnit.Framework;
using Shouldly;
using EcnReport = ecn.communicator.main.blasts.Report.Fakes;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    /// <summary>
    /// Unit test for <see cref="EmailFatigueReport"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EmailFatigueReportTest : BasePageTests
    {
        private const string TestOne = "1";
        private const string ButtonReportClick = "btnReport_Click";
        private const string DefaultText = "Unit Test";
        private const int DefaultId = 1;
        private const string TextBoxStartDate = "txtstartDate";
        private const string TextBoxEndDate = "txtendDate";
        private const string DropDownListExport = "ddlExport";
        private const string DropDownListFilterField = "ddlFilterField";
        private const string DropDownListFilterValue = "ddlFilterValue";
        private const string DropDownListOutputType = "ddlOutputType";
        private const string ReportPath = "ReportPath";
        private const string Counts = "Counts";
        private const string Count = "Count";
        private const string Pdf = "PDF";
        private const string Xls = "xls";
        private const string other = "EXCEL";
        private const string InvalidErrorMessage = "Date range must be less than or equal to 31 days to run this report between 8AM and 6PM CST on weekdays.";
        private const string InvalidDateErrorMessage = "Please select a valid date";
        private bool _addHeader;
        private bool _binaryWriteByteArray;
        private bool _responseClear;
        private bool _responseEnd;
        private bool _loadReport;
        private EmailFatigueReport _emailFatigueReport;
        private IDisposable _shimsContext;

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            _emailFatigueReport = new EmailFatigueReport();
            InitializePage(_emailFatigueReport);
        }

        [TearDown]
        public void TearDown()
        {
            ShimsContext.Reset();
            _shimsContext.Dispose();
        }

        [TestCase(TestOne, Xls, Counts)]
        [TestCase(TestOne, Xls, Count)]
        [TestCase(TestOne, other, Counts)]
        [TestCase(TestOne, other, Count)]
        public void ButtonReportClick_ExportTypeIsXlsdata_RenderReportResult(string testCase, string renderFormat, string outputType)
        {
            // Arrange
            EcnReport.ShimEmailFatigueReport.AllInstances.ValidToRun = (sender) => true;
            var reportParameters = new List<ReportParameter>();
            var parameters = new object[] { this, EventArgs.Empty };
            SetPagePrivateObject(testCase);
            var drpExport = Get<DropDownList>(DropDownListExport);
            drpExport.DataSource = new[] { other, Pdf, Xls };
            drpExport.SelectedValue = renderFormat;
            drpExport.DataBind();
            var ddlOutputType = Get<DropDownList>(DropDownListOutputType);
            ddlOutputType.DataSource = new[] { Counts, Count };
            ddlOutputType.SelectedValue = outputType;
            ddlOutputType.DataBind();
            CreatePageShimObject();
            ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (sender, parameter) =>
            {
                reportParameters = parameter.ToList();
            };

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            _addHeader.ShouldBeTrue();
            _binaryWriteByteArray.ShouldBeTrue();
            _responseClear.ShouldBeTrue();
            _responseEnd.ShouldBeTrue();
            _loadReport.ShouldBeTrue();
            if (renderFormat == other)
            {
                reportParameters.ShouldSatisfyAllConditions(
                    () => reportParameters.ShouldNotBeNull(),
                    () => reportParameters.Any().ShouldBeTrue(),
                    () => reportParameters.Count.ShouldBe(6));
            }
        }

        [Test]
        public void ButtonReportClick_ValidToRunIsFalse_ThrowECNException()
        {
            // Arrange
            var errorMessage = string.Empty;
            EcnReport.ShimEmailFatigueReport.AllInstances.ValidToRun = (sender) => false;
            var parameters = new object[] { this, EventArgs.Empty };
            EcnReport.ShimEmailFatigueReport.AllInstances.throwECNExceptionString = (sender, message) => errorMessage = message;

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            errorMessage.ShouldBe(InvalidErrorMessage);
        }

        [Test]
        public void ButtonReportClick_StartAndStopHaveInvalidDate_ThrowECNException()
        {
            // Arrange
            Get<TextBox>(TextBoxStartDate).Text = DateTime.MinValue.ToString();
            Get<TextBox>(TextBoxEndDate).Text = DateTime.MinValue.ToString();
            var errorMessage = string.Empty;
            EcnReport.ShimEmailFatigueReport.AllInstances.ValidToRun = (sender) => true;
            var parameters = new object[] { this, EventArgs.Empty };
            EcnReport.ShimEmailFatigueReport.AllInstances.throwECNExceptionString = (sender, message) => errorMessage = message;

            // Act
            _privateObject.Invoke(ButtonReportClick, parameters);

            // Assert
            errorMessage.ShouldBe(InvalidDateErrorMessage);
        }

        private void SetPagePrivateObject(string testCase)
        {
            Get<TextBox>(TextBoxStartDate).Text = DateTime.Now.ToString();
            Get<TextBox>(TextBoxEndDate).Text = DateTime.Now.ToString();
            var dataSource = Enumerable.Range(0, 10);
            var ddlFilterField = Get<DropDownList>(DropDownListFilterField);
            ddlFilterField.DataSource = dataSource;
            ddlFilterField.SelectedValue = testCase;
            ddlFilterField.DataBind();
            var ddlFilterValue = Get<DropDownList>(DropDownListFilterValue);
            ddlFilterValue.DataSource = dataSource;
            ddlFilterValue.SelectedValue = testCase;
            ddlFilterValue.DataBind();
        }

        private void CreatePageShimObject()
        {
            var shimSession = CreateShimEcnSessionObject();
            shimSession.Instance.CurrentUser = CreateUserObject();
            shimSession.Instance.CurrentCustomer = new Customer
            {
                CustomerID = DefaultId,
                BaseChannelID = DefaultId,
                CreatedUserID = DefaultId,
            };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            EcnReport.ShimEmailFatigueReport.AllInstances.MasterGet = (x) =>
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

            ShimEmailFatigueReport.GetInt32DateTimeDateTimeStringInt32 = (x, y, z, m, n) =>
                new List<FrameworkEntities.EmailFatigueReport>
                {
                    new FrameworkEntities.EmailFatigueReport
                    {
                         Action=DefaultText,
                          Grouping=DefaultId
                    }
                };
            ShimLocalReport.AllInstances.RenderStringStringPageCountModeStringOutStringOutStringOutStringArrayOutWarningArrayOut =
                (LocalReport sender,
                string fileformat,
                string deviceInfo,
                PageCountMode pageCountMode,
                out string mimeType,
                out string encoding,
                out string fileNameExtension,
                out string[] streams,
                out Warning[] warnings
                ) =>
                {
                    _loadReport = true;
                    mimeType = string.Empty;
                    encoding = string.Empty;
                    fileNameExtension = string.Empty;
                    streams = new string[3];
                    warnings = new Warning[1];
                    return new byte[10];
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

        private static User CreateUserObject()
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
