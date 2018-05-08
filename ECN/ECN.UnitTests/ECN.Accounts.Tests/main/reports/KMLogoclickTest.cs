using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using ecn.accounts.main.reports;
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
    public class KMLogoclickTest : BasePageTests
    {
        #region Private members

        private Mock<IKMLogoClickReportProxy> _logoClickReportProxyMock;
        private List<KMLogoClickReport> _logoClickReportProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private KMLogoclick _page;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private DropDownList _drpExport;
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

            MockBillingProxy();
            MockReportContentGenerator();

            _page = new KMLogoclick(_logoClickReportProxyMock.Object, _reportContentGeneratorMock.Object);

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
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _logoClickReportProxyMock.Verify(proxy => proxy.Get(
                It.Is<DateTime>(dateFrom => dateFrom == DateTime.Today.AddDays(-1)),
                It.Is<DateTime>(dateTo => dateTo == DateTime.Today)
            ));
        }

        [Test]
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_KMLogoClick.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateReportDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=KMLogoClickReport.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnReportClick_ExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            var responseContentType = "application/vnd.ms-excel";

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

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
        public void BtnReportClick_ExportTypeRequestedExcel_ReportContentGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypeXLS;
            var responseContentType = "application/vnd.ms-excel";

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

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

        private bool ValidateReportDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_KMLogoClickReport", dataSource.Name);
            Assert.AreEqual(_logoClickReportProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsTrue(parameters.Any(
                param => param.Name == "StartDate" &&
                param.Values.Contains(DateTime.Today.AddDays(-1).ToShortDateString())));

            Assert.IsTrue(parameters.Any(
                param => param.Name == "EndDate" && 
                param.Values.Contains(DateTime.Today.ToShortDateString())));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _txtStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            _txtEndDate.Text = DateTime.Today.ToShortDateString();
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
        }

        private void RetrievePageControls()
        {
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
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

        private void MockBillingProxy()
        {
            _logoClickReportProxyMock = new Mock<IKMLogoClickReportProxy>();
            _logoClickReportProxyMockGetResult = new List<KMLogoClickReport>();
            _logoClickReportProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(_logoClickReportProxyMockGetResult);
        }

        #endregion
    }
}

