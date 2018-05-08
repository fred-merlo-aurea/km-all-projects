using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DiagnosticsTrace = System.Diagnostics.Trace;
using KM.Common;
using ApplicationLog = KM.Common.Entity.ApplicationLog;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessUser = KMPlatform.BusinessLogic.User;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using WebCaching = System.Web.Caching;

namespace ecn.activityengines
{
    public partial class pubSubscriptions_SO_subscribe : System.Web.UI.Page
    {
        private const string AppSettingCommonApplication = "KMCommon_Application";

        private const string CacheUserByAccessKeyTemplate = "cache_user_by_AccessKey_{0}";
        private const string AppSettingEcnEngineAccessKey = "ECNEngineAccessKey";

        private const string UserTransactionType = "user_TRANSACTIONTYPE";
        private const string UserDemo7 = "user_DEMO7";

        private const int EmailMinLength = 5;
        private const string SchemeHttp = "http://";

        private const string KmpsPageSettingsTemplate = "kmps_g_{0}";
        
        private const string ErrorLocationRedirect = "pubSubscriptions_SO_subscribe.Page_Load(Canon Redirect)";
        private const string ErrorLocation = "pubSubscriptions_SO_subscribe.Page_Load";
        private const string ErrorUnknownGroupOrEmail = "Unknown GroupID: {0} or EmailID: {1}";

        private KMPlatform.Entity.User User = null;
        private int CustomerID = 0;
        private int GroupID = 0;
        private int SmartFormID = 0;
        private string EmailAddress = string.Empty;
        private string MasterEmailAddress = string.Empty;
        private int EmailID = 0;
        private string Format = string.Empty;
        private string SubscribeTypeCode = string.Empty;

        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(15);

        Hashtable hProfileFields = new Hashtable();
        Hashtable hUDFFields = new Hashtable();        

        protected void Page_Load(object sender, EventArgs e)
        {
            PageLoadInitFields();

            var transType = getUDF(UserTransactionType);
            var demo7 = getUDF(UserDemo7);
            MsgLabel.Visible = false;
            var okToProcess = false;
            var saveAltEmail = false;
            EntitiesCommunicator.SmartFormsHistory smartFormHistory = null;
            var conversionTrackingBlastId = string.Empty;
            var conversionTrackingEmailId = string.Empty;
            EntitiesCommunicator.Group communicatorGroup = null;
            EntitiesCommunicator.Email email = null;

            var commonApplication = GetCommonApplication();

            var pageLoadProcess = new ProcessLoadArgs();
            try
            {
                GetEmailAddress();

                pageLoadProcess = new ProcessLoadArgs
                {
                    SaveAltEmail = saveAltEmail,
                    TransType = transType,
                    Demo7 = demo7,
                    CommonApplication = commonApplication,
                    Email = email,
                    CommunicatorGroup = communicatorGroup,
                    SmartFormHistory = smartFormHistory,
                    ConversionTrackingBlastId = conversionTrackingBlastId,
                    ConversionTrackingEmailId = conversionTrackingEmailId
                };
                PageLoadProcess(pageLoadProcess);
            }
            catch (Exception ex)
            {
                PageLoadError(ex, pageLoadProcess);
            }
        }

        private void PageLoadError(Exception exception, ProcessLoadArgs processErrorArgs)
        {
            Guard.NotNull(processErrorArgs, nameof(processErrorArgs));

            ApplicationLog.LogCriticalError(
                exception,
                ErrorLocation,
                processErrorArgs.CommonApplication,
                CreateNote());

            if (processErrorArgs.SmartFormHistory != null &&
                processErrorArgs.SmartFormHistory.Response_UserScreen.StartsWith(SchemeHttp, StringComparison.OrdinalIgnoreCase))
            {
                if (processErrorArgs.CommunicatorGroup != null && processErrorArgs.Email != null)
                {
                    processErrorArgs.SmartFormHistory.Response_UserScreen = 
                        ReplaceCodeSnippets(
                            processErrorArgs.CommunicatorGroup, 
                            processErrorArgs.Email, 
                            processErrorArgs.SmartFormHistory.Response_UserScreen);
                    var scriptEmail = processErrorArgs.SaveAltEmail ? MasterEmailAddress : EmailAddress;
                    var responseSscript = string.Format(
                        "<script>document.location.href='{0}?ctrk_eid={1}&ctrk_bid={2}&ead={3}&demo7={4}';</script>",
                        processErrorArgs.SmartFormHistory.Response_UserScreen, 
                        processErrorArgs.ConversionTrackingEmailId, 
                        processErrorArgs.ConversionTrackingBlastId, 
                        scriptEmail,
                        processErrorArgs.Demo7);
                    Response.Write(responseSscript);
                }
            }
            else if (processErrorArgs.SmartFormHistory.Response_UserScreen.Trim().Length > 0)
            {
                if (processErrorArgs.CommunicatorGroup != null && processErrorArgs.Email != null)
                {
                    processErrorArgs.SmartFormHistory.Response_UserScreen =
                        ReplaceCodeSnippets(processErrorArgs.CommunicatorGroup, processErrorArgs.Email, processErrorArgs.SmartFormHistory.Response_UserScreen);
                    Response.Write(processErrorArgs.SmartFormHistory.Response_UserScreen);
                }
            }
            else
            {
                MsgLabel.Visible = true;
            }
        }

        private void PageLoadProcess(ProcessLoadArgs processLoadArgs)
        {
            Guard.NotNull(processLoadArgs, nameof(processLoadArgs));

            bool okToProcess;
            if (GroupID > 0 && BusinessCommunicator.Group.GetByGroupID_NoAccessCheck(GroupID) != null)
            {
                var saveAtEmail = processLoadArgs.SaveAltEmail;
                okToProcess = GetOkToProcess(ref saveAtEmail);
                processLoadArgs.SaveAltEmail = saveAtEmail;

                if (okToProcess)
                {
                    SubscribeToGroup(processLoadArgs.SaveAltEmail);
                    var oldEmail = BusinessCommunicator.EmailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(
                        GroupID,
                        CustomerID,
                        EmailAddress);

                    if (SmartFormID > 0)
                    {
                        processLoadArgs.Email = BusinessCommunicator.Email.GetByEmailID_NoAccessCheck(oldEmail);
                        processLoadArgs.CommunicatorGroup = BusinessCommunicator.Group.GetByGroupID_NoAccessCheck(GroupID);
                        PageLoadSendEmails(processLoadArgs, oldEmail);
                    }
                }
            }
            else
            {
                ApplicationLog.LogNonCriticalError(
                    "Unknown GroupID: " + GroupID,
                    ErrorLocation,
                    processLoadArgs.CommonApplication,
                    CreateNote());
            }

            processLoadArgs.ReturnValue = processLoadArgs.SaveAltEmail;
        }

        private void PageLoadSendEmails(ProcessLoadArgs processLoadArgs, int oldEmail)
        {
            Guard.NotNull(processLoadArgs, nameof(processLoadArgs));

            if (processLoadArgs.Email != null && processLoadArgs.CommunicatorGroup != null)
            {
                processLoadArgs.SmartFormHistory = BusinessCommunicator.SmartFormsHistory.GetBySmartFormID_NoAccessCheck(
                    SmartFormID,
                    GroupID);

                SendUserResponseEmails(processLoadArgs.SmartFormHistory, processLoadArgs.CommunicatorGroup,
                    processLoadArgs.Email);
                SendAdminResponseEmails(processLoadArgs.SmartFormHistory, processLoadArgs.CommunicatorGroup,
                    processLoadArgs.Email);

                processLoadArgs.ConversionTrackingBlastId = getConversionTrackingBlastID();
                processLoadArgs.ConversionTrackingEmailId = getConversionTrackingEmailID();

                if (GroupID > 0)
                {
                    try
                    {
                        var pageId = string.Empty;

                        try
                        {
                            pageId = ConfigurationManager.AppSettings[
                                string.Format(KmpsPageSettingsTemplate, GroupID)];
                        }
                        catch (Exception ex)
                        {
                            DiagnosticsTrace.TraceError(ex.ToString());
                        }

                        PageLoadRedirect(pageId, oldEmail, processLoadArgs);
                    }
                    catch (Exception exception)
                    {
                        ApplicationLog.LogCriticalError(
                            exception,
                            ErrorLocationRedirect,
                            processLoadArgs.CommonApplication,
                            CreateNote());
                    }
                }

                FinallyShowUser(processLoadArgs);
            }
            else
            {
                ApplicationLog.LogNonCriticalError(
                    string.Format(ErrorUnknownGroupOrEmail, GroupID, EmailID),
                    ErrorLocation,
                    processLoadArgs.CommonApplication,
                    CreateNote());
            }
        }

        private void GetEmailAddress()
        {
            if (EmailAddress.Trim().Length < EmailMinLength)
            {
                EmailAddress = getDummyEmailAddress();
            }
        }

        private void PageLoadInitFields()
        {
            PageLoadProcessQueryString();

            var smartFormTracking = new EntitiesCommunicator.SmartFormTracking();
            if (CustomerID != 0)
            {
                smartFormTracking.CustomerID = CustomerID;
            }

            if (SmartFormID != 0)
            {
                smartFormTracking.SFID = SmartFormID;
            }

            if (GroupID != 0)
            {
                smartFormTracking.GroupID = GroupID;
            }

            smartFormTracking.ReferringUrl = Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString();
            smartFormTracking.ActivityDate = DateTime.Now;
            BusinessCommunicator.SmartFormTracking.Insert(smartFormTracking);

            GetCachedUser();
        }

        private void PageLoadProcessQueryString()
        {
            if (Request.Url.Query.Length > 0)
            {
                GetValuesFromQuerystring(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            }
        }

        private void PageLoadRedirect(string pageId, int oldEmail, ProcessLoadArgs processLoadArgs)
        {
            Guard.NotNull(processLoadArgs, nameof(processLoadArgs));

            if (!string.IsNullOrWhiteSpace(pageId))
            {
                var builder = new StringBuilder();
                builder.Append("http://serve.ifficient.com/asp/serve.aspx?w=5854");
                builder.AppendFormat("&CRID={0}", pageId);
                builder.AppendFormat("&f={0}", Server.UrlEncode(getFirstName()));
                builder.AppendFormat("&l={0}", Server.UrlEncode(getLastName()));
                builder.AppendFormat("&m={0}", Server.UrlEncode(getCompanyName()));
                builder.AppendFormat("&e={0}", Server.UrlEncode(EmailAddress));
                builder.AppendFormat("&a1={0}", Server.UrlEncode(getStreetAddress()));
                builder.AppendFormat("&a2={0}", Server.UrlEncode(getStreetAddress2()));
                builder.AppendFormat("&ci={0}", Server.UrlEncode(getCity()));
                builder.AppendFormat("&st={0}", Server.UrlEncode(getState()));
                builder.AppendFormat("&zi={0}", Server.UrlEncode(getZipCode()));
                builder.AppendFormat("&co={0}", Server.UrlEncode(getCountry()));
                builder.AppendFormat("&db={0}", Server.UrlEncode(getTitle()));
                builder.AppendFormat("&ph={0}", Server.UrlEncode(getPhone()));
                builder.AppendFormat("&ip={0}", Request.ServerVariables["REMOTE_ADDR"].ToString());
                builder.AppendFormat(
                    "&wlp={0}",
                    processLoadArgs.SmartFormHistory.Response_UserScreen.StartsWith(SchemeHttp, StringComparison.OrdinalIgnoreCase)
                        ? Server.UrlEncode(
                            string.Format(
                                "{0}?e={1}&g={2}&type={3}&ctrk_eid={4}&ctrk_bid={5}&ead={6}&demo7={7}",
                                processLoadArgs.SmartFormHistory.Response_UserScreen,
                                oldEmail,
                                GroupID,
                                processLoadArgs.TransType,
                                processLoadArgs.ConversionTrackingEmailId,
                                processLoadArgs.ConversionTrackingBlastId,
                                MasterEmailAddress,
                                processLoadArgs.Demo7))
                        : string.Empty);

                var url = builder.ToString();
                Response.Redirect(url, true);
            }
        }

        private void FinallyShowUser(ProcessLoadArgs processLoadArgs)
        {
            Guard.NotNull(processLoadArgs, nameof(processLoadArgs));

            if (processLoadArgs.SmartFormHistory.Response_UserScreen.StartsWith(SchemeHttp, StringComparison.OrdinalIgnoreCase))
            {
                processLoadArgs.SmartFormHistory.Response_UserScreen = ReplaceCodeSnippets(
                    processLoadArgs.CommunicatorGroup,
                    processLoadArgs.Email, processLoadArgs.SmartFormHistory.Response_UserScreen);

                var responseEmail = processLoadArgs.SaveAltEmail ? MasterEmailAddress : EmailAddress;
                var scriptHtml =
                    string.Format(
                        "<script>document.location.href='{0}?ctrk_eid={1}&ctrk_bid={2}&ead={3}&demo7={4}';</script>",
                        processLoadArgs.SmartFormHistory.Response_UserScreen,
                        processLoadArgs.ConversionTrackingEmailId,
                        processLoadArgs.ConversionTrackingBlastId,
                        responseEmail,
                        processLoadArgs.Demo7);
                Response.Write(scriptHtml);
            }
            else if (processLoadArgs.SmartFormHistory.Response_UserScreen.Trim().Length > 0)
            {
                processLoadArgs.SmartFormHistory.Response_UserScreen =
                    ReplaceCodeSnippets(
                        processLoadArgs.CommunicatorGroup, 
                        processLoadArgs.Email, 
                        processLoadArgs.SmartFormHistory.Response_UserScreen);
                Response.Write(processLoadArgs.SmartFormHistory.Response_UserScreen);
            }
            else
            {
                MsgLabel.Visible = true;
            }
        }

        private bool GetOkToProcess(ref bool saveAltEmail)
        {
            bool okToProcess;
            if (EmailID > 0)
            {
                var oldEmail = BusinessCommunicator.EmailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(
                    GroupID,
                    CustomerID,
                    EmailAddress);
                if (oldEmail <= 0)
                {
                    okToProcess = true;
                }
                else
                {
                    if (oldEmail == EmailID)
                    {
                        okToProcess = true;
                    }
                    else
                    {
                        EmailAddress = getDummyEmailAddress();
                        saveAltEmail = true;
                        okToProcess = true;
                    }
                }
            }
            else
            {
                okToProcess = true;
            }

            return okToProcess;
        }

        private static int GetCommonApplication()
        {
            int commonApplication;
            if (!int.TryParse(
                ConfigurationManager.AppSettings[AppSettingCommonApplication],
                out commonApplication))
            {
                throw new InvalidOperationException();
            }

            return commonApplication;
        }

        private void GetCachedUser()
        {
            var engineAccessKey = ConfigurationManager.AppSettings[AppSettingEcnEngineAccessKey];
            var cacheKey = string.Format(CacheUserByAccessKeyTemplate, engineAccessKey);
            if (Cache[cacheKey] == null)
            {
                User = BusinessUser.GetByAccessKey(engineAccessKey);
                Cache.Add(
                    cacheKey,
                    User,
                    null,
                    WebCaching.Cache.NoAbsoluteExpiration,
                    CacheExpiration,
                    WebCaching.CacheItemPriority.Normal,
                    null);
            }
            else
            {
                User = (KMPlatform.Entity.User) Cache[cacheKey];
            }
        }

        #region Get Request Variables methods
        private void GetValuesFromQuerystring(string queryString)
        {
            ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(QSCleanUP(queryString)));
            try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.CustomerID).ParameterValue, out CustomerID); }
            catch (Exception) { }
            try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.GroupID).ParameterValue, out GroupID); }
            catch (Exception) { }
            try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.SmartFormID).ParameterValue, out SmartFormID); }
            catch (Exception) { }
            try { EmailAddress = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailAddress).ParameterValue; }
            catch (Exception) { }
            MasterEmailAddress = EmailAddress;
            try { SubscribeTypeCode = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Subscribe).ParameterValue; }
            catch (Exception) { }
            try { Format = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Format).ParameterValue; }
            catch (Exception) { }
            //try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.BlastID).ParameterValue, out BlastID); }
            //catch (Exception) { }
            //try { ReturnURL = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.URL).ParameterValue; }
            //catch (Exception) { }
            try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailID).ParameterValue, out EmailID); }
            catch (Exception) { }
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

        private int getBlastID()
        {
            int theBlastID = 0;
            try
            {
                theBlastID = Convert.ToInt32(Request.QueryString["b"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theBlastID;
        }
        private string getReturnURL()
        {
            string theReturnURL = "";
            try
            {
                theReturnURL = Request.QueryString["url"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theReturnURL;
        }
        private string getTitle()
        {
            string theTitle = "";
            try
            {
                theTitle = Request.QueryString["t"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theTitle;
        }
        private string getFirstName()
        {
            string theFirstName = "";
            try
            {
                theFirstName = Request.QueryString["fn"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theFirstName;
        }
        private string getLastName()
        {
            string theLastName = "";
            try
            {
                theLastName = Request.QueryString["ln"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theLastName;
        }
        private string getFullName()
        {
            string theFullName = "";
            try
            {
                theFullName = Request.QueryString["n"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theFullName;
        }
        private string getStreetAddress()
        {
            string theStreetAddress = "";
            try
            {
                theStreetAddress = Request.QueryString["adr"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theStreetAddress;
        }
        private string getStreetAddress2()
        {
            string theStreetAddress2 = "";
            try
            {
                theStreetAddress2 = Request.QueryString["adr2"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theStreetAddress2;
        }
        private string getCompanyName()
        {
            string theCompanyName = "";
            try
            {
                theCompanyName = Request.QueryString["compname"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCompanyName;
        }
        private string getCity()
        {
            string theCity = "";
            try
            {
                theCity = Request.QueryString["city"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCity;
        }
        private string getState()
        {
            string theState = "";
            try
            {
                theState = Request.QueryString["state"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theState;
        }
        private string getCountry()
        {
            string theCountry = "";
            try
            {
                theCountry = Request.QueryString["ctry"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCountry;
        }
        private string getZipCode()
        {
            string theZipCode = "";
            try
            {
                theZipCode = Request.QueryString["zc"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theZipCode;
        }
        private string getPhone()
        {
            string thePhone = "";
            try
            {
                thePhone = Request.QueryString["ph"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return thePhone;
        }
        private string getMobilePhone()
        {
            string theMobilePhone = "";
            try
            {
                theMobilePhone = Request.QueryString["mph"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theMobilePhone;
        }
        private string getFax()
        {
            string theFax = "";
            try
            {
                theFax = Request.QueryString["fax"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theFax;
        }
        private string getWebsite()
        {
            string theWebsite = "";
            try
            {
                theWebsite = Request.QueryString["website"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theWebsite;
        }
        private string getAge()
        {
            string theAge = "";
            try
            {
                theAge = Request.QueryString["age"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theAge;
        }
        private string getIncome()
        {
            string theIncome = "";
            try
            {
                theIncome = Request.QueryString["income"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theIncome;
        }
        private string getGender()
        {
            string theGender = "";
            try
            {
                theGender = Request.QueryString["gndr"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theGender;
        }
        private string getOccupation()
        {
            string theOccupation = "";
            try
            {
                theOccupation = Request.QueryString["occ"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theOccupation;
        }
        private string getBirthdate()
        {
            string theBirthdate = "";
            try
            {
                theBirthdate = (DateTime.Parse(Request.QueryString["bdt"].ToString())).ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }

            if (theBirthdate.Length == 0)
            {
                //theBirthdate = DateTime.Now.Date.ToString();
            }
            return theBirthdate;
        }
        private string getUser1()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr1"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser2()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr2"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser3()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr3"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser4()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr4"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser5()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr5"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser6()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr6"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUserEvent1()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usrevt1"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUserEvent1Date()
        {
            string theUserEvent1Date = "";
            try
            {
                theUserEvent1Date = (DateTime.Parse(Request.QueryString["usrevtdt1"].ToString())).ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }

            if (theUserEvent1Date.Length == 0)
            {
                //theUserEvent1Date = DateTime.Now.Date.ToString();
            }
            return theUserEvent1Date;
        }
        private string getUserEvent2()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usrevt2"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUserEvent2Date()
        {
            string theUserEvent2Date = "";
            try
            {
                theUserEvent2Date = (DateTime.Parse(Request.QueryString["usrevtdt2"].ToString())).ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }

            if (theUserEvent2Date.Length == 0)
            {
                //theUserEvent2Date = DateTime.Now.Date.ToString();
            }
            return theUserEvent2Date;
        }

        //Get UDF Values.. 
        private string getUDF(string _value)
        {
            string UDFstuff = "";
            try
            {
                UDFstuff = Request.QueryString[_value].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            if (UDFstuff == null || UDFstuff.Length == 0)
            {
                UDFstuff = "";
            }

            return UDFstuff;
        }

        private string getConversionTrackingBlastID()
        {
            string theBlastID = "";
            try
            {
                theBlastID = Request.QueryString["ctrk_bid"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theBlastID;
        }
        private string getConversionTrackingEmailID()
        {
            string theEmailID = "";
            try
            {
                theEmailID = Request.QueryString["ctrk_eid"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theEmailID;
        }
        #endregion        

        #region Send User & Admin Emails
        private void SendUserResponseEmails(ECN_Framework_Entities.Communicator.SmartFormsHistory smartFormHistory, ECN_Framework_Entities.Communicator.Group group, ECN_Framework_Entities.Communicator.Email email)
        {
            if (smartFormHistory.Response_FromEmail.Length > 5 && smartFormHistory.Response_UserMsgSubject.Trim().Length > 0 && smartFormHistory.Response_UserMsgBody.Trim().Length > 0)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = email.EmailAddress;
                ed.EmailSubject = smartFormHistory.Response_UserMsgSubject;
                ed.FromName = "Activity Engine";
                ed.FromEmailAddress = smartFormHistory.Response_FromEmail;
                ed.Process = "Activity Engine - pubSubscriptions_SO_subscribe.SendUserResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = smartFormHistory.Response_FromEmail;
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(group, email, smartFormHistory.Response_UserMsgBody);
                string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + EmailAddress + "&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U'>Unsubscribe</a>.</div></p>";
                ed.Content += unsubscribeText;

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }

        private void SendAdminResponseEmails(ECN_Framework_Entities.Communicator.SmartFormsHistory smartFormHistory, ECN_Framework_Entities.Communicator.Group group, ECN_Framework_Entities.Communicator.Email email)
        {
            if (smartFormHistory.Response_AdminEmail.Length > 5 && smartFormHistory.Response_FromEmail.Length > 5)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = smartFormHistory.Response_AdminEmail;
                ed.EmailSubject = smartFormHistory.Response_AdminMsgSubject;
                ed.FromName = "Activity Engine";
                ed.Process = "Activity Engine - pubSubscriptions_SO_subscribe.SendUserResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = smartFormHistory.Response_FromEmail;
                ed.FromEmailAddress = smartFormHistory.Response_FromEmail;
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(group, email, smartFormHistory.Response_AdminMsgBody);

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }

        private string ReplaceCodeSnippets(ECN_Framework_Entities.Communicator.Group group, ECN_Framework_Entities.Communicator.Email email, string emailbody)
        {
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%GroupID%%", group.GroupID.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%GroupName%%", group.GroupName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%EmailID%%", email.EmailID.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%EmailAddress%%", email.EmailAddress);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Title%%", email.Title);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%FirstName%%", email.FirstName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%LastName%%", email.LastName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%FullName%%", email.FullName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Company%%", email.Company);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Occupation%%", email.Occupation);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Address%%", email.Address);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Address2%%", email.Address2);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%City%%", email.City);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%State%%", email.State);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Zip%%", email.Zip);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Country%%", email.Country);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Voice%%", email.Voice);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Mobile%%", email.Mobile);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Fax%%", email.Fax);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Website%%", email.Website);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Age%%", email.Age);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Income%%", email.Income);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Gender%%", email.Gender);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Notes%%", email.Notes);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%BirthDate%%", email.Birthdate == null ? "" : email.Birthdate.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User1%%", email.User1);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User2%%", email.User2);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User3%%", email.User3);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User4%%", email.User4);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User5%%", email.User5);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User6%%", email.User6);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent1%%", email.UserEvent1);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent1Date%%", email.UserEvent1Date == null ? "" : email.UserEvent1Date.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent2%%", email.UserEvent2);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent2Date%%", email.UserEvent2Date == null ? "" : email.UserEvent2Date.ToString());

            //UDF Data
            ArrayList _keyArrayList = new ArrayList();
            ArrayList _UDFData = new ArrayList();

            if (hUDFFields.Count > 0)
            {
                IDictionaryEnumerator UDFHashEnumerator = hUDFFields.GetEnumerator();
                while (UDFHashEnumerator.MoveNext())
                {
                    string UDFData = "";
                    string _value = UDFHashEnumerator.Value.ToString();
                    string _key = UDFHashEnumerator.Key.ToString();
                    UDFData = getUDF(_value);
                    _keyArrayList.Add(_key);
                    _UDFData.Add(UDFData);
                    emailbody = emailbody.Replace("%%" + _value + "%%", UDFData);
                }
            }

            return emailbody;
        }

        #endregion

        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text.Trim();
        }

        private void SubscribeToGroup(bool saveAltEmail)
        {
            string fromURL = string.Empty;
            try { Request.UrlReferrer.ToString(); }
            catch (Exception) { }

            string fromIP = string.Empty;
            try { Request.UserHostAddress.ToString(); }
            catch (Exception) { }

            LoadFields();
            hUDFFields = GetGroupDataFields(GroupID);

            StringBuilder xmlProfile = new StringBuilder("");
            StringBuilder xmlUDF = new StringBuilder("");

            IDictionaryEnumerator en = hProfileFields.GetEnumerator();

            xmlProfile.Append("<Emails>");

            while (en.MoveNext())
            {
                try
                {
                    if (en.Key.ToString().ToLower() == "notes")
                        xmlProfile.Append("<" + en.Key.ToString() + ">" + "<![CDATA[ [" + fromIP + "] [" + fromURL + "] [" + DateTime.Now.ToString() + "] ]]> " + "</" + en.Key.ToString() + ">");
                    else
                        xmlProfile.Append("<" + en.Key.ToString() + ">" + CleanXMLString(en.Value.ToString()) + "</" + en.Key.ToString() + ">");
                }
                catch
                { }
            }

            xmlProfile.Append("</Emails>");

            if (hUDFFields.Count > 0)
            {
                ArrayList _keyArrayList = new ArrayList();
                ArrayList _UDFData = new ArrayList();
                string altEmailAddress_key = string.Empty;

                IDictionaryEnumerator UDFHashEnumerator = hUDFFields.GetEnumerator();
                while (UDFHashEnumerator.MoveNext())
                {
                    string UDFData = string.Empty;
                    UDFData = getUDF(UDFHashEnumerator.Value.ToString());
                    _keyArrayList.Add(UDFHashEnumerator.Key.ToString());
                    _UDFData.Add(UDFData);
                    if (UDFHashEnumerator.Value.ToString().Equals("user_ALTERNATE_EMAILADDRESS"))
                    {
                        altEmailAddress_key = UDFHashEnumerator.Key.ToString();
                    }
                }
                xmlUDF.Append("<row>");
                xmlUDF.Append("<ea>" + CleanXMLString(EmailAddress) + "</ea>");
                for (int i = 0; i < _UDFData.Count; i++)
                {
                    if ((_keyArrayList[i].ToString() == altEmailAddress_key) && (saveAltEmail))
                    {
                        xmlUDF.Append("<udf id=\"" + _keyArrayList[i].ToString() + "\"><v><![CDATA[" + MasterEmailAddress.ToString() + "]]></v></udf>");
                    }
                    else
                    {
                        xmlUDF.Append("<udf id=\"" + _keyArrayList[i].ToString() + "\"><v><![CDATA[" + _UDFData[i].ToString() + "]]></v></udf>");
                    }
                }
                xmlUDF.Append("</row>");
            }

            try
            {
                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(User, CustomerID, GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", Format, SubscribeTypeCode, false, "", "ActivityEngine.PubSubscriptions_SO_subscribe");

            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("violation of unique key"))
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "OptIn.SubscribeToGroup.UpdateToDB", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote());
                }
            }
        }

       private void LoadFields()
        {
            hProfileFields.Add("emailaddress", EmailAddress);
            hProfileFields.Add("title", "");
            hProfileFields.Add("firstname", getFirstName());
            hProfileFields.Add("lastname", getLastName());
            hProfileFields.Add("fullname", getFullName());
            hProfileFields.Add("company", getCompanyName());
            hProfileFields.Add("occupation", getTitle());
            hProfileFields.Add("address", getStreetAddress());
            hProfileFields.Add("address2", getStreetAddress2());
            hProfileFields.Add("city", getCity());
            hProfileFields.Add("state", getState());
            hProfileFields.Add("zip", getZipCode());
            hProfileFields.Add("country", getCountry());
            hProfileFields.Add("voice", getPhone());
            hProfileFields.Add("mobile", getMobilePhone());
            hProfileFields.Add("fax", getFax());
            hProfileFields.Add("website", getWebsite());
            hProfileFields.Add("age", getAge());
            hProfileFields.Add("income", getIncome());
            hProfileFields.Add("gender", getGender());
            hProfileFields.Add("user1", getUser1());
            hProfileFields.Add("user2", getUser2());
            hProfileFields.Add("user3", getUser3());
            hProfileFields.Add("user4", getUser4());
            hProfileFields.Add("user5", getUser5());
            hProfileFields.Add("user6", getUser6());
            hProfileFields.Add("birthdate", getBirthdate());
            hProfileFields.Add("userevent1", getUserEvent1());
            hProfileFields.Add("userevent1date", getUserEvent1Date());
            hProfileFields.Add("userevent2", getUserEvent2());
            hProfileFields.Add("userevent2date", getUserEvent2Date());
            hProfileFields.Add("notes", "SO Subscription through Website. DateAdded: " + DateTime.Now.ToString());
        }

        private Hashtable GetGroupDataFields(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);

            Hashtable fields = new Hashtable();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
            {
                fields.Add(gdf.GroupDataFieldsID, "user_" + gdf.ShortName.ToLower());
            }

            return fields;
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<br><b>Customer ID:</b>&nbsp;" + CustomerID);
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + EmailID);
                adminEmailVariables.AppendLine("<br><b>Master Email Address:</b>&nbsp;" + EmailAddress);
                adminEmailVariables.AppendLine("<br><b>Smart Form ID:</b>&nbsp;" + SmartFormID);
                adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + GroupID);
                adminEmailVariables.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        private string getDummyEmailAddress()
        {
            string GUID = "", pubCde = "", dummyEmail = "";
            GUID = Guid.NewGuid().ToString();//ECN_Framework_BusinessLayer.Communicator.Email.GetGUIDForDummyEmail();
            pubCde = getUDF("user_PUBLICATIONCODE");
            dummyEmail = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-" + GUID.Substring(0, 6) + "@" + pubCde + ".kmpsgroup.com";

            return dummyEmail;
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