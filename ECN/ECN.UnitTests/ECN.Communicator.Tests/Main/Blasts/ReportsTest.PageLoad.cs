using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Fakes;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.blastsmanager.Fakes;
using static ecn.communicator.blastsmanager.reports;
using ecn.communicator.MasterPages.Fakes;
using ECN.Common.Fakes;
using ecn.controls;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Entities.Communicator.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using BlastFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using PageCommunicator = ecn.communicator.MasterPages.Communicator;
using PlatformFakes = KM.Platform.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit tests for <see cref="reports"/>
    /// </summary>
    public partial class ReportsTest : BasePageTests
    {
        private const string MethoPageLoad = "Page_Load";
        private const string MethodLoadSocialSimpleGrid = "LoadSocialSimpleGrid";
        private const string MethodLoadCampaignItemFormData = "LoadCampaignItemFormData";
        private const string MethodLoadFormData = "LoadFormData";
        private const string MethodDownloadDeliveredEmails = "downloadDeliveredEmails";
        private const string MethodSocialSimpleGrid_RowDataBound = "SocialSimpleGrid_RowDataBound";
        private const string TestUser = "TestUser";
        private const string TestBlastN = "N";
        private const string TestBlastY = "Y";
        private const string DummyString = "dummyString";
        private const string EmailAddress = "EmailAddress";
        private const string Groups = "Groups";
        private const string Layout = "Layout";
        private const string LayoutID = "LayoutID";
        private const string EmailSubject = "EmailSubject";
        private const string EmailFromName = "EmailFromName";
        private const string SendTime = "SendTime";
        private const string FinishTime = "FinishTime";
        private const string EmailFrom = "EmailFrom";
        private const string DummyFilters = "Filter:";
        private const string DummySms = "sms";
        private const string LayoutValueString = "1";
        private const string StringOne = "1";
        private const int One = 1;
        private const int ZeroValue = 0;
        private const int LayoutId = 1;

        [Test]
        public void Page_Load_WhenDynamicContentExits_DCReportPanelShouldBeBeVisible()
        {
            // Arrange 
            Initialize();
            CreateShims();
            InitializeSession();

            // Act
            _privateObject.Invoke(MethoPageLoad, null, EventArgs.Empty);

            // Assert
            var panelDcReport = ReflectionHelper.GetFieldValue(_page, "pnlDCReport") as Panel;
            _page.ShouldSatisfyAllConditions(
                () => panelDcReport.ShouldNotBeNull(),
                () => panelDcReport.Visible.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_WhenDynamicContentDoesNotExists_DCReportPanelShouldNotBeVisible()
        {
            // Arrange 
            Initialize();
            CreateShims();
            InitializeSession();
            BlastFakes.ShimBlast.DynamicCotentExistsInt32 = (x) => false;

            // Act
            _privateObject.Invoke(MethoPageLoad, null, EventArgs.Empty);

            // Assert
            var panelDcReport = ReflectionHelper.GetFieldValue(_page, "pnlDCReport") as Panel;
            _page.ShouldSatisfyAllConditions(
                () => panelDcReport.ShouldNotBeNull(),
                () => panelDcReport.Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_WhenCustomerHasProductFeature_IspLinkShouldBeEnabled()
        {
            // Arrange 
            Initialize();
            CreateShims();
            InitializeSession();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => true;
            Shimreports.AllInstances.getBlastID = (x) => 1;

            // Act
            _privateObject.Invoke(MethoPageLoad, null, EventArgs.Empty);

            // Assert
            var lnkIsp = ReflectionHelper.GetFieldValue(_page, "lnkISP") as HyperLink;
            _page.ShouldSatisfyAllConditions(
                () => lnkIsp.ShouldNotBeNull(),
                () => lnkIsp.Enabled.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_WhenRequestBlastIdDoesNotExist_LoadCampaignItemFormData()
        {
            // Arrange 
            Initialize();
            CreateShims();
            InitializeSession();
            Shimreports.AllInstances.getBlastID = (x) => 0;
            Shimreports.AllInstances.LoadCampaignItemFormDataInt32 = (x, y) => { };

            // Act
            _privateObject.Invoke(MethoPageLoad, null, EventArgs.Empty);

            // Assert
            var lnkIsp = ReflectionHelper.GetFieldValue(_page, "lnkISP") as HyperLink;
            _page.ShouldSatisfyAllConditions(
                () => lnkIsp.ShouldNotBeNull(),
                () => lnkIsp.Enabled.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_WhenUserHasNotAccess_ThrowsSecurityException()
        {
            // Arrange 
            Initialize();
            CreateShims();
            InitializeSession();
            PlatformFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => false;

            //Act
            var exp = NUnit.Framework.Assert.Throws<TargetInvocationException>(() =>
                _privateObject.Invoke(MethoPageLoad, null, EventArgs.Empty));

            // Assert
            _page.ShouldSatisfyAllConditions(
               () => exp.ShouldNotBeNull(),
               () => exp.InnerException.ShouldBeOfType<ECN_Framework_Common.Objects.SecurityException>());
        }

        [Test]
        public void LoadCampaignItemFormData_WhenFilterListContainsItems_SuppressionLabelContainFilters()
        {
            // Arrange 
            Initialize();
            InitializeSession();
            CreateShims();
            CreateShims(MethodLoadCampaignItemFormData);

            // Act
            _privateObject.Invoke(MethodLoadCampaignItemFormData, One);

            // Assert
            var suppressionLabel = ReflectionHelper.GetFieldValue(_page, "SuppressionList") as Label;
            _page.ShouldSatisfyAllConditions(
               () => suppressionLabel.ShouldNotBeNull(),
               () => suppressionLabel.Text.ShouldNotBeNullOrWhiteSpace(),
               () => suppressionLabel.Text.ShouldContain(DummyFilters));
        }

        [Test]
        public void LoadCampaignItemFormData_WhenFilterListIsNull_SuppressionLabelDoesNotContainFilters()
        {
            // Arrange 
            Initialize();
            InitializeSession();
            CreateShims();
            CreateShims(MethodLoadCampaignItemFormData);
            var campaignItemSuppression = ReflectionHelper.CreateInstance(typeof(CampaignItemSuppression));
            campaignItemSuppression.Filters = null;
            var campaignItemSuppressionlist = new List<CampaignItemSuppression> { campaignItemSuppression };
            BlastFakes.ShimCampaignItemSuppression.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItemSuppressionlist;

            // Act
            _privateObject.Invoke(MethodLoadCampaignItemFormData, One);

            // Assert
            var suppressionLabel = ReflectionHelper.GetFieldValue(_page, "SuppressionList") as Label;
            _page.ShouldSatisfyAllConditions(
               () => suppressionLabel.ShouldNotBeNull(),
               () => suppressionLabel.Text.ShouldNotBeNullOrWhiteSpace(),
               () => suppressionLabel.Text.ShouldNotContain(DummyFilters));
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        public void LoadCampaignItemFormData_WhenFbAccountIsNull_ErrorIsDisplayed(int mediaId)
        {
            // Arrange s
            Initialize();
            InitializeSession();
            CreateShims();
            CreateShims(MethodLoadSocialSimpleGrid);
            var errorText = "Unable to get page name";
            var socialMediaIdd = ReflectionHelper.CreateInstance(typeof(SocialMedia));
            socialMediaIdd.SocialMediaID = mediaId;
            BlastFakes.ShimSocialMedia.GetSocialMediaByIDInt32 = (x) => socialMediaIdd;

            // Act
            _privateObject.Invoke(MethodLoadSocialSimpleGrid, One);

            // Assert
            var socialSimpleGrid = ReflectionHelper.GetFieldValue(_page, "SocialSimpleGrid") as ecnGridView;
            var socialSimpleGridList = (IOrderedEnumerable<SsmModel>)socialSimpleGrid.DataSource;
            _page.ShouldSatisfyAllConditions(
                 () => socialSimpleGrid.ShouldNotBeNull(),
                 () => socialSimpleGridList.ShouldNotBeNull(),
                 () => socialSimpleGridList.FirstOrDefault().Page.ShouldBe(errorText));
        }

        [TestCase(3)]
        [TestCase(4)]
        public void LoadCampaignItemFormData_WhenFbAccountAndLiAccountIsNotNull_SsmModelPageIsSet(int socialMediaId)
        {
            // Arrange 
            Initialize();
            InitializeSession();
            CreateShims();
            var errorText = "Unable to get page name";
            CreateShims(MethodLoadSocialSimpleGrid);
            var socialMediaIdd = ReflectionHelper.CreateInstance(typeof(SocialMedia));
            socialMediaIdd.SocialMediaID = socialMediaId;
            BlastFakes.ShimSocialMedia.GetSocialMediaByIDInt32 = (x) => socialMediaIdd;
            var fbAccount = ReflectionHelper.CreateInstance(typeof(SocialMediaHelper.FBAccount));
            fbAccount.id = StringOne;
            var userAccountList = new List<SocialMediaHelper.FBAccount> { fbAccount };
            ShimSocialMediaHelper.GetUserAccountsString = (x) => userAccountList;
            var campaignItemSocialMedia = ReflectionHelper.CreateInstance(typeof(CampaignItemSocialMedia));
            campaignItemSocialMedia.PageID = StringOne;
            var campaignItemSocialMediaList = new List<CampaignItemSocialMedia> { campaignItemSocialMedia };
            BlastFakes.ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (x) => campaignItemSocialMediaList;
            var liAccount = ReflectionHelper.CreateInstance(typeof(SocialMediaHelper.LIAccount));
            liAccount.id = StringOne;
            var liCompaniesList = new List<SocialMediaHelper.LIAccount> { liAccount };
            ShimSocialMediaHelper.GetLICompaniesString = (x) => liCompaniesList;

            // Act
            _privateObject.Invoke(MethodLoadSocialSimpleGrid, One);

            // Assert
            var socialSimpleGrid = ReflectionHelper.GetFieldValue(_page, "SocialSimpleGrid") as ecnGridView;
            var socialSimpleGridList = (IOrderedEnumerable<SsmModel>)socialSimpleGrid.DataSource;
            _page.ShouldSatisfyAllConditions(
                 () => socialSimpleGrid.ShouldNotBeNull(),
                 () => socialSimpleGridList.ShouldNotBeNull(),
                 () => socialSimpleGridList.FirstOrDefault().Page.ShouldNotBe(errorText));
        }

        [Test]
        public void LoadFormData_WhenTestBlastEqualsNAndBlastGroupIsNotNull_NavigateUrlIsNotEmpty()
        {
            // Arrange 
            Initialize();
            InitializeSession();
            CreateShims(MethodLoadFormData);
            var blast = ReflectionHelper.CreateInstance(typeof(Blast));
            blast.TestBlast = TestBlastN;
            blast.Group = new Group();
            blast.Layout = new Layout();
            var navigateUrl = "/ecn.communicator.mvc/Subscriber/Index/-1";

            // Act
            _privateObject.Invoke(MethodLoadFormData, blast);

            // Assert
            var hyperLink = ReflectionHelper.GetFieldValue(_page, "GroupTo") as HyperLink;
            _page.ShouldSatisfyAllConditions(
                () => hyperLink.ShouldNotBeNull(),
                () => hyperLink.NavigateUrl.Contains(navigateUrl));
        }

        [Test]
        public void LoadFormData_WhenTestBlastEqualsYAndBlastGroupIsNull_NavigateUrlIsEmpty()
        {
            // Arrange 
            Initialize();
            InitializeSession();
            CreateShims(MethodLoadFormData);
            var blast = ReflectionHelper.CreateInstance(typeof(Blast));
            blast.TestBlast = TestBlastY;
            blast.Group = null;
            blast.Layout = new Layout();
            blast.IgnoreSuppression = true;
            BlastFakes.ShimCampaignItemTestBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => new CampaignItemTestBlast();

            // Act
            _privateObject.Invoke(MethodLoadFormData, blast);

            // Assert
            var hyperLink = ReflectionHelper.GetFieldValue(_page, "GroupTo") as HyperLink;
            _page.ShouldSatisfyAllConditions(
                () => hyperLink.ShouldNotBeNull(),
                () => hyperLink.NavigateUrl.ShouldBeEmpty());
        }

        [Test]
        public void downloadDeliveredEmails_Success_EmailsAreDelivered()
        {
            // Arrange 
            Initialize();
            CreateShims();
            InitializeSession();
            ShimDirectory.CreateDirectoryString = (x) => new DirectoryInfo(DummyString);
            ShimFile.ExistsString = (x) => true;
            var datatable = new DataTable();
            datatable.Columns.Add(DummyString);
            datatable.Columns.Add(EmailAddress);
            datatable.Rows.Add('1', '2');
            ShimHttpResponse.AllInstances.WriteFileString = (x, y) => { };
            ShimBlastActivity.DownloadBlastReportDetailsInt32BooleanStringStringStringUserStringStringStringBoolean = (a, b, c, d, e, f, g, h, i, j) => datatable;
            var emailsDelivered = false;
            ShimHttpResponse.AllInstances.End = (x) =>
            {
                emailsDelivered = true;
            };

            // Act
            _privateObject.Invoke(MethodDownloadDeliveredEmails, null, EventArgs.Empty);

            // Assert
            emailsDelivered.ShouldBeTrue();
        }

        [TestCase(SocialMediaStatusType.Sent)]
        [TestCase(SocialMediaStatusType.Pending)]
        public void SocialSimpleGrid_RowDataBound_WhenStatusIsSentOrPending_LinkButtonStatusChanges(SocialMediaStatusType status)
        {
            // Arrange 
            var statusText = "return false;";
            var clickText = "OnClick";
            var gridViewRowObject = InitializeGrid(status.ToString()).Rows[0];
            var campaignItemSocialMedia = ReflectionHelper.CreateInstance(typeof(CampaignItemSocialMedia));
            var gridViewRowEventArgsObject = new GridViewRowEventArgs(gridViewRowObject);
            BlastFakes.ShimCampaignItemSocialMedia.GetByCampaignItemSocialMediaIDInt32 = (x) => campaignItemSocialMedia;
            BlastFakes.ShimSocialMediaErrorCodes.GetByErrorCodeInt32Int32Boolean = (x, y, z) => ReflectionHelper.CreateInstance(typeof(SocialMediaErrorCodes));
            var methodArgs = new object[] { null, gridViewRowEventArgsObject };

            // Act
            _privateObject.Invoke(MethodSocialSimpleGrid_RowDataBound, null, methodArgs);

            // Assert
            var linkButton = gridViewRowEventArgsObject.Row.FindControl("lbStatus") as LinkButton;
            _page.ShouldSatisfyAllConditions(
                () => linkButton.ShouldNotBeNull(),
                () => linkButton.Attributes.ShouldNotBeNull(),
                () => linkButton.Attributes[clickText].ShouldNotBeNull(),
                () => linkButton.Attributes[clickText].ShouldBe(statusText));
        }

        [TestCase(SocialMediaStatusType.Failed, 401, 1)]
        [TestCase(SocialMediaStatusType.Failed, 200, 1)]
        [TestCase(SocialMediaStatusType.Failed, 200, 3)]
        public void SocialSimpleGrid_RowDataBound_WhenStatusIsFailed_LinkButtonStatusChanges(SocialMediaStatusType status, int lastErrorCode, int socialMediaID)
        {
            // Arrange
            var statusText = "return false;";
            var clickText = "OnClick";
            var gridViewRowObject = InitializeGrid(status.ToString()).Rows[0];
            var campaignItemSocialMedia = ReflectionHelper.CreateInstance(typeof(CampaignItemSocialMedia));
            campaignItemSocialMedia.SocialMediaID = socialMediaID;
            campaignItemSocialMedia.LastErrorCode = lastErrorCode;
            var gridViewRowEventArgsObject = new GridViewRowEventArgs(gridViewRowObject);
            BlastFakes.ShimCampaignItemSocialMedia.GetByCampaignItemSocialMediaIDInt32 = (x) => campaignItemSocialMedia;
            BlastFakes.ShimSocialMediaErrorCodes.GetByErrorCodeInt32Int32Boolean = (x, mediaType, y) =>
            {
                return mediaType == 1 ? null : ReflectionHelper.CreateInstance(typeof(SocialMediaErrorCodes));
            };
            var methodArgs = new object[] { null, gridViewRowEventArgsObject };

            // Act
            _privateObject.Invoke(MethodSocialSimpleGrid_RowDataBound, null, methodArgs);

            // Assert
            var linkButton = gridViewRowEventArgsObject.Row.FindControl("lbStatus") as LinkButton;
            _page.ShouldSatisfyAllConditions(
                () => linkButton.ShouldNotBeNull(),
                () => linkButton.Attributes.ShouldNotBeNull(),
                () => linkButton.Attributes[clickText].ShouldNotBeNull(),
                () => linkButton.Attributes[clickText].ShouldBe(statusText));
        }

        private GridView InitializeGrid(string statusValue, int numberOfRows = 2)
        {
            var dataTable = new DataTable("grdSearch");
            dataTable.Columns.Add("ddlField", typeof(string));
            dataTable.Columns.Add("ddlLogic", typeof(string));
            dataTable.Columns.Add("ddlBoolean", typeof(string));
            dataTable.Columns.Add("txtSearch", typeof(string));
            for (var i = 0; i < numberOfRows; i++)
            {
                dataTable.Rows.Add(One, One, One, One);
            }
            var gvSearch = new GridView();
            gvSearch.DataSource = dataTable;
            gvSearch.DataBind();
            for (var i = 0; i < numberOfRows; i++)
            {
                gvSearch.Rows[i].Cells[0].Controls.Add(new LinkButton
                {
                    ID = "lbStatus",
                    CommandArgument = "1",
                    Text = statusValue
                });
            }
            return gvSearch;
        }

        private void Initialize()
        {
            ReflectionHelper.SetField(_page, "lnkISP", ReflectionHelper.CreateInstance(typeof(HyperLink)));
            ReflectionHelper.SetField(_page, "lnkDCTracking", ReflectionHelper.CreateInstance(typeof(HyperLink)));
            ReflectionHelper.SetField(_page, "pnlDCReport", ReflectionHelper.CreateInstance(typeof(Panel)));
            ReflectionHelper.SetField(_page, "lnkROITracking", ReflectionHelper.CreateInstance(typeof(HyperLink)));
            ReflectionHelper.SetField(_page, "ROITrkingSetupLNK", ReflectionHelper.CreateInstance(typeof(HyperLink)));
            ReflectionHelper.SetField(_page, "lnkConversionTracking", ReflectionHelper.CreateInstance(typeof(HyperLink)));
            ReflectionHelper.SetField(_page, "ConversionTrkingSetupLNK", ReflectionHelper.CreateInstance(typeof(HyperLink)));
            ShimBlast.AllInstances.BlastTypeGet = (x) => DummySms;
            ReflectionHelper.SetField(_page, "pnlEmail", new Panel());
            ReflectionHelper.SetField(_page, "pnlSMS", new Panel());
            ReflectionHelper.SetField(_page, "lnkConversionTracking", new HyperLink { Enabled = true });
            ReflectionHelper.SetField(_page, "lnkROITracking", new HyperLink { Enabled = true });
        }

        private void CreateShims(string methodName = "Default")
        {
            if (methodName == "Default")
            {
                BlastFakes.ShimBlast.DynamicCotentExistsInt32 = (x) => true;
                var masterPage = new PageCommunicator();
                Shimreports.AllInstances.MasterGet = (x) => masterPage;
                ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
                ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
                ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
                ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
                Shimreports.AllInstances.getUDFName = (x) => DummyString;
                Shimreports.AllInstances.getUDFData = (x) => DummyString;
                Shimreports.AllInstances.getCampaignItemID = (x) => One;
                Shimreports.AllInstances.getBlastID = (x) => One;
                ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => false;
                var blast = ReflectionHelper.CreateInstance(typeof(BlastRegular));
                BlastFakes.ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => blast;
                Shimreports.AllInstances.LoadFormDataBlast = (x, y) => { };
                PlatformFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
                ConfigurationManager.AppSettings["ValidateB4Tracking"] = DummyString;
                ConfigurationManager.AppSettings["Activity_DomainPath"] = DummyString;
                ConfigurationManager.AppSettings["SocialPreview"] = DummyString;
                ConfigurationManager.AppSettings["FBAPPID"] = DummyString;
                ShimPage.AllInstances.CacheGet = (x) => new Cache();
                ShimUser.GetByAccessKeyStringBoolean = (x, y) => new User();
                ShimBlastActivitySocial.InsertBlastActivitySocial = (x) => ZeroValue;
            }
            if (methodName == MethodLoadCampaignItemFormData)
            {
                Shimreports.AllInstances.LoadSocialSimpleGridInt32 = (x, y) => { };
                var masterPage = new PageCommunicator();
                Shimreports.AllInstances.MasterGet = (x) => masterPage;
                ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
                ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
                ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
                ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
                Shimreports.AllInstances.getBlastID = (x) => 0;
                var dataTable = new System.Data.DataTable();
                dataTable.Columns.Add(Layout);
                dataTable.Columns.Add(LayoutID);
                dataTable.Columns.Add(EmailSubject);
                dataTable.Columns.Add(EmailFromName);
                dataTable.Columns.Add(SendTime);
                dataTable.Columns.Add(FinishTime);
                dataTable.Columns.Add(Groups);
                dataTable.Columns.Add(EmailFrom);
                dataTable.Rows.Add('1', '2', '3', '4', '5', '6', '7', '8');
                BlastFakes.ShimBlast.GetGroupNamesByBlastsInt32Int32 = (x, y) => dataTable;
                var socialSummary = ReflectionHelper.CreateInstance(typeof(SocialSummary));
                var list = new List<SocialSummary> { socialSummary };
                ShimSocialSummary.GetSocialSummaryByCampaignItemIDInt32Int32 = (x, y) => list;
                BlastFakes.ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => new CampaignItem();
                var campaignItemSuppression = ReflectionHelper.CreateInstance(typeof(CampaignItemSuppression));
                Shimreports.AllInstances.loadReportsDataDataTable = (x, y) => { };
                BlastFakes.ShimSmartSegment.GetSmartSegmentByIDInt32 = (x) => ReflectionHelper.CreateInstance(typeof(SmartSegment));
                ShimBlastActivity.GetBlastReportDataByCampaignItemIDInt32 = (x) => new DataTable();
                var campaignItemSuppressionlist = new List<CampaignItemSuppression> { campaignItemSuppression };
                BlastFakes.ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => new Group();
                var dummycampaignItemBlastFilter1 = ReflectionHelper.CreateInstance(typeof(CampaignItemBlastFilter));
                var dummycampaignItemBlastFilter2 = ReflectionHelper.CreateInstance(typeof(CampaignItemBlastFilter));
                dummycampaignItemBlastFilter1.SmartSegmentID = null;
                var campaignItemBlastFilterlist = new List<CampaignItemBlastFilter> { dummycampaignItemBlastFilter1, dummycampaignItemBlastFilter2 };
                campaignItemSuppression.Filters = campaignItemBlastFilterlist;
                BlastFakes.ShimCampaignItemSuppression.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItemSuppressionlist;
                BlastFakes.ShimFilter.GetByFilterID_NoAccessCheckInt32 = (x) => new Filter();
            }
            if (methodName == MethodLoadSocialSimpleGrid)
            {
                BlastFakes.ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => new CampaignItem();
                var campaignItemSocialMedia = ReflectionHelper.CreateInstance(typeof(CampaignItemSocialMedia));
                BlastFakes.ShimSocialMedia.GetSocialMediaByIDInt32 = (x) => new SocialMedia();
                var campaignItemSocialMediaList = new List<CampaignItemSocialMedia> { campaignItemSocialMedia };
                BlastFakes.ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (x) => campaignItemSocialMediaList;
                BlastFakes.ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (x) => new SocialMediaAuth();
                var fbAccount = ReflectionHelper.CreateInstance(typeof(SocialMediaHelper.FBAccount));
                var socialMediaHelper = ReflectionHelper.CreateInstance(typeof(SocialMediaHelper));
                var userAccountList = new List<SocialMediaHelper.FBAccount> { fbAccount };
                ShimSocialMediaHelper.GetUserAccountsString = (x) => userAccountList;
                var liAccount = ReflectionHelper.CreateInstance(typeof(SocialMediaHelper.LIAccount));
                var liCompaniesList = new List<SocialMediaHelper.LIAccount> { liAccount };
                ShimSocialMediaHelper.GetLICompaniesString = (x) => liCompaniesList;
            }

            if (methodName == MethodLoadFormData)
            {
                var socialSummary = ReflectionHelper.CreateInstance(typeof(SocialSummary));
                var socialSummaryList = new List<SocialSummary> { socialSummary };
                ShimSocialSummary.GetSocialSummaryByBlastIDInt32Int32 = (x, y) => socialSummaryList;
                BlastFakes.ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => new CampaignItem();
                Shimreports.AllInstances.LoadSocialSimpleGridInt32 = (x, y) => { };
                BlastFakes.ShimCampaignItemBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => new CampaignItemBlast();
                var campaignItemBlastFilter = ReflectionHelper.CreateInstance(typeof(CampaignItemBlastFilter));
                var campaignItemBlastFilterList = new List<CampaignItemBlastFilter> { campaignItemBlastFilter };
                var campaignItemBlast = ReflectionHelper.CreateInstance(typeof(CampaignItemBlast));
                campaignItemBlast.Filters = campaignItemBlastFilterList;
                var campaignItemSuppression = ReflectionHelper.CreateInstance(typeof(CampaignItemSuppression));
                var campaignItemSuppressionList = new List<CampaignItemSuppression> { campaignItemSuppression };
                BlastFakes.ShimCampaignItemSuppression.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItemSuppressionList;
                BlastFakes.ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => new Group();
                campaignItemSuppression.Filters = campaignItemBlastFilterList;
                BlastFakes.ShimSmartSegment.GetSmartSegmentByIDInt32 = (x) => new SmartSegment();
                ReflectionHelper.SetField(_page, "lnkConversionTracking", new HyperLink() { Enabled = true });
                ReflectionHelper.SetField(_page, "lnkROITracking", new HyperLink() { Enabled = true });
                ShimBlastActivity.GetBlastReportDataInt32StringString = (x, y, z) => new DataTable();
                Shimreports.AllInstances.loadReportsDataDataTable = (x, y) => { };
            }
        }

        private void InitializeSession()
        {
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            var CustomerID = 1;
            var UserID = 1;
            var config = new NameValueCollection();
            var reqParams = new NameValueCollection();
            var queryString = new NameValueCollection();
            var dummyCustormer = ReflectionHelper.CreateInstance(typeof(Customer));
            var dummyUser = ReflectionHelper.CreateInstance(typeof(User));
            var authTkt = ReflectionHelper.CreateInstance(typeof(ECN_Framework_Entities.Application.AuthenticationTicket));
            var ecnSession = ReflectionHelper.CreateInstance(typeof(ECNSession));
            dummyCustormer.CustomerID = CustomerID;
            dummyUser.UserID = UserID;
            ReflectionHelper.SetField(authTkt, "CustomerID", CustomerID);
            ReflectionHelper.SetField(ecnSession, "CurrentUser", dummyUser);
            ReflectionHelper.SetField(ecnSession, "CurrentCustomer", dummyCustormer);
            HttpContext.Current = MockHelpers.FakeHttpContext();
            ShimECNSession.CurrentSession = () => ecnSession;
            ShimAuthenticationTicket.getTicket = () => authTkt;
            ShimUserControl.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimUserControl.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
            ShimConfigurationManager.AppSettingsGet = () => config;
            ShimHttpRequest.AllInstances.QueryStringGet = (h) => queryString;
            ShimPage.AllInstances.SessionGet = x => HttpContext.Current.Session;
            ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimHttpRequest.AllInstances.ParamsGet = (x) => reqParams;
            ShimControl.AllInstances.ParentGet = (control) => new Page();
            ShimGridView.AllInstances.DataBind = (x) => { };
        }

        private void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                            TryLinkFieldWithPage(obj, page);
                        }
                    }
                }
            }
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public |
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Static |
                                                                  BindingFlags.Instance);
                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                        Trace.TraceError($"Unable to set value as :{ex}");
                    }
                }
            }
        }
    }
}