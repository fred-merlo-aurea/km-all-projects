using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.blasts.Report;
using ECN.Tests.Helpers;
using ECN_Framework.Common;
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
    public class ChampionAuditReportTest: BasePageTests
    {
        #region Private members

        private Mock<IChampionAuditReportProxy> _championAuditProxyMock;
        private List<FrameworkEntities.ChampionAuditReport> _championAuditProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private ChampionAuditReport _page;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
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
            MockReportContentGenerator();

            _page = new ChampionAuditReport(_championAuditProxyMock.Object, _reportContentGeneratorMock.Object);
            
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
        }

        [Test]
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();
            
            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _championAuditProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(customerId => customerId == CustomerId),
                It.Is<DateTime>(startDate => startDate == DateTime.Today.AddDays(-1)),
                It.Is<DateTime>(endDate => endDate == DateTime.Today)
            ));
        }

        [Test]
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var reportLocation = ConfigurationManager.AppSettings["ReportPath"] + "rpt_ChampionAudit.rdlc";
            var expectedReportPath = _page.Server.MapPath(reportLocation);

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

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
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=ChampionAuditReport.PDF", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnReportClick_ExportTypeRequestedPDF_ReportContentGeneratorCalledWithPDF()
        {
            // Arrange
            InitializeReportParametersControls();

            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            var responseContentType = ResponseConsts.ContentTypePDF;

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

            _drpExport.SelectedValue = ReportConsts.OutputTypeXLS;
            var responseContentType = ResponseConsts.ContentTypeExcel;

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

        [Test]
        public void BtnReportClick_ExportTypeRequestedHTML_ReportContentGeneratorNotCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            _drpExport.SelectedValue = ReportConsts.OutputTypeHTML;
            string responseContentType;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.IsAny<string>(),
                It.IsAny<ReportDataSource>(),
                It.IsAny<ReportParameter[]>(),
                It.IsAny<string>(),
                out responseContentType
            ), Times.Never);
        }

        [Test]
        public void BtnReportClick_DateRangeIsValid_ReportViewerPreparedToGenerateReportAndErrorNotShown()
        {
            // Arrange
            InitializeReportParametersControls();

            _reportViewer.Visible = false;
            _reportViewer.ShowBackButton = true;
            _reportViewer.PageCountMode = PageCountMode.Estimate;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_reportViewer.Visible);
            Assert.IsFalse(_reportViewer.ShowBackButton);
            Assert.AreEqual(PageCountMode.Actual, _reportViewer.PageCountMode);
            Assert.IsFalse(_phError.Visible);
        }

        [Test]
        public void BtnReportClick_DateRangeIsInValid_ReportViewerNotPreparedToGenerateReportAndErrorShown()
        {
            // Arrange
            InitializeReportParametersControls();

            _txtStartDate.Text = DateTime.Today.AddDays(1).ToShortDateString();
            _txtEndDate.Text = DateTime.Today.ToShortDateString();
            _reportViewer.Visible = true;
            _reportViewer.ShowBackButton = true;
            _reportViewer.PageCountMode = PageCountMode.Estimate;
            var expectedErrorMessage = "Please enter a valid date starting no earlier than the last year";

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsFalse(_reportViewer.Visible);
            Assert.IsTrue(_reportViewer.ShowBackButton);
            Assert.AreEqual(PageCountMode.Estimate, _reportViewer.PageCountMode);

            Assert.AreEqual(expectedErrorMessage, _lblErrorMessage.Text);
            Assert.IsTrue(_phError.Visible);
        }

        #endregion
        #region Private methods

        private bool ValidateDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DataSet1", dataSource.Name);
            Assert.AreEqual(_championAuditProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
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

        private void MockChampionAuditProxy()
        {
            _championAuditProxyMock = new Mock<IChampionAuditReportProxy>();
            _championAuditProxyMockGetResult = new List<FrameworkEntities.ChampionAuditReport>();
            _championAuditProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(_championAuditProxyMockGetResult);
        }

        #endregion
    }
}
