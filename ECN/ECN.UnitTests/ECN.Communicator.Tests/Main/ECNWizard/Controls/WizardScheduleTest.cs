using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit.Fakes;
using ecn.communicator.main.blasts;
using ecn.communicator.main.blasts.Fakes;
using ecn.communicator.main.ECNWizard.Controls;
using ecn.communicator.main.ECNWizard.Controls.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Application;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECNBusiness = ECN_Framework_BusinessLayer.Application;
using ECNMasterPageFake = ecn.communicator.MasterPages.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthenticationTicket = ECN_Framework_Entities.Application.AuthenticationTicket;
using EntitiesGroup = ECN_Framework_Entities.Communicator.Group;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WizardScheduleTest
    {
        private const string MethodSetupScheduler = "SetupScheduler";
        private const string MethodValidateScheduleReportFields = "ValidateScheduleReportFields";
        private const string MethodCancelDetailEditsClick = "btnCancelDetailEdits_Click";
        private const string MethodSaveDetailEditsClick = "btnSaveReportDetails_Click";
        private const string MethodEditScheduleClick = "btnEditScheduleReport_Click";
        private const string MethodCheckQueryParamLength = "checkQueryParamLength";
        private const string MethodValidateEmailAddress = "ValidateEmailAddress";
        private const string MethodAddEmails = "AddEmails";
        private const string CampaignItemAB = "ab";
        private const string CampaignItemRegular = "regular";
        private const string DummyString = "dummyString";
        private const string InvalidCcs = "dummyString, dummyString, dummyString, dummyString, dummyString, dummyString";
        private const string IdFtpUrlTextBox = "txtFtpUrl";
        private const string IdFtpExportCheckBox = "chkFtpExport";
        private const string TestValue = "TestValue";
        public static readonly string[] DisplayName = { "Google", "ECN Conversion Tracking", "Omniture" };
        public static readonly string[] DisplayNameLinkParam = { "utm_source", "utm_medium", "utm_term", "utm_content", "utm_campaign", "eid" };
        public static readonly string[] DisplayNameCampaignLinkParam = { "Omniture1", "Omniture2", "Omniture3", "Omniture4", "Omniture5", "Omniture6", "Omniture7", "Omniture8", "Omniture9", "Omniture1", "Omniture10" };
        private NameValueCollection _queryString;
        private IDisposable _context;
        private Type _wizardScheduleType;
        private WizardSchedule _wizardScheduleObject;
        private int _sampleId;
        private object[] _methodArgs;
        private CheckBox _chkCacheBuster;
        private CheckBox _chkboxConvTracking;
        private CheckBox _chkOptOutMasterSuppression;
        private CheckBox _chkOptOutSpecificGroup;
        private CheckBox _chkboxGoogleAnalytics;
        private TextBox _txtCampaignSource;
        private CheckBox _chkboxOmnitureTracking;
        private DropDownList _drpCampaignSource;
        private GridView _gvOptOutGroups;
        private Panel _pnlGoogleAnalytics;
        private Panel _pnlOmniture;
        private UpdatePanel _pnlOptOutSpecificGroups;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase(InvalidCcs)]
        [TestCase(DummyString)]
        public void ValidateScheduleReportFields_WhenInvalidCCsForEmail_ReturnFalseAndErrorMessageIsShown(string emailCc)
        {
            //Arrange 
            Initialize();
            ShimEmail.IsValidEmailAddressString = (x) => false;
            SetField(_wizardScheduleObject, "txtAddCc", new TextBox { Text = emailCc });
            _methodArgs = new object[0];

            //Act
            var isValidReportFieleds = (bool)CallMethod(
                _wizardScheduleType,
                MethodValidateScheduleReportFields,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            AssertReportFields(isValidReportFieleds);
        }

        [Test]
        public void ValidateScheduleReportFields_WhenInvalidFtpFields_ReturnFalseAndErrorMessageIsShown()
        {
            //Arrange 
            Initialize();
            ShimEmail.IsValidEmailAddressString = (x) => true;
            SetField(_wizardScheduleObject, IdFtpExportCheckBox, new CheckBox { Checked = true });
            SetField(_wizardScheduleObject, IdFtpUrlTextBox, new TextBox { Text = string.Empty });
            _methodArgs = new object[] { };

            //Act
            var isValidReportFieleds = (bool)CallMethod(
                _wizardScheduleType,
                MethodValidateScheduleReportFields,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            AssertReportFields(isValidReportFieleds);
        }

        [Test]
        public void ValidateScheduleReportFields_WhenInvalidEmail_ReturnFalseAndErrorMessageIsShown()
        {
            //Arrange 
            Initialize();
            ShimEmail.IsValidEmailAddressString = (x) => false;
            _methodArgs = new object[] { };

            //Act
            var isValidReportFieleds = (bool)CallMethod(
                _wizardScheduleType,
                MethodValidateScheduleReportFields,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            AssertReportFields(isValidReportFieleds);
        }

        [Test]
        public void btnEditScheduleReport_Click_WhenFtpListNotContainItems_FtpPanelIsNotShown()
        {
            //Arrange 
            Initialize();
            var schedulePopupShown = false;
            var toBeExported = DummyString;
            ShimModalPopupExtender.AllInstances.Show = (x) =>
            {
                schedulePopupShown = true;
            };
            ShimWizardSchedule.AllInstances.scheduleFtpExportGet = (x) => false;
            SetField(_wizardScheduleObject, "ftpExports", new List<string> { toBeExported });
            SetField(_wizardScheduleObject, "lbFtpExports", new ListBox());
            _methodArgs = new object[] { null, EventArgs.Empty };

            //Act
            CallMethod(
                _wizardScheduleType,
                MethodEditScheduleClick,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            var ftpPanelShown = (GetField(_wizardScheduleObject, "pnlFtp") as Panel).Visible;
            _wizardScheduleObject.ShouldSatisfyAllConditions(
                () => ftpPanelShown.ShouldBeFalse(),
                () => schedulePopupShown.ShouldBeTrue());
        }

        [Test]
        public void btnEditScheduleReport_Click_WhenFtpListContainItems_FtpPanelIsShown()
        {
            //Arrange 
            Initialize();
            var schedulePopupShown = false;
            var toBeExported = DummyString;
            ShimModalPopupExtender.AllInstances.Show = (x) =>
            {
                schedulePopupShown = true;
            };
            SetField(_wizardScheduleObject, "scheduleFtpExport", true);
            SetField(_wizardScheduleObject, "ftpExports", new List<string> { toBeExported });
            SetField(_wizardScheduleObject, "lbFtpExports", new ListBox
            {
                Items =
                {
                    new ListItem
                    {
                        Value = toBeExported
                    }
                }
            });
            _methodArgs = new object[] { null, EventArgs.Empty };

            //Act
            CallMethod(
                _wizardScheduleType,
                MethodEditScheduleClick,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            var ftpPanelShown = (GetField(_wizardScheduleObject, "pnlFtp") as Panel).Visible;
            _wizardScheduleObject.ShouldSatisfyAllConditions(
                () => ftpPanelShown.ShouldBeTrue(),
                () => schedulePopupShown.ShouldBeTrue());
        }

        [Test]
        public void btnSaveReportDetails_Click_WhenCalled_HidesReportSchedulePopup()
        {
            //Arrange 
            Initialize();
            var schedulePopupHidden = false;
            ShimModalPopupExtender.AllInstances.Hide = (x) =>
            {
                schedulePopupHidden = true;
            };
            ShimWizardSchedule.AllInstances.ValidateScheduleReportFields = (x) => true;
            _methodArgs = new object[] { null, EventArgs.Empty };

            //Act
            CallMethod(
                _wizardScheduleType,
                MethodSaveDetailEditsClick,
                _methodArgs,
                _wizardScheduleObject);

            //Assert

            var errorMessage = GetField(_wizardScheduleObject, "divScheduleReportErrorMessage") as HtmlGenericControl;
            _wizardScheduleObject.ShouldSatisfyAllConditions(
                () => errorMessage.ShouldNotBeNull(),
                () => errorMessage.Visible.ShouldBeFalse(),
                () => schedulePopupHidden.ShouldBeTrue());
        }

        [Test]
        public void BtnCancelDetailEdits_Click_WhenCalled_HidesReportSchedulePopup()
        {
            //Arrange 
            Initialize();
            var schedulePopupHidden = false;
            ShimModalPopupExtender.AllInstances.Hide = (x) =>
            {
                schedulePopupHidden = true;
            };
            _methodArgs = new object[] { null, EventArgs.Empty };

            //Act
            CallMethod(
                _wizardScheduleType,
                MethodCancelDetailEditsClick,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            schedulePopupHidden.ShouldBeTrue();
        }

        [Test]
        public void SetupScheduler_WhenCustomerDoesNotHaveServiceFeatures_AllTrackingsAreDisabled()
        {
            //Arrange 
            Initialize(MethodSetupScheduler);
            var fillParamWithOmnitureDisplayNames = -1;
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(CreateCampaignItemLinkTracking(1, fillParamWithOmnitureDisplayNames, "1"));
            SetOmnitureDropDowns();
            _queryString.Add("campaignItemType", CampaignItemAB);
            ShimHttpRequest.AllInstances.QueryStringGet = (h) => _queryString;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (a, b, c) => false;
            _methodArgs = new object[] { true };

            //Act
            CallMethod(
                _wizardScheduleType,
                MethodSetupScheduler,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            var googleAnalyticsEnabled = (GetField(_wizardScheduleObject, "chkboxGoogleAnalytics") as CheckBox).Enabled;
            var conversionTrackingEnabled = (GetField(_wizardScheduleObject, "chkboxConvTracking") as CheckBox).Enabled;
            var omnitureTrackingEnabled = (GetField(_wizardScheduleObject, "chkboxOmnitureTracking") as CheckBox).Enabled;
            _wizardScheduleObject.ShouldSatisfyAllConditions(
                () => googleAnalyticsEnabled.ShouldBeFalse(),
                () => conversionTrackingEnabled.ShouldBeFalse(),
                () => omnitureTrackingEnabled.ShouldBeFalse());
        }

        [TestCase(CampaignItemAB)]
        [TestCase(CampaignItemRegular)]
        public void SetupScheduler_WhenLinkTrackingParamListContainsOmnitureDisplayNames_OmniturePanelAndBoxesAreVisibleAndContainsValues(string campaignItemType)
        {
            //Arrange 
            Initialize(MethodSetupScheduler);
            var fillParamWithOmnitureDisplayNames = -1;
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(CreateCampaignItemLinkTracking(1, fillParamWithOmnitureDisplayNames, "1"));
            SetOmnitureDropDowns();
            _queryString.Add("campaignItemType", campaignItemType);
            ShimHttpRequest.AllInstances.QueryStringGet = (h) => _queryString;
            _methodArgs = new object[] { true };

            //Act
            CallMethod(
                _wizardScheduleType,
                MethodSetupScheduler,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            AssertOmnituresAreEnabled();
        }

        [TestCase(CampaignItemAB)]
        [TestCase(CampaignItemRegular)]
        public void SetupScheduler_WhenLinkTrackingParamListNotContainsOmnitureDisplayNames_OmniturePanelAndBoxesAreHidden(string campaignItemType)
        {
            //Arrange 
            Initialize(MethodSetupScheduler);
            SetOmnitureDropDowns();
            var fillParamWithOmnitureDisplayNames = 6;
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(CreateCampaignItemLinkTracking(1, fillParamWithOmnitureDisplayNames, "1"));
            _queryString.Add("campaignItemType", campaignItemType);
            ShimHttpRequest.AllInstances.QueryStringGet = (h) => _queryString;
            _methodArgs = new object[] { true };

            //Act
            CallMethod(
                _wizardScheduleType,
                MethodSetupScheduler,
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            AssertOmnituresAreDisabled();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PrePopFromAB_WhenCampaignItemsHasCacheBuster_CacheBusterCheckBoxGetsItsValue(bool? cacheBusterEnabled)
        {
            //Arrange 
            Initialize();
            ShimCampaignItem.GetBySampleIDInt32EnumsCampaignItemTypeUserBoolean = (sampleID, x, y, z) =>
            {
                var campaignItemObject = CreateCampaignItem();
                SetProperty(campaignItemObject, "EnableCacheBuster", cacheBusterEnabled);
                return campaignItemObject;
            };

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            var isCacheBusterEnabled = (bool?)_chkCacheBuster.Checked;
            isCacheBusterEnabled.ShouldBe(cacheBusterEnabled);
        }

        [TestCase(6, true)]
        [TestCase(7, true)]
        [TestCase(1, false)]
        public void PrePopFromAB_WhenCampaignItemLinkTrackingWihtLTPID6OR7FoundInList_ChkboxConvTrackingIsChecked(int ltpId, bool isCheckedChkboxConvTracking)
        {
            //Arrange 
            Initialize();
            var campaignItemLinkTrackingObject = CreateCampaignItemLinkTracking(ltpId, 6, "1");
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(campaignItemLinkTrackingObject);

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            var isCacheBusterEnabled = _chkboxConvTracking.Checked;
            isCacheBusterEnabled.ShouldBe(isCheckedChkboxConvTracking);
        }

        [Test]
        public void PrePopFromAB_WhenCampaignItemHaveOptOutGroupList_pnlOptOutSpecificGroupsBecomesVisible()
        {
            //Arrange 
            Initialize();

            ShimCampaignItem.GetBySampleIDInt32EnumsCampaignItemTypeUserBoolean = (sampleID, x, y, z) => CreateCampaignItem();

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            _pnlOptOutSpecificGroups.Visible.ShouldBeTrue();
        }

        [Test]
        public void PrePopFromAB_WhenCampaignItemDoesNotHaveOptOutGroupList_pnlOptOutSpecificGroupsIsHidden()
        {
            //Arrange 
            Initialize();
            var campaignItemObject = CreateCampaignItem();
            campaignItemObject.OptOutGroupList = null;
            ShimCampaignItem.GetBySampleIDInt32EnumsCampaignItemTypeUserBoolean = (sampleID, x, y, z) => campaignItemObject;

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            _pnlOptOutSpecificGroups.Visible.ShouldBeFalse();
        }

        [Test]
        public void PrePopFromAB_WhenCampaignItemLinkTrackingListIsEmpty_pnlOmnitureAndpnlGoogleAnalyticsGoHidden()
        {
            //Arrange 
            Initialize();
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => new List<CampaignItemLinkTracking>();

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            _pnlGoogleAnalytics.ShouldSatisfyAllConditions(
                () => _pnlOmniture.Visible.ShouldBeFalse(),
                () => _pnlGoogleAnalytics.Visible.ShouldBeFalse());
        }

        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(4, 1)]
        [TestCase(5, 1)]
        public void PrePopFromAB_WhenCampaignItemLinkTrackingListHaveGoogleAnalytics_panelGoogleAnalyticsBecomesVisible(int ltpId, int ltIdForGoogleAnalytics)
        {
            //Arrange 
            Initialize();
            var campaignItemLinkTrackingObject = CreateCampaignItemLinkTracking(ltpId, 6, "1");
            var linkTrackingParamObject = CreateLinkTrackingParam(ltpId, ltIdForGoogleAnalytics);
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(campaignItemLinkTrackingObject);
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) => CreateLinkTrackingParamList(linkTrackingParamObject);

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            _pnlGoogleAnalytics.Visible.ShouldBeTrue();
        }

        [TestCase(8, 3)]
        [TestCase(9, 3)]
        [TestCase(10, 3)]
        [TestCase(11, 3)]
        [TestCase(12, 3)]
        [TestCase(13, 3)]
        [TestCase(14, 3)]
        [TestCase(15, 3)]
        [TestCase(16, 3)]
        [TestCase(17, 3)]
        public void PrePopFromAB_WhenCampaignItemLinkTrackingListHaveOmnitureSetup_panelOmnitureBecomesVisible(int ltpId, int ltIdForOmniture)
        {
            //Arrange 
            Initialize();
            var campaignItemLinkTrackingObject = CreateCampaignItemLinkTracking(ltpId, -1, "1");
            var linkTrackingParamObject = CreateLinkTrackingParam(ltpId, ltIdForOmniture);
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(campaignItemLinkTrackingObject);
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) => CreateLinkTrackingParamList(linkTrackingParamObject);

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            _pnlOmniture.Visible.ShouldBeTrue();
        }

        [TestCase(1, 100)]
        public void PrePopFromAB_WhenNotValidLTID_panelOmnitureAndPanelGoogleAnalyticsBecomesHidden(int ltpId, int ltId)
        {
            //Arrange 
            Initialize();
            var campaignItemLinkTrackingObject = CreateCampaignItemLinkTracking(ltpId, -1, "1");
            var linkTrackingParamObject = CreateLinkTrackingParam(ltpId, ltId);
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(campaignItemLinkTrackingObject);
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) => CreateLinkTrackingParamList(linkTrackingParamObject);

            //Act
            CallMethod(
                _wizardScheduleType,
                "PrePopFromAB",
                _methodArgs,
                _wizardScheduleObject);

            //Assert
            _pnlOmniture.ShouldSatisfyAllConditions(
                () => _pnlOmniture.Visible.ShouldBeFalse(),
                () => _pnlGoogleAnalytics.Visible.ShouldBeFalse());
        }

        [TestCase("fromName")]
        [TestCase("ftpUrl")]
        [TestCase("ftpUsername")]
        [TestCase("ftpExportFormat")]
        [TestCase("modalError")]
        public void StringProperties_DefaultValueSetValue_ReturnsEmptyStringOrSetValue(string propertyName)
        {
            // Arrange
            using(var testObject = new WizardSchedule())
            {
                var privateObject = new PrivateObject(testObject);
                Func<string> getPropertyValue = () => (string) privateObject.GetFieldOrProperty(propertyName);

                // Act
                var defaultValue = getPropertyValue();
                privateObject.SetFieldOrProperty(propertyName, TestValue);

                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => defaultValue.ShouldBeEmpty(),
                    () => getPropertyValue().ShouldBe(TestValue));
            }
        }

        [Test]
        public void CcList_DefaultValueSetValue_ExpectDefaultEmptyListAndSetValue()
        {
            // Arrange
            using (var testObject = new WizardSchedule())
            {
                // Act
                var defaultValue = testObject.ccList;
                testObject.ccList = new List<string> { TestValue };

                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => defaultValue.ShouldBeEmpty(),
                    () => testObject.ccList.Count.ShouldBe(1),
                    () => testObject.ccList[0].ShouldBe((TestValue)));
            }
        }

        private void Initialize(string method = "default")
        {
            _wizardScheduleType = typeof(WizardSchedule);
            _wizardScheduleObject = CreateInstance(_wizardScheduleType);
            _sampleId = 0;
            _methodArgs = new object[] { _sampleId };
            _chkCacheBuster = new CheckBox();
            _chkboxConvTracking = new CheckBox();
            _chkOptOutMasterSuppression = new CheckBox();
            _chkOptOutSpecificGroup = new CheckBox();
            _chkboxGoogleAnalytics = new CheckBox();
            _chkboxOmnitureTracking = new CheckBox();
            _txtCampaignSource = new TextBox();
            _gvOptOutGroups = new GridView();
            _pnlGoogleAnalytics = new Panel();
            _pnlOmniture = new Panel();
            _pnlOptOutSpecificGroups = CreateInstance(typeof(UpdatePanel));
            _pnlOptOutSpecificGroups.UpdateMode = UpdatePanelUpdateMode.Conditional;
            _drpCampaignSource = new DropDownList();
            CreateShims(method);
            InitializeControls(_wizardScheduleObject);
            SetDefaults();
            if (method == MethodSetupScheduler)
            {
                InitializeSession();
                _queryString = new NameValueCollection();
                SetField(_wizardScheduleObject, "BlastScheduler1", new BlastScheduler());
            }
        }

        private void SetDefaults()
        {
            SetField(_wizardScheduleObject, "chkCacheBuster", _chkCacheBuster);
            SetField(_wizardScheduleObject, "chkboxConvTracking", _chkboxConvTracking);
            SetField(_wizardScheduleObject, "chkboxGoogleAnalytics", _chkboxGoogleAnalytics);
            SetField(_wizardScheduleObject, "chkOptOutMasterSuppression", _chkOptOutMasterSuppression);
            SetField(_wizardScheduleObject, "chkOptOutSpecificGroup", _chkOptOutSpecificGroup);
            SetField(_wizardScheduleObject, "gvOptOutGroups", _gvOptOutGroups);
            SetField(_wizardScheduleObject, "pnlOptOutSpecificGroups", _pnlOptOutSpecificGroups);
            SetField(_wizardScheduleObject, "pnlGoogleAnalytics", _pnlGoogleAnalytics);
            SetField(_wizardScheduleObject, "pnlOmniture", _pnlOmniture);
            SetField(_wizardScheduleObject, "drpCampaignSource", _drpCampaignSource);
            SetField(_wizardScheduleObject, "txtCampaignSource", _txtCampaignSource);
            SetField(_wizardScheduleObject, "chkboxOmnitureTracking", _chkboxOmnitureTracking);
            SetField(_wizardScheduleObject, "txtCampaignMedium", new TextBox());
            SetField(_wizardScheduleObject, "txtCampaignTerm", new TextBox());
            SetField(_wizardScheduleObject, "txtCampaignContent", new TextBox());
            SetField(_wizardScheduleObject, "txtCampaignName", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture1", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture2", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture3", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture4", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture5", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture6", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture7", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture8", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture9", new TextBox());
            SetField(_wizardScheduleObject, "txtOmniture10", new TextBox());
            SetField(_wizardScheduleObject, "ddlOmniture1", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture2", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture3", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture4", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture5", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture6", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture7", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture8", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture9", new DropDownList());
            SetField(_wizardScheduleObject, "ddlOmniture10", new DropDownList());
            SetField(_wizardScheduleObject, "drpCampaignTerm", new DropDownList());
            SetField(_wizardScheduleObject, "drpCampaignContent", new DropDownList());
            SetField(_wizardScheduleObject, "drpCampaignMedium", new DropDownList());
            SetField(_wizardScheduleObject, "drpCampaignName", new DropDownList());
        }

        private void CreateShims(string method = "default")
        {
            ShimCampaignItem.GetBySampleIDInt32EnumsCampaignItemTypeUserBoolean = (sampleID, x, y, z) => CreateCampaignItem();
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(CreateCampaignItemLinkTracking(1, 6, "1"));
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) => CreateLinkTrackingParamList(CreateLinkTrackingParam(1, 1));
            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (x, y, z) =>
            {
                var campaignItemBlastmObject = CreateInstance(typeof(CampaignItemBlast));
                var campaignItemBlastList = new List<CampaignItemBlast>();
                campaignItemBlastList.Add(campaignItemBlastmObject);
                return campaignItemBlastList;
            };
            ShimGroup.GetByGroupIDInt32User = (x, y) => CreateInstance(typeof(EntitiesGroup));
            ConfigurationManager.AppSettings["KMCommon_Application"] = "1";
            ConfigurationManager.AppSettings["IsDemo"] = "1";
            ShimAuthenticationTicket.getTicket = () =>
            {
                AuthenticationTicket authTkt = CreateInstance(typeof(AuthenticationTicket));
                SetField(authTkt, "CustomerID", 1);
                return authTkt;
            };
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            ShimECNSession.CurrentSession = () =>
            {
                ECNBusiness.ECNSession ecnSession = CreateInstance(typeof(ECNBusiness.ECNSession));
                SetField(ecnSession, "CustomerID", 1);
                SetField(ecnSession, "BaseChannelID", 1);
                return ecnSession;
            };
            ECNMasterPageFake.ShimCommunicator.AllInstances.UserSessionGet = (x) =>
            {
                return CreateInstance(typeof(ECNBusiness.ECNSession));
            };
            if (method == MethodSetupScheduler)
            {
                CreateShimsForSetupScheduler();
            }
        }

        private dynamic CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            return ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetField(obj, fieldName);
        }

        private void SetField(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetField(obj, fieldName, value);
        }

        private void SetProperty(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetProperty(obj, fieldName, value);
        }

        private dynamic GetProperty(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetPropertyValue(obj, fieldName);
        }

        private void SetSessionVariable(string name, object value)
        {
            HttpContext.Current.Session.Add(name, value);
        }

        private CampaignItem CreateCampaignItem()
        {
            var campaignItemOptOutGroupObject = CreateInstance(typeof(CampaignItemOptOutGroup));
            var temp = (int?)1;
            SetField(campaignItemOptOutGroupObject, "LTPID", temp);
            SetProperty(campaignItemOptOutGroupObject, "LTPID", temp);
            var campaignItemOptOutGroupList = new List<CampaignItemOptOutGroup>();
            campaignItemOptOutGroupList.Add(campaignItemOptOutGroupObject);
            var campaignItemObject = CreateInstance(typeof(CampaignItem));
            SetProperty(campaignItemObject, "OptOutGroupList", campaignItemOptOutGroupList);
            return campaignItemObject;
        }

        private List<CampaignItemLinkTracking> CreateCampaignItemTrackingList(CampaignItemLinkTracking campaignItemLinkTrackingObject)
        {
            var campaignItemLinkTrackingList = new List<CampaignItemLinkTracking>();
            campaignItemLinkTrackingList.Add(campaignItemLinkTrackingObject);
            return campaignItemLinkTrackingList;
        }

        private CampaignItemLinkTracking CreateCampaignItemLinkTracking(int LTPID, int LTPOID, string CustomValue)
        {
            var campaignItemLinkTrackingObject = CreateInstance(typeof(CampaignItemLinkTracking));
            campaignItemLinkTrackingObject.LTPID = LTPID;
            campaignItemLinkTrackingObject.LTPOID = LTPOID;
            campaignItemLinkTrackingObject.CustomValue = CustomValue;
            return campaignItemLinkTrackingObject;
        }

        private LinkTrackingParam CreateLinkTrackingParam(int ltpId, int ltId)
        {
            var linkTrackingParamObject = CreateInstance(typeof(LinkTrackingParam));
            linkTrackingParamObject.LTPID = ltpId;
            linkTrackingParamObject.LTID = ltId;
            return linkTrackingParamObject;
        }

        private List<LinkTrackingParam> CreateLinkTrackingParamList(LinkTrackingParam linkTrackingParamObject)
        {
            var linkTrackingParamList = new List<LinkTrackingParam>();
            linkTrackingParamList.Add(linkTrackingParamObject);
            return linkTrackingParamList;
        }

        private void CreateShimsForSetupScheduler()
        {
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (x, y, z) => CreateInstance(typeof(CampaignItem));
            ShimBlastScheduler.AllInstances.SetupWizardBoolean = (x, y) => { };
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (a, b, c) => true;
            ShimLinkTracking.GetAll = () => { return CreateLinkTrackingObject(); };
            ShimCustomerLinkTracking.GetByCustomerIDInt32 = (x) => CreateCustomerLinkTrackingList();
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) => CreateLinkTrackingParamListWithDisplayNames(CreateLinkTrackingParam(1, 1));
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (x, y) => CreateCampaignItemTrackingList(CreateCampaignItemLinkTracking(1, -1, "1"));
        }

        private List<LinkTrackingParam> CreateLinkTrackingParamListWithDisplayNames(LinkTrackingParam linkTrackingParamObject)
        {
            var linkTrackingParamList = new List<LinkTrackingParam>();
            foreach (var item in DisplayNameLinkParam)
            {
                linkTrackingParamList.Add(new LinkTrackingParam
                {
                    IsActive = true,
                    LTPID = 1,
                    LTID = 1,
                    DisplayName = item
                });
            }
            foreach (var item in DisplayNameCampaignLinkParam)
            {
                linkTrackingParamList.Add(new LinkTrackingParam
                {
                    IsActive = true,
                    LTPID = 1,
                    LTID = 1,
                    DisplayName = item
                });
            }
            return linkTrackingParamList;
        }

        private void InitializeControls(object page)
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
            if (page is UserControl)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to set field value: {0}", ex);
                    }
                }
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
            var dummyCustormer = CreateInstance(typeof(Customer));
            var dummyUser = CreateInstance(typeof(User));
            var authTkt = CreateInstance(typeof(AuthenticationTicket));
            var ecnSession = CreateInstance(typeof(ECNSession));
            dummyCustormer.CustomerID = CustomerID;
            dummyUser.UserID = UserID;
            SetField(authTkt, "CustomerID", CustomerID);
            SetField(ecnSession, "CurrentUser", dummyUser);
            SetField(ecnSession, "CurrentCustomer", dummyCustormer);
            HttpContext.Current = MockHelpers.FakeHttpContext();
            ShimECNSession.CurrentSession = () => ecnSession;
            ShimAuthenticationTicket.getTicket = () => authTkt;
            ShimUserControl.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimUserControl.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
            ShimConfigurationManager.AppSettingsGet = () => config;
            ShimPage.AllInstances.SessionGet = x => HttpContext.Current.Session;
            ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimHttpRequest.AllInstances.ParamsGet = (x) => reqParams;
            ShimControl.AllInstances.ParentGet = (control) => new Page();
            ShimGridView.AllInstances.DataBind = (x) => { };
            InitializeControls(_wizardScheduleObject);
            SetSessionVariable("CanSendAll", false);
        }

        private void SetOmnitureDropDowns()
        {
            var omnitureDropDown = new DropDownList
            {
                Items =
                {
                    new ListItem { Value = "-1" }
                }
            };
            SetField(_wizardScheduleObject, "ddlOmniture1", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture2", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture3", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture4", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture5", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture6", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture7", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture8", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture9", omnitureDropDown);
            SetField(_wizardScheduleObject, "ddlOmniture10", omnitureDropDown);
        }

        private void AssertOmnituresAreEnabled()
        {
            var omnitureTrackingEnabled = (GetField(_wizardScheduleObject, "chkboxOmnitureTracking") as CheckBox).Checked;
            var omniturePanelVisible = (GetField(_wizardScheduleObject, "pnlOmniture") as Panel).Visible;
            var omniture1 = GetField(_wizardScheduleObject, "txtOmniture1") as TextBox;
            var omniture2 = GetField(_wizardScheduleObject, "txtOmniture2") as TextBox;
            var omniture3 = GetField(_wizardScheduleObject, "txtOmniture3") as TextBox;
            var omniture4 = GetField(_wizardScheduleObject, "txtOmniture4") as TextBox;
            var omniture5 = GetField(_wizardScheduleObject, "txtOmniture5") as TextBox;
            var omniture6 = GetField(_wizardScheduleObject, "txtOmniture6") as TextBox;
            var omniture7 = GetField(_wizardScheduleObject, "txtOmniture7") as TextBox;
            var omniture8 = GetField(_wizardScheduleObject, "txtOmniture8") as TextBox;
            var omniture9 = GetField(_wizardScheduleObject, "txtOmniture9") as TextBox;
            var omniture10 = GetField(_wizardScheduleObject, "txtOmniture10") as TextBox;
            _wizardScheduleObject.ShouldSatisfyAllConditions(
                () => omniture1.ShouldNotBeNull(),
                () => omniture2.ShouldNotBeNull(),
                () => omniture3.ShouldNotBeNull(),
                () => omniture4.ShouldNotBeNull(),
                () => omniture5.ShouldNotBeNull(),
                () => omniture6.ShouldNotBeNull(),
                () => omniture7.ShouldNotBeNull(),
                () => omniture8.ShouldNotBeNull(),
                () => omniture9.ShouldNotBeNull(),
                () => omniture10.ShouldNotBeNull(),
                () => omnitureTrackingEnabled.ShouldBeTrue(),
                () => omniturePanelVisible.ShouldBeTrue(),
                () => omniture1.Visible.ShouldBeTrue(),
                () => omniture2.Visible.ShouldBeTrue(),
                () => omniture3.Visible.ShouldBeTrue(),
                () => omniture4.Visible.ShouldBeTrue(),
                () => omniture5.Visible.ShouldBeTrue(),
                () => omniture6.Visible.ShouldBeTrue(),
                () => omniture7.Visible.ShouldBeTrue(),
                () => omniture8.Visible.ShouldBeTrue(),
                () => omniture9.Visible.ShouldBeTrue(),
                () => omniture10.Visible.ShouldBeTrue(),
                () => omniture1.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture2.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture3.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture4.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture5.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture6.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture7.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture8.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture9.Text.ShouldNotBeNullOrWhiteSpace(),
                () => omniture10.Text.ShouldNotBeNullOrWhiteSpace());
        }

        private void AssertOmnituresAreDisabled()
        {
            var omniture1 = GetField(_wizardScheduleObject, "txtOmniture1") as TextBox;
            var omniture2 = GetField(_wizardScheduleObject, "txtOmniture2") as TextBox;
            var omniture3 = GetField(_wizardScheduleObject, "txtOmniture3") as TextBox;
            var omniture4 = GetField(_wizardScheduleObject, "txtOmniture4") as TextBox;
            var omniture5 = GetField(_wizardScheduleObject, "txtOmniture5") as TextBox;
            var omniture6 = GetField(_wizardScheduleObject, "txtOmniture6") as TextBox;
            var omniture7 = GetField(_wizardScheduleObject, "txtOmniture7") as TextBox;
            var omniture8 = GetField(_wizardScheduleObject, "txtOmniture8") as TextBox;
            var omniture9 = GetField(_wizardScheduleObject, "txtOmniture9") as TextBox;
            var omniture10 = GetField(_wizardScheduleObject, "txtOmniture10") as TextBox;
            _wizardScheduleObject.ShouldSatisfyAllConditions(
                () => omniture1.Visible.ShouldBeFalse(),
                () => omniture2.Visible.ShouldBeFalse(),
                () => omniture3.Visible.ShouldBeFalse(),
                () => omniture4.Visible.ShouldBeFalse(),
                () => omniture5.Visible.ShouldBeFalse(),
                () => omniture6.Visible.ShouldBeFalse(),
                () => omniture7.Visible.ShouldBeFalse(),
                () => omniture8.Visible.ShouldBeFalse(),
                () => omniture9.Visible.ShouldBeFalse(),
                () => omniture10.Visible.ShouldBeFalse());
        }

        private List<CustomerLinkTracking> CreateCustomerLinkTrackingList()
        {
            var customerLinkTrackingList = new List<CustomerLinkTracking>();
            foreach (var item in DisplayName)
            {
                customerLinkTrackingList.Add(new CustomerLinkTracking
                {
                    IsActive = true,
                    LTPID = 1,
                    LTPOID = 1
                });
            }
            return customerLinkTrackingList;
        }

        private List<LinkTracking> CreateLinkTrackingObject()
        {
            var linkTrackingList = new List<LinkTracking>();
            foreach (var item in DisplayName)
            {
                linkTrackingList.Add(new LinkTracking
                {
                    DisplayName = item,
                    LTID = 1,
                    IsActive = true
                });
            }
            return linkTrackingList;
        }

        private void AssertReportFields(bool isValidReportFieleds)
        {
            var errorMessage = GetField(_wizardScheduleObject, "divScheduleReportErrorMessage") as HtmlGenericControl;
            _wizardScheduleObject.ShouldSatisfyAllConditions(
                () => errorMessage.ShouldNotBeNull(),
                () => isValidReportFieleds.ShouldBeFalse(),
                () => errorMessage.Visible.ShouldBeTrue(),
                () => errorMessage.InnerHtml.ShouldNotBeNullOrWhiteSpace());
        }
    }
}
