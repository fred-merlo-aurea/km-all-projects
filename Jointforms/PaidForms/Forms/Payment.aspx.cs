using ecn.communicator.classes;
using Encore.PayPal.Nvp;
using KMPS_JF_Objects.Objects;
using NXTBookAPI.BusinessLogic;
using NXTBookAPI.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PaidForms.Forms
{
    public partial class Payment : System.Web.UI.Page
    {
        private string ECNPostFormat = string.Empty;
        private static int PubID = -1;
        private PaidForm paidform = null;

        private Hashtable ECNPosthProfileFields = new Hashtable();
        private Hashtable ECNPosthUDFFields = new Hashtable();
        Hashtable hECNPostParams = new Hashtable();
        
        private string TranID = string.Empty;

        private int FormID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["FormID"].ToString());
                }
                catch { return 0; }
            }
        }

        public string PubCode
        {
            get
            {
                return Request["pubcode"].ToString();
            }
            set
            {
                ViewState["PubCode"] = value;
            }
        }

        public string EmailAddress
        {
            get
            {
                return Request["emailaddress"].ToString();
            }
        }

        public string Token
        {
            get
            {
                return Request["token"].ToString();
            }
        }

        public List<int> GroupID
        {
            get
            {
                return Request["GroupID"].Split(',').Select(Int32.Parse).ToList();
            }
            set
            {
                ViewState["GroupID"] = value;
            }
        }

        public string TransactionCacheName
        {
            get
            {
                try
                {
                    return EmailAddress + "_" + FormID.ToString() + "_TransactionProcessed";
                }
                catch
                {
                    return "";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (FormID == 0)
                    {
                        phError.Visible = true;
                        lblErrorMessage.Text = "FormID is missing in the URL.";
                        return;
                    }

                    try
                    {
                        PubID = Convert.ToInt32(getQueryString("pubid"));
                    }
                    catch { }

                    try
                    {
                        PubCode = getQueryString("pubcode");

                        if (PubCode.Length > 15)
                        {
                            PubCode = PubCode.Substring(0, 15);
                        }

                        PubCode = PubCode.Replace("'", "''");
                    }
                    catch { }

                    paidform = PaidForm.GetByPaidFormID(FormID);

                    loadHeaderFooter();

                    string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;

                    if (Request.QueryString.Get("token") != null)
                    {
                        ProcessPaypalPayment();
                    }
                    else
                    {
                        //No token in the query string
                        lblMessage.Text = "Payment was not successful";
                        lblMessage.Visible = true;

                        return;
                    }
                }
            }
            catch  (Exception ex)
            {
                lblMessage.Text = "Payment was not successful";
                lblMessage.Visible = true;
            }
        }

        private void AddToCache(string Name, object Value)
        {
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch { }


            if (isCacheEnabled)
            {

                try
                {
                    KM.Common.CacheUtil.AddToCache(Name, Value);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string emailMsg = "Error in Paid forms <br /><br />";

                        emailMsg += "Errorlocation : Adding Demographics to Cache<br /><br />";

                        emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                        emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                        emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                        emailMsg += "<b>Exception Details:</b>" + ex.Message;

                        EmailFunctions emailFunctions = new EmailFunctions();
                        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                    }
                    catch { }
                }
            }

        }

        private object GetFromCache(string Name)
        {
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch { }


            if (isCacheEnabled)
            {
                try
                {
                    return KM.Common.CacheUtil.GetFromCache(Name);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string emailMsg = "Error in Paid forms <br /><br />";

                        emailMsg += "Errorlocation : Getting " + Name + " Cache object from Cache<br /><br />";

                        emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                        emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                        emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                        emailMsg += "<b>Exception Details:</b>" + ex.Message;

                        EmailFunctions emailFunctions = new EmailFunctions();
                        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                    }
                    catch { }
                }
            }
            return null;
        }

        private void RemoveFromCache(string Name)
        {
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch { }


            if (isCacheEnabled)
            {

                try
                {
                    KM.Common.CacheUtil.RemoveFromCache(Name);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string emailMsg = "Error in Paid forms <br /><br />";

                        emailMsg += "Errorlocation : Removing " + Name + " Cache object from Cache<br /><br />";

                        emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                        emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                        emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                        emailMsg += "<b>Exception Details:</b>" + ex.Message;

                        EmailFunctions emailFunctions = new EmailFunctions();
                        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                    }
                    catch { }
                }
            }

        }

        private static string getQueryString(string qs)
        {
            try { return HttpContext.Current.Request.QueryString[qs].ToString(); }
            catch { return string.Empty; }
        }

        private void loadHeaderFooter()
        {
            phHeader.Controls.Add(new LiteralControl(paidform.HeaderHTML));
            phFooter.Controls.Add(new LiteralControl(paidform.FooterHTML));
        }

        private Dictionary<int, Dictionary<string, string>> GetCacheValue(string cookieName)
        {
            Dictionary<int, Dictionary<string, string>> retValue = new Dictionary<int, Dictionary<string, string>>(); ;
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch { }


            if (isCacheEnabled)
            {
                try
                {
                    retValue = (Dictionary<int, Dictionary<string, string>>)KM.Common.CacheUtil.GetFromCache(cookieName);
                    return retValue;
                }
                catch 
                {
                    return retValue;
                }
            }
            else
            {
                return retValue;
            }
        }

        private void ProcessPaypalPayment()
        {
            string ProfileParams = string.Empty;
            string DemographicParams = string.Empty;
            string groupParams = string.Empty;
            string PaidParams = string.Empty;

            List<string> AutoSubpubCodes = new List<string>();
            Dictionary<int, Dictionary<string, string>> dPurchasedProducts = GetCacheValue(EmailAddress.ToLower().Trim() + "_" + FormID.ToString() + "_PurchasedProducts");

            NvpGetExpressCheckoutDetails ppGet = new NvpGetExpressCheckoutDetails();
            ppGet.Add(NvpGetExpressCheckoutDetails.Request.TOKEN, Token);

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (paidform.UseTestMode)
            {
                ppGet.Environment = NvpEnvironment.Sandbox;
                ppGet.Credentials.Username = WebConfigurationManager.AppSettings["PayflowAccount"].ToString();
                ppGet.Credentials.Password = WebConfigurationManager.AppSettings["PayflowPassword"].ToString();
                ppGet.Credentials.Signature = WebConfigurationManager.AppSettings["PayflowSignature"].ToString();
            }
            else
            {
                ppGet.Credentials.Username = ViewState["PayflowAccount"].ToString();
                ppGet.Credentials.Password = ViewState["PayflowPassword"].ToString();
                ppGet.Credentials.Signature = ViewState["PayflowSignature"].ToString();
            }

            if (ppGet.Post())
            {

                NvpDoExpressCheckoutPayment ppPay = new NvpDoExpressCheckoutPayment();
                ppPay.Add(NvpSetExpressCheckout.Request.EMAIL, EmailAddress);

                ppPay.Add(NvpDoExpressCheckoutPayment.Request._TOKEN, ppGet.Get(NvpGetExpressCheckoutDetails.Response.TOKEN));
                ppPay.Add(NvpDoExpressCheckoutPayment.Request._PAYERID, ppGet.Get(NvpGetExpressCheckoutDetails.Response.PAYERID));
                ppPay.Add(NvpDoExpressCheckoutPayment.Request._AMT, decimal.Parse(ppGet.Get(NvpGetExpressCheckoutDetails.Response.AMT)).ToString("f"));

                ppPay.Add(NvpDoExpressCheckoutPayment.Request._PAYMENTACTION, NvpPaymentActionCodeType.Sale.ToString());
                ppPay.Add(NvpSetExpressCheckout.Request.LANDINGPAGE, NvpLandingPageType.Billing);
                ppPay.Add(NvpSetExpressCheckout.Request.PAYMENTACTION, NvpPaymentActionCodeType.Sale);

                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTONAME, System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.FIRSTNAME)) + " " + System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.LASTNAME)));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET, System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTREET)));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET2, ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTREET2));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOCITY, System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOCITY)));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOZIP, System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOZIP)));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOSTATE, System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTATE)));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOCOUNTRYCODE, System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.COUNTRYCODE)));

                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOPHONENUM, System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.PHONENUM)));

                if (paidform.UseTestMode)
                {
                    ppPay.Environment = NvpEnvironment.Sandbox;
                    ppPay.Credentials.Username = WebConfigurationManager.AppSettings["PayflowAccount"].ToString();
                    ppPay.Credentials.Password = WebConfigurationManager.AppSettings["PayflowPassword"].ToString();
                    ppPay.Credentials.Signature = WebConfigurationManager.AppSettings["PayflowSignature"].ToString();
                }
                else
                {
                    ppPay.Credentials.Username = ViewState["PayflowAccount"].ToString();
                    ppPay.Credentials.Password = ViewState["PayflowPassword"].ToString();
                    ppPay.Credentials.Signature = ViewState["PayflowSignature"].ToString();
                }

                List<NvpPayItem> itemList = new List<NvpPayItem>();

                foreach (NvpPayItem npi in ppGet.LineItems)
                {
                    itemList.Add(npi);
                }

                ppPay.Add(itemList);

                if (ppPay.Post())
                {
                    //LName = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.LASTNAME));
                    //Address = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTREET));
                    //Address2 = ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTREET2);
                    //City = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOCITY));
                    //Zip = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOZIP));
                    //State = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTATE));

                    //Country = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.COUNTRYCODE));
                 
                    TranID = ppPay.Get(NvpGetTransactionDetails.Request.TRANSACTIONID);
                    AddToCache(TransactionCacheName, TranID);
                    lblMessage.Text = "Payment has been processed";

                    #region Post Data to ECN after successful CC transaction.

                    HttpBrowserCapabilities browser = Request.Browser;
                    string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                    foreach (KeyValuePair<int, Dictionary<string, string>> product in dPurchasedProducts)
                    {
                        int productID = product.Key;
                        Dictionary<string, string> dsubscriberdata = product.Value;

                        dsubscriberdata.Add("t_transactionid", TranID);

                        #region Send data to ECN (write also to HTTP POST)

                        try
                        {
                            ECNUtils.SubscribeToGroup(Convert.ToInt32(dsubscriberdata["c"]), Convert.ToInt32(dsubscriberdata["g"]), dsubscriberdata["publicationcode"], dsubscriberdata, browserInfo);
                        }
                        catch (Exception ex)
                        {
                            //send notification email if NXTbook API fails.
                            string emailMsg = "Error in Payment.aspx - Auto Subscribe Groups <br /><br />";

                            emailMsg += "Errorlocation : ECNUtils.SubscribeToGroup <br /><br />";

                            emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                            emailMsg += "formID :" + FormID + "<br /><br />";
                            emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                            emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                            emailMsg += "<b>Exception Details:</b>" + ex.Message;

                            emailMsg += "<b>Pubcode:</b>" + dsubscriberdata["publicationcode"];
                            emailMsg += "<b>Data:</b></b>" + string.Join(", ", dsubscriberdata.Select(x => string.Format("&{0}={1}", x.Key, x.Value)).ToArray());

                            EmailFunctions emailFunctions = new EmailFunctions();
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Paidforms - DB Update Failed in SubscribeToGroup method", emailMsg);
                        }

                        #endregion

                        #region NXTBook Posting
                        try
                        {
                            //Check for NXTBook Integration & Call NXTBookAPI.
                            if (bool.Parse(dsubscriberdata["isnxtbookapienabled"]))
                            {
                                NXTBook nxtbook = NXTBook.GetbyProductID(productID);

                                if (nxtbook != null)
                                {
                                    drmProfile dp = new NXTBookAPI.Entity.drmProfile();

                                    dp.subscriptionid = nxtbook.SubscriptionID;

                                    dp.update = true;
                                    dp.noupdate = false;
                                    dp.email = EmailAddress;
                                    dp.password = dsubscriberdata["usr1"].ToString();
                                    //dp.changepassword = "";
                                    dp.firstname = dsubscriberdata["firstname"].ToString();
                                    dp.lastname = dsubscriberdata["lastname"].ToString();
                                    dp.phone = dsubscriberdata["voice"].ToString();
                                    dp.address1 = dsubscriberdata["address"].ToString();
                                    dp.address2 = dsubscriberdata["address2"].ToString();
                                    dp.city = dsubscriberdata["city"].ToString();
                                    dp.country = dsubscriberdata["country"].ToString();
                                    dp.state = dsubscriberdata["state"].ToString();
                                    dp.zipcode = dsubscriberdata["zip"].ToString();
                                    dp.organization = dsubscriberdata["company"].ToString();
                                    dp.extraparams = "";

                                    dp.access_nbissues = "";
                                    dp.access_firstissue = "";
                                    //dp.access_limitdate = "";


                                    if (Convert.ToBoolean(dsubscriberdata["issubscription"]))
                                    {
                                        dp.access_type = "timerestricted";
                                        dp.access_startdate = NXTBook.GetRecentIssueDatebyPubcode(dsubscriberdata["pubcode"].ToString()).ToString("yyyy-MM-dd");
                                        dp.access_enddate = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        dp.access_type = "single";
                                        //public List<drmBook> access_issues
                                        drmBook dbook = new drmBook();

                                        //dbook.bookid = nxtbook.BookID;
                                        dbook.url = nxtbook.BookURL + (nxtbook.BookURL.EndsWith("/") ? nxtbook.BookID : "/" + nxtbook.BookID);

                                        dp.access_issues = new List<drmBook>();

                                        dp.access_issues.Add(dbook);
                                    }


                                    drmRESTAPI.SetProfile(nxtbook.APIKey, dp);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {

                                //send notification email if NXTbook API fails.
                                string emailMsg = "Error send data to NXTBOOK <br /><br />";
                                emailMsg += " Check TrackHTTPPost table for data.";

                                emailMsg += "emailaddress :" + EmailAddress;
                                emailMsg += " Group ID:" + dsubscriberdata["g"] + "Cust ID:" + dsubscriberdata["c"];
                                emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                                emailMsg += "<b>Exception Details:</b>" + ex.Message;

                                EmailFunctions emailFunctions = new EmailFunctions();
                                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal Transaction Response-FAIL", emailMsg);

                                throw ex;
                            }
                            catch
                            {
                            }
                        }
                        #endregion

                        #region 3rd Party posting for each purchased product

                        try
                        {
                            DataTable dt = new DataTable();
                            dt = HttpPost.getPaidFormProductHttpPostParams(productID);

                            if (dt.Rows.Count > 0)
                                buildParamsAndPost(dt, dsubscriberdata);
                        }
                        catch (Exception ex)
                        {
                            //send notification email if NXTbook API fails.
                            string emailMsg = "Error in payment.aspx - 3rd Party posting <br /><br />";

                            emailMsg += "Errorlocation : 3rd Party posting <br /><br />";

                            emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                            emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                            emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                            emailMsg += "<b>Exception Details:</b>" + ex.Message;

                            emailMsg += "<b>Pubcode:</b>" + dsubscriberdata["publicationcode"];
                            emailMsg += "<b>Data:</b></b>" + string.Join(", ", dsubscriberdata.Select(x => string.Format("&{0}={1}", x.Key, x.Value)).ToArray());

                            EmailFunctions emailFunctions = new EmailFunctions();
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                        }

                        #endregion

                        #region  Auto Subscribe Groups

                        try
                        {
                            //auto subscribe to each group
                            string profileString = BuildProfile(dsubscriberdata).ToString();

                            if (!AutoSubpubCodes.Contains(dsubscriberdata["publicationcode"]))
                            {
                                AutoSubpubCodes.Add(dsubscriberdata["publicationcode"]);
                                Dictionary<int, int> AutoGroups = new Dictionary<int, int>();

                                Publication p = Publication.GetPublicationbyID(0, dsubscriberdata["publicationcode"]);
                                AutoGroups = Publication.GetAutoSubscriptions(p.PubID);

                                foreach (KeyValuePair<int, int> kvp in AutoGroups)
                                {
                                    string profile = profileString + "&g=" + kvp.Key.ToString() + "&c=" + kvp.Value.ToString() + "&sfID=&s=S" + BuildStandalone(p.ECNDefaultGroupID);
                                    ECNUtils.ECNHttpPost(EmailAddress, dsubscriberdata["publicationcode"], profile.ToString(), browserInfo);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            //send notification email if NXTbook API fails.
                            string emailMsg = "Error in payment.aspx - Auto Subscribe Groups <br /><br />";

                            emailMsg += "Errorlocation : Auto Subscribe Groups <br /><br />";

                            emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                            emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                            emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                            emailMsg += "<b>Exception Details:</b>" + ex.Message;

                            emailMsg += "<b>Pubcode:</b>" + dsubscriberdata["publicationcode"];
                            emailMsg += "<b>Data:</b></b>" + string.Join(", ", dsubscriberdata.Select(x => string.Format("&{0}={1}", x.Key, x.Value)).ToArray());

                            EmailFunctions emailFunctions = new EmailFunctions();
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                        }

                        #endregion

                    }

                    #endregion

                    if (paidform.FormRedirect != null && paidform.FormRedirect != string.Empty)
                    {
                        string redirectURL = paidform.FormRedirect;
                        if (!redirectURL.Contains("http://") && !redirectURL.Contains("https://"))
                            redirectURL = "http://" + redirectURL;
                        Response.Redirect(redirectURL);
                    }
                    else
                    {
                        Response.Redirect("thankyou.aspx?formid=" + FormID + "&e=" + EmailAddress.Replace("+", "%2B"));
                    }
                }
                else
                {
                    //ppSet post failed
                    lblMessage.Text = "Payment was not successful";
                    lblMessage.Visible = true;
                    return;
                }

            }
            else
            {
                //ppSet post failed
                lblMessage.Text = "Payment was not successful";
                lblMessage.Visible = true;
                return;
            }
        }

        private void buildParamsAndPost(DataTable dtPostParams, Dictionary<string, string> dSubscribedata)
        {
            string postURL = string.Empty;
            StringBuilder postparams = new StringBuilder();

            string paramvalue = string.Empty;

            if (dtPostParams.Rows.Count > 0)
            {
                postURL = dtPostParams.Rows[0]["URL"].ToString();
                foreach (DataRow dr in dtPostParams.Rows)
                {
                    paramvalue = string.Empty;

                    try
                    {
                        if (!dr["ParamValue"].ToString().Equals("CustomValue"))
                        {
                            if (dSubscribedata.ContainsKey(dr["ParamValue"].ToString().ToLower()))
                            {
                                paramvalue = dSubscribedata[dr["ParamValue"].ToString().ToLower()];

                                if (paramvalue != string.Empty && !dr.IsNull("DataType") && dr["DataType"].ToString().ToUpper() == "DATETIME" && !dr.IsNull("DataFormat") && dr["DataFormat"].ToString() != string.Empty)
                                {
                                    paramvalue = Convert.ToDateTime(paramvalue).ToString(dr["DataFormat"].ToString());
                                }

                                postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(paramvalue));
                            }
                            else
                            {
                                postparams.Append("&" + dr["ParamName"].ToString() + "=");
                            }
                        }
                        else
                        {
                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(dr["CustomValue"].ToString()));
                        }
                    }
                    catch
                    {
                    }
                }
                Utilities.ExternalHttpPost(postURL + "?" + postparams.ToString().TrimStart('&'));
            }

        }


        private StringBuilder BuildProfile(Dictionary<string, string> dsubscriberdata )
        {
            StringBuilder profile = new StringBuilder();
            profile.Append("fn=" + dsubscriberdata["firstname"].ToString());
            profile.Append("&ln=" + dsubscriberdata["lastname"].ToString());
            profile.Append("&compname=" + dsubscriberdata["company"].ToString());
            profile.Append("&adr=" + dsubscriberdata["address"].ToString());
            profile.Append("&adr2=" + dsubscriberdata["address2"].ToString());
            profile.Append("&city=" + dsubscriberdata["city"].ToString());
            profile.Append("&state=" + dsubscriberdata["state"].ToString());

            profile.Append("&zc=" + dsubscriberdata["zip"].ToString());
            profile.Append("&ctry=" + dsubscriberdata["country"].ToString());
            profile.Append("&ph=" + dsubscriberdata["voice"].ToString());
            profile.Append("&fax=" + dsubscriberdata["fax"].ToString());
            profile.Append("&e=" + dsubscriberdata["emailaddress"].ToString());
            profile.Append("&f=html");
            return profile;
        }

        private string BuildStandalone(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = @"SELECT	gdf.ShortName, edv.DataValue 
                            FROM EmailDataValues edv WITH(NOLOCK)  
                            join  GroupDatafields gdf WITH(NOLOCK)  on edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
                            JOIN Emails e with(nolock) on edv.EmailID = e.EmailID
                            WHERE e.EmailAddress = @emailAddress and gdf.GroupID = @groupID and isnull(gdf.DatafieldSetID,0) = 0";

            cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@emailAddress", EmailAddress);
            cmd.Parameters.AddWithValue("@GroupID", groupID.ToString());

            DataTable dtUDFs = DataFunctions.GetDataTable("communicator", cmd);
            StringBuilder sbUdfs = new StringBuilder();
            if (dtUDFs != null && dtUDFs.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUDFs.Rows)
                {
                    sbUdfs.Append("&" + "user_" + dr["ShortName"].ToString() + "=" + Server.UrlEncode(dr["DataValue"].ToString()));
                    //dProfile.Add(dr["ShortName"].ToString().ToUpper(), dr["DataValue"].ToString());
                }
            }

            return sbUdfs.ToString();
        }

        private DataTable GetUDFValues(int groupID)
        {
            string sql = @"SELECT	gdf.ShortName, edv.DataValue 
                            FROM EmailDataValues edv WITH(NOLOCK)  
                            join  GroupDatafields gdf WITH(NOLOCK)  on edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
                            join Emails e with(nolock) on edv.EmailID = e.emailID
                            WHERE e.EmailAddress = @emailAddress and gdf.GroupID = @groupID and isnull(gdf.DatafieldSetID,0) = 0";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@emailAddress", EmailAddress);
            cmd.Parameters.AddWithValue("@GroupID", groupID.ToString());

            DataTable dtUDFs = DataFunctions.GetDataTable("communicator", cmd);

            return dtUDFs;
        }
    }
}