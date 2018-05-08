using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using aspNetMX;
using aspNetEmail;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic;
using Enums = ECN_Framework_Common.Objects.Enums;
using CommonEnums = KMPlatform.Enums;
using BusinessSample = ECN_Framework_BusinessLayer.Communicator.Sample;
using BusinessBlastActivity = ECN_Framework_BusinessLayer.Activity.View.BlastActivity;
using BusinessCampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using BusinessCustomer = ECN_Framework_BusinessLayer.Accounts.Customer;
using BusinessBlastFieldName = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName;
using BusinessBlastFieldsValue = ECN_Framework_BusinessLayer.Communicator.BlastFieldsValue;
using KMUser = KMPlatform.Entity.User;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardCampaign : System.Web.UI.UserControl, IECNWizard
    {
        private const string CustomValueName = "Custom Value";
        private const string NoItemText = "-Select-";

        int _campaignItemID = 0;
        public int CampaignItemID
        {
            set
            {
                _campaignItemID = value;
            }
            get
            {
                return _campaignItemID;
            }
        }

        private int getCampaignItemTemplate()
        {
            if (Request.QueryString["CampaignItemTemplateID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["CampaignItemTemplateID"]);
            }
            else
                return -1;
        }

        public int SampleID
        {
            get
            {
                if (Request.QueryString["SampleID"] != null)
                    return Convert.ToInt32(Request.QueryString["SampleID"].ToString());
                else
                    return -1;
            }
        }

        public string CampaignItemType
        {
            get
            {
                return Request.QueryString["campaignitemtype"].ToString();
            }
        }            
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Redit2", "doMouseOver();", true);
        }

        public string ErrorMessage
        {
            get; set;
        }

        public void Initialize()
        {
            if (User.HasAccess(
                ECNSession.CurrentSession().CurrentUser,
                CommonEnums.Services.EMAILMARKETING,
                CommonEnums.ServiceFeatures.Blast,
                CommonEnums.Access.Edit))
            {
                loadCampaigns();
                LoadBlastFieldConfig();
                if (SampleID > 0)
                {
                    InitializeSample();
                }
                else
                {
                    InitializeCampaign();
                }

                InitializeBlastOptions();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void InitializeBlastOptions()
        {
            pnlBlastFields.Visible = BusinessCustomer.HasProductFeature(
                ECNSession.CurrentSession().CurrentUser.CustomerID,
                CommonEnums.Services.EMAILMARKETING,
                CommonEnums.ServiceFeatures.ReportingBlastFields);
            var canIgnoreSuppression = false;
            bool.TryParse(ConfigurationManager.AppSettings[$"IgnoreSuppression_{ECNSession.CurrentSession().CurrentUser.UserID}"], out canIgnoreSuppression);
            cbIgnoreSuppression.Visible = canIgnoreSuppression;
        }

        private void InitializeCampaign()
        {
            if (CampaignItemID != 0)
            {
                try
                {
                    rbExistingCampaign.Checked = true;
                    plExistingCampaign.Visible = true;
                    plCreateCampaign.Visible = false;
                    var campaignItem = BusinessCampaignItem.GetByCampaignItemID(CampaignItemID, ECNSession.CurrentSession().CurrentUser, false);
                    txtCampaignItemName.Text = campaignItem.CampaignItemName;
                    drpdownCampaign.SelectedValue = campaignItem.CampaignID.ToString();
                    cbIgnoreSuppression.Checked = campaignItem.IgnoreSuppression.GetValueOrDefault();
                    if (campaignItem.CampaignItemTemplateID != null)
                    {
                        loadTemplateData(campaignItem.CampaignItemTemplateID.Value, false);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                rbNewCampaign.Checked = false;
                rbExistingCampaign.Checked = true;
                plCreateCampaign.Visible = false;
                plExistingCampaign.Visible = true;
                loadTemplateData(getCampaignItemTemplate(), true);
            }
        }

        private void InitializeSample()
        {
            rbExistingCampaign.Checked = true;
            plExistingCampaign.Visible = true;
            plCreateCampaign.Visible = false;
            var sample = BusinessSample.GetBySampleID(SampleID, ECNSession.CurrentSession().CurrentUser);
            var championData = BusinessBlastActivity.ChampionByProc(
                SampleID,
                false,
                ECNSession.CurrentSession().CurrentUser,
                sample.ABWinnerType);

            int winningBlastId;
            if (championData.Rows.Count <= 0 ||
                !int.TryParse(championData.Rows[0]["BlastID"].ToString(), out winningBlastId))
            {
                return;
            }

            var campaignItem = BusinessCampaignItem.GetByBlastID(winningBlastId, ECNSession.CurrentSession().CurrentUser, false);
            txtCampaignItemName.Text = $"{campaignItem.CampaignItemName} Champion";
            drpdownCampaign.SelectedValue = campaignItem.CampaignID.ToString();
            if (campaignItem.CampaignItemTemplateID != null)
            {
                loadTemplateData(campaignItem.CampaignItemTemplateID.Value, false);
            }

            InitializeBlastField(campaignItem.BlastField1, drpBlastField1, txtBlastField1);
            InitializeBlastField(campaignItem.BlastField2, drpBlastField2, txtBlastField2);
            InitializeBlastField(campaignItem.BlastField3, drpBlastField3, txtBlastField3);
            InitializeBlastField(campaignItem.BlastField4, drpBlastField4, txtBlastField4);
            InitializeBlastField(campaignItem.BlastField5, drpBlastField5, txtBlastField5);
        }

        private static void InitializeBlastField(string campaignItemBlastField, DropDownList blastDropDown, TextBox blastTextBox)
        {
            if (string.IsNullOrWhiteSpace(campaignItemBlastField))
            {
                return;
            }

            if (blastDropDown.Items.FindByText(campaignItemBlastField) != null)
            {
                blastDropDown.ClearSelection();
                blastDropDown.Items.FindByText(campaignItemBlastField).Selected = true;
                blastTextBox.Visible = false;
            }
            else
            {
                blastDropDown.ClearSelection();
                blastDropDown.Items.FindByValue(CustomValueName).Selected = true;
                blastTextBox.Visible = true;
                blastTextBox.Text = campaignItemBlastField;
            }
        }

        private void LoadBlastFieldConfig()
        {
            drpBlastField1.Items.Clear();
            drpBlastField2.Items.Clear();
            drpBlastField3.Items.Clear();
            drpBlastField4.Items.Clear();
            drpBlastField5.Items.Clear();

            CampaignItem campaignItem = null;
            if (CampaignItemID != 0)
            {
                campaignItem = BusinessCampaignItem.GetByCampaignItemID(CampaignItemID, ECNSession.CurrentSession().CurrentUser, false);
            }
            var currentUser = ECNSession.CurrentSession().CurrentUser;

            SetBlastFieldData(currentUser, campaignItem, lblBlastField1, drpBlastField1, txtBlastField1, 1, campaignItem?.BlastField1);
            SetBlastFieldData(currentUser, campaignItem, lblBlastField2, drpBlastField2, txtBlastField2, 2, campaignItem?.BlastField2);
            SetBlastFieldData(currentUser, campaignItem, lblBlastField3, drpBlastField3, txtBlastField3, 3, campaignItem?.BlastField3);
            SetBlastFieldData(currentUser, campaignItem, lblBlastField4, drpBlastField4, txtBlastField4, 4, campaignItem?.BlastField4);
            SetBlastFieldData(currentUser, campaignItem, lblBlastField5, drpBlastField5, txtBlastField5, 5, campaignItem?.BlastField5);
        }

        private void SetBlastFieldData(
            KMUser currentUser,
            CampaignItem campaignItem,
            Label blastFieldLabel,
            DropDownList blastFieldDropDown,
            TextBox blastFieldTextBox,
            int blastFieldId,
            string campaignItemBlastField)
        {
            var blastFieldsName = BusinessBlastFieldName.GetByBlastFieldID(blastFieldId, currentUser);
            if (blastFieldsName != null)
            {
                blastFieldLabel.Text = blastFieldsName.Name;
            }

            var blastFieldsValue1Dt = BusinessBlastFieldsValue.GetByBlastFieldID(blastFieldId, currentUser);
            if (blastFieldsValue1Dt.Rows.Count > 0)
            {
                blastFieldDropDown.DataSource = blastFieldsValue1Dt;
                blastFieldDropDown.DataBind();
            }

            blastFieldDropDown.Items.Insert(0, new ListItem(NoItemText, NoItemText));
            blastFieldDropDown.Items.Insert(1, new ListItem(CustomValueName, CustomValueName));
            if (campaignItem == null)
            {
                return;
            }

            var result = (from src in blastFieldsValue1Dt.AsEnumerable()
                where src.Field<string>("Value") == campaignItemBlastField
                select src).ToList();
            if (result.Count > 0)
            {
                blastFieldDropDown.SelectedValue = campaignItemBlastField;
            }
            else if (campaignItemBlastField != string.Empty)
            {

                blastFieldDropDown.SelectedValue = CustomValueName;
                blastFieldTextBox.Visible = true;
                blastFieldTextBox.Text = campaignItemBlastField;
            }
        }

        public bool Save() 
        {
            if (Page.IsValid)
            {
                List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
                ECN_Framework_Entities.Communicator.CampaignItem ci;
                if (CampaignItemID == 0)
                {                        
                    ci= new ECN_Framework_Entities.Communicator.CampaignItem();
                    ci.CompletedStep = 1;
                    if (CampaignItemType.ToLower().Equals("regular"))
                    {
                        ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString();
                    }
                    else if (CampaignItemType.ToLower().Equals("ab"))
                    {
                        ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString();
                    }
                    else if (CampaignItemType.ToLower().Equals("champion"))
                    {
                        ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString();
                    }
                    else if (CampaignItemType.ToLower().Equals("sms"))
                    {
                        ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.SMS.ToString();
                    }
                    else if (CampaignItemType.ToLower().Equals("social"))
                    {
                        ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Social.ToString();
                    }
                    else if (CampaignItemType.ToLower().Equals("salesforce"))
                    {
                        ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Salesforce.ToString();
                    }

                    ci.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    if (getCampaignItemTemplate() > 0)
                        ci.CampaignItemTemplateID = getCampaignItemTemplate();
                }
                else
                {
                    ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                    ci.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                }

                if (rbNewCampaign.Checked)
                {
                    ECN_Framework_Entities.Communicator.Campaign c = new ECN_Framework_Entities.Communicator.Campaign();
                    c.CampaignName = txtCampaignName.Text;
                    c.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                    c.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    c.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        ci.CampaignID = c.CampaignID;
                    }
                    catch(ECN_Framework_Common.Objects.ECNException ex)
                    {
                        ErrorMessage = "";
                        foreach (ECN_Framework_Common.Objects.ECNError error in ex.ErrorList)
                        {
                            ErrorMessage = ErrorMessage + error.ErrorMessage + "<br/>";
                        }
                        return false;
                    }
                }
                else if (rbExistingCampaign.Checked)
                {
                    if (drpdownCampaign.SelectedValue != null && drpdownCampaign.SelectedValue != string.Empty)
                        ci.CampaignID = Int32.Parse(drpdownCampaign.SelectedValue);
                    else
                        throwECNException("Invalid Campaign Selection");
                }
                ci.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();
                if (pnlBlastFields.Visible)
                {
                    ci.BlastField1 = txtBlastField1.Visible == true ? txtBlastField1.Text : drpBlastField1.SelectedValue == NoItemText ? string.Empty : drpBlastField1.SelectedValue;
                    ci.BlastField2 = txtBlastField2.Visible == true ? txtBlastField2.Text : drpBlastField2.SelectedValue == NoItemText ? string.Empty : drpBlastField2.SelectedValue; ;
                    ci.BlastField3 = txtBlastField3.Visible == true ? txtBlastField3.Text : drpBlastField3.SelectedValue == NoItemText ? string.Empty : drpBlastField3.SelectedValue; ;
                    ci.BlastField4 = txtBlastField4.Visible == true ? txtBlastField4.Text : drpBlastField4.SelectedValue == NoItemText ? string.Empty : drpBlastField4.SelectedValue; ;
                    ci.BlastField5 = txtBlastField5.Visible == true ? txtBlastField5.Text : drpBlastField5.SelectedValue == NoItemText ? string.Empty : drpBlastField5.SelectedValue; ;
                }
                if (ci.CompletedStep == null)
                {
                    ci.CompletedStep = 1;
                }
                ci.CampaignItemName = txtCampaignItemName.Text;
                ci.CampaignItemNameOriginal = txtCampaignItemName.Text;
                ci.IsHidden = false;
                ci.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                ci.IgnoreSuppression = cbIgnoreSuppression.Checked;
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
               
                //ELENA: JUST COMMENTING OUT FOR NOW, THIS LOOKS LIKE OLD DESIGN
                //if(campaignItem.SampleID != null)
                //{
                //    ECN_Framework_Entities.Communicator.Sample sample = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID(campaignItem.SampleID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                //    sample.SampleName = campaignItem.CampaignItemName;
                //    sample.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                //    ECN_Framework_BusinessLayer.Communicator.Sample.Save(sample, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                //}
                CampaignItemID = ci.CampaignItemID;
                ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                return true;
            }
            return false;
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Campaign, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
        }
        
        private void loadCampaigns()
        {
            List<ECN_Framework_Entities.Communicator.Campaign> myCampaigns = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NonArchived(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
            myCampaigns = myCampaigns.OrderBy(o => o.CampaignName).ToList();   
            drpdownCampaign.DataSource = myCampaigns;
            drpdownCampaign.DataTextField = "CampaignName";
            drpdownCampaign.DataValueField = "CampaignID";
            drpdownCampaign.DataBind();
        }

        protected void rbNewCampaign_CheckedChanged(object sender, System.EventArgs e)
        {
            plCreateCampaign.Visible = true;
            plExistingCampaign.Visible = false;
        }

        protected void rbExistingCampaign_CheckedChanged(object sender, System.EventArgs e)
        {
            plCreateCampaign.Visible = false;
            plExistingCampaign.Visible = true;
        }

        #region BlastFields
        protected void btnBlastFieldsConfig_Save(object sender, EventArgs e)
        {
            Label errorMessage = (Label)BlastFieldsConfig1.FindControl("lblBlastFieldsNameMessage");
            errorMessage.Visible = true;
            errorMessage.Text = "";
            try
            {
                BlastFieldsConfig1.save();
                BlastFieldsConfig1.Reset();
                modalPopupBlastFieldsConfig.Hide();
                LoadBlastFieldConfig();
            }
            catch (ECNException ex)
            {
                
                foreach (ECN_Framework_Common.Objects.ECNError er in ex.ErrorList)
                {
                    errorMessage.Text += er.ErrorMessage + "<br />";
                }
                errorMessage.Visible = true;
                return;
            }
        }

        protected void btnBlastFieldsConfig_Cancel(object sender, EventArgs e)
        {
            BlastFieldsConfig1.Reset();
            modalPopupBlastFieldsConfig.Hide();
        }

        protected void imgBtnBlastFieldsConfig1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            BlastFieldsConfig1.BlastFieldID = 1;
            BlastFieldsConfig1.loadData();
            modalPopupBlastFieldsConfig.Show();
        }

        protected void imgBtnBlastFieldsConfig2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            BlastFieldsConfig1.BlastFieldID = 2;
            BlastFieldsConfig1.loadData();
            modalPopupBlastFieldsConfig.Show();
        }

        protected void imgBtnBlastFieldsConfig3_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            BlastFieldsConfig1.BlastFieldID = 3;
            BlastFieldsConfig1.loadData();
            modalPopupBlastFieldsConfig.Show();
        }

        protected void imgBtnBlastFieldsConfig4_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            BlastFieldsConfig1.BlastFieldID = 4;
            BlastFieldsConfig1.loadData();
            modalPopupBlastFieldsConfig.Show();
        }

        protected void imgBtnBlastFieldsConfig5_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            BlastFieldsConfig1.BlastFieldID = 5;
            BlastFieldsConfig1.loadData();
            modalPopupBlastFieldsConfig.Show();
        }

        protected void drpBlastField1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBlastField1.SelectedValue.Equals(CustomValueName))
            {
                txtBlastField1.Visible = true;
            }
            else
            {
                txtBlastField1.Visible = false;
            }
        }

        protected void drpBlastField2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBlastField2.SelectedValue.Equals(CustomValueName))
            {
                txtBlastField2.Visible = true;
            }
            else
            {
                txtBlastField2.Visible = false;
            }
        }

        protected void drpBlastField3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBlastField3.SelectedValue.Equals(CustomValueName))
            {
                txtBlastField3.Visible = true;
            }
            else
            {
                txtBlastField3.Visible = false;
            }
        }

        protected void drpBlastField4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBlastField4.SelectedValue.Equals(CustomValueName))
            {
                txtBlastField4.Visible = true;
            }
            else
            {
                txtBlastField4.Visible = false;
            }
        }

        protected void drpBlastField5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBlastField5.SelectedValue.Equals(CustomValueName))
            {
                txtBlastField5.Visible = true;
            }
            else
            {
                txtBlastField5.Visible = false;
            }
        }
        #endregion

        private void loadTemplateData(int CampaignItemTemplateID, bool PrepopulateBlastFields)
        {
            if (CampaignItemTemplateID > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItemTemplate ciTemplate =
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(CampaignItemTemplateID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                lblCampaignItemTemplate.Text = ciTemplate.TemplateName;
                drpdownCampaign.SelectedValue = ciTemplate.CampaignID.ToString();
                if (PrepopulateBlastFields)
                {
                    if (ciTemplate.BlastField1 != null && ciTemplate.BlastField1 != string.Empty)
                    {
                        updateBlastField_Template(ciTemplate.BlastField1, drpBlastField1, txtBlastField1);
                    }
                    if (ciTemplate.BlastField2 != null && ciTemplate.BlastField2 != string.Empty)
                    {
                        updateBlastField_Template(ciTemplate.BlastField2, drpBlastField2, txtBlastField2);
                    }
                    if (ciTemplate.BlastField3 != null && ciTemplate.BlastField3 != string.Empty)
                    {
                        updateBlastField_Template(ciTemplate.BlastField3, drpBlastField3, txtBlastField3);
                    }
                    if (ciTemplate.BlastField4 != null && ciTemplate.BlastField4 != string.Empty)
                    {
                        updateBlastField_Template(ciTemplate.BlastField4, drpBlastField4, txtBlastField4);
                    }
                    if (ciTemplate.BlastField5 != null && ciTemplate.BlastField5 != string.Empty)
                    {
                        updateBlastField_Template(ciTemplate.BlastField5, drpBlastField5, txtBlastField5);
                    }
                }
            }
            else
            {
                lblCampaignItemTemplate.Text = " - No Template Selected -";
            }
        }

        private void updateBlastField_Template(string value, DropDownList dr, TextBox txt)
        {
            bool itemFound = false;
            foreach (ListItem item in dr.Items)
            {
                if (item.Value.Equals(value))
                {
                    dr.ClearSelection();
                    dr.SelectedValue = value;
                    itemFound = true;
                    break;
                }
            }
            if (!itemFound)
            {
                txt.Text = value;
                txt.Visible = true;
                dr.SelectedValue = CustomValueName;
            }
        }
    }
}
