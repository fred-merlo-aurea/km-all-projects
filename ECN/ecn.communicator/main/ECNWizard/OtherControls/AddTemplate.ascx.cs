using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using ecn.communicator.main.ECNWizard.Group;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using BusinessApplication = ECN_Framework_BusinessLayer.Application;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using System.Text.RegularExpressions;

namespace ecn.communicator.main.ECNWizard.OtherControls
{
    public partial class AddTemplate : System.Web.UI.UserControl
    {
        private const string AllowCustomerOverrideKey = "AllowCustomerOverride";
        private const string DefaultOption = "0";
        private const string DisplayNameKey = "DisplayName";
        private const string LtpoidKey = "LTPOID";
        private const string OmnitureKey = "Omniture";
        private const string OmnitureProperty = "omniture";
        private const string OverrideKey = "Override";
        private const string SelectionOption = "-Select-";
        private const string SettingsPath = "/Settings";

        delegate void HidePopup();        
        public string SuppressOrSelect
        {
            get
            {
                if (ViewState["SuppressOrSelect" + this.ClientID] != null)
                    return ViewState["SuppressOrSelect" + this.ClientID].ToString();
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["SuppressOrSelect" + this.ClientID] = value;
            }
        }
        private static int _GroupID;
        public int GroupID
        {
            get
            {
                if (_GroupID != null)
                    return _GroupID;
                else
                    return -1;
            }
            set
            {
                _GroupID = value;
            }
        }
        private List<CampaignItemTemplateGroup> SelectedGroups
        {
            get
            {
                if(ViewState["SelectedGroups"] != null)
                    return (List<CampaignItemTemplateGroup>)ViewState["SelectedGroups"];
                else
                    return new List<CampaignItemTemplateGroup>();
            }
            set
            {
                ViewState["SelectedGroups"] = value;
            }
        }
        private List<CampaignItemTemplateSuppressionGroup> SelectedSuppressionGroups
        {
            get
            {
                if (ViewState["SelectedSuppressionGroups"] != null)
                    return (List<CampaignItemTemplateSuppressionGroup>)ViewState["SelectedSuppressionGroups"];
                else
                    return new List<CampaignItemTemplateSuppressionGroup>();
            }
            set
            {
                ViewState["SelectedSuppressionGroups"] = value;
            }
        }
        private List<CampaignItemTemplateOptoutGroup> OptoutGroups_DT
        {
            get
            {
                if (ViewState["OptoutGroups_DT"] != null)
                {
                    return (List<CampaignItemTemplateOptoutGroup>)ViewState["OptoutGroups_DT"];
                }
                else
                {
                    return new List<CampaignItemTemplateOptoutGroup>();
                }
            }
            set
            {
                ViewState["OptoutGroups_DT"] = value;
            }
        }
        private int getCampaignItemTemplateID()
        {
            if (Request.QueryString["CampaignItemTemplateID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["CampaignItemTemplateID"]);
            }
            else
                return -1;
        }

        private bool IsCustomerSetup
        {
            get
            {
                if (ViewState["IsCustomerSetup"] != null)
                    return (bool)ViewState["IsCustomerSetup"];
                else
                    return false;
            }
            set
            {
                ViewState["IsCustomerSetup"] = value;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                layoutExplorer.enableSelectMode();
            }
            phError.Visible = false;
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(SelectedGroups == null || (SelectedGroups != null && SelectedGroups.Count == 0))
            {
                gvSelectedGroups.Visible = false;
                lblSelectGroupName.Visible = true;
            }
            else
            {
                gvSelectedGroups.Visible = true;
                lblSelectGroupName.Visible = false;
            }

            if (SelectedSuppressionGroups == null || (SelectedSuppressionGroups != null && SelectedSuppressionGroups.Count == 0))
            {
                gvSuppressionGroup.Visible = false;
                lblSuppressionGroupName.Visible = true;
            }
            else
            {
                gvSuppressionGroup.Visible = true;
                lblSuppressionGroupName.Visible = false;
            }
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }
        private void loadCampaigns()
        {
            List<ECN_Framework_Entities.Communicator.Campaign> myCampaigns = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NonArchived(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
            myCampaigns = myCampaigns.OrderBy(o => o.CampaignName).ToList();
            drpdownCampaign.DataSource = myCampaigns;
            drpdownCampaign.DataTextField = "CampaignName";
            drpdownCampaign.DataValueField = "CampaignID";
            drpdownCampaign.DataBind();

            drpdownCampaign.Items.Insert(0, new ListItem() { Value = "", Text = "-- Select --", Selected = true });
        }
        public void LoadControl()
        {
            int currentClientiD = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID;
            if (KMPlatform.BusinessLogic.Client.HasServiceFeature(currentClientiD, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ReportingBlastFields))
            {
                loadBlastFieldsData();
                
            }
            else
            {
                pnlBlastFields.Visible = false;
            }
            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Omniture))
            {
                loadOmnitureFieldsData();
                pnlOmniture.Visible = true;
            }

            pnlSuppression.Visible = KMPlatform.BusinessLogic.Client.HasServiceFeature(currentClientiD, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastSuppression);

            loadCampaigns();
            if (getCampaignItemTemplateID() > 0)
            {
                loadData();
                lblCampaignItemTemplate.Text = "Edit Campaign Item Template";
            }
            upMain.Update();
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = ctrlgroupsLookup1.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        if (SelectedGroups == null)
                            SelectedGroups = new List<CampaignItemTemplateGroup>();
                        if(SelectedGroups.Count(x => x.GroupID == groupID) == 0)
                        {
                            if (SelectedSuppressionGroups != null && SelectedSuppressionGroups.Count(y => y.GroupID == groupID && y.IsDeleted == false)  == 0)
                            {
                                List<CampaignItemTemplateGroup> tempList = SelectedGroups;
                                CampaignItemTemplateGroup citg = new CampaignItemTemplateGroup();
                                citg.GroupID = groupID;
                                citg.IsDeleted = false;
                                citg.Filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
                                tempList.Add(citg);
                                SelectedGroups = tempList;
                                gvSelectedGroups.Visible = true;
                                gvSelectedGroups.DataSource = SelectedGroups;
                                gvSelectedGroups.DataBind();
                            }
                        }                        
                    }
                    else if (hfGroupSelectionMode.Value.Equals("OptOutGroup"))
                    {
                        if (OptoutGroups_DT == null)
                            OptoutGroups_DT = new List<CampaignItemTemplateOptoutGroup>();
                        if (OptoutGroups_DT.Count(x => x.GroupID == groupID) == 0)
                        {
                            List<CampaignItemTemplateOptoutGroup> tempList = OptoutGroups_DT;
                            CampaignItemTemplateOptoutGroup citg = new CampaignItemTemplateOptoutGroup();
                            citg.GroupID = groupID;
                            citg.IsDeleted = false;
                            tempList.Add(citg);
                            OptoutGroups_DT = tempList;
                            loadOptoutGroupsGrid();
                        }
                    }
                    else
                    {
                        if (SelectedSuppressionGroups == null)
                            SelectedSuppressionGroups = new List<CampaignItemTemplateSuppressionGroup>();
                        if (SelectedSuppressionGroups.Count(x => x.GroupID == groupID) == 0)
                        {
                            if (SelectedGroups != null && SelectedGroups.Count(y => y.GroupID == groupID && y.IsDeleted == false) == 0)
                            {
                                List<CampaignItemTemplateSuppressionGroup> tempList = SelectedSuppressionGroups;
                                CampaignItemTemplateSuppressionGroup citg = new CampaignItemTemplateSuppressionGroup();
                                citg.GroupID = groupID;
                                citg.IsDeleted = false;
                                citg.Filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
                                tempList.Add(citg);
                                SelectedSuppressionGroups = tempList;
                                gvSuppressionGroup.Visible = true;
                                gvSuppressionGroup.DataSource = SelectedSuppressionGroups;
                                gvSuppressionGroup.DataBind();
                            }
                        }
                    }
                    ctrlgroupsLookup1.Visible = false;
                }
                ECN_Framework_Entities.Communicator.Layout layout = (ECN_Framework_Entities.Communicator.Layout)sender;
                if (hfWhichLayout.Value.Equals("Trigger"))
                {
                    hfSelectedLayoutTrigger.Value = layout.LayoutID.ToString();
                    lblSelectedLayoutTrigger.Text = layout.LayoutName;
                    imgCleanSelectedLayout.Visible = true;
                    mpeLayoutExplorer.Hide();
                    upMain.Update();
                }
            }
            catch { }
            return true;
        }

        private void loadBlastFieldsData()
        {
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName1 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(1, currentUser);
            ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName2 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(2, currentUser);
            ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName3 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(3, currentUser);
            ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName4 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(4, currentUser);
            ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName5 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(5, currentUser);
            if (blastFieldsName1 != null)
                lblBlastField1.Text = blastFieldsName1.Name;
            if (blastFieldsName2 != null)
                lblBlastField2.Text = blastFieldsName2.Name;
            if (blastFieldsName3 != null)
                lblBlastField3.Text = blastFieldsName3.Name;
            if (blastFieldsName4 != null)
                lblBlastField4.Text = blastFieldsName4.Name;
            if (blastFieldsName5 != null)
                lblBlastField5.Text = blastFieldsName5.Name;
        }

        private void loadOmnitureFieldsData()
        {
            var linkTrackingList = BusinessCommunicator.LinkTracking.GetAll();
            var baseChannelId = BusinessApplication.ECNSession.CurrentSession()
                .CurrentBaseChannel
                .BaseChannelID;

            var ltsBase = BusinessCommunicator.LinkTrackingSettings.GetByBaseChannelID_LTID(
                baseChannelId,
                linkTrackingList.First(x => x.DisplayName == OmnitureKey).LTID);

            var allowCustOverride = true;
            var overrideBaseChannel = true;
            var hasBaseSetup = false;
            var hasCustSetup = false;

            if (ltsBase?.LTSID > 0)
            {
                CheckIfCustOverrideIsAllowed(ltsBase, ref allowCustOverride, ref hasBaseSetup);
            }

            if (allowCustOverride)
            {
                TryCustOverride(ref overrideBaseChannel, ref hasCustSetup);
            }

            var listParam = BusinessCommunicator.LinkTrackingParam.GetByLinkTrackingID(
                linkTrackingList.First(x => x.DisplayName == OmnitureKey).LTID);

            var isCustomerType = IsCustomerType(allowCustOverride, overrideBaseChannel, hasBaseSetup, hasCustSetup);

            MapProperties(lblOmniture1, ddlOmniture1, listParam, 1, isCustomerType);
            MapProperties(lblOmniture2, ddlOmniture2, listParam, 2, isCustomerType);
            MapProperties(lblOmniture3, ddlOmniture3, listParam, 3, isCustomerType);
            MapProperties(lblOmniture4, ddlOmniture4, listParam, 4, isCustomerType);
            MapProperties(lblOmniture5, ddlOmniture5, listParam, 5, isCustomerType);
            MapProperties(lblOmniture6, ddlOmniture6, listParam, 6, isCustomerType);
            MapProperties(lblOmniture7, ddlOmniture7, listParam, 7, isCustomerType);
            MapProperties(lblOmniture8, ddlOmniture8, listParam, 8, isCustomerType);
            MapProperties(lblOmniture9, ddlOmniture9, listParam, 9, isCustomerType);
            MapProperties(lblOmniture10, ddlOmniture10, listParam, 10, isCustomerType);
        }

        private void MapProperties(Label label, DropDownList element, IList<LinkTrackingParam> listParam, int position, bool? isCustomerType)
        {
            var baseChannelId = BusinessApplication.ECNSession.CurrentSession()
                .CurrentBaseChannel
                .BaseChannelID;

            label.Text = GetDisplayName(listParam, $"{OmnitureProperty}{position}", isCustomerType, baseChannelId);
            SetDdlOmnitureFields(element, listParam, $"{OmnitureProperty}{position}", baseChannelId, isCustomerType);
        }

        private void SetDdlOmnitureFields(DropDownList element, IList<LinkTrackingParam> listParam, string propertyName, int baseChannelId, bool? isCustomerType)
        {
            Guard.NotNull(element, nameof(element));
            Guard.NotNull(listParam, nameof(listParam));
            Guard.NotNullOrWhitespace(propertyName, nameof(propertyName));

            element.DataSource = GetDataSource(listParam, propertyName, isCustomerType, baseChannelId);
            element.DataTextField = DisplayNameKey;
            element.DataValueField = LtpoidKey;
            element.DataBind();
            element.Items.Insert(0, new ListItem { Text = SelectionOption, Value = DefaultOption, Selected = true });
        }

        private void CheckIfCustOverrideIsAllowed(LinkTrackingSettings ltsBase, ref bool allowCustOverride, ref bool hasBaseSetup)
        {
            Guard.NotNull(ltsBase, nameof(ltsBase));

            var doc = new XmlDocument();
            doc.LoadXml(ltsBase.XMLConfig);

            var rootNode = doc.SelectSingleNode($"{SettingsPath}");
            if (rootNode?.HasChildNodes == true)
            {
                bool.TryParse(rootNode[AllowCustomerOverrideKey]?.InnerText, out allowCustOverride);
                hasBaseSetup = true;
                IsCustomerSetup = false;
            }
        }

        private void TryCustOverride(ref bool overrideBaseChannel, ref bool hasCustSetup)
        {
            var ltsCustomer = BusinessCommunicator.LinkTrackingSettings.GetByCustomerID_LTID(
                BusinessApplication.ECNSession.CurrentSession().CurrentCustomer.CustomerID, 3);

            if (ltsCustomer?.LTSID > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(ltsCustomer.XMLConfig);

                var rootNode = doc.SelectSingleNode($"{SettingsPath}");
                if (rootNode?.HasChildNodes == true)
                {
                    bool.TryParse(rootNode[OverrideKey]?.InnerText, out overrideBaseChannel);
                    hasCustSetup = true;
                    IsCustomerSetup = overrideBaseChannel;
                }
            }
        }

        public string GetDisplayName(IList<LinkTrackingParam> listParams, string propertyName, bool? isCustomerType, int baseChannelId)
        {
            Guard.NotNull(listParams, nameof(listParams));
            Guard.NotNullOrWhitespace(propertyName, nameof(propertyName));

            if (isCustomerType == null)
            {
                return null;
            }

            return isCustomerType.Value
                ? GetLinkTrackingParamSettings(listParams, propertyName).DisplayName
                : GetLinkTrackingParamSettingsByBaseChannel(listParams, propertyName, baseChannelId).DisplayName;
        }

        public IList<LinkTrackingParamOption> GetDataSource(IList<LinkTrackingParam> listParams, string propertyName, bool? isCustomerType, int baseChannelId)
        {
            Guard.NotNull(listParams, nameof(listParams));
            Guard.NotNullOrWhitespace(propertyName, nameof(propertyName));

            if (isCustomerType == null)
            {
                return new List<LinkTrackingParamOption>();
            }

            return isCustomerType.Value
                ? GetListOfLinkTrackingParamOption(listParams, propertyName)
                : GetListOfLinkTrackingParamOptionByBaseChannel(listParams, propertyName, baseChannelId);
        }

        private static bool? IsCustomerType(bool allowCustOverride, bool overrideBaseChannel, bool hasBaseSetup, bool hasCustSetup)
        {
            if (allowCustOverride && overrideBaseChannel && hasCustSetup)
            {
                return true;
            }

            if ((!allowCustOverride || !overrideBaseChannel) && hasBaseSetup)
            {
                return false;
            }

            return null;
        }

        private LinkTrackingParamSettings GetLinkTrackingParamSettings(
            IList<LinkTrackingParam> listParams,
            string displayName)
        {
            Guard.NotNull(listParams, nameof(listParams));

            var customerId = BusinessApplication.ECNSession.CurrentSession().CurrentCustomer.CustomerID;

            return BusinessCommunicator.LinkTrackingParamSettings.Get_LTPID_CustomerID(
                listParams.First(x => x.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)).LTPID, customerId);
        }

        private IList<LinkTrackingParamOption> GetListOfLinkTrackingParamOption(
            IList<LinkTrackingParam> listParams,
            string displayName)
        {
            Guard.NotNull(listParams, nameof(listParams));

            var customerId = BusinessApplication.ECNSession.CurrentSession().CurrentCustomer.CustomerID;

            return BusinessCommunicator.LinkTrackingParamOption.Get_LTPID_CustomerID(
                listParams.First(x => x.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)).LTPID, customerId);
        }

        private LinkTrackingParamSettings GetLinkTrackingParamSettingsByBaseChannel(
            IList<LinkTrackingParam> listParams,
            string displayName,
            int baseChannelId)
        {
            Guard.NotNull(listParams, nameof(listParams));

            return BusinessCommunicator.LinkTrackingParamSettings.Get_LTPID_BaseChannelID(
                listParams.First(x => x.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)).LTPID, baseChannelId);
        }

        private IList<LinkTrackingParamOption> GetListOfLinkTrackingParamOptionByBaseChannel(
            IList<LinkTrackingParam> listParams,
            string displayName,
            int baseChannelId)
        {
            Guard.NotNull(listParams, nameof(listParams));

            return BusinessCommunicator.LinkTrackingParamOption.Get_LTPID_BaseChannelID(
                listParams.First(x => x.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)).LTPID, baseChannelId);
        }

        private void loadData()
        {
            var currentUser = BusinessApplication.ECNSession.CurrentSession().CurrentUser;
            var campaignItemTemplate = 
                BusinessCommunicator.CampaignItemTemplate.GetByCampaignItemTemplateID(
                    getCampaignItemTemplateID(), 
                    currentUser);
            SetBlastFieldControls(campaignItemTemplate);

            var listGroups = 
                BusinessCommunicator.CampaignItemTemplateGroup.GetByCampaignItemTemplateID(
                    getCampaignItemTemplateID());
            SetSelectedGroups(listGroups);

            if (campaignItemTemplate.LayoutID.HasValue) {
                var layout = BusinessCommunicator.Layout.GetByLayoutID_NoAccessCheck(
                    campaignItemTemplate.LayoutID.Value, 
                    false);
                hfSelectedLayoutTrigger.Value = layout.LayoutID.ToString();
                lblSelectedLayoutTrigger.Text = layout.LayoutName;
                imgCleanSelectedLayout.Visible = true;
            }

            if(campaignItemTemplate.CampaignID.HasValue)
            {
                var campaign = BusinessCommunicator.Campaign.GetByCampaignID_NoAccessCheck(
                    campaignItemTemplate.CampaignID.Value, 
                    false);
                if (campaign != null)
                {
                    drpdownCampaign.ClearSelection();
                    drpdownCampaign.Items.FindByText(campaign.CampaignName).Selected = true;
                }
            }

            SetOmnitures(campaignItemTemplate);

            txtFromEmail.Text = campaignItemTemplate.FromEmail;
            txtFromName.Text = campaignItemTemplate.FromName;
            txtReplyTo.Text = campaignItemTemplate.ReplyTo;
            txtSubject.Text = campaignItemTemplate.Subject;

            if (pnlSuppression.Visible)
            {
                loadSuppressionGroups();
            }

            if (campaignItemTemplate.OptOutMasterSuppression.Value)
            {
                chkOptOutMasterSuppression.Checked = true;
                chkOptOutSpecificGroup.Enabled = false;
                pnlOptOutSpecificGroups.Visible = false;
            }

            SetOptoutGroups(campaignItemTemplate);
            upMain.Update();
        }

        private void SetSelectedGroups(List<CampaignItemTemplateGroup> listGroups)
        {
            Guard.NotNull(listGroups, nameof(listGroups));

            if (listGroups.Count > 0)
            {
                SelectedGroups = listGroups;
                gvSelectedGroups.DataSource = SelectedGroups;
                gvSelectedGroups.DataBind();
            }
        }

        private void SetOptoutGroups(CampaignItemTemplate campaignItemTemplate)
        {
            Guard.NotNull(campaignItemTemplate, nameof(campaignItemTemplate));

            if (campaignItemTemplate.OptOutSpecificGroup.Value)
            {
                chkOptOutSpecificGroup.Checked = true;
                pnlOptOutSpecificGroups.Visible = true;
                chkOptOutMasterSuppression.Enabled = false;
                var listOptoutGroups =
                    BusinessCommunicator.CampaignItemTemplateOptoutGroup.GetByCampaignItemTemplateID(
                        getCampaignItemTemplateID());
                if (listOptoutGroups.Count > 0)
                {
                    OptoutGroups_DT = listOptoutGroups;
                    loadOptoutGroupsGrid();
                }
            }
        }

        private void SetBlastFieldControls(CampaignItemTemplate campaignItemTemplate)
        {
            Guard.NotNull(campaignItemTemplate, nameof(campaignItemTemplate));

            txtTemplateName.Text = campaignItemTemplate.TemplateName;
            txtBlastField1.Text = campaignItemTemplate.BlastField1;
            txtBlastField2.Text = campaignItemTemplate.BlastField2;
            txtBlastField3.Text = campaignItemTemplate.BlastField3;
            txtBlastField4.Text = campaignItemTemplate.BlastField4;
            txtBlastField5.Text = campaignItemTemplate.BlastField5;
        }

        private void SetOmnitures(CampaignItemTemplate campaignItemTemplate)
        {
            if (pnlOmniture.Visible)
            {
                SetOmniture(campaignItemTemplate.Omniture1, ddlOmniture1);
                SetOmniture(campaignItemTemplate.Omniture2, ddlOmniture2);
                SetOmniture(campaignItemTemplate.Omniture3, ddlOmniture3);
                SetOmniture(campaignItemTemplate.Omniture4, ddlOmniture4);
                SetOmniture(campaignItemTemplate.Omniture5, ddlOmniture5);
                SetOmniture(campaignItemTemplate.Omniture6, ddlOmniture6);
                SetOmniture(campaignItemTemplate.Omniture7, ddlOmniture7);
                SetOmniture(campaignItemTemplate.Omniture8, ddlOmniture8);
                SetOmniture(campaignItemTemplate.Omniture9, ddlOmniture9);
                SetOmniture(campaignItemTemplate.Omniture10, ddlOmniture10);
            }
        }

        private static void SetOmniture(string omniture, ListControl list)
        {
            Guard.NotNull(list, nameof(list));

            if (omniture != null && list.Items.FindByText(omniture) != null)
            {
                list.ClearSelection();
                list.Items.FindByText(omniture).Selected = true;
            }
        }

        public int Save()
        {

            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            ECN_Framework_Entities.Communicator.CampaignItemTemplate campaignItemTemplate;
            if (getCampaignItemTemplateID() > 0)
            {
                campaignItemTemplate = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(getCampaignItemTemplateID(), currentUser);
                campaignItemTemplate.UpdatedUserID = currentUser.UserID;
            }
            else
            {
                campaignItemTemplate = new ECN_Framework_Entities.Communicator.CampaignItemTemplate();
                campaignItemTemplate.CreatedUserID = currentUser.UserID;
            }
            campaignItemTemplate.CustomerID = currentUser.CustomerID;
            campaignItemTemplate.BlastField1 = txtBlastField1.Text == string.Empty ? null : txtBlastField1.Text;
            campaignItemTemplate.BlastField2 = txtBlastField2.Text == string.Empty ? null : txtBlastField2.Text;
            campaignItemTemplate.BlastField3 = txtBlastField3.Text == string.Empty ? null : txtBlastField3.Text;
            campaignItemTemplate.BlastField4 = txtBlastField4.Text == string.Empty ? null : txtBlastField4.Text;
            campaignItemTemplate.BlastField5 = txtBlastField5.Text == string.Empty ? null : txtBlastField5.Text;
            campaignItemTemplate.Omniture1 = ddlOmniture1.SelectedIndex > 0 ? ddlOmniture1.SelectedItem.Text : null;
            campaignItemTemplate.Omniture2 = ddlOmniture2.SelectedIndex > 0 ? ddlOmniture2.SelectedItem.Text : null;
            campaignItemTemplate.Omniture3 = ddlOmniture3.SelectedIndex > 0 ? ddlOmniture3.SelectedItem.Text : null;
            campaignItemTemplate.Omniture4 = ddlOmniture4.SelectedIndex > 0 ? ddlOmniture4.SelectedItem.Text : null;
            campaignItemTemplate.Omniture5 = ddlOmniture5.SelectedIndex > 0 ? ddlOmniture5.SelectedItem.Text : null;
            campaignItemTemplate.Omniture6 = ddlOmniture6.SelectedIndex > 0 ? ddlOmniture6.SelectedItem.Text : null;
            campaignItemTemplate.Omniture7 = ddlOmniture7.SelectedIndex > 0 ? ddlOmniture7.SelectedItem.Text : null;
            campaignItemTemplate.Omniture8 = ddlOmniture8.SelectedIndex > 0 ? ddlOmniture8.SelectedItem.Text : null;
            campaignItemTemplate.Omniture9 = ddlOmniture9.SelectedIndex > 0 ? ddlOmniture9.SelectedItem.Text : null;
            campaignItemTemplate.Omniture10 = ddlOmniture10.SelectedIndex > 0 ? ddlOmniture10.SelectedItem.Text : null;
            campaignItemTemplate.OmnitureCustomerSetup = IsCustomerSetup;
            campaignItemTemplate.FromName = txtFromName.Text;
            campaignItemTemplate.FromEmail = txtFromEmail.Text;
            campaignItemTemplate.ReplyTo = txtReplyTo.Text;
            campaignItemTemplate.Subject = txtSubject.Text;
            campaignItemTemplate.TemplateName = txtTemplateName.Text;

            if (drpdownCampaign.SelectedValue != null && drpdownCampaign.SelectedValue != string.Empty)
                campaignItemTemplate.CampaignID = Int32.Parse(drpdownCampaign.SelectedValue);
            else
                campaignItemTemplate.CampaignID = null;

            if (hfSelectedLayoutTrigger.Value == "")
                hfSelectedLayoutTrigger.Value = "0";
            int layoutID = Convert.ToInt32(hfSelectedLayoutTrigger.Value);
            if (layoutID > 0)
                campaignItemTemplate.LayoutID = layoutID;
            else
                campaignItemTemplate.LayoutID = null;
            campaignItemTemplate.OptOutMasterSuppression = chkOptOutMasterSuppression.Checked;
            campaignItemTemplate.OptOutSpecificGroup = chkOptOutSpecificGroup.Checked;
            //int groupID = Convert.ToInt32(hfSelectGroupID.Value);
            //if (groupID > 0)
            //    campaignItemTemplate.GroupID = groupID;
            //else
            //    campaignItemTemplate.GroupID = null;

            try
            {
                int CampaignItemTemplateID = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Save(campaignItemTemplate, currentUser);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateGroup.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, currentUser);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateSuppressionGroup.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, currentUser);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateFilter.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, currentUser);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateOptoutGroup.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, currentUser);

                if (SelectedGroups != null)
                {
                    foreach(CampaignItemTemplateGroup g in SelectedGroups)
                    {
                        g.IsDeleted = false;
                        g.CampaignItemTemplateID = CampaignItemTemplateID;
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateGroup.Save(g, currentUser);
                    }
                }

                if (SelectedSuppressionGroups != null)
                {
                    foreach (CampaignItemTemplateSuppressionGroup sg in SelectedSuppressionGroups)
                    {
                        sg.IsDeleted = false;
                        sg.CampaignItemTemplateID = CampaignItemTemplateID;
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateSuppressionGroup.Save(sg, currentUser);
                    }
                }

                if (OptoutGroups_DT != null && chkOptOutSpecificGroup.Checked)
                {
                    foreach (CampaignItemTemplateOptoutGroup oo in OptoutGroups_DT)
                    {
                        oo.IsDeleted = false;
                        oo.CampaignItemTemplateID = CampaignItemTemplateID;
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateOptoutGroup.Save(oo, currentUser);
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
                return -1;
            }
            return campaignItemTemplate.CampaignItemTemplateID;
        }

        internal void Reset()
        {
            txtBlastField1.Text = string.Empty;
            txtBlastField2.Text = string.Empty;
            txtBlastField3.Text = string.Empty;
            txtBlastField4.Text = string.Empty;
            txtBlastField5.Text = string.Empty;

            if (ddlOmniture1.Items.Count > 0)
                ddlOmniture1.SelectedIndex = 0;
            if (ddlOmniture2.Items.Count > 0)
                ddlOmniture2.SelectedIndex = 0;
            if (ddlOmniture3.Items.Count > 0)
                ddlOmniture3.SelectedIndex = 0;
            if (ddlOmniture4.Items.Count > 0)
                ddlOmniture4.SelectedIndex = 0;
            if (ddlOmniture5.Items.Count > 0)
                ddlOmniture5.SelectedIndex = 0;
            if (ddlOmniture6.Items.Count > 0)
                ddlOmniture6.SelectedIndex = 0;
            if (ddlOmniture7.Items.Count > 0)
                ddlOmniture7.SelectedIndex = 0;
            if (ddlOmniture8.Items.Count > 0)
                ddlOmniture8.SelectedIndex = 0;
            if (ddlOmniture9.Items.Count > 0)
                ddlOmniture9.SelectedIndex = 0;
            if (ddlOmniture10.Items.Count > 0)
                ddlOmniture10.SelectedIndex = 0;

            txtFromEmail.Text = string.Empty;
            txtFromName.Text = string.Empty;
            txtReplyTo.Text = string.Empty;
            txtSubject.Text = string.Empty;

            txtTemplateName.Text = string.Empty;

            lblSelectGroupName.Text = "-No Group Selected-";
            lblSelectGroupName.Visible = true;

            lblSuppressionGroupName.Text = "-No Group Selected-";
            lblSuppressionGroupName.Visible = true;

            SelectedGroups = null;
            SelectedSuppressionGroups = null;
            gvSelectedGroups.DataSource = null;
            gvSelectedGroups.DataBind();
            gvSuppressionGroup.DataSource = null;
            gvSuppressionGroup.DataBind();

        }

        protected void lnkSelectGroup_Click(object sender, EventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            ctrlgroupsLookup1.ShowArchiveFilter = false;
            ctrlgroupsLookup1.LoadControl();            
            ctrlgroupsLookup1.Visible = true;
        }

        protected void lnkSelectSuppressionGroup_Click(object sender, EventArgs e)
        {
            hfGroupSelectionMode.Value = "SuppressGroup";
            ctrlgroupsLookup1.LoadControl();
            ctrlgroupsLookup1.Visible = true;
        }        

        private void loadSuppressionGroups()
        {
            List<CampaignItemTemplateSuppressionGroup> CampaignItemTemplateSuppressionGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateSuppressionGroup.GetByCampaignItemTemplateID(getCampaignItemTemplateID(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            if (CampaignItemTemplateSuppressionGroupList.Count > 0)
            {
                SelectedSuppressionGroups = CampaignItemTemplateSuppressionGroupList;
                loadSuppressionGroupsGrid();
            }
        }

        private void loadSuppressionGroupsGrid()
        {
            if (SelectedSuppressionGroups != null && SelectedSuppressionGroups.Count > 0)
            {
                gvSuppressionGroup.Visible = true;
                lblSuppressionGroupName.Visible = false;
                gvSuppressionGroup.DataSource = SelectedSuppressionGroups;
                gvSuppressionGroup.DataBind();
            }
            else
            {
                gvSuppressionGroup.Visible = false;
                lblSuppressionGroupName.Visible = true;
            }
        }        

        protected void imgSelectLayoutTrigger_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            layoutExplorer.enableShowArchivedOnlyMode();
            hfWhichLayout.Value = "Trigger";
            mpeLayoutExplorer.Show();
        }

        protected void imgCleanSelectedLayout_Click(object sender, EventArgs e)
        {
            hfSelectedLayoutTrigger.Value = "";
            lblSelectedLayoutTrigger.Text = "-No Message Selected-";
            imgCleanSelectedLayout.Visible = false;
        }


        protected void btnCloseLayoutExplorer_Click(object sender, System.EventArgs e)
        {
            mpeLayoutExplorer.Hide();
        }

        protected void gvSelectedGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                CampaignItemTemplateGroup group = (CampaignItemTemplateGroup)e.Row.DataItem;
                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(group.GroupID);
                Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                lblGroupName.Text = g.GroupName;
                ImageButton imgbtnDeleteGroup = (ImageButton)e.Row.FindControl("imgbtnDeleteGroup");
                imgbtnDeleteGroup.CommandArgument = group.GroupID.ToString();

                GroupObject rowView = new GroupObject();
                rowView.GroupID = group.GroupID;
                rowView.GroupName = g.GroupName;
                rowView.filters = group.Filters;
                ecn.communicator.main.ECNWizard.Group.filtergrid filterGrid = (ecn.communicator.main.ECNWizard.Group.filtergrid)e.Row.FindControl("fgGroupFilterGrid");
                filterGrid.GroupID = group.GroupID;
                filterGrid.RowIndex = e.Row.RowIndex;
                filterGrid.SetFilters(rowView, e.Row.RowIndex, true);
            }
        }

        protected void gvSelectedGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName.Equals("deletegroup"))
            {
                int groupID = Convert.ToInt32(e.CommandArgument.ToString());
                SelectedGroups.RemoveAll(x => x.GroupID == groupID);
                gvSelectedGroups.DataSource = SelectedGroups;
                gvSelectedGroups.DataBind();
            }
            else if (e.CommandName.Equals("AddFilter"))
            {
                GridViewRow gvr = gvSelectedGroups.Rows[Convert.ToInt32(e.CommandArgument.ToString())];
                Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
                int GroupID = Convert.ToInt32(lblGroupID.Text);
                filtergrid fg = (filtergrid)gvr.FindControl("fgGroupFilterGrid");
                SuppressOrSelect = "select";
                ResetAddFilter("select", GroupID, true, true, "false");
            }
            else if (e.CommandName.ToLower().Equals("deletecustomfilter"))
            {
                string[] IDs = e.CommandArgument.ToString().Split('_');
                int filterID = Convert.ToInt32(IDs[0].ToString());
                int GroupID = Convert.ToInt32(IDs[1].ToString());
                List<CampaignItemTemplateGroup> listGroups = SelectedGroups;

                CampaignItemTemplateGroup currentGroup = listGroups.Find(x => x.GroupID == GroupID);
                int currentIndex = listGroups.IndexOf(currentGroup);
                listGroups.Remove(currentGroup);
                if (currentGroup != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = currentGroup.Filters.Find(x => x.FilterID == filterID);
                    if (cibf != null)
                    {
                        currentGroup.Filters.Remove(cibf);
                    }
                }
                listGroups.Insert(currentIndex, currentGroup);

                SelectedGroups = listGroups;
                gvSelectedGroups.DataSource = SelectedGroups;
                gvSelectedGroups.DataBind();
            }
        }

        protected void gvSupression_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CampaignItemTemplateSuppressionGroup group = (CampaignItemTemplateSuppressionGroup)e.Row.DataItem;                
                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(group.GroupID.Value);
                List<ECN_Framework_Entities.Communicator.Filter> filterList = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID_NoAccessCheck(group.GroupID.Value, true);
                Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                lblGroupName.Text = g.GroupName;
                ImageButton imgbtnDeleteGroup = (ImageButton)e.Row.FindControl("imgbtnDeleteGroup");
                imgbtnDeleteGroup.CommandArgument = group.GroupID.Value.ToString();

                GroupObject rowView = new GroupObject();
                rowView.GroupID = group.GroupID.Value;
                rowView.GroupName = g.GroupName;
                rowView.filters = group.Filters;
                ecn.communicator.main.ECNWizard.Group.filtergrid filterGrid = (ecn.communicator.main.ECNWizard.Group.filtergrid)e.Row.FindControl("fgSuppressionFilterGrid");
                filterGrid.GroupID = group.GroupID.Value;
                filterGrid.RowIndex = e.Row.RowIndex;
                filterGrid.SetFilters(rowView, e.Row.RowIndex, true);
            }
        }

        protected void gvSuppressionGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("deletegroup"))
            {
                int groupID = Convert.ToInt32(e.CommandArgument.ToString());
                SelectedSuppressionGroups.RemoveAll(x => x.GroupID == groupID);
                gvSuppressionGroup.DataSource = SelectedSuppressionGroups;
                gvSuppressionGroup.DataBind();
            }
            else if (e.CommandName.Equals("AddFilter"))
            {
                GridViewRow gvr = gvSuppressionGroup.Rows[Convert.ToInt32(e.CommandArgument.ToString())];
                Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
                int GroupID = Convert.ToInt32(lblGroupID.Text);
                filtergrid fg = (filtergrid)gvr.FindControl("fgSuppressionFilterGrid");
                SuppressOrSelect = "suppress";
                ResetAddFilter("suppress", GroupID, true, true, "false");
            }
            else if (e.CommandName.ToLower().Equals("deletecustomfilter"))
            {
                string[] IDs = e.CommandArgument.ToString().Split('_');
                int filterID = Convert.ToInt32(IDs[0].ToString());
                int GroupID = Convert.ToInt32(IDs[1].ToString());
                List<CampaignItemTemplateSuppressionGroup> listGroups = SelectedSuppressionGroups;

                CampaignItemTemplateSuppressionGroup currentGroup = listGroups.Find(x => x.GroupID == GroupID);
                int currentIndex = listGroups.IndexOf(currentGroup);
                listGroups.Remove(currentGroup);
                if (currentGroup != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = currentGroup.Filters.Find(x => x.FilterID == filterID);
                    if (cibf != null)
                    {
                        currentGroup.Filters.Remove(cibf);
                    }
                }
                listGroups.Insert(currentIndex, currentGroup);

                SelectedSuppressionGroups = listGroups;
                gvSuppressionGroup.DataSource = SelectedSuppressionGroups;
                gvSuppressionGroup.DataBind();
            }
        }

        #region OptOut Groups
        protected void chkOptOutMasterSuppression_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptOutMasterSuppression.Checked)
            {
                chkOptOutSpecificGroup.Enabled = false;
                pnlOptOutSpecificGroups.Visible = false;
            }
            else
            {
                chkOptOutSpecificGroup.Enabled = true;
            }
        }

        protected void chkOptOutSpecificGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptOutSpecificGroup.Checked)
            {
                pnlOptOutSpecificGroups.Visible = true;
                chkOptOutMasterSuppression.Enabled = false;
            }
            else
            {
                pnlOptOutSpecificGroups.Visible = false;
                chkOptOutMasterSuppression.Enabled = true;
            }
        }

        protected void lnkSelectOptOutGroups_Click(object sender, EventArgs e)
        {
            hfGroupSelectionMode.Value = "OptOutGroup";
            ctrlgroupsLookup1.ShowArchiveFilter = false;
            ctrlgroupsLookup1.LoadControl();
            ctrlgroupsLookup1.Visible = true;
            upMain.Update();
        }

        protected void gvOptOutGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CampaignItemTemplateOptoutGroup group = (CampaignItemTemplateOptoutGroup)e.Row.DataItem;
                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(group.GroupID);
                Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                lblGroupName.Text = g.GroupName;
                ImageButton imgbtnDeleteGroup = (ImageButton)e.Row.FindControl("imgbtnDeleteGroup");
                imgbtnDeleteGroup.CommandArgument = group.GroupID.ToString();

                GroupObject rowView = new GroupObject();
                rowView.GroupID = group.GroupID;
                rowView.GroupName = g.GroupName;
            }
        }


        protected void gvOptOutGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("deletegroup"))
            {
                int groupID = Convert.ToInt32(e.CommandArgument.ToString());
                OptoutGroups_DT.RemoveAll(x => x.GroupID == groupID);
                loadOptoutGroupsGrid();
            }
        }

        private void loadOptoutGroupsGrid()
        {
            if (OptoutGroups_DT != null)
            {
                gvOptOutGroups.Visible = true;
                gvOptOutGroups.DataSource = OptoutGroups_DT;
                gvOptOutGroups.DataBind();
            }
            else
            {
                gvOptOutGroups.Visible = false;
            }
            pnlOptOutSpecificGroups.Update();
        }
        #endregion

        #region Filters
        protected void btnSaveFilter_Click(object sender, EventArgs e)
        {
            if (SuppressOrSelect.ToLower().Equals("select"))
            {
                List<CampaignItemTemplateGroup> citg_list = SelectedGroups;
                CampaignItemTemplateGroup current = citg_list.Find(x => x.GroupID == GroupID);
                if (current != null)
                {
                    int currentIndex = citg_list.IndexOf(current);

                    citg_list.Remove(current);
                    foreach (ListItem li in lbAvailableFilters.Items)
                    {
                        if (li.Selected)
                        {
                            if (current.Filters.Count(x => x.FilterID == Convert.ToInt32(li.Value.ToString())) == 0)
                            {
                                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                                cibf.FilterID = Convert.ToInt32(li.Value.ToString());
                                cibf.IsDeleted = false;

                                current.Filters.Add(cibf);
                            }
                        }
                    }

                    citg_list.Insert(currentIndex, current);

                    if (ViewState["SelectedGroups"] != null)
                        ViewState["SelectedGroups"] = citg_list;
                    else
                        ViewState.Add("SelectedGroups", citg_list);

                    gvSelectedGroups.DataSource = citg_list;
                    gvSelectedGroups.DataBind();
                }
            }
            else if (SuppressOrSelect.ToLower().Equals("suppress"))
            {
                List<CampaignItemTemplateSuppressionGroup> citsg_list = SelectedSuppressionGroups;
                CampaignItemTemplateSuppressionGroup current = citsg_list.Find(x => x.GroupID == GroupID);
                if (current != null)
                {
                    int currentIndex = citsg_list.IndexOf(current);

                    citsg_list.Remove(current);
                    foreach (ListItem li in lbAvailableFilters.Items)
                    {
                        if (li.Selected)
                        {
                            if (current.Filters.Count(x => x.FilterID == Convert.ToInt32(li.Value.ToString())) == 0)
                            {
                                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                                cibf.FilterID = Convert.ToInt32(li.Value.ToString());
                                cibf.IsDeleted = false;

                                current.Filters.Add(cibf);
                            }
                        }
                    }

                    citsg_list.Insert(currentIndex, current);

                    if (ViewState["SelectedSuppressionGroups"] != null)
                        ViewState["SelectedSuppressionGroups"] = citsg_list;
                    else
                        ViewState.Add("SelectedSuppressionGroups", citsg_list);

                    gvSuppressionGroup.DataSource = citsg_list;
                    gvSuppressionGroup.DataBind();
                }
            }
            mpeAddFilter.Hide();
            upMain.Update();
        }
        protected void btnCancelFilter_Click(object sender, EventArgs e)
        {
            mpeAddFilter.Hide();
        }

        private void ResetAddFilter(string suppressorselect, int groupID, bool showSelect, bool custom = false, string istestblast = "false")
        {
            pnlCustomFilter.Visible = true;
            
            SuppressOrSelect = suppressorselect;

            GroupID = groupID;
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> citg_list = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup>();

            if (suppressorselect.Equals("select"))
            {
                citg_list = (List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup>)ViewState["SelectedGroups"];
            }
            else if (suppressorselect.Equals("suppress"))
            {
                citg_list = (List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup>)ViewState["SupressionGroups_List"];
            }

            List<ECN_Framework_Entities.Communicator.Filter> filterList = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID_NoAccessCheck(groupID, true, "active");
            lbAvailableFilters.DataSource = filterList;
            lbAvailableFilters.DataTextField = "FilterName";
            lbAvailableFilters.DataValueField = "FilterID";
            lbAvailableFilters.DataBind();

            pnlFilterConfig.Update();
            if (showSelect)
                mpeAddFilter.Show();
        }
        #endregion
    }
}