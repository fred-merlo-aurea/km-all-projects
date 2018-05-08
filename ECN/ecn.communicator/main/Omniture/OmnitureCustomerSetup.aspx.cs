using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Xml;
using System.Text;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects.Communicator;

namespace ecn.communicator.main.Omniture
{
    public partial class OmnitureCustomerSetup : System.Web.UI.Page
    {
        private const string DelimiterSettingName = "Delimiter";
        private const string QueryStringSettingName = "QueryString";
        private const string OverrideSettingName = "Override";
        private const string SettingsXmlXPath = "/Settings";
        private const string HelpTitle = "Omniture";
        private const string DisplayNameFieldName = "DisplayName";
        private const string LtpoidFieldName = "LTPOID";
        private const string LTPIDAttributeName = "LTPID";
        private const string AllowCustomerOverrideSettingName = "AllowCustomerOverride";
        private const string False = "False";
        private const string True = "True";
        private const string IsDeleted = "IsDeleted";
        private const string Value = "Value";
        private const string IsDynamic = "IsDynamic";
        private const string IsDefault = "IsDefault";

        private DataTable ParamOptionsDT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["ParamOptionsDT"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["ParamOptionsDT"] = value;
            }
        }

        public int ParamID
        {
            get
            {
                try
                {
                    return (int)ViewState["ParamOptionID"];
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                ViewState["ParamOptionID"] = value;
            }
        }

        private string LabelID
        {
            get
            {
                try
                {
                    return (string)ViewState["LabelID"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["LabelID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeMasterPage();
            phError.Visible = false;
            pnlNoAccess.Visible = false;
            pnlContent.Visible = true;
            if (IsPostBack)
            {
                return;
            }

            if (Customer.HasProductFeature(
                ECNSession.CurrentSession().CurrentUser.CustomerID,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.Omniture))
            {
                var u = ECNSession.CurrentSession().CurrentUser;

                if (!(KM.Platform.User.IsSystemAdministrator(u) ||
                      KM.Platform.User.IsChannelAdministrator(u) ||
                      KM.Platform.User.IsAdministrator(u)))
                {
                    pnlContent.Visible = false;
                    pnlNoAccess.Visible = true;
                    return;
                }

                LoadLinkTrackingSettings();
                LoadLinkTrackingParams();

                var allowOverride = LoadCustomerOverrideMode();

                InitializeUserInterface(allowOverride);
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        private bool CheckSettingsChange()
        {
            ECN_Framework_Entities.Communicator.LinkTrackingSettings lts = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.GetByCustomerID_LTID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, 3);
            if (lts != null)
            {
                string XMLConfig = lts.XMLConfig;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XMLConfig);

                XmlNode rootNode = doc.SelectSingleNode("/Settings");

                if (rootNode != null && rootNode.HasChildNodes)
                {
                    string CustDoesOverride = rootNode["Override"].InnerText;


                    bool overRideBase = false;
                    bool.TryParse(CustDoesOverride, out overRideBase);

                    if (overRideBase != chkboxOverride.Checked && chkboxOverride.Checked)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        protected void btnSaveSettings_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(txtDelimiter.Text.Trim()) && !string.IsNullOrEmpty(txtQueryName.Text.Trim()))
            {
                Button btnSave = (Button)sender;
                ECN_Framework_Entities.Communicator.LinkTrackingSettings lts = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.GetByCustomerID_LTID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, 3);
                if (!CheckSettingsChange() && !btnSave.ID.ToLower().Equals("btnconfirmtemplate"))
                {
                    //clear out omniture values for Customer CampaignItemTemplates
                    List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> citList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetTemplatesBySetupLevel(Master.UserSession.BaseChannelID, Master.UserSession.CustomerID, false);
                    if (citList != null && citList.Count > 0)
                    {
                        string message = "";
                        message = "The following Campaign Item Template(s) are using Base Channel level Omniture values that will be deleted: <br /> ";
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplate cit in citList)
                        {
                            message += cit.TemplateName + ", ";
                        }
                        message = message.TrimEnd(' ');
                        message = message.TrimEnd(',');
                        //show message for 
                        lblTemplateMessage.Text = message;
                        mpeTemplateNotif.Show();
                        return;
                    }
                }
                else if (btnSave.ID.ToLower().Equals("btnconfirmtemplate"))
                {
                    //They've confirmed they want to clear out templates
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.ClearOutOmniDataBySetupLevel(Master.UserSession.BaseChannelID, Master.UserSession.CustomerID, false, Master.UserSession.CurrentUser.UserID);
                    mpeTemplateNotif.Hide();
                }


                if (lts != null && lts.LTSID > 0)
                {
                    if (IsQueryStringValid())
                    {
                        lts.UpdatedDate = DateTime.Now;
                        lts.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                        StringBuilder sbConfig = new StringBuilder();
                        sbConfig.Append("<Settings><Override>" + chkboxOverride.Checked.ToString() + "</Override>");
                        sbConfig.Append("<QueryString>" + txtQueryName.Text.Trim() + "</QueryString>");
                        sbConfig.Append("<Delimiter>" + txtDelimiter.Text.Trim() + "</Delimiter></Settings>");
                        lts.XMLConfig = sbConfig.ToString();
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.Update(lts);
                            success = true;
                        }
                        catch (ECN_Framework_Common.Objects.ECNException ex)
                        {
                            setECNError(ex);
                            success = false;
                        }
                    }
                    else
                    {
                        lblErrorMessage.Text = "Please enter a different query string";
                        phError.Visible = true;
                        return;
                    }
                }
                else
                {
                    if (IsQueryStringValid())
                    {
                        lts = new ECN_Framework_Entities.Communicator.LinkTrackingSettings();
                        lts.CreatedDate = DateTime.Now;
                        lts.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                        lts.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID;
                        lts.IsDeleted = false;
                        lts.LTID = 3;

                        StringBuilder sbConfig = new StringBuilder();
                        sbConfig.Append("<Settings><Override>" + chkboxOverride.Checked.ToString() + "</Override>");
                        sbConfig.Append("<QueryString>" + txtQueryName.Text.Trim() + "</QueryString>");
                        sbConfig.Append("<Delimiter>" + txtDelimiter.Text.Trim() + "</Delimiter></Settings>");
                        lts.XMLConfig = sbConfig.ToString();
                        try
                        {

                            ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.Insert(lts);
                            success = true;
                        }
                        catch (ECN_Framework_Common.Objects.ECNException ex)
                        {
                            setECNError(ex);
                            success = false;
                        }
                    }
                    else
                    {
                        lblErrorMessage.Text = "Please enter a different query string";
                        phError.Visible = true;
                        return;
                    }

                }

                #region LinkTrackingParam Settings
                List<ECN_Framework_Entities.Communicator.LinkTrackingParam> lstLTP = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParam.GetByLinkTrackingID(3);
                int ecnCustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID;
                int ecnUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                try
                {
                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps1 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture1")).LTPID, ecnCustomerID);
                    if (ltps1 != null && ltps1.LTPSID > 0)
                    {
                        ltps1.IsRequired = rblReqOmni1.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps1.AllowCustom = rblCustomOmni1.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps1.UpdatedDate = DateTime.Now;
                        ltps1.UpdatedUserID = ecnUserID;
                        ltps1.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps1);
                    }
                    else
                    {
                        ltps1 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps1.IsRequired = rblReqOmni1.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps1.AllowCustom = rblCustomOmni1.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps1.DisplayName = lblOmniture1.Text;
                        ltps1.CreatedDate = DateTime.Now;
                        ltps1.CreatedUserID = ecnUserID;
                        ltps1.CustomerID = ecnCustomerID;
                        ltps1.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture1")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps1);
                    }
                    if (!ddlOmniDefault1.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo1 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault1.SelectedValue.ToString()));
                        ltpo1.IsDefault = true;
                        ltpo1.UpdatedDate = DateTime.Now;
                        ltpo1.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo1);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps1.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps2 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture2")).LTPID, ecnCustomerID);
                    if (ltps2 != null && ltps2.LTPSID > 0)
                    {
                        ltps2.IsRequired = rblReqOmni2.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps2.AllowCustom = rblCustomOmni2.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps2.UpdatedDate = DateTime.Now;
                        ltps2.UpdatedUserID = ecnUserID;
                        ltps2.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps2);
                    }
                    else
                    {
                        ltps2 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps2.IsRequired = rblReqOmni2.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps2.AllowCustom = rblCustomOmni2.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps2.DisplayName = lblOmniture2.Text;
                        ltps2.CreatedDate = DateTime.Now;
                        ltps2.CreatedUserID = ecnUserID;
                        ltps2.CustomerID = ecnCustomerID;
                        ltps2.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture2")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps2);
                    }
                    if (!ddlOmniDefault2.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo2 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault2.SelectedValue.ToString()));
                        ltpo2.IsDefault = true;
                        ltpo2.UpdatedDate = DateTime.Now;
                        ltpo2.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo2);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps2.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps3 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture3")).LTPID, ecnCustomerID);
                    if (ltps3 != null && ltps3.LTPSID > 0)
                    {
                        ltps3.IsRequired = rblReqOmni3.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps3.AllowCustom = rblCustomOmni3.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps3.UpdatedDate = DateTime.Now;
                        ltps3.UpdatedUserID = ecnUserID;
                        ltps3.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps3);
                    }
                    else
                    {
                        ltps3 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps3.IsRequired = rblReqOmni3.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps3.AllowCustom = rblCustomOmni3.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps3.DisplayName = lblOmniture3.Text;
                        ltps3.CreatedDate = DateTime.Now;
                        ltps3.CreatedUserID = ecnUserID;
                        ltps3.CustomerID = ecnCustomerID;
                        ltps3.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture3")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps3);
                    }
                    if (!ddlOmniDefault3.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo3 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault3.SelectedValue.ToString()));
                        ltpo3.IsDefault = true;
                        ltpo3.UpdatedDate = DateTime.Now;
                        ltpo3.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo3);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps3.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps4 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture4")).LTPID, ecnCustomerID);
                    if (ltps4 != null && ltps4.LTPSID > 0)
                    {
                        ltps4.IsRequired = rblReqOmni4.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps4.AllowCustom = rblCustomOmni4.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps4.UpdatedDate = DateTime.Now;
                        ltps4.UpdatedUserID = ecnUserID;
                        ltps4.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps4);
                    }
                    else
                    {
                        ltps4 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps4.IsRequired = rblReqOmni4.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps4.AllowCustom = rblCustomOmni4.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps4.DisplayName = lblOmniture4.Text;
                        ltps4.CreatedDate = DateTime.Now;
                        ltps4.CreatedUserID = ecnUserID;
                        ltps4.CustomerID = ecnCustomerID;
                        ltps4.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture4")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps4);
                    }
                    if (!ddlOmniDefault4.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo4 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault4.SelectedValue.ToString()));
                        ltpo4.IsDefault = true;
                        ltpo4.UpdatedDate = DateTime.Now;
                        ltpo4.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo4);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps4.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps5 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture5")).LTPID, ecnCustomerID);
                    if (ltps5 != null && ltps5.LTPSID > 0)
                    {
                        ltps5.IsRequired = rblReqOmni5.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps5.AllowCustom = rblCustomOmni5.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps5.UpdatedDate = DateTime.Now;
                        ltps5.UpdatedUserID = ecnUserID;
                        ltps5.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps5);
                    }
                    else
                    {
                        ltps5 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps5.IsRequired = rblReqOmni5.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps5.AllowCustom = rblCustomOmni5.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps5.DisplayName = lblOmniture5.Text;
                        ltps5.CreatedDate = DateTime.Now;
                        ltps5.CreatedUserID = ecnUserID;
                        ltps5.CustomerID = ecnCustomerID;
                        ltps5.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture5")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps5);
                    }
                    if (!ddlOmniDefault5.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo5 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault5.SelectedValue.ToString()));
                        ltpo5.IsDefault = true;
                        ltpo5.UpdatedDate = DateTime.Now;
                        ltpo5.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo5);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps5.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps6 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture6")).LTPID, ecnCustomerID);
                    if (ltps6 != null && ltps6.LTPSID > 0)
                    {
                        ltps6.IsRequired = rblReqOmni6.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps6.AllowCustom = rblCustomOmni6.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps6.UpdatedDate = DateTime.Now;
                        ltps6.UpdatedUserID = ecnUserID;
                        ltps6.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps6);
                    }
                    else
                    {
                        ltps6 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps6.IsRequired = rblReqOmni6.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps6.AllowCustom = rblCustomOmni6.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps6.DisplayName = lblOmniture6.Text;
                        ltps6.CreatedDate = DateTime.Now;
                        ltps6.CreatedUserID = ecnUserID;
                        ltps6.CustomerID = ecnCustomerID;
                        ltps6.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture6")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps6);
                    }
                    if (!ddlOmniDefault6.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo6 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault6.SelectedValue.ToString()));
                        ltpo6.IsDefault = true;
                        ltpo6.UpdatedDate = DateTime.Now;
                        ltpo6.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo6);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps6.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps7 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture7")).LTPID, ecnCustomerID);
                    if (ltps7 != null && ltps7.LTPSID > 0)
                    {
                        ltps7.IsRequired = rblReqOmni7.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps7.AllowCustom = rblCustomOmni7.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps7.UpdatedDate = DateTime.Now;
                        ltps7.UpdatedUserID = ecnUserID;
                        ltps7.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps7);
                    }
                    else
                    {
                        ltps7 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps7.IsRequired = rblReqOmni7.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps7.AllowCustom = rblCustomOmni7.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps7.DisplayName = lblOmniture7.Text;
                        ltps7.CreatedDate = DateTime.Now;
                        ltps7.CreatedUserID = ecnUserID;
                        ltps7.CustomerID = ecnCustomerID;
                        ltps7.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture7")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps7);
                    }
                    if (!ddlOmniDefault7.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo7 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault7.SelectedValue.ToString()));
                        ltpo7.IsDefault = true;
                        ltpo7.UpdatedDate = DateTime.Now;
                        ltpo7.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo7);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps7.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps8 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture8")).LTPID, ecnCustomerID);
                    if (ltps8 != null && ltps8.LTPSID > 0)
                    {
                        ltps8.IsRequired = rblReqOmni8.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps8.AllowCustom = rblCustomOmni8.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps8.UpdatedDate = DateTime.Now;
                        ltps8.UpdatedUserID = ecnUserID;
                        ltps8.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps8);
                    }
                    else
                    {
                        ltps8 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps8.IsRequired = rblReqOmni8.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps8.AllowCustom = rblCustomOmni8.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps8.DisplayName = lblOmniture8.Text;
                        ltps8.CreatedDate = DateTime.Now;
                        ltps8.CreatedUserID = ecnUserID;
                        ltps8.CustomerID = ecnCustomerID;
                        ltps8.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture8")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps8);
                    }
                    if (!ddlOmniDefault8.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo8 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault8.SelectedValue.ToString()));
                        ltpo8.IsDefault = true;
                        ltpo8.UpdatedDate = DateTime.Now;
                        ltpo8.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo8);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps8.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps9 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture9")).LTPID, ecnCustomerID);
                    if (ltps9 != null && ltps9.LTPSID > 0)
                    {
                        ltps9.IsRequired = rblReqOmni9.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps9.AllowCustom = rblCustomOmni9.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps9.UpdatedDate = DateTime.Now;
                        ltps9.UpdatedUserID = ecnUserID;
                        ltps9.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps9);
                    }
                    else
                    {
                        ltps9 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps9.IsRequired = rblReqOmni9.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps9.AllowCustom = rblCustomOmni9.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps9.DisplayName = lblOmniture9.Text;
                        ltps9.CreatedDate = DateTime.Now;
                        ltps9.CreatedUserID = ecnUserID;
                        ltps9.CustomerID = ecnCustomerID;
                        ltps9.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture9")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps9);
                    }
                    if (!ddlOmniDefault9.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo9 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault9.SelectedValue.ToString()));
                        ltpo9.IsDefault = true;
                        ltpo9.UpdatedDate = DateTime.Now;
                        ltpo9.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo9);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps9.LTPID, ecnCustomerID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps10 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture10")).LTPID, ecnCustomerID);
                    if (ltps10 != null && ltps10.LTPSID > 0)
                    {
                        ltps10.IsRequired = rblReqOmni10.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps10.AllowCustom = rblCustomOmni10.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps10.UpdatedDate = DateTime.Now;
                        ltps10.UpdatedUserID = ecnUserID;
                        ltps10.CustomerID = ecnCustomerID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Update(ltps10);
                    }
                    else
                    {
                        ltps10 = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                        ltps10.IsRequired = rblReqOmni10.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps10.AllowCustom = rblCustomOmni10.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps10.DisplayName = lblOmniture10.Text;
                        ltps10.CreatedDate = DateTime.Now;
                        ltps10.CreatedUserID = ecnUserID;
                        ltps10.CustomerID = ecnCustomerID;
                        ltps10.LTPID = lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture10")).LTPID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Insert(ltps10);
                    }
                    if (!ddlOmniDefault10.SelectedValue.Equals("-1"))
                    {
                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo10 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(ddlOmniDefault10.SelectedValue.ToString()));
                        ltpo10.IsDefault = true;
                        ltpo10.UpdatedDate = DateTime.Now;
                        ltpo10.UpdatedUserID = ecnUserID;
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.Update(ltpo10);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(ltps10.LTPID, ecnCustomerID);
                    }

                    success = true;
                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    setECNError(ex);
                    success = false;
                }
                #endregion

                if (success)
                {
                    ShowMessage("Save Successful", "SUCCESS", Salesforce.Controls.Message.Message_Icon.info);
                }
                else
                {
                    ShowMessage("Save Unsuccessful", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                }

            }
            else if (!string.IsNullOrEmpty(txtQueryName.Text.Trim()) && string.IsNullOrEmpty(txtDelimiter.Text.Trim()))
            {
                lblErrorMessage.Text = "Please enter a character for the delimiter";
                phError.Visible = true;
            }
            else if (!string.IsNullOrEmpty(txtDelimiter.Text.Trim()) && string.IsNullOrEmpty(txtQueryName.Text.Trim()))
            {
                lblErrorMessage.Text = "Please enter a query string";
                phError.Visible = true;
            }
            else if (string.IsNullOrEmpty(txtQueryName.Text.Trim()) && string.IsNullOrEmpty(txtDelimiter.Text.Trim()))
            {
                lblErrorMessage.Text = "Please enter a character for the delimiter and a query string";
                phError.Visible = true;
            }
        }

        private bool IsQueryStringValid()
        {
            string queryName = txtQueryName.Text.ToLower();
            if (!queryName.Equals("eid") && !queryName.Equals("bid") && !queryName.Equals("utm_source") && !queryName.Equals("utm_medium") && !queryName.Equals("utm_campaign") && !queryName.Equals("utm_term") && !queryName.Equals("utm_content"))
            {
                return true;
            }
            else
                return false;
        }

        protected void imgbtnOmniEdit_Click(object sender, ImageClickEventArgs e)
        {
            int LTPID = -1;
            ImageButton imgbtn = (ImageButton)sender;
            txtOmniDisplayName.Text = "";
            txtOmniParamOption.Text = "";
            txtOmniParamName.Text = "";
            //check if it's been set up and load LinkTrackingParamOptions else load up an empty grid
            try
            {
                if (int.TryParse(imgbtn.Attributes[LTPIDAttributeName].ToString(), out LTPID))
                {
                    DataTable ltpoList = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetDT_LTPID_CustomerID(LTPID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltp = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(LTPID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
                    if (ltp != null)
                    {
                        txtOmniDisplayName.Text = ltp.DisplayName.Trim();
                    }

                    ParamOptionsDT = ltpoList;
                    ParamID = LTPID;

                    loadGrid();
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                ParamOptionsDT = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetDT_LTPID_CustomerID(LTPID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
            }
            LabelID = imgbtn.ID;
            lblEditError.Visible = false;
            modalPopupOmnitureConfig.Show();
        }

        private void loadGrid()
        {
            var result = (from src in ParamOptionsDT.AsEnumerable()
                          where src.Field<bool>("IsDeleted") == false && src.Field<bool>("IsDynamic") == false
                          select new
                          {
                              LTPOID = src.Field<string>(LtpoidFieldName),
                              DisplayName = src.Field<string>("DisplayName"),
                              Value = src.Field<string>("Value"),
                              IsDeleted = src.Field<bool>("IsDeleted"),
                              IsDynamic = src.Field<bool>("IsDynamic")
                          }).ToList();

            var dynamicResults = (from src in ParamOptionsDT.AsEnumerable()
                                  where src.Field<bool>("IsDeleted") == false && src.Field<bool>("IsDynamic") == true
                                  select new
                                  {
                                      LTPOID = src.Field<string>(LtpoidFieldName),
                                      DisplayName = src.Field<string>("DisplayName"),
                                      Value = src.Field<string>("Value"),
                                      IsDeleted = src.Field<bool>("IsDeleted"),
                                      IsDynamic = src.Field<bool>("IsDynamic")
                                  }).ToList();

            List<ListItem> liList = new List<ListItem>();
            ListItem liBlastID = new ListItem() { Text = "BlastID", Value = "BlastID" };
            ListItem liGroupName = new ListItem() { Text = "GroupName", Value = "GroupName" };
            for (int i = 0; i < dynamicResults.Count; i++)
            {
                if (dynamicResults[i].Value.ToString().ToLower().Equals("blastid"))
                {
                    liBlastID.Selected = true;
                }

                if (dynamicResults[i].Value.ToString().ToLower().Equals("groupname"))
                {
                    liGroupName.Selected = true;
                }
            }
            liList.Add(liBlastID);
            liList.Add(liGroupName);
            chklstDynamicFields.DataSource = liList;
            chklstDynamicFields.DataBind();
            if (liBlastID.Selected)
                chklstDynamicFields.Items[chklstDynamicFields.Items.IndexOf(liBlastID)].Selected = true;
            if (liGroupName.Selected)
                chklstDynamicFields.Items[chklstDynamicFields.Items.IndexOf(liGroupName)].Selected = true;

            gvOmniParamOptions.DataSource = result;
            gvOmniParamOptions.DataBind();
            if (result.Count > 0)
                gvOmniParamOptions.Visible = true;
            else
                gvOmniParamOptions.Visible = false;
        }

        protected void btnAddParamOption_Click(object sender, EventArgs e)
        {
            lblEditError.Visible = false;
            if (!txtOmniParamOption.Text.Replace(" ", "").ToLower().Equals("blastid") && !txtOmniParamOption.Text.Replace(" ", "").ToLower().Equals("groupname"))
            {
                if (ParamOptionsDT.Select("(Value = '" + txtOmniParamOption.Text.Trim() + "' OR DisplayName = '" + txtOmniParamName.Text.Trim() + "') AND IsDeleted = 'false'").Count() == 0)
                {
                    if (!string.IsNullOrEmpty(txtOmniParamOption.Text) && !string.IsNullOrEmpty(txtOmniParamName.Text))
                    {
                        DataTable dt = ParamOptionsDT;
                        if (dt == null)
                            dt = new DataTable();

                        DataRow dr = dt.NewRow();
                        dr[LtpoidFieldName] = Guid.NewGuid();
                        dr["DisplayName"] = txtOmniParamName.Text;
                        dr["Value"] = txtOmniParamOption.Text;
                        dr["IsDeleted"] = false;
                        dr["IsDynamic"] = false;

                        dt.Rows.Add(dr);
                        txtOmniParamOption.Text = "";
                        txtOmniParamName.Text = "";
                        ParamOptionsDT = dt;
                        loadGrid();
                    }
                    else
                    {
                        lblEditError.Text = "Please enter a value";
                        lblEditError.Visible = true;
                    }
                }
                else
                {
                    lblEditError.Text = "Value already exists";
                    lblEditError.Visible = true;
                }
            }
            else
            {
                lblEditError.Text = "Cannot use reserved values";
                lblEditError.Visible = true;
            }
        }

        protected void gvOmniParamOptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblEditError.Visible = false;
            string ltpoid = e.CommandArgument.ToString();

            if (e.CommandName == "ValueDelete")
            {
                foreach (DataRow dr in ParamOptionsDT.AsEnumerable())
                {
                    if (dr[LtpoidFieldName].Equals(ltpoid))
                    {
                        if (!ltpoid.Contains("-"))
                        {
                            List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> listCILT = ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking.GetByLinkTrackingParamOptionID(Convert.ToInt32(ltpoid));
                            if (listCILT.Count > 0)
                            {
                                lblEditError.Text = "Cannot delete, value used in blast";
                                lblEditError.Visible = true;
                            }
                            else
                            {
                                dr["IsDeleted"] = true;
                            }
                        }
                        else
                        {
                            dr["IsDeleted"] = true;
                        }
                    }
                }
                loadGrid();
            }
        }

        protected void btnOmniEditSave_Click(object sender, EventArgs e)
        {
            var user = ECNSession.CurrentSession().CurrentUser;
            var linkTrackingSettings = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();

            linkTrackingSettings = LinkTrackingParamSettings.Get_LTPID_CustomerID(ParamID, user.CustomerID);
            if (linkTrackingSettings != null && linkTrackingSettings.LTPSID > 0)
            {
                UpdateLinkTrackingSettings(linkTrackingSettings, user);
            }
            else
            {
                linkTrackingSettings = InsertLinkTrackingSettings(user);
            }

            UpdateDynamicFields();

            if (ParamOptionsDT != null && ParamOptionsDT.Rows.Count > 0 && linkTrackingSettings.LTPSID > 0)
            {
                foreach (DataRow dataRow in ParamOptionsDT.AsEnumerable())
                {
                    var isDeleted = dataRow[IsDeleted].ToString();
                    if (dataRow[LtpoidFieldName].ToString().Contains("-") && isDeleted.Equals(False, StringComparison.OrdinalIgnoreCase))
                    {
                        InsertLinkTrackingOption(dataRow, user);
                    }

                    if (!dataRow[LtpoidFieldName].ToString().Contains("-") && isDeleted.Equals(True, StringComparison.OrdinalIgnoreCase))
                    {
                        DeleteLinkTrackingOption(dataRow[LtpoidFieldName].ToString(), user);
                    }
                }
            }

            SetOmnitureUI(user, linkTrackingSettings);

            modalPopupOmnitureConfig.Hide();
            txtOmniDisplayName.Text = string.Empty;
            ParamOptionsDT = new DataTable();
        }

        private void UpdateLinkTrackingSettings(ECN_Framework_Entities.Communicator.LinkTrackingParamSettings linkTrackingSettings, KMPlatform.Entity.User user)
        {
            linkTrackingSettings.UpdatedUserID = user.UserID;
            linkTrackingSettings.UpdatedDate = DateTime.Now;
            linkTrackingSettings.DisplayName = txtOmniDisplayName.Text.Trim();

            try
            {
                LinkTrackingParamSettings.Update(linkTrackingSettings);
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        private ECN_Framework_Entities.Communicator.LinkTrackingParamSettings InsertLinkTrackingSettings(KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamSettings linkTrackingSettings = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
            linkTrackingSettings.CreatedUserID = user.UserID;
            linkTrackingSettings.CreatedDate = DateTime.Now;
            linkTrackingSettings.DisplayName = txtOmniDisplayName.Text.Trim();
            linkTrackingSettings.LTPID = ParamID;
            linkTrackingSettings.CustomerID = ECNSession.CurrentSession().CurrentCustomer.CustomerID;

            try
            {
                linkTrackingSettings.LTPSID = LinkTrackingParamSettings.Insert(linkTrackingSettings);
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }

            return linkTrackingSettings;
        }

        private void UpdateDynamicFields()
        {
            foreach (ListItem item in chklstDynamicFields.Items)
            {
                var itemDynamicRows = ParamOptionsDT.Select($"Value = '{item.Value}' and IsDynamic = true");
                if (item.Selected)
                {
                    if (itemDynamicRows.Length == 0)
                    {
                        var dataRow = ParamOptionsDT.NewRow();
                        dataRow[LtpoidFieldName] = Guid.NewGuid();
                        dataRow[DisplayNameFieldName] = item.Text;
                        dataRow[Value] = item.Value;
                        dataRow[IsDeleted] = false;
                        dataRow[IsDynamic] = true;
                        dataRow[IsDefault] = false;
                        ParamOptionsDT.Rows.Add(dataRow);
                    }
                }
                else if (itemDynamicRows.Length > 0)
                {
                    var dataRow = itemDynamicRows.First();
                    ParamOptionsDT.Rows[ParamOptionsDT.Rows.IndexOf(dataRow)][IsDeleted] = true;
                }
            }
        }

        private void InsertLinkTrackingOption(DataRow dataRow, KMPlatform.Entity.User user)
        {
            var isDefault = false;
            bool.TryParse(dataRow[IsDefault].ToString(), out isDefault);

            var linkTrackingOption = new ECN_Framework_Entities.Communicator.LinkTrackingParamOption
            {
                Value = dataRow[Value].ToString(),
                DisplayName = dataRow[DisplayNameFieldName].ToString(),
                ColumnName = null,
                IsDefault = isDefault,
                IsActive = true,
                IsDeleted = false,
                IsDynamic = Convert.ToBoolean(dataRow[IsDynamic].ToString()),
                CustomerID = user.CustomerID,
                CreatedUserID = user.UserID,
                CreatedDate = DateTime.Now,
                LTPID = ParamID
            };

            try
            {
                LinkTrackingParamOption.Insert(linkTrackingOption);
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void DeleteLinkTrackingOption(string linkTrackingOptionId, KMPlatform.Entity.User user)
        {
            var actualID = -1;
            int.TryParse(linkTrackingOptionId, out actualID);
            if (actualID > 0)
            {
                var linkTrackingOption = LinkTrackingParamOption.GetByLTPOID(actualID);
                linkTrackingOption.IsDeleted = true;
                linkTrackingOption.IsActive = false;
                linkTrackingOption.UpdatedDate = DateTime.Now;
                linkTrackingOption.UpdatedUserID = user.UserID;

                try
                {
                    LinkTrackingParamOption.Delete(linkTrackingOption);
                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    setECNError(ex);
                }
            }
        }

        private void SetOmnitureUI(KMPlatform.Entity.User user, ECN_Framework_Entities.Communicator.LinkTrackingParamSettings linkTrackingSettings)
        {
            switch (LabelID)
            {
                case "imgbtnOmni1":
                    SetOmnitureUI(lblOmniture1, ddlOmniDefault1, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni2":
                    SetOmnitureUI(lblOmniture2, ddlOmniDefault2, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni3":
                    SetOmnitureUI(lblOmniture3, ddlOmniDefault3, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni4":
                    SetOmnitureUI(lblOmniture4, ddlOmniDefault4, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni5":
                    SetOmnitureUI(lblOmniture5, ddlOmniDefault5, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni6":
                    SetOmnitureUI(lblOmniture6, ddlOmniDefault6, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni7":
                    SetOmnitureUI(lblOmniture7, ddlOmniDefault7, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni8":
                    SetOmnitureUI(lblOmniture8, ddlOmniDefault8, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni9":
                    SetOmnitureUI(lblOmniture9, ddlOmniDefault9, user, linkTrackingSettings);
                    break;
                case "imgbtnOmni10":
                    SetOmnitureUI(lblOmniture10, ddlOmniDefault10, user, linkTrackingSettings);
                    break;
            }
        }

        private void SetOmnitureUI(Label omnitureLabel, DropDownList omnitureDefaultDropDown, KMPlatform.Entity.User user, ECN_Framework_Entities.Communicator.LinkTrackingParamSettings linkTrackingSettings)
        {
            omnitureLabel.Text = linkTrackingSettings.DisplayName;
            var linkTrackingOption = LinkTrackingParamOption.Get_LTPID_CustomerID(ParamID, user.CustomerID);
            omnitureDefaultDropDown.DataSource = linkTrackingOption;
            omnitureDefaultDropDown.DataTextField = DisplayNameFieldName;
            omnitureDefaultDropDown.DataValueField = LtpoidFieldName;
            omnitureDefaultDropDown.DataBind();
            omnitureDefaultDropDown.Items.Insert(0, new ListItem { Selected = true, Text = "-Select-", Value = "-1" });

            if (linkTrackingOption.Exists(x => x.IsDefault))
            {
                omnitureDefaultDropDown.SelectedValue = linkTrackingOption.First(x => x.IsDefault).LTPOID.ToString();
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

        private void ShowMessage(string msg, string title, Salesforce.Controls.Message.Message_Icon icon)
        {
            kmMsg.Show(msg, title, icon);
        }

        private void InitializeUserInterface(bool allowOverride)
        {
            btnSaveSettings.Enabled = allowOverride;
            txtDelimiter.Enabled = allowOverride;
            txtQueryName.Enabled = allowOverride;

            imgbtnOmni1.Enabled = allowOverride;
            imgbtnOmni2.Enabled = allowOverride;
            imgbtnOmni3.Enabled = allowOverride;
            imgbtnOmni4.Enabled = allowOverride;
            imgbtnOmni5.Enabled = allowOverride;
            imgbtnOmni6.Enabled = allowOverride;
            imgbtnOmni7.Enabled = allowOverride;
            imgbtnOmni8.Enabled = allowOverride;
            imgbtnOmni9.Enabled = allowOverride;
            imgbtnOmni10.Enabled = allowOverride;

            ddlOmniDefault1.Enabled = allowOverride;
            ddlOmniDefault2.Enabled = allowOverride;
            ddlOmniDefault3.Enabled = allowOverride;
            ddlOmniDefault4.Enabled = allowOverride;
            ddlOmniDefault5.Enabled = allowOverride;
            ddlOmniDefault6.Enabled = allowOverride;
            ddlOmniDefault7.Enabled = allowOverride;
            ddlOmniDefault8.Enabled = allowOverride;
            ddlOmniDefault9.Enabled = allowOverride;
            ddlOmniDefault10.Enabled = allowOverride;

            rblCustomOmni1.Enabled = allowOverride;
            rblCustomOmni2.Enabled = allowOverride;
            rblCustomOmni3.Enabled = allowOverride;
            rblCustomOmni4.Enabled = allowOverride;
            rblCustomOmni5.Enabled = allowOverride;
            rblCustomOmni6.Enabled = allowOverride;
            rblCustomOmni7.Enabled = allowOverride;
            rblCustomOmni8.Enabled = allowOverride;
            rblCustomOmni9.Enabled = allowOverride;
            rblCustomOmni10.Enabled = allowOverride;

            rblReqOmni1.Enabled = allowOverride;
            rblReqOmni2.Enabled = allowOverride;
            rblReqOmni3.Enabled = allowOverride;
            rblReqOmni4.Enabled = allowOverride;
            rblReqOmni5.Enabled = allowOverride;
            rblReqOmni6.Enabled = allowOverride;
            rblReqOmni7.Enabled = allowOverride;
            rblReqOmni8.Enabled = allowOverride;
            rblReqOmni9.Enabled = allowOverride;
            rblReqOmni10.Enabled = allowOverride;
        }

        private bool LoadCustomerOverrideMode()
        {
            var ltsBase = LinkTrackingSettings.GetByBaseChannelID_LTID(Master.UserSession.CurrentBaseChannel.BaseChannelID, 3);
            var xDoc = new XmlDocument();
            if (ltsBase == null)
            {
                return false;
            }

            xDoc.LoadXml(ltsBase.XMLConfig);
            var node = xDoc.SelectSingleNode(SettingsXmlXPath);
            if (node == null || !node.HasChildNodes)
            {
                return false;
            }

            var allowCustOverride = node[AllowCustomerOverrideSettingName]?.InnerText;
            bool allowOverride;
            bool.TryParse(allowCustOverride, out allowOverride);
            return allowOverride;
        }

        private void LoadLinkTrackingParams()
        {
            try
            {
                var customerId = ECNSession.CurrentSession().CurrentCustomer.CustomerID;
                var ltpList = LinkTrackingParam.GetByLinkTrackingID(3);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture1",
                    imgbtnOmni1,
                    lblOmniture1,
                    rblReqOmni1,
                    rblCustomOmni1,
                    ddlOmniDefault1);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture2",
                    imgbtnOmni2,
                    lblOmniture2,
                    rblReqOmni2,
                    rblCustomOmni2,
                    ddlOmniDefault2);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture3",
                    imgbtnOmni3,
                    lblOmniture3,
                    rblReqOmni3,
                    rblCustomOmni3,
                    ddlOmniDefault3);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture4",
                    imgbtnOmni4,
                    lblOmniture4,
                    rblReqOmni4,
                    rblCustomOmni4,
                    ddlOmniDefault4);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture5",
                    imgbtnOmni5,
                    lblOmniture5,
                    rblReqOmni5,
                    rblCustomOmni5,
                    ddlOmniDefault5);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture6",
                    imgbtnOmni6,
                    lblOmniture6,
                    rblReqOmni6,
                    rblCustomOmni6,
                    ddlOmniDefault6);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture7",
                    imgbtnOmni7,
                    lblOmniture7,
                    rblReqOmni7,
                    rblCustomOmni7,
                    ddlOmniDefault7);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture8",
                    imgbtnOmni8,
                    lblOmniture8,
                    rblReqOmni8,
                    rblCustomOmni8,
                    ddlOmniDefault8);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture9",
                    imgbtnOmni9,
                    lblOmniture9,
                    rblReqOmni9,
                    rblCustomOmni9,
                    ddlOmniDefault9);

                InitializeOmnitureChannel(
                    ltpList,
                    customerId,
                    "omniture10",
                    imgbtnOmni10,
                    lblOmniture10,
                    rblReqOmni10,
                    rblCustomOmni10,
                    ddlOmniDefault10);
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void InitializeOmnitureChannel(
            IEnumerable<ECN_Framework_Entities.Communicator.LinkTrackingParam> ltpList,
            int customerId,
            string channelName,
            ImageButton omniImageButton,
            Label omniLabel,
            RadioButtonList omniReqRbl,
            RadioButtonList omniCustomRbl,
            DropDownList omniDefaultDdl)
        {
            var ltpItem = ltpList.First(x => x.DisplayName.Equals(channelName, StringComparison.OrdinalIgnoreCase));

            var ltps1 = LinkTrackingParamSettings.Get_LTPID_CustomerID(ltpItem.LTPID, customerId);
            var ltpoOmni1 = LinkTrackingParamOption.Get_LTPID_CustomerID(ltpItem.LTPID, customerId);
            omniImageButton.Attributes.Add(LTPIDAttributeName, ltpItem.LTPID.ToString());
            if (ltps1 != null)
            {
                omniLabel.Text = ltps1.DisplayName;
                omniReqRbl.SelectedValue = ltps1.IsRequired ? "1" : "0";
                omniCustomRbl.SelectedValue = ltps1.AllowCustom ? "1" : "0";
            }

            omniDefaultDdl.DataSource = ltpoOmni1;
            omniDefaultDdl.DataTextField = DisplayNameFieldName;
            omniDefaultDdl.DataValueField = LtpoidFieldName;
            omniDefaultDdl.DataBind();
            omniDefaultDdl.Items.Insert(0, new ListItem() { Selected = true, Text = "-Select-", Value = "-1" });
            if (ltpoOmni1.Exists(x => x.IsDefault))
            {
                omniDefaultDdl.SelectedValue = ltpoOmni1.First(x => x.IsDefault).LTPOID.ToString();
            }
        }

        private void LoadLinkTrackingSettings()
        {
            var lts = LinkTrackingSettings.GetByCustomerID_LTID(ECNSession.CurrentSession().CurrentCustomer.CustomerID, 3);
            if (lts == null)
            {
                return;
            }

            var xmlConfig = lts.XMLConfig;
            var doc = new XmlDocument();
            doc.LoadXml(xmlConfig);

            var rootNode = doc.SelectSingleNode(SettingsXmlXPath);

            if (rootNode == null || !rootNode.HasChildNodes)
            {
                return;
            }

            var overrideBaseChannel = rootNode[OverrideSettingName]?.InnerText;
            var queryString = rootNode[QueryStringSettingName]?.InnerText ?? string.Empty;
            var delimiter = rootNode[DelimiterSettingName]?.InnerText ?? string.Empty;

            bool overRideBase;
            bool.TryParse(overrideBaseChannel, out overRideBase);

            chkboxOverride.Checked = overRideBase;
            txtQueryName.Text = queryString.Trim();
            txtDelimiter.Text = delimiter.Trim();
        }

        private void InitializeMasterPage()
        {
            Master.CurrentMenuCode = Enums.MenuCode.OMNITURE;
            Master.SubMenu = string.Empty;
            Master.Heading = string.Empty;
            Master.HelpContent = string.Empty;
            Master.HelpTitle = HelpTitle;
        }
    }

}