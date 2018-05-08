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
using ecn.communicator.main.blasts.reports;
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
    public class BlastDeliveryReportTest : BasePageTests
    {
        #region

        private const string CustomerIds = "123";

        #endregion
        #region Private members

        private Mock<IBlastDeliveryProxy> _blastDeliveryProxyMock;
        private List<FrameworkEntities.BlastDelivery> _blastDeliveryProxyMockGetResult;
        private Mock<IReportDefinitionProvider> _reportDefinitionProviderMock;
        private Stream _reportDefinitionStream;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private BlastDeliveryReport _page;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private DropDownList _drpExport;
        private ListBox _lstboxCustomer;
        private DropDownList _drpIsUnique;
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

            MockBlastDeliveryProxy();
            MockReportDefinitionProvider();
            MockReportContentGenerator();

            _page = new BlastDeliveryReport(
                _blastDeliveryProxyMock.Object,
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
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();
            
            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _blastDeliveryProxyMock.Verify(proxy => proxy.Get(
                It.Is<string>(customerIds => customerIds == CustomerIds),
                It.Is<DateTime>(startDate => startDate == DateTime.Today.AddDays(-1)),
                It.Is<DateTime>(endDate => endDate == DateTime.Today),
                It.Is<bool>(downloadDetails => downloadDetails)
            ));
        }

        [Test]
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ReportDefinitionProviderAndReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();
            string responseContentType;
            var expectedLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var expectedReportName = "ECN_Framework_Common.Reports.rpt_BlastDelivery.rdlc";

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
        public void BtnReportClick_ReportParametersControlsContainCorrectValues_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=BlastDelivery.PDF", _responseHeaders["content-disposition"]);
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
                It.IsAny<Stream>(),
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
                It.IsAny<Stream>(),
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
            Assert.AreEqual("DS_BlastDelivery", dataSource.Name);
            Assert.AreEqual(_blastDeliveryProxyMockGetResult, dataSource.Value);

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
                param.Name == "Type" &&
                param.Values.Contains(ReportConsts.OutputTypePDF)));

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
            _lstboxCustomer.Items.Add(CustomerIds);
            _lstboxCustomer.SelectedValue = CustomerIds;
            _lstboxCustomer.Visible = true;
            _drpIsUnique.Items.Add(true.ToString());
            _drpIsUnique.Items.Add(false.ToString());
            _drpIsUnique.SelectedValue = true.ToString();
        }

        private void RetrievePageControls()
        {
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _drpIsUnique = ReflectionHelper.GetValue<DropDownList>(_page, "drpIsUnique");
            _lstboxCustomer = ReflectionHelper.GetValue<ListBox>(_page, "lstboxCustomer");
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

        private void MockBlastDeliveryProxy()
        {
            _blastDeliveryProxyMock = new Mock<IBlastDeliveryProxy>();
            _blastDeliveryProxyMockGetResult = new List<FrameworkEntities.BlastDelivery>();
            _blastDeliveryProxyMock
                .Setup(proxy => proxy.Get(
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<bool>()))
                .Returns(_blastDeliveryProxyMockGetResult);
        }

        #endregion
    }
}
