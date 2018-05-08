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
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using KM.Common.Extensions;
using KMPlatform.Entity;
using BusinessLinkTrackingParamSettings = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings;
using BusinessLinkTrackingParamOption = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption;
using BusinessCustomer = ECN_Framework_BusinessLayer.Accounts.Customer;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator.Enums;
using KMPlatformUser = KM.Platform.User;
using PlatformEnums = KMPlatform.Enums;

namespace ecn.communicator.main.Omniture
{
    public partial class OmnitureBaseChannelSetup : System.Web.UI.Page
    {
        private const string LtpoidFieldName = "LTPOID";
        private const string DisplayNameFieldName = "DisplayName";
        private const string IsDefaultColumnName = "IsDefault";
        private const string IsDynamicColumnName = "IsDynamic";
        private const string IsDeletedColumnName = "IsDeleted";
        private const string ValueColumnName = "Value";
        private const string HelpTypeOmniture = "Omniture";
        private const int OmnitureLtId3 = 3;
        private const string XmlNodeSettings = "/Settings";
        private const string XmlNodeAllowCustomerOverride = "AllowCustomerOverride";
        private const string XmlNodeQueryString = "QueryString";
        private const string XmlNodeDelimiter = "Delimiter";
        private const string AttributeLptId = "LTPID";
        private const string SelectedValue0 = "0";
        private const string SelectedValue1 = "1";
        private const string SelectedValueMinus1 = "-1";
        private const string LiteralSelect = "-Select-";
        private const string OmnitureTemplate = "omniture{0}";

        private DataTable ParamOptionsDT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["ParamOptionsBaseDT"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["ParamOptionsBaseDT"] = value;
            }
        }

        public int ParamID
        {
            get
            {
                try
                {
                    return (int)ViewState["ParamOptionBaseID"];
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                ViewState["ParamOptionBaseID"] = value;
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
            Master.CurrentMenuCode = CommunicatorEnums.MenuCode.OMNITURE;
            Master.SubMenu = string.Empty;
            Master.Heading = string.Empty;
            Master.HelpContent = string.Empty;
            Master.HelpTitle = HelpTypeOmniture;
            phError.Visible = false;
            pnlNoAccess.Visible = false;
            pnlContent.Visible = true;
            if (!IsPostBack)
            {
                var currentUser = ECNSession.CurrentSession().CurrentUser;

                if (!(KMPlatformUser.IsSystemAdministrator(currentUser) || 
                      KMPlatformUser.IsChannelAdministrator(currentUser)) || 
                      !BusinessCustomer.HasProductFeature(
                          ECNSession.CurrentSession().CurrentUser.CustomerID,
                          PlatformEnums.Services.EMAILMARKETING,
                          PlatformEnums.ServiceFeatures.Omniture))
                {
                    pnlContent.Visible = false;
                    pnlNoAccess.Visible = true;
                    return;
                }

                PageLoadLInkTrackingSettings();

                PageLoadLinkTrackingParams();
            }
        }

        private void PageLoadLinkTrackingParams()
        {
            try
            {
                var baseChannelId = ECNSession.CurrentSession().CurrentCustomer.BaseChannelID.Value;
                var ltpList = BusinessCommunicator.LinkTrackingParam.GetByLinkTrackingID(3);

                var argsList = new[] 
                {
                    new OmniArgs(imgbtnOmni1, lblOmniture1, rblReqOmni1, rblCustomOmni1, ddlOmniDefault1),
                    new OmniArgs(imgbtnOmni2, lblOmniture2, rblReqOmni2, rblCustomOmni2, ddlOmniDefault2),
                    new OmniArgs(imgbtnOmni3, lblOmniture3, rblReqOmni3, rblCustomOmni3, ddlOmniDefault3),
                    new OmniArgs(imgbtnOmni4, lblOmniture4, rblReqOmni4, rblCustomOmni4, ddlOmniDefault4),
                    new OmniArgs(imgbtnOmni5, lblOmniture5, rblReqOmni5, rblCustomOmni5, ddlOmniDefault5),
                    new OmniArgs(imgbtnOmni6, lblOmniture6, rblReqOmni6, rblCustomOmni6, ddlOmniDefault6),
                    new OmniArgs(imgbtnOmni7, lblOmniture7, rblReqOmni7, rblCustomOmni7, ddlOmniDefault7),
                    new OmniArgs(imgbtnOmni8, lblOmniture8, rblReqOmni8, rblCustomOmni8, ddlOmniDefault8),
                    new OmniArgs(imgbtnOmni9, lblOmniture9, rblReqOmni9, rblCustomOmni9, ddlOmniDefault9),
                    new OmniArgs(imgbtnOmni10, lblOmniture10, rblReqOmni10, rblCustomOmni10, ddlOmniDefault10)
                };

                for (var iArgs = 0; iArgs < argsList.Length; iArgs++)
                {
                    var args = argsList[iArgs];
                    args.LtpList = ltpList;
                    args.BaseChannelId = baseChannelId;
                    args.ParamValueOmniture = string.Format(OmnitureTemplate, iArgs + 1);

                    ProcessOmni(args);
                }
            }
            catch (ECNException exception)
            {
                setECNError(exception);
            }
        }

        private void ProcessOmni(OmniArgs omniArgs)
        {
            Guard.NotNull(omniArgs, nameof(omniArgs));
            omniArgs.EnsureNotNull();

            var firstLtpId = omniArgs.LtpList.First(
                x => omniArgs.ParamValueOmniture.EqualsIgnoreCase(x.DisplayName)).LTPID;

            var linkTrackingParamSettings = BusinessLinkTrackingParamSettings.Get_LTPID_BaseChannelID(
                firstLtpId, 
                omniArgs.BaseChannelId);

            var trackingParamOptions = BusinessLinkTrackingParamOption.Get_LTPID_BaseChannelID(
                firstLtpId, 
                omniArgs.BaseChannelId);

            omniArgs.ImgBtnOmni.Attributes.Add(
                AttributeLptId,
                firstLtpId.ToString());

            if (linkTrackingParamSettings != null)
            {
                omniArgs.LblOmniture.Text = linkTrackingParamSettings.DisplayName;
                omniArgs.RblReqOmni.SelectedValue = linkTrackingParamSettings.IsRequired ?
                    SelectedValue1 :
                    SelectedValue0;
                omniArgs.RblCustomOmni.SelectedValue = linkTrackingParamSettings.AllowCustom ?
                    SelectedValue1 :
                    SelectedValue0;
            }

            omniArgs.DdlOmniDefault.DataSource = trackingParamOptions;
            omniArgs.DdlOmniDefault.DataTextField = DisplayNameFieldName;
            omniArgs.DdlOmniDefault.DataValueField = LtpoidFieldName;
            omniArgs.DdlOmniDefault.DataBind();
            var newListItemOmni = new ListItem { Selected = true, Text = LiteralSelect, Value = SelectedValueMinus1 };
            omniArgs.DdlOmniDefault.Items.Insert(0, newListItemOmni);
            if (trackingParamOptions.Exists(x => x.IsDefault))
            {
                omniArgs.DdlOmniDefault.SelectedValue = trackingParamOptions.First(x => x.IsDefault).LTPOID.ToString();
            }
        }

        private void PageLoadLInkTrackingSettings()
        {
            var baseChannelIdLtid = BusinessCommunicator.LinkTrackingSettings.GetByBaseChannelID_LTID(
                ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, OmnitureLtId3);
            if (baseChannelIdLtid != null)
            {
                var xmlConfig = baseChannelIdLtid.XMLConfig;
                var document = new XmlDocument();
                document.LoadXml(xmlConfig);

                var rootNode = document.SelectSingleNode(XmlNodeSettings);

                if (rootNode != null && rootNode.HasChildNodes)
                {
                    var overrideBaseChannel = rootNode[XmlNodeAllowCustomerOverride].InnerText;
                    var queryString = rootNode[XmlNodeQueryString].InnerText;
                    var delimiter = rootNode[XmlNodeDelimiter].InnerText;

                    bool overRideBase;
                    bool.TryParse(overrideBaseChannel, out overRideBase);

                    chkboxOverride.Checked = overRideBase;
                    txtQueryName.Text = queryString.Trim();
                    txtDelimiter.Text = delimiter.Trim();
                }
            }
        }

        private bool CheckSettingsChange()
        {
            ECN_Framework_Entities.Communicator.LinkTrackingSettings lts = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.GetByBaseChannelID_LTID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, 3);
            if (lts != null)
            {
                string XMLConfig = lts.XMLConfig;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XMLConfig);

                XmlNode rootNode = doc.SelectSingleNode("/Settings");

                if (rootNode != null && rootNode.HasChildNodes)
                {
                    string allowCustOverride = rootNode["AllowCustomerOverride"].InnerText;


                    bool overRideBase = false;
                    bool.TryParse(allowCustOverride, out overRideBase);

                    if (overRideBase != chkboxOverride.Checked && !chkboxOverride.Checked)
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
                ECN_Framework_Entities.Communicator.LinkTrackingSettings lts = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.GetByBaseChannelID_LTID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.BaseChannelID.Value, 3);
                if (!CheckSettingsChange() && !btnSave.ID.ToLower().Equals("btnconfirmtemplate"))
                {
                    //clear out omniture values for Customer CampaignItemTemplates
                    List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> citList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetTemplatesBySetupLevel(Master.UserSession.BaseChannelID,null, true);
                    if (citList != null && citList.Count > 0)
                    {
                        string message = "";
                        message = "The following Campaign Item Template(s) are using Customer level Omniture values that will be deleted: <br /> ";
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
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.ClearOutOmniDataBySetupLevel(Master.UserSession.BaseChannelID,null, true, Master.UserSession.CurrentUser.UserID);
                    mpeTemplateNotif.Hide();
                }

                if (lts != null && lts.LTSID > 0)
                {
                    if (IsQueryStringValid())
                    {
                        lts.UpdatedDate = DateTime.Now;
                        lts.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                        StringBuilder sbConfig = new StringBuilder();
                        sbConfig.Append("<Settings><AllowCustomerOverride>" + chkboxOverride.Checked.ToString() + "</AllowCustomerOverride>");
                        sbConfig.Append("<QueryString>" + txtQueryName.Text.Trim() + "</QueryString>");
                        sbConfig.Append("<Delimiter>" + txtDelimiter.Text.Trim() + "</Delimiter></Settings>");
                        lts.XMLConfig = sbConfig.ToString();
                        
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.Update(lts);
                            if(!chkboxOverride.Checked)
                            {
                                //update all customer level omniture settings to not override basechannel
                                ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.UpdateCustomerOmnitureOverride(Master.UserSession.BaseChannelID, false, Master.UserSession.CurrentUser.UserID);
                            }
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
                        lblErrorMessage.Text = "Please enter a different value for the query string";
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
                        lts.BaseChannelID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.BaseChannelID.Value;
                        lts.IsDeleted = false;
                        lts.LTID = 3;

                        StringBuilder sbConfig = new StringBuilder();
                        sbConfig.Append("<Settings><AllowCustomerOverride>" + chkboxOverride.Checked.ToString() + "</AllowCustomerOverride>");
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
                        lblErrorMessage.Text = "Please enter a different value for the query string";
                        phError.Visible = true;
                        return;
                    }
                }

                #region LinkTrackingParam Settings
                List<ECN_Framework_Entities.Communicator.LinkTrackingParam> lstLTP = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParam.GetByLinkTrackingID(3);
                int ecnBaseChannelID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.BaseChannelID.Value;
                int ecnUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                try
                {
                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps1 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture1")).LTPID, ecnBaseChannelID);
                    if (ltps1 != null && ltps1.LTPSID > 0)
                    {
                        ltps1.IsRequired = rblReqOmni1.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps1.AllowCustom = rblCustomOmni1.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps1.UpdatedDate = DateTime.Now;
                        ltps1.UpdatedUserID = ecnUserID;
                        ltps1.BaseChannelID = ecnBaseChannelID;
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
                        ltps1.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps1 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps1.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps2 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture2")).LTPID, ecnBaseChannelID);
                    if (ltps2 != null && ltps2.LTPSID > 0)
                    {
                        ltps2.IsRequired = rblReqOmni2.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps2.AllowCustom = rblCustomOmni2.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps2.UpdatedDate = DateTime.Now;
                        ltps2.UpdatedUserID = ecnUserID;
                        ltps2.BaseChannelID = ecnBaseChannelID;
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
                        ltps2.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps2 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps2.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps3 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture3")).LTPID, ecnBaseChannelID);
                    if (ltps3 != null && ltps3.LTPSID > 0)
                    {
                        ltps3.IsRequired = rblReqOmni3.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps3.AllowCustom = rblCustomOmni3.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps3.UpdatedDate = DateTime.Now;
                        ltps3.UpdatedUserID = ecnUserID;
                        ltps3.BaseChannelID = ecnBaseChannelID;
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
                        ltps3.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps3 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps3.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps4 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture4")).LTPID, ecnBaseChannelID);
                    if (ltps4 != null && ltps4.LTPSID > 0)
                    {
                        ltps4.IsRequired = rblReqOmni4.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps4.AllowCustom = rblCustomOmni4.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps4.UpdatedDate = DateTime.Now;
                        ltps4.UpdatedUserID = ecnUserID;
                        ltps4.BaseChannelID = ecnBaseChannelID;
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
                        ltps4.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps4 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps4.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps5 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture5")).LTPID, ecnBaseChannelID);
                    if (ltps5 != null && ltps5.LTPSID > 0)
                    {
                        ltps5.IsRequired = rblReqOmni5.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps5.AllowCustom = rblCustomOmni5.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps5.UpdatedDate = DateTime.Now;
                        ltps5.UpdatedUserID = ecnUserID;
                        ltps5.BaseChannelID = ecnBaseChannelID;
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
                        ltps5.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps5 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps5.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps6 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture6")).LTPID, ecnBaseChannelID);
                    if (ltps6 != null && ltps6.LTPSID > 0)
                    {
                        ltps6.IsRequired = rblReqOmni6.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps6.AllowCustom = rblCustomOmni6.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps6.UpdatedDate = DateTime.Now;
                        ltps6.UpdatedUserID = ecnUserID;
                        ltps6.BaseChannelID = ecnBaseChannelID;
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
                        ltps6.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps6 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps6.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps7 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture7")).LTPID, ecnBaseChannelID);
                    if (ltps7 != null && ltps7.LTPSID > 0)
                    {
                        ltps7.IsRequired = rblReqOmni7.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps7.AllowCustom = rblCustomOmni7.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps7.UpdatedDate = DateTime.Now;
                        ltps7.UpdatedUserID = ecnUserID;
                        ltps7.BaseChannelID = ecnBaseChannelID;
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
                        ltps7.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps7 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps7.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps8 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture8")).LTPID, ecnBaseChannelID);
                    if (ltps8 != null && ltps8.LTPSID > 0)
                    {
                        ltps8.IsRequired = rblReqOmni8.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps8.AllowCustom = rblCustomOmni8.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps8.UpdatedDate = DateTime.Now;
                        ltps8.UpdatedUserID = ecnUserID;
                        ltps8.BaseChannelID = ecnBaseChannelID;
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
                        ltps8.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps8 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps8.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps9 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture9")).LTPID, ecnBaseChannelID);
                    if (ltps9 != null && ltps9.LTPSID > 0)
                    {
                        ltps9.IsRequired = rblReqOmni9.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps9.AllowCustom = rblCustomOmni9.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps9.UpdatedDate = DateTime.Now;
                        ltps9.UpdatedUserID = ecnUserID;
                        ltps9.BaseChannelID = ecnBaseChannelID;
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
                        ltps9.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps9 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps9.LTPID, ecnBaseChannelID);
                    }

                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps10 = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(lstLTP.First(x => x.DisplayName.ToLower().Equals("omniture10")).LTPID, ecnBaseChannelID);
                    if (ltps10 != null && ltps10.LTPSID > 0)
                    {
                        ltps10.IsRequired = rblReqOmni10.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps10.AllowCustom = rblCustomOmni10.SelectedValue.ToString().Equals("1") ? true : false;
                        ltps10.UpdatedDate = DateTime.Now;
                        ltps10.UpdatedUserID = ecnUserID;
                        ltps10.BaseChannelID = ecnBaseChannelID;
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
                        ltps10.BaseChannelID = ecnBaseChannelID;
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
                    else if (ltps10 != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(ltps10.LTPID, ecnBaseChannelID);
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
                if (int.TryParse(imgbtn.Attributes["LTPID"].ToString(), out LTPID))
                {
                    DataTable ltpoList = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetDT_LTPID_BaseChannelID(LTPID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID);
                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltp = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(LTPID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID);
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
                ParamOptionsDT = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetDT_LTPID_BaseChannelID(LTPID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID);
            }
            LabelID = imgbtn.ID;
            lblEditError.Visible = false;
            modalPopupOmnitureConfig.Show();
        }

        private void loadGrid()
        {
            var result = (from src in ParamOptionsDT.AsEnumerable()
                          where src.Field<bool>(IsDeletedColumnName) == false && src.Field<bool>(IsDynamicColumnName) == false
                          select new
                          {
                              LTPOID = src.Field<string>(LtpoidFieldName),
                              DisplayName = src.Field<string>(DisplayNameFieldName),
                              Value = src.Field<string>(ValueColumnName),
                              IsDeleted = src.Field<bool>(IsDeletedColumnName),
                              IsDynamic = src.Field<bool>(IsDynamicColumnName)
                          }).ToList();

            var dynamicResults = (from src in ParamOptionsDT.AsEnumerable()
                                  where src.Field<bool>(IsDeletedColumnName) == false && src.Field<bool>(IsDynamicColumnName) == true
                                  select new
                                  {
                                      LTPOID = src.Field<string>(LtpoidFieldName),
                                      DisplayName = src.Field<string>(DisplayNameFieldName),
                                      Value = src.Field<string>(ValueColumnName),
                                      IsDeleted = src.Field<bool>(IsDeletedColumnName),
                                      IsDynamic = src.Field<bool>(IsDynamicColumnName)
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
                        dr[DisplayNameFieldName] = txtOmniParamName.Text;
                        dr[ValueColumnName] = txtOmniParamOption.Text;
                        dr[IsDeletedColumnName] = false;
                        dr[IsDynamicColumnName] = false;

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
                            List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> listCILT = ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking.GetByLinkTrackingParamOptionID(Convert.ToInt32(dr[LtpoidFieldName].ToString()));
                            if (listCILT.Count > 0)
                            {
                                lblEditError.Text = "Cannot delete, value used in blast";
                                lblEditError.Visible = true;
                            }
                            else
                            {
                                dr[IsDeletedColumnName] = true;
                            }
                        }
                        else
                        {
                            dr[IsDeletedColumnName] = true;
                        }
                    }
                }
                loadGrid();
            }
        }

        protected void btnOmniEditSave_Click(object sender, EventArgs e)
        {
            var baseChannelId = ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;
            var user = ECNSession.CurrentSession().CurrentUser;
            var ltps = UpdateAndReturnLinkTrackingParamSettings(baseChannelId, user);

            // POSSIBLE BUG: Next line will fail if ParamOptionsDT is null
            ModifyParamOptionsOnDynamicFields();

            if (ParamOptionsDT != null && ltps.LTPSID > 0)
            {
                ApplyParamOptions(baseChannelId, user);
            }

            UpdateUIOnOmniEditSave(ltps, baseChannelId);
        }

        private void UpdateUIOnOmniEditSave(LinkTrackingParamSettings ltps, int baseChannelId)
        {
            switch (LabelID)
            {
                case "imgbtnOmni1":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture1, ddlOmniDefault1);
                    break;
                case "imgbtnOmni2":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture2, ddlOmniDefault2);
                    break;
                case "imgbtnOmni3":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture3, ddlOmniDefault3);
                    break;
                case "imgbtnOmni4":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture4, ddlOmniDefault4);
                    break;
                case "imgbtnOmni5":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture5, ddlOmniDefault5);
                    break;
                case "imgbtnOmni6":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture6, ddlOmniDefault6);
                    break;
                case "imgbtnOmni7":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture7, ddlOmniDefault7);
                    break;
                case "imgbtnOmni8":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture8, ddlOmniDefault8);
                    break;
                case "imgbtnOmni9":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture9, ddlOmniDefault9);
                    break;
                case "imgbtnOmni10":
                    InitializeChannelSettings(ltps, baseChannelId, lblOmniture10, ddlOmniDefault10);
                    break;
            }

            modalPopupOmnitureConfig.Hide();
            txtOmniDisplayName.Text = string.Empty;
            ParamOptionsDT = new DataTable();
        }

        private void ApplyParamOptions(int baseChannelId, User user)
        {
            foreach (var dr in ParamOptionsDT.AsEnumerable())
            {
                var isDeleted = dr[IsDeletedColumnName].ToString();
                var isLtpoidComplex = dr[LtpoidFieldName].ToString().Contains("-");

                if (isLtpoidComplex && isDeleted.Equals(bool.FalseString))
                {
                    InsertLinkTrackingParamOption(baseChannelId, user, dr);
                }

                if (isDeleted.Equals(bool.TrueString) && !isLtpoidComplex)
                {
                    DeleteLinkTrackingParamOption(user, dr);
                }
            }
        }

        private void DeleteLinkTrackingParamOption(User user, DataRow dr)
        {
            int actualID;
            int.TryParse(dr[LtpoidFieldName].ToString(), out actualID);
            if (actualID <= 0)
            {
                return;
            }

            var ltpo = BusinessLinkTrackingParamOption.GetByLTPOID(actualID);
            ltpo.IsDeleted = true;
            ltpo.IsActive = false;
            ltpo.UpdatedDate = DateTime.Now;
            ltpo.UpdatedUserID = user.UserID;
            try
            {
                BusinessLinkTrackingParamOption.Delete(ltpo);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void InsertLinkTrackingParamOption(int baseChannelId, User user, DataRow row)
        {
            bool isDefault;
            bool.TryParse(row[IsDefaultColumnName].ToString(), out isDefault);

            var ltpo = new LinkTrackingParamOption
            {
                Value = row[ValueColumnName].ToString(),
                DisplayName = row[DisplayNameFieldName].ToString(),
                ColumnName = null,
                IsDefault = isDefault,
                IsActive = true,
                IsDeleted = false,
                IsDynamic = Convert.ToBoolean(row[IsDynamicColumnName].ToString()),
                BaseChannelID = baseChannelId,
                CreatedUserID = user.UserID,
                CreatedDate = DateTime.Now,
                LTPID = ParamID
            };
            try
            {
                BusinessLinkTrackingParamOption.Insert(ltpo);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void ModifyParamOptionsOnDynamicFields()
        {
            foreach (ListItem li in chklstDynamicFields.Items)
            {
                var hasDynamicOptionSelected =
                    ParamOptionsDT.Select($"{ValueColumnName} = '{li.Value}' and {IsDynamicColumnName} = true").FirstOrDefault();
                if (li.Selected && hasDynamicOptionSelected == null)
                {
                    var dt = ParamOptionsDT;
                    var dr = dt.NewRow();
                    dr[LtpoidFieldName] = Guid.NewGuid();
                    dr[DisplayNameFieldName] = li.Text;
                    dr[ValueColumnName] = li.Value;
                    dr[IsDeletedColumnName] = false;
                    dr[IsDynamicColumnName] = true;
                    dr[IsDefaultColumnName] = false;
                    dt.Rows.Add(dr);

                    ParamOptionsDT = dt;
                }
                else if (hasDynamicOptionSelected != null)
                {
                    var dRow = hasDynamicOptionSelected;
                    ParamOptionsDT.Rows[ParamOptionsDT.Rows.IndexOf(dRow)][IsDeletedColumnName] = true;
                }
            }
        }

        private LinkTrackingParamSettings UpdateAndReturnLinkTrackingParamSettings(int baseChannelId, User user)
        {
            var ltps = BusinessLinkTrackingParamSettings.Get_LTPID_BaseChannelID(ParamID, baseChannelId);
            if (ltps != null && ltps.LTPSID > 0)
            {
                ltps.UpdatedUserID = user.UserID;
                ltps.UpdatedDate = DateTime.Now;
                ltps.DisplayName = txtOmniDisplayName.Text.Trim();
                try
                {
                    BusinessLinkTrackingParamSettings.Update(ltps);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
            else
            {
                ltps = new LinkTrackingParamSettings
                {
                    CreatedUserID = user.UserID,
                    CreatedDate = DateTime.Now,
                    DisplayName = txtOmniDisplayName.Text.Trim(),
                    LTPID = ParamID,
                    BaseChannelID = baseChannelId
                };
                try
                {
                    ltps.LTPSID = BusinessLinkTrackingParamSettings.Insert(ltps);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }

            return ltps;
        }

        private void InitializeChannelSettings(LinkTrackingParamSettings ltps, int baseChannelId, ITextControl omnitureLabel, ListControl defaultDropDown)
        {
            omnitureLabel.Text = ltps.DisplayName;
            var lstLtpo = BusinessLinkTrackingParamOption.Get_LTPID_BaseChannelID(ParamID, baseChannelId);
            defaultDropDown.DataSource = lstLtpo;
            defaultDropDown.DataTextField = DisplayNameFieldName;
            defaultDropDown.DataValueField = LtpoidFieldName;
            defaultDropDown.DataBind();
            defaultDropDown.Items.Insert(0, new ListItem {Selected = true, Text = "-Select-", Value = "-1"});
            if (lstLtpo.Exists(x => x.IsDefault))
            {
                defaultDropDown.SelectedValue = lstLtpo.First(x => x.IsDefault).LTPOID.ToString();
            }
        }

        private void setECNError(ECNException ecnException)
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
    }
}