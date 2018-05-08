using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ecn.communicator.main.blasts;
using ecn.communicator.main.blasts.Fakes;
using ecn.communicator.main.ECNWizard.Controls;
using ecn.communicator.main.ECNWizard.Controls.Fakes;
using ecn.communicator.main.ECNWizard.Group;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    /// <summary>
    /// Unit test for <see cref="WizardSchedule"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WizardScheduleTestButtonScheduleClick : PageHelper
    {
        private const string ButtonScheduleClick = "btnSchedule_Click";
        private const string DropDownOmniDefault = "ddlOmniture";
        private const string CheckBoxOmnitureTracking = "chkboxOmnitureTracking";
        private const string DefaultText = "Unit test";
        private const string TextOmniture = "txtOmniture";
        private const string DefaultCampaignText = "ab";
        private const string BlastScheduler = "BlastScheduler1";
        private const string TestgroupExplorer = "testgroupExplorer1";
        private const string CheckQueryParamLength = "checkQueryParamLength";
        private const string EmailPreview = "cbEmailPreview";
        private const string HiddenFieldLicenseExceed = "hfLicenseExceed";
        private const string RbTestNew = "rbTestNew";
        private const string TextGroupName = "txtGroupName";
        private const string TextEmailAddress = "txtEmailAddress";
        private const string Omniture = "Omniture";
        public static readonly string[] PageCheckBoxBoxControl = { "chkboxGoogleAnalytics", "chkboxConvTracking", "chkOptOutSpecificGroup", "chkOptOutMasterSuppression", "chkCacheBuster", "chkGoToChampion" };
        public static readonly string[] PageTextBoxControl = { "txtCampaignSource", "txtCampaignContent", "txtCampaignMedium", "txtCampaignName", "txtCampaignTerm" };
        public static readonly string[] DisplayName = { "Google", "ECN Conversion Tracking", "Omniture" };
        public static readonly string[] DropDowmList = { "drpCampaignSource", "drpCampaignMedium", "drpCampaignTerm", "drpCampaignContent", "drpCampaignName" };
        private string _responseSting = string.Empty;
        private WizardSchedule _wizardSchedule;
        private BlastScheduler _blastScheduler;
        private testGroupExplorer _testGroupExplorer;
        private PrivateObject _privateObject;
        private PrivateObject _privateObjectBlastScheduler;
        private PrivateObject _privateObjectTestGroupExplorer;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _wizardSchedule = new WizardSchedule();
            _privateObject = new PrivateObject(_wizardSchedule);
            InitializeAllControls(_wizardSchedule);

            _blastScheduler = new BlastScheduler();
            InitializeAllControls(_blastScheduler);
            _privateObjectBlastScheduler = new PrivateObject(_blastScheduler);
            _privateObject.SetFieldOrProperty(BlastScheduler, _blastScheduler);

            _testGroupExplorer = new testGroupExplorer();
            InitializeAllControls(_testGroupExplorer);
            _privateObjectTestGroupExplorer = new PrivateObject(_testGroupExplorer);
            _privateObject.SetFieldOrProperty(TestgroupExplorer, _testGroupExplorer);
        }

        [TestCase(true, true, 6)]
        [TestCase(true, false, 6)]
        [TestCase(false, false, 6)]
        [TestCase(true, true, 1)]
        public void ButtonScheduleClick_BlastSetupInfoIsNotNullAndIsTestBlastIsFalse_UpdateControlValues(bool checkBoxvaue, bool allowCustOverride, int selectedIndex)
        {
            // Arrange
            CreatePageFakeObject(allowCustOverride);
            ShimCurrentUser(true);
            SetPageControlValue(checkBoxvaue);
            BindDropDown(selectedIndex);
            var parameters = new object[] { this, EventArgs.Empty };
            Get<CheckBox>(_privateObject, CheckBoxOmnitureTracking).Checked = checkBoxvaue;

            // Act
            _privateObject.Invoke(ButtonScheduleClick, parameters);

            //Assert
            _responseSting.ShouldNotBeNullOrEmpty();
        }


        [TestCase(true, true, 6, "rbTestExisting", true)]
        [TestCase(true, true, 6, "rbTestNew", true)]
        public void ButtonScheduleClick_BlastSetupInfoIsNullAndIsTestBlastIsTrue_UpdateControlValues(bool checkBoxvaue, bool allowCustOverride, int selectedIndex, string controlName, bool controlValue)
        {
            // Arrange
            CreatePageFakeObject(allowCustOverride, true);
            ShimCurrentUser(true);
            SetPageControlValue(checkBoxvaue);
            BindDropDown(selectedIndex);
            var parameters = new object[] { this, EventArgs.Empty };
            Get<CheckBox>(_privateObject, CheckBoxOmnitureTracking).Checked = checkBoxvaue;
            Get<CheckBox>(_privateObjectBlastScheduler, EmailPreview).Checked = true;
            Get<HiddenField>(_privateObjectTestGroupExplorer, HiddenFieldLicenseExceed).Value = string.Empty;
            Get<TextBox>(_privateObject, TextGroupName).Text = DefaultText;
            Get<TextBox>(_privateObject, TextEmailAddress).Text = DefaultText;
            Get<CheckBox>(_privateObject, controlName).Checked = controlValue;
            FindServerControl();

            // Act
            _privateObject.Invoke(ButtonScheduleClick, parameters);

            //Assert
            _responseSting.ShouldNotBeNullOrEmpty();
        }

        [TestCase(true, true, 6, "txtGroupName", "")]
        [TestCase(true, true, 6, "txtEmailAddress", "")]
        public void ButtonScheduleClick_BlastSetupInfoIsNullAndIsTestBlastIsTrueAndControlAreEmpty_UpdateControlValues(bool checkBoxvaue, bool allowCustOverride, int selectedIndex, string controlName, string controlValue)
        {
            // Arrange
            var resultMessage = string.Empty;
            CreatePageFakeObject(allowCustOverride, true);
            ShimCurrentUser(true);
            SetPageControlValue(checkBoxvaue);
            BindDropDown(selectedIndex);
            var parameters = new object[] { this, EventArgs.Empty };
            Get<CheckBox>(_privateObject, CheckBoxOmnitureTracking).Checked = checkBoxvaue;
            Get<CheckBox>(_privateObjectBlastScheduler, EmailPreview).Checked = true;
            Get<HiddenField>(_privateObjectTestGroupExplorer, HiddenFieldLicenseExceed).Value = string.Empty;
            Get<TextBox>(_privateObject, controlName).Text = controlValue;
            Get<CheckBox>(_privateObject, RbTestNew).Checked = true;
            FindServerControl();
            ShimWizardSchedule.AllInstances.throwECNExceptionString = (sender, messeage) => { resultMessage = messeage; };

            // Act
            _privateObject.Invoke(ButtonScheduleClick, parameters);

            //Assert
            resultMessage.ShouldNotBeNullOrEmpty();
        }

        [TestCase(true, true, "ddlOmniture1", 6)]
        [TestCase(true, true, "ddlOmniture2", 6)]
        [TestCase(true, true, "ddlOmniture3", 6)]
        [TestCase(true, true, "ddlOmniture4", 6)]
        [TestCase(true, true, "ddlOmniture5", 6)]
        [TestCase(true, true, "ddlOmniture6", 6)]
        [TestCase(true, true, "ddlOmniture7", 6)]
        [TestCase(true, true, "ddlOmniture8", 6)]
        [TestCase(true, true, "ddlOmniture9", 6)]
        [TestCase(true, true, "ddlOmniture10", 6)]

        public void ButtonScheduleClick_OmitureDropDownSelectedIndexIsZero_ThrowException(bool checkBoxvaue, bool allowCustOverride, string controlName, int selectedIndex)
        {
            // Arrange
            var resultMessage = string.Empty;
            CreatePageFakeObject(allowCustOverride);
            ShimCurrentUser(true);
            SetPageControlValue(checkBoxvaue);
            BindDropDown(selectedIndex);
            var parameters = new object[] { this, EventArgs.Empty };
            Get<CheckBox>(_privateObject, CheckBoxOmnitureTracking).Checked = checkBoxvaue;
            Get<DropDownList>(_privateObject, controlName).SelectedIndex = 0;
            ShimWizardSchedule.AllInstances.throwECNExceptionString = (sender, messeage) => { resultMessage = messeage; };

            // Act
            _privateObject.Invoke(ButtonScheduleClick, parameters);

            //Assert
            resultMessage.ShouldNotBeNullOrEmpty();
        }

        [TestCase(true, true, "txtOmniture1", 1, true)]
        [TestCase(true, true, "txtOmniture2", 1, true)]
        [TestCase(true, true, "txtOmniture3", 1, true)]
        [TestCase(true, true, "txtOmniture4", 1, true)]
        [TestCase(true, true, "txtOmniture5", 1, true)]
        [TestCase(true, true, "txtOmniture6", 1, true)]
        [TestCase(true, true, "txtOmniture7", 1, true)]
        [TestCase(true, true, "txtOmniture8", 1, true)]
        [TestCase(true, true, "txtOmniture9", 1, true)]
        [TestCase(true, true, "txtOmniture10", 1, true)]
        [TestCase(true, true, "checkQueryParamLength", 1, false)]

        public void ButtonScheduleClick_OmitureTextBoxIsEmpty_ThrowException(bool checkBoxvaue, bool allowCustOverride, string controlName, int selectedIndex, bool isQueryStringExceed)
        {
            // Arrange
            var resultMessage = string.Empty;
            CreatePageFakeObject(allowCustOverride);
            ShimCurrentUser(true);
            SetPageControlValue(checkBoxvaue);
            BindDropDown(selectedIndex);
            var parameters = new object[] { this, EventArgs.Empty };
            Get<CheckBox>(_privateObject, CheckBoxOmnitureTracking).Checked = checkBoxvaue;
            if (controlName != CheckQueryParamLength)
            {
                Get<TextBox>(_privateObject, controlName).Text = string.Empty;
            }
            ShimWizardSchedule.AllInstances.throwECNExceptionString = (sender, messeage) => { resultMessage = messeage; };
            ShimWizardSchedule.AllInstances.CheckQueryParamLength = (x) => { return isQueryStringExceed; };

            // Act
            _privateObject.Invoke(ButtonScheduleClick, parameters);

            //Assert
            resultMessage.ShouldNotBeNullOrEmpty();
        }

        [TestCase(true, true, "drpCampaignMedium", 7)]
        [TestCase(true, true, "drpCampaignMedium", 6)]
        [TestCase(true, true, "drpCampaignSource", 6)]
        [TestCase(true, true, "drpCampaignContent", 6)]
        [TestCase(true, true, "drpCampaignMedium", 6)]
        [TestCase(true, true, "drpCampaignName", 6)]
        [TestCase(true, true, "drpCampaignTerm", 6)]
        public void ButtonScheduleClick_GoogleAnalyticsIsEnabledAndControlHaveEmptyValue_ThrowException(bool checkBoxvaue, bool allowCustOverride, string controlName, int selectedIndex)
        {
            // Arrange
            var resultMessage = string.Empty;
            CreatePageFakeObject(allowCustOverride);
            ShimCurrentUser(true);
            SetPageControlValue(checkBoxvaue, false);
            BindDropDown(selectedIndex);
            var parameters = new object[] { this, EventArgs.Empty };
            Get<CheckBox>(_privateObject, CheckBoxOmnitureTracking).Checked = checkBoxvaue;
            Get<DropDownList>(_privateObject, controlName).SelectedIndex = selectedIndex;
            ShimWizardSchedule.AllInstances.throwECNExceptionString = (sender, messeage) => { resultMessage = messeage; };

            // Act
            _privateObject.Invoke(ButtonScheduleClick, parameters);

            //Assert
            resultMessage.ShouldNotBeNullOrEmpty();
        }

        private void FindServerControl()
        {
            ShimControl.AllInstances.FindControlString = (sender, id) =>
            {
                if (id == HiddenFieldLicenseExceed)
                {
                    return new HiddenField { ID = HiddenFieldLicenseExceed, Value = string.Empty };
                }
                else if (id == EmailPreview)
                {
                    return new CheckBox { ID = EmailPreview, Checked = true };
                }
                else
                {
                    return new System.Web.UI.Control { ID = id };
                }
            };
        }


        private void SetPageControlValue(bool allowCustOverride, bool setPageTextBoxValue = true)
        {
            foreach (var item in PageCheckBoxBoxControl)
            {
                Get<CheckBox>(_privateObject, item).Checked = allowCustOverride;
            }
            if (setPageTextBoxValue)
            {

                foreach (var item in PageTextBoxControl)
                {
                    Get<TextBox>(_privateObject, item).Text = DefaultText;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                Get<TextBox>(_privateObject, string.Concat(TextOmniture, i + 1)).Text = DefaultText;
            }
        }

        private void CreatePageFakeObject(bool allowCustOverride, bool isTestBlast = false)
        {
            var xmlResult = CreateSettingXml(allowCustOverride);
            ShimPage.AllInstances.IsPostBackGet = (x) => { return false; };
            ShimWizardSchedule.AllInstances.getCampaignItemType = (x) => { return DefaultCampaignText; };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => { return CreateCampaignItemObject(); };
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (x, y, z) => { return CreateCampaignItemObject(); };
            ShimCampaignItem.SaveCampaignItemUser = (x, y) => { return 1; };
            ShimBlastScheduler.AllInstances.SetupScheduleString = (x, y) => { return CreateBlastSetupInfoObject(isTestBlast); };
            ShimContent.ValidateContentStatusInt32 = (x) => { };
            ShimContent.ValidateLinksInt32 = (x) => { };
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) => { return new LinkTrackingSettings { }; };
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) => { return CreateLinkTrackingSettingsObject(xmlResult); };
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) => { return CreateLinkTrackingSettingsObject(xmlResult); };
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) => { return GetByLinkTrackingID(); };
            ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) => { return CreateLinkTrackingParamSettingsByCustomerId(x); };
            ShimLinkTrackingParamOption.Get_LTPID_CustomerIDInt32Int32 = (x, y) => { return CreateLinkTrackingParamOptionListObject(x); };
            ShimLinkTrackingParamSettings.Get_LTPID_BaseChannelIDInt32Int32 = (x, y) => { return CreateLinkTrackingParamSettingsByBaseChannelId(x); };
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (x, y, z) => { return 1; };
            ShimCampaignItemLinkTracking.DeleteByCampaignItemIDInt32User = (x, y) => { };
            ShimLinkTracking.GetAll = () => { return CreateLinkTrackingObject(); };
            ShimCampaignItemLinkTracking.SaveCampaignItemLinkTrackingUser = (x, y) => { return 1; };
            ShimCampaignItemLinkTracking.DeleteByLTIDInt32Int32User = (x, y, z) => { };
            ShimWizardSchedule.AllInstances.OptoutGroups_DTGet = (x) => { return CreateOptoutGroupsDataTable(); };
            ShimWizardSchedule.AllInstances.AddEmails = (x) => { return 1; };
            ShimHttpResponse.AllInstances.RedirectString = (x, y) => { _responseSting = y; };
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (x, y, z) => { return 1; };
            ShimCampaignItemBlast.SaveInt32ListOfCampaignItemBlastUser = (x, y, z) => { };
            ShimCampaignItemOptOutGroup.DeleteInt32Int32User = (x, y, z) => { };
            ShimCampaignItemOptOutGroup.DeleteInt32User = (x, y) => { };
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (x, y) => { };
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (x, y, z) => { };
            ShimBlast.GetByCampaignItemIDInt32UserBoolean = (x, y, z) => { return new List<BlastAbstract>(); };
            ShimtestGroupExplorer.AllInstances.getSelectedGroups = (x) => { return CreateGroupListObject(); };
            ShimEmailGroup.GetByGroupIDInt32User = (x, y) => { return new List<EmailGroup>(); };
        }

        private LinkTrackingParamSettings CreateLinkTrackingParamSettingsByBaseChannelId(int x)
        {
            return new LinkTrackingParamSettings
            {
                DisplayName = string.Concat(Omniture, x),
                IsRequired = true,
                IsDeleted = true,
                AllowCustom = true,
                LTPID = x
            };
        }

        private List<LinkTrackingParamOption> CreateLinkTrackingParamOptionListObject(int x)
        {
            return new List<LinkTrackingParamOption>
                  {
                    new LinkTrackingParamOption
                    {
                        IsActive = true,
                        IsDeleted = false,
                        IsDefault = true,
                        IsDynamic = true,
                        LTPOID = 1,
                        LTPID = 1,
                        BaseChannelID = 1,
                        DisplayName = string.Concat(Omniture, x)
                    }
                  };
        }

        private LinkTrackingParamSettings CreateLinkTrackingParamSettingsByCustomerId(int x)
        {
            return new LinkTrackingParamSettings
            {
                DisplayName = string.Concat(Omniture, x),
                IsRequired = true,
                IsDeleted = true,
                AllowCustom = true,
            };
        }

        private List<LinkTrackingParam> GetByLinkTrackingID()
        {
            var linkTrackingParamResult = new List<LinkTrackingParam>();
            for (int i = 0; i <= 10; i++)
            {
                linkTrackingParamResult.Add(new LinkTrackingParam
                {
                    DisplayName = string.Concat(Omniture, i),
                    LTID = i,
                    IsActive = true,
                    LTPID = i
                });
            }
            return linkTrackingParamResult;
        }

        private LinkTrackingSettings CreateLinkTrackingSettingsObject(string xmlResult)
        {
            return new LinkTrackingSettings
            {
                LTSID = 1,
                XMLConfig = xmlResult
            };
        }

        private BlastSetupInfo CreateBlastSetupInfoObject(bool isTestBlast)
        {
            return new BlastSetupInfo
            {
                IsTestBlast = isTestBlast,
                SendNowAmount = 100,
                SendTime = DateTime.Now,
                SendNowIsAmount = true,
                BlastScheduleID = 1,
                SendTextTestBlast = true,
            };
        }

        private CampaignItem CreateCampaignItemObject()
        {
            return new CampaignItem
            {
                BlastList = new List<CampaignItemBlast>
                    {
                        new CampaignItemBlast
                        {
                            LayoutID = 1,
                            CampaignItemID =1,
                            CampaignItemBlastID =1,
                            CustomerID =1,
                        }
                    },
                SampleID = 1
            };
        }

        private List<ecn.communicator.main.ECNWizard.Group.GroupObject> CreateGroupListObject()
        {
            return new List<ecn.communicator.main.ECNWizard.Group.GroupObject>
            {
                 new ecn.communicator.main.ECNWizard.Group.GroupObject
                 {
                     GroupID=1,
                     filters=new List<CampaignItemBlastFilter>
                     {
                         new CampaignItemBlastFilter
                         {
                             FilterID = 1,
                             IsDeleted = false,
                             CampaignItemBlastFilterID = 1
                         }
                     }
                 }
            };
        }

        private DataTable CreateOptoutGroupsDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("IsDeleted", typeof(string));
            dataTable.Columns.Add("CampaignItemOptOutID", typeof(string));
            dataTable.Columns.Add("GroupID", typeof(string));
            dataTable.Rows.Add("False", "A-B", "100");
            dataTable.Rows.Add("True", "100", "100");
            return dataTable;
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

        private void ShimCurrentUser(bool isActive = false)
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new KMPlatform.Entity.User
            {
                UserID = 1,
                UserName = DefaultText,
                IsActive = isActive,
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true,
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1, CreatedUserID = 1 };
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = 1, CustomerName = DefaultText };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimMarketingAutomation.CheckIfControlExistsInt32EnumsMarketingAutomationControlType = (x, y) =>
            {
                return new List<MarketingAutomation>
                {
                    new MarketingAutomation
                    {
                        Name = DefaultText
                    }
                };
            };
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }

        private string CreateSettingXml(bool allowCustomerOverride = false)
        {
            var element = new XElement("Settings",
             new XElement("Override", allowCustomerOverride.ToString()),
             new XElement("QueryString", "a"),
             new XElement("Delimiter", ","),
             new XElement("AllowCustomerOverride", allowCustomerOverride.ToString()));
            return element.ToString();
        }

        private T GetField<T>(string name) where T : class
        {
            var field = _privateObject.GetFieldOrProperty(name) as T;

            field.ShouldNotBeNull();

            return field;
        }

        private Dictionary<string, string> CreateDropDownDataSource()
        {
            var itemList = new string[] { "1", "-1", "2", "3", "4", "5", "6", "0" };
            var dropDownDataSource = new Dictionary<string, string>();
            foreach (var item in itemList)
            {
                dropDownDataSource.Add(item, item);
            }
            return dropDownDataSource;
        }

        private void BindDropDown(int selectedIndex = 1)
        {
            var dropDownDataSource = CreateDropDownDataSource();
            for (int i = 0; i < DropDowmList.Length; i++)
            {
                var dropDownKey = DropDowmList[i].ToString();
                var ddlOmniDefault = _privateObject.GetFieldOrProperty(dropDownKey) as DropDownList;
                ddlOmniDefault.DataSource = dropDownDataSource;
                ddlOmniDefault.DataValueField = "Key";
                ddlOmniDefault.DataTextField = "Value";
                ddlOmniDefault.SelectedIndex = selectedIndex;
                ddlOmniDefault.DataBind();
                _privateObject.SetFieldOrProperty(dropDownKey, ddlOmniDefault);
            }

            for (int i = 0; i < 10; i++)
            {
                var dropDownKey = string.Concat(DropDownOmniDefault, i + 1);
                var ddlOmniDefault = _privateObject.GetFieldOrProperty(dropDownKey) as DropDownList;
                ddlOmniDefault.DataSource = dropDownDataSource;
                ddlOmniDefault.DataValueField = "Key";
                ddlOmniDefault.DataTextField = "Value";
                ddlOmniDefault.SelectedIndex = selectedIndex;
                ddlOmniDefault.DataBind();
                _privateObject.SetFieldOrProperty(dropDownKey, ddlOmniDefault);
            }
        }
    }
}
