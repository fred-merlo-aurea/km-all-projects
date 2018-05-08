using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using static FrameworkSubGen.Entity.Enums;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Subscription : BusinessLogicBase
    {
        private const string ErrorMessageSubscriptionIdInvalid = "subscription_id is required";
        private const string SubscriptionUriExtension = "subscriptions/";
        private const string SubscriptionIdParameterKey = "subscription_id";

        public Entity.Subscription GetSubscription(Client client, int subscription_id)
        {
            if (subscription_id > 0)
            {
                Entity.Subscription item = new Entity.Subscription();
                try
                {
                    //GET https://api.knowledgemarketing.com/2/subscriptions/{subscription_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    //webClient.QueryString.Add("subscription_id", subscription_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + SubscriptionUriExtension + subscription_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<Entity.Subscription>(json);
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
        public List<Entity.Subscription> GetSubscriptions(Client client)
        {
            Response.Subscription resp = new Response.Subscription();
            try
            {
                //GET https://api.knowledgemarketing.com/2/subscriptions/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                string json = webClient.DownloadString(auth.BaseUri + SubscriptionUriExtension);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    resp = jf.FromJson<Response.Subscription>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp.subscriptions;
        }
        public List<Entity.Subscription> GetSubscriptions(Client client, bool campaign_active, SubscriptionType type, int copies = -1, int issues_left = -1, int limit = -1, int max_copies = -1,
            int max_issues_left = -1, int min_copies = -1, int min_issues_left = -1, int publication_id = 0, int subscriber_id = 0)
        {
            Response.Subscription resp = new Response.Subscription();
            try
            {
                //GET https://api.knowledgemarketing.com/2/subscribers/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);

                webClient.QueryString.Add("campaign_active", campaign_active.ToString());
                if (copies > -1)
                    webClient.QueryString.Add("copies", copies.ToString());
                if (issues_left > -1)
                    webClient.QueryString.Add("issues_left", issues_left.ToString());
                if (limit > -1)
                    webClient.QueryString.Add("limit", limit.ToString());
                if (max_copies > -1)
                    webClient.QueryString.Add("max_copies", max_copies.ToString());
                if (max_issues_left > -1)
                    webClient.QueryString.Add("max_issues_left", max_issues_left.ToString());
                if (min_copies > -1)
                    webClient.QueryString.Add("min_copies", min_copies.ToString());
                if (min_issues_left > -1)
                    webClient.QueryString.Add("min_issues_left", min_issues_left.ToString());
                if (publication_id > 0)
                    webClient.QueryString.Add("publication_id", publication_id.ToString());
                if (subscriber_id > 0)
                    webClient.QueryString.Add("subscriber_id", subscriber_id.ToString());
                webClient.QueryString.Add("type", type.ToString());

                string json = webClient.DownloadString(auth.BaseUri + SubscriptionUriExtension);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    resp = jf.FromJson<Response.Subscription>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp.subscriptions;
        }

        public int Create(Client client, Entity.Subscription subscription, int subscriber_id)
        {
            if (subscription.issues > -2 && subscription.mailing_address_id > 0 && subscription.publication_id > 0 && subscriber_id > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/subscriptions/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    MapNameValueCollectionParameters(subscription, reqparm, subscriber_id);

                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + SubscriptionUriExtension, HttpMethod.POST.ToString(), reqparm);
                    if (responsebytes != null)
                    {
                        Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                        string[] json = Encoding.UTF8.GetString(responsebytes).Split(':');
                        if (json != null && json.Count() > 1)
                            item = jf.FromJson<int>(json[1].TrimEnd('}'));
                    }
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

        public void MapNameValueCollectionParameters(
            Entity.Subscription subscription, 
            NameValueCollection nameValueCollection, 
            int? subscriberId)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            foreach (var prop in typeof(Entity.Subscription).GetProperties())
            {
                nameValueCollection.Add($"{prop.Name}", prop.GetValue(subscription).ToString());
            }

            if (subscriberId != null)
            {
                nameValueCollection.Add("subscriber_id", subscriberId.ToString());
            }
        }

        public int Create(Client client, int issues, int mailing_address_id, int publication_id, int subscriber_id, bool renew_campaign_active, SubscriptionType type, int billing_address_id = 0, int copies = 0)
        {
            if (issues > -2 && mailing_address_id > 0 && publication_id > 0 && subscriber_id > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/subscriptions/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("issues", issues.ToString());
                    reqparm.Add("mailing_address_id", mailing_address_id.ToString());
                    reqparm.Add("publication_id", publication_id.ToString());
                    reqparm.Add("subscriber_id", subscriber_id.ToString());
                    if (billing_address_id > 0)
                        reqparm.Add("billing_address_id", billing_address_id.ToString());
                    if (copies > 0)
                        reqparm.Add("copies", copies.ToString());
                    reqparm.Add("renew_campaign_active", renew_campaign_active.ToString());
                    reqparm.Add("type", type.ToString());

                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + SubscriptionUriExtension, HttpMethod.POST.ToString(), reqparm);
                    if (responsebytes != null)
                    {
                        Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                        string[] json = Encoding.UTF8.GetString(responsebytes).Split(':');
                        if (json != null && json.Count() > 1)
                            item = jf.FromJson<int>(json[1].TrimEnd('}'));
                    }
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

        public string Update(Client client, Entity.Subscription subscription)
        {
            if (subscription.issues > -2 && subscription.mailing_address_id > 0 && subscription.subscription_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                    //PUT https://api.knowledgemarketing.com/2/subscriptions/{subscription_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    MapNameValueCollectionParameters(subscription, reqparm, null);
                    responsebytes = webClient.UploadValues(auth.BaseUri + SubscriptionUriExtension + subscription.subscription_id.ToString(), HttpMethod.PUT.ToString(), reqparm);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "issues, mailing_address_id and subscription_id are required";
        }
        public string Update(Client client, int subscription_id, int issues, int mailing_address_id, bool renew_campaign_active, SubscriptionType type, int billing_address_id = 0, int copies = -1)
        {
            if (issues > -2 && mailing_address_id > 0 && subscription_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                    //PUT https://api.knowledgemarketing.com/2/subscriptions/{subscription_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("issues", issues.ToString());
                    reqparm.Add("mailing_address_id", mailing_address_id.ToString());
                    if (billing_address_id > 0)
                        reqparm.Add("billing_address_id", billing_address_id.ToString());
                    if (copies > -1)
                        reqparm.Add("copies", copies.ToString());
                    reqparm.Add("renew_campaign_active", renew_campaign_active.ToString());
                    reqparm.Add("type", type.ToString());
                    responsebytes = webClient.UploadValues(auth.BaseUri + SubscriptionUriExtension + subscription_id.ToString(), HttpMethod.PUT.ToString(), reqparm);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "issues, mailing_address_id and subscription_id are required";
        }

        public string Delete(Client client, int subscriptionId)
        {
            if (subscriptionId <= 0)
            {
                return ErrorMessageSubscriptionIdInvalid;
            }

            var requestParameter = new NameValueCollection
                                   {
                                       { SubscriptionIdParameterKey, subscriptionId.ToString() }
                                   };

            return Delete(client, SubscriptionUriExtension, requestParameter);
        }
    }
}
