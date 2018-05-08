using System;
using System.Linq;
using System.Net;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class SubscriptionList
    {
        private readonly string extension = "downloadmailinglist/";
        public Entity.SubscriptionList Get(Entity.Enums.Client client, int mailing_list_id, bool download)
        {
            Entity.SubscriptionList item = new Entity.SubscriptionList();
            try
            {
                //GET https://api.knowledgemarketing.com/2/downloadmailinglist/#/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);

                webClient.QueryString.Add("download", download.ToString());
                string json = webClient.DownloadString(auth.BaseUri + extension + mailing_list_id.ToString());
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    item = jf.FromJson<Entity.SubscriptionList>(json);
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
