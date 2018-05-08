using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using System.Text.RegularExpressions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using Sample = ECN_Framework_Entities.Communicator.Sample;
using EntitiesCampaignItem = ECN_Framework_Entities.Communicator.CampaignItem;
using BusinessContent = ECN_Framework_BusinessLayer.Communicator.Content;
using BusinessSample = ECN_Framework_BusinessLayer.Communicator.Sample;


namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardContent_AB : WizardContentBase, IECNWizard
    {
        private const string NonBrakingSpace = "&nbsp;";
        private const string Whitespace = " ";
        private const int MaxSubjectLength = 255;
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

        string _errormessage = string.Empty;
        public string ErrorMessage
        {
            set
            {
                _errormessage = value;
            }
            get
            {
                return _errormessage;
            }
        }

        bool _isPredefinedEnvelopeInfo = true;
        public bool IsPredefinedEnvelopeInfo
        {
            set
            {
                _isPredefinedEnvelopeInfo = value;
            }
            get
            {
                return _isPredefinedEnvelopeInfo;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                layoutExplorer.enableSelectMode();
            }
        }

        private string BlastFromName
        {
            get
            {
                if (ddlFromNameA.Visible)
                    return ddlFromNameA.SelectedItem.Text;
                else
                    return txtFromNameA.Text.Trim();
            }
            set
            {
                if (ddlFromNameA.Visible)
                {
                    ddlFromNameA.ClearSelection();
                    try
                    {
                        ddlFromNameA.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }
                    txtFromNameA.Text = value.ToString().Trim();

                }
                else
                {
                    txtFromNameA.Text = value.ToString().Trim();
                }
            }
        }

        private string BlastFromEmail
        {
            get
            {
                if (ddlFromNameA.Visible)
                    return ddlFromEmailA.SelectedItem.Text;
                else
                    return txtEmailFromA.Text;
            }
            set
            {
                if (ddlFromNameA.Visible)
                {
                    ddlFromEmailA.ClearSelection();
                    try
                    {
                        ddlFromEmailA.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }

                    txtEmailFromA.Text = value.ToString().Trim();
                }
                else
                {
                    txtEmailFromA.Text = value.ToString().Trim();
                }
            }
        }

        private string BlastReplyToEmail
        {
            get
            {
                if (ddlFromNameA.Visible)
                    return ddlReplyToA.SelectedItem.Text;
                else
                    return txtReplyToA.Text;
            }
            set
            {
                if (ddlFromNameA.Visible)
                {
                    ddlReplyToA.ClearSelection();
                    try
                    {
                        ddlReplyToA.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }
                    txtReplyToA.Text = value.Trim(); ;
                }
                else
                {
                    txtReplyToA.Text = value.Trim();
                }
            }
        }

        public void drpEmailFrom_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlFromNameA.ClearSelection();
            ddlReplyToA.ClearSelection();
            if (ddlFromEmailA.SelectedIndex > -1)
            {
                ddlFromNameA.Items.FindByValue(ddlFromEmailA.SelectedValue).Selected = true;
                ddlReplyToA.Items.FindByValue(ddlFromEmailA.SelectedValue).Selected = true;
            }

            BlastFromName = ddlFromNameA.SelectedItem.Text;
            BlastFromEmail = ddlFromEmailA.SelectedItem.Text;
            BlastReplyToEmail = ddlReplyToA.SelectedItem.Text;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "AlreadyDone = []; pageloaded();", true);
        }

        public void drpReplyTo_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlFromNameA.ClearSelection();
            ddlFromEmailA.ClearSelection();

            if (ddlReplyToA.SelectedIndex > -1)
            {
                ddlFromNameA.Items.FindByValue(ddlReplyToA.SelectedValue).Selected = true;
                ddlFromEmailA.Items.FindByValue(ddlReplyToA.SelectedValue).Selected = true;
            }

            BlastFromName = ddlFromNameA.SelectedItem.Text;
            BlastFromEmail = ddlFromEmailA.SelectedItem.Text;
            BlastReplyToEmail = ddlReplyToA.SelectedItem.Text;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "AlreadyDone = []; pageloaded();", true);
        }

        public void drpEmailFromName_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlFromEmailA.ClearSelection();
            ddlReplyToA.ClearSelection();
            if (ddlFromNameA.SelectedIndex > -1)
            {
                ddlReplyToA.Items.FindByValue(ddlFromNameA.SelectedValue).Selected = true;
                ddlFromEmailA.Items.FindByValue(ddlFromNameA.SelectedValue).Selected = true;
            }

            BlastFromName = ddlFromNameA.SelectedItem.Text;
            BlastFromEmail = ddlFromEmailA.SelectedItem.Text;
            BlastReplyToEmail = ddlReplyToA.SelectedItem.Text;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "AlreadyDone = []; pageloaded();", true);
        }

        public void Initialize()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci =
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                loadData(ci);
                List<ECN_Framework_Entities.Communicator.BlastEnvelope> blastEnvelopeList =
               ECN_Framework_BusinessLayer.Communicator.BlastEnvelope.GetByCustomerID_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);

                if (blastEnvelopeList.Count > 0)
                {
                    btnChangeEnvelopeA.Visible = true;
                    ToggleEnvelopePanel(IsPredefinedEnvelopeInfo);

                    ddlFromEmailA.DataSource = blastEnvelopeList;
                    ddlFromEmailA.DataValueField = "BlastEnvelopeID";
                    ddlFromEmailA.DataTextField = "FromEmail";
                    ddlFromEmailA.DataBind();
                    ddlFromEmailA.Items.Insert(0, new ListItem("Select From Email", ""));

                    ddlReplyToA.DataSource = blastEnvelopeList;
                    ddlReplyToA.DataValueField = "BlastEnvelopeID";
                    ddlReplyToA.DataTextField = "FromEmail";
                    ddlReplyToA.DataBind();
                    ddlReplyToA.Items.Insert(0, new ListItem("Select Reply To Email", ""));

                    ddlFromNameA.DataSource = blastEnvelopeList;
                    ddlFromNameA.DataValueField = "BlastEnvelopeID";
                    ddlFromNameA.DataTextField = "FromName";
                    ddlFromNameA.DataBind();
                    ddlFromNameA.Items.Insert(0, new ListItem("Select From Name", ""));
                }
                else
                {
                    btnChangeEnvelopeA.Visible = false;
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void loadData(ECN_Framework_Entities.Communicator.CampaignItem ci)
        {


            ECN_Framework_Entities.Accounts.Customer currentCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, false);
            if (ci.SampleID != null)
            {
                ECN_Framework_Entities.Communicator.Sample currentSample = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID(ci.SampleID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                ddlAbWinnerType.SelectedValue = currentSample.ABWinnerType.ToLower();
            }
            else if (currentCustomer.ABWinnerType == null)
            {
                ddlAbWinnerType.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["KMWinnerTypeDefault"];
            }
            else
            {
                ddlAbWinnerType.SelectedValue = currentCustomer.ABWinnerType.ToLower();
            }

            if (ci.BlastList.Count > 0)
            {
                if (ci.BlastList[0].LayoutID != null)
                {
                    hfSelectedLayoutA.Value = ci.BlastList[0].LayoutID.ToString();
                    ECN_Framework_Entities.Communicator.Layout lA = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(ci.BlastList[0].LayoutID.Value, false);
                    lblSelectedLayoutA.Text = lA.LayoutName;
                    if (ci.BlastList[1].LayoutID != null)
                    {
                        hfSelectedLayoutB.Value = ci.BlastList[1].LayoutID.ToString();
                        ECN_Framework_Entities.Communicator.Layout lB = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(ci.BlastList[1].LayoutID.Value, false);
                        lblSelectedLayoutB.Text = lB.LayoutName;
                    }

                    txtEmailFromA.Text = ci.BlastList[0].EmailFrom;
                    txtFromNameA.Text = ci.BlastList[0].FromName;
                    txtReplyToA.Text = ci.BlastList[0].ReplyTo;

                    txtEmailFromB.Text = ci.BlastList[1].EmailFrom;
                    txtFromNameB.Text = ci.BlastList[1].FromName;
                    txtReplyToB.Text = ci.BlastList[1].ReplyTo;

                    txtSubjectA.Value = ci.BlastList[0].EmailSubject;
                    txtSubjectB.Value = ci.BlastList[1].EmailSubject;

                    IsPredefinedEnvelopeInfo = false;
                }
                else
                {
                    if (ci.CampaignItemTemplateID != null)
                    {
                        ECN_Framework_Entities.Communicator.CampaignItemTemplate cit = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(ci.CampaignItemTemplateID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        if (cit.FromEmail != null)
                            txtEmailFromA.Text = cit.FromEmail;
                        if (cit.FromEmail != null)
                            txtFromNameA.Text = cit.FromName;
                        if (cit.FromEmail != null)
                            txtReplyToA.Text = cit.ReplyTo;
                        if (cit.FromEmail != null)
                            txtSubjectA.Value = cit.Subject;
                        IsPredefinedEnvelopeInfo = false;
                    }
                }

            }

        }

        protected void btnChangeEnvelope_onclick(object sender, System.EventArgs e)
        {
            if (ddlFromEmailA.Visible)
                ToggleEnvelopePanel(false);
            else
                ToggleEnvelopePanel(true);

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "AlreadyDone = []; pageloaded();", true);
        }

        private void ToggleEnvelopePanel(bool IsPredefinedEnvelopeInfo)
        {
            if (IsPredefinedEnvelopeInfo)
            {
                txtEmailFromA.Attributes.Add("style", "display:none");
                txtReplyToA.Attributes.Add("style", "display:none");
                txtFromNameA.Attributes.Add("style", "display:none");

                ddlFromEmailA.Visible = true;
                ddlReplyToA.Visible = true;
                ddlFromNameA.Visible = true;
            }
            else
            {
                txtEmailFromA.Attributes.Add("style", "display:inline");
                txtReplyToA.Attributes.Add("style", "display:inline");
                txtFromNameA.Attributes.Add("style", "display:inline");

                ddlFromEmailA.Visible = false;
                ddlReplyToA.Visible = false;
                ddlFromNameA.Visible = false;

            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Layout, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
        }

        private static void throwECNWarning(string message, bool bShow = true)
        {
            ECNWarning ecnError = new ECNWarning(Enums.Entity.Layout, Enums.Method.Save, message);
            List<ECNWarning> errorList = new List<ECNWarning>();
            errorList.Add(ecnError);
            throw new ECNWarning(errorList, Enums.ExceptionLayer.WebSite);
        }

        public bool Save()
        {
            ProcessSave();
            return true;

        }

        public void SaveNoUdfCheck(object sender, EventArgs eventArgs)
        {
            ProcessSave(false);
            var rawUrl = Request.RawUrl;
            if (rawUrl.Contains("CampaignItemID="))
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                Response.Redirect(Request.RawUrl + "&CampaignItemID=" + CampaignItemID);
            }

        }

        private void ProcessSave(bool bValidate = true)
        {
            using (var scope = new TransactionScope())
            {
                var campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                SaveSampleForCompain(campaignItem);
                ValidateControl();
                var cleanEmailSubjectA = CleanEmailSubject(txtSubjectA);
                var cleanEmailSubjectB = CleanEmailSubject(txtSubjectB);

                var listErrorGroups = new List<string>();
                listErrorGroups = ValidateBlast(bValidate, campaignItem, 0, listErrorGroups, hfSelectedLayoutA);
                ValidateLayoutAndContent(hfSelectedLayoutA);
                FillAndSaveBlast(campaignItem, cleanEmailSubjectA, 0, hfSelectedLayoutA);

                listErrorGroups = ValidateBlast(bValidate, campaignItem, 1, listErrorGroups, hfSelectedLayoutB);
                ValidateLayoutAndContent(hfSelectedLayoutB);
                FillAndSaveBlast(campaignItem, cleanEmailSubjectB, 1, hfSelectedLayoutB);

                if (listErrorGroups.Count > 0)
                {
                    var distinctItems = listErrorGroups.Distinct();
                    var errorGroups = distinctItems.Aggregate(string.Empty, (current, item) => current + (item + ','));
                    errorGroups = errorGroups.Remove(errorGroups.Length - 1);
                    modalPopupUdfChoice.Show();
                    btnEditGroup.OnClientClick = "window.location.href='../../../ecn.communicator.mvc/Group'; return false;";
                    lblErrorMessage.Text = $"The content being used contains topic codes which have not been set up in the following groups ({errorGroups}). <br/> If you continue, any of the topic tracking will not be collected.<br/> If you wish for the data to be collected please add the UDF to the groups.";
                    throwECNWarning($"The content being used contains topic codes which have not been set up in the following groups ({errorGroups}). <br/> If you continue, any of the topic tracking will not be collected.<br/> If you wish for the data to be collected please add the UDF to the groups.");
                    throwECNException($"The content being used contains topic codes which have not been set up in the following groups ({errorGroups}).");
                }

                UpdateAndSaveBlast(campaignItem);
                Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, ECNSession.CurrentSession().CurrentUser, true);
                scope.Complete();
            }
        }

        private void UpdateAndSaveBlast(EntitiesCampaignItem campaignItem)
        {
            campaignItem.FromEmail = BlastFromEmail.Trim();
            campaignItem.FromName = BlastFromName.Trim();
            campaignItem.ReplyTo = BlastReplyToEmail.Trim();
            if (campaignItem.CompletedStep < 3)
            {
                campaignItem.CompletedStep = 3;
            }

            campaignItem.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
            CampaignItem.Save(campaignItem, ECNSession.CurrentSession().CurrentUser);
        }

        private List<string> ValidateBlast(
            bool bValidate,
            EntitiesCampaignItem campaignItem,
            int blastIndex,
            List<string> listErrorGroups,
            HiddenField selectedLayout)
        {
            if (bValidate && campaignItem.BlastList[blastIndex].GroupID != null)
            {
                listErrorGroups = ValidateUdfTopic(Convert.ToInt32(selectedLayout.Value),
                    campaignItem.BlastList[blastIndex].GroupID,
                    ECNSession.CurrentSession().CurrentUser,
                    listErrorGroups);
            }

            return listErrorGroups;
        }

        private void FillAndSaveBlast(EntitiesCampaignItem campaignItem, string cleanEmailSubjectA, int blastIndex, HiddenField selectedLayout)
        {
            campaignItem.BlastList[blastIndex].LayoutID = Convert.ToInt32(selectedLayout.Value);
            campaignItem.BlastList[blastIndex].UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
            campaignItem.BlastList[blastIndex].EmailSubject = cleanEmailSubjectA.Replace("&nbsp;", "").Trim();
            campaignItem.BlastList[blastIndex].EmailFrom = txtEmailFromA.Text.Trim();
            campaignItem.BlastList[blastIndex].ReplyTo = txtReplyToA.Text.Trim();
            campaignItem.BlastList[blastIndex].FromName = txtFromNameA.Text.Trim();
            CampaignItemBlast.Save(campaignItem.BlastList[blastIndex], ECNSession.CurrentSession().CurrentUser);
        }

        private void ValidateLayoutAndContent(HiddenField selectedLayout)
        {
            int layoutId;
            if (!int.TryParse(selectedLayout.Value, out layoutId))
            {
                throwECNException("Layout Id is incorrect");
                return;
            }

            var layout = Layout.GetByLayoutID_NoAccessCheck(layoutId, false);
            var template = Template.GetByTemplateID_NoAccessCheck(layout.TemplateID.GetValueOrDefault());
            if (template.SlotsTotal != 1)
            {
                return;
            }

            var content = BusinessContent.GetByContentID_NoAccessCheck(
                layout.ContentSlot1.GetValueOrDefault(),
                false);
            if (content?.ContentSource == null)
            {
                return;
            }

            try
            {
                BusinessContent.ValidateHTMLContent(content.ContentSource);
            }
            catch (ECNException)
            {
                throwECNException("Content for A blast is missing required closing tags");
            }
        }

        private string CleanEmailSubject(HiddenField txtSubject)
        {
            var cleanEmailSubjectA = CleanAndStripHTML(txtSubject.Value);
            cleanEmailSubjectA = cleanEmailSubjectA.Replace(NonBrakingSpace, Whitespace).Trim();
            if (cleanEmailSubjectA.Length > MaxSubjectLength)
            {
                throwECNException("Email Subject cannot be more than 255 characters");
            }

            return cleanEmailSubjectA;
        }

        private void ValidateControl()
        {
            if (string.IsNullOrWhiteSpace(hfSelectedLayoutA.Value))
            {
                throwECNException("Please select a Message for Slot A");
            }

            if (string.IsNullOrWhiteSpace(hfSelectedLayoutB.Value))
            {
                throwECNException("Please select a Message for Slot B");
            }

            if (txtEmailFromA.Text == string.Empty ||
                txtFromNameA.Text == string.Empty ||
                txtReplyToA.Text == string.Empty ||
                txtSubjectA.Value == string.Empty ||
                txtSubjectB.Value == string.Empty ||
                txtEmailFromB.Text == string.Empty ||
                txtFromNameB.Text == string.Empty ||
                txtReplyToB.Text == string.Empty)
            {
                throwECNException("Incomplete envelope information");
            }
            else
            {
                ValidateEmailSubject(txtSubjectA);
                ValidateEmailSubject(txtSubjectB);
            }


            if (BlastFromEmail.Equals("Select From Email") ||
                BlastReplyToEmail.Equals("Select Reply To Email") ||
                BlastReplyToEmail.Equals("Select From Name"))
            {
                throwECNException("Incomplete envelope information");
            }
        }

        private void ValidateEmailSubject(HiddenField subjectField)
        {
            var subjectValid = RegexUtilities.IsValidEmailSubject(subjectField.Value);
            if (!string.IsNullOrWhiteSpace(subjectValid))
            {
                throwECNException(
                    $"Email Subject contains invalid characters - {subjectValid} If you copied and pasted the Email Subject you can try first copying and pasting to Notepad which will likely remove all of the invalid characters");
            }
        }

        private void SaveSampleForCompain(EntitiesCampaignItem campaignItem)
        {
            if (campaignItem.SampleID == null)
            {
                var sample = new Sample
                {
                    SampleName = campaignItem.CampaignItemName,
                    CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                    CustomerID = ECNSession.CurrentSession().CurrentUser.CustomerID,
                    ABWinnerType = ddlAbWinnerType.SelectedValue
                };
                BusinessSample.Save(sample, ECNSession.CurrentSession().CurrentUser);
                campaignItem.SampleID = sample.SampleID;
            }
            else
            {
                var sample = BusinessSample.GetBySampleID(campaignItem.SampleID.Value,
                    ECNSession.CurrentSession().CurrentUser);
                sample.ABWinnerType = ddlAbWinnerType.SelectedValue;
                sample.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
                BusinessSample.Save(sample, ECNSession.CurrentSession().CurrentUser);
            }
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

        protected void imgSelectLayoutA_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            layoutExplorer.enableSelectMode();
            pnlLayoutExplorer.Update();
            hfWhichLayout.Value = "A";
            mpeLayoutExplorer.Show();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "AlreadyDone = [];pageloaded();", true);
        }

        protected void imgSelectLayoutB_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            layoutExplorer.enableSelectMode();
            pnlLayoutExplorer.Update();
            hfWhichLayout.Value = "B";
            mpeLayoutExplorer.Show();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "AlreadyDone = []; pageloaded();", true);
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                if (sender is ECN_Framework_Entities.Communicator.Layout)
                {
                    ECN_Framework_Entities.Communicator.Layout layout = (ECN_Framework_Entities.Communicator.Layout)sender;
                    if (hfWhichLayout.Value.Equals("A"))
                    {
                        lblSelectedLayoutA.Text = layout.LayoutName;
                        hfSelectedLayoutA.Value = layout.LayoutID.ToString();


                    }
                    else if (hfWhichLayout.Value.Equals("B"))
                    {
                        lblSelectedLayoutB.Text = layout.LayoutName;
                        hfSelectedLayoutB.Value = layout.LayoutID.ToString();

                    }
                    UpdatePanel1.Update();
                    mpeLayoutExplorer.Hide();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", " AlreadyDone = []; pageloaded();", true);
                    return true;
                }
                return true;

            }
            catch
            {
                mpeLayoutExplorer.Hide();
                return true;
            }
        }

        protected void btnPullFromA_Click(object sender, EventArgs e)
        {
            txtEmailFromB.Text = txtEmailFromA.Text;
            txtFromNameB.Text = txtFromNameA.Text;
            txtReplyToB.Text = txtReplyToA.Text;
            txtSubjectB.Value = txtSubjectA.Value;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", " AlreadyDone = []; pageloaded();", true);
        }

        protected void btnCloseLayoutExplorer_Click(object sender, EventArgs e)
        {
            mpeLayoutExplorer.Hide();
        }
    }
}