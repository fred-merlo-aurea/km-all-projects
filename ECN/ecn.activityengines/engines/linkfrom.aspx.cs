using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.DomainTracker;
using KM.Common;
using KM.Common.Extensions;
using KM.Common.Utilities.Email;
using KMPlatform.BusinessLogic;
using ApplicationLog = KM.Common.Entity.ApplicationLog;
using BaseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel;
using CampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using CampaignItemLinkTracking = ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking;
using Customer = ECN_Framework_BusinessLayer.Accounts.Customer;
using LinkTracking = ECN_Framework_BusinessLayer.Communicator.LinkTracking;
using User = KMPlatform.Entity.User;

namespace ecn.activityengines
{
    public partial class linkfrom : System.Web.UI.Page
    {
        public int BlastID = 0;
        public int EmailID = 0;
        public int BlastLinkID = 0;
        public int UniqueLinkID = 0;
        public string LinkFromURL = string.Empty;
        public string LinkFromBlastLinkID = string.Empty;
        public int RefBlastID = 0;
        public KMPlatform.Entity.User User = null;
        public bool RedirectOnError = false;
        public string UserAgent = string.Empty;
        public string LinkToStore = string.Empty;
        public bool DecodeURL = false;

        private const string AdminToEmailConfigurationKey = "Admin_ToEmail";
        private const string AdminFromEmailConfigurationKey = "Admin_FromEmail";
        private const string Delimiter = "Delimiter";
        private const string DisplayName = "DisplayName";
        private const string ColumnName = "ColumnName";
        private const string EqualsOperator = "=";
        private const string Unknown = "UNKNOWN";
        private const string KnowledgeMarketing = "KnowledgeMarketing";
        private const string Settings = "/Settings";
        private const string TrueString = "true";
        private const string FalseString = "false";
        private const string LinkFromCreateTrackingLink = "LinkFrom.CreateTrackingLink";
        private const string NotFoundForLinkTracking = " not found for LinkTracking";
        private const string Ltpoid = "LTPOID";
        private const string Omniture = "omniture";
        private const string FolderName = "FolderName";
        private const string GroupName = "GroupName";
        private const string LayoutName = "LayoutName";
        private const string EmailSubject = "EmailSubject";
        private const string KmCommonApplication = "KMCommon_Application";
        private const string CustomValue = "CustomValue";
        private const string Ltid = "LTID";
        private const string AllowCustomerOverride = "AllowCustomerOverride";
        private const string Override = "Override";
        private const string Space = " ";
        private const string Ampersand = "&";
        private const string QuestionMark = "?";
        private const string UrlSpace = "%20";
        private const string Eid = "eid";
        private const string Bid = "bid";
        private const string BlastIdSmall = "blastid";
        private const string GroupNameSmall = "groupname";
        private const string Omniture1 = "omniture1";
        private const string Omniture2 = "omniture2";
        private const string Omniture3 = "omniture3";
        private const string Omniture4 = "omniture4";
        private const string Omniture5 = "omniture5";
        private const string Omniture6 = "omniture6";
        private const string Omniture7 = "omniture7";
        private const string Omniture8 = "omniture8";
        private const string Omniture9 = "omniture9";
        private const string Omniture10 = "omniture10";
        private const string JavascriptWindowLocation = "<script language='javascript'>window.location.href='";
        private const string ScriptClose = "'</script>";
        private const string SingleQuote = "'";
        private const string DoubleSlash = "\\'";
        private const string MailTo = "mailto:";
        private const string MaintenanceMode = "Maintenance_Mode";
        private const string BlackBerry = "blackberry";
        private const string CacheUserByAccessKey = "cache_user_by_AccessKey_";
        private const string EcnEngineAccessKey = "ECNEngineAccessKey";
        private const string BlastLinkIdOrUrlEmpty = "BlastLinkID or URL from database are empty and URL in querystring is empty";
        private const string BlastLinkUrlQueryString = "BlastLinkID = {0} and URL from database = {1}, using URL from querystring";
        private const string LinkFromPageLoad = "LinkFrom.Page_Load";
        private const string UnknownIssue = "(Unknown Issue)";
        private const string ValidateB4Tracking = "ValidateB4Tracking";
        private const string UsePatchForDouble = "UsePatchForDouble";
        private const string DoubleClick = "doubleclick";
        private const string ConversionTrkCde = "%%ConversionTrkCDE%%";
        private const string DoublePercentage = "%%";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                errorMsgPanel.Visible = false;
                GetUserAgent();
                LinkFromURL = getLink();

                if (ConfigurationManager.AppSettings[MaintenanceMode]?.BoolTryParse() == true)
                {
                    PageLoadMaintenanceMode();
                }
                else
                {
                    PageLoadNonMainenanceMode();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex is TimeoutException)
                    {
                        ApplicationLog.LogNonCriticalError(
                            ex,
                            $"{LinkFromPageLoad}{UnknownIssue}",
                            ConfigurationManager.AppSettings[KmCommonApplication].IntTryParse(),
                            CreateNote());
                    }
                    else
                    {
                        ApplicationLog.LogCriticalError(
                            ex,
                            $"{LinkFromPageLoad}{UnknownIssue}",
                            ConfigurationManager.AppSettings[KmCommonApplication].IntTryParse(),
                            CreateNote());
                    }
                }
                catch (Exception innerException)
                {
                    System.Diagnostics.Trace.TraceError(innerException.Message);
                }

                if (RedirectOnError)
                {
                    if (LinkToStore.Length == 0)
                    {
                        LinkToStore = LinkFromURL;
                    }

                    Redirect();
                }
                else
                {
                    errorMsgPanel.Visible = true;
                }
            }
        }

        private void PageLoadNonMainenanceMode()
        {
            if (Cache[$"{CacheUserByAccessKey}{ConfigurationManager.AppSettings[EcnEngineAccessKey]}"] == null)
            {
                User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings[EcnEngineAccessKey], false);
                Cache.Add(
                    $"{CacheUserByAccessKey}{ConfigurationManager.AppSettings[EcnEngineAccessKey]}",
                    User,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(15),
                    CacheItemPriority.Normal,
                    null);
            }
            else
            {
                User = (User)Cache[$"{CacheUserByAccessKey}{ConfigurationManager.AppSettings[EcnEngineAccessKey]}"];
            }

            BlastID = getBlastID();
            EmailID = getEmailID();
            BlastLinkID = getBlastLinkID();
            LinkFromBlastLinkID = getLinkFromBlastLinkID();
            LinkToStore = LinkFromBlastLinkID;
            RefBlastID = getRefBlastID();
            UniqueLinkID = getUniqueLinkID();

            if (BlastID > 0 && EmailID > 0 && (LinkFromURL.Trim().Length > 0 || LinkFromBlastLinkID.Trim().Length > 0))
            {
                PageLoadBlasIdLinkUrl();
            }
        }

        private void PageLoadMaintenanceMode()
        {
            if (LinkFromURL.StartsWith(MailTo, StringComparison.OrdinalIgnoreCase))
            {
                pnlCloseBrowser.Visible = true;
                Response.Write($"{JavascriptWindowLocation}{LinkFromURL.Replace(SingleQuote, DoubleSlash)}{ScriptClose}");
            }
            else
            {
                if (UserAgent.IndexOf(BlackBerry, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Response.Redirect(LinkFromURL, false);
                }
                else
                {
                    Response.Write($"{JavascriptWindowLocation}{LinkFromURL.Replace(SingleQuote, DoubleSlash)}{ScriptClose}");
                }
            }
        }

        private void PageLoadBlasIdLinkUrl()
        {
            RedirectOnError = true;

            if (LinkFromBlastLinkID.Length == 0)
            {
                DecodeURL = true;

                if (LinkFromURL.Length == 0)
                {
                    throw new InvalidOperationException(BlastLinkIdOrUrlEmpty);
                }

                try
                {
                    ApplicationLog.LogNonCriticalError(
                        string.Format(BlastLinkUrlQueryString, BlastLinkID, LinkFromBlastLinkID),
                        LinkFromPageLoad,
                        ConfigurationManager.AppSettings[KmCommonApplication].IntTryParse(),
                        CreateNote());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }

                LinkToStore = LinkFromURL;
            }

            if ((ConfigurationManager.AppSettings[ValidateB4Tracking]
                     .Equals(TrueString, StringComparison.OrdinalIgnoreCase)
                 && EmailGroup.ValidForTracking(RefBlastID, EmailID))
                || !ConfigurationManager.AppSettings[ValidateB4Tracking]
                    .Equals(TrueString, StringComparison.OrdinalIgnoreCase))
            {
                if (TrackData() > 0 && ContainsTopics(LinkToStore))
                {
                    LogTransactionalUDF();
                }
            }

            Redirect();
        }

        #region Get Request Variables
        private int getBlastID()
        {
            try
            {
                // Added this 'cos the customers who use custom UnSubscribe links have 2 blast params ('b=').
                // one for this handler & the other in the link param which will redirect to the Unsubscribe hander.
                // this throws an exception. So get the first blastID from the URL.
                if (Request.QueryString["b"].ToString().IndexOf(",") > 0)
                {
                    return Convert.ToInt32(Request.QueryString["b"].ToString().Substring(0, Request.QueryString["b"].ToString().IndexOf(",")));
                }
                else
                {
                    return Convert.ToInt32(Request.QueryString["b"].ToString());
                }
            }
            catch
            {
                return 0;
            }
        }

        private int getBlastLinkID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["lid"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getUniqueLinkID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["ulid"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private bool ContainsTopics(string link)
        {
            bool found = false;
            try
            {
                if (link.ToLower().Contains("topic="))
                {
                    found = true;
                }
            }
            catch
            {
            }
            return found;
        }

        private int getEmailID()
        {
            try
            {
                // Added this 'cos the customers who use custom UnSubscribe links have 2 blast params ('e=').
                // one for this handler & the other in the link param which will redirect to the Unsubscribe hander.
                // this throws an exception. So get the first emailID from the URL.
                if (Request.QueryString["e"].ToString().IndexOf(",") > 0)
                {
                    return Convert.ToInt32(Request.QueryString["e"].ToString().Substring(0, Request.QueryString["e"].ToString().IndexOf(",")));
                }
                else
                {
                    return Convert.ToInt32(Request.QueryString["e"].ToString());
                }
            }
            catch
            {
                return 0;
            }
        }

        private string getLink()
        {
            int indexOfl = 0;
            String theLink = string.Empty;
            try
            {
                theLink = Request.RawUrl.ToString();
                indexOfl = theLink.IndexOf("l=");
                int length = theLink.Length;
                if (indexOfl >= 0)
                {
                    theLink = Server.UrlDecode(LinkCleanUP(theLink.Substring(indexOfl + 2, (theLink.Length - indexOfl) - 2)));
                    //theLink = theLink.Replace("%%ConversionTrkCDE%%", "");
                }
                else
                {
                    theLink = string.Empty;
                }
            }
            catch (Exception)
            {
            }

            return theLink.Trim();
        }
        #endregion

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
            var linkToRedirect = LinkToStore;

            var usePatch = ConfigurationManager.AppSettings[UsePatchForDouble];

            if (usePatch.EqualsIgnoreCase(FalseString))
            {
                linkToRedirect = AppendTrackingLink(LinkToStore, false);
            }
            else if (usePatch.EqualsIgnoreCase(TrueString) && !linkToRedirect.ContainsIgnoreCase(DoubleClick))
            {
                linkToRedirect = AppendTrackingLink(LinkToStore, true);
            }

            return linkToRedirect;
        }

        private string AppendTrackingLink(string link, bool trimAmpersand)
        {
            var linkToRedirect = new StringBuilder(link);
            var conversionTrackingExists = linkToRedirect.ToString().Contains(ConversionTrkCde);

            linkToRedirect = link.Contains(DoublePercentage)
                                 ? new StringBuilder(replaceUDFWithValue(link))
                                 : new StringBuilder(link);

            var campaignItem = CampaignItem.GetByBlastID_NoAccessCheck(BlastID, false);

            if (campaignItem != null)
            {
                foreach (var linkTracking in LinkTracking.GetByCampaignItemID(campaignItem.CampaignItemID))
                {
                    if (campaignItem.CustomerID != null && LinkTracking.CreateLinkTrackingParams(campaignItem.CustomerID.Value, getDomainName(linkToRedirect.ToString()), linkTracking.LTID))
                    {
                        var dataTable = CampaignItemLinkTracking.GetParamInfo(BlastID, linkTracking.LTID);

                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            linkToRedirect.Append(
                                linkToRedirect.ToString().Contains(QuestionMark)
                                    ? Ampersand
                                    : QuestionMark);
                            linkToRedirect.Append(CreateTrackingLink(dataTable, conversionTrackingExists));

                            if (trimAmpersand && linkToRedirect.ToString().EndsWith(Ampersand, StringComparison.Ordinal))
                            {
                                linkToRedirect = new StringBuilder(linkToRedirect.ToString().Remove(linkToRedirect.ToString().LastIndexOf(Ampersand, StringComparison.Ordinal)));
                            }
                        }
                    }
                }

                AppendTrackingLinkCustomer(campaignItem, linkToRedirect);
            }

            return linkToRedirect.ToString();
        }

        private void AppendTrackingLinkCustomer(ECN_Framework_Entities.Communicator.CampaignItem campaignItem, StringBuilder linkToRedirect)
        {
            if (campaignItem.CustomerID != null)
            {
                var campaignItemCustomer = Customer.GetByCustomerID(campaignItem.CustomerID.Value, false);

                if (campaignItemCustomer.BaseChannelID != null)
                {
                    var bc = BaseChannel.GetByBaseChannelID(campaignItemCustomer.BaseChannelID.Value);

                    if (ClientGroup.HasServiceFeature(bc.PlatformClientGroupID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DomainSetup)
                        && DomainTracker.Exists(getDomainName(linkToRedirect.ToString()), campaignItemCustomer.BaseChannelID.Value))
                    {
                        var redirectUri = new Uri(linkToRedirect.ToString());
                        var eidValue = HttpUtility.ParseQueryString(redirectUri.Query).Get(Eid);

                        if (eidValue == null)
                        {
                            linkToRedirect.Append(
                                linkToRedirect.ToString().Contains(QuestionMark)
                                    ? Ampersand
                                    : QuestionMark);
                            linkToRedirect.Append($"{Eid}={EmailID}");
                        }

                        var bidValue = HttpUtility.ParseQueryString(redirectUri.Query).Get(Bid);

                        if (bidValue == null)
                        {
                            linkToRedirect.Append(
                                linkToRedirect.ToString().Contains(QuestionMark)
                                    ? Ampersand
                                    : QuestionMark);
                            linkToRedirect.Append($"{Bid}={BlastID}");
                        }
                    }
                }
            }
        }

        private string getDomainName(string url)
        {
            try
            {
                Uri myUri = new Uri(url);
                return myUri.Host;
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
                Response.Redirect("~/Error.aspx?E=" + ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink.ToString(), false);
                return string.Empty;
            }
        }

        private void Redirect()
        {
            //code for anchor tags in URL Jwelter 05/06/2014
            string anchor = "";
            bool hasAnchor = false;
            try
            {
                if (LinkToStore.Contains("#") && !LinkToStore.StartsWith("mailto:"))
                {
                    anchor = LinkToStore.Substring(LinkToStore.IndexOf("#"));
                    hasAnchor = true;
                }
                else if (LinkToStore.Contains("%23") && !LinkToStore.StartsWith("mailto:"))
                {
                    anchor = LinkToStore.Substring(LinkToStore.IndexOf("%23"));
                    hasAnchor = true;
                }
            }
            catch { }
            string linkToRedirect = CreateRedirectLink();
            if (hasAnchor)
            {
                linkToRedirect = linkToRedirect.Replace(anchor, "");
                linkToRedirect += anchor;
            }

            if (linkToRedirect.ToLower().ToString().StartsWith("mailto:"))
            {
                // The following code will open a blank send email window when some one clicks on the mailto link in an email. But it will open a blank page. To 
                // avoid it, we are redirecting back to www.ecn5.com. We can put some sensible text in this same page as well.. 
                pnlCloseBrowser.Visible = true;
                Response.Write("<script language='javascript'>window.location.href='" + linkToRedirect.Replace("'", "\\'") + "';</script>");
                Response.Write("<script language='javascript'>window.close();</script>");
                //ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "closePage", "window.close();");
            }
            else
            {
                //The following is a patch for BlackBerry users.. it didn't support the java script method of recirecting the URL's. the following will do a straight redirect to the link.                        
                if (UserAgent.ToLower().Contains("blackberry"))
                {
                    Response.Redirect(linkToRedirect, false);
                }
                else
                {
                    Response.Write("<script language='javascript'>window.location.href='" + linkToRedirect.Replace("'", "\\'") + "'</script>");
                }
            }
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
                adminEmailVariables.AppendLine("<BR>URLFromQueryString: " + LinkFromURL);
                adminEmailVariables.AppendLine("<BR>LinkToStore: " + LinkToStore);
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

        private string CreateTrackingLink(DataTable dataTable, bool conversionTrackingExists)
        {
            const int TrackingLinkLtid = 3;
            var omniQs = string.Empty;
            var omniDelimiter = string.Empty;
            var hasOmniture = HasOmniture(dataTable, TrackingLinkLtid, ref omniQs, ref omniDelimiter);
            var omniParams = new string[10];
            var queryStringParams = new StringBuilder();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (!dataRow[ColumnName].ToString().ContainsIgnoreCase(Omniture))
                {
                    BuildQsParams(conversionTrackingExists, dataRow, queryStringParams);
                }
                else if (hasOmniture)
                {
                    switch (dataRow[ColumnName].ToString().ToLower())
                    {
                        case Omniture1:
                        case Omniture2:
                        case Omniture3:
                        case Omniture4:
                        case Omniture5:
                        case Omniture6:
                        case Omniture7:
                        case Omniture8:
                        case Omniture9:
                        case Omniture10:
                            BuildOmniParams(dataRow, omniParams);
                            break;
                    }
                }
            }

            return QsParamsString(hasOmniture, omniParams, omniDelimiter, omniQs, queryStringParams);
        }

        private void BuildQsParams(bool conversionTrackingExists, DataRow dataRow, StringBuilder queryStringParams)
        {
            Action<object, Exception> logException = (columnName, ex) =>
                {
                    System.Diagnostics.Trace.TraceError(ex.ToString());
                    queryStringParams.Append(Unknown);
                    ApplicationLog.LogNonCriticalError(
                        $"{columnName}{NotFoundForLinkTracking}",
                        LinkFromCreateTrackingLink,
                        ConfigurationManager.AppSettings[KmCommonApplication].IntTryParse(),
                        CreateNote());
                };

            var columnValue = dataRow[ColumnName].ToString();

            switch (columnValue)
            {
                case KnowledgeMarketing:
                    queryStringParams.Append(dataRow[DisplayName]).Append(EqualsOperator).Append($"{KnowledgeMarketing}&");
                    break;
                case FolderName:
                case GroupName:
                case LayoutName:
                case EmailSubject:
                    queryStringParams.Append(dataRow[DisplayName] + EqualsOperator);

                    try
                    {
                        var paramValue = Blast.GetLinkTrackingParam(BlastID, columnValue);
                        queryStringParams.Append(paramValue);
                    }
                    catch (Exception ex)
                    {
                        logException(columnValue, ex);
                    }

                    queryStringParams.Append(Ampersand);
                    break;
                case CustomValue:
                    queryStringParams.Append(dataRow[DisplayName]).Append(EqualsOperator);

                    try
                    {
                        queryStringParams.Append(dataRow[CustomValue].ToString().Replace(Space, UrlSpace));
                    }
                    catch (Exception ex)
                    {
                        logException(CustomValue, ex);
                    }

                    queryStringParams.Append(Ampersand);
                    break;
                case nameof(BlastID):
                    queryStringParams.Append(dataRow[DisplayName]).Append(EqualsOperator);

                    try
                    {
                        queryStringParams.Append(BlastID.ToString());
                    }
                    catch (Exception ex)
                    {
                        logException(nameof(BlastID), ex);
                    }

                    queryStringParams.Append(Ampersand);
                    break;
                case Eid:
                case Bid:

                    if (!conversionTrackingExists)
                    {
                        var redirectUri = new Uri(LinkToStore);
                        var queryStringValue = HttpUtility.ParseQueryString(redirectUri.Query)
                            .Get(
                                columnValue == Eid
                                    ? Eid
                                    : Bid);

                        if (queryStringValue == null)
                        {
                            queryStringParams.Append(dataRow[DisplayName])
                                .Append(EqualsOperator)
                                .Append(
                                    columnValue == Eid
                                        ? EmailID.ToString()
                                        : BlastID.ToString())
                                .Append(Ampersand);
                        }
                    }

                    break;
            }
        }

        private string QsParamsString(bool hasOmniture, string[] omniParams, string omniDelimiter, string omniQs, StringBuilder queryStringParams)
        {
            if (hasOmniture)
            {
                var finalOmni = new StringBuilder();

                for (var omniParamIndex = omniParams.Length - 1; omniParamIndex >= 0; omniParamIndex--)
                {
                    if (!string.IsNullOrWhiteSpace(omniParams[omniParamIndex]))
                    {
                        finalOmni.Insert(0, $"{omniDelimiter}{EncodeParamValue(omniParams[omniParamIndex])}");
                    }
                }

                if (omniQs.Length >= 1)
                {
                    omniQs += finalOmni.ToString().Remove(0, 1);
                    queryStringParams.Append($"{omniQs}{Ampersand}");
                }
            }

            var queryStringParamsString = queryStringParams.ToString();

            if (queryStringParamsString.Length >= 1)
            {
                queryStringParamsString = queryStringParamsString.Remove(queryStringParamsString.Length - 1);
            }

            return queryStringParamsString;
        }

        private void BuildOmniParams(DataRow row, string[] omniParams)
        {
            var paramNumber = row[ColumnName].ToString().Substring(8).IntTryParse() - 1;

            try
            {
                if (row[Ltpoid].ToString().Equals("-1"))
                {
                    omniParams[paramNumber] = row[CustomValue].ToString().Replace(Space, UrlSpace);
                }
                else
                {
                    var ltpo = LinkTrackingParamOption.GetByLTPOID(row[Ltpoid].ToString().IntTryParse());

                    if (ltpo.IsDynamic)
                    {
                        switch (ltpo.Value.ToLower())
                        {
                            case BlastIdSmall:

                                try
                                {
                                    omniParams[paramNumber] = BlastID.ToString();
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Trace.TraceError(ex.ToString());
                                    ApplicationLog.LogNonCriticalError(
                                        $"{nameof(BlastID)}{NotFoundForLinkTracking}",
                                        LinkFromCreateTrackingLink,
                                        ConfigurationManager.AppSettings[KmCommonApplication].IntTryParse());
                                    omniParams[0] = Unknown;
                                }

                                break;
                            case GroupNameSmall:
                                var groupName = string.Empty;

                                try
                                {
                                    groupName = Blast.GetLinkTrackingParam(BlastID, ltpo.Value);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Trace.TraceError(ex.ToString());
                                    omniParams[paramNumber] = Unknown;
                                    ApplicationLog.LogNonCriticalError(
                                        $"{GroupName}{NotFoundForLinkTracking}",
                                        LinkFromCreateTrackingLink,
                                        ConfigurationManager.AppSettings[KmCommonApplication].IntTryParse());
                                }

                                omniParams[paramNumber] = groupName;
                                break;
                        }
                    }
                    else
                    {
                        omniParams[paramNumber] = ltpo.Value.Replace(Space, UrlSpace);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        private bool HasOmniture(DataTable dataTable, int trackingLinkLtid, ref string omniQs, ref string omniDelimiter)
        {
            var hasOmniture = false;

            if (dataTable.Rows[0][Ltid].ToString().Equals(trackingLinkLtid.ToString()))
            {
                var campaignItem = CampaignItem.GetByBlastID_NoAccessCheck(BlastID, false);

                if (campaignItem.CustomerID != null)
                {
                    var customer = Customer.GetByCustomerID(campaignItem.CustomerID.Value, false);

                    if (customer.BaseChannelID != null)
                    {
                        var ltsBase = LinkTrackingSettings.GetByBaseChannelID_LTID(customer.BaseChannelID.Value, trackingLinkLtid);
                        var ltsCustomer = LinkTrackingSettings.GetByCustomerID_LTID(customer.CustomerID, trackingLinkLtid);

                        try
                        {
                            var baseDoc = new XmlDocument();
                            var custDoc = new XmlDocument();
                            baseDoc.LoadXml(ltsBase.XMLConfig);

                            try
                            {
                                custDoc.LoadXml(ltsCustomer.XMLConfig);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.TraceError(ex.ToString());
                            }

                            var baseRoot = baseDoc.SelectSingleNode(Settings);

                            XmlNode custRoot = null;

                            try
                            {
                                custRoot = custDoc.SelectSingleNode(Settings);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.TraceError(ex.ToString());
                            }

                            if (baseRoot?[AllowCustomerOverride] == null || baseRoot[Delimiter] == null || baseRoot[nameof(QueryString)] == null)
                            {
                                throw new NullReferenceException(nameof(baseRoot));
                            }

                            if (custRoot?[Override] == null || custRoot[Delimiter] == null || custRoot[nameof(QueryString)] == null)
                            {
                                throw new NullReferenceException(nameof(custRoot));
                            }

                            var allowCustomerOverride = baseRoot[AllowCustomerOverride].InnerText.Equals(TrueString, StringComparison.OrdinalIgnoreCase);
                            hasOmniture = true;
                            var overrideBaseChannel = custRoot[Override].InnerText.Equals(TrueString, StringComparison.OrdinalIgnoreCase);

                            if (allowCustomerOverride && overrideBaseChannel)
                            {
                                omniQs = $"{custRoot[nameof(QueryString)]?.InnerText.Trim()}{EqualsOperator}";
                                omniDelimiter = custRoot[Delimiter].InnerText.Trim();
                            }
                            else
                            {
                                omniQs = $"{baseRoot[nameof(QueryString)]?.InnerText.Trim()}{EqualsOperator}";
                                omniDelimiter = baseRoot[Delimiter].InnerText.Trim();
                            }

                            try
                            {
                                var linkSub = LinkToStore.Substring(LinkToStore.IndexOf('?'));

                                if (linkSub.Length > 0 && linkSub.ContainsIgnoreCase(omniQs))
                                {
                                    hasOmniture = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.TraceError(ex.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.TraceError(ex.ToString());
                            hasOmniture = false;
                        }
                    }
                }
            }

            return hasOmniture;
        }

        private string EncodeParamValue(string paramValue)
        {
            return HttpContext.Current.Server.UrlEncode(paramValue);
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
            catch (Exception)
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
                adminEmailBody.AppendLine("<BR>URLFromQueryString: " + LinkFromURL);
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
            int eaid = 0;
            string spyinfo = string.Empty;

            //spyinfo = Request.UserHostAddress + " | " + Request.UserAgent;
            try
            {
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
                string urlToInsert = string.Empty;
                if(DecodeURL)
                {
                    urlToInsert = Server.UrlDecode(LinkToStore.Replace("'", "''"));
                }
                else
                {
                    urlToInsert = LinkToStore.Replace("'", "''");
                }
                eaid = ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertClick(EmailID, BlastID, urlToInsert, spyinfo, UniqueLinkID, User);
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
            return eaid;
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

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
        }
        #endregion
    }
}

