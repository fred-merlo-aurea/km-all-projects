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
    public class DigitalEditionTest: BasePageTests
    {
        #region Consts

        private const string ExpectedReportMonth = "12";
        private const string ExpectedReportYear = "2000";

        #endregion
        #region Private members

        private Mock<IDigitalEditionBillingProxy> _billingProxyMock;
        private List<DigitalEditionBilling> _billingProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private DigitalEdition _page;
        private DropDownList _drpMonth;
        private DropDownList _drpYear;
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

            MockBillingProxy();
            MockReportContentGenerator();

            _page = new DigitalEdition(_billingProxyMock.Object, _reportContentGeneratorMock.Object);

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
            _billingProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(month => month == 12),
                It.Is<int>(year => year == 2000)
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportParametersControlsContainCorrectValues_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_DigitalEdition.rdlc", _page.Server.MapPath("~/main/reports/"));

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
            Assert.AreEqual("attachment; filename=DigitalEdition.PDF", _responseHeaders["content-disposition"]);
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
            Assert.AreEqual("DS_DigitalEditionBilling", dataSource.Name);
            Assert.AreEqual(_billingProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsTrue(parameters.Any(param => 
                param.Name == "Year" && param.Values.Contains(ExpectedReportYear)));

            Assert.IsTrue(parameters.Any(param => 
                param.Name == "Month" && param.Values.Contains(ExpectedReportMonth)));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpMonth.Items.Add(ExpectedReportMonth);
            _drpYear.Items.Add(ExpectedReportYear);
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
        }

        private void RetrievePageControls()
        {
            _drpMonth = ReflectionHelper.GetValue<DropDownList>(_page, "drpMonth");
            _drpYear = ReflectionHelper.GetValue<DropDownList>(_page, "drpYear");
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
            _billingProxyMock = new Mock<IDigitalEditionBillingProxy>();
            _billingProxyMockGetResult = new List<DigitalEditionBilling>();
            _billingProxyMock.Setup(billingProxy
                    => billingProxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(_billingProxyMockGetResult);
        }

        #endregion
    }
}
