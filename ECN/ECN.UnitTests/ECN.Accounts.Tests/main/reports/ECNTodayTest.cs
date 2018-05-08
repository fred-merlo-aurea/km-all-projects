using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using ECN.Tests.Helpers;
using ECN_Framework.Accounts.Report;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Moq;
using NUnit.Framework;

namespace ECN.Accounts.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ECNTodayTest : BasePageTests
    {
        #region Consts

        private const string ReportTypeEcnToday = "1";
        private const string ReportTypeNewCustomer = "2";
        private const string ReportTypeNewUser = "3";
        
        #endregion
        #region Private members

        private Mock<INewCustomerProxy> _newCustomerProxyMock;
        private Mock<INewUserProxy> _newUserProxyMock;
        private Mock<IECNTodayProxy> _ecnTodayProxyMock;
        private List<NewCustomer> _newCustomerProxyMockGetResult;
        private List<NewUser> _newUserProxyMockGetResult;
        private List<ECNToday> _ecnTodayProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private ecn.accounts.main.reports.ECNToday _page;
        private DropDownList _drpMonth;
        private DropDownList _drpYear;
        private DropDownList _drpExport;
        private RadioButtonList _rbReportType;
        private CheckBox _chkTestBlastOnly;
        private byte[] _reportOutput;
        private IDisposable _shimsContext;
        private FakeHttpContext.FakeHttpContext _fakeHttpContext;

        #endregion
        #region Public methods

        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            _shimsContext = ShimsContext.Create();
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();

            MockNewCustomerProxy();
            MockNewUserProxy();
            MockECNTodayProxy();
            MockReportContentGenerator();

            _page = new ecn.accounts.main.reports.ECNToday(
                _newCustomerProxyMock.Object,
                _newUserProxyMock.Object,
                _ecnTodayProxyMock.Object,
                _reportContentGeneratorMock.Object);

            InitializePage(_page);
            RetrievePageControls();
        }

        [TearDown]
        public void TearDown()
        {
            ShimsContext.Reset();
            _shimsContext.Dispose();
            _fakeHttpContext.Dispose();
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNewCustomer_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeNewCustomer;

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _newCustomerProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(month => month == 12),
                It.Is<int>(year => year == 2000),
                It.Is<bool>(isTestBlastOnly => isTestBlastOnly)
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNewCustomer_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeNewCustomer;

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_NewCustomer.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateReportDataSource(dataSource, ReportTypeNewCustomer)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNewCustomer_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeNewCustomer;

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=NewCustomer.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNewUser_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeNewUser;

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _newUserProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(month => month == 12),
                It.Is<int>(year => year == 2000),
                It.Is<bool>(isTestBlastOnly => isTestBlastOnly)
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNewUser_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeNewUser;

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_NewUser.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateReportDataSource(dataSource, ReportTypeNewUser)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNewUser_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeNewUser;

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=NewUser.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ReportTypeEcnToday_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeEcnToday;

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _ecnTodayProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(month => month == 12),
                It.Is<int>(year => year == 2000),
                It.Is<bool>(isTestBlastOnly => isTestBlastOnly)
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeEcnToday_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeEcnToday;

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_ECNToday.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateReportDataSource(dataSource, ReportTypeEcnToday)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeEcnToday_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();
            _rbReportType.SelectedValue = ReportTypeEcnToday;

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=ECNToday.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            var responseContentType = "application/pdf";

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.IsAny<string>(),
                It.IsAny<ReportDataSource>(),
                It.IsAny<ReportParameter[]>(),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ExportTypeRequestedExcel_ReportContentGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypeXLS;
            var responseContentType = "application/vnd.ms-excel";

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.IsAny<string>(),
                It.IsAny<ReportDataSource>(),
                It.IsAny<ReportParameter[]>(),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypeXLS),
                out responseContentType
            ));
        }

        #endregion
        #region Private methods

        private bool ValidateReportDataSource(ReportDataSource dataSource, string reportType)
        {
            Assert.IsNotNull(dataSource);

            if (reportType == ReportTypeNewCustomer)
            {
                Assert.AreEqual("DS_NewCustomer", dataSource.Name);
                Assert.AreEqual(_newCustomerProxyMockGetResult, dataSource.Value);
            }

            if (reportType == ReportTypeNewUser)
            {
                Assert.AreEqual("DS_NewUser", dataSource.Name);
                Assert.AreEqual(_newUserProxyMockGetResult, dataSource.Value);
            }

            if (reportType == ReportTypeEcnToday)
            {
                Assert.AreEqual("DS_ECNToday", dataSource.Name);
                Assert.AreEqual(_ecnTodayProxyMockGetResult, dataSource.Value);
            }

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsTrue(parameters.Any(param => param.Name == "Month" && param.Values.Contains("12")));
            Assert.IsTrue(parameters.Any(param => param.Name == "Year" && param.Values.Contains("2000")));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpMonth.Items.Add("12");
            _drpYear.Items.Add("2000");
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _rbReportType.Items.Add(ReportTypeNewCustomer);
            _rbReportType.Items.Add(ReportTypeNewUser);
            _rbReportType.Items.Add(ReportTypeEcnToday);
            _rbReportType.SelectedValue = ReportTypeNewCustomer;
            _chkTestBlastOnly.Checked = true;
        }

        private void RetrievePageControls()
        {
            _rbReportType = ReflectionHelper.GetValue<RadioButtonList>(_page, "rbReportType");
            _drpMonth = ReflectionHelper.GetValue<DropDownList>(_page, "drpMonth");
            _drpYear = ReflectionHelper.GetValue<DropDownList>(_page, "drpYear");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _chkTestBlastOnly = ReflectionHelper.GetValue<CheckBox>(_page, "chkTestBlastOnly");
        }

        private void MockReportContentGenerator()
        {
            string responseContentType;
            _reportOutput = new byte[1];
            _reportContentGeneratorMock = new Mock<IReportContentGenerator>();
            _reportContentGeneratorMock.Setup(reportContentGenerator
                => reportContentGenerator.CreateReportContent(
                    It.IsAny<string>(),
                    It.IsAny<ReportDataSource>(),
                    It.IsAny<ReportParameter[]>(),
                    It.IsAny<string>(),
                    out responseContentType)).Returns(_reportOutput);
        }

        private void MockNewCustomerProxy()
        {
            _newCustomerProxyMock = new Mock<INewCustomerProxy>();
            _newCustomerProxyMockGetResult = new List<NewCustomer>();
            _newCustomerProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<bool>()))
                .Returns(_newCustomerProxyMockGetResult);
        }

        private void MockNewUserProxy()
        {
            _newUserProxyMock = new Mock<INewUserProxy>();
            _newUserProxyMockGetResult = new List<NewUser>();
            _newUserProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<bool>()))
                .Returns(_newUserProxyMockGetResult);
        }

        private void MockECNTodayProxy()
        {
            _ecnTodayProxyMock = new Mock<IECNTodayProxy>();
            _ecnTodayProxyMockGetResult = new List<ECNToday>();
            _ecnTodayProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<bool>()))
                .Returns(_ecnTodayProxyMockGetResult);
        }

        #endregion
    }
}

