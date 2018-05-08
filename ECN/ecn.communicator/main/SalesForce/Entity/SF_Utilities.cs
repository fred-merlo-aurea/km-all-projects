using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public static class SF_Utilities
    {
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

        public static ECN_Framework_Entities.Salesforce.SF_Utilities.SFObject Fit(SFObject sfObject)
        {
            return (ECN_Framework_Entities.Salesforce.SF_Utilities.SFObject) (int) sfObject;
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

        public static string GetStateAbbr(string state)
        {
            return ECN_Framework_Entities.Salesforce.SF_Utilities.GetStateAbbr(state);
        }

        public static string GetXMLForMasterSuppressJob(Dictionary<string, string> MasterSuppress)
        {
           return ECN_Framework_Entities.Salesforce.SF_Utilities.GetXMLForMasterSuppressJob(MasterSuppress);
        }

        public static string GetXMLForOptOutJob(Dictionary<string, string> OptOut)
        {
           return ECN_Framework_Entities.Salesforce.SF_Utilities.GetXMLForOptOutJob(OptOut);
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
            sb.Append("%20FROM%20" + obj.Name.ToString().Replace("SF_",""));
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
            if (where.ToUpper().StartsWith("WHERE")==false && where!=string.Empty)
                where = "%20WHERE%20" + where;
            sb.Append("%20" + where.Replace(" ","%20"));
            return sb.ToString();
        }
        public static void WriteToLog(string text)
        {
            ECN_Framework_Entities.Salesforce.SF_Utilities.WriteToLog(text);
        }

        public static void LogWebException(WebException ex, string requestURL)
        {
            ECN_Framework_Entities.Salesforce.SF_Utilities.LogWebException(ex, requestURL);
        }

        public static void LogException(Exception ex)
        {
            ECN_Framework_Entities.Salesforce.SF_Utilities.LogException(ex);
        }

        public static List<string> ExcludedCharacters()
        {
            return ECN_Framework_Entities.Salesforce.SF_Utilities.ExcludedCharacters();
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