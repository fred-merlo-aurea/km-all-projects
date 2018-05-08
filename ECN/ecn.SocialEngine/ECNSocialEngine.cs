using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Net;
using System.IO;
using ECN_Framework_Entities.Communicator;
using System.Text.RegularExpressions;
using ecn.SocialEngine.Processors;
using KM.Common.Entity;
using BusinessCampaignItemSocialMedia = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia;
using BusinessBlast = ECN_Framework_BusinessLayer.Communicator.Blast;
using BusinessSimpleShareDetail = ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail;
using BusinessSocialMediaAuth =  ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth;
using BusinessEmojiFunctions = ECN_Framework_Common.Functions.EmojiFunctions;

namespace ecn.SocialEngine
{
    class ECNSocialEngine
    {
        private const int FaceboolNetworkId = 1;
        private const int TwitterNetworkId = 2;
        private const int LinkedInNetworkId = 3;

        //private string _StatusCode = ConfigurationManager.AppSettings["StatusCode"];
        private ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType _StatusCode = (ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType)System.Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType), ConfigurationManager.AppSettings["StatusCode"].ToString());
        private KMPlatform.Entity.User _User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);

        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            new ECNSocialEngine().StartEngine();
        }

        public void StartEngine()
        {
            Console.WriteLine(string.Format("Social Engine({1}) starts at {0}", DateTime.Now.ToString(), System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
            Console.WriteLine(string.Format("With additonal StatusCode = {0}", _StatusCode));

            while (true)
            {
                ProcessQueue();
            }
        }

        private void ProcessQueue()
        {
            CampaignItemSocialMedia cism = null;
            var applicationId = GetApplicationId();

            try
            {
                Console.WriteLine("---Checking for items to process : {0}", DateTime.Now);
                cism = BusinessCampaignItemSocialMedia.GetFirstToSendByStatus(_StatusCode);
                if (cism != null)
                {
                    Console.WriteLine("---START CampaignItemSocialMediaID : {0} at {1}", cism.CampaignItemSocialMediaID, DateTime.Now);
                    var blast = BusinessBlast.GetByCampaignItemID_NoAccessCheck(cism.CampaignItemID, false).OrderBy(x => x.BlastID).ToList().FirstOrDefault();

                    var encryption = Encryption.GetCurrentByApplicationID(applicationId);

                    if (cism.SimpleShareDetailID != null)
                    {
                        ISocialNetworkProcessor processor = null;

                        switch (cism.SocialMediaID)
                        {
                            case FaceboolNetworkId:
                                processor = new FacebookProcessor(applicationId, cism, blast, encryption, SetToError);
                                break;
                            case TwitterNetworkId:
                                processor = new TwitterProcessor(applicationId, cism, blast, encryption, SetToError);
                                break;
                            case LinkedInNetworkId:
                                processor = new LinkedInProcessor(applicationId, cism, blast, SetToError);
                                break;
                        }

                        processor?.Execute();
                    }
                }
                else
                {
                    Console.WriteLine("---No items to process : {0}", DateTime.Now);
                }
            }
            catch (Exception e)
            {
                LogProcessError(cism, e, applicationId);
            }
            Console.WriteLine(" ");
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        private void LogProcessError(CampaignItemSocialMedia cism, Exception e, int applicationId)
        {
            const string processQueueErrorTemplate = "Social engine({0}) encountered an exception in ProcessQueue.";
            if (cism != null)
            {
                var exceptionHint = string.Format(
                    "An exception happened when handling CampaignItemID = {5} for CampaignItemSocialMediaID = {6}{0} <BR>Exception Message: {1}{0} <BR>Exception Source: {2}{0} <BR>Stack Trace: {3}{0} <BR>Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message,
                    e.Source,
                    e.StackTrace,
                    e.InnerException,
                    cism.CampaignItemID,
                    cism.CampaignItemSocialMediaID);

                ApplicationLog.LogCriticalError(
                    e,
                    string.Format(processQueueErrorTemplate,
                        Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)),
                    applicationId,
                    exceptionHint);
                SetToError(cism);
            }
            else
            {
                var exceptionHint = string.Format(
                    "An exception happened.  <BR>Exception Message: {1}{0} <BR>Exception Source: {2}{0} <BR>Stack Trace: {3}{0} <BR>Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message,
                    e.Source,
                    e.StackTrace,
                    e.InnerException);
                ApplicationLog.LogCriticalError(
                    e,
                    string.Format(processQueueErrorTemplate,
                        Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)),
                    applicationId,
                    exceptionHint);

                Console.WriteLine("---FINISHED : Status : Failed at {0}", DateTime.Now);
            }

            Console.WriteLine(e.Message);
        }

        private static int GetApplicationId()
        {
            int applicationId;
            if (!int.TryParse(ConfigurationManager.AppSettings["KMCommon_Application"], out applicationId))
            {
                throw new ConfigurationErrorsException("KMCommon_Application");
            }

            return applicationId;
        }

        private void SetToError(CampaignItemSocialMedia cism, int errorCode = 0)
        {
            if (errorCode != 0)
            {
                cism.LastErrorCode = errorCode;
            }
            cism.Status = "Failed";
            BusinessCampaignItemSocialMedia.Save(cism);
            Console.WriteLine($"---STATUS : Failed at {DateTime.Now}");
        }
    }
}
