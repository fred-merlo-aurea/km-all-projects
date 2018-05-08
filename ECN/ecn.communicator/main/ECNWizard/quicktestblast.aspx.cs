using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using EntityCampaignItem = ECN_Framework_Entities.Communicator.CampaignItem;
using EntityCampaignItemTestBlast = ECN_Framework_Entities.Communicator.CampaignItemTestBlast;
using FrameworkGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using KMEnums = KMPlatform.Enums;
using KMUser = KM.Platform.User;

namespace ecn.communicator.main.ECNWizard
{
    public partial class quicktestblast : ECN_Framework.WebPageHelper
    {
        private const string BlastEnvelopeId = "BlastEnvelopeID";
        private const string FromEmail = "FromEmail";
        private const string FromName = "FromName";
        private const string SelectFromEmail = "Select From Email";
        private const string SelectReplyToEmail = "Select Reply To Email";
        private const string SelectFromName = "Select From Name";
        private const string Text = "TEXT";
        private const string Campaignname = "CampaignName";
        private const string CampaignId = "CampaignID";
        private const string SelectCampaign = "Select Campaign";
        private const string UnselectedValue = "-1";
        private const string Colon = ":";
        private const string ColonSpaceAround = " : ";
        private const string IncorrectSnippet = "Incorrectly formed code snippet";
        private const string DoublePercent = "%%";
        private const string RegexPattern1 = "%%[a-zA-Z0-9_]+?%%";
        private const string RegexPattern2 = "%%.+?%%";

        private ECN_Framework_Entities.Communicator.QuickTestBlastConfig QTB
        {
            get
            {
                if (ViewState["QTB"] != null)
                    return (ECN_Framework_Entities.Communicator.QuickTestBlastConfig)ViewState["QTB"];
                else
                    return new ECN_Framework_Entities.Communicator.QuickTestBlastConfig();
            }
            set { ViewState["QTB"] = value; }
        }

        private int GetCampaignItemID()

        {
            int campID = -1;
            if (Request.QueryString["campaignitemid"] != null)
            {
                campID = Convert.ToInt32(Request.QueryString["campaignitemid"].ToString());
            }
            return campID;
        }

        delegate void HidePopup();

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
            DoTwemoji();
        }

        private void throwECNException(Enums.Entity entity, string message)
        {
            ECNError ecnError = new ECNError(entity, Enums.Method.None, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        KMPlatform.Entity.User SessionCurrentUser { get { return Master.UserSession.CurrentUser; } }

        private string BlastFromName
        {
            get
            {
                if (drpEmailFromName.Visible)
                    return drpEmailFromName.SelectedItem.Text;
                else
                    return txtEmailFromName.Text.Trim();
            }
            set
            {
                if (drpEmailFromName.Visible)
                {
                    drpEmailFromName.ClearSelection();
                    try
                    {
                        drpEmailFromName.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }
                    txtEmailFromName.Text = value.ToString().Trim();

                }
                else
                {
                    txtEmailFromName.Text = value.ToString().Trim();
                }
            }
        }

        private string BlastFromEmail
        {
            get
            {
                if (drpEmailFromName.Visible)
                    return drpEmailFrom.SelectedItem.Text;
                else
                    return txtEmailFrom.Text;
            }
            set
            {
                if (drpEmailFromName.Visible)
                {
                    drpEmailFrom.ClearSelection();
                    try
                    {
                        drpEmailFrom.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }

                    txtEmailFrom.Text = value.ToString().Trim();
                }
                else
                {
                    txtEmailFrom.Text = value.ToString().Trim();
                }
            }
        }

        private string BlastReplyToEmail
        {
            get
            {
                if (drpEmailFromName.Visible)
                    return drpReplyTo.SelectedItem.Text;
                else
                    return txtReplyTo.Text;
            }
            set
            {
                if (drpEmailFromName.Visible)
                {
                    drpReplyTo.ClearSelection();
                    try
                    {
                        drpReplyTo.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }
                    txtReplyTo.Text = value.Trim();
                }
                else
                {
                    txtReplyTo.Text = value.Trim();
                }
            }
        }

        protected void btnChangeEnvelope_onclick(object sender, System.EventArgs e)
        {
            if (drpEmailFrom.Visible)
                ToggleEnvelopePanel(false);
            else
                ToggleEnvelopePanel(true);
            DoTwemoji();
        }

        private void ToggleEnvelopePanel(bool IsPredefinedEnvelopeInfo)
        {
            if (IsPredefinedEnvelopeInfo)
            {
                //txtEmailFrom.Attributes.Add("style", "display:none");
                //txtReplyTo.Attributes.Add("style", "display:none");
                //txtEmailFromName.Attributes.Add("style", "display:none");

                txtEmailFrom.Visible = false;
                txtReplyTo.Visible = false;
                txtEmailFromName.Visible = false;

                drpEmailFrom.Visible = true;
                drpReplyTo.Visible = true;
                drpEmailFromName.Visible = true;

                val_txtEmailFrom.ControlToValidate = "drpEmailFrom";
                val_txtReplyTo.ControlToValidate = "drpReplyTo";
                val_txtEmailFromName.ControlToValidate = "drpEmailFromName";
            }
            else
            {
                txtEmailFrom.Visible = true;
                txtReplyTo.Visible = true;
                txtEmailFromName.Visible = true;

                drpEmailFrom.Visible = false;
                drpReplyTo.Visible = false;
                drpEmailFromName.Visible = false;

                val_txtEmailFrom.ControlToValidate = "txtEmailFrom";
                val_txtReplyTo.ControlToValidate = "txtReplyTo";
                val_txtEmailFromName.ControlToValidate = "txtEmailFromName";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(SessionCurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                if (!Page.IsPostBack)
                {
                    if (!KMPlatform.BusinessLogic.Client.HasServiceFeature(Master.UserSession.CurrentCustomer.PlatformClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreview))
                    {
                        trEmailPreview.Visible = false;
                    }

                    layoutExplorer.enableSelectMode();
                    loadData(GetCampaignItemID());
                    txtCampaignNameTr.Visible = false;
                    
                }
                HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
                this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Redit2", "doMouseOver();", true);
        }

        protected void CampaignChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCampaignChoice2.Checked)
            {
                ddlCampaignsTr.Visible = true;
                txtCampaignNameTr.Visible = false;
            }
            else
            {
                ddlCampaignsTr.Visible = false;
                txtCampaignNameTr.Visible = true;
            }
            DoTwemoji();
        }

        protected void GroupChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGroupChoice2.Checked)
            {
                AdhocEmailsTr.Visible = true;
                SelectGroupTr.Visible = false;
                taAddresses.Text = "";
            }
            else
            {
                AdhocEmailsTr.Visible = false;
                SelectGroupTr.Visible = true;
                taAddresses.Text = "";
            }
            DoTwemoji();
        }

        private void loadData(int campaignItemID = -1)
        {
            SetQuickTestBlastConfig();
            RetrieveAndBindEnvelopeList();
            UpdateCampaigns(campaignItemID);
            ApplyAccessRightsToControls();
            SetVisibilityForCreateGroup();
        }

        private void SetQuickTestBlastConfig()
        {
            var baseChannelId = Master.UserSession.CurrentCustomer.BaseChannelID;
            if (baseChannelId.HasValue)
            {
                QTB = QuickTestBlastConfig.GetByBaseChannelID(baseChannelId.Value);
            }

            if (QTB?.CustomerCanOverride.Value==true)
            {
                var customQuickTestBlast = QuickTestBlastConfig.GetByCustomerID(SessionCurrentUser.CustomerID);

                if (customQuickTestBlast?.CustomerDoesOverride == true)
                {
                    QTB = customQuickTestBlast;
                    QTB.BaseChannelDoesOverride = true;
                }
            }

            if (QTB == null || !QTB.BaseChannelDoesOverride.HasValue || !QTB.BaseChannelDoesOverride.Value)
            {
                QTB = QuickTestBlastConfig.GetKMDefaultConfig();
            }
        }

        private void RetrieveAndBindEnvelopeList()
        {
            var blastEnvelopeList =
                BlastEnvelope.GetByCustomerID_NoAccessCheck(ECNSession.CurrentSession().CurrentUser.CustomerID);

            if (blastEnvelopeList.Count > 0)
            {
                btnChangeEnvelope.Visible = true;
                ToggleEnvelopePanel(true);
                drpEmailFrom.DataSource = blastEnvelopeList;
                drpEmailFrom.DataValueField = BlastEnvelopeId;
                drpEmailFrom.DataTextField = FromEmail;
                drpEmailFrom.DataBind();
                drpEmailFrom.Items.Insert(0, new ListItem(SelectFromEmail, string.Empty));
                drpReplyTo.DataSource = blastEnvelopeList;
                drpReplyTo.DataValueField = BlastEnvelopeId;
                drpReplyTo.DataTextField = FromEmail;
                drpReplyTo.DataBind();
                drpReplyTo.Items.Insert(0, new ListItem(SelectReplyToEmail, string.Empty));
                drpEmailFromName.DataSource = blastEnvelopeList;
                drpEmailFromName.DataValueField = BlastEnvelopeId;
                drpEmailFromName.DataTextField = FromName;
                drpEmailFromName.DataBind();
                drpEmailFromName.Items.Insert(0, new ListItem(SelectFromName, string.Empty));
            }
            else
            {
                btnChangeEnvelope.Visible = false;
            }
        }

        private void ApplyAccessRightsToControls()
        {
            if (QTB?.AllowAdhocEmails==false || UserHasAccessEditGroups())
            {
                rbGroupChoice1.Checked = true;
                rbGroupChoice2.Checked = false;
                rbGroupChoice2.Visible = false;
                SelectGroupTr.Visible = true;
                AdhocEmailsTr.Visible = false;
            }
        }

        private bool UserHasAccessEditGroups()
        {
            return !KMUser.HasAccess(
                SessionCurrentUser,
                KMEnums.Services.EMAILMARKETING,
                KMEnums.ServiceFeatures.Groups,
                KMEnums.Access.Edit);
        }

        private void SetVisibilityForCreateGroup()
        {
            if (QTB.AutoCreateGroup == true)
            {
                tbGroupName.Visible = false;
                tbGroupNameLabel.Visible = false;
            }
        }

        private void UpdateCampaigns(int campaignItemId)
        {
            if (campaignItemId > 0)
            {
                rbCampaignChoiceTr.Visible = false;
                ddlCampaignsTr.Visible = false;
                txtCampaignNameTr.Visible = false;
                txtCampaignItemNameTr.Visible = false;

                var campaignItem = CampaignItem.GetByCampaignItemID(campaignItemId, SessionCurrentUser, true);
                SetCampaignNames(campaignItem);

                var campaignItemTestBlastList =
                    CampaignItemTestBlast.GetByCampaignItemID(campaignItem.CampaignItemID, SessionCurrentUser, true);

                if (campaignItemTestBlastList.Count > 0)
                {
                    var campaignItemTestBlast =
                        campaignItemTestBlastList.OrderByDescending(u => u.CampaignItemTestBlastID).FirstOrDefault();
                    UpdateCampaignItemTestBlast(campaignItemTestBlast, campaignItem);
                }
                else
                {
                    rbGroupChoice1.Checked = false;
                    rbGroupChoice2.Checked = true;
                    SelectGroupTr.Visible = false;
                    AdhocEmailsTr.Visible = true;
                    rbTextVersionNo.Checked = true;
                }
            }
            else
            {
                UpdateLayoutWhenNoSelectedCampaign();
            }
        }

        private void UpdateCampaignItemTestBlast(EntityCampaignItemTestBlast campaignItemTestBlast, EntityCampaignItem campaignItem)
        {
            rbGroupChoice1.Checked = true;
            rbGroupChoice2.Checked = false;
            SelectGroupTr.Visible = true;
            AdhocEmailsTr.Visible = false;

            if (campaignItemTestBlast != null)
            {
                if (campaignItemTestBlast.GroupID != null)
                {
                    var campaignGroup =
                        FrameworkGroup.GetByGroupID_NoAccessCheck(campaignItemTestBlast.GroupID.Value);
                    lblSelectGroupName.Text = campaignGroup.GroupName;
                    hfSelectGroupID.Value = campaignGroup.GroupID.ToString();
                }

                hfSelectedLayoutTrigger.Value = campaignItemTestBlast.Blast.Layout.LayoutID.ToString();
                lblSelectedLayoutTrigger.Text = campaignItemTestBlast.Blast.Layout.LayoutName;


                if (campaignItemTestBlast.CampaignItemTestBlastID > 0)
                {
                    SetEmailAttributes(campaignItemTestBlast);
                }
                else
                {
                    SetEmailAttributes(campaignItem, campaignItemTestBlast);
                }
            }

            ToggleEnvelopePanel(false);

            if (campaignItem.EnableCacheBuster.HasValue)
            {
                chbEnableCacheBuster.Checked = campaignItem.EnableCacheBuster.Value;
            }

            if (campaignItemTestBlast?.Blast.HasEmailPreview != null && trEmailPreview.Visible)
            {
                chbEmailPreview.Checked = campaignItemTestBlast.Blast.HasEmailPreview.Value;
            }

            if (campaignItemTestBlast != null && campaignItemTestBlast.CampaignItemTestBlastType == Text)
            {
                rbTextVersionYes.Checked = true;
            }
            else
            {
                rbTextVersionNo.Checked = true;
            }
        }

        private void SetCampaignNames(EntityCampaignItem campaignItem)
        {
            if (campaignItem.CampaignID != null)
            {
                var campaign = Campaign.GetByCampaignID(campaignItem.CampaignID.Value, SessionCurrentUser, true);
                lblCampaignName.Text = campaign.CampaignName;
            }

            lblCampaignItemName.Text = campaignItem.CampaignItemName;
        }

        private void SetEmailAttributes(EntityCampaignItem campaignItem, EntityCampaignItemTestBlast campaignItemTestBlast)
        {
            txtEmailFrom.Text = 
                !string.IsNullOrWhiteSpace(campaignItem.FromEmail) 
                    ? campaignItem.FromEmail 
                    : string.Empty;

            txtEmailFromName.Text = 
                !string.IsNullOrWhiteSpace(campaignItem.FromName) 
                    ? campaignItem.FromName 
                    : string.Empty;

            txtReplyTo.Text = 
                !string.IsNullOrWhiteSpace(campaignItem.ReplyTo) 
                    ? campaignItem.ReplyTo 
                    : string.Empty;

            txtEmailSubject.Value = 
                campaignItem.BlastList != null 
                && campaignItem.BlastList.Count > 0
                ? campaignItem.BlastList[0].EmailSubject
                : campaignItemTestBlast.Blast.EmailSubject.Replace(Colon, ColonSpaceAround);
        }

        private void SetEmailAttributes(EntityCampaignItemTestBlast campaignItemTestBlast)
        {
            txtEmailFrom.Text = 
                !string.IsNullOrWhiteSpace(campaignItemTestBlast.FromEmail)
                ? campaignItemTestBlast.FromEmail
                : string.Empty;

            txtEmailFromName.Text =
                !string.IsNullOrWhiteSpace(campaignItemTestBlast.FromName) 
                    ? campaignItemTestBlast.FromName 
                    : string.Empty;

            txtReplyTo.Text = 
                !string.IsNullOrWhiteSpace(campaignItemTestBlast.ReplyTo) 
                    ? campaignItemTestBlast.ReplyTo 
                    : string.Empty;

            txtEmailSubject.Value = 
                !string.IsNullOrWhiteSpace(campaignItemTestBlast.EmailSubject)
                ? campaignItemTestBlast.EmailSubject
                : string.Empty;
        }

        private void UpdateLayoutWhenNoSelectedCampaign()
        {
            lblCampaignNameTr.Visible = false;
            lblCampaignItemNameTr.Visible = false;
            var campaigns =Campaign.GetByCustomerID(
                Master.UserSession.CurrentCustomer.CustomerID, 
                SessionCurrentUser, false);
            campaigns = campaigns.OrderBy(c => c.CampaignName).ToList();
            rbGroupChoice1.Checked = false;
            rbGroupChoice2.Checked = true;
            SelectGroupTr.Visible = false;
            AdhocEmailsTr.Visible = true;
            rbTextVersionNo.Checked = true;
            chbEnableCacheBuster.Checked = true;
            ddlCampaigns.DataSource = campaigns;
            ddlCampaigns.DataTextField = Campaignname;
            ddlCampaigns.DataValueField = CampaignId;
            ddlCampaigns.DataBind();
            ddlCampaigns.Items.Insert(0, new ListItem() {Value = UnselectedValue, Text = SelectCampaign, Selected = true});
        }
        
        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            ctrlgroupsLookup1.LoadControl(true);
            ctrlgroupsLookup1.Visible = true;
            DoTwemoji();
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
                        lblSelectGroupName.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                    }
                    else
                    {
                        //noop
                    }
                    ctrlgroupsLookup1.Visible = false;
                }
                ECN_Framework_Entities.Communicator.Layout layout = (ECN_Framework_Entities.Communicator.Layout)sender;
                if (hfWhichLayout.Value.Equals("Trigger"))
                {
                    hfSelectedLayoutTrigger.Value = layout.LayoutID.ToString();
                    lblSelectedLayoutTrigger.Text = layout.LayoutName;

                    ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetTopOneByLayoutID_NoAccessCheck(layout.LayoutID, false);
                    if (blast != null)
                    {
                        txtEmailFrom.Text = blast.EmailFrom;
                        txtEmailFromName.Text = blast.EmailFromName;
                        txtReplyTo.Text = blast.ReplyTo;
                        txtEmailSubject.Value = blast.EmailSubject;
                    }
                    //else
                    //{
                    //    txtEmailFrom.Text = "";
                    //    txtEmailFromName.Text = "";
                    //    txtReplyTo.Text = "";
                    //    txtEmailSubject.Value = "";
                    //}
                    ToggleEnvelopePanel(false);
                }

                DoTwemoji();
            }
            catch
            {
                //noop
            }
            return true;
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        protected void imgSelectLayoutTrigger_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            hfWhichLayout.Value = "Trigger";
            mpeLayoutExplorer.Show();
            DoTwemoji();
        }

        private void DoTwemoji()
        {
            update1.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "pageloaded();", true);
        }

        public void drpEmailFrom_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            drpEmailFromName.ClearSelection();
            drpReplyTo.ClearSelection();
            if (drpEmailFrom.SelectedIndex > -1)
            {
                drpEmailFromName.Items.FindByValue(drpEmailFrom.SelectedValue).Selected = true;
                drpReplyTo.Items.FindByValue(drpEmailFrom.SelectedValue).Selected = true;
            }

            BlastFromName = drpEmailFromName.SelectedItem.Text;
            BlastFromEmail = drpEmailFrom.SelectedItem.Text;
            BlastReplyToEmail = drpReplyTo.SelectedItem.Text;
            DoTwemoji();

        }

        public void drpReplyTo_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            drpEmailFromName.ClearSelection();
            drpEmailFrom.ClearSelection();

            if (drpReplyTo.SelectedIndex > -1)
            {
                drpEmailFromName.Items.FindByValue(drpReplyTo.SelectedValue).Selected = true;
                drpEmailFrom.Items.FindByValue(drpReplyTo.SelectedValue).Selected = true;
            }

            BlastFromName = drpEmailFromName.SelectedItem.Text;
            BlastFromEmail = drpEmailFrom.SelectedItem.Text;
            BlastReplyToEmail = drpReplyTo.SelectedItem.Text;
            DoTwemoji();
        }

        public void drpEmailFromName_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            drpEmailFrom.ClearSelection();
            drpReplyTo.ClearSelection();
            if (drpEmailFromName.SelectedIndex > -1)
            {
                drpReplyTo.Items.FindByValue(drpEmailFromName.SelectedValue).Selected = true;
                drpEmailFrom.Items.FindByValue(drpEmailFromName.SelectedValue).Selected = true;
            }

            BlastFromName = drpEmailFromName.SelectedItem.Text;
            BlastFromEmail = drpEmailFrom.SelectedItem.Text;
            BlastReplyToEmail = drpReplyTo.SelectedItem.Text;
            DoTwemoji();
        }

        protected void btnCloseLayoutExplorer_Click(object sender, System.EventArgs e)
        {
            mpeLayoutExplorer.Hide();

        }

        private string CleanAndStripHTML(string dirty)
        {
            string retString = "";
            Regex htmlStrip = new Regex("<.*?>");
            retString = htmlStrip.Replace(dirty, "");
            retString = retString.Replace("&gt;", ">");
            retString = retString.Replace("&lt;", "<");

            return retString;
        }

        private IList<string> GetShortNamesForLayoutTemplate(IList<string> list, int groupId)
        {
            var Entity = Enums.Entity.Group;
            var Method = Enums.Method.Validate;
            var errorList = new List<ECNError>();
            var subList = new List<string>();

            var listCS = GetList(groupId);

            foreach (var listValue in list)
            {
                #region Badly Formed Snippets
                //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
                var regMatch = new Regex(DoublePercent, RegexOptions.IgnoreCase);
                var matchList = regMatch.Matches(listValue);

                if (matchList.Count > 0)
                {
                    if ((matchList.Count % 2) != 0)
                    {
                        //return error
                        errorList.Add(new ECNError(Entity, Method, IncorrectSnippet));
                    }
                    else
                    {
                        var regMatchGood = new Regex(RegexPattern1, RegexOptions.IgnoreCase);
                        var matchListGood = regMatchGood.Matches(listValue);
                        if ((matchList.Count / 2) > matchListGood.Count)
                        {
                            //return error
                            errorList.Add(new ECNError(Entity, Method, IncorrectSnippet));
                        }
                    }
                }
                #endregion

                //%% and ##
                regMatch = new Regex(RegexPattern2, RegexOptions.IgnoreCase);
                matchList = regMatch.Matches(listValue);

                foreach (Match match in matchList)
                {
                    if (!string.IsNullOrEmpty(match.Value.ToString()))
                    {
                        if (!subList.Contains(match.Value.ToString().ToLower().Replace(DoublePercent, string.Empty)))
                        {
                            subList.Add(match.Value.ToString().ToLower().Replace(DoublePercent, string.Empty));
                        }
                    }
                }
            }

            return RemoveList(listCS, subList);
        }

        private IList<string> GetList(int groupId)
        {
            var list = new List<string>();
            var emailDataTable = Email.GetColumnNames();

            foreach (DataRow dataRow in emailDataTable.Rows)
            {
                list.Add(dataRow["columnName"].ToString().ToLower());
            }

            var groupDataFields = GroupDataFields.GetByGroupID_NoAccessCheck(groupId);
            foreach (var groupDataField in groupDataFields)
            {
                list.Add(groupDataField.ShortName.ToLower());
            }

            list.Add("emailaddress");
            list.Add("formattypecode");
            list.Add("subscribetypecode");
            list.Add("title");
            list.Add("firstname");
            list.Add("lastname");
            list.Add("fullname");
            list.Add("company");
            list.Add("occupation");
            list.Add("address");
            list.Add("address2");
            list.Add("city");
            list.Add("state");
            list.Add("zip");
            list.Add("country");
            list.Add("voice");
            list.Add("mobile");
            list.Add("fax");
            list.Add("website");
            list.Add("age");
            list.Add("income");
            list.Add("gender");
            list.Add("user1");
            list.Add("user2");
            list.Add("user3");
            list.Add("user4");
            list.Add("user5");
            list.Add("user6");
            list.Add("birthdate");
            list.Add("userevent1");
            list.Add("userevent1date");
            list.Add("userevent2");
            list.Add("userevent2date");
            list.Add("notes");

            return list;
        }

        private IList<string> RemoveList(IList<string> list, IList<string> subList)
        {
            var listNoExist = new List<string>();
            foreach (var stringToRemove in subList)
            {
                if (!list.Contains(stringToRemove))
                {
                    listNoExist.Add(stringToRemove);
                }
            }

            listNoExist.Remove("blastid");
            listNoExist.Remove("groupid");
            listNoExist.Remove("groupname");
            listNoExist.Remove("emailtofriend");
            listNoExist.Remove("conversiontrkcde");
            listNoExist.Remove("unsubscribelink");
            listNoExist.Remove("lastchanged");
            listNoExist.Remove("createdon");
            listNoExist.Remove("publicview");
            listNoExist.Remove("company_address");
            listNoExist.Remove("surveytitle");
            listNoExist.Remove("surveylink");
            listNoExist.Remove("currdate");
            listNoExist.Remove("reportabuselink");
            listNoExist.Remove("profilepreferences");
            listNoExist.Remove("emailfromaddress");
            listNoExist.Remove("customer_name");
            listNoExist.Remove("customer_address");
            listNoExist.Remove("customer_webaddress");
            listNoExist.Remove("customer_udf1");
            listNoExist.Remove("customer_udf2");
            listNoExist.Remove("customer_udf3");
            listNoExist.Remove("customer_udf4");
            listNoExist.Remove("customer_udf5");
            listNoExist.Remove("slot1");
            listNoExist.Remove("slot2");
            listNoExist.Remove("slot3");
            listNoExist.Remove("slot4");
            listNoExist.Remove("slot5");
            listNoExist.Remove("slot6");
            listNoExist.Remove("slot7");
            listNoExist.Remove("slot8");
            listNoExist.Remove("slot9");
            listNoExist.Remove("slot10");

            return listNoExist;
        }

        private List<string> GetShortNamesForDynamicStrings(List<string> listLY, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Group;
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            DataTable dtEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetColumnNames();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);

            System.Collections.Generic.List<string> listCS = new System.Collections.Generic.List<string>();

            foreach (DataRow dr in dtEmail.Rows)
            {
                listCS.Add(dr["columnName"].ToString().ToLower());
            }
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
            {
                listCS.Add(gdf.ShortName.ToLower());
            }

            System.Collections.Generic.List<string> subLY = new System.Collections.Generic.List<string>();
            foreach (string s in listLY)
            {
                #region Badly Formed Snippets
                //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
                System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
                if (MatchList.Count > 0)
                {
                    if ((MatchList.Count % 2) != 0)
                    {
                        //return error
                        errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("%%[a-zA-Z0-9_]+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
                        if ((MatchList.Count / 2) > MatchListGood.Count)
                        {
                            //return error
                            errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                        }
                    }
                }
                #endregion

                //%% and ##
                System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("%%.+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchCollection MatchList1 = reg1.Matches(s);

                foreach (System.Text.RegularExpressions.Match m in MatchList1)
                {
                    if (!string.IsNullOrEmpty(m.Value.ToString()))
                    {
                        if (!subLY.Contains(m.Value.ToString().Replace("%%", string.Empty)))
                            subLY.Add(m.Value.ToString().Replace("%%", string.Empty));
                    }
                }
            }
            System.Collections.Generic.List<string> listNoExist = new System.Collections.Generic.List<string>();
            foreach (string s in subLY)
            {
                if (!listCS.Contains(s))
                    listNoExist.Add(s);
            }

            listNoExist.Remove("blastid");
            listNoExist.Remove("groupid");
            listNoExist.Remove("groupname");
            listNoExist.Remove("emailtofriend");
            listNoExist.Remove("conversiontrkcde");
            listNoExist.Remove("unsubscribelink");
            listNoExist.Remove("lastchanged");
            listNoExist.Remove("createdon");
            listNoExist.Remove("publicview");
            listNoExist.Remove("company_address");
            listNoExist.Remove("surveytitle");
            listNoExist.Remove("surveylink");
            listNoExist.Remove("currdate");
            listNoExist.Remove("reportabuselink");

            listNoExist.Remove("customer_name");
            listNoExist.Remove("customer_address");
            listNoExist.Remove("customer_webaddress");

            listNoExist.Remove("customer_udf1");
            listNoExist.Remove("customer_udf2");
            listNoExist.Remove("customer_udf3");
            listNoExist.Remove("customer_udf4");
            listNoExist.Remove("customer_udf5");

            return listNoExist;
        }

        protected void btnSubmitQTB_onclick(object sender, EventArgs e)
        {
            
            bool createGroup = false;
            
            int? groupID = null;
            int? campaignItemID = null;
            int? campaignID = null;
            string groupName = "";
            string campaignName = "";
            string campaignItemName = "";
            string emails = "";
            

            ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
            ECN_Framework_Entities.Communicator.Campaign camp = new ECN_Framework_Entities.Communicator.Campaign();

            // Campaign
            if (GetCampaignItemID() > 0)
            {
                campaignItemID = GetCampaignItemID();
                campaignID = null;
                
            }
            else
            {
                if (rbCampaignChoice1.Checked)  // new campaign
                {
                    if (txtCampaignName.Text == string.Empty)
                    {
                        throwECNException(Enums.Entity.CampaignItem, "CampaignName cannot be empty");
                        return;
                    }
                    if (txtCampaignItemName.Text == string.Empty)
                    {
                        throwECNException(Enums.Entity.CampaignItem, "CampaignItemName cannot be empty");
                        return;
                    }
                    campaignName = txtCampaignName.Text.Trim();
                    campaignItemName = txtCampaignItemName.Text.Trim();
                }
                else                            // existing campaign
                {
                    if (Convert.ToInt32(ddlCampaigns.SelectedValue) == -1)
                    {
                        throwECNException(Enums.Entity.CampaignItem, "Campaign not selected");
                        return;
                    }
                    if (txtCampaignItemName.Text == string.Empty)
                    {
                        throwECNException(Enums.Entity.CampaignItem, "CampaignItemName cannot be empty");
                        return;
                    }
                    campaignID = Convert.ToInt32(ddlCampaigns.SelectedValue.ToString());
                    campaignItemName = txtCampaignItemName.Text.Trim();
                }
            }

            StringBuilder xmlInsert = new StringBuilder();
            // Group
            if (rbGroupChoice1.Checked) // select Group
            {
                createGroup = false;
                if (hfSelectGroupID.Value.ToString() == "0" || hfSelectGroupID.Value.ToString() == "")
                {
                    throwECNException(Enums.Entity.Group, "Group not selected");
                    return;
                }
                groupID = Convert.ToInt32(hfSelectGroupID.Value.ToString());
                groupName = "";
                emails = "";
            }
            else                        // adhoc
            {
                if (tbGroupName.Text == string.Empty && tbGroupName.Visible)
                {
                    throwECNException(Enums.Entity.Group, "GroupName cannot be empty");
                    return;
                }
                if (taAddresses.Text == string.Empty)
                {
                    throwECNException(Enums.Entity.Group, "Addresses cannot be empty");
                    return;
                }
                createGroup = true;
                groupName = tbGroupName.Text.Trim();
                emails = taAddresses.Text;
            }

            if (hfSelectedLayoutTrigger.Value == "")
            {
                throwECNException(Enums.Entity.Layout, "Message Layout not selected");
                return;
            }

            int layoutID = -1;
            int.TryParse(hfSelectedLayoutTrigger.Value, out layoutID);

            try
            {

                ECN_Framework_BusinessLayer.Communicator.QuickTestBlast.CreateQuickTestBlast(Master.UserSession.CustomerID, Master.UserSession.BaseChannelID, groupID,groupName, emails, layoutID, campaignItemID, campaignItemName,campaignID, campaignName, chbEmailPreview.Checked, chbEnableCacheBuster.Checked, rbTextVersionYes.Checked, BlastFromEmail, BlastReplyToEmail, BlastFromName, txtEmailSubject.Value.Trim(), Master.UserSession.CurrentUser);
            }
            catch (ECNException ecn)
            {
                setECNError(ecn);
                return;
            }

            // Redirect to /ecn.communicator/main/ecnwizard
            Response.Redirect("default.aspx");
        }
    }
}