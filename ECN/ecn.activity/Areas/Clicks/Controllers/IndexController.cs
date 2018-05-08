using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using KM.Common;
using KM.Common.Utilities.Email;

namespace ecn.activity.Areas.Clicks.Controllers
{
    public class IndexController : Controller
    {
        public int BlastID = 0;
        public int EmailID = -2;
        public int BlastLinkID = 0;
        public int UniqueLinkID = 0;
        public string LinkFromBlastLinkID = string.Empty;
        public int RefBlastID = 0;
        public KMPlatform.Entity.User User = null;
        public bool RedirectOnError = false;
        public string UserAgent = string.Empty;
        public string LinkToStore = string.Empty;
        public int customerID = 0;

        private const string AdminToEmailConfigurationKey = "Admin_ToEmail";
        private const string AdminFromEmailConfigurationKey = "Admin_FromEmail";

        //
        // GET: /Clicks/Index/
        [HttpGet]

        public ActionResult Index(string query)
        {
            string rawURL = Request.ServerVariables["HTTP_URL"].ToString();
            query = rawURL.Substring(rawURL.ToLower().IndexOf("clicks/") + 7);
            string redirectURL = string.Empty;
            //aHR0cDovL3d3dy5rbm93bGVkZ2VtYXJrZXRpbmcuY29t

            try
            {

                if (HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                {
                    User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                    HttpRuntime.Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    User = (KMPlatform.Entity.User)HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                }

                GetQueryParams(query);

                if (BlastID > 0 && EmailID >= -1 && (LinkFromBlastLinkID.Trim().Length > 0))
                {
                    //redirect even if we hit an error later
                    RedirectOnError = true;
                    if (LinkFromBlastLinkID.Length == 0)
                    {

                        throw new Exception("BlastLinkID or URL from database are empty and URL in querystring is empty");

                    }

                    if (ConfigurationManager.AppSettings["ValidateB4Tracking"].ToString().ToLower().Equals("true"))
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidForTracking(RefBlastID, EmailID))
                        {
                            TrackData();
                            //track topics in trans UDF
                            if (ContainsTopics(LinkToStore))
                            {
                                LogTransactionalUDF();
                            }                            
                        }
                    }
                    else
                    {
                        TrackData();
                        //track topics in trans UDF
                        if (ContainsTopics(LinkToStore))
                        {
                            LogTransactionalUDF();
                        }                        
                    }
                    if ((LinkToStore.ToLower().StartsWith("http://ad.doubleclick.net/") || LinkToStore.ToLower().StartsWith("https://ad.doubleclick.net/")) && ConfigurationManager.AppSettings["UsePatchForDouble"].ToLower().Equals("true"))//patch for doubleclick urls
                    {
                        return Redirect(LinkToStore);
                    }
                    else
                    {
                        redirectURL = RedirectTo();
                    }
                }
            }
            catch (TimeoutException te)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(te, "LinkFrom.Page_Load(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch { }
                //we hit an error but we do have a URL so send them to their destination
                if (RedirectOnError && LinkFromBlastLinkID.Length != 0)
                {
                    //if the db link was empty use the one from the querystring

                    LinkToStore = LinkFromBlastLinkID;
                    return Redirect(LinkToStore);
                }
                else
                    return RedirectToRoute(new { controller = "Error", action = "Error", error = "InvalidLink", area = "" });// errorMsgPanel.Visible = true;
            }
            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "LinkFrom.Page_Load(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch (Exception)
                {
                }
                //we hit an error but we do have a URL so send them to their destination
                if (RedirectOnError && LinkFromBlastLinkID.Length != 0)
                {
                    //if the db link was empty use the one from the querystring

                    //LinkToStore = LinkFromBlastLinkID;
                    //return Redirect(LinkToStore);
                    ViewBag.URL = LinkFromBlastLinkID;
                    if (LinkFromBlastLinkID.Contains("mailto:"))
                        ViewBag.MailTo = true;
                    else
                        ViewBag.MailTo = false;
                    return View("~/Areas/Clicks/Views/Index.cshtml");// Redirect(redirectURL);
                }
                else
                    return RedirectToRoute(new { controller = "Error", action = "Error", error = "InvalidLink", area = "" });// errorMsgPanel.Visible = true;
            }



            //return View();
            if (!string.IsNullOrEmpty(redirectURL))
            {
                ViewBag.URL = redirectURL;
                if (redirectURL.Contains("mailto:"))
                {
                    ViewBag.MailTo = true;
                    string queryParams = "";
                    if (redirectURL.Contains("?"))
                    {
                        
                        List<string> validMailToQP = new List<string>();
                        validMailToQP.Add("subject");
                        validMailToQP.Add("body");
                        validMailToQP.Add("cc");
                        validMailToQP.Add("bcc");
                        queryParams = redirectURL.Substring(redirectURL.IndexOf("?") + 1);
                        //queryParams = HttpUtility.UrlDecode(queryParams);
                        string[] keyValue = queryParams.Split('&');
                        string finalParams = "";
                        for (int i = 0; i < keyValue.Length ; i++)
                        {
                            string s = keyValue[i];
                            string[] nameValue = s.Split('=');
                            if(validMailToQP.Contains(nameValue[0]) && nameValue.Length > 1)
                            {
                                finalParams += nameValue[0] + "=" + Uri.EscapeDataString(HttpUtility.UrlDecode(nameValue[1])) + "&";
                            }
                            
                            
                        }
                        finalParams = finalParams.TrimEnd('&');

                        redirectURL = redirectURL.Substring(0, redirectURL.IndexOf("?") + 1);
                        redirectURL += finalParams;
                    }

                    ViewBag.URL = redirectURL;

                }
                else
                {
                    Uri newUri = new Uri(redirectURL);
                    string Path = newUri.AbsolutePath;
                    string[] pathFragments = Path.Split('/');
                    StringBuilder sbURLPath = new StringBuilder();

                    foreach (string s in pathFragments)
                    {
                        if (s.Contains("%") || s.Contains("+"))//doing this to make sure we don't encode "+" signs
                            sbURLPath.Append("/" + s);
                        else
                            sbURLPath.Append("/" + HttpUtility.UrlEncode(s));

                    }
                    string finalPath = "";
                    if (sbURLPath.ToString().StartsWith("//"))
                        finalPath = sbURLPath.ToString().Substring(1);
                    else
                        finalPath = sbURLPath.ToString();
                    redirectURL = newUri.AbsoluteUri.Replace(newUri.AbsolutePath, finalPath);
                    ViewBag.URL = redirectURL;
                    ViewBag.MailTo = false;
                }
                return View("~/Areas/Clicks/Views/Index.cshtml");// Redirect(redirectURL);

            }
            else
                return RedirectToRoute(new { controller = "Error", action = "Error", error = "InvalidLink", area = "" });// errorMsgPanel.Visible = true;
        }
        //
        // HEAD: /Clicks/Index/
        [HttpHead]
        public ActionResult Index(string query, bool amaPost = true)
        {
            string rawURL = Request.ServerVariables["HTTP_URL"].ToString();
            query = rawURL.Substring(rawURL.ToLower().IndexOf("clicks/") + 7);
            string redirectURL = string.Empty;
            // throw new HttpAntiForgeryException();
            try
            {
                if (HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                {
                    User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                    HttpRuntime.Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    User = (KMPlatform.Entity.User)HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                }
                GetQueryParams(query);
                if (BlastID > 0 && EmailID > 0 && (LinkFromBlastLinkID.Trim().Length > 0))
                {
                    //redirect even if we hit an error later
                    RedirectOnError = true;
                    if (LinkFromBlastLinkID.Length == 0)
                    {

                        throw new Exception("BlastLinkID or URL from database are empty and URL in querystring is empty");

                    }
                }
                redirectURL = RedirectTo();
                if (!string.IsNullOrEmpty(redirectURL))
                    return Redirect(redirectURL);
                else
                    return RedirectToRoute(new { controller = "Error", action = "Error", error = "InvalidLink", area = "" });// errorMsgPanel.Visible = true;
            }
            catch (TimeoutException te)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(te, "LinkFrom.Page_Load(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch { }
                //we hit an error but we do have a URL so send them to their destination
                if (RedirectOnError && LinkFromBlastLinkID.Length != 0)
                {
                    //if the db link was empty use the one from the querystring

                    LinkToStore = LinkFromBlastLinkID;
                    return Redirect(RedirectTo());
                }
                else
                    return RedirectToRoute(new { controller = "Error", action = "Error", error = "InvalidLink", area = "" });// errorMsgPanel.Visible = true;
            }
            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "LinkFrom.Page_Load(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch (Exception)
                {
                }
                //we hit an error but we do have a URL so send them to their destination
                if (RedirectOnError && LinkFromBlastLinkID.Length != 0)
                {
                    //if the db link was empty use the one from the querystring

                    LinkToStore = LinkFromBlastLinkID;
                    return Redirect(RedirectTo());
                }
                else
                    return RedirectToRoute(new { controller = "Error", action = "Error", error = "InvalidLink", area = "" });// errorMsgPanel.Visible = true;
            }
        }


        #region private methods

        private void GetQueryParams(string dirtyQuery)
        {
            string[] parse = null;
            string b = "0";
            string e = "0";
            string lid = "0";
            string ulid = "0";
            string oldLink = "";
            string unencrypted = "";
            try
            {
                KM.Common.Entity.Encryption ecLink = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                unencrypted = KM.Common.Encryption.Base64Decrypt(System.Web.HttpUtility.UrlDecode(dirtyQuery), ecLink);

            }
            catch
            {
                //Cant decode/decrypt, it might not be encoded/encrypted so try to pull the params out as usual
                unencrypted = dirtyQuery.StartsWith("?") ? dirtyQuery.Substring(1) : dirtyQuery;
            }

            try
            {
                parse = unencrypted.Split('&');
                foreach (string s in parse)
                {
                    string[] kvp = s.Split('=');
                    switch (kvp[0])
                    {
                        case "b":
                            b = kvp[1];
                            break;
                        case "e":
                            e = kvp[1];
                            break;
                        case "lid":
                            lid = kvp[1];
                            break;
                        case "ulid":
                            ulid = kvp[1];
                            break;
                        case "l":
                            oldLink = kvp[1];
                            break;
                    }
                }

                if (b.IndexOf(",") > 0)
                {
                    BlastID = Convert.ToInt32(b.Substring(0, b.IndexOf(",")));
                }
                else
                {
                    BlastID = Convert.ToInt32(b.ToString());
                }

                BlastLinkID = Convert.ToInt32(lid);
                UniqueLinkID = Convert.ToInt32(ulid);
                EmailID = Convert.ToInt32(e);
                LinkFromBlastLinkID = getLinkFromBlastLinkID();
                if (LinkFromBlastLinkID.Length > 0)
                    LinkToStore = LinkFromBlastLinkID;
                else
                {
                    LinkToStore = oldLink;
                    LinkFromBlastLinkID = oldLink;
                }
                RefBlastID = getRefBlastID();
            }
            catch { }
        }

        private string getLinkFromBlastLinkID()
        {
            string theLink = string.Empty;
            try
            {
                if (BlastLinkID != 0)
                {
                    ECN_Framework_Entities.Communicator.BlastLink blastLink = ECN_Framework_BusinessLayer.Communicator.BlastLink.GetByBlastLinkID(BlastID, BlastLinkID);
                    if (blastLink != null && blastLink.LinkURL.Trim().Length > 0)
                    {
                        string lk = blastLink.LinkURL;
                        string tempGuid = Guid.NewGuid().ToString();
                        // to avoid ascii chars during URL decode - replace %% with "$ECN$" and replace it back after decode.
                        lk = lk.Replace("%%", "$" + tempGuid + "$");
                        theLink = Server.UrlDecode(LinkCleanUP(lk));
                        theLink = lk.Replace("$" + tempGuid + "$", "%%");

                        //theLink = theLink.Replace("%%ConversionTrkCDE%%", "");
                    }
                }
            }
            catch (Exception)
            {
            }
            return theLink.Trim();
        }

        private bool ContainsTopics(string link)
        {
            if (link.ToLower().Contains("topic="))
            {
                return true;
            }
            else
                return false;
        }

        private int getRefBlastID()
        {
            RefBlastID = BlastID;
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(BlastID, false);
            try
            {
                if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                {
                    RefBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, EmailID, blast.CustomerID.Value, blast.BlastType);
                }
            }
            catch (Exception) { }
            return RefBlastID;
        }
        private string CreateRedirectLink()
        {
            string linkToRedirect = LinkToStore;
            bool bConversionTrackingExists = false;

            if (linkToRedirect.Contains("%%ConversionTrkCDE%%"))
            {
                bConversionTrackingExists = true;
            }

            //if UDFs exist
            if (linkToRedirect.IndexOf("%%") >= 0)
            {
                linkToRedirect = replaceUDFWithValue(linkToRedirect);
            }

            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(BlastID, false);

            if (ci != null)
            {
                customerID = ci.CustomerID.Value;
                List<ECN_Framework_Entities.Communicator.LinkTracking> LinkTrackingList = ECN_Framework_BusinessLayer.Communicator.LinkTracking.GetByCampaignItemID(ci.CampaignItemID);
                foreach (ECN_Framework_Entities.Communicator.LinkTracking linkTracking in LinkTrackingList)
                {
                    if (ECN_Framework_BusinessLayer.Communicator.LinkTracking.CreateLinkTrackingParams(ci.CustomerID.Value, getDomainName(linkToRedirect), linkTracking.LTID))
                    {
                        DataTable dt = ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking.GetParamInfo(BlastID, linkTracking.LTID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (linkToRedirect.IndexOf("?") > 0)
                                linkToRedirect += "&";
                            else
                                linkToRedirect += "?";
                            linkToRedirect += CreateTrackingLink(dt, bConversionTrackingExists);
                        }
                    }
                }

                //Check for Domain Tracking
                ECN_Framework_Entities.Accounts.Customer campaignItemCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(ci.CustomerID.Value, false);
                if ((ECN_Framework_BusinessLayer.Accounts.BaseChannel.HasProductFeature(campaignItemCustomer.BaseChannelID.Value, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup)) && (ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.Exists(getDomainName(linkToRedirect), campaignItemCustomer.BaseChannelID.Value)))
                {
                    Uri RedirectUri = new Uri(linkToRedirect);
                    string eidValue = HttpUtility.ParseQueryString(RedirectUri.Query).Get("eid");
                    if (eidValue == null)
                    {
                        if (linkToRedirect.IndexOf("?") > 0)
                            linkToRedirect += "&";
                        else
                            linkToRedirect += "?";
                        linkToRedirect += "eid=" + EmailID;
                    }

                    string bidValue = HttpUtility.ParseQueryString(RedirectUri.Query).Get("bid");
                    if (bidValue == null)
                    {
                        if (linkToRedirect.IndexOf("?") > 0)
                            linkToRedirect += "&";
                        else
                            linkToRedirect += "?";
                        linkToRedirect += "bid=" + BlastID;
                    }
                }
            }
            return linkToRedirect;
        }

        private string CreateTrackingLink(DataTable dt, bool ConversionTrackingExists)
        {
            StringBuilder qsParams = new StringBuilder();
            //Omniture QS setup
            bool allowCustomerOverride = true;
            bool overrideBaseChannel = true;
            bool hasOmniture = false;
            bool hasOmniValues = false;
            ECN_Framework_Entities.Communicator.CampaignItem ci = null;
            ECN_Framework_Entities.Accounts.Customer cu = null;
            ECN_Framework_Entities.Communicator.LinkTrackingSettings ltsBase = null;
            ECN_Framework_Entities.Communicator.LinkTrackingSettings ltsCustomer = null;
            string omniQS = "";
            string omniDelimiter = "";
            XmlNode baseRoot = null;
            XmlNode custRoot = null;
            if (dt.Rows[0]["LTID"].ToString().Equals("3"))
            {
                ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(BlastID, false);
                cu = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(ci.CustomerID.Value, false);
                ltsBase = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.GetByBaseChannelID_LTID(cu.BaseChannelID.Value, 3);

                ltsCustomer = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings.GetByCustomerID_LTID(cu.CustomerID, 3);

                try
                {
                    XmlDocument baseDoc = new XmlDocument();
                    XmlDocument custDoc = new XmlDocument();
                    baseDoc.LoadXml(ltsBase.XMLConfig);
                    try
                    {

                        custDoc.LoadXml(ltsCustomer.XMLConfig);
                    }
                    catch { }
                    baseRoot = baseDoc.SelectSingleNode("/Settings");
                    try
                    {

                        custRoot = custDoc.SelectSingleNode("/Settings");
                    }
                    catch { }

                    if (baseRoot["AllowCustomerOverride"].InnerText.ToLower().Equals("true"))
                    {
                        allowCustomerOverride = true;
                        hasOmniture = true;
                    }
                    else
                    {
                        allowCustomerOverride = false;
                        hasOmniture = true;
                    }

                    if (custRoot != null && custRoot["Override"].InnerText.ToLower().Equals("true"))
                    {
                        overrideBaseChannel = true;
                        hasOmniture = true;
                    }
                    else
                    {
                        overrideBaseChannel = false;

                    }


                    if (allowCustomerOverride && overrideBaseChannel)
                    {
                        omniQS = custRoot["QueryString"].InnerText.Trim() + "=";
                        omniDelimiter = custRoot["Delimiter"].InnerText.Trim();
                    }
                    else if ((!allowCustomerOverride) || (allowCustomerOverride && !overrideBaseChannel))
                    {
                        omniQS = baseRoot["QueryString"].InnerText.Trim() + "=";
                        omniDelimiter = baseRoot["Delimiter"].InnerText.Trim();
                    }

                    //This is to check if the link already has the omniture query paramater name
                    //If it does, we don't want to add the omniture stuff
                    try
                    {
                        string linkSub = LinkToStore.Substring(LinkToStore.IndexOf('?'));
                        if (linkSub.Length > 0 && hasOmniture)
                        {
                            if (linkSub.ToLower().Contains(omniQS.ToLower()))
                                hasOmniture = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {
                    hasOmniture = false;
                    //KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Error with Omniture setup");
                }
            }
            string[] omniParams = new string[10];


            foreach (DataRow row in dt.Rows)
            {
                if (!row["ColumnName"].ToString().ToLower().Contains("omniture"))
                {
                    switch (row["ColumnName"].ToString())
                    {
                        case "KnowledgeMarketing":
                            qsParams.Append(row["DisplayName"].ToString() + "=");
                            qsParams.Append("KnowledgeMarketing&");
                            break;
                        case "FolderName":
                        case "GroupName":
                        case "LayoutName":
                        case "EmailSubject":
                            qsParams.Append(row["DisplayName"].ToString() + "=");
                            string paramValue = string.Empty;
                            try
                            {
                                paramValue = ECN_Framework_BusinessLayer.Communicator.Blast.GetLinkTrackingParam(BlastID, row["ColumnName"].ToString());
                                qsParams.Append(paramValue);
                            }
                            catch (Exception)
                            {
                                qsParams.Append("UNKNOWN");
                                KM.Common.Entity.ApplicationLog.LogNonCriticalError(row["ColumnName"].ToString() + " not found for LinkTracking", "LinkFrom.CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                            }
                            qsParams.Append("&");
                            break;
                        case "CustomValue":
                            qsParams.Append(row["DisplayName"].ToString() + "=");
                            try
                            {
                                string Other = row["CustomValue"].ToString().Replace(" ", "%20");
                                qsParams.Append(Other);
                            }
                            catch (Exception)
                            {
                                qsParams.Append("UNKNOWN");
                                KM.Common.Entity.ApplicationLog.LogNonCriticalError("CustomValue not found for LinkTracking", "LinkFrom.CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                            }
                            qsParams.Append("&");
                            break;
                        case "BlastID":
                            qsParams.Append(row["DisplayName"].ToString() + "=");
                            try
                            {
                                qsParams.Append(BlastID.ToString());
                            }
                            catch (Exception)
                            {
                                qsParams.Append("UNKNOWN");
                                KM.Common.Entity.ApplicationLog.LogNonCriticalError("BlastID not found for LinkTracking", "LinkFrom.CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                            }
                            qsParams.Append("&");
                            break;
                        case "eid":
                            if (!ConversionTrackingExists)
                            {
                                Uri RedirectUri = new Uri(LinkToStore);
                                string bidValue = HttpUtility.ParseQueryString(RedirectUri.Query).Get("eid");
                                if (bidValue == null)
                                {
                                    qsParams.Append(row["DisplayName"].ToString() + "=");
                                    qsParams.Append(EmailID.ToString());
                                    qsParams.Append("&");
                                }
                            }
                            break;
                        case "bid":
                            if (!ConversionTrackingExists)
                            {
                                Uri RedirectUri = new Uri(LinkToStore);
                                string bidValue = HttpUtility.ParseQueryString(RedirectUri.Query).Get("bid");
                                if (bidValue == null)
                                {
                                    qsParams.Append(row["DisplayName"].ToString() + "=");
                                    qsParams.Append(BlastID.ToString());
                                    qsParams.Append("&");
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    #region Omniture
                    if (hasOmniture)
                    {
                        int paramNumber = 0;
                        switch (row["ColumnName"].ToString().ToLower())
                        {
                            case "omniture1":
                            case "omniture2":
                            case "omniture3":
                            case "omniture4":
                            case "omniture5":
                            case "omniture6":
                            case "omniture7":
                            case "omniture8":
                            case "omniture9":
                                paramNumber = Convert.ToInt32(row["ColumnName"].ToString().Substring(8, 1)) - 1;
                                try
                                {
                                    hasOmniValues = true;
                                    if (row["LTPOID"].ToString().Equals("-1"))
                                    {
                                        omniParams[paramNumber] = row["CustomValue"].ToString().Replace(" ", "%20");
                                    }
                                    else
                                    {
                                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(row["LTPOID"].ToString()));
                                        if (ltpo.IsDynamic)
                                        {
                                            switch (ltpo.Value.ToLower())
                                            {
                                                case "blastid":
                                                    try
                                                    {
                                                        omniParams[paramNumber] = BlastID.ToString();
                                                    }
                                                    catch (Exception)
                                                    {
                                                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("BlastID not found for LinkTracking", "LinkFrom.CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                                                        omniParams[0] = "UNKNOWN";
                                                    }
                                                    break;
                                                case "groupname":
                                                    string groupName = string.Empty;
                                                    try
                                                    {
                                                        groupName = ECN_Framework_BusinessLayer.Communicator.Blast.GetLinkTrackingParam(BlastID, ltpo.Value);
                                                    }
                                                    catch (Exception)
                                                    {
                                                        omniParams[paramNumber] = "UNKNOWN";
                                                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("GroupName not found for LinkTracking", "LinkFrom.CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                                                    }
                                                    omniParams[paramNumber] = groupName;
                                                    break;
                                            }
                                        }
                                        else
                                            omniParams[paramNumber] = ltpo.Value.Replace(" ", "%20");
                                    }
                                    break;
                                }
                                catch
                                {

                                    break;
                                }
                            case "omniture10":
                                paramNumber = Convert.ToInt32(row["ColumnName"].ToString().Substring(8, 2)) - 1;
                                try
                                {
                                    hasOmniValues = true;
                                    if (row["LTPOID"].ToString().Equals("-1"))
                                    {
                                        omniParams[paramNumber] = row["CustomValue"].ToString().Replace(" ", "%20");
                                    }
                                    else
                                    {
                                        ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(Convert.ToInt32(row["LTPOID"].ToString()));
                                        if (ltpo.IsDynamic)
                                        {
                                            switch (ltpo.Value.ToLower())
                                            {
                                                case "blastid":
                                                    try
                                                    {
                                                        omniParams[paramNumber] = BlastID.ToString();
                                                    }
                                                    catch (Exception)
                                                    {
                                                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("BlastID not found for LinkTracking", "LinkFrom.CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                                                        omniParams[0] = "UNKNOWN";
                                                    }
                                                    break;
                                                case "groupname":
                                                    string groupName = string.Empty;
                                                    try
                                                    {
                                                        groupName = ECN_Framework_BusinessLayer.Communicator.Blast.GetLinkTrackingParam(BlastID, ltpo.Value);
                                                    }
                                                    catch (Exception)
                                                    {
                                                        omniParams[paramNumber] = "UNKNOWN";
                                                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("GroupName not found for LinkTracking", "LinkFrom.CreateTrackingLink", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                                                    }
                                                    omniParams[paramNumber] = groupName;
                                                    break;
                                            }
                                        }
                                        else
                                            omniParams[paramNumber] = ltpo.Value.Replace(" ", "%20");
                                    }
                                    break;
                                }
                                catch
                                {

                                    break;
                                }
                        }
                    }
                    #endregion
                }
            }

            if (hasOmniture && hasOmniValues)
            {
                StringBuilder sbFinalOmni = new StringBuilder();
                for (int i = omniParams.Length - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(omniParams[i]))
                    {
                        sbFinalOmni.Insert(0, omniDelimiter + EncodeParamValue(omniParams[i].ToString()));
                    }
                }
                if (omniQS.Length >= 1 && !string.IsNullOrWhiteSpace(sbFinalOmni.ToString()))
                {

                    omniQS += sbFinalOmni.ToString().Remove(0, 1);
                    qsParams.Append(omniQS + "&");
                }
            }

            if (qsParams.Length >= 1 && qsParams.ToString().EndsWith("&"))
            {
                qsParams = qsParams.Remove(qsParams.Length - 1, 1);

            }

            return qsParams.ToString();
        }

        private string getDomainName(string url)
        {
            try
            {
                return new Uri(url).Host;
            }
            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "LinkFrom.getDomainName", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch (Exception)
                {
                }
                return string.Empty;
            }
        }

        private string RedirectTo()
        {
            //code for anchor tags in URL Jwelter 05/06/2014
            string anchor = "";

            try
            {
                if (LinkToStore.Contains("#") && !LinkToStore.StartsWith("mailto:"))
                {
                    anchor = LinkToStore.Substring(LinkToStore.IndexOf("#"));

                }
                else if (LinkToStore.Contains("%23") && !LinkToStore.StartsWith("mailto:"))
                {
                    anchor = LinkToStore.Substring(LinkToStore.IndexOf("%23"));

                }
            }
            catch { }
            string linkToRedirect = CreateRedirectLink();
            if (!string.IsNullOrEmpty(anchor))
            {
                if (anchor.Length == 1)
                {
                    linkToRedirect = linkToRedirect.Replace(anchor, "");
                    linkToRedirect += anchor;
                }
                else
                {
                    linkToRedirect = linkToRedirect.Replace("#", "%23");
                }
            }

            //if (linkToRedirect.ToLower().ToString().StartsWith("mailto:"))
            //{
            //    // The following code will open a blank send email window when some one clicks on the mailto link in an email. But it will open a blank page. To 
            //    // avoid it, we are redirecting back to www.ecn5.com. We can put some sensible text in this same page as well.. 
            //    //pnlCloseBrowser.Visible = true;
            //    Response.Write("<script language='javascript'>window.location.href='" + linkToRedirect.Replace("'", "\\'") + "';</script>");
            //    Response.Write("<script language='javascript'>window.close();</script>");
            //    //ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "closePage", "window.close();");
            //}
            //else
            //{
            //    //The following is a patch for BlackBerry users.. it didn't support the java script method of recirecting the URL's. the following will do a straight redirect to the link.                        
            //    if (UserAgent.ToLower().Contains("blackberry"))
            //    {
            //        Response.Redirect(linkToRedirect, false);
            //    }
            //    else
            //    {
            //        Response.Write("<script language='javascript'>window.location.href='" + linkToRedirect.Replace("'", "\\'") + "'</script>");
            //    }
            //}
            return linkToRedirect;
        }

        private void GetUserAgent()
        {
            try
            {
                if (Request != null && Request.UserAgent != null)
                {
                    UserAgent = Request.UserAgent.ToString();
                }
            }
            catch (Exception)
            {
            }
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<BR><BR>BlastID: " + BlastID.ToString());
                adminEmailVariables.AppendLine("<BR>EmailID: " + EmailID.ToString());
                adminEmailVariables.AppendLine("<BR>BlastLinkID: " + BlastLinkID.ToString());
                adminEmailVariables.AppendLine("<BR>URLFromLinkID: " + LinkFromBlastLinkID);

                adminEmailVariables.AppendLine("<BR>LinkToStore: " + LinkToStore);
                adminEmailVariables.AppendLine("<BR>Page URL: " + Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
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

        private string replaceUDFWithValue(string theLink)
        {
            string modifiedLink = string.Empty;
            string udfName = string.Empty;

            try
            {
                DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.FilterEmailsAllWithSmartSegment(EmailID, RefBlastID);

                if (dt.Rows.Count > 0)
                {
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("%%.+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.MatchCollection matchList = reg.Matches(theLink);
                    foreach (System.Text.RegularExpressions.Match m in matchList)
                    {
                        udfName = m.Value.ToString().Replace("%%", string.Empty);
                        theLink = theLink.Replace("%%" + udfName + "%%", dt.Rows[0][udfName].ToString());
                    }
                }

            }
            catch (Exception ed)
            {
            }
            modifiedLink = theLink;

            return modifiedLink.Trim();
        }

        private void LogTransactionalUDF()
        {
            try
            {
                int start = LinkToStore.ToLower().IndexOf("topic=") + 6;
                int totalLength = LinkToStore.Length;
                if (totalLength - start > 0)
                {
                    string udfValue = LinkToStore.Substring(LinkToStore.ToLower().IndexOf("topic=") + 6, LinkToStore.Length - (LinkToStore.ToLower().IndexOf("topic=") + 6));

                    if (ECN_Framework_BusinessLayer.Communicator.EmailDataValues.RecordTopicsValue(RefBlastID, EmailID, udfValue) == 0)
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("UDF of Topics not found in db", "LinkFrom.LogTransactionalUDF", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                        NotifyOfMissingTopicUDF();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "LinkFrom.LogTransactionalUDF(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch (Exception)
                {
                }
            }
        }

        private void NotifyOfMissingTopicUDF()
        {
            if (ConfigurationManager.AppSettings["Admin_Notify"] == "true")
            {
                string adminEmailSubject = "Missing 'Topics' UDF for Blast";
                StringBuilder adminEmailBody = new StringBuilder("Activity Engines encountered a link with a 'topics' code but none was found in the group.");
                adminEmailBody.AppendLine("<BR><BR>BlastID: " + BlastID.ToString());
                adminEmailBody.AppendLine("<BR>EmailID: " + EmailID.ToString());
                adminEmailBody.AppendLine("<BR>BlastLinkID: " + BlastLinkID.ToString());
                adminEmailBody.AppendLine("<BR>URLFromLinkID: " + LinkFromBlastLinkID);

                adminEmailBody.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());

                var addressTo = ConfigurationManager.AppSettings[AdminToEmailConfigurationKey];
                var addressFrom = ConfigurationManager.AppSettings[AdminFromEmailConfigurationKey];
                var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
                var emailMessage = new EmailMessage
                {
                    From = addressFrom,
                    Subject = adminEmailSubject,
                    Body = adminEmailBody.ToString()
                };
                emailMessage.To.Add(addressTo);
                emailService.SendEmail(emailMessage);
            }
        }


        private int TrackData()
        {

            //spyinfo = Request.UserHostAddress + " | " + Request.UserAgent;
            try
            {
                string spyinfo = string.Empty;
                if (Request.UserHostAddress != null)
                {
                    spyinfo = Request.UserHostAddress;
                }
                if (Request.UserAgent != null)
                {
                    if (spyinfo.Length > 0)
                    {
                        spyinfo += " | ";
                    }
                    spyinfo += Request.UserAgent;
                }
                return ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertClick(EmailID, BlastID, LinkToStore, spyinfo, UniqueLinkID, User);
            }

            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "LinkFrom.TrackData(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch (Exception)
                {
                }
            }

            return 0;

        }

        private string LinkCleanUP(string link)
        {
            try
            {
                link = link.Replace("&amp;", "&");
                link = link.Replace("&lt;", "<");
                link = link.Replace("&gt;", ">");
            }
            catch (Exception)
            {
            }

            return link.Trim();
        }

        private string EncodeParamValue(string paramValue)
        {
            return HttpContext.Server.UrlEncode(paramValue);
        }
        #endregion
    }
}
