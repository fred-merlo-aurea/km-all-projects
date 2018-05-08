using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
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
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN.Accounts.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdvertiserReportTest : BasePageTests
    {
        #region Consts

        private const string SelectedGroupId = "123";

        private const string SelectedGroupName = "SampleGroupName";

        #endregion
        #region Private members

        private Mock<IAdvertiserClickReportProxy> _advertiserClickProxyMock;
        private List<FrameworkEntities.AdvertiserClickReport> _advertiserClickProxyMockGetResult;
        private Mock<IReportDefinitionProvider> _reportDefinitionProviderMock;
        private Stream _reportDefinitionStream;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private AdvertiserReport _page;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private Label _lblSelectGroupName;
        private HiddenField _hfSelectGroupId;
        private DropDownList _drpExport;
        private ReportViewer _reportViewer;
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

            MockChampionAuditProxy();
            MockReportDefinitionProvider();
            MockReportContentGenerator();

            _page = new AdvertiserReport(
                _advertiserClickProxyMock.Object,
                _reportDefinitionProviderMock.Object,
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
            _advertiserClickProxyMock.Verify(proxy => proxy.GetList(
                It.Is<int>(groupId => groupId.ToString() == SelectedGroupId),
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
            var expectedReportName = "ECN_Framework_Common.Reports.rpt_AdvertiserClickReport.rdlc";

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
            Assert.AreEqual("attachment; filename=AdvertiserClickReport.PDF", _responseHeaders["content-disposition"]);
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
        public void BtnReportClick_ReportPeriodLessThanYear_ViewerPreparedAndErrorNotShown()
        {
            // Arrange
            InitializeReportParametersControls();
            _reportViewer.Visible = true;
            _phError.Visible = false;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsFalse(_reportViewer.Visible);
            Assert.IsFalse(_phError.Visible);
        }

        [Test]
        public void BtnReportClick_ReportPeriodMoreThanYear_ViewerNotPreparedAndErrorShown()
        {
            // Arrange
            InitializeReportParametersControls();
            _txtStartDate.Text = DateTime.Today.AddYears(-2).ToShortDateString();
            _reportViewer.Visible = true;
            var expectedErrorMessage = "<br/>DateRange: Search range longer than the max of one year";

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_reportViewer.Visible);
            Assert.AreEqual(expectedErrorMessage, _lblErrorMessage.Text);
            Assert.IsTrue(_phError.Visible);
        }

        #endregion
        #region Private methods

        private bool ValidateDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_AdvertiserClickReport", dataSource.Name);
            Assert.AreEqual(_advertiserClickProxyMockGetResult, dataSource.Value);

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

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.Items.Add(ReportConsts.OutputTypeHTML);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _txtStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            _txtEndDate.Text = DateTime.Today.ToShortDateString();
            _hfSelectGroupId.Value = SelectedGroupId;
            _lblSelectGroupName.Text = SelectedGroupName;
        }

        private void RetrievePageControls()
        {
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _reportViewer = ReflectionHelper.GetValue<ReportViewer>(_page, "ReportViewer1");
            _lblErrorMessage = ReflectionHelper.GetValue<Label>(_page, "lblErrorMessage");
            _phError = ReflectionHelper.GetValue<PlaceHolder>(_page, "phError");
            _hfSelectGroupId = ReflectionHelper.GetValue<HiddenField>(_page, "hfSelectGroupID");
            _lblSelectGroupName = ReflectionHelper.GetValue<Label>(_page, "lblSelectGroupName");
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

        private void MockChampionAuditProxy()
        {
            _advertiserClickProxyMock = new Mock<IAdvertiserClickReportProxy>();
            _advertiserClickProxyMockGetResult = new List<FrameworkEntities.AdvertiserClickReport>();
            _advertiserClickProxyMock
                .Setup(proxy => proxy.GetList(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .Returns(_advertiserClickProxyMockGetResult);
        }

        #endregion
    }
}
