using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Core_AMS.Utilities;
using Enums = FrameworkSubGen.Entity.Enums;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class CustomField
    {
        private const string SupplyValuesForAllRequiredFieldsErrorMessage = "supply values for all required fields";
        private const string RequestParameterDisplayAsKey = "display_as";
        private const string RequestParameterDisqualifierKey = "disqualifier";
        private const string RequestParameterOrderKey = "order";
        private const string RequestParameterValueKey = "value";
        private const string RequestParameterActiveKey = "active";
        private const string CustomFieldsUriExtension = "customfields/";

        public List<Entity.CustomField> GetCustomFields(Entity.Enums.Client client)
        {
            List<Entity.CustomField> cfList = new List<Entity.CustomField>();
            try
            {
                //GET https://api.knowledgemarketing.com/2/customfields/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                string json = webClient.DownloadString(auth.BaseUri + CustomFieldsUriExtension);
                json = json.Replace("values", "value_options");
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    cfList = jf.FromJson<List<Entity.CustomField>>(json);
                //Response.CustomField resp = jf.FromJson<Response.CustomField>(json);
                webClient.Dispose();

            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            cfList.RemoveAll(x => x.text_value == null && x.value_options == null);
            foreach (var cf in cfList)
            {
                cf.value_options.RemoveAll(x => x.value == null);
            }
            return cfList;
        }
        public List<Entity.CustomField> GetCustomFieldsForSubscriber(Entity.Enums.Client client, int subscriberId)
        {
            //Response.CustomField resp = new Response.CustomField();
            List<Entity.CustomField> cfList = new List<Entity.CustomField>();
            try
            {
                //GET https://api.knowledgemarketing.com/2/customfields/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                //webClient.QueryString.Add("subscriber_id", subscriberId.ToString());
                string json = webClient.DownloadString(auth.BaseUri + CustomFieldsUriExtension + subscriberId.ToString());
                json = json.Replace("values", "value_options");
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    cfList = jf.FromJson<List<Entity.CustomField>>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            cfList.RemoveAll(x => x.text_value == null && x.value_options == null);
            foreach (var cf in cfList)
            {
                cf.value_options.RemoveAll(x => x.value == null);
            }
            return cfList;
        }
        public string SubscriberFieldUpdate(Entity.Enums.Client client, Object.SubscriberFieldUpdate subscriberFieldUpdate)
        {
            if (subscriberFieldUpdate.subscriber_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                string result = string.Empty;
                try
                {
                    ////PUT https://api.knowledgemarketing.com/2/customfields/
                    //Authentication auth = new Authentication();
                    //WebClient webClient = auth.GetClient(client);
                    //System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    //reqparm.Add("subscriber_id", subscriberFieldUpdate.subscriber_id.ToString());
                    //foreach (var f in subscriberFieldUpdate.fields)
                    //{
                    //    reqparm.Add("field_id", f.field_id.ToString());
                    //    reqparm.Add("text_value", f.text_value.ToString());
                    //    foreach (int i in f.option_ids)
                    //        reqparm.Add("option_id", i.ToString());
                    //}
                    //string url = auth.BaseUri;//auth.BaseUri   "https://api.subscriptiongenius.com/2/kmtest.php/"
                    //responsebytes = webClient.UploadValues(url + extension, Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                    //result = Encoding.UTF8.GetString(responsebytes);
                    ////byte[] testResp = new byte[] { };
                    ////testResp = webClient.UploadValues("https://api.subscriptiongenius.com/2/kmtest.php/", Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                    ////string test = Encoding.UTF8.GetString(testResp);
                    //webClient.Dispose();

                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    string jsonOrder = jf.ToJson<Object.SubscriberFieldUpdate>(subscriberFieldUpdate);
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string url = auth.BaseUri;//auth.BaseUri   "https://api.subscriptiongenius.com/2/kmtest.php/"

                    //string testJson = webClient.UploadString("https://api.subscriptiongenius.com/2/kmtest.php/", "PUT", jsonOrder);
                    string json = webClient.UploadString(url + CustomFieldsUriExtension, "PUT", jsonOrder);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return result;
            }
            else
                return "must supply a subscriber_id";
        }
        public string SubscriberFieldUpdate(Entity.Enums.Client client, List<Object.SubscriberFieldUpdate> subscriberFieldUpdates)
        {
            byte[] responsebytes = new byte[] { };
            try
            {
                //PUT https://api.knowledgemarketing.com/2/customfields/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                foreach (Object.SubscriberFieldUpdate sfu in subscriberFieldUpdates)
                {
                    reqparm.Add("subscriber_id", sfu.subscriber_id.ToString());
                    foreach (var f in sfu.fields)
                    {
                        reqparm.Add("field_id", f.field_id.ToString());
                        //reqparm.Add("text_value", f.text_value.ToString());
                        foreach (int i in f.option_ids)
                            reqparm.Add("option_id", i.ToString());
                    }
                }
                responsebytes = webClient.UploadValues(auth.BaseUri + CustomFieldsUriExtension, Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return Encoding.UTF8.GetString(responsebytes);
        }
        public int Create(Entity.Enums.Client client, ref Entity.CustomField customField)
        {
            if (!string.IsNullOrEmpty(customField.display_as) && !string.IsNullOrEmpty(customField.name))
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/customfields/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();

                    reqparm.Add("allow_other", customField.allow_other.ToString());
                    reqparm.Add("display_as", customField.display_as.ToString());
                    reqparm.Add("field_id", customField.field_id.ToString());
                    reqparm.Add("name", customField.name.ToString());
                    if (customField.text_value != null)
                        reqparm.Add("text_value", customField.text_value.ToString());
                    else
                        reqparm.Add("text_value", string.Empty);
                    reqparm.Add("type", customField.type.ToString());

                    //foreach (var vo in customField.value_options)
                    //{
                    //    reqparm.Add("active", vo.active.ToString());
                    //    reqparm.Add("display_as", vo.display_as.ToString());
                    //    reqparm.Add("disqualifier", vo.disqualifier.ToString());
                    //    reqparm.Add("option_id", vo.option_id.ToString());
                    //    reqparm.Add("order", vo.order.ToString());
                    //    reqparm.Add("value", vo.value.ToString());
                    //}

                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + CustomFieldsUriExtension, Entity.Enums.HttpMethod.POST.ToString(), reqparm);
                    if (responsebytes != null)
                    {
                        Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                        string[] json = Encoding.UTF8.GetString(responsebytes).Split(':');
                        if (json != null && json.Count() > 1)
                            item = jf.FromJson<int>(json[1].TrimEnd('}'));
                    }
                    webClient.Dispose();

                    customField.field_id = item;
                    foreach (var vo in customField.value_options)
                    {
                        try
                        {
                            vo.field_id = item;
                            vo.option_id = CreateFieldOption(client, item, vo);
                        }
                        catch (Exception ex)
                        {
                            Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                        }
                    }


                    //Authentication auth = new Authentication();
                    //WebClient webClient = auth.GetClient(client);
                    //Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    //string jsonOrder = jf.ToJson<Entity.CustomField>(customField);
                    //webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    //string url = auth.BaseUri;//auth.BaseUri   "https://api.subscriptiongenius.com/2/kmtest.php/"
                    //string[] json = webClient.UploadString(url + extension, "POST", jsonOrder).Split(':');
                    //if (json != null && json.Count() > 1)
                    //    item = jf.FromJson<int>(json[1].TrimEnd('}'));
                    //webClient.Dispose();
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
        public int Create(Entity.Enums.Client client, bool allow_other, string display_as, string name, Entity.Enums.HtmlFieldType fieldType)
        {
            if (!string.IsNullOrEmpty(display_as) && !string.IsNullOrEmpty(name))
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/customfields/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("allow_other", allow_other.ToString());
                    reqparm.Add("display_as", display_as.ToString());
                    reqparm.Add("name", name.ToString());
                    reqparm.Add("type", fieldType.ToString());

                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();

                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + CustomFieldsUriExtension, Entity.Enums.HttpMethod.POST.ToString(), reqparm);
                    if (responsebytes != null)
                    {
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
        public string Update(Entity.Enums.Client client, Entity.CustomField customField)
        {
            if (customField.field_id > 0 && !string.IsNullOrEmpty(customField.display_as) && !string.IsNullOrEmpty(customField.name))
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                    //PUT https://api.knowledgemarketing.com/2/customfields/{field_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("allow_other", customField.allow_other.ToString());
                    reqparm.Add("display_as", customField.display_as.ToString());
                    reqparm.Add("field_id", customField.field_id.ToString());
                    reqparm.Add("name", customField.name.ToString());
                    reqparm.Add("text_value", customField.text_value.ToString());
                    reqparm.Add("type", customField.type.ToString());
                    foreach (var vo in customField.value_options)
                    {
                        reqparm.Add("active", vo.active.ToString());
                        reqparm.Add("display_as", vo.display_as.ToString());
                        reqparm.Add("disqualifier", vo.disqualifier.ToString());
                        reqparm.Add("option_id", vo.option_id.ToString());
                        reqparm.Add("order", vo.order.ToString());
                        reqparm.Add("value", vo.value.ToString());
                    }

                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    responsebytes = webClient.UploadValues(auth.BaseUri + CustomFieldsUriExtension + customField.field_id.ToString(), Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "supply values for all required fields";
        }
        public string Update(Entity.Enums.Client client, int field_id, bool allow_other, string display_as, string name, Entity.Enums.HtmlFieldType fieldType)
        {
            if (field_id > 0 && !string.IsNullOrEmpty(display_as) && !string.IsNullOrEmpty(name))
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                    //PUT https://api.knowledgemarketing.com/2/customfields/{field_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("allow_other", allow_other.ToString());
                    reqparm.Add("display_as", display_as.ToString());
                    reqparm.Add("field_id", field_id.ToString());
                    reqparm.Add("name", name.ToString());
                    reqparm.Add("type", fieldType.ToString());

                    responsebytes = webClient.UploadValues(auth.BaseUri + CustomFieldsUriExtension + field_id.ToString(), Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "supply values for all required fields";
        }
        #region Field/ValueOption
        public int CreateFieldOption(Entity.Enums.Client client, int fieldId, Entity.ValueOption valueOption)
        {
            if (fieldId > 0 && !string.IsNullOrEmpty(valueOption.display_as) && !string.IsNullOrEmpty(valueOption.value) && valueOption.order > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/customfields/{field_id}/options
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    int active = 0;
                    if (valueOption.active == true)
                        active = 1;
                    reqparm.Add("active", active.ToString());
                    reqparm.Add("display_as", valueOption.display_as.ToString());
                    reqparm.Add("disqualifier", valueOption.disqualifier.ToString());
                    reqparm.Add("option_id", valueOption.option_id.ToString());
                    reqparm.Add("order", valueOption.order.ToString());
                    reqparm.Add("value", valueOption.value.ToString());
                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + CustomFieldsUriExtension + fieldId.ToString() + "/options", Entity.Enums.HttpMethod.POST.ToString(), reqparm);
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

        public int CreateFieldOption(Enums.Client client, int fieldId, bool active, string displayAs, bool disqualifier, int order, string value)
        {
            if (fieldId <= 0
                || string.IsNullOrWhiteSpace(displayAs)
                || string.IsNullOrWhiteSpace(value)
                || order <= 0)
            {
                return 0;
            }

            var item = 0;
            try
            {
                // POST https://api.knowledgemarketing.com/2/customfields/{field_id}/options
                var authentication = new Authentication();
                var address = $"{authentication.BaseUri}{CustomFieldsUriExtension}{fieldId}/options";

                var responseBytes = PostToServer(client, address, active, displayAs, disqualifier, order, value);

                if (responseBytes != null)
                {
                    var jsonFunctions = new JsonFunctions();
                    var json = Encoding.UTF8.GetString(responseBytes);

                    var stringParts = json.Split(':');
                    if (stringParts.Length > 1)
                    {
                        item = jsonFunctions.FromJson<int>(stringParts[1].TrimEnd('}'));
                    }
                }
            }
            catch (Exception ex) when
                   (ex is WebException
                    || ex is ArgumentNullException
                    || ex is DecoderFallbackException)
            {
                Authentication.SaveApiLog(ex, GetType().ToString(), GetType().Name);
            }

            return item;
        }

        public string UpdateFieldOption(Enums.Client client, int fieldId, bool active, string displayAs, bool disqualifier, int order, string value)
        {
            if (fieldId <= 0
                || string.IsNullOrWhiteSpace(displayAs)
                || string.IsNullOrWhiteSpace(value)
                || order <= 0)
            {
                return SupplyValuesForAllRequiredFieldsErrorMessage;
            }

            try
            {
                //POST https://api.knowledgemarketing.com/2/customfields/options/{option_id}
                var authentication = new Authentication();
                var address = $"{authentication.BaseUri}{CustomFieldsUriExtension}/options/{fieldId}";

                var responseBytes = PostToServer(client, address, active, displayAs, disqualifier, order, value, fieldId);
                return Encoding.UTF8.GetString(responseBytes);
            }
            catch (Exception ex) when
            (ex is WebException
             || ex is ArgumentNullException
             || ex is DecoderFallbackException)
            {
                Authentication.SaveApiLog(ex, GetType().ToString(), GetType().Name);
            }

            return string.Empty;
        }

        public string UpdateFieldOption(Entity.Enums.Client client, Entity.ValueOption valueOption)
        {
            if (valueOption.option_id > 0 && !string.IsNullOrEmpty(valueOption.display_as) && !string.IsNullOrEmpty(valueOption.value) && valueOption.order > 0)
            {
                string result = string.Empty;
                try
                {
                    //PUT https://api.knowledgemarketing.com/2/customfields/options/{option_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);

                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    //reqparm.Add("option_id", valueOption.option_id.ToString());
                    reqparm.Add("active", valueOption.active.ToString());
                    reqparm.Add("display_as", valueOption.display_as.ToString());
                    reqparm.Add("disqualifier", valueOption.disqualifier.ToString());
                    reqparm.Add("order", valueOption.order.ToString());
                    reqparm.Add("value", valueOption.value.ToString());

                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    string data = jf.ToJson<Entity.ValueOption>(valueOption);
                    string url = auth.BaseUri + CustomFieldsUriExtension + "/options/" + valueOption.option_id.ToString();

                    try
                    {
                        //url += "?active=" + valueOption.active.ToString() + "&display_as=" + valueOption.display_as.ToString() +
                        //    "&disqualifier=" + valueOption.disqualifier.ToString() + "&order=" + valueOption.order.ToString() + "&value=" + valueOption.value.ToString();
                        //string result = webClient.UploadString(url, "PUT","");
                        result = webClient.UploadString(url, "PUT", data);
                    }
                    catch (Exception ex)
                    {
                        Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                    }
                    try
                    {
                        byte[] responsebytes = webClient.UploadValues(url, Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                        webClient.Dispose();
                        result = Encoding.UTF8.GetString(responsebytes);
                    }
                    catch (Exception ex)
                    {
                        Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return result;
            }
            else
                return "supply values for all required fields";
        }

        private byte[] PostToServer(
            Enums.Client client,
            string address,
            bool active,
            string displayAs,
            bool disqualifier,
            int order,
            string value,
            int? fieldId = null)
        {
            var authentication = new Authentication();
            byte[] responsebytes;

            using (var webClient = authentication.GetClient(client))
            {
                var reqparm = new System.Collections.Specialized.NameValueCollection
                        {
                            { RequestParameterActiveKey, active.ToString() },
                            { RequestParameterDisplayAsKey, displayAs },
                            { RequestParameterDisqualifierKey, disqualifier.ToString() },
                            { RequestParameterOrderKey, order.ToString() },
                            { RequestParameterValueKey, value }
                        };

                if (fieldId.HasValue)
                {
                    reqparm.Add("field_option_id", fieldId.Value.ToString());
                }

                responsebytes = webClient.UploadValues(
                    address,
                    Enums.HttpMethod.POST.ToString(),
                    reqparm);
            }

            return responsebytes;
        }
        #endregion
    }
}
