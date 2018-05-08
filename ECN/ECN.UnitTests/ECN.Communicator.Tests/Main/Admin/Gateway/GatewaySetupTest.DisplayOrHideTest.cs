using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.communicator.main.admin.Gateway.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using commEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.Admin.Gateway
{
    public partial class GatewaySetupTest
    {
        private const string DOH_DataFilePath = "DataFilePath";
        private const string DOH_ForgotPasswordText = "ForgotPasswordText";
        private const string DOH_SignupText = "SignupText";
        private const string DOH_SignupURL = "SignupURL";
        private const string DOH_Style = "DataFilePath/Style";
        private const string DOH_ConfirmationMessage = "ConfirmationMessage";
        private const string DOH_ConfirmationText = "ConfirmationText";
        private const string DOH_RedirectURL = "RedirectURL";
        private const int DOH_RedirectDelay = 2;
        private const bool DOH_ValidatePassword = false;
        private TextBox _txtForgotPasswordText;
        private TextBox _txtSignupText;
        private TextBox _txtStyle;
        private TextBox _txtSignupURL;
        private TextBox _txtConfirmationMessage;
        private TextBox _txtConfirmationText;
        private TextBox _txtConfirmationRedirectURL;
        private RadioButtonList _rblForgotPasswordVisible;
        private RadioButtonList _rblConfirmationPage;
        private RadioButtonList _rblSignupVisible;
        private RadioButtonList _rblLoginOrCapture;
        private RadioButtonList _rblStyling;
        private DropDownList _ddlConfirmationRedirectDelay;
        private CheckBox _chkLoginValidatePassword;
        private CheckBox _chkLoginValidateCustom;
        private HtmlTableRow _trForgotPasswordText;
        private HtmlTableRow _trSignupURL;
        private HtmlTableRow _trSignupText;
        private Panel _pnlPassword;
        private Panel _pnlSignup;
        private Panel _pnlLogin;
        private Panel _pnlCapture;
        private Panel _pnlCSS;
        private Panel _pnlRedirect;
        private Panel _pnlAutoRedirect;
        private GridView _gvCaptureCustom;
        private GridView _gvCaptureValues;
        private GridView _gvCustomValidate;
        private RequiredFieldValidator _revForgotPassword;
        private RequiredFieldValidator _revSignupURL;
        private RequiredFieldValidator _revStyle;
        private RequiredFieldValidator _revConfirmationMessage;
        private RequiredFieldValidator _revConfirmationText;
        private RegularExpressionValidator _revConfirmationRedirectURL;
        private UpdatePanel _updatePanel;

        [TestCase(true, false, "default", "page")]
        [TestCase(false, true, "external", "pagewithredirect")]
        [TestCase(false, true, "upload", "pagewithautoredirect")]
        public void DisplayOrHide_DifferentInitializers_ControlsInitializedWithProperValues(bool forgotPasswordVisibile, bool signUpVisible, string selectedStyle, string selectedConfirmation)
        {
            // Arrange
            InitTestDisplayOrHide(confirmationPage: selectedConfirmation, forgotPasswordVisible: forgotPasswordVisibile, loginOrCapture: "skip_", signupVisible: signUpVisible, styleSelectedValue: selectedStyle, gateway: out commEntities.Gateway gateway);
            var expectedStyle = string.Empty;
            if (selectedStyle == "external")
            {
                expectedStyle = DOH_Style;
            }
            else if (selectedStyle == "upload")
            {
                expectedStyle = DOH_Style.Replace(DOH_DataFilePath + "/", "");
            }

            // Act
            _gatewaySetupPrivateObject.Invoke("DisplayOrHide", new object[] { gateway });

            // Assert
            gateway.ShouldSatisfyAllConditions(
                    () => _trForgotPasswordText.Visible.ShouldBe(forgotPasswordVisibile),
                    () => _revForgotPassword.Enabled.ShouldBe(forgotPasswordVisibile),
                    () => _txtForgotPasswordText.Text.ShouldBe(forgotPasswordVisibile ? DOH_ForgotPasswordText : string.Empty),
                    () => _trSignupText.Visible.ShouldBe(signUpVisible),
                    () => _trSignupURL.Visible.ShouldBe(signUpVisible),
                    () => _revSignupURL.Enabled.ShouldBe(signUpVisible),
                    () => _txtSignupText.Text.ShouldBe(signUpVisible ? DOH_SignupText : string.Empty),
                    () => _txtSignupURL.Text.ShouldBe(signUpVisible ? DOH_SignupURL : string.Empty),
                    () => _pnlCSS.Visible.ShouldBe(selectedStyle != "default"),
                    () => _revStyle.Enabled.ShouldBe(selectedStyle != "default"),
                    () => _txtStyle.Text.ShouldBe(expectedStyle),
                    () => _pnlRedirect.Visible.ShouldBe(selectedConfirmation != "page"),
                    () => _pnlAutoRedirect.Visible.ShouldBe(selectedConfirmation == "pagewithautoredirect"),
                    () => _txtConfirmationRedirectURL.Text.ShouldBe(selectedConfirmation == "page" ? string.Empty : DOH_RedirectURL),
                    () => _txtConfirmationMessage.Text.ShouldBe(DOH_ConfirmationMessage),
                    () => _txtConfirmationText.Text.ShouldBe(DOH_ConfirmationText),
                    () => _ddlConfirmationRedirectDelay.SelectedIndex.ShouldBe(selectedConfirmation == "pagewithautoredirect" ? DOH_RedirectDelay : 0),
                    () => _revConfirmationMessage.Enabled.ShouldBe(true),
                    () => _revConfirmationRedirectURL.Enabled.ShouldBe(selectedConfirmation != "page"),
                    () => _revConfirmationText.Enabled.ShouldBe(true));
        }

        [Test]
        public void DisplayOrHide_LoginNoCustomValidate_ControlsInitializedWithProperValues()
        {
            // Arrange
            InitTestDisplayOrHide(loginOrCapture: "login", gateway: out commEntities.Gateway gateway);
            gateway.ValidateCustom = false;

            // Act
            _gatewaySetupPrivateObject.Invoke("DisplayOrHide", new object[] { gateway });

            // Assert
            gateway.ShouldSatisfyAllConditions(
                 () => _pnlPassword.Visible.ShouldBeTrue(),
                 () => _pnlSignup.Visible.ShouldBeTrue(),
                 () => _pnlLogin.Visible.ShouldBeTrue(),
                 () => _pnlCapture.Visible.ShouldBeFalse(),
                 () => _chkLoginValidatePassword.Checked.ShouldBe(DOH_ValidatePassword),
                 () => _chkLoginValidateCustom.Checked.ShouldBeFalse(),
                 () => _gvCaptureCustom.Visible.ShouldBeFalse(),
                 () => _gvCaptureValues.Visible.ShouldBeFalse(),
                 () => _gvCustomValidate.Visible.ShouldBeFalse());
        }

        [Test]
        public void DisplayOrHide_LoginWithCustomValidate_ValidateDatatableInitializedWithProperValues()
        {
            // Arrange
            InitTestDisplayOrHide(loginOrCapture: "login", gateway: out commEntities.Gateway gateway);
            gateway.ValidateCustom = true;
            var gatewayValue = GenerateGatewayValue_DisplayOrHideTest();
            ShimGatewayValue.GetByGatewayIDInt32 = (id) => new List<commEntities.GatewayValue>() { gatewayValue };
            DataTable customValidateDTWrapper = null;
            ShimGatewaySetup.AllInstances.CustomValidateDTGet = (gws) => customValidateDTWrapper;
            ShimGatewaySetup.AllInstances.CustomValidateDTSetDataTable = (gws, dataTable) => customValidateDTWrapper = dataTable;

            // Act
            _gatewaySetupPrivateObject.Invoke("DisplayOrHide", new object[] { gateway });

            // Assert
            customValidateDTWrapper.Rows.Count.ShouldBe(1);
            var dataRow = customValidateDTWrapper.Rows[0];
            dataRow.ShouldSatisfyAllConditions(
                () => _gvCustomValidate.Visible.ShouldBeTrue(),
                () => dataRow["ID"].ShouldBe(gatewayValue.GatewayValueID.ToString()),
                () => dataRow["Label"].ShouldBe(gatewayValue.Label),
                () => dataRow["Field"].ShouldBe(gatewayValue.Field),
                () => dataRow["Value"].ShouldBe(gatewayValue.Value),
                () => dataRow["IsDeleted"].ShouldBe("false"),
                () => dataRow["Type"].ShouldBe(gatewayValue.FieldType),
                () => dataRow["NOT"].ShouldBe(gatewayValue.NOT.ToString().ToLower()),
                () => dataRow["Comparator"].ShouldBe(gatewayValue.Comparator),
                () => dataRow["IsStatic"].ShouldBe(gatewayValue.IsStatic.ToString().ToLower()));
        }

        [Test]
        public void DisplayOrHide_Capture_CaptureCustomDatatableInitializedWithProperValues()
        {
            // Arrange
            InitTestDisplayOrHide(loginOrCapture: "capture", gateway: out commEntities.Gateway gateway);
            gateway.ValidateCustom = true;
            var gatewayValue = GenerateGatewayValue_DisplayOrHideTest();
            ShimGatewayValue.GetByGatewayIDInt32 = (id) => new List<commEntities.GatewayValue>() { gatewayValue };
            DataTable captureCustomDTWrapper = null;
            ShimGatewaySetup.AllInstances.CaptureCustomDTGet = (gws) => captureCustomDTWrapper;
            ShimGatewaySetup.AllInstances.CaptureCustomDTSetDataTable = (gws, dataTable) => captureCustomDTWrapper = dataTable;

            // Act
            _gatewaySetupPrivateObject.Invoke("DisplayOrHide", new object[] { gateway });

            // Assert
            captureCustomDTWrapper.Rows.Count.ShouldBe(1);
            var dataRow = captureCustomDTWrapper.Rows[0];
            dataRow.ShouldSatisfyAllConditions(
                () => _gvCustomValidate.Visible.ShouldBeFalse(),
                () => _pnlPassword.Visible.ShouldBeFalse(),
                () => _trForgotPasswordText.Visible.ShouldBeFalse(),
                () => _trSignupText.Visible.ShouldBeFalse(),
                () => _trSignupURL.Visible.ShouldBeFalse(),
                () => _pnlSignup.Visible.ShouldBeFalse(),
                () => _pnlLogin.Visible.ShouldBeFalse(),
                () => _pnlCapture.Visible.ShouldBeTrue(),
                () => _rblForgotPasswordVisible.SelectedValue.ShouldBe("false"),
                () => _rblSignupVisible.SelectedValue.ShouldBe("false"),
                () => dataRow["ID"].ShouldBe(gatewayValue.GatewayValueID.ToString()),
                () => dataRow["Label"].ShouldBe(gatewayValue.Label),
                () => dataRow["Field"].ShouldBe(gatewayValue.Field),
                () => dataRow["Value"].ShouldBe(gatewayValue.Value),
                () => dataRow["IsDeleted"].ShouldBe("false"),
                () => dataRow["IsStatic"].ShouldBe(gatewayValue.IsStatic.ToString().ToLower()));
        }

        private commEntities.GatewayValue GenerateGatewayValue_DisplayOrHideTest()
        {
            return new commEntities.GatewayValue()
            {
                IsLoginValidator = true,
                IsCaptureValue = true,
                GatewayValueID = 10,
                Label = "Label",
                Field = "",
                Value = "",
                FieldType = "",
                NOT = true,
                Comparator = "Comparator",
                IsStatic = true
            };
        }

        private void InitTestDisplayOrHide(out commEntities.Gateway gateway, bool forgotPasswordVisible = true, bool signupVisible = true, string styleSelectedValue = "default", string confirmationPage = "page", string loginOrCapture = "login")
        {
            gateway = new commEntities.Gateway()
            {
                GatewayID = 10,
                ConfirmationMessage = DOH_ConfirmationMessage,
                ConfirmationText = DOH_ConfirmationText,
                ForgotPasswordText = DOH_ForgotPasswordText,
                ValidatePassword = DOH_ValidatePassword,
                SignupText = DOH_SignupText,
                SignupURL = DOH_SignupURL,
                RedirectDelay = DOH_RedirectDelay,
                Style = DOH_Style,
                RedirectURL = DOH_RedirectURL
            };
            SetDisplayOrHideTest_PageControls();
            _gatewaySetupPrivateObject.SetField("DataFilePath", BindingFlags.Instance | BindingFlags.NonPublic, DOH_DataFilePath);
            _rblForgotPasswordVisible.Items.FindByValue(forgotPasswordVisible.ToString().ToLower()).Selected = true;
            _rblSignupVisible.Items.FindByValue(signupVisible.ToString().ToLower()).Selected = true;
            _rblStyling.Items.FindByValue(styleSelectedValue).Selected = true;
            _rblConfirmationPage.Items.FindByValue(confirmationPage).Selected = true;
            _rblLoginOrCapture.Items.FindByValue(loginOrCapture).Selected = true;
        }

        private void SetDisplayOrHideTest_PageControls()
        {
            _txtForgotPasswordText = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtForgotPasswordText", BindingFlags.Instance | BindingFlags.NonPublic, _txtForgotPasswordText);
            _txtSignupText = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtSignupText", BindingFlags.Instance | BindingFlags.NonPublic, _txtSignupText);
            _txtStyle = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtStyle", BindingFlags.Instance | BindingFlags.NonPublic, _txtStyle);
            _txtSignupURL = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtSignupURL", BindingFlags.Instance | BindingFlags.NonPublic, _txtSignupURL);
            _txtConfirmationMessage = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtConfirmationMessage", BindingFlags.Instance | BindingFlags.NonPublic, _txtConfirmationMessage);
            _txtConfirmationText = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtConfirmationText", BindingFlags.Instance | BindingFlags.NonPublic, _txtConfirmationText);
            _txtConfirmationRedirectURL = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtConfirmationRedirectURL", BindingFlags.Instance | BindingFlags.NonPublic, _txtConfirmationRedirectURL);
            _rblForgotPasswordVisible = new RadioButtonList();
            _rblForgotPasswordVisible.Items.Add(new ListItem("true", "true"));
            _rblForgotPasswordVisible.Items.Add(new ListItem("false", "false"));
            _gatewaySetupPrivateObject.SetField("rblForgotPasswordVisible", BindingFlags.Instance | BindingFlags.NonPublic, _rblForgotPasswordVisible);
            _rblConfirmationPage = new RadioButtonList();
            _rblConfirmationPage.Items.Add(new ListItem("page", "page"));
            _rblConfirmationPage.Items.Add(new ListItem("pagewithredirect", "pagewithredirect"));
            _rblConfirmationPage.Items.Add(new ListItem("pagewithautoredirect", "pagewithautoredirect"));
            _gatewaySetupPrivateObject.SetField("rblConfirmationPage", BindingFlags.Instance | BindingFlags.NonPublic, _rblConfirmationPage);
            _rblSignupVisible = new RadioButtonList();
            _rblSignupVisible.Items.Add(new ListItem("true", "true"));
            _rblSignupVisible.Items.Add(new ListItem("false", "false"));
            _gatewaySetupPrivateObject.SetField("rblSignupVisible", BindingFlags.Instance | BindingFlags.NonPublic, _rblSignupVisible);
            _rblLoginOrCapture = new RadioButtonList();
            _rblLoginOrCapture.Items.Add(new ListItem("login", "login"));
            _rblLoginOrCapture.Items.Add(new ListItem("capture", "capture"));
            _rblLoginOrCapture.Items.Add(new ListItem("skip_", "skip_"));
            _gatewaySetupPrivateObject.SetField("rblLoginOrCapture", BindingFlags.Instance | BindingFlags.NonPublic, _rblLoginOrCapture);
            _rblStyling = new RadioButtonList();
            _rblStyling.Items.Add(new ListItem("default", "default"));
            _rblStyling.Items.Add(new ListItem("external", "external"));
            _rblStyling.Items.Add(new ListItem("upload", "upload"));
            _gatewaySetupPrivateObject.SetField("rblStyling", BindingFlags.Instance | BindingFlags.NonPublic, _rblStyling);
            _ddlConfirmationRedirectDelay = new DropDownList();
            _ddlConfirmationRedirectDelay.Items.Add(new ListItem("0", "0"));
            _ddlConfirmationRedirectDelay.Items.Add(new ListItem("1", "1"));
            _ddlConfirmationRedirectDelay.Items.Add(new ListItem("2", "2"));
            _ddlConfirmationRedirectDelay.SelectedIndex = 1;
            _gatewaySetupPrivateObject.SetField("ddlConfirmationRedirectDelay", BindingFlags.Instance | BindingFlags.NonPublic, _ddlConfirmationRedirectDelay);
            _chkLoginValidatePassword = new CheckBox();
            _gatewaySetupPrivateObject.SetField("chkLoginValidatePassword", BindingFlags.Instance | BindingFlags.NonPublic, _chkLoginValidatePassword);
            _chkLoginValidateCustom = new CheckBox();
            _gatewaySetupPrivateObject.SetField("chkLoginValidateCustom", BindingFlags.Instance | BindingFlags.NonPublic, _chkLoginValidateCustom);
            _trForgotPasswordText = new HtmlTableRow();
            _gatewaySetupPrivateObject.SetField("trForgotPasswordText", BindingFlags.Instance | BindingFlags.NonPublic, _trForgotPasswordText);
            _trSignupURL = new HtmlTableRow();
            _gatewaySetupPrivateObject.SetField("trSignupURL", BindingFlags.Instance | BindingFlags.NonPublic, _trSignupURL);
            _trSignupText = new HtmlTableRow();
            _gatewaySetupPrivateObject.SetField("trSignupText", BindingFlags.Instance | BindingFlags.NonPublic, _trSignupText);
            _pnlPassword = new Panel();
            _gatewaySetupPrivateObject.SetField("pnlPassword", BindingFlags.Instance | BindingFlags.NonPublic, _pnlPassword);
            _pnlSignup = new Panel();
            _gatewaySetupPrivateObject.SetField("pnlSignup", BindingFlags.Instance | BindingFlags.NonPublic, _pnlSignup);
            _pnlLogin = new Panel();
            _gatewaySetupPrivateObject.SetField("pnlLogin", BindingFlags.Instance | BindingFlags.NonPublic, _pnlLogin);
            _pnlCapture = new Panel();
            _gatewaySetupPrivateObject.SetField("pnlCapture", BindingFlags.Instance | BindingFlags.NonPublic, _pnlCapture);
            _pnlCSS = new Panel();
            _gatewaySetupPrivateObject.SetField("pnlCSS", BindingFlags.Instance | BindingFlags.NonPublic, _pnlCSS);
            _pnlRedirect = new Panel();
            _gatewaySetupPrivateObject.SetField("pnlRedirect", BindingFlags.Instance | BindingFlags.NonPublic, _pnlRedirect);
            _pnlAutoRedirect = new Panel();
            _gatewaySetupPrivateObject.SetField("pnlAutoRedirect", BindingFlags.Instance | BindingFlags.NonPublic, _pnlAutoRedirect);
            _gvCaptureCustom = new GridView();
            _gatewaySetupPrivateObject.SetField("gvCaptureCustom", BindingFlags.Instance | BindingFlags.NonPublic, _gvCaptureCustom);
            _gvCaptureValues = new GridView();
            _gatewaySetupPrivateObject.SetField("gvCaptureValues", BindingFlags.Instance | BindingFlags.NonPublic, _gvCaptureValues);
            _gvCustomValidate = new GridView();
            _gvCustomValidate.Visible = false;
            _gatewaySetupPrivateObject.SetField("gvCustomValidate", BindingFlags.Instance | BindingFlags.NonPublic, _gvCustomValidate);
            _revForgotPassword = new RequiredFieldValidator();
            _gatewaySetupPrivateObject.SetField("revForgotPassword", BindingFlags.Instance | BindingFlags.NonPublic, _revForgotPassword);
            _revSignupURL = new RequiredFieldValidator();
            _gatewaySetupPrivateObject.SetField("revSignupURL", BindingFlags.Instance | BindingFlags.NonPublic, _revSignupURL);
            _revStyle = new RequiredFieldValidator();
            _gatewaySetupPrivateObject.SetField("revStyle", BindingFlags.Instance | BindingFlags.NonPublic, _revStyle);
            _revConfirmationMessage = new RequiredFieldValidator();
            _revConfirmationMessage.Enabled = false;
            _gatewaySetupPrivateObject.SetField("revConfirmationMessage", BindingFlags.Instance | BindingFlags.NonPublic, _revConfirmationMessage);
            _revConfirmationRedirectURL = new RegularExpressionValidator();
            _gatewaySetupPrivateObject.SetField("revConfirmationRedirectURL", BindingFlags.Instance | BindingFlags.NonPublic, _revConfirmationRedirectURL);
            _revConfirmationText = new RequiredFieldValidator();
            _revConfirmationText.Enabled = false;
            _gatewaySetupPrivateObject.SetField("revConfirmationText", BindingFlags.Instance | BindingFlags.NonPublic, _revConfirmationText);
            _updatePanel = new UpdatePanel();
            _updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
            _gatewaySetupPrivateObject.SetField("UpdatePanel1", BindingFlags.Instance | BindingFlags.NonPublic, _updatePanel);
        }
    }
}
