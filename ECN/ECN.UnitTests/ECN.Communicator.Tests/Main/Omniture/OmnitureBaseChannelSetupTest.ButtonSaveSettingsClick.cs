using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI.WebControls;
using ecn.communicator.main.Omniture;
using ecn.communicator.main.Omniture.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Omniture
{
    /// <summary>
    /// Unit test for <see cref="OmnitureBaseChannelSetup"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class OmnitureBaseChannelSetupTestButtonSaveSettingsClick : PageHelper
    {
        private const string ButtonSaveSettingsClick = "btnSaveSettings_Click";
        private const string TextDelimiter = "txtDelimiter";
        private const string TextQueryName = "txtQueryName";
        private const string PlaceHolderError = "phError";
        private const string CheckBoxOverride = "chkboxOverride";
        private const string DropDownOmniDefault = "ddlOmniDefault";
        private const string Message = "Save Successful";
        private const string PageTitle = "SUCCESS";
        private const string ButtonId = "btnconfirmtemplate";
        private const string Omniture = "omniture";
        private const string TestUser = "Unit Test";
        private const string MessageResult = "The following Campaign Item Template(s) are using Customer level Omniture values that will be deleted: <br /> Unit Test";
        private const string LabelTemplateMessage = "lblTemplateMessage";
        private const string LabelErrorMessage = "lblErrorMessage";
        private const string QueryStringErrorMessage = "Please enter a different value for the query string";
        private OmnitureBaseChannelSetup _omnitureBaseChannelSetup;
        private PrivateObject _privateObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _omnitureBaseChannelSetup = new OmnitureBaseChannelSetup();
            InitializeAllControls(_omnitureBaseChannelSetup);
            _privateObject = new PrivateObject(_omnitureBaseChannelSetup);
            ShimCurrentUser();
            CreatePageShimObject();
        }

        [TestCase("A", " ")]
        [TestCase(" ", "A")]
        [TestCase(" ", " ")]
        public void ButtonSaveSettingsClick_TextDelimiterIsNullAndTextQueryNameIsull_SaveSettingsData(string queryName, string delimiter)
        {
            //  Arrange
            var parameters = new object[] { this, EventArgs.Empty };
            var txtDelimiter = GetField<TextBox>(TextDelimiter);
            txtDelimiter.Text = delimiter;
            _privateObject.SetFieldOrProperty(TextDelimiter, txtDelimiter);
            var txtQueryName = GetField<TextBox>(TextQueryName);
            txtQueryName.Text = queryName;
            _privateObject.SetFieldOrProperty(TextQueryName, txtQueryName);

            //  Act
            _privateObject.Invoke(ButtonSaveSettingsClick, parameters);
            var phError = GetField<PlaceHolder>(PlaceHolderError);

            //  Assert
            phError.Visible.ShouldBeTrue();
        }

        [TestCase(100, 1)]
        [TestCase(0, 0)]
        public void ButtonSaveSettingsClick_TextDelimiterIsNotNullAndTxtQueryNameIsNotNull_SaveSettingsData(int ltsId, int selectedIndex)
        {
            //  Arrange
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings { LTSID = ltsId };
            };
            var btnSave = new Button();
            btnSave.ID = ButtonId;
            var sampleText = PageTitle;
            var parameters = new object[] { btnSave, EventArgs.Empty };
            var txtDelimiter = GetField<TextBox>(TextDelimiter);
            txtDelimiter.Text = sampleText;
            _privateObject.SetFieldOrProperty(TextDelimiter, txtDelimiter);
            var txtQueryName = GetField<TextBox>(TextQueryName);
            txtQueryName.Text = sampleText;
            _privateObject.SetFieldOrProperty(TextQueryName, txtQueryName);
            BindDropDown(selectedIndex);
            CreateLinkTrackingParamSettings(ltsId);
            var message = string.Empty;
            var pageTitle = string.Empty;
            ShimOmnitureBaseChannelSetup.AllInstances.ShowMessageStringStringMessageMessage_Icon = (sender, msg, title, icon) =>
              {
                  message = msg;
                  pageTitle = title;
              };

            //  Act
            _privateObject.Invoke(ButtonSaveSettingsClick, parameters);

            //  Assert
            message.ShouldSatisfyAllConditions
            (
                () => message.ShouldBe(Message),
                () => pageTitle.ShouldBe(PageTitle)
            );
        }

        [TestCase(100, 1)]
        [TestCase(0, 0)]
        public void ButtonSaveSettingsClick_IsQueryStringNotValid_ReturnsMessage(int ltsId, int selectedIndex)
        {
            //  Arrange
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings { LTSID = ltsId };
            };
            ShimOmnitureBaseChannelSetup.AllInstances.IsQueryStringValid = (x) => { return false; };
            var btnSave = new Button();
            btnSave.ID = ButtonId;
            var sampleText = PageTitle;
            var parameters = new object[] { btnSave, EventArgs.Empty };
            var txtDelimiter = GetField<TextBox>(TextDelimiter);
            txtDelimiter.Text = sampleText;
            _privateObject.SetFieldOrProperty(TextDelimiter, txtDelimiter);

            var txtQueryName = GetField<TextBox>(TextQueryName);
            txtQueryName.Text = sampleText;
            _privateObject.SetFieldOrProperty(TextQueryName, txtQueryName);
            BindDropDown(selectedIndex);
            CreateLinkTrackingParamSettings(ltsId);

            //  Act
            _privateObject.Invoke(ButtonSaveSettingsClick, parameters);

            //  Assert
            var labelErrorMessage = Get<Label>(_privateObject, LabelErrorMessage);
            labelErrorMessage.ShouldSatisfyAllConditions(
                () => labelErrorMessage.Text.ShouldNotBeNullOrEmpty(),
                () => labelErrorMessage.Text.ShouldBe(QueryStringErrorMessage)
            );
        }

        [Test]
        public void ButtonSaveSettingsClick_ClickedButtonIsNotSaveButton_ReturnsStringMessage()
        {
            //  Arrange
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings { LTSID = 0 };
            };
            ShimOmnitureBaseChannelSetup.AllInstances.CheckSettingsChange = (sender) => { return false; };
            var btnSave = new Button();
            btnSave.ID = "btn1";
            var sampleText = PageTitle;
            var parameters = new object[] { btnSave, EventArgs.Empty };
            var txtDelimiter = GetField<TextBox>(TextDelimiter);
            txtDelimiter.Text = sampleText;
            _privateObject.SetFieldOrProperty(TextDelimiter, txtDelimiter);

            var txtQueryName = GetField<TextBox>(TextQueryName);
            txtQueryName.Text = sampleText;
            _privateObject.SetFieldOrProperty(TextQueryName, txtQueryName);

            //  Act
            _privateObject.Invoke(ButtonSaveSettingsClick, parameters);

            //  Assert
            var lblTemplateMessage = Get<Label>(_privateObject, LabelTemplateMessage);
            lblTemplateMessage.ShouldSatisfyAllConditions(
                () => lblTemplateMessage.Text.ShouldNotBeNullOrEmpty(),
                () => lblTemplateMessage.Text.ShouldBe(MessageResult)
            );
        }

        private void CreateLinkTrackingParamSettings(int ltsId)
        {
            ShimLinkTrackingParamSettings.Get_LTPID_BaseChannelIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingParamSettings
                {
                    DisplayName = string.Concat(Omniture, x),
                    IsRequired = true,
                    IsDeleted = true,
                    AllowCustom = true,
                    LTPSID = ltsId != 0 ? x : ltsId
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

        private void BindDropDown(int selectedIndex = 1)
        {
            var dropDownDataSource = CreateDropDownDataSource();
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

        private static Dictionary<string, string> CreateDropDownDataSource()
        {
            var dropDownDataSource = new Dictionary<string, string>();
            dropDownDataSource.Add("-1", "-1");
            dropDownDataSource.Add("1", "1");
            dropDownDataSource.Add("2", "2");
            dropDownDataSource.Add("3", "3");
            dropDownDataSource.Add("4", "4");
            return dropDownDataSource;
        }

        private void CreatePageShimObject()
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            ecnSession.CurrentBaseChannel = new BaseChannel { BaseChannelID = 3 };
            ecnSession.CurrentUser = CreateUserObject();
            ecnSession.CurrentCustomer = new Customer { CustomerID = 1, BaseChannelID = 1 };
            ShimECNSession.AllInstances.BaseChannelIDGet = (x) => 1;
            Common.Fakes.ShimMasterPageEx.AllInstances.HeadingSetString = (masterpageEx, inputString) => { };
            Common.Fakes.ShimMasterPageEx.AllInstances.HelpContentSetString = (masterpageEx, inputString) => { };
            Common.Fakes.ShimMasterPageEx.AllInstances.HelpTitleSetString = (masterpageEx, inputString) => { };
            ShimOmnitureBaseChannelSetup.AllInstances.MasterGet = (x) =>
            {
                return CreateCommunicatorObject(ecnSession);
            };
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) =>
            {
                return CreateLinkTrackingParamObject();
            };
            ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
            {
                return CreateLinkTrackingParamSettingsObject(x);
            };

            ShimLinkTrackingParamOption.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
            {
                return CreateLinkTrackingParamOption(x);
            };
            ShimLinkTrackingParamSettings.UpdateLinkTrackingParamSettings = (x) => { };
            ShimLinkTrackingParamOption.ResetBaseDefaultInt32Int32 = (x, y) => { };
            ShimLinkTrackingParamOption.GetByLTPOIDInt32 = (x) => { return new LinkTrackingParamOption { BaseChannelID = 1 }; };
            ShimOmnitureBaseChannelSetup.AllInstances.CheckSettingsChange = (x) => { return true; };
            ShimCampaignItemTemplate.ClearOutOmniDataBySetupLevelInt32NullableOfInt32BooleanInt32 = (x, y, z, n) => { };
            ShimLinkTrackingSettings.UpdateLinkTrackingSettings = (x) => { };
            ShimLinkTrackingSettings.UpdateCustomerOmnitureOverrideInt32BooleanInt32 = (x, y, z) => { };
            ShimLinkTrackingParamOption.UpdateLinkTrackingParamOption = (x) => { };
            ShimLinkTrackingParamOption.ResetBaseDefaultInt32Int32 = (x, y) => { };
            ShimLinkTrackingSettings.InsertLinkTrackingSettings = (x) => { return 1; };
            ShimLinkTrackingParamSettings.InsertLinkTrackingParamSettings = (x) => { return 1; };
            ShimCampaignItemTemplate.GetTemplatesBySetupLevelInt32NullableOfInt32Boolean = (x, y, z) =>
            {
                return new List<CampaignItemTemplate>
                {
                    new CampaignItemTemplate
                    {
                        TemplateName=TestUser
                    }
                };
            };
        }

        private KMPlatform.Entity.User CreateUserObject()
        {
            return new KMPlatform.Entity.User
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true,
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true
            };
        }

        private ecn.communicator.MasterPages.Communicator CreateCommunicatorObject(ECNSession ecnSession)
        {
            return new ShimCommunicator
            {
                CurrentMenuCodeGet = () => { return MenuCode.OMNITURE; },
                CurrentMenuCodeSetEnumsMenuCode = (y) => { },
                UserSessionGet = () => { return ecnSession; }
            };
        }

        private List<LinkTrackingParam> CreateLinkTrackingParamObject()
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

        private LinkTrackingParamSettings CreateLinkTrackingParamSettingsObject(int x)
        {
            return new LinkTrackingParamSettings
            {
                DisplayName = string.Concat(Omniture, x),
                IsRequired = true,
                IsDeleted = true,
                AllowCustom = true,
            };
        }

        private List<LinkTrackingParamOption> CreateLinkTrackingParamOption(int x)
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

        private T GetField<T>(string name) where T : class
        {
            var field = _privateObject.GetFieldOrProperty(name) as T;

            field.ShouldNotBeNull();

            return field;
        }

        private void ShimCurrentUser(bool isActive = true)
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new KMPlatform.Entity.User
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = isActive,
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true
            };
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = 1, CustomerName = TestUser, BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}
