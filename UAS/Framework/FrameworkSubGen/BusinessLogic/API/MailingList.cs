using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class MailingList
    {
        private readonly string extension = "mailinglist/";
        public int EstimateListSize(Entity.Enums.Client client, int publication_id, DateTime grace_date, int grace_issues = 0)
        {
            if (publication_id > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/mailingList/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    webClient.QueryString.Add("publication_id", publication_id.ToString());
                    if (grace_date != null)
                        webClient.QueryString.Add("grace_date", grace_date.ToString());
                    if (grace_issues > 0)
                        webClient.QueryString.Add("grace_issues", grace_issues.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + extension);
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<int>(json);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return item;
            }
            else
                return 0;
        }
        public int GenerateList(Entity.Enums.Client client, int publication_id, DateTime? grace_date = null, bool run_ncoa = false, int grace_issues = 0, string name = "")
        {
            if (publication_id > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/mailingList/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    webClient.QueryString.Add("publication_id", publication_id.ToString());
                    webClient.QueryString.Add("run_ncoa", run_ncoa.ToString());
                    if (grace_date != null)
                        webClient.QueryString.Add("grace_date", grace_date.ToString());
                    if (grace_issues > 0)
                        webClient.QueryString.Add("grace_issues", grace_issues.ToString());
                    if (!string.IsNullOrEmpty(name))
                        webClient.QueryString.Add("name", name.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + extension);
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<int>(json);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return item;
            }
            else
                return 0;
        }
        public Entity.MailingList GetMailingList(Entity.Enums.Client client, int mailing_list_id)
        {
            if (mailing_list_id > 0)
            {
                Entity.MailingList item = new Entity.MailingList();
                try
                {
                    //GET https://api.knowledgemarketing.com/2/mailinglist/{mailing_list_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    //webClient.QueryString.Add("mailing_list_id", mailing_list_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + extension + mailing_list_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<Entity.MailingList>(json);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return item;
            }
            else
                return null;
        }
        public List<Entity.SubscriptionList> GetSubscriptionList(Entity.Enums.Client client, int mailingListId)
        {
            List<Entity.SubscriptionList> item = new List<Entity.SubscriptionList>();
            try
            {
                //GET https://api.knowledgemarketing.com/2/downloadmailinglist/#/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                string json = webClient.DownloadString(auth.BaseUri + "downloadmailinglist/" + mailingListId.ToString());
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json))
                    item = jf.FromJson<List<Entity.SubscriptionList>>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return item;
        }
    }
}
