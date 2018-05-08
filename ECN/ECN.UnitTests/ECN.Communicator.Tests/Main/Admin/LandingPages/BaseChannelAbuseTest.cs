using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.main.admin.LandingPages;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Interfaces;
using ECN.Tests.Helpers;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BaseChannelAbuseTest
    {
        private const string InvalidCodeSnippet = "%%";
        private const int DummyUserID = -1;
        private const int DummyBaseChannelID = -2;
        private const string Yes = "Yes";
        private const string NonEmptyText = "Non Empty Text";
        private const string ECNButtonMedium = "ECN-Button-Medium";
        private const string BlastId = "1";
        private const string EmailId = "1";
        private const int LandingPageID = 4;
        private const int ZeroSelectedIndex = 0;
        private const int NonZeroSelectedIndex = 1;
        private const string CustomerDropDownItem1 = "11";
        private const string CustomerDropDownItem2 = "22";
        private const string ActivityDomainUrl = "Activity_DomainPath/engines/reportSpam.aspx?p=1&preview=";
        private const string OnClickAttribute = "onclick";
        private const string DdlCustomerSelectedIndexChangedMethodName = "ddlCustomer_SelectedIndexChanged";
        private const string HtmlPreviewShowButtonName = "btnHtmlPreviewShow";
        private const string UrlWarningLabelName = "lblUrlWarning";
        private const string CustomerDropDownListName = "ddlCustomer";
        private const string FieldLandingPageAssign = "LPA";
        private const string PageLoadMethodName = "Page_Load";
        private const string NoAccessPanelName = "pnlNoAccess";
        private const string SettingsPanelName = "pnlSettings";
        private const string PreviewButtonName = "btnPreview";
        private const string ThankYouTextBoxName = "txtThankYou";
        private const string OverrideDefaultSettingsRadioButtonListName = "rblOverrideDefaultSettings";
        private const string AllowCustomerOverrideSettingsRadioButtonListName = "rblAllowCustomerOverrideSettings";
        private const string Label1Name = "Label1";
        private const string TextContainsThankYou = "Text Contains Thank You";

        private Mock<HttpResponseBase> _response;
        private BaseChannelAbuse _channel;

        [SetUp]
        public void SetUp()
        {
            _channel = CreateChannel();
        }

        [Test]
        public void BtnSaveClick_NullLandingPageAssign_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            InitializeControls(NonEmptyText, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, FieldLandingPageAssign).GetValue(_channel) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.CreatedUserID.ShouldBe(DummyUserID);
            landingPageAssign.BaseChannelID.HasValue.ShouldBeTrue();
            landingPageAssign.BaseChannelID.Value.ShouldBe(DummyBaseChannelID);
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldNotBeNull();
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.CustomerCanOverride.Value.ShouldNotBeNull();
            landingPageAssign.CustomerCanOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBe(NonEmptyText);
            landingPageAssign.Footer.ShouldBe(NonEmptyText);
            landingPageAssign.UpdatedUserID.ShouldBe(DummyUserID);
            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, PreviewButtonName).GetValue(_channel) as Button;
            previewButton.ShouldNotBeNull();
            previewButton.Enabled.ShouldBeTrue();
            previewButton.Visible.ShouldBeTrue();
            previewButton.CssClass.ShouldBe(ECNButtonMedium);

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "lblErrorMessage").GetValue(_channel) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe(NonEmptyText);

            _response.Verify(x => x.Redirect("BaseChannelMain.aspx"), Times.Once());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForHeaderText_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            InitializeControls(InvalidCodeSnippet, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, FieldLandingPageAssign).GetValue(_channel) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldNotBeNull();
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.CustomerCanOverride.Value.ShouldNotBeNull();
            landingPageAssign.CustomerCanOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBeEmpty();
            landingPageAssign.Footer.ShouldBeEmpty();
            landingPageAssign.UpdatedUserID.ShouldBeNull();

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, PreviewButtonName).GetValue(_channel) as Button;
            previewButton.ShouldNotBeNull();
            previewButton.Enabled.ShouldBeFalse();
            previewButton.Visible.ShouldBeFalse();
            previewButton.CssClass.ShouldBeEmpty();

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "lblErrorMessage").GetValue(_channel) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe("<br/>LandingPage: There is a badly formed codesnippet in Header");

            _response.Verify(x => x.Redirect("BaseChannelMain.aspx"), Times.Never());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForFooterText_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            InitializeControls(NonEmptyText, InvalidCodeSnippet);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, FieldLandingPageAssign).GetValue(_channel) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldNotBeNull();
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.CustomerCanOverride.Value.ShouldNotBeNull();
            landingPageAssign.CustomerCanOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBeEmpty();
            landingPageAssign.Footer.ShouldBeEmpty();
            landingPageAssign.UpdatedUserID.ShouldBeNull();

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, PreviewButtonName).GetValue(_channel) as Button;
            previewButton.ShouldNotBeNull();
            previewButton.Enabled.ShouldBeFalse();
            previewButton.Visible.ShouldBeFalse();
            previewButton.CssClass.ShouldBeEmpty();

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "lblErrorMessage").GetValue(_channel) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe("<br/>LandingPage: There is a badly formed codesnippet in Footer");

            _response.Verify(x => x.Redirect("BaseChannelMain.aspx"), Times.Never());
        }

        [Test]
        public void DdlCustomerSelectedIndexChanged_NonEmptyUrl_EnablePreview()
        {
            // Arrange
            InitializeDropDownCustomerControls(NonZeroSelectedIndex, NonEmptyText, false, BorderStyle.Dashed);
            ReflectionHelper.SetField(_channel, FieldLandingPageAssign, new LandingPageAssign() { LPAID = LandingPageID });

            // Act
            var parameters = new object[] { null, EventArgs.Empty};
            ReflectionHelper.ExecuteMethod(_channel, DdlCustomerSelectedIndexChangedMethodName, parameters);

            // Assert
            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, HtmlPreviewShowButtonName).GetValue(_channel) as Button;
            previewButton.ShouldSatisfyAllConditions(
                () => previewButton.ShouldNotBeNull(),
                () => previewButton.Enabled.ShouldBeTrue(),
                () => previewButton.Visible.ShouldBeTrue(),
                () => previewButton.Attributes[OnClickAttribute].ShouldBe(
                    $"window.open('{ActivityDomainUrl}{LandingPageID}', 'popup_window', 'width=1000,height=750,resizable=yes');"));

            var urlWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, UrlWarningLabelName).GetValue(_channel) as Label;
            urlWarning.ShouldSatisfyAllConditions(
                () => urlWarning.ShouldNotBeNull(),
                () => urlWarning.BorderColor.ShouldNotBe(Color.Red),
                () => urlWarning.BorderStyle.ShouldBe(BorderStyle.None),
                () => urlWarning.Text.ShouldBeEmpty());
        }

        [Test]
        public void DdlCustomerSelectedIndexChanged_NullUrl_DisablePreview()
        {
            // Arrange
            _channel = CreateChannelWithEmptyPreviewParameters();
            InitializeDropDownCustomerControls(NonZeroSelectedIndex, NonEmptyText, true, BorderStyle.None);
            ReflectionHelper.SetField(_channel, FieldLandingPageAssign, new LandingPageAssign() { LPAID = LandingPageID });

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            ReflectionHelper.ExecuteMethod(_channel, DdlCustomerSelectedIndexChangedMethodName, parameters);

            // Assert
            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, HtmlPreviewShowButtonName)
                .GetValue(_channel) as Button;
            previewButton.ShouldSatisfyAllConditions(
                () => previewButton.ShouldNotBeNull(),
                () => previewButton.Enabled.ShouldBeFalse(),
                () => previewButton.Visible.ShouldBeFalse());

            var urlWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, UrlWarningLabelName)
                .GetValue(_channel) as Label;
            urlWarning.ShouldSatisfyAllConditions(
                () => urlWarning.ShouldNotBeNull(),
                () => urlWarning.BorderColor.ShouldBe(Color.Red),
                () => urlWarning.BorderStyle.ShouldBe(BorderStyle.Dashed),
                () => urlWarning.Text.ShouldBe(
                    $"Something is wrong with the {CustomerDropDownItem2} report abuse page. \n Please ensure that the customer has sent blasts to at least one group."));
        }

        [Test]
        public void DdlCustomerSelectedIndexChanged_ZeroSelectedIndex_DisablePreview()
        {
            // Arrange
            _channel = CreateChannelWithEmptyPreviewParameters();
            InitializeDropDownCustomerControls(ZeroSelectedIndex, NonEmptyText, true, BorderStyle.Dashed);
            ReflectionHelper.SetField(_channel, FieldLandingPageAssign, new LandingPageAssign() { LPAID = LandingPageID });

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            ReflectionHelper.ExecuteMethod(_channel, DdlCustomerSelectedIndexChangedMethodName, parameters);

            // Assert
            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, HtmlPreviewShowButtonName)
                .GetValue(_channel) as Button;
            previewButton.ShouldSatisfyAllConditions(
                () => previewButton.ShouldNotBeNull(),
                () => previewButton.Enabled.ShouldBeFalse(),
                () => previewButton.Visible.ShouldBeFalse());

            var urlWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, UrlWarningLabelName)
                .GetValue(_channel) as Label;
            urlWarning.ShouldSatisfyAllConditions(
                () => urlWarning.ShouldNotBeNull(),
                () => urlWarning.BorderStyle.ShouldBe(BorderStyle.None),
                () => urlWarning.Text.ShouldBeEmpty());
        }

        [Test]
        public void PageLoad_ChannelAdministrator_EnableSettingsPanelAndLoadData()
        {
            // Arrange
            var landingPageOption = new Mock<ILandingPageOption>();
            landingPageOption.Setup(x => x.GetByLPID(It.IsAny<int>()))
                .Returns(new List<LandingPageOption>()
                {
                    new LandingPageOption()
                    {
                        LPOID = LandingPageID,
                        Name = TextContainsThankYou
                    }
                });

            _channel = CreateChannel(landingPageOption.Object);
            InitializePageLoadControls(true, false, true);

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            ReflectionHelper.ExecuteMethod(_channel, PageLoadMethodName, parameters);

            // Assert
            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, PreviewButtonName)
                .GetValue(_channel) as Button;
            previewButton.ShouldSatisfyAllConditions(
                () => previewButton.ShouldNotBeNull(),
                () => previewButton.Enabled.ShouldBeTrue(),
                () => previewButton.Visible.ShouldBeTrue());

            AssertPanelVisibility(NoAccessPanelName, false);
            AssertPanelVisibility(SettingsPanelName, true);
            AssertTextBox(ThankYouTextBoxName, NonEmptyText);
        }

        [Test]
        public void PageLoad_NotChannelAdministrator_ShowNoAccessPanelAndDisplayError()
        {
            // Arrange
            _channel = CreateChannel(null, false);
            InitializePageLoadControls(false, true, false);

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            ReflectionHelper.ExecuteMethod(_channel, PageLoadMethodName, parameters);

            // Assert
            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, PreviewButtonName)
                .GetValue(_channel) as Button;
            previewButton.ShouldSatisfyAllConditions(
                () => previewButton.ShouldNotBeNull(),
                () => previewButton.Enabled.ShouldBeFalse(),
                () => previewButton.Visible.ShouldBeFalse());

            AssertPanelVisibility(NoAccessPanelName, true);
            AssertPanelVisibility(SettingsPanelName, false);

            var errorLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, Label1Name)
                .GetValue(_channel) as Label;
            errorLabel.ShouldSatisfyAllConditions(
                () => errorLabel.ShouldNotBeNull(),
                () => errorLabel.Text.ShouldBe("You do not have access to this page."));
        }

        [Test]
        public void BtnSaveClick_NullLandingPageAssignAndNonEmptyThankYouText_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            var listOptions = ReflectionHelper.GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelAbuse), "_listOptions");
            listOptions.SetValue(
                null,
                new List<LandingPageOption>()
                {
                    new LandingPageOption()
                    {
                        LPOID = LandingPageID,
                        Name = TextContainsThankYou
                    }
                });

            LandingPageAssignContent landingPageAssignContentParameterValue = null;
            var landingPageAssignContent = new Mock<ILandingPageAssignContent>();
            landingPageAssignContent
                .Setup(x => x.Save(It.IsAny<LandingPageAssignContent>(), It.IsAny<User>()))
                .Callback<LandingPageAssignContent, User>((landingPageAssignContentValue, userValue) => 
                {
                    landingPageAssignContentParameterValue = landingPageAssignContentValue;
                });

            _channel = CreateChannel(null, true, null, landingPageAssignContent);
            InitializeControls(NonEmptyText, NonEmptyText, NonEmptyText);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", parameters);

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, FieldLandingPageAssign).GetValue(_channel) as LandingPageAssign;
            landingPageAssign.ShouldSatisfyAllConditions(
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.HasValue.ShouldBeTrue(),
                () => landingPageAssign.BaseChannelID.Value.ShouldBe(DummyBaseChannelID),
                () => landingPageAssign.LPID.ShouldBe(LandingPageID),
                () => landingPageAssign.BaseChannelDoesOverride.Value.ShouldNotBeNull(),
                () => landingPageAssign.BaseChannelDoesOverride.Value.ShouldBeFalse(),
                () => landingPageAssign.CustomerCanOverride.Value.ShouldNotBeNull(),
                () => landingPageAssign.CustomerCanOverride.Value.ShouldBeFalse(),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(NonEmptyText),
                () => landingPageAssign.UpdatedUserID.ShouldBe(DummyUserID));

            landingPageAssignContentParameterValue.ShouldSatisfyAllConditions(
                () => landingPageAssignContentParameterValue.ShouldNotBeNull(),
                () => landingPageAssignContentParameterValue.Display.ShouldBe(NonEmptyText),
                () => landingPageAssignContentParameterValue.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssignContentParameterValue.LPAID = landingPageAssign.LPAID,
                () => landingPageAssignContentParameterValue.LPOID = LandingPageID);

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, PreviewButtonName).GetValue(_channel) as Button;
            previewButton.ShouldSatisfyAllConditions(
                () => previewButton.ShouldNotBeNull(),
                () => previewButton.Enabled.ShouldBeTrue(),
                () => previewButton.Visible.ShouldBeTrue(),
                () => previewButton.CssClass.ShouldBe(ECNButtonMedium));

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "lblErrorMessage").GetValue(_channel) as Label;
            errorMessageLabel.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldBe(NonEmptyText));

            _response.Verify(x => x.Redirect("BaseChannelMain.aspx"), Times.Once());
        }

        [Test]
        public void BtnSaveClick_ExceptionOnSave_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            var applicationLog = new Mock<IApplicationLog>();
            applicationLog.Setup(x => x.LogCriticalError(
                It.IsAny<ECNException>(),
                "BaseChannelAbuse.btnSave_Click",
                DummyUserID,
                string.Empty,
                DummyUserID,
                DummyUserID));

            var landingPageAssignContent = new Mock<ILandingPageAssignContent>();
            landingPageAssignContent
                .Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new ECNException(null));

            var master = new Mock<IMasterCommunicator>();
            master.Setup(x => x.GetUserID()).Throws(new ECNException(null));

            _channel = CreateChannel(null, true, applicationLog.Object, landingPageAssignContent, master);
            InitializeBtnSaveException();

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", parameters);

            // Assert
            applicationLog.VerifyAll();
        }

        private void AssertTextBox(string textBoxName, string textValue)
        {
            var textBox = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, textBoxName)
                .GetValue(_channel) as TextBox;
            textBox.ShouldSatisfyAllConditions(
                () => textBox.ShouldNotBeNull(),
                () => textBox.Text.ShouldBe(textValue));
        }

        private void AssertPanelVisibility(string panelName, bool isVisible)
        {
            var panel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, panelName)
                .GetValue(_channel) as Panel;
            panel.ShouldSatisfyAllConditions(
                () => panel.ShouldNotBeNull(),
                () => panel.Visible.ShouldBe(isVisible));
        }

        private void InitializeBtnSaveException()
        {
            ReflectionHelper.SetValue(_channel, "phError", new PlaceHolder() { Visible = true });
            ReflectionHelper.SetValue(_channel, "lblErrorMessage", new Label() { Text = NonEmptyText });
        }

        private void InitializePageLoadControls(bool noAccessVisible, bool settingsVisible, bool previewEnabled)
        {
            ReflectionHelper.SetValue(_channel, "phError", new PlaceHolder() { Visible = true });
            ReflectionHelper.SetValue(_channel, NoAccessPanelName, new Panel() { Visible = noAccessVisible });
            ReflectionHelper.SetValue(_channel, SettingsPanelName, new Panel() { Visible = settingsVisible });
            ReflectionHelper.SetValue(_channel, PreviewButtonName, new Button() { Enabled = previewEnabled, Visible = previewEnabled });
            ReflectionHelper.SetValue(_channel, "txtHeader", new TextBox() { Text = string.Empty });
            ReflectionHelper.SetValue(_channel, "txtFooter", new TextBox() { Text = string.Empty });
            ReflectionHelper.SetValue(_channel, ThankYouTextBoxName, new TextBox() { Text = string.Empty });
            ReflectionHelper.SetValue(_channel, Label1Name, new Label() { Text = string.Empty });
            ReflectionHelper.SetValue(_channel, OverrideDefaultSettingsRadioButtonListName, new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(_channel, AllowCustomerOverrideSettingsRadioButtonListName, new RadioButtonList() { SelectedValue = string.Empty });
        }

        private void InitializeDropDownCustomerControls(int selectedIndex, string urlWarning, bool previewActive, BorderStyle borderStyle)
        {
            var customerDropDown = new DropDownList();
            customerDropDown.Items.Add(CustomerDropDownItem1);
            customerDropDown.Items.Add(CustomerDropDownItem2);
            customerDropDown.SelectedIndex = selectedIndex;

            ReflectionHelper.SetValue(_channel, CustomerDropDownListName, customerDropDown);
            ReflectionHelper.SetValue(_channel,
                UrlWarningLabelName, 
                new Label()
                {
                    Text = urlWarning,
                    BorderStyle = borderStyle
                });
            ReflectionHelper.SetValue(
                _channel, 
                HtmlPreviewShowButtonName, 
                new Button()
                {
                    Enabled = previewActive,
                    Visible = previewActive
                });
        }

        private void InitializeControls(string headerText = "", string footerText = "", string thankYouText = "")
        {
            ReflectionHelper.SetValue(_channel, OverrideDefaultSettingsRadioButtonListName, new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(_channel, AllowCustomerOverrideSettingsRadioButtonListName, new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(_channel, "txtHeader", new TextBox() { Text = headerText });
            ReflectionHelper.SetValue(_channel, "txtFooter", new TextBox() { Text = footerText });
            ReflectionHelper.SetValue(_channel, ThankYouTextBoxName, new TextBox() { Text = thankYouText });
            ReflectionHelper.SetValue(_channel, PreviewButtonName, new Button() { Enabled = false, Visible = false, CssClass = string.Empty });
            InitializeBtnSaveException();
        }

        private BaseChannelAbuse CreateChannelWithEmptyPreviewParameters()
        {
            var master = new Mock<IMasterCommunicator>();
            var landingPageAssignContent = new Mock<ILandingPageAssignContent>();
            _response = new Mock<HttpResponseBase>();

            var landingPageAssign = new Mock<ILandingPageAssign>();
            landingPageAssign.Setup(x => x.GetPreviewParameters(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new DataTable());

            return new BaseChannelAbuse(master.Object, landingPageAssign.Object, landingPageAssignContent.Object, _response.Object, null, null);
        }

        private BaseChannelAbuse CreateChannel(
            ILandingPageOption landingPageOption = null, 
            bool isUserActive = true, 
            IApplicationLog applicationLog = null, 
            Mock<ILandingPageAssignContent> landingPageAssignContent = null, 
            Mock<IMasterCommunicator> master = null)
        {
            if (master == null)
            {
                master = new Mock<IMasterCommunicator>();
                master.Setup(x => x.GetUserID()).Returns(DummyUserID);
                master.Setup(x => x.GetBaseChannelID()).Returns(DummyBaseChannelID);
                master.Setup(x => x.GetCurrentUser())
                    .Returns(new User()
                    {
                        IsActive = isUserActive,
                        CurrentSecurityGroup = new SecurityGroup()
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    });
            }

            var landingPageAssign = new Mock<ILandingPageAssign>();
            landingPageAssign.Setup(x => x.Save(It.IsAny<LandingPageAssign>(), It.IsAny<User>()));
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn());
            dataTable.Columns.Add(new DataColumn());
            var dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
            {
                BlastId,
                EmailId
            };
            dataTable.Rows.Add(dataRow);
            landingPageAssign.Setup(x => x.GetPreviewParameters(It.IsAny<int>(), It.IsAny<int>())).Returns(dataTable);
            landingPageAssign.Setup(x => x.GetByBaseChannelID(It.IsAny<int>(), LandingPageID))
                .Returns(new LandingPageAssign()
                {
                    LPAID = LandingPageID,
                    BaseChannelDoesOverride = true,
                    CustomerCanOverride = true,
                    Header = NonEmptyText,
                    Footer = NonEmptyText
                });

            if (landingPageAssignContent == null)
            {
                landingPageAssignContent = new Mock<ILandingPageAssignContent>();
                landingPageAssignContent.Setup(x => x.Save(It.IsAny<LandingPageAssignContent>(), It.IsAny<User>()));
                landingPageAssignContent.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<User>()));
                landingPageAssignContent.Setup(x => x.GetByLPAID(It.IsAny<int>()))
                    .Returns(new List<LandingPageAssignContent>()
                    {
                        new LandingPageAssignContent()
                        {
                            LPOID = LandingPageID,
                            Display = NonEmptyText
                        }
                    });
            }

            _response = new Mock<HttpResponseBase>();
            _response.Setup(x => x.Redirect("BaseChannelMain.aspx"));

            return new BaseChannelAbuse(
                master.Object, 
                landingPageAssign.Object, 
                landingPageAssignContent.Object, 
                _response.Object, 
                landingPageOption,
                applicationLog);
        }
    }
}
