using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ECN_Framework_Entities.Salesforce
{
    [Serializable]
    [DataContract]
    public static class SF_Utilities
    {
        private const string XmlMasterSuppressedTag = "<Master_Suppressed__c>true</Master_Suppressed__c>";
        private const string XmlHasOptedOutOfEmail = "<HasOptedOutOfEmail>true</HasOptedOutOfEmail>";
        private const string XmlRootTag = "<sObjects xmlns=\"http://www.force.com/2009/06/asyncapi/dataload\">";
        private const string XmlEndTag = "</sObjects>";
        private const string XmlOpenObjectTag = "<sObject>";
        private const string XmlCloseObjectTag = "</sObject>";

        private static readonly Dictionary<string, string> StateDictionary = new Dictionary<string, string>()
        {
            ["alabama"] = "AL",
            ["alaska"] = "AK",
            ["new brunswick"] = "NB",
            ["ontario"] = "ON",
            ["alberta"] = "AB",
            ["quebec"] = "QC",
            ["manitoba"] = "MB",
            ["washington"] = "WA",
            ["texas"] = "TX",
            ["tennessee"] = "TN",
            ["arizona"] = "AZ",
            ["arkansas"] = "AR",
            ["california"] = "CA",
            ["colorado"] = "CO",
            ["connecticut"] = "CT",
            ["delaware"] = "DE",
            ["florida"] = "FL",
            ["georgia"] = "FA",
            ["hawaii"] = "HI",
            ["idaho"] = "ID",
            ["illinois"] = "IL",
            ["indiana"] = "IN",
            ["iowa"] = "IA",
            ["kansas"] = "KS",
            ["kentucky"] = "KY",
            ["louisiana"] = "LA",
            ["maine"] = "ME",
            ["maryland"] = "MD",
            ["massachusetts"] = "MA",
            ["michigan"] = "MI",
            ["minnesota"] = "MN",
            ["mississippi"] = "MS",
            ["missouri"] = "MO",
            ["montana"] = "MT",
            ["nebraska"] = "NE",
            ["nevada"] = "NV",
            ["new hampshire"] = "NH",
            ["new jersey"] = "NJ",
            ["new mexico"] = "NM",
            ["new york"] = "NY",
            ["north carolina"] = "NC",
            ["north dakota"] = "ND",
            ["ohio"] = "OH",
            ["oklahoma"] = "OK",
            ["oregon"] = "OR",
            ["pennsylvania"] = "PA",
            ["rhode island"] = "RI",
            ["south carolina"] = "SC",
            ["south dakota"] = "SD",
            ["utah"] = "UT",
            ["vermont"] = "VT",
            ["virginia"] = "VA",
            ["west virginia"] = "WV",
            ["wisconsin"] = "WI",
            ["wyoming"] = "WY",
            ["british columbia"] = "BC",
            ["newfoundland and labrador"] = "NL",
            ["nova scotia"] = "NS",
            ["northwest territories"] = "NT",
            ["prince edward island"] = "PE",
            ["saskatchewan"] = "SK",
            ["yukon"] = "YT",
            ["nunavut"] = "NU"
        };

        public enum Method
        {
            HEAD,
            GET,
            POST,
            PATCH,
            DELETE,
            PUT
        }
        public enum ResponseType
        {
            JSON,
            XML
        }
        public enum SFObject
        {
            Account,
            Contact,
            ContactTag,
            Lead,
            LeadTag,
            EmailMessage,
            EmailTemplate,
            Campaign,
            CampaignMember,
            CampaignMemberStatus,
            TagDefinition
        }

        public static string GetInstance()
        {
            if (SF_Authentication.Token != null)
                return SF_Authentication.Token.instance_url;
            else
                return System.Configuration.ConfigurationManager.AppSettings["SF_Instance"].ToString();
        }

        private static string GetBulkPrefix()
        {
            return "/services/async/28.0/";
        }
        private static string GetQueryPrefix()
        {
            return "/services/data/v28.0/query/?q=";
        }
        private static string GetInsertPrefix(SFObject sfObject)
        {
            return "/services/data/v28.0/sobjects/" + sfObject.ToString() + "/";
        }
        public static string PrepareBulkOperation()
        {
            string fullQ = GetInstance() + GetBulkPrefix();
            return fullQ;
        }
        public static string PrepareBatchAdd(string jobID)
        {
            string fullQ = GetInstance() + GetBulkPrefix() + "job/" + jobID + "/batch";
            return fullQ;
        }
        public static string PrepareInsert(SFObject sfObject)
        {
            string fullQ = GetInstance() + GetInsertPrefix(sfObject);
            return fullQ;
        }
        public static string PrepareUpdate(SFObject sfObject, string objectID)
        {
            string fullQ = GetInstance() + GetInsertPrefix(sfObject) + objectID;
            return fullQ;
        }
        public static string PrepareQuery(string query)
        {
            string fullQ = GetInstance() + GetQueryPrefix() + query;
            return fullQ;
        }

        private static string GetXMLForJobCreation(SFObject sfObject, string operation, string contentType)
        {
            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sbXML.AppendLine("<jobInfo xmlns=\"http://www.force.com/2009/06/asyncapi/dataload\">");
            sbXML.AppendLine("<operation>" + operation.ToLower() + "</operation>");
            sbXML.AppendLine("<object>" + sfObject.ToString() + "</object>");
            sbXML.AppendLine("<contentType>" + contentType + "</contentType>");
            sbXML.AppendLine("</jobInfo>");
            return sbXML.ToString();
        }

        private static string GetXMLForJobClosing()
        {
            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sbXML.AppendLine("<jobInfo xmlns=\"http://www.force.com/2009/06/asyncapi/dataload\">");
            sbXML.AppendLine("<state>Closed</state></jobInfo>");
            return sbXML.ToString();
        }

        public static WebRequest CreateNewJob(string accessToken, SFObject obj, string operation)
        {
            string q = PrepareBulkOperation() + "job";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(q);
            //req.Headers.Add("Authorization: OAuth " + accessToken);
            req.Headers.Add("X-SFDC-Session: " + accessToken);
            req.ContentType = "application/xml;charset=\"utf-8\"";
            req.Method = "POST";

            string xmlForJob = GetXMLForJobCreation(obj, operation, "XML");
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xmlForJob);
            req.ContentLength = buffer.Length;

            Stream str = req.GetRequestStream();

            str.Write(buffer, 0, buffer.Length);
            str.Flush();
            str.Close();

            return req;
        }

        public static WebRequest AddBatchToJob(string accessToken, string jobID, string jobToRun)
        {
            string batchURL = PrepareBatchAdd(jobID);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(batchURL);
            //req.Headers.Add("Authorization: OAuth " + accessToken);
            req.Headers.Add("X-SFDC-Session: " + accessToken);
            req.ContentType = "application/xml;charset=\"utf-8\"";
            req.Method = "POST";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(jobToRun);
            req.ContentLength = buffer.Length;
            Stream str = req.GetRequestStream();

            str.Write(buffer, 0, buffer.Length);
            str.Flush();
            str.Close();

            return req;
        }

        public static WebRequest CloseJob(string accessToken, string jobID)
        {
            string q = PrepareBulkOperation() + "job/" + jobID;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(q);
            //req.Headers.Add("Authorization: OAuth " + accessToken);
            req.Headers.Add("X-SFDC-Session: " + accessToken);
            req.ContentType = "application/xml;charset=\"utf-8\"";
            req.Method = "POST";

            string xmlForJob = GetXMLForJobClosing();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xmlForJob);
            req.ContentLength = buffer.Length;
            Stream str = req.GetRequestStream();

            str.Write(buffer, 0, buffer.Length);
            str.Flush();
            str.Close();

            return req;
        }

        public static WebRequest GetBatchState(string accessToken, string jobID, string batchID)
        {
            string q = PrepareBulkOperation() + "job/" + jobID + "/batch/" + batchID;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(q);
            //req.Headers.Add("Authorization: OAuth " + accessToken);
            req.Headers.Add("X-SFDC-Session: " + accessToken);
            //req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Method = "GET";

            return req;
        }

        public static WebRequest GetBatchResults(string accessToken, string jobID, string batchID)
        {
            string q = PrepareBulkOperation() + "job/" + jobID + "/batch/" + batchID + "/result";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(q);
            req.Headers.Add("X-SFDC-Session: " + accessToken);
            req.Method = "GET";

            return req;
        }

        public static StreamReader GetNextURL(string url)
        {
            string finalURL = GetInstance() + url;
            System.Net.WebRequest req = System.Net.WebRequest.Create(finalURL);
            req.Method = "GET";
            req.Headers.Add("Authorization: OAuth " + SF_Authentication.Token.access_token);
            req.Headers.Add("X-PrettyPrint:1");
            req.ContentType = "application/json";
            System.Net.WebResponse resp = req.GetResponse();

            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr;
        }

        public static string GetStateAbbr(string state)
        {
            string stateAbbreviation;
            return StateDictionary.TryGetValue(state.ToLower(), out stateAbbreviation)
                       ? stateAbbreviation
                       : state;
        }

        public static string GetXMLForMasterSuppressJob(Dictionary<string, string> dictionary)
        {
            return CreateXml(dictionary.Keys, XmlMasterSuppressedTag);
        }

        public static string GetXMLForOptOutJob(Dictionary<string, string> dictionary)
        {
            return CreateXml(dictionary.Keys, XmlHasOptedOutOfEmail);
        }

        private static string CreateXml(IEnumerable<string> keys, string additionalTag)
        {
            var builder = new StringBuilder();
            builder.Append(XmlRootTag);
            foreach (var key in keys)
            {
                builder.Append(XmlOpenObjectTag);
                builder.Append($"<id>{key}</id>");
                builder.Append(additionalTag);
                builder.Append(XmlCloseObjectTag);
            }
            builder.Append(XmlEndTag);

            return builder.ToString();
        }

        public static WebRequest CreateQueryRequest(string accessToken, string query, Method method, ResponseType rt = ResponseType.JSON)
        {
            string q = SF_Utilities.PrepareQuery(query);
            WebRequest req = WebRequest.Create(q);
            req.Headers.Add("Authorization: OAuth " + accessToken);
            req.Headers.Add("X-PrettyPrint:1");
            req.Method = method.ToString();
            req.ContentType = "application/" + rt.ToString();

            return req;
        }
        public static WebRequest CreateInsertRequest(string accessToken, string json, SFObject sfObject, ResponseType rt = ResponseType.JSON)
        {
            var q = SF_Utilities.PrepareInsert(sfObject);
            var req = WebRequest.Create(q);
            req.Headers.Add($"Authorization: OAuth {accessToken}");
            req.Method = Method.POST.ToString();
            req.ContentType = $"application/{rt.ToString().ToLower()}";

            var data = System.Text.Encoding.ASCII.GetBytes(json);
            req.ContentLength = data.Length;
            var os = req.GetRequestStream();
            os.Write(data, 0, data.Length);
            os.Flush();
            os.Close();

            return req;
        }
        public static WebRequest CreateUpdateRequest(string accessToken, string json, SFObject sfObject, string objectID, ResponseType rt = ResponseType.JSON)
        {
            string q = SF_Utilities.PrepareUpdate(sfObject, objectID);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(q);
            req.Headers.Add("Authorization: OAuth " + accessToken);
            req.ContentType = "application/json; charset=ASCII";
            req.Method = Method.PATCH.ToString();
            //req.ContentType = "application/" + rt.ToString().ToLower();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(json);
            req.ContentLength = data.Length;
            var os = req.GetRequestStream();
            os.Write(data, 0, data.Length);
            os.Flush();
            os.Close();

            return req;
        }
        public static string SelectAllQuery(Type obj)
        {
            List<string> props = obj.GetProperties().Select(pi => pi.Name).ToList();
            //SELECT%20Id,%20Title,%20FirstName,%20LastName,%20Street,%20City,%20State,%20PostalCode,%20Country,%20Email,%20Fax,%20MobilePhone,%20Phone%20FROM%20Lead
            int totalProps = props.Count;
            int counter = 1;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("SELECT%20");
            foreach (string s in props)
            {
                if (counter == totalProps)
                    sb.Append(s);
                else
                    sb.Append(s + ",%20");

                counter++;
            }
            sb.Append("%20FROM%20" + obj.Name.ToString().Replace("SF_", ""));
            return sb.ToString();
        }
        public static string SelectWhere(Type obj, string where)
        {
            List<string> props = obj.GetProperties().Select(pi => pi.Name).ToList();
            //SELECT%20Id,%20Title,%20FirstName,%20LastName,%20Street,%20City,%20State,%20PostalCode,%20Country,%20Email,%20Fax,%20MobilePhone,%20Phone%20FROM%20Lead
            int totalProps = props.Count;
            int counter = 1;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("SELECT%20");
            foreach (string s in props)
            {
                if (counter == totalProps)
                    sb.Append(s);
                else
                    sb.Append(s + ",%20");

                counter++;
            }
            sb.Append("%20FROM%20" + obj.Name.ToString().Replace("SF_", ""));
            if (where.ToUpper().StartsWith("WHERE") == false && where != string.Empty)
                where = "%20WHERE%20" + where;
            sb.Append("%20" + where.Replace(" ", "%20"));
            return sb.ToString();
        }
        public static void WriteToLog(string text)
        {
            StreamWriter logFile;
            logFile = new StreamWriter(new FileStream(System.Configuration.ConfigurationManager.AppSettings["LogPath"] + DateTime.Now.ToString("MM-dd-yyyy") + "_.log", System.IO.FileMode.Append));
            logFile.AutoFlush = true;
            logFile.WriteLine(DateTime.Now.ToString() + " " + text);
            logFile.Flush();
            logFile.Close();
        }

        public static void LogWebException(WebException ex, string requestURL)
        {
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine("**********************");
            sbLog.AppendLine("-- RequestURL --");
            sbLog.AppendLine(requestURL.ToString());
            sbLog.AppendLine(string.Empty);
            sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
            sbLog.AppendLine("-- Message --");
            if (ex.Message != null)
                sbLog.AppendLine(ex.Message);
            sbLog.AppendLine("-- InnerException --");
            if (ex.InnerException != null)
                sbLog.AppendLine(ex.InnerException.ToString());
            sbLog.AppendLine("-- Stack Trace --");
            if (ex.StackTrace != null)
                sbLog.AppendLine(ex.StackTrace);
            sbLog.AppendLine("-- Source --");
            if (ex.Source != null)
                sbLog.AppendLine(ex.Source);
            sbLog.AppendLine("-- Response --");
            if (ex.Response != null)
            {
                var response = ((HttpWebResponse)ex.Response);
                sbLog.AppendLine("Status Code: " + response.StatusCode.ToString());
                sbLog.AppendLine("Status Desc: " + response.StatusDescription.ToString());
                try
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var text = reader.ReadToEnd();
                            sbLog.AppendLine("RespStream: " + text);
                        }
                    }
                }
                catch { }
            }

            sbLog.AppendLine("**********************");

            WriteToLog(sbLog.ToString());
        }

        public static void LogException(Exception ex)
        {
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine("**********************");
            sbLog.AppendLine(string.Empty);
            sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
            sbLog.AppendLine("-- Message --");
            if (ex.Message != null)
                sbLog.AppendLine(ex.Message);
            sbLog.AppendLine("-- InnerException --");
            if (ex.InnerException != null)
                sbLog.AppendLine(ex.InnerException.ToString());
            sbLog.AppendLine("-- Stack Trace --");
            if (ex.StackTrace != null)
                sbLog.AppendLine(ex.StackTrace);
            sbLog.AppendLine("-- Source --");
            if (ex.Source != null)
                sbLog.AppendLine(ex.Source);
            sbLog.AppendLine("**********************");

            WriteToLog(sbLog.ToString());
        }

        public static List<string> ExcludedCharacters()
        {
            List<string> list = new List<string>();
            list.Add(",");
            list.Add("'");
            list.Add("%");
            list.Add("*");
            list.Add("#");
            list.Add("--");
            list.Add("&");
            list.Add("<");
            list.Add(">");
            list.Add(";");
            list.Add("xp_");
            list.Add("_");
            list.Add("/*");
            list.Add("*/");

            return list;
        }
        public static string CleanStringSqlInjection(string dirty)
        {
            if (!string.IsNullOrEmpty(dirty))
            {
                string clean = dirty;
                foreach (string s in ExcludedCharacters())
                {
                    clean = clean.Replace(s, "");
                }

                return clean;
            }
            else
                return string.Empty;
        }

        public class InsertResult
        {
            public InsertResult() { }
            //response - check for "success" : true
            //{
            //    "id" : "001D000000IqhSLIAZ",
            //    "errors" : [ ],
            //    "success" : true
            //}
            public string id { get; set; }
            public string[] errors { get; set; }
            public bool success { get; set; }

            public static bool Success(string resp)
            {
                InsertResult r = (InsertResult)JsonConvert.DeserializeObject(resp, typeof(InsertResult));
                return r.success;
            }

            public static string SuccessId(string resp)
            {
                InsertResult r = (InsertResult)JsonConvert.DeserializeObject(resp, typeof(InsertResult));
                return r.id;
            }
        }
    }
}