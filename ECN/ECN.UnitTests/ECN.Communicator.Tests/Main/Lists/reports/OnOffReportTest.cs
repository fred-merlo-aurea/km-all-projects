using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists.reports;
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
    public class OnOffReportTest : BasePageTests
    {
        #region Consts

        private const string SelectedGroupId = "123";
        private const string SelectedGroupName = "SampleGroupName";
        private const string SelectedFieldName = "SampleField";
        private const string SelectedReportType = "SampleReportType";

        #endregion
        #region Private members

        private Mock<IOnOffsByFieldProxy> _onOffsByFieldProxyMock;
        private List<FrameworkEntities.OnOffsByField> _onOffsByFieldProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private OnOffReport _page;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private DropDownList _drpExport;
        private HiddenField _hfSelectGroupId;
        private DropDownList _drpFields;
        private RadioButtonList _rbReporttype;
        private Label _lblSelectGroupName;
        private ReportViewer _reportViewer;
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

            MockOnOffsByFieldProxy();
            MockReportContentGenerator();

            _page = new OnOffReport(_onOffsByFieldProxyMock.Object, _reportContentGeneratorMock.Object);
            
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
            _onOffsByFieldProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(customerId => customerId == CustomerId),
                It.Is<string>(field => field == SelectedFieldName),
                It.Is<DateTime>(startDate => startDate == DateTime.Today.AddDays(-1)),
                It.Is<DateTime>(endDate => endDate == DateTime.Today),
                It.Is<string>(reportType => reportType == SelectedReportType)
            ));
        }

        [Test]
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = _page.Server.MapPath("~/main/lists/Reports/rpt_OnOffReport.rdlc");

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
            Assert.AreEqual("attachment; filename=OnOffsByField.PDF", _responseHeaders["content-disposition"]);
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
        public void BtnReportClick_ExportTypeRequestedXLSData_ReportContentGeneratorNotCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            _drpExport.SelectedValue = ReportConsts.OutputTypeXLSDATA;
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
        
        #endregion
        #region Private methods

        private bool ValidateDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_OnOff", dataSource.Name);
            Assert.AreEqual(_onOffsByFieldProxyMockGetResult, dataSource.Value);

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
            _drpExport.Items.Add(ReportConsts.OutputTypeXLSDATA);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _txtStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            _txtEndDate.Text = DateTime.Today.ToShortDateString();
            _lblSelectGroupName.Text = SelectedGroupName;
            _hfSelectGroupId.Value = SelectedGroupId;
            _drpFields.Items.Add(SelectedFieldName);
            _drpFields.SelectedValue = SelectedFieldName;
            _rbReporttype.Items.Add(SelectedReportType);
            _rbReporttype.SelectedValue = SelectedReportType;
        }

        private void RetrievePageControls()
        {
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _reportViewer = ReflectionHelper.GetValue<ReportViewer>(_page, "ReportViewer1");
            _hfSelectGroupId = ReflectionHelper.GetValue<HiddenField>(_page, "hfSelectGroupID");
            _lblSelectGroupName = ReflectionHelper.GetValue<Label>(_page, "lblSelectGroupName");
            _drpFields = ReflectionHelper.GetValue<DropDownList>(_page, "drpFields");
            _rbReporttype = ReflectionHelper.GetValue<RadioButtonList>(_page, "rbReporttype");
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

        private void MockOnOffsByFieldProxy()
        {
            _onOffsByFieldProxyMock = new Mock<IOnOffsByFieldProxy>();
            _onOffsByFieldProxyMockGetResult = new List<FrameworkEntities.OnOffsByField>();
            _onOffsByFieldProxyMock.Setup(proxy
                    => proxy.Get(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .Returns(_onOffsByFieldProxyMockGetResult);
        }

        #endregion
    }
}
