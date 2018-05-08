using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using static FrameworkSubGen.Entity.Enums;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Subscriber : BusinessLogicBase
    {
        private const string SubscriberIdParameterKey = "subscriber_id";
        private const string ErrorMessageSubscriberIdInvalid = "subscriber_id is required";
        private const string SubscriberUriExtension = "subscribers/";

        public List<Entity.Subscriber> GetSubscribers(Client client)
        {
            Response.Subscriber resp = new Response.Subscriber();
            try
            {
                //GET https://api.knowledgemarketing.com/2/subscribers/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                string json = webClient.DownloadString(auth.BaseUri + SubscriberUriExtension);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    resp = jf.FromJson<Response.Subscriber>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp.subscribers;
        }
        public List<Entity.Subscriber> GetSubscribers(Client client, string email = "", string first_name = "", string last_name = "", int limit = 0, string renewal_code = "", string source = "")
        {
            Response.Subscriber resp = new Response.Subscriber();
            try
            {
                //GET https://api.knowledgemarketing.com/2/subscribers/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                if (!string.IsNullOrEmpty(email))
                    webClient.QueryString.Add("email", email.ToString());
                if (!string.IsNullOrEmpty(first_name))
                    webClient.QueryString.Add("first_name", first_name.ToString());
                if (!string.IsNullOrEmpty(last_name))
                    webClient.QueryString.Add("last_name", last_name.ToString());
                if (limit > 0)
                    webClient.QueryString.Add("limit", limit.ToString());
                if (!string.IsNullOrEmpty(renewal_code))
                    webClient.QueryString.Add("renewal_code", renewal_code.ToString());
                if (!string.IsNullOrEmpty(source))
                    webClient.QueryString.Add("source", source.ToString());

                string json = webClient.DownloadString(auth.BaseUri + SubscriberUriExtension);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    resp = jf.FromJson<Response.Subscriber>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp.subscribers;
        }
        public Entity.Subscriber GetSubscriber(Client client, int subscriber_id)
        {
            if (subscriber_id > 0)
            {
                Entity.Subscriber item = new Entity.Subscriber();
                try
                {
                    //GET https://api.knowledgemarketing.com/2/subscribers/{subscriber_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    //webClient.QueryString.Add("subscriber_id", subscriber_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + SubscriberUriExtension + subscriber_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<Entity.Subscriber>(json);
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
        public int CreateSubscriber(Client client, Entity.Subscriber subscriber)
        {
            if (!string.IsNullOrEmpty(subscriber.first_name) && !string.IsNullOrEmpty(subscriber.last_name))
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/subscribers/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("create_date", subscriber.create_date.ToShortDateString());
                    reqparm.Add("delete_date", subscriber.delete_date.ToShortDateString());
                    reqparm.Add("email", subscriber.email);
                    reqparm.Add("first_name", subscriber.first_name);
                    reqparm.Add("last_name", subscriber.last_name);
                    reqparm.Add("password", subscriber.password);
                    reqparm.Add("renewal_code", subscriber.renewal_code);
                    reqparm.Add("source", subscriber.source);
                    reqparm.Add("subscriber_id", subscriber.subscriber_id.ToString());
                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + SubscriberUriExtension, reqparm);
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
        public int CreateSubscriber(Client client, string first_name, string last_name, string email = "", string password = "", string source = "")
        {
            if (!string.IsNullOrEmpty(first_name) && !string.IsNullOrEmpty(last_name))
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/subscribers/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("first_name", first_name);
                    reqparm.Add("last_name", last_name);
                    if (!string.IsNullOrEmpty(email))
                        reqparm.Add("email", email.ToString());
                    if (!string.IsNullOrEmpty(password))
                        reqparm.Add("password", password.ToString());
                    if (!string.IsNullOrEmpty(source))
                        reqparm.Add("source", source.ToString());
                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + SubscriberUriExtension, HttpMethod.POST.ToString(), reqparm);
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
        public string Update(Client client, Entity.Subscriber subscriber)
        {
            if (subscriber.subscriber_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                    //PUT https://api.knowledgemarketing.com/2/subscribers/{subscriber_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("create_date", subscriber.create_date.ToShortDateString());
                    reqparm.Add("delete_date", subscriber.delete_date.ToShortDateString());
                    reqparm.Add("email", subscriber.email);
                    reqparm.Add("first_name", subscriber.first_name);
                    reqparm.Add("last_name", subscriber.last_name);
                    reqparm.Add("password", subscriber.password);
                    reqparm.Add("renewal_code", subscriber.renewal_code);
                    reqparm.Add("source", subscriber.source);
                    reqparm.Add("subscriber_id", subscriber.subscriber_id.ToString());
                    responsebytes = webClient.UploadValues(auth.BaseUri + SubscriberUriExtension + subscriber.subscriber_id.ToString(), HttpMethod.PUT.ToString(), reqparm);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "subscriber_id is required";
        }
        public string Update(Client client, int subscriber_id, string first_name = "", string last_name = "", string email = "", string password = "")
        {
            if (subscriber_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                    //PUT https://api.knowledgemarketing.com/2/subscribers/{subscriber_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("subscriber_id", subscriber_id.ToString());
                    if (!string.IsNullOrEmpty(first_name))
                        reqparm.Add("first_name", first_name.ToString());
                    if (!string.IsNullOrEmpty(last_name))
                        reqparm.Add("last_name", last_name.ToString());
                    if (!string.IsNullOrEmpty(email))
                        reqparm.Add("email", email.ToString());
                    if (!string.IsNullOrEmpty(password))
                        reqparm.Add("password", password.ToString());

                    responsebytes = webClient.UploadValues(auth.BaseUri + SubscriberUriExtension + subscriber_id.ToString(), HttpMethod.PUT.ToString(), reqparm);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "subscriber_id is required";
        }

        public string DeleteSubscriber(Client client, int subscriberId)
        {
            if (subscriberId <= 0)
            {
                return ErrorMessageSubscriberIdInvalid;
            }

            var requestParameter = new NameValueCollection
                                   {
                                       { SubscriberIdParameterKey, subscriberId.ToString() }
                                   };

            return Delete(client, $"{SubscriberUriExtension}{subscriberId}", requestParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="login">Subscriber's email or renewal code</param>
        /// <param name="password">Subscriber's password or address postal code</param>
        public Object.SubscriberAuthenticate Authenticate(Client client, string login, string password)
        {
            Object.SubscriberAuthenticate item = new Object.SubscriberAuthenticate();
            try
            {
                //GET https://api.knowledgemarketing.com/2/subscribers/authenticate/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();

                if (!string.IsNullOrEmpty(login))
                    webClient.QueryString.Add("login", login.ToString());
                if (!string.IsNullOrEmpty(password))
                    webClient.QueryString.Add("password", password.ToString());

                string json = webClient.DownloadString(auth.BaseUri + SubscriberUriExtension);
                if (!string.IsNullOrEmpty(json) && json != "null")
                    item = jf.FromJson<Object.SubscriberAuthenticate>(json);
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
