using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using BusinessSimpleShareDetail = ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail;
using BusinessSocialMediaAuth = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth;
using BusinessCampaignItemSocialMedia = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia;

namespace ecn.SocialEngine.Processors
{
    internal class TwitterProcessor : ISocialNetworkProcessor
    {
        private const string ActionMethod = "ECNSocialEngine.ProcessQueue";
        private readonly CampaignItemSocialMedia _cism;
        private readonly BlastAbstract _blast;
        private readonly Encryption _encryption;
        private readonly SetToErrorDelegate _setToError;
        private readonly int _applicationId;

        public TwitterProcessor(int applicationId, CampaignItemSocialMedia cism, BlastAbstract blast, Encryption encryption, SetToErrorDelegate setToError)
        {
            _applicationId = applicationId;
            _cism = cism;
            _blast = blast;
            _encryption = encryption;
            _setToError = setToError;

            if (_cism.SimpleShareDetailID == null)
            {
                throw new ArgumentException("SimpleShareDetailID is not defined", nameof(cism));
            }

            if (_cism.SocialMediaAuthID == null)
            {
                throw new ArgumentException("SocialMediaAuthID is not defined", nameof(cism));
            }

            if (_blast.LayoutID == null)
            {
                throw new ArgumentException("LayoutID is not defined", nameof(cism));
            }

            if (_blast.GroupID == null)
            {
                throw new ArgumentException("GroupID is not defined", nameof(cism));
            }
        }

        public void Execute()
        {
            SimpleShareDetail simpleShareDetail = null;
            SocialMediaAuth mediaAuth = null;
            var oauth = new OAuthHelper();
            try
            {
                simpleShareDetail = BusinessSimpleShareDetail.GetBySimpleShareDetailID(_cism.SimpleShareDetailID.Value);
                mediaAuth = BusinessSocialMediaAuth.GetBySocialMediaAuthID(_cism.SocialMediaAuthID.Value);

                simpleShareDetail.Title = ProcessorHelper.ClearText(simpleShareDetail.Title);
                simpleShareDetail.Content = ProcessorHelper.ClearText(simpleShareDetail.Content);
                simpleShareDetail.SubTitle = ProcessorHelper.ClearText(simpleShareDetail.SubTitle);

                var postData = $"{simpleShareDetail.Content} {ConfigurationManager.AppSettings["Activity_DomainPath"]}{ConfigurationManager.AppSettings["SocialPreview"]}";
                var queryString = $"blastID={_blast.BlastID}&layoutID={_blast.LayoutID.Value}&m=2&g={_blast.GroupID.Value}";
                var encryptedString = HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, _encryption)) ?? string.Empty;

                // Encode +'s additionally as %2b
                var rgx = new Regex(@"\+");
                encryptedString = rgx.Replace(encryptedString, "%2b");

                postData += encryptedString;

                oauth.TweetOnBehalfOf(mediaAuth.Access_Token, mediaAuth.Access_Secret, postData, string.Empty);

                if (string.IsNullOrWhiteSpace(oauth.oauth_error))
                {
                    _cism.Status = Enums.SocialMediaStatusType
                        .Sent.ToString();
                    _cism.PostID = oauth.postid;
                    BusinessCampaignItemSocialMedia.Save(_cism);
                    Console.WriteLine("---STATUS : Sent at {0}", DateTime.Now);
                }
                else
                {
                    //unsuccessful post
                    var ex = new Exception(oauth.oauth_error);
                    ApplicationLog.LogCriticalError(ex,
                        ActionMethod,
                        Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                        $"Unsuccessful post to Twitter<BR>CampaignItemSocialMediaID: {_cism.CampaignItemSocialMediaID}" +
                        $"<BR>Token: {mediaAuth.Access_Token}<BR>Secret: {mediaAuth.Access_Secret}<BR>Content: {simpleShareDetail.Content}<BR>OauthError: {oauth.oauth_error}");

                    _setToError(_cism, GetTwitterErrorCode(ex));
                }
            }
            catch (Exception ex)
            {
                var oauthError = oauth.oauth_error;

                if (mediaAuth == null || simpleShareDetail == null)
                {
                    ApplicationLog.LogCriticalError(ex,
                        ActionMethod,
                        Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                        $"Unsuccessful post to Twitter<BR>CampaignItemSocialMediaID: {_cism.CampaignItemSocialMediaID}" +
                        $"<BR>OauthError: {oauthError}");
                }
                else
                {
                    ApplicationLog.LogCriticalError(ex,
                        ActionMethod,
                        Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                        $"Unsuccessful post to Twitter<BR>CampaignItemSocialMediaID: {_cism.CampaignItemSocialMediaID}" +
                        $"<BR>Token: {mediaAuth.Access_Token}<BR>Secret: {mediaAuth.Access_Secret}<BR>Content: {simpleShareDetail.Content}<BR>OauthError: {oauthError}");
                }

                _setToError(_cism, GetTwitterErrorCode(ex));
            }
        }        

        private static int GetTwitterErrorCode(Exception ex)
        {
            var errorCodeMap = new Dictionary<string, string>
            {
                {"88", "88"},
                {"130", "130"},
                {"131", "131"},
                {"185", "185"},
                {"226", "226"},
                {"429", "429"},
                {"500", "500"},
                {"501", "501"},
                {"502", "502"},
                {"503", "503"}, 
                {"504", "504"}
            };
            var statusCode = ProcessorHelper.GetStatusCodeByMap(ex.Message, errorCodeMap) ?? "401";

            int retValue;
            int.TryParse(statusCode, out retValue);
            return retValue;
        }
    }
}
