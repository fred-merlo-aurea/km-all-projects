using System;
using System.Collections.Generic;
using System.Configuration;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using BusinessSimpleShareDetail = ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail;
using BusinessSocialMediaAuth = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth;
using BusinessCampaignItemSocialMedia = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia;

namespace ecn.SocialEngine.Processors
{
    internal class LinkedInProcessor : ISocialNetworkProcessor
    {
        private const string SuccessResponseParam = "success";
        private const string UpdateKeyResponseParam = "updateKey";
        private const string ApplicationNameSetting = "KMCommon_Application";
        private const string ErrorMessageResponseParam = "LinkedInErrorMsg";
        private const string ActionName = "ECNSocialEngine.ProcessQueue";
        private readonly CampaignItemSocialMedia _cism;
        private readonly BlastAbstract _blast;
        private readonly SetToErrorDelegate _setToError;
        private readonly int _applicationId;

        public LinkedInProcessor(int applicationId, CampaignItemSocialMedia cism, BlastAbstract blast, SetToErrorDelegate setToError)
        {
            _applicationId = applicationId;
            _cism = cism;
            _blast = blast;
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
            try
            {
                simpleShareDetail = BusinessSimpleShareDetail.GetBySimpleShareDetailID(_cism.SimpleShareDetailID.Value);
                mediaAuth = BusinessSocialMediaAuth.GetBySocialMediaAuthID(_cism.SocialMediaAuthID.Value);
                simpleShareDetail.Title = EmojiFunctions.GetSubjectUTF(ProcessorHelper.ClearText(simpleShareDetail.Title));
                simpleShareDetail.Content = EmojiFunctions.GetSubjectUTF(ProcessorHelper.ClearText(simpleShareDetail.Content));
                simpleShareDetail.SubTitle = EmojiFunctions.GetSubjectUTF(ProcessorHelper.ClearText(simpleShareDetail.SubTitle));

                var results = SocialMediaHelper.PostToLI(mediaAuth.Access_Token,
                    simpleShareDetail.Title.Trim(),
                    simpleShareDetail.SubTitle.Trim(),
                    simpleShareDetail.Content,
                    simpleShareDetail.UseThumbnail.GetValueOrDefault(false),
                    simpleShareDetail.ImagePath.Trim(),
                    simpleShareDetail.PageID,
                    _blast.BlastID,
                    _blast.LayoutID.Value,
                    _blast.GroupID.Value);

                if (results[SuccessResponseParam].Equals(bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
                {
                    _cism.Status = Enums.SocialMediaStatusType.Sent.ToString();
                    _cism.PostID = results[UpdateKeyResponseParam];
                    BusinessCampaignItemSocialMedia.Save(_cism);
                    Console.WriteLine("---STATUS : Sent at {0}", DateTime.Now);
                }
                else if (results[SuccessResponseParam].Equals(bool.FalseString, StringComparison.InvariantCultureIgnoreCase))
                {
                    var errorMsg = results[ErrorMessageResponseParam];
                    _setToError(_cism, GetLinkedinErrorCode(errorMsg));
                }
            }
            catch (Exception ex)
            {
                if (mediaAuth == null || simpleShareDetail == null)
                {
                    ApplicationLog.LogCriticalError(ex,
                        "ECNSocialEngine.ProcessQueue",
                        Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationNameSetting]),
                        $"Unsuccessful post to LinkedIn<BR>CampaignItemSocialMediaID: {_cism.CampaignItemSocialMediaID}");
                }
                else
                {
                    ApplicationLog.LogCriticalError(ex,
                        ActionName,
                        Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationNameSetting]),
                        $"Unsuccessful post to LinkedIn<BR>CampaignItemSocialMediaID: {_cism.CampaignItemSocialMediaID}<BR>Token: {mediaAuth.Access_Token}<BR>Title: {simpleShareDetail.Title.Trim()}<BR>SubTitle: {simpleShareDetail.SubTitle.Trim()}");
                }

                _setToError(_cism);
            }
        }

        private static int GetLinkedinErrorCode(string ex)
        {
            var errorCodeMap = new Dictionary<string, string>
            {
                {"400", "400"},
                {"401", "401"},
                {"402", "402"},
                {"404", "404"},
                {"409", "409"}
            };
            var statusCode = ProcessorHelper.GetStatusCodeByMap(ex, errorCodeMap) ?? string.Empty;

            int retValue;
            int.TryParse(statusCode, out retValue);
            return retValue;
        }
    }
}
