using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Formatting;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

namespace EmailMarketing.API.Specs
{
    [Binding]
    public class TestHelper
    {

        public static ConcurrentDictionary<int, object> ModelObjectStore = new ConcurrentDictionary<int, object>();
        public static void StoreObject(int id, object modelObject)
        {
            ModelObjectStore[id] = modelObject;
        }
        public static object RetreiveObject(int id)
        {
            if (false == ModelObjectStore.ContainsKey(id))
            {
                throw new ArgumentException(String.Format(@"""{0}"" does not exist in the test model object store", id));
            }
            return ModelObjectStore[id];
        }

        public static int? DeletableObjectId = null;
        //public static ConcurrentBag<int> DeletableObject = new ConcurrentBag<int>();

        //public Func<Object> GenerateNewContextObject { get; set; }

        public const string APIUriRoot = "http://localhost/api";
        public string ControllerName { get; set; }

        public EmailGroup.ISubscriberTestDataProvider TestDataProvider { get; set; }

        public HttpClient Client;
        public HttpRequestMessage Request;
        public HttpResponseMessage Response;
        public HttpMethod Method;
        public HttpError Error;
        public int RecordID;
        public string APIAccessKey;
        public string CustomerID;
        public string ActionName = String.Empty; // needed when we use attribute routing

        public string RandomString = String.Empty;
        public string GenerateRandomString(string inputString, int max)
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

        public object ContextObject { 
            get; 
            set; 
        }
        public T GetContextObject<T>() where T:class
        {
            Type contextObjectType = ContextObject.GetType();
            Assert.AreSame(typeof(T), contextObjectType, "** ERROR WITHIN TEST ** Context object not of expected type"); // test the test

            return ContextObject as T;
        }

        //protected TestHelper(string controllerName, Func<Object> generateNewContextObject)
        public TestHelper()
        {
            //ControllerName = controllerName;
            //GenerateNewContextObject = generateNewContextObject;
            ControllerName = FeatureContext.Current.FeatureInfo.Title.ToLower();

            Request = new HttpRequestMessage();
            var config = new HttpConfiguration();
            EmailMarketing.API.WebApiConfig.Register(config);
            var server = new HttpServer(config);
            Client = new HttpClient(server);
        }

        public Uri CreateRequestUri(int? id = null)
        {
            if(id.HasValue)
            {
                if(false == String.IsNullOrEmpty(ActionName))
                {
                    return CreateRequestUri("{0}/{1}/{2}/{3}", APIUriRoot, ControllerName, ActionName, id);
                }
                else
                {
                    return CreateRequestUri("{0}/{1}/{2}", APIUriRoot, ControllerName, id);
                }
            }
            else
            {
                if(false == String.IsNullOrEmpty(ActionName))
                {
                    return CreateRequestUri("{0}/{1}/{2}", APIUriRoot, ControllerName, ActionName);
                }
                else
                {
                    return CreateRequestUri("{0}/{1}", APIUriRoot, ControllerName);
                }
            }
            //return id.HasValue
            //    ? CreateRequestUri("{0}/{1}/{2}", APIUriRoot, ControllerName, id)
            //    : CreateRequestUri("{0}/{1}", APIUriRoot, ControllerName);
        }
        public Uri CreateRequestUri(string template, params object[] args)
        {
            return new Uri(String.Format(template, args));
        }

        public void SetAccessKey(string apiAccessKey)
        {
            APIAccessKey = apiAccessKey;
            if (null != TestDataProvider)
            {
                TestDataProvider.ApiAccessKey = APIAccessKey;
            }
            Request.Headers.Add(EmailMarketing.API.Strings.Headers.APIAccessKeyHeader, APIAccessKey);
        }

        public void SetCustomerID(string customerID)
        {
            CustomerID = customerID;
            if(null != TestDataProvider)
            {
                int parsedCustomerID;
                if(Int32.TryParse(customerID, out parsedCustomerID))
                {
                    TestDataProvider.CustomerID = parsedCustomerID;
                }                
            }
            Request.Headers.Add(EmailMarketing.API.Strings.Headers.CustomerIdHeader, customerID);
        }

        public void SetRecordId(int recordID)
        {
            RecordID = recordID;
        }

        public void InvokeMethod(HttpMethod method)
        {
            Request.Method = method;
            Request.RequestUri = RecordID > 0 ? CreateRequestUri(RecordID) : CreateRequestUri();
            Response = Client.SendAsync(Request).Result;
        }

        public void InvokeMethodWithJsonContent(HttpMethod method)
        {
            Request.Content = new ObjectContent(
                ContextObject.GetType(),
                ContextObject,
                new JsonMediaTypeFormatter(),
                JsonMediaTypeFormatter.DefaultMediaType);
            InvokeMethod(method);
        }

        public void ResponseContentShouldNotBeNull<APIModel>()
        {
            ContextObject = Response.Content.ReadAsAsync<APIModel>().Result;
            ContextObject.ShouldNotBeNull();
        }

        public void ResponseContentShouldBeType<APIModel>()
        {
            ResponseContentShouldNotBeNull<APIModel>();
            ContextObject.ShouldBeType<APIModel>();
        }

        public void ResponseContentShouldImplementInterface<APIModel>()
        {
            ResponseContentShouldNotBeNull<APIModel>();
            ContextObject.ShouldImplement<APIModel>();
        }
    }
}
