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
    public class ChannelLookTest: BasePageTests
    {
        #region Consts

        private const string ReportTypeNoUsage = "2";
        private const string ReportTypeChannelLook = "1";
        private const string ExpectedDateFrom = "01/01/2000";
        private const string ExpectedDateTo = "31/12/2010";
        private const string ExpectedChannel = "123";
        
        #endregion
        #region Private members

        private Mock<INoUsageProxy> _noUsageProxyMock;
        private Mock<IChannelLookProxy> _channelLookProxyMock;
        private List<NoUsage> _noUsageProxyMockGetResult;
        private List<ChannelLook> _channelLookProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private ecn.accounts.main.reports.ChannelLook _page;
        private DropDownList _drpChannel;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private DropDownList _drpExport;
        private ListBox _lstCustomers;
        private RadioButtonList _rbReportType;
        private CheckBox _chkTestBlastOnly;
        private CheckBox _chkIncludeBlastDetails;
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

            MockNoUsageProxy();
            MockChannelLookProxy();
            MockReportContentGenerator();

            _page = new ecn.accounts.main.reports.ChannelLook(
                _noUsageProxyMock.Object,
                _channelLookProxyMock.Object,
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
        public void BtnSubmitClick_ReportTypeNoUsage_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeNoUsage);

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _noUsageProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(channel => channel == 123),
                It.Is<string>(customer => customer.Equals("SampleCustomer1")),
                It.Is<string>(dateFrom => dateFrom == ExpectedDateFrom),
                It.Is<string>(dateTo => dateTo == ExpectedDateTo)
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNoUsage_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeNoUsage);

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_NoUsage.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateNoUsageDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => ValidateNoUsageReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNoUsage_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeNoUsage);

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=NoUsage.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ReportTypeNoUsageAndExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeNoUsage);
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
        public void BtnSubmitClick_ReportTypeNoUsageAndExportTypeRequestedExcel_ReportContentGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeNoUsage);
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

        [Test]
        public void BtnSubmitClick_ReportTypeChannelLook_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeChannelLook);

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _channelLookProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(channel => channel == 123),
                It.Is<string>(customer => customer.Equals("SampleCustomer1")),
                It.Is<string>(dateFrom => dateFrom == ExpectedDateFrom),
                It.Is<string>(dateTo => dateTo == ExpectedDateTo),
                It.Is<bool>(isTestBlast => isTestBlast))
            );
        }

        [Test]
        public void BtnSubmitClick_ReportTypeChannelLook_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeChannelLook);

            string responseContentType;
            var expectedReportPath = string.Format("{0}rpt_ChannelLook.rdlc", _page.Server.MapPath("~/main/reports/"));

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateChannelLookDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => ValidateChannelLookReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnSubmitClick_ReportTypeChannelLook_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeChannelLook);

            // Act
            _privateObject.Invoke("btnSubmit_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=ChannelLook.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnSubmitClick_ReportTypeChannelLookAndExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeChannelLook);
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
        public void BtnSubmitClick_ReportTypeChannelLookAndExportTypeRequestedExcel_ReportContentGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls(ReportTypeChannelLook);
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
        
        private bool ValidateNoUsageDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_NoUsage", dataSource.Name);
            Assert.AreEqual(_noUsageProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateChannelLookDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_ChannelLook", dataSource.Name);
            Assert.AreEqual(_channelLookProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateNoUsageReportParameters(ReportParameter[] parameters)
        {
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Length);
            Assert.IsTrue(parameters.Any(param => param.Name == "StartDate" && param.Values[0] == ExpectedDateFrom));
            Assert.IsTrue(parameters.Any(param => param.Name == "EndDate" && param.Values[0] == ExpectedDateTo));

            return true;
        }

        private bool ValidateChannelLookReportParameters(ReportParameter[] parameters)
        {
            Assert.IsNotNull(parameters);
            Assert.AreEqual(3, parameters.Length);
            Assert.IsTrue(parameters.Any(param => param.Name == "StartDate" && param.Values[0] == ExpectedDateFrom));
            Assert.IsTrue(parameters.Any(param => param.Name == "EndDate" && param.Values[0] == ExpectedDateTo));
            Assert.IsTrue(parameters.Any(param => param.Name == "BlastDetails" && param.Values[0] == "true"));

            return true;
        }

        private void InitializeReportParametersControls(string reportType)
        {
            _rbReportType.Items.Add(ReportTypeChannelLook);
            _rbReportType.Items.Add(ReportTypeNoUsage);
            _rbReportType.SelectedValue = reportType;
            _drpChannel.Items.Add(ExpectedChannel);
            _txtStartDate.Text = ExpectedDateFrom;
            _txtEndDate.Text = ExpectedDateTo;
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _lstCustomers.Items.Add("SampleCustomer1");
            _lstCustomers.Items.Add("SampleCustomer2");
            _lstCustomers.SelectedValue = "SampleCustomer1";
            _chkTestBlastOnly.Checked = true;
            _chkIncludeBlastDetails.Checked = true;
        }

        private void RetrievePageControls()
        {
            _drpChannel = ReflectionHelper.GetValue<DropDownList>(_page, "drpChannel");
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _lstCustomers = ReflectionHelper.GetValue<ListBox>(_page, "lstCustomer");
            _rbReportType = ReflectionHelper.GetValue<RadioButtonList>(_page, "rbReportType");
            _chkTestBlastOnly = ReflectionHelper.GetValue<CheckBox>(_page, "chkTestBlastOnly");
            _chkIncludeBlastDetails = ReflectionHelper.GetValue<CheckBox>(_page, "chkIncludeBlstDetails");
        }

        private void MockNoUsageProxy()
        {
            _noUsageProxyMock = new Mock<INoUsageProxy>();
            _noUsageProxyMockGetResult = new List<NoUsage>();
            _noUsageProxyMock.Setup(noUsageProxy
                    => noUsageProxy.Get(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_noUsageProxyMockGetResult);
        }

        private void MockChannelLookProxy()
        {
            _channelLookProxyMock = new Mock<IChannelLookProxy>();
            _channelLookProxyMockGetResult = new List<ChannelLook>();
            _channelLookProxyMock.Setup(noUsageProxy
                    => noUsageProxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .Returns(_channelLookProxyMockGetResult);
        }

        private void MockReportContentGenerator()
        {
            var responseContentType = "application/pdf";
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

        #endregion
    }
}
