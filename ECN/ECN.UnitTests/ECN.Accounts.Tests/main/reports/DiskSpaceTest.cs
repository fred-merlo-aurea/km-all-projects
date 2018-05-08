using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public class DiskSpaceTest: BasePageTests
    {
        #region Consts

        private const string ExpectedReportMonth = "12";
        private const string ExpectedChannel = "123";

        #endregion
        #region Private members

        private Mock<IDiskMonitorProxy> _diskMonitorProxyMock;
        private List<DiskMonitor> _diskMonitorProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private DiskSpace _page;
        private DropDownList _drpChannel;
        private DropDownList _drpMonth;
        private CheckBox _chkOverLimit;
        private DropDownList _drpExport;
        private byte[] _reportOutput;
        private IDisposable _shimsContext;
        private FakeHttpContext.FakeHttpContext _fakeHttpContext;

        #endregion
        #region Public methods

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();

            MockDiskMonitorProxy();
            MockReportContentGenerator();

            _page = new DiskSpace(_diskMonitorProxyMock.Object, _reportContentGeneratorMock.Object);

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
        public void BtnSubmitClick_ReportParametersControlsContainCorrectValues_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _diskMonitorProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(channel => channel == 123),
                It.Is<int>(month => month == 12),
                It.Is<bool>(showOverLimit => showOverLimit)
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportParametersControlsContainCorrectValues_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = string.Format(
                "{0}rpt_DiskMonitor.rdlc",
                _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportParametersControlsContainCorrectValues_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=DiskUsage.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
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

        private bool ValidateDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_DiskMonitor", dataSource.Name);
            Assert.AreEqual(_diskMonitorProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsTrue(parameters.Any(param => 
                param.Name == "Month" && 
                param.Values.Contains(ExpectedReportMonth)));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpChannel.Items.Add(ExpectedChannel);
            _drpMonth.Items.Add(ExpectedReportMonth);
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _chkOverLimit.Checked = true;
        }

        private void RetrievePageControls()
        {
            _drpChannel = ReflectionHelper.GetValue<DropDownList>(_page, "drpChannel");
            _drpMonth = ReflectionHelper.GetValue<DropDownList>(_page, "drpMonth");
            _chkOverLimit = ReflectionHelper.GetValue<CheckBox>(_page, "chkOverLimit");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
        }

        private void MockReportContentGenerator()
        {
            string responseContentType;
            _reportOutput = new byte[1];
            _reportContentGeneratorMock = new Mock<IReportContentGenerator>();
            _reportContentGeneratorMock.Setup(reportContentGenerator
                => reportContentGenerator.CreateReportContent(It.IsAny<string>(), It.IsAny<ReportDataSource>(), It.IsAny<ReportParameter[]>(),
                    It.IsAny<string>(), out responseContentType)).Returns(_reportOutput);
        }

        private void MockDiskMonitorProxy()
        {
            _diskMonitorProxyMock = new Mock<IDiskMonitorProxy>();
            _diskMonitorProxyMockGetResult = new List<DiskMonitor>();
            _diskMonitorProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<bool>()))
                .Returns(_diskMonitorProxyMockGetResult);
        }

        #endregion
    }
}
