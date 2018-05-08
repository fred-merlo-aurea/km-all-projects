using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.blastsmanager;
using ecn.communicator.blastsmanager.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ecn.controls;
using ECN.Common.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using KM.Common.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using KmEntity = KM.Common.Entity;
using KmEntityFakes = KM.Common.Entity.Fakes;
using MasterPage = ecn.communicator.MasterPages.Communicator;

namespace ECN.Communicator.Tests.Main.Blasts
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class SocialTest : PageHelper
    {
        private const string MethodPageLoad = "Page_Load";
        private const string MethodDownloadEmails = "DownloadEmails";
        private const string MethodLoadPreviewsGrid = "LoadPreviewsGrid";
        private const string MethodLoadChartPreviews = "LoadChartPreviews";
        private const string MethodLoadChartShares = "LoadChartShares";
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string One = "1";
        private const string Zero = "0";
        private const string XlsType = ".xls";
        private const string CsvType = ".csv";
        private NameValueCollection _queryString;
        private StateBag _viewState;
        private Social _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new Social();
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
            _queryString = new NameValueCollection();
            _queryString.Clear();
            _viewState = new StateBag();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        public void LoadChartShares_Success_SharesChartIsInitialized(int blastIdValue, bool isExport)
        {
            // Arrange 
            CreateShims();
            var valText = "#VALX";
            ReflectionHelper.SetValue(_testEntity, "_BlastID", blastIdValue);
            var sharesChart = ReflectionHelper.GetFieldValue(_testEntity, "chartShares") as Chart;
            sharesChart.Series.Add("Series1");
            sharesChart.ChartAreas.Add("ChartArea1");

            // Act
            _privateTestObject.Invoke(MethodLoadChartShares, isExport);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => sharesChart.ShouldNotBeNull(),
                () => sharesChart.Legends.ShouldNotBeNull(),
                () => sharesChart.Legends[0].ShouldNotBeNull(),
                () => sharesChart.Series.ShouldNotBeNull(),
                () => sharesChart.Series[0].ShouldNotBeNull(),
                () => sharesChart.Series[0].Label.Contains(valText).ShouldBeTrue(),
                () => sharesChart.Series[0].LegendText.Contains(valText).ShouldBeTrue());
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        public void LoadChartPreviews_Success_PreviewChatIsInitialized(int blastIdValue, bool isExport)
        {
            // Arrange 
            CreateShims();
            var valText = "#VALX";
            ReflectionHelper.SetValue(_testEntity, "_BlastID", blastIdValue);
            var previewChart = ReflectionHelper.GetFieldValue(_testEntity, "chartPreviews") as Chart;
            previewChart.Series.Add("Series1");
            previewChart.ChartAreas.Add("ChartArea1");

            // Act
            _privateTestObject.Invoke(MethodLoadChartPreviews, isExport);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => previewChart.ShouldNotBeNull(),
                () => previewChart.Legends.ShouldNotBeNull(),
                () => previewChart.Legends[0].ShouldNotBeNull(),
                () => previewChart.Series.ShouldNotBeNull(),
                () => previewChart.Series[0].ShouldNotBeNull(),
                () => previewChart.Series[0].Label.Contains(valText).ShouldBeTrue(),
                () => previewChart.Series[0].LegendText.Contains(valText).ShouldBeTrue());
        }

        [TestCase(Zero, "Click", "ASC")]
        [TestCase(Zero, "Click", "DESC")]
        [TestCase(One, "DisplayName", "ASC")]
        [TestCase(One, "DisplayName", "DESC")]
        [TestCase(One, "EmailAddress", "ASC")]
        [TestCase(One, "EmailAddress", "DESC")]
        public void LoadPreviewsGrid_Success_PreviewsGridIsInitialized(string blastIdValue, string sortingField, string sortingDirection)
        {
            // Arrange 
            CreateShims();
            SetEcnParameters(blastIdValue);
            CreateSocialDetailShims(blastIdValue);

            // Act
            _privateTestObject.Invoke(MethodPageLoad, null, EventArgs.Empty);
            SetSortingFields(sortingField, sortingDirection);
            _privateTestObject.Invoke(MethodLoadPreviewsGrid, null);

            // Assert
            var previewsGrid = ReflectionHelper.GetFieldValue(_testEntity, "PreviewsGrid") as ecnGridView;
            _testEntity.ShouldSatisfyAllConditions(
                () => previewsGrid.ShouldNotBeNull(),
                () => previewsGrid.ShowEmptyTable.ShouldBeTrue(),
                () => previewsGrid.EmptyTableRowText.ShouldNotBeNullOrWhiteSpace());
        }

        [TestCase(One, XlsType)]
        [TestCase(Zero, CsvType)]
        public void DownloadEmails_Success_EmailsAreDownloaded(string blastIdValue, string downloadType)
        {
            // Arrange 
            CreateShims();
            SetEcnParameters(blastIdValue);
            CreateSocialDetailShims(blastIdValue);
            var emailsDownloaded = false;
            ShimHttpResponse.AllInstances.End = (x) =>
            {
                emailsDownloaded = true;
            };

            // Act
            _privateTestObject.Invoke(MethodPageLoad, null, EventArgs.Empty);
            _privateTestObject.Invoke(MethodDownloadEmails, string.Empty, downloadType);

            // Assert
            emailsDownloaded.ShouldBeTrue();
        }

        [TestCase(One)]
        [TestCase(Zero)]
        public void Page_Load_Success_SocialGridIsLoaded(string ecnParamValue)
        {
            // Arrange 
            CreateShims();
            SetEcnParameters(ecnParamValue);
            var socialGridLoaded = false;
            ShimSocialSummary.GetSocialSummaryByBlastIDInt32Int32 = (x, y) =>
            {
                socialGridLoaded = true;
                return new List<SocialSummary> { ReflectionHelper.CreateInstance(typeof(SocialSummary)) };
            };
            ShimSocialSummary.GetSocialSummaryByCampaignItemIDInt32Int32 = (x, y) =>
            {
                socialGridLoaded = true;
                return new List<SocialSummary> { ReflectionHelper.CreateInstance(typeof(SocialSummary)) };
            };

            // Act
            _privateTestObject.Invoke(MethodPageLoad, null, EventArgs.Empty);

            // Assert
            socialGridLoaded.ShouldBeTrue();
        }

        [TestCase(true, "ASC")]
        [TestCase(true, "DESC")]
        [TestCase(false, "")]
        public void PreviewsGridSorting_SortField_SetsViewState(bool isSortField, string sortDescription)
        {
            // Arrange
            using (var testObject1 = new Social())
            {
                ShimDataGridSortCommandEventArgs.AllInstances.SortExpressionGet = (obj) => isSortField
                                                                                               ? "SortField"
                                                                                               : string.Empty;
                var privateObject = new PrivateObject(testObject1);
                var viewState = privateObject.GetProperty("ViewState") as StateBag;

                viewState["SortField"] = "SortField";
                viewState["SortDirection"] = sortDescription;

                // Act
                testObject1.PreviewsGrid_sortCommand(null, new ShimDataGridSortCommandEventArgs().Instance);

                // Assert
                testObject1.ShouldSatisfyAllConditions(
                    () => viewState["SortDirection"]
                        .ShouldBe(
                            sortDescription == "ASC"
                                ? "DESC"
                                : "ASC"),
                    () => viewState["SortField"]
                        .ShouldBe(
                            isSortField
                                ? "SortField"
                                : string.Empty));
            }
        }

        private void SetSortingFields(string sortingField, string sortingDirection)
        {
            _viewState.Add("SortField", sortingField);
            _viewState.Add("SortDirection", sortingDirection);
        }

        private void CreateShims()
        {
            var masterPage = new MasterPage();
            ShimSocial.AllInstances.MasterGet = (x) => masterPage;
            ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
            KmEntityFakes.ShimEncryption.GetCurrentByApplicationIDInt32 = (x) => ReflectionHelper.CreateInstance(typeof(KmEntity.Encryption));
            ShimHttpUtility.UrlDecodeString = (x) => DummyString;
            ShimEncryption.DecryptStringEncryption = (x, y) => DummyString;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            ShimPage.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                QueryStringGet = () => _queryString,
                UrlGet = () => new Uri("http://www.km.com?dummyQueryString=1")
            };
            var socialSummaryList = new List<SocialSummary> { ReflectionHelper.CreateInstance(typeof(SocialSummary)) };
            ShimSocialSummary.GetSocialSummaryByBlastIDInt32Int32 = (x, y) => socialSummaryList;
            ShimSocialSummary.GetSocialSummaryByCampaignItemIDInt32Int32 = (x, y) => socialSummaryList;
            ShimDirectory.ExistsString = (x) => false;
            ShimDirectory.CreateDirectoryString = (x) => new ShimDirectoryInfo();
            ShimFile.ExistsString = (x) => true;
            ShimFile.DeleteString = (x) => { };
            ShimFile.AppendTextString = (x) => new ShimStreamWriter();
            CreateSocialDetailShims(One);
            ShimHttpResponse.AllInstances.WriteFileString = (x, y) => { };
            ShimHttpResponse.AllInstances.End = (x) => { };
            ShimControl.AllInstances.ViewStateGet = (x) => _viewState;
            var socialShareChart = ReflectionHelper.CreateInstance(typeof(SocialShareChart));
            var socialShareChartList = new List<SocialShareChart> { socialShareChart };
            ShimSocialShareChart.GetChartPreviewsByBlastIDInt32Int32 = (x, y) => socialShareChartList;
            ShimSocialShareChart.GetChartPreviewsByCampaignItemIDInt32Int32 = (x, y) => socialShareChartList;
            ShimSocialShareChart.GetChartSharesByBlastIDInt32Int32 = (x, y) => socialShareChartList;
            ShimSocialShareChart.GetChartSharesByCampaignItemIDInt32Int32 = (x, y) => socialShareChartList;
        }

        private void CreateSocialDetailShims(string blastIdValue)
        {
            var socialDetailObject = ReflectionHelper.CreateInstance(typeof(SocialDetail));
            socialDetailObject.SocialMediaID = Convert.ToInt32(blastIdValue);
            var socialDetailList = new List<SocialDetail> { socialDetailObject };
            ShimSocialDetail.GetSocialDetailByBlastIDInt32Int32 = (x, y) => socialDetailList;
            ShimSocialDetail.GetSocialDetailByCampaignItemIDInt32Int32 = (x, y) => socialDetailList;
        }

        private void SetEcnParameters(string paramValue)
        {
            var kmCommonQueryString = ReflectionHelper.CreateInstance(typeof(QueryString));
            kmCommonQueryString.ParameterList = new List<QueryStringParameters>
            {
                new QueryStringParameters
                {
                    Parameter = ECNParameterTypes.BlastID,
                    ParameterValue = paramValue
                },
                new QueryStringParameters
                {
                    Parameter = ECNParameterTypes.CampaignItemID,
                    ParameterValue = paramValue
                },
                new QueryStringParameters
                {
                    Parameter = ECNParameterTypes.SocialMediaID,
                    ParameterValue = paramValue
                }
            };
            ShimQueryString.GetECNParametersString = (queryString) => kmCommonQueryString;
            var socialMediaObject = ReflectionHelper.CreateInstance(typeof(SocialMedia));
            socialMediaObject.SocialMediaID = Convert.ToInt32(paramValue);
            ShimSocialMedia.GetSocialMediaCanShare = () => new List<SocialMedia> { socialMediaObject };
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            var client = ReflectionHelper.CreateInstance(typeof(Client));
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true,
                CurrentClient = client
            };
            shimSession.Instance.CurrentCustomer = new Customer
            {
                CustomerID = 1
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}