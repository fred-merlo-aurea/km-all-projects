using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using ECN_Framework;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic;
using BusinessCampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using BusinessCampaignItemBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast;
using BusinessBlast = ECN_Framework_BusinessLayer.Communicator.Blast;
using Enums = ECN_Framework_Common.Objects.Enums;
using BusinessLayout = ECN_Framework_BusinessLayer.Communicator.Layout;
using BusinessTemplate = ECN_Framework_BusinessLayer.Communicator.Template;
using BusinessContent = ECN_Framework_BusinessLayer.Communicator.Content;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardContent : WizardContentBase, IECNWizard
    {
        private const string Nbsp = "&nbsp;";
        private const string Whitespace = " ";

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

        private bool readData
        {
            get
            {
                if (ViewState["WizardContent_readData"] == null)
                    return true;
                else
                    return (bool)ViewState["WizardContent_readData"];
            }

            set { ViewState["WizardContent_readData"] = value; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            phWarning.Visible = false;
            modalPopupUdfChoice.Hide();
            if (readData)
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci =
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                loadData(ci);
                readData = false;
            }

        }

        private void DoTwemoji()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "doTwemoji", "AlreadyDone = []; pageloaded();", true);
        }

        public void Initialize()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {

                layoutExplorer1.enableSelectMode();
                layoutExplorer1.reset();
                layoutExplorer1.loadFolders();
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPersonalization))
                {
                    setDynamicPersonalizationFieldsVisibility(true);
                }
                if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
                {
                    layoutEditor1.Initialize();
                    rbNewLayout.Visible = true;
                }
                else
                {
                    rbNewLayout.Visible = false;
                }
                List<ECN_Framework_Entities.Communicator.BlastEnvelope> blastEnvelopeList =
                ECN_Framework_BusinessLayer.Communicator.BlastEnvelope.GetByCustomerID_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
                if (blastEnvelopeList.Count > 0)
                {
                    btnChangeEnvelope.Visible = true;
                    ToggleEnvelopePanel(IsPredefinedEnvelopeInfo);

                    drpEmailFrom.DataSource = blastEnvelopeList;
                    drpEmailFrom.DataValueField = "BlastEnvelopeID";
                    drpEmailFrom.DataTextField = "FromEmail";
                    drpEmailFrom.DataBind();
                    drpEmailFrom.Items.Insert(0, new ListItem("Select From Email", ""));

                    drpReplyTo.DataSource = blastEnvelopeList;
                    drpReplyTo.DataValueField = "BlastEnvelopeID";
                    drpReplyTo.DataTextField = "FromEmail";
                    drpReplyTo.DataBind();
                    drpReplyTo.Items.Insert(0, new ListItem("Select Reply To Email", ""));

                    drpEmailFromName.DataSource = blastEnvelopeList;
                    drpEmailFromName.DataValueField = "BlastEnvelopeID";
                    drpEmailFromName.DataTextField = "FromName";
                    drpEmailFromName.DataBind();
                    drpEmailFromName.Items.Insert(0, new ListItem("Select From Name", ""));
                }
                else
                {
                    btnChangeEnvelope.Visible = false;
                }

                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                if (ci.FromEmail == string.Empty && ci.CampaignItemTemplateID.HasValue && ci.CampaignItemTemplateID.Value > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTemplate cit = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(ci.CampaignItemTemplateID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                    if (cit.LayoutID != null)
                    {
                        layoutExplorer1.selectedLayoutID = cit.LayoutID.Value;
                        //ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetTopOneByLayoutID_NoAccessCheck(cit.LayoutID.Value, false);
                        //if (blast != null)
                        //{
                        //    txtEmailFrom.Text = blast.EmailFrom;
                        //    txtEmailFromName.Text = blast.EmailFromName;
                        //    txtReplyTo.Text = blast.ReplyTo;
                        //    txtEmailSubject.Value = blast.EmailSubject;
                        //}
                        //else
                        //{
                        //    txtEmailFrom.Text = "";
                        //    txtEmailFromName.Text = "";
                        //    txtReplyTo.Text = "";
                        //    txtEmailSubject.Value = "";
                        //}                        
                    }

                    if (!string.IsNullOrEmpty(cit.FromEmail))
                        txtEmailFrom.Text = cit.FromEmail;
                    if (!string.IsNullOrEmpty(cit.FromName))
                        txtEmailFromName.Text = cit.FromName;
                    if (!string.IsNullOrEmpty(cit.ReplyTo))
                        txtReplyTo.Text = cit.ReplyTo;
                    if (!string.IsNullOrEmpty(cit.Subject))
                        txtEmailSubject.Value = cit.Subject;

                    ToggleEnvelopePanel(false);
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }

        }

        private void loadData(ECN_Framework_Entities.Communicator.CampaignItem ci)
        {
            if (ci.BlastList.Count > 0)
            {
                if (ci.BlastList[0].LayoutID != null)
                {
                    if (rbExistingLayout.Checked)
                    {
                        ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = ci.BlastList[0];
                        rbExistingLayout.Checked = true;
                        plNewLayout.Visible = false;
                        plExistingLayout.Visible = true;
                        if (layoutExplorer1.selectedLayoutID == 0)
                            layoutExplorer1.selectedLayoutID = ciBlast.LayoutID.Value;
                        txtEmailFrom.Text = ci.FromEmail;
                        txtReplyTo.Text = ci.ReplyTo;
                        txtEmailFromName.Text = ci.FromName;
                        txtEmailSubject.Value = ciBlast.EmailSubject;
                        IsPredefinedEnvelopeInfo = false;
                    }

                }

                if (ci.BlastList.Count == 1)
                {
                    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPersonalization))
                    {
                        List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList =
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(ci.BlastList[0].GroupID.Value);
                        var result = (from src in gdfList
                                      select new
                                      {
                                          ShortNameText = src.ShortName,
                                          ShortNameValue = "%%" + src.ShortName + "%%"
                                      }).ToList();

                        if (result.Count > 0)
                        {
                            dyanmicEmailFrom.Items.Clear();
                            dyanmicEmailFrom.DataSource = result;
                            dyanmicEmailFrom.DataBind();
                            dyanmicEmailFrom.Items.Insert(0, new ListItem("-- select --", ""));
                            dyanmicEmailFrom.Enabled = true;
                            if (ci.BlastList[0].DynamicFromEmail != null)
                            {
                                if (ci.BlastList[0].DynamicFromEmail != string.Empty)
                                {
                                    dyanmicEmailFrom.ClearSelection();
                                    dyanmicEmailFrom.Items.FindByValue(ci.BlastList[0].DynamicFromEmail).Selected = true;
                                }
                            }

                            dyanmicReplyToEmail.Items.Clear();
                            dyanmicReplyToEmail.DataSource = result;
                            dyanmicReplyToEmail.DataBind();
                            dyanmicReplyToEmail.Items.Insert(0, new ListItem("-- select --", ""));
                            dyanmicReplyToEmail.Enabled = true;
                            if (ci.BlastList[0].DynamicReplyTo != null)
                            {
                                if (ci.BlastList[0].DynamicReplyTo != string.Empty)
                                {
                                    dyanmicReplyToEmail.ClearSelection();
                                    dyanmicReplyToEmail.Items.FindByValue(ci.BlastList[0].DynamicReplyTo).Selected = true;
                                }
                            }

                            dyanmicEmailFromName.Items.Clear();
                            dyanmicEmailFromName.DataSource = result;
                            dyanmicEmailFromName.DataBind();
                            dyanmicEmailFromName.Items.Insert(0, new ListItem("-- select --", ""));
                            dyanmicEmailFromName.Enabled = true;
                            if (ci.BlastList[0].DynamicFromName != null)
                            {
                                if (ci.BlastList[0].DynamicFromName != string.Empty)
                                {
                                    dyanmicEmailFromName.ClearSelection();
                                    dyanmicEmailFromName.Items.FindByValue(ci.BlastList[0].DynamicFromName).Selected = true;
                                }
                            }
                        }
                        else
                        {
                            dyanmicEmailFrom.Enabled = false;
                            dyanmicReplyToEmail.Enabled = false;
                            dyanmicEmailFromName.Enabled = false;
                        }
                    }
                }
                else
                {
                    dyanmicEmailFrom.Enabled = false;
                    dyanmicReplyToEmail.Enabled = false;
                    dyanmicEmailFromName.Enabled = false;
                }
            }

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

        private void throwECNException(string message, bool Bubble = true)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Layout, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            if (Bubble)
                throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
            else
                setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));

        }



        private void throwECNWarning(string message, bool bShow = false, bool Bubble = true)
        {
            ECNWarning ecnError = new ECNWarning(Enums.Entity.Layout, Enums.Method.Save, message);
            List<ECNWarning> errorList = new List<ECNWarning>();
            errorList.Add(ecnError);
            if (Bubble)
                throw new ECNWarning(errorList, Enums.ExceptionLayer.WebSite);
            else
                setECNWarning(new ECNWarning(errorList, Enums.ExceptionLayer.WebSite));

        }


        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                ECN_Framework_Entities.Communicator.Layout layout = (ECN_Framework_Entities.Communicator.Layout)sender;
                ECN_Framework_Entities.Communicator.Blast blast =
                ECN_Framework_BusinessLayer.Communicator.Blast.GetTopOneByLayoutID_NoAccessCheck(layout.LayoutID, false);
                txtEmailFrom.Text = blast.EmailFrom;
                txtEmailFromName.Text = blast.EmailFromName;
                txtReplyTo.Text = blast.ReplyTo;
                txtEmailSubject.Value = blast.EmailSubject;
                ToggleEnvelopePanel(false);

                DoTwemoji();
                upEnvelope.Update();


                return true;

            }
            catch
            {
                //System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "doTwemoji", "pageloaded();", true);
                return true;
            }
        }

        protected void btnChangeEnvelope_onclick(object sender, EventArgs e)
        {
            ToggleEnvelopePanel(!drpEmailFrom.Visible);
            DoTwemoji();
        }

        private void ToggleEnvelopePanel(bool IsPredefinedEnvelopeInfo)
        {
            ToggleEnvelopePanelHelper.ToggleEnvelopePanel(
                IsPredefinedEnvelopeInfo,
                new TextBox[] { txtEmailFrom, txtReplyTo, txtEmailFromName },
                new DropDownList[] { drpEmailFrom, drpReplyTo, drpEmailFromName });

            var requiredFieldValidators = new RequiredFieldValidator[] { val_txtEmailFrom, val_txtReplyTo, val_txtEmailFromName };
            if (IsPredefinedEnvelopeInfo)
            {
                ToggleEnvelopePanelHelper.SetControlsToValidate(requiredFieldValidators, new string[] { "drpEmailFrom", "drpReplyTo", "drpEmailFromName" });
            }
            else
            {
                ToggleEnvelopePanelHelper.SetControlsToValidate(requiredFieldValidators, new string[] { "txtEmailFrom", "txtReplyTo", "txtEmailFromName" });
            }
        }

        public bool Save()
        {
            ProcessSave();
            return true;
        }

        public void SaveNoUdfCheck(object sender, EventArgs eventArgs)
        {
            try
            {
                ProcessSave(false, false);
                var rawUrl = Request.RawUrl;
                if (rawUrl.ToLower().Contains("campaignitemid="))
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    Response.Redirect(Request.RawUrl + "&CampaignItemID=" + CampaignItemID);
                }
            }
            catch (ECNException ecn)
            {
                setECNError(ecn);
            }
            catch (ECNWarning warn)
            {
                setECNWarning(warn, false);
            }



        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage1.Text = string.Empty;
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage1.Text = lblErrorMessage1.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void setECNWarning(ECN_Framework_Common.Objects.ECNWarning ecnException, bool bVisible = true)
        {
            phWarning.Visible = bVisible;
            lblWarningMessage.Text = string.Empty;
            foreach (ECN_Framework_Common.Objects.ECNWarning ecnError in ecnException.ErrorList)
            {
                lblWarningMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.WarningMessage;
            }
        }

        private void ProcessSave(bool validate = true, bool bubble = true)
        {
            var layoutId = GetLayoutId();
            if (layoutId == 0)
            {
                throwECNException("Layout not selected", bubble);
            }
            else
            {
                ValidateHtmlContentForLayout(bubble, layoutId);
            }

            ValidateEmailAddresses(bubble);

            if (BlastFromEmail.Equals("Select From Email") ||
                BlastReplyToEmail.Equals("Select Reply To Email") ||
                BlastReplyToEmail.Equals("Select From Name"))
            {
                throwECNException("Incomplete envelope information", bubble);
            }

            var cleanEmailSubject = ValidateEmailSubject(bubble);
            var campaignItem = BusinessCampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            var listErrorGroups = new List<string>();
            foreach (var ciBlast in campaignItem.BlastList)
            {
                if (validate && ciBlast.GroupID != null)
                {
                    listErrorGroups = ValidateUdfTopic(layoutId, ciBlast.GroupID, ECNSession.CurrentSession().CurrentUser, listErrorGroups);
                }

                SetBlastPropertiesForSave(ciBlast, layoutId, cleanEmailSubject);
            }

            if (listErrorGroups.Count > 0)
            {
                RaiseListError(bubble, listErrorGroups);
            }

            SetCampaignItemProperties(bubble, campaignItem);
            using (var scope = new TransactionScope())
            {
                BusinessCampaignItem.Save(
                    campaignItem,
                    ECNSession.CurrentSession().CurrentUser);
                BusinessCampaignItemBlast.Save(
                    CampaignItemID,
                    campaignItem.BlastList,
                    ECNSession.CurrentSession().CurrentUser);
                BusinessBlast.CreateBlastsFromCampaignItem(
                    campaignItem.CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser,
                    true);
                scope.Complete();
            }
        }

        private void SetCampaignItemProperties(bool bubble, CampaignItem campaignItem)
        {
            campaignItem.FromEmail = BlastFromEmail.Trim();
            campaignItem.FromName = BlastFromName.Trim();
            campaignItem.ReplyTo = BlastReplyToEmail.Trim();
            if (campaignItem.CompletedStep < 3)
            {
                campaignItem.CompletedStep = 3;
            }
            else if (!bubble && campaignItem.CompletedStep > 2)
            {
                campaignItem.CompletedStep = 3;
            }

            campaignItem.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
        }

        private void RaiseListError(bool bubble, IEnumerable<string> listErrorGroups)
        {
            var distinctItems = listErrorGroups.Distinct();
            var errorGroups = distinctItems.Aggregate(string.Empty, (current, item) => current + (item + ','));
            errorGroups = errorGroups.Remove(errorGroups.Length - 1);

            modalPopupUdfChoice.Show();
            btnEditGroup.OnClientClick = "window.location.href='../../../ecn.communicator.mvc/Group'; return false;";
            lblErrorMessage.Text = (
                $"The content being used contains topic codes which have not been set up in the following groups ({errorGroups}). <br/> If you continue, any of the topic tracking will not be collected.<br/> If you wish for the data to be collected please add the UDF to the groups."
            );
            throwECNWarning(
                $"The content being used contains topic codes which have not been set up in the following groups ({errorGroups}). <br/> If you continue, any of the topic tracking will not be collected.<br/> If you wish for the data to be collected please add the UDF to the groups.",
                bubble);
            throwECNException(
                $"The content being used contains topic codes which have not been set up in the following groups ({errorGroups}).",
                bubble);
        }

        private void SetBlastPropertiesForSave(CampaignItemBlast ciBlast, int layoutId, string cleanEmailSubject)
        {
            ciBlast.LayoutID = layoutId;
            ciBlast.EmailSubject = cleanEmailSubject;
            if (Client.HasServiceFeature(
                ECNSession.CurrentSession().CurrentUser.CurrentClient.ClientID,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.EmailPersonalization))
            {
                if (dyanmicEmailFrom.Enabled)
                {
                    ciBlast.DynamicFromEmail = dyanmicEmailFrom.SelectedValue;
                }

                if (dyanmicEmailFromName.Enabled)
                {
                    ciBlast.DynamicFromName = dyanmicEmailFromName.SelectedValue;
                }

                if (dyanmicReplyToEmail.Enabled)
                {
                    ciBlast.DynamicReplyTo = dyanmicReplyToEmail.SelectedValue;
                }
            }

            ciBlast.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
        }

        private string ValidateEmailSubject(bool bubble)
        {
            var cleanEmailSubject = CleanAndStripHTML(txtEmailSubject.Value);
            cleanEmailSubject = cleanEmailSubject.Replace(Nbsp, Whitespace).Trim();
            if (cleanEmailSubject.Length > 255)
            {
                throwECNException("Email Subject cannot be more than 255 characters", bubble);
            }

            return cleanEmailSubject;
        }

        private void ValidateEmailAddresses(bool bubble)
        {
            if (string.IsNullOrWhiteSpace(txtEmailFrom.Text) ||
                string.IsNullOrWhiteSpace(txtEmailFromName.Text) ||
                string.IsNullOrWhiteSpace(txtReplyTo.Text) ||
                string.IsNullOrWhiteSpace(txtEmailSubject.Value))
            {
                throwECNException("Incomplete envelope information", bubble);
            }
            else
            {
                var subjectValid = RegexUtilities.IsValidEmailSubject(txtEmailSubject.Value);
                if (!string.IsNullOrWhiteSpace(subjectValid))
                {
                    throwECNException(
                        $"Email Subject contains invalid characters - {subjectValid} If you copied and pasted the Email Subject you can try first copying and pasting to Notepad which will likely remove all of the invalid characters",
                        bubble);
                }
            }
        }

        private void ValidateHtmlContentForLayout(bool bubble, int layoutId)
        {
            var layout = BusinessLayout.GetByLayoutID_NoAccessCheck(layoutId, false);
            var template = BusinessTemplate.GetByTemplateID_NoAccessCheck(layout.TemplateID.GetValueOrDefault());
            if (template.SlotsTotal == 1)
            {
                var content = BusinessContent.GetByContentID_NoAccessCheck(
                    layout.ContentSlot1.GetValueOrDefault(),
                    false);
                if (content?.ContentSource != null)
                {
                    try
                    {
                        BusinessContent.ValidateHTMLContent(content.ContentSource);
                    }
                    catch (ECNException ex)
                    {
                        var errorString = ex.ErrorList.Aggregate(string.Empty,
                            (current, ecnError) => current + ($"{ecnError.Entity}: {ecnError.ErrorMessage}<br/>"));
                        throwECNException(string.IsNullOrWhiteSpace(errorString)
                                ? "Content is missing required closing tags"
                                : errorString,
                            bubble);
                    }
                }
            }
        }

        private int GetLayoutId()
        {
            if (rbNewLayout.Checked && string.IsNullOrWhiteSpace(HiddenSelectedLayoutID.Value))
            {
                var layoutId = layoutEditor1.SaveLayout();
                HiddenSelectedLayoutID.Value = layoutId.ToString();
                layoutExplorer1.selectedLayoutID = layoutId;
                return layoutId;
            }

            return layoutExplorer1.selectedLayoutID;
        }

        private string CleanAndStripHTML(string dirty)
        {
            var htmlStrip = new Regex("<.*?>");
            var retString = htmlStrip.Replace(dirty, string.Empty);
            retString = retString.Replace("&gt;", ">");
            retString = retString.Replace("&lt;", "<");

            return retString;
        }


        private void setDynamicPersonalizationFieldsVisibility(bool status)
        {
            dyanmicFieldsLbl.Visible = status;
            dyanmicEmailFrom.Visible = status;
            dyanmicReplyToEmail.Visible = status;
            dyanmicEmailFromName.Visible = status;
            dyanmicEmailFromLbl.Visible = status;
            dyanmicReplyToEmailLbl.Visible = status;
            dyanmicEmailFromNameLbl.Visible = status;
        }

        protected void rbNewLayout_CheckedChanged(object sender, System.EventArgs e)
        {
            plNewLayout.Visible = true;
            plExistingLayout.Visible = false;
            HiddenSelectedLayoutID.Value = string.Empty;
        }

        protected void rbExistingLayout_CheckedChanged(object sender, System.EventArgs e)
        {
            plNewLayout.Visible = false;
            plExistingLayout.Visible = true;
        }

    }
}