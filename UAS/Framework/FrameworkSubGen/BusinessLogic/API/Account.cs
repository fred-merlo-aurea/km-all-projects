using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Core_AMS.Utilities;
using KMPlatform.BusinessLogic;
using static FrameworkSubGen.Entity.Enums;
using Enums = FrameworkSubGen.Entity.Enums;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Account
    {
        #region API

        private const string AccountUriExtension = "accounts/";

        private const string ClassName = "FrameworkSubGen.BusinessLogic.API.Account";

        private const string CreateMethod = "Create";

        private const string CompanyParameterKey = "company_name";

        private const string EmailParameterKey = "email";

        private const string FirstNameParameterKey = "first_name";

        private const string LastNameParameterKey = "last_name";

        private const string PasswordParameterKey = "password";

        private const string PlanParameterKey = "plan";

        private const string WebsiteParameterKey = "website";

        public List<Entity.Account> GetAccounts()
        {
            List<Entity.Account> accounts = new List<Entity.Account>();
            try
            {
                // GET https://api.knowledgemarketing.com/2/accounts/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(Enums.Client.Master);
                string json = webClient.DownloadString(auth.BaseUri + AccountUriExtension);
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    Response.Account resp = jf.FromJson<Response.Account>(json);
                    accounts = resp.accounts;
                }

                webClient.Dispose();
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                alWrk.LogCriticalError(
                    msg,
                    "SubGen.GetLoginToken",
                    KMPlatform.BusinessLogic.Enums.Applications.AMS_Web,
                    "SubGen Integration");
            }

            return accounts;
        }

        public Entity.Account GetAccount(Enums.Client client, int account_id)
        {
            Entity.Account item = new Entity.Account();
            try
            {
                // GET https://api.knowledgemarketing.com/2/accounts/{account_id}/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);

                // webClient.QueryString.Add("account_id", account_id.ToString());
                string json = webClient.DownloadString(auth.BaseUri + AccountUriExtension + account_id.ToString());
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    item = jf.FromJson<Entity.Account>(json);
                }

                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.API.Account", "GetAccount");
            }

            return item;
        }

        public int Create(Enums.Client client, Entity.Account account, string firstName = "", string lastName = "", string password = "")
        {
            var item = 0;
            if (account == null)
            {
                return 0;
            }

            try
            {
                // POST https://api.knowledgemarketing.com/2/accounts/
                var authentication = new Authentication();
                using (var webClient = authentication.GetClient(client))
                {
                    var reqparm = new NameValueCollection();
                    AddToRequestParameter(reqparm, CompanyParameterKey, account.company_name);
                    AddToRequestParameter(reqparm, EmailParameterKey, account.email);
                    AddToRequestParameter(reqparm, FirstNameParameterKey, firstName, false);
                    AddToRequestParameter(reqparm, LastNameParameterKey, lastName, false);
                    AddToRequestParameter(reqparm, PasswordParameterKey, password, false);
                    AddToRequestParameter(reqparm, PlanParameterKey, account.plan.ToString());
                    AddToRequestParameter(reqparm, WebsiteParameterKey, account.website);

                    var responseBytes = webClient.UploadValues(
                        $"{authentication.BaseUri}{AccountUriExtension}",
                        HttpMethod.POST.ToString(),
                        reqparm);

                    if (responseBytes != null)
                    {
                        var json = Encoding.UTF8.GetString(responseBytes).Split(':');
                        if (json.Count() > 1)
                        {
                            var jsonFunctions = new JsonFunctions();
                            item = jsonFunctions.FromJson<int>(json[1].TrimEnd('}'));
                        }
                    }
                }
            }
            catch (Exception exception) when (exception is WebException 
                                              || exception is ArgumentNullException
                                              || exception is DecoderFallbackException
                                              || exception is ArgumentException)
            {
                Authentication.SaveApiLog(exception, ClassName, CreateMethod);
            }

            return item;
        }

        public string Delete(Enums.Client client, int account_id)
        {
            string json = string.Empty;
            try
            {
                // DELETE https://api.knowledgemarketing.com/2/accounts/{account_id}
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                json = webClient.UploadString(auth.BaseUri + AccountUriExtension, "DELETE", account_id.ToString());
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.API.Account", "Delete");
            }

            return json;
        }

        #endregion

        private static void AddToRequestParameter(
            NameValueCollection requestParameters,
            string name,
            string value,
            bool allowEmpty = true)
        {
            if (allowEmpty || !string.IsNullOrWhiteSpace(value))
            {
                requestParameters.Add(name, value);
            }
        }
    }
}