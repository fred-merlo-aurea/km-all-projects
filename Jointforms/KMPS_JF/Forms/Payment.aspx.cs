using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Encore.PayPal.Nvp;
using KMPS_JF_Objects.Objects;
using KMPS_JF_Objects.Controls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace KMPS_JF.Forms
{
    public partial class Payment : System.Web.UI.Page
    {
        private static Publication pub = null;
        private static PubForm pf = null;
        private static int PubID = -1;
        private int GroupID = -1;
        private string ECNPostFormat = string.Empty;
        private Hashtable ECNPosthProfileFields = new Hashtable();
        private Hashtable ECNPosthUDFFields = new Hashtable();
        Hashtable hECNPostParams = new Hashtable();
        private string FName = string.Empty;
        private string LName = string.Empty;
        private string Token = string.Empty;
        private string Address = string.Empty;
        private string Address2 = string.Empty;
        private string Zip = string.Empty;
        private string State = string.Empty;
        private string City = string.Empty;
        private string Country = string.Empty;
        private string AmountPaid = string.Empty;
        private string timestamp = string.Empty;
        private string TranID = string.Empty;
        private string emailaddress = string.Empty;
        private DateTime newDate = new DateTime();

        //private int ECNPostCustomerID = 0;
        //private int ECNPostGroupID = 0;
        //private int ECNPostSmartFormID = 0;
        //private int ECNPostEmailID = 0;
        //private int ECNPostBlastID = 0;

        //private string ECNPostEmailAddress = "";
        //private string ECNPostSubscribe = "";

        //private string ECNPostReturnURL = "";
        //private string ECNPostfromURL = "";
        //private string ECNPostfromIP = "";

        //private string ECNPostResponse_FromEmail = "";
        //private string ECNPostResponse_UserMsgSubject = "";
        //private string ECNPostResponse_UserMsgBody = "";
        //private string ECNPostResponse_UserScreen = "";
        //private string ECNPostResponse_AdminEmail = "";
        //private string ECNPostResponse_AdminMsgSubject = "";
        //private string ECNPostResponse_AdminMsgBody = "";

        //private Groups ECNPostgroup;


        private static string getQueryString(string qs)
        {
            try { return HttpContext.Current.Request.QueryString[qs].ToString(); }
            catch { return string.Empty; }
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

        private string GetCookieValue(string cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {

                return Request.Cookies[cookieName].Value.ToString();

            }
            return "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

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
                pub = Publication.GetPublicationbyID(PubID, PubCode);
                GroupID = pub.ECNDefaultGroupID;

                SetupPage();
                if (Request.QueryString.Get("token") != null)
                {

                    ProcessPaypalPayment(Request.QueryString.Get("token"));

                }
                else
                {
                    //No token in the query string
                    lblMessage.Text = "Payment was not successful";
                    lblMessage.Visible = true;

                    return;
                }
            }
            catch 
            {

            }
        }


        private void ProcessPaypalPayment(string token)
        {

            string PostParams = string.Empty;
            string ProfileParams = string.Empty;
            string DemographicParams = string.Empty;
            string groupParams = string.Empty;
            string PaidParams = string.Empty;
            //bool isPrintOverwritten = false;




            string sessionString = GetCookieValue("DemographicFields");


            string[] qStringPairs = sessionString.Split('&');
            Dictionary<string, string> DemographicFields = new Dictionary<string, string>();
            foreach (string s in qStringPairs)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string[] valuePairs = s.Split('=');
                    string key = valuePairs[0];
                    string value = valuePairs[1];
                    if (!DemographicFields.ContainsKey(key))
                    {
                        DemographicFields.Add(key, value);
                    }
                    else
                    {
                        DemographicFields[key] = value;
                    }
                }
            }

            NvpGetExpressCheckoutDetails ppGet = new NvpGetExpressCheckoutDetails();
            ppGet.Add(NvpGetExpressCheckoutDetails.Request.TOKEN, Request.QueryString.Get("token"));
            ppGet.Credentials.Username = pub.PayflowAccount;
            ppGet.Credentials.Password = pub.PayflowPassword;
            ppGet.Credentials.Signature = pub.PayflowSignature;

            if (ppGet.Post())
            {

                //PostParams = "?user_publicationcode=" + Server.UrlEncode(PubCode) + "&f=html&s=S" + "&e=" + Request.QueryString.Get("emailaddress");

                //if (!string.IsNullOrEmpty(getQueryString("password")))
                //    PostParams += "&password=" + Server.UrlEncode(getQueryString("password"));

                //groupParams += "&g=" + Server.UrlEncode(pub.ECNDefaultGroupID.ToString()) + "&sfID=" + Server.UrlEncode(pub.ECNSFID.ToString());

                DataTable subscriber = GetSubscriber(getQueryString("emailaddress"));

                PubCountry pc = pub.GetPubCountry(subscriber.Rows[0]["Country"].ToString());
                if (pc != null)
                    pf = PubForm.GetPubForm(pc.PFID);
                NvpDoExpressCheckoutPayment ppPay = new NvpDoExpressCheckoutPayment();
                ppPay.Add(NvpDoExpressCheckoutPayment.Request._TOKEN, ppGet.Get(NvpGetExpressCheckoutDetails.Response.TOKEN));
                ppPay.Add(NvpDoExpressCheckoutPayment.Request._PAYERID, ppGet.Get(NvpGetExpressCheckoutDetails.Response.PAYERID));
                ppPay.Add(NvpDoExpressCheckoutPayment.Request._AMT, decimal.Parse(ppGet.Get(NvpGetExpressCheckoutDetails.Response.AMT)).ToString("f"));
                ppPay.Add(NvpDoExpressCheckoutPayment.Request._PAYMENTACTION, NvpPaymentActionCodeType.Sale.ToString());
                ppPay.Add(NvpSetExpressCheckout.Request.LANDINGPAGE, NvpLandingPageType.Billing);
                ppPay.Add(NvpSetExpressCheckout.Request.PAYMENTACTION, NvpPaymentActionCodeType.Sale);
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTONAME, System.Web.HttpUtility.UrlEncode(subscriber.Rows[0]["FullName"].ToString() + " " + subscriber.Rows[0]["LastName"].ToString()));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOCITY, System.Web.HttpUtility.UrlEncode(subscriber.Rows[0]["City"].ToString()));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOZIP, System.Web.HttpUtility.UrlEncode(subscriber.Rows[0]["Zip"].ToString()));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET, System.Web.HttpUtility.UrlEncode(subscriber.Rows[0]["Address"].ToString()));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET2, System.Web.HttpUtility.UrlEncode(subscriber.Rows[0]["Address2"].ToString()));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOSTATE, System.Web.HttpUtility.UrlEncode(subscriber.Rows[0]["State"].ToString()));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOCOUNTRYCODE, pc != null ? pc.CountryCode : "");
                ppPay.Add(NvpSetExpressCheckout.Request.EMAIL, getQueryString("emailaddress"));
                ppPay.Add(NvpSetExpressCheckout.Request.SHIPTOPHONENUM, System.Web.HttpUtility.UrlEncode(subscriber.Rows[0]["Voice"].ToString()));
                ppPay.Credentials.Username = pub.PayflowAccount;
                ppPay.Credentials.Password = pub.PayflowPassword;
                ppPay.Credentials.Signature = pub.PayflowSignature;

                List<NvpPayItem> itemList = new List<NvpPayItem>();
                NvpPayItem item = new NvpPayItem();
                item.Name = pub.PubCode + " Subscription";
                //item.Description = "item.Description";
                //item.Quantity = "1";
                item.Amount = decimal.Parse(pf.PaidPrice.ToString()).ToString("f");
                //item.Tax = decimal.Parse("0.00").ToString("f");
                itemList.Add(item);

                ppPay.Add(itemList);


                if (ppPay.Post())
                {
                    FName = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.FIRSTNAME));
                    LName = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.LASTNAME));
                    Token = ppGet.Get(NvpGetExpressCheckoutDetails.Response.TOKEN);
                    Address = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTREET));
                    Address2 = ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTREET2);
                    Zip = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOZIP));
                    State = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOSTATE));
                    City = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.SHIPTOCITY));
                    Country = System.Web.HttpUtility.UrlDecode(ppGet.Get(NvpGetExpressCheckoutDetails.Response.COUNTRYCODE));
                    AmountPaid = ppGet.Get(NvpGetExpressCheckoutDetails.Response.AMT);
                    timestamp = ppPay.Get("TIMESTAMP");

                    DateTime.TryParse(timestamp, out newDate);
                    TranID = ppPay.Get(NvpGetTransactionDetails.Request.TRANSACTIONID);
                    emailaddress = ppGet.Get(NvpGetExpressCheckoutDetails.Response.EMAIL);

                    lblMessage.Text = "Payment has been processed";

                    List<string> lPubcodes = new List<string>();

                    foreach (NvpPayItem npi in ppGet.LineItems)
                    {
                        try
                        {
                            UpdatePAIDorFREE(pub.ECNDefaultGroupID);

                            if (!lPubcodes.Contains(pub.PubCode))
                            {
                                lPubcodes.Add(pub.PubCode);

                                HttpPostAutoSubscription(pub.PubCode, subscriber, DemographicFields);

                            }

                        }
                        catch (Exception ex)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("Error when updating email profile in Payment Confirmation for Paypal Redirect");
                            sb.AppendLine("Email:" + emailaddress);
                            sb.AppendLine("FName:" + FName);
                            sb.AppendLine("LName:" + LName);
                            sb.AppendLine("Token:" + Token);
                            sb.AppendLine("Country:" + Country);
                            sb.AppendLine("AmountPaid:" + AmountPaid.ToString());
                            sb.AppendLine("TranID:" + TranID.ToString());
                            foreach (NvpPayItem nvp in itemList)
                            {
                                sb.AppendLine("ItemName:" + nvp.Name);
                                sb.AppendLine("Price:" + nvp.Amount);
                            }
                            sb.AppendLine("");

                            sb.AppendLine(ex.StackTrace);

                            KMPS_JF_Objects.Objects.Utilities.SendMail(sb.ToString());
                        }
                    }



                    if (pub.ThankYouPageLink.Length > 0 && (pub.ThankYouPageLink.ToUpper().StartsWith("HTTP:") || pub.ThankYouPageLink.ToUpper().StartsWith("HTTPS:")))
                    {

                        if (Request.QueryString["rURL"] != null && !string.IsNullOrEmpty(Request.QueryString["rURL"]))
                        {
                            Response.Redirect(Request.QueryString["rURL"].ToString() + "?pubcode=" + PubCode + "&emailaddress=" + getQueryString("emailaddress"));
                        }
                        else
                        {
                            RedirecToThankYouLink();
                        }
                    }
                    else
                    {
                        if (Request.QueryString["rURL"] != null && !string.IsNullOrEmpty(Request.QueryString["rURL"]))
                        {
                            Response.Redirect(Request.QueryString["rURL"].ToString() + "?pubcode=" + PubCode + "&emailaddress=" + getQueryString("emailaddress"));
                        }
                        else
                        {
                            string query = Request.RawUrl.Substring(Request.RawUrl.IndexOf('?') + 1);
                            string sThankYoulink = "ThankYou.aspx?" + query;
                            Response.Redirect(sThankYoulink, true);
                        }
                    }
                    hlBackToForm.Visible = false;
                }
                else
                {
                    //ppSet post failed
                    lblMessage.Text = "Payment was not successful";
                    lblMessage.Visible = true;
                    string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;
                    hlBackToForm.NavigateUrl = basePath + "/Forms/Subscription.aspx" + Request.Url.Query.ToString() + "&step=form";
                    return;
                }
            }
            else
            {
                //ppSet post failed
                lblMessage.Text = "Payment was not successful";
                lblMessage.Visible = true;
                string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;
                hlBackToForm.NavigateUrl = basePath + "/Forms/Subscription.aspx" + Request.Url.Query.ToString() + "&step=form";
                return;
            }
        }



        private void RedirecToThankYouLink()
        {
            string link = pub.ThankYouPageLink;
            try
            {
                Regex r = new Regex("&");
                StringBuilder sb = new StringBuilder();
                Array codesnippets = r.Split(link.ToLower());
                DataTable subscriber = GetSubscriber(getQueryString("emailaddress"));
                if (subscriber.Rows[0] != null)
                {
                    link = link.Replace("%%e%%", getQueryString("emailaddress"));
                    link = link.Replace("%%ctry%%", subscriber.Rows[0]["Country"].ToString());
                    link = link.Replace("%%fn%%", subscriber.Rows[0]["FirstName"].ToString());
                    link = link.Replace("%%ln%%", subscriber.Rows[0]["LastName"].ToString());
                    link = link.Replace("%%n%%", subscriber.Rows[0]["FullName"].ToString());
                    link = link.Replace("%%compname%%", subscriber.Rows[0]["Company"].ToString());
                    link = link.Replace("%%t%%", subscriber.Rows[0]["Title"].ToString());
                    link = link.Replace("%%occ%%", subscriber.Rows[0]["Occupation"].ToString());
                    link = link.Replace("%%adr%%", subscriber.Rows[0]["Address"].ToString());
                    link = link.Replace("%%adr2%%", subscriber.Rows[0]["Address2"].ToString());
                    link = link.Replace("%%city%%", subscriber.Rows[0]["City"].ToString());
                    link = link.Replace("%%state%%", subscriber.Rows[0]["State"].ToString());
                    //link = link.Replace("%%stateint%%", Server.UrlEncode(SubscriberResponse.ContainsKey("STATE_INT") ? SubscriberResponse["STATE_INT"] : string.Empty));
                    link = link.Replace("%%zc%%", subscriber.Rows[0]["Zip"].ToString());
                    //link = link.Replace("%%zcfor%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FORZIP") ? SubscriberResponse["FORZIP"] : string.Empty));
                    link = link.Replace("%%ph%%", subscriber.Rows[0]["Voice"].ToString());
                    link = link.Replace("%%mph%%", subscriber.Rows[0]["Mobile"].ToString());
                    link = link.Replace("%%fax%%", subscriber.Rows[0]["Fax"].ToString());
                    link = link.Replace("%%website%%", subscriber.Rows[0]["Website"].ToString());
                    link = link.Replace("%%age%%", subscriber.Rows[0]["Age"].ToString());
                    link = link.Replace("%%income%%", subscriber.Rows[0]["Income"].ToString());
                    link = link.Replace("%%gndr%%", subscriber.Rows[0]["Gender"].ToString());
                    link = link.Replace("%%bdt%%", subscriber.Rows[0]["Birthdate"].ToString());
                    link = link.Replace("%%ip%%", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());
                    link = link.Replace("%%wlp%%", HttpContext.Current.Server.UrlEncode("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + ResolveUrl("~/Forms/ThankYou.aspx?") + HttpContext.Current.Request.QueryString.ToString()));

                    //Dictionary<string, string> subsUDF = new Dictionary<string, string>();
                    //for (int i = 0; i < codesnippets.Length; i++)
                    //{
                    //    string key = codesnippets.GetValue(i).ToString().Split('=')[1].Trim('%');

                    //    if (SubscriberResponse.ContainsKey(key.ToUpper()))
                    //        subsUDF.Add(key, SubscriberResponse[key.ToUpper()]);
                    //}

                    //foreach (KeyValuePair<string, string> kvp in subsUDF)
                    //    link = link.Replace("%%" + kvp.Key + "%%", kvp.Value);

                }
                HttpContext.Current.Response.Redirect(link);
            }
            catch { HttpContext.Current.Response.Redirect(link); }
        }

        private static DataTable GetSubscriber(string email)
        {
            SqlCommand cmd = new SqlCommand("sp_subscriberLogin");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            cmd.Parameters["@GroupID"].Value = pub.ECNDefaultGroupID;

            cmd.Parameters.Add(new SqlParameter("@subscriberID", SqlDbType.VarChar, 50));
            cmd.Parameters["@subscriberID"].Value = email;

            cmd.Parameters.Add(new SqlParameter("@filter", SqlDbType.Char, 1));
            cmd.Parameters["@filter"].Value = "D";

            cmd.Parameters.Add(new SqlParameter("@filterval", SqlDbType.VarChar, 10));
            cmd.Parameters["@filterval"].Value = "";

            DataTable dtEmails = DataFunctions.GetDataTable("communicator", cmd);

            return dtEmails;
        }

        //private void getPubForm()
        //{
        //    try
        //    {
        //        if ((PubFormID == 0 || pf == null) && CountryID > 0)
        //        {
        //            pc = pub.GetPubCountry(CountryID);

        //            if (pc != null)
        //            {
        //                pf = PubForm.GetPubForm(pc.PFID);
        //                PubFormID = pf.PFID;
        //            }
        //        }
        //    }
        //    catch (Exception e) { }
        //}

        private void UpdatePAIDorFREE(int groupID)
        {
            //do http post tracking
            try
            {
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                SqlCommand cmdHttpPost = new SqlCommand("insert into TrackHttpPost (EmailAddress, Pubcode, PostData, BrowserInfo) values ( @EmailAddress, @PubCode, @PostParams, @BrowserInfo)");
                cmdHttpPost.CommandType = CommandType.Text;
                cmdHttpPost.Parameters.AddWithValue("@EmailAddress", emailaddress);
                cmdHttpPost.Parameters.AddWithValue("@PubCode", PubCode);
                cmdHttpPost.Parameters.AddWithValue("@PostParams", Request.QueryString.ToString().TrimStart('?'));
                cmdHttpPost.Parameters.AddWithValue("@BrowserInfo", browserInfo);
                DataFunctions.Execute(cmdHttpPost);
            }
            catch
            { }

            KMPlatform.Entity.User User = null;

            int ECNUserID = 0;

            try
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

                ECNUserID = User.UserID;
            }
            catch
            {

            }



            StringBuilder xmlProfile = new StringBuilder("");
            StringBuilder xmlUDF = new StringBuilder();

            xmlProfile.Append("<Emails>");
            xmlProfile.Append("<emailaddress>" + emailaddress + "</emailaddress>");
            xmlProfile.Append("<firstname>" + FName + "</firstname>");
            xmlProfile.Append("<lastname>" + LName + "</lastname>");
            xmlProfile.Append("<country>" + Country + "</country>");
            xmlProfile.Append("<address>" + Address + "</address>");
            xmlProfile.Append("<address2>" + Address2 + "</address2>");
            xmlProfile.Append("<state>" + State + "</state>");
            xmlProfile.Append("<zip>" + Zip + "</zip>");
            xmlProfile.Append("<city>" + City + "</city>");

            //xmlProfile.Append("<user_PAIDorFREE>PAID</user_PAIDorFREE>");
            xmlProfile.Append("</Emails>");

            ECNPosthUDFFields = GetGroupDataFields(groupID);

            if (ECNPosthUDFFields.Count > 0)
            {
                xmlUDF.Append("<row>");
                xmlUDF.Append("<ea>" + CleanXMLString(getQueryString("emailaddress")) + "</ea>");

                IDictionaryEnumerator en1 = ECNPosthUDFFields.GetEnumerator();
                while (en1.MoveNext())
                {
                    try
                    {
                        if (en1.Value.ToString().ToLower().Equals("user_paidorfree"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>PAID</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_paymentstatus"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>PAID</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_transactionid"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + TranID + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_amountpaid"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + AmountPaid + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_country"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + Country + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_lastname"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + LName + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_firstname"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + FName + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_transdate"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + newDate.ToString() + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_street"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + Address.ToString() + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_street2"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + Address2.ToString() + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_city"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + City.ToString() + "</v></udf>");
                        }
                        else if (en1.Value.ToString().ToLower().Equals("user_t_zip"))
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + Zip.ToString() + "</v></udf>");
                        }
                    }
                    catch
                    { }
                }
                xmlUDF.Append("</row>");
            }

            //SqlCommand cmd = new SqlCommand("sp_importEmails");
            SqlCommand cmd = new SqlCommand("e_EmailGroup_ImportEmails");
            try
            {

                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar);
                cmd.Parameters["@CustomerID"].Value = pub.ECNCustomerID;

                cmd.Parameters.Add("@GroupID", SqlDbType.VarChar);
                cmd.Parameters["@GroupID"].Value = pub.ECNDefaultGroupID;

                cmd.Parameters.Add("@xmlProfile", SqlDbType.Text);
                cmd.Parameters["@xmlProfile"].Value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString().Replace("%23", "#") + "</XML>";

                cmd.Parameters.Add("@xmlUDF", SqlDbType.Text);
                cmd.Parameters["@xmlUDF"].Value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>";

                cmd.Parameters.Add("@formattypecode", SqlDbType.VarChar);
                cmd.Parameters["@formattypecode"].Value = "HTML";

                cmd.Parameters.Add("@subscribetypecode", SqlDbType.VarChar);
                cmd.Parameters["@subscribetypecode"].Value = "S";

                cmd.Parameters.Add("@EmailAddressOnly", SqlDbType.Bit);
                cmd.Parameters["@EmailAddressOnly"].Value = 0;

                cmd.Parameters.Add("@UserID", SqlDbType.VarChar);
                cmd.Parameters["@UserID"].Value = ECNUserID;

                cmd.Parameters.Add("@source", SqlDbType.VarChar);
                cmd.Parameters["@source"].Value = "KMPS_JF.Payment.UpdatePAIDorFREE method";

                cmd.Parameters.Add("@insertMS", SqlDbType.Bit);
                cmd.Parameters["@insertMS"].Value = "true";

                DataFunctions.Execute("communicator", cmd);

            }
            catch (Exception ex)
            {
                SendNotification(ConfigurationManager.AppSettings["JFsFromEmail"].ToString(), ConfigurationManager.AppSettings["JFsFromName"].ToString(), "sunil@teamkm.com,bill.hipps@teamkm.com,justin.welter@teamkm.com", "DB Update Failed in UpdateToDB method", ex.Message + "<br/>" + xmlProfile.ToString() + "<br/>" + xmlUDF.ToString());
            }
            finally
            {
                cmd.Dispose();
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
        //Load all UDF Fields for the group
        private Hashtable GetGroupDataFields(int groupID)
        {
            SqlCommand cmdsqlstmt = new SqlCommand("SELECT * FROM GroupDatafields WHERE GroupID = @groupID");
            cmdsqlstmt.CommandType = CommandType.Text;
            cmdsqlstmt.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int)).Value = groupID;

            //string sqlstmt = " SELECT * FROM GroupDatafields WHERE GroupID=" + groupID;
            DataTable emailstable = DataFunctions.GetDataTable("communicator", cmdsqlstmt);

            Hashtable fields = new Hashtable();
            foreach (DataRow dr in emailstable.Rows)
                fields.Add(Convert.ToInt32(dr["GroupDataFieldsID"]), "user_" + dr["ShortName"].ToString().ToLower());

            return fields;
        }


        //private void RedirecToThankYouLink()
        //{
        //    string link = pub.ThankYouPageLink;
        //    try
        //    {
        //        Regex r = new Regex("&");
        //        StringBuilder sb = new StringBuilder();
        //        Array codesnippets = r.Split(link.ToLower());

        //        link = link.Replace("%%e%%", Server.UrlEncode(txtemailaddress.Text));
        //        link = link.Replace("%%ctry%%", Server.UrlEncode(drpCountry.SelectedItem.Text));
        //        link = link.Replace("%%fn%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FIRSTNAME") ? SubscriberResponse["FIRSTNAME"] : string.Empty));
        //        link = link.Replace("%%ln%%", Server.UrlEncode(SubscriberResponse.ContainsKey("LASTNAME") ? SubscriberResponse["LASTNAME"] : string.Empty));
        //        link = link.Replace("%%n%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FULLNAME") ? SubscriberResponse["FULLNAME"] : string.Empty));
        //        link = link.Replace("%%compname%%", Server.UrlEncode(SubscriberResponse.ContainsKey("COMPANY") ? SubscriberResponse["COMPANY"] : string.Empty));
        //        link = link.Replace("%%t%%", Server.UrlEncode(SubscriberResponse.ContainsKey("TITLE") ? SubscriberResponse["TITLE"] : string.Empty));
        //        link = link.Replace("%%occ%%", Server.UrlEncode(SubscriberResponse.ContainsKey("OCCUPATION") ? SubscriberResponse["OCCUPATION"] : string.Empty));
        //        link = link.Replace("%%adr%%", Server.UrlEncode(SubscriberResponse.ContainsKey("ADDRESS") ? SubscriberResponse["ADDRESS"] : string.Empty));
        //        link = link.Replace("%%adr2%%", Server.UrlEncode(SubscriberResponse.ContainsKey("ADDRESS2") ? SubscriberResponse["ADDRESS2"] : string.Empty));
        //        link = link.Replace("%%city%%", Server.UrlEncode(SubscriberResponse.ContainsKey("CITY") ? SubscriberResponse["CITY"] : string.Empty));
        //        link = link.Replace("%%state%%", Server.UrlEncode(SubscriberResponse.ContainsKey("STATE") ? SubscriberResponse["STATE"] : string.Empty));
        //        link = link.Replace("%%stateint%%", Server.UrlEncode(SubscriberResponse.ContainsKey("STATE_INT") ? SubscriberResponse["STATE_INT"] : string.Empty));
        //        link = link.Replace("%%zc%%", Server.UrlEncode(SubscriberResponse.ContainsKey("ZIP") ? SubscriberResponse["ZIP"] : string.Empty));
        //        link = link.Replace("%%zcfor%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FORZIP") ? SubscriberResponse["FORZIP"] : string.Empty));
        //        link = link.Replace("%%ph%%", Server.UrlEncode(SubscriberResponse.ContainsKey("VOICE") ? SubscriberResponse["VOICE"] : string.Empty));
        //        link = link.Replace("%%mph%%", Server.UrlEncode(SubscriberResponse.ContainsKey("MOBILE") ? SubscriberResponse["MOBILE"] : string.Empty));
        //        link = link.Replace("%%fax%%", Server.UrlEncode(SubscriberResponse.ContainsKey("FAX") ? SubscriberResponse["FAX"] : string.Empty));
        //        link = link.Replace("%%website%%", Server.UrlEncode(SubscriberResponse.ContainsKey("WEBSITE") ? SubscriberResponse["WEBSITE"] : string.Empty));
        //        link = link.Replace("%%age%%", Server.UrlEncode(SubscriberResponse.ContainsKey("AGE") ? SubscriberResponse["AGE"] : string.Empty));
        //        link = link.Replace("%%income%%", Server.UrlEncode(SubscriberResponse.ContainsKey("INCOME") ? SubscriberResponse["INCOME"] : string.Empty));
        //        link = link.Replace("%%gndr%%", Server.UrlEncode(SubscriberResponse.ContainsKey("GENDER") ? SubscriberResponse["GENDER"] : string.Empty));
        //        link = link.Replace("%%bdt%%", Server.UrlEncode(SubscriberResponse.ContainsKey("BIRTHDATE") ? SubscriberResponse["BIRTHDATE"] : string.Empty));
        //        link = link.Replace("%%ip%%", Request.ServerVariables["REMOTE_ADDR"].ToString());
        //        link = link.Replace("%%wlp%%", Server.UrlEncode("http://" + Request.ServerVariables["HTTP_HOST"] + ResolveUrl("~/Forms/ThankYou.aspx?") + Request.QueryString.ToString()));

        //        Dictionary<string, string> subsUDF = new Dictionary<string, string>();
        //        for (int i = 0; i < codesnippets.Length; i++)
        //        {
        //            string key = codesnippets.GetValue(i).ToString().Split('=')[1].Trim('%');

        //            if (SubscriberResponse.ContainsKey(key.ToUpper()))
        //                subsUDF.Add(key, SubscriberResponse[key.ToUpper()]);
        //        }

        //        foreach (KeyValuePair<string, string> kvp in subsUDF)
        //            link = link.Replace("%%" + kvp.Key + "%%", kvp.Value);

        //        Response.Redirect(link);
        //    }
        //    catch
        //    {
        //        Response.Redirect(link);
        //    }
        //}


        private void SetupPage()
        {
            if (pub != null)
            {
                if (pub.PageTitle.Trim().Length > 0)
                    Page.Title = pub.PageTitle;
                else
                    Page.Title = pub.PubName + " Subscription Form";

                divcss.InnerHtml = pub.GetCSS();

                phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML));
                phFooter.Controls.Add(new LiteralControl(pub.FooterHTML));
            }
        }

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

        private void LoadECNInputFields(string postparams)
        {
            try
            {
                hECNPostParams.Clear();
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
            }
            catch  { }
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
                    simpleMail.Body = Body;
                    simpleMail.IsBodyHtml = true;
                    simpleMail.Priority = System.Net.Mail.MailPriority.Normal;

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
                    smtpclient.Send(simpleMail);
                }
            }
            catch 
            { }
        }

        private void HttpPostAutoSubscription(string pubcode, DataTable subscriber, Dictionary<string, string> Demographics)
        {

            int AutoSubGroupID = -1;
            try
            {
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);
                Publication pub = Publication.GetPublicationbyID(0, pubcode);

                //string[] autoSubsPost = queryString.Split('&');
                #region set up post field data
                Dictionary<string, string> postFields = new Dictionary<string, string>();
                //KMPS_JF_Objects.Objects.Utilities.SendNotification("info@knowledgemarketing.com", "jointforms - paid forms", "justin.welter@teamkm.com", "Step 5", "create dictionary");
                //foreach (DataColumn st in subscriber.Columns)
                //{
                //    string key = st.ColumnName;
                //    string value = subscriber.Rows[0][st].ToString();

                //    if (!postFields.ContainsKey(key))
                //    {
                //        postFields.Add(key, value);
                //    }
                //}

                foreach (KeyValuePair<string, string> kvp in Demographics)
                {
                    if (!postFields.ContainsKey(kvp.Key.ToLower()))
                    {
                        //add
                        postFields.Add(kvp.Key.ToLower(), kvp.Value);
                    }
                    else
                    {
                        //replace
                        postFields[kvp.Key.ToLower()] = kvp.Value;
                    }
                }

                if (postFields.ContainsKey("user_paidorfree"))
                {
                    postFields["user_paidorfree"] = "PAID";
                }
                else
                {
                    postFields.Add("user_paidorfree", "PAID");
                }
                #endregion

                if (pub != null)
                {
                    //Loop through auto sub groups and post for each one
                    Dictionary<int, int> autoSubGroups = Publication.GetAutoSubscriptions(pub.PubID);
                    foreach (KeyValuePair<int, int> kvp in autoSubGroups)
                    {
                        try
                        {
                            if (postFields.ContainsKey("g"))
                                postFields["g"] = kvp.Key.ToString();
                            else
                                postFields.Add("g", kvp.Key.ToString());

                            if (postFields.ContainsKey("c"))
                                postFields["c"] = kvp.Value.ToString();
                            else
                                postFields.Add("c", kvp.Value.ToString());
                        }
                        catch { }
                        AutoSubGroupID = kvp.Key;

                        ECNUtils.ECNHttpPost(emailaddress, pub.PubCode, GetQueryStringFromDictionary(postFields), browserInfo);
                    }
                }
            }
            catch (Exception ex)
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error when posting to AutoSub groups");
                sb.AppendLine("Email:" + emailaddress);
                sb.AppendLine("FName:" + FName);
                sb.AppendLine("LName:" + LName);
                sb.AppendLine("Token:" + Token);
                sb.AppendLine("Country:" + Country);
                sb.AppendLine("AutoSubGroup:" + AutoSubGroupID.ToString());

                sb.AppendLine("");

                sb.AppendLine(ex.StackTrace);

                KMPS_JF_Objects.Objects.Utilities.SendMail(sb.ToString());
            }
        }

        private string GetQueryStringFromDictionary(Dictionary<string, string> postFields)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in postFields)
                sb.Append(kvp.Key + "=" + kvp.Value + "&");

            return sb.ToString().TrimEnd('&');
        }



    }
}