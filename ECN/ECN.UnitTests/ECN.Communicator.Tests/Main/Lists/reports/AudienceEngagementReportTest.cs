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
using Castle.Core.Internal;
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
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN.Communicator.Tests.Main.Lists.reports
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AudienceEngagementReportTest : BasePageTests
    {
        #region Consts

        private const int SelectedGroupId = 123;
        private const int SelectedClickPercent = 50;
        private const int SelectedDays = 365;
        private const string SelectedGroupName = "SampleGroupName";

        #endregion
        #region Private members

        private Mock<IAudienceEngagementReportProxy> _audienceEngagementProxyMock;
        private List<ReportEntities.AudienceEngagementReport> _audienceEngagementProxyMockGetResult;
        private Mock<IReportDefinitionProvider> _reportDefinitionProviderMock;
        private Stream _reportDefinitionStream;
        private Mock<IReportContentGenerator> _reportContentGeneratorMock;
        private AudienceEngagementReport _page;
        private DropDownList _drpExport;
        private HiddenField _hfSelectGroupId;
        private TextBox _txtClickPercent;
        private TextBox _txtDays;
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

            MockChampionAuditProxy();
            MockReportDefinitionProvider();
            MockReportContentGenerator();

            _page = new AudienceEngagementReport(
                _audienceEngagementProxyMock.Object,
                _reportDefinitionProviderMock.Object,
                _reportContentGeneratorMock.Object);

            InitializePage(_page);
            RetrievePageControls();
            ReflectionHelper.SetValue(_page, typeof(Page), "_master", new ecn.communicator.MasterPages.Communicator());

            GrantUserAccess(
                KMPlatform.Enums.Services.EMAILMARKETING.ToString(),
                KMPlatform.Enums.ServiceFeatures.AudienceEngagementReport.ToString());
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
            const string expectedDownload = "N";

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _audienceEngagementProxyMock.Verify(proxy => proxy.GetList(
                It.Is<int>(groupId => groupId == SelectedGroupId),
                It.Is<int>(clickPercent => clickPercent == SelectedClickPercent),
                It.Is<int>(days => days == SelectedDays),
                It.Is<string>(downloadDetails => downloadDetails.Equals(expectedDownload)),
                It.Is<string>(downloadType => downloadType.IsNullOrEmpty())
            ));
        }

        [Test]
        public void BtnReportClick_ValidReportParameters_ReportProviderAndGeneratorCalled()
        {
            // Arrange
            InitializeReportParametersControls();
            string responseContentType;
            var expectedLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var expectedReportName = "ECN_Framework_Common.Reports.rpt_AudienceEngagementReport.rdlc";

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
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters, true)),
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
            Assert.AreEqual("attachment; filename=AudienceEngagementReport.PDF", _responseHeaders["content-disposition"]);
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
        public void BtnReportClick_ExportTypeRequestedHTML_ReportGeneratorNotCalled()
        {
            // Arrange
            InitializeReportParametersControls();

            _drpExport.SelectedValue = ReportConsts.OutputTypeHTML;
            string responseContentType;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.IsAny<Stream>(),
                It.IsAny<ReportDataSource>(),
                It.IsAny<ReportParameter[]>(),
                It.IsAny<string>(),
                out responseContentType
            ), Times.Never);
        }

        [Test]
        public void BtnReportClick_UserHasAccess_HyperLinksEnabled()
        {
            // Arrange
            InitializeReportParametersControls();

            _reportViewer.Visible = false;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_reportViewer.Visible);
            Assert.IsTrue(_reportViewer.LocalReport.EnableHyperlinks);
        }

        [Test]
        public void BtnReportClick_UserHasNoAccess_HyperLinksDisabled()
        {
            // Arrange
            InitializeReportParametersControls();
            RevokeUserAccess(KMPlatform.Enums.Services.EMAILMARKETING.ToString());
            _reportViewer.Visible = false;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_reportViewer.Visible);
            Assert.IsFalse(_reportViewer.LocalReport.EnableHyperlinks);
        }

        [Test]
        public void BtnReportClick_UserHasNoAccess_ReportGeneratorCalledWithProperParams()
        {
            // Arrange
            InitializeReportParametersControls();
            RevokeUserAccess(KMPlatform.Enums.Services.EMAILMARKETING.ToString());
            string responseContentType;

            // Act
            _privateObject.Invoke("btnReport_Click", this, EventArgs.Empty);

            // Assert
            _reportContentGeneratorMock.Verify(reportGenerator => reportGenerator.CreateReportContent(
                It.IsAny<Stream>(),
                It.IsAny<ReportDataSource>(),
                It.Is<ReportParameter[]>(parameters => ValidateReportParameters(parameters, false)),
                It.IsAny<string>(),
                out responseContentType
            ));
        }

        #endregion
        #region Private methods

        private bool ValidateDataSource(ReportDataSource dataSource)
        {
            Assert.IsNotNull(dataSource);
            Assert.AreEqual("DS_AudienceEngagementReport", dataSource.Name);
            Assert.AreEqual(_audienceEngagementProxyMockGetResult, dataSource.Value);

            return true;
        }

        private bool ValidateReportParameters(ReportParameter[] parameters, bool userHasAccess)
        {
            Assert.IsNotNull(parameters);

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "GroupName" &&
                param.Values.Contains(SelectedGroupName)));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "Format" &&
                param.Values.Contains(userHasAccess ? ReportConsts.OutputTypePDF : string.Empty)));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "ClickPercentage" &&
                param.Values.Contains(SelectedClickPercent.ToString())));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "GroupID" &&
                param.Values.Contains(SelectedGroupId.ToString())));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "Days" &&
                param.Values.Contains(SelectedDays.ToString())));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "URL" &&
                param.Values.Contains(RequestUrl)));

            Assert.IsTrue(parameters.Any(param =>
                param.Name == "EnableHyperLinks" &&
                param.Values.Contains(userHasAccess.ToString())));

            return true;
        }

        private void InitializeReportParametersControls()
        {
            _drpExport.Items.Add(ReportConsts.OutputTypePDF);
            _drpExport.Items.Add(ReportConsts.OutputTypeXLS);
            _drpExport.Items.Add(ReportConsts.OutputTypeHTML);
            _drpExport.SelectedValue = ReportConsts.OutputTypePDF;
            _hfSelectGroupId.Value = SelectedGroupId.ToString();
            _txtClickPercent.Text = SelectedClickPercent.ToString();
            _txtDays.Text = SelectedDays.ToString();
            _lblSelectGroupName.Text = SelectedGroupName;
        }

        private void RetrievePageControls()
        {
            _drpExport = ReflectionHelper.GetValue<DropDownList>(_page, "drpExport");
            _reportViewer = ReflectionHelper.GetValue<ReportViewer>(_page, "ReportViewer1");
            _hfSelectGroupId = ReflectionHelper.GetValue<HiddenField>(_page, "hfSelectGroupID");
            _txtClickPercent = ReflectionHelper.GetValue<TextBox>(_page, "txtClickPercent");
            _txtDays = ReflectionHelper.GetValue<TextBox>(_page, "txtDays");
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
            _audienceEngagementProxyMock = new Mock<IAudienceEngagementReportProxy>();
            _audienceEngagementProxyMockGetResult = new List<ReportEntities.AudienceEngagementReport>();
            _audienceEngagementProxyMock
                .Setup(proxy => proxy.GetList(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(_audienceEngagementProxyMockGetResult);
        }

        #endregion
    }
}