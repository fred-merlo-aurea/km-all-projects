using System;
using System.Linq;
using System.Net;
using System.Text;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Authentication
    {
        public readonly string BaseUri = " https://api.knowledgemarketing.com/2/"; //" https://api.knowledgemarketing.com/2/" " https://api.subscriptiongenius.com/2/";;

        private readonly string MasterApiKey = "e057def7646741afb6b6e24c89dd2aeb";
        private readonly string MasterApiPassword = "b13f64c4a527adaa901f7434d4e198d1";

        private readonly string ApiKey_KM = "9cd1f2fd2d5d02df4e370787df470ee3";//KM account
        private readonly string ApiPassword_KM = "7d259940adb64ea0d9a4f577e96fd8c3";//KM account

        private readonly string ApiKey_ABCPublishing = "b8301c16ea864d38c78e8586800fb48f";
        private readonly string ApiPassword_ABCPublishing = "a168fd168b7ee56af04e606beee27626";

        private readonly string ApiKey_SourceMedia = "6673147e236c04243474d66e7e2ae862";
        private readonly string ApiPassword_SourceMedia = "0e783df1c42df7cbcc1806eefd49ae2a";

        private readonly string ApiKey_KMAPITesting = "8e0b907a95bc91da658ec8d286815867";
        private readonly string ApiPassword_KMAPITesting = "dae29558401be3db8e5bc28f68cdd462";

        private readonly string ApiKey_MacFadden = "d68443341f2cac491c218fde3d699e7b";
        private readonly string ApiPassword_MacFadden = "d79e08909500da5ec10aa005b8cc282e";

        private readonly string ApiKey_CEG = "462cb304288503e97446c4b61acdba00";
        private readonly string ApiPassword_CEG = "ae01b36cd66a2bc6a53b087ddadfc964";

        public static void ServicePointManagerSecurity()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public WebClient GetClient_Master()
        {
            WebClient client = new WebClient();
            ServicePointManagerSecurity();
            client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(MasterApiKey + ":" + MasterApiPassword)));
            return client;
        }

        public WebClient GetClient_KM()
        {
            WebClient client = new WebClient();
            ServicePointManagerSecurity();
            client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(ApiKey_KM + ":" + ApiPassword_KM)));
            return client;
        }

        public WebClient GetClient(Entity.Enums.Client client)
        {
            WebClient webclient = new WebClient();
            ServicePointManagerSecurity();
            switch (client)
            {
                case Entity.Enums.Client.Master:
                    webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(MasterApiKey + ":" + MasterApiPassword)));
                    break;
                case Entity.Enums.Client.Knowledge_Marketing:
                    webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(ApiKey_KM + ":" + ApiPassword_KM)));
                    break;
                case Entity.Enums.Client.Source_Media:
                    webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(ApiKey_SourceMedia + ":" + ApiPassword_SourceMedia)));
                    break;
                case Entity.Enums.Client.ABC_Publishing:
                    webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(ApiKey_ABCPublishing + ":" + ApiPassword_ABCPublishing)));
                    break;
                case Entity.Enums.Client.KM_API_Testing:
                    webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(ApiKey_KMAPITesting + ":" + ApiPassword_KMAPITesting)));
                    break;
                case Entity.Enums.Client.MacFadden:
                    webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(ApiKey_MacFadden + ":" + ApiPassword_MacFadden)));
                    break;
                case Entity.Enums.Client.Chief_Executive_Group:
                    webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(ApiKey_CEG + ":" + ApiPassword_CEG)));
                    break;
            }

            return webclient;
        }

        public static void SaveApiLog(Exception ex, string myClass, string myMethod)
        {
            KMPlatform.Entity.ApiLog log = new KMPlatform.Entity.ApiLog();
            log.Entity = myClass;
            log.Method = myMethod;
            log.ErrorMessage = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            log.RequestStartDate = DateTime.Now;
            log.RequestStartTime = DateTime.Now.TimeOfDay;
            log.RequestEndDate = DateTime.Now;
            log.RequestEndTime = DateTime.Now.TimeOfDay;

            KMPlatform.BusinessLogic.ApiLog alWorker = new KMPlatform.BusinessLogic.ApiLog();
            alWorker.Save(log);
        }
    }
}
