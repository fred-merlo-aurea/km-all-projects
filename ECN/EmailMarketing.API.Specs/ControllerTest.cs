using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Formatting;

using Status = System.Net.HttpStatusCode;

//using Xbehave;
//using Xunit;
//using Should;

namespace EmailMarketing.API.Specs
{
    abstract public class ControllerTest<T> where T : new()
    {

        public static ConcurrentDictionary<int, T> ModelObjectStore = new ConcurrentDictionary<int,T>();
        public static void StoreObject(int id, T modelObject)
        {
            ModelObjectStore[id] = modelObject;
        }
        public static T RetreiveObject(int id)
        {
            if(false == ModelObjectStore.ContainsKey(id))
            {
                throw new ArgumentException(String.Format(@"""{0}"" does not exist in the test model object store","id"));
            }
            return ModelObjectStore[id];
        }

        public static int? DeletableObjectId = null;
        //public static ConcurrentBag<int> DeletableObject = new ConcurrentBag<int>();

        abstract public T GenerateNewContextObject();

        public const string APIUriRoot = "http://localhost:3440/api";
        public readonly string ControllerName;

        /*public const int IdOfExistingRecord = 293229;
        public const string ValidApiKey = "8CAB09B9-BEC9-453F-A689-E85D5C9E4898";*/

        public HttpClient Client;
        public HttpRequestMessage Request;
        public HttpResponseMessage Response;
        public HttpMethod Method;
        public HttpError Error;
        public int RecordID;
        public string APIAccessKey;

        public string RandomString = String.Empty;
        protected string GenerateRandomString(string inputString, int max)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            
            string returnString = inputString + "_" + new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            if (returnString.Length > max)
            {
                returnString = returnString.Substring(0, max);
            }
            return returnString;
        }

        public T ContextObject;

        protected ControllerTest(string controllerName)
        {
            ControllerName = controllerName;
            Request = new HttpRequestMessage();
            var config = new HttpConfiguration();
            EmailMarketing.API.WebApiConfig.Register(config);
            var server = new HttpServer(config);
            Client = new HttpClient(server);
        }

        public Uri CreateRequestUri(int? id = null)
        {
            return id.HasValue
                ? CreateRequestUri("{0}/{1}/{2}", APIUriRoot ,ControllerName, id)
                : CreateRequestUri("{0}/{1}", APIUriRoot,ControllerName);
        }
        public Uri CreateRequestUri(string template, params object[] args)
        {
            return new Uri(String.Format(template, args));
        }

        public void SetAccessKey(string apiAccessKey)
        {
            APIAccessKey = apiAccessKey;
            Request.Headers.Add(EmailMarketing.API.Strings.Headers.APIAccessKeyHeader, APIAccessKey);
        }

        public void SetRecordId(int recordID)
        {
            RecordID = recordID;
        }

        public void InvokeMethod(HttpMethod method)
        {
            Request.Method = method;
            Request.RequestUri = RecordID  > 0 ? CreateRequestUri(RecordID) : CreateRequestUri();
            Response = Client.SendAsync(Request).Result;
        }

        public void InvokeMethodWithJsonContent(HttpMethod method)
        {
            Request.Content = new ObjectContent<T>(
                ContextObject,
                new JsonMediaTypeFormatter(),
                JsonMediaTypeFormatter.DefaultMediaType);
            InvokeMethod(method);
        }
    }
}
