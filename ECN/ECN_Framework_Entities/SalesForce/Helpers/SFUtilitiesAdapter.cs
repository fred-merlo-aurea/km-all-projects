using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using ECN_Framework_Entities.Salesforce.Convertors;
using ECN_Framework_Entities.Salesforce.Interfaces;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    public class SFUtilitiesAdapter : ISFUtilities
    {
        public WebRequest CreateQueryRequest(string accessToken, string query, Method method, ResponseType rt = ResponseType.JSON)
        {
            return SF_Utilities.CreateQueryRequest(accessToken, query, method, rt);
        }

        public WebRequest CreateUpdateRequest(string accessToken, string json, SFObject sfObject, string objectID, ResponseType rt = ResponseType.JSON)
        {
            return SF_Utilities.CreateUpdateRequest(accessToken, json, sfObject, objectID, rt);
        }

        public StreamReader GetNextURL(string url)
        {
            return SF_Utilities.GetNextURL(url);
        }

        public void LogWebException(WebException ex, string requestURL)
        {
            SF_Utilities.LogWebException(ex, requestURL);
        }

        public bool ProceedRequest(WebRequest request)
        {
            var result = GetResponseAsString(request);
            return result.Length == 0;
        }

        public string ProceedJobRequest(WebRequest request)
        {
            return GetResponseAsString(request);
        }

        public bool Update(string accessToken, SalesForceBase obj, SFObject objectType, string id)
        {
            var json = JsonConvert.SerializeObject(obj);
            var req = CreateUpdateRequest(accessToken, json, objectType, id, ResponseType.JSON);

            return SafeExecute<WebException, bool>(
                () => ProceedRequest(req),
                (ex) => LogWebException(ex, json));
        }

        public static TResult SafeExecute<T, TResult>(Func<TResult> func, Action<T> onException) where T : Exception
        {
            try
            {
                return func();
            }
            catch (T ex)
            {
                onException(ex);
            }

            return default(TResult);
        }

        public WebRequest CreateWebRequest(string url)
        {
            return WebRequest.Create(url);
        }

        public T ReadToken<T>(WebRequest request)
        {
            using (var resp = request.GetResponse())
            {
                using (Stream ms = resp.GetResponseStream())
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                    var token = (T)ser.ReadObject(ms);
                    return token;
                }
            }
        }

        public void WriteToLog(string text)
        {
            SF_Utilities.WriteToLog(text);
        }

        public WebRequest GetBatchResults(string accessToken, string jobId, string batchId)
        {
            return SF_Utilities.GetBatchResults(accessToken, jobId, batchId);
        }

        public WebRequest CreateNewJob(string accessToken, SFObject sfObject, string operation)
        {
            return SF_Utilities.CreateNewJob(accessToken, sfObject, operation);
        }

        public WebRequest GetBatchState(string accessToken, string jobId, string batchId)
        {
            return SF_Utilities.GetBatchState(accessToken, jobId, batchId);
        }

        public WebRequest AddBatchToJob(string accessToken, string jobId, string xmlString)
        {
            return SF_Utilities.AddBatchToJob(accessToken, jobId, xmlString);
        }

        public WebRequest CloseJob(string accessToken, string jobId)
        {
            return SF_Utilities.CloseJob(accessToken, jobId);
        }

        private static string GetResponseAsString(WebRequest request)
        {
            using (var resp = request.GetResponse())
            {
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    var sb = new StringBuilder();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        sb.AppendLine(line);
                    }
                    return sb.ToString();
                }
            }
        }

        public string GetXmlForOutJob(Dictionary<string, string> dic)
        {
            return SF_Utilities.GetXMLForOptOutJob(dic);
        }

        public T GetSingle<T>(string accessToken, string condition, EntityConverterBase converter) where T : new()
        {
            var query = SF_Utilities.SelectWhere(typeof(T), condition);
            var req = CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);
            try
            {
                var resp = req.GetResponse();
                var reader = new StreamReader(resp.GetResponseStream());
                var jsonList = new List<string>();
                var json = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    json.AppendLine(line);
                    jsonList.Add(line.Trim());
                }
                resp.Close();
                WriteToLog(json.ToString());
                return converter.Convert<T>(jsonList).FirstOrDefault();
            }
            catch (WebException ex)
            {
                LogWebException(ex, query);
            }
            return default(T);
        }

        public bool Insert(string accessToken, object obj, SFObject objectType)
        {
            var json = JsonConvert.SerializeObject(obj);
            var req = CreateInsertRequest(accessToken, json, objectType, ResponseType.JSON);

            return SafeExecute<WebException, bool>(() =>
            {
                var result = GetResponseAsString(req);
                return InsertResult.Success(result);
            },
            e => LogWebException(e, json));
        }
    }
}
