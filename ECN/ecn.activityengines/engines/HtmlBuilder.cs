using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using ecn.common.classes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity;
using BaseChannel = ECN_Framework_Entities.Accounts.BaseChannel;
using CampaignItemSocialMedia = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia;
using CampaignItemTestBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast;
using Content = ECN_Framework_BusinessLayer.Communicator.Content;
using ContentFilter = ECN_Framework_BusinessLayer.Communicator.ContentFilter;
using DynamicTag = ECN_Framework_BusinessLayer.Communicator.DynamicTag;
using Layout = ECN_Framework_BusinessLayer.Communicator.Layout;
using RSSFeed = ECN_Framework_BusinessLayer.Communicator.ContentReplacement.RSSFeed;
using SocialMedia = ECN_Framework_BusinessLayer.Communicator.SocialMedia;
using SocialMediaAuth = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth;
using BusinessBlastLink = ECN_Framework_BusinessLayer.Communicator.BlastLink;
using EntitiesCampaignItemSocialMedia = ECN_Framework_Entities.Communicator.CampaignItemSocialMedia;
using EntitiesSocialMedia = ECN_Framework_Entities.Communicator.SocialMedia;
using BusinessCampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator.Enums;

namespace ecn.activityengines
{
    public class HtmlBuilder
    {
        private const string ContentSourceColumn = "ContentSource";
        private const string TestBlastScript = @"
                        <script type=""text/javascript"" src=""http://ecn5.com/ecn.accounts/scripts/js/jquery-1.7.2.min.js""></script>
                        <script src=""http://ecn5.com/ecn.accounts/scripts/js/jquery-ui-1.8.22.custom.min.js"" type=""text/javascript""></script>
                        <script type=""text/javascript"" src=""http://cdn.jsdelivr.net/qtip2/2.2.1/jquery.qtip.js""></script>
                        <script src=""http://cdn.jsdelivr.net/jquery.cookie/1.4.0/jquery.cookie.min.js""></script>
                        <script type""text/javascript"">                          
                     
                     $(function () {
		                    $.cookie.json = true;
                            var actualValue = $('#testblastlistpreview').prop('checked');
                            var cookieValue = myReadCookie();
                     
                            if(cookieValue === 'true' && actualValue === false)
                                { ShowHideTips(myReadCookie());
                                }
                           else
                                { ShowHideTips($('#testblastlistpreview'));
		                        }
 	                    });

                        function myCreateCookie(value) {  $.cookie('checkboxValues', value, { path: '/' }); }
                        
                        function myReadCookie() {  return  $.cookie('checkboxValues'); } 

                        function HideTips() {
                          
                           $('[orighref!=""""]').qtip('destroy'); 

                            }

                        function ShowTips() {
                            $('[orighref!=""""]').each(function (i,o) {

                                var $o = $( o );
	                            if($o != null) {
                                    var href = $o.attr(""orighref"");
                                    if(href != null && href.length > 0)
                                        {
                                           
                                            $(o).qtip({
			                                    show: { solo: true },
			                                    hide: { when: 'inactive', delay: 4000 }, //{ distance: 30 },
			                                    position: { target: 'mouse',  adjust: { x: 5, y: 5 } },
			                                    content: href, //$(this).attr(""href""),
			                                    style: { name: 'green', tip: 'topLeft' }
		                                    });
	                                    }
                                    }
                                 });    
                            }

                        function ShowHideTips(o) {
                        
                            var actualValue = o.prop('checked');

                            myCreateCookie( actualValue);
                          
                            if(actualValue) ShowTips();
                            else HideTips();
                        }               
                                             
                     </script>
                        <span id=""TestBlastListPreviewSpan"" style=""display: none; font-family: Arial; background-color: rgba(255, 0, 0, 0.3); display: block; z-index: 99999; position: absolute; top: 30px; right: 30px; margin: 50px 50px 50px 50px; padding: 15px 15px 15px 15px; border-radius: 25px;"" >
	                        <span>Show Decoded Link Targets</span>
	                        <input type=""checkbox"" id=""testblastlistpreview"" value=""1"" onchange = ""ShowHideTips($('#testblastlistpreview'));"" />
                        </span>";
        private const int FacebookSocialId = 1;
        private const int TwitterSocialId = 2;
        private const int LinkedInSocialId = 3;
        private const int FacebookLikeSocialId = 4;
        private const int F2FSocialMediaId = 5;
        private readonly Action<string> _writeToLog;
        private static readonly Regex FindLid = new Regex(@"lid=(\d+)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly int _blastId;
        private ArrayList _transnippet;
        private readonly Literal _literalPreview;
        private readonly int _emailId;
        private readonly TraceContext _trace;

        private StringBuilder _html = new StringBuilder();
        private static readonly int ApplicationId = Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]);

        public HtmlBuilder(
            Action<string> writeToLogFunc,
            int blastId,
            int emailId,
            ArrayList transnippet,
            Literal literalPreview,
            TraceContext trace)
        {
            _writeToLog = writeToLogFunc;
            _blastId = blastId;
            _emailId = emailId;
            _transnippet = transnippet;
            _literalPreview = literalPreview;
            _trace = trace;
        }

        public void GenerateTransnippets(
            BlastAbstract blast,
            Encryption enc,
            DataTable emailRowsDt,
            DataRow firstrow,
            bool testBlast,
            Dictionary<string, int> testBlastLinks)
        {
            _writeToLog("Start Transnippets");
            var htmlOriginal = _html;
            RebuildHtmlForTransnippets(emailRowsDt, htmlOriginal);
            var useOldLinks = Convert.ToBoolean(ConfigurationManager.AppSettings["OpenClick_UseOldSite"]);

            if (firstrow != null)
            {
                FinalizeBuildHtml(blast, enc, emailRowsDt, firstrow, testBlast, testBlastLinks, useOldLinks);
            }
            else
            {
                _literalPreview.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
                ApplicationLog.LogNonCriticalError(
                    $"Invalid BlastID: {_blastId} or EmailID: {_emailId}",
                    "PublicPreview.Page_Load",
                    ApplicationId);
            }
        }

        private void FinalizeBuildHtml(
            BlastAbstract blast,
            Encryption enc,
            DataTable emailRowsDt,
            DataRow firstrow,
            bool testBlast,
            Dictionary<string, int> testBlastLinks,
            bool useOldLinks)
        {
            var regex = new Regex("%%");
            ProcessBreakupHtmlMail(enc, testBlast, testBlastLinks, useOldLinks, regex, firstrow);
            ProcessTransnippetTemplate(emailRowsDt);
            _writeToLog("Done with Transnippets");
            ProcessTestBlastCode(blast, emailRowsDt, testBlast, testBlastLinks);
            _writeToLog("Start setting html to page");
            _literalPreview.Text = _html.ToString();
            _writeToLog("Done setting html to page");
        }

        private void ProcessTestBlastCode(BlastAbstract blast, DataTable emailRowsDt, bool testBlast, Dictionary<string, int> testBlastLinks)
        {
            if (!testBlast)
            {
                return;
            }

            foreach (var kvp in testBlastLinks)
            {
                var search = $"href=\"{ConfigurationManager.AppSettings["MVCActivity_DomainPath"]}/Clicks/{kvp.Key}\"";
                var link = BusinessBlastLink.GetByBlastLinkID(blast.BlastID, kvp.Value);
                if (string.IsNullOrWhiteSpace(link?.LinkURL))
                {
                    continue;
                }

                var originalLink = link.LinkURL;
                if (originalLink.Contains("%%"))
                {
                    foreach (DataColumn column in emailRowsDt.Columns)
                    {
                        var tag = $"%%{column.ColumnName}%%";
                        if (originalLink.Contains(tag))
                        {
                            originalLink = originalLink.Replace(tag, emailRowsDt.Rows[0][column.ColumnName].ToString());
                        }
                    }
                }

                var replace = $"orighref=\"{originalLink}\" {search}";
                _html = _html.Replace(search, replace);
            }

            _html.Append(TestBlastScript);
        }

        private void ProcessTransnippetTemplate(DataTable emailRowsDt)
        {
            if (_html.ToString().IndexOf("##TRANSNIPPET|", StringComparison.Ordinal) <= 0)
            {
                return;
            }

            var transnippetRegEx = new Regex("#{2}.*#{2}");
            var transnippetMatches = transnippetRegEx.Matches(_html.ToString());
            if (transnippetMatches.Count > 0)
            {
                _transnippet = new ArrayList();
                if (transnippetMatches.Count > 0)
                {
                    for (var i = 0; i < transnippetMatches.Count; i++)
                    {
                        _transnippet.Add(transnippetMatches[i].Value);
                    }
                }

                char[] splitter = {'|'};
                char[] udfSplitter = {','};
                if (_transnippet.Count <= 0)
                {
                    return;
                }

                foreach (var snippet in _transnippet)
                {
                    //Just Build the Table Frame Once for all the emails for the Transnippet History
                    var transnippetHtml = BuildSnippetHtmlTable(emailRowsDt, snippet, splitter, udfSplitter);
                    _html = _html.Replace(snippet.ToString(), transnippetHtml);
                }
            }
        }

        private static string BuildSnippetHtmlTable(DataTable emailRowsDt, object snippet, char[] splitter, char[] udfSplitter)
        {
            var transnippetHtml = new StringBuilder();

            var transnippetSplits =
                (((snippet.ToString()).Replace("##", "")).Replace("$$", ""))
                .Split(splitter);
            if (transnippetSplits.Length > 0)
            {
                var transnippetUdFs = transnippetSplits[2].Split(udfSplitter);
                transnippetHtml.Append($"<table cellpadding=1 cellspacing=1 style=\"{transnippetSplits[4].Replace("TBL-STYLE=", "")}\">");
                if (transnippetUdFs.Length > 0)
                {
                    transnippetHtml.Append($"<tr style=\"{transnippetSplits[3].Replace("HDR-STYLE=", "")}\">");
                    if (transnippetUdFs.Length > 0)
                    {
                        foreach (var udf in transnippetUdFs)
                        {
                            transnippetHtml.Append($"<td><b>{udf}</b></td>");
                        }
                    }

                    transnippetHtml.Append("</tr>");
                    for (var l = 0; l < emailRowsDt.Rows.Count; l++)
                    {
                        transnippetHtml.Append("<tr>");
                        foreach (var udf in transnippetUdFs)
                        {
                            try
                            {
                                transnippetHtml.Append($"<td>{emailRowsDt.Rows[l][udf]}</td>");
                            }
                            catch
                            {
                                transnippetHtml.Append("<td>&nbsp;</td>");
                            }
                        }

                        transnippetHtml.Append("</tr>");
                    }

                    transnippetHtml.Append("</table>");
                }
            }

            return transnippetHtml.ToString();
        }

        private void ProcessBreakupHtmlMail(
            Encryption enc,
            bool testBlast,
            Dictionary<string, int> testBlastLinks,
            bool useOldLinks,
            Regex regex,
            DataRow dataRow)
        {
            Array breakupHtmlMail = regex.Split(_html.ToString());
            _html = new StringBuilder();
            if (breakupHtmlMail.Length <= 0)
            {
                return;
            }

            for (var i = 0; i < breakupHtmlMail.Length; i++)
            {
                var lineData = breakupHtmlMail.GetValue(i).ToString();
                if (i % 2 == 0)
                {
                    _html.Append(lineData);
                }
                else
                {
                    if (lineData.Equals("ECN_Encrypt_Open"))
                    {
                        var finalLink = $"b={_blastId}&e={dataRow["EmailID"]}";
                        if (!useOldLinks)
                        {
                            finalLink = HttpUtility.UrlEncode(KM.Common.Encryption.Base64Encrypt(finalLink, enc));
                        }

                        _html.Append(finalLink);
                    }
                    else if (lineData.StartsWith("ECN_Encrypt", StringComparison.Ordinal))
                    {
                        var items = lineData.Split('_');
                        var finalLink = items.Length > 3 && !string.IsNullOrEmpty(items[3])
                            ? $"b={_blastId}&e={dataRow["EmailID"]}&lid={items[2]}&ulid={items[3]}"
                            : $"b={_blastId}&e={dataRow["EmailID"]}&lid={items[2]}";

                        if (!useOldLinks)
                        {
                            var encrypt = HttpUtility.UrlEncode(KM.Common.Encryption.Base64Encrypt(finalLink, enc));
                            if (testBlast && encrypt!=null && !testBlastLinks.ContainsKey(encrypt))
                            {
                                var match = FindLid.Match(finalLink);
                                if (match.Success)
                                {
                                    testBlastLinks.Add(encrypt, Convert.ToInt32(match.Groups[1].Value));
                                }
                            }

                            finalLink = encrypt;
                        }

                        _html.Append(finalLink);
                    }
                    else
                    {
                        _html.Append(dataRow[lineData]);
                    }
                }
            }
        }

        private void RebuildHtmlForTransnippets(DataTable emailRowsDt, StringBuilder htmlOriginal)
        {
            var transTotalCount = Content.CheckForTransnippet(_html.ToString());
            if (transTotalCount == -1)
            {
                throw new InvalidOperationException("Error Transnippet in HTML content");
            }

            if (transTotalCount > 0)
            {
                _html = new StringBuilder(Content.ModifyHTML(htmlOriginal.ToString(), emailRowsDt));
            }
        }

        public HtmlBuilder GenerateSocialScripts(BlastAbstract blast, Encryption enc, int groupId)
        {
            var campaignItem = GetCampaignItem(blast);
            if (campaignItem == null)
            {
                return this;
            }

            _writeToLog("Start Social Media Get");
            var listCism = CampaignItemSocialMedia.GetByCampaignItemID(campaignItem.CampaignItemID)
                    .OrderBy(x => x.SocialMediaID)
                    .ToList();
            var socialString = new StringBuilder();
            if (listCism.FindAll(x => x.SimpleShareDetailID == null).Count > 0)
            {
                socialString.Append("<table style=\"background-color:#FFFFFF;\"><tr style=\"background-color:#FFFFFF;text-decoration:none;border:none;\">");

                foreach (var campaignItemSocialMedia in listCism)
                {
                    AddSocialMediaWidget(campaignItemSocialMedia, socialString);
                }

                socialString.Append("</tr></table>");
                if (socialString.Length > 0)
                {
                    _html.Insert(0, socialString);
                }
            }

            _writeToLog("Done getting social media html");
            ReplaceSocialMediaLinks(enc, groupId);

            return this;
        }

        private void ReplaceSocialMediaLinks(Encryption enc, int groupId)
        {
            var socialDt = SocialMedia.GetSocialMediaCanShare();
            if (socialDt == null || socialDt.Count <= 0)
            {
                return;
            }

            _writeToLog("Start Social Media Replace");
            if (enc == null || enc.ID <= 0)
            {
                return;
            }

            foreach (var row in socialDt)
            {
                if (row.SocialMediaID.Equals(F2FSocialMediaId))
                {
                    _html = _html.Replace(
                        row.MatchString,
                        $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/emailtofriend.aspx?e={_emailId}&b={_blastId}");
                }
                else if (!row.SocialMediaID.Equals(FacebookLikeSocialId))
                {
                    var queryString = $"b={_blastId}&g={groupId}&e={_emailId}&m={row.SocialMediaID}";
                    var encryptedQuery = HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, enc));
                    _html = _html.Replace(row.MatchString,
                        ConfigurationManager.AppSettings["Social_DomainPath"] +
                        ConfigurationManager.AppSettings["SocialClick"] +
                        encryptedQuery);
                    _html = _html.Replace(row.MatchString,
                        ConfigurationManager.AppSettings["Social_DomainPath"] +
                        ConfigurationManager.AppSettings["SocialClick"] +
                        encryptedQuery);
                }
            }

            _writeToLog("Done with Social MEdia Replace");
        }

        private void AddSocialMediaWidget(EntitiesCampaignItemSocialMedia campaignItemSocialMedia, StringBuilder socialString)
        {
            if (campaignItemSocialMedia.SimpleShareDetailID != null)
            {
                return;
            }

            var sm = SocialMedia.GetSocialMediaByID(campaignItemSocialMedia.SocialMediaID);
            switch (campaignItemSocialMedia.SocialMediaID)
            {
                case FacebookSocialId:
                    socialString.Append(
                        $"<td><a href=\"{sm.MatchString}\"><img width=\"30px\" style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"{ConfigurationManager.AppSettings["Image_DomainPath"]}{sm.ImagePath}\" alt=\"FaceBook\" /></a></td>");
                    break;
                case TwitterSocialId:
                    socialString.Append(
                        $"<td><a href=\"{sm.MatchString}\"><img style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"{ConfigurationManager.AppSettings["Image_DomainPath"]}{sm.ImagePath}\" alt=\"Twitter\" /></a></td>");
                    break;
                case LinkedInSocialId:
                    socialString.Append(
                        $"<td><a href=\"{sm.MatchString}\"><img style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"{ConfigurationManager.AppSettings["Image_DomainPath"]}{sm.ImagePath}\" alt=\"LinkedIn\" /></a></td>");
                    break;
                case FacebookLikeSocialId:
                    BuildFacebookLikeWidget(campaignItemSocialMedia, sm, socialString);
                    break;
                case F2FSocialMediaId:
                    socialString.Append(
                        $"<td><a href=\"{sm.MatchString}\"><img style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"{ConfigurationManager.AppSettings["Image_DomainPath"]}{sm.ImagePath}\" alt=\"Forward\" /></a></td>");
                    break;
            }
        }

        private void BuildFacebookLikeWidget(
            EntitiesCampaignItemSocialMedia campaignItemSocialMedia,
            EntitiesSocialMedia sm,
            StringBuilder socialString)
        {
            try
            {
                var sma = SocialMediaAuth.GetBySocialMediaAuthID(campaignItemSocialMedia.SocialMediaAuthID.GetValueOrDefault());
                var fbProfile = SocialMediaHelper.GetFBUserProfile(sma.Access_Token);
                if (fbProfile != null)
                {
                    var sbFbLike = new StringBuilder();
                    sbFbLike.Append($"<a href=\"{sm.ShareLink.Replace("|link|", fbProfile["link"])}\">");
                    sbFbLike.Append($"<img src=\"{ConfigurationManager.AppSettings["Image_DomainPath"]}{sm.ImagePath}\" alt=\"FB Like\" style=\"border:none;height:30px;width:30px;text-decoration:none;\" /></a>");
                    socialString.Append($"<td>{sbFbLike}</td>");
                }
            }
            catch (Exception ex)
            {
                _trace.Warn("Error", "Exception suppressed", ex);
            }
        }

        private CampaignItem GetCampaignItem(BlastAbstract blast)
        {
            CampaignItem campaignItem;
            if (blast.TestBlast.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                _writeToLog("Get Campaignitem");
                var citb = CampaignItemTestBlast.GetByBlastID_NoAccessCheck(blast.BlastID, false);
                campaignItem = BusinessCampaignItem.GetByCampaignItemTestBlastID_NoAccessCheck(
                        citb.CampaignItemTestBlastID,
                        false);
                _writeToLog("Got Campaign item object");
            }
            else
            {
                _writeToLog("Get CampaignItem");
                campaignItem = BusinessCampaignItem.GetByBlastID_NoAccessCheck(blast.BlastID, false);
                _writeToLog("Got Campaign item object");
            }

            return campaignItem;
        }

        public HtmlBuilder GenerateLayoutPreview(bool isMobile, BlastAbstract blast, int groupId, DataRow firstrow, BaseChannel bc)
        {
            var htmlCandidate = GetHtmlCandidate(isMobile, blast, groupId, firstrow);

            var toParse = new List<string> { htmlCandidate };
            var dynamicTags = Content.GetTags(toParse, true) ?? Enumerable.Empty<string>();

            htmlCandidate = DoDynamicTags( htmlCandidate, dynamicTags, firstrow, blast.CustomerID.GetValueOrDefault());
            _writeToLog("Got Layout Preview");

            RSSFeed.Replace(ref htmlCandidate, blast.CustomerID.GetValueOrDefault(), false, blast.BlastID);
            _writeToLog("Done with RSS Feed replacement");

            htmlCandidate = TemplateFunctions.LinkReWriter(
                htmlCandidate,
                blast,
                blast.CustomerID.GetValueOrDefault().ToString(),
                ConfigurationManager.AppSettings["Communicator_VirtualPath"],
                bc.ChannelURL);
            _writeToLog("Done with link rewriter");
            _html.Append(htmlCandidate);
            return this;
        }

        private static string GetHtmlCandidate(bool isMobile, BlastAbstract blast, int groupId, DataRow firstrow)
        {
            string htmlCandidate;
            if (!ContentFilter.HasDynamicContent(blast.LayoutID.GetValueOrDefault()))
            {
                htmlCandidate = Layout.GetPreviewNoAccessCheck(
                    blast.LayoutID.GetValueOrDefault(),
                    CommunicatorEnums.ContentTypeCode.HTML,
                    isMobile,
                    blast.CustomerID.GetValueOrDefault(),
                    null,
                    groupId);
            }
            else
            {
                var sqlQuery =
                    $" select * from layout l, template t  where l.layoutid={blast.LayoutID.GetValueOrDefault()}  and l.templateid=t.templateid and l.IsDeleted = 0 and t.IsDeleted = 0";
                var layoutTable = DataFunctions.GetDataTable(sqlQuery);

                if (layoutTable.Rows.Count < 1)
                {
                    throw new InvalidOperationException("Layout template not found");
                }

                var dr = layoutTable.Rows[0];
                var templateSource = dr["TemplateSource"].ToString();
                var tableOptions = dr["TableOptions"].ToString();
                var slots = new int[9];

                slots[0] = GetContentSlotId(firstrow, dr, "ContentSlot1", "slot1");
                slots[1] = GetContentSlotId(firstrow, dr, "ContentSlot2", "slot2");
                slots[2] = GetContentSlotId(firstrow, dr, "ContentSlot3", "slot3");
                slots[3] = GetContentSlotId(firstrow, dr, "ContentSlot4", "slot4");
                slots[4] = GetContentSlotId(firstrow, dr, "ContentSlot5", "slot5");
                slots[5] = GetContentSlotId(firstrow, dr, "ContentSlot6", "slot6");
                slots[6] = GetContentSlotId(firstrow, dr, "ContentSlot7", "slot7");
                slots[7] = GetContentSlotId(firstrow, dr, "ContentSlot8", "slot8");
                slots[8] = GetContentSlotId(firstrow, dr, "ContentSlot9", "slot9");
                htmlCandidate = TemplateFunctions.EmailHTMLBody(templateSource,
                    tableOptions,
                    slots[0],
                    slots[1],
                    slots[2],
                    slots[3],
                    slots[4],
                    slots[5],
                    slots[6],
                    slots[7],
                    slots[8]);
            }

            return htmlCandidate;
        }

        private static int GetContentSlotId(DataRow firstrow, DataRow dr, string contentSlotField, string slotField)
        {
            if (firstrow.IsNull(slotField))
            {
                int defaultSlot1;
                int.TryParse(dr[contentSlotField].ToString(), out defaultSlot1);
                return defaultSlot1;
            }

            return Convert.ToInt32(firstrow[slotField]);
        }

        private static string DoDynamicTags(string html, IEnumerable<string> dynamicTags, DataRow dataRow, int customerId)
        {
            foreach (var tag in dynamicTags)
            {
                var dynamicContent = new StringBuilder();
                DataTable contentDataTable;
                if (dataRow[tag].ToString().Trim().Length > 0)
                {
                    contentDataTable = DataFunctions.GetDataTable(
                        $"SELECT * FROM Content WHERE ContentID={dataRow[tag]} and IsDeleted = 0");
                    try
                    {
                        dynamicContent.Append(contentDataTable.Rows[0][ContentSourceColumn]);
                    }
                    catch
                    {
                        dynamicContent.Append(contentDataTable.Rows[0][ContentSourceColumn]);
                    }

                }
                else
                {
                    var cleanTag = tag.Replace("ECN.DynamicTag.", "").Replace(".ECN.DynamicTag", "");
                    var dynamicTag = DynamicTag.GetByTag_NoAccessCheck(cleanTag, customerId, false);
                    contentDataTable = DataFunctions.GetDataTable(
                        $"SELECT * FROM Content WHERE ContentID={dynamicTag.ContentID} and IsDeleted = 0");
                    dynamicContent.Append(contentDataTable.Rows[0][ContentSourceColumn]);
                }
                html = html.Replace(tag, dynamicContent.ToString());
            }
            return html;
        }
    }
}