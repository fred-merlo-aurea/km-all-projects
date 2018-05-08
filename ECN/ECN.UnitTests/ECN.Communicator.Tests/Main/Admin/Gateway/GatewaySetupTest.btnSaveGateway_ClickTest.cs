using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.admin.Gateway.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using commEntities = ECN_Framework_Entities.Communicator;
using ShimPage = System.Web.UI.Fakes.ShimPage;

namespace ECN.Communicator.Tests.Main.Admin.Gateway
{
    public partial class GatewaySetupTest
    {
        private const int SG_CustomerID = 1;
        private const string SG_GatewayName = "GatewayName";
        private const string SG_GatewayPubCode = "GatewayPubCode";
        private const string SG_GatewayTypeCode = "GatewayTypeCode";
        private const string SG_GatewayHeaderText = "GatewayHeaderText";
        private const string SG_GatewayFooterText = "GatewayFooterText";
        private const string SG_SubmitText = "SubmitText";
        private const string SG_ForgotPasswordText = "ForgotPasswordText";
        private const string SG_SignupText = "SignupText";
        private const string SG_SignupURLText = "SignupURLText";
        private const string SG_StyleText = "StyleText";
        private const string SG_ConfirmationMessageText = "ConfirmationMessageText";
        private const string SG_ConfirmationText = "ConfirmationText";
        private const string SG_ConfirmationRedirectURLText = "ConfirmationRedirectURLText";
        private const string SG_DataFilePath = "DataFilePath";
        private const int SG_GatewayId = 10;
        private HiddenField _hfSelectGroupID;
        private TextBox _txtGatewayName;
        private TextBox _txtGatewayPubCode;
        private TextBox _txtGatewayTypeCode;
        private TextBox _txtGatewayHeader;
        private TextBox _txtGatewayFooter;
        private TextBox _txtSubmitText;

        [TestCase("0")]
        [TestCase("")]
        public void BtnSaveGateway_Click_InvalidSelectedGroup_Error(string selectedGroup)
        {
            // Arrange
            InitTestBtnSaveGatewayClick(selectedGroupId: selectedGroup, gatewayID: 0, gateway: null);

            var expectedError = $"<br/>{Enums.Entity.Gateway}: Please select a group";

            // Act
            _gatewaySetupPrivateObject.Invoke("btnSaveGateway_Click", new object[] { null, null });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(expectedError));
        }

        [TestCase(0, true, false, "default", "page", "login")]
        [TestCase(1, false, true, "external", "pagewithredirect", "login")]
        [TestCase(1, false, true, "upload", "pagewithautoredirect", "capture")]
        public void BtnSaveGateway_Click_DifferentInitializers_GatewayInitializedWithProperValues(int gatewayId, bool forgotPasswordVisibile, bool signUpVisible, string selectedStyle, string selectedConfirmation, string loginOrCaptureValue)
        {
            // Arrange
            var gateway = new commEntities.Gateway() { GatewayID = gatewayId };
            InitTestBtnSaveGatewayClick(
                selectedGroupId: "10", 
                gatewayID: gatewayId, 
                gateway: gateway, 
                chkLoginValidateCustom: true,
                chkLoginValidatePassword: true, 
                confirmationPage: selectedConfirmation, 
                forgotPasswordVisible: forgotPasswordVisibile,
                loginOrCapture: loginOrCaptureValue, 
                signupVisible: signUpVisible, 
                styleSelectedValue: selectedStyle);
            var ecnError = new ECNError(Enums.Entity.Gateway, Enums.Method.Save, "saveGatewayError");
            var expectedError = $"<br/>{Enums.Entity.Gateway}: saveGatewayError";
            var expectedGatewayStyle = string.Empty;
            if (selectedStyle == "external")
            {
                expectedGatewayStyle = SG_StyleText;
            }
            else if (selectedStyle == "upload")
            {
                expectedGatewayStyle = $"{SG_DataFilePath}/{SG_StyleText}";
            }
            var expectedUseRedirect = selectedConfirmation != "page";
            var expectedUseConfirmation = selectedConfirmation != "pagewithredirect";
            ShimGateway.SaveGatewayUser = (savedGateway, u) =>
            {
                gateway = savedGateway;
                throw new ECNException(new List<ECNError>() { ecnError });
            };

            // Act
            _gatewaySetupPrivateObject.Invoke("btnSaveGateway_Click", new object[] { null, null });

            // Assert
            _phError.ShouldSatisfyAllConditions(
              () => _phError.Visible.ShouldBeTrue(),
              () => _lblErrorMessage.Text.ShouldBe(expectedError),
              () => gateway.Name.ShouldBe(SG_GatewayName),
              () => gateway.CustomerID.ShouldBe(SG_CustomerID),
              () => gateway.GroupID.ShouldBe(10),
              () => gateway.PubCode.ShouldBe(SG_GatewayPubCode),
              () => gateway.TypeCode.ShouldBe(SG_GatewayTypeCode),
              () => gateway.Header.ShouldBe(SG_GatewayHeaderText),
              () => gateway.Footer.ShouldBe(SG_GatewayFooterText),
              () => gateway.SubmitText.ShouldBe(SG_SubmitText),
              () => gateway.ShowForgotPassword.ShouldBe(forgotPasswordVisibile),
              () => gateway.ForgotPasswordText.ShouldBe(forgotPasswordVisibile ? SG_ForgotPasswordText : string.Empty),
              () => gateway.ShowSignup.ShouldBe(signUpVisible),
              () => gateway.SignupText.ShouldBe(signUpVisible ? SG_SignupText : string.Empty),
              () => gateway.SignupURL.ShouldBe(signUpVisible ? SG_SignupURLText : string.Empty),
              () => gateway.UseStyleFrom.ShouldBe(selectedStyle),
              () => gateway.Style.ShouldBe(expectedGatewayStyle),
              () => gateway.ConfirmationMessage.ShouldBe(SG_ConfirmationMessageText),
              () => gateway.ConfirmationText.ShouldBe(selectedConfirmation == "page" ? string.Empty : SG_ConfirmationText),
              () => gateway.RedirectURL.ShouldBe(selectedConfirmation == "page" ? string.Empty : SG_ConfirmationRedirectURLText),
              () => gateway.RedirectDelay.ShouldBe(selectedConfirmation == "pagewithautoredirect" ? 1 : 0),
              () => gateway.LoginOrCapture.ShouldBe(loginOrCaptureValue),
              () => gateway.ValidateEmail.ShouldBe(loginOrCaptureValue == "login"),
              () => gateway.ValidatePassword.ShouldBe(loginOrCaptureValue == "login"),
              () => gateway.ValidateCustom.ShouldBe(loginOrCaptureValue == "login"));
        }

        [TestCase("10", true, true, false)]
        [TestCase("10", false, false, true)]
        [TestCase("10-0", true, true, false)]
        [TestCase("10-0", false, false, true)]
        [TestCase("10-0", false, true, false)]
        public void BtnSaveGateway_Click_Login(string id, bool isDeleted, bool isStatic, bool isNot)
        {
            // Arrange
            var gateway = new commEntities.Gateway() { GatewayID = SG_GatewayId };
            commEntities.GatewayValue gatewayValue = null;
            InitTestBtnSaveGatewayClick(selectedGroupId: "10", gatewayID: SG_GatewayId, gateway: gateway, chkLoginValidateCustom: true, chkLoginValidatePassword: true, loginOrCapture: "login");
            InitCustomValidateDT(idColumnValue: id, isDeletedColumnValue: isDeleted.ToString().ToLower(), isStaticColumnValue: isStatic.ToString().ToLower(), notColumnValue: isNot.ToString().ToLower());
            ShimGateway.SaveGatewayUser = (g, u) => SG_GatewayId;
            ShimGatewayValue.SaveGatewayValueUser = (gwv, u) =>
            {
                gatewayValue = gwv;
                return 1;
            };

            // Act
            _gatewaySetupPrivateObject.Invoke("btnSaveGateway_Click", new object[] { null, null });

            // Assert
            if (id.Contains("-") && !isDeleted)
            {
                gatewayValue.ShouldSatisfyAllConditions(
                 () => _gatewayValueWipeOutValuesMethodCallCount.ShouldBe(1),
                 () => gatewayValue.IsLoginValidator.ShouldBeTrue(),
                 () => gatewayValue.Field.ShouldBe("Field"),
                 () => gatewayValue.Value.ShouldBe("Value"),
                 () => gatewayValue.Comparator.ShouldBe("Comparator"),
                 () => gatewayValue.NOT.ShouldBe(isNot),
                 () => gatewayValue.GatewayID.ShouldBe(SG_GatewayId),
                 () => gatewayValue.FieldType.ShouldBe("Type"),
                 () => gatewayValue.Label.ShouldBe("Label"),
                 () => gatewayValue.IsStatic.ShouldBe(isStatic));
            }
            else if (!id.Contains("-") && isDeleted)
            {
                _gatewayValueDeleteByIDMethodCallCount.ShouldBe(1);
            }
        }

        [TestCase("10", true, true)]
        [TestCase("10", false, false)]
        [TestCase("10-0", true, true)]
        [TestCase("10-0", false, false)]
        [TestCase("10-0", false, true)]
        public void BtnSaveGateway_Click_Capture(string id, bool isDeleted, bool isStatic)
        {
            // Arrange
            var gateway = new commEntities.Gateway() { GatewayID = SG_GatewayId };
            commEntities.GatewayValue gatewayValue = null;
            InitTestBtnSaveGatewayClick(selectedGroupId: "10", gatewayID: SG_GatewayId, gateway: gateway, chkLoginValidateCustom: true, chkLoginValidatePassword: true, loginOrCapture: "capture");
            InitCaptureCustomDT(idColumnValue: id, isDeletedColumnValue: isDeleted.ToString().ToLower(), isStaticColumnValue: isStatic.ToString().ToLower());
            ShimGateway.SaveGatewayUser = (g, u) => SG_GatewayId;
            ShimGatewayValue.SaveGatewayValueUser = (gwv, u) =>
            {
                gatewayValue = gwv;
                return 1;
            };
            // Act
            _gatewaySetupPrivateObject.Invoke("btnSaveGateway_Click", new object[] { null, null });

            // Assert
            if (id.Contains("-") && !isDeleted)
            {
                gatewayValue.ShouldSatisfyAllConditions(
                 () => _gatewayValueWipeOutValuesMethodCallCount.ShouldBe(1),
                 () => gatewayValue.Field.ShouldBe("Field"),
                 () => gatewayValue.Value.ShouldBe("Value"),
                 () => gatewayValue.GatewayID.ShouldBe(SG_GatewayId),
                 () => gatewayValue.Label.ShouldBe("Label"),
                 () => gatewayValue.IsCaptureValue.ShouldBeTrue(),
                 () => gatewayValue.IsStatic.ShouldBe(isStatic));
            }
            else if (!id.Contains("-") && isDeleted)
            {
                _gatewayValueDeleteByIDMethodCallCount.ShouldBe(1);
            }
        }

        [TestCase("login")]
        [TestCase("capture")]
        public void BtnSaveGateway_Click_GateWayValueSaveError_Error(string loginOrCapture)
        {
            // Arrange
            ShimGateway.SaveGatewayUser = (g, u) => 1;
            var ecnError = new ECNError(Enums.Entity.GatewayValue, Enums.Method.Save, "saveGatewayValueError");
            var expectedError = $"<br/>{Enums.Entity.GatewayValue}: saveGatewayValueError";
            ShimGatewayValue.SaveGatewayValueUser = (gwv, u) =>
            {
                throw new ECNException(new List<ECNError>() { ecnError });
            };
            InitTestBtnSaveGatewayClick(selectedGroupId: "10", gatewayID: 10, gateway: new commEntities.Gateway() { GatewayID = 10 }, chkLoginValidateCustom: true, chkLoginValidatePassword: true, loginOrCapture: loginOrCapture);
            if (loginOrCapture == "login")
            {
                InitCustomValidateDT(idColumnValue: "10-0", isDeletedColumnValue: "false", isStaticColumnValue: "false", notColumnValue: "false");
            }
            else
            {
                InitCaptureCustomDT(idColumnValue: "10-0", isDeletedColumnValue: "false", isStaticColumnValue: "false");
            }

            // Act
            _gatewaySetupPrivateObject.Invoke("btnSaveGateway_Click", new object[] { null, null });

            // Assert
            _phError.ShouldSatisfyAllConditions(
              () => _phError.Visible.ShouldBeTrue(),
              () => _lblErrorMessage.Text.ShouldBe(expectedError));
        }

        private void InitTestBtnSaveGatewayClick(string selectedGroupId, int gatewayID, commEntities.Gateway gateway, bool chkLoginValidatePassword = false, bool chkLoginValidateCustom = false, bool forgotPasswordVisible = true, bool signupVisible = true, string styleSelectedValue = "default", string confirmationPage = "page", string loginOrCapture = "login", string redirectDelayValue = "1")
        {
            SetBtnSaveGatewayClickTest_PageProperties(0);
            SetBtnSaveGatewayClickTest_PageControls();
            _hfSelectGroupID.Value = selectedGroupId;
            ShimGatewaySetup.AllInstances.GatewayIDGet = (gw) => gatewayID;
            ShimGateway.GetByGatewayIDInt32 = (id) => gateway;
            ShimGatewayValue.WipeOutValuesBooleanInt32 = (b, i) => _gatewayValueWipeOutValuesMethodCallCount++;
            ShimGatewayValue.DeleteByIDInt32 = (i) => _gatewayValueDeleteByIDMethodCallCount++;
            _txtGatewayName.Text = SG_GatewayName;
            _txtGatewayPubCode.Text = SG_GatewayPubCode;
            _txtGatewayTypeCode.Text = SG_GatewayTypeCode;
            _txtGatewayHeader.Text = SG_GatewayHeaderText;
            _txtGatewayFooter.Text = SG_GatewayFooterText;
            _txtSubmitText.Text = SG_SubmitText;
            _txtForgotPasswordText.Text = SG_ForgotPasswordText;
            _txtSignupText.Text = SG_SignupText;
            _txtSignupURL.Text = SG_SignupURLText;
            _txtStyle.Text = SG_StyleText;
            _txtConfirmationMessage.Text = SG_ConfirmationMessageText;
            _txtConfirmationText.Text = SG_ConfirmationText;
            _txtConfirmationRedirectURL.Text = SG_ConfirmationRedirectURLText;
            _chkLoginValidateCustom.Checked = chkLoginValidateCustom;
            _chkLoginValidatePassword.Checked = chkLoginValidatePassword;
            _gatewaySetupPrivateObject.SetField("DataFilePath", BindingFlags.Instance | BindingFlags.NonPublic, SG_DataFilePath);
            _ddlConfirmationRedirectDelay.Items.FindByValue(redirectDelayValue).Selected = true;
            _rblForgotPasswordVisible.Items.FindByValue(forgotPasswordVisible.ToString().ToLower()).Selected = true;
            _rblSignupVisible.Items.FindByValue(signupVisible.ToString().ToLower()).Selected = true;
            _rblStyling.Items.FindByValue(styleSelectedValue).Selected = true;
            _rblConfirmationPage.Items.FindByValue(confirmationPage).Selected = true;
            _rblLoginOrCapture.Items.FindByValue(loginOrCapture).Selected = true;
        }
        private void SetBtnSaveGatewayClickTest_PageProperties(int userId)
        {
            var shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new User() { UserID = userId };
            shimECNSession.Instance.CurrentCustomer = new Customer() { CustomerID = SG_CustomerID };
            ShimCommunicator.AllInstances.UserSessionGet = (c) => shimECNSession;
            ShimGatewaySetup.AllInstances.MasterGet = (mt) => new ecn.communicator.MasterPages.Communicator();
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (h, u, b) => { };
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            ShimPage.AllInstances.RequestGet = (p) => new ShimHttpRequest();
        }
        private void SetBtnSaveGatewayClickTest_PageControls()
        {
            _hfSelectGroupID = new HiddenField();
            _gatewaySetupPrivateObject.SetField("hfSelectGroupID", BindingFlags.Instance | BindingFlags.NonPublic, _hfSelectGroupID);
            _txtGatewayName = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtGatewayName", BindingFlags.Instance | BindingFlags.NonPublic, _txtGatewayName);
            _txtGatewayPubCode = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtGatewayPubCode", BindingFlags.Instance | BindingFlags.NonPublic, _txtGatewayPubCode);
            _txtGatewayTypeCode = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtGatewayTypeCode", BindingFlags.Instance | BindingFlags.NonPublic, _txtGatewayTypeCode);
            _txtGatewayHeader = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtGatewayHeader", BindingFlags.Instance | BindingFlags.NonPublic, _txtGatewayHeader);
            _txtGatewayFooter = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtGatewayFooter", BindingFlags.Instance | BindingFlags.NonPublic, _txtGatewayFooter);
            _txtSubmitText = new TextBox();
            _gatewaySetupPrivateObject.SetField("txtSubmitText", BindingFlags.Instance | BindingFlags.NonPublic, _txtSubmitText);
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
            _gatewaySetupPrivateObject.SetField("rblLoginOrCapture", BindingFlags.Instance | BindingFlags.NonPublic, _rblLoginOrCapture);
            _rblStyling = new RadioButtonList();
            _rblStyling.Items.Add(new ListItem("default", "default"));
            _rblStyling.Items.Add(new ListItem("external", "external"));
            _rblStyling.Items.Add(new ListItem("upload", "upload"));
            _gatewaySetupPrivateObject.SetField("rblStyling", BindingFlags.Instance | BindingFlags.NonPublic, _rblStyling);
            _ddlConfirmationRedirectDelay = new DropDownList();
            _ddlConfirmationRedirectDelay.Items.Add(new ListItem("1", "1"));
            _ddlConfirmationRedirectDelay.Items.Add(new ListItem("2", "2"));
            _gatewaySetupPrivateObject.SetField("ddlConfirmationRedirectDelay", BindingFlags.Instance | BindingFlags.NonPublic, _ddlConfirmationRedirectDelay);
            _chkLoginValidatePassword = new CheckBox();
            _gatewaySetupPrivateObject.SetField("chkLoginValidatePassword", BindingFlags.Instance | BindingFlags.NonPublic, _chkLoginValidatePassword);
            _chkLoginValidateCustom = new CheckBox();
            _gatewaySetupPrivateObject.SetField("chkLoginValidateCustom", BindingFlags.Instance | BindingFlags.NonPublic, _chkLoginValidateCustom);
        }
        private void InitCustomValidateDT(string idColumnValue, string isDeletedColumnValue, string isStaticColumnValue, string notColumnValue)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("IsDeleted");
            dataTable.Columns.Add("Field");
            dataTable.Columns.Add("Value");
            dataTable.Columns.Add("Comparator");
            dataTable.Columns.Add("NOT");
            dataTable.Columns.Add("Type");
            dataTable.Columns.Add("IsStatic");
            dataTable.Columns.Add("Label");
            var row = dataTable.NewRow();
            row["ID"] = idColumnValue;
            row["IsDeleted"] = isDeletedColumnValue;
            row["Field"] = "Field";
            row["Value"] = "Value";
            row["Comparator"] = "Comparator";
            row["NOT"] = notColumnValue;
            row["Type"] = "Type";
            row["IsStatic"] = isStaticColumnValue;
            row["Label"] = "Label";
            dataTable.Rows.Add(row);
            ShimGatewaySetup.AllInstances.CustomValidateDTGet = (gws) => dataTable;
        }
        private void InitCaptureCustomDT(string idColumnValue, string isDeletedColumnValue, string isStaticColumnValue)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("IsDeleted");
            dataTable.Columns.Add("Field");
            dataTable.Columns.Add("Value");
            dataTable.Columns.Add("IsStatic");
            dataTable.Columns.Add("Label");
            var row = dataTable.NewRow();
            row["ID"] = idColumnValue;
            row["IsDeleted"] = isDeletedColumnValue;
            row["Field"] = "Field";
            row["Value"] = "Value";
            row["IsStatic"] = isStaticColumnValue;
            row["Label"] = "Label";
            dataTable.Rows.Add(row);
            ShimGatewaySetup.AllInstances.CaptureCustomDTGet = (gws) => dataTable;
        }
    }
}
