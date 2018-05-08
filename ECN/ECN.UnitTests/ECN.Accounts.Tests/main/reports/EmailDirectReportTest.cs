using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Moq;
using NUnit.Framework;
using ECN.Tests.Helpers;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Accounts.Report;

namespace ECN.Accounts.Tests.main.reports
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EmailDirectReportTest: BasePageTests
    {
        private const string ExpectedChannel = "123";
        
        private Mock<IEmailDirectReportProxy> _emailDirectReportProxyMock;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private DataTable _emailDirectReportGetResult;
        private ecn.accounts.main.reports.EmailDirectReport _page;
        private DropDownList _drpChannel;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private ListBox _lstCustomers;
        private byte[] _reportOutput;
        private IDisposable _shimsContext;
        private FakeHttpContext.FakeHttpContext _fakeHttpContext;

        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            _shimsContext = ShimsContext.Create();
            ShimHttpApplication.AllInstances.CompleteRequest = application => { };
            ShimHttpContext.AllInstances.ApplicationInstanceGet = context => new ShimHttpApplication();
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();
            
            MockEmailDirectReportProxy();
            MockReportContentGenerator();

            _page = new ecn.accounts.main.reports.EmailDirectReport(
                _emailDirectReportProxyMock.Object,
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
        public void BtnSubmitClick_ReportParametersValid_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=EmailDirectReport.xls", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ReportParametersValid_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _emailDirectReportProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(channel => channel == 123),
                It.Is<string>(customerIds => customerIds == "SampleCustomer1"),
                It.Is<DateTime>(dateFrom => dateFrom == DateTime.Today.AddDays(-1)),
                It.Is<DateTime>(dateTo => dateTo == DateTime.Today))
            );
        }

        [Test]
        public void BtnSubmitClick_ReportParametersValid_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = string.Format(
                "{0}rpt_EmailDirectReport.rdlc",
                _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateReportDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypeXLS),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportParametersValid_ReportContentGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls();
            string responseContentType;

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

        private bool ValidateReportDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_EmailDirect", dataSource.Name);
            Assert.AreEqual(_emailDirectReportGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Length);
            Assert.IsTrue(parameters.Any(param => 
                param.Name == "StartDate" &&
                param.Values[0] == DateTime.Today.AddDays(-1).ToShortDateString()));
            Assert.IsTrue(parameters.Any(param => 
                param.Name == "EndDate" &&
                param.Values[0] == DateTime.Today.ToShortDateString()));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpChannel.Items.Add(ExpectedChannel);
            _txtStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            _txtEndDate.Text = DateTime.Today.ToShortDateString();
            _lstCustomers.Items.Add("SampleCustomer1");
            _lstCustomers.Items.Add("SampleCustomer2");
            _lstCustomers.SelectedValue = "SampleCustomer1";
        }

        private void RetrievePageControls()
        {
            _drpChannel = ReflectionHelper.GetValue<DropDownList>(_page, "drpChannel");
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _lstCustomers = ReflectionHelper.GetValue<ListBox>(_page, "lstCustomer");
        }

        private void MockEmailDirectReportProxy()
        {
            _emailDirectReportProxyMock = new Mock<IEmailDirectReportProxy>();
            _emailDirectReportGetResult = new DataTable();
            _emailDirectReportProxyMock
                .Setup(proxy => proxy.Get(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .Returns(_emailDirectReportGetResult);
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
    }
}
