using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Moq;
using NUnit.Framework;
using ecn.communicator.main.blasts.reports;
using ECN.Tests.Helpers;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ECN_Framework_Entities.Activity.Report.Undeliverable;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UndeliverableReportTest: BasePageTests
    {
        #region Consts

        private const int ExpectedCustomerId = 0;

        #endregion
        #region Private members

        private Mock<IUndeliverableReportProxy> _undeliverableReportProxyMock;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private IList<IUndeliverable> _undeliverableReportGetResult;
        private UndeliverableReport _page;
        private DropDownList _drpExport;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private DropDownList _drpUndeliverableType;
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
            ShimPage.AllInstances.IsValidGet = page => true;
            
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();

            MockUndeliverableReportProxy();
            MockReportContentGenerator();

            _page = new UndeliverableReport(
                _undeliverableReportProxyMock.Object,
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
        public void BtnReportClick_ExportTypeIsXls_ResponseInitializedWithReportOuput()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_page.Response.Buffer);
            Assert.AreEqual("attachment; filename=UnsubscribeReasonDetail.xls", _responseHeaders["content-disposition"]);
            Assert.AreEqual(_reportOutput, _responseContent);
        }

        [Test]
        public void BtnReportClick_ExportTypeIsXls_DataProxyCalledToGetReportData()
        {
            // Arrange
            InitializeReportParametersControls();

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _undeliverableReportProxyMock.Verify(proxy => proxy.GetAll(
                It.Is<DateTime>(dateFrom => dateFrom == DateTime.Today.AddDays(-1)),
                It.Is<DateTime>(dateTo => dateTo == DateTime.Today),
                It.Is<int>(customerId => customerId == ExpectedCustomerId))
            );
        }

        [Test]
        public void BtnReportClick_ExportTypeIsXls_ReportContentGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            string responseContentType;
            var expectedReportPath = _page.Server.MapPath("rpt_UndeliverableReport.rdlc");

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.Is<string>(path => path == expectedReportPath),
                It.Is<ReportDataSource>(dataSource => ValidateReportDataSource(dataSource)),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters)),
                It.Is<string>(outputType => outputType == ReportConsts.OutputTypeXLS),
                out responseContentType
            ));
        }

        [Test]
        public void BtnReportClick_ExportTypeIsXls_ReportContentGeneratorCalledWithExcel()
        {
            // Arrange
            InitializeReportParametersControls();
            string responseContentType;

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

        #endregion
        #region Private methods
        
        private bool ValidateReportDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_Undeliverable", dataSource.Name);
            Assert.AreEqual(_undeliverableReportGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters)
        {
            Assert.IsNotNull(parameters);
            Assert.AreEqual(5, parameters.Length);
            Assert.IsTrue(parameters.Any(param => 
                param.Name == "StartDate" &&
                param.Values[0] == DateTime.Today.AddDays(-1).ToShortDateString()));
            Assert.IsTrue(parameters.Any(param => 
                param.Name == "EndDate" &&
                param.Values[0] == DateTime.Today.ToShortDateString()));
            Assert.IsTrue(parameters.Any(param =>
                param.Name == "CustomerID" &&
                param.Values[0] == ExpectedCustomerId.ToString()));
            Assert.IsTrue(parameters.Any(param =>
                param.Name == "UndeliverableType" &&
                param.Values[0] == UndeliverableReport.UndeliverableType.All.ToString()));
            
            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _txtStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            _txtEndDate.Text = DateTime.Today.ToShortDateString();
            _drpUndeliverableType.Items.Add(UndeliverableReport.UndeliverableType.All.ToString());
        }

        private void RetrievePageControls()
        {
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _txtStartDate = ReflectionHelper.GetValue<TextBox>(_page, "txtstartDate");
            _txtEndDate = ReflectionHelper.GetValue<TextBox>(_page, "txtendDate");
            _drpUndeliverableType = ReflectionHelper.GetValue<DropDownList>(_page, "drpUndeliverableType");
        }

        private void MockUndeliverableReportProxy()
        {
            _undeliverableReportProxyMock = new Mock<IUndeliverableReportProxy>();
            _undeliverableReportGetResult = new List<IUndeliverable>();
            _undeliverableReportProxyMock
                .Setup(proxy => proxy.GetAll(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<int>()))
                .Returns(_undeliverableReportGetResult);
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

        #endregion
    }
}
