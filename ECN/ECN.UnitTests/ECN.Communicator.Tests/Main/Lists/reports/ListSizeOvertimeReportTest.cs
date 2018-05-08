using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Moq;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.lists.reports;
using ECN.Tests.Helpers;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Communicator.Report.Interfaces;
using FrameworkEntities = ECN_Framework_Entities.Communicator.Report;

namespace ECN.Accounts.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ListSizeOvertimeReportTest : BasePageTests
    {
        #region Consts

        private const string SelectedGroupName = "SampleGroupName";
        private const int SelectedGroupId = 123;
        private const string SelectedGroupIdString = "123";

        #endregion
        #region Private members

        private Mock<IListSizeOvertimeReportProxy> _sizeOvertimeReportProxyMock;
        private List<FrameworkEntities.ListSizeOvertimeReport> _sizeOvertimeReportProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private ListSizeOvertimeReport _page;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private DropDownList _drpExport;
        private Label _lblSelectGroupName;
        private HiddenField _hfSelectGroupId;
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

            MockSizeOvertimeReportProxy();
            MockReportContentGenerator();

            _page = new ListSizeOvertimeReport(_sizeOvertimeReportProxyMock.Object, _reportContentGeneratorMock.Object);
            
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
            _sizeOvertimeReportProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(groupId => groupId == SelectedGroupId),
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
            var expectedReportPath = _page.Server.MapPath("~/main/lists/Reports/rpt_ListSizeOverTime.rdlc");

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
            Assert.AreEqual("attachment; filename=ListSizeOverTimeReport.PDF", _responseHeaders["content-disposition"]);
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
            Assert.AreEqual("DataSet1", dataSource.Name);
            Assert.AreEqual(_sizeOvertimeReportProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsNotNull(parameters);

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "StartDate" && 
                param.Values.Contains(DateTime.Today.AddDays(-1).ToShortDateString())));

            Assert.IsTrue(parameters.Any(param => 
                param.Name == "EndDate" && 
                param.Values.Contains(DateTime.Today.ToShortDateString())));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "GroupName" &&
                param.Values.Contains(SelectedGroupName)));

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
            _lblSelectGroupName.Text = SelectedGroupName;
            _hfSelectGroupId.Value = SelectedGroupIdString;
        }

        private void RetrievePageControls()
        {
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _lblSelectGroupName = ReflectionHelper.GetValue<Label>(_page, "lblSelectGroupName");
            _hfSelectGroupId = ReflectionHelper.GetValue<HiddenField>(_page, "hfSelectGroupID");
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

        private void MockSizeOvertimeReportProxy()
        {
            _sizeOvertimeReportProxyMock = new Mock<IListSizeOvertimeReportProxy>();
            _sizeOvertimeReportProxyMockGetResult = new List<FrameworkEntities.ListSizeOvertimeReport>();
            _sizeOvertimeReportProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(_sizeOvertimeReportProxyMockGetResult);
        }

        #endregion
    }
}
