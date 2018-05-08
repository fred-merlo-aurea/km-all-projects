using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using ecn.common.classes;
using ecn.communicator.classes;
using System.Text;
using ECN_Framework_Common.Functions;

namespace ecn.webservice
{
    [WebService(Namespace = "http://webservices.ecn5.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class DomainTracker : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public bool VerifyAccount(string TrackerKey)
        {
            if (verifyAccess(TrackerKey) != -1)
                return true;
            else
                return false;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public List<DomainTrackerFieldSource> GetDomainTrackerFields(string TrackerKey)
        {
            int DomainTrackerID = verifyAccess(TrackerKey);
            if (DomainTrackerID != -1)
            {
                List<DomainTrackerFieldSource> DomainTrackerFieldCollection = new List<DomainTrackerFieldSource>();
                ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = GetTracker(TrackerKey);
                KMPlatform.Entity.User superUser = getSuperUser(domainTracker.BaseChannelID);
                DataTable resultsDT = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(DomainTrackerID, superUser);
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

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateDomainTrackerActivity(List<DomainTrackerFieldSource> DomainTrackerFieldCollection, string TrackerKey, string EmailAddress, string SourceBlastID, string ReferralURL, string CurrentURL)
        {
            try
            {
                if (verifyAccess(TrackerKey) != -1)
                {
                    ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = GetTracker(TrackerKey);
                    KMPlatform.Entity.User superUser = getSuperUser(domainTracker.BaseChannelID);
                    ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile domainTrackerUserProfile = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.Get(EmailAddress, domainTracker.BaseChannelID, superUser);
                    if (domainTrackerUserProfile != null && domainTrackerUserProfile.ProfileID > 0)
                    {
                        ECN_Framework_Entities.DomainTracker.DomainTrackerActivity domainTrackerActivity = new ECN_Framework_Entities.DomainTracker.DomainTrackerActivity();
                        domainTrackerActivity.UserAgent = HttpContext.Current.Request.UserAgent;
                        domainTrackerActivity.TimeStamp = DateTime.Now;
                        domainTrackerActivity.DomainTrackerID = domainTracker.DomainTrackerID;
                        domainTrackerActivity.ProfileID = domainTrackerUserProfile.ProfileID;
                        domainTrackerActivity.PageURL = CurrentURL;
                        
                        //domainTrackerActivity.PageURL = HttpContext.Current.Request.UrlReferrer.ToString();
                        domainTrackerActivity.IPAddress = HttpContext.Current.Request.UserHostAddress;
                        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("chrome"))
                            domainTrackerActivity.Browser = "Chrome";
                        else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("trident"))
                            domainTrackerActivity.Browser = "IE";
                        else
                            domainTrackerActivity.Browser = HttpContext.Current.Request.Browser.Browser;
                        domainTrackerActivity.OS = GetOperatingSystem(HttpContext.Current.Request.UserAgent);
                        domainTrackerActivity.SourceBlastID = Convert.ToInt32(SourceBlastID);
                        domainTrackerActivity.ReferralURL = ReferralURL;
                        ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.Save(domainTrackerActivity);
                        if (DomainTrackerFieldCollection != null && DomainTrackerFieldCollection.Count > 0)
                        {
                            foreach (DomainTrackerFieldSource domainTrackerFieldSource in DomainTrackerFieldCollection)
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
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("Email Address: " + EmailAddress + "<br/ >" + "TrackerKey: " + TrackerKey + "<br />" + ex.ToString(), "ecn.webservice.DomainTracker.UpdateDomainTrackerActivity", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
                catch (Exception)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("Email Address: " + EmailAddress + "<br/ >" + "TrackerKey: " + TrackerKey + "<br />" + ex.Message.ToString(), "ecn.webservice.DomainTracker.UpdateDomainTrackerActivity", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
            }
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

            if (domainTracker != null && domainTracker.Domain.Equals(domain))
            {
                return domainTracker.DomainTrackerID;
            }
            else
            {
                return -1;
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string GetEmailAddress(string EmailID)
        {

            ECN_Framework_Entities.Communicator.Email e = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(Convert.ToInt32(EmailID));
            return e.EmailAddress;
        }

        #region WrapperClasses for Old Tracking Script
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public List<GroupUDF> GetUDFSources(string TrackerKey)
        {
            List<GroupUDF> GroupCollection = new List<GroupUDF>();
            ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByTrackerKey(TrackerKey);
            GroupUDF obj = new GroupUDF();
            obj.GroupID = domainTracker.DomainTrackerID.ToString();
            List<GroupDataFieldSource> sourceList = new List<GroupDataFieldSource>();
            GroupDataFieldSource gdfObj;

            List<DomainTrackerFieldSource> DomainTrackerFieldSourceList = GetDomainTrackerFields(TrackerKey);
            foreach (DomainTrackerFieldSource DomainTrackerFieldSource in DomainTrackerFieldSourceList)
            {
                gdfObj = new GroupDataFieldSource("Wrapper", DomainTrackerFieldSource.Source, DomainTrackerFieldSource.SourceID, DomainTrackerFieldSource.DomainTrackerFieldsID.ToString());
                sourceList.Add(gdfObj);
            }
            gdfObj = new GroupDataFieldSource("URL", string.Empty, string.Empty, string.Empty);
            sourceList.Add(gdfObj);
            gdfObj = new GroupDataFieldSource("TimeStamp", string.Empty, string.Empty, string.Empty); sourceList.Add(gdfObj);
            obj.GroupDataFields = sourceList;
            GroupCollection.Add(obj);
            return GroupCollection;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateDataJSON(List<GroupUDF> GroupCollection, string TrackerKey, string EmailAddress)
        {
            List<DomainTrackerFieldSource> DomainTrackerFieldCollection = new List<DomainTrackerFieldSource>();
            GroupUDF groupUDF = GroupCollection[0];
            foreach (GroupDataFieldSource gdfSource in groupUDF.GroupDataFields)
            {
                if (!gdfSource.GroupDataFieldName.Equals("URL") && !gdfSource.GroupDataFieldName.Equals("TimeStamp"))
                {
                    DomainTrackerFieldSource domainTrackerFieldSource = new DomainTrackerFieldSource();
                    domainTrackerFieldSource.DomainTrackerFieldsID = gdfSource.GroupDataFieldsID;
                    domainTrackerFieldSource.FieldValue = ECN_Framework_Common.Functions.StringFunctions.CleanXMLString(gdfSource.GroupDataFieldValue);
                    DomainTrackerFieldCollection.Add(domainTrackerFieldSource);
                }
            }
            UpdateDomainTrackerActivity(DomainTrackerFieldCollection, TrackerKey, EmailAddress, "0", string.Empty, string.Empty);
        }

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

    /// <summary>
    /// WrapperClasses for Old Tracking Script
    /// </summary>
    public class GroupUDF
    {
        public string GroupID;
        public List<GroupDataFieldSource> GroupDataFields;

    }

    /// <summary>
    /// WrapperClasses for Old Tracking Script
    /// </summary>
    public class GroupDataFieldSource
    {
        public string GroupDataFieldName;
        public string Source;
        public string SourceID;
        public string GroupDataFieldValue;
        public string GroupDataFieldsID;

        public GroupDataFieldSource(string _name, string _source, string _sourceid, string _groupDataFieldsID)
        {
            GroupDataFieldName = _name;
            Source = _source;
            SourceID = _sourceid;
            GroupDataFieldsID = _groupDataFieldsID;

        }

        public GroupDataFieldSource()
        {
        }
    }
        #endregion
}
