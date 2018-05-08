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
    public class BillingReportTest: BasePageTests
    {
        #region Consts

        private const string ExpectedReportMonth = "12";
        private const string ExpectedReportYear = "2000";
        private const string ExpectedChannel = "123";

        #endregion
        #region Private members

        private Mock<IBillingProxy> _billingProxyMock;
        private Mock<IBillingNoteProxy> _billingNoteProxyMock;
        private List<Billing> _billingProxyMockGetResult;
        private List<BillingNote> _billingNoteProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private BillingReport _page;
        private DropDownList _drpChannel;
        private DropDownList _drpMonth;
        private DropDownList _drpYear;
        private DropDownList _drpExport;
        private ListBox _lstCustomers;
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
            MockBillingNoteProxy();
            MockReportContentGenerator();

            _page = new BillingReport(
                _billingProxyMock.Object,
                _billingNoteProxyMock.Object,
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
        public void BtnSubmitClick_ReportParametersControlsContainCorrectValues_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _billingProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(channel => channel == 123),
                It.Is<string>(customer => customer.Equals("SampleCustomer1")),
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
            var expectedReportPath = string.Format("{0}rpt_BillingReport.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateBillingDataSource(dataSource)),
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
            Assert.AreEqual("attachment; filename=BillingReport_12_2000.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            var responseContentType = ResponseConsts.ContentTypePDF;

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
            var responseContentType = ResponseConsts.ContentTypeExcel;

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

        [Test]
        public void BtnBillingNotesClick_ReportParametersControlsContainCorrectValues_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnBillingNotes_Click", this, EventArgs.Empty);

            // Assert
            _billingNoteProxyMock.Verify(proxy => proxy.GetAll());
        }

        [Test]
        public void BtnBillingNotesClick_ReportParametersControlsContainCorrectValues_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_BillingNotes.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnBillingNotes_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateBillingNotesDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => parameters == null),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnBillingNotesClick_ReportParametersControlsContainCorrectValues_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnBillingNotes_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=BillingNotes.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnBillingNotesClick_ExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            var responseContentType = ResponseConsts.ContentTypePDF;

            // Act
            _privateObject.Invoke("btnBillingNotes_Click", this, EventArgs.Empty);

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
        public void BtnBillingNotesClick_ExportTypeRequestedExcel_ReportContentGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls();
            _drpExport.SelectedValue = ReportConsts.OutputTypeXLS;
            var responseContentType = ResponseConsts.ContentTypeExcel;

            // Act
            _privateObject.Invoke("btnBillingNotes_Click", this, EventArgs.Empty);

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

        private bool ValidateBillingDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_Billing", dataSource.Name);
            Assert.AreEqual(_billingProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateBillingNotesDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_BillingNote", dataSource.Name);
            Assert.AreEqual(_billingNoteProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsTrue(parameters.Any(param => param.Name == "month" && param.Values[0] == ExpectedReportMonth));
            Assert.IsTrue(parameters.Any(param => param.Name == "year" && param.Values[0] == ExpectedReportYear));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpChannel.Items.Add(ExpectedChannel);
            _drpMonth.Items.Add(ExpectedReportMonth);
            _drpYear.Items.Add(ExpectedReportYear);
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _lstCustomers.Items.Add("SampleCustomer1");
            _lstCustomers.Items.Add("SampleCustomer2");
            _lstCustomers.SelectedValue = "SampleCustomer1";
        }

        private void RetrievePageControls()
        {
            _drpChannel = ReflectionHelper.GetValue<DropDownList>(_page, "drpChannel");
            _drpMonth = ReflectionHelper.GetValue<DropDownList>(_page, "drpMonth");
            _drpYear = ReflectionHelper.GetValue<DropDownList>(_page, "drpYear");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _lstCustomers = ReflectionHelper.GetValue<ListBox>(_page, "lstCustomer");
        }

        private void MockReportContentGenerator()
        {
            string responseContentType;
            _reportOutput = new byte[1];
            _reportContentGeneratorMock = new Mock<IReportContentGenerator>();
            _reportContentGeneratorMock
                .Setup(reportContentGenerator => reportContentGenerator.CreateReportContent(
                    It.IsAny<string>(),
                    It.IsAny<ReportDataSource>(),
                    It.IsAny<ReportParameter[]>(),
                    It.IsAny<string>(),
                    out responseContentType))
                .Returns(_reportOutput);
        }

        private void MockBillingProxy()
        {
            _billingProxyMock = new Mock<IBillingProxy>();
            _billingProxyMockGetResult = new List<Billing>();
            _billingProxyMock
                .Setup(billingProxy => billingProxy.Get(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(_billingProxyMockGetResult);
        }

        private void MockBillingNoteProxy()
        {
            _billingNoteProxyMock = new Mock<IBillingNoteProxy>();
            _billingNoteProxyMockGetResult = new List<BillingNote>();
            _billingNoteProxyMock
                .Setup(billingProxy => billingProxy.GetAll())
                .Returns(_billingNoteProxyMockGetResult);
        }

        #endregion
    }
}
