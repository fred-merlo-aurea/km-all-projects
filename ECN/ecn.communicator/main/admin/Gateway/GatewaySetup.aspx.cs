using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using KM.Common.Extensions;
using FrameworkGateway = ECN_Framework_Entities.Communicator.Gateway;
using GatewayValue = ECN_Framework_BusinessLayer.Communicator.GatewayValue;

namespace ecn.communicator.main.admin.Gateway
{
    public partial class GatewaySetup : ECN_Framework.WebPageHelper
    {
        private const string Zero = "0";
        private const string TrueValue = "true";
        private const string Default = "default";
        private const string External = "external";
        private const string Upload = "upload";
        private const string NamePage = "page";
        private const string PageWithRedirect = "pagewithredirect";
        private const string PageWithAutoRedirect = "pagewithautoredirect";
        private const string Login = "login";
        private const string Capture = "capture";
        private const string Id = "ID";
        private const string Hyphen = "-";
        private const string IsDeleted = "IsDeleted";
        private const string FalseValue = "false";
        private const string Field = "Field";
        private const string Value = "Value";
        private const string NameLabel = "Label";
        private const string IsStatic = "IsStatic";
        private const string GateWayListUrl = "GatewayList.aspx";
        private const string SelectGroupMessage = "Please select a group";
        private const string NameComparator = "Comparator";
        private const string NameNot = "NOT";
        private const string NameType = "Type";
        private string FileToUpload
        {
            get
            {
                if (ViewState["FileToUpload"] != null)
                    return ViewState["FileToUpload"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileToUpload"] = value;
            }
        }
        private DataTable CaptureCustomDT
        {
            get
            {
                try
                {

                    return (DataTable)ViewState["CaptureCustomDT"];
                }
                catch { return null; }
            }
            set
            {
                ViewState["CaptureCustomDT"] = value;
            }
        }
        private DataTable CustomValidateDT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["CustomValidateDT"];
                }
                catch { return null; }
            }
            set
            {
                ViewState["CustomValidateDT"] = value;
            }
        }
        private int GatewayID
        {
            get
            {
                if (Request.QueryString["GatewayID"] != null)
                    return Convert.ToInt32(Request.QueryString["GatewayID"].ToString());
                else
                    return -1;
            }
        }
        string DataFilePath = "";
        delegate void HidePopup();

        private static bool ShowUpload;

        private static readonly Func<RadioButtonList, string, bool> SelectedValueEqualsIgnoreCase = (radioButtonList, compareValue) => radioButtonList.SelectedValue.EqualsIgnoreCase(compareValue);
        private static readonly Func<RadioButtonList, bool> SelectedValueIsTrue = (radioButtonList) => SelectedValueEqualsIgnoreCase(radioButtonList, TrueValue);

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.MasterRegisterButtonForPostBack(hlKMDefault);
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.groupExplorer.hideGroupsLookupPopup = delGroupsLookupPopup;
            phError.Visible = false;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CUSTOMER;
            string channelID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();

            DataFilePath = "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/data";

            if (!Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath)))
                Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath));

            uploader.uploadDirectory = DataFilePath;
            if (!Page.IsPostBack)
            {
                if (false == KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                {
                    pnlNoAccess.Visible = true;
                    pnlSettings.Visible = false;
                    Label1.Text = "You do not have access to this page because you are not an Administrator.";
                    return;
                }

                if (GatewayID > 0)
                {
                    pnlNoAccess.Visible = false;
                    pnlSettings.Visible = true;
                    txtGatewayName.Enabled = false;
                    rfvGatewayName.Enabled = false;
                    LoadGateway(GatewayID);
                }
                else
                {
                    pnlNoAccess.Visible = false;
                    pnlSettings.Visible = true;
                    DisplayOrHide(new ECN_Framework_Entities.Communicator.Gateway());
                    txtGatewayName.Enabled = true;
                }
            }
            else
            {
                if (ShowUpload)
                {
                    mpeUploadControl.Show();
                }
            }

        }
        private void GroupsLookupPopupHide()
        {
            groupExplorer.Visible = false;
            btnCloseGroupExplorer.Visible = false;
        }

        private void LoadGateway(int gatewayID)
        {
            ECN_Framework_Entities.Communicator.Gateway current = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByGatewayID(gatewayID);
            if (current != null)
            {
                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(current.GroupID, Master.UserSession.CurrentUser);
                if (g != null && g.GroupID > 0)
                {
                    txtGatewayGroup.Text = g.GroupName;
                    hfSelectGroupID.Value = g.GroupID.ToString();
                }
                txtGatewayName.Text = current.Name;

                txtGatewayPubCode.Text = current.PubCode;
                txtGatewayTypeCode.Text = current.TypeCode;
                txtGatewayHeader.Text = current.Header;
                txtGatewayFooter.Text = current.Footer;
                txtSubmitText.Text = current.SubmitText;

                rblForgotPasswordVisible.SelectedValue = current.ShowForgotPassword.ToString().ToLower();
                if (current.UseConfirmation && !current.UseRedirect)
                    rblConfirmationPage.SelectedValue = "page";
                else if (current.UseConfirmation && current.UseRedirect)
                    rblConfirmationPage.SelectedValue = "pagewithautoredirect";
                else if (!current.UseConfirmation && current.UseRedirect)
                    rblConfirmationPage.SelectedValue = "pagewithredirect";

                rblLoginOrCapture.SelectedValue = current.LoginOrCapture;

                rblSignupVisible.SelectedValue = current.ShowSignup.ToString().ToLower();

                rblStyling.SelectedValue = current.UseStyleFrom.ToLower();

                DisplayOrHide(current);

                List<ECN_Framework_Entities.Communicator.GatewayValue> gatewayValues = ECN_Framework_BusinessLayer.Communicator.GatewayValue.GetByGatewayID(current.GatewayID);

            }
        }

        private void DisplayOrHide(FrameworkGateway current)
        {
            var isRblForgotPasswordVisible = SelectedValueIsTrue(rblForgotPasswordVisible);
            var isRblSignupVisible = SelectedValueIsTrue(rblSignupVisible);

            trForgotPasswordText.Visible = isRblForgotPasswordVisible;
            txtForgotPasswordText.Text = isRblForgotPasswordVisible
                                             ? current.ForgotPasswordText
                                             : string.Empty;
            revForgotPassword.Enabled = isRblForgotPasswordVisible;
            txtSignupText.Text = isRblSignupVisible
                                     ? current.SignupText
                                     : string.Empty;
            txtSignupURL.Text = isRblSignupVisible
                                    ? current.SignupURL
                                    : string.Empty;
            trSignupText.Visible = isRblSignupVisible;
            trSignupURL.Visible = isRblSignupVisible;
            revSignupURL.Enabled = isRblSignupVisible;

            if (SelectedValueEqualsIgnoreCase(rblStyling, Default))
            {
                pnlCSS.Visible = false;
                revStyle.Enabled = false;
            }
            else if (SelectedValueEqualsIgnoreCase(rblStyling, External))
            {
                pnlCSS.Visible = true;
                txtStyle.Text = current.Style;
                revStyle.Enabled = true;
            }
            else if (SelectedValueEqualsIgnoreCase(rblStyling, Upload))
            {
                pnlCSS.Visible = true;
                txtStyle.Text = current.Style.Replace($"{DataFilePath}/", string.Empty);
                revStyle.Enabled = true;
            }

            DisplayOrHideConfirmationPage(current);

            if (SelectedValueEqualsIgnoreCase(rblLoginOrCapture, Login))
            {
                DisplayOrHideLogin(current);
            }
            else if (SelectedValueEqualsIgnoreCase(rblLoginOrCapture, Capture))
            {
                DisplayOrHideCapture(current);
            }
        }

        private void DisplayOrHideCapture(FrameworkGateway current)
        {
            pnlPassword.Visible = false;
            rblForgotPasswordVisible.SelectedValue = FalseValue;
            trForgotPasswordText.Visible = false;
            rblSignupVisible.SelectedValue = FalseValue;
            trSignupText.Visible = false;
            trSignupURL.Visible = false;
            pnlSignup.Visible = false;
            pnlLogin.Visible = false;
            pnlCapture.Visible = true;
            gvCustomValidate.Visible = false;

            var gatewayValues = GatewayValue.GetByGatewayID(current.GatewayID)
                .Where(x => x.IsCaptureValue)
                .ToList();

            if (CaptureCustomDT == null)
            {
                CaptureCustomDT = new DataTable();
                CaptureCustomDT.Columns.Add(Id);
                CaptureCustomDT.Columns.Add(NameLabel);
                CaptureCustomDT.Columns.Add(Field);
                CaptureCustomDT.Columns.Add(Value);
                CaptureCustomDT.Columns.Add(IsDeleted);
                CaptureCustomDT.Columns.Add(IsStatic);
            }

            foreach (var gatewayValue in gatewayValues)
            {
                var dataRow = CaptureCustomDT.NewRow();
                dataRow[Id] = gatewayValue.GatewayValueID.ToString();
                dataRow[NameLabel] = gatewayValue.Label;
                dataRow[Field] = gatewayValue.Field;
                dataRow[Value] = gatewayValue.Value;
                dataRow[IsDeleted] = FalseValue;
                dataRow[IsStatic] = gatewayValue.IsStatic.ToString()
                    .ToLower();
                CaptureCustomDT.Rows.Add(dataRow);
            }

            LoadGVCaptureCustom();
        }

        private void DisplayOrHideLogin(FrameworkGateway current)
        {
            pnlPassword.Visible = true;
            pnlSignup.Visible = true;
            pnlLogin.Visible = true;
            pnlCapture.Visible = false;
            chkLoginValidatePassword.Checked = current.ValidatePassword;
            chkLoginValidateCustom.Checked = current.ValidateCustom;
            gvCaptureCustom.Visible = false;
            gvCaptureValues.Visible = false;

            if (current.ValidateCustom)
            {
                gvCustomValidate.Visible = true;
                var gatewayValues = GatewayValue.GetByGatewayID(current.GatewayID).Where(x => x.IsLoginValidator).ToList();

                if (CustomValidateDT == null)
                {
                    CustomValidateDT = new DataTable();
                    CustomValidateDT.Columns.Add(Id);
                    CustomValidateDT.Columns.Add(NameLabel);
                    CustomValidateDT.Columns.Add(Field);
                    CustomValidateDT.Columns.Add(Value);
                    CustomValidateDT.Columns.Add(IsDeleted);
                    CustomValidateDT.Columns.Add(NameType);
                    CustomValidateDT.Columns.Add(NameNot);
                    CustomValidateDT.Columns.Add(NameComparator);
                    CustomValidateDT.Columns.Add(IsStatic);
                }

                foreach (var gatewayValue in gatewayValues)
                {
                    var dataRow = CustomValidateDT.NewRow();
                    dataRow[Id] = gatewayValue.GatewayValueID.ToString();
                    dataRow[NameLabel] = gatewayValue.Label;
                    dataRow[Field] = gatewayValue.Field;
                    dataRow[Value] = gatewayValue.Value;
                    dataRow[IsDeleted] = FalseValue;
                    dataRow[NameType] = gatewayValue.FieldType;
                    dataRow[NameNot] = gatewayValue.NOT.ToString().ToLower();
                    dataRow[NameComparator] = gatewayValue.Comparator;
                    dataRow[IsStatic] = gatewayValue.IsStatic.ToString().ToLower();

                    CustomValidateDT.Rows.Add(dataRow);
                }

                LoadGVCustomValidate();
            }
            else
            {
                gvCustomValidate.Visible = false;
            }
        }

        private void DisplayOrHideConfirmationPage(FrameworkGateway current)
        {
            if (SelectedValueEqualsIgnoreCase(rblConfirmationPage, NamePage))
            {
                pnlRedirect.Visible = false;
                pnlAutoRedirect.Visible = false;
                txtConfirmationRedirectURL.Text = string.Empty;
                txtConfirmationMessage.Text = current.ConfirmationMessage;
                txtConfirmationText.Text = current.ConfirmationText;
                ddlConfirmationRedirectDelay.SelectedIndex = 0;
                revConfirmationMessage.Enabled = true;
                revConfirmationRedirectURL.Enabled = false;
                revConfirmationText.Enabled = true;
            }
            else if (SelectedValueEqualsIgnoreCase(rblConfirmationPage, PageWithRedirect))
            {
                pnlRedirect.Visible = true;
                txtConfirmationRedirectURL.Text = current.RedirectURL;
                txtConfirmationMessage.Text = current.ConfirmationMessage;
                txtConfirmationText.Text = current.ConfirmationText;
                pnlAutoRedirect.Visible = false;
                ddlConfirmationRedirectDelay.SelectedIndex = 0;
                revConfirmationMessage.Enabled = true;
                revConfirmationRedirectURL.Enabled = true;
                revConfirmationText.Enabled = true;
            }
            else if (SelectedValueEqualsIgnoreCase(rblConfirmationPage, PageWithAutoRedirect))
            {
                pnlRedirect.Visible = true;
                txtConfirmationRedirectURL.Text = current.RedirectURL;
                txtConfirmationMessage.Text = current.ConfirmationMessage;
                txtConfirmationText.Text = current.ConfirmationText;
                pnlAutoRedirect.Visible = true;
                ddlConfirmationRedirectDelay.SelectedValue = current.RedirectDelay.ToString();
                revConfirmationMessage.Enabled = true;
                revConfirmationRedirectURL.Enabled = true;
                revConfirmationText.Enabled = true;
            }
        }

        #region CustomValidator
        protected void imgbtnAddCustom_Click(object sender, ImageClickEventArgs e)
        {
            ResetLogin();
            mpeCustomValidate.Show();
        }

        private void ResetLogin()
        {
            int groupID = -1;
            int.TryParse(hfSelectGroupID.Value.ToString(), out groupID);
            if (CustomValidateDT == null)
            {
                CustomValidateDT = new DataTable();
                CustomValidateDT.Columns.Add("ID");
                CustomValidateDT.Columns.Add("Label");
                CustomValidateDT.Columns.Add("Field");
                CustomValidateDT.Columns.Add("Value");
                CustomValidateDT.Columns.Add("IsDeleted");
                CustomValidateDT.Columns.Add("Type");
                CustomValidateDT.Columns.Add("NOT");
                CustomValidateDT.Columns.Add("Comparator");
                CustomValidateDT.Columns.Add("IsStatic");
            }
            else
            {
                LoadGVCustomValidateValues();
            }



            if (groupID > 0)
            {
                ddlCustomField.Items.Clear();
                List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFS = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser).OrderBy(x => x.ShortName).ToList();
                ddlCustomField.DataSource = listUDFS;
                ddlCustomField.DataTextField = "ShortName";
                ddlCustomField.DataValueField = "GroupDataFieldsID";
                ddlCustomField.DataBind();
            }
            DtTime_SingleDate.Text = "";

            txtCustomValue.Text = "";
            chkValidateIsStatic.Checked = true;
            pnlNonStaticValidate.Visible = false;
            pnlStaticValidate.Visible = true;
            chkCustomCompNot.Checked = false;
            ddlCustomFieldType.SelectedIndex = 0;
            BuildComparatorDR(0);
            pnlCustomValidateRule.Update();
        }

        private void LoadGVCustomValidate()
        {
            DataTable dt = CustomValidateDT;
            var result = (from src in dt.AsEnumerable()
                          where src.Field<string>("IsDeleted") == "false"
                          select new
                          {
                              ID = src.Field<string>("ID"),
                              Label = src.Field<string>("Label"),
                              Field = src.Field<string>("Field"),
                              Value = src.Field<string>("Value"),
                              IsDeleted = src.Field<string>("IsDeleted"),
                              Type = src.Field<string>("Type"),
                              NOT = src.Field<string>("NOT"),
                              Comparator = src.Field<string>("Comparator"),
                              IsStatic = src.Field<string>("IsStatic")
                          }).ToList();
            gvCustomValidate.DataSource = result;
            gvCustomValidate.DataBind();
        }

        private void LoadGVCustomValidateValues()
        {
            DataTable dt = CustomValidateDT;
            var result = (from src in dt.AsEnumerable()
                          where src.Field<string>("IsDeleted") == "false"
                          select new
                          {
                              ID = src.Field<string>("ID"),
                              Label = src.Field<string>("Label"),
                              Field = src.Field<string>("Field"),
                              Value = src.Field<string>("Value"),
                              IsDeleted = src.Field<string>("IsDeleted"),
                              Type = src.Field<string>("Type"),
                              NOT = src.Field<string>("NOT"),
                              Comparator = src.Field<string>("Comparator"),
                              IsStatic = src.Field<string>("IsStatic")
                          }).ToList();
            gvCustomValidateValues.DataSource = result;
            gvCustomValidateValues.DataBind();
            pnlCustomValidateRule.Update();
        }

        protected void btnSaveCustomValidator_Click(object sender, EventArgs e)
        {

            gvCustomValidate.Visible = true;
            LoadGVCustomValidate();
            mpeCustomValidate.Hide();
            UpdatePanel1.Update();
        }

        protected void btnCloseCustomValidator_Click(object sender, EventArgs e)
        {
            mpeCustomValidate.Hide();
        }

        protected void ddlCustomFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CompFieldName_selIndex = Convert.ToInt32(ddlCustomField.SelectedIndex.ToString());
            int ConvertToDataType_selIndex = Convert.ToInt32(ddlCustomFieldType.SelectedIndex.ToString());
            ErrorLabel.Text = "";
            ErrorLabel.Visible = false;

            if (ConvertToDataType_selIndex == 3)
            {
                BuildComparatorDR(3);
            }
            else if (ConvertToDataType_selIndex == 2)
            {
                BuildComparatorDR(2);
                CompValueNumberValidator.Enabled = true;
            }
            else
            {
                CompValueNumberValidator.Enabled = false;
                BuildComparatorDR(ConvertToDataType_selIndex);
                ErrorLabel.Visible = false;
            }
        }

        private void BuildComparatorDR(int index)
        {
            switch (index.ToString())
            {
                case "0":
                    ddlComparator.Items.Clear();
                    ddlComparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    ddlComparator.Items.Add(new ListItem("contains", "contains"));
                    ddlComparator.Items.Add(new ListItem("ends with", "ending with"));
                    ddlComparator.Items.Add(new ListItem("starts with", "starting with"));
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    break;

                case "1":
                    ddlComparator.Items.Clear();
                    ddlComparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    ddlComparator.Items.Add(new ListItem("contains", "contains"));
                    ddlComparator.Items.Add(new ListItem("ends with", "ending with"));
                    ddlComparator.Items.Add(new ListItem("starts with", "starting with"));
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    break;

                case "2":
                    ddlComparator.Items.Clear();
                    ddlComparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    ddlComparator.Items.Add(new ListItem("greater than [ > ]", "greater than"));
                    ddlComparator.Items.Add(new ListItem("less than [ < ]", "less than"));
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    break;

                case "3":
                    ddlComparator.Items.Clear();
                    ddlComparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    ddlComparator.Items.Add(new ListItem("greater than [ > ]", "greater than"));
                    ddlComparator.Items.Add(new ListItem("less than [ < ]", "less than"));
                    ddlComparator.Items.Add(new ListItem("is empty", "is empty"));
                    Default_CompareValuePanel.Visible = false;
                    DtTime_CompareValuePanel.Visible = true;

                    trSingleDate.Visible = true;
                    break;

                default:
                    ddlComparator.Items.Clear();
                    ddlComparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    ddlComparator.Items.Add(new ListItem("contains", "contains"));
                    ddlComparator.Items.Add(new ListItem("ends with", "ending with"));
                    ddlComparator.Items.Add(new ListItem("starts with", "starting with"));
                    break;
            }
        }

        protected void gvCustomValidate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deletecustom"))
            {
                DataTable DT = CustomValidateDT;
                DataRow[] dr = DT.Select("ID = '" + e.CommandArgument.ToString() + "'");
                if (dr[0] != null)
                {
                    
                    DT.Rows[DT.Rows.IndexOf(dr[0])]["IsDeleted"] = "true";
                }
                CustomValidateDT = DT;
                LoadGVCustomValidate();
            }
        }

        protected void imgbtnAddLoginValidator_Click(object sender, ImageClickEventArgs e)
        {

            DataRow dr = CustomValidateDT.NewRow();
            dr["ID"] = Guid.NewGuid().ToString();
            if (chkValidateIsStatic.Checked)
            {
                if (ddlCustomFieldType.SelectedItem.Value.ToLower().Trim().Equals("datetime"))
                {

                    dr["Label"] = "";
                    dr["IsStatic"] = "true";
                    dr["Field"] = ddlCustomField.SelectedItem.Text;
                    dr["Value"] = DtTime_SingleDate.Text;
                    dr["IsDeleted"] = "false";
                    dr["Type"] = ddlCustomFieldType.SelectedItem.Value.Trim();
                    dr["NOT"] = chkCustomCompNot.Checked.ToString().ToLower();
                    dr["Comparator"] = ddlComparator.SelectedItem.Value;

                }
                else
                {
                    dr["Label"] = "";
                    dr["IsStatic"] = "true";
                    dr["Field"] = ddlCustomField.SelectedItem.Text;
                    dr["Value"] = txtCustomValue.Text.Trim();
                    dr["IsDeleted"] = "false";
                    dr["Type"] = ddlCustomFieldType.SelectedItem.Value.Trim();
                    dr["NOT"] = chkCustomCompNot.Checked.ToString().ToLower();
                    dr["Comparator"] = ddlComparator.SelectedItem.Value;
                }

            }
            else
            {
                dr["Label"] = txtValidateNonStaticLabel.Text;
                dr["IsStatic"] = "false";
                dr["Field"] = "";
                dr["Value"] = txtCustomValue.Text.Trim();
                dr["IsDeleted"] = "false";
                dr["Type"] = ddlCustomFieldType.SelectedItem.Value.Trim();
                dr["NOT"] = chkCustomCompNot.Checked.ToString().ToLower();
                dr["Comparator"] = ddlComparator.SelectedItem.Value;
            }

            CustomValidateDT.Rows.Add(dr);

            ResetLogin();
        }

        protected void gvCustomValidateValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deletecustomvalue"))
            {
                DataTable DT = CustomValidateDT;
                DataRow[] rowToDelete = DT.Select("ID = '" + e.CommandArgument + "'");
                if (rowToDelete[0] != null)
                {
                    DT.Rows[DT.Rows.IndexOf(rowToDelete[0])]["IsDeleted"] = "true";
                }
                CustomValidateDT = DT;

                ResetLogin();
            }
        }


        #endregion

        #region CaptureValues
        private void ResetCapture()
        {

            txtStaticValue.Text = "";
            txtCaptureLabel.Text = "";
            if (CaptureCustomDT == null)
            {
                CaptureCustomDT = new DataTable();
                CaptureCustomDT.Columns.Add("ID");
                CaptureCustomDT.Columns.Add("Label");
                CaptureCustomDT.Columns.Add("Field");
                CaptureCustomDT.Columns.Add("Value");
                CaptureCustomDT.Columns.Add("IsDeleted");
                CaptureCustomDT.Columns.Add("IsStatic");
            }
            else if (CaptureCustomDT.Rows.Count > 0)
            {
                LoadGVCaptureValues();

            }

            int groupID = -1;
            int.TryParse(hfSelectGroupID.Value.ToString(), out groupID);
            if (groupID > 0)
            {
                List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFS = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser);
                List<string> usedFields = CaptureCustomDT.AsEnumerable().Select(x => x["Field"].ToString()).ToList();

                ddlUDF.DataSource = listUDFS.Where(x => !usedFields.Contains(x.ShortName));
                ddlUDF.DataValueField = "GroupDataFieldsID";
                ddlUDF.DataTextField = "ShortName";
                ddlUDF.DataBind();
                ddlUDF.Items.Insert(0, "--Select--");
            }

            pnlCaptureCustom.Update();

        }
        protected void imgbtnAddCapture_Click(object sender, ImageClickEventArgs e)
        {

            DataRow dr = CaptureCustomDT.NewRow();
            if (chkCaptureIsStatic.Checked)
            {
                if (!string.IsNullOrEmpty(txtStaticValue.Text.Trim()) && ddlUDF.SelectedIndex > 0)
                {

                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["IsDeleted"] = "false";
                    dr["IsStatic"] = "true";
                    dr["Value"] = txtStaticValue.Text.Trim();
                    dr["Label"] = "";
                    dr["Field"] = ddlUDF.SelectedItem.Text.ToString();

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtCaptureLabel.Text.Trim()) && ddlUDF.SelectedIndex > 0)
                {
                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["IsDeleted"] = "false";
                    dr["IsStatic"] = "false";
                    dr["Value"] = "";
                    dr["Label"] = txtCaptureLabel.Text;
                    dr["Field"] = ddlUDF.SelectedItem.Text.ToString();

                }
            }
            CaptureCustomDT.Rows.Add(dr);

            ResetCapture();
        }

        private void LoadGVCaptureCustom()
        {
            DataTable dt = CaptureCustomDT;
            var result = (from src in dt.AsEnumerable()
                          where src.Field<string>("IsDeleted") == "false"
                          select new
                          {
                              ID = src.Field<string>("ID"),
                              Label = src.Field<string>("Label"),
                              Field = src.Field<string>("Field"),
                              Value = src.Field<string>("Value"),
                              IsDeleted = src.Field<string>("IsDeleted"),
                              IsStatic = src.Field<string>("IsStatic")

                          }).ToList();
            if (result.Count > 0)
            {
                gvCaptureCustom.Visible = true;
                gvCaptureCustom.DataSource = result;
                gvCaptureCustom.DataBind();
            }
            else
            {
                gvCaptureCustom.Visible = false;
            }
            UpdatePanel1.Update();
        }

        private void LoadGVCaptureValues()
        {
            DataTable dt = CaptureCustomDT;
            var result = (from src in dt.AsEnumerable()
                          where src.Field<string>("IsDeleted") == "false"
                          select new
                          {
                              ID = src.Field<string>("ID"),
                              Label = src.Field<string>("Label"),
                              Field = src.Field<string>("Field"),
                              Value = src.Field<string>("Value"),
                              IsDeleted = src.Field<string>("IsDeleted"),
                              IsStatic = src.Field<string>("IsStatic")

                          }).ToList();
            if (result.Count > 0)
            {
                gvCaptureValues.DataSource = result;
                gvCaptureValues.DataBind();
                gvCaptureValues.Visible = true;
            }
            else
            {
                gvCaptureValues.Visible = false;
            }
            pnlCaptureCustom.Update();
        }


        protected void gvCaptureValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deletecapturevalue"))
            {
                DataTable DT = CaptureCustomDT;
                DataRow[] rowToDelete = DT.Select("ID = '" + e.CommandArgument + "'");
                if (rowToDelete[0] != null)
                {
                    DT.Rows[DT.Rows.IndexOf(rowToDelete[0])]["IsDeleted"] = "true";
                }
                CaptureCustomDT = DT;
                LoadGVCaptureValues();

            }
        }

        protected void gvCaptureCustom_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deletecustom"))
            {
                DataTable DT = CaptureCustomDT;
                DataRow[] rowToDelete = DT.Select("ID = '" + e.CommandArgument + "'");
                if (rowToDelete[0] != null)
                {
                    DT.Rows[DT.Rows.IndexOf(rowToDelete[0])]["IsDeleted"] = "true";
                }
                CaptureCustomDT = DT;
                LoadGVCaptureCustom();

            }
        }

        protected void btnSaveCaptureFields_Click(object sender, EventArgs e)
        {
            LoadGVCaptureCustom();
            mpeCaptureCustom.Hide();
        }

        protected void btnCancelCaptureFields_Click(object sender, EventArgs e)
        {
            mpeCaptureCustom.Hide();
        }

        protected void imgbtnAddCaptureField_Click(object sender, ImageClickEventArgs e)
        {
            ResetCapture();
            mpeCaptureCustom.Show();
        }
        #endregion

        #region RadioButtonLists
        protected void rblForgotPasswordVisible_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblForgotPasswordVisible.SelectedValue.ToLower().Equals("true"))
            {
                trForgotPasswordText.Visible = true;
                revForgotPassword.Enabled = true;
            }
            else
            {
                trForgotPasswordText.Visible = false;
                revForgotPassword.Enabled = false;
            }

            txtForgotPasswordText.Text = "";
        }

        protected void rblSignupVisible_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblSignupVisible.SelectedValue.ToLower().Equals("true"))
            {
                trSignupText.Visible = true;
                trSignupURL.Visible = true;
                revSignupText.Enabled = true;
                revSignupURL.Enabled = true;
            }
            else
            {
                trSignupText.Visible = false;
                trSignupURL.Visible = false;
                revSignupText.Enabled = false;
                revSignupURL.Enabled = false;
            }
            txtSignupText.Text = "";
            txtSignupURL.Text = "";
        }

        protected void rblStyling_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblStyling.SelectedValue.ToLower().Equals("default"))
            {
                pnlCSS.Visible = false;
                imgbtnUploadCSS.Visible = false;
                revStyle.Enabled = false;
            }
            else if (rblStyling.SelectedValue.ToLower().Equals("external"))
            {
                pnlCSS.Visible = true;
                imgbtnUploadCSS.Visible = false;
                revStyle.Enabled = true;
                lblStyleSelector.Text = "URL";
            }
            else if (rblStyling.SelectedValue.ToLower().Equals("upload"))
            {
                pnlCSS.Visible = true;
                imgbtnUploadCSS.Visible = true;
                revStyle.Enabled = true;
                lblStyleSelector.Text = "File";
            }
            txtStyle.Text = "";
        }

        protected void rblConfirmationPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblConfirmationPage.SelectedValue.ToLower().Equals("page"))
            {
                pnlAutoRedirect.Visible = false;
                pnlRedirect.Visible = false;
                rfvConfirmationRedirectURL.Enabled = false;
                revConfirmationRedirectURL.Enabled = false;
                revConfirmationText.Enabled = false;
            }
            else if (rblConfirmationPage.SelectedValue.ToLower().Equals("pagewithredirect"))
            {
                pnlAutoRedirect.Visible = false;
                pnlRedirect.Visible = true;
                rfvConfirmationRedirectURL.Enabled = true;
                revConfirmationRedirectURL.Enabled = true;
                revConfirmationText.Enabled = true;
            }
            else if (rblConfirmationPage.SelectedValue.ToLower().Equals("pagewithautoredirect"))
            {
                pnlAutoRedirect.Visible = true;
                pnlRedirect.Visible = true;
                rfvConfirmationRedirectURL.Enabled = true;
                revConfirmationRedirectURL.Enabled = true;
                revConfirmationText.Enabled = true;
            }
        }

        protected void rblLoginOrCapture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblLoginOrCapture.SelectedValue.ToLower().Equals("login"))
            {
                imgbtnAddCaptureField.Enabled = false;
                pnlLogin.Visible = true;
                pnlCapture.Visible = false;
                pnlSignup.Visible = true;
                pnlPassword.Visible = true;
            }
            else if (rblLoginOrCapture.SelectedValue.ToLower().Equals("capture"))
            {
                imgbtnAddCaptureField.Enabled = true;
                pnlLogin.Visible = false;
                pnlCapture.Visible = true;
                pnlSignup.Visible = false;
                trForgotPasswordText.Visible = false;
                trSignupText.Visible = false;
                trSignupURL.Visible = false;
                rblSignupVisible.SelectedValue = "false";
                pnlPassword.Visible = true;
                rblForgotPasswordVisible.SelectedValue = "false";
            }
        }
        #endregion

        #region Select Groups
        protected void imgbtnSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            groupExplorer.ShowArchiveFilter = false;
            groupExplorer.LoadControl();

            groupExplorer.Visible = true;
            btnCloseGroupExplorer.Visible = true;
            pnlSelectGroup.Update();
        }
        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = groupExplorer.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        if (!groupID.ToString().Equals(hfSelectGroupID.Value.ToString()))
                        {
                            DeleteALLValues();
                        }

                        txtGatewayGroup.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                    }
                    else
                    {

                    }
                    UpdatePanel1.Update();
                    GroupsLookupPopupHide();
                }
                else if (source.Equals("upload"))
                {
                    dgFiles.CurrentPageIndex = 0;
                    dgFiles.DataSource = createDataSource(DataFilePath);
                    dgFiles.DataBind();
                    upUpload.Update();
                }
            }
            catch { }
            return true;
        }

        private void DeleteALLValues()
        {
            if (CustomValidateDT != null)
            {
                foreach (DataRow dr in CustomValidateDT.AsEnumerable())
                {
                    dr["IsDeleted"] = "true";
                }
                LoadGVCustomValidate();
            }

            if (CaptureCustomDT != null)
            {
                foreach (DataRow dr in CaptureCustomDT.AsEnumerable())
                {
                    dr["IsDeleted"] = "true";
                }
                LoadGVCaptureCustom();
            }
        }

        protected void btnCloseGroupExplorer_Click(object sender, EventArgs e)
        {
            GroupsLookupPopupHide();
        }

        #endregion

        protected void btnSaveGateway_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfSelectGroupID.Value) || hfSelectGroupID.Value.Equals(Zero))
            {
                throwECNException(SelectGroupMessage);
            }
            else
            {
                var gateway = SetGateway();

                int gatewayId;

                try
                {
                    gatewayId = ECN_Framework_BusinessLayer.Communicator.Gateway.Save(gateway, Master.UserSession.CurrentUser);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                    return;
                }

                if (gateway.LoginOrCapture.Equals(Login))
                {
                    GatewayLogin(gatewayId);
                }
                else if (gateway.LoginOrCapture.Equals(Capture))
                {
                    GatewayCapture(gatewayId);
                }

                Response.Redirect(GateWayListUrl);
            }
        }

        private void GatewayCapture(int gatewayId)
        {
            GatewayValue.WipeOutValues(true, gatewayId);

            if (CaptureCustomDT != null)
            {
                foreach (var dataRow in CaptureCustomDT.AsEnumerable())
                {
                    if (dataRow[Id].ToString().Contains(Hyphen) && dataRow[IsDeleted].ToString().Equals(FalseValue))
                    {
                        var gatewayValue = new ECN_Framework_Entities.Communicator.GatewayValue
                                           {
                                               GatewayID = gatewayId,
                                               Field = dataRow[Field].ToString(),
                                               Value = dataRow[Value].ToString(),
                                               Label = dataRow[NameLabel].ToString(),
                                               IsStatic = dataRow[IsStatic].ToString().EqualsIgnoreCase(TrueValue),
                                               IsCaptureValue = true
                                           };

                        try
                        {
                            GatewayValue.Save(gatewayValue, Master.UserSession.CurrentUser);
                        }
                        catch (ECNException ex)
                        {
                            setECNError(ex);
                        }
                    }
                    else if (!dataRow[Id].ToString().Contains(Hyphen) && dataRow[IsDeleted].ToString().Equals(TrueValue))
                    {
                        GatewayValue.DeleteByID(IntTryParse(dataRow[Id].ToString()));
                    }
                }
            }
        }

        private void GatewayLogin(int gatewayId)
        {
            GatewayValue.WipeOutValues(false, gatewayId);

            if (CustomValidateDT != null)
            {
                foreach (var dataRow in CustomValidateDT.AsEnumerable())
                {
                    if (dataRow[Id].ToString().Contains(Hyphen) && dataRow[IsDeleted].ToString().Equals(FalseValue))
                    {
                        var gatewayValue = new ECN_Framework_Entities.Communicator.GatewayValue
                                           {
                                               IsLoginValidator = true,
                                               Field = dataRow[Field].ToString(),
                                               Value = dataRow[Value].ToString(),
                                               Comparator = dataRow[NameComparator].ToString(),
                                               NOT = dataRow[NameNot].ToString().Equals(TrueValue),
                                               FieldType = dataRow[NameType].ToString(),
                                               GatewayID = gatewayId,
                                               IsStatic = dataRow[IsStatic].ToString().EqualsIgnoreCase(TrueValue),
                                               Label = dataRow[NameLabel].ToString()
                                           };

                        try
                        {
                            GatewayValue.Save(gatewayValue, Master.UserSession.CurrentUser);
                        }
                        catch (ECNException ex)
                        {
                            setECNError(ex);
                        }
                    }
                    else if (!dataRow[Id].ToString().Contains(Hyphen) && dataRow[IsDeleted].ToString().Equals(TrueValue))
                    {
                        GatewayValue.DeleteByID(IntTryParse(dataRow[Id].ToString()));
                    }
                }
            }
        }

        private FrameworkGateway SetGateway()
        {
            var gateway = new FrameworkGateway();

            if (GatewayID > 0)
            {
                gateway = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByGatewayID(GatewayID);
            }

            gateway.Name = txtGatewayName.Text.Trim();
            gateway.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
            gateway.GroupID = IntTryParse(hfSelectGroupID.Value);
            gateway.PubCode = txtGatewayPubCode.Text.Trim();
            gateway.TypeCode = txtGatewayTypeCode.Text.Trim();
            gateway.Header = txtGatewayHeader.Text.Trim();
            gateway.Footer = txtGatewayFooter.Text.Trim();
            gateway.SubmitText = txtSubmitText.Text.Trim();

            var isRblForgotPasswordVisible = SelectedValueIsTrue(rblForgotPasswordVisible);
            gateway.ShowForgotPassword = isRblForgotPasswordVisible;
            gateway.ForgotPasswordText = isRblForgotPasswordVisible
                                             ? txtForgotPasswordText.Text.Trim()
                                             : string.Empty;

            var isRblSignupVisible = SelectedValueIsTrue(rblSignupVisible);
            gateway.ShowSignup = isRblSignupVisible;
            gateway.SignupText = isRblSignupVisible
                                     ? txtSignupText.Text.Trim()
                                     : string.Empty;
            gateway.SignupURL = isRblSignupVisible
                                    ? txtSignupURL.Text.Trim()
                                    : string.Empty;

            SetGatewayByRadioButtonList(gateway);

            return gateway;
        }

        private void SetGatewayByRadioButtonList(FrameworkGateway gateway)
        {
            if (SelectedValueEqualsIgnoreCase(rblStyling, Default))
            {
                gateway.UseStyleFrom = Default;
                gateway.Style = string.Empty;
            }
            else if (SelectedValueEqualsIgnoreCase(rblStyling, External))
            {
                gateway.UseStyleFrom = External;
                gateway.Style = txtStyle.Text.Trim();
            }
            else if (SelectedValueEqualsIgnoreCase(rblStyling, Upload))
            {
                gateway.UseStyleFrom = Upload;
                gateway.Style = $"{DataFilePath}/{txtStyle.Text.Trim()}";
            }

            if (SelectedValueEqualsIgnoreCase(rblConfirmationPage, NamePage))
            {
                gateway.UseConfirmation = true;
                gateway.UseRedirect = false;
                gateway.ConfirmationMessage = txtConfirmationMessage.Text.Trim();
                gateway.ConfirmationText = string.Empty;
                gateway.RedirectURL = string.Empty;
                gateway.RedirectDelay = 0;
            }
            else if (SelectedValueEqualsIgnoreCase(rblConfirmationPage, PageWithRedirect))
            {
                gateway.UseConfirmation = false;
                gateway.UseRedirect = true;
                gateway.ConfirmationMessage = txtConfirmationMessage.Text.Trim();
                gateway.ConfirmationText = txtConfirmationText.Text.Trim();
                gateway.RedirectURL = txtConfirmationRedirectURL.Text.Trim();
                gateway.RedirectDelay = 0;
            }
            else if (SelectedValueEqualsIgnoreCase(rblConfirmationPage, PageWithAutoRedirect))
            {
                gateway.UseConfirmation = true;
                gateway.UseRedirect = true;
                gateway.ConfirmationMessage = txtConfirmationMessage.Text.Trim();
                gateway.ConfirmationText = txtConfirmationText.Text.Trim();
                gateway.RedirectURL = txtConfirmationRedirectURL.Text.Trim();
                gateway.RedirectDelay = IntTryParse(ddlConfirmationRedirectDelay.SelectedValue);
            }

            if (SelectedValueEqualsIgnoreCase(rblLoginOrCapture, Login))
            {
                gateway.LoginOrCapture = Login;
                gateway.ValidateEmail = true;
                gateway.ValidatePassword = chkLoginValidatePassword.Checked;
                gateway.ValidateCustom = chkLoginValidateCustom.Checked;
            }
            else if (SelectedValueEqualsIgnoreCase(rblLoginOrCapture, Capture))
            {
                gateway.LoginOrCapture = Capture;
                gateway.ValidateEmail = false;
                gateway.ValidatePassword = false;
                gateway.ValidateCustom = false;
            }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Gateway, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void chkValidateIsStatic_CheckedChanged(object sender, EventArgs e)
        {
            if (chkValidateIsStatic.Checked)
            {
                pnlStaticValidate.Visible = true;
                pnlNonStaticValidate.Visible = false;
            }
            else
            {
                pnlStaticValidate.Visible = false;
                pnlNonStaticValidate.Visible = true;
            }
        }

        protected void chkCaptureIsStatic_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCaptureIsStatic.Checked)
            {
                pnlCaptureStatic.Visible = true;
                pnlCaptureNonStatic.Visible = false;
                txtStaticValue.Text = "";
                txtCaptureLabel.Text = "";
            }
            else
            {
                pnlCaptureStatic.Visible = false;
                pnlCaptureNonStatic.Visible = true;
                txtStaticValue.Text = "";
                txtCaptureLabel.Text = "";
            }
        }

        protected void imgbtnSelect_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtnSelect = (ImageButton)sender;
            hfFilePath.Value = DataFilePath + imgbtnSelect.CommandArgument.ToString();
            txtStyle.Text = imgbtnSelect.CommandArgument.ToString();
            ShowUpload = false;
            mpeUploadControl.Hide();
            UpdatePanel1.Update();
        }

        protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtnDelete = (ImageButton)sender;
            deleteFile(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath + "/" + imgbtnDelete.CommandArgument));
            dgFiles.CurrentPageIndex = 0;
            dgFiles.DataSource = createDataSource(DataFilePath);
            dgFiles.DataBind();
            ShowUpload = true;
            mpeUploadControl.Show();

        }

        public void deleteFile(string thefile)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(thefile);
            file.Delete();
            dgFiles.CurrentPageIndex = 0;
            dgFiles.DataSource = createDataSource(DataFilePath);
            dgFiles.DataBind();
            upUpload.Update();
        }

        public DataView createDataSource(string datapath)
        {
            DataTable dtFiles = new DataTable();
            DataColumn dcFileName = new DataColumn("FileName", typeof(string));
            DataRow drFiles;
            dtFiles.Columns.Add(dcFileName);

            System.IO.FileInfo file = null;
            string[] files = null;
            string filename = "";
            files = System.IO.Directory.GetFiles(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + datapath), "*.css");

            for (int i = 0; i <= files.Length - 1; i++)
            {
                file = new System.IO.FileInfo(files[i]);
                filename = file.Name.ToString();
                if (filename.ToLower().EndsWith(".css"))
                {
                    drFiles = dtFiles.NewRow();
                    drFiles[0] = file.Name;
                    dtFiles.Rows.Add(drFiles);
                }
            }
            DataView dvFiles = new DataView(dtFiles);
            return dvFiles;
        }

        protected void dgFiles_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton imgbtnSelect = (ImageButton)e.Item.FindControl("imgbtnSelect");
                ImageButton imgbtnDelete = (ImageButton)e.Item.FindControl("imgbtnDelete");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                imgbtnSelect.CommandArgument = drv["FileName"].ToString();
                imgbtnDelete.CommandArgument = drv["FileName"].ToString();
            }
        }

        protected void imgbtnUploadCSS_Click(object sender, ImageClickEventArgs e)
        {
            dgFiles.CurrentPageIndex = 0;
            dgFiles.DataSource = createDataSource(DataFilePath);
            dgFiles.DataBind();
            ShowUpload = true;
            mpeUploadControl.Show();
            upUpload.Update();
        }

        protected void btnCancelUpload_Click(object sender, EventArgs e)
        {
            ShowUpload = false;
            mpeUploadControl.Hide();
        }

        protected void hlKMDefault_Click(object sender, EventArgs e)
        {

            string filepath = Server.MapPath("~/main/admin/Gateway/Site.css");
            FileInfo fInfo = new FileInfo(filepath);

            Response.Clear();
            Response.ContentType = "text/css";
            Response.AddHeader("Content-Disposition", "attachment; filename=gateway.css;");
            Response.AddHeader("Content-Length", fInfo.Length.ToString());
            Response.WriteFile(filepath);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            UpdatePanel1.Update();
        }

        protected void ddlComparator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomFieldType.SelectedValue.ToString().ToLower().Equals("datetime"))
            {

                trSingleDate.Visible = true;

            }
        }




    }
}