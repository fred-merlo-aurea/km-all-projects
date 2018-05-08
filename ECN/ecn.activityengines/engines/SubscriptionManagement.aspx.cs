using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessAccounts = ECN_Framework_BusinessLayer.Accounts;

namespace ecn.activityengines.engines
{
    public partial class SubscriptionManagement : System.Web.UI.Page
    {
        private const string SubscribedRadioButtonId = "rbSubscribed";
        private const string UnsubscribedRadioButtonId = "rbUnsubscribed";
        private const string InitialHiddenFieldId = "hfInitial";
        private const string ValueS = "S";
        private const string ValueOther = "other";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                KMPlatform.Entity.User user = GetUser();
                string emailAddress = getEmailAddress();
                int smID = getSMID();
                bool isEmbedded = IsEmbedded();
                lblReasonError.Visible = false;
                if (smID > 0 && ECN_Framework_Common.Functions.StringFunctions.HasValue(emailAddress))
                {
                    ECN_Framework_Entities.Accounts.SubscriptionManagement currentSM = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.GetBySubscriptionManagementID(smID);
                    if (currentSM != null && currentSM.SubscriptionManagementID == smID)
                    {
                        if (!Page.IsPostBack)
                        {
                            pnlContent.Visible = true;
                            pnlThankYou.Visible = false;
                            phError.Visible = false;

                            if (!isEmbedded)
                            {
                                if (ECN_Framework_Common.Functions.StringFunctions.HasValue(currentSM.Header))
                                    litHeader.Text = currentSM.Header;
                                else
                                    litHeader.Visible = false;

                                if (ECN_Framework_Common.Functions.StringFunctions.HasValue(currentSM.Footer))
                                    litFooter.Text = currentSM.Footer;
                                else
                                    litFooter.Visible = false;
                            }
                            else
                            {
                                litHeader.Visible = false;
                                litFooter.Visible = false;
                            }

                            lblTitle.Text = "Manage Subscription for: " + emailAddress;
                            if (currentSM.IncludeMSGroups.HasValue && currentSM.IncludeMSGroups.Value)
                            {
                                lblMSMessage.Visible = true;
                                lblMSMessage.Text = currentSM.MSMessage;
                                gvMasterSuppressed.Visible = true;
                            }
                            else
                            {
                                lblMSMessage.Visible = false;
                                gvMasterSuppressed.Visible = false;
                            }

                            if (currentSM.ReasonVisible.HasValue && currentSM.ReasonVisible.Value)
                            {
                                lblReasonMessage.Text = currentSM.ReasonLabel;
                                lblReasonMessage.Visible = true;

                                if (currentSM.UseReasonDropDown.HasValue && currentSM.UseReasonDropDown.Value)
                                {
                                    List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> listReason = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.GetBySMID(currentSM.SubscriptionManagementID);
                                    if (listReason.Count > 0)
                                    {
                                        ddlReason.DataSource = listReason.OrderBy(x => x.SortOrder).ToList();
                                        ddlReason.DataTextField = "Reason";
                                        ddlReason.DataValueField = "SubscriptionManagementReasonID";
                                        ddlReason.DataBind();
                                        ddlReason.Items.Insert(0, new ListItem() { Text = "--Select--", Selected = true, Value = "0" });
                                        ddlReason.Items.Insert(ddlReason.Items.Count, new ListItem() { Text = "Other(Please specify)", Value = "other" });

                                        ddlReason.Visible = true;

                                    }
                                    txtReason.Visible = false;
                                }
                                else
                                {
                                    ddlReason.Visible = false;
                                    txtReason.Visible = true;
                                }
                            }

                            LoadEmailGroupData(emailAddress, currentSM, user);
                        }
                    }
                    else
                    {
                        try
                        {
                            KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid SMID", "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                            SetError(Enums.ErrorMessage.InvalidLink);
                        }
                        catch (Exception) { }
                    }
                }
                else
                {
                    try
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid SMID or EmailAddress", "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                        SetError(Enums.ErrorMessage.InvalidLink);
                    }
                    catch (Exception) { }
                }
            }
            catch (System.Web.UI.ViewStateException vsEx)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid Viewstate<BR>" + vsEx.ToString(), "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.Timeout);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.HardError);
            }
        }

        private void SetError(Enums.ErrorMessage errorMessage, string pageMessage = "")
        {
            pnlContent.Visible = false;
            pnlThankYou.Visible = false;
            phError.Visible = true;
            lblErrorMessage.Text = ActivityError.GetErrorMessage(errorMessage, pageMessage);
        }

        private void LoadEmailGroupData(string emailAddress, ECN_Framework_Entities.Accounts.SubscriptionManagement SM, KMPlatform.Entity.User user)
        {
            btnSubmit.Enabled = false;
            chkSendResponse.Enabled = false;
            //Get currently subscribed/unsubscribed groups
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> listSMGroups = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.GetBySMID(SM.SubscriptionManagementID);
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> listAvailableGroups = new List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup>();
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> listMSGroups = new List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup>();
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> listFinalSMGroups = listSMGroups.ToList();
            foreach (ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg in listSMGroups)
            {
                ECN_Framework_Entities.Communicator.EmailGroup eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(emailAddress, smg.GroupID);
                if (eg != null)
                {
                    if (eg.SubscribeTypeCode.ToUpper().Equals("M"))
                    {
                        listMSGroups.Add(smg);
                        listFinalSMGroups.Remove(smg);
                    }
                    else if (eg.SubscribeTypeCode.ToUpper().Equals("S") || eg.SubscribeTypeCode.ToUpper().Equals("U"))
                    {
                        ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup_NoAccessCheck(eg.CustomerID.Value);
                        ECN_Framework_Entities.Communicator.EmailGroup MSEG = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailIDGroupID_NoAccessCheck(eg.EmailID, MSGroup.GroupID);
                        if (MSEG != null && MSEG.EmailID == eg.EmailID)
                        {
                            listMSGroups.Add(smg);
                            listFinalSMGroups.Remove(smg);
                        }
                        else
                        {
                            listAvailableGroups.Add(smg);
                            listFinalSMGroups.Remove(smg);
                        }
                    }
                }
                else
                {
                    ECN_Framework_Entities.Communicator.Group SMGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(smg.GroupID);
                    ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup_NoAccessCheck(SMGroup.CustomerID);
                    ECN_Framework_Entities.Communicator.EmailGroup MSEG = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(emailAddress, MSGroup.GroupID);
                    if (MSEG != null && MSEG.EmailID > 0)
                    {
                        listMSGroups.Add(smg);
                        listFinalSMGroups.Remove(smg);
                    }

                }

            }

            if (listMSGroups.Count > 0 && (SM.IncludeMSGroups.HasValue && SM.IncludeMSGroups.Value == true))
            {
                gvMasterSuppressed.DataSource = listMSGroups;
                gvMasterSuppressed.DataBind();
                lblMSMessage.Visible = true;
                gvMasterSuppressed.Visible = true;
            }
            else
            {
                lblMSMessage.Visible = false;
                gvMasterSuppressed.Visible = false;
            }

            if (listAvailableGroups.Count > 0)
            {
                gvCurrent.DataSource = listAvailableGroups;
                gvCurrent.DataBind();
                gvCurrent.Visible = true;
                lblCurrentHeading.Visible = true;
                btnSubmit.Enabled = true;
                chkSendResponse.Enabled = true;
                ddlReason.Visible = false;
                txtReason.Visible = false;
                lblReasonMessage.Visible = false;
            }
            else
            {
                gvCurrent.Visible = false;
                lblCurrentHeading.Visible = false;
                ddlReason.Visible = false;
                txtReason.Visible = false;
                lblReasonMessage.Visible = false;
            }

            if (listFinalSMGroups.Count > 0)
            {
                gvAvailable.DataSource = listFinalSMGroups;
                gvAvailable.DataBind();
                gvAvailable.Visible = true;
                lblAvailableHeading.Visible = true;
                btnSubmit.Enabled = true;
                chkSendResponse.Enabled = true;
            }
            else
            {
                gvAvailable.Visible = false;
                lblAvailableHeading.Visible = false;

            }

        }

        protected void gvCurrent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg = (ECN_Framework_Entities.Accounts.SubscriptionManagementGroup)e.Row.DataItem;

                ECN_Framework_Entities.Communicator.EmailGroup eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(getEmailAddress(), smg.GroupID);

                Label groupName = (Label)e.Row.FindControl("lblGroupName");
                groupName.Text = smg.Label;

                RadioButton rbSub = (RadioButton)e.Row.FindControl("rbSubscribed");
                RadioButton rbUnSub = (RadioButton)e.Row.FindControl("rbUnsubscribed");
                HiddenField hfInitial = (HiddenField)e.Row.FindControl("hfInitial");
                if (eg != null && eg.SubscribeTypeCode.ToUpper().Equals("S"))
                {
                    rbSub.Checked = true;
                    rbUnSub.Checked = false;
                    hfInitial.Value = "S";
                }
                else if (eg != null && eg.SubscribeTypeCode.ToUpper().Equals("U"))
                {
                    rbSub.Checked = false;
                    rbUnSub.Checked = true;
                    hfInitial.Value = "U";
                }
            }
        }

        private string getEmailAddress()
        {
            try
            {
                string email = Request.QueryString["e"].ToString().Trim();

                if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(email))
                    return email;
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        private int getSMID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["smid"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private bool IsEmbedded()
        {
            try
            {
                bool isEmbedded = false;

                bool.TryParse(Request.QueryString["embedded"].ToString(), out isEmbedded);
                return isEmbedded;
            }
            catch
            {
                return false;
            }
        }

        private KMPlatform.Entity.User GetUser()
        {
            KMPlatform.Entity.User user = null;
            if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
            {
                user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), user, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                user = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
            }
            return user;
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<BR><BR>SMID: " + getSMID().ToString());
                adminEmailVariables.AppendLine("<BR>EmailAddress: " + getEmailAddress());
                adminEmailVariables.AppendLine("<BR>Embedded: " + IsEmbedded().ToString());
                adminEmailVariables.AppendLine("<BR>Page URL: " + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
                adminEmailVariables.AppendLine("<BR>SPY Info:&nbsp;[" + Request.UserHostAddress + "] / [" + Request.UserAgent + "]");
                if (Request.UrlReferrer != null)
                {
                    adminEmailVariables.AppendLine("<BR>Referring URL: " + Request.UrlReferrer.ToString());
                }
                adminEmailVariables.AppendLine("<BR>HEADERS");
                var headers = String.Empty;
                foreach (var key in Request.Headers.AllKeys)
                    headers += "<BR>" + key + ":" + Request.Headers[key];
                adminEmailVariables.AppendLine(headers);
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        protected void rbSubscribed_CheckedChanged(object sender, EventArgs e)
        {
            SetControlVisibilityOnCheckedChanged(sender, UnsubscribedRadioButtonId);
        }

        protected void rbUnsubscribed_CheckedChanged(object sender, EventArgs e)
        {
            SetControlVisibilityOnCheckedChanged(sender, SubscribedRadioButtonId);
        }

        private void SetControlVisibilityOnCheckedChanged(object sender, string radioButtonId)
        {
            var senderRadioButton = sender as RadioButton;
            if (senderRadioButton != null)
            {
                var gridViewRow = senderRadioButton.Parent.NamingContainer as GridViewRow;
                if (gridViewRow != null)
                {
                    var radioButton = gridViewRow.FindControl(radioButtonId) as RadioButton;
                    if (radioButton != null)
                    {
                        radioButton.Checked = false;
                    }
                }
            }

            foreach (GridViewRow gridViewRow in gvCurrent.Rows)
            {
                var unSubscribeRadioButton = gridViewRow.FindControl(UnsubscribedRadioButtonId) as RadioButton;
                var hiddenField = gridViewRow.FindControl(InitialHiddenFieldId) as HiddenField;

                if (hiddenField != null && 
                    unSubscribeRadioButton != null && 
                    unSubscribeRadioButton.Checked && 
                    hiddenField.Value.Equals(ValueS))
                {
                    var subscriptionManagementId = getSMID();
                    var subscriptionManagement = BusinessAccounts.SubscriptionManagement
                        .GetBySubscriptionManagementID(subscriptionManagementId);

                    if (subscriptionManagement?.ReasonVisible != null && subscriptionManagement.ReasonVisible.Value)
                    {
                        if (subscriptionManagement.UseReasonDropDown.HasValue && 
                            subscriptionManagement.UseReasonDropDown.Value)
                        {
                            if (ddlReason.SelectedValue.Equals(ValueOther, StringComparison.OrdinalIgnoreCase))
                            {
                                lblReasonMessage.Visible = true;
                                ddlReason.Visible = true;
                                txtReason.Visible = true;
                                return;
                            }

                            lblReasonMessage.Visible = true;
                            ddlReason.Visible = true;
                            txtReason.Visible = false;
                            txtReason.Text = string.Empty;
                            return;
                        }

                        lblReasonMessage.Visible = true;
                        ddlReason.Visible = false;
                        txtReason.Visible = true;
                        return;
                    }

                    lblReasonMessage.Visible = false;
                    ddlReason.Visible = false;
                    txtReason.Visible = false;
                    return;
                }

                ddlReason.Visible = false;
                txtReason.Visible = false;
                lblReasonMessage.Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                KMPlatform.Entity.User user = GetUser();
                string emailAddress = getEmailAddress();
                int smID = getSMID();
                bool isEmbedded = IsEmbedded();

                if (smID > 0 && ECN_Framework_Common.Functions.StringFunctions.HasValue(emailAddress))
                {
                    ECN_Framework_Entities.Accounts.SubscriptionManagement currentSM = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.GetBySubscriptionManagementID(smID);

                    if (currentSM != null && currentSM.SubscriptionManagementID == smID)
                    {

                        StringBuilder sbImport = new StringBuilder();
                        sbImport.Append("<root><subscriptionManagement id=\"" + smID.ToString() + "\" emailAddress=\"" + emailAddress + "\">");

                        List<ECN_Framework_Entities.Communicator.Group> listGroups = new List<ECN_Framework_Entities.Communicator.Group>();
                        List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> listSMGroups = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.GetBySMID(smID);

                        foreach (ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg in listSMGroups)
                        {
                            ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(smg.GroupID);
                            listGroups.Add(g);
                        }

                        List<GroupList> groupListForEmail = new List<GroupList>();

                        if (gvCurrent != null && gvCurrent.Rows.Count > 0)
                        {
                            foreach (GridViewRow gvr in gvCurrent.Rows)
                            {
                                if (gvr.RowType == DataControlRowType.DataRow)
                                {
                                    ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg = listSMGroups.First(x => x.SubscriptionManagementGroupID == Convert.ToInt32(gvCurrent.DataKeys[gvr.RowIndex].Value.ToString()));
                                    ECN_Framework_Entities.Communicator.Group g = listGroups.First(x => x.GroupID == smg.GroupID);

                                    bool Subscribe = false;
                                    RadioButton rbSub = (RadioButton)gvr.FindControl("rbSubscribed");
                                    Subscribe = rbSub.Checked;
                                    HiddenField hfInitial = (HiddenField)gvr.FindControl("hfInitial");

                                    groupListForEmail.Add(new GroupList() { GroupLabel = smg.Label, SubscribedTypeCode = Subscribe ? "S" : "U" });
                                    if ((Subscribe && hfInitial.Value.ToString().ToUpper().Equals("U")) || (!Subscribe && hfInitial.Value.ToString().ToUpper().Equals("S")))
                                    {
                                        sbImport.Append("<group id=\"" + smg.GroupID + "\" customer=\"" + g.CustomerID.ToString() + "\" subscribeTypeCode=\"" + (Subscribe ? "S" : "U") + "\"");
                                        if (!Subscribe && hfInitial.Value.ToString().ToUpper().Equals("S"))
                                        {
                                            string Reason = string.Empty;
                                            bool checkReason = false;
                                            if (!currentSM.ReasonVisible.HasValue)
                                            {
                                                checkReason = false;
                                            }
                                            else if (!currentSM.ReasonVisible.Value)
                                                checkReason = false;
                                            else
                                                checkReason = true;

                                            if (checkReason)
                                            {
                                                if (currentSM.UseReasonDropDown.HasValue && currentSM.UseReasonDropDown.Value)
                                                {
                                                    if (ddlReason.SelectedIndex == 0)
                                                    {
                                                        //Need to select a reason
                                                        lblReasonError.Text = "Please select a reason from the list of available reasons";
                                                        lblReasonError.Visible = true;
                                                        return;
                                                    }
                                                    else if (ddlReason.SelectedValue.ToString().ToLower().Equals("other"))
                                                    {

                                                        if (string.IsNullOrEmpty(txtReason.Text.Trim()))
                                                        {
                                                            //Need to have a reason in textbox
                                                            lblReasonError.Text = "Please enter a reason in the text box.";
                                                            lblReasonError.Visible = true;
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            Reason = ". Reason: Other " + CleanStringForXML(txtReason.Text.Trim());//.Replace("'", "'''");
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Reason = ". Reason: " + CleanStringForXML(ddlReason.SelectedItem.Text.Trim());//.Replace("'", "'''");
                                                    }
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(txtReason.Text.Trim()))
                                                    {
                                                        //Need to have a reason in textbox
                                                        lblReasonError.Text = "Please enter a reason in the text box.";
                                                        lblReasonError.Visible = true;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        Reason = ". Reason: Other " + CleanStringForXML(txtReason.Text.Trim());//.Replace("'", "'''");
                                                    }
                                                }

                                            }
                                            sbImport.Append(" reason=\"" + CleanStringForXML(Reason) + "\">");
                                        }
                                        else
                                        {
                                            sbImport.Append(">");
                                        }

                                        List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> listUDFs = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementUDF.GetBySMGID(smg.SubscriptionManagementGroupID);
                                        if (listUDFs.Count > 0)
                                        {
                                            foreach (ECN_Framework_Entities.Accounts.SubsriptionManagementUDF udf in listUDFs)
                                            {
                                                sbImport.Append("<udf id=\"" + udf.GroupDataFieldsID.ToString() + "\">" + udf.StaticValue + "</udf>");
                                            }
                                        }
                                        sbImport.Append("</group>");
                                    }
                                }
                            }
                        }

                        if (gvAvailable != null && gvAvailable.Rows.Count > 0)
                        {
                            foreach (GridViewRow gvr in gvAvailable.Rows)
                            {
                                if (gvr.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chSub = (CheckBox)gvr.FindControl("chkSubscribe");
                                    ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg = listSMGroups.First(x => x.SubscriptionManagementGroupID == Convert.ToInt32(gvAvailable.DataKeys[gvr.RowIndex].Value.ToString()));
                                    ECN_Framework_Entities.Communicator.Group g = listGroups.First(x => x.GroupID == smg.GroupID);

                                    GroupList gl = new GroupList();
                                    gl.GroupLabel = smg.Label;
                                    if (chSub.Checked)
                                    {
                                        gl.SubscribedTypeCode = "S";

                                        sbImport.Append("<group id=\"" + smg.GroupID + "\" customer=\"" + g.CustomerID.ToString() + "\" subscribeTypeCode=\"S\" >");

                                        List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> listUDFs = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementUDF.GetBySMGID(smg.SubscriptionManagementGroupID);
                                        if (listUDFs.Count > 0)
                                        {
                                            foreach (ECN_Framework_Entities.Accounts.SubsriptionManagementUDF udf in listUDFs)
                                            {
                                                sbImport.Append("<udf id=\"" + udf.GroupDataFieldsID.ToString() + "\">" + udf.StaticValue + "</udf>");
                                            }
                                        }
                                        sbImport.Append("</group>");
                                    }
                                    else
                                    {
                                        gl.SubscribedTypeCode = "U";
                                    }
                                    groupListForEmail.Add(gl);
                                }
                            }
                        }

                        sbImport.Append("</subscriptionManagement></root>");

                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailToGroups_NoAccessCheck(sbImport.ToString(), user.UserID);
                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                                throw;
                        }

                        if (chkSendResponse.Checked || ECN_Framework_Common.Functions.StringFunctions.HasValue(currentSM.AdminEmail))
                        {
                            StringBuilder sbSubInfo = new StringBuilder();
                            sbSubInfo.Append("<HTML><HEAD></HEAD><BODY>");
                            sbSubInfo.Append(ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(currentSM.EmailHeader));
                            sbSubInfo.Append("<p>Group subscription status for: " + emailAddress + "</p>");
                            sbSubInfo.Append("<table>");
                            foreach (GroupList gl in groupListForEmail)
                            {
                                sbSubInfo.Append("<tr><td style='padding:5px;'>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(gl.GroupLabel) + "</td>");
                                sbSubInfo.Append("<td>" + (ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(gl.SubscribedTypeCode).ToLower().Equals("s") ? "Subscribed" : "Unsubscribed").ToString() + "</td></tr>");
                            }
                            sbSubInfo.Append("</table>");
                            sbSubInfo.Append(ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(currentSM.EmailFooter));
                            sbSubInfo.Append("</BODY></HTML>");


                            //Setup EmailDirect
                            ECN_Framework_Entities.Communicator.Group g = listGroups.First();
                            ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                            ed.CustomerID = g.CustomerID;
                            ed.FromName = "Subscription Management";
                            ed.Process = "Subscription Management - Subscriber Email";
                            ed.Source = "Subscription Management";
                            ed.CreatedUserID = currentSM.CreatedUserID.Value;



                            if (chkSendResponse.Checked)
                            {
                                ed.EmailAddress = emailAddress; 
                                ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                                ed.EmailSubject  = "Subscription Status";
                                ed.Content = sbSubInfo.ToString();


                                try
                                {
                                    ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                                }
                                catch (SmtpException smEX)
                                {
                                    try
                                    {
                                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("SMTP User Send<BR>" + smEX.ToString(), "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                                    }
                                    catch (Exception) { }
                                }
                            }

                            if (ECN_Framework_Common.Functions.StringFunctions.HasValue(currentSM.AdminEmail))
                            {
                                string[] adminEmails = currentSM.AdminEmail.Split(',');
                                for (int i = 0; i < adminEmails.Length; i++)
                                {
                                    ed.EmailAddress = adminEmails[i].ToString();
                                    ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                                    ed.EmailSubject = "Subscription Status";
                                    ed.Content = sbSubInfo.ToString();
                                    ed.Process = "Subscription Management - Admin Email";
                                    try
                                    {
                                        ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                                    }
                                    catch (SmtpException smEX)
                                    {
                                        try
                                        {
                                            KM.Common.Entity.ApplicationLog.LogNonCriticalError("SMTP Admin Send<BR>" + smEX.ToString(), "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                                        }
                                        catch (Exception) { }
                                    }
                                }

                            }
                        }
                        phError.Visible = false;
                        pnlContent.Visible = false;
                        if (currentSM.UseThankYou.HasValue && currentSM.UseThankYou.Value && (!currentSM.UseRedirect.HasValue || !currentSM.UseRedirect.Value))
                        {
                            pnlThankYou.Visible = true;
                            lblThankYouHeading.Text = currentSM.ThankYouLabel;
                        }
                        else if (currentSM.UseRedirect.HasValue && currentSM.UseRedirect.Value && (!currentSM.UseThankYou.HasValue || !currentSM.UseThankYou.Value))
                        {
                            Response.Redirect(currentSM.RedirectURL, false);
                        }
                        else if (currentSM.UseThankYou.HasValue && currentSM.UseRedirect.HasValue && currentSM.UseThankYou.Value && currentSM.UseRedirect.Value)
                        {
                            string URL = currentSM.RedirectURL;
                            int delay = 5;
                            int.TryParse(currentSM.RedirectDelay.ToString(), out delay);
                            delay = delay * 1000;
                            pnlThankYou.Visible = true;
                            lblThankYouHeading.Text = currentSM.ThankYouLabel;
                            ClientScript.RegisterStartupScript(this.GetType(), "redirect", "<script type='text/javascript'>window.setTimeout(function(){window.location.href='" + URL + "';}," + delay.ToString() + ")</script>");
                        }
                        else
                        {
                            pnlThankYou.Visible = true;

                        }
                    }
                    else
                    {
                        try
                        {
                            KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid SMID", "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                            SetError(Enums.ErrorMessage.InvalidLink);
                        }
                        catch (Exception) { }
                    }


                }
                else
                {
                    try
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid SMID or EmailAddress", "SubscriptionManagement.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                        SetError(Enums.ErrorMessage.InvalidLink);
                    }
                    catch (Exception) { }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("ECN Exception<BR>" + ecnEX.ToString(), "SubscriptionManagement.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
                SetError(Enums.ErrorMessage.HardError);
            }
            catch (System.Web.UI.ViewStateException vsEx)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid Viewstate<BR>" + vsEx.ToString(), "SubscriptionManagement.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.Timeout);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SubscriptionManagement.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.HardError);
            }
        }

        protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReason.SelectedValue.ToLower().Equals("other"))
            {
                txtReason.Visible = true;
            }
            else
                txtReason.Visible = false;
        }

        private string CleanStringForXML(string dirty)
        {
            dirty = dirty.Replace("&", "&amp;");

            return dirty;
        }
    }

    public class GroupList
    {
        public GroupList() { }

        #region properties
        public string GroupLabel { get; set; }
        public string SubscribedTypeCode { get; set; }

        #endregion
    }
}