using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApiRestService;
using System.Net;

namespace EmailPreview
{
    class LitmusApi
    {
        private readonly RestClient restClient;
        private Accounts account;
        private Uri baseURL;

        public LitmusApi()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls; // comparable to modern browsers
            account = new Accounts("previews-api", ConfigurationManager.AppSettings["LitmusAPIKey"], ConfigurationManager.AppSettings["LitmusAPIPassword"]);
            baseURL = new Uri(account.LitmusBaseUrl);
            this.restClient = new RestClient(account.LitmusBaseUrl);
            this.restClient.Authenticator = new HttpBasicAuthenticator(account.Username, account.Password);
        }

        private HttpClient PrepareClient(HttpClient client)
        {

            var byteArray = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["LitmusAPIKey"] + ":" + ConfigurationManager.AppSettings["LitmusAPIPassword"]);
            client.BaseAddress = baseURL;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public List<string> GetSpamSeedAddresses()
        {
            var request = new RestRequest("api/v1/SpamSeedAddresses", Method.GET);

            return ExecuteGet<List<string>>(request);
        }

        public List<TestingApplication> GetTestingApplication()
        {
            var request = new RestRequest("api/v1/EmailTests/TestingApplications", Method.GET);

            return ExecuteGet<List<TestingApplication>>(request);
        }

        public List<TestingApplication> GetExistingTestResults(int EmailTestID)
        {
            var request = new RestRequest(string.Format("api/v1/EmailTests/{0}", EmailTestID), Method.GET);
            EmailTest et = ExecuteGet<EmailTest>(request);
            return et.TestingApplications;
        }

        public EmailTest CreateEmailTests(EmailTest EmailTest)
        {
            EmailTest.TestType = "0";
            var restRequest = new RestRequest("api/v1/EmailTests", Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(EmailTest);

            var response = restClient.Execute<EmailTest>(restRequest);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<EmailTest>(response.Content);
            //return response.Data;
            //return ExecuteUpdatedPost<EmailTest>("api/v1/EmailTests", EmailTest);
        }

        private string BuildNote(HttpResponseMessage message, Task<EmailTest> emailTest)
        {
            StringBuilder sbNote = new StringBuilder();
            try
            {

                sbNote.AppendLine("ResponseMessage Headers:" + message.Headers.ToString());
                sbNote.AppendLine("ResponseMessage Content:" + message.Content.ToString());
                sbNote.AppendLine("ResponseMessage StatusCode:" + message.StatusCode.ToString());
                sbNote.AppendLine("--------------------");
            }
            catch { }
            try
            {
                EmailTest et = emailTest.Result;
                sbNote.AppendLine("EmailTest Error: " + et.ErrorMessage.ToString());
                sbNote.AppendLine("EmailTest InboxGuid:" + et.InboxGuid.ToString());
                sbNote.AppendLine("EmailTest UserGuid: " + et.UserGuid.ToString());
            }
            catch { }

            return sbNote.ToString();
        }

        public CodeAnalysisTest GetCodeAnalysisTest(string html)
        {
            CodeAnalysisTest cat = ExecuteUpdatedPostString<CodeAnalysisTest>("api/v1/CodeAnalysis", html);
            return cat;
        }

        public LinkTest CreateLinkTest(string html)
        {
            LinkTest ltPost = new LinkTest();
            //ltPost.HTML = html;
            var restRequest = new RestRequest("api/v1/LinkTests", Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(html);

            var response = restClient.Execute<LinkTest>(restRequest);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<LinkTest>(response.Content);
            //LinkTest lt = ExecuteUpdatedPostString<LinkTest>("api/v1/LinkTests", html);
            //return lt;
        }

        public LinkTest GetLinkTestResults(int LinkTestID)
        {
            var request = new RestRequest(string.Format("api/v1/LinkTests/{0}", LinkTestID), Method.GET);
            
            LinkTest lt = ExecuteGet<LinkTest>(request);
            return lt;
        }

        private T ExecuteUpdatedPost<T>(string UriToPostTo, T objectToPost) where T : new()
        {
            HttpResponseMessage result = new HttpResponseMessage();
            Task<T> resultTask = null;
            using (System.Net.Http.HttpClient client = PrepareClient(new HttpClient()))
            {
                //var response = Task.Run(() => client.PostAsJsonAsync<T>(UriToPostTo, objectToPost)).Result;
                Task<HttpResponseMessage> response = client.PostAsJsonAsync<T>(UriToPostTo, objectToPost);
                response.Wait();
                result = response.Result;

                result.EnsureSuccessStatusCode();

                resultTask = result.Content.ReadAsAsync<T>();
                resultTask.Wait();
                return resultTask.Result;
                //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result.Content.ToString());
            }

        }

        private T ExecuteUpdatedPostString<T>(string UriToPostTo, string objectToPost)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            Task<T> resultTask = null;
            using (System.Net.Http.HttpClient client = PrepareClient(new HttpClient()))
            {
                //var response = Task.Run(() => client.PostAsJsonAsync<string>(UriToPostTo, objectToPost)).Result;
                Task<HttpResponseMessage> response = client.PostAsJsonAsync<string>(UriToPostTo, objectToPost);
                response.Wait();
                result = response.Result;

                result.EnsureSuccessStatusCode();

                resultTask = result.Content.ReadAsAsync<T>();
                resultTask.Wait();
                return resultTask.Result;

                //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result.Content.ToString());
            }

        }

        private T ExecuteGet<T>(RestRequest request) where T : new()
        {
            var response = restClient.Execute<T>(request);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
        }

        private T ExecutePost<T>(RestRequest request) where T : new()
        {
            var response = restClient.Post<T>(request);
            return response.Data;
        }

        
    }
}
