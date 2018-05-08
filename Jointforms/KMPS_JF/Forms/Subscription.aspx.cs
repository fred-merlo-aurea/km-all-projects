using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using Encore.PayPal.Nvp;
using KMPS_JF_Objects.Objects;
using KMPS_JF_Objects.Controls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Text.RegularExpressions;
using ECN_Framework_BusinessLayer;
using ECN_Framework_Entities;
using NXTBookAPI.BusinessLogic;
using NXTBookAPI.Entity;
using KM.Common;

namespace KMPS_JF.Forms
{
    public partial class Subscription : System.Web.UI.Page
    {
        Hashtable hECNFields = new Hashtable();
        Dictionary<string, string> SubscriberResponse;
        Hashtable hECNPostParams = new Hashtable();
        Publication pub = null;
        PubCountry pc = null;
        PubForm pf = null;
        Dictionary<string, string> Subscriber;

        #region Non Qual variables

        private bool IsNonQualCountry = false;

        private string requiredFieldMessage = "";


        #endregion

        #region local variables

        public string SessionID
        {
            get
            {
                try { return ViewState["SessionID"].ToString(); }
                catch { return string.Empty; }
            }
            set
            {
                ViewState["SessionID"] = value;
            }
        }

        private Dictionary<string, string> SubscriberFromCacheorSession
        {
            get
            {
                if (CacheUtil.IsCacheEnabled())
                {
                    try
                    {
                        if (CacheUtil.GetFromCache(SessionID, "JOINTFORMS") != null)
                        {
                            return (Dictionary<string, string>)CacheUtil.GetFromCache(SessionID, "JOINTFORMS");
                        }
                        else
                            return null;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    try
                    {
                        return (Dictionary<string, string>)Session["Subscriber"];
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set
            {
                if (CacheUtil.IsCacheEnabled())
                {
                    CacheUtil.AddToCache(SessionID, value, "JOINTFORMS");
                }
                else
                {
                    Session["Subscriber"] = value;
                }
            }
        }

        public int PubID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["PubID"]);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["PubID"] = value;
            }
        }

        public string PubCode
        {
            get
            {
                try { return ViewState["PubCode"].ToString(); }
                catch { return string.Empty; }
            }
            set
            {
                ViewState["PubCode"] = value;
            }
        }

        public int CustomerID
        {
            get
            {
                return Convert.ToInt32(ViewState["CustomerID"]);
            }
            set
            {
                ViewState["CustomerID"] = value;
            }
        }

        public int GroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["GroupID"]);
            }
            set
            {
                ViewState["GroupID"] = value;
            }
        }

        private int EmailID
        {
            get
            {
                return Convert.ToInt32(ViewState["EmailID"]);
            }
            set
            {
                ViewState["EmailID"] = value;
            }
        }

        private int PubFormID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["PubFormID"]);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["PubFormID"] = value;
            }
        }

        private int CountryID
        {
            get
            {
                try { return Convert.ToInt32(ViewState["CountryID"]); }
                catch { return 0; }
            }
            set
            {
                ViewState["CountryID"] = value;
            }
        }

        public int MagazineID
        {
            get
            {
                try
                {
                    if (Request.QueryString["btx_m"] != null)
                        return Convert.ToInt32(Request.QueryString["btx_m"]);
                    else
                        return Convert.ToInt32(ConfigurationManager.AppSettings["Magazine_" + PubCode].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["MagazineID"] = value;
            }
        }

        public int IssueID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["btx_i"]);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["IssueID"] = value;
            }
        }

        public int EnableCS
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["enableCS"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
        }

        private string getQueryString(string qs)
        {
            try { return Request.QueryString[qs].ToString(); }
            catch { return string.Empty; }
        }

        private string getFromForm(string nm)
        {
            try { return Request[nm].ToString(); }
            catch { return string.Empty; }
        }

        private string getConversionTrackingBlastID
        {
            get
            {
                return getQueryString("ctrk_bid");
            }
        }

        private string getConversionTrackingEmailID
        {
            get
            {
                return getQueryString("ctrk_eid");
            }
        }

        private int ColsNo
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["colsNo"]);
                }
                catch
                {
                    return 2;
                }
            }
            set
            {
                ViewState["colsNo"] = value;
            }
        }

        public string NewsletterExcludeGroupID
        {
            get
            {
                try
                {
                    return Request.QueryString["exnlgrpid"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public bool CatCheckBoxLoaded
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(Session["CatCheckBoxLoaded"]);
                }
                catch { return false; }
            }
            set
            {
                Session["CatCheckBoxLoaded"] = value;
            }
        }

        #endregion

        #region Variable declaration for ECN Web Request

        private Groups ECNPostgroup;

        private Hashtable ECNPosthProfileFields = new Hashtable();
        private Hashtable ECNPosthUDFFields = new Hashtable();

        private string ECNPostResponse_FromEmail = "";
        private string ECNPostResponse_UserMsgSubject = "";
        private string ECNPostResponse_UserMsgBody = "";
        private string ECNPostResponse_UserScreen = "";
        private string ECNPostResponse_AdminEmail = "";
        private string ECNPostResponse_AdminMsgSubject = "";
        private string ECNPostResponse_AdminMsgBody = "";

        private int ECNPostCustomerID = 0;
        private int ECNPostGroupID = 0;
        private int ECNPostSmartFormID = 0;
        private int ECNPostEmailID = 0;
        private int ECNPostBlastID = 0;

        private string ECNPostEmailAddress = "";
        private string ECNPostSubscribe = "";
        private string ECNPostFormat = "";

        private string ECNPostReturnURL = "";
        private string ECNPostfromURL = "";
        private string ECNPostfromIP = "";

        #endregion

        #region Page Events

        protected override void InitializeCulture()
        {
            try
            {
                if (getQueryString("lang") != string.Empty)
                {
                    String selectedLanguage = getQueryString("lang");
                    UICulture = selectedLanguage;
                    Culture = selectedLanguage;

                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(selectedLanguage);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(selectedLanguage);
                }

                base.InitializeCulture();
            }
            catch
            { }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Theme = "";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            lblErrorMessage.Visible = false;
            lblInvalidEmail.Visible = false;


            lblLoginEmailInvalid.Visible = false;
            lblUserNameInvalid.Visible = false;
            //ClientScriptManager csm = Page.ClientScript;
            //btnSubmit.Attributes.Add("onclick", "javascript:" + btnSubmit.ClientID + ".disabled=true;" + csm.GetPostBackEventReference(btnSubmit,""));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            //Response.Cache.SetNoStore();
            //Response.AppendHeader("Pragma", "no-cache");

            if (!IsPostBack)
            {
                SessionID = System.Guid.NewGuid().ToString();

                try
                {
                    PubID = Convert.ToInt32(getQueryString("PubID"));
                }
                catch { }

                try
                {
                    PubCode = getQueryString("PubCode");

                    if (PubCode.Length > 15)
                    {
                        PubCode = PubCode.Substring(0, 15);
                    }

                    PubCode = PubCode.Replace("'", "''");
                }
                catch { }

                PubClearCache();      //clear the cache
            }

            try
            {
                pub = Publication.GetPublicationbyID(PubID, PubCode);

                if (pub == null)
                {
                    HideAllPanels();
                    lblErrorMessage.Text = "Invalid Publication Code.";
                    lblErrorMessage.Visible = true;
                    phError.Visible = true;
                    return;
                }
                else
                {
                    PubID = pub.PubID;
                    PubCode = pub.PubCode;
                    CustomerID = pub.ECNCustomerID;
                    GroupID = pub.ECNDefaultGroupID;

                    if (pub.MailingLabel.Length > 0 && File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["MailingLabelPath"].ToString() + pub.MailingLabel)))
                    {
                        imgPubLabel.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["MailingLabelPath"] + pub.MailingLabel;
                    }

                    this.ColsNo = !String.IsNullOrEmpty(pub.ColumnFormat) ? Convert.ToInt32(pub.ColumnFormat) : 2;
                }

                SetupPublicationPage();


                try
                {
                    if (!pub.IsActive)
                    {
                        if (pub.RedirectLink.Trim().Length > 0)
                        {
                            if (pub.RedirectLink.StartsWith("http"))
                                Response.Redirect(pub.RedirectLink, false);
                            else
                                Response.Redirect("http://" + pub.RedirectLink, false);
                        }
                        else if (pub.RedirectHTML.Trim().Length > 0)
                        {
                            pnlRedirectMsg.Visible = true;
                            lblRedirectMsg.Text = pub.RedirectHTML;
                        }

                        lnkCustomerService.Visible = false;
                        lnkTradeShow.Visible = false;
                        plProfileQuestions.Visible = false;
                        plDemoQuestions.Visible = false;
                        HideAllPanels();

                        return;
                    }
                }
                catch
                {
                    pnlRedirectMsg.Visible = true;
                    lblRedirectMsg.Text = "The Publication is inactive!!";
                    lnkCustomerService.Visible = false;
                    lnkTradeShow.Visible = false;
                    HideAllPanels();
                }

                if (!IsPostBack)
                {
                    if (pub.DisableSubcriberLogin)
                    {
                        imgPubLabel.Visible = false;
                        lblSubscriberID.Visible = false;
                        txtSubscriberID.Visible = false;
                        lblVerfication.Visible = false;
                        txtVerification.Visible = false;
                        btnLoginWithSubscriberID.Visible = false;
                        lblVerficationTEXT.Visible = false;
                    }

                    BindEvents();
                    BindCustomPages();
                    Bindcountry();

                    if (getQueryString("mode").Equals("PREVIEW", StringComparison.OrdinalIgnoreCase))
                        btnSubmit.Visible = false;

                    if (!getQueryString("subscriberID").Equals(string.Empty) && !getQueryString("pwd").Equals(string.Empty))
                    {
                        SubscriberLogin(getQueryString("subscriberID"), pub.LoginVerfication.ToUpper(), getQueryString("pwd").ToString());
                    }
                    else if (!getQueryString("e").Equals(string.Empty) && pub.DisablePassword)
                    {
                        SubscriberLogin(getQueryString("e"), "D", "");
                    }
                    else if (!getQueryString("e").Equals(string.Empty) && !getQueryString("pwd").ToString().Equals(string.Empty) && !pub.DisablePassword)
                    {
                        SubscriberLogin(getQueryString("e"), "U", getQueryString("pwd").ToString());
                    }
                    else if (getQueryString("LoginParam") != string.Empty)
                    {
                        string LoginParam = KMPS_JF_Objects.Objects.Utilities.Decrypt(getQueryString("LoginParam"), "Pas5pr@se", "s@1tValue", "SHA1", 2, "@1B2c3D4e5F6g7H8", 64);
                        string loginType = LoginParam.Split('&')[0].Split('=')[0].ToString();
                        string LoginID = LoginParam.Split('&')[0].Split('=')[1].ToString();
                        string LoginPass = LoginParam.Split('&')[1].Split('=')[1].ToString();

                        if (loginType.ToUpper() == "SUBSCRIBERID")
                            SubscriberLogin(LoginID, pub.LoginVerfication, LoginPass);
                        else if (!pub.DisablePassword)
                            SubscriberLogin(LoginID, "U", LoginPass);
                        else
                            SubscriberLogin(LoginID, "D", "");
                    }
                    else
                    {
                        switch (getQueryString("step").ToLower())
                        {
                            case "step1":
                                ShowPanel("NEWFORMSTEP1");
                                hidTRANSACTIONTYPE.Value = "NEW";
                                break;
                            case "login":
                                ShowPanel("LOGIN");
                                break;
                            case "form":
                                hidTRANSACTIONTYPE.Value = "NEW";
                                drpCountry.Items[0].Selected = true;
                                CountryID = Convert.ToInt32(drpCountry.SelectedItem.Value);
                                getPubForm();
                                ShowPanel("NEWFORM");
                                break;
                            default:
                                ShowPanel("HOME");
                                break;
                        }
                    }
                }
                else
                {
                    if (pnlFormStep2.Visible)
                    {
                        if (CountryID > 0 && CountryID == Convert.ToInt32(drpCountry.SelectedItem.Value))
                            LoadSubscriptionForm();
                    }
                }

            }
            catch (ApplicationException Aex)
            {
                HideAllPanels();
                lblErrorMessage.Text = Aex.Message;
                lblErrorMessage.Visible = true;
                phError.Visible = true;
            }
            catch (Exception ex)
            {
                HideAllPanels();
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
                phError.Visible = true;

                try
                {
                    StringBuilder sbEx = new StringBuilder();

                    sbEx.AppendLine("An exception Happened when handling in page_load in Jointforms Subscription.aspx in Jointforms.</br>");
                    sbEx.AppendLine("<b>Exception Message:</b>" + ex.Message + "</br>");
                    sbEx.AppendLine("<b>Exception Source:</b>" + ex.Source + "</br>");
                    sbEx.AppendLine("<b>Stack Trace:</b>" + ex.StackTrace + "</br>");
                    sbEx.AppendLine("<b>Inner Exception:</b>" + ex.InnerException + "</br>");

                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "KMPS_JF.Forms.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), sbEx.ToString());

                    sbEx.Clear();
                }
                catch
                { }
                return;

            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            bool IsVisible = false;

            if (pnlNewslettersA.Visible || pnlNewslettersB.Visible)
            {
                foreach (RepeaterItem ritem in ((Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition)).Items)
                {
                    HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)ritem.FindControl("chkNewsletterCat");
                    HtmlImage imgCollapseA = (HtmlImage)ritem.FindControl("imgCollapseA");
                    HtmlImage imgCollapseB = (HtmlImage)ritem.FindControl("imgCollapseB");

                    if (!pf.ShowNewsletterAsCollapsed)
                    {
                        chkSelect.Visible = false;

                        if (imgCollapseA != null)
                            imgCollapseA.Visible = false;

                        if (imgCollapseB != null)
                            imgCollapseB.Visible = false;
                    }
                    else if (chkSelect != null && this.GetUnCheckedCount() == 0)
                    {
                        if (chkSelect != null)
                            chkSelect.Checked = true;
                    }
                }
            }

            if (plProfileQuestions.Visible || plDemoQuestions.Visible)
            {
                DataTable dtFieldSettings = getFormFieldSettings();

                foreach (DataRow drFieldSetting in dtFieldSettings.Rows)
                {
                    IsVisible = false;
                    try
                    {
                        if ((drFieldSetting["Grouping"].ToString().ToUpper() == "P" && plProfileQuestions.Visible) || (drFieldSetting["Grouping"].ToString().ToUpper() == "D" && plDemoQuestions.Visible))
                        {
                            switch (drFieldSetting["ControlType"].ToString().ToLower().Trim())
                            {
                                case "checkbox":
                                    CheckBoxList cb;

                                    if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                        cb = (CheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                    else
                                        cb = (CheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                    foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                    {
                                        try
                                        {
                                            if (cb.Items.FindByValue(fv).Selected)
                                                IsVisible = true;
                                        }
                                        catch { }
                                    }
                                    break;
                                case "catcheckbox":
                                    CategorizedCheckBoxList ccb;

                                    if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                        ccb = (CategorizedCheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                    else
                                        ccb = (CategorizedCheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                    foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                    {
                                        string selectedvalues = string.Empty;
                                        foreach (string chked in ccb.Selections)
                                        {
                                            try
                                            {
                                                if (chked.ToString().ToLower() == fv.ToLower())
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                    }
                                    break;
                                case "catradio":
                                    CategorizedRadioButtonList crbl;

                                    if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                        crbl = (CategorizedRadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                    else
                                        crbl = (CategorizedRadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                    foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                    {
                                        string selectedvalues = string.Empty;
                                        foreach (string chked in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            try
                                            {
                                                if (crbl.SelectedValue.ToLower() == chked.ToLower())
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                    }
                                    break;
                                case "radio":
                                    RadioButtonList rb;
                                    if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                        rb = (RadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                    else
                                        rb = (RadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                    foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                    {
                                        try
                                        {
                                            if (rb.Items.FindByValue(fv).Selected)
                                                IsVisible = true;
                                        }
                                        catch { }
                                    }
                                    break;
                                case "dropdown":
                                    DropDownList ddl;
                                    if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                        ddl = (DropDownList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                    else
                                        ddl = (DropDownList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                    foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                    {
                                        try
                                        {
                                            if (ddl.Items.FindByValue(fv).Selected)
                                                IsVisible = true;
                                        }
                                        catch { }
                                    }
                                    break;
                            }

                            if (IsVisible)
                            {
                                if (drFieldSetting["ChildGrouping"].ToString().ToUpper() == "D")
                                {
                                    plDemoQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper()).Visible = true;
                                    plDemoQuestions.FindControl(string.Format("tr_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = true;
                                    plDemoQuestions.FindControl(string.Format("tbl_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = true;

                                    if (Convert.ToBoolean(drFieldSetting["AddSeparator"]))
                                        plDemoQuestions.FindControl(string.Format("tr2_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = true;
                                }
                                else
                                {
                                    plProfileQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper()).Visible = true;
                                    plProfileQuestions.FindControl(string.Format("tr_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = true;

                                    if (Convert.ToBoolean(drFieldSetting["AddSeparator"]))
                                        plProfileQuestions.FindControl(string.Format("tr2_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = true;
                                }
                            }
                            else
                            {
                                if (drFieldSetting["ChildGrouping"].ToString().ToUpper() == "D")
                                {
                                    plDemoQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper()).Visible = false;
                                    plDemoQuestions.FindControl(string.Format("tr_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = false;
                                    plDemoQuestions.FindControl(string.Format("tbl_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = false;

                                    if (Convert.ToBoolean(drFieldSetting["AddSeparator"]))
                                        plDemoQuestions.FindControl(string.Format("tr2_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = false;
                                }
                                else
                                {
                                    plProfileQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper()).Visible = false;
                                    plProfileQuestions.FindControl(string.Format("tr_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = false;

                                    if (Convert.ToBoolean(drFieldSetting["AddSeparator"]))
                                        plProfileQuestions.FindControl(string.Format("tr2_q_{0}", drFieldSetting["ECNFieldName"].ToString())).Visible = false;
                                }

                                switch (drFieldSetting["ChildControlType"].ToString().ToLower().Trim())
                                {
                                    case "checkbox":
                                        CheckBoxList cb;

                                        if (drFieldSetting["ChildGrouping"].ToString().ToUpper() == "D")
                                            cb = (CheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        else
                                            cb = (CheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        cb.ClearSelection();

                                        break;

                                    case "catcheckbox":
                                        CategorizedCheckBoxList ccb;

                                        if (drFieldSetting["ChildGrouping"].ToString().ToUpper() == "D")
                                            ccb = (CategorizedCheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        else
                                            ccb = (CategorizedCheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        ccb.Selections.Clear();

                                        break;

                                    case "catradio":
                                        CategorizedRadioButtonList crbl;

                                        if (drFieldSetting["ChildGrouping"].ToString().ToUpper() == "D")
                                            crbl = (CategorizedRadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        else
                                            crbl = (CategorizedRadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        crbl.SelectedValue = string.Empty;

                                        break;

                                    case "radio":
                                        RadioButtonList rb;
                                        if (drFieldSetting["ChildGrouping"].ToString().ToUpper() == "D")
                                            rb = (RadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        else
                                            rb = (RadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        rb.ClearSelection();

                                        break;

                                    case "dropdown":
                                        DropDownList ddl;
                                        if (drFieldSetting["ChildGrouping"].ToString().ToUpper() == "D")
                                            ddl = (DropDownList)plDemoQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());
                                        else
                                            ddl = (DropDownList)plProfileQuestions.FindControl("question_" + drFieldSetting["ECNFieldName"].ToString().ToUpper());

                                        ddl.ClearSelection();

                                        break;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            StringBuilder sbEx = new StringBuilder();

                            sbEx.AppendLine("An exception Happened when handling in Page_PreRender in Jointforms Subscription.aspx in Jointforms.</br>");
                            sbEx.AppendLine("PubCode: " + PubCode + "<br />");
                            sbEx.AppendLine("Query String: ");
                            try
                            {
                                for (int i = 0; i < Request.QueryString.Keys.Count; i++)
                                {
                                    sbEx.AppendLine(Request.QueryString.Keys[i].ToString() + ":" + Request.QueryString.Get(i) + "<br />");
                                }
                            }
                            catch { }
                            sbEx.AppendLine("<b>Exception Message:</b>" + ex.Message + "</br>");
                            sbEx.AppendLine("<b>Exception Source:</b>" + ex.Source + "</br>");
                            sbEx.AppendLine("<b>Stack Trace:</b>" + ex.StackTrace + "</br>");
                            sbEx.AppendLine("<b>Inner Exception:</b>" + ex.InnerException + "</br>");

                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "KMPS_JF.Forms.Page_PreRender", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), sbEx.ToString());

                            sbEx.Clear();
                        }
                        catch
                        { }
                    }
                }
            }
        }

        #endregion

        private void PubClearCache()
        {
            bool bfirst = true;

            if (CacheUtil.IsCacheEnabled())
            {
                if (PubID > 0 || PubCode.Length > 0)
                {
                    SqlCommand cmddtPubs = new SqlCommand("spClearCache");
                    cmddtPubs.CommandType = CommandType.StoredProcedure;
                    cmddtPubs.Parameters.Add(new SqlParameter("@pubID", PubID));
                    cmddtPubs.Parameters.Add(new SqlParameter("@pubcode", PubCode));
                    DataTable dtPubs = DataFunctions.GetDataTable(cmddtPubs);

                    if (dtPubs != null)
                    {
                        foreach (DataRow dr in dtPubs.Rows)
                        {
                            if (bfirst)
                            {
                                if (CacheUtil.GetFromCache("Pub_" + dr["PubCode"].ToString().ToUpper(), "JOINTFORMS") != null)
                                {
                                    CacheUtil.RemoveFromCache("Pub_" + dr["PubCode"].ToString().ToUpper(), "JOINTFORMS");
                                }

                                bfirst = false;
                            }

                            if (CacheUtil.GetFromCache("PubForm_" + dr["PFID"].ToString(), "JOINTFORMS") != null)
                            {
                                CacheUtil.RemoveFromCache("PubForm_" + dr["PFID"].ToString(), "JOINTFORMS");
                            }
                        }
                    }
                }
            }
        }

        #region Bind Grids & Static Dropdowns
        private void BindEvents()
        {
            DataTable dtevents = Publication.GetPublicationEvents(PubID);

            if (dtevents.Rows.Count > 0)
            {
                pnlEvents.Visible = true;
                dlEvents.DataSource = dtevents;
                dlEvents.DataBind();
            }
        }

        private void BindCustomPages()
        {
            DataTable dtCustomPages = Publication.GetPublicationCustomPages(PubID);

            if (dtCustomPages.Rows.Count > 0)
            {
                pnlCustomPages.Visible = true;
                dlCustomPages.DataSource = dtCustomPages;
                dlCustomPages.DataBind();
            }
        }

        private void Bindcountry()
        {
            drpCountry.DataSource = pub.Countries;
            drpCountry.DataValueField = "CountryID";
            drpCountry.DataTextField = "CountryName";
            drpCountry.DataBind();

            drpNewCountry.DataSource = pub.Countries;
            drpNewCountry.DataValueField = "CountryID";
            drpNewCountry.DataTextField = "CountryName";
            drpNewCountry.DataBind();
        }

        private int GetUnCheckedCount()
        {
            int CheckedCount = 0;

            foreach (RepeaterItem ritem in ((Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition)).Items)
            {
                GridView gvNewsletters = (GridView)ritem.FindControl("gvNewsletters");


                if (gvNewsletters != null)
                {
                    foreach (GridViewRow r in gvNewsletters.Rows)
                    {
                        HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)r.FindControl("chkSelect");

                        if (!chkSelect.Checked)
                        {
                            CheckedCount++;
                        }
                    }
                }
            }

            return CheckedCount;
        }


        private void BindNewslettersGrid()
        {
            pnlNewslettersA.Visible = false;
            pnlNewslettersB.Visible = false;

            if (pub.ShowNewsletters)
            {
                DataTable dtCat = getCategoryforPubFormCountry();

                Repeater rptNewsletter = (Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition);

                if (rptNewsletter != null)
                {
                    rptNewsletter.DataSource = dtCat;
                    rptNewsletter.DataBind();
                }
            }
        }

        protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblcategoryName = (Label)e.Item.FindControl("lblcategoryName");
                Label newsLetterHeader = (Label)e.Item.FindControl("lblNewsletterHeaderB");

                HtmlGenericControl divNewsletterDetailsA = (HtmlGenericControl)e.Item.FindControl("divNewsletterDetailsA");
                HtmlImage imgNewsLetterDetailsA = (HtmlImage)e.Item.FindControl("imgCollapseA");

                if (divNewsletterDetailsA != null && pf.ShowNewsletterAsCollapsed)
                {
                    divNewsletterDetailsA.Attributes.Add("style", "display:block");
                    imgNewsLetterDetailsA.Src = ConfigurationManager.AppSettings["COLLAPSEIMG"].ToString();
                    imgNewsLetterDetailsA.Attributes.Add("title", "Click to expand");
                }
                else if (divNewsletterDetailsA != null)
                {
                    divNewsletterDetailsA.Attributes.Add("style", "display:block");
                    imgNewsLetterDetailsA.Src = ConfigurationManager.AppSettings["COLLAPSEIMG"].ToString();
                    imgNewsLetterDetailsA.Attributes.Add("title", "Click to collapse");
                }

                HtmlGenericControl divNewsletterDetailsB = (HtmlGenericControl)e.Item.FindControl("divNewsletterDetailsB");
                HtmlImage imgNewsLetterDetailsB = (HtmlImage)e.Item.FindControl("imgCollapseB");

                if (divNewsletterDetailsB != null && pf.ShowNewsletterAsCollapsed)
                {
                    divNewsletterDetailsB.Attributes.Add("style", "display:block");
                    imgNewsLetterDetailsB.Src = ConfigurationManager.AppSettings["COLLAPSEIMG"].ToString();
                    imgNewsLetterDetailsB.Attributes.Add("title", "Click to expand");
                }
                else if (divNewsletterDetailsB != null)
                {
                    divNewsletterDetailsB.Attributes.Add("style", "display:block");
                    imgNewsLetterDetailsB.Src = ConfigurationManager.AppSettings["COLLAPSEIMG"].ToString();
                    imgNewsLetterDetailsB.Attributes.Add("title", "Click to collapse");
                }

                if (newsLetterHeader != null)
                    newsLetterHeader.Text = pub.NewsletterHeaderHTML;

                SqlCommand cmdNewsletter = new SqlCommand("sp_getNewslettersforCategory");
                cmdNewsletter.CommandType = CommandType.StoredProcedure;
                cmdNewsletter.Parameters.Add(new SqlParameter("@PubID", PubID.ToString()));
                cmdNewsletter.Parameters.Add(new SqlParameter("@countryID", drpCountry.SelectedItem.Value.ToString()));
                cmdNewsletter.Parameters.Add(new SqlParameter("@CategoryName", lblcategoryName.Text));
                cmdNewsletter.Parameters.Add(new SqlParameter("@EmailAddress", txtemailaddress.Text));
                cmdNewsletter.Parameters.Add(new SqlParameter("@IsRenew", hidTRANSACTIONTYPE.Value == "RENEW" ? "1" : "0"));

                DataTable dtNewsletter = DataFunctions.GetDataTable(cmdNewsletter);
                DataView dvNewsletter = dtNewsletter.DefaultView;

                // not required to hide the selected newsletter.

                //if (!string.IsNullOrEmpty(NewsletterExcludeGroupID))
                //{
                //    dvNewsletter.RowFilter = "ECNGroupID <> " + NewsletterExcludeGroupID.ToString();
                //}

                if (dtNewsletter.Rows.Count > 0)
                {
                    GridView gvNewsletters = (GridView)e.Item.FindControl("gvNewsletters");

                    gvNewsletters.DataSource = dvNewsletter;
                    gvNewsletters.DataBind();

                    ((Panel)Page.FindControl("pnlNewsletters" + pf.NewsletterPosition)).Visible = true;
                }
            }
        }

        #endregion

        private void SetupPublicationPage()
        {
            if (pub.PageTitle.Trim().Length > 0)
                Page.Title = pub.PageTitle;
            else
                Page.Title = pub.PubName + " Subscription Form";

            divcss.InnerHtml = pub.GetCSS();

            phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML));
            phFooter.Controls.Add(new LiteralControl(pub.FooterHTML));
            lnkNewSubscription.Visible = pub.ShowNewSubLink;
            lnkManageSubscription.Visible = pub.ShowRenewSubLink;
            lnkCustomerService.Visible = pub.ShowCustomerServiceLink;
            lnkTradeShow.Visible = pub.ShowTradeShowLink;

            lblRequiredField.Text = pub.RequiredFieldHTML;
            lblNewsletterHeaderA.Text = pub.NewsletterHeaderHTML;

            hidVerificationType.Value = pub.LoginVerfication;

            switch (pub.LoginVerfication.ToUpper())
            {
                case "S":
                    lblVerfication.Text = "State or Province";
                    txtVerification.MaxLength = 2;
                    break;
                case "C":
                    lblVerfication.Text = "First letter of your Country";
                    txtVerification.MaxLength = 1;
                    break;
                case "L":
                    lblVerfication.Text = Resources.Resource.ResourceManager.GetObject("firstletterlastname").ToString(); //"First letter of your last name";
                    txtVerification.MaxLength = 1;
                    break;
                default:
                    lblVerfication.Text = "State or Province";
                    txtVerification.MaxLength = 2;
                    break;
            }
        }

        #region Load Events

        private void getPubForm()
        {
            try
            {
                if ((PubFormID == 0 || pf == null) && CountryID > 0)
                {
                    pc = pub.GetPubCountry(CountryID);

                    if (pc != null)
                    {
                        pf = PubForm.GetPubForm(pc.PFID);
                        PubFormID = pf.PFID;
                    }
                }
            }
            catch { }
        }

        private void ClearSubscriberDetails()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                if (CacheUtil.GetFromCache(SessionID, "JOINTFORMS") != null)
                {
                    CacheUtil.RemoveFromCache(SessionID, "JOINTFORMS");
                }
            }
            else
            {
                if (Session["Subscriber"] != null)
                {
                    Session["Subscriber"] = null;
                }
            }

            if (Subscriber != null)
            {
                Subscriber.Clear();
            }
        }

        private void ShowPanel(string paneltype)
        {
            HideAllPanels();

            switch (paneltype.ToUpper())
            {
                case "HOME":
                    pnlHomePage.Visible = true;
                    try
                    {
                        lblPageDesc.Text = pub.HomePageDesc;

                        if (pub.NewSubscriptionHeader != string.Empty)
                        {
                            plNewSubscriptionHeader.Controls.Add(new LiteralControl(pub.NewSubscriptionHeader));
                        }
                        else
                        {
                            plNewSubscriptionHeader.Visible = false;
                        }

                        if (pub.ManageSubscriptionHeader != string.Empty)
                        {
                            plManageSubscriptionHeader.Controls.Add(new LiteralControl(pub.ManageSubscriptionHeader));
                        }
                        else
                        {
                            plManageSubscriptionHeader.Visible = false;
                        }

                        lnkNewSubscription.Text = String.IsNullOrEmpty(pub.NewSubscriptionLink) ? "New Subscription" : pub.NewSubscriptionLink;
                        lnkManageSubscription.Text = String.IsNullOrEmpty(pub.ManageSubscriptionLink) ? "Manage my Subscription" : pub.ManageSubscriptionLink;
                    }
                    catch { }
                    break;
                case "LOGIN":
                    pnlLogin.Visible = true;
                    imgOR.Src = Resources.Resource.ResourceManager.GetObject("imgOR").ToString();
                    imgOR.Visible = !pub.DisableEmail && !pub.DisableSubcriberLogin;
                    lblPageDesc.Text = pub.LoginPageDesc;
                    pnlPasswordAndLogin.Visible = !pub.DisablePassword;
                    pnlCantAccessAccount.Visible = !pub.DisablePassword;
                    pnlEmailAddress.Visible = !pub.DisableEmail;
                    pnlLoginButton.Visible = !pub.DisableEmail;
                    break;
                case "NEWFORMSTEP1":
                    lblPageDesc.Text = pub.Step2PageDesc;
                    pnlFormStep1.Visible = true;
                    break;
                case "NEWFORM":
                    ClearSubscriberDetails();
                    pnlSearch.Visible = true;
                    pnlFormStep2.Visible = true;
                    lblPageDesc.Text = pub.NewPageDesc;
                    txtemailaddress.Text = txtLoginEmailAddress.Text;
                    drpCountry.ClearSelection();
                    pnlPassword.Visible = !pub.DisablePassword;

                    try
                    {
                        drpCountry.Items.FindByValue(CountryID.ToString()).Selected = true;
                    }
                    catch
                    { }

                    LoadSubscriptionForm();
                    BindNewslettersGrid();
                    break;
                case "RENEWFORM":
                    CatCheckBoxLoaded = false;
                    pnlSearch.Visible = true;
                    pnlFormStep2.Visible = true;
                    lblPageDesc.Text = pub.RenewPageDesc;
                    pnlPassword.Visible = !pub.DisablePassword;

                    if (!pub.DisablePassword)
                    {
                        rfvPassword.Enabled = false;
                        rfvcPassword.Enabled = false;
                    }

                    if (txtemailaddress.Text.ToUpper().EndsWith(PubCode + ".KMPSGROUP.COM", StringComparison.OrdinalIgnoreCase))
                    {
                        txtemailaddress.Enabled = true;
                        txtemailaddress.Text = "";
                    }

                    LoadSubscriptionForm();
                    BindNewslettersGrid();

                    try
                    {
                        bool IsShowCopy = Convert.ToBoolean(Request.QueryString["cp"].ToString());

                        if (IsShowCopy)
                        {
                            lblCopy.Visible = true;
                            lblCopy.Text = Server.HtmlDecode(ConfigurationManager.AppSettings[PubCode + "_COPY"].ToString());
                        }
                    }
                    catch { lblCopy.Visible = false; }

                    try { }
                    catch { }
                    break;
            }
        }

        private void HideAllPanels()
        {
            pnlSearch.Visible = false;
            pnlHomePage.Visible = false;
            pnlLogin.Visible = false;
            pnlFormStep1.Visible = false;
            pnlFormStep2.Visible = false;
        }

        private bool CheckPaidProductsEmailValidation(string emailAddress)
        {
            if (pub.RepeatEmails)
            {
                string sql = "SELECT e.EmailID FROM Emails e  with (NOLOCK) join EmailGroups eg  with (NOLOCK) on eg.EmailID = e.EmailID WHERE e.EmailAddress = @EmailAddress " +
                             " AND e.CustomerID = @CustomerID and eg.GroupID = @GroupID";

                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                cmd.Parameters.AddWithValue("@CustomerID", pub.ECNCustomerID.ToString());
                cmd.Parameters.AddWithValue("@GroupID", pub.ECNDefaultGroupID.ToString());
                DataTable dt = DataFunctions.GetDataTable("communicator", cmd);

                if (dt.Rows.Count > 0)
                    return true;
            }

            return false;
        }

        #endregion

        protected void rbuser_SUBSCRIPTION_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbuser_SUBSCRIPTION.SelectedValue.Equals("Y"))
            {
                pnlReceiveCopy.Visible = true;
            }
            else
            {
                pnlReceiveCopy.Visible = false;
                pnlPayPalQuestions.Visible = false;
                chk_user_Demo7_Digital.Checked = false;
                chk_user_Demo7_Print.Checked = false;
                plDemoQuestions.Visible = false;
            }
        }

        protected void chk_user_Demo7_Digital_CheckedChanged(object sender, EventArgs e)
        {
            plDemoQuestions.Visible = true;
            pnlPayPalQuestions.Visible = false;
            chk_user_Demo7_Both.Checked = false;
            chk_user_Demo7_Print.Checked = false;
        }

        protected void chk_user_Demo7_Print_CheckedChanged(object sender, EventArgs e)
        {
            plDemoQuestions.Visible = true;
            if (pf.IsPaid)
                pnlPayPalQuestions.Visible = true;
            else
                pnlPayPalQuestions.Visible = false;

            chk_user_Demo7_Digital.Checked = false;
            chk_user_Demo7_Both.Checked = false;
        }

        protected void chk_user_Demo7_Both_CheckedChanged(object sender, EventArgs e)
        {
            plDemoQuestions.Visible = true;
            if (pf.IsPaid)
                pnlPayPalQuestions.Visible = true;
            else
                pnlPayPalQuestions.Visible = false;

            chk_user_Demo7_Digital.Checked = false;
            chk_user_Demo7_Print.Checked = false;
        }

        #region Question

        private void LoadSubscriptionForm()
        {
            getPubForm();

            if (pf != null)
            {
                if (pf.SUBSCRIPTIONQuestion.Equals(string.Empty))
                    lblSubscriptionQuestion.Text = "Do you wish to receive/continue receiving your copy of " + pub.PubName + " magazine?";
                else
                    lblSubscriptionQuestion.Text = pf.SUBSCRIPTIONQuestion;

                if (pf.PRINTDIGITALQuestion.Equals(string.Empty))
                    lblPRINTDIGITALQuestion.Text = "How would you like to receive your copy of " + pub.PubName + " magazine?";
                else
                    lblPRINTDIGITALQuestion.Text = pf.PRINTDIGITALQuestion;

                if (!pf.ShowPrint)
                    chk_user_Demo7_Print.Visible = false;
                else
                    chk_user_Demo7_Print.Visible = true;

                if (!pf.ShowDigital)
                    chk_user_Demo7_Digital.Visible = false;
                else
                    chk_user_Demo7_Digital.Visible = true;

                if (!pf.EnablePrintAndDigital)
                    chk_user_Demo7_Both.Visible = false;
                else
                    chk_user_Demo7_Both.Visible = true;

                if (!pf.ShowPrint && !pf.ShowDigital && !pf.EnablePrintAndDigital)
                {
                    pnlReceiveCopy.Visible = false;
                    pnlSubscription.Visible = false;
                }


                if (pf.ShowNewsletterSearch)
                    pnlSearch.Visible = true;
                else
                    pnlSearch.Visible = false;

                RenderQuestions(FieldGroup.Profile);

                if (rbuser_SUBSCRIPTION.SelectedValue.Equals("Y") && (chk_user_Demo7_Print.Checked || chk_user_Demo7_Digital.Checked || (chk_user_Demo7_Both != null && chk_user_Demo7_Both.Checked && pf.EnablePrintAndDigital)))
                {
                    RenderQuestions(FieldGroup.Demographic);

                    // if it is a Paid form is paid then display paypal panel
                    if (pub.HasPaid && pf.IsPaid && !chk_user_Demo7_Digital.Checked)
                    {
                        PaypalPrice.Text = pf.PaidPrice.ToString();

                        if (!pub.ProcessExternal)
                            pnlPayPalQuestions.Visible = true;
                        else
                            pnlPayPalQuestions.Visible = false;
                    }
                    else
                    {
                        pnlPayPalQuestions.Visible = false;
                    }
                }
                else
                {
                    plDemoQuestions.Visible = false;
                }
            }
            else
            {
                plProfileQuestions.Visible = false;
                plDemoQuestions.Visible = false;
                pnlPayPalQuestions.Visible = false;
            }
        }

        private HtmlTable GetPhoneFields(PubFormField question)
        {
            string original_voice = question.ECNFieldName;
            HtmlTable phoneTable = new HtmlTable();
            phoneTable.Border = 0;
            HtmlTableRow phoneRow1 = new HtmlTableRow();
            HtmlTableCell phoneRow1Cell1 = new HtmlTableCell();
            HtmlTableCell phoneRow1Cell2 = new HtmlTableCell();
            HtmlTableCell phoneRow1Cell3 = new HtmlTableCell();
            HtmlTableCell phoneRow1Cell4 = new HtmlTableCell();
            HtmlTableRow phoneRow2 = new HtmlTableRow();
            HtmlTableCell phoneRow2Cell1 = new HtmlTableCell();
            HtmlTableCell phoneRow2Cell2 = new HtmlTableCell();
            HtmlTableCell phoneRow2Cell3 = new HtmlTableCell();
            HtmlTableCell phoneRow2Cell4 = new HtmlTableCell();
            if (drpCountry.SelectedValue.Equals("205") || drpCountry.SelectedValue.Equals("174"))
            {
                question.ECNFieldName = "Voice1";
                question.MaxLength = 3;
                phoneRow1Cell1.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Voice2";
                question.MaxLength = 3;
                phoneRow1Cell2.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Voice3";
                question.MaxLength = 4;
                phoneRow1Cell3.Controls.Add(CreateControl(question));

                if (question.Required)
                {
                    //phoneRow1Cell4.Controls.Add(SetRequiredValidation("question_Voice1", "Phone1"));
                    //phoneRow1Cell4.Controls.Add(SetRequiredValidation("question_Voice2", "Phone2"));
                    //phoneRow1Cell4.Controls.Add(SetRequiredValidation("question_Voice3", "Phone3"));

                    CustomValidator cv = new CustomValidator();
                    cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                    cv.ErrorMessage = question.DisplayName;
                    cv.Display = ValidatorDisplay.Dynamic;
                    cv.ClientValidationFunction = "ValidatePhone";
                    phoneRow1Cell4.Controls.Add(cv);
                }

                phoneRow1.Controls.Add(phoneRow1Cell1);
                phoneRow1.Controls.Add(phoneRow1Cell2);
                phoneRow1.Controls.Add(phoneRow1Cell3);
                phoneRow1.Controls.Add(phoneRow1Cell4);
                phoneTable.Controls.Add(phoneRow1);
            }
            else
            {
                question.ECNFieldName = "Voice1";
                question.MaxLength = 4;
                phoneRow1Cell1.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Voice2";
                question.MaxLength = 5;
                phoneRow1Cell2.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Voice3";
                question.MaxLength = 11;
                phoneRow1Cell3.Controls.Add(CreateControl(question));

                phoneRow2Cell1.Controls.Add(new LiteralControl("Country"));
                phoneRow2Cell1.Align = "middle";
                phoneRow2Cell2.Controls.Add(new LiteralControl("City"));
                phoneRow2Cell2.Align = "middle";
                phoneRow2Cell3.Controls.Add(new LiteralControl("Phone Number"));
                phoneRow2Cell3.Align = "middle";

                if (question.Required)
                {
                    //phoneRow1Cell4.Controls.Add(SetRequiredValidation("question_Voice1", "Phone1"));
                    //phoneRow1Cell4.Controls.Add(SetRequiredValidation("question_Voice2", "Phone2"));
                    //phoneRow1Cell4.Controls.Add(SetRequiredValidation("question_Voice3", "Phone3"));

                    CustomValidator cv = new CustomValidator();
                    cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                    cv.ErrorMessage = question.DisplayName;
                    cv.Display = ValidatorDisplay.Dynamic;
                    cv.ClientValidationFunction = "ValidatePhone";
                    phoneRow1Cell4.Controls.Add(cv);
                }

                phoneRow1.Controls.Add(phoneRow1Cell1);
                phoneRow1.Controls.Add(phoneRow1Cell2);
                phoneRow1.Controls.Add(phoneRow1Cell3);
                phoneRow1.Controls.Add(phoneRow1Cell4);
                phoneRow2.Controls.Add(phoneRow2Cell1);
                phoneRow2.Controls.Add(phoneRow2Cell2);
                phoneRow2.Controls.Add(phoneRow2Cell3);
                phoneRow2.Controls.Add(phoneRow2Cell4);
                phoneTable.Controls.Add(phoneRow1);
                phoneTable.Controls.Add(phoneRow2);
            }
            question.ECNFieldName = original_voice;

            return phoneTable;
        }

        private HtmlTable GetFaxFields(PubFormField question)
        {
            string original_fax = question.ECNFieldName;
            HtmlTable faxTable = new HtmlTable();
            faxTable.Border = 0;
            HtmlTableRow faxRow1 = new HtmlTableRow();
            HtmlTableCell faxRow1Cell1 = new HtmlTableCell();
            HtmlTableCell faxRow1Cell2 = new HtmlTableCell();
            HtmlTableCell faxRow1Cell3 = new HtmlTableCell();
            HtmlTableCell faxRow1Cell4 = new HtmlTableCell();
            HtmlTableRow faxRow2 = new HtmlTableRow();
            HtmlTableCell faxRow2Cell1 = new HtmlTableCell();
            HtmlTableCell faxRow2Cell2 = new HtmlTableCell();
            HtmlTableCell faxRow2Cell3 = new HtmlTableCell();
            HtmlTableCell faxRow2Cell4 = new HtmlTableCell();
            if (drpCountry.SelectedValue.Equals("205") || drpCountry.SelectedValue.Equals("174"))
            {
                question.ECNFieldName = "Fax1";
                question.MaxLength = 3;
                faxRow1Cell1.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Fax2";
                question.MaxLength = 3;
                faxRow1Cell2.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Fax3";
                question.MaxLength = 4;
                faxRow1Cell3.Controls.Add(CreateControl(question));

                if (question.Required)
                {
                    //faxRow1Cell4.Controls.Add(SetRequiredValidation("question_Fax1", "Fax1"));
                    //faxRow1Cell4.Controls.Add(SetRequiredValidation("question_Fax2", "Fax2"));
                    //faxRow1Cell4.Controls.Add(SetRequiredValidation("question_Fax3", "Fax3"));

                    CustomValidator cv = new CustomValidator();
                    cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                    cv.ErrorMessage = question.DisplayName;
                    cv.Display = ValidatorDisplay.Dynamic;
                    cv.ClientValidationFunction = "ValidateFax";
                    faxRow1Cell4.Controls.Add(cv);
                }

                faxRow1.Controls.Add(faxRow1Cell1);
                faxRow1.Controls.Add(faxRow1Cell2);
                faxRow1.Controls.Add(faxRow1Cell3);
                faxRow1.Controls.Add(faxRow1Cell4);
                faxTable.Controls.Add(faxRow1);
            }
            else
            {
                question.ECNFieldName = "Fax1";
                question.MaxLength = 4;
                faxRow1Cell1.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Fax2";
                question.MaxLength = 5;
                faxRow1Cell2.Controls.Add(CreateControl(question));

                question.ECNFieldName = "Fax3";
                question.MaxLength = 11;
                faxRow1Cell3.Controls.Add(CreateControl(question));
                faxRow2Cell1.Controls.Add(new LiteralControl("Country"));
                faxRow2Cell1.Align = "middle";
                faxRow2Cell2.Controls.Add(new LiteralControl("City"));
                faxRow2Cell2.Align = "middle";
                faxRow2Cell3.Controls.Add(new LiteralControl("Fax Number"));
                faxRow2Cell3.Align = "middle";

                if (question.Required)
                {
                    //faxRow1Cell4.Controls.Add(SetRequiredValidation("question_Fax1", "Fax1"));
                    //faxRow1Cell4.Controls.Add(SetRequiredValidation("question_Fax2", "Fax2"));
                    //faxRow1Cell4.Controls.Add(SetRequiredValidation("question_Fax3", "Fax3"));

                    CustomValidator cv = new CustomValidator();
                    cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                    cv.ErrorMessage = question.DisplayName;
                    cv.Display = ValidatorDisplay.Dynamic;
                    cv.ClientValidationFunction = "ValidateFax";
                    faxRow1Cell4.Controls.Add(cv);
                }

                faxRow1.Controls.Add(faxRow1Cell1);
                faxRow1.Controls.Add(faxRow1Cell2);
                faxRow1.Controls.Add(faxRow1Cell3);
                faxRow1.Controls.Add(faxRow1Cell4);
                faxRow2.Controls.Add(faxRow2Cell1);
                faxRow2.Controls.Add(faxRow2Cell2);
                faxRow2.Controls.Add(faxRow2Cell3);
                faxRow2.Controls.Add(faxRow2Cell4);
                faxTable.Controls.Add(faxRow1);
                faxTable.Controls.Add(faxRow2);
            }

            question.ECNFieldName = original_fax;

            return faxTable;
        }

        private void RenderQuestions(FieldGroup fg)
        {
            int ProfileFieldIndex = 0;
            int rowIndex = 0;
            string colsWidth = ((int)100 / ColsNo).ToString() + "%";

            PlaceHolder ph = (fg == FieldGroup.Profile ? plProfileQuestions : plDemoQuestions);
            List<PubFormField> pffs = pf.Fields.Where(p => p.Grouping == fg).ToList();

            if (pffs.Count == 0)
            {
                ph.Visible = false;
                return;
            }
            else
            {
                ph.Visible = true;
            }

            HtmlTable htable = new HtmlTable();
            htable.Width = Unit.Percentage(100.0).ToString();
            htable.Border = 0;
            HtmlTableRow[] tRows = new HtmlTableRow[pffs.Count];
            HtmlTableCell[,] tCells = new HtmlTableCell[pffs.Count, ColsNo];

            for (int i = 0; i < pffs.Count; i++)
            {
                tRows[i] = new HtmlTableRow();
            }

            for (int i = 0; i < pffs.Count; i++)
            {
                for (int j = 0; j < ColsNo; j++)
                {
                    tCells[i, j] = new HtmlTableCell();
                    //tCells[i, j].Align = "left";
                    //tCells[i, j].Style.Add("padding-top", "3px");
                    //tCells[i, j].Style.Add("padding-bottom", "3px");
                    //tCells[i, j].Style.Add("padding-left", "0px");
                }
            }

            foreach (PubFormField question in pffs)
            {
                HtmlTable tblDemoGraphic = new HtmlTable();
                tblDemoGraphic.Border = 0;
                tblDemoGraphic.Width = "100%";
                HtmlTableRow tr = new HtmlTableRow();
                tblDemoGraphic.ID = string.Format("tbl_q_{0}", question.ECNFieldName);
                tr.ID = string.Format("tr_q_{0}", question.ECNFieldName);
                tr.Visible = question.IsVisible;

                switch (question.ControlType)
                {
                    case ControlType.Checkbox:
                    case ControlType.CatCheckbox:
                    case ControlType.Dropdown:
                    case ControlType.Radio:

                        #region
                        if (fg == FieldGroup.Demographic)
                        {
                            HtmlTableCell td = new HtmlTableCell();
                            td.ColSpan = ColsNo;
                            td.Attributes.Add("class", "label");
                            td.VAlign = "middle";
                            td.Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));

                            try
                            {
                                if (question.Required)
                                {
                                    if (question.ControlType.Equals(ControlType.Checkbox))
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                        CustomValidator cv = new CustomValidator();
                                        cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                        cv.ErrorMessage = question.DisplayName;
                                        cv.Display = ValidatorDisplay.Dynamic;
                                        cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                        td.Controls.Add(cv);
                                    }
                                    else if (question.ControlType.Equals(ControlType.CatCheckbox))
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid =  CatCheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "');}", true);
                                        CustomValidator cv = new CustomValidator();
                                        cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                        cv.ErrorMessage = question.DisplayName;
                                        cv.Display = ValidatorDisplay.Dynamic;
                                        cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                        td.Controls.Add(cv);
                                    }
                                    else
                                        td.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                                }
                                else
                                {
                                    if (question.ControlType.Equals(ControlType.Checkbox) && question.MaxSelections > 0)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                        CustomValidator cv = new CustomValidator();
                                        cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                        cv.ErrorMessage = question.DisplayName;
                                        cv.Display = ValidatorDisplay.Dynamic;
                                        cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                        td.Controls.Add(cv);
                                    }
                                }
                            }
                            catch { }

                            td.Controls.Add(new LiteralControl("<BR>"));
                            td.Controls.Add(CreateControl(question));

                            #region Show TEXT FIELD FOR OTHER
                            try
                            {
                                if (question.ShowTextField)
                                {
                                    TextBox txtOptions = new TextBox();
                                    txtOptions.Width = 200;
                                    txtOptions.ID = string.Format("question_{0}", question.ECNTextFieldName);

                                    Panel optntextpanel = new Panel();
                                    optntextpanel.ID = string.Format("pnl_question_{0}", question.ECNTextFieldName);
                                    optntextpanel.Attributes.Add("style", "padding-left:0px;display:none;");

                                    optntextpanel.Controls.Add(txtOptions);
                                    optntextpanel.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNTextFieldName), question.DisplayName));
                                    //CustomValidator cv = new CustomValidator();
                                    //cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                    //cv.ErrorMessage = question.DisplayName;
                                    //cv.Display = ValidatorDisplay.Dynamic;
                                    //cv.ClientValidationFunction = string.Format("cv_optional_textbox_question", question.ECNFieldName);
                                    //optntextpanel.Controls.Add(cv);
                                    td.Controls.Add(optntextpanel);
                                }
                            }
                            catch { }
                            #endregion

                            tr.Controls.Add(td);
                            tblDemoGraphic.Visible = question.IsVisible;
                            tblDemoGraphic.Controls.Add(tr);
                            ph.Controls.Add(tblDemoGraphic);
                        }
                        else if (fg == FieldGroup.Profile)
                        {
                            if (ProfileFieldIndex == ColsNo)
                            {
                                rowIndex = rowIndex + 1;
                                ProfileFieldIndex = 0;
                            }

                            if (question.DisplayName.Length > 30)
                            {
                                HtmlTableRow trq = new HtmlTableRow();
                                trq.ID = string.Format("tr_q_{0}", question.ECNFieldName);
                                trq.Visible = question.IsVisible;
                                HtmlTableCell tdq = new HtmlTableCell();
                                tdq.ColSpan = ColsNo;
                                tdq.Attributes.Add("class", "label");
                                tdq.VAlign = "middle";
                                tdq.Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));

                                try
                                {
                                    if (question.Required)
                                    {
                                        if (question.ControlType.Equals(ControlType.Checkbox))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                            tdq.Controls.Add(cv);
                                        }
                                        else if (question.ControlType.Equals(ControlType.CatCheckbox))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid =  CatCheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "');}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                            tdq.Controls.Add(cv);
                                        }
                                        else
                                        {
                                            tdq.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                                        }
                                    }
                                    else
                                    {
                                        if (question.ControlType.Equals(ControlType.Checkbox) && question.MaxSelections > 0)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                            tdq.Controls.Add(cv);
                                        }
                                    }
                                }
                                catch { }

                                tdq.Controls.Add(new LiteralControl("<BR>"));
                                tdq.Controls.Add(CreateControl(question));
                                trq.Controls.Add(tdq);

                                #region Show TEXT FIELD FOR OTHER
                                try
                                {
                                    if (question.ShowTextField)
                                    {
                                        TextBox txtOptions = new TextBox();
                                        txtOptions.Width = 200;
                                        txtOptions.ID = string.Format("question_{0}", question.ECNTextFieldName);

                                        Panel optntextpanel = new Panel();
                                        optntextpanel.ID = string.Format("pnl_question_{0}", question.ECNTextFieldName);
                                        optntextpanel.Attributes.Add("style", "padding-left:0px;display:none");

                                        optntextpanel.Controls.Add(txtOptions);
                                        optntextpanel.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNTextFieldName), question.DisplayName));

                                        tdq.Controls.Add(optntextpanel);
                                    }
                                }
                                catch { }
                                #endregion

                                htable.Rows.Add(trq);
                                ProfileFieldIndex = 0;
                                rowIndex = rowIndex + 1;
                            }
                            else if (question.SeparatorType == "L" || question.SeparatorType == "B" || !question.IsVisible)  //add dependant question condition
                            {
                                #region

                                HtmlTableRow trq = new HtmlTableRow();
                                trq.Visible = question.IsVisible;
                                trq.ID = string.Format("tr_q_{0}", question.ECNFieldName);
                                HtmlTableCell tdq1 = new HtmlTableCell();
                                HtmlTableCell tdq2 = new HtmlTableCell();
                                tdq1.Width = "20%";
                                tdq2.Width = "80%";
                                tdq2.ColSpan = 3;
                                tdq1.Attributes.Add("class", "label");
                                tdq1.VAlign = "middle";
                                tdq1.Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));
                                tdq2.Controls.Add(CreateControl(question));
                                trq.Controls.Add(tdq1);
                                trq.Controls.Add(tdq2);

                                try
                                {
                                    if (question.Required)
                                    {
                                        if (question.ControlType.Equals(ControlType.Checkbox))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            tdq2.Controls.Add(cv);
                                        }
                                        if (question.ControlType.Equals(ControlType.CatCheckbox))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid =  CatCheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "');}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            tdq2.Controls.Add(cv);
                                        }
                                        else
                                        {
                                            tdq2.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                                        }
                                    }
                                    else
                                    {
                                        if (question.ControlType.Equals(ControlType.Checkbox) && question.MaxSelections > 0)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            tdq2.Controls.Add(cv);
                                        }
                                    }
                                }
                                catch { }

                                #region Show TEXT FIELD FOR OTHER
                                try
                                {
                                    if (question.ShowTextField)
                                    {
                                        TextBox txtOptions = new TextBox();
                                        txtOptions.Width = 200;
                                        txtOptions.ID = string.Format("question_{0}", question.ECNTextFieldName);

                                        Panel optntextpanel = new Panel();
                                        optntextpanel.ID = string.Format("pnl_question_{0}", question.ECNTextFieldName);
                                        optntextpanel.Attributes.Add("style", "padding-left:0px;display:none");

                                        optntextpanel.Controls.Add(txtOptions);
                                        optntextpanel.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNTextFieldName), question.DisplayName));

                                        tdq2.Controls.Add(optntextpanel);
                                    }
                                }
                                catch { }
                                #endregion

                                htable.Rows.Add(trq);
                                ProfileFieldIndex = 0;
                                rowIndex = rowIndex + 1;
                                #endregion
                            }
                            else
                            {
                                tCells[rowIndex, ProfileFieldIndex].Width = (ColsNo == 4 ? "20%" : Resources.Resource.ResourceManager.GetObject("profileleftColWidth").ToString());
                                tCells[rowIndex, ProfileFieldIndex].Attributes.Add("class", "label");
                                tCells[rowIndex, ProfileFieldIndex].VAlign = "top";

                                tCells[rowIndex, ProfileFieldIndex + 1].Width = (ColsNo == 4 ? "30%" : Resources.Resource.ResourceManager.GetObject("profileRightColWidth").ToString());
                                tCells[rowIndex, ProfileFieldIndex + 1].Attributes.Add("class", "label");
                                tCells[rowIndex, ProfileFieldIndex + 1].VAlign = "middle";

                                tCells[rowIndex, ProfileFieldIndex].Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));
                                tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(CreateControl(question));

                                tCells[rowIndex, ProfileFieldIndex + 1].Visible = question.IsVisible;

                                try
                                {
                                    if (question.Required)
                                    {
                                        if (question.ControlType.Equals(ControlType.Checkbox))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                            tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(cv);
                                        }
                                        else if (question.ControlType.Equals(ControlType.Checkbox) || question.ControlType.Equals(ControlType.CatCheckbox))
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid =  CatCheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "');}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                            tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(cv);
                                        }
                                        else
                                        {
                                            tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                                        }
                                    }
                                    else
                                    {
                                        if (question.ControlType.Equals(ControlType.Checkbox) && question.MaxSelections > 0)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                            CustomValidator cv = new CustomValidator();
                                            cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                            cv.ErrorMessage = question.DisplayName;
                                            cv.Display = ValidatorDisplay.Dynamic;
                                            cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                            tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(cv);
                                        }
                                    }
                                }
                                catch { }

                                #region Show TEXT FIELD FOR OTHER
                                try
                                {
                                    if (question.ShowTextField)
                                    {
                                        TextBox txtOptions = new TextBox();
                                        txtOptions.Width = 200;
                                        txtOptions.ID = string.Format("question_{0}", question.ECNTextFieldName);

                                        Panel optntextpanel = new Panel();
                                        optntextpanel.ID = string.Format("pnl_question_{0}", question.ECNTextFieldName);
                                        optntextpanel.Attributes.Add("style", "padding-left:0px;display:none;");
                                        optntextpanel.Controls.Add(txtOptions);
                                        optntextpanel.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNTextFieldName), question.DisplayName));

                                        tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(optntextpanel);
                                    }
                                }
                                catch { }
                                #endregion

                                try
                                {
                                    tRows[rowIndex].ID = string.Format("tr_q_{0}", question.ECNFieldName);
                                    tRows[rowIndex].Visible = question.IsVisible;
                                    tRows[rowIndex].Controls.Add(tCells[rowIndex, ProfileFieldIndex]);
                                    tRows[rowIndex].Controls.Add(tCells[rowIndex, ProfileFieldIndex + 1]);
                                    ProfileFieldIndex = ProfileFieldIndex + 2;
                                    htable.Rows.Add(tRows[rowIndex]);
                                }
                                catch { }
                            }
                        }
                        break;
                    #endregion Radio

                    case ControlType.TextBox:

                        #region Textbox

                        if (fg == FieldGroup.Demographic)
                        {
                            HtmlTableCell td = new HtmlTableCell();
                            td.ColSpan = ColsNo;
                            td.Attributes.Add("class", "label");
                            td.VAlign = "middle";
                            td.Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));
                            td.Controls.Add(new LiteralControl("<BR>"));
                            td.Controls.Add(CreateControl(question));

                            try
                            {
                                if (question.Required)
                                    td.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                            }
                            catch { }

                            tr.Controls.Add(td);
                            tblDemoGraphic.Controls.Add(tr);
                            ph.Controls.Add(tblDemoGraphic);
                        }
                        else
                        {
                            if (ProfileFieldIndex == ColsNo)
                            {
                                rowIndex = rowIndex + 1;
                                ProfileFieldIndex = 0;
                            }

                            if (question.DisplayName.Length > 30)
                            {
                                HtmlTableRow trq = new HtmlTableRow();
                                trq.Visible = question.IsVisible;
                                trq.ID = trq.ID = string.Format("tr_q_{0}", question.ECNFieldName);
                                HtmlTableCell tdq = new HtmlTableCell();
                                tdq.ColSpan = ColsNo;
                                tdq.Attributes.Add("class", "label");
                                tdq.VAlign = "middle";
                                tdq.Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));
                                tdq.Controls.Add(new LiteralControl("<BR>"));

                                if (question.ECNFieldName.ToLower().Equals("voice"))
                                {
                                    ViewState["Phone_ECNFieldName"] = question.ECNFieldName;
                                    tdq.Controls.Add(GetPhoneFields(question));
                                }
                                else if (question.ECNFieldName.ToLower().Equals("fax"))
                                {
                                    ViewState["Fax_ECNFieldName"] = question.ECNFieldName;
                                    tdq.Controls.Add(GetFaxFields(question));
                                }
                                else
                                {
                                    tdq.Controls.Add(CreateControl(question));
                                    try
                                    {
                                        if (question.Required)
                                        {
                                            tdq.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                                        }
                                    }
                                    catch { }
                                }
                                trq.Controls.Add(tdq);
                                htable.Rows.Add(trq);
                                ProfileFieldIndex = 0;
                                rowIndex = rowIndex + 1;
                            }
                            else if (question.SeparatorType == "L" || question.SeparatorType == "B" || !question.IsVisible)
                            {
                                HtmlTableRow trq = new HtmlTableRow();
                                trq.Visible = question.IsVisible;
                                trq.ID = string.Format("tr_q_{0}", question.ECNFieldName);
                                HtmlTableCell tdq1 = new HtmlTableCell();
                                HtmlTableCell tdq2 = new HtmlTableCell();
                                tdq1.Width = "20%";
                                tdq2.Width = "80%";
                                tdq2.ColSpan = 3;
                                tdq2.Attributes.Add("class", "label");
                                tdq1.Attributes.Add("class", "label");
                                tdq1.VAlign = "middle";
                                tdq1.Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));
                                if (question.ECNFieldName.ToLower().Equals("voice"))
                                {
                                    ViewState["Phone_ECNFieldName"] = question.ECNFieldName;
                                    tdq2.Controls.Add(GetPhoneFields(question));
                                }
                                else if (question.ECNFieldName.ToLower().Equals("fax"))
                                {
                                    ViewState["Fax_ECNFieldName"] = question.ECNFieldName;
                                    tdq2.Controls.Add(GetFaxFields(question));
                                }
                                else
                                {
                                    tdq2.Controls.Add(CreateControl(question));
                                    try
                                    {
                                        if (question.Required)
                                        {
                                            if (question.ControlType.Equals(ControlType.Checkbox))
                                            {
                                                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                                CustomValidator cv = new CustomValidator();
                                                cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                                cv.ErrorMessage = question.DisplayName;
                                                cv.Display = ValidatorDisplay.Dynamic;
                                                cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                                tdq2.Controls.Add(cv);
                                            }
                                            else if (question.ControlType.Equals(ControlType.CatCheckbox))
                                            {
                                                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid =  CatCheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "');}", true);
                                                CustomValidator cv = new CustomValidator();
                                                cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                                cv.ErrorMessage = question.DisplayName;
                                                cv.Display = ValidatorDisplay.Dynamic;
                                                cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                                tdq2.Controls.Add(cv);
                                            }
                                            else
                                            {
                                                tdq2.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                                            }
                                        }
                                        else
                                        {
                                            if (question.ControlType.Equals(ControlType.Checkbox) && question.MaxSelections > 0)
                                            {
                                                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), string.Format("cv_question_{0}", question.ECNFieldName), "function " + string.Format("cv_question_{0}", question.ECNFieldName) + "(source, args) {args.IsValid = CheckBox_RFV('" + string.Format("question_{0}", question.ECNFieldName) + "'," + (question.Required ? "1" : "0") + "," + question.MaxSelections + ");}", true);
                                                CustomValidator cv = new CustomValidator();
                                                cv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
                                                cv.ErrorMessage = question.DisplayName;
                                                cv.Display = ValidatorDisplay.Dynamic;
                                                cv.ClientValidationFunction = string.Format("cv_question_{0}", question.ECNFieldName);
                                                tdq2.Controls.Add(cv);
                                            }
                                        }
                                    }
                                    catch { }
                                }

                                trq.Controls.Add(tdq1);
                                trq.Controls.Add(tdq2);



                                #region Show TEXT FIELD FOR OTHER
                                try
                                {
                                    if (question.ShowTextField)
                                    {
                                        TextBox txtOptions = new TextBox();
                                        txtOptions.Width = 200;
                                        txtOptions.ID = string.Format("question_{0}", question.ECNTextFieldName);

                                        Panel optntextpanel = new Panel();
                                        optntextpanel.ID = string.Format("pnl_question_{0}", question.ECNTextFieldName);
                                        optntextpanel.Attributes.Add("style", "padding-left:0px;display:none");

                                        optntextpanel.Controls.Add(txtOptions);
                                        optntextpanel.Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNTextFieldName), question.DisplayName));

                                        tdq2.Controls.Add(optntextpanel);
                                    }
                                }
                                catch { }
                                #endregion

                                htable.Rows.Add(trq);
                                ProfileFieldIndex = 0;
                                rowIndex = rowIndex + 1;
                            }
                            else
                            {
                                tCells[rowIndex, ProfileFieldIndex].Width = (ColsNo == 4 ? "20%" : Resources.Resource.ResourceManager.GetObject("profileleftColWidth").ToString());
                                tCells[rowIndex, ProfileFieldIndex].Attributes.Add("class", "label");
                                tCells[rowIndex, ProfileFieldIndex].VAlign = "middle";

                                tCells[rowIndex, ProfileFieldIndex + 1].Width = (ColsNo == 4 ? "30%" : Resources.Resource.ResourceManager.GetObject("profileRightColWidth").ToString());
                                tCells[rowIndex, ProfileFieldIndex + 1].Attributes.Add("class", "label");
                                tCells[rowIndex, ProfileFieldIndex + 1].VAlign = "middle";

                                tCells[rowIndex, ProfileFieldIndex].Controls.Add(new LiteralControl((question.Required ? "* " : "") + question.DisplayName));
                                if (question.ECNFieldName.ToLower().Equals("voice"))
                                {
                                    ViewState["Phone_ECNFieldName"] = question.ECNFieldName;
                                    tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(GetPhoneFields(question));
                                }
                                else if (question.ECNFieldName.ToLower().Equals("fax"))
                                {
                                    ViewState["Fax_ECNFieldName"] = question.ECNFieldName;
                                    tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(GetFaxFields(question));
                                }
                                else
                                {
                                    tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(CreateControl(question));
                                    try
                                    {
                                        if (question.Required)
                                        {
                                            tCells[rowIndex, ProfileFieldIndex + 1].Controls.Add(SetRequiredValidation(string.Format("question_{0}", question.ECNFieldName), question.DisplayName));
                                        }
                                    }
                                    catch { }
                                }
                                try
                                {
                                    tRows[rowIndex].ID = string.Format("tr_q_{0}", question.ECNFieldName);
                                    tRows[rowIndex].Visible = question.IsVisible;
                                    tRows[rowIndex].Controls.Add(tCells[rowIndex, ProfileFieldIndex]);
                                    tRows[rowIndex].Controls.Add(tCells[rowIndex, ProfileFieldIndex + 1]);
                                    ProfileFieldIndex = ProfileFieldIndex + 2;
                                    htable.Rows.Add(tRows[rowIndex]);
                                }
                                catch { }
                            }
                        }
                        break;
                    #endregion Textbox

                    case KMPS_JF_Objects.Objects.ControlType.Hidden:
                        HtmlTableCell td3 = new HtmlTableCell();
                        td3.ColSpan = ColsNo;
                        td3.Attributes.Add("class", "label");
                        td3.VAlign = "middle";
                        td3.Controls.Add(CreateControl(question));
                        tr.Controls.Add(td3);
                        htable.Rows.Add(tr);
                        break;
                }

                HtmlTableRow tr_Separator = new HtmlTableRow();
                tr_Separator.ID = string.Format("tr2_q_{0}", question.ECNFieldName);

                // Add Line 
                if (question.AddSeparator && fg == FieldGroup.Profile)
                {
                    HtmlTableCell td9 = new HtmlTableCell();
                    td9.ColSpan = ColsNo;
                    td9.Attributes.Add("class", "label");
                    td9.VAlign = "middle";

                    if (question.SeparatorType == "L")
                    {
                        td9.Controls.Add(new LiteralControl("<br/><hr color='black'/>"));
                    }
                    else if (question.SeparatorType == "B")
                    {
                        td9.Controls.Add(new LiteralControl("&nbsp;"));
                    }

                    tr_Separator.Controls.Add(td9);
                    htable.Rows.Add(tr_Separator);
                }
                else if (question.AddSeparator && fg == FieldGroup.Demographic)
                {
                    HtmlTableCell td = new HtmlTableCell();
                    td.ColSpan = ColsNo;
                    td.Attributes.Add("class", "label");
                    td.VAlign = "middle";

                    if (question.SeparatorType == "L")
                    {
                        td.Controls.Add(new LiteralControl("<br/><hr color='black'/>"));
                    }
                    else if (question.SeparatorType == "B")
                    {
                        td.Controls.Add(new LiteralControl("&nbsp;"));
                    }
                    tr_Separator.Controls.Add(td);
                    tblDemoGraphic.Rows.Add(tr_Separator);
                }
                else if (fg == FieldGroup.Demographic)
                {
                    HtmlTableCell td = new HtmlTableCell();
                    td.ColSpan = ColsNo;
                    td.Attributes.Add("class", "label");
                    td.VAlign = "middle";
                    td.Controls.Add(new LiteralControl("&nbsp;"));
                    tr_Separator.Controls.Add(td);
                    tblDemoGraphic.Rows.Add(tr_Separator);
                }
            }


            foreach (HtmlTableRow r in htable.Rows)
            {
                if (r.Cells.Count != 1)
                {
                    foreach (HtmlTableCell c in r.Cells)
                    {
                        c.Align = "left";
                        c.Style.Add("padding-top", "3px");
                        c.Style.Add("padding-bottom", "3px");
                        c.Style.Add("padding-left", "0px");
                    }
                }
            }


            ph.Controls.Add(htable);
            makeVisible(ph);
        }

        private void makeVisible(Control ph)
        {
            foreach (Control c in ph.Controls)
            {
                c.Visible = true;
                makeVisible(c);
            }
        }

        private string[] GetValuesForPrePopulation(string vals, char sep)
        {
            return vals.Split(sep);
        }

        private Control CreateControl(PubFormField question)
        {

            Subscriber = SubscriberFromCacheorSession;

            List<FieldData> fdd = null;

            try
            {
                fdd = question.Data.FindAll(x => x.IsDefault == true);
            }
            catch { fdd = null; }

            switch (question.ControlType)
            {
                case ControlType.CatCheckbox:
                    CategorizedCheckBoxList CCBox = new CategorizedCheckBoxList();

                    CCBox.AutoPostBack = question.AutoPostBack;
                    CCBox.CategoryCssClass = "label Grouppadding";
                    CCBox.TextCssClass = "labelAnswer addpadding";

                    #region Build datatable from LIST<>

                    DataTable dtOptions = new DataTable();

                    DataColumn dc;
                    DataRow dr;

                    dc = new DataColumn("DataText");
                    dtOptions.Columns.Add(dc);

                    dc = new DataColumn("DataValue");
                    dtOptions.Columns.Add(dc);

                    dc = new DataColumn("Category");
                    dtOptions.Columns.Add(dc);

                    foreach (FieldData fd in question.Data)
                    {
                        dr = dtOptions.NewRow();
                        dr["DataText"] = fd.DataText;
                        dr["DataValue"] = fd.DataValue;
                        dr["Category"] = fd.Category;

                        dtOptions.Rows.Add(dr);
                    }

                    #endregion

                    CCBox.DataTable = dtOptions;
                    CCBox.Columns = question.ColumnFormat;
                    CCBox.SharedTable = true;

                    CCBox.DataTextColumn = "DataText";
                    CCBox.DataValueColumn = "DataValue";
                    CCBox.DataCategoryColumn = "Category";

                    CCBox.ID = string.Format("question_{0}", question.ECNFieldName);
                    CCBox.Visible = question.IsVisible;

                    try
                    {
                        if (question.ShowTextField)
                        {
                            CCBox.AutoPostBack = true;
                            CCBox.Attributes.Add("onclick", "javascript:EnableTextControl('cc', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (dtOptions.Rows.Count) + ");");
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), string.Format("call_enable_rfv_question_{0}", question.ECNTextFieldName), "EnableTextControl('cc', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (dtOptions.Rows.Count) + ");", true);
                        }
                    }
                    catch
                    { }

                    CCBox.DataBind();

                    if (fdd != null)
                    {
                        try
                        {
                            CCBox.Selections.Clear();

                            foreach (FieldData fddt in fdd)
                                CCBox.Selections.Add(fddt.DataValue);
                        }
                        catch { }
                    }

                    if (question.ECNFieldName.ToUpper() == "VERIFY" || !question.IsPrepopulate)
                    {
                        return CCBox;
                    }

                    try
                    {
                        if (Subscriber != null && !CatCheckBoxLoaded)
                        {
                            string[] selectionList = GetValuesForPrePopulation(Subscriber[question.ECNFieldName.ToUpper()].ToString(), ',');
                            CCBox.Selections.Clear();

                            foreach (string st in selectionList)
                                CCBox.Selections.Add(st);

                            Session["CatCheckBoxLoaded"] = true;
                        }
                    }
                    catch { }

                    return CCBox;

                case ControlType.Checkbox:
                    CheckBoxList chkOption = new CheckBoxList();
                    chkOption.CssClass = "labelAnswer";
                    chkOption.AutoPostBack = question.AutoPostBack;
                    chkOption.DataSource = question.Data;
                    chkOption.DataTextField = "DataText";
                    chkOption.DataValueField = "DataValue";
                    chkOption.ID = string.Format("question_{0}", question.ECNFieldName);
                    chkOption.Visible = question.IsVisible;

                    chkOption.RepeatColumns = question.ColumnFormat;

                    if (question.RepeatDirection.ToUpper() == "VER")
                        chkOption.RepeatDirection = RepeatDirection.Vertical;
                    else
                        chkOption.RepeatDirection = RepeatDirection.Horizontal;

                    chkOption.RepeatLayout = RepeatLayout.Table;
                    chkOption.TextAlign = TextAlign.Right;

                    if (question.ColumnFormat >= 3)
                    {
                        chkOption.Width = Unit.Percentage(100);
                    }

                    try
                    {
                        if (question.ShowTextField)
                        {
                            chkOption.Attributes.Add("onclick", "javascript:EnableTextControl('c', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (question.Data.Count) + ");");
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), string.Format("call_enable_rfv_question_{0}", question.ECNTextFieldName), "EnableTextControl('c', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (question.Data.Count) + ");", true);
                        }
                    }
                    catch
                    { }

                    chkOption.DataBind();

                    if (fdd != null)
                    {
                        try
                        {
                            chkOption.ClearSelection();

                            foreach (FieldData fddt in fdd)
                                chkOption.Items.FindByValue(fddt.DataValue).Selected = true;
                        }
                        catch { }
                    }

                    if (question.ECNFieldName.ToUpper() == "VERIFY" || !question.IsPrepopulate)
                    {
                        return chkOption;
                    }

                    try
                    {
                        if (Subscriber != null)
                        {
                            string[] selectionList = GetValuesForPrePopulation(Subscriber[question.ECNFieldName.ToUpper()].ToString(), ',');
                            chkOption.ClearSelection();
                            foreach (string st in selectionList)
                                chkOption.Items.FindByValue(st).Selected = true;
                        }
                    }
                    catch { }

                    divcss.InnerHtml += "<style>#question_" + question.ECNFieldName + " td{vertical-align:top; border: 0px solid black;padding: 1px 1px 1px 1px; } #question_" + question.ECNFieldName + " label { margin-left: 2px;}</style>";

                    return chkOption;
                case ControlType.Dropdown:
                    DropDownList ddlOption = new DropDownList();
                    ddlOption.CssClass = "labelAnswer";
                    ddlOption.ID = string.Format("question_{0}", question.ECNFieldName);
                    ddlOption.Visible = question.IsVisible;
                    ddlOption.AutoPostBack = question.AutoPostBack;

                    if (!question.ECNFieldName.Equals("STATE", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {   // If Querystring value exists popuplate 
                            if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                            {
                                ddlOption.Items.Insert(0, new ListItem(getQueryString(question.QueryStringName), getQueryString(question.QueryStringName)));
                                ddlOption.ClearSelection();
                                ddlOption.Items.FindByValue(getQueryString(question.QueryStringName)).Selected = true;
                            }
                            else
                            {
                                foreach (FieldData fd in question.Data)
                                {
                                    ListItem item = new ListItem(fd.DataText, fd.DataValue);

                                    if (!fd.Category.Equals(string.Empty))
                                    {
                                        item.Attributes["OptionGroup"] = fd.Category;
                                    }

                                    ddlOption.Items.Add(item);
                                }

                                try
                                {
                                    if (question.ShowTextField)
                                    {
                                        ddlOption.Attributes.Add("onChange", "javascript:EnableTextControl('d', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (question.Data.Count) + ");");
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), string.Format("call_enable_rfv_question_{0}", question.ECNTextFieldName), "EnableTextControl('d', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (question.Data.Count) + ");", true);
                                    }
                                }
                                catch
                                { }

                                ddlOption.DataBind();

                                if (Resources.Resource.ResourceManager.GetObject("selectanitem") != null)
                                    ddlOption.Items.Insert(0, new ListItem(Resources.Resource.ResourceManager.GetObject("selectanitem").ToString(), ""));

                                if (fdd != null)
                                {
                                    try
                                    {
                                        ddlOption.ClearSelection();
                                        ddlOption.Items.FindByValue(fdd[0].DataValue).Selected = true;
                                    }
                                    catch { }
                                }
                            }
                        }
                        catch
                        { }
                    }
                    else if (question.ECNFieldName.Equals("STATE", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlOption.Items.Clear();
                        ddlOption.DataSource = State.GetStates(CountryID);
                        ddlOption.DataTextField = "StateName";
                        ddlOption.DataValueField = "StateAbbr";
                        ddlOption.DataBind();

                        if (drpCountry.SelectedItem.Value == "174")
                            ddlOption.Items.Insert(0, new ListItem("----- SELECT PROVINCE  -----", ""));
                        else
                            ddlOption.Items.Insert(0, new ListItem("----- SELECT STATE  -----", ""));
                    }

                    if (question.ECNFieldName.ToUpper() == "VERIFY" || !question.IsPrepopulate)
                    {
                        ddlOption.ClearSelection();
                        return ddlOption;
                    }

                    //code for prepopulation
                    try
                    {
                        if (Subscriber != null)
                        {
                            ddlOption.ClearSelection();
                            ddlOption.Items.FindByValue(Subscriber[question.ECNFieldName.ToUpper()].ToString()).Selected = true;
                        }
                    }
                    catch { }

                    return ddlOption;
                case ControlType.Radio:
                    RadioButtonList rdoOptions = new RadioButtonList();
                    rdoOptions.CssClass = "labelAnswer";

                    rdoOptions.AutoPostBack = question.AutoPostBack;
                    rdoOptions.Visible = question.IsVisible;

                    rdoOptions.DataSource = question.Data;
                    rdoOptions.DataTextField = "DataText";
                    rdoOptions.DataValueField = "DataValue";
                    rdoOptions.ID = string.Format("question_{0}", question.ECNFieldName);

                    if (question.RepeatDirection.ToUpper() == "VER")
                        rdoOptions.RepeatDirection = RepeatDirection.Vertical;
                    else
                        rdoOptions.RepeatDirection = RepeatDirection.Horizontal;

                    rdoOptions.RepeatLayout = RepeatLayout.Table;

                    if (question.ColumnFormat >= 3)
                    {
                        rdoOptions.Width = Unit.Percentage(100);
                    }

                    try
                    {
                        if (question.ShowTextField)
                        {
                            rdoOptions.Attributes.Add("onclick", "javascript:EnableTextControl('r', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (question.Data.Count) + ");");
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), string.Format("call_enable_rfv_question_{0}", question.ECNTextFieldName), "EnableTextControl('r', '" + string.Format("question_{0}", question.ECNFieldName) + "','" + string.Format("question_{0}", question.ECNTextFieldName) + "'," + (question.Data.Count) + ");", true);
                        }
                    }
                    catch
                    { }
                    rdoOptions.DataBind();

                    if (fdd != null && fdd.Count > 0)
                    {
                        try
                        {
                            rdoOptions.ClearSelection();
                            rdoOptions.Items.FindByValue(fdd[0].DataValue).Selected = true;
                        }
                        catch { }
                    }

                    //Prepopulate radio button field
                    if (!question.IsPrepopulate || question.ECNFieldName.ToUpper() == "VERIFY")
                    {
                        return rdoOptions;
                    }

                    try
                    {
                        if (Subscriber != null)
                        {
                            rdoOptions.ClearSelection();
                            rdoOptions.Items.FindByValue(Subscriber[question.ECNFieldName.ToUpper()].ToString()).Selected = true;
                        }
                    }
                    catch { }

                    divcss.InnerHtml += "<style>#question_" + question.ECNFieldName + " td{vertical-align:top; border: 0px solid black;padding: 1px 1px 1px 1px; } #question_" + question.ECNFieldName + " label { margin-left: 2px;}</style>";

                    return rdoOptions;
                case ControlType.TextBox:

                    TextBox txtOptions = new TextBox();
                    txtOptions.CssClass = "labelAnswer";
                    txtOptions.Visible = question.IsVisible;
                    if (question.MaxLength > 0)
                        txtOptions.Attributes.Add("maxLength", question.MaxLength.ToString());

                    txtOptions.ID = string.Format("question_{0}", question.ECNFieldName);
                    if (pub.Width == "100%" || ColsNo == 2)   // SET width either for full screen or two column format
                    {
                        txtOptions.Width = 200;
                    }

                    if (question.ECNFieldName.Equals("Voice1") || question.ECNFieldName.Equals("Voice2") || question.ECNFieldName.Equals("Fax1") || question.ECNFieldName.Equals("Fax2"))
                    {
                        txtOptions.Attributes.Add("onkeypress", "return isNumberKey(event)");
                        txtOptions.Width = 40;
                    }

                    if (question.ECNFieldName.Equals("Voice3") || question.ECNFieldName.Equals("Fax3"))
                    {
                        if (ColsNo == 2)   // SET width either for full screen or two column format
                        {
                            txtOptions.Attributes.Add("onkeypress", "return isNumberKey(event)");
                            txtOptions.Width = 100;
                        }
                        else if (ColsNo == 4)   // SET width either for full screen or two column format
                        {
                            txtOptions.Attributes.Add("onkeypress", "return isNumberKey(event)");
                            txtOptions.Width = 40;
                        }
                    }


                    if (question.MaxLength <= 50)
                        txtOptions.Rows = 1;
                    else if (question.MaxLength > 50 && question.MaxLength < 100)
                        txtOptions.Rows = 2;
                    else if (question.MaxLength > 100 && question.MaxLength < 150)
                        txtOptions.Rows = 3;
                    else if (question.MaxLength > 150 && question.MaxLength < 200)
                        txtOptions.Rows = 4;
                    else if (question.MaxLength > 200 && question.MaxLength <= 250)
                        txtOptions.Rows = 5;

                    if (txtOptions.Rows > 1)
                        txtOptions.TextMode = TextBoxMode.MultiLine;

                    if (question.ECNFieldName.ToUpper() == "VERIFY" || !question.IsPrepopulate && !question.ECNFieldName.Equals("ZIP", StringComparison.OrdinalIgnoreCase))
                    {
                        return txtOptions;
                    }

                    //Code for prepopulation
                    try
                    {

                        if (Subscriber == null && question.ECNFieldName.Equals("ZIP", StringComparison.OrdinalIgnoreCase))
                        {
                            CustomValidator stateZipValdiator = new CustomValidator();
                            stateZipValdiator.ErrorMessage = "Invalid state / Zip Code Combination";
                            stateZipValdiator.ClientValidationFunction = "ValidateStateAndZip";
                            stateZipValdiator.Display = ValidatorDisplay.Dynamic;
                            Page.Form.Controls.Add(stateZipValdiator);
                        }

                        if (Subscriber != null)
                        {
                            string val = string.Empty;

                            if (question.ECNFieldName.Equals("ZIP", StringComparison.OrdinalIgnoreCase))
                            {
                                val = Subscriber[question.ECNFieldName.ToUpper()].ToString();
                                txtOptions.Text = (val.IndexOf("-") > -1 ? val.Substring(0, val.IndexOf("-")) : val);
                                CustomValidator stateZipValdiator = new CustomValidator();
                                stateZipValdiator.ErrorMessage = "Invalid state / Zip Code Combination";
                                stateZipValdiator.ClientValidationFunction = "ValidateStateAndZip";
                                stateZipValdiator.Display = ValidatorDisplay.Dynamic;
                                Page.Form.Controls.Add(stateZipValdiator);
                            }
                            else if (question.ECNFieldName.Equals("ZIPPLUS", StringComparison.OrdinalIgnoreCase))
                            {
                                val = Subscriber["ZIP"].ToString();
                                txtOptions.Text = (val.IndexOf("-") > -1 ? val.Substring(val.IndexOf("-") + 1) : val);
                            }
                            else if (question.ECNFieldName.Equals("Voice1") || question.ECNFieldName.Equals("Voice2") || question.ECNFieldName.Equals("Voice3"))
                            {
                                //Code for prepopulation
                                string tempECNFieldName = question.ECNFieldName;
                                question.ECNFieldName = ViewState["Phone_ECNFieldName"].ToString();
                                string phoneNumber = string.Empty;
                                if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                                {
                                    phoneNumber = getQueryString(question.QueryStringName);
                                }
                                else
                                {
                                    phoneNumber = Subscriber[question.ECNFieldName.ToUpper()].ToString();
                                }
                                phoneNumber = phoneNumber.Replace("-", "");
                                phoneNumber = phoneNumber.Replace(" ", "");
                                if (tempECNFieldName.Equals("Voice1"))
                                {
                                    txtOptions.Text = phoneNumber.Substring(0, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Voice2"))
                                {
                                    if (question.MaxLength == 3)
                                        txtOptions.Text = phoneNumber.Substring(3, question.MaxLength);
                                    else if (question.MaxLength == 5)
                                        txtOptions.Text = phoneNumber.Substring(4, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Voice3"))
                                {
                                    if (question.MaxLength == 4)
                                        txtOptions.Text = phoneNumber.Substring(6, question.MaxLength);
                                    else if (question.MaxLength == 11)
                                        txtOptions.Text = phoneNumber.Substring(9, phoneNumber.Length - 9);
                                }
                            }
                            else if (question.ECNFieldName.Equals("Fax1") || question.ECNFieldName.Equals("Fax2") || question.ECNFieldName.Equals("Fax3"))
                            {
                                string tempECNFieldName = question.ECNFieldName;
                                question.ECNFieldName = ViewState["Fax_ECNFieldName"].ToString();
                                string faxNumber = string.Empty;
                                if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                                {
                                    faxNumber = getQueryString(question.QueryStringName);
                                }
                                else
                                {
                                    faxNumber = Subscriber[question.ECNFieldName.ToUpper()].ToString();
                                }
                                faxNumber = faxNumber.Replace("-", "");
                                faxNumber = faxNumber.Replace(" ", "");
                                if (tempECNFieldName.Equals("Fax1"))
                                {
                                    txtOptions.Text = faxNumber.Substring(0, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Fax2"))
                                {
                                    if (question.MaxLength == 3)
                                        txtOptions.Text = faxNumber.Substring(3, question.MaxLength);
                                    else if (question.MaxLength == 5)
                                        txtOptions.Text = faxNumber.Substring(4, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Fax3"))
                                {
                                    if (question.MaxLength == 4)
                                        txtOptions.Text = faxNumber.Substring(6, question.MaxLength);
                                    else if (question.MaxLength == 11)
                                        txtOptions.Text = faxNumber.Substring(9, faxNumber.Length - 9);
                                }
                            }
                            else
                            {
                                if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                                {
                                    txtOptions.Text = getQueryString(question.QueryStringName);
                                }
                                else
                                {
                                    txtOptions.Text = Subscriber[question.ECNFieldName.ToUpper()].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (question.ECNFieldName.Equals("Voice1") || question.ECNFieldName.Equals("Voice2") || question.ECNFieldName.Equals("Voice3"))
                            {
                                //Code for prepopulation
                                string tempECNFieldName = question.ECNFieldName;
                                question.ECNFieldName = ViewState["Phone_ECNFieldName"].ToString();
                                string phoneNumber = string.Empty;
                                if (question.Grouping.Equals(FieldGroup.Profile) && Request[string.Format("question_{0}", question.ECNFieldName)] != null && Request[string.Format("question_{0}", question.ECNFieldName)] != "")
                                {
                                    phoneNumber = getFromForm(string.Format("question_{0}", question.ECNFieldName)).ToString();
                                }
                                else
                                {
                                    if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                                    {
                                        phoneNumber = getQueryString(question.QueryStringName);
                                    }
                                }
                                phoneNumber = phoneNumber.Replace("-", "");
                                phoneNumber = phoneNumber.Replace(" ", "");
                                if (tempECNFieldName.Equals("Voice1"))
                                {
                                    txtOptions.Text = phoneNumber.Substring(0, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Voice2"))
                                {
                                    if (question.MaxLength == 3)
                                        txtOptions.Text = phoneNumber.Substring(3, question.MaxLength);
                                    else if (question.MaxLength == 5)
                                        txtOptions.Text = phoneNumber.Substring(4, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Voice3"))
                                {
                                    if (question.MaxLength == 4)
                                        txtOptions.Text = phoneNumber.Substring(6, question.MaxLength);
                                    else if (question.MaxLength == 11)
                                        txtOptions.Text = phoneNumber.Substring(9, phoneNumber.Length - 9);
                                }
                            }
                            else if (question.ECNFieldName.Equals("Fax1") || question.ECNFieldName.Equals("Fax2") || question.ECNFieldName.Equals("Fax3"))
                            {
                                string tempECNFieldName = question.ECNFieldName;
                                question.ECNFieldName = ViewState["Fax_ECNFieldName"].ToString();
                                string faxNumber = string.Empty;
                                if (question.Grouping.Equals(FieldGroup.Profile) && Request[string.Format("question_{0}", question.ECNFieldName)] != null && Request[string.Format("question_{0}", question.ECNFieldName)] != "")
                                {
                                    faxNumber = getFromForm(string.Format("question_{0}", question.ECNFieldName)).ToString();
                                }
                                else
                                {
                                    if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                                    {
                                        faxNumber = getQueryString(question.QueryStringName);
                                    }
                                }
                                faxNumber = faxNumber.Replace("-", "");
                                faxNumber = faxNumber.Replace(" ", "");
                                if (tempECNFieldName.Equals("Fax1"))
                                {
                                    txtOptions.Text = faxNumber.Substring(0, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Fax2"))
                                {
                                    if (question.MaxLength == 3)
                                        txtOptions.Text = faxNumber.Substring(3, question.MaxLength);
                                    else if (question.MaxLength == 5)
                                        txtOptions.Text = faxNumber.Substring(4, question.MaxLength);
                                }
                                if (tempECNFieldName.Equals("Fax3"))
                                {
                                    if (question.MaxLength == 4)
                                        txtOptions.Text = faxNumber.Substring(6, question.MaxLength);
                                    else if (question.MaxLength == 11)
                                        txtOptions.Text = faxNumber.Substring(9, faxNumber.Length - 9);
                                }
                            }
                            else
                            {
                                if (question.Grouping.Equals(FieldGroup.Profile) && Request[string.Format("question_{0}", question.ECNFieldName)] != null && Request[string.Format("question_{0}", question.ECNFieldName)] != "")
                                {
                                    txtOptions.Text = getFromForm(string.Format("question_{0}", question.ECNFieldName)).ToString();
                                }
                                else
                                {
                                    if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                                    {
                                        txtOptions.Text = getQueryString(question.QueryStringName);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    { }

                    return txtOptions;
                case ControlType.Hidden:
                    HiddenField hf = new HiddenField();
                    hf.ID = string.Format("question_{0}", question.ECNFieldName);

                    try
                    {   // If Querystring value exists popuplate 
                        if (!question.QueryStringName.Equals(string.Empty) && !getQueryString(question.QueryStringName).Equals(string.Empty))
                        {
                            hf.Value = getQueryString(question.QueryStringName);
                        }
                        else
                        {
                            hf.Value = question.DefaultValue;
                        }
                    }
                    catch
                    {
                    }

                    return hf;
                default:
                    return new LiteralControl(question.DataType.ToString() + " " + question.ControlType.ToString());
            }
        }

        private Control SetRequiredValidation(string ctrl, string questionname)
        {
            RequiredFieldValidator rfv = new RequiredFieldValidator();
            rfv.ID = "rfv_" + ctrl;
            rfv.Text = Resources.Resource.ResourceManager.GetObject("requiredfieldImage").ToString();
            rfv.ErrorMessage = questionname;
            rfv.InitialValue = "";

            rfv.ControlToValidate = ctrl;
            return rfv;
        }

        #endregion

        #region linkbutton events

        protected void lnkNewSubscription_Click(object sender, EventArgs e)
        {
            ShowPanel("NEWFORMSTEP1");
            hidTRANSACTIONTYPE.Value = "NEW";
        }

        protected void lnkManageSubscription_Click(object sender, EventArgs e)
        {
            ShowPanel("LOGIN");
            hidTRANSACTIONTYPE.Value = "RENEW";
        }

        protected void lnkCustomerService_Click(object sender, EventArgs e)
        {
            ShowPanel("CUSTOMERSERVICE");
        }

        protected void lnkTradeShow_Click(object sender, EventArgs e)
        {
            ShowPanel("TRADESHOW");
        }

        #endregion

        #region Login Handlers
        protected void btnLoginWithSubscriberID_Click(object sender, EventArgs e)
        {
            SubscriberLogin(txtSubscriberID.Text, pub.LoginVerfication.ToUpper(), txtVerification.Text);
        }

        protected void btnLoginWithUserName_Click(object sender, EventArgs e)
        {
            //if (CheckPaidProductsEmailValidation(txtUserName.Text))
            //{
            //    lblEmailValidationText.Text = pub.RepeatEmailsMessage.Replace("%%EmailAddress%%", txtUserName.Text.Trim());
            //    ModalPopupExtenderEmailValidation.Show();
            //    pnlEmailValidationPopup.Style.Add("display", "block");
            //    return;
            //}

            if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtUserName.Text.Trim()))
            {
                if (!pub.DisablePassword)
                {
                    SubscriberLogin(txtUserName.Text, "U", txtUserPassword.Text);
                }
                else
                {
                    SubscriberLogin(txtUserName.Text, "D", "");
                }
            }
            else
            {
                lblUserNameInvalid.Visible = true;
            }
        }

        private void SubscriberLogin(string loginID, string filter, string filterval)
        {
            string redirectPaymentUrl = string.Empty;
            string redirectNQUrl = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand("sp_subscriberLogin");
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
                cmd.Parameters["@GroupID"].Value = pub.ECNDefaultGroupID;

                cmd.Parameters.Add(new SqlParameter("@subscriberID", SqlDbType.VarChar, 50));
                cmd.Parameters["@subscriberID"].Value = loginID;

                cmd.Parameters.Add(new SqlParameter("@filter", SqlDbType.Char, 1));
                cmd.Parameters["@filter"].Value = filter;

                cmd.Parameters.Add(new SqlParameter("@filterval", SqlDbType.VarChar, 25));
                cmd.Parameters["@filterval"].Value = filterval;

                DataTable dtEmails = DataFunctions.GetDataTable("communicator", cmd);

                if (dtEmails != null && dtEmails.Rows.Count > 0)
                {
                    hidTRANSACTIONTYPE.Value = "RENEW";

                    Subscriber = new Dictionary<string, string>();

                    foreach (DataColumn dc in dtEmails.Columns)
                    {
                        Subscriber.Add(dc.ColumnName.ToUpper(), dtEmails.Rows[0].IsNull(dc.ColumnName) ? string.Empty : dtEmails.Rows[0][dc.ColumnName].ToString());
                    }

                    EmailID = Convert.ToInt32(Subscriber["EMAILID"]);
                    txtemailaddress.Text = Subscriber["EMAILADDRESS"].ToString();

                    try
                    {
                        drpCountry.ClearSelection();
                        drpCountry.Items.FindByText(Subscriber["COUNTRY"].ToUpper().Trim()).Selected = true;

                        pc = pub.GetPubCountry(Subscriber["COUNTRY"]);
                        CountryID = pc.CountryID;
                    }
                    catch
                    {
                        drpCountry.Items[0].Selected = true;
                        pc = pub.GetPubCountry(drpCountry.SelectedItem.Text);
                        CountryID = pc.CountryID;
                    }

                    hidSUBSCRIBERID.Value = Subscriber["SUBSCRIBERID"].ToString();

                    SqlCommand cmdUDF = new SqlCommand(string.Format("select upper(ShortName) as shortname, datavalue from groupdatafields gdf  with (NOLOCK) join emaildatavalues edv with (NOLOCK) on gdf.GroupDatafieldsID = edv.GroupDatafieldsID where groupID = {0} and EmailID = {1} and gdf.IsDeleted=0 ", pub.ECNDefaultGroupID, EmailID));
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    DataTable dtUDF = DataFunctions.GetDataTable("communicator", cmdUDF);

                    foreach (DataRow drUDF in dtUDF.Rows)
                    {
                        if (!Subscriber.ContainsKey(drUDF["SHORTNAME"].ToString()))
                        {
                            Subscriber.Add(drUDF["SHORTNAME"].ToString().ToUpper(), drUDF.IsNull("DataValue") ? string.Empty : drUDF["DataValue"].ToString());
                        }
                    }

                    SubscriberFromCacheorSession = Subscriber;

                    try
                    {
                        if (Subscriber["PASSWORD"].Length > 0)
                        {
                            rfvPassword.Enabled = false;
                            rfvcPassword.Enabled = false;
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        SqlCommand cmdPaymentStatus = new SqlCommand("select datavalue from EmailDataValues  with (NOLOCK) where EmailID = @EmailID and GroupDatafieldsID in (select gdf.GroupDatafieldsID from GroupDatafields gdf  with (NOLOCK) where GroupID = @ECNDefaultGroupID  and IsDeleted=0 and ShortName = 'PaymentStatus')");
                        cmdPaymentStatus.CommandType = CommandType.Text;
                        cmdPaymentStatus.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                        cmdPaymentStatus.Parameters.Add(new SqlParameter("@ECNDefaultGroupID", pub.ECNDefaultGroupID));
                        string PaymentStatus = DataFunctions.ExecuteScalar("communicator", cmdPaymentStatus).ToString();

                        if (PaymentStatus.Equals("paid", StringComparison.OrdinalIgnoreCase) || PaymentStatus.Equals("processing", StringComparison.OrdinalIgnoreCase) || PaymentStatus.Equals("pending", StringComparison.OrdinalIgnoreCase))
                        {
                            if (pc != null)
                            {
                                getPubForm();

                                if (pf != null)
                                {
                                    if (pub.HasPaid && pf.IsPaid && pub.ProcessExternal && !String.IsNullOrEmpty(pub.ProcessExternalURL))
                                    {
                                        Response.Redirect(pub.ProcessExternalURL + "?e=" + Subscriber["EMAILADDRESS"] + "&ei=" + EmailID + "&ctry=" + Subscriber["COUNTRY"] + "&pubcode=" + pub.PubCode + "&tAmount=" + pf.PaidPrice);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    ShowPanel("RENEWFORM");
                }
                else
                {
                    ShowPanel("LOGIN");

                    if (filter.Equals("L"))
                        lblErrorMessage.Text = "Invalid Subscriber ID or first letter of your Last Name!";
                    else if (filter.Equals("C"))
                        lblErrorMessage.Text = "Invalid Subscriber ID or first letter of your Country!";
                    else if (filter.Equals("U"))
                        lblErrorMessage.Text = "Invalid Email Address or Password!";
                    else if (filter.Equals("D"))
                    {

                        if (getQueryString("rURL") != string.Empty)
                        {
                            lblErrorMessage.Text = "Your email address is not recognized with this form. Please <a href='Subscription.aspx?pubcode=" + PubCode + "&rURL=" + getQueryString("rURL") + "&step=step1'>" + "click here </a> to return to the New Signup login area.";
                        }
                        else
                        {
                            lblErrorMessage.Text = "Your email address is not recognized with this form. Please <a href='Subscription.aspx?pubcode=" + PubCode + "&step=step1'>" + "click here </a> to return to the New Signup login area.";
                        }
                    }
                    else
                        lblErrorMessage.Text = "Invalid Subscriber ID or State!";

                    lblErrorMessage.Visible = true;
                    phError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowPanel("LOGIN");
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
                phError.Visible = true;
            }
        }

        #endregion

        protected void btnStep1Submit_Click(object sender, EventArgs e)
        {
            bool EmailExists = false;
            if (!IsValidEmailAddress(txtLoginEmailAddress.Text))
            {
                lblLoginEmailInvalid.Visible = true;
                return;
            }
            if (IsValid)
            {
                SqlCommand cmdEmailExists = new SqlCommand("select count(e.EmailID) from ecn5_communicator..emails e with (NOLOCK) join ecn5_communicator..emailgroups eg with (NOLOCK) on e.emaiLID = eg.emailID where eg.groupID = @ECNDefaultGroupID and CustomerID = @ECNCustomerID and emailaddress = @LoginEmailAddress");
                cmdEmailExists.CommandType = CommandType.Text;
                cmdEmailExists.CommandTimeout = 0;
                cmdEmailExists.Parameters.Add(new SqlParameter("@ECNCustomerID", pub.ECNCustomerID.ToString()));
                cmdEmailExists.Parameters.Add(new SqlParameter("@ECNDefaultGroupID", pub.ECNDefaultGroupID.ToString()));
                cmdEmailExists.Parameters.Add(new SqlParameter("@LoginEmailAddress", txtLoginEmailAddress.Text));
                EmailExists = Convert.ToBoolean((Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", cmdEmailExists).ToString()) > 0));

                if (EmailExists && pub.CheckSubscriber)
                {
                    lblErrorMessage.Text = "Subscriber account already exists. Click <a href='" + Request.Url.AbsoluteUri + "&step=login" + "'>here</a> to manage your account.";
                    lblErrorMessage.Visible = true;
                    phError.Visible = true;
                    return;
                }

                CountryID = Convert.ToInt32(drpNewCountry.SelectedItem.Value);
                getPubForm();

                if (pc.IsNonQualified && !pf.DisableNonQualForDigital && !pf.ShowNQPrintAsDigital)
                {
                    if (getQueryString("rURL") != string.Empty)
                    {
                        Response.Redirect(String.Format("{0}?qn=country&qv={1}&PubCode={2}&PubID={3}&PFID={4}&NQCountry=1&ei={5}&btx_m={6}&btx_i={7}&rURL=" + getQueryString("rURL"), ConfigurationManager.AppSettings["NQLandingPage"].ToString(), drpCountry.SelectedItem.Text, PubCode, pub.PubID, pf.PFID, EmailID, MagazineID, IssueID));

                    }
                    else
                    {
                        Response.Redirect(String.Format("{0}?qn=country&qv={1}&PubCode={2}&PubID={3}&PFID={4}&NQCountry=1&ei={5}&btx_m={6}&btx_i={7}", ConfigurationManager.AppSettings["NQLandingPage"].ToString(), drpCountry.SelectedItem.Text, PubCode, pub.PubID, pf.PFID, EmailID, MagazineID, IssueID));

                    }
                }

                ShowPanel("NEWFORM");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (getQueryString("step") != string.Empty)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                Response.Redirect(Request.Url.AbsoluteUri + "&step=form");
            }
        }

        private bool ValidateFields(FieldGroup fg)
        {
            PlaceHolder ph = (fg == FieldGroup.Profile ? plProfileQuestions : plDemoQuestions);
            List<PubFormField> pffs = pf.Fields.Where(p => p.Grouping == fg).ToList();
            bool isValid = true;

            foreach (PubFormField question in pffs)
            {
                switch (question.ControlType)
                {
                    case ControlType.TextBox:
                        TextBox txtquestion = (TextBox)ph.FindControl("question_" + question.ECNFieldName);
                        if (txtquestion != null && question.Required && txtquestion.Text.Trim().Length == 0)
                        {
                            requiredFieldMessage += question.ECNFieldName + " is required!!";
                            isValid = false;
                        }
                        break;

                    case ControlType.Radio:
                        RadioButtonList rbList = (RadioButtonList)ph.FindControl("question_" + question.ECNFieldName);
                        if (rbList != null && rbList.SelectedIndex == -1 && question.Required)
                        {
                            requiredFieldMessage += question.DisplayName + " is required!!";
                            isValid = false;
                        }
                        break;

                    case ControlType.Dropdown:
                        DropDownList ddlControl = (DropDownList)ph.FindControl("question_" + question.ECNFieldName);
                        if (ddlControl != null && ddlControl.SelectedIndex == -1 && question.Required)
                        {
                            requiredFieldMessage += question.DisplayName + " is required!!";
                            isValid = false;
                        }
                        break;

                    case ControlType.Checkbox:
                        CheckBoxList chkControl = (CheckBoxList)ph.FindControl("question_" + question.ECNFieldName);
                        if (chkControl != null && chkControl.SelectedIndex == -1 && question.Required)
                        {
                            requiredFieldMessage += question.DisplayName + " is required!!";
                            isValid = false;
                        }
                        break;

                    case ControlType.CatCheckbox:
                        CategorizedCheckBoxList catChkControl = (CategorizedCheckBoxList)ph.FindControl("question_" + question.ECNFieldName);
                        if (catChkControl != null && catChkControl.Selections.Count <= 0 && question.Required)
                        {
                            requiredFieldMessage += question.DisplayName + " is required!!";
                            isValid = false;
                        }
                        break;
                }
            }

            phError.Visible = true;
            lblErrorMessage.Text = requiredFieldMessage;
            return isValid;
        }



        protected void txtemailaddress_TextChanged(object sender, EventArgs e)
        {
            if (CheckPaidProductsEmailValidation(txtemailaddress.Text.Trim()))
            {
                lblEmailValidationText.Text = pub.RepeatEmailsMessage.Replace("%%EmailAddress%%", txtemailaddress.Text.Trim());
                ModalPopupExtenderEmailValidation.Show();
                pnlEmailValidationPopup.Style.Add("display", "block");
                txtemailaddress.Text = "";
                return;
            }

        }

        protected void txtLoginEmailAddress_TextChanged(object sender, EventArgs e)
        {
            if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtLoginEmailAddress.Text.Trim()))
            {
                if (CheckPaidProductsEmailValidation(txtLoginEmailAddress.Text.Trim()))
                {
                    lblEmailValidationText.Text = pub.RepeatEmailsMessage.Replace("%%EmailAddress%%", txtLoginEmailAddress.Text.Trim());
                    ModalPopupExtenderEmailValidation.Show();
                    pnlEmailValidationPopup.Style.Add("display", "block");
                    txtLoginEmailAddress.Text = "";
                }
            }
            else
            {
                //rexEmail4.IsValid = false;
                lblLoginEmailInvalid.Visible = true;
                return;
            }
        }

        private static void SaveUDF(int groupID, string shortname, string longname)
        {
            try
            {
                shortname = DataFunctions.CleanString(shortname.Trim());
                longname = DataFunctions.CleanString(longname.Trim());
                string sqlcheck = "SELECT COUNT(groupdatafieldsID) FROM GROUPDATAFIELDS  with (NOLOCK)  WHERE shortname='" + shortname + "'  and IsDeleted=0 AND groupID =" + groupID;
                int alreadyexist = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlcheck));

                if (alreadyexist == 0)
                {
                    string sqlquery = " INSERT INTO GroupDataFields ( GroupID, ShortName, LongName, IsPublic, IsDeleted) VALUES ( " + groupID + ", '" + shortname + "', '" + longname + "','N', 0);select @@IDENTITY ";
                    DataFunctions.ExecuteScalar("communicator", sqlquery).ToString();
                }
            }
            catch { }
        }

        public static bool IsValidEmailAddress(string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.fn_ValidateEmailAddress";
            cmd.Parameters.AddWithValue("@EmailAddr", emailAddress);
            SqlParameter outParam = new SqlParameter("@IsValid", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(outParam);
            DataFunctions.Execute("communicator", cmd);
            return Convert.ToBoolean(cmd.Parameters["@IsValid"].Value);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string PostParams = string.Empty;
                string ProfileParams = string.Empty;
                string DemographicParams = string.Empty;
                string groupParams = string.Empty;
                string PaidParams = string.Empty;
                bool isPrintOverwritten = false;
                string user_demo7 = string.Empty;
                string user_SUBSCRIPTION = string.Empty;

                string externalURLParams = string.Empty;
                string NONQualRedirectURL = string.Empty;
                string NONQualPost = string.Empty;
                bool IsNONQualSubscription = false;
                bool EmailExists = false;

                bool IsNewsLetterSelected = false;
                string msgBody = string.Empty;

                //if (Page.IsValid)
                //{

                string EncodedResponse = Request.Form["g-Recaptcha-Response"];

                string reValidate = @"Page_ValidationSummariesReset();";
                if (ConfigurationManager.AppSettings["ValidateCaptcha"].ToString().ToLower().Equals("true"))
                {
                    if (!ReCaptchaClass.Validate(EncodedResponse))
                    {
                        lblErrorMessage.Text = "Invalid Captcha";
                        lblErrorMessage.Visible = true;
                        phError.Visible = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reValidateOthers", reValidate, true);
                        return;
                    }
                }

                if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtemailaddress.Text.Trim()))
                {

                    lblInvalidEmail.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reValidateOthers", reValidate, true);
                    return;
                }

                #region CheckEmailExists
                SqlCommand cmdEmailExists = new SqlCommand("select count(e.EmailID) from ecn5_communicator..emails e with (NOLOCK) join ecn5_communicator..emailgroups eg with (NOLOCK) on e.emaiLID = eg.emailID where e.emailID <> @EmailID and eg.groupID = @ECNDefaultGroupID and CustomerID = @ECNCustomerID and emailaddress = @EmailAddress");
                cmdEmailExists.CommandType = CommandType.Text;
                cmdEmailExists.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int)).Value = EmailID;
                cmdEmailExists.Parameters.Add(new SqlParameter("@ECNDefaultGroupID", SqlDbType.Int)).Value = pub.ECNDefaultGroupID;
                cmdEmailExists.Parameters.Add(new SqlParameter("@EmailAddress", SqlDbType.VarChar)).Value = txtemailaddress.Text;
                cmdEmailExists.Parameters.Add(new SqlParameter("@ECNCustomerID", SqlDbType.Int)).Value = pub.ECNCustomerID;
                EmailExists = Convert.ToBoolean((Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", cmdEmailExists).ToString()) > 0));

                if (!EmailExists)
                {
                    if (EmailID > 0)
                    {
                        SqlCommand cmdUpdateEmail = new SqlCommand("update emails set emailaddress = @EmailAddress, DateUpdated=getdate()  where emailID = @EmailID");
                        cmdUpdateEmail.CommandType = CommandType.Text;
                        cmdUpdateEmail.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int)).Value = EmailID;
                        cmdUpdateEmail.Parameters.Add(new SqlParameter("@EmailAddress", SqlDbType.VarChar)).Value = txtemailaddress.Text;
                        DataFunctions.Execute("communicator", cmdUpdateEmail);
                    }
                }
                else if (pub.CheckSubscriber && hidTRANSACTIONTYPE.Value.ToUpper() != "RENEW")
                {
                    lblErrorMessage.Text = "Subscriber account already exists. Click <a href='" + Request.Url.AbsoluteUri + "&step=login" + "'>here</a> to manage your account.";
                    lblErrorMessage.Visible = true;
                    phError.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reValidateOthers", reValidate, true);
                    return;
                }
                #endregion

                if (pub.HasPaid && pf.IsPaid)
                {
                    #region CC Processing
                    // Set this to run credit cards when needed.
                    if (pnlPayPalQuestions.Visible && !pub.ProcessExternal)
                    {
                        NvpDoDirectPayment ppPay = new Encore.PayPal.Nvp.NvpDoDirectPayment();
                        ppPay.Environment = NvpEnvironment.Live;

                        ppPay.Credentials.Username = pub.PayflowAccount;
                        ppPay.Credentials.Password = pub.PayflowPassword;
                        ppPay.Credentials.Signature = pub.PayflowSignature;

                        // Add the required parameters
                        ppPay.Add(NvpDoDirectPayment.Request._IPADDRESS, Request.UserHostAddress);
                        ppPay.Add(NvpDoDirectPayment.Request._AMT, PaypalPrice.Text);
                        ppPay.Add(NvpDoDirectPayment.Request._PAYMENTACTION, NvpPaymentActionCodeType.Sale);

                        if (PaypalCardType.SelectedItem.Value == "MasterCard")
                            ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.MasterCard);
                        else if (PaypalCardType.Text == "Visa")
                            ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Visa);
                        else if (PaypalCardType.Text == "Amex")
                            ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Amex);

                        ppPay.Add(NvpDoDirectPayment.Request._ACCT, PaypalAcct.Text);
                        ppPay.Add(NvpDoDirectPayment.Request._EXPDATE, PaypalExpMonth.Text + PaypalExpYear.Text);

                        //ppPay.Add(NvpDoDirectPayment.Request.CVV2, "123");
                        ppPay.Add(NvpDoDirectPayment.Request._FIRSTNAME, PaypalFirstName.Text);
                        ppPay.Add(NvpDoDirectPayment.Request._LASTNAME, PaypalLastName.Text);
                        ppPay.Add(NvpDoDirectPayment.Request.STREET, PaypalStreet.Text);
                        ppPay.Add(NvpDoDirectPayment.Request.STREET2, PaypalStreet2.Text);
                        ppPay.Add(NvpDoDirectPayment.Request.CITY, PaypalCity.Text);
                        ppPay.Add(NvpDoDirectPayment.Request.STATE, PaypalState.Text);
                        ppPay.Add(NvpDoDirectPayment.Request.ZIP, PaypalZip.Text);
                        ppPay.Add(NvpDoDirectPayment.Request.COUNTRYCODE, PaypalCountry.SelectedItem.Value);

                        if (ppPay.Post())
                        {
                            // Register success in ECN with Transactional UDF's
                            // Build a string to pass in the Paypal UDF data into the group.
                            PaidParams += "&user_t_FirstName=" + Server.UrlEncode(PaypalFirstName.Text) + "&user_t_LastName=" + Server.UrlEncode(PaypalLastName.Text) + "&user_t_Street=" + Server.UrlEncode(PaypalStreet.Text) + "&user_t_Street2=" + Server.UrlEncode(PaypalStreet2.Text) + "&user_t_City=" + Server.UrlEncode(PaypalCity.Text) + "&user_t_State=" + Server.UrlEncode(PaypalState.Text) + "&user_t_Zip=" + Server.UrlEncode(PaypalZip.Text) + "&user_t_Country=" + Server.UrlEncode(PaypalCountry.SelectedItem.Value) + "&user_t_CardType=" + Server.UrlEncode(PaypalCardType.SelectedItem.Value) + "&user_t_CardNumber=" + Server.UrlEncode(PaypalAcct.Text.Substring(PaypalAcct.Text.Length - 4)) + "&user_t_ExpirationDate=" + Server.UrlEncode(PaypalExpMonth.SelectedItem.Value + "/" + PaypalExpYear.SelectedItem.Value) + "&user_t_transdate=" + Server.UrlEncode(DateTime.Now.ToString("MM/dd/yyyy")) + "&user_t_AmountPaid=" + Server.UrlEncode(ppPay.Get(NvpDoDirectPayment.Response.AMT.ToString())) + "&user_t_TransactionID=" + Server.UrlEncode(ppPay.Get(NvpDoDirectPayment.Response.TRANSACTIONID) + "&user_PaymentStatus=paid");
                        }
                        else
                        {
                            //ppPay.ErrorList[0].Code + " / " + ppPay.ErrorList[0].Severity + " / " + ppPay.ErrorList[0].ShortMessage + 
                            lblErrorMessage.Text = "ERROR :" + ppPay.ErrorList[0].Code + " / " + ppPay.ErrorList[0].Severity + " / " + ppPay.ErrorList[0].ShortMessage + " / " + ppPay.ErrorList[0].LongMessage;
                            lblErrorMessage.Visible = true;
                            phError.Visible = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reValidateOthers", reValidate, true);
                            return;
                        }
                    }
                    else if (pub.ProcessExternal)
                    {
                        PaidParams += "&tAmount=" + PaypalPrice.Text + "&user_PaymentStatus=pending";
                    }
                    #endregion
                }

                LoadECNFieldsHashtable();
                getFormResponses();

                try
                {
                    if (EmailID > 0 && txtPassword.Text.Trim().Length > 0)
                    {
                        SqlCommand cmdUpdateEmail = new SqlCommand("UPDATE emails set password = @Password, DateUpdated=getdate()  where emailID = @EmailID");
                        cmdUpdateEmail.CommandType = CommandType.Text;
                        cmdUpdateEmail.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar)).Value = txtPassword.Text;
                        cmdUpdateEmail.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int)).Value = EmailID;
                        DataFunctions.Execute("communicator", cmdUpdateEmail);
                    }
                }
                catch
                { }

                PostParams = "?user_publicationcode=" + Server.UrlEncode(PubCode) + "&f=html&s=S" + "&e=" + Server.UrlEncode(txtemailaddress.Text);

                if (txtPassword.Text.Trim().Length > 0)
                    PostParams += "&password=" + Server.UrlEncode(txtPassword.Text);

                groupParams += "&g=" + Server.UrlEncode(pub.ECNDefaultGroupID.ToString()) + "&sfID=" + Server.UrlEncode(pub.ECNSFID.ToString());

                try
                {
                    if (pf.EnablePrintAndDigital && chk_user_Demo7_Both.Checked)
                    {
                        user_demo7 = "C";
                    }
                    else if (chk_user_Demo7_Print.Checked && pf.ShowPrintAsDigital)
                    {
                        user_demo7 = "B";
                        isPrintOverwritten = true;
                    }
                    else if (chk_user_Demo7_Print.Checked)
                        user_demo7 = "A";
                    else if (chk_user_Demo7_Digital.Checked)
                        user_demo7 = "B";
                }
                catch
                { }

                try
                {
                    if (pnlSubscription.Visible)
                    {
                        user_SUBSCRIPTION = rbuser_SUBSCRIPTION.SelectedItem.Value;

                        if (user_SUBSCRIPTION == "Y" && !chk_user_Demo7_Print.Checked && !chk_user_Demo7_Digital.Checked && !chk_user_Demo7_Both.Checked)
                        {
                            lblErrorMessage.Text = "You must answer the question to <br> How would you like to receive your copy?";
                            lblErrorMessage.Visible = true;
                            phError.Visible = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reValidateOthers", reValidate, true);
                            return;
                        }
                    }
                    else
                        user_SUBSCRIPTION = "Y"; //auto subscribe if print & digital are disabled.   
                }
                catch
                { }

                PostParams += "&user_DEMO7=" + Server.UrlEncode(user_demo7) + "&user_Demo5=y&user_SubSrc=NNWEB";

                if (!string.IsNullOrEmpty(hidSUBSCRIBERID.Value))
                    PostParams += "&user_SUBSCRIBERID=" + hidSUBSCRIBERID.Value;

                PostParams += "&user_SUBSCRIPTION=" + user_SUBSCRIPTION;
                PostParams += "&ctry=" + Server.UrlEncode(drpCountry.SelectedItem.Text);

                //PAIDorFREE Update -Check & Create PAIDorFREE UDF
                SaveUDF(pub.ECNDefaultGroupID, "PAIDorFREE", "PAIDorFREE");
                PostParams += "&user_PAIDorFREE=FREE";

                //Third Party Posting
                #region ThirdPartyPosting
                extPostParams();
                #endregion

                /*CHECK ANY NON QUAL RULES EXISTS AND IF EXISTS - VALIDATE THE RULES*/

                #region CheckNonQualification

                if (pf.IsNonQualSetup)
                {
                    NONQualRedirectURL = ConfigurationManager.AppSettings["NQLandingPage"].ToString();

                    if (user_demo7 == "A" && pf.NQPrintRedirectUrl.Trim().Length > 0)
                    {
                        NONQualRedirectURL = pf.NQPrintRedirectUrl.Trim();
                    }
                    else if (user_demo7 == "B" && pf.NQDigitalRedirectUrl.Trim().Length > 0)
                    {
                        NONQualRedirectURL = pf.NQDigitalRedirectUrl.Trim();
                    }
                    else if (user_demo7 == "C" && pf.NQBothRedirectUrl.Trim().Length > 0)
                    {
                        NONQualRedirectURL = pf.NQBothRedirectUrl.Trim();
                    }

                    bool CheckNonqQualCondition = true;

                    if (chk_user_Demo7_Digital.Checked && pf.DisableNonQualForDigital)
                        CheckNonqQualCondition = false;

                    if (CheckNonqQualCondition)
                    {
                        #region NONQual redirect /* Redirection to Non qualification page on the basis of country selection */

                        if (pc.IsNonQualified)
                        {
                            IsNonQualCountry = true;
                            IsNONQualSubscription = true;

                            if (!(chk_user_Demo7_Print.Checked && pf.ShowNQPrintAsDigital))
                            {
                                if (getQueryString("rURL") != string.Empty)
                                {
                                    NONQualRedirectURL += "&rURL=" + getQueryString("rURL");
                                }
                                if (!NONQualRedirectURL.Contains("?"))
                                    Response.Redirect(String.Format("{0}?PubCode={1}&PubID={2}&PFID={3}&NQCountry=1&qn=Country&qv={4}&ei={5}&btx_m={6}&btx_i={7}", NONQualRedirectURL, PubCode, pub.PubID, pf.PFID, drpCountry.SelectedItem.Text, EmailID, MagazineID, IssueID));
                                else
                                    Response.Redirect(String.Format("{0}&PubCode={1}&PubID={2}&PFID={3}&NQCountry=1&qn=Country&qv={4}&ei={5}&btx_m={6}&btx_i={7}", NONQualRedirectURL, PubCode, pub.PubID, pf.PFID, drpCountry.SelectedItem.Text, EmailID, MagazineID, IssueID));
                            }
                        }

                        #endregion

                        /* Redirection to Non qualification page on the basis of Non qualified Response */

                        #region NON QUAL Questions

                        //DataTable dtNQ = DataFunctions.GetDataTable("select  psf.DisplayName,  psf.ECNFieldName, Controltype, psfd.DataValue, psfd.DataText, psf.Grouping from	NonQualSettings nq join PubFormFields pff on nq.PFFieldID = pff.PFFieldID join PubSubscriptionFields psf on psf.PSFieldID = pff.PSFieldID join PubSubscriptionFieldData psfd on psfd.PSFieldID = psf.PSFieldID and psfd.DataValue = nq.DataValue where  PFF.PFID = " + PubFormID + " order by ecnfieldname, psfd.DataValue");

                        //fields.DisplayName,  fields.ECNFieldName, fields.Controltype, fields.Grouping, fd.DataValue, fd.DataText
                        var NQFields = pf.Fields.Where(f => f.NonQualExists == true).ToList();

                        var x = (from n in NQFields
                                 from nd in n.Data
                                 where nd.IsNonQual == true
                                 select new
                                 {
                                     n.DisplayName,
                                     n.ECNFieldName,
                                     n.ControlType,
                                     n.Grouping,
                                     nd.DataValue,
                                     nd.DataText
                                 }).ToList();


                        foreach (var drNQ in x)
                        {
                            try
                            {
                                foreach (string v in SubscriberResponse[drNQ.ECNFieldName.ToUpper()].Split(new char[] { ',' }))
                                {
                                    if (drNQ.DataValue.Equals(v, StringComparison.OrdinalIgnoreCase))
                                    {
                                        PostParams += "&NQCountry=0&qv=" + Server.UrlEncode(drNQ.DataText) + "&qn=" + Server.UrlEncode(System.Text.RegularExpressions.Regex.Replace(drNQ.DisplayName, "<[^>]*>", string.Empty));
                                        IsNONQualSubscription = true;
                                        break;
                                    }
                                }

                                if (IsNONQualSubscription)
                                    break;
                            }
                            catch
                            { }
                        }
                        #endregion
                    }
                }

                #endregion

                if (plProfileQuestions.Visible)
                {
                    ProfileParams += getPostParams(FieldGroup.Profile);
                    ProfileParams += "&user_TRANSACTIONTYPE=" + Server.UrlEncode(hidTRANSACTIONTYPE.Value);

                    if (plDemoQuestions.Visible) // print or Digital subscription selected.
                        DemographicParams += getPostParams(FieldGroup.Demographic);

                    // Do not POST to DEFAULT GROUP if the print or digital is not selected.
                    externalURLParams = PostParams + "&c=" + pub.ECNCustomerID.ToString() + groupParams + PaidParams + ProfileParams + DemographicParams;

                    if (externalURLParams.IndexOf("&e=") > -1)
                    {
                        int indexOfEmail = externalURLParams.IndexOf("&e=") + 3;
                        int indexOfEnd = externalURLParams.IndexOf("&", indexOfEmail);
                        int leng = indexOfEnd - indexOfEmail;
                        string email = externalURLParams.Substring(indexOfEmail, leng);
                        string newEmail = Server.UrlEncode(email);

                        externalURLParams.Replace(email, newEmail);
                    }

                    if (!IsNONQualSubscription)
                    {
                        #region Post to ECN

                        CheckRequiredandNotifybyEmail();

                        bool PostforDigitalSubscriber = false;
                        bool PostForPrintSubscriber = false;
                        string NXTBookPassword = string.Empty; //store in User1 in email table.

                        if (user_demo7.Equals("B", StringComparison.OrdinalIgnoreCase) || user_demo7.Equals("C", StringComparison.OrdinalIgnoreCase))
                        {
                            PostforDigitalSubscriber = NXTBook.IsPostforDigitalSubscriber(PubCode);

                            if (PostforDigitalSubscriber)
                            {
                                //Check if Password in User1 - if not, create it.
                                try
                                {
                                    NXTBookPassword = DataFunctions.ExecuteScalar("communicator", string.Format("select user1 from emails with (NOLOCK) where CustomerID = {0} and emailaddress = '{1}'", pub.ECNCustomerID, txtemailaddress.Text)).ToString();
                                }
                                catch
                                {
                                }

                                if (string.IsNullOrEmpty(NXTBookPassword))
                                {
                                    NXTBookPassword = KMPS_JF_Objects.Objects.Utilities.CreatePassword(16);
                                }

                                ProfileParams += "&usr1=" + NXTBookPassword;

                            }
                        }
                        else if (user_demo7.Equals("A", StringComparison.OrdinalIgnoreCase))
                        {
                            PostForPrintSubscriber = NXTBook.IsPostforPrintSubscriber(PubCode);
                            if (PostForPrintSubscriber)
                            {
                                //Check if Password in User1 - if not, create it.
                                try
                                {
                                    NXTBookPassword = DataFunctions.ExecuteScalar("communicator", string.Format("select user1 from emails with (NOLOCK) where CustomerID = {0} and emailaddress = '{1}'", pub.ECNCustomerID, txtemailaddress.Text)).ToString();
                                }
                                catch
                                {
                                }

                                if (string.IsNullOrEmpty(NXTBookPassword))
                                {
                                    NXTBookPassword = KMPS_JF_Objects.Objects.Utilities.CreatePassword(16);
                                }

                                ProfileParams += "&usr1=" + NXTBookPassword;
                            }
                        }


                        HttpPost(PostParams + "&c=" + pub.ECNCustomerID.ToString() + "&ei=" + (EmailID > 0 && hidTRANSACTIONTYPE.Value != "NEW" ? EmailID.ToString() : "") + groupParams + PaidParams + ProfileParams + DemographicParams, false);

                        //Post Data to NXTBook

                        if ((PostforDigitalSubscriber || PostForPrintSubscriber) && PubCode != string.Empty)
                        {
                            #region NXTBook Post
                            //Check for NXTBook Integration & Call NXTBookAPI.
                            try
                            {

                                NXTBook nxtbook = null;
                                if (PostforDigitalSubscriber)
                                    nxtbook = NXTBook.GetbyProductCodeforDigital(PubCode);
                                else if (PostForPrintSubscriber)
                                {
                                    nxtbook = NXTBook.GetbyProductCodeforPrint(PubCode);
                                }

                                if (nxtbook != null)
                                {
                                    drmProfile dp = new NXTBookAPI.Entity.drmProfile();

                                    dp.subscriptionid = nxtbook.SubscriptionID;

                                    dp.update = true;
                                    dp.noupdate = false;
                                    dp.email = txtemailaddress.Text;
                                    dp.password = NXTBookPassword;
                                    //dp.changepassword = "";

                                    try
                                    {
                                        dp.firstname = SubscriberResponse["FIRSTNAME"].ToString();
                                    }
                                    catch { }

                                    try
                                    {
                                        dp.lastname = SubscriberResponse["LASTNAME"].ToString();
                                    }
                                    catch { }

                                    try
                                    {
                                        dp.phone = SubscriberResponse["VOICE"].ToString();
                                    }
                                    catch { }

                                    try
                                    {
                                        dp.address1 = SubscriberResponse["ADDRESS"].ToString();
                                    }
                                    catch { }
                                    try
                                    {
                                        dp.address2 = SubscriberResponse["ADDRESS2"].ToString();
                                    }
                                    catch { }
                                    try
                                    {
                                        dp.city = SubscriberResponse["CITY"].ToString();
                                    }
                                    catch { }
                                    try
                                    {
                                        dp.country = drpCountry.SelectedItem.Text.ToUpper();
                                    }
                                    catch { }
                                    try
                                    {
                                        dp.state = SubscriberResponse["STATE"].ToString();
                                    }
                                    catch { }

                                    try
                                    {
                                        dp.zipcode = SubscriberResponse["ZIP"].ToString();
                                    }
                                    catch { }

                                    dp.access_nbissues = "";
                                    dp.access_firstissue = "";
                                    //dp.access_limitdate = "";

                                    DateTime issueDate = NXTBook.GetRecentIssueDatebyPubcode(PubCode);

                                    if (nxtbook.IsUnlimited.HasValue && nxtbook.IsUnlimited.Value)
                                    {
                                        dp.access_type = "unlimited";

                                    }
                                    else
                                    {

                                        dp.access_type = "timerestricted";
                                        dp.access_startdate = NXTBook.GetRecentIssueDatebyPubcode(PubCode).ToString("yyyy-MM-dd");
                                        dp.access_enddate = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                                    }
                                    drmRESTAPI.SetProfile(nxtbook.APIKey, dp);
                                }

                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    //send notification email if NXTbook API fails.
                                    string emailMsg = "Error send data to NXTBOOK from Free form <br /><br />";
                                    emailMsg += " Check TrackHTTPPost table for data.";

                                    emailMsg += "emailaddress :" + txtemailaddress.Text;
                                    emailMsg += " Group ID:" + pub.ECNDefaultGroupID + "Cust ID:" + pub.ECNCustomerID;
                                    emailMsg += "<b>Error Request:</b>" + BuildRequestErrorString((HttpWebRequest)ex.Data["Request"]) + "<br /><br />";
                                    if (ex.Data.Contains("ResponseData"))
                                        emailMsg += "<b>Response Data:</b>" + ex.Data["ResponseData"].ToString() + "<br /><br />";
                                    emailMsg += "<b>Exception Details:</b>" + ex.Message;

                                    ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
                                    emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "NXTBook post Free Form -FAIL", emailMsg);

                                    throw ex;
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                        }

                        #endregion
                    }
                    else
                    {
                        NONQualPost = PostParams + "&c=" + pub.ECNCustomerID.ToString() + "&ei=" + (EmailID > 0 && hidTRANSACTIONTYPE.Value != "NEW" ? EmailID.ToString() : "") + groupParams + PaidParams + ProfileParams + DemographicParams;
                        NONQualPost += "&btx_m=" + MagazineID.ToString() + "&btx_i=" + IssueID.ToString();
                        if (NONQualRedirectURL.Contains("?"))
                        {
                            PostParams = PostParams.Substring(1, PostParams.Length - 1);
                            PostParams = "&" + PostParams;
                        }
                        NONQualRedirectURL += PostParams + "&c=" + pub.ECNCustomerID.ToString() + groupParams + PaidParams + ProfileParams + DemographicParams;
                    }
                }

                #region NewsLetterSubscription
                if (pnlNewslettersA.Visible || pnlNewslettersB.Visible)
                {
                    string sqlquery = string.Empty;

                    foreach (RepeaterItem ritem in ((Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition)).Items)
                    {
                        GridView gvNewsletters = (GridView)ritem.FindControl("gvNewsletters");

                        foreach (GridViewRow r in gvNewsletters.Rows)
                        {
                            int GroupID = Convert.ToInt32(gvNewsletters.DataKeys[Convert.ToInt32(r.RowIndex)].Value);

                            //select all group and customer id related to this groupid

                            HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)r.FindControl("chkSelect");
                            Label lblCustomerID = (Label)r.FindControl("lblCustomerID");
                            Label lblNewsLetterID = (Label)r.FindControl("lblNewsLetterID");

                            if (chkSelect.Checked)
                            {
                                IsNewsLetterSelected = true;
                                groupParams = "&sfID=&g=" + GroupID.ToString() + "&c=" + lblCustomerID.Text + "&s=S";
                                HttpPost(PostParams + groupParams + ProfileParams + DemographicParams, true);
                                //sqlquery += "update emailgroups set subscribetypecode='S', LastChanged=getdate() where emailID in (select top 1 emailID from emails  with (NOLOCK) where customerID =" + lblCustomerID.Text + " and emailaddress = '" + txtemailaddress.Text.ToString().Replace("'", "''") + "') and groupID = " + GroupID.ToString() + ";";

                                //Auto subscription to newsletters
                                DataTable dtAutoNL = AutoSubscribeNL(Convert.ToInt32(lblNewsLetterID.Text));
                                foreach (DataRow dr in dtAutoNL.Rows)
                                {
                                    string grpID = dr["PubNLAutoGroupID"].ToString();
                                    string custID = dr["PubNLAutoCustID"].ToString();
                                    groupParams = "&sfID=&g=" + grpID + "&c=" + custID + "&s=S";
                                    HttpPost(PostParams + groupParams + ProfileParams + DemographicParams, true);
                                    //sqlquery += "update emailgroups set subscribetypecode='S', LastChanged=getdate() where emailID in (select top 1 emailID from emails  with (NOLOCK) where customerID =" + lblCustomerID.Text + " and emailaddress = '" + txtemailaddress.Text.ToString().Replace("'", "''") + "') and groupID = " + grpID.ToString() + ";";
                                }
                            }
                            else if (EmailID > 0)
                            {
                                sqlquery += "update emailgroups set subscribetypecode='U', LastChanged=getdate() where emailID in (select top 1 emailID from emails  with (NOLOCK) where customerID =" + lblCustomerID.Text + " and emailaddress = '" + txtemailaddress.Text.ToString().Replace("'", "''") + "') and groupID = " + GroupID.ToString() + ";";
                            }
                        }
                    }

                    if (sqlquery != string.Empty)
                        DataFunctions.Execute("communicator", sqlquery);
                }
                #endregion

                #region Paypal Redirect
                if (plProfileQuestions.Visible)
                {
                    if (IsNONQualSubscription && !pub.ProcessExternal && pub.PaymentGateway == KMPS_JF_Objects.Objects.Enums.CCProcessors.PaypalRedirect)
                    {
                        #region paypal Redirect

                        if (pf.ShowNQPrintAsDigital || chk_user_Demo7_Both.Checked)
                        {
                            /* Replace print with digital when ShowNQPrintAsDigital is true and then subscribe to digital group and send NQ Response email */

                            NONQualPost = NONQualPost.Replace("user_DEMO7=A", "user_DEMO7=B");
                            NONQualPost = NONQualPost.Replace("user_DEMO7=C", "user_DEMO7=B");
                        }

                        groupParams = "&g=" + Server.UrlEncode(pub.ECNDefaultGroupID.ToString()) + "&sfID=" + Server.UrlEncode(pub.ECNSFID.ToString());

                        HttpPost(PostParams.Replace("user_PAIDorFREE=FREE", "user_PAIDorFREE=NONQUALIFIED") + "&user_PaymentStatus=pending&c=" + pub.ECNCustomerID.ToString() + "&ei=" + (EmailID > 0 && hidTRANSACTIONTYPE.Value != "NEW" ? EmailID.ToString() : "") + groupParams + PaidParams + ProfileParams + DemographicParams, false);

                        //NvpConfig.Settings.Username = pub.PayflowAccount;
                        //NvpConfig.Settings.Password = pub.PayflowPassword;
                        //NvpConfig.Settings.Signature = pub.PayflowSignature;
                        //if(ConfigurationManager.AppSettings["NVPEnvironment"].ToString().Equals("sandbox"))
                        //{
                        //    NvpConfig.Settings.Environment = NvpEnvironment.Sandbox;
                        //}
                        //else
                        //{
                        //    NvpConfig.Settings.Environment = NvpEnvironment.Live;
                        //}

                        NvpSetExpressCheckout ppSet = new NvpSetExpressCheckout();
                        ppSet.Credentials.Username = pub.PayflowAccount;
                        ppSet.Credentials.Password = pub.PayflowPassword;
                        ppSet.Credentials.Signature = pub.PayflowSignature;
                        Dictionary<string, string> profileDict = new Dictionary<string, string>();
                        string[] profileArray = ProfileParams.Split('&');

                        foreach (string s in profileArray)
                        {
                            if (s.Length > 0)
                            {
                                string name = s.Substring(0, s.IndexOf("="));
                                string value = s.Substring(s.IndexOf("=") + 1);
                                profileDict.Add(name, System.Web.HttpUtility.UrlDecode(value));
                            }
                        }

                        string fullName = profileDict.ContainsKey("fn") ? profileDict["fn"].ToString() : "";
                        fullName += profileDict.ContainsKey("ln") ? " " + profileDict["ln"].ToString() : "";
                        string fullAddress = profileDict.ContainsKey("adr") ? profileDict["adr"].ToString() : "";
                        string address2 = profileDict.ContainsKey("adr2") ? " " + profileDict["adr2"].ToString() : "";

                        string phone = string.Empty;
                        try
                        {
                            phone = getFromForm(string.Format("question_{0}1", "Voice")) +
                                  getFromForm(string.Format("question_{0}2", "Voice")) +
                                  getFromForm(string.Format("question_{0}3", "Voice"));
                        }
                        catch { }

                        ppSet.Add(NvpSetExpressCheckout.Request._AMT, decimal.Parse(pf.PaidPrice.ToString()).ToString("f"));
                        ppSet.Add(NvpSetExpressCheckout.Request.LANDINGPAGE, NvpLandingPageType.Billing);
                        ppSet.Add(NvpSetExpressCheckout.Request.PAYMENTACTION, NvpPaymentActionCodeType.Sale);
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTONAME, fullName);
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOCITY, profileDict.ContainsKey("city") ? profileDict["city"].ToString() : "");
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOZIP, profileDict.ContainsKey("zc") ? profileDict["zc"].ToString() : "");
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET, fullAddress.Replace("+", " "));
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET2, address2);
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOSTATE, profileDict.ContainsKey("state") ? profileDict["state"].ToString() : "");
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOCOUNTRYCODE, pc.CountryCode);
                        ppSet.Add(NvpSetExpressCheckout.Request.EMAIL, txtemailaddress.Text);
                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOPHONENUM, phone);
                        if (!string.IsNullOrEmpty(pub.PayflowPageStyle))
                        {
                            ppSet.Add(NvpSetExpressCheckout.Request.PAGESTYLE, pub.PayflowPageStyle);
                        }
                        List<NvpPayItem> itemList = new List<NvpPayItem>();
                        NvpPayItem item = new NvpPayItem();
                        item.Name = pub.PubCode + " Subscription";
                        //item.Description = "item.Description";
                        //item.Quantity = "1";
                        item.Amount = decimal.Parse(pf.PaidPrice.ToString()).ToString("f");
                        //item.Tax = decimal.Parse("0.00").ToString("f");
                        itemList.Add(item);

                        ppSet.Add(itemList);

                        ppSet.Add(NvpSetExpressCheckout.Request.SOLUTIONTYPE, NvpSolutionTypeType.Sole);

                        string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;

                        ppSet.Add(NvpSetExpressCheckout.Request._RETURNURL, basePath + "/Forms/Payment.aspx?pubid=" + pub.PubID.ToString() + "&pubcode=" + PubCode + "&PFID=" + pf.PFID + "&btx_m=" + MagazineID.ToString() + "&btx_i=" + IssueID.ToString() + "&ei=" + EmailID.ToString() + "&rURL=" + getQueryString("rURL") + "&emailaddress=" + txtemailaddress.Text + "&demo7=" + (isPrintOverwritten ? "A" : user_demo7) + "&" + PostParams.ToString() + ProfileParams.ToString() + groupParams);

                        ppSet.Add(NvpSetExpressCheckout.Request._CANCELURL, basePath + "/Forms/Cancel.aspx?pubid=" + pub.PubID.ToString() + "&pubcode=" + Request.QueryString.Get("pubcode") + "&e=" + txtemailaddress.Text + "&pwd=" + txtPassword.Text);

                        if (ppSet.Post())
                        {
                            //HttpPost(PostParams.Replace("user_PAIDorFREE=NONQUALIFIED", "user_PAIDorFREE=PENDING") + "&c=" + pub.ECNCustomerID.ToString() + "&ei=" + (EmailID > 0 && hidTRANSACTIONTYPE.Value != "NEW" ? EmailID.ToString() : "") + groupParams + PaidParams + ProfileParams + DemographicParams, false);
                            Response.Redirect(ppSet.RedirectUrl + "&force_sa=true");
                        }
                        else
                        {
                            //ppSet post failed
                            lblErrorMessage.Text = "Payment redirect failed.";
                            //foreach(NvpError p in ppSet.ErrorList)
                            //{
                            //    lblErrorMessage.Text += p.LongMessage;
                            //}
                            lblErrorMessage.Visible = true;
                            phError.Visible = true;
                            return;
                        }
                        #endregion
                    }
                }
                #endregion

                Dictionary<NotificatonFor, PubResponseEmail> dResponseEmail = PubResponseEmail.GetByPFID(pf.PFID);
                PubResponseEmail responseemail = null;

                if (dResponseEmail != null)
                {
                    if ((user_demo7.ToUpper() == "A" && !isPrintOverwritten) && dResponseEmail.ContainsKey(NotificatonFor.Print) && (dResponseEmail[NotificatonFor.Print].SendUserEmail || dResponseEmail[NotificatonFor.Print].SendAdminEmail))
                        responseemail = dResponseEmail[NotificatonFor.Print];
                    else if ((user_demo7.ToUpper() == "B" || (user_demo7.ToUpper() == "A" && isPrintOverwritten)) && dResponseEmail.ContainsKey(NotificatonFor.Digital) && (dResponseEmail[NotificatonFor.Digital].SendUserEmail || dResponseEmail[NotificatonFor.Digital].SendAdminEmail))
                        responseemail = dResponseEmail[NotificatonFor.Digital];
                    else if (user_demo7.ToUpper() == "C" && dResponseEmail.ContainsKey(NotificatonFor.Both) && (dResponseEmail[NotificatonFor.Both].SendUserEmail || dResponseEmail[NotificatonFor.Both].SendAdminEmail))
                        responseemail = dResponseEmail[NotificatonFor.Both];
                    else if (user_SUBSCRIPTION.ToUpper() == "N" && user_demo7.ToUpper() == string.Empty && dResponseEmail.ContainsKey(NotificatonFor.Cancel) && (dResponseEmail[NotificatonFor.Cancel].SendUserEmail || dResponseEmail[NotificatonFor.Cancel].SendAdminEmail))
                        responseemail = dResponseEmail[NotificatonFor.Cancel];
                    else if (user_demo7.ToUpper() == string.Empty && dResponseEmail.ContainsKey(NotificatonFor.Other) && (dResponseEmail[NotificatonFor.Other].SendUserEmail || dResponseEmail[NotificatonFor.Other].SendAdminEmail))
                        responseemail = dResponseEmail[NotificatonFor.Other];
                }

                if (IsNONQualSubscription)
                {
                    #region Redirect to NQ

                    if ((chk_user_Demo7_Print.Checked && pf.ShowNQPrintAsDigital) || (chk_user_Demo7_Both.Checked && !pf.SuspendECNPostforBoth))
                    {
                        /* Replace print with digital when ShowNQPrintAsDigital is true and then subscribe to digital group and send NQ Response email */

                        NONQualPost = NONQualPost.Replace("user_DEMO7=A", "user_DEMO7=B");
                        NONQualPost = NONQualPost.Replace("user_DEMO7=C", "user_DEMO7=B");
                        HttpPost(NONQualPost, false);
                        SendNQResponseEmail('A');

                        /* Send Digital Response Emails */
                        try
                        {
                            if (dResponseEmail.ContainsKey(NotificatonFor.Digital))
                                responseemail = dResponseEmail[NotificatonFor.Digital];
                        }
                        catch
                        {
                            responseemail = null;
                        }

                        /* Send digital newsletter and admin email */

                        if (responseemail != null && responseemail.SendUserEmail)
                        {
                            msgBody = ReplaceCodeSnippets(responseemail.Response_UserMsgBody);
                            SendNotification(responseemail.Response_FromEmail, responseemail.Response_FromName, txtemailaddress.Text, responseemail.Response_UserMsgSubject, msgBody);
                        }

                        if (responseemail != null && responseemail.SendAdminEmail)
                        {
                            msgBody = ReplaceCodeSnippets(responseemail.Response_AdminMsgBody);
                            SendNotification(responseemail.Response_FromEmail, responseemail.Response_FromName, responseemail.Response_AdminEmail, responseemail.Response_AdminMsgSubject, msgBody);
                        }

                        if (dResponseEmail.ContainsKey(NotificatonFor.Newsletter))
                        {
                            try
                            {
                                responseemail = dResponseEmail[NotificatonFor.Newsletter];
                            }
                            catch
                            {
                                responseemail = null;
                            }

                            if (IsNewsLetterSelected && responseemail != null && responseemail.SendUserEmail)
                            {
                                msgBody = ReplaceCodeSnippets(responseemail.Response_UserMsgBody);
                                SendNotification(responseemail.Response_FromEmail, responseemail.Response_FromName, txtemailaddress.Text, responseemail.Response_UserMsgSubject, msgBody);
                            }
                        }
                    }

                    if (hidTRANSACTIONTYPE.Value.Equals("NEW"))
                    {
                        try
                        {
                            EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "select e.EmailID from Emails e  with (NOLOCK) join emailgroups eg  with (NOLOCK) on e.emailID = eg.emailID where CustomerID = " + pub.ECNCustomerID + " and eg.groupID = " + pub.ECNDefaultGroupID + " and e.emailaddress = '" + txtemailaddress.Text.Replace("'", "''") + "'"));
                        }
                        catch { }
                    }

                    if (Request.QueryString != null && Request.QueryString.ToString().Trim().Length > 0)
                    {
                        string redirectURL = NONQualRedirectURL + "&PFID=" + pf.PFID + "&btx_m=" + MagazineID.ToString() + "&btx_i=" + IssueID.ToString() + "&ei=" + EmailID.ToString();
                        string finalURL = rebuildURL(redirectURL, Request.QueryString.ToString());
                        Response.Redirect(finalURL);
                    }
                    else
                    {
                        if (getQueryString("rURL") != string.Empty)
                        {
                            Response.Redirect(NONQualRedirectURL + "&pubcode=" + PubCode + "&PFID=" + pf.PFID + "&btx_m=" + MagazineID.ToString() + "&btx_i=" + IssueID.ToString() + "&ei=" + EmailID.ToString() + "&rURL=" + getQueryString("rURL"));

                        }
                        else
                        {
                            Response.Redirect(NONQualRedirectURL + "&pubcode=" + PubCode + "&PFID=" + pf.PFID + "&btx_m=" + MagazineID.ToString() + "&btx_i=" + IssueID.ToString() + "&ei=" + EmailID.ToString());

                        }
                    }

                    #endregion
                }
                else
                {
                    if (dResponseEmail != null)
                    {
                        if (responseemail != null)
                        {
                            if (responseemail.SendUserEmail)
                            {
                                msgBody = ReplaceCodeSnippets(responseemail.Response_UserMsgBody);
                                SendNotification(responseemail.Response_FromEmail, responseemail.Response_FromName, txtemailaddress.Text, responseemail.Response_UserMsgSubject, msgBody);
                            }

                            if (responseemail.SendAdminEmail)
                            {
                                msgBody = ReplaceCodeSnippets(responseemail.Response_AdminMsgBody);
                                SendNotification(responseemail.Response_FromEmail, responseemail.Response_FromName, responseemail.Response_AdminEmail, responseemail.Response_AdminMsgSubject, msgBody);
                            }
                        }


                        if (dResponseEmail.ContainsKey(NotificatonFor.Newsletter))
                        {
                            responseemail = dResponseEmail[NotificatonFor.Newsletter];
                            if (IsNewsLetterSelected && responseemail != null && responseemail.SendUserEmail)
                            {
                                msgBody = ReplaceCodeSnippets(responseemail.Response_UserMsgBody);
                                SendNotification(responseemail.Response_FromEmail, responseemail.Response_FromName, txtemailaddress.Text, responseemail.Response_UserMsgSubject, msgBody);
                            }
                        }
                    }
                    #region Auto Subscriptions

                    Dictionary<int, int> AutoGroups = new Dictionary<int, int>();
                    AutoGroups = Publication.GetAutoSubscriptions(pub.PubID);

                    //auto subscribe to each group
                    foreach (KeyValuePair<int, int> kv in AutoGroups)
                    {
                        groupParams = "&sfID=&g=" + kv.Key.ToString() + "&c=" + kv.Value.ToString() + "&s=S";
                        HttpPost(PostParams + groupParams + ProfileParams + DemographicParams, true);
                    }

                    #endregion

                    string rRUL = Request.QueryString["rURL"];
                    if (rRUL != null)
                    {
                        if (!rRUL.Contains("http"))
                            rRUL = "http://" + rRUL;

                        if (rRUL.Contains("http%3a%2f%2f"))
                            rRUL = rRUL.Replace("http%3a%2f%2f", "http://");
                    }

                    if (pub.HasPaid && pf.IsPaid && pub.ProcessExternal && !pub.ProcessExternalURL.Equals(string.Empty))
                    {
                        Response.Redirect(pub.ProcessExternalURL + externalURLParams + "&ei=" + EmailID + "&pubcode=" + PubCode);
                    }
                    else if (pub.ThankYouPageLink.Length > 0 && (pub.ThankYouPageLink.ToUpper().StartsWith("HTTP:") || pub.ThankYouPageLink.ToUpper().StartsWith("HTTPS:")))
                    {
                        if (Request.QueryString["rURL"] != null)
                        {
                            Response.Redirect(rRUL + "?pubcode=" + PubCode + "&emailaddress=" + txtemailaddress.Text.Replace("+","%2B"));
                        }
                        else
                        {
                            RedirecToThankYouLink();
                        }
                    }
                    else
                    {
                        if (hidTRANSACTIONTYPE.Value.Equals("NEW"))
                        {
                            try
                            {
                                EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "select e.EmailID from Emails e  with (NOLOCK) join emailgroups eg  with (NOLOCK) on e.emailID = eg.emailID where CustomerID = " + pub.ECNCustomerID + " and eg.groupID = " + pub.ECNDefaultGroupID + " and e.emailaddress = '" + txtemailaddress.Text.Replace("'", "''") + "'"));
                            }
                            catch { }
                        }

                        if (Request.QueryString["rURL"] != null)
                        {
                            Response.Redirect(rRUL + "?pubcode=" + PubCode + "&emailaddress=" + txtemailaddress.Text.Replace("+", "%2B"));
                        }
                        else
                        {
                            string TKParams = PostParams.ToString() + ProfileParams.ToString() + groupParams;
                            if (TKParams.IndexOf("&e=") > -1)
                            {
                                int indexOfEmail = TKParams.IndexOf("&e=") + 3;
                                int indexOfEnd = TKParams.IndexOf("&", indexOfEmail);
                                int leng = indexOfEnd - indexOfEmail;
                                string email = TKParams.Substring(indexOfEmail, leng);
                                string newEmail = Server.UrlEncode(email);

                                TKParams.Replace(email, newEmail);
                            }
                            if (TKParams.StartsWith("?"))
                                TKParams = TKParams.Substring(1);

                            if (TKParams.StartsWith("&"))
                                TKParams = TKParams.Substring(1);

                            string sThankYoulink = "ThankYou.aspx?pubcode=" + PubCode + "&PFID=" + PubFormID + "&ei=" + EmailID.ToString() + "&btx_m=" + MagazineID.ToString() + "&btx_i=" + IssueID.ToString() + "&demo7=" + (isPrintOverwritten ? "A" : user_demo7) + "&" + TKParams;
                            Response.Redirect(sThankYoulink, true);
                        }
                    }
                }
                //}
                //else
                //{


                //    string msg = "";
                //    // Loop through all validation controls to see which
                //    // generated the errors.
                //    foreach (IValidator aValidator in this.Validators)
                //    {
                //        if (!aValidator.IsValid)
                //        {
                //            msg += "<br />" + aValidator.ErrorMessage;
                //        }
                //    }
                //    lblErrorMessage.Text = msg;
                //    lblErrorMessage.Visible = true;
                //    phError.Visible = true;
                //    return;
                //}
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "ERROR : " + ex.Message;
                lblErrorMessage.Visible = true;
                phError.Visible = true;
            }

        }

        private string BuildRequestErrorString(HttpWebRequest request)
        {
            StringBuilder sbRequest = new StringBuilder();
            sbRequest.Append("Address:" + request.Address + "<br />");
            sbRequest.Append("Headers:");
            for (int i = 0; i < request.Headers.Count; ++i)
            {
                string header = request.Headers.GetKey(i);
                foreach (string s in request.Headers.GetValues(i))
                {
                    sbRequest.Append(string.Format("{0}: {1}", header, s) + "<br />");
                }
            }




            return sbRequest.ToString();
        }

        private void getFormResponses()
        {
            SubscriberResponse = new Dictionary<string, string>();

            foreach (PubFormField question in pf.Fields)
            {
                if (question.ControlType == ControlType.Checkbox)
                {
                    PlaceHolder ph = (question.Grouping == FieldGroup.Profile ? plProfileQuestions : plDemoQuestions);
                    CheckBoxList cb = (CheckBoxList)ph.FindControl(string.Format("question_{0}", question.ECNFieldName));
                    if (cb != null)
                    {
                        addSubscriberResponse(SubscriberResponse, question, getCheckboxValues(cb));
                    }
                }
                else if (question.ECNFieldName.ToLower().Equals("voice") || question.ECNFieldName.ToLower().Equals("fax"))
                {
                    string value = getFromForm(string.Format("question_{0}1", question.ECNFieldName)) +
                                  getFromForm(string.Format("question_{0}2", question.ECNFieldName)) +
                                  getFromForm(string.Format("question_{0}3", question.ECNFieldName));
                    addSubscriberResponse(SubscriberResponse, question, value);
                }
                else
                {
                    addSubscriberResponse(SubscriberResponse, question, getFromForm(string.Format("question_{0}", question.ECNFieldName)));
                }
            }
        }

        private void addSubscriberResponse(Dictionary<string, string> SubscriberResponse, PubFormField question, string value)
        {
            if (!SubscriberResponse.ContainsKey(question.ECNFieldName.ToUpper()))
            {
                SubscriberResponse.Add(question.ECNFieldName.ToUpper(), value);
            }

            //Check if question has ECNCombinedFieldName
            if (question.ECNCombinedFieldName.Length > 0 && !value.Equals(string.Empty))
            {
                if (SubscriberResponse.ContainsKey(question.ECNCombinedFieldName.ToUpper()))
                {
                    string val = SubscriberResponse[question.ECNCombinedFieldName.ToUpper()];
                    SubscriberResponse[question.ECNCombinedFieldName.ToUpper()] = (val + "," + value).TrimEnd(',');
                    SubscriberResponse[question.ECNCombinedFieldName.ToUpper()] = (val + "," + value).TrimStart(',');
                }
                else
                {
                    SubscriberResponse.Add(question.ECNCombinedFieldName.ToUpper(), value);
                }
            }

            //Check if question has ShowTextField enabled
            if (question.ShowTextField)
            {
                SubscriberResponse.Add(question.ECNTextFieldName.ToUpper(), getFromForm(string.Format("question_{0}", question.ECNTextFieldName)));
            }

        }

        private string rebuildURL(string primaryString, string secondaryString)
        {
            UriBuilder tempUri = new UriBuilder(primaryString);
            string queryString_PrimaryString = tempUri.Query;
            Regex r = new Regex("&");
            Array qsValues = r.Split(secondaryString);
            for (int i = 0; i < qsValues.Length; i++)
            {
                if (qsValues.GetValue(i).ToString().Contains("="))
                {
                    string keyToSearch = qsValues.GetValue(i).ToString().Split('=')[0];
                    string valueinSecondary = qsValues.GetValue(i).ToString().Split('=')[1];
                    string valueinPrimary = HttpUtility.ParseQueryString(queryString_PrimaryString).Get(keyToSearch);
                    if (valueinPrimary == null)
                    {
                        tempUri.Query = tempUri.Query.Substring(1) + "&" + keyToSearch + "=" + valueinSecondary;
                    }
                    else if (valueinPrimary.Equals(""))
                    {
                        tempUri.Query = Regex.Replace(tempUri.Query, "&" + keyToSearch + "=", "&" + keyToSearch + "=" + valueinSecondary, RegexOptions.IgnoreCase);
                    }
                }
            }
            return tempUri.ToString();
        }

        private bool CheckRequiredandNotifybyEmail()
        {
            try
            {
                bool IsMissingRequiredField = false;
                bool IsVisible = false;

                DataTable dtFieldSettings = getFormFieldSettings();

                StringBuilder htmlbody = new StringBuilder();

                List<PubFormField> pffs = pf.Fields.Where(p => p.Grouping == FieldGroup.Profile).ToList();

                htmlbody.Append("<table cellpadding='3' cellspacing='3' border='1' width='1040px'>");

                string user_SUBSCRIPTION = string.Empty;

                if (rbuser_SUBSCRIPTION.SelectedItem != null)
                {
                    user_SUBSCRIPTION = rbuser_SUBSCRIPTION.SelectedItem.Value;
                }

                string user_demo7 = string.Empty;
                List<DataRow> lFieldSettings = null;
                DataRow drFieldSetting = null;

                try
                {
                    if (pf.EnablePrintAndDigital && chk_user_Demo7_Both.Checked)
                    {
                        user_demo7 = "C";
                    }
                    else if (chk_user_Demo7_Print.Checked && pf.ShowPrintAsDigital)
                    {
                        user_demo7 = "B";
                    }
                    else if (chk_user_Demo7_Print.Checked)
                        user_demo7 = "A";
                    else if (chk_user_Demo7_Digital.Checked)
                        user_demo7 = "B";
                }
                catch
                { }

                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);


                htmlbody.Append("<TR><TD colspan='5'><h1>Pubcode " + PubCode + "</h1></TD></TR>");
                htmlbody.Append("<TR><TD colspan='5'><h4><b>Browser Info : </b> " + browserInfo + "</h1></TD></TR>");
                htmlbody.Append("<TR><TD>Email Address</TD><TD colspan='4'>" + txtemailaddress.Text + "</TD></TR>");

                htmlbody.Append("<TR><TD>user_SUBSCRIPTION</TD><TD colspan='4'>" + user_SUBSCRIPTION + (user_SUBSCRIPTION == "" ? " ( MISSING ) " : "") + "</TD></TR>");
                htmlbody.Append("<TR><TD>user_demo7</TD><TD colspan='4'>" + user_demo7 + "</TD></TR>");

                htmlbody.Append("<TR><TD colspan='4'><h2>Profile questions</h2></TD><TD> Visible = " + plProfileQuestions.Visible.ToString() + "</TD></TR>");

                if (plProfileQuestions.Visible)
                {
                    htmlbody.Append("<TR><TD>Field Name</TD><TD>QS Name</TD><TD>Value</TD><TD>Required</TD><TD>Visible</TD></TR>");
                    foreach (PubFormField question in pffs)
                    {
                        IsVisible = false;

                        if (dtFieldSettings != null)
                        {
                            lFieldSettings = dtFieldSettings.AsEnumerable().ToList();

                            drFieldSetting = lFieldSettings.Find(x => x["ECNFieldName"].ToString().Equals(question.ECNFieldName, StringComparison.OrdinalIgnoreCase));

                            if (drFieldSetting != null)
                            {
                                #region Branch logic
                                switch (drFieldSetting["ControlType"].ToString().ToLower().Trim())
                                {
                                    case "checkbox":
                                        CheckBoxList cb;

                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            cb = (CheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            cb = (CheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            try
                                            {
                                                if (cb.Items.FindByValue(fv).Selected)
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                        break;
                                    case "catcheckbox":
                                        CategorizedCheckBoxList ccb;

                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            ccb = (CategorizedCheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            ccb = (CategorizedCheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            string selectedvalues = string.Empty;
                                            foreach (string chked in ccb.Selections)
                                            {
                                                try
                                                {
                                                    if (chked.ToString().ToLower() == fv.ToLower())
                                                        IsVisible = true;
                                                }
                                                catch { }
                                            }
                                        }
                                        break;
                                    case "catradio":
                                        CategorizedRadioButtonList crbl;

                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            crbl = (CategorizedRadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            crbl = (CategorizedRadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            string selectedvalues = string.Empty;
                                            foreach (string chked in drFieldSetting["DataValue"].ToString().Split(','))
                                            {
                                                try
                                                {
                                                    if (crbl.SelectedValue.ToLower() == chked.ToLower())
                                                        IsVisible = true;
                                                }
                                                catch { }
                                            }
                                        }
                                        break;
                                    case "radio":
                                        RadioButtonList rb;
                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            rb = (RadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            rb = (RadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            try
                                            {
                                                if (rb.Items.FindByValue(fv).Selected)
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                        break;
                                    case "dropdown":
                                        DropDownList ddl;
                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            ddl = (DropDownList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            ddl = (DropDownList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            try
                                            {
                                                if (ddl.Items.FindByValue(fv).Selected)
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                        break;
                                }
                                #endregion
                            }
                            else
                            {
                                IsVisible = true;
                            }
                        }
                        else
                        {
                            IsVisible = true;
                        }

                        if (IsVisible && question.Required && SubscriberResponse[question.ECNFieldName.ToUpper()] == string.Empty)
                        {
                            IsMissingRequiredField = true;
                            htmlbody.Append(String.Format("<TR style='background-color:red;'><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD><TD>{4}</TD></TR>", question.ECNFieldName, getQSParametername(question.ECNFieldName), SubscriberResponse[question.ECNFieldName.ToUpper()], question.Required.ToString(), IsVisible.ToString()));
                            htmlbody.Append(String.Format("<TR style='background-color:red;'><TD colspan='5'>Question : {0}</TD></TR>", question.DisplayName));
                        }
                        else
                            htmlbody.Append(String.Format("<TR><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD><TD>{4}</TD></TR>", question.ECNFieldName, getQSParametername(question.ECNFieldName), SubscriberResponse[question.ECNFieldName.ToUpper()], question.Required.ToString(), IsVisible.ToString()));

                        if (question.ShowTextField)
                            htmlbody.Append(String.Format("<TR><TD>{0}</TD><TD>{1}</TD><TD colspan='3'> other TEXT Field</TD></TR>", getQSParametername(question.ECNTextFieldName), SubscriberResponse[question.ECNTextFieldName.ToUpper()]));

                    }
                }
                htmlbody.Append("<TR><TD colspan='4'><h2>Demo questions</h2></TD><TD> Visible = " + plDemoQuestions.Visible.ToString() + "</TD></TR>");

                if (plDemoQuestions.Visible)
                {
                    pffs = pf.Fields.Where(p => p.Grouping == FieldGroup.Demographic).ToList();

                    foreach (PubFormField question in pffs)
                    {
                        IsVisible = false;

                        if (dtFieldSettings != null)
                        {
                            lFieldSettings = dtFieldSettings.AsEnumerable().ToList();

                            drFieldSetting = lFieldSettings.Find(x => x["ECNFieldName"].ToString().Equals(question.ECNFieldName, StringComparison.OrdinalIgnoreCase));

                            if (drFieldSetting != null)
                            {
                                #region Branch logic
                                switch (drFieldSetting["ControlType"].ToString().ToLower().Trim())
                                {
                                    case "checkbox":
                                        CheckBoxList cb;

                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            cb = (CheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            cb = (CheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            try
                                            {
                                                if (cb.Items.FindByValue(fv).Selected)
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                        break;
                                    case "catcheckbox":
                                        CategorizedCheckBoxList ccb;

                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            ccb = (CategorizedCheckBoxList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            ccb = (CategorizedCheckBoxList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            string selectedvalues = string.Empty;
                                            foreach (string chked in ccb.Selections)
                                            {
                                                try
                                                {
                                                    if (chked.ToString().ToLower() == fv.ToLower())
                                                        IsVisible = true;
                                                }
                                                catch { }
                                            }
                                        }
                                        break;
                                    case "catradio":
                                        CategorizedRadioButtonList crbl;

                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            crbl = (CategorizedRadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            crbl = (CategorizedRadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            string selectedvalues = string.Empty;
                                            foreach (string chked in drFieldSetting["DataValue"].ToString().Split(','))
                                            {
                                                try
                                                {
                                                    if (crbl.SelectedValue.ToLower() == chked.ToLower())
                                                        IsVisible = true;
                                                }
                                                catch { }
                                            }
                                        }
                                        break;
                                    case "radio":
                                        RadioButtonList rb;
                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            rb = (RadioButtonList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            rb = (RadioButtonList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            try
                                            {
                                                if (rb.Items.FindByValue(fv).Selected)
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                        break;
                                    case "dropdown":
                                        DropDownList ddl;
                                        if (drFieldSetting["Grouping"].ToString().ToUpper() == "D")
                                            ddl = (DropDownList)plDemoQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());
                                        else
                                            ddl = (DropDownList)plProfileQuestions.FindControl("question_" + drFieldSetting["DependentQuestionName"].ToString().ToUpper());

                                        foreach (string fv in drFieldSetting["DataValue"].ToString().Split(','))
                                        {
                                            try
                                            {
                                                if (ddl.Items.FindByValue(fv).Selected)
                                                    IsVisible = true;
                                            }
                                            catch { }
                                        }
                                        break;
                                }
                                #endregion
                            }
                            else
                            {
                                IsVisible = true;
                            }
                        }
                        else
                        {
                            IsVisible = true;
                        }

                        if (IsVisible && question.Required && SubscriberResponse[question.ECNFieldName.ToUpper()] == string.Empty)
                        {
                            IsMissingRequiredField = true;
                            htmlbody.Append(String.Format("<TR style='background-color:red;'><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD><TD>{4}</TD></TR>", question.ECNFieldName, getQSParametername(question.ECNFieldName), SubscriberResponse[question.ECNFieldName.ToUpper()], question.Required.ToString(), IsVisible.ToString()));
                            htmlbody.Append(String.Format("<TR style='background-color:red;'><TD colspan='5'>Question : {0}</TD></TR>", question.DisplayName));
                        }
                        else
                            htmlbody.Append(String.Format("<TR><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD><TD>{4}</TD></TR>", question.ECNFieldName, getQSParametername(question.ECNFieldName), SubscriberResponse[question.ECNFieldName.ToUpper()], question.Required.ToString(), IsVisible.ToString()));

                        if (question.ShowTextField)
                            htmlbody.Append(String.Format("<TR><TD>{0}</TD><TD>{1}</TD><TD colspan='3'> other TEXT Field</TD></TR>", getQSParametername(question.ECNTextFieldName), SubscriberResponse[question.ECNTextFieldName.ToUpper()]));

                    }
                }
                htmlbody.Append("</table>");

                if (IsMissingRequiredField)
                    SendNotification(ConfigurationManager.AppSettings["JFsFromEmail"].ToString(), ConfigurationManager.AppSettings["JFsFromName"].ToString(), ConfigurationManager.AppSettings["MissingFieldNotificationTo"].ToString(), PubCode + " Missing Required Fields", htmlbody.ToString());

                return IsMissingRequiredField;
            }
            catch
            {
                return false;
            }
        }

        private string getPostParams(FieldGroup fg)
        {
            List<PubFormField> pffs = pf.Fields.Where(p => p.Grouping == fg).ToList();

            StringBuilder postparams = new StringBuilder();
            Dictionary<string, string> dictCombinedField = new Dictionary<string, string>();

            foreach (PubFormField question in pffs)
            {
                postparams.Append("&" + getQSParametername(question.ECNFieldName) + "=" + Server.UrlEncode(SubscriberResponse[question.ECNFieldName.ToUpper()]));

                if (!dictCombinedField.ContainsKey(question.ECNCombinedFieldName.ToUpper()) && SubscriberResponse.ContainsKey(question.ECNCombinedFieldName.ToUpper()))
                    dictCombinedField.Add(question.ECNCombinedFieldName.ToUpper(), SubscriberResponse[question.ECNCombinedFieldName.ToUpper()]);

                if (question.ECNFieldName.ToUpper() == "ZIP")
                {
                    if (SubscriberResponse.ContainsKey("ZIPPLUS") && SubscriberResponse["ZIPPLUS"].ToString().Trim().Length > 0)
                        postparams.Append("-" + SubscriberResponse["ZIPPLUS"]);
                }

                if (question.ShowTextField)
                    postparams.Append("&" + getQSParametername(question.ECNTextFieldName) + "=" + Server.UrlEncode(SubscriberResponse[question.ECNTextFieldName.ToUpper()]));
            }

            foreach (KeyValuePair<string, string> kv in dictCombinedField)
                postparams.Append("&" + getQSParametername(kv.Key) + "=" + Server.UrlEncode(kv.Value));

            return postparams.ToString();
        }

        private DataTable getHttpPostParams(int entityID, bool isNewsLetter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from HttpPost hp  with (NOLOCK) inner join HttpPostParams hpp  with (NOLOCK) on hp.HttpPostID=hpp.HttpPostID where hp.EntityID=@entityID and hp.IsNewsLetter=@isNewsLetter";
            cmd.Parameters.AddWithValue("@entityID", entityID);
            cmd.Parameters.AddWithValue("@isNewsLetter", isNewsLetter);
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }

        private void extPostParams()
        {
            try
            {
                DataTable dt = new DataTable();
                List<PubFormField> pffs = pf.Fields;
                StringBuilder postparams = new StringBuilder();
                string postURL = string.Empty;
                #region NewsLetterExtPost
                if (pnlNewslettersA.Visible || pnlNewslettersB.Visible)
                {
                    foreach (RepeaterItem ritem in ((Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition)).Items)
                    {
                        GridView gvNewsletters = (GridView)ritem.FindControl("gvNewsletters");
                        foreach (GridViewRow r in gvNewsletters.Rows)
                        {
                            HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)r.FindControl("chkSelect");
                            if (chkSelect.Checked)
                            {
                                int newsLetterID = Convert.ToInt32(((Label)r.FindControl("lblNewsLetterID")).Text);
                                dt = getHttpPostParams(newsLetterID, true);
                                buildParamsAndPost(dt, pffs);
                            }
                        }
                    }
                }
                #endregion

                #region PubExtPost
                dt = new DataTable();
                dt = getHttpPostParams(PubID, false);
                buildParamsAndPost(dt, pffs);
                #endregion
            }
            catch
            {
            }
        }

        private void buildParamsAndPost(DataTable dt, List<PubFormField> pffs)
        {
            string postURL = string.Empty;
            StringBuilder postparams = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                postURL = dt.Rows[0]["URL"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr["ParamValue"].ToString().Equals("CustomValue"))
                    {
                        bool questionInForm = false;
                        foreach (PubFormField question in pffs)
                        {
                            if (question.ECNFieldName.ToUpper().Equals(dr["ParamValue"].ToString().ToUpper()))
                            {
                                questionInForm = true;
                                if (SubscriberResponse.ContainsKey(dr["ParamValue"].ToString().ToUpper()))
                                {
                                    string response = SubscriberResponse[question.ECNFieldName.ToUpper()];
                                    if (question.Grouping.Equals(FieldGroup.Demographic))
                                    {
                                        if (response.Contains(','))
                                        {
                                            string[] CSV = response.Split(',');
                                            foreach (string value in CSV)
                                            {
                                                postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(value));
                                            }
                                        }
                                        else
                                        {
                                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(response));
                                        }
                                    }
                                    else
                                    {
                                        postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(response));
                                    }
                                }
                                break;
                            }
                        }
                        if (dr["ParamValue"].ToString().Equals("EmailAddress"))
                        {
                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(txtemailaddress.Text));
                            questionInForm = true;
                        }
                        if (dr["ParamValue"].ToString().Equals("Password"))
                        {
                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(txtPassword.Text));
                            questionInForm = true;
                        }
                        if (dr["ParamValue"].ToString().Equals("Country"))
                        {
                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(drpCountry.SelectedItem.Text));
                            questionInForm = true;
                        }
                        if (dr["ParamValue"].ToString().Equals("FullName") && SubscriberResponse.ContainsKey("FULLNAME"))
                        {
                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(SubscriberResponse["FULLNAME"]));
                            questionInForm = true;
                        }
                        if (!questionInForm)
                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + getEmailDataValue(txtemailaddress.Text, dr["ParamName"].ToString()));
                    }
                    else
                    {
                        postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(dr["CustomValue"].ToString()));
                    }
                }
                extHttpPost(postURL + "?" + postparams.ToString().TrimStart('&'));
            }
        }

        private string getEmailDataValue(string emailAddress, string shortName)
        {
            SqlCommand cmdHttpPost = new SqlCommand("e_EmailDataValue_Select_EmailAddress_GroupID");
            cmdHttpPost.CommandType = CommandType.StoredProcedure;
            cmdHttpPost.Parameters.AddWithValue("@groupID", pub.ECNDefaultGroupID);
            cmdHttpPost.Parameters.AddWithValue("@shortName", shortName);
            cmdHttpPost.Parameters.AddWithValue("@emailAddress", emailAddress);
            DataTable dt = DataFunctions.GetDataTable("communicator", cmdHttpPost);
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return string.Empty;
        }

        private void SendNQResponseEmail(char subsType)
        {
            Dictionary<NotificatonFor, PubResponseEmail> dResponseEmail = PubResponseEmail.GetByPFID(pf.PFID);
            PubResponseEmail re = null;
            if (dResponseEmail != null)
            {

                if (subsType == 'A')
                {
                    if (dResponseEmail.ContainsKey(NotificatonFor.Print))
                        re = dResponseEmail[NotificatonFor.Print];
                }
                else if (subsType == 'B')
                {
                    if (dResponseEmail.ContainsKey(NotificatonFor.Digital))
                        re = dResponseEmail[NotificatonFor.Digital];
                }

                if (re != null)
                {
                    if (IsNonQualCountry)
                    {
                        if (dResponseEmail.ContainsKey(NotificatonFor.NQ))
                        {
                            re = dResponseEmail[NotificatonFor.NQ];

                            if (re.SendUserEmail)
                                SendNotification(re.Response_FromEmail, re.Response_FromName, txtemailaddress.Text, re.Response_UserMsgSubject, re.Response_UserMsgBody);
                        }
                    }
                    else if (re.SendNQRespEmail)
                    {
                        SendNotification(re.Response_FromEmail, re.Response_FromName, txtemailaddress.Text, re.Response_UserMsgNQRespSub, re.Response_UserMsgNQRespMsgBody);
                    }
                }
            }
        }

        public void SendNotification(string FromEmail, string FromName, string ToEmail, string Subject, string Body)
        {
            try
            {
                if (FromEmail.Trim().Length > 0 && ToEmail.Trim().Length > 0 && Body.Trim().Length > 0)
                {
                    System.Net.Mail.MailMessage simpleMail = new System.Net.Mail.MailMessage();
                    simpleMail.From = new System.Net.Mail.MailAddress(FromEmail, FromName);
                    simpleMail.To.Add(ToEmail);
                    simpleMail.Subject = Subject;
                    simpleMail.Body = ReplaceCodeSnippets(Body);
                    simpleMail.IsBodyHtml = true;
                    simpleMail.Priority = System.Net.Mail.MailPriority.Normal;

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
                    smtpclient.Send(simpleMail);
                }
            }
            catch
            { }
        }

        private void RedirecToThankYouLink()
        {
            string link = pub.ThankYouPageLink;
            try
            {
                Regex r = new Regex("&");
                StringBuilder sb = new StringBuilder();
                Array codesnippets = r.Split(link.ToLower());

                link = link.Replace("%%e%%", Server.UrlEncode(txtemailaddress.Text));
                link = link.Replace("%%ctry%%", Server.UrlEncode(drpCountry.SelectedItem.Text));
                link = link.Replace("%%fn%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FIRSTNAME") ? SubscriberResponse["FIRSTNAME"] : string.Empty));
                link = link.Replace("%%ln%%", Server.UrlEncode(SubscriberResponse.ContainsKey("LASTNAME") ? SubscriberResponse["LASTNAME"] : string.Empty));
                link = link.Replace("%%n%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FULLNAME") ? SubscriberResponse["FULLNAME"] : string.Empty));
                link = link.Replace("%%compname%%", Server.UrlEncode(SubscriberResponse.ContainsKey("COMPANY") ? SubscriberResponse["COMPANY"] : string.Empty));
                link = link.Replace("%%t%%", Server.UrlEncode(SubscriberResponse.ContainsKey("TITLE") ? SubscriberResponse["TITLE"] : string.Empty));
                link = link.Replace("%%occ%%", Server.UrlEncode(SubscriberResponse.ContainsKey("OCCUPATION") ? SubscriberResponse["OCCUPATION"] : string.Empty));
                link = link.Replace("%%adr%%", Server.UrlEncode(SubscriberResponse.ContainsKey("ADDRESS") ? SubscriberResponse["ADDRESS"] : string.Empty));
                link = link.Replace("%%adr2%%", Server.UrlEncode(SubscriberResponse.ContainsKey("ADDRESS2") ? SubscriberResponse["ADDRESS2"] : string.Empty));
                link = link.Replace("%%city%%", Server.UrlEncode(SubscriberResponse.ContainsKey("CITY") ? SubscriberResponse["CITY"] : string.Empty));
                link = link.Replace("%%state%%", Server.UrlEncode(SubscriberResponse.ContainsKey("STATE") ? SubscriberResponse["STATE"] : string.Empty));
                link = link.Replace("%%stateint%%", Server.UrlEncode(SubscriberResponse.ContainsKey("STATE_INT") ? SubscriberResponse["STATE_INT"] : string.Empty));
                link = link.Replace("%%zc%%", Server.UrlEncode(SubscriberResponse.ContainsKey("ZIP") ? SubscriberResponse["ZIP"] : string.Empty));
                link = link.Replace("%%zcfor%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FORZIP") ? SubscriberResponse["FORZIP"] : string.Empty));
                link = link.Replace("%%ph%%", Server.UrlEncode(SubscriberResponse.ContainsKey("VOICE") ? SubscriberResponse["VOICE"] : string.Empty));
                link = link.Replace("%%mph%%", Server.UrlEncode(SubscriberResponse.ContainsKey("MOBILE") ? SubscriberResponse["MOBILE"] : string.Empty));
                link = link.Replace("%%fax%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FAX") ? SubscriberResponse["FAX"] : string.Empty));
                link = link.Replace("%%website%%", Server.UrlEncode(SubscriberResponse.ContainsKey("WEBSITE") ? SubscriberResponse["WEBSITE"] : string.Empty));
                link = link.Replace("%%age%%", Server.UrlEncode(SubscriberResponse.ContainsKey("AGE") ? SubscriberResponse["AGE"] : string.Empty));
                link = link.Replace("%%income%%", Server.UrlEncode(SubscriberResponse.ContainsKey("INCOME") ? SubscriberResponse["INCOME"] : string.Empty));
                link = link.Replace("%%gndr%%", Server.UrlEncode(SubscriberResponse.ContainsKey("GENDER") ? SubscriberResponse["GENDER"] : string.Empty));
                link = link.Replace("%%bdt%%", Server.UrlEncode(SubscriberResponse.ContainsKey("BIRTHDATE") ? SubscriberResponse["BIRTHDATE"] : string.Empty));
                link = link.Replace("%%ip%%", Request.ServerVariables["REMOTE_ADDR"].ToString());
                link = link.Replace("%%wlp%%", Server.UrlEncode("http://" + Request.ServerVariables["HTTP_HOST"] + ResolveUrl("~/Forms/ThankYou.aspx?") + Request.QueryString.ToString()));

                Dictionary<string, string> subsUDF = new Dictionary<string, string>();
                for (int i = 0; i < codesnippets.Length; i++)
                {
                    string key = codesnippets.GetValue(i).ToString().Split('=')[1].Trim('%');

                    if (SubscriberResponse.ContainsKey(key.ToUpper()))
                        subsUDF.Add(key, SubscriberResponse[key.ToUpper()]);
                }

                foreach (KeyValuePair<string, string> kvp in subsUDF)
                    link = link.Replace("%%" + kvp.Key + "%%", kvp.Value);

                Response.Redirect(link);
            }
            catch
            {
                Response.Redirect(link);
            }
        }

        private string ReplaceCodeSnippets(string emailbody)
        {
            emailbody = Regex.Replace(emailbody, "%%EmailID%%", EmailID.ToString(), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%EmailAddress%%", txtemailaddress.Text, RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Password%%", txtPassword.Text, RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Country%%", drpCountry.SelectedItem.Text, RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Title%%", SubscriberResponse.ContainsKey("TITLE") ? SubscriberResponse["TITLE"] : string.Empty, RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%FirstName%%", SubscriberResponse.ContainsKey("FIRSTNAME") ? SubscriberResponse["FIRSTNAME"] : string.Empty, RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%LastName%%", SubscriberResponse.ContainsKey("LASTNAME") ? SubscriberResponse["LASTNAME"] : string.Empty, RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%FullName%%", (SubscriberResponse.ContainsKey("FULLNAME") ? SubscriberResponse["FULLNAME"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Company%%", (SubscriberResponse.ContainsKey("COMPANY") ? SubscriberResponse["COMPANY"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Occupation%%", (SubscriberResponse.ContainsKey("OCCUPATION") ? SubscriberResponse["OCCUPATION"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Address%%", (SubscriberResponse.ContainsKey("ADDRESS") ? SubscriberResponse["ADDRESS"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Address2%%", (SubscriberResponse.ContainsKey("ADDRESS2") ? SubscriberResponse["ADDRESS2"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%City%%", (SubscriberResponse.ContainsKey("CITY") ? SubscriberResponse["CITY"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%State%%", (SubscriberResponse.ContainsKey("STATE") ? SubscriberResponse["STATE"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Zip%%", (SubscriberResponse.ContainsKey("ZIP") ? SubscriberResponse["ZIP"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Voice%%", (SubscriberResponse.ContainsKey("VOICE") ? SubscriberResponse["VOICE"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Mobile%%", (SubscriberResponse.ContainsKey("MOBILE") ? SubscriberResponse["MOBILE"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Fax%%", (SubscriberResponse.ContainsKey("FAX") ? SubscriberResponse["FAX"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Website%%", (SubscriberResponse.ContainsKey("WEBSITE") ? SubscriberResponse["WEBSITE"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Age%%", (SubscriberResponse.ContainsKey("AGE") ? SubscriberResponse["AGE"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Income%%", (SubscriberResponse.ContainsKey("INCOME") ? SubscriberResponse["INCOME"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%Gender%%", (SubscriberResponse.ContainsKey("GENDER") ? SubscriberResponse["GENDER"] : string.Empty), RegexOptions.IgnoreCase);
            emailbody = Regex.Replace(emailbody, "%%BirthDate%%", (SubscriberResponse.ContainsKey("BIRTHDATE") ? SubscriberResponse["BIRTHDATE"] : string.Empty), RegexOptions.IgnoreCase);

            Regex r = new Regex("%%");
            StringBuilder sb = new StringBuilder();

            Array codesnippets = r.Split(emailbody);
            for (int i = 0; i < codesnippets.Length; i++)
            {
                string line_data = codesnippets.GetValue(i).ToString();

                if (i % 2 == 0)
                    sb.Append(line_data);
                else
                {
                    line_data = line_data.ToUpper().Replace("USER_", "");
                    PubFormField f = pf.Fields.SingleOrDefault(x => x.ECNFieldName == line_data || x.ECNTextFieldName == line_data);

                    if (f != null)
                    {
                        string data = (SubscriberResponse.ContainsKey(line_data.ToUpper()) ? SubscriberResponse[line_data.ToUpper()] : string.Empty);

                        if (f.ECNTextFieldName == line_data || f.ControlType.Equals(ControlType.TextBox) || f.ControlType.Equals(ControlType.Hidden))
                            sb.Append(data);
                        else if (data != string.Empty)
                        {
                            string datatext = string.Empty;

                            foreach (string v in data.Split(new char[] { ',' }))
                            {
                                var fielddata = (from d in f.Data
                                                 where (d.DataValue == v)
                                                 select d);

                                foreach (var fd in fielddata)
                                    datatext += datatext.Equals(string.Empty) ? fd.DataText + " (" + fd.DataValue + ")" : ", " + fd.DataText + " (" + fd.DataValue + ")";
                            }

                            sb.Append(datatext);
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private string getCheckboxValues(CheckBoxList cb)
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in cb.Items)
            {
                if (item.Selected)
                {
                    selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
                }
            }

            return selectedvalues;
        }

        private string getCatCheckboxValues(CategorizedCheckBoxList cb)
        {
            string selectedvalues = string.Empty;
            foreach (string check in cb.Selections)
            {
                selectedvalues += selectedvalues == string.Empty ? check : "," + check;
            }
            return selectedvalues;
        }

        private void LoadECNFieldsHashtable()
        {
            hECNFields.Add("EMAILADDRESS", "e");
            hECNFields.Add("PASSWORD", "pwd");
            hECNFields.Add("FIRSTNAME", "fn");
            hECNFields.Add("LASTNAME", "ln");
            hECNFields.Add("FULLNAME", "n");
            hECNFields.Add("COMPANY", "compname");
            hECNFields.Add("TITLE", "t");
            hECNFields.Add("OCCUPATION", "occ");
            hECNFields.Add("ADDRESS", "adr");
            hECNFields.Add("ADDRESS2", "adr2");
            hECNFields.Add("CITY", "city");
            hECNFields.Add("STATE", "state");
            hECNFields.Add("ZIP", "zc");
            hECNFields.Add("COUNTRY", "ctry");
            hECNFields.Add("VOICE", "ph");
            hECNFields.Add("MOBILE", "mph");
            hECNFields.Add("FAX", "fax");
            hECNFields.Add("WEBSITE", "website");
            hECNFields.Add("AGE", "age");
            hECNFields.Add("INCOME", "income");
            hECNFields.Add("GENDER", "gndr");
            hECNFields.Add("BIRTHDATE", "bdt");
        }

        private string getQSParametername(string ECNFieldName)
        {
            if (hECNFields.Contains(ECNFieldName.ToUpper()))
            {
                return hECNFields[ECNFieldName.ToUpper()].ToString();
            }
            else
            {
                return "user_" + ECNFieldName.ToUpper();
            }
        }

        #region Post Data to ECN

        private void extHttpPost(string postURL)
        {
            try
            {
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                SqlCommand cmdHttpPost = new SqlCommand("insert into TrackHttpPost (EmailAddress, Pubcode, PostData, BrowserInfo) values ( @EmailAddress, @PubCode, @PostParams, @BrowserInfo)");
                cmdHttpPost.CommandType = CommandType.Text;
                cmdHttpPost.Parameters.AddWithValue("@EmailAddress", txtemailaddress.Text);
                cmdHttpPost.Parameters.AddWithValue("@PubCode", PubCode);
                cmdHttpPost.Parameters.AddWithValue("@PostParams", postURL);
                cmdHttpPost.Parameters.AddWithValue("@BrowserInfo", browserInfo);
                DataFunctions.Execute(cmdHttpPost);
                KMPS_JF_Objects.Objects.Utilities.ExternalHttpPost(postURL);
            }
            catch
            { }



        }

        private void HttpPost(string postparams, bool IsNewsletterSubscribe)
        {
            try
            {
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                SqlCommand cmdHttpPost = new SqlCommand("insert into TrackHttpPost (EmailAddress, Pubcode, PostData, BrowserInfo) values ( @EmailAddress, @PubCode, @PostParams, @BrowserInfo)");
                cmdHttpPost.CommandType = CommandType.Text;
                cmdHttpPost.Parameters.AddWithValue("@EmailAddress", txtemailaddress.Text);
                cmdHttpPost.Parameters.AddWithValue("@PubCode", PubCode);
                cmdHttpPost.Parameters.AddWithValue("@PostParams", postparams.TrimStart('?'));
                cmdHttpPost.Parameters.AddWithValue("@BrowserInfo", browserInfo);
                DataFunctions.Execute(cmdHttpPost);
            }
            catch
            { }

            LoadECNInputFields(postparams.TrimStart('?'));

            try
            {
                ECNPostCustomerID = hECNPostParams["c"] == null ? 0 : hECNPostParams["c"].ToString() == "" ? 0 : Convert.ToInt32(hECNPostParams["c"].ToString());
                ECNPostGroupID = hECNPostParams["g"] == null ? 0 : ECNPostGroupID = hECNPostParams["g"].ToString() == "" ? 0 : Convert.ToInt32(hECNPostParams["g"].ToString());
                ECNPostSmartFormID = hECNPostParams["sfid"] == null ? 0 : ECNPostSmartFormID = hECNPostParams["sfid"].ToString() == "" ? 0 : Convert.ToInt32(hECNPostParams["sfid"].ToString());
                ECNPostEmailID = hECNPostParams["ei"] == null ? 0 : hECNPostParams["ei"].ToString() == "" ? 0 : Convert.ToInt32(hECNPostParams["ei"].ToString());
                //this.EmailID = ECNPostEmailID;
                ECNPostBlastID = hECNPostParams["b"] == null ? 0 : hECNPostParams["b"].ToString() == "" ? 0 : Convert.ToInt32(hECNPostParams["b"].ToString());

                ECNPostEmailAddress = hECNPostParams["e"] == null ? string.Empty : Server.UrlDecode(hECNPostParams["e"].ToString().Trim());
                ECNPostSubscribe = hECNPostParams["s"] == null ? "S" : hECNPostParams["s"].ToString().Trim();
                ECNPostFormat = hECNPostParams["f"] == null ? "html" : hECNPostParams["f"].ToString().Trim();

                ECNPostReturnURL = hECNPostParams["url"] == null ? string.Empty : hECNPostParams["url"].ToString().Trim();
                ECNPostfromURL = Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString();
                ECNPostfromIP = Request.UserHostAddress == null ? string.Empty : Request.UserHostAddress.ToString();

                ECNPostgroup = new Groups(ECNPostGroupID);
                Emails old_email = SubscribeToGroup();

                //temporarily Commented  -  TODO -  Add new framework DLLs with PU changes & recompile
                try
                {
                    KMPlatform.Entity.User User = ECNUtils.GetUserFromECNAccessKey();

                    ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.Insert(0, old_email.ID(), "subscribe", "S", "", User);
                    ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(CustomerID, ECNPostGroupID, old_email.ID(), User, 0);
                }
                catch (Exception groupTriggerExc)
                {
                    SendNotification(ConfigurationManager.AppSettings["JFsFromEmail"].ToString(), ConfigurationManager.AppSettings["JFsFromName"].ToString(), "sunil@teamkm.com,bill.hipps@teamkm.com", "Group Trigger Failure", "<br/>Exception Message<br/>" + groupTriggerExc.Message + "<br/>Stack Trace<br/>" + groupTriggerExc.StackTrace + "<br/>URL<br/>" + Request.RawUrl);
                }


                //Check for Group Trigger Events 
                //int eID = old_email.ID();
                //int id = EmailActivityLog.InsertSubscribe(eID, 0, "S");
                //EmailActivityLog log = new EmailActivityLog(id);

                //log.SetGroup(ECNPostgroup);
                //log.SetEmail(new Emails(eID));

                //EventOrganizer eventer = new EventOrganizer();
                //eventer.CustomerID(ECNPostgroup.CustomerID());
                //eventer.Event(log);

                if (ECNPostSmartFormID > 0)
                {
                    //get smartForm Details
                    getSmartFormDetails(ECNPostSmartFormID);

                    //now send the response emails confirmation to the user & admins.
                    SendUserResponseEmails(ECNPostgroup, old_email);
                    SendAdminResponseEmails(ECNPostgroup, old_email);

                    //Finally show the user, the Thankyou Page / redirect'em to a page that's setup.
                    if (ECNPostResponse_UserScreen.ToLower().StartsWith("http://"))
                    {
                        ECNPostResponse_UserScreen = ReplaceCodeSnippets(ECNPostgroup, old_email, ECNPostResponse_UserScreen);
                        Response.Write("<script>document.location.href='" + ECNPostResponse_UserScreen + "';</script>");
                    }
                    else
                    {
                        ECNPostResponse_UserScreen = ReplaceCodeSnippets(ECNPostgroup, old_email, ECNPostResponse_UserScreen);
                        Response.Write(ECNPostResponse_UserScreen);
                    }
                }

            }
            catch (Exception ex)
            {
                SendNotification(ConfigurationManager.AppSettings["JFsFromEmail"].ToString(), ConfigurationManager.AppSettings["JFsFromName"].ToString(), "sunil@teamkm.com,bill.hipps@teamkm.com", "DB Update Failed in HttpPost method", "<br/>Exception Message<br/>" + ex.Message + "<br/>Stack Trace<br/>" + ex.StackTrace + "<br/>URL<br/>" + Request.RawUrl);
            }
        }

        private void LoadECNInputFields(string postparams)
        {
            try
            {
                hECNPostParams.Clear();

                if (postparams.StartsWith("&"))
                    postparams = postparams.TrimStart('&');

                string[] postItems = postparams.Split('&');
                string key = string.Empty;

                foreach (string sItem in postItems)
                {
                    key = sItem.Split('=')[0].ToLower();

                    if (!hECNPostParams.Contains(key))
                    {
                        hECNPostParams.Add(key, sItem.Split('=')[1]);
                    }
                }
                if (!hECNPostParams.ContainsKey("notes"))
                {
                    hECNPostParams.Add("notes", "");
                }
            }
            catch { }
        }

        //get smartform details
        private void getSmartFormDetails(int sfID)
        {
            if (sfID > 0)
            {
                SqlCommand cmdSmartForm = new SqlCommand("SELECT * FROM SmartFormsHistory  with (NOLOCK) WHERE SmartFormID = @sfID");
                cmdSmartForm.CommandType = CommandType.Text;
                cmdSmartForm.Parameters.Add(new SqlParameter("@sfID", SqlDbType.Int)).Value = sfID;
                DataTable dt = DataFunctions.GetDataTable("communicator", cmdSmartForm);
                DataRow dr = dt.Rows[0];

                ECNPostResponse_FromEmail = dr.IsNull("Response_FromEmail") ? string.Empty : dr["Response_FromEmail"].ToString();
                ECNPostResponse_UserMsgSubject = dr.IsNull("Response_UserMsgSubject") ? string.Empty : dr["Response_UserMsgSubject"].ToString();
                ECNPostResponse_UserMsgBody = dr.IsNull("Response_UserMsgBody") ? string.Empty : dr["Response_UserMsgBody"].ToString();
                ECNPostResponse_UserScreen = dr.IsNull("Response_UserScreen") ? string.Empty : dr["Response_UserScreen"].ToString();
                ECNPostResponse_AdminEmail = dr.IsNull("Response_AdminEmail") ? string.Empty : dr["Response_AdminEmail"].ToString();
                ECNPostResponse_AdminMsgSubject = dr.IsNull("Response_AdminMsgSubject") ? string.Empty : dr["Response_AdminMsgSubject"].ToString();
                ECNPostResponse_AdminMsgBody = dr.IsNull("Response_AdminMsgBody") ? string.Empty : dr["Response_AdminMsgBody"].ToString();
            }
        }

        //User Email
        private void SendUserResponseEmails(Groups grpObject, Emails theEmailObject)
        {
            if (ECNPostResponse_FromEmail.Length > 5 && ECNPostResponse_UserMsgSubject.Trim().Length > 0 && ECNPostResponse_UserMsgBody.Trim().Length > 0)
            {
                string userEmailbody = ReplaceCodeSnippets(grpObject, theEmailObject, ECNPostResponse_UserMsgBody);

                //Add Unsubscribe Link at the bottom of the email as per CAN-SPAM & USI requested it to be done. 
                string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/websubscribe.aspx?e=" + ECNPostEmailAddress + "&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U'>Unsubscribe</a>.</div></p>";
                userEmailbody += unsubscribeText;
                ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
                emailFunctions.SimpleSend(theEmailObject.EmailAddress(), ECNPostResponse_FromEmail, ECNPostResponse_UserMsgSubject, userEmailbody);
            }
        }

        //Admin Email
        private void SendAdminResponseEmails(Groups grpObject, Emails theEmailObject)
        {
            if (ECNPostResponse_AdminEmail.Length > 5 && ECNPostResponse_FromEmail.Length > 5)
            {
                string adminEmailbody = ReplaceCodeSnippets(grpObject, theEmailObject, ECNPostResponse_AdminMsgBody);
                ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
                emailFunctions.SimpleSend(ECNPostResponse_AdminEmail, ECNPostResponse_FromEmail, ECNPostResponse_AdminMsgSubject, adminEmailbody);
            }
        }

        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text.Trim();
        }

        //subscribe email to the group
        private Emails SubscribeToGroup()
        {
            try
            {
                LoadFields();
                ECNPosthUDFFields = GetGroupDataFields(ECNPostGroupID);

                StringBuilder xmlProfile = new StringBuilder("");
                StringBuilder xmlUDF = new StringBuilder("");
                IDictionaryEnumerator en = ECNPosthProfileFields.GetEnumerator();

                xmlProfile.Append("<Emails>");
                xmlProfile.Append("<subscribetypecode>S</subscribetypecode>");

                while (en.MoveNext())
                {
                    try
                    {
                        if (hECNPostParams.ContainsKey(en.Value))
                        {
                            string keyValue = Server.UrlDecode(Convert.ToString(hECNPostParams[en.Value.ToString()]));
                            if (en.Key.ToString().ToLower() == "voice")
                            {
                                keyValue.Replace("(", "");
                                keyValue.Replace(")", "");
                                keyValue.Replace("-", "");
                                keyValue.Replace(" ", "");
                            }

                            if (en.Key.ToString().ToLower() == "notes")
                                xmlProfile.Append("<" + en.Key.ToString() + ">" + "<![CDATA[ [" + ECNPostfromIP + "] [" + ECNPostfromURL + "] [" + DateTime.Now.ToString() + "] ]]> " + "</" + en.Key.ToString() + ">");
                            else
                                xmlProfile.Append("<" + en.Key.ToString() + ">" + CleanXMLString(keyValue) + "</" + en.Key.ToString() + ">");
                        }
                    }
                    catch
                    { }
                }

                xmlProfile.Append("</Emails>");

                if (ECNPosthUDFFields.Count > 0)
                {
                    xmlUDF.Append("<row>");
                    xmlUDF.Append("<ea>" + CleanXMLString(ECNPostEmailAddress) + "</ea>");

                    IDictionaryEnumerator en1 = ECNPosthUDFFields.GetEnumerator();
                    while (en1.MoveNext())
                    {
                        try
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + CleanXMLString(Server.UrlDecode(Convert.ToString(hECNPostParams[en1.Value.ToString()]))) + "</v></udf>");
                        }
                        catch
                        { }
                    }
                    xmlUDF.Append("</row>");
                }

                UpdateToDB(xmlProfile.ToString(), xmlUDF.ToString());

                Emails email = null;
                string body = string.Empty;
                try
                {
                    email = Emails.GetEmailByID(ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(ECNPostEmailAddress, ECNPostGroupID).EmailID);
                }
                catch (Exception ex)
                {
                    body = "DB Update Failed in  SubscribeToGroup method - Emails.GetEmailByID(ECNPostgroup.WhatEmail(ECNPostEmailAddress).ID()) method;<BR><BR>";
                    body += "<br/>Exception Message<br/>" + ex.Message + "<br/>Stack Trace<br/>" + ex.StackTrace + "<br/>URL<br/>" + Request.RawUrl + "<BR><BR>";

                    body += "DateTime : " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "<BR><BR>";

                    body += "EmailAddress : " + ECNPostEmailAddress + "<BR><BR>";
                    body += "GroupID : " + ECNPostGroupID + "<BR><BR>";
                    body += "xmlProfile : " + xmlProfile.Replace("<", "&lt;").Replace(">", "&gt;") + "<BR><BR>";
                    body += "xmlUDF : " + xmlUDF.Replace("<", "&lt;").Replace(">", "&gt;") + "<BR>";

                    SendNotification(ConfigurationManager.AppSettings["JFsFromEmail"].ToString(), ConfigurationManager.AppSettings["JFsFromName"].ToString(), "sunil@teamkm.com,bill.hipps@teamkm.com", "DB Update Failed in Emails.GetEmailByID(ECNPostgroup.WhatEmail(ECNPostEmailAddress).ID()) method", body);
                    return null;
                }

                return email;
            }
            catch (Exception ex)
            {
                SendNotification(ConfigurationManager.AppSettings["JFsFromEmail"].ToString(), ConfigurationManager.AppSettings["JFsFromName"].ToString(), "sunil@teamkm.com,bill.hipps@teamkm.com", "DB Update Failed in SubscribeToGroup method", "<br/>Exception Message<br/>" + ex.Message + "<br/>Stack Trace<br/>" + ex.StackTrace + "<br/>URL<br/>" + Request.RawUrl);
                Response.Write(ex.Message);
                return null;
            }
        }

        //Save the profile and UDF for given XML
        private void UpdateToDB(string xmlProfile, string xmlUDF)
        {
            int ECNUserID = ECNUtils.GetECNUserIDFromAccessKey();

            //SqlCommand cmd = new SqlCommand("sp_ImportEmails");
            SqlCommand cmd = new SqlCommand("e_EmailGroup_ImportEmails");
            try
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar);
                cmd.Parameters["@CustomerID"].Value = ECNPostCustomerID;

                cmd.Parameters.Add("@GroupID", SqlDbType.VarChar);
                cmd.Parameters["@GroupID"].Value = ECNPostGroupID;

                cmd.Parameters.Add("@xmlProfile", SqlDbType.Text);
                cmd.Parameters["@xmlProfile"].Value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>";

                cmd.Parameters.Add("@xmlUDF", SqlDbType.Text);
                cmd.Parameters["@xmlUDF"].Value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF + "</XML>";

                cmd.Parameters.Add("@formattypecode", SqlDbType.VarChar);
                cmd.Parameters["@formattypecode"].Value = ECNPostFormat;

                cmd.Parameters.Add("@subscribetypecode", SqlDbType.VarChar);
                cmd.Parameters["@subscribetypecode"].Value = ECNPostSubscribe;

                cmd.Parameters.Add("@EmailAddressOnly", SqlDbType.Bit);
                cmd.Parameters["@EmailAddressOnly"].Value = 0;

                cmd.Parameters.Add("@UserID", SqlDbType.VarChar);
                cmd.Parameters["@UserID"].Value = ECNUserID;

                cmd.Parameters.Add("@source", SqlDbType.VarChar);
                cmd.Parameters["@source"].Value = "KMPS_JF.Subscriptions.UpdateToDB method";

                cmd.Parameters.Add("@insertMS", SqlDbType.Bit);
                cmd.Parameters["@insertMS"].Value = "true";

                DataFunctions.Execute("communicator", cmd);

            }
            catch (Exception ex)
            {
                SendNotification(ConfigurationManager.AppSettings["JFsFromEmail"].ToString(), ConfigurationManager.AppSettings["JFsFromName"].ToString(), "sunil@teamkm.com,bill.hipps@teamkm.com", "DB Update Failed in UpdateToDB method", ex.Message + "<br/>" + xmlProfile + "<br/>" + xmlUDF);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        //Load all UDF Fields for the group
        private Hashtable GetGroupDataFields(int groupID)
        {
            SqlCommand cmdsqlstmt = new SqlCommand("SELECT * FROM GroupDatafields  with (NOLOCK) WHERE GroupID = @groupID");
            cmdsqlstmt.CommandType = CommandType.Text;
            cmdsqlstmt.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int)).Value = groupID;

            //string sqlstmt = " SELECT * FROM GroupDatafields WHERE GroupID=" + groupID;
            DataTable emailstable = DataFunctions.GetDataTable("communicator", cmdsqlstmt);

            Hashtable fields = new Hashtable();
            foreach (DataRow dr in emailstable.Rows)
                fields.Add(Convert.ToInt32(dr["GroupDataFieldsID"]), "user_" + dr["ShortName"].ToString().ToLower());

            return fields;
        }

        //Load profile fields
        private void LoadFields()
        {
            ECNPosthProfileFields.Clear();
            ECNPosthProfileFields.Add("emailaddress", "e");
            ECNPosthProfileFields.Add("title", "t");
            ECNPosthProfileFields.Add("firstname", "fn");
            ECNPosthProfileFields.Add("lastname", "ln");
            ECNPosthProfileFields.Add("fullname", "n");
            ECNPosthProfileFields.Add("company", "compname");
            ECNPosthProfileFields.Add("occupation", "t");
            ECNPosthProfileFields.Add("address", "adr");
            ECNPosthProfileFields.Add("address2", "adr2");
            ECNPosthProfileFields.Add("city", "city");
            ECNPosthProfileFields.Add("state", "state");
            ECNPosthProfileFields.Add("zip", "zc");
            ECNPosthProfileFields.Add("country", "ctry");
            ECNPosthProfileFields.Add("voice", "ph");
            ECNPosthProfileFields.Add("mobile", "mph");
            ECNPosthProfileFields.Add("fax", "fax");
            ECNPosthProfileFields.Add("website", "website");
            ECNPosthProfileFields.Add("age", "age");
            ECNPosthProfileFields.Add("income", "income");
            ECNPosthProfileFields.Add("gender", "gndr");
            ECNPosthProfileFields.Add("user1", "usr1");
            ECNPosthProfileFields.Add("user2", "usr2");
            ECNPosthProfileFields.Add("user3", "usr3");
            ECNPosthProfileFields.Add("user4", "usr4");
            ECNPosthProfileFields.Add("user5", "usr5");
            ECNPosthProfileFields.Add("user6", "usr6");
            ECNPosthProfileFields.Add("birthdate", "bdt");
            ECNPosthProfileFields.Add("userevent1", "usrevt1");
            ECNPosthProfileFields.Add("userevent1date", "usrevtdt1");
            ECNPosthProfileFields.Add("userevent2", "usrevt2");
            ECNPosthProfileFields.Add("userevent2date", "usrevtdt2");
            ECNPosthProfileFields.Add("notes", "notes");
            ECNPosthProfileFields.Add("password", "password");
        }

        //Replace the code snippet
        private string ReplaceCodeSnippets(Groups group, Emails emailObj, string emailbody)
        {
            emailbody = emailbody.Replace("%%GroupID%%", group.ID().ToString());
            emailbody = emailbody.Replace("%%GroupName%%", group.Name.ToString());
            emailbody = emailbody.Replace("%%EmailID%%", emailObj.ID().ToString());
            emailbody = emailbody.Replace("%%EmailAddress%%", emailObj.EmailAddress().ToString());
            emailbody = emailbody.Replace("%%Password%%", emailObj.Password.ToString());
            emailbody = emailbody.Replace("%%Title%%", emailObj.Title.ToString());
            emailbody = emailbody.Replace("%%FirstName%%", emailObj.FirstName.ToString());
            emailbody = emailbody.Replace("%%LastName%%", emailObj.LastName.ToString());
            emailbody = emailbody.Replace("%%FullName%%", emailObj.FullName.ToString());
            emailbody = emailbody.Replace("%%Company%%", emailObj.Company.ToString());
            emailbody = emailbody.Replace("%%Occupation%%", emailObj.Title.ToString());
            emailbody = emailbody.Replace("%%Address%%", emailObj.Address.ToString());
            emailbody = emailbody.Replace("%%Address2%%", emailObj.Address2.ToString());
            emailbody = emailbody.Replace("%%City%%", emailObj.City.ToString());
            emailbody = emailbody.Replace("%%State%%", emailObj.State.ToString());
            emailbody = emailbody.Replace("%%Zip%%", emailObj.Zip.ToString());
            emailbody = emailbody.Replace("%%Country%%", emailObj.Country.ToString());
            emailbody = emailbody.Replace("%%Voice%%", emailObj.Voice.ToString());
            emailbody = emailbody.Replace("%%Mobile%%", emailObj.Mobile.ToString());
            emailbody = emailbody.Replace("%%Fax%%", emailObj.Fax.ToString());
            emailbody = emailbody.Replace("%%Website%%", emailObj.Website.ToString());
            emailbody = emailbody.Replace("%%Age%%", emailObj.Age.ToString());
            emailbody = emailbody.Replace("%%Income%%", emailObj.Income.ToString());
            emailbody = emailbody.Replace("%%Gender%%", emailObj.Gender.ToString());
            emailbody = emailbody.Replace("%%Notes%%", emailObj.Notes.ToString());
            emailbody = emailbody.Replace("%%BirthDate%%", emailObj.BirthDate.ToString());
            emailbody = emailbody.Replace("%%User1%%", emailObj.User1.ToString());
            emailbody = emailbody.Replace("%%User2%%", emailObj.User2.ToString());
            emailbody = emailbody.Replace("%%User3%%", emailObj.User3.ToString());
            emailbody = emailbody.Replace("%%User4%%", emailObj.User4.ToString());
            emailbody = emailbody.Replace("%%User5%%", emailObj.User5.ToString());
            emailbody = emailbody.Replace("%%User6%%", emailObj.User6.ToString());
            emailbody = emailbody.Replace("%%UserEvent1%%", emailObj.UserEvent1.ToString());
            emailbody = emailbody.Replace("%%UserEvent1Date%%", emailObj.UserEvent1Date.ToString());
            emailbody = emailbody.Replace("%%UserEvent2%%", emailObj.UserEvent2.ToString());
            emailbody = emailbody.Replace("%%UserEvent2Date%%", emailObj.UserEvent2Date.ToString());

            //UDF Data 
            SortedList UDFHash = group.UDFHash;
            ArrayList _keyArrayList = new ArrayList();
            ArrayList _UDFData = new ArrayList();

            if (UDFHash.Count > 0)
            {
                IDictionaryEnumerator UDFHashEnumerator = UDFHash.GetEnumerator();
                while (UDFHashEnumerator.MoveNext())
                {
                    string UDFData = "";
                    string _value = "user_" + UDFHashEnumerator.Value.ToString();
                    string _key = UDFHashEnumerator.Key.ToString();
                    try
                    {
                        UDFData = Convert.ToString(hECNPostParams[_value]);
                        _keyArrayList.Add(_key);
                        _UDFData.Add(UDFData);
                        emailbody = emailbody.Replace("%%" + _value + "%%", UDFData);
                    }
                    catch
                    {
                        emailbody = emailbody.Replace("%%" + _value + "%%", "");
                    }
                }
            }
            //End UDF Data

            return emailbody;
        }
        #endregion


        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool IsOverrideNQ = false;
                bool IsNonQualifiedCountrySelected = false;

                CountryID = Convert.ToInt32(drpCountry.SelectedItem.Value);

                pc = null;
                pf = null;

                getPubForm();

                if (pf.IsNonQualSetup)
                {
                    IsOverrideNQ = ((chk_user_Demo7_Digital.Checked && pf.DisableNonQualForDigital) || (chk_user_Demo7_Print.Checked && pf.ShowNQPrintAsDigital));
                    IsNonQualifiedCountrySelected = pc.IsNonQualified;
                }

                if (IsOverrideNQ)
                {
                    LoadSubscriptionForm();
                }
                //else if (NonQualExp && pf.NQPrintRedirectUrl.Trim().Length > 0)
                //{

                //    if (getQueryString("rURL") != string.Empty)
                //    {
                //        Response.Redirect(String.Format("{0}?qn=Country&qv={1}&ctry={2}&e={3}&pubcode={4}&NQCountry=1&ei={5}&btx_m={6}&btx_i={7}&rURL=" + getQueryString("rURL"), pf.NQPrintRedirectUrl, drpCountry.SelectedItem.Text, drpCountry.SelectedItem.Text, txtemailaddress.Text, pub.PubCode, EmailID, MagazineID, IssueID), true);
                //    }
                //    else
                //    {
                //        Response.Redirect(String.Format("{0}?qn=Country&qv={1}&ctry={2}&e={3}&pubcode={4}&NQCountry=1&ei={5}&btx_m={6}&btx_i={7}", pf.NQPrintRedirectUrl, drpCountry.SelectedItem.Text, drpCountry.SelectedItem.Text, txtemailaddress.Text, pub.PubCode, EmailID, MagazineID, IssueID), true);
                //    }
                //}
                else if (IsNonQualifiedCountrySelected)
                {
                    string NONQualRedirectURL = ConfigurationManager.AppSettings["NQLandingPage"].ToString();
                    string user_demo7 = string.Empty;

                    try
                    {
                        if (pf.EnablePrintAndDigital && chk_user_Demo7_Both.Checked)
                        {
                            user_demo7 = "C";
                        }
                        else if ((chk_user_Demo7_Print.Checked && pf.ShowPrintAsDigital) || chk_user_Demo7_Digital.Checked)
                        {
                            user_demo7 = "B";
                        }
                        else if (chk_user_Demo7_Print.Checked)
                            user_demo7 = "A";
                    }
                    catch
                    { }

                    if (user_demo7 == "A" && pf.NQPrintRedirectUrl.Trim().Length > 0)
                    {
                        NONQualRedirectURL = pf.NQPrintRedirectUrl.Trim();
                    }
                    else if (user_demo7 == "B" && pf.NQDigitalRedirectUrl.Trim().Length > 0)
                    {
                        NONQualRedirectURL = pf.NQDigitalRedirectUrl.Trim();
                    }
                    else if (user_demo7 == "C" && pf.NQBothRedirectUrl.Trim().Length > 0)
                    {
                        NONQualRedirectURL = pf.NQBothRedirectUrl.Trim();
                    }

                    if (getQueryString("rURL") != string.Empty)
                    {
                        Response.Redirect(String.Format("{0}?qn=country&qv={1}&PubCode={2}&PubID={3}&PFID={4}&NQCountry=1&ei={5}&btx_m={6}&btx_i={7}&rURL=" + getQueryString("rURL"), NONQualRedirectURL, drpCountry.SelectedItem.Text, PubCode, pub.PubID, pf.PFID, EmailID, MagazineID, IssueID));
                    }
                    else
                    {
                        Response.Redirect(String.Format("{0}?qn=country&qv={1}&PubCode={2}&PubID={3}&PFID={4}&NQCountry=1&ei={5}&btx_m={6}&btx_i={7}", NONQualRedirectURL, drpCountry.SelectedItem.Text, PubCode, pub.PubID, pf.PFID, EmailID, MagazineID, IssueID));
                    }
                }
                else
                {
                    LoadSubscriptionForm();
                }
            }
            catch (Exception ex) { Response.Write(ex.ToString()); }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dtNewsletterSearch = new DataTable();
            dtNewsletterSearch.Columns.Add("subscribed");
            dtNewsletterSearch.Columns.Add("CustomerID");
            dtNewsletterSearch.Columns.Add("Displayname");
            dtNewsletterSearch.Columns.Add("ShowDisplayName");
            dtNewsletterSearch.Columns.Add("Description");
            dtNewsletterSearch.Columns.Add("EcnGroupID");

            try
            {
                foreach (RepeaterItem ritem in ((Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition)).Items)
                {
                    GridView gvNewsletters = (GridView)ritem.FindControl("gvNewsletters");

                    foreach (GridViewRow r in gvNewsletters.Rows)
                    {
                        HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)r.FindControl("chkselect");
                        Label lblCustomerID = (Label)r.FindControl("lblCustomerID");
                        Label lblDisplayName = (Label)r.FindControl("lblDisplayName");
                        //Label lblSpace = (Label)r.FindControl("lblSpace");
                        Label lblDescription = (Label)r.FindControl("lblDescription");

                        int EcnGroupID = Convert.ToInt32(gvNewsletters.DataKeys[r.RowIndex].Value.ToString());

                        DataRow dr = dtNewsletterSearch.NewRow();
                        dr["subscribed"] = chkSelect.Checked ? "Y" : "N";
                        dr["CustomerID"] = lblCustomerID.Text;
                        dr["Displayname"] = lblDisplayName.Text;
                        //dr["ShowDisplayName"] = lblSpace.Visible;
                        dr["Description"] = lblDescription.Text;
                        dr["EcnGroupID"] = EcnGroupID;

                        dtNewsletterSearch.Rows.Add(dr);
                    }
                }

                DataView dvNewsletterSearchResult = (from t in dtNewsletterSearch.AsEnumerable()
                                                     where t.Field<string>("Displayname").ToUpper().Contains(txtSearch.Text.ToUpper())
                                                     select t).AsDataView();

                if (dvNewsletterSearchResult != null && dvNewsletterSearchResult.Count == 0)
                {
                    pnlNewsletterSearchResults.Style.Add("display", "block");
                    lblResultMsg.Visible = true;
                    lblResultMsg.Font.Bold = true;
                    lblResultMsg.Text = "No Results found";
                    dvNewsletterSearchResult.Table.Clear();
                    gvNewsletterResults.DataSource = dvNewsletterSearchResult;
                    gvNewsletterResults.DataBind();
                }
                else
                {
                    pnlNewsletterSearchResults.Style.Add("display", "block");
                    lblResultMsg.Visible = false;
                    gvNewsletterResults.DataSource = dvNewsletterSearchResult;
                    gvNewsletterResults.DataBind();
                }
            }
            catch { }

            extpnlNewsletter.Show();
        }

        protected void btnNewsletterSearchResults_Click(object sender, EventArgs e)
        {
            ArrayList unsubGroups = new ArrayList();
            ArrayList subGroups = new ArrayList();
            GridView gvNewslettersSearch = (GridView)Page.FindControl("gvNewsletterResults");

            try
            {
                foreach (GridViewRow r in gvNewslettersSearch.Rows)
                {
                    HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)r.FindControl("chkselect");
                    int EcnGroupID = Convert.ToInt32(gvNewslettersSearch.DataKeys[r.RowIndex].Value.ToString());

                    if (chkSelect != null && !chkSelect.Checked && EcnGroupID > 0)
                        unsubGroups.Add(EcnGroupID);
                    else if (chkSelect != null && chkSelect.Checked && EcnGroupID > 0)
                        subGroups.Add(EcnGroupID);
                }

                int unchekedcount = 0;
                foreach (RepeaterItem ritem in ((Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition)).Items)
                {
                    GridView gvNewsletters = (GridView)ritem.FindControl("gvNewsletters");

                    foreach (GridViewRow r in gvNewsletters.Rows)
                    {
                        HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)r.FindControl("chkselect");
                        int EcnGroupID = Convert.ToInt32(gvNewsletters.DataKeys[r.RowIndex].Value.ToString());

                        if (chkSelect != null && chkSelect.Checked && unsubGroups.Contains(EcnGroupID))
                        {
                            chkSelect.Checked = false;
                            unchekedcount++;
                        }
                        else if (chkSelect != null && !chkSelect.Checked && subGroups.Contains(EcnGroupID))
                        {
                            chkSelect.Checked = true;
                        }
                    }
                }

                foreach (RepeaterItem ritem in ((Repeater)Page.FindControl("rptCategory" + pf.NewsletterPosition)).Items)
                {
                    HtmlInputCheckBox chkSelect = (HtmlInputCheckBox)ritem.FindControl("chkNewsletterCat");

                    if (chkSelect != null && unchekedcount > 0)
                        chkSelect.Checked = false;
                }
            }
            catch { }

            extpnlNewsletter.Hide();
        }

        #region CACHED OBJECTS

        private DataTable getFormFieldSettings()
        {
            DataTable dtFormFieldSettings = null;

            SqlCommand cmddtFFS = new SqlCommand("sp_getFormFieldSettings");
            cmddtFFS.CommandType = CommandType.StoredProcedure;
            cmddtFFS.Parameters.Add(new SqlParameter("@PubID", PubID.ToString()));
            cmddtFFS.Parameters.Add(new SqlParameter("@countryID", drpCountry.SelectedItem.Value.ToString()));

            if (CacheUtil.IsCacheEnabled())
            {
                dtFormFieldSettings = (DataTable)CacheUtil.GetFromCache("FORMFIELDSETTINGS_" + PubID.ToString() + "_" + drpCountry.SelectedItem.Value, "JOINTFORMS");

                if (dtFormFieldSettings == null)
                {
                    dtFormFieldSettings = DataFunctions.GetDataTable(cmddtFFS);

                    CacheUtil.AddToCache("FORMFIELDSETTINGS_" + PubID.ToString() + "_" + drpCountry.SelectedItem.Value, dtFormFieldSettings, "JOINTFORMS");
                }
                return dtFormFieldSettings;
            }
            else
            {
                return DataFunctions.GetDataTable(cmddtFFS);
            }
        }

        private DataTable getCategoryforPubFormCountry()
        {
            DataTable dtCategoryforPubFormCountry = null;

            SqlCommand cmddtCat = new SqlCommand("sp_getCategoryforPubFormCountry");
            cmddtCat.CommandType = CommandType.StoredProcedure;
            cmddtCat.Parameters.Add(new SqlParameter("@PubID", PubID.ToString()));
            cmddtCat.Parameters.Add(new SqlParameter("@countryID", drpCountry.SelectedItem.Value.ToString()));

            if (CacheUtil.IsCacheEnabled())
            {
                dtCategoryforPubFormCountry = (DataTable)CacheUtil.GetFromCache("CATEGORYFORPUBFORMCOUNTRY_" + PubID.ToString() + "_" + drpCountry.SelectedItem.Value, "JOINTFORMS");

                if (dtCategoryforPubFormCountry == null)
                {
                    dtCategoryforPubFormCountry = DataFunctions.GetDataTable(cmddtCat);

                    CacheUtil.AddToCache("CATEGORYFORPUBFORMCOUNTRY_" + PubID.ToString() + "_" + drpCountry.SelectedItem.Value, dtCategoryforPubFormCountry, "JOINTFORMS");
                }

                return dtCategoryforPubFormCountry;
            }
            else
            {
                return DataFunctions.GetDataTable(cmddtCat);
            }
        }

        private DataTable AutoSubscribeNL(int newsletterID)
        {
            DataTable dtAutoSubscribeNL = null;

            SqlCommand cmdAutoNL = new SqlCommand("spGetNewsLetterAutoSubscription");
            cmdAutoNL.CommandType = CommandType.StoredProcedure;
            cmdAutoNL.Parameters.AddWithValue("@NewsletterID", newsletterID.ToString());

            if (CacheUtil.IsCacheEnabled())
            {
                dtAutoSubscribeNL = (DataTable)CacheUtil.GetFromCache("NEWSLETTERAUTOSUBSCRIPTION_" + newsletterID.ToString(), "JOINTFORMS");

                if (dtAutoSubscribeNL == null)
                {
                    dtAutoSubscribeNL = DataFunctions.GetDataTable(cmdAutoNL);

                    CacheUtil.AddToCache("NEWSLETTERAUTOSUBSCRIPTION_" + newsletterID.ToString(), dtAutoSubscribeNL, "JOINTFORMS");
                }

                return dtAutoSubscribeNL;
            }
            else
            {
                return DataFunctions.GetDataTable(cmdAutoNL);
            }
        }



        #endregion

    }
}