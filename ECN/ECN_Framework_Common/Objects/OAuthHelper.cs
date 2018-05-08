using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace ECN_Framework_Common.Objects
{
    public class OAuthHelper
    {
        public OAuthHelper() { }

        static string TW_oauth_consumer_key = ConfigurationManager.AppSettings["TW_CONSUMER_KEY"].ToString();
        static string TW_oauth_consumer_secret = ConfigurationManager.AppSettings["TW_CONSUMER_SECRET"].ToString();

        #region (Changable) Do Not Change It
        static string REQUEST_TOKEN = "https://api.twitter.com/oauth/request_token";
        static string AUTHORIZE = "https://api.twitter.com/oauth/authorize";
        static string ACCESS_TOKEN = "https://api.twitter.com/oauth/access_token";


        public enum httpMethod
        {
            POST, GET
        }
        public string oauth_request_token { get; set; }
        public string oauth_access_token { get; set; }
        public string oauth_access_token_secret { get; set; }
        public string user_id { get; set; }
        public string screen_name { get; set; }
        public string oauth_error { get; set; }

        public string postid { get; set; }
        public string GetRequestToken(string callbackURL)
        {
            HttpWebRequest request = FetchRequestToken(httpMethod.POST, TW_oauth_consumer_key, TW_oauth_consumer_secret, callbackURL);
            string result = getResponce(request);
            Dictionary<string, string> resultData = OAuthUtility.GetQueryParameters(result);
            if (resultData.Keys.Contains("oauth_token"))
                return resultData["oauth_token"];
            else
            {
                this.oauth_error = result;
                return "";
            }
        }
        public string GetAuthorizeUrl(string requestToken)
        {
            return string.Format("{0}?oauth_token={1}", AUTHORIZE, requestToken);
        }
        public void GetUserTwAccessToken(string oauth_token, string oauth_verifier, string callbackURL)
        {
            HttpWebRequest request = FetchAccessToken(httpMethod.POST, TW_oauth_consumer_key, TW_oauth_consumer_secret, oauth_token, oauth_verifier, callbackURL);
            string result = getResponce(request);

            Dictionary<string, string> resultData = OAuthUtility.GetQueryParameters(result);
            if (resultData.Keys.Contains("oauth_token"))
            {
                this.oauth_access_token = resultData["oauth_token"];
                this.oauth_access_token_secret = resultData["oauth_token_secret"];
                this.user_id = resultData["user_id"];
                this.screen_name = resultData["screen_name"];
            }
            else
                this.oauth_error = result;
        }
        public void TweetOnBehalfOf(string oauth_access_token, string oauth_token_secret, string postData,string callbackURL)
        {
            postData = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(postData);
            HttpWebRequest request = PostTwits(TW_oauth_consumer_key, TW_oauth_consumer_secret, oauth_access_token, oauth_token_secret, postData, callbackURL);
            string results = getResponce(request);

            Dictionary<string, string> dcResult = ECN_Framework_Common.Objects.SocialMediaHelper.GetJSONDict(results);// OAuthUtility.GetQueryParameters(results);
            if (dcResult.ContainsKey("status") && dcResult["status"] != "200")
            {
                this.oauth_error = results;
            }
            else
            {
                this.postid = dcResult["id"].ToString();
            }

        }

        public string GetTwitterProfile(string userID, string access_Token, string access_Secret)
        {
            HttpWebRequest request = GetTWProfile(httpMethod.GET, TW_oauth_consumer_key, TW_oauth_consumer_secret, access_Token, access_Secret, userID);
            return getResponce(request);
        }

        HttpWebRequest FetchRequestToken(httpMethod method, string oauth_consumer_key, string oauth_consumer_secret,string callbackURL)
        {
            string OutUrl = "";
            string OAuthHeader = OAuthUtility.GetAuthorizationHeaderForPost_OR_QueryParameterForGET(new Uri(REQUEST_TOKEN), callbackURL, method.ToString(), oauth_consumer_key, oauth_consumer_secret, "", "", out OutUrl);

            if (method == httpMethod.GET)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(OutUrl + "?" + OAuthHeader);
                request.Method = method.ToString();
                return request;
            }
            else if (method == httpMethod.POST)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(OutUrl);
                request.Method = method.ToString();
                request.Headers["Authorization"] = OAuthHeader;
                return request;
            }
            else
                return null;


        }

        HttpWebRequest GetTWProfile(httpMethod method, string oauth_consumer_key, string oauth_consumer_secret, string oauth_token,string oauth_token_secret, string userID)
        {

            string getProfileURL = string.Format("https://api.twitter.com/1.1/users/lookup.json?skip_status=true&user_id={0}", userID);
            string oAuthHeader = OAuthUtility.GetAuthorizationHeaderForPost_OR_QueryParameterForGET(new Uri(getProfileURL), "", method.ToString(), oauth_consumer_key, oauth_consumer_secret, oauth_token, oauth_token_secret, out getProfileURL);
            getProfileURL = string.Format("https://api.twitter.com/1.1/users/lookup.json?skip_status=true&user_id={0}", userID);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getProfileURL);
            request.Headers.Add("Authorization", oAuthHeader);
            request.Method = method.ToString();
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            return request;
        }
        HttpWebRequest FetchAccessToken(httpMethod method, string oauth_consumer_key, string oauth_consumer_secret, string oauth_token, string oauth_verifier, string callbackURL)
        {
            string postData = "oauth_verifier=" + oauth_verifier;
            string AccessTokenURL = string.Format("{0}?{1}", ACCESS_TOKEN, postData);
            string OAuthHeader = OAuthUtility.GetAuthorizationHeaderForPost_OR_QueryParameterForGET(new Uri(AccessTokenURL), callbackURL, method.ToString(), oauth_consumer_key, oauth_consumer_secret, oauth_token, "", out AccessTokenURL);

            if (method == httpMethod.GET)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AccessTokenURL + "?" + OAuthHeader);
                request.Method = method.ToString();
                return request;
            }
            else if (method == httpMethod.POST)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AccessTokenURL);
                request.Method = method.ToString();
                request.Headers["Authorization"] = OAuthHeader;

                byte[] array = Encoding.ASCII.GetBytes(postData);
                request.GetRequestStream().Write(array, 0, array.Length);
                return request;
            }
            else
                return null;

        }
        HttpWebRequest PostTwits(string oauth_consumer_key, string oauth_consumer_secret, string oauth_access_token, string oauth_token_secret, string postData, string callbackURL)
        {
            
            string updateStatusURL = "https://api.twitter.com/1.1/statuses/update.json";
            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";
            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_access_token,
                                        oauth_version,
                                        Uri.EscapeDataString(postData)
                                        );

            baseString = string.Concat("POST&", Uri.EscapeDataString(updateStatusURL), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_access_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );


            // make the request
            var postBody = "status=" + Uri.EscapeDataString(postData);

            ServicePointManager.Expect100Continue = false;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateStatusURL);
            request.Method = httpMethod.POST.ToString();
            request.Headers.Add("Authorization", authHeader);
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }
            //byte[] array = Encoding.ASCII.GetBytes(postData);
            //request.GetRequestStream().Write(array, 0, array.Length);
            return request;

        }

        public static string getResponce(HttpWebRequest request)
        {
            try
            {
                HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                string result = reader.ReadToEnd();
                reader.Close();
                return result + "&status=200";
            }
            catch (Exception ex)
            {
                string statusCode = "";
                return string.Format("status:{0},error:{1}", statusCode, ex.Message);
            }
        }
        #endregion
    }
}