using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KM.Common.Entity;
using EmailDirect = ECN_Framework_Entities.Communicator.EmailDirect;
using LandingPageAssignContent = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent;

namespace ecn.activityengines.engines
{
    public partial class UpdateEmailAddress : System.Web.UI.Page
    {
        private const string OldEmailNotExistsMessage = "Old email address doesn't exist";
        private const string NewEmailDoNotMatch = "New email addresses don't match";
        private const string NewEmailNotValid = "New Email address is not a valid email";
        private const string OldEmailNotValid = "Old Email address is not a valid email";
        private const string SubmitClickMethodName = "btnSubmit_Click";
        private const string AppSettingKmApplication = "KMCommon_Application";
        private const string AppSettingActivityDomainPath = "Activity_DomainPath";
        private const string EmailDirectFromName = "Activity Engine";
        private const string EmailDirectProcess = "Activity Engine - UpdateEmailAddress.btnSubmit_click";
        private const string EmailDirectSource = "Activity Engine  - UpdateEmailAddress";
        private const string OldEmailParam = "oldemail";
        private const string NewEmailParam = "newemail";
        private const string BcidParam = "bcid";
        private const string ConfirmChangeMessage = "Please confirm this change by clicking";
        private const string HtmlBr = "</BR>";
        private const int LandingPageOptionId24 = 24;
        private const int LandingPageOptionId25 = 25;
        private const int LandingPageOptionId26 = 26;
        private const int LandingPageOptionId27 = 27;
        private const int LandingPageOptionId28 = 28;
        private const int LandingPageOptionId29 = 29;
        private const int LandingPageId5 = 5;

        private int PageID
        {
            get
            {
                if (Request.QueryString["LPAID"] != null)
                    return Convert.ToInt32(Request.QueryString["LPAID"].ToString());
                else
                    return -1;
            }
        }

        private int BCID
        {
            get
            {
                if (Request.QueryString["bcid"] != null)
                    return Convert.ToInt32(Request.QueryString["bcid"].ToString());
                else
                    return -1;
            }
        }

        private int _Preview
        {
            get
            {
                if (Request.QueryString["preview"] != null)
                {
                    int previewID = Convert.ToInt32(Request.QueryString["preview"].ToString());
                    Preview = previewID;
                    return previewID;

                }
                else
                    return -1;
            }
        }

        private int Preview = 0;
        private int CustomerID = 0;
        private int BaseChannelID = 0;
        private string newEmail = "";
        private string oldEmail = "";
        string Decrypted = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ECN_Framework_Entities.Accounts.LandingPageAssign lpa = new ECN_Framework_Entities.Accounts.LandingPageAssign();
                if (BCID > 0)
                {
                    lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(BCID).FirstOrDefault(x => x.LPID == 5);
                    LoadPage(lpa);
                }
                else if(_Preview > 0)
                {
                    lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByLPAID(Preview, true);
                    LoadPage(lpa);
                }
                else
                {

                    if (Request.Url.Query.ToString().Length > 0)
                    {
                        Decrypted = Helper.DeCrypt_DeCode_EncryptedQueryString(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
                    }
                    if (Decrypted != string.Empty)
                    {
                        GetValuesFromQuerystring(Decrypted);
                    }

                    if (!string.IsNullOrEmpty(oldEmail) && !string.IsNullOrEmpty(newEmail) && BaseChannelID > 0)
                    {
                        lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(BaseChannelID).FirstOrDefault(x => x.LPID == 5);
                        lpa = LoadPage(lpa);
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.Email.UpdateEmail_BaseChannel(oldEmail, newEmail, BaseChannelID, lpa.CreatedUserID.Value, "ecn.activityEngines.UpdateEmailAddress");
                        }
                        catch (ECNException ecn)
                        {
                            setECNError(ecn);
                            pnlMain.Visible = false;
                            pnlConfirmation.Visible = false;

                            return;
                        }
                        catch (Exception ex)
                        {
                            pnlMain.Visible = false;
                            pnlConfirmation.Visible = false;
                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "UpdateEmailAddress.Page_Load.UpdateEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                            throwECNException(ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError));
                            return;
                        }

                        litConfirmation.Text = lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 30) != null ? lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 30).Display : "";
                        pnlConfirmation.Visible = true;
                        pnlMain.Visible = false;
                    }
                    else if(!string.IsNullOrEmpty(oldEmail) && !string.IsNullOrEmpty(newEmail) && CustomerID > 0)
                    {
                        ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);

                        lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(c.BaseChannelID.Value).FirstOrDefault(x => x.LPID == 5);
                        lpa = LoadPage(lpa);

                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.Email.UpdateEmail_Customer(oldEmail, newEmail, CustomerID, lpa.CreatedUserID.Value,"ecn.activityengines.UpdateEmailAddress");
                        }
                        catch (ECNException ecn)
                        {
                            setECNError(ecn);
                            pnlMain.Visible = false;
                            pnlConfirmation.Visible = false;

                            return;
                        }
                        catch (Exception ex)
                        {
                            pnlMain.Visible = false;
                            pnlConfirmation.Visible = false;
                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "UpdateEmailAddress.Page_Load.UpdateEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                            throwECNException(ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError));
                            return;
                        }
                        litConfirmation.Text = lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 30) != null ? lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 30).Display : "";
                        pnlConfirmation.Visible = true;
                        pnlMain.Visible = false;
                    }
                    else
                    {
                        lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetDefault().FirstOrDefault(x => x.LPID == 5);
                        LoadPage(lpa);
                        pnlConfirmation.Visible = false;
                        pnlMain.Visible = false;
                        throwECNException(ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink));
                        return;
                    }
                }
            }
        }

        private ECN_Framework_Entities.Accounts.LandingPageAssign LoadPage(ECN_Framework_Entities.Accounts.LandingPageAssign lpa)
        {

            if (lpa == null || lpa.LPAID <= 0)
            {
                try
                {
                    lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(BCID).FirstOrDefault(x => x.LPID == 5);//.GetByLPAID(PageID, true);
                }
                catch { }
                if (lpa == null || (lpa.BaseChannelDoesOverride.HasValue && lpa.BaseChannelDoesOverride.Value))
                {
                    lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetDefault().FirstOrDefault(x => x.LPID == 5);
                    lpa.AssignContentList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(lpa.LPAID);
                }
                else
                {
                    lpa.AssignContentList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(lpa.LPAID);
                }


            }
            else if(!lpa.BaseChannelDoesOverride.HasValue || (lpa.BaseChannelDoesOverride.HasValue && !lpa.BaseChannelDoesOverride.Value))
            {
                lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetDefault().FirstOrDefault(x => x.LPID == 5);
                lpa.AssignContentList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(lpa.LPAID);
            }
            else if (lpa.AssignContentList == null)
            {
                lpa.AssignContentList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(lpa.LPAID);
            }
            SetHeaderFooter(lpa);
            AssignContent(lpa.AssignContentList);
            return lpa;
        }

        private void GetValuesFromQuerystring(string queryString)
        {

            string cleanQS = Server.UrlDecode(QSCleanUP(queryString));
            ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(cleanQS);
            try
            {
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.BaseChannelID).ParameterValue, out BaseChannelID);
            }
            catch { }
            try
            {
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Preview).ParameterValue, out Preview);
            }
            catch { }

            try
            {
                oldEmail = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.OldEmail).ParameterValue;
            }
            catch { }
            try
            {
                newEmail = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.NewEmail).ParameterValue;
            }
            catch { }
            try
            {
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.CustomerID).ParameterValue, out CustomerID);
            }
            catch { }


        }

        private string QSCleanUP(string querystring)
        {
            try
            {
                querystring = querystring.Replace("&amp;", "&");
                querystring = querystring.Replace("&lt;", "<");
                querystring = querystring.Replace("&gt;", ">");
            }
            catch (Exception)
            {
            }

            return querystring.Trim();
        }

        private void SetHeaderFooter(ECN_Framework_Entities.Accounts.LandingPageAssign LPA)
        {
            //if it's a preview just show them the one they are requesting
            if (Preview > 0)
                LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByLPAID(Preview, true);

            Page.Title = "Update Email Address";

            Label lblHeader = Master.FindControl("lblHeader") as Label;
            lblHeader.Text = LPA.Header;

            Label lblFooter = Master.FindControl("lblFooter") as Label;
            lblFooter.Text = LPA.Footer;
        }

        private void AssignContent(List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> content)
        {
            if (content != null && content.Count > 0)
            {
                if (content.Exists(x => x.LPOID == 18))
                {
                    lblMessageText.Text = content.FirstOrDefault(x => x.LPOID == 18).Display;

                }

                if (content.Exists(x => x.LPOID == 19))
                {
                    lblOldEmail.Text = content.FirstOrDefault(x => x.LPOID == 19).Display;

                }

                if (content.Exists(x => x.LPOID == 20))
                {
                    lblNewEmail.Text = content.FirstOrDefault(x => x.LPOID == 20).Display;
                }

                if (content.Exists(x => x.LPOID == 21))
                {
                    btnSubmit.Text = content.FirstOrDefault(x => x.LPOID == 21).Display;

                }

                if (content.Exists(x => x.LPOID == 22) && content.Find(x => x.LPOID == 22).Display.ToLower().Equals("true"))
                {

                    //This is reentry text box
                    if (content.Exists(x => x.LPOID == 23) && !string.IsNullOrEmpty(content.Find(x => x.LPOID == 23).Display))
                    {
                        pnlReEnter.Visible = true;
                        lblReEnter.Text = content.FirstOrDefault(x => x.LPOID == 23).Display;
                        rfvReEnter.Enabled = true;
                    }
                }
                else
                {
                    pnlReEnter.Visible = false;
                    rfvReEnter.Enabled = false;
                }

                if (content.Exists(x => x.LPOID == 24))
                {
                    litConfirmation.Text = content.FirstOrDefault(x => x.LPOID == 24).Display;
                }


            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var landingPageAssign = GetLandingPageAssign();
                if (landingPageAssign == null)
                {
                    throw new ArgumentNullException(nameof(landingPageAssign));
                }

                if (Preview <= 0 && BCID > 0)
                {
                    if (Email.IsValidEmailAddress(txtOldEmail.Text.Trim()))
                    {
                        if (!Email.Exists_BaseChannel(txtOldEmail.Text.Trim(), BCID))
                        {
                            throwECNException(OldEmailNotExistsMessage);
                            return;
                        }

                        if (Email.IsValidEmailAddress(txtNewEmail.Text.Trim()))
                        {
                            if (pnlReEnter.Visible)
                            {
                                if (!txtNewEmail.Text.Trim().Equals(txtReEnter.Text.Trim()))
                                {
                                    throwECNException(NewEmailDoNotMatch);
                                    return;
                                }
                            }

                            if (isEmailSuppressed(txtNewEmail.Text.Trim()))
                            {
                                return;
                            }

                            //Start the process
                            try
                            {
                                if (BCID > 0)
                                {
                                    if (!SaveEmailDirect(landingPageAssign))
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    throwECNException(ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError));
                                }
                            }
                            catch (ECNException ecn)
                            {
                                setECNError(ecn);
                                return;
                            }
                            catch (Exception exception)
                            {
                                HandleExceptionForSubmitClick(true, exception);
                                return;
                            }
                        }
                        else
                        {
                            throwECNException(NewEmailNotValid);
                            return;
                        }
                    }
                    else
                    {
                        throwECNException(OldEmailNotValid);
                        return;
                    }
                }
                ConfimationAfterSubmit(landingPageAssign);
            }
            catch (Exception exception)
            {
                HandleExceptionForSubmitClick(true, exception);
            }
        }

        private LandingPageAssign GetLandingPageAssign()
        {
            var landingPageAssign = new LandingPageAssign();
            if (Preview <= 0)
            {
                landingPageAssign = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(BCID)
                    .FirstOrDefault(pageAssign => pageAssign.LPID == LandingPageId5);

                if (landingPageAssign == null ||
                    (landingPageAssign != null &&
                     (!landingPageAssign.BaseChannelDoesOverride.HasValue ||
                      (landingPageAssign.BaseChannelDoesOverride.HasValue &&
                       !landingPageAssign.BaseChannelDoesOverride.Value))))
                {
                    landingPageAssign = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetDefault()
                        .Find(pageAssign => pageAssign.LPID == LandingPageId5);
                }
            }
            else
            {
                landingPageAssign = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByLPAID(Preview, false);
            }

            landingPageAssign.AssignContentList = LandingPageAssignContent.GetByLPAID(landingPageAssign.LPAID);
            return landingPageAssign;
        }

        private bool SaveEmailDirect(LandingPageAssign landingPageAssign)
        {
            var stringBuilder = new StringBuilder();
            if (landingPageAssign.AssignContentList.Exists(content => content.LPOID == LandingPageOptionId25))
            {
                stringBuilder.AppendLine(
                    landingPageAssign.AssignContentList.
                        FirstOrDefault(content => content.LPOID == LandingPageOptionId25)?.Display);
            }
            if (landingPageAssign.AssignContentList.Exists(content => content.LPOID == LandingPageOptionId27))
            {
                stringBuilder.AppendLine(
                    landingPageAssign.AssignContentList.
                        FirstOrDefault(content => content.LPOID == LandingPageOptionId27)?.Display);
            }

            var currentByApplicationId = Encryption.GetCurrentByApplicationID(
                Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]));

            var queryString = string.Concat(
                $"{OldEmailParam}={txtOldEmail.Text.Trim()}&",
                $"{NewEmailParam}={txtNewEmail.Text.Trim()}&",
                $"{BcidParam}={BCID}");

            var encryptedQuery = HttpUtility.UrlEncode(
                KM.Common.Encryption.Encrypt(queryString, currentByApplicationId));

            stringBuilder.AppendLine(string.Concat($"{HtmlBr}{ConfirmChangeMessage} <a href=\"",
                                     $"{ConfigurationManager.AppSettings[AppSettingActivityDomainPath]}",
                                     $"/engines/UpdateEmailAddress.aspx?{encryptedQuery}\">here</a>"));

            if (landingPageAssign.AssignContentList.Exists(content => content.LPOID == LandingPageOptionId26))
            {
                stringBuilder.AppendLine(
                    landingPageAssign.AssignContentList.
                        FirstOrDefault(content => content.LPOID == LandingPageOptionId26)?.Display);
            }

            if (landingPageAssign.CreatedUserID == null)
            {
                throw new ArgumentNullException(nameof(landingPageAssign.CreatedUserID));
            }

            var emailDirect = new EmailDirect
            {
                CustomerID = CustomerID,
                FromName = EmailDirectFromName,
                Process = EmailDirectProcess,
                Source = EmailDirectSource,
                CreatedUserID = landingPageAssign.CreatedUserID.Value,
                Content = stringBuilder.ToString(),
                ReplyEmailAddress = landingPageAssign.AssignContentList
                    .FirstOrDefault(content => content.LPOID == LandingPageOptionId28)?.Display,
                EmailAddress = txtOldEmail.Text,
                EmailSubject = landingPageAssign.AssignContentList.
                    FirstOrDefault(content => content.LPOID == LandingPageOptionId29)?.Display
            };

            try
            {
                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(emailDirect);
                pnlConfirmation.Visible = true;
                pnlMain.Visible = false;
                phError.Visible = false;
            }
            catch
            {
                HandleExceptionForSubmitClick(false);
                return false;
            }
            return true;
        }
        
        private void ConfimationAfterSubmit(LandingPageAssign landingPageAssign)
        {
            if (landingPageAssign.AssignContentList.Exists(content => content.LPOID == LandingPageOptionId24))
            {
                litConfirmation.Text = landingPageAssign.AssignContentList.
                    FirstOrDefault(content => content.LPOID == LandingPageOptionId24)?.Display;
                pnlConfirmation.Visible = true;
                pnlMain.Visible = false;
                phError.Visible = false;
            }
        }

        private void HandleExceptionForSubmitClick(bool isLogCriticalError, Exception exception = null)
        {
            pnlConfirmation.Visible = false;
            pnlMain.Visible = false;
            if (isLogCriticalError)
            {
                ApplicationLog.LogCriticalError(
                    exception,
                    SubmitClickMethodName,
                    Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]));
            }
            throwECNException(ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError));
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(ECN_Framework_Common.Objects.Enums.Entity.LandingPage, ECN_Framework_Common.Objects.Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList));
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

        private bool isEmailSuppressed(string email)
        {
            if (CustomerID > 0)
            {
                ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup_NoAccessCheck(CustomerID);
                ECN_Framework_Entities.Communicator.EmailGroup msEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(email, MSGroup.GroupID);
                if (msEmailGroup != null && msEmailGroup.CustomerID.HasValue)
                {
                    throwECNException("New email address is Master Suppressed. Updating is not allowed.");
                    return true;
                }
            }

            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(BCID, email.Replace("'", "''"), new KMPlatform.Entity.User());
            if (channelMasterSuppressionList_List.Count > 0)
            {
                throwECNException("New email address is Channel Master Suppressed. Updating is not allowed.");
                return true;
            }

            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(email.Replace("'", "''"), new KMPlatform.Entity.User());
            if (globalMasterSuppressionList_List.Count > 0)
            {
                throwECNException("New email address is Global Master Suppressed. Updating is not allowed.");
                return true;
            }

            return false;
        }
    }
}