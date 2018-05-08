using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using static FrameworkSubGen.Entity.Enums;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class History : BusinessLogicBase
    {
        private const string HistoryUriExtension = "history/";
        private const string ErrorMessageHistoryIdInvalid = "history_id is required";
        private const string HistoryIdParameterKey = "history_id";

        public int Create(Client client, Entity.History history)
        {
            if (!string.IsNullOrEmpty(history.notes) && history.subscriber_id > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/history/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("date", history.date.ToShortDateString());
                    reqparm.Add("history_id", history.history_id.ToString());
                    reqparm.Add("notes", history.notes);
                    reqparm.Add("subscriber_id", history.subscriber_id.ToString());
                    reqparm.Add("user_id", history.user_id.ToString());

                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + HistoryUriExtension, HttpMethod.POST.ToString(), reqparm);
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
        public int Create(Client client, string notes, int subscriber_id)
        {
            if (!string.IsNullOrEmpty(notes) && subscriber_id > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/history/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("notes", notes);
                    reqparm.Add("subscriber_id", subscriber_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + HistoryUriExtension, HttpMethod.POST.ToString(), reqparm);
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
        public string Update(Client client, string notes, int history_id)
        {
            if (history_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                try 
                { 
                    //PUT https://api.knowledgemarketing.com/2/history/{history_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("notes", notes);
                    reqparm.Add("history_id", history_id.ToString());
                    responsebytes = webClient.UploadValues(auth.BaseUri + HistoryUriExtension + history_id.ToString(), HttpMethod.PUT.ToString(), reqparm);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "history_id is required";
        }

        public string Delete(Client client, int historyId)
        {
            if (historyId <= 0)
            {
                return ErrorMessageHistoryIdInvalid;
            }

            var requestParameter = new NameValueCollection
                                   {
                                       { HistoryIdParameterKey, historyId.ToString() }
                                   };

            return Delete(client, HistoryUriExtension, requestParameter);
        }

        public List<Entity.History> Get(Client client, int subscriber_id)
        {
            if (subscriber_id > 0)
            {
                Response.History resp = new Response.History();
                try
                {
                    //GET https://api.knowledgemarketing.com/2/history/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    webClient.QueryString.Add("subscriber_id", subscriber_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + HistoryUriExtension);
                    if (!string.IsNullOrEmpty(json) && json != "null")
                    {
                        Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                        resp = jf.FromJson<Response.History>(json);
                    }
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return resp.historyEntries;
            }
            else
                return null;
        }
    }
}
