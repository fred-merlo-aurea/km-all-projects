using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data;
using System.Configuration;
using ECN_Framework_Common.Functions;
using System.Net.Http.Headers;

namespace EmailMarketing.API.Controllers.Internal
{
    /// <summary>
    /// REST Services for Domain Tracking  
    /// </summary>

    [RoutePrefix("api/internal/domaintracking")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DomainTrackerController : ApiController
    {
        [Route("VerifyAccountAnon")]
        [HttpPost]
        public VerifyAccountObject VerifyAccountAnon([FromBody]string TrackerKey)
        {
            return verifyAccessAnon(TrackerKey);
        }
        [Route("VerifyAccount")]
        [HttpPost]
        public int VerifyAccount([FromBody]string TrackerKey)
        {
            return verifyAccess(TrackerKey);
        }

        [Route("GetEmailAddress")]
        [HttpPost]
        public string EmailAddress([FromBody]string EmailID)
        {
            ECN_Framework_Entities.Communicator.Email e = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(Convert.ToInt32(EmailID));
            return e != null ? e.EmailAddress : "";
        }

        [Route("GetDomainTrackerFields")]
        [HttpPost]
        public List<DomainTrackerFieldSource> DomainTrackerFields([FromBody]string TrackerKey)
        {
            VerifyAccountObject BaseChannel = verifyAccessAnon(TrackerKey);
            if (BaseChannel.BaseChannelID != -1)
            {
                List<DomainTrackerFieldSource> DomainTrackerFieldCollection = new List<DomainTrackerFieldSource>();
                ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = GetTracker(TrackerKey);
                KMPlatform.Entity.User superUser = getSuperUser(domainTracker.BaseChannelID);
                DataTable resultsDT = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(domainTracker.DomainTrackerID, superUser);
                if (resultsDT.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    DomainTrackerFieldSource obj;
                    for (int i = 0; i < resultsDT.Rows.Count; i++)
                    {
                        obj = new DomainTrackerFieldSource(resultsDT.Rows[i]["DomainTrackerFieldsID"].ToString(), resultsDT.Rows[i]["Source"].ToString(), resultsDT.Rows[i]["SourceID"].ToString());
                        DomainTrackerFieldCollection.Add(obj);
                    }
                    return DomainTrackerFieldCollection;
                }
            }
            else
                return null;
        }

        [Route("UpdateDomainTrackerActivity")]
        [HttpPost]
        public void UpdateDomainTrackerActivity([FromBody]UpdateDomainTrackerActivityObject UDTAO)
        {
            try
            {
                if (verifyAccess(UDTAO.TrackerKey) != -1)
                {
                    ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = GetTracker(UDTAO.TrackerKey);
                    KMPlatform.Entity.User superUser = getSuperUser(domainTracker.BaseChannelID);
                    ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile domainTrackerUserProfile = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.Get(UDTAO.EmailAddress, domainTracker.BaseChannelID, superUser);
                    if (domainTrackerUserProfile != null && domainTrackerUserProfile.ProfileID > 0)
                    {
                        ECN_Framework_Entities.DomainTracker.DomainTrackerActivity domainTrackerActivity = new ECN_Framework_Entities.DomainTracker.DomainTrackerActivity();
                        domainTrackerActivity.UserAgent = HttpContext.Current.Request.UserAgent;
                        domainTrackerActivity.TimeStamp = DateTime.Now;
                        domainTrackerActivity.DomainTrackerID = domainTracker.DomainTrackerID;
                        domainTrackerActivity.ProfileID = domainTrackerUserProfile.ProfileID;
                        domainTrackerActivity.PageURL = UDTAO.CurrentURL;

                        //domainTrackerActivity.PageURL = HttpContext.Current.Request.UrlReferrer.ToString();
                        domainTrackerActivity.IPAddress = HttpContext.Current.Request.UserHostAddress;
                        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("chrome"))
                            domainTrackerActivity.Browser = "Chrome";
                        else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("trident"))
                            domainTrackerActivity.Browser = "IE";
                        else
                            domainTrackerActivity.Browser = HttpContext.Current.Request.Browser.Browser;
                        domainTrackerActivity.OS = GetOperatingSystem(HttpContext.Current.Request.UserAgent);
                        domainTrackerActivity.SourceBlastID = Convert.ToInt32(UDTAO.SourceBlastID);
                        domainTrackerActivity.ReferralURL = UDTAO.ReferralURL;
                        ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.Save(domainTrackerActivity);
                        if (UDTAO.DomainTrackerFieldCollection != null && UDTAO.DomainTrackerFieldCollection.Count > 0)
                        {
                            foreach (DomainTrackerFieldSource domainTrackerFieldSource in UDTAO.DomainTrackerFieldCollection)
                            {
                                if (!domainTrackerFieldSource.FieldValue.Equals("null"))
                                {
                                    ECN_Framework_Entities.DomainTracker.DomainTrackerValue domainTrackerValue = new ECN_Framework_Entities.DomainTracker.DomainTrackerValue();
                                    domainTrackerValue.Value = domainTrackerFieldSource.FieldValue;
                                    domainTrackerValue.CreatedUserID = superUser.UserID;
                                    domainTrackerValue.DomainTrackerFieldsID = Convert.ToInt32(domainTrackerFieldSource.DomainTrackerFieldsID);
                                    domainTrackerValue.DomainTrackerActivityID = domainTrackerActivity.DomainTrackerActivityID;
                                    ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerValue.Save(domainTrackerValue, superUser);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("Email Address: " + UDTAO.EmailAddress + "<br/ >" + "TrackerKey: " + UDTAO.TrackerKey + "<br />" + ex.ToString(), "EmailMarketing.API.Controllers.Internal.DomainTrackerController.UpdateDomainTrackerActivity", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
                catch (Exception)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("Email Address: " + UDTAO.EmailAddress + "<br/ >" + "TrackerKey: " + UDTAO.TrackerKey + "<br />" + ex.Message.ToString(), "EmailMarketing.API.Controllers.Internal.DomainTrackerController.UpdateDomainTrackerActivity", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
            }
        }

        [Route("CreateCookieECN5")]
        [HttpPost]
        public HttpResponseMessage CreateCookieECN5([FromBody]Cookie cookieData)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Headers.Add("P3P", "CP=\"CAO IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            CookieHeaderValue cookie = new CookieHeaderValue(cookieData.Name, cookieData.Value);
            cookie.Expires = DateTime.Now.AddYears(1);
            cookie.Domain = ".ecn5.com";
            cookie.Path = "/";
            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return resp;
        }

        [Route("ReadCookieECN5")]
        [HttpPost]
        public string ReadCookieECN5([FromBody]string name)
        {
            //return HttpContext.Current.Request.Cookies[name] != null ?
            //    HttpContext.Current.Request.Cookies[name].Value.ToString() : null;

            string email = null;

            CookieHeaderValue cookie = Request.Headers.GetCookies(name).FirstOrDefault();
            if (cookie != null)
                email = cookie[name].Value;

            return email;
        }

        /// <summary>
        /// Gets users public IP
        /// </summary>
        /// <returns></returns>
        [Route("GetPublicIP")]
        [HttpPost]
        public string GetPublicIP()
        {

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var ctx = Request.Properties["MS_HttpContext"] as HttpContextBase;
            if (ctx != null)
            {
                return ctx.Request.UserHostAddress;
            }
            // Checks the HTTP_X_FORWARDED_FOR Header (which can be multiple IPs)
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //If that is not empty
            if (!string.IsNullOrEmpty(ipAddress))
            {
                // Grab the first address
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            // Otherwise use the REMOTE_ADDR Header
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// This will merge Anonymous activity and known activity
        /// </summary>
        /// <param name="merge"></param>
        [Route("MergeAnonActivity")]
        [HttpPost]
        public void MergeAnonActivity([FromBody]MergeAnonActivityObject merge)
        {
            if (merge.BaseChannelID > 0 && !string.IsNullOrWhiteSpace(merge.ActualEmail) && !string.IsNullOrWhiteSpace(merge.AnonEmail))
            {
                KMPlatform.Entity.User superUser = getSuperUser(merge.BaseChannelID);
                //merge anon activity to actual email activity
                ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile domainTrackerUserProfileAnon = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.Get(merge.AnonEmail, merge.BaseChannelID, superUser);
                ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile domainTrackerUserProfileActual = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.Get(merge.ActualEmail, merge.BaseChannelID, superUser);


                ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.MergeAnonActivity(merge.AnonEmail, merge.ActualEmail, merge.BaseChannelID);

            }
        }

        private VerifyAccountObject verifyAccessAnon(string TrackerKey)
        {
            ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = null;
            domainTracker = GetTracker(TrackerKey);

            string domain = string.Empty;
            try
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    domain = HttpContext.Current.Request.UrlReferrer.Host;
            }
            catch (Exception) { }
            try
            {


                List<string> domainsToLog = ConfigurationManager.AppSettings["DomainsToLog"].ToString().Split(',').ToList();
                if (domainsToLog.Contains(domain))
                {
                    string logInfo = domain + "<br />";
                    logInfo += "TrackerKey: " + TrackerKey + "<br />";
                    logInfo += "Found DomainTracker Object: " + (domainTracker != null ? "true<br />" : "false<br />").ToString();

                    if (domainTracker != null)
                    {
                        logInfo += "DomainTrackerID: " + domainTracker.DomainTrackerID.ToString() + "<br />";
                        logInfo += "DomainTracker.Domain: " + domainTracker.Domain + "<br />";
                    }

                    if (HttpContext.Current.Request.UrlReferrer != null)
                        logInfo += "Referring URL: " + HttpContext.Current.Request.UrlReferrer.ToString() + "<br />";
                    logInfo += "Referring URL Host: " + domain + "<br />";

                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(logInfo, "Domain Logging", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                }
            }
            catch { }
            if (domainTracker != null && domainTracker.Domain.Equals(domain))
            {
                ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(domainTracker.BaseChannelID);
                bool hasAnonTracking = KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(bc.PlatformClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.TrackAnonymous);
                VerifyAccountObject vao = new Internal.DomainTrackerController.VerifyAccountObject() { BaseChannelID = domainTracker.BaseChannelID, TrackAnon = hasAnonTracking };
                return vao;
            }
            else
            {
                return new VerifyAccountObject() { BaseChannelID = 0, TrackAnon = false };
            }
        }

        private int verifyAccess(string TrackerKey)
        {
            ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = null;
            domainTracker = GetTracker(TrackerKey);

            string domain = string.Empty;
            try
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    domain = HttpContext.Current.Request.UrlReferrer.Host;
            }
            catch (Exception) { }
            try
            {


                List<string> domainsToLog = ConfigurationManager.AppSettings["DomainsToLog"].ToString().Split(',').ToList();
                if (domainsToLog.Contains(domain))
                {
                    string logInfo = domain + "<br />";
                    logInfo += "TrackerKey: " + TrackerKey + "<br />";
                    logInfo += "Found DomainTracker Object: " + (domainTracker != null ? "true<br />" : "false<br />").ToString();

                    if (domainTracker != null)
                    {
                        logInfo += "DomainTrackerID: " + domainTracker.DomainTrackerID.ToString() + "<br />";
                        logInfo += "DomainTracker.Domain: " + domainTracker.Domain + "<br />";
                    }

                    if (HttpContext.Current.Request.UrlReferrer != null)
                        logInfo += "Referring URL: " + HttpContext.Current.Request.UrlReferrer.ToString() + "<br />";
                    logInfo += "Referring URL Host: " + domain + "<br />";

                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(logInfo, "Domain Logging", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                }
            }
            catch { }
            if (domainTracker != null && domainTracker.Domain.Equals(domain))
            {
                return domainTracker.BaseChannelID;
            }
            else
            {
                return -1;
            }
        }

        private ECN_Framework_Entities.DomainTracker.DomainTracker GetTracker(string TrackerKey)
        {
            string cacheKey = "DomainTracker-" + TrackerKey;
            ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = null;
            if (CacheHelper.IsCacheEnabled() && CacheHelper.GetCurrentCache(cacheKey) != null)
            {
                domainTracker = (ECN_Framework_Entities.DomainTracker.DomainTracker)CacheHelper.GetCurrentCache(cacheKey);
            }
            if (domainTracker == null)
            {
                domainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByTrackerKey(TrackerKey);
                if (CacheHelper.IsCacheEnabled() && domainTracker != null)
                    CacheHelper.AddToCache(cacheKey, domainTracker);
            }
            return domainTracker;
        }

        public class DomainTrackerFieldSource
        {
            public string DomainTrackerFieldsID;
            public string Source;
            public string SourceID;
            public string FieldValue;

            public DomainTrackerFieldSource(string _domainTrackerFieldsID, string _source, string _sourceid)
            {
                DomainTrackerFieldsID = _domainTrackerFieldsID;
                Source = _source;
                SourceID = _sourceid;
            }

            public DomainTrackerFieldSource()
            {
            }
        }

        public class UpdateDomainTrackerActivityObject
        {
            public List<DomainTrackerFieldSource> DomainTrackerFieldCollection;
            public string TrackerKey;
            public string EmailAddress;
            public string SourceBlastID;
            public string ReferralURL;
            public string CurrentURL;
        }

        public class MergeAnonActivityObject
        {
            public string AnonEmail;
            public string ActualEmail;
            public int BaseChannelID;
        }

        public class VerifyAccountObject
        {
            public int BaseChannelID;
            public bool TrackAnon;
        }

        public class Cookie
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private KMPlatform.Entity.User getSuperUser(int baseChannelID)
        {
            string cacheKey = "DomainTrackerSuperUser-" + baseChannelID.ToString();
            KMPlatform.Entity.User superUser = null;
            if (CacheHelper.IsCacheEnabled() && CacheHelper.GetCurrentCache(cacheKey) != null)
            {
                superUser = (KMPlatform.Entity.User)CacheHelper.GetCurrentCache(cacheKey);
            }
            if (superUser == null)
            {
                List<KMPlatform.Entity.User> userList = KMPlatform.BusinessLogic.User.GetUsersByChannelID(baseChannelID);
                foreach (KMPlatform.Entity.User user in userList)
                {
                    KMPlatform.Entity.User userToCheck = new KMPlatform.BusinessLogic.User().ECN_SetAuthorizedUserObjects(user, user.DefaultClientGroupID, user.DefaultClientID);
                    if (KM.Platform.User.IsChannelAdministrator(userToCheck))
                    {
                        superUser = user;
                        break;
                    }
                    if (KM.Platform.User.IsAdministrator(userToCheck))
                    {
                        superUser = user;
                        break;
                    }
                    //if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.HasPermission(KMPlatform.Enums.Access.Edit, user))
                    if (KM.Platform.User.HasAccess(userToCheck, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.View))
                    {
                        superUser = user;
                        break;
                    }
                }
                if (CacheHelper.IsCacheEnabled() && superUser != null)
                    CacheHelper.AddToCache(cacheKey, superUser);
            }
            return superUser;
        }

        private string GetOperatingSystem(string userAgent)
        {
            var clientOsName = string.Empty;
            if (userAgent.Contains("Windows 98"))
                clientOsName = "Windows 98";
            else if (userAgent.Contains("Windows NT 5.0"))
                clientOsName = "Windows 2000";
            else if (userAgent.Contains("Windows NT 5.1"))
                clientOsName = "Windows XP";
            else if (userAgent.Contains("Windows NT 6.0"))
                clientOsName = "Windows Vista";
            else if (userAgent.Contains("Windows NT 6.1"))
                clientOsName = "Windows 7";
            else if (userAgent.Contains("Windows NT 6.2"))
                clientOsName = "Windows 8";
            else if (userAgent.Contains("Windows NT 6.3"))
                clientOsName = "Windows 8.1";
            else if (userAgent.Contains("Windows"))
            {
                clientOsName = GetOsVersion(userAgent, "Windows");
            }
            else if (userAgent.Contains("Android"))
            {
                clientOsName = GetOsVersion(userAgent, "Android");
            }
            else if (userAgent.Contains("Linux"))
            {
                clientOsName = GetOsVersion(userAgent, "Linux");
            }
            else if (userAgent.Contains("iPhone"))
            {
                clientOsName = GetOsVersion(userAgent, "iPhone");
            }
            else if (userAgent.Contains("iPad"))
            {
                clientOsName = GetOsVersion(userAgent, "iPad");
            }
            else if (userAgent.Contains("Macintosh"))
            {
                clientOsName = GetOsVersion(userAgent, "Macintosh");
            }
            else
            {
                clientOsName = "Unknown OS";
            }

            return clientOsName;
        }

        private string GetOsVersion(string userAgent, string osName)
        {
            if (userAgent.Split(new[] { osName }, StringSplitOptions.None)[1].Split(new[] { ';', ')' }).Length != 0)
            {
                return string.Format("{0}{1}", osName, userAgent.Split(new[] { osName }, StringSplitOptions.None)[1].Split(new[] { ';', ')' })[0]);
            }

            return osName;
        }
    }
}
