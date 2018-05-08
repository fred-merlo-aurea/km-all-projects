using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Core_AMS.Utilities;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class ChangeDataCapture
    {
        private const string ChangedSinceQueryStringKey = "changed_since";
        private const string BundlesEntityName = "bundles";
        private const string SubscribersEntityName = "subscribers";
        private const string ListDownloadsEntityName = "listdownloads";
        private const string AddressesEntityName = "addresses";
        private const string JsonStringResponseNull = "null";
        private const string EntitiesQueryStringKey = "entities";
        private const string ChangeDataCaptureUriExtension = "cdc/";

        public Entity.ChangeDataCapture Get(Entity.Enums.Client client, DateTime changed_since, List<Entity.Enums.Entities> entities)
        {
            FrameworkSubGen.Entity.ChangeDataCapture resp = new Entity.ChangeDataCapture();
            try
            {
                //DateTime.TryParse("3/29/2016 00:00:00", out changed_since);
                //GET https://api.knowledgemarketing.com/2/cdc
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                webClient.QueryString.Add(ChangedSinceQueryStringKey, changed_since.ToString());
                StringBuilder ents = new StringBuilder();
                foreach (var x in entities)
                    ents.Append(x.ToString() + ", ");
                webClient.QueryString.Add(EntitiesQueryStringKey, ents.ToString().TrimEnd(','));
                string url = auth.BaseUri;// "https://api.subscriptiongenius.com/2/kmtest.php/"; //auth.BaseUri
                string json = webClient.DownloadString(url + ChangeDataCaptureUriExtension);
                if (!string.IsNullOrEmpty(json) && json != JsonStringResponseNull)
                {
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    resp = jf.FromJson<FrameworkSubGen.Entity.ChangeDataCapture>(json);
                }
                webClient.Dispose();

            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp;
        }
        public Entity.ChangeDataCapture GetCustomfields(Entity.Enums.Client client, DateTime changed_since)
        {
            FrameworkSubGen.Entity.ChangeDataCapture resp = new Entity.ChangeDataCapture();
            try
            {
                //GET https://api.knowledgemarketing.com/2/cdc
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                webClient.QueryString.Add(ChangedSinceQueryStringKey, changed_since.ToString());
                webClient.QueryString.Add(EntitiesQueryStringKey, "customfields");
                string json = webClient.DownloadString(auth.BaseUri + ChangeDataCaptureUriExtension);
                json = json.Replace("values", "value_options");
                if (!string.IsNullOrEmpty(json) && json != JsonStringResponseNull)
                {
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    resp = jf.FromJson<FrameworkSubGen.Entity.ChangeDataCapture>(json);
                }
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp;
        }
        public Entity.ChangeDataCapture GetSubscriptions(Entity.Enums.Client client, DateTime changed_since)
        {
            FrameworkSubGen.Entity.ChangeDataCapture resp = new Entity.ChangeDataCapture();
            try
            {
                //GET https://api.knowledgemarketing.com/2/cdc
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                webClient.QueryString.Add(ChangedSinceQueryStringKey, changed_since.ToString());
                webClient.QueryString.Add(EntitiesQueryStringKey, "subscriptions");
                string json = webClient.DownloadString(auth.BaseUri + ChangeDataCaptureUriExtension);
                if (!string.IsNullOrEmpty(json) && json != JsonStringResponseNull)
                {
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    resp = jf.FromJson<FrameworkSubGen.Entity.ChangeDataCapture>(json);
                }
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp;
        }
        public Entity.ChangeDataCapture GetSubscribers(Entity.Enums.Client client, DateTime changedSince)
        {
            return GetChangeDataCaptureForEntity(client, changedSince, SubscribersEntityName);
        }

        public Entity.ChangeDataCapture GetAddresses(Entity.Enums.Client client, DateTime changedSince)
        {
            return GetChangeDataCaptureForEntity(client, changedSince, AddressesEntityName);
        }

        public Entity.ChangeDataCapture GetListdownloads(Entity.Enums.Client client, DateTime changedSince)
        {
            return GetChangeDataCaptureForEntity(client, changedSince, ListDownloadsEntityName);
        }

        public Entity.ChangeDataCapture GetPurchases(Entity.Enums.Client client, DateTime changed_since)
        {
            FrameworkSubGen.Entity.ChangeDataCapture resp = new Entity.ChangeDataCapture();
            try
            {
                //GET https://api.knowledgemarketing.com/2/cdc
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                webClient.QueryString.Add(ChangedSinceQueryStringKey, changed_since.ToString());
                webClient.QueryString.Add(EntitiesQueryStringKey, "purchases");
                string json = webClient.DownloadString(auth.BaseUri + ChangeDataCaptureUriExtension);
                if (!string.IsNullOrEmpty(json) && json != JsonStringResponseNull)
                {
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    resp = jf.FromJson<FrameworkSubGen.Entity.ChangeDataCapture>(json);
                }
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }

            return resp;
        }

        public Entity.ChangeDataCapture GetBundles(Entity.Enums.Client client, DateTime changedSince)
        {
            return GetChangeDataCaptureForEntity(client, changedSince, BundlesEntityName);
        }

        private Entity.ChangeDataCapture GetChangeDataCaptureForEntity(
            Entity.Enums.Client client,
            DateTime changedSince,
            string entityName)
        {
            var changeDataCapture = new Entity.ChangeDataCapture();
            try
            {
                var authentication = new Authentication();
                using (var webClient = authentication.GetClient(client))
                {
                    webClient.QueryString.Add(ChangedSinceQueryStringKey, changedSince.ToString());
                    webClient.QueryString.Add(EntitiesQueryStringKey, entityName);

                    var json = webClient.DownloadString(authentication.BaseUri + ChangeDataCaptureUriExtension);
                    if (!string.IsNullOrWhiteSpace(json) && json != JsonStringResponseNull)
                    {
                        var jsonFunctions = new JsonFunctions();
                        changeDataCapture = jsonFunctions.FromJson<Entity.ChangeDataCapture>(json);
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is WebException ||
                                       ex is NotSupportedException)
            {
                Authentication.SaveApiLog(ex, GetType().ToString(), GetType().Name);
            }

            return changeDataCapture;
        }
    }
}
