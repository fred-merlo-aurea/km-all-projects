using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using KMPS_JF_Objects.Objects;
using System.Data.SqlClient;
using KMPS_JF_Objects;
using AjaxControlToolkit;
using System.Collections.Generic;

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_FormsCreate : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();
        string PubCode = string.Empty;


        private int PubID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PubId"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int PFID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["PFID"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["PFID"] = value;
            }
        }

        private bool ProcessExternal
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ViewState["ProcessExternal"].ToString());
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["ProcessExternal"] = value;
            }
        }

        public int ActiveTab
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["ActiveTab"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ActiveTab"] = value;
            }
        }


        public int ActiveResponseEmailTab
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["ActiveResponseEmailTab"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ActiveResponseEmailTab"] = value;
            }
        }

        private ArrayList aProfileFields = new ArrayList();
        private ArrayList aDemographicFields = new ArrayList();

        Publication pub;

        private bool responseStatus = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.btnReload, this.GetType(), "reloaditems", "function reloadpage(){" + Page.ClientScript.GetPostBackEventReference((Control)this.btnReload, "btnReload_Click") + "}", true);
            btnReload.Attributes.Add("style", "visibility:hidden");
            lblMessage.Text = "";
            pnlEditInactiveFields.Visible = false;

            if (!IsPostBack)
            {
                try
                {
                    PFID = Convert.ToInt32(Request.QueryString["PFID"]);
                }
                catch
                {
                    PFID = 0;
                }


                ActiveTab = 0;
                ActiveResponseEmailTab = 0;

                SqlCommand cmdSelectByPubID = new SqlCommand("select PubCode from Publications where PubID = @PubID");
                cmdSelectByPubID.CommandType = CommandType.Text;
                cmdSelectByPubID.Parameters.Add(new SqlParameter("@PubID", PubID.ToString()));

                PubCode = DataFunctions.ExecuteScalar(cmdSelectByPubID).ToString();

                pub = Publication.GetPublicationbyID(PubID, PubCode);

                ProcessExternal = pub.ProcessExternal;
                BoxPanel2.Title = "Manage Subscription forms for " + PubCode + ":";

                if (PFID > 0)
                    LoadSubscriptionForm();
                else
                    LoadFormFields();

                if (!pub.HasPaid)
                {
                    paidSubscription.Visible = false;
                    pnlpaid.Visible = false;
                    rbpaidsub.Items.FindByValue("false").Selected = true;
                }

                pnlPassword.Visible = !pub.DisablePassword;

                LoadTab();
            }
            else
            {
                TempSave();
            }
        }

        private void TempSave()
        {
            foreach (GridViewRow gr in grdProfileFields.Rows)
                aProfileFields.Add(grdProfileFields.DataKeys[gr.RowIndex].Value.ToString());

            foreach (GridViewRow gr in grdDemoGraphicFields.Rows)
                aDemographicFields.Add(grdDemoGraphicFields.DataKeys[gr.RowIndex].Value.ToString());
        }

        protected void LoadSubscriptionForm()
        {
            try
            {
                SqlCommand cmdGetFrom = new SqlCommand("Select pf.* from PubForms pf where  pf.PFID= @PFID and pf.PubId=@PubID");
                cmdGetFrom.CommandType = CommandType.Text;
                cmdGetFrom.Parameters.Add(new SqlParameter("@PFID", PFID));
                cmdGetFrom.Parameters.Add(new SqlParameter("@PubID", PubID));
                DataTable dtform = DataFunctions.GetDataTable(cmdGetFrom);

                if (dtform.Rows.Count > 0)
                {

                    lblFormName.Text = dtform.Rows[0]["FormName"].ToString();
                    lblFormDescription.Text = dtform.Rows[0]["Description"].ToString().Trim();

                    txtFormName.Text = dtform.Rows[0]["FormName"].ToString();
                    txtDescription.Text = dtform.Rows[0]["Description"].ToString().Trim();

                    if (dtform.Rows[0]["SUBSCRIPTIONQuestion"].ToString() != "")
                        RadEditorSUBSCRIPTIONQuestion.Content = dtform.Rows[0]["SUBSCRIPTIONQuestion"].ToString();
                    else
                        RadEditorSUBSCRIPTIONQuestion.Content = "Do you wish to receive/continue receiving your copy of " + pub.PubName + " magazine?";


                    if (dtform.Rows[0]["PRINTDIGITALQuestion"].ToString() != "")
                        RadEditorPRINTDIGITALQuestion.Content = dtform.Rows[0]["PRINTDIGITALQuestion"].ToString();
                    else
                        RadEditorPRINTDIGITALQuestion.Content = "How would you like to receive your copy of " + pub.PubName + " magazine?";


                    rbshowprint.ClearSelection();
                    if (Convert.ToBoolean(dtform.Rows[0]["ShowPrint"].ToString()))
                        rbshowprint.Items.FindByValue("true").Selected = true;
                    else
                        rbshowprint.Items.FindByValue("false").Selected = true;

                    rbshowdigital.ClearSelection();
                    if (Convert.ToBoolean(dtform.Rows[0]["ShowDigital"].ToString()))
                        rbshowdigital.Items.FindByValue("true").Selected = true;
                    else
                        rbshowdigital.Items.FindByValue("false").Selected = true;

                    rbsShowPrintasDigital.ClearSelection();
                    if (Convert.ToBoolean(dtform.Rows[0]["ShowPrintAsDigital"].ToString()))
                        rbsShowPrintasDigital.Items.FindByValue("true").Selected = true;
                    else
                        rbsShowPrintasDigital.Items.FindByValue("false").Selected = true;


                    rblstPrintDigital.ClearSelection();

                    try
                    {
                        if (Convert.ToBoolean(dtform.Rows[0]["EnablePrintAndDigital"].ToString()))
                            rblstPrintDigital.Items.FindByValue("true").Selected = true;
                        else
                            rblstPrintDigital.Items.FindByValue("false").Selected = true;
                    }
                    catch
                    {
                        rblstPrintDigital.Items.FindByValue("false").Selected = true;
                    }

                    rbpaidsub.ClearSelection();
                    if (Convert.ToBoolean(dtform.Rows[0]["IsPaid"].ToString()))
                    {
                        rbpaidsub.Items.FindByValue("true").Selected = true;

                        if (ProcessExternal)
                            pnlpaid.Visible = false;
                        else
                            pnlpaid.Visible = Convert.ToBoolean(rbpaidsub.SelectedValue);

                        paidCost.Visible = Convert.ToBoolean(rbpaidsub.SelectedValue);
                    }
                    else
                    {
                        rbpaidsub.Items.FindByValue("false").Selected = true;
                    }

                    rbPreSelectNewsletters.ClearSelection();
                    if (Convert.ToBoolean(dtform.Rows[0]["PreSelectNewsletters"].ToString()))
                    {
                        rbPreSelectNewsletters.Items.FindByValue("true").Selected = true;
                    }
                    else
                    {
                        rbPreSelectNewsletters.Items.FindByValue("false").Selected = true;
                    }

                    rbNewsletterCollapsible.ClearSelection();
                    try
                    {
                        if (Convert.ToBoolean(dtform.Rows[0]["ShowNewsletterAsCollapsed"].ToString()))
                            rbNewsletterCollapsible.Items.FindByValue("true").Selected = true;
                        else
                            rbNewsletterCollapsible.Items.FindByValue("false").Selected = true;
                    }
                    catch { }

                    rbNewsletterSearch.ClearSelection();
                    try
                    {
                        if (Convert.ToBoolean(dtform.Rows[0]["ShowNewsletterSearch"].ToString()))
                            rbNewsletterSearch.Items.FindByValue("true").Selected = true;
                        else
                            rbNewsletterSearch.Items.FindByValue("false").Selected = true;
                    }
                    catch { }

                    drpNewsletterPosition.ClearSelection();
                    try
                    {
                        if (dtform.Rows[0]["NewsletterPosition"].ToString().ToUpper() == "B")
                            drpNewsletterPosition.Items.FindByValue("B").Selected = true;
                        else
                            drpNewsletterPosition.Items.FindByValue("A").Selected = true;
                    }
                    catch
                    {
                        drpNewsletterPosition.Items.FindByValue("A").Selected = true;
                    }

                    PaypalPaidCost.Text = dtform.Rows[0]["PaidPrice"].ToString();

                    MultiViewForms.ActiveViewIndex = 2;

                    try
                    {
                        Dictionary<NotificatonFor, PubResponseEmail> dResponseEmail = PubResponseEmail.GetByPFID(PFID);

                        foreach (KeyValuePair<NotificatonFor, PubResponseEmail> ResponseEmailkvp in dResponseEmail)
                        {
                            PubResponseEmail pre = ResponseEmailkvp.Value;

                            if (ResponseEmailkvp.Key.Equals(NotificatonFor.Print))
                            {
                                #region PRINT

                                FromNamePrint.Text = pre.Response_FromName;
                                txtFormEmailPrint.Text = pre.Response_FromEmail;

                                try
                                {
                                    rbUserNotificationPrint.ClearSelection();

                                    if (pre.SendUserEmail)
                                    {
                                        pnlUserNotificationPrint.Visible = true;
                                        rbUserNotificationPrint.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlUserNotificationPrint.Visible = false;
                                        rbUserNotificationPrint.Items.FindByValue("false").Selected = true;
                                    }

                                }
                                catch { }

                                txtUserEmailSubPrint.Text = pre.Response_UserMsgSubject;
                                RadEditorUserEmailBodyPrint.Content = Server.HtmlDecode(pre.Response_UserMsgBody);

                                try
                                {
                                    rbAdminNotificationPrint.ClearSelection();

                                    if (pre.SendAdminEmail)
                                    {
                                        pnlAdminNotificationPrint.Visible = true;
                                        rbAdminNotificationPrint.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlAdminNotificationPrint.Visible = false;
                                        rbAdminNotificationPrint.Items.FindByValue("false").Selected = true;
                                    }

                                }
                                catch
                                { }

                                txtAdminEmailPrint.Text = pre.Response_AdminEmail;
                                txtAdminEmailSubPrint.Text = pre.Response_AdminMsgSubject;
                                RadEditorAdminEmailBodyPrint.Content = Server.HtmlDecode(pre.Response_AdminMsgBody);

                                try
                                {
                                    rbUserNotificationNQResponsePrint.ClearSelection();

                                    if (pre.SendNQRespEmail)
                                    {
                                        pnlNonQualResponse.Visible = true;
                                        rbUserNotificationNQResponsePrint.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlNonQualResponse.Visible = false;
                                        rbUserNotificationNQResponsePrint.Items.FindByValue("false").Selected = true;
                                    }
                                }
                                catch
                                { }

                                txtUserEmailSubNQResponsePrint.Text = pre.Response_UserMsgNQRespSub;
                                RadEditorEmailBodyNQResponsePrint.Content = Server.HtmlDecode(pre.Response_UserMsgNQRespMsgBody);

                                if (!responseStatus)
                                {
                                    MnuResponseType.Items[0].Selected = true;
                                    multiViewResponseType.ActiveViewIndex = 0;
                                    ActiveResponseEmailTab = multiViewResponseType.ActiveViewIndex;

                                    pnlFromPrint.Visible = (pnlUserNotificationPrint.Visible || pnlAdminNotificationPrint.Visible || pnlNonQualResponse.Visible);
                                    responseStatus = pnlFromPrint.Visible;
                                }

                                #endregion PRINT
                            }
                            else if (ResponseEmailkvp.Key.Equals(NotificatonFor.Digital))
                            {
                                #region DIGITAL

                                FromNameDigital.Text = pre.Response_FromName;
                                txtFormEmailDigital.Text = pre.Response_FromEmail;

                                try
                                {
                                    rbUserNotificationDigital.ClearSelection();

                                    if (pre.SendUserEmail)
                                    {
                                        pnlUserNotificationDigital.Visible = true;
                                        rbUserNotificationDigital.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlUserNotificationDigital.Visible = false;
                                        rbUserNotificationDigital.Items.FindByValue("false").Selected = true;
                                    }

                                }
                                catch { }

                                txtUserEmailSubDigital.Text = pre.Response_UserMsgSubject;
                                RadEditorUserEmailBodyDigital.Content = Server.HtmlDecode(pre.Response_UserMsgBody);

                                try
                                {
                                    rbAdminNotificationDigital.ClearSelection();

                                    if (pre.SendAdminEmail)
                                    {
                                        pnlAdminNotificationDigital.Visible = true;
                                        rbAdminNotificationDigital.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlAdminNotificationDigital.Visible = false;
                                        rbAdminNotificationDigital.Items.FindByValue("false").Selected = true;
                                    }

                                }
                                catch
                                { }

                                txtAdminEmailDigital.Text = pre.Response_AdminEmail;
                                txtAdminEmailSubDigital.Text = pre.Response_AdminMsgSubject;
                                RadEditorAdminEmailBodyDigital.Content = Server.HtmlDecode(pre.Response_AdminMsgBody);


                                if (!responseStatus)
                                {
                                    MnuResponseType.Items[1].Selected = true;
                                    multiViewResponseType.ActiveViewIndex = 1;
                                    ActiveResponseEmailTab = multiViewResponseType.ActiveViewIndex;

                                    pnlFromDigital.Visible = (pnlUserNotificationDigital.Visible || pnlAdminNotificationDigital.Visible);
                                    responseStatus = pnlFromDigital.Visible;
                                }

                                #endregion DIGITAL
                            }
                            else if (ResponseEmailkvp.Key.Equals(NotificatonFor.Both))
                            {
                                #region BOTH

                                FromNameBoth.Text = pre.Response_FromName;
                                txtFormEmailBoth.Text = pre.Response_FromEmail;

                                try
                                {
                                    rbUserNotificationBoth.ClearSelection();

                                    if (pre.SendUserEmail)
                                    {
                                        pnlUserNotificationBoth.Visible = true;
                                        rbUserNotificationBoth.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlUserNotificationBoth.Visible = false;
                                        rbUserNotificationBoth.Items.FindByValue("false").Selected = true;
                                    }
                                }
                                catch { }

                                txtUserEmailSubBoth.Text = pre.Response_UserMsgSubject;
                                RadEditorUserEmailBodyBoth.Content = Server.HtmlDecode(pre.Response_UserMsgBody);

                                try
                                {
                                    rbAdminNotificationBoth.ClearSelection();

                                    if (pre.SendAdminEmail)
                                    {
                                        pnlAdminNotificationBoth.Visible = true;
                                        rbAdminNotificationBoth.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlAdminNotificationBoth.Visible = false;
                                        rbAdminNotificationBoth.Items.FindByValue("false").Selected = true;
                                    }
                                }
                                catch
                                { }

                                txtAdminEmailBoth.Text = pre.Response_AdminEmail;
                                txtAdminEmailSubBoth.Text = pre.Response_AdminMsgSubject;
                                RadEditorAdminEmailBodyBoth.Content = Server.HtmlDecode(pre.Response_AdminMsgBody);

                                if (!responseStatus)
                                {
                                    MnuResponseType.Items[2].Selected = true;
                                    multiViewResponseType.ActiveViewIndex = 2;

                                    pnlFromBoth.Visible = (pnlNonQualResponse.Visible || pnlNQResponseCountry.Visible || pnlUserNotificationBoth.Visible || pnlAdminNotificationBoth.Visible);
                                    responseStatus = pnlFromBoth.Visible;
                                    ActiveResponseEmailTab = multiViewResponseType.ActiveViewIndex;
                                }
                                #endregion BOTH
                            }
                            else if (ResponseEmailkvp.Key.Equals(NotificatonFor.NQ))
                            {
                                #region NON QUALIFIED

                                FromNameNQ.Text = pre.Response_FromName;
                                txtFormEmailNQ.Text = pre.Response_FromEmail;

                                try
                                {
                                    rbUserNotificationNQCountry.ClearSelection();

                                    if (pre.SendUserEmail)
                                    {
                                        pnlNQResponseCountry.Visible = true;
                                        rbUserNotificationNQCountry.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlNQResponseCountry.Visible = false;
                                        rbUserNotificationNQCountry.Items.FindByValue("false").Selected = true;
                                    }

                                }
                                catch
                                { }

                                txtUserNotificationSubNQCountry.Text = pre.Response_UserMsgSubject;
                                RadEditorNotificationSubNQCountry.Content = Server.HtmlDecode(pre.Response_UserMsgBody);


                                if (!responseStatus)
                                {
                                    MnuResponseType.Items[3].Selected = true;
                                    multiViewResponseType.ActiveViewIndex = 3;
                                    ActiveResponseEmailTab = 3;

                                    pnlFromEmailNQ.Visible = pnlNQResponseCountry.Visible;
                                    responseStatus = pnlFromEmailNQ.Visible;
                                }

                                #endregion NONQUALIFIED
                            }
                            else if (ResponseEmailkvp.Key.Equals(NotificatonFor.Cancel))
                            {
                                #region CANCEL
                                FromNameCancel.Text = pre.Response_FromName;
                                txtFormEmailCancel.Text = pre.Response_FromEmail;

                                try
                                {

                                    rbUserNotificationCancel.ClearSelection();

                                    if (pre.SendUserEmail)
                                    {
                                        pnlUserNotificationCancel.Visible = true;
                                        rbUserNotificationCancel.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlUserNotificationCancel.Visible = false;
                                        rbUserNotificationCancel.Items.FindByValue("false").Selected = true;
                                    }

                                }
                                catch { }

                                txtUserEmailSubCancel.Text = pre.Response_UserMsgSubject;
                                RadEditorUserEmailBodyCancel.Content = Server.HtmlDecode(pre.Response_UserMsgBody);

                                try
                                {
                                    rbAdminNotificationCancel.ClearSelection();

                                    if (pre.SendAdminEmail)
                                    {
                                        pnlAdminNotificationCancel.Visible = true;
                                        rbAdminNotificationCancel.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlAdminNotificationCancel.Visible = false;
                                        rbAdminNotificationCancel.Items.FindByValue("false").Selected = true;
                                    }
                                }
                                catch
                                { }

                                txtAdminEmailCancel.Text = pre.Response_AdminEmail;
                                txtAdminEmailSubCancel.Text = pre.Response_AdminMsgSubject;
                                RadEditorAdminEmailBodyCancel.Content = Server.HtmlDecode(pre.Response_AdminMsgBody);

                                if (!responseStatus)
                                {
                                    pnlFromCancel.Visible = (pnlNonQualResponse.Visible || pnlNQResponseCountry.Visible || pnlUserNotificationCancel.Visible || pnlAdminNotificationCancel.Visible);
                                    responseStatus = pnlFromCancel.Visible;
                                    ActiveResponseEmailTab = multiViewResponseType.ActiveViewIndex;
                                }
                                #endregion CANCEL
                            }
                            else if (ResponseEmailkvp.Key.Equals(NotificatonFor.Other))
                            {
                                #region OTHER

                                FromNameOther.Text = pre.Response_FromName;
                                txtFormEmailOther.Text = pre.Response_FromEmail;

                                try
                                {
                                    rbUserNotificationOther.ClearSelection();

                                    if (pre.SendUserEmail)
                                    {
                                        pnlUserNotificationOther.Visible = true;
                                        rbUserNotificationOther.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlUserNotificationOther.Visible = false;
                                        rbUserNotificationOther.Items.FindByValue("false").Selected = true;
                                    }
                                }
                                catch { }

                                txtUserEmailSubOther.Text = pre.Response_UserMsgSubject;
                                RadEditorUserEmailBodyOther.Content = Server.HtmlDecode(pre.Response_UserMsgBody);

                                try
                                {
                                    if (!pre.SendAdminEmail)
                                    {
                                        rbAdminNotificationOther.ClearSelection();

                                        if (pre.SendAdminEmail)
                                        {
                                            pnlAdminNotificationOther.Visible = true;
                                            rbAdminNotificationOther.Items.FindByValue("true").Selected = true;
                                        }
                                        else
                                        {
                                            pnlAdminNotificationOther.Visible = false;
                                            rbAdminNotificationOther.Items.FindByValue("false").Selected = true;
                                        }
                                    }
                                    else
                                    {
                                        rbAdminNotificationOther.Items.FindByValue("false").Selected = true;
                                    }
                                }
                                catch
                                { }

                                txtAdminEmailOther.Text = pre.Response_AdminEmail;
                                txtAdminEmailSubOther.Text = pre.Response_AdminMsgSubject;
                                RadEditorAdminEmailBodyOther.Content = Server.HtmlDecode(pre.Response_AdminMsgBody);

                                if (!responseStatus)
                                {
                                    MnuResponseType.Items[5].Selected = true;
                                    multiViewResponseType.ActiveViewIndex = 5;

                                    pnlFromOther.Visible = (pnlNonQualResponse.Visible || pnlNQResponseCountry.Visible || pnlUserNotificationOther.Visible || pnlAdminNotificationOther.Visible);
                                    responseStatus = pnlFromOther.Visible;
                                    ActiveResponseEmailTab = multiViewResponseType.ActiveViewIndex;
                                }
                                #endregion OTHER
                            }
                            else if (ResponseEmailkvp.Key.Equals(NotificatonFor.Newsletter))
                            {
                                #region Newsletter

                                FromNameNewsletter.Text = pre.Response_FromName;
                                txtFormEmailNewsletter.Text = pre.Response_FromEmail;

                                try
                                {
                                    rbNewletterNotification.ClearSelection();

                                    if (pre.SendUserEmail)
                                    {
                                        pnlNewsLetterNotification.Visible = true;
                                        rbNewletterNotification.Items.FindByValue("true").Selected = true;
                                    }
                                    else
                                    {
                                        pnlNewsLetterNotification.Visible = false;
                                        rbNewletterNotification.Items.FindByValue("false").Selected = true;
                                    }
                                }
                                catch
                                { }

                                txtNewsLetterSub.Text = pre.Response_UserMsgSubject;
                                RadEditorNewsLetterBody.Content = Server.HtmlDecode(pre.Response_UserMsgBody);

                                if (!responseStatus)
                                {
                                    MnuResponseType.Items[6].Selected = true;
                                    multiViewResponseType.ActiveViewIndex = 6;

                                    responseStatus = pnlNewsLetterNotification.Visible;
                                    ActiveResponseEmailTab = multiViewResponseType.ActiveViewIndex;
                                }

                                #endregion OTHER
                            }
                        }
                    }
                    catch { }

                    SqlCommand cmdFormFields = new SqlCommand("select PSubField.PSFieldID, PSubField.ECNFieldName, Grouping from PubFormFields PForm join PubSubscriptionFields PSubField on PForm.PSFieldID = PSubField.PSFieldID  where PFID = @PFID and ISActive = 1  order by psubfield.grouping, pform.sortorder");
                    cmdFormFields.CommandType = CommandType.Text;
                    cmdFormFields.Parameters.Add(new SqlParameter("@PFID", PFID));
                    DataTable dtFormFields = DataFunctions.GetDataTable(cmdFormFields);

                    foreach (DataRow dr in dtFormFields.Rows)
                    {
                        if (dr["Grouping"].ToString().ToUpper() == "P")
                        {
                            if (!aProfileFields.Contains(dr["PSFieldID"].ToString()))
                                aProfileFields.Add(dr["PSFieldID"].ToString());
                        }
                        else
                            aDemographicFields.Add(dr["PSFieldID"].ToString());
                    }

                    LoadFormFields();
                    MultiViewForms.ActiveViewIndex = 0;
                }

                bool NonQualSetup = dtform.Rows[0].IsNull("IsNonQualSetup") ? false : Convert.ToBoolean(dtform.Rows[0]["IsNonQualSetup"]);

                if (NonQualSetup)
                {
                    rbNonQualSetup.Items.FindByValue("true").Selected = true;
                    txtNQPrintRedirectUrl.Text = dtform.Rows[0]["NQPrintRedirectUrl"].ToString().Trim();
                    txtNQDigitalRedirectUrl.Text = dtform.Rows[0]["NQDigitalRedirectUrl"].ToString().Trim();
                    txtNQBothRedirectUrl.Text = dtform.Rows[0]["NQBothRedirectUrl"].ToString().Trim();
                    txtPaidPageLink.Text = dtform.Rows[0]["PaidPageURL"].ToString().Trim();

                    rblDisableNonQualSetup.ClearSelection();

                    if (Convert.ToBoolean(dtform.Rows[0]["DisableNonQualDigital"].ToString()))
                    {
                        rblDisableNonQualSetup.Items.FindByValue("true").Selected = true;
                        pnlNQDigitalRedirectUrl.Visible = false;
                        txtNQDigitalRedirectUrl.Text = "";
                    }
                    else
                    {
                        rblDisableNonQualSetup.Items.FindByValue("false").Selected = true;
                    }

                    rbNQOverridePrintAsDigital.ClearSelection();

                    if (Convert.ToBoolean(dtform.Rows[0]["ShowNQPrintAsDigital"].ToString()))
                    {
                        rbNQOverridePrintAsDigital.Items.FindByValue("true").Selected = true;
                    }
                    else
                    {
                        rbNQOverridePrintAsDigital.Items.FindByValue("false").Selected = true;
                    }



                    rbSuspendECNPostforBoth.ClearSelection();

                    if (!dtform.Rows[0].IsNull("SuspendECNPostforBoth"))
                    {
                        if (Convert.ToBoolean(dtform.Rows[0]["SuspendECNPostforBoth"].ToString()))
                        {
                            rbSuspendECNPostforBoth.Items.FindByValue("true").Selected = true;
                        }
                        else
                        {
                            rbSuspendECNPostforBoth.Items.FindByValue("false").Selected = true;
                        }
                    }
                    else
                    {

                    }

                    pnlNonQual.Visible = true;
                }
                else
                {
                    rbNonQualSetup.Items.FindByValue("false").Selected = true;
                    pnlNonQual.Visible = false;
                }


                SqlCommand cmdThankYou = new SqlCommand("SELECT pfl.PrintThankYouPageHTML,pfl.DigitalThankYouPageHTML,pfl.DefaultThankYouPageHTML, pfl.NQResponsePageHTML, pfl.NQResponseCounPageHTML " +
                                                                        " from PubFormLandingPages pfl join PubForms pf on pfl.PFID=pf.PFID join Publications pub on pf.PubID=pub.PubID" +
                                                                        " where pub.PubID= @PubID and pfl.PFID=@PFID");
                cmdThankYou.CommandType = CommandType.Text;
                cmdThankYou.Parameters.Add(new SqlParameter("@PubID", PubID));
                cmdThankYou.Parameters.Add(new SqlParameter("@PFID", PFID));
                DataTable dtThankYou = DataFunctions.GetDataTable(cmdThankYou);

                if (dtThankYou.Rows.Count > 0)
                {
                    RadEditorPrintThankYou.Content = dtThankYou.Rows[0]["PrintThankYouPageHTML"].ToString();
                    RadEditorDigitalThankYou.Content = dtThankYou.Rows[0]["DigitalThankYouPageHTML"].ToString();
                    RadEditorDefaultThankYou.Content = dtThankYou.Rows[0]["DefaultThankYouPageHTML"].ToString();
                    RadEditorNQResponseThankYou.Content = dtThankYou.Rows[0]["NQResponsePageHTML"].ToString();
                    RadEditorNQCountryThankyou.Content = dtThankYou.Rows[0]["NQResponseCounPageHTML"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadFormFields()
        {
            string profilefieldIDs = "0"; // Added 0 to get Empty DataTable to bind  
            string demofieldIDs = "0"; // Added 0 to get Empty DataTable to bind     

            for (int i = 0; i < aProfileFields.Count; i++)
            {
                profilefieldIDs += "," + aProfileFields[i].ToString();
            }

            for (int i = 0; i < aDemographicFields.Count; i++)
            {
                demofieldIDs += "," + aDemographicFields[i].ToString();
            }

            SqlCommand cmdProfileFields = new SqlCommand("select PSF.*, PFF.PFID, PFF.sortorder, pff.AddSeparator, isnull(pff.SeparatorType,'N') as SeparatorType from PubSubscriptionFields PSF left outer join pubformfields pff on PSF.PSFieldID = PFF.PSFieldID and PFF.PFID = @PFID where PSF.PubID = @PubID and PSF.PSFieldID in ( select items from dbo.fn_Split (@ProfileFieldIDs,',') ) and ISActive = 1");
            cmdProfileFields.CommandType = CommandType.Text;
            cmdProfileFields.Parameters.Add(new SqlParameter("@PFID", SqlDbType.Int)).Value = PFID;
            cmdProfileFields.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = PubID;
            cmdProfileFields.Parameters.Add(new SqlParameter("@ProfileFieldIDs", SqlDbType.VarChar)).Value = profilefieldIDs;
            DataTable dtProfileFields = DataFunctions.GetDataTable(cmdProfileFields);

            foreach (DataRow dr in dtProfileFields.Rows)
            {
                dr["sortorder"] = getIndex(aProfileFields, dr["PSFieldID"].ToString());
            }

            dtProfileFields.DefaultView.Sort = "sortorder";
            grdProfileFields.DataSource = dtProfileFields;
            grdProfileFields.DataBind();

            SqlCommand cmdDemoGraphicFields = new SqlCommand("select PSF.*, PFF.PFID, PFF.sortorder, pff.AddSeparator, isnull(pff.SeparatorType,'N') as SeparatorType from PubSubscriptionFields PSF left outer join pubformfields pff on PSF.PSFieldID = PFF.PSFieldID and PFF.PFID = @PFID where PSF.PSFieldID in ( select items from dbo.fn_Split (@demofieldIDs, ',') ) and ISActive = 1");
            cmdDemoGraphicFields.CommandType = CommandType.Text;
            cmdDemoGraphicFields.Parameters.Add(new SqlParameter("@PFID", SqlDbType.Int)).Value = PFID;
            cmdDemoGraphicFields.Parameters.Add(new SqlParameter("@demofieldIDs", SqlDbType.VarChar)).Value = demofieldIDs;
            DataTable dtDemoGraphicFields = DataFunctions.GetDataTable(cmdDemoGraphicFields);

            foreach (DataRow dr in dtDemoGraphicFields.Rows)
            {
                dr["sortorder"] = getIndex(aDemographicFields, dr["PSFieldID"].ToString());
            }

            dtDemoGraphicFields.DefaultView.Sort = "sortorder";
            grdDemoGraphicFields.DataSource = dtDemoGraphicFields;
            grdDemoGraphicFields.DataBind();

            SqlCommand cmdEcnField = new SqlCommand("SELECT PSFieldID, ECNFieldName from PubSubscriptionFields where PubID=@PubID and PSFieldID not in (select items from dbo.fn_Split(@profiledemoids,','))");
            cmdEcnField.CommandType = CommandType.Text;
            string profiledemoids = profilefieldIDs + "," + demofieldIDs;
            cmdEcnField.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = PubID;
            cmdEcnField.Parameters.Add(new SqlParameter("@profiledemoids", SqlDbType.VarChar)).Value = profiledemoids;
            drpFields.DataSource = DataFunctions.GetDataTable(cmdEcnField);
            drpFields.DataTextField = "ECNFieldName";
            drpFields.DataValueField = "PSFieldID";
            drpFields.DataBind();

            lnkpopup.HRef = "NewPub_FieldForm.aspx?PubId=" + PubID.ToString();
        }

        private string getIndex(ArrayList al, string key)
        {

            for (int i = 0; i < al.Count; i++)
            {
                if (key == al[i].ToString())
                    return (i + 1).ToString();
            }

            return "0";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PublisherList.aspx");
        }

        protected void MnuForm_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                if (Save(ActiveTab))
                {
                    ActiveTab = Convert.ToInt32(e.Item.Value);
                    LoadTab();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (Save(ActiveTab))
                {
                    ActiveTab = ActiveTab + 1;
                    LoadTab();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveTab = ActiveTab - 1;
                LoadTab();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            try
            {
                if (Save(ActiveTab))
                    Response.Redirect("Pub_Forms.aspx?PubId=" + PubID.ToString() + "&PubName=" + PubCode);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }


        private bool Save(int step)
        {
            if (ValidatePage(step))
            {
                try
                {
                    switch (step)
                    {
                        case 0:
                            #region STEP - Form Details

                            if ((RadEditorSUBSCRIPTIONQuestion.Content.Trim() == string.Empty) || RadEditorPRINTDIGITALQuestion.Content.Trim() == string.Empty)
                            {
                                throw new Exception("SUBSCRIPTION or PRINT/DIGITAL Question is required.");
                            }

                            string strPSFields = string.Empty;
                            string strCountryId = "";

                            foreach (ListItem ls in lstDestination.Items)
                            {
                                strCountryId = strCountryId + ls.Value + ",";
                            }

                            string strNewsLetterId = "";
                            foreach (ListItem lsNews in lstDestinationNewsLetter.Items)
                            {
                                strNewsLetterId = strNewsLetterId + lsNews.Value + ",";
                            }

                            SqlCommand cmd = new SqlCommand("sp_SavePubForm");
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = PubID;
                            cmd.Parameters.Add(new SqlParameter("@PFID", SqlDbType.Int)).Value = PFID;
                            cmd.Parameters.Add(new SqlParameter("@FormName", SqlDbType.VarChar)).Value = txtFormName.Text;
                            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = txtDescription.Text;
                            cmd.Parameters.Add(new SqlParameter("@ShowPrint", SqlDbType.Bit)).Value = rbshowprint.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@ShowDigital", SqlDbType.Bit)).Value = rbshowdigital.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@ShowPrintAsDigital", SqlDbType.Bit)).Value = rbsShowPrintasDigital.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@IsPaid", SqlDbType.Bit)).Value = rbpaidsub.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@PaidPrice", SqlDbType.Decimal)).Value = (PaypalPaidCost.Text == "" ? "0" : PaypalPaidCost.Text);
                            cmd.Parameters.Add(new SqlParameter("@NewsletterPosition", SqlDbType.Char)).Value = drpNewsletterPosition.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.VarChar)).Value = (strCountryId == "" ? "0" : strCountryId);
                            cmd.Parameters.Add(new SqlParameter("@NewsLetterId", SqlDbType.VarChar)).Value = (strNewsLetterId == "" ? "0" : strNewsLetterId);
                            cmd.Parameters.Add(new SqlParameter("@ShowNewsletterAsCollapsed", SqlDbType.Bit)).Value = rbNewsletterCollapsible.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@ShowNewsletterSearch", SqlDbType.Bit)).Value = rbNewsletterSearch.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@PreSelectNewsletters", SqlDbType.Bit)).Value = rbPreSelectNewsletters.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = jfsess.UserName();
                            cmd.Parameters.Add(new SqlParameter("@SUBSCRIPTIONQuestion", SqlDbType.VarChar)).Value = RadEditorSUBSCRIPTIONQuestion.Content;
                            cmd.Parameters.Add(new SqlParameter("@PRINTDIGITALQuestion", SqlDbType.VarChar)).Value = RadEditorPRINTDIGITALQuestion.Content;
                            cmd.Parameters.Add(new SqlParameter("@ShowNQPrintAsDigital", SqlDbType.Bit)).Value = rbNQOverridePrintAsDigital.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@EnablePrintAndDigital", SqlDbType.Bit)).Value = rblstPrintDigital.SelectedItem.Value;
                            cmd.Parameters.Add(new SqlParameter("@SuspendECNPostforBoth", SqlDbType.Bit)).Value = rbSuspendECNPostforBoth.SelectedItem.Value;



                            //if (PFID != 0)
                            //DataFunctions.ExecuteScalar("Delete from PubFormFields where PFID=" + PFID.ToString()); 

                            PFID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));

                            foreach (GridViewRow gvr in grdProfileFields.Rows)
                            {
                                DropDownList drpSeparator = (DropDownList)gvr.FindControl("drpSeparator");

                                SqlCommand cmdpfields = new SqlCommand("sp_SavePubFormFields");
                                cmdpfields.CommandType = CommandType.StoredProcedure;
                                cmdpfields.CommandTimeout = 0;

                                cmdpfields.Parameters.Add(new SqlParameter("@PFID", SqlDbType.Int)).Value = PFID;
                                cmdpfields.Parameters.Add(new SqlParameter("@PSFieldID", SqlDbType.Int)).Value = grdProfileFields.DataKeys[gvr.RowIndex].Value;
                                cmdpfields.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.Int)).Value = gvr.RowIndex + 1;

                                if (drpSeparator.SelectedItem.Value == "N")
                                {
                                    cmdpfields.Parameters.Add(new SqlParameter("@AddSeparator", SqlDbType.Bit)).Value = 0;
                                }
                                else
                                {
                                    cmdpfields.Parameters.Add(new SqlParameter("@AddSeparator", SqlDbType.Bit)).Value = 1;
                                }

                                cmdpfields.Parameters.Add(new SqlParameter("@SeparatorType", SqlDbType.VarChar)).Value = drpSeparator.SelectedItem.Value;
                                cmdpfields.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = jfsess.UserName();

                                strPSFields += (strPSFields == string.Empty ? grdProfileFields.DataKeys[gvr.RowIndex].Value : "," + grdProfileFields.DataKeys[gvr.RowIndex].Value);

                                DataFunctions.Execute(cmdpfields);
                            }

                            foreach (GridViewRow gvr in grdDemoGraphicFields.Rows)
                            {
                                DropDownList drpSeparator = (DropDownList)gvr.FindControl("drpSeparator");

                                SqlCommand cmddfields = new SqlCommand("sp_SavePubFormFields");
                                cmddfields.CommandType = CommandType.StoredProcedure;
                                cmddfields.CommandTimeout = 0;

                                cmddfields.Parameters.Add(new SqlParameter("@PFID", SqlDbType.Int)).Value = PFID;
                                cmddfields.Parameters.Add(new SqlParameter("@PSFieldID", SqlDbType.Int)).Value = grdDemoGraphicFields.DataKeys[gvr.RowIndex].Value;
                                cmddfields.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.Int)).Value = gvr.RowIndex + 1;

                                if (drpSeparator.SelectedItem.Value == "N")
                                {
                                    cmddfields.Parameters.Add(new SqlParameter("@AddSeparator", SqlDbType.Bit)).Value = 0;
                                }
                                else
                                {
                                    cmddfields.Parameters.Add(new SqlParameter("@AddSeparator", SqlDbType.Bit)).Value = 1;
                                }

                                cmddfields.Parameters.Add(new SqlParameter("@SeparatorType", SqlDbType.VarChar)).Value = drpSeparator.SelectedItem.Value;
                                cmddfields.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = jfsess.UserName();

                                strPSFields += (strPSFields == string.Empty ? grdDemoGraphicFields.DataKeys[gvr.RowIndex].Value : "," + grdDemoGraphicFields.DataKeys[gvr.RowIndex].Value);

                                DataFunctions.Execute(cmddfields);
                            }

                            if (strPSFields != string.Empty)
                            {
                                DataFunctions.Execute("delete from PubFormFields where PFID= " + PFID + " and PSFieldID not in (" + strPSFields + ")");

                            }

                            break;
                            #endregion
                        case 1:

                            break;

                        case 2:
                            SaveResponseEmail(ActiveResponseEmailTab);
                            break;

                        case 3:

                            string strNonQualCtry = string.Empty;

                            SqlCommand cmddNonQualFields = new SqlCommand("sp_UpdateNonQualSettings");
                            cmddNonQualFields.CommandType = CommandType.StoredProcedure;
                            cmddNonQualFields.CommandTimeout = 0;

                            if (Convert.ToBoolean(rbNonQualSetup.SelectedValue))
                            {
                                if (txtNQPrintRedirectUrl.Text.Length > 1000)
                                    throw new Exception("Print Non Qualification Page URL should be less than 1000 characters");

                                if (txtNQDigitalRedirectUrl.Text.Length > 1000)
                                    throw new Exception("Digital Non Qualification Page URL should be less than 1000 characters");

                                if (txtNQBothRedirectUrl.Text.Length > 1000)
                                    throw new Exception("Both Non Qualification Page URL should be less than 1000 characters");

                                if (txtPaidPageLink.Text.Length > 1000)
                                    throw new Exception("Paid Page URL should be less than 1000 characters");

                                cmddNonQualFields.Parameters.Add(new SqlParameter("@NQPrintRedirectUrl", SqlDbType.VarChar)).Value = txtNQPrintRedirectUrl.Text;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@NQDigitalRedirectUrl", SqlDbType.VarChar)).Value = txtNQDigitalRedirectUrl.Text;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@NQBothRedirectUrl", SqlDbType.VarChar)).Value = txtNQBothRedirectUrl.Text;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@IsNonQualSetup", SqlDbType.Bit)).Value = true;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@DisableForDigital", SqlDbType.Bit)).Value = rblDisableNonQualSetup.SelectedItem.Value;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@ShowNQPrintAsDigital", SqlDbType.Bit)).Value = rbNQOverridePrintAsDigital.SelectedItem.Value;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@SuspendECNPostforBoth", SqlDbType.Bit)).Value = rbSuspendECNPostforBoth.SelectedItem.Value;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@PaidPageURL", SqlDbType.VarChar)).Value = txtPaidPageLink.Text;
                            }
                            else
                            {
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@NQPrintRedirectUrl", SqlDbType.VarChar)).Value = string.Empty;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@NQDigitalRedirectUrl", SqlDbType.VarChar)).Value = string.Empty;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@NQBothRedirectUrl", SqlDbType.VarChar)).Value = string.Empty;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@IsNonQualSetup", SqlDbType.Bit)).Value = false;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@DisableForDigital", SqlDbType.Bit)).Value = false;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@ShowNQPrintAsDigital", SqlDbType.Bit)).Value = false;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@SuspendECNPostforBoth", SqlDbType.Bit)).Value = false;
                                cmddNonQualFields.Parameters.Add(new SqlParameter("@PaidPageURL", SqlDbType.VarChar)).Value = string.Empty;
                                lstDestinationNonQual.Items.Clear();
                            }


                            foreach (ListItem li in lstDestinationNonQual.Items)
                            {
                                strNonQualCtry = strNonQualCtry + li.Value + ",";
                            }

                            cmddNonQualFields.Parameters.Add(new SqlParameter("@pfid", SqlDbType.Int)).Value = PFID;
                            cmddNonQualFields.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.VarChar)).Value = strNonQualCtry;

                            DataFunctions.Execute(cmddNonQualFields);
                            break;

                        case 4:
                            using (SqlCommand cmdThankYouPage = new SqlCommand("sp_UpdatePubFormLandingPages"))
                            {
                                cmdThankYouPage.CommandType = CommandType.StoredProcedure;
                                cmdThankYouPage.CommandTimeout = 0;

                                cmdThankYouPage.Parameters.AddWithValue("@PFID", PFID);
                                cmdThankYouPage.Parameters.AddWithValue("@PrintThankYouPageHTML", RadEditorPrintThankYou.Content);
                                cmdThankYouPage.Parameters.AddWithValue("@DigitalThankYouPageHTML", RadEditorDigitalThankYou.Content);
                                cmdThankYouPage.Parameters.AddWithValue("@DefaultThankYouPageHTML", RadEditorDefaultThankYou.Content);
                                cmdThankYouPage.Parameters.AddWithValue("@NQResponsePageHTML", RadEditorNQResponseThankYou.Content);
                                cmdThankYouPage.Parameters.AddWithValue("@NQResponseCounPageHTML", RadEditorNQCountryThankyou.Content);

                                DataFunctions.Execute(cmdThankYouPage);
                            }
                            break;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MultiViewForms.ActiveViewIndex = step;
                    MnuForm.Items[step].Selected = true;
                    throw ex;
                }
            }
            else
            {
                return false;
            }
        }

        private void SavePubResponseEmails(int pfid, string responseType, string responseFromName, string responseFromEmail, string sendUserEmail, string Response_UserMsgSubject, string Response_UserMsgBody, string SendAdminEmail, string Response_AdminEmail, string Response_AdminMsgSubject, string Response_AdminMsgBody, string Response_UserMsgNQRespSub, string Response_UserMsgNQRespMsgBody, string SendNQRespEmail)
        {
            SqlCommand cmdResponse = new SqlCommand("sp_SaveFormResponseEmails");
            cmdResponse.CommandType = CommandType.StoredProcedure;
            cmdResponse.CommandTimeout = 0;

            cmdResponse.Parameters.Add(new SqlParameter("@PFID", SqlDbType.Int)).Value = pfid;
            cmdResponse.Parameters.Add(new SqlParameter("@ResponseType", SqlDbType.VarChar)).Value = responseType;
            cmdResponse.Parameters.Add(new SqlParameter("@Response_FromName", SqlDbType.VarChar)).Value = responseFromName;
            cmdResponse.Parameters.Add(new SqlParameter("@Response_FromEmail", SqlDbType.VarChar)).Value = responseFromEmail;

            cmdResponse.Parameters.Add(new SqlParameter("@SendUserEmail ", SqlDbType.Bit)).Value = Convert.ToBoolean(sendUserEmail);

            cmdResponse.Parameters.Add(new SqlParameter("@Response_UserMsgSubject", SqlDbType.VarChar)).Value = Response_UserMsgSubject;
            cmdResponse.Parameters.Add(new SqlParameter("@Response_UserMsgBody", SqlDbType.Text)).Value = Response_UserMsgBody;

            cmdResponse.Parameters.Add(new SqlParameter("@SendAdminEmail", SqlDbType.Bit)).Value = Convert.ToBoolean(SendAdminEmail);

            cmdResponse.Parameters.Add(new SqlParameter("@Response_AdminEmail", SqlDbType.VarChar)).Value = Response_AdminEmail;
            cmdResponse.Parameters.Add(new SqlParameter("@Response_AdminMsgSubject", SqlDbType.VarChar)).Value = Response_AdminMsgSubject;
            cmdResponse.Parameters.Add(new SqlParameter("@Response_AdminMsgBody", SqlDbType.Text)).Value = Response_AdminMsgBody;

            cmdResponse.Parameters.Add(new SqlParameter("@Response_UserMsgNQRespSub", SqlDbType.VarChar)).Value = Response_UserMsgNQRespSub;
            cmdResponse.Parameters.Add(new SqlParameter("@Response_UserMsgNQRespMsgBody", SqlDbType.Text)).Value = Response_UserMsgNQRespMsgBody;

            cmdResponse.Parameters.Add(new SqlParameter("@SendNQRespEmail", SqlDbType.Bit)).Value = Convert.ToBoolean(SendNQRespEmail);

            DataFunctions.Execute(cmdResponse);
        }


        private void LoadTab()
        {

            MultiViewForms.ActiveViewIndex = ActiveTab;
            MnuForm.Items[ActiveTab].Selected = true;

            switch (ActiveTab)
            {
                case 0:
                    lstSource.DataBind();
                    lstDestination.DataBind();
                    btnPrevious.Visible = false;
                    btnNext.Visible = true;
                    break;
                case 1:
                    SqlCommand cmdgrdRulesDS = new SqlCommand("select distinct case when fs.PFFieldID IS NOT NULL THEN 'Y' else 'N' end as IsSelected ,PFF.PFID, PFF.PFFieldID,PFF.sortorder, PSF.* from pubformfields pff join PubSubscriptionFields PSF on PFF.PSFieldID = PSF.PSFieldID left outer join Fieldsettings fs on fs.pffieldID = pff.PFFieldID where PFF.PFID = @PFID and isactive=1 order by grouping desc, pff.sortorder");
                    cmdgrdRulesDS.CommandType = CommandType.Text;
                    cmdgrdRulesDS.Parameters.Add(new SqlParameter("@PFID", PFID));
                    grdRules.DataSource = DataFunctions.GetDataTable(cmdgrdRulesDS);
                    grdRules.DataBind();
                    btnPrevious.Visible = true;
                    btnNext.Visible = true;
                    break;
                case 2:
                    btnPrevious.Visible = true;
                    btnNext.Visible = true;
                    break;
                case 3:
                    SqlCommand cmdgrdNonQual = new SqlCommand("select distinct case when fs.PFFieldID IS NOT NULL THEN 'Y' else 'N' end as IsSelected ,PFF.PFID, PFF.PFFieldID,PFF.sortorder, PSF.* from pubformfields pff join PubSubscriptionFields PSF on PFF.PSFieldID = PSF.PSFieldID left outer join NonQualsettings fs on fs.pffieldID = pff.PFFieldID where PFF.PFID = @PFID and isactive=1 and ControlType not in ('TextBox','Hidden') order by grouping desc, pff.sortorder");
                    cmdgrdNonQual.CommandType = CommandType.Text;
                    cmdgrdNonQual.Parameters.Add(new SqlParameter("@PFID", PFID.ToString()));
                    grdNonQual.DataSource = DataFunctions.GetDataTable(cmdgrdNonQual);
                    grdNonQual.DataBind();
                    lstSourceNonQual.DataBind();
                    lstDestinationNonQual.DataBind();
                    grdNonQual.Visible = true;
                    btnPrevious.Visible = true;
                    btnNext.Visible = true;
                    break;
                case 4:
                    btnPrevious.Visible = true;
                    btnNext.Visible = false;
                    break;
            }
        }

        private bool ValidatePage(int ActiveTab)
        {
            int savedIndex = MultiViewForms.ActiveViewIndex;

            MultiViewForms.ActiveViewIndex = ActiveTab;
            Page.Validate();

            if (!Page.IsValid)
            {
                MultiViewForms.ActiveViewIndex = ActiveTab;
                MnuForm.Items[ActiveTab].Selected = true;
                return false;
            }

            return true;
        }

        private bool ChkExistForm()
        {
            SqlCommand cmdFormField = new SqlCommand("Select PFID from PubForms where PFID <> @PFID and FormName=@FromName and PubId=@PubId");
            cmdFormField.Parameters.Add(new SqlParameter("@PFID", PFID));
            cmdFormField.Parameters.Add(new SqlParameter("@FromName", txtFormName.Text.Trim().Replace("'", "''")));
            cmdFormField.Parameters.Add(new SqlParameter("@PubId", PubID));
            DataTable dtFormField = DataFunctions.GetDataTable(cmdFormField);

            if (dtFormField.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        #region listbox events
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstSource, lstDestination, false);
        }


        protected void btnRemove_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstDestination, lstSource, false);
        }

        protected void btnNewsletterAdd_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstSourceNewsLetter, lstDestinationNewsLetter, false);
        }

        protected void btnNewsletterRemove_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstDestinationNewsLetter, lstSourceNewsLetter, false);
        }

        private void moveSelectedItems(ListBox source, ListBox target, bool moveAllItems)
        {
            for (int i = source.Items.Count - 1; i >= 0; i--)
            {
                ListItem item = source.Items[i];
                if (item.Selected)
                {
                    target.Items.Add(item);
                    item.Selected = false;
                    source.Items.Remove(item);
                }
            }
        }

        #endregion


        protected void grdProfileFields_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "FormDelete")
            {
                aProfileFields.Remove(e.CommandArgument);
            }
            else if (e.CommandName.ToUpper() == "UP")
            {
                int newindex = aProfileFields.IndexOf(e.CommandArgument) - 1;
                aProfileFields.Remove(e.CommandArgument);
                aProfileFields.Insert(newindex, e.CommandArgument);
            }
            else if (e.CommandName.ToUpper() == "DOWN")
            {
                int newindex = aProfileFields.IndexOf(e.CommandArgument) + 1;
                aProfileFields.Remove(e.CommandArgument);
                aProfileFields.Insert(newindex, e.CommandArgument);
            }

            LoadFormFields();
        }

        protected void grdDemoGraphicFields_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "FormDelete")
            {
                
                SqlCommand cmdDeleteCheck = new SqlCommand();
                cmdDeleteCheck.CommandType = CommandType.Text;
                cmdDeleteCheck.CommandText = @"if exists (select top 1 * 
                                                                            from PubFormFields pff with(nolock)
                                                                            join NonQualSettings nq with(nolock) on pff.PFFieldID = nq.PFFieldID
                                                                            where pff.PSFieldID in (" + e.CommandArgument + @") and pff.PFID = " + PFID + @")
                                                               SELECT 1
                                                               else
                                                               SELECT 0";
                if (Convert.ToInt32(DataFunctions.ExecuteScalar(cmdDeleteCheck).ToString()) > 0)
                {
                    lblMessage.Text = "This field has non qualified responses.  Please remove the non qualified responses and then delete the field.";
                }
                else
                {
                    SqlCommand cmdDeleteCheckBranch = new SqlCommand();
                    cmdDeleteCheckBranch.CommandType = CommandType.Text;
                    cmdDeleteCheckBranch.CommandText = @"if exists (select top 1 * 
                                                                            from fieldsettings fs with(nolock)          
                                                                           join PubFormFields pff with(nolock) on fs.PFFieldID = pff.PFFieldID or fs.PFFReferenceID = pff.PFFieldID                                                                   
                                                                            where pff.PSFieldID in (" + e.CommandArgument + ") and pff.PFID = " + PFID + @" )
                                                               SELECT 1
                                                               else
                                                               SELECT 0";
                    if (Convert.ToInt32(DataFunctions.ExecuteScalar(cmdDeleteCheckBranch).ToString()) > 0)
                    {
                        lblMessage.Text = "This field has branching logic.  Please remove the branching rules and then delete the field.";
                    }
                    else
                    {
                        aDemographicFields.Remove(e.CommandArgument);
                    }
                    
                }

                
            }
            else if (e.CommandName.ToUpper() == "UP")
            {
                int newindex = aDemographicFields.IndexOf(e.CommandArgument) - 1;
                aDemographicFields.Remove(e.CommandArgument);
                aDemographicFields.Insert(newindex, e.CommandArgument);
            }
            else if (e.CommandName.ToUpper() == "DOWN")
            {
                int newindex = aDemographicFields.IndexOf(e.CommandArgument) + 1;
                aDemographicFields.Remove(e.CommandArgument);
                aDemographicFields.Insert(newindex, e.CommandArgument);
            }

            LoadFormFields();
        }

        protected void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (drpFields.SelectedIndex != -1)
            {
                SqlCommand cmdCheckActive = new SqlCommand("select IsActive from PubSubscriptionFields where PSFieldID = @PSFieldID");
                cmdCheckActive.CommandType = CommandType.Text;
                cmdCheckActive.Parameters.Add(new SqlParameter("@PSFieldID", drpFields.SelectedItem.Value));

                if (!Convert.ToBoolean(DataFunctions.ExecuteScalar(cmdCheckActive)))
                {
                    lnkEditInactiveFields.HRef = "NewPub_FieldForm.aspx?PubID=" + PubID + "&PFID=" + PFID + "&PSFieldID=" + drpFields.SelectedItem.Value;
                    lblInActiveMsg.Text = "Selected field is InActive. Edit the field to make it Active.";
                    pnlEditInactiveFields.Visible = true;
                }
                else
                {
                    AddFieldtoSession(Convert.ToInt32(drpFields.SelectedItem.Value));
                    pnlEditInactiveFields.Visible = false;
                }

                string psfid = drpFields.SelectedItem.Value;

                LoadFormFields();

                if (drpFields.Items.FindByValue(psfid) != null)
                {
                    drpFields.Items[drpFields.Items.IndexOf(drpFields.Items.FindByValue(psfid))].Selected = true;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:alert('Fields are Empty.')", true);
            }
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["PSFieldID"] != null && Convert.ToInt32(Session["PSFieldID"]) > 0)
                {
                    AddFieldtoSession(Convert.ToInt32(Session["PSFieldID"]));
                    LoadFormFields();
                    pnlEditInactiveFields.Visible = false;
                    Session["PSFieldID"] = "0";
                }

                SqlCommand cmdGrdRules = new SqlCommand("select distinct case when fs.PFFieldID IS NOT NULL THEN 'Y' else 'N' end as IsSelected ,PFF.PFID, PFF.PFFieldID,PFF.sortorder, PSF.* from pubformfields pff join PubSubscriptionFields PSF on PFF.PSFieldID = PSF.PSFieldID left outer join Fieldsettings fs on fs.pffieldID = pff.PFFieldID where PFF.PFID = @PFID and isactive=1 order by grouping desc, pff.sortorder");
                cmdGrdRules.CommandType = CommandType.Text;
                cmdGrdRules.Parameters.Add(new SqlParameter("@PFID", PFID));
                grdRules.DataSource = DataFunctions.GetDataTable(cmdGrdRules);
                grdRules.DataBind();

                SqlCommand cmdGrdNonQual = new SqlCommand("select distinct case when fs.PFFieldID IS NOT NULL THEN 'Y' else 'N' end as IsSelected ,PFF.PFID, PFF.PFFieldID,PFF.sortorder, PSF.* from pubformfields pff join PubSubscriptionFields PSF on PFF.PSFieldID = PSF.PSFieldID left outer join NonQualsettings fs on fs.pffieldID = pff.PFFieldID where PFF.PFID = @PFID and isactive=1 and ControlType not in ('TextBox','Hidden') order by grouping desc, pff.sortorder");
                cmdGrdNonQual.CommandType = CommandType.Text;
                cmdGrdNonQual.Parameters.Add(new SqlParameter("@PFID", PFID));
                grdNonQual.DataSource = DataFunctions.GetDataTable(cmdGrdNonQual);
                grdNonQual.DataBind();
            }
            catch
            { }
        }

        private void AddFieldtoSession(int FieldID)
        {
            SqlCommand cmdFieldtoSession = new SqlCommand("select PSF.*, 0 as sortorder from PubSubscriptionFields PSF where PSFieldID=@PSFieldID");
            cmdFieldtoSession.Parameters.Add(new SqlParameter("@PSFieldID", FieldID));
            cmdFieldtoSession.CommandType = CommandType.Text;

            DataTable dt = DataFunctions.GetDataTable(cmdFieldtoSession);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Grouping"].ToString().ToUpper() == "P")
                {
                    if (!aProfileFields.Contains(dt.Rows[0]["PSFieldID"].ToString()))
                        aProfileFields.Add(dt.Rows[0]["PSFieldID"].ToString());

                    if (aDemographicFields.Contains(dt.Rows[0]["PSFieldID"].ToString()))
                        aDemographicFields.Remove(dt.Rows[0]["PSFieldID"].ToString());
                }
                else
                {
                    if (aProfileFields.Contains(dt.Rows[0]["PSFieldID"].ToString()))
                        aProfileFields.Remove(dt.Rows[0]["PSFieldID"].ToString());

                    if (!aDemographicFields.Contains(dt.Rows[0]["PSFieldID"].ToString()))
                        aDemographicFields.Add(dt.Rows[0]["PSFieldID"].ToString());
                }

                //Remove inactive fields from the list

                if (!Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                {
                    if (dt.Rows[0]["Grouping"].ToString().ToUpper() == "P")
                    {
                        if (aProfileFields.Contains(dt.Rows[0]["PSFieldID"].ToString()))
                            aProfileFields.Remove(dt.Rows[0]["PSFieldID"].ToString());
                    }
                    else
                    {
                        if (aDemographicFields.Contains(dt.Rows[0]["PSFieldID"].ToString()))
                            aDemographicFields.Remove(dt.Rows[0]["PSFieldID"].ToString());
                    }
                }
            }
        }

        protected void grdProfileFields_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    ImageButton btnUP = (ImageButton)e.Row.FindControl("btnup");
                    btnUP.Visible = false;
                }

                if (e.Row.RowIndex == aProfileFields.Count - 1)
                {
                    ImageButton btnDOWN = (ImageButton)e.Row.FindControl("btndown");
                    btnDOWN.Visible = false;
                }

                DropDownList drpProfileFieldsSort = (DropDownList)e.Row.FindControl("drpProfileFieldsSort");
                Label lblSortorder = (Label)e.Row.FindControl("lblSortorder");

                for (int i = 1; i <= aProfileFields.Count; i++)
                {
                    drpProfileFieldsSort.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                try
                {
                    if (Convert.ToInt32(lblSortorder.Text) > 0)
                    {
                        drpProfileFieldsSort.Items.FindByValue(lblSortorder.Text).Selected = true;
                    }
                }
                catch
                { }

                HiddenField spType = (HiddenField)e.Row.FindControl("hiddenSeparatorType");
                DropDownList drpSeparatorType = (DropDownList)e.Row.FindControl("drpSeparator");
                drpSeparatorType.Items.FindByValue(spType.Value).Selected = true;

            }
        }

        protected void grdDemoGraphicFields_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    ImageButton btnUP = (ImageButton)e.Row.FindControl("btnup");
                    btnUP.Visible = false;
                }

                if (e.Row.RowIndex == aDemographicFields.Count - 1)
                {
                    ImageButton btnDOWN = (ImageButton)e.Row.FindControl("btndown");
                    btnDOWN.Visible = false;
                }

                DropDownList drpDemoGraphicFieldsSort = (DropDownList)e.Row.FindControl("drpDemoGraphicFieldsSort");
                Label lblSortorder = (Label)e.Row.FindControl("lblSortorder");

                for (int i = 1; i <= aDemographicFields.Count; i++)
                {
                    drpDemoGraphicFieldsSort.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                try
                {
                    if (Convert.ToInt32(lblSortorder.Text) > 0)
                    {
                        drpDemoGraphicFieldsSort.Items.FindByValue(lblSortorder.Text).Selected = true;
                    }
                }
                catch
                { }

                HiddenField spType = (HiddenField)e.Row.FindControl("hiddenSeparatorType");
                DropDownList drpSeparatorType = (DropDownList)e.Row.FindControl("drpSeparator");
                drpSeparatorType.Items.FindByValue(spType.Value).Selected = true;
            }
        }

        protected void drpProfileFieldsSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newindex = 0;       //Convert.ToInt32(ddl.SelectedItem.Value);
            int sortedFieldID = 0;
            aProfileFields.Clear();

            foreach (GridViewRow gr in grdProfileFields.Rows)
            {
                DropDownList dl = (DropDownList)gr.FindControl("drpProfileFieldsSort");
                Label lblSortOrder = (Label)gr.FindControl("lblSortorder");

                if (Convert.ToInt32(dl.SelectedValue) == Convert.ToInt32(lblSortOrder.Text))
                    aProfileFields.Add(grdProfileFields.DataKeys[gr.RowIndex].Value.ToString());
                else
                {
                    newindex = Convert.ToInt32(dl.SelectedValue) - 1;
                    sortedFieldID = Convert.ToInt32(grdProfileFields.DataKeys[gr.RowIndex].Value);
                }
            }

            aProfileFields.Insert(newindex, sortedFieldID.ToString());

            LoadFormFields();
        }

        protected void drpDemoGraphicFieldsSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newindex = 0;
            int sortedFieldID = 0;

            aDemographicFields.Clear();

            foreach (GridViewRow gr in grdDemoGraphicFields.Rows)
            {
                DropDownList dl = (DropDownList)gr.FindControl("drpDemoGraphicFieldsSort");
                Label lblSortOrder = (Label)gr.FindControl("lblSortorder");

                if (Convert.ToInt32(dl.SelectedValue) == Convert.ToInt32(lblSortOrder.Text))
                    aDemographicFields.Add(grdDemoGraphicFields.DataKeys[gr.RowIndex].Value.ToString());
                else
                {
                    newindex = Convert.ToInt32(dl.SelectedValue) - 1;
                    sortedFieldID = Convert.ToInt32(grdDemoGraphicFields.DataKeys[gr.RowIndex].Value);
                }
            }

            aDemographicFields.Insert(newindex, sortedFieldID.ToString());
            LoadFormFields();
        }


        protected void btnmoveup_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList selecteditem = new ArrayList();
            int i = 0;
            foreach (ListItem item in lstDestinationNewsLetter.Items)
            {
                if (item.Selected)
                {
                    selecteditem.Add(i);
                }
                i++;
            }
            lstDestinationNewsLetter.ClearSelection();

            for (int j = 0; j <= selecteditem.Count - 1; j++)
                moveItem(-1, Convert.ToInt32(selecteditem[j]));
        }

        protected void evtPaidChanged(object sender, EventArgs e)
        {
            if (ProcessExternal)
                pnlpaid.Visible = false;
            else
                pnlpaid.Visible = Convert.ToBoolean(rbpaidsub.SelectedValue);

            paidCost.Visible = Convert.ToBoolean(rbpaidsub.SelectedValue);
        }


        protected void rbUserNotificationPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbUserNotificationPrint.SelectedItem.Value))
                pnlUserNotificationPrint.Visible = true;
            else
                pnlUserNotificationPrint.Visible = false;

            pnlFromPrint.Visible = (pnlUserNotificationPrint.Visible || pnlAdminNotificationPrint.Visible || pnlNonQualResponse.Visible);
        }

        protected void rbUserNotificationDigital_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbUserNotificationDigital.SelectedItem.Value))
                pnlUserNotificationDigital.Visible = true;
            else
                pnlUserNotificationDigital.Visible = false;

            pnlFromDigital.Visible = (pnlUserNotificationDigital.Visible || pnlAdminNotificationDigital.Visible);
        }

        protected void rbUserNotificationBoth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbUserNotificationBoth.SelectedItem.Value))
                pnlUserNotificationBoth.Visible = true;
            else
                pnlUserNotificationBoth.Visible = false;

            pnlFromBoth.Visible = (pnlUserNotificationBoth.Visible || pnlAdminNotificationBoth.Visible);
        }

        protected void rbUserNotificationCancel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbUserNotificationCancel.SelectedItem.Value))
                pnlUserNotificationCancel.Visible = true;
            else
                pnlUserNotificationCancel.Visible = false;

            pnlFromCancel.Visible = (pnlUserNotificationCancel.Visible || pnlAdminNotificationCancel.Visible);
        }


        protected void rbNewletterNotification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbNewletterNotification.SelectedItem.Value))
                pnlNewsLetterNotification.Visible = true;
            else
                pnlNewsLetterNotification.Visible = false;

            pnlFromNewsletter.Visible = (pnlNewsLetterNotification.Visible);
        }

        protected void rbUserNotificationOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbUserNotificationOther.SelectedItem.Value))
                pnlUserNotificationOther.Visible = true;
            else
                pnlUserNotificationOther.Visible = false;

            pnlFromOther.Visible = (pnlUserNotificationOther.Visible || pnlAdminNotificationOther.Visible);
        }

        protected void rbAdminNotificationPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbAdminNotificationPrint.SelectedItem.Value))
                pnlAdminNotificationPrint.Visible = true;
            else
                pnlAdminNotificationPrint.Visible = false;

            pnlFromPrint.Visible = (pnlNonQualResponse.Visible || pnlUserNotificationPrint.Visible || pnlAdminNotificationPrint.Visible);
        }

        protected void rbAdminNotificationDigital_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbAdminNotificationDigital.SelectedItem.Value))
                pnlAdminNotificationDigital.Visible = true;
            else
                pnlAdminNotificationDigital.Visible = false;

            pnlFromDigital.Visible = (pnlUserNotificationDigital.Visible || pnlAdminNotificationDigital.Visible);
        }


        protected void rbAdminNotificationBoth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbAdminNotificationBoth.SelectedItem.Value))
                pnlAdminNotificationBoth.Visible = true;
            else
                pnlAdminNotificationBoth.Visible = false;

            pnlFromBoth.Visible = (pnlUserNotificationBoth.Visible || pnlAdminNotificationBoth.Visible);
        }

        protected void rbAdminNotificationCancel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbAdminNotificationCancel.SelectedItem.Value))
                pnlAdminNotificationCancel.Visible = true;
            else
                pnlAdminNotificationCancel.Visible = false;

            pnlFromCancel.Visible = (pnlUserNotificationCancel.Visible || pnlAdminNotificationCancel.Visible);
        }

        protected void rbAdminNotificationOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbAdminNotificationOther.SelectedItem.Value))
                pnlAdminNotificationOther.Visible = true;
            else
                pnlAdminNotificationOther.Visible = false;

            pnlFromOther.Visible = (pnlUserNotificationOther.Visible || pnlAdminNotificationOther.Visible);
        }


        protected void rbNonQualSetup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbNonQualSetup.SelectedItem.Value))
                pnlNonQual.Visible = true;
            else
                pnlNonQual.Visible = false;
        }




        protected void btnmovedown_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList selecteditem = new ArrayList();
            int i = 0;
            foreach (ListItem item in lstDestinationNewsLetter.Items)
            {
                if (item.Selected)
                {
                    selecteditem.Add(i);
                }
                i++;
            }

            lstDestinationNewsLetter.ClearSelection();

            for (int j = selecteditem.Count - 1; j >= 0; j--)
                moveItem(1, Convert.ToInt32(selecteditem[j]));
        }

        private void moveItem(int i, int selIndex)
        {
            string selValue;
            string selText;

            if (selIndex + i < 0 || selIndex + i > lstDestinationNewsLetter.Items.Count - 1) { return; }

            selValue = lstDestinationNewsLetter.Items[selIndex].Value;
            selText = lstDestinationNewsLetter.Items[selIndex].Text;

            lstDestinationNewsLetter.Items[selIndex].Value = lstDestinationNewsLetter.Items[selIndex + i].Value;
            lstDestinationNewsLetter.Items[selIndex].Text = lstDestinationNewsLetter.Items[selIndex + i].Text;

            lstDestinationNewsLetter.Items[selIndex + i].Value = selValue;
            lstDestinationNewsLetter.Items[selIndex + i].Text = selText;

            lstDestinationNewsLetter.Items.FindByValue(selValue).Selected = true;

        }

        protected void btnAddNonQual_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstSourceNonQual, lstDestinationNonQual, false);
        }

        protected void btnRemoveNonQual_Click(object sender, EventArgs e)
        {
            this.moveSelectedItems(lstDestinationNonQual, lstSourceNonQual, false);
        }

        protected void drpFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlEditInactiveFields.Visible)
                pnlEditInactiveFields.Visible = false;
        }

        protected void val_MultipleEmails_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void rbUserNotificationNQCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbUserNotificationNQCountry.SelectedItem.Value))
                pnlNQResponseCountry.Visible = true;
            else
                pnlNQResponseCountry.Visible = false;

            pnlFromEmailNQ.Visible = pnlNQResponseCountry.Visible;
        }

        protected void rbUserNotificationNQResponsePrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbUserNotificationNQResponsePrint.SelectedItem.Value))
                pnlNonQualResponse.Visible = true;
            else
                pnlNonQualResponse.Visible = false;

            pnlFromEmailNQ.Visible = (pnlNQResponseCountry.Visible);
        }


        protected void MnuResponseType_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                SaveResponseEmail(ActiveResponseEmailTab);
                ActiveResponseEmailTab = Convert.ToInt32(e.Item.Value);
                LoadResponseEmailTab();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadResponseEmailTab()
        {
            multiViewResponseType.ActiveViewIndex = ActiveResponseEmailTab;
            MnuResponseType.Items[ActiveResponseEmailTab].Selected = true;
            SqlCommand cmdGetPubResponseEmail;
            DataTable dtform;
            DataRow dr;

            switch (ActiveResponseEmailTab)
            {
                #region case print
                case 0:
                    cmdGetPubResponseEmail = new SqlCommand("Select * from PubResponseEmails where PFID= @PFID and responseType = 'print'");
                    cmdGetPubResponseEmail.CommandType = CommandType.Text;
                    cmdGetPubResponseEmail.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtform = DataFunctions.GetDataTable(cmdGetPubResponseEmail);

                    if (dtform.Rows.Count > 0)
                    {
                        dr = dtform.Rows[0];

                        try
                        {
                            if (!String.IsNullOrEmpty(dtform.Rows[0]["SendUserEmail"].ToString()))
                            {
                                rbUserNotificationPrint.ClearSelection();

                                if (Convert.ToBoolean(dr["SendUserEmail"]))
                                {
                                    pnlUserNotificationPrint.Visible = true;
                                    rbUserNotificationPrint.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlUserNotificationPrint.Visible = false;
                                    rbUserNotificationPrint.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbUserNotificationPrint.ClearSelection();
                                rbUserNotificationPrint.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch { }

                        txtUserEmailSubPrint.Text = dr["Response_UserMsgSubject"].ToString().Trim();
                        RadEditorUserEmailBodyPrint.Content = Server.HtmlDecode((dr["Response_UserMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgBody"].ToString().Trim()));

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendAdminEmail"].ToString()))
                            {
                                rbAdminNotificationPrint.ClearSelection();

                                if (Convert.ToBoolean(dr["SendAdminEmail"]))
                                {
                                    pnlAdminNotificationPrint.Visible = true;
                                    rbAdminNotificationPrint.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlAdminNotificationPrint.Visible = false;
                                    rbAdminNotificationPrint.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbAdminNotificationPrint.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch
                        { }

                        txtAdminEmailPrint.Text = dr["Response_AdminEmail"].ToString().Trim();
                        txtAdminEmailSubPrint.Text = dr["Response_AdminMsgSubject"].ToString().Trim();
                        RadEditorAdminEmailBodyPrint.Content = Server.HtmlDecode((dr["Response_AdminMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_AdminMsgBody"].ToString().Trim()));

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendNQRespEmail"].ToString()))
                            {
                                rbUserNotificationNQResponsePrint.ClearSelection();

                                if (Convert.ToBoolean(dr["SendNQRespEmail"]))
                                {
                                    pnlNonQualResponse.Visible = true;
                                    rbUserNotificationNQResponsePrint.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlNonQualResponse.Visible = false;
                                    rbUserNotificationNQResponsePrint.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbUserNotificationNQResponsePrint.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch
                        { }

                        txtUserEmailSubNQResponsePrint.Text = dr["Response_UserMsgNQRespSub"].ToString().Trim();
                        RadEditorEmailBodyNQResponsePrint.Content = Server.HtmlDecode((dr["Response_UserMsgNQRespMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgNQRespMsgBody"].ToString().Trim()));

                        //if (!responseStatus)
                        //{                           
                        pnlFromPrint.Visible = (pnlUserNotificationPrint.Visible || pnlAdminNotificationPrint.Visible || pnlNonQualResponse.Visible);
                        //rbSetupResponseEmail.ClearSelection();
                        //rbSetupResponseEmail.Items.FindByValue(pnlFromPrint.Visible ? "true":"false").Selected = true;    
                        //responseStatus = pnlFromPrint.Visible;                              
                        //}
                    }
                    break;
                #endregion

                #region case digital
                case 1:
                    cmdGetPubResponseEmail = new SqlCommand("Select * from PubResponseEmails where PFID= @PFID and responseType = 'digital'");
                    cmdGetPubResponseEmail.CommandType = CommandType.Text;
                    cmdGetPubResponseEmail.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtform = DataFunctions.GetDataTable(cmdGetPubResponseEmail);

                    if (dtform.Rows.Count > 0)
                    {
                        dr = dtform.Rows[0];

                        FromNameDigital.Text = dr["Response_FromName"].ToString().Trim();
                        txtFormEmailDigital.Text = dr["Response_FromEmail"].ToString().Trim();

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendUserEmail"].ToString()))
                            {
                                rbUserNotificationDigital.ClearSelection();

                                if (Convert.ToBoolean(dr["SendUserEmail"]))
                                {
                                    pnlUserNotificationDigital.Visible = true;
                                    rbUserNotificationDigital.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlUserNotificationDigital.Visible = false;
                                    rbUserNotificationDigital.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbUserNotificationDigital.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch { }

                        txtUserEmailSubDigital.Text = dr["Response_UserMsgSubject"].ToString().Trim();
                        RadEditorUserEmailBodyDigital.Content = Server.HtmlDecode((dr["Response_UserMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgBody"].ToString().Trim()));

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendAdminEmail"].ToString()))
                            {
                                rbAdminNotificationDigital.ClearSelection();

                                if (Convert.ToBoolean(dr["SendAdminEmail"]))
                                {
                                    pnlAdminNotificationDigital.Visible = true;
                                    rbAdminNotificationDigital.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlAdminNotificationDigital.Visible = false;
                                    rbAdminNotificationDigital.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbAdminNotificationDigital.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch
                        { }

                        txtAdminEmailDigital.Text = dr["Response_AdminEmail"].ToString().Trim();
                        txtAdminEmailSubDigital.Text = dr["Response_AdminMsgSubject"].ToString().Trim();
                        RadEditorAdminEmailBodyDigital.Content = Server.HtmlDecode((dr["Response_AdminMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_AdminMsgBody"].ToString().Trim()));

                        //if (!responseStatus) 
                        //{                           
                        pnlFromDigital.Visible = (pnlUserNotificationDigital.Visible || pnlAdminNotificationDigital.Visible);
                        //rbSetupResponseEmail.ClearSelection();
                        //rbSetupResponseEmail.Items.FindByValue(pnlFromDigital.Visible?"true":"false").Selected = true;   

                        //} 
                    }

                    break;
                #endregion

                #region case BOTH
                case 2:
                    cmdGetPubResponseEmail = new SqlCommand("Select * from PubResponseEmails where PFID= @PFID and responseType = 'both'");
                    cmdGetPubResponseEmail.CommandType = CommandType.Text;
                    cmdGetPubResponseEmail.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtform = DataFunctions.GetDataTable(cmdGetPubResponseEmail);

                    if (dtform.Rows.Count > 0)
                    {
                        dr = dtform.Rows[0];
                        FromNameBoth.Text = dr["Response_FromName"].ToString().Trim();
                        txtFormEmailBoth.Text = dr["Response_FromEmail"].ToString().Trim();

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendUserEmail"].ToString()))
                            {
                                rbUserNotificationBoth.ClearSelection();

                                if (Convert.ToBoolean(dr["SendUserEmail"]))
                                {
                                    pnlUserNotificationBoth.Visible = true;
                                    rbUserNotificationBoth.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlUserNotificationBoth.Visible = false;
                                    rbUserNotificationBoth.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbUserNotificationBoth.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch { }

                        txtUserEmailSubBoth.Text = dr["Response_UserMsgSubject"].ToString().Trim();
                        RadEditorUserEmailBodyBoth.Content = Server.HtmlDecode((dr["Response_UserMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgBody"].ToString().Trim()));

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendAdminEmail"].ToString()))
                            {
                                rbAdminNotificationBoth.ClearSelection();

                                if (Convert.ToBoolean(dr["SendAdminEmail"]))
                                {
                                    pnlAdminNotificationBoth.Visible = true;
                                    rbAdminNotificationBoth.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlAdminNotificationBoth.Visible = false;
                                    rbAdminNotificationBoth.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbAdminNotificationBoth.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch
                        { }

                        txtAdminEmailBoth.Text = dr["Response_AdminEmail"].ToString().Trim();
                        txtAdminEmailSubBoth.Text = dr["Response_AdminMsgSubject"].ToString().Trim();
                        RadEditorAdminEmailBodyBoth.Content = Server.HtmlDecode((dr["Response_AdminMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_AdminMsgBody"].ToString().Trim()));

                        //if (!responseStatus) 
                        //{                            
                        pnlFromBoth.Visible = (pnlUserNotificationBoth.Visible || pnlAdminNotificationBoth.Visible);
                        //rbSetupResponseEmail.ClearSelection();
                        //rbSetupResponseEmail.Items.FindByValue(pnlFromBoth.Visible?"true":"false").Selected = true;  
                        //}
                    }
                    break;
                #endregion

                #region NQ response
                case 3:
                    cmdGetPubResponseEmail = new SqlCommand("Select * from PubResponseEmails where PFID= @PFID and responseType = 'nq'");
                    cmdGetPubResponseEmail.CommandType = CommandType.Text;
                    cmdGetPubResponseEmail.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtform = DataFunctions.GetDataTable(cmdGetPubResponseEmail);

                    if (dtform.Rows.Count > 0)
                    {
                        dr = dtform.Rows[0];
                        FromNameNQ.Text = dr["Response_FromName"].ToString().Trim();
                        txtFormEmailNQ.Text = dr["Response_FromEmail"].ToString().Trim();

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendUserEmail"].ToString()))
                            {
                                rbUserNotificationNQCountry.ClearSelection();

                                if (Convert.ToBoolean(dr["SendUserEmail"]))
                                {
                                    pnlNQResponseCountry.Visible = true;
                                    rbUserNotificationNQCountry.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlNQResponseCountry.Visible = false;
                                    rbUserNotificationNQCountry.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbUserNotificationNQCountry.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch
                        { }

                        txtUserNotificationSubNQCountry.Text = dr["Response_UserMsgSubject"].ToString().Trim();
                        RadEditorNotificationSubNQCountry.Content = Server.HtmlDecode((dr["Response_UserMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgBody"].ToString().Trim()));

                        pnlFromEmailNQ.Visible = pnlNQResponseCountry.Visible;
                        //rbSetupResponseEmail.ClearSelection();
                        //rbSetupResponseEmail.Items.FindByValue(pnlFromEmailNQ.Visible?"true":"false").Selected = true;      
                    }
                    break;

                #endregion

                #region case Cancel
                case 4:
                    cmdGetPubResponseEmail = new SqlCommand("Select * from PubResponseEmails where PFID= @PFID and responseType = 'Cancel'");
                    cmdGetPubResponseEmail.CommandType = CommandType.Text;
                    cmdGetPubResponseEmail.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtform = DataFunctions.GetDataTable(cmdGetPubResponseEmail);

                    if (dtform.Rows.Count > 0)
                    {
                        dr = dtform.Rows[0];
                        FromNameCancel.Text = dr["Response_FromName"].ToString().Trim();
                        txtFormEmailCancel.Text = dr["Response_FromEmail"].ToString().Trim();

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendUserEmail"].ToString()))
                            {
                                rbUserNotificationCancel.ClearSelection();

                                if (Convert.ToBoolean(dr["SendUserEmail"]))
                                {
                                    pnlUserNotificationCancel.Visible = true;
                                    rbUserNotificationCancel.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlUserNotificationCancel.Visible = false;
                                    rbUserNotificationCancel.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbUserNotificationCancel.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch { }

                        txtUserEmailSubCancel.Text = dr["Response_UserMsgSubject"].ToString().Trim();
                        RadEditorUserEmailBodyCancel.Content = Server.HtmlDecode((dr["Response_UserMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgBody"].ToString().Trim()));

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendAdminEmail"].ToString()))
                            {
                                rbAdminNotificationCancel.ClearSelection();

                                if (Convert.ToBoolean(dr["SendAdminEmail"]))
                                {
                                    pnlAdminNotificationCancel.Visible = true;
                                    rbAdminNotificationCancel.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlAdminNotificationCancel.Visible = false;
                                    rbAdminNotificationCancel.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbAdminNotificationCancel.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch
                        { }

                        txtAdminEmailCancel.Text = dr["Response_AdminEmail"].ToString().Trim();
                        txtAdminEmailSubCancel.Text = dr["Response_AdminMsgSubject"].ToString().Trim();
                        RadEditorAdminEmailBodyCancel.Content = Server.HtmlDecode((dr["Response_AdminMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_AdminMsgBody"].ToString().Trim()));

                        //if (!responseStatus) 
                        //{                            
                        pnlFromCancel.Visible = (pnlUserNotificationCancel.Visible || pnlAdminNotificationCancel.Visible);
                        //rbSetupResponseEmail.ClearSelection();
                        //rbSetupResponseEmail.Items.FindByValue(pnlFromCancel.Visible?"true":"false").Selected = true;  
                        //}
                    }
                    break;
                #endregion

                #region case OTHER
                case 5:
                    cmdGetPubResponseEmail = new SqlCommand("Select * from PubResponseEmails where PFID= @PFID and responseType = 'Other'");
                    cmdGetPubResponseEmail.CommandType = CommandType.Text;
                    cmdGetPubResponseEmail.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtform = DataFunctions.GetDataTable(cmdGetPubResponseEmail);

                    if (dtform.Rows.Count > 0)
                    {
                        dr = dtform.Rows[0];
                        FromNameOther.Text = dr["Response_FromName"].ToString().Trim();
                        txtFormEmailOther.Text = dr["Response_FromEmail"].ToString().Trim();

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendUserEmail"].ToString()))
                            {
                                rbUserNotificationOther.ClearSelection();

                                if (Convert.ToBoolean(dr["SendUserEmail"]))
                                {
                                    pnlUserNotificationOther.Visible = true;
                                    rbUserNotificationOther.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlUserNotificationOther.Visible = false;
                                    rbUserNotificationOther.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbUserNotificationOther.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch { }

                        txtUserEmailSubOther.Text = dr["Response_UserMsgSubject"].ToString().Trim();
                        RadEditorUserEmailBodyOther.Content = Server.HtmlDecode((dr["Response_UserMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgBody"].ToString().Trim()));

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendAdminEmail"].ToString()))
                            {
                                rbAdminNotificationOther.ClearSelection();

                                if (Convert.ToBoolean(dr["SendAdminEmail"]))
                                {
                                    pnlAdminNotificationOther.Visible = true;
                                    rbAdminNotificationOther.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlAdminNotificationOther.Visible = false;
                                    rbAdminNotificationOther.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbAdminNotificationOther.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch
                        { }

                        txtAdminEmailOther.Text = dr["Response_AdminEmail"].ToString().Trim();
                        txtAdminEmailSubOther.Text = dr["Response_AdminMsgSubject"].ToString().Trim();
                        RadEditorAdminEmailBodyOther.Content = Server.HtmlDecode((dr["Response_AdminMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_AdminMsgBody"].ToString().Trim()));

                        //if (!responseStatus) 
                        //{                            
                        pnlFromOther.Visible = (pnlUserNotificationOther.Visible || pnlAdminNotificationOther.Visible);
                        //rbSetupResponseEmail.ClearSelection();
                        //rbSetupResponseEmail.Items.FindByValue(pnlFromOther.Visible?"true":"false").Selected = true;  
                        //}
                    }
                    break;
                #endregion

                #region case Newsletter
                case 6:
                    cmdGetPubResponseEmail = new SqlCommand("Select * from PubResponseEmails where PFID= @PFID and responseType = 'Newsletter'");
                    cmdGetPubResponseEmail.CommandType = CommandType.Text;
                    cmdGetPubResponseEmail.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtform = DataFunctions.GetDataTable(cmdGetPubResponseEmail);

                    if (dtform.Rows.Count > 0)
                    {
                        dr = dtform.Rows[0];
                        FromNameNewsletter.Text = dr["Response_FromName"].ToString().Trim();
                        txtFormEmailNewsletter.Text = dr["Response_FromEmail"].ToString().Trim();

                        try
                        {
                            if (!String.IsNullOrEmpty(dr["SendUserEmail"].ToString()))
                            {
                                rbNewletterNotification.ClearSelection();

                                if (Convert.ToBoolean(dr["SendUserEmail"]))
                                {
                                    pnlNewsLetterNotification.Visible = true;
                                    rbNewletterNotification.Items.FindByValue("true").Selected = true;
                                }
                                else
                                {
                                    pnlNewsLetterNotification.Visible = false;
                                    rbNewletterNotification.Items.FindByValue("false").Selected = true;
                                }
                            }
                            else
                            {
                                rbNewletterNotification.Items.FindByValue("false").Selected = true;
                            }
                        }
                        catch { }

                        txtNewsLetterSub.Text = dr["Response_UserMsgSubject"].ToString().Trim();
                        RadEditorNewsLetterBody.Content = Server.HtmlDecode((dr["Response_UserMsgBody"].ToString().Trim() == "0" ? "" : dr["Response_UserMsgBody"].ToString().Trim()));
                    }
                    break;
                #endregion
                default:
                    goto case 0;

            }
        }


        private void SaveResponseEmail(int step)
        {

            switch (step)
            {
                case 0:
                    SavePubResponseEmails(PFID, "Print", FromNamePrint.Text, txtFormEmailPrint.Text,
                        rbUserNotificationPrint.SelectedItem.Value, txtUserEmailSubPrint.Text,
                        RadEditorUserEmailBodyPrint.Content,
                        rbAdminNotificationPrint.SelectedItem.Value, txtAdminEmailPrint.Text, txtAdminEmailSubPrint.Text,
                        RadEditorAdminEmailBodyPrint.Content, txtUserEmailSubNQResponsePrint.Text, RadEditorEmailBodyNQResponsePrint.Content,
                        rbUserNotificationNQResponsePrint.SelectedItem.Value);
                    break;

                case 1:
                    SavePubResponseEmails(PFID, "Digital", FromNameDigital.Text, txtFormEmailDigital.Text,
                        rbUserNotificationDigital.SelectedItem.Value, txtUserEmailSubDigital.Text, RadEditorUserEmailBodyDigital.Content,
                        rbAdminNotificationDigital.SelectedItem.Value,
                        txtAdminEmailDigital.Text, txtAdminEmailSubDigital.Text,
                        RadEditorAdminEmailBodyDigital.Content, string.Empty, string.Empty, "false");
                    break;
                case 2:
                    SavePubResponseEmails(PFID, "Both", FromNameBoth.Text, txtFormEmailBoth.Text,
                        rbUserNotificationBoth.SelectedItem.Value, txtUserEmailSubBoth.Text,
                        RadEditorUserEmailBodyBoth.Content,
                        rbAdminNotificationBoth.SelectedItem.Value, txtAdminEmailBoth.Text,
                        txtAdminEmailSubBoth.Text, RadEditorAdminEmailBodyBoth.Content, string.Empty,
                        string.Empty,
                        "false");
                    break;

                case 3:
                    SavePubResponseEmails(PFID, "NQ", FromNameNQ.Text, txtFormEmailNQ.Text, rbUserNotificationNQCountry.SelectedItem.Value, txtUserNotificationSubNQCountry.Text, RadEditorNotificationSubNQCountry.Content, "false", string.Empty, string.Empty, string.Empty, string.Empty,
                        string.Empty, "false");
                    break;

                case 4:
                    SavePubResponseEmails(PFID, "Cancel", FromNameCancel.Text, txtFormEmailCancel.Text,
                        rbUserNotificationCancel.SelectedItem.Value, txtUserEmailSubCancel.Text,
                        RadEditorUserEmailBodyCancel.Content,
                        rbAdminNotificationCancel.SelectedItem.Value, txtAdminEmailCancel.Text,
                        txtAdminEmailSubCancel.Text, RadEditorAdminEmailBodyCancel.Content, string.Empty,
                        string.Empty,
                        "false");
                    break;
                case 5:

                    SavePubResponseEmails(PFID, "Other", FromNameOther.Text, txtFormEmailOther.Text,
                        rbUserNotificationOther.SelectedItem.Value, txtUserEmailSubOther.Text,
                        RadEditorUserEmailBodyOther.Content,
                        rbAdminNotificationOther.SelectedItem.Value, txtAdminEmailOther.Text,
                        txtAdminEmailSubOther.Text, RadEditorAdminEmailBodyOther.Content, string.Empty,
                        string.Empty, "false");
                    break;
                case 6:

                    SavePubResponseEmails(PFID, "Newsletter", FromNameNewsletter.Text, txtFormEmailNewsletter.Text,
                        rbNewletterNotification.SelectedItem.Value, txtNewsLetterSub.Text,
                        RadEditorNewsLetterBody.Content,
                        "false", string.Empty,
                        string.Empty, string.Empty, string.Empty,
                        string.Empty, "false");
                    break;
                default:
                    goto case 0;
            }
        }

        protected void rblDisableNonQualSetup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rblDisableNonQualSetup.SelectedItem.Value))
            {
                pnlNQDigitalRedirectUrl.Visible = false;
                txtNQDigitalRedirectUrl.Text = "";
            }
            else
            {
                pnlNQDigitalRedirectUrl.Visible = true;
            }
        }
    }
}
