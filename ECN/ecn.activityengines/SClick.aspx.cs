using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Caching;
using KM.Common.Entity;
using BusinessActivity = ECN_Framework_BusinessLayer.Activity;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using CommonFunction = ECN_Framework_Common.Functions;
using EntityCommunicator = ECN_Framework.Communicator.Entity;
using KMBusinessLogic = KMPlatform.BusinessLogic;
using User = KMPlatform.Entity.User;

namespace ecn.activityengines
{
    public partial class SClick : System.Web.UI.Page
    {
        private const string KmCommonApplicationKey = "KMCommon_Application";
        private const string EcnEngineAccessKey = "ECNEngineAccessKey";
        private const string SocialDomainPathKey = "Social_DomainPath";
        private const string SocialPreviewKey = "SocialPreview";
        private const string UserCacheKeyPrefix = "cache_user_by_AccessKey_";
        private const string RedactedTitle = "(redacted)";
        private const string BlastIdKey = "blastID";
        private const string LayoutIdKey = "layoutID";
        private const string FacebookGraphLink = "https://graph.facebook.com";
        private const string FacebookPreCacheMethodName = "FacebookPreCache";
        private const string BlackBerryUserAgent = "blackberry";
        private const string JavascriptStartTag = "<script language='javascript'>";
        private const string JavascriptEndTag = "</script>";
        private const string WindowLocationHref = "window.location.href";
        private const int SocialMediaId1 = 1;
        private const int SocialMediaId2 = 2;
        private const string HashtagsProperty = "hashtags";
        private const string UrlProperty = "url";
        private const string LinkTitleProperty = "LinkTitle";

        int BlastID = 0;
        int EmailID = 0;
        int GroupID = 0;
        int SocialMediaID = 0;
        string Decrypted = string.Empty;
        string Link = string.Empty;
        //ApplicationId AppID = null;
        public int RefBlastID = 0;
        public KMPlatform.Entity.User User = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SetUserFromCache();

            if (Request.Url.Query.Length > 0)
            {
                Decrypted = Helper.DeCrypt_DeCode_EncryptedQueryString(
                    Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            }

            if (Decrypted != string.Empty)
            {
                GetValuesFromQuerystring(Decrypted);
            }

            if (BlastID > 0)
            {
                getRefBlast();
            }

            // must have EmailID and BlastID
            if (BlastID == 0 || EmailID == 0 || GroupID == 0 || SocialMediaID == 0)
            {
                errorMsgPanel.Visible = true;
            }
            else
            {
                errorMsgPanel.Visible = false;

                var blast = new EntityCommunicator.Blast();
                SetPageTitle(ref blast);

                var socialDomainPath = ConfigurationManager.AppSettings[SocialDomainPathKey];
                var socialPreview = ConfigurationManager.AppSettings[SocialPreviewKey];
                Link = $"{socialDomainPath}{socialPreview}";

                var queryString = $"b={BlastID}&g={GroupID}&e={EmailID}&m={SocialMediaID}";
                Link += Helper.Encrypt_UrlEncode_QueryString(queryString);

                // Work around for facebook pre cache issue. Doing this as a work around to FB's pre cache issue
                FacebookPreCache(socialDomainPath, socialPreview, blast);

                var redirect = GetRedirectUrl(blast);

                // The following is a patch for BlackBerry users. It didn't support the java script 
                // method of redirecting the URL's. The following will do a straight redirect to the link.
                var userAgent = Request.UserAgent;
                if (userAgent != null && userAgent.IndexOf(BlackBerryUserAgent, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Response.Redirect(redirect, false);
                }
                else
                {
                    Response.Write($"{JavascriptStartTag}{WindowLocationHref}='{redirect.Replace("'", "\\'")}'" +
                                   $"{JavascriptEndTag}");
                }
            }
        }

        private void SetUserFromCache()
        {
            var ecnEngineAccessKey = ConfigurationManager.AppSettings[EcnEngineAccessKey];
            var cacheKeyUser = $"{UserCacheKeyPrefix}{ecnEngineAccessKey}";
            if (Cache[cacheKeyUser] == null)
            {
                User = KMBusinessLogic.User.GetByAccessKey(ecnEngineAccessKey, false);
                Cache.Add(
                    cacheKeyUser,
                    User,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(15),
                    CacheItemPriority.Normal,
                    null);
            }
            else
            {
                User = Cache[cacheKeyUser] as User;
            }
        }

        private void SetPageTitle(ref EntityCommunicator.Blast blast)
        {
            var title = new StringBuilder();
            try
            {
                blast = EntityCommunicator.Blast.GetByBlastID(BlastID);

                var regex = new Regex("%%"); // Split on percents.
                var breakupEmailSubject = regex.Split(blast.EmailSubject);

                for (var index = 0; index < breakupEmailSubject.Length; index++)
                {
                    var lineData = breakupEmailSubject.GetValue(index).ToString();
                    title.Append(index % 2 == 0 
                        ? lineData : 
                        RedactedTitle);
                }
            }
            catch
            {
                title.Clear();
            }

            Page.Title = CommonFunction.EmojiFunctions.GetSubjectUTF(title.ToString());
        }

        private void FacebookPreCache(string socialDomainPath, string socialPreview, EntityCommunicator.Blast blast)
        {
            try
            {
                if (!BusinessActivity.BlastActivitySocial.FBHasBeenSharedAlready(BlastID) && 
                    SocialMediaID == SocialMediaId1)
                {
                    var preCacheLink = $"{socialDomainPath}{socialPreview}";
                    var preCacheString = $"{BlastIdKey}={BlastID}&{LayoutIdKey}=" +
                                         $"{blast.LayoutID}&m=1&g={blast.GroupID}";
                    preCacheLink += Helper.Encrypt_UrlEncode_QueryString(preCacheString);

                    var requestFb = WebRequest.Create(
                        $"{FacebookGraphLink}?{preCacheLink}&scrape=true") as HttpWebRequest;

                    if (requestFb != null)
                    {
                        requestFb.Method = "POST";
                        using (var response = requestFb.GetResponse() as HttpWebResponse)
                        {
                            if (response != null)
                            {
                                var stream = new StreamReader(response.GetResponseStream());
                                stream.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[KmCommonApplicationKey]);
                ApplicationLog.LogNonCriticalError(exception, FacebookPreCacheMethodName, applicationId);
            }
        }

        private string GetRedirectUrl(EntityCommunicator.Blast blast)
        {
            var socialMedia = BusinessCommunicator.SocialMedia.GetSocialMediaByID(SocialMediaID);
            var redirect = string.Empty;
            if (socialMedia != null)
            {
                if (socialMedia.SocialMediaID == SocialMediaId2)
                {
                    var hashTags = string.Empty;
                    int? campaignItemId = null;
                    var campaignItem = BusinessCommunicator.CampaignItem.GetByBlastID_NoAccessCheck(BlastID, false);

                    if (campaignItem != null)
                    {
                        campaignItemId = campaignItem.CampaignItemID;
                    }
                    else
                    {
                        var itemTestBlast = BusinessCommunicator.CampaignItemTestBlast.GetByBlastID_NoAccessCheck(
                            BlastID, 
                            false);
                        if (itemTestBlast != null)
                        {
                            campaignItemId = itemTestBlast.CampaignItemID;
                        }
                    }

                    if (campaignItemId != null)
                    {
                        var itemMetaTags = BusinessCommunicator.CampaignItemMetaTag.GetByCampaignItemID(
                            campaignItemId.Value);
                        if (itemMetaTags.Count(tag => 
                                tag.SocialMediaID == SocialMediaId2 && 
                                tag.Property.Equals(HashtagsProperty, StringComparison.OrdinalIgnoreCase)) > 0)
                        {
                            hashTags = itemMetaTags.Find(tag => 
                                tag.SocialMediaID == SocialMediaId2 && 
                                tag.Property.Equals(HashtagsProperty, StringComparison.OrdinalIgnoreCase)).Content;
                        }
                    }
                    redirect = socialMedia.ShareLink.Replace($"|{HashtagsProperty}|", hashTags.Replace("#", "")).
                        Replace($"|{UrlProperty}|", Link);
                }
                else
                {
                    // |LinkTitle| getting a FaceBook title
                    var facebookTitle = socialMedia.ShareLink.Replace(
                        $"|{LinkTitleProperty}|",
                        CommonFunction.EmojiFunctions.GetSubjectUTF(blast.EmailSubject));
                    redirect = $"{facebookTitle}{Link}";
                }
            }

            return redirect;
        }

        #region Get Request Variables
        private void GetValuesFromQuerystring(string queryString)
        {
            KM.Common.QueryString qs = KM.Common.QueryString.GetECNParameters(queryString);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.BlastID).ParameterValue, out BlastID);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.EmailID).ParameterValue, out EmailID);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.GroupID).ParameterValue, out GroupID);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.SocialMediaID).ParameterValue, out SocialMediaID);
        }
        #endregion

        private void getRefBlast()
        {
            RefBlastID = BlastID;
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(BlastID,false);
            try
            {
                if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                {
                    RefBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, EmailID, blast.CustomerID.Value, blast.BlastType);
                    blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(RefBlastID, false);
                    GroupID = blast.GroupID.Value;
                }
            }
            catch (Exception) { }
        }

        private int TrackData()
        {
            ECN_Framework_Entities.Activity.BlastActivitySocial social = new ECN_Framework_Entities.Activity.BlastActivitySocial();
            social.BlastID = BlastID;
            social.EmailID = EmailID;
            social.SocialActivityCodeID = 1;
            social.URL = Link;
            social.SocialMediaID = SocialMediaID;
            return ECN_Framework_BusinessLayer.Activity.BlastActivitySocial.Insert(social);
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