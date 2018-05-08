using System;
using System.Collections.Generic;
using System.Linq;
using KMPlatform.BusinessLogic;
using BusinessLogicApi = FrameworkSubGen.BusinessLogic.API;
using Enums = FrameworkSubGen.Entity.Enums;
using KMPlatformEntity = KMPlatform.Entity;
using User = KMPlatform.Entity.User;

namespace FrameworkSubGen
{
    public class SubGenUtils
    {
        private List<Entity.Account> accounts;

        public SubGenUtils()
        {
            SetAccounts();
        }
        public List<Entity.Account> GetAccounts()
        {
            if (accounts == null)
            {
                try
                {
                    BusinessLogic.API.Account aWrk = new BusinessLogic.API.Account();
                    accounts = aWrk.GetAccounts();
                }
                catch (Exception ex)
                {
                    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                    alWrk.LogCriticalError(msg, "SubGenUtils.GetLoginToken", KMPlatform.BusinessLogic.Enums.Applications.AMS_Web, "SubGen Integration");

                }
            }
            return accounts;
        }
        private void SetAccounts()
        {
            try
            {
                BusinessLogic.API.Account aWrk = new BusinessLogic.API.Account();
                accounts = aWrk.GetAccounts();
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                alWrk.LogCriticalError(msg, "SubGenUtils.GetLoginToken", KMPlatform.BusinessLogic.Enums.Applications.AMS_Web, "SubGen Integration");

            }
        }

        public string GetLoginToken(Entity.Enums.Client sgClient, KMPlatform.Entity.User user, bool isSgAdmin)
        {
            KMPlatform.BusinessLogic.SubGenUserMap sgumWorker = new KMPlatform.BusinessLogic.SubGenUserMap();
            List<KMPlatform.Entity.SubGenUserMap> sgUserMap = sgumWorker.Select(user.UserID);

            if (accounts == null)
                SetAccounts();

            string loginToken = string.Empty;
            try
            {
                BusinessLogic.API.User sgUserWrk = new BusinessLogic.API.User();
                if (sgUserMap.Count > 0 && sgUserMap.Exists(x => x.SubGenAccountName.Equals(sgClient.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                                                            x.ClientID == user.CurrentClient.ClientID))
                {
                    KMPlatform.Entity.SubGenUserMap sgum = sgUserMap.Single(x => x.SubGenAccountName.Equals(sgClient.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                                                                            x.ClientID == user.CurrentClient.ClientID);
                    loginToken = sgUserWrk.GetLoginToken(sgClient, sgum.SubGenUserId, false).api_login_token;
                }
                else
                {
                    loginToken = GetLoginToken_SgUserMapNotExists(sgClient, user, isSgAdmin);
                }
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                alWrk.LogCriticalError(msg, "SubGenUtils.GetLoginToken", KMPlatform.BusinessLogic.Enums.Applications.AMS_Web, "SubGen Integration");
            }

            return loginToken;
        }
        public string GetTestingLoginToken(KMPlatform.Entity.User user, bool isSgAdmin)
        {
            Entity.Enums.Client sgClient = Entity.Enums.Client.KM_API_Testing;

            KMPlatform.BusinessLogic.SubGenUserMap sgumWorker = new KMPlatform.BusinessLogic.SubGenUserMap();
            List<KMPlatform.Entity.SubGenUserMap> sgUserMap = sgumWorker.Select(user.UserID);

            if (accounts == null)
                SetAccounts();

            string loginToken = string.Empty;
            try
            {
                BusinessLogic.API.User sgUserWrk = new BusinessLogic.API.User();
                if (sgUserMap.Count > 0 && sgUserMap.Exists(x => x.SubGenAccountName.Equals(sgClient.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) && 
                                                            x.ClientID == user.CurrentClient.ClientID))
                {
                    KMPlatform.Entity.SubGenUserMap sgum = sgUserMap.Single(x => x.SubGenAccountName.Equals(sgClient.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                                                                            x.ClientID == user.CurrentClient.ClientID);
                    loginToken = sgUserWrk.GetLoginToken(sgClient, sgum.SubGenUserId, false).api_login_token;
                }
                else
                {
                    loginToken = GetLoginToken_SgUserMapNotExists(sgClient, user, isSgAdmin);
                }
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                alWrk.LogCriticalError(msg, "SubGenUtils.GetLoginToken", KMPlatform.BusinessLogic.Enums.Applications.AMS_Web, "SubGen Integration");
            }

            return loginToken;
        }

        private string GetLoginToken_SgUserMapNotExists(Enums.Client sgClient, User user, bool isSgAdmin)
        {
            var loginToken = string.Empty;
            var sgUserWrk = new BusinessLogicApi.User();
            var sgClientCompanyName = sgClient.ToString().Replace("_", " ");

            //get SugGen user, update KMUser.SubGenUserId then save
            var sgUsers = sgUserWrk.GetUsers(
                sgClient, 
                isSgAdmin, 
                0, 
                user.EmailAddress, 
                user.FirstName, 
                user.LastName);

            var entityUser = sgUsers.FirstOrDefault(x =>
                x.email.Equals(user.EmailAddress, StringComparison.CurrentCultureIgnoreCase));

            if (entityUser != null)
            {
                if (accounts.Exists(x => x.company_name.Equals(sgClientCompanyName)))
                {
                    var sgAccountId = accounts.Single(x => x.company_name.Equals(sgClientCompanyName)).account_id;

                    var subGenUserMap = SaveSubGenUserMap(user, sgAccountId, sgClient.ToString(), entityUser.user_id);
                    
                    loginToken = sgUserWrk.GetLoginToken(sgClient, subGenUserMap.SubGenUserId, false).api_login_token;
                }
            }
            else
            {
                if (accounts.Exists(x => x.company_name.Equals(sgClientCompanyName)))
                {
                    var sgAccountId = accounts.Single(x => x.company_name.Equals(sgClientCompanyName)).account_id;
                    
                    //create our user
                    var sgUser = new Entity.User
                    {
                        email = user.EmailAddress,
                        first_name = user.FirstName,
                        is_admin = isSgAdmin,
                        last_name = user.LastName,
                        password = user.Password
                    };

                    sgUser.user_id = sgUserWrk.Create(sgClient, sgUser);

                    var subGenUserMap = SaveSubGenUserMap(user, sgAccountId, sgClientCompanyName, sgUser.user_id);
                    
                    loginToken = sgUserWrk.GetLoginToken(sgClient, subGenUserMap.SubGenUserId, false).api_login_token;
                }
            }
            return loginToken;
        }

        private KMPlatformEntity.SubGenUserMap SaveSubGenUserMap(
            User user, 
            int sgAccountId, 
            string sgAccountName, 
            int sgUserId)
        {
            var subGenUserMap = new KMPlatformEntity.SubGenUserMap
            {
                ClientID = user.CurrentClient.ClientID, //needs to be set to UAS ClientId we are working in
                SubGenAccountId = sgAccountId,
                SubGenAccountName = sgAccountName,
                SubGenUserId = sgUserId,
                UserID = user.UserID
            };

            var sgUserMapWorker = new SubGenUserMap();
            sgUserMapWorker.Save(subGenUserMap);

            return subGenUserMap;
        }
    }
}