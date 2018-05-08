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
    public class StatisticsByFieldTest: BasePageTests
    {
        #region Consts

        private const int ExpectedBlastId = 123;
        private const string ExpectedBlastValue = "_01/01/2000__BlastSubject";
        private const string ExpectedFieldName = "SampleFieldName";
        private const string ExpectedGroupName = "SampleGroupName";
        private const string ExpectedBlastSubject = "BlastSubject";
        private const string ExpectedBlastSendDate = "01/01/2000";

        #endregion
        #region Private members

        private Mock<IStatisticsByFieldProxy> _statisticsByFieldProxyMock;
        private List<FrameworkEntities.StatisticsByField> _statisticsByFieldProxyMockGetResult;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private StatisticsbyField _page;
        private DropDownList _drpBlast;
        private DropDownList _drpFields;
        private DropDownList _drpExport;
        private ReportViewer _reportViewer;
        private HiddenField _hfSelectGroupId;
        private Label _lblSelectGroupName;
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

            _page = new StatisticsbyField(_statisticsByFieldProxyMock.Object, _reportContentGeneratorMock.Object);
            
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
            _statisticsByFieldProxyMock.Verify(proxy => proxy.Get(
                It.Is<int>(blastId => blastId == ExpectedBlastId),
                It.Is<string>(fieldName => fieldName == ExpectedFieldName)
            ));
        }

        [Test]
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = _page.Server.MapPath("~/main/lists/Reports/rpt_StatisticsByField.rdlc");

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
            Assert.AreEqual("attachment; filename=StatisticsByField.PDF", _responseHeaders["content-disposition"]);
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
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ReportViewerPreparedToGenerateReport()
        {
            // Arrange
            InitializeReportParametersControls();
            _reportViewer.Visible = true;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsFalse(_reportViewer.Visible);
        }

        #endregion
        #region Private methods

        private bool ValidateDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_StatisticsByField", dataSource.Name);
            Assert.AreEqual(_statisticsByFieldProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsNotNull(parameters);

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "GroupName" && 
                param.Values.Contains(ExpectedGroupName)));

            Assert.IsTrue(parameters.Any(param => 
                param.Name == "BlastSubject" && 
                param.Values.Contains(ExpectedBlastSubject)));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "Field" &&
                param.Values.Contains(ExpectedFieldName)));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "SendDate" &&
                param.Values.Contains(ExpectedBlastSendDate)));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.Items.Add(ReportConsts.OutputTypeHTML);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLSDATA);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _lblSelectGroupName.Text = ExpectedGroupName;
            _drpBlast.Items.Add(new ListItem(ExpectedBlastValue, ExpectedBlastId.ToString()));
            _drpFields.Items.Add(ExpectedFieldName);
            _hfSelectGroupId.Value = ExpectedBlastId.ToString();
        }

        private void RetrievePageControls()
        {
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _reportViewer = ReflectionHelper.GetValue<ReportViewer>(_page, "ReportViewer1");
            _lblSelectGroupName = ReflectionHelper.GetValue<Label>(_page, "lblSelectGroupName");
            _drpBlast = ReflectionHelper.GetValue<DropDownList>(_page, "drpBlast");
            _drpFields = ReflectionHelper.GetValue<DropDownList>(_page, "drpFields");
            _hfSelectGroupId = ReflectionHelper.GetValue<HiddenField>(_page, "hfSelectGroupID");
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

        private void MockChampionAuditProxy()
        {
            _statisticsByFieldProxyMock = new Mock<IStatisticsByFieldProxy>();
            _statisticsByFieldProxyMockGetResult = new List<FrameworkEntities.StatisticsByField>();
            _statisticsByFieldProxyMock
                .Setup(proxy
                    => proxy.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(_statisticsByFieldProxyMockGetResult);
        }

        #endregion
    }
}
