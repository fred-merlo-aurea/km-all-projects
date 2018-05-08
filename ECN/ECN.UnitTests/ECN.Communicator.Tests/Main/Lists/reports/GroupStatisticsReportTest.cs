using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists.reports;
using ECN.Tests.Helpers;
using ECN_Framework.Common;
using ECN_Framework.Common.Interfaces;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Moq;
using NUnit.Framework;
using Shouldly;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN.Accounts.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GroupStatisticsReportTest : BasePageTests
    {
        #region Consts

        private const int SelectedGroupId = 123;
        private const string SelectedGroupName = "SampleGroupName";

        #endregion
        #region Private members

        private Mock<IGroupStatisticsReportProxy> _groupStatisticsProxyMock;
        private List<FrameworkEntities.GroupStatisticsReport> _groupStatisticsProxyMockGetResult;
        private Mock<IReportDefinitionProvider> _reportDefinitionProviderMock;
        private Stream _reportDefinitionStream;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private groupStatisticsReport _page;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private DropDownList _drpExport;
        private CheckBox _chkDetails;
        private ReportViewer _reportViewer;
        private HiddenField _hfSelectGroupId;
        private Label _lblSelectGroupName;
        private Label _lblErrorMessage;
        private PlaceHolder _phError;
        private byte[] _reportOutput;
        private IDisposable _shimsContext;
        private FakeHttpContext.FakeHttpContext _fakeHttpContext;

        #endregion
        #region Public methods

        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(DefaultCulture);
            _shimsContext = ShimsContext.Create();
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();

            MockGroupStatisticsProxy();
            MockReportDefinitionProvider();
            MockReportContentGenerator();

            _page = new groupStatisticsReport(
                _groupStatisticsProxyMock.Object,
                _reportDefinitionProviderMock.Object,
                _reportContentGeneratorMock.Object);
            
            InitializePage(_page);
            RetrievePageControls();
            ReflectionHelper.SetValue(_page, typeof(Page), "_master", new ecn.communicator.MasterPages.Communicator());
        }

        [TearDown]
        public void TearDown()
        {
            ShimsContext.Reset();
            _shimsContext.Dispose();
            _fakeHttpContext.Dispose();
            _reportDefinitionStream.Dispose();
        }

        [Test]
        public void BtnReportClick_ValidReportParameters_DataProxyCalled()
        {
            // Arrange
            InitializeReportParametersControls();
            
            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _groupStatisticsProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(groupId => groupId == SelectedGroupId),
                It.Is<DateTime>(startDate => startDate == DateTime.Today.AddDays(-1)),
                It.Is<DateTime>(endDate => endDate == DateTime.Today)
            ));
        }

        [Test]
        public void BtnReportClick_ValidReportParameters_ReportProviderAndGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();
            string responseContentType;
            var expectedLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var expectedReportName = "ECN_Framework_Common.Reports.rpt_GroupStatisticsReport.rdlc";

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportDefinitionProviderMock.Verify(reportProvider => reportProvider.GetReportDefinitionStream(
                It.Is<string>(location => location == expectedLocation),
                It.Is<string>(reportName => reportName == expectedReportName)
            ));

            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<Stream>(stream => stream == _reportDefinitionStream),
                It.Is<ReportDataSource>(dataSource => ValidateDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnReportClick_ValidReportParameters_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=GroupStatisticsReport.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnReportClick_ExportTypeRequestedPDF_ReportGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls();

            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            var responseContentType = ResponseConsts.ContentTypePDF;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.IsAny<Stream>(),
                It.IsAny<ReportDataSource>(),
                It.IsAny<ReportParameter[]>(),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypePDF),
                out responseContentType
            ));
        }

        [Test]
        public void BtnReportClick_ExportTypeRequestedExcel_ReportGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls();

            _drpExport.SelectedValue = ReportConsts.OutputTypeXLS;
            var responseContentType = ResponseConsts.ContentTypeExcel;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.IsAny<Stream>(),
                It.IsAny<ReportDataSource>(),
                It.IsAny<ReportParameter[]>(),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypeXLS),
                out responseContentType
            ));
        }

        [Test]
        public void BtnReportClick_ExportTypeRequestedPDF_ReportViewerPreparedToGenerateReport()
        {
            // Arrange
            InitializeReportParametersControls();

            _reportViewer.Visible = true;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsFalse(_reportViewer.Visible);
        }

        [Test]
        public void BtnReportClick_InvalidSelectedGroupId_ErrorMessageShown()
        {
            // Arrange
            InitializeReportParametersControls();
            _hfSelectGroupId.Value = string.Empty;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe("<br/>Group: Please select a Group.");
        }

        #endregion
        #region Private methods

        private bool ValidateDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_GroupStatisticsReport", dataSource.Name);
            Assert.AreEqual(_groupStatisticsProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsNotNull(parameters);

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "GroupName" &&
                param.Values.Contains(SelectedGroupName)));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "StartDate" && 
                param.Values.Contains(DateTime.Today.AddDays(-1).ToShortDateString())));

            Assert.IsTrue(parameters.Any(param => 
                param.Name == "EndDate" && 
                param.Values.Contains(DateTime.Today.ToShortDateString())));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "Details" &&
                param.Values.Contains(true.ToString())));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.Items.Add(ReportConsts.OutputTypeHTML);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLSDATA);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _txtStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            _txtEndDate.Text = DateTime.Today.ToShortDateString();
            _lblSelectGroupName.Text = SelectedGroupName;
            _chkDetails.Checked = true;
            _hfSelectGroupId.Value = "123";
        }

        private void RetrievePageControls()
        {
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _reportViewer = ReflectionHelper.GetValue<ReportViewer>(_page, "ReportViewer1");
            _lblSelectGroupName = ReflectionHelper.GetValue<Label>(_page, "lblSelectGroupName");
            _chkDetails = ReflectionHelper.GetValue<CheckBox>(_page, "chkDetails");
            _hfSelectGroupId = ReflectionHelper.GetValue<HiddenField>(_page, "hfSelectGroupID");
            _lblErrorMessage = ReflectionHelper.GetValue<Label>(_page, "lblErrorMessage");
            _phError = ReflectionHelper.GetValue<PlaceHolder>(_page, "phError");
        }

        private void MockReportDefinitionProvider()
        {
            _reportDefinitionStream = new MemoryStream();
            _reportDefinitionProviderMock = new Mock<IReportDefinitionProvider>();
            _reportDefinitionProviderMock
                .Setup(definitionProvider => definitionProvider.GetReportDefinitionStream(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(_reportDefinitionStream);
        }

        private void MockReportContentGenerator()
        {
            string responseContentType;
            _reportOutput = new byte[1];
            _reportContentGeneratorMock = new Mock<IReportContentGenerator>();
            _reportContentGeneratorMock
                .Setup(reportContentGenerator => reportContentGenerator.CreateReportContent(
                    It.IsAny<Stream>(),
                    It.IsAny<ReportDataSource>(),
                    It.IsAny<ReportParameter[]>(),
                    It.IsAny<string>(),
                    out responseContentType))
                .Returns(_reportOutput);
        }

        private void MockGroupStatisticsProxy()
        {
            _groupStatisticsProxyMock = new Mock<IGroupStatisticsReportProxy>();
            _groupStatisticsProxyMockGetResult = new List<FrameworkEntities.GroupStatisticsReport>();
            _groupStatisticsProxyMock.
                Setup(proxy => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(_groupStatisticsProxyMockGetResult);
        }

        #endregion
    }
}
