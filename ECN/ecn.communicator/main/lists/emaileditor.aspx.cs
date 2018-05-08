using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using BusinessGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using BusinessEmail = ECN_Framework_BusinessLayer.Communicator.Email;
using Enums = ECN_Framework_Common.Objects.Enums;
using StringFunctions = ECN_Framework_Common.Functions.StringFunctions;

namespace ecn.communicator.listsmanager
{
    public partial class emaileditor : ECN_Framework.WebPageHelper
    {
        private const string DateTimePattern = "MM/dd/yyyy";
        private const string EmailRemovedTemplate = 
            "EmailID: {0} has been removed from groupId: {1}<br/>" + "Due to this there is nothing to display.";
        private const string UpdateEmailFullName = "Ecn.communicator.main.lists.emaileditor.updateEmail";
        private const string ScriptKeyMergeProfile = "MergeProfile";
        private const string AlertCannotUpsertEmail = 
            "alert('You cannot create or update an email profile with an empty email address')";
        private const string ScriptTemplate =
            "if(confirm('This email address already exists for this Customer Account. Would you like to merge these two profiles?')){{ window.location='mergeProfiles.aspx?oldemailid={0}&newemailid={1}';}}";
        private const string GroupEditorTemplate = "groupeditor.aspx?groupID={0}";
        private const string ErrorCouldntParseEmailId = "Couldn't parse Email Id";

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "";
            Master.Heading = "Groups > Manage Groups > Group Editor > Edit Profile Record";
            Master.HelpContent = "<b>Viewing/Editing Email Profiles (including adding notes to profile)</b><div id='par1'><ul><li>Edit information within the fields as needed; click <em>Update</em> when finished.</li>&#13;&#10;<li>To enter or view notes on this subscriber, click the <em>View Notes</em> button.</li><li>To see the history of what messages have been sent to this subscriber, click the <em>View Logs</em> button. To see what message was sent, click on the blast &#13;&#10;title and a preview will appear.</li><li>To view the number of opens or pages the person has clicked on, click the <em>Profile Manager</em>, then click on <em>Email Opens Activity</em> or <em>Email Clicks Activity.</em></li></ul></div>&#13;&#10;&#13;&#10;";
            Master.HelpTitle = "Groups Manager";

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "grouppriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))

            if (KMPlatform.BusinessLogic.Client.HasServiceFeature(Master.UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email))
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit))
                {


                    if (Page.IsPostBack == false)
                    {
                        int requestEmailID = getEmailID();

                        LoadDropDowns();

                        if (requestEmailID > 0)
                        {
                            LoadFormData(requestEmailID, getGroupID());
                            LoadLog(requestEmailID);
                            if (getGroupID() == 0)
                            {
                                UserDefLink.NavigateUrl += "?EmailID=" + requestEmailID.ToString();
                            }
                            else
                            {
                                UserDefLink.NavigateUrl += "?EmailID=" + requestEmailID.ToString() + "&groupId=" + getGroupID().ToString();
                                FormatPanel.Visible = true;
                            }
                            UserDefLink.Visible = true;

                        }
                    }

                    string profileURL = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailProfileManager.aspx?action=edit&ead=" + this.EmailAddress.Text.ToString() + "&eid=" + getEmailID() + "&gid=" + getGroupID() + "&cid=" + Master.UserSession.CurrentCustomer.CustomerID;
                    ProfileManagerButton.Attributes.Add("Onclick", "return popManagerWindow('" + profileURL + "');");
                }
            
                else
                {
                  throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }

            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public int getEmailID()
        {
            int theEmailID = 0;
            if (Request.QueryString["EmailID"] != null)
            {
                Int32.TryParse(Request.QueryString["EmailID"].ToString(), out theEmailID);
                //theEmailID = Convert.ToInt32(Request.QueryString["EmailID"].ToString());
            }
            return theEmailID;
        }
        public int getGroupID()
        {
            int theGroupID = 0;
            if (Request.QueryString["groupId"] != null)
            {
                Int32.TryParse(Request.QueryString["groupId"].ToString(), out theGroupID);
            }
            return theGroupID;
        }

        #region Form Prep
        private void LoadDropDowns()
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(getGroupID(), Master.UserSession.CurrentUser);
            if (group != null)
            {
                if (group.MasterSupression == null || group.MasterSupression.Value == 0)
                {
                    SubscribeTypeCode.Items.Clear();
                    SubscribeTypeCode.Items.Add(new ListItem("Subscribes", "S"));
                    SubscribeTypeCode.Items.Add(new ListItem("UnSubscribes", "U"));
                    SubscribeTypeCode.Items.Add(new ListItem("Master Suppressed", "M"));
                    SubscribeTypeCode.Items.Add(new ListItem("Marked as Bad Records", "D"));
                    SubscribeTypeCode.Items.Add(new ListItem("Pending Subscribes", "P"));
                }
                else
                {
                    SubscribeTypeCode.Items.Clear();
                    SubscribeTypeCode.Items.Add(new ListItem("UnSubscribes", "U"));
                    SubscribeTypeCode.Items.Add(new ListItem("Bounce", "B"));
                    SubscribeTypeCode.Items.Add(new ListItem("Abuse Complaint", "A"));
                    SubscribeTypeCode.Items.Add(new ListItem("Manual Upload", "M"));
                    SubscribeTypeCode.Items.Add(new ListItem("Feedback Loop(or Spam Complaint)", "F"));
                    SubscribeTypeCode.Items.Add(new ListItem("Email Address Change", "E"));
                    SubscribeTypeCode.Items.Add(new ListItem("Unknown User", "?"));
                }
            }


            List<ECN_Framework_Entities.Accounts.Code> codeList = ECN_Framework_BusinessLayer.Accounts.Code.GetAll();
            //List<ECN_Framework_Entities.Accounts.Code> codeListSubscribeType = (from src in codeList
            //                                                                    where src.CodeType == "SubscribeType"
            //                                                                    select src).ToList();
            //SubscribeTypeCode.DataSource = codeListSubscribeType;
            //SubscribeTypeCode.DataBind();

            List<ECN_Framework_Entities.Accounts.Code> codeListFormatType = (from src in codeList
                                                                             where src.CodeType == "FormatType"
                                                                             select src).ToList();
            FormatTypeCode.DataSource = codeListFormatType;
            FormatTypeCode.DataBind();
        }
        #endregion

        #region Data Load

        private void LoadLog(int setEmailID)
        {
            List<ECN_Framework_Entities.Activity.View.BlastActivity> blastActivity = new List<ECN_Framework_Entities.Activity.View.BlastActivity>();
            if (setEmailID > 0)
            {
                blastActivity = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetByEmailID(setEmailID, Master.UserSession.CurrentUser);
                foreach(ECN_Framework_Entities.Activity.View.BlastActivity ba in blastActivity)
                {
                    ba.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(ba.EmailSubject);
                }
                LogGrid.DataSource = blastActivity;
                LogGrid.DataBind();
                LogPager.RecordCount = blastActivity.Count;
            }
            
        }

        private void LoadNotes(int setEmailID)
        {
            if (setEmailID > 0)
            {
                ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(setEmailID, Master.UserSession.CurrentUser);
                NotesBox.Text = email.Notes;
                EmailID.Text = setEmailID.ToString();
            }
        }

        private void LoadFormData(int setEmailId, int groupId)
        {
            var email = BusinessEmail.GetByEmailIDGroupID(setEmailId, groupId, Master.UserSession.CurrentUser);
            if (email != null)
            {
                LoadFormDataInitTextControls(setEmailId, email);
                var userEvent1Dt = email.UserEvent1Date.ToString();
                if (!Convert.IsDBNull(userEvent1Dt) && userEvent1Dt.Length > 0)
                {
                    DateTime dtEvent;
                    UserEvent1Date.Text = DateTime.TryParse(userEvent1Dt, out dtEvent)
                        ? dtEvent.ToString(DateTimePattern)
                        : string.Empty;
                }

                var userEvent2Dt = email.UserEvent2Date.ToString();
                if (!Convert.IsDBNull(userEvent2Dt) && userEvent2Dt.Length > 0)
                {
                    DateTime dtEvent;
                    UserEvent2Date.Text = DateTime.TryParse(userEvent2Dt, out dtEvent)
                        ? dtEvent.ToString(DateTimePattern)
                        : string.Empty;
                }

                var birthDt = email.Birthdate.ToString();
                if (!Convert.IsDBNull(birthDt))
                {
                    DateTime dtEvent;
                    if (DateTime.TryParse(birthDt, out dtEvent))
                    {
                        BirthDate.Text = dtEvent.ToString(DateTimePattern);
                    }
                }

                FormatTypeCode.ClearSelection();
                SubscribeTypeCode.ClearSelection();

                FormatTypeCode.Items.FindByValue(email.FormatTypeCode.ToLower()).Selected = true;
                SubscribeTypeCode.Items.FindByValue(email.SubscribeTypeCode.ToUpper()).Selected = true;
                var businessGroup = BusinessGroup.GetByGroupID(getGroupID(), Master.UserSession.CurrentUser);
                if (businessGroup != null)
                {
                    if (businessGroup.MasterSupression == null || businessGroup.MasterSupression.Value == 0)
                    {
                        SubscribeTypeCode.Enabled = true;
                    }
                    else
                    {
                        SubscribeTypeCode.Enabled = false;
                    }
                }
            }
            else
            {
                phError.Visible = true;
                lblErrorMessage.Text = string.Format(EmailRemovedTemplate, setEmailId, groupId);
            }
        }

        private void LoadFormDataInitTextControls(int setEmailId, Email email)
        {
            Guard.NotNull(email, nameof(email));

            EmailID.Text = setEmailId.ToString();
            EmailAddress.Text = email.EmailAddress;
            Password.Text = email.Password;
            txtTitle.Text = email.Title;
            FirstName.Text = email.FirstName;
            LastName.Text = email.LastName;
            FullName.Text = email.FullName;
            CompanyName.Text = email.Company;
            Occupation.Text = email.Occupation;
            Address.Text = email.Address;
            Address2.Text = email.Address2;
            City.Text = email.City;
            State.Text = email.State;
            Zip.Text = email.Zip;
            Country.Text = email.Country;
            Voice.Text = email.Voice;
            Mobile.Text = email.Mobile;
            Fax.Text = email.Fax;
            Website.Text = email.Website;
            Age.Text = email.Age;
            Income.Text = email.Income;
            Gender.Text = email.Gender;
            User1.Text = email.User1;
            User2.Text = email.User2;
            User3.Text = email.User3;
            User4.Text = email.User4;
            User5.Text = email.User5;
            User6.Text = email.User6;
            UserEvent1.Text = email.UserEvent1;
            UserEvent2.Text = email.UserEvent2;
            BounceScore.Text =
                email.BounceScore.HasValue.Equals(true) ? email.BounceScore.Value.ToString() : string.Empty;
            txtSoftBounceScore.Text =
                email.SoftBounceScore.HasValue.Equals(true) ? email.SoftBounceScore.Value.ToString() : string.Empty;
        }

        #endregion

        #region Data Handlers
        public void UpdateNotes(object sender, System.EventArgs e)
        {
            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(Convert.ToInt32(EmailID.Text), Master.UserSession.CurrentUser);
            email.Notes = StringFunctions.CleanString(NotesBox.Text);
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Email.Save(Master.UserSession.CurrentUser, email, getGroupID(), "Ecn.communicator.main.lists.emaildataeditor.updateNotes");
                ViewDetails(sender, e);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }


        }

        public void UpdateEmail(object sender, EventArgs e)
        {
            var email = UpdateEmailFillObject();

            if (email.EmailAddress.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    string.Empty,
                    AlertCannotUpsertEmail,
                    true);
            }
            else
            {
                UpdateEmailSave(email);
            }
        }

        private void UpdateEmailSave(Email email)
        {
            int emailId;
            if (!int.TryParse(EmailID.Text, out emailId))
            {
                throw new InvalidOperationException(ErrorCouldntParseEmailId);
            }
            var exists = BusinessEmail.Exists(email.EmailAddress, Master.UserSession.CurrentUser.CustomerID, emailId);

            if (exists)
            {
                var newEmail = BusinessEmail.GetByEmailAddress(
                    email.EmailAddress,
                    Master.UserSession.CurrentUser.CustomerID,
                    emailId,
                    Master.UserSession.CurrentUser);

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    ScriptKeyMergeProfile,
                    string.Format(ScriptTemplate, EmailID.Text, newEmail.EmailID),
                    true);
            }
            else
            {
                if (!isEmailSuppressed(email.EmailAddress))
                {
                    int bounceScore;
                    int.TryParse(BounceScore.Text.Trim(), out bounceScore);
                    email.BounceScore = bounceScore;

                    int softBounceScore;
                    int.TryParse(txtSoftBounceScore.Text.Trim(), out softBounceScore);
                    email.SoftBounceScore = softBounceScore;

                    try
                    {
                        BusinessEmail.Save(
                            Master.UserSession.CurrentUser,
                            email,
                            getGroupID(),
                            UpdateEmailFullName);
                        Response.Redirect(string.Format(GroupEditorTemplate, getGroupID()));
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                    }
                }
            }
        }

        private Email UpdateEmailFillObject()
        {
            var emailObj = new Email();

            emailObj.EmailAddress = StringFunctions.CleanString(EmailAddress.Text);
            emailObj.Title = StringFunctions.CleanString(txtTitle.Text);
            emailObj.FirstName = StringFunctions.CleanString(FirstName.Text);
            emailObj.LastName = StringFunctions.CleanString(LastName.Text);
            emailObj.FullName = StringFunctions.CleanString(FullName.Text);
            emailObj.Company = StringFunctions.CleanString(CompanyName.Text);
            emailObj.Occupation = StringFunctions.CleanString(Occupation.Text);
            emailObj.Address = StringFunctions.CleanString(Address.Text);
            emailObj.Address2 = StringFunctions.CleanString(Address2.Text);
            emailObj.City = StringFunctions.CleanString(City.Text);
            emailObj.State = StringFunctions.CleanString(State.Text);
            emailObj.Zip = StringFunctions.CleanString(Zip.Text);
            emailObj.Country = StringFunctions.CleanString(Country.Text);
            emailObj.Voice = StringFunctions.CleanString(Voice.Text);
            emailObj.Mobile = StringFunctions.CleanString(Mobile.Text);
            emailObj.Fax = StringFunctions.CleanString(Fax.Text);
            emailObj.Website = StringFunctions.CleanString(Website.Text);
            emailObj.Age = StringFunctions.CleanString(Age.Text);
            emailObj.Income = StringFunctions.CleanString(Income.Text);
            emailObj.Gender = StringFunctions.CleanString(Gender.Text);
            emailObj.User1 = StringFunctions.CleanString(User1.Text);
            emailObj.User2 = StringFunctions.CleanString(User2.Text);
            emailObj.User3 = StringFunctions.CleanString(User3.Text);
            emailObj.User4 = StringFunctions.CleanString(User4.Text);
            emailObj.User5 = StringFunctions.CleanString(User5.Text);
            emailObj.User6 = StringFunctions.CleanString(User6.Text);
            emailObj.UserEvent1 = StringFunctions.CleanString(UserEvent1.Text);
            emailObj.UserEvent2 = StringFunctions.CleanString(UserEvent2.Text);
            emailObj.Password = StringFunctions.CleanString(Password.Text);

            DateTime parsedDate;

            emailObj.Birthdate =
                DateTime.TryParse(BirthDate.Text, out parsedDate) ? parsedDate : (DateTime?) null;

            emailObj.UserEvent1Date =
                DateTime.TryParse(UserEvent1Date.Text, out parsedDate) ? parsedDate : (DateTime?) null;

            emailObj.UserEvent2Date =
                DateTime.TryParse(UserEvent2Date.Text, out parsedDate) ? parsedDate : (DateTime?) null;

            emailObj.FormatTypeCode = FormatTypeCode.SelectedItem.Value;
            emailObj.SubscribeTypeCode = SubscribeTypeCode.SelectedItem.Value;
            return emailObj;
        }

        #endregion

        public void ViewDetails(object sender, System.EventArgs e)
        {
            int theEmailID = -1;
            int.TryParse(EmailID.Text, out theEmailID);
            LoadFormData(theEmailID, getGroupID());
            DetailsPanel.Visible = true;
            NotesPanel.Visible = false;
            LogPanel.Visible = false;
            DetailsButton.Enabled = false;
            NotesButton.Enabled = true;
            LogButton.Enabled = true;
        }

        public void ViewNotes(object sender, System.EventArgs e)
        {
            int theEmailID = -1;
            int.TryParse(EmailID.Text, out theEmailID);
            LoadNotes(theEmailID);
            DetailsPanel.Visible = false;
            NotesPanel.Visible = true;
            LogPanel.Visible = false;
            DetailsButton.Enabled = true;
            NotesButton.Enabled = false;
            LogButton.Enabled = true;
        }

        public void ViewLog(object sender, System.EventArgs e)
        {
            int theEmailID = -1;
            int.TryParse(EmailID.Text, out theEmailID);
            LoadLog(theEmailID);
            DetailsPanel.Visible = false;
            NotesPanel.Visible = false;
            LogPanel.Visible = true;
            DetailsButton.Enabled = true;
            NotesButton.Enabled = true;
            LogButton.Enabled = false;
        }

        protected void LogPager_IndexChanged(object sender, System.EventArgs e)
        {
            int theEmailID = Convert.ToInt32(EmailID.Text);
            LoadLog(theEmailID);
        }

        protected void LogGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            ECN_Framework_Entities.Activity.View.BlastActivity ba = new ECN_Framework_Entities.Activity.View.BlastActivity();
            ba = (ECN_Framework_Entities.Activity.View.BlastActivity)e.Item.DataItem;
            if (ba != null)
            {
                Label lblActionTypeCode = (Label)e.Item.FindControl("lblActionTypeCode");
                if (ba.ActionTypeCode.ToLower().Equals("subscribe") && lblActionTypeCode != null)
                    lblActionTypeCode.Text = "unsubscribe";
                else if (lblActionTypeCode != null)
                    lblActionTypeCode.Text = ba.ActionTypeCode;

            }
        }

        private void throwECNException(string message)
        {
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(new ECNError(Enums.Entity.Email, Enums.Method.Validate, message));
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private bool isEmailSuppressed(string email)
        {
            ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            ECN_Framework_Entities.Communicator.EmailGroup msEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(email, MSGroup.GroupID, Master.UserSession.CurrentUser);
            if (msEmailGroup != null && msEmailGroup.CustomerID.HasValue)
            {
                throwECNException("New email address is Master Suppressed. Updating is not allowed.");
                return true;
            }

            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(Master.UserSession.CurrentBaseChannel.BaseChannelID, email.Replace("'", "''"), Master.UserSession.CurrentUser);
            if (channelMasterSuppressionList_List.Count > 0)
            {
                throwECNException("New email address is Channel Master Suppressed. Updating is not allowed.");
                return true;
            }

            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(email.Replace("'", "''"), Master.UserSession.CurrentUser);
            if (globalMasterSuppressionList_List.Count > 0)
            {
                throwECNException("New email address is Global Master Suppressed. Updating is not allowed.");
                return true;
            }

            return false;
        }
    }
}
