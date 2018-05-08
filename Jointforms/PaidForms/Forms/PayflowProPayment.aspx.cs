using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS_JF_Objects.Objects;
using NXTBookAPI.BusinessLogic;
using NXTBookAPI.Entity;
using System.Data;
using ecn.communicator.classes;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;

namespace PaidForms.Forms
{
    public partial class PayPalPayment : System.Web.UI.Page
    {
        private string ECNPostFormat = string.Empty;
        private static int PubID = -1;
        private PaidForm paidform = null;
        private bool paymentSuccessfule = false;
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
                try
                {
                    return Request["emailaddress"].ToString();
                }
                catch
                {
                    return Request.Form["EMAIL"].ToString();
                }
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

        protected NameValueCollection httpRequestVariables()
        {
            var post = Request.Form;       // $_POST
            var get = Request.QueryString; // $_GET
            return Merge(post, get);
        }

        public static NameValueCollection Merge(NameValueCollection first, NameValueCollection second)
        {
            if (first == null && second == null)
                return null;
            else if (first != null && second == null)
                return new NameValueCollection(first);
            else if (first == null && second != null)
                return new NameValueCollection(second);

            NameValueCollection result = new NameValueCollection(first);
            for (int i = 0; i < second.Count; i++)
                result.Set(second.GetKey(i), second.Get(i));
            return result;
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

                    
                    ProcessPaypalPayment();

                }
            }
            catch (Exception ex)
            {
                try
                {
                    //send notification email if NXTbook API fails.
                    string emailMsg = "Error in PayflowProPaymen.aspx  <br /><br />";


                    emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                    emailMsg += "formID :" + FormID + "<br /><br />";
                    emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                    emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                    emailMsg += "<b>Exception Details:</b>" + ex.Message;
                    emailMsg += "<b>StackTrace:</b>" + ex.StackTrace + "<br />";

                    emailMsg += "<b>NameValue Collection:";
                    NameValueCollection response = httpRequestVariables();

                    foreach (string key in response)
                    {
                        emailMsg += key + ":" + response[key] + "<br />";
                    }


                    EmailFunctions emailFunctions = new EmailFunctions();
                    emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Paidforms - DB Update Failed in ProcessPaypalPayment method", emailMsg);
                    lblMessage.Text = "Payment was not successful";
                    lblMessage.Visible = true;
                }
                catch(Exception ex2)
                {
                    if(paymentSuccessfule)
                    {
                        if (paidform.FormRedirect != null && paidform.FormRedirect != string.Empty)
                        {
                            paymentSuccessfule = true;
                            string redirectURL = paidform.FormRedirect;
                            if (!redirectURL.Contains("http://") && !redirectURL.Contains("https://"))
                                redirectURL = "http://" + redirectURL;
                            Response.Redirect(redirectURL, false);
                            Context.ApplicationInstance.CompleteRequest();

                        }
                        else
                        {
                            paymentSuccessfule = true;
                            Response.Redirect("thankyou.aspx?formid=" + FormID.ToString() + "&emailaddress=" + EmailAddress.Replace("+","%2B"), false);
                            Context.ApplicationInstance.CompleteRequest();
                        }

                    }
                    else
                    {
                        lblMessage.Text = "Payment was not successful";
                        lblMessage.Visible = true;
                    }
                }
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
            NameValueCollection response = httpRequestVariables();
            
            if (response["RESULT"] != null && response["RESULT"].ToString().Equals("0") && response["RESPMSG"] != null && response["RESPMSG"].ToString().ToLower().Equals("approved"))
            {

                //transaction was successful, post to ECN
                string ccNumber = "************" + response["ACCT"];
                string tranID = response["PNREF"];
                string firstName = response["FIRSTNAME"];
                string lastName = response["LASTNAME"];
                string fullName = (firstName + " " + lastName).Trim();
                string cardEXP = response["EXPDATE"].Substring(0,2) + "/20" + response["EXPDATE"].Substring(2,2);
                string cardType = response["CARDTYPE"];
                string cardBillZip = response["BILLTOZIP"];
                string t_street = response["BILLTOSTREET"];
                //string t_street2 = response["BILLTOSTREET2"];
                string t_city = response["BILLTOCITY"];
                string t_State = response["BILLTOSTATE"];
                string t_Zip = response["BILLTOZIP"];
                string t_Country = response["BILLTOCOUNTRY"];
                string t_TransDate = response["TRANSTIME"];
                string shipAddress = response["SHIPTOSTREET"];
                string shipCity = response["SHIPTOCITY"];
                string shipState = response["SHIPTOSTATE"];
                string shipCountry = response["SHIPTOCOUNTRY"];
                string shipForZip = response["SHIPTOZIP"];
                string shipToName = response["NAMETOSHIP"];
                string shipFirstName = "";
                try
                {
                    shipFirstName = shipToName.Substring(0, shipToName.IndexOf(" "));
                }catch { }
                string shipLastName = "";
                try
                {
                    shipLastName = shipToName.Substring(shipToName.IndexOf(" ") + 1);
                }catch { }
                string shipZip = response["SHIPTOZIP"];
                switch(cardType)
                {
                    case "0":
                        cardType = "Visa";
                        break;
                    case "1":
                        cardType = "MasterCard";
                        break;
                    case "2":
                        cardType = "Discover";
                        break;
                    case "3":
                        cardType = "American Express";
                        break;
                    case "4":
                        cardType = "Diners Club";
                        break;
                    case "5":
                        cardType = "JCB";
                        break;
                    default:
                        
                        break;
                }
                //AddToCache(txtemail.Text.ToLower().Trim() + "_" + getFormID().ToString() + "_PurchasedProducts", dPurchasedProducts);
                Dictionary<int, Dictionary<string, string>> dPurchasedProducts = GetCacheValue(EmailAddress.ToLower() + "_" + FormID + "_PurchasedProducts");
                string queryParamsForError = "";


                RemoveFromCache(EmailAddress.ToLower() + "_" + FormID + "_PurchasedProducts");
                AddToCache(TransactionCacheName, tranID);
                Dictionary<string, string> profileFields = ECNUtils.GetECNProfileFields();
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                bool gotParams = false;
                foreach (KeyValuePair<int, Dictionary<string, string>> product in dPurchasedProducts)
                {

                    int productID = product.Key;

                    Product prod = Product.GetByProductID(productID);
                    Dictionary<string, string> dsubscriberdata = new Dictionary<string, string>();
                    List<ECN_Framework_Entities.Communicator.GroupDataFields> udfs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(prod.GroupID).Where(x => !x.DatafieldSetID.HasValue).ToList();
                    foreach (KeyValuePair<string,string> kvp in product.Value)
                    {
                        dsubscriberdata.Add(kvp.Key, kvp.Value);
                    }
                    if(!gotParams)
                    {
                        queryParamsForError = GetQueryParams(dsubscriberdata, udfs);
                        gotParams = true;
                    }

                    if(dsubscriberdata.ContainsKey("cf_pubcode") && !dsubscriberdata["cf_pubcode"].ToLower().Equals(prod.PubCode.ToLower()))
                    {
                        dsubscriberdata = RemoveUDFsFromDict(dsubscriberdata, prod.GroupID, udfs);
                    }
                    
                    
                    

                    dsubscriberdata = AddToDict("t_transactionid", tranID, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_cardnumber", ccNumber, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_firstname", firstName, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_lastname", lastName, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_fullname", fullName, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_expirationdate", cardEXP, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_cardbillzip", cardBillZip, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_cardtype", cardType, dsubscriberdata);
                    dsubscriberdata = AddToDict("shipto_address1", shipAddress, dsubscriberdata);
                    dsubscriberdata = AddToDict("shipto_address2", "", dsubscriberdata);
                    dsubscriberdata = AddToDict("shipto_city", shipCity, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_city", t_city, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_country", t_Country, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_street", t_street, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_state", t_State, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_zip", t_Zip, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_transactionid", tranID, dsubscriberdata);
                    dsubscriberdata = AddToDict("t_street2", "", dsubscriberdata);

                    if(shipCountry.ToUpper().Equals("US") || shipCountry.ToUpper().Equals("CA"))
                    {
                        dsubscriberdata = AddToDict("shipto_state", shipState, dsubscriberdata);

                        dsubscriberdata = AddToDict("shipto_zip", shipZip, dsubscriberdata);

                        dsubscriberdata = AddToDict("shipto_stat_int", "", dsubscriberdata);
                        dsubscriberdata = AddToDict("shipto_forzip", "", dsubscriberdata);


                    }
                    else
                    {
                        dsubscriberdata = AddToDict("shipto_state_int", shipState, dsubscriberdata);
                        dsubscriberdata = AddToDict("shipto_forzip", shipZip, dsubscriberdata);

                        dsubscriberdata = AddToDict("shipto_state", "", dsubscriberdata);

                        dsubscriberdata = AddToDict("shipto_zip", "", dsubscriberdata);

                    }

                    #region Send data to ECN (write also to HTTP POST)
                    SendPaypalResponseEmail(response, dsubscriberdata, EmailAddress);
                    try
                    {
                        ECNUtils.SubscribeToGroup(Convert.ToInt32(dsubscriberdata["c"]), Convert.ToInt32(dsubscriberdata["g"]), dsubscriberdata["publicationcode"], dsubscriberdata, browserInfo);
                    }
                    catch (Exception ex)
                    {
                        //send notification email if NXTbook API fails.
                        string emailMsg = "Error in Payment.aspx - Auto Subscribe Groups <br /><br />";

                        emailMsg += "Errorlocation : PayflowProPayment.aspx.cs.ProcessPaypalPayment <br /><br />";

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
                        List<string> AutoSubpubCodes = new List<string>();
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

                if (paidform.FormRedirect != null && paidform.FormRedirect != string.Empty)
                {
                    paymentSuccessfule = true;
                    string redirectURL = paidform.FormRedirect;
                    if (!redirectURL.Contains("http://") && !redirectURL.Contains("https://"))
                        redirectURL = "http://" + redirectURL;
                    Response.Redirect(redirectURL, false);
                    Context.ApplicationInstance.CompleteRequest();
                    
                }
                else
                {
                    paymentSuccessfule = true;
                    Response.Redirect("thankyou.aspx?formid=" + FormID + "&emailaddress=" + EmailAddress.Replace("+", "%2B"), false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                Dictionary<int, Dictionary<string, string>> dPurchasedProducts = GetCacheValue(EmailAddress.ToLower() + "_" + FormID + "_PurchasedProducts");
                string queryParamsForError = "";
                foreach (KeyValuePair<int, Dictionary<string, string>> product in dPurchasedProducts)
                {
                    Product prod = Product.GetByProductID(product.Key);
                    List<ECN_Framework_Entities.Communicator.GroupDataFields> udfs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(prod.GroupID).Where(x => !x.DatafieldSetID.HasValue).ToList();
                    queryParamsForError = GetQueryParams(product.Value, udfs);
                    break;
                }
                


                RemoveFromCache(EmailAddress.ToLower() + "_" + FormID + "_PurchasedProducts");
                //transaction wasn't successful
                phError.Visible = true;
                lblErrorMessage.Text = "Payment was not successful<br />" + response["RESPMSG"].ToString();
                lblErrorMessage.Visible = true;

                hlPaidFormLink.NavigateUrl = "/PaidForms/PaidForm.aspx?formid=" + FormID.ToString() + queryParamsForError; 
            }
        }

        private string GetQueryParams(Dictionary<string,string> dict, List<ECN_Framework_Entities.Communicator.GroupDataFields> udfs)
        {
            StringBuilder sb = new StringBuilder();

            List<string> keysToRemove = new List<string>();
            keysToRemove.Add("paymentstatus");
            keysToRemove.Add("paidorfree");
            keysToRemove.Add("publicationcode");
            keysToRemove.Add("pubcode");
            keysToRemove.Add("cf_pubcode");
            keysToRemove.Add("isnxtbookapienabled");
            keysToRemove.Add("issubscription");
            keysToRemove.Add("c");
            keysToRemove.Add("g");
            keysToRemove.Add("formid");

            List<string> namesAdded = new List<string>();
            foreach(KeyValuePair<string, string> data in dict)
            {
                if (!data.Key.StartsWith("t_") && !data.Key.StartsWith("shipto") && !keysToRemove.Contains(data.Key.ToLower()))
                {
                    if (!namesAdded.Contains(data.Key.ToLower()))
                    {
                        
                        if (udfs.Exists(x => x.ShortName.ToLower() == data.Key.ToLower()) && !namesAdded.Contains("user_" + data.Key.ToLower()))
                        {
                            namesAdded.Add("user_" + data.Key.ToLower());
                            sb.Append("&user_" + data.Key + "=" + data.Value);
                        }
                        else
                        {
                            namesAdded.Add(data.Key.ToLower());
                            switch (data.Key.ToLower())
                            {
                                case "firstname":
                                    sb.Append("&fn=" + data.Value);
                                    namesAdded.Add("fn");
                                    break;
                                case "lastname":
                                    sb.Append("&ln=" + data.Value);
                                    namesAdded.Add("ln");
                                    break;
                                case "email":
                                case "emailaddress":
                                    sb.Append("&e=" + data.Value);
                                    namesAdded.Add("e");
                                    break;
                                case "phone":
                                case "voice":
                                    sb.Append("&ph=" + data.Value);
                                    namesAdded.Add("ph");
                                    break;
                                case "address1":
                                    sb.Append("&adr=" + data.Value);
                                    namesAdded.Add("adr");
                                    break;
                                case "address2":
                                    sb.Append("&adr2=" + data.Value);
                                    namesAdded.Add("adr2");
                                    break;
                                case "zip":
                                    sb.Append("&zc=" + data.Value);
                                    namesAdded.Add("zc");
                                    break;
                                case "country":
                                    sb.Append("&ctry=" + data.Value);
                                    namesAdded.Add("ctry");
                                    break;
                                case "title":
                                    sb.Append("&t=" + data.Value);
                                    namesAdded.Add("t");
                                    break;
                                case "fullname":
                                    sb.Append("&n=" + data.Value);
                                    namesAdded.Add("n");
                                    break;
                                case "company":
                                    sb.Append("&compname=" + data.Value);
                                    namesAdded.Add("compname");
                                    break;
                                case "occupation":
                                    sb.Append("&occ=" + data.Value);
                                    namesAdded.Add("occ");
                                    break;
                                case "mobile":
                                    sb.Append("&mph=" + data.Value);
                                    namesAdded.Add("mph");
                                    break;
                                case "gender":
                                    sb.Append("&gndr=" + data.Value);
                                    namesAdded.Add("gndr");
                                    break;
                                case "user1":
                                    sb.Append("&usr1=" + data.Value);
                                    namesAdded.Add("usr1");
                                    break;
                                case "user2":
                                    sb.Append("&usr2=" + data.Value);
                                    namesAdded.Add("usr2");
                                    break;
                                case "user3":
                                    sb.Append("&usr3=" + data.Value);
                                    namesAdded.Add("usr3");
                                    break;
                                case "user4":
                                    sb.Append("&usr4=" +data.Value);
                                    namesAdded.Add("usr4");
                                    break;
                                case "user5":
                                    sb.Append("&usr5=" + data.Value);
                                    namesAdded.Add("usr5");
                                    break;
                                case "user6":
                                    sb.Append("&usr6=" + data.Value);
                                    namesAdded.Add("usr6");
                                    break;
                                case "birthdate":
                                    sb.Append("&bdt=" + data.Value);
                                    namesAdded.Add("bdt");
                                    break;
                                default:
                                    sb.Append("&" + data.Key + "=" + data.Value);
                                    break;
                            }
                        }
                    }
                    

                }
            }


            return sb.ToString();
        }

        private Dictionary<string,string> AddToDict(string key, string value, Dictionary<string,string> dict)
        {
            if(dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
            return dict;
        }

        private Dictionary<string, string> RemoveFromDict(string key, Dictionary<string, string> dict)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
           
            return dict;
        }

        private Dictionary<string, string> RemoveUDFsFromDict(Dictionary<string, string> dict, int groupID, List<ECN_Framework_Entities.Communicator.GroupDataFields> udfsToRemove)
        {
            
            udfsToRemove.ForEach(x => x.ShortName = x.ShortName.ToLower());
            

            foreach(string key in dict.Keys)
            {
                if(key.ToLower().StartsWith("user_"))
                {
                    dict.Remove(key);
                }
                else if(udfsToRemove.Exists(x => x.ShortName == key.ToLower()))
                {
                    dict.Remove(key);
                }
            }            

            return dict;
        }


        private static string getQueryString(string qs)
        {
            try { return HttpContext.Current.Request.QueryString[qs].ToString(); }
            catch { return string.Empty; }
        }

        private void SendPaypalResponseEmail(NameValueCollection paypalResponse, Dictionary<string,string> request, string EmailAddress)
        {
            try
            {
                string emailMsg = "Paypal Flow Redirect Response  <br /><br />";


                emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                emailMsg += "formID :" + FormID.ToString() + "<br /><br />";
                emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";


                string nvpstring = "";
                foreach (string key in paypalResponse)
                {
                    //format:  "PARAMETERNAME[lengthofvalue]=VALUE&".  Never URL encode.
                    var val = paypalResponse[key];
                    //nvpstring += key + "[ " + val.Length + "]=" + val + "&";
                    nvpstring += "<b>" + key + "</b>" + ":" + val + "<br />";
                }
                emailMsg += "<b>NameValue Collection:</b><br />" + nvpstring;

                
                string ecnString = "";
                foreach (KeyValuePair<string,string> kvp in request)
                {
                    ecnString += "<b>" + kvp.Key + "</b>" + ":" + kvp.Value + "<br />";
                }
                emailMsg += "<br /><br /><b>Inserted Into ECN:</b><br />" + ecnString;

                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Paidforms - PaypalFlow Redirect Response", emailMsg);
            }
            catch (Exception ex)
            {

            }
        }

        private void loadHeaderFooter()
        {
            phHeader.Controls.Add(new LiteralControl(paidform.HeaderHTML));
            phFooter.Controls.Add(new LiteralControl(paidform.FooterHTML));
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

        private StringBuilder BuildProfile(Dictionary<string, string> dsubscriberdata)
        {
            StringBuilder profile = new StringBuilder();
            try
            {
                profile.Append("fn=" + dsubscriberdata["firstname"].ToString());
            }catch { }
            try
            {
                profile.Append("&ln=" + dsubscriberdata["lastname"].ToString());
            }catch { }
            try
            {
                profile.Append("&compname=" + dsubscriberdata["company"].ToString());
            }catch { }
            try
            {
                profile.Append("&adr=" + dsubscriberdata["address"].ToString());
            }catch { }
            try
            {
                profile.Append("&adr2=" + dsubscriberdata["address2"].ToString());
            }catch { }
            try
            {
                profile.Append("&city=" + dsubscriberdata["city"].ToString());
            }catch { }
            try
            {
                profile.Append("&state=" + dsubscriberdata["state"].ToString());
            }catch { }
            try
            {
                profile.Append("&zc=" + dsubscriberdata["zip"].ToString());
            }catch { }
            try
            {
                profile.Append("&ctry=" + dsubscriberdata["country"].ToString());
            }catch { }
            try
            {
                profile.Append("&ph=" + dsubscriberdata["voice"].ToString());
            }catch { }
            try
            {
                profile.Append("&fax=" + dsubscriberdata["fax"].ToString());
            }catch { }
            try
            {
                profile.Append("&e=" + dsubscriberdata["emailaddress"].ToString());
            }catch { }
            try
            {
                profile.Append("&f=html");
            }catch { }
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
    }
}