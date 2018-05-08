using System;
using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web.UI.HtmlControls;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using KM.Common.Extensions;
using Business = ECN_Framework_BusinessLayer.Communicator;
using DataFunctions = ecn.common.classes.DataFunctions;
using WebCaching = System.Web.Caching;
using Encryption = KM.Common.Entity.Encryption;
using CommonFunctions = ECN_Framework_Common.Functions;
using ChannelCheck = ECN_Framework.Common.ChannelCheck;
using ContentTypeCode = ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode;

namespace ecn.activityengines
{
    public partial class SPreview : PreviewBasePage
    {
        private const string CacheUserByAccessKeyTemplate = "cache_user_by_AccessKey_{0}";
        private const string AppSettingEcnEngineAccessKey = "ECNEngineAccessKey";
        private const string QueryParamFbSource = "&fb_source=message";
        private const string LiteralPlus = "+";
        private const string Literal2B = "%2b";
        private const int CacheDurationMins = 15;
        private const string TestBlastN = "n";
        private const string CacheCampaignItemByBlastIdTemplate = "cache_CampaignItem_by_BlastID_{0}";
        private const string CacheMetaTagsByCampaignItemId = "cache_MetaTags_by_CampaignItemID_{0}";
        private const string CacheCampaignItemTestBlastByBlastIdTemplate = "cache_CampaignItemTestBlast_by_BlastID_{0}";
        private const string AppSettingkKmCommonApplication = "KMCommon_Application";
        private const string AppSettingActivityDomainPath = "Activity_DomainPath";
        private const string AppSettingSocialPreview = "SocialPreview";
        private const string BlastLayoutUrlTemplate = "blastID={0}&layoutID={1}&m=1&g={2}";
        private const string AttributeProperty = "property";
        private const string AppSettingFbAppId = "FBAPPID";
        private const string OpenGraphUrlTag = "og:url";
        private const string OpenGraphTitleTag = "og:title";
        private const string OpenGraphDescriptionTag = "og:description";
        private const string DelimPercent = "%%";
        private const string TitleRedacted = "(redacted)";
        private const string HtmlTagFontFragment = "<FONT color=#666666><em>(redacted)</em></FONT>";
        private const string AppSettingTrueValue = "true";
        private const string AppSettingValidateB4Tracking = "ValidateB4Tracking";
        private const string ErrorParseTemplate = "Couldn't parse {0} from '{1}'";
        int BlastID = 0;
        int EmailID = 0;
        int GroupID = 0;
        int SocialMediaID = 0;
        string Decrypted = string.Empty;
        string Link = string.Empty;
        //ApplicationId AppID = null;
        public int RefBlastID = 0;
        public KMPlatform.Entity.User User = null;

        public static bool isMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            HttpContext context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                if (CheckMobileType(context))
                {
                    return true;
                }
            }

            return false;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetUserOnPageLoad();

            var querystring = string.Empty;
            //remove extra param added by FB
            if (Request.Url.Query.Length > 0)
            {
                querystring = Request.Url.Query.Substring(1, Request.Url.Query.Length - 1)
                    .Replace(QueryParamFbSource, string.Empty);
            }

            //trying to resolve issue of FB replacing %2b with +
            querystring = querystring.Replace(LiteralPlus, Literal2B);

            Decrypted = Helper.DeCrypt_DeCode_EncryptedQueryString(querystring);
            if (!Decrypted.IsNullOrWhiteSpace())
            {
                GetValuesFromQuerystring(Decrypted);
            }

            if (BlastID > 0)
            {
                getRefBlast();
            }

            //must have EmailID and BlastID
            if (BlastID == 0 || GroupID == 0 || SocialMediaID == 0)
            {
                LabelPreview.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
            }
            else
            {
                TrackDataOnEmailExists();
                var isMobile = GetIsMobile();
                CreateControlsOnPageLoad(isMobile);
            }
        }

        private void SetUserOnPageLoad()
        {
            var cacheKey = string.Format(
                CacheUserByAccessKeyTemplate,
                ConfigurationManager.AppSettings[AppSettingEcnEngineAccessKey]);

            if (Cache[cacheKey] == null)
            {
                User = KMPlatform.BusinessLogic.User.GetByAccessKey(
                    ConfigurationManager.AppSettings[AppSettingEcnEngineAccessKey]);
                Cache.Add(
                    cacheKey, 
                    User, 
                    null,
                    WebCaching.Cache.NoAbsoluteExpiration, 
                    TimeSpan.FromMinutes(CacheDurationMins),
                    WebCaching.CacheItemPriority.Normal, null);
            }
            else
            {
                User = (KMPlatform.Entity.User) Cache[cacheKey];
            }
        }

        private void CreateControlsOnPageLoad(bool isMobile)
        {
            try
            {
                BlastAbstract blast;
                var html = PreProcessHtml(isMobile, out blast);

                var listMeta = FillCampaignItemMetaTags(blast);

                var head = Page.Header;
                var applicationId = ToInt32(ConfigurationManager.AppSettings[AppSettingkKmCommonApplication]);
                //trying this to get around subscriber share pre cache issues

                Encryption.GetCurrentByApplicationID(applicationId);
                var activityDomainPath = ConfigurationManager.AppSettings[AppSettingActivityDomainPath];
                var socialPreview = ConfigurationManager.AppSettings[AppSettingSocialPreview];
                var previewLink = string.Concat(activityDomainPath, socialPreview);
                var queryString = string.Format(BlastLayoutUrlTemplate, blast.BlastID, blast.LayoutID, blast.GroupID);
                var encryptedString = Helper.Encrypt_UrlEncode_QueryString(queryString);
                previewLink += encryptedString.Replace(Literal2B, LiteralPlus);

                var urlMeta = new HtmlMeta();
                urlMeta.Attributes.Add(AttributeProperty, OpenGraphUrlTag);
                if (SocialMediaID == 1)
                {
                    urlMeta.Content = previewLink;
                }
                else
                {
                    urlMeta.Content = Request.Url.OriginalString;
                }

                head.Controls.Add(urlMeta);

                if (SocialMediaID == 1)
                {
                    var appidMeta = new HtmlMeta();
                    appidMeta.Attributes.Add(AttributeProperty, OpenGraphUrlTag);
                    appidMeta.Content = ConfigurationManager.AppSettings[AppSettingFbAppId];
                    head.Controls.Add(appidMeta);
                }

                ProcessItemMetaTags(listMeta, head);

                LabelPreview.Text = RemoveSocial(html);
            }
            catch
            {
                Page.Title = string.Empty;
                LabelPreview.Text = string.Empty;
            }
        }

        private void ProcessItemMetaTags(IEnumerable<CampaignItemMetaTag> listMeta, HtmlHead head)
        {
            Guard.NotNull(listMeta, nameof(listMeta));

            foreach (var itemMetaTag in listMeta)
            {
                if (itemMetaTag.SocialMediaID == SocialMediaID && !string.IsNullOrWhiteSpace(itemMetaTag.Content))
                {
                    var meta = new HtmlMeta();

                    meta.Name = itemMetaTag.Property;
                    meta.Attributes.Add(AttributeProperty, itemMetaTag.Property);
                    if (itemMetaTag.Property.EqualsAnyIgnoreCase(OpenGraphTitleTag, OpenGraphDescriptionTag))
                    {
                        meta.Content = CommonFunctions.EmojiFunctions.GetSubjectUTF(itemMetaTag.Content);
                    }
                    else
                    {
                        meta.Content = itemMetaTag.Content;
                    }

                    head.Controls.Add(meta);
                }
            }
        }

        private IEnumerable<CampaignItemMetaTag> FillCampaignItemMetaTags(BlastAbstract blast)
        {
            Guard.NotNull(blast, nameof(blast));
            var listMeta = new List<CampaignItemMetaTag>();
            if (TestBlastN.EqualsIgnoreCase(blast.TestBlast))
            {
                CampaignItem campaignItem;
                var cacheKeyCampaignItem = string.Format(CacheCampaignItemByBlastIdTemplate, BlastID);
                if (Cache[cacheKeyCampaignItem] == null)
                {
                    campaignItem = Business.CampaignItem.GetByBlastID_NoAccessCheck(BlastID, false);
                    Business.CampaignItemMetaTag.GetByCampaignItemID(campaignItem.CampaignItemID);
                    Cache.Add(
                        cacheKeyCampaignItem,
                        campaignItem,
                        null,
                        WebCaching.Cache.NoAbsoluteExpiration,
                        TimeSpan.FromMinutes(CacheDurationMins),
                        WebCaching.CacheItemPriority.Normal,
                        null);
                }
                else
                {
                    campaignItem = (CampaignItem)Cache[cacheKeyCampaignItem];
                }

                var cacheKeyMetaTags = string.Format(CacheMetaTagsByCampaignItemId, campaignItem.CampaignItemID);
                if (Cache[cacheKeyMetaTags] == null)
                {
                    listMeta = Business.CampaignItemMetaTag.GetByCampaignItemID(campaignItem.CampaignItemID);
                    Cache.Add(
                        cacheKeyMetaTags,
                        listMeta,
                        null,
                        WebCaching.Cache.NoAbsoluteExpiration,
                        TimeSpan.FromMinutes(CacheDurationMins),
                        WebCaching.CacheItemPriority.Normal, null);
                }
                else
                {
                    listMeta = (List<CampaignItemMetaTag>)Cache[cacheKeyMetaTags];
                }
            }
            else
            {
                listMeta = CampaignItemMetaTagsOnBlast();
            }

            return listMeta;
        }

        private List<CampaignItemMetaTag> CampaignItemMetaTagsOnBlast()
        {
            List<CampaignItemMetaTag> listMeta;
            var cacheKeyCampaignItemTestBlastByBlastId =
                string.Format(CacheCampaignItemTestBlastByBlastIdTemplate, BlastID);
            CampaignItemTestBlast testBlast = null;
            if (Cache[cacheKeyCampaignItemTestBlastByBlastId] == null)
            {
                testBlast = Business.CampaignItemTestBlast.GetByBlastID_NoAccessCheck(BlastID, false);
                Cache.Add(
                    cacheKeyCampaignItemTestBlastByBlastId,
                    testBlast,
                    null,
                    WebCaching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(CacheDurationMins),
                    WebCaching.CacheItemPriority.Normal,
                    null);
            }
            else
            {
                testBlast = (CampaignItemTestBlast) Cache[cacheKeyCampaignItemTestBlastByBlastId];
            }

            var cacheKeyMetaTags = string.Format(CacheMetaTagsByCampaignItemId, testBlast?.CampaignItemID.Value);
            if (Cache[cacheKeyMetaTags] == null)
            {
                listMeta = Business.CampaignItemMetaTag.GetByCampaignItemID(testBlast.CampaignItemID.Value);
                Cache.Add(cacheKeyMetaTags,
                    listMeta,
                    null,
                    WebCaching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(CacheDurationMins),
                    WebCaching.CacheItemPriority.Normal,
                    null);
            }
            else
            {
                listMeta = (List<CampaignItemMetaTag>) Cache[cacheKeyMetaTags];
            }

            return listMeta;
        }

        private string PreProcessHtml(bool isMobile, out BlastAbstract blast)
        {
            var title = string.Empty;
            var html = string.Empty;
            blast = Business.Blast.GetByBlastID_NoAccessCheck(BlastID, false);

            //get subject
            var regex = new Regex(DelimPercent); // Split on percents.
            var breakupEmailSubject = regex.Split(blast.EmailSubject);
            for (var i = 0; i < breakupEmailSubject.Length; i++)
            {
                var lineData = breakupEmailSubject.GetValue(i).ToString();
                if (i % 2 == 0)
                {
                    title += lineData;
                }
                else
                {
                    title += TitleRedacted;
                }
            }

            Page.Title = CommonFunctions.EmojiFunctions.GetSubjectUTF(title);

            var channelCheck = new ChannelCheck(); // Not certain if this is singleton. Needs to be one.			
            channelCheck.CustomerID(blast.CustomerID.Value);

            html = Business.Layout.GetPreviewNoAccessCheck(
                blast.LayoutID.Value,
                ContentTypeCode.HTML, 
                isMobile, 
                blast.CustomerID.Value);

            regex = new Regex(DelimPercent); // Split on percents.
            // The array is now split into [0] = text  and [1] = token to insert
            var breakupHtmlMail = regex.Split(html);
            var builder = new StringBuilder();
            for (var i = 0; i < breakupHtmlMail.Length; i++)
            {
                var lineData = breakupHtmlMail.GetValue(i).ToString();
                builder.Append(i % 2 == 0 ? lineData : HtmlTagFontFragment);
            }
            html = builder.ToString();
            html = RemoveTags(html);
            html = DoRSSFeed(html);
            html = DecodeLinks(html);
            html = RemoveTransnippets(html);
            return html;
        }

        private static bool GetIsMobile()
        {
            return isMobileBrowser();
        }

        private void TrackDataOnEmailExists()
        {
            if (EmailID > 0)
            {
                if (AppSettingTrueValue.EqualsIgnoreCase(ConfigurationManager.AppSettings[AppSettingValidateB4Tracking]))
                {
                    if (Business.EmailGroup.ValidForTracking(RefBlastID, EmailID))
                    {
                        TrackData();
                    }
                }
                else
                {
                    TrackData();
                }
            }
        }

        private string RemoveTags(string html)
        {
            System.Collections.Generic.List<string> toParse = new System.Collections.Generic.List<string>();
            toParse.Add(html.ToString());
            System.Collections.Generic.List<string> tags = ECN_Framework_BusinessLayer.Communicator.Content.GetTags(toParse, true);
            if (tags.Count > 0)
            {
                foreach (string tag in tags)
                {
                    html = html.Replace(tag, "");
                }
            }
            return html;
        }

        private string DecodeLinks(string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["href"];
                    if (attHREF != null && attHREF.Value.Contains("%23"))
                    {
                        html = html.Replace(attHREF.Value, attHREF.Value.Replace("%23", "#"));
                    }

                }
            }
            return html;
        }

        private string DoRSSFeed(string html)
        {
            if (html.ToString().IndexOf("ECN.RSSFEED") > 0)
            {
                string sqlBlastQuery = " SELECT * " +
                        " FROM Blast " +
                        " WHERE BlastID=" + BlastID + " ";
                DataTable dt = DataFunctions.GetDataTable(sqlBlastQuery);
                DataRow blast_info = dt.Rows[0];


                Regex r = new Regex("ECN.RSSFEED"); // Split on RSS Tag.
                Array rssName = r.Split(html.ToString());

                ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(BlastID, false);
                int userID = blast.CreatedUserID.Value;
                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByUserID(userID, false);
                ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck(blast.CustomerID.Value);

                string currentRSSTag = "";
                for (int i = 0; i < rssName.Length; i++)
                {
                    try
                    {
                        StringBuilder sbRSSFeed = new StringBuilder();
                        StringBuilder sbRSSFeedText = new StringBuilder();
                        string line_data = rssName.GetValue(i).ToString();
                        if (i % 2 == 0)
                        {
                            //sbRSSFeed.Append(line_data);
                        }
                        else
                        {
                            currentRSSTag = line_data;
                            ECN_Framework_Entities.Communicator.RSSFeed rss = ECN_Framework_BusinessLayer.Communicator.RSSFeed.GetByFeedName(line_data.Remove(line_data.Length - 1, 1).Remove(0, 1), blast.CustomerID.Value);
                            if (rss != null)
                            {
                                XmlReader reader = XmlReader.Create(rss.URL);
                                System.ServiceModel.Syndication.SyndicationFeed sf = System.ServiceModel.Syndication.SyndicationFeed.Load(reader);
                                if (rss.StoriesToShow.HasValue)
                                {
                                    int index = 0;
                                    foreach (System.ServiceModel.Syndication.SyndicationItem si in sf.Items)
                                    {
                                        if (index < rss.StoriesToShow.Value)
                                        {
                                            sbRSSFeed.Append("<a href='" + si.Id + "'>" + si.Title.Text + "</a><br />");
                                            sbRSSFeed.Append("<span>" + si.Summary.Text + "</span><br /><br />");

                                            sbRSSFeedText.Append("<" + si.Id + "\n");
                                            sbRSSFeedText.Append(si.Summary.Text + "\n\n");
                                            index++;
                                        }
                                        else
                                            break;
                                    }

                                }
                                else
                                {
                                    int index = 0;
                                    foreach (System.ServiceModel.Syndication.SyndicationItem si in sf.Items)
                                    {
                                        if (index < 10)
                                        {
                                            sbRSSFeed.Append("<a href=\"" + si.Id + "\">" + si.Title.Text + "</a><br />");
                                            sbRSSFeed.Append("<span>" + si.Summary.Text + "</span><br /><br />");
                                            sbRSSFeedText.Append("<" + si.Id + "\n");
                                            sbRSSFeedText.Append(si.Summary.Text + "\n\n");
                                            index++;
                                        }
                                        else
                                            break;
                                    }
                                }
                                html = html.Replace("ECN.RSSFEED" + currentRSSTag + "ECN.RSSFEED", TemplateFunctions.LinkReWriter(sbRSSFeed.ToString(), blast, blast.CustomerID.Value.ToString(), ConfigurationManager.AppSettings["Communicator_VirtualPath"].ToString(), cc.getHostName()));

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }

            }
            return html;
        }

        private string RemoveTransnippets(string html)
        {
            bool containsTrans = true;
            if (html.Contains("##TRANSNIPPET"))
            {
                while (containsTrans)
                {
                    html = html.Remove(html.IndexOf("##TRANSNIPPET"), html.IndexOf("##", html.IndexOf("##TRANSNIPPET") + 13) - html.IndexOf("##TRANSNIPPET") + 2);
                    if (html.Contains("##TRANSNIPPET"))
                        containsTrans = true;
                    else
                        containsTrans = false;
                }
            }
            return html;
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
            if (Cache[string.Format("cache_RefBlast_by_BlastID_{0}", RefBlastID.ToString())] == null)
            {
                ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(BlastID, false);
                try
                {
                    if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                    {
                        RefBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, EmailID, blast.CustomerID.Value, blast.BlastType);
                        blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(RefBlastID, false);
                        GroupID = blast.GroupID.Value;
                    }
                    Cache.Add(string.Format("cache_RefBlast_by_BlastID_{0}", RefBlastID), blast, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                catch (Exception) { }

            }
            else
            {
                ECN_Framework_Entities.Communicator.BlastAbstract blast = (ECN_Framework_Entities.Communicator.BlastAbstract)Cache[string.Format("cache_RefBlast_by_BlastID_{0}", RefBlastID.ToString())];

                GroupID = blast.GroupID.Value;
            }

        }

        private string RemoveSocial(string html)
        {
            List<ECN_Framework_Entities.Communicator.SocialMedia> mediaList = null;
            if (Cache["cache_SocialMediaCanShare"] == null)
            {
                mediaList = ECN_Framework_BusinessLayer.Communicator.SocialMedia.GetSocialMediaCanShare();
                Cache.Add("cache_SocialMediaCanShare", mediaList, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                mediaList = (List<ECN_Framework_Entities.Communicator.SocialMedia>)Cache["cache_SocialMediaCanShare"];
            }
            if (mediaList != null && mediaList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.SocialMedia media in mediaList)
                {
                    int startPos = 0;
                    int endPos = 0;
                    try
                    {
                        startPos = html.IndexOf("<a href=\"" + media.MatchString + "\">");
                        endPos = html.IndexOf("</a>", startPos);
                        if (startPos >= 0 && endPos > startPos)
                        {
                            html = html.Substring(0, startPos) + html.Substring(endPos + 4, html.Length - (endPos + 4));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return html;
        }

        private int TrackData()
        {
            ECN_Framework_Entities.Activity.BlastActivitySocial social = new ECN_Framework_Entities.Activity.BlastActivitySocial();
            social.BlastID = BlastID;
            social.RefEmailID = EmailID;
            social.SocialActivityCodeID = 2;
            social.URL = Request.Url.ToString();
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

        private static int ToInt32(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        private static int ToInt32WithThrow(string str)
        {
            int result;
            if (!int.TryParse(str, out result))
            {
                var exceptionMessage = string.Format(ErrorParseTemplate, typeof(int).Name, str);
                throw new InvalidOperationException(exceptionMessage);
            }
            return result;
        }
    }
}