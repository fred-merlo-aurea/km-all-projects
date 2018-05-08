using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace ecn.activityengines.engines
{
    public partial class Unsubscribe : System.Web.UI.Page
    {
        #region Private Properties
        private int CustomerID = 0;
        private int GroupID = 0;
        private int BlastID = 0;
        private int RefBlastID = 0;
        private int Preview = 0;

        private string EmailAddress = string.Empty;
        private string CustomerName = string.Empty;
        private string GroupName = string.Empty;
        private string GroupDescription = string.Empty;

        private KMPlatform.Entity.User User;
        private static ECN_Framework_Entities.Accounts.LandingPageAssign LPA;
        #endregion

        #region Protected Page Events
        protected override void InitializeCulture()
        {
            try
            {
                if (Request.QueryString["lang"].ToString() != "")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Request.QueryString["lang"].ToString());
                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(Request.QueryString["lang"].ToString());
                }
            }
            catch { }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (Request.Url.Query.Length > 0 && Request.Url.Query.Contains("="))
                {
                    if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                    {
                        User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                        Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        User = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                    }

                    GetValuesFromQuerystring(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));

                    if (BlastID > 0)
                        getRefBlast();

                    //make sure we have email address, group id, customer id and a valid, non-test blast id
                    if ((EmailAddress.Length > 0 && CustomerID > 0 && GroupID > 0) && ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(EmailAddress, GroupID) != null)
                    {
                        if (IsValidUnsubBlast(BlastID))
                        {
                            try
                            {
                                ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
                                if (customer != null)
                                {
                                    string redirect = string.Empty;
                                    try
                                    {
                                        redirect = ConfigurationManager.AppSettings["UnsubRedirect_" + customer.BaseChannelID.ToString()];
                                    }
                                    catch (Exception) { }
                                    if (redirect != null && redirect != string.Empty)
                                    {
                                        Response.Redirect(redirect + EmailAddress, false);
                                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    }
                                    else
                                        CustomerName = customer.CustomerName;
                                }

                                //setup page
                                if (!IsPostBack)
                                    SetupPageValues();
                            }
                            catch (TimeoutException te)
                            {
                                try
                                {
                                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(te, "Unsubscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                                    SetError(Enums.ErrorMessage.Timeout);
                                }
                                catch { }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Unsubscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                                    SetError(Enums.ErrorMessage.HardError);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        else
                        {
                            SetError(Enums.ErrorMessage.Unknown, "You cannot unsubscribe from a test email. This link will be functional in the Live email.");
                        }
                    }

                    else
                    {



                        //WGH: 10/31/2014 - Removing old logging
                        //
                        //try
                        //{
                        //KM.Common.Entity.ApplicationLog.LogNonCriticalError("EmailAddress, GroupID, CustomerID are empty or is invalid", "Unsubscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                        SetError(Enums.ErrorMessage.InvalidLink);
                        //}
                        //catch (Exception)
                        //{
                        //}

                    }
                }
                else
                {
                    //WGH: 10/31/2014 - Removing old logging
                    //KM.Common.Entity.ApplicationLog.LogNonCriticalError("Querystring appears invalid", "Unsubscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                    SetError(Enums.ErrorMessage.InvalidLink);
                }
            }
            catch (System.Web.UI.ViewStateException)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid Viewstate", "Unsubscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                SetError(Enums.ErrorMessage.HardError);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //they clicked unsubscribe, if it's a preview don't record the unsubscribe
            if (RefBlastID == 0)
            {
                //update current emailgroup SubscribeTypeCode to "U" 
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.UnsubscribeSubscribers_NoAccessCheck(GroupID, "'" + EmailAddress.Replace("'", "''") + "'");
            }
            else
            {
                try
                {
                    if (Preview <= 0)
                    {
                        LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(1, CustomerID, true);
                        //they clicked unsubscribe            
                        ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(RefBlastID, true);
                        int EmailID = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(GroupID, CustomerID, EmailAddress);
                        if ((blast.AddOptOuts_to_MS != null && blast.AddOptOuts_to_MS == true) || (cbCustomerUnsubscribe.Visible && cbCustomerUnsubscribe.Checked))
                        {
                            //update current emailgroup SubscribeTypeCode to "U" 
                            ECN_Framework_BusinessLayer.Communicator.EmailGroup.UnsubscribeSubscribers_NoAccessCheck(GroupID, "'" + EmailAddress.Replace("'", "''") + "'");

                            string Reason = string.Empty;
                            if (txtReason.Visible && !ddlReason.Visible)
                                Reason = ". Reason: Other " + txtReason.Text.Trim();//.Replace("'","''");
                            else if (txtReason.Visible && ddlReason.Visible)
                                Reason = ". Reason: Other " + txtReason.Text.Trim();//.Replace("'", "''");
                            else if (ddlReason.Visible && !txtReason.Visible)
                                Reason = ". Reason: " + ddlReason.SelectedItem.ToString();//.Replace("'", "''");

                            //insert an entry in the emailactivitylog that we unsubscribed to this emailgroup
                            ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.Insert(BlastID, EmailID, "subscribe", "U", "UNSUBSCRIBED THRU BLAST: " + RefBlastID.ToString() + " FOR GROUP: " + GroupID.ToString() + Reason, User);
                            //master suppress and emailactivitylog insert/update                
                            ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertSpamFeedback(BlastID, EmailID, Reason, "U", "MASTSUP_UNSUB");
                        }
                        else
                        {
                            //get list of opt out groups
                            string Groups = GroupID.ToString();
                            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(RefBlastID, true);
                            if (ci != null && ci.OptOutGroupList.Count > 0)
                            {
                                Groups = Groups + "," + string.Join(", ", from item in ci.OptOutGroupList select item.GroupID);
                            }
                            //unsubscribe to groups and emailactivitylog insert/update
                            string Reason = string.Empty;
                            if (txtReason.Visible && !ddlReason.Visible)
                                Reason = "Other " + txtReason.Text.Trim();//.Replace("'", "''");
                            else if (txtReason.Visible && ddlReason.Visible)
                                Reason = "Other " + txtReason.Text.Trim();//.Replace("'", "''");
                            else if (ddlReason.Visible && !txtReason.Visible)
                                Reason = ddlReason.SelectedItem.ToString();//.Replace("'", "''");
                            ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertOptOutFeedback(BlastID, Groups, EmailID, Reason);
                        }
                    }

                    if (LPA.AssignContentList.Exists(x => x.LPOID == 12))
                    {

                        if (LPA.AssignContentList.Exists(x => x.LPOID == 13) && LPA.AssignContentList.Exists(x => x.LPOID == 5))
                        {
                            //They have a redirect and thank you page so delay the redirect
                            phError.Visible = false;
                            pnlMain.Visible = false;
                            pnlThankYou.Visible = true;

                            string URL = LPA.AssignContentList.Find(x => x.LPOID == 12).Display;
                            int delay = 5;
                            int.TryParse(LPA.AssignContentList.Find(x => x.LPOID == 13).Display, out delay);
                            delay = delay * 1000;
                            ClientScript.RegisterStartupScript(this.GetType(), "redirect", "<script type='text/javascript'>window.setTimeout(function(){window.location.href='" + URL + "';}," + delay.ToString() + ")</script>");
                        }
                        else
                        {
                            //they have a redirect but no thank you or delay, do immediate redirect
                            Response.Redirect(LPA.AssignContentList.Find(x => x.LPOID == 12).Display);
                        }
                    }
                    else//they either only have a thank you page or neither.  Either way display thank you panel
                    {
                        //They only have a thank you page
                        phError.Visible = false;
                        pnlMain.Visible = false;
                        pnlThankYou.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Unsubscribe.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote());
                    SetError(Enums.ErrorMessage.HardError);
                }

            }
        }
        #endregion

        #region Private Methods
        private void SetError(Enums.ErrorMessage errorMessage, string pageMessage = "")
        {
            pnlMain.Visible = false;
            pnlThankYou.Visible = false;
            phError.Visible = true;
            lblErrorMessage.Text = ActivityError.GetErrorMessage(errorMessage, pageMessage);
            Label lblHeader = Master.FindControl("lblHeader") as Label;
            if (lblHeader.Text.Trim().Length == 0)
            {
                SetHeaderFooter();
            }
        }

        private void getRefBlast()
        {
            RefBlastID = BlastID;
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(BlastID, false);
            try
            {
                if (GroupID > 0)
                {
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.GroupID.Value);
                    GroupName = group.GroupName;
                    GroupDescription = group.GroupDescription;
                }



                if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                {
                    ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(EmailAddress, blast.CustomerID.Value);
                    RefBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, email.EmailID, blast.CustomerID.Value, blast.BlastType);
                    blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(RefBlastID, true);
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.GroupID.Value);
                    GroupID = blast.GroupID.Value;
                    GroupName = group.GroupName;
                    GroupDescription = group.GroupDescription;
                }
                else if (blast.GroupID.Value != GroupID)//doing this check to make sure the blastid in the url has the same groupid as the groupid in the URL JWelter 3/9/2017
                {
                    CustomerID = 0;
                    GroupID = 0;
                }

            }
            catch (Exception) { }
        }

        private void GetValuesFromQuerystring(string queryString)
        {
            try
            {
                ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(QSCleanUP(queryString)));
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.CustomerID).ParameterValue, out CustomerID);
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.GroupID).ParameterValue, out GroupID);
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.BlastID).ParameterValue, out BlastID);
                EmailAddress = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailAddress).ParameterValue;
                EmailAddress = EmailAddress.Replace(" ", "+");
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Preview).ParameterValue, out Preview);
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

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<br><b>Customer ID:</b>&nbsp;" + CustomerID);
                adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + GroupID);
                adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + BlastID);
                adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + EmailAddress);
                adminEmailVariables.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        private string ReplaceStandardFields(string input)
        {
            string output = input;
            output = output.Replace("%%groupname%%", GroupName);
            output = output.Replace("%%groupdescription%%", GroupDescription);
            output = output.Replace("%%customername%%", CustomerName);
            output = output.Replace("%%emailaddress%%", EmailAddress);

            return output;
        }

        private bool IsValidUnsubBlast(int blastID)
        {
            if (blastID == 0)
                return true;
            else
            {
                ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID, false);
                if (blast != null && blast.TestBlast.ToLower().Equals("n"))
                    return true;
                else
                    return false;
            }
        }

        private void SetHeaderFooter()
        {
            //if it's a preview just show them the one they are requesting
            if (Preview > 0)
                LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByLPAID(Preview, true);
            else
                LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(1, CustomerID, true);

            Page.Title = "Unsubscribe from Group";

            Label lblHeader = Master.FindControl("lblHeader") as Label;
            lblHeader.Text = ReplaceStandardFields(LPA.Header);

            Label lblFooter = Master.FindControl("lblFooter") as Label;
            lblFooter.Text = ReplaceStandardFields(LPA.Footer);
        }

        private void SetupPageValues()
        {
            pnlMain.Visible = true;
            pnlThankYou.Visible = false;
            phError.Visible = false;

            SetHeaderFooter();

            //lblHeader.Text = ReplaceStandardFields(lp.Header);
            //lblFooter.Text = ReplaceStandardFields(lp.Footer);

            //Get the page label
            lblPageInfo.Text = string.Empty;
            lblPageInfo.Visible = false;
            if (LPA.AssignContentList != null && LPA.AssignContentList.Count > 0)
            {
                if (LPA.AssignContentList.Exists(x => x.LPOID == 6))
                {
                    lblPageInfo.Visible = true;
                    lblPageInfo.Text = ReplaceStandardFields(LPA.AssignContentList.FirstOrDefault(x => x.LPOID == 6).Display);
                }
            }

            //Get the email address
            lblEmailAddress.Text = "Subscription changes for: " + EmailAddress;

            //Get the main label
            lblMainInfo.Text = string.Empty;
            lblMainInfo.Visible = false;
            if (LPA.AssignContentList != null && LPA.AssignContentList.Count > 0)
            {
                if (LPA.AssignContentList.Exists(x => x.LPOID == 7))
                {
                    lblMainInfo.Visible = true;
                    lblMainInfo.Text = ReplaceStandardFields(LPA.AssignContentList.FirstOrDefault(x => x.LPOID == 7).Display);
                }
            }

            //Get the group checkbox and label
            cbGroupUnsubscribe.Visible = true;
            cbGroupUnsubscribe.Checked = true;
            cbGroupUnsubscribe.Text = string.Empty;
            if (LPA.AssignContentList != null && LPA.AssignContentList.Count > 0)
            {
                if (LPA.AssignContentList.Exists(x => x.LPOID == 4))
                {
                    cbGroupUnsubscribe.Text = ReplaceStandardFields(LPA.AssignContentList.FirstOrDefault(x => x.LPOID == 4).Display);
                }
            }
            cbGroupUnsubscribe.Enabled = false;

            //Get the customer checkbox and label
            cbCustomerUnsubscribe.Checked = false;
            cbCustomerUnsubscribe.Text = string.Empty;
            cbCustomerUnsubscribe.Visible = false;
            if (LPA.AssignContentList != null && LPA.AssignContentList.Count > 0)
            {
                if (LPA.AssignContentList.Exists(x => x.LPOID == 1) && RefBlastID > 0)
                {
                    cbCustomerUnsubscribe.Visible = true;
                    cbCustomerUnsubscribe.Text = ReplaceStandardFields(LPA.AssignContentList.FirstOrDefault(x => x.LPOID == 1).Display);
                }
            }

            //Get the main label
            lblThankYou.Text = string.Empty;
            lblThankYou.Visible = false;
            if (LPA.AssignContentList != null && LPA.AssignContentList.Count > 0)
            {
                if (LPA.AssignContentList.Exists(x => x.LPOID == 5))
                {
                    lblThankYou.Visible = true;
                    lblThankYou.Text = ReplaceStandardFields(LPA.AssignContentList.FirstOrDefault(x => x.LPOID == 5).Display);
                }
            }

            //need to do the reason textbox and dropdown
            lblReason.Text = string.Empty;
            lblReason.Visible = false;
            ddlReason.Visible = false;
            txtReason.Text = string.Empty;
            txtReason.Visible = false;
            rfvReason.Enabled = false;

            if (LPA.AssignContentList != null && LPA.AssignContentList.Count > 0)
            {
                if (LPA.AssignContentList.Exists(x => x.LPOID == 3) && RefBlastID > 0)
                {
                    ddlReason.Visible = true;
                    lblReason.Visible = true;
                    lblReason.Text = ReplaceStandardFields(LPA.AssignContentList.FirstOrDefault(x => x.LPOID == 3).Display);

                    if (LPA.AssignContentList.Exists(x => x.LPOID == 14))
                    {
                        ddlReason.Items.Clear();
                        ddlReason.DataSource = LPA.AssignContentList.Where(x => x.LPOID == 14).OrderBy(x => x.SortOrder).ToList();
                        ddlReason.DataTextField = "Display";
                        ddlReason.DataValueField = "Display";
                        ddlReason.DataBind();
                        ddlReason.Items.Insert(ddlReason.Items.Count, new ListItem() { Value = "other", Text = "Other(Please specify)" });
                    }


                }
                if (LPA.AssignContentList.Exists(x => x.LPOID == 2) && RefBlastID > 0)
                {

                    txtReason.Visible = true;
                    txtReason.Text = string.Empty;// ReplaceStandardFields(lp.AssignContentList.FirstOrDefault(x => x.LPOID == 2).Display);
                    rfvReason.Enabled = true;
                    lblReason.Visible = true;
                    lblReason.Text = ReplaceStandardFields(LPA.AssignContentList.FirstOrDefault(x => x.LPOID == 2).Display);
                }
            }

        }
        #endregion

        protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReason.SelectedValue.ToString().ToLower().Equals("other"))
            {
                txtReason.Visible = true;
                rfvReason.Enabled = true;
            }
            else
            {
                txtReason.Visible = false;
                rfvReason.Enabled = false;
            }
        }
    }
}