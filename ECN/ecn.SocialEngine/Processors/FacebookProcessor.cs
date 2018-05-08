using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using BusinessSimpleShareDetail = ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail;
using BusinessCampaignItemSocialMedia = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia;

namespace ecn.SocialEngine.Processors
{
    internal class FacebookProcessor : ISocialNetworkProcessor
    {
        private const string ActionName = "ECNSocialEngine.ProcessQueue";
        private const int MaxStandardEmoji = 255;
        private readonly CampaignItemSocialMedia _cism;
        private readonly BlastAbstract _blast;
        private readonly Encryption _encryption;
        private readonly SetToErrorDelegate _setToError;
        private readonly int _applicationId;

        public FacebookProcessor(int applicationId, CampaignItemSocialMedia cism, BlastAbstract blast, Encryption encryption, SetToErrorDelegate setToError)
        {
            _applicationId = applicationId;
            _cism = cism;
            _blast = blast;
            _encryption = encryption;
            _setToError = setToError;
        }

        public void Execute()
        {
            var simpleShareDetail = BusinessSimpleShareDetail.GetBySimpleShareDetailID(_cism.SimpleShareDetailID.Value);
            simpleShareDetail.Title = EmojiFunctions.GetSubjectUTF(simpleShareDetail.Title);
            simpleShareDetail.Content = EmojiFunctions.GetSubjectUTF(simpleShareDetail.Content);
            simpleShareDetail.SubTitle = EmojiFunctions.GetSubjectUTF(simpleShareDetail.SubTitle);
            simpleShareDetail.Title = ProcessorHelper.ClearText(simpleShareDetail.Title);
            simpleShareDetail.Content = ProcessorHelper.ClearText(simpleShareDetail.Content);
            simpleShareDetail.SubTitle = ProcessorHelper.ClearText(simpleShareDetail.SubTitle);
            simpleShareDetail.SubTitle = GetSubjectDecimal(simpleShareDetail.SubTitle);

            var previewLink = BuildPreviewLink();
            var fbPagePosturl = BuildFbPageFeedUrl(simpleShareDetail, previewLink);

            var request = (HttpWebRequest)WebRequest.Create(fbPagePosturl);
            request.Method = "POST";

            ExecuteFbFeedCall(request, fbPagePosturl);

            // It's a bug from 2009-2010 does it actual now? 
            CallPreviewLinkToEnforceFbCacheReady(previewLink);
        }

        private static void CallPreviewLinkToEnforceFbCacheReady(string previewLink)
        {
            try
            {
                var requestFb = WebRequest.Create($"https://graph.facebook.com?id={previewLink}&scrape=true") as HttpWebRequest;
                requestFb.Method = "POST";
                using (var response = (HttpWebResponse) requestFb.GetResponse())
                {
                    new StreamReader(response.GetResponseStream());
                }
            }
            catch
            {
                // POSSIBLE BUG: No exception logging there
            }
        }

        private void ExecuteFbFeedCall(WebRequest request, string fbPagePosturl)
        {
            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    var stream = new StreamReader(response.GetResponseStream());
                    var vals = stream.ReadToEnd();
                    vals = SocialMediaHelper.CleanJSONString(vals);
                    if (vals.Contains("id:"))
                    {
                        //successful post
                        var results = SocialMediaHelper.GetJSONDict(vals);
                        _cism.Status = Enums.SocialMediaStatusType.Sent.ToString();
                        try
                        {
                            _cism.PostID = results["id"];
                        }
                        catch
                        {
                            //POSSIBLE BUG: no exception logging
                        }

                        BusinessCampaignItemSocialMedia.Save(_cism);
                        Console.WriteLine("---STATUS : Sent at {0}", DateTime.Now);
                    }
                    else
                    {
                        //unsuccessful post

                        var ex = new Exception(vals);
                        ApplicationLog.LogCriticalError(ex,
                            ActionName,
                            _applicationId,
                            $"Unsuccessful post to FaceBook: {fbPagePosturl}<BR>CampaignItemSocialMediaID: {_cism.CampaignItemSocialMediaID}<BR>Vals: {vals}");
                        _setToError(_cism, GetFacebookErrorCode(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                _setToError(_cism, GetFacebookErrorCode(ex));
            }
        }

        private string BuildPreviewLink()
        {
            var previewLink =
                $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}{ConfigurationManager.AppSettings["SocialPreview"]}";
            var queryString = $"blastID={_blast.BlastID}&layoutID={_blast.LayoutID.Value}&m=1&g={_blast.GroupID.Value}";
            var encryptedString = HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, _encryption));
            previewLink += encryptedString;
            return previewLink;
        }

        private string BuildFbPageFeedUrl(SimpleShareDetail ssd, string previewLink)
        {
            var fbPagePosturl = new StringBuilder("https://graph.facebook.com/v2.2/{ssd.PageID}/feed?");
            fbPagePosturl.Append($"&access_token={ssd.PageAccessToken}");
            fbPagePosturl.Append($"&link={HttpUtility.UrlEncode(previewLink.Trim())}");
            if (ssd.UseThumbnail.HasValue && ssd.UseThumbnail.Value)
            {
                fbPagePosturl.Append($"&picture={HttpUtility.UrlEncode(ssd.ImagePath.Trim())}");
            }

            fbPagePosturl.Append($"&name={HttpUtility.UrlEncode(ssd.Title.Trim())}");
            fbPagePosturl.Append($"&description={HttpUtility.UrlEncode(ssd.SubTitle.Trim())}");
            fbPagePosturl.Append($"&message={HttpUtility.UrlEncode(ssd.Content.Trim())}");
            fbPagePosturl.Append($"&access_token={_cism.PageAccessToken.Trim()}");
            return fbPagePosturl.ToString();
        }

        private static string GetSubjectDecimal(string subject)
        {
            //parse fb emoji's to html rep
            if (!subject.Any(x => x > MaxStandardEmoji))
            {
                return subject;
            }

            var uniRegex = new Regex(@"([\u2000-\u23FF])");
            var otherRegex = new Regex(@"([\uD000-\uDBFF][\uD000-\uDFFF])");

            var matches = uniRegex.Matches(subject);
            foreach (Match m in matches)
            {
                subject = subject.Replace(m.Value, $"&#{EmojiFunctions.GetDecimalCodePoint(m.Value)};");
            }

            matches = otherRegex.Matches(subject);
            foreach (Match m in matches)
            {
                subject = subject.Replace(m.Value, $"&#{EmojiFunctions.GetDecimalCodePoint(m.Value)};");
            }
            return subject;
        }

        private static int GetFacebookErrorCode(Exception ex)
        {
            var errorCodeMap = new Dictionary<string, string>
            {
                {"1", "1"},
                {"2", "2"},
                {"4", "4"},
                {"5", "5"},
                {"17", "17"},
                {"18", "18"},
                {"300", "300"},
                {"310", "310"},
                {"340", "340"},
                {"400", "401"}, //This is supposed to be 401 
                {"401", "401"},
                {"613", "613"},
                {"700", "700"},
                {"750", "750"},
                {"752", "752"},
                {"754", "754"},
                {"800", "800"},
                {"802", "802"},
                {"805", "805"},
                {"951", "951"},
                {"Unauthorized", "401"},
            };
            var statusCode = ProcessorHelper.GetStatusCodeByMap(ex.Message, errorCodeMap)?? "Unauthorized";

            int retValue;
            int.TryParse(statusCode, out retValue);
            return retValue;
        }
    }
}
