using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Bundle
    {
        private readonly string extension = "bundles/";
        public List<Entity.Bundle> GetBundles(Entity.Enums.Client client, bool active, bool promotional, double max_price = -1, double min_price = -1, string name = "", double price = -1,
            string promo_code = "")
        {
            List<Entity.Bundle> retList = new List<Entity.Bundle>();
            try
            {
                //GET https://api.knowledgemarketing.com/2/bundles/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                webClient.QueryString.Add("active", active.ToString());
                if (max_price > -1)
                    webClient.QueryString.Add("max_price", max_price.ToString());
                if (min_price > -1)
                    webClient.QueryString.Add("min_price", min_price.ToString());
                if (!string.IsNullOrEmpty(name))
                    webClient.QueryString.Add("name", name.ToString());
                if (price > -1)
                    webClient.QueryString.Add("price", price.ToString());
                webClient.QueryString.Add("promotional", promotional.ToString());
                if (!string.IsNullOrEmpty(promo_code))
                    webClient.QueryString.Add("promo_code", promo_code.ToString());
                string json = webClient.DownloadString(auth.BaseUri + extension);
                //Response.Bundle resp = jf.FromJson<Response.Bundle>(json);
                if (!string.IsNullOrEmpty(json) && json != "null")
                    retList = jf.FromJson<List<Entity.Bundle>>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return retList;//resp.bundles;
        }
        public Entity.Bundle GetBundle(Entity.Enums.Client client, int bundle_id)
        {
            Entity.Bundle item = new Entity.Bundle();
            if (bundle_id > 0)
            {
                try
                {
                    //GET https://api.knowledgemarketing.com/2/bundles/{bundle_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    //webClient.QueryString.Add("bundle_id", bundle_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + extension + bundle_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<Entity.Bundle>(json);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
            }
            return item;
        }
        public Entity.Bundle GetBundle(Entity.Enums.Client client, Entity.Bundle bundle)
        {
            Entity.Bundle item = new Entity.Bundle();
            if (bundle.bundle_id > 0)
            {
                try
                {
                    //GET https://api.knowledgemarketing.com/2/bundles/{bundle_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    //webClient.QueryString.Add("bundle_id", bundle.bundle_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + extension + bundle.bundle_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<Entity.Bundle>(json);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
            }
            return item;
        }
    }
}
