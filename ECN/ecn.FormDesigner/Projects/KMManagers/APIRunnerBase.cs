using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Text;
using System.Data;
using KMManagers.APITypes;
using aspNetMX;
using System.Data;

namespace KMManagers
{
    public abstract class APIRunnerBase
    {
        protected const string GetVerb = "GET";
        protected const string PostVerb = "POST";
        protected const string contentType = "application/json";
        protected const string APIAccessKey = "APIAccessKey";
        protected const string X_Customer_ID = "X-Customer-ID";
        private const string http = "http://";
        private const string https = "https://";
        private const string ApiDomainKey = "ApiDomain";
        private const string ApiTimeoutKey = "ApiTimeout";
        private const string EmailAddressField = "EmailAddress";
        protected const string EndPointMacros = "%domain%/api/%item%";
        protected const string FoldersJson = "{\"name\":\"Type\",\"comparator\":\"=\",\"valueSet\":\"GRP\"}";
        protected const string GroupsJson = "\"name\":\"FolderID\",\"comparator\":\"=\",\"valueSet\":{0}";
        protected const string GFieldsJson = "\"name\":\"GroupID\",\"comparator\":\"=\",\"valueSet\":{0}";
        private const string JsonMacros = "%json%";
        private const string SearchCriteriaMacros = "{\"SearchCriteria\":[%json%]}";

        protected static NameValueCollection customers = new NameValueCollection();
        protected static JavaScriptSerializer serializer = new JavaScriptSerializer();
        private static readonly string Domain;
        private static readonly int Timeout;

        private static MXValidate mx = new MXValidate();
        private static readonly MXValidateLevel Level;
        private const string AspNetMxLevelKey = "aspNetMXLevel";
        private static readonly bool BlockSubmitIfTimeout;
        private const string BlockSubmitIfTimeoutKey = "BlockSubmitIfTimeout";
        private const string EmailIsIncorrectError = "We have tried to make sure your address is reachable and unfortunately discovered email address's mail server is unreachable.";

        static APIRunnerBase()
        {
            Domain = WebConfigurationManager.AppSettings[ApiDomainKey];
            if (!Domain.StartsWith(http) && !Domain.StartsWith(https))
            {
                Domain = http + Domain;
            }
            Timeout = 30000;
            try
            {
                Timeout = int.Parse(WebConfigurationManager.AppSettings[ApiTimeoutKey]);
            }
            catch
            { }
            if (Timeout < 1000)
            {
                Timeout *= 1000;
            }

            //for ASPNETMX
            Level = MXValidateLevel.Syntax;
            int _level = (int)Level;
            try
            {
                _level = int.Parse(WebConfigurationManager.AppSettings[AspNetMxLevelKey]);
            }
            catch
            { }
            if (_level > (int)Level && Enum.IsDefined(typeof(MXValidateLevel), _level))
            {
                Level = (MXValidateLevel)_level;
            }
            string data = WebConfigurationManager.AppSettings[BlockSubmitIfTimeoutKey] ?? string.Empty;
            BlockSubmitIfTimeout = data.ToLower() != "false" && data != "0";
        }

        #region KM API
        protected string GetCURLWithItem(string item)
        {
            return EndPointMacros.Replace("%domain%", Domain).Replace("%item%", item);
        }

        protected int SendCommand(string uri, NameValueCollection data, out string responseData)
        {
            return SendCommand(uri, data, null, null, out responseData);
        }

        protected int SendCommand(string uri, NameValueCollection data, string json, out string responseData)
        {
            return SendCommand(uri, data, null, json, out responseData);
        }

        protected int SendCommand(string uri, NameValueCollection data, string json, out string responseData, out string error)
        {
            return SendCommand(uri, data, null, json, out responseData, out error);
        }

        protected int SendCommand(string uri, NameValueCollection data, string method, string json, out string responseData)
        {
            string error = null;

            return SendCommand(uri, data, method, json, out responseData, out error);
        }

        protected int SendCommand(string uri, NameValueCollection data, string method, string json, out string responseData, out string error)
        {
            responseData = error = null;
            int statusCode = -1;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                //req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                req.Timeout = Timeout;
                req.Method = json == null ? GetVerb : PostVerb;
                if (method != null)
                {
                    req.Method = method.ToUpper();
                }
                req.ContentType = contentType;
                req.Headers.Add(data);
                if (json != null)
                {
                    using (StreamWriter writer = new StreamWriter(req.GetRequestStream()))
                    {
                        writer.Write(json);
                        writer.Close();
                    }
                }

                try
                {
                    using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                    {
                        statusCode = ReadResponse(resp, out responseData);
                    }
                }
                catch (WebException wex)
                {
                    if (wex.Response == null)
                    {
                        error = wex.Message;
                    }
                    else
                    {
                        using (HttpWebResponse errorResp = (HttpWebResponse)wex.Response)
                        {
                            statusCode = ReadResponse(errorResp, out error);
                            var message = serializer.Deserialize<Error>(error).Message;
                            error = GetError(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                req.Abort();
            }
            catch(Exception ex)
            {
                StringBuilder Note = new StringBuilder();
                Note.AppendLine(uri);
                Note.AppendLine(method);
                Note.AppendLine(json);
                for (int i = 0; i < data.Count; i++ )
                {
                    Note.AppendLine(data[i]);
                }

                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "APIRunnerBase.SendCommand", Convert.ToInt32(WebConfigurationManager.AppSettings["KMCommon_Application"].ToString()),Note.ToString());
            }

            return statusCode;
        }

        private int ReadResponse(HttpWebResponse resp, out string responseData)
        {
            int statusCode = (int)resp.StatusCode;
            StreamReader reader = new StreamReader(resp.GetResponseStream());
            responseData = reader.ReadToEnd();
            reader.Close();
            resp.Close();

            return statusCode;
        }

        private string GetError(string message) 
        {
            return message.Substring(message.LastIndexOf(':') + 1);
        }

        protected string GetSearchCriteriaJson(string json)
        {
            return SearchCriteriaMacros.Replace(JsonMacros, json);
        }

        protected string GetFieldNameFromControl(KMEntities.Control c, List<ECN_Framework_Entities.Communicator.GroupDataFields> fields)
        {
            string res = null;
            if (c.IsCustom())
            {
                if (c.FieldID.HasValue)
                {
                    ECN_Framework_Entities.Communicator.GroupDataFields f = fields.SingleOrDefault(x => x.GroupDataFieldsID == c.FieldID);
                    if (f != null)
                    {
                        res = f.ShortName;
                    }
                }
            }
            else
            {
                KMEnums.ControlType type = (KMEnums.ControlType)c.Type_Seq_ID;
                res = type.ToString();
                switch (type)
                {
                    case KMEnums.ControlType.Email:
                        res = EmailAddressField;
                        break;
                    case KMEnums.ControlType.Address1:
                        res = res.Substring(0, res.Length - 1);
                        break;
                }
            }

            return res;
        }

        protected string CheckEmailByNewsLetter(string accessKey, int customerID, int groupId, string email)
        {
            

            DataTable dt = ECN_Framework_BusinessLayer.Communicator.EmailGroup.
                GetBestProfileForEmailAddress(groupId, customerID, email);
            string idString = groupId.ToString();
            if(dt == null)
            {
                return "";
            }
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    if (row["GroupID"].ToString().Equals(idString, StringComparison.InvariantCultureIgnoreCase))
                    {  // include all, exclude list
                        return serializer.Serialize(ToSimpleDictionary(dt,null, new string[] { "temp_EmailID", "entryID" }).FirstOrDefault());
                    }
                }
                catch { }
            }

            string[] includeFields = new string[] {
                            "EmailID","EmailAddress","Title","FirstName","LastName","FullName","Company","Occupation",
                            "Address","Address2","City","State","Zip","Country","Voice","Mobile","Fax","Website","Age",
                            "Income","Gender","User1","User2","User3","User4","User5","User6","Birthdate",
                            "UserEvent1","UserEvent1Date","UserEvent2","UserEvent2Date","password", "Notes"
                        };
            
            return serializer.Serialize(ToSimpleDictionary(dt,includeFields).FirstOrDefault());

            
        }

        public static IEnumerable<Dictionary<string, string>> ToSimpleDictionary(DataTable dt, IEnumerable<string> fieldNames = null, IEnumerable<string> excludeFieldNames = null)
        {
            if (fieldNames == null)
            {
                fieldNames = (from DataColumn c in dt.Columns select c.ColumnName).ToArray();
            }
            if (null != excludeFieldNames)
            {
                fieldNames = from f in fieldNames where false == excludeFieldNames.Contains(f) select f;
            }
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (string column in fieldNames)
                {
                    dictionary[column] = (dataRow.Field<object>(column) ?? "").ToString();
                }
                yield return dictionary;
            }
        }
        #endregion

        #region ASPNETMX
        public static bool CheckEmail(ref string email)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(email))
            {
                res = true;
                if (Level > MXValidateLevel.Syntax)
                {
                    // Slow and Invalid check
                    string SlowAndInvalid = WebConfigurationManager.AppSettings["aspNetMX.SlowAndInvalid"];
                    string[] si = SlowAndInvalid.Split(';');
                    string[] em = email.Split('@');
                    if (si.Contains(em[1].ToLower()))
                        res = false;
                    else
                    {                        
                        // Known domains
                        mx.AddKnownDomains(WebConfigurationManager.AppSettings["aspNetMX.KnownDomains"]);
                        bool timedOut = false;
                        MXValidateLevel res_level = mx.Validate(email, Level, Timeout, out timedOut);
                        res = res_level >= Level && (!timedOut || !BlockSubmitIfTimeout);
                        // for kmpsgroup.com
                        if (email.ToLower().EndsWith("kmpsgroup.com")) res = true;
                    }
                }
                if (!res)
                {
                    email = EmailIsIncorrectError;
                }
            }

            return res;
        }
        #endregion
    }
}