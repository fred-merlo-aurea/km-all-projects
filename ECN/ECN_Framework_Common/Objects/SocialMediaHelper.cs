using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Web;
using System.Runtime.Serialization;
using System.Data;
using ServiceStack.Text;
using Newtonsoft.Json;

namespace ECN_Framework_Common.Objects
{
    public partial class SocialMediaHelper
    {
        private static string CreateNote(string url, string accessToken, string payloadXML, HttpWebRequest request, HttpWebResponse response)
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            try
            {
                adminEmailVariables.AppendLine("<BR><BR>URL: " + url);
                adminEmailVariables.AppendLine("<BR>AccessToken: " + accessToken);
                adminEmailVariables.AppendLine("<BR>PayloadXML: " + payloadXML);
                if (request != null)
                {
                    adminEmailVariables.AppendLine("<BR>Request Method: " + request.Method);
                    adminEmailVariables.AppendLine("<BR>Request ContentType: " + request.ContentType);
                    adminEmailVariables.AppendLine("<BR>Request ContentLength: " + request.ContentLength);
                    if (request.Headers.Count > 0)
                    {
                        adminEmailVariables.AppendLine("<BR>Request Headers: ");
                        var headers = String.Empty;
                        foreach (var key in request.Headers.AllKeys)
                            headers += "<BR>" + key + ":" + request.Headers[key];
                        adminEmailVariables.AppendLine(headers);
                    }
                }

                if (response != null)
                {
                    adminEmailVariables.AppendLine("<BR>Response StatusCode: " + response.StatusCode);
                    adminEmailVariables.AppendLine("<BR>Response StatusDescription: " + response.StatusDescription);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        if (reader != null)
                        {
                            string vals = reader.ReadToEnd();
                            adminEmailVariables.AppendLine("<BR>Response Stream: " + vals);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        #region Facebook calls/checks
        public static Dictionary<string, string> GetFBAccessToken(string code, string AbsolutURI)
        {
            string FBapp_ID = ConfigurationManager.AppSettings["FBAPPID"].ToString();
            string FBapp_Secret = ConfigurationManager.AppSettings["FBAPPSECRET"].ToString();
            string scope = ConfigurationManager.AppSettings["FBSCOPE"].ToString();

            Dictionary<string, string> tokens = new Dictionary<string, string>();
            string url = string.Format("https://graph.facebook.com/v2.9/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}",
                           FBapp_ID, HttpUtility.UrlEncode(AbsolutURI), scope, code, FBapp_Secret);

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    
                    string vals = reader.ReadToEnd();
                    FBAccessTokenResponse result = JsonConvert.DeserializeObject<FBAccessTokenResponse>(vals);
                    tokens.Add("access_token", result.access_token);
                    tokens.Add("token_type", result.token_type);
                    tokens.Add("expires_in", result.expires_in);
                    //tokens = GetURLParamDict(vals);
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "GetFBAccessToken", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }
            return tokens;
        }



        public static Dictionary<string, string> GetFBLongLivedToken(string access_token)
        {
            string FBapp_ID = ConfigurationManager.AppSettings["FBAPPID"].ToString();
            string FBapp_Secret = ConfigurationManager.AppSettings["FBAPPSECRET"].ToString();
            string scope = ConfigurationManager.AppSettings["FBSCOPE"].ToString();

            string longLivedTokenURL = string.Format("https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id={0}&client_secret={1}&fb_exchange_token={2}", FBapp_ID, FBapp_Secret, access_token);
            Dictionary<string, string> longlivedTokens = new Dictionary<string, string>();
            HttpWebRequest longLivedRequest = WebRequest.Create(longLivedTokenURL) as HttpWebRequest;

            using (HttpWebResponse response = longLivedRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string vals = reader.ReadToEnd();

                FBLongLivedTokenResponse longlived = JsonConvert.DeserializeObject<FBLongLivedTokenResponse>(vals);

                longlivedTokens.Add("access_token", longlived.access_token);
                longlivedTokens.Add("expires_in", longlived.expires_in);
                longlivedTokens.Add("machine_id", longlived.machine_id);

            }
            return longlivedTokens;
        }
        public static Dictionary<string, string> GetFBUserProfile(string access_token)
        {

            //graph.facebook.com/debug_token?input_token={token-to-inspect}&access_token={app-token-or-admin-token}
            string appToken = GetAppTokenFB();
            string checkURL = string.Format("https://graph.facebook.com/v2.2/me?access_token={0}&fields=first_name,last_name,picture,link", access_token);//debug_token?input_token={0}&access_token={1}", access_token, appToken);
            HttpWebRequest existingCheckRequest = WebRequest.Create(checkURL) as HttpWebRequest;
            Dictionary<string, string> checkDict = new Dictionary<string, string>();

            using (HttpWebResponse response = existingCheckRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string vals = reader.ReadToEnd();
                checkDict = GetJSONDict(vals);
            }
            return checkDict;
        }
        public static string GetAppTokenFB()
        {
            string FBapp_ID = ConfigurationManager.AppSettings["FBAPPID"].ToString();
            string FBapp_Secret = ConfigurationManager.AppSettings["FBAPPSECRET"].ToString();
            string scope = ConfigurationManager.AppSettings["FBSCOPE"].ToString();

            string retAPPToken = string.Empty;
            string appTokenURL = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials", FBapp_ID, FBapp_Secret);
            HttpWebRequest request = WebRequest.Create(appTokenURL) as HttpWebRequest;
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                
                string vals = reader.ReadToEnd();
                dynamic access = JsonConvert.DeserializeObject(vals);
                retAPPToken = access.access_token;
            }
            return retAPPToken;
        }

        public static List<FBAccount> GetUserAccounts(string access_token)
        {
            string endpoint = "https://graph.facebook.com/v2.2/me/accounts?access_token={0}&fields=name,id,link,access_token,perms,picture";
            endpoint = string.Format(endpoint, access_token);
            HttpWebRequest request = WebRequest.Create(endpoint) as HttpWebRequest;
            request.Method = "GET";
            List<FBAccount> accounts = new List<FBAccount>();
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string json = string.Empty;
                    while (reader.EndOfStream == false)
                    {
                        json = reader.ReadLine();

                    }

                    XmlDocument xDoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json, "data");
                    accounts = GetAccountsFromXML(xDoc);
                }
            }
            catch (Exception ex)
            {
                FBAccount failAccount = new FBAccount { id = "-1", name = ex.Message };
                accounts.Add(failAccount);
                //KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SocialMediaHelper.GerUserAccounts", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "access_Token:" + access_token);
            }
            return accounts;
        }

        public static Dictionary<string, int> GetFBPostData(string postID, string access_token)
        {
            Dictionary<string,int> dictResults = new Dictionary<string,int>();
            //Get general stats
            string fbPostURL = "https://graph.facebook.com/v2.2/{0}?access_token={1}";
            fbPostURL = string.Format(fbPostURL, postID, access_token);

            HttpWebRequest request = WebRequest.Create(fbPostURL) as HttpWebRequest;
            request.Method = "GET";
            Dictionary<string, string> dictGeneral = new Dictionary<string, string>();
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = string.Empty;
                while (reader.EndOfStream == false)
                {
                    json = reader.ReadLine();

                }
                
                dictGeneral = ServiceStack.Text.JsonSerializer.DeserializeFromString<Dictionary<string, string>>(json);
            }

            //GetLikes
            string fbLikesURL = "https://graph.facebook.com/v2.2/{0}/likes?access_token={1}";
            fbLikesURL = string.Format(fbLikesURL, postID, access_token);

            HttpWebRequest requestLikes = WebRequest.Create(fbLikesURL) as HttpWebRequest;
            request.Method = "GET";
            Dictionary<string, string> dictLikes= new Dictionary<string, string>();
            using (HttpWebResponse response = requestLikes.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = string.Empty;
                while (reader.EndOfStream == false)
                {
                    json = reader.ReadLine();

                }

                dictLikes = ServiceStack.Text.JsonSerializer.DeserializeFromString<Dictionary<string, string>>(json);
            }

            //Getshares
            string fbSharesURL = "https://graph.facebook.com/v2.2/{0}?fields=shares&access_token={1}";
            fbSharesURL = string.Format(fbSharesURL, postID, access_token);

            HttpWebRequest requestShares = WebRequest.Create(fbSharesURL) as HttpWebRequest;
            request.Method = "GET";
            Dictionary<string, string> dictShares = new Dictionary<string, string>();
            using (HttpWebResponse response = requestShares.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = string.Empty;
                while (reader.EndOfStream == false)
                {
                    json = reader.ReadLine();

                }

                XmlDocument xDoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json, "shares");
                if (xDoc.DocumentElement.HasChildNodes)
                {
                    XmlNode sharesCount = xDoc.DocumentElement.SelectSingleNode("shares/count");
                    if (sharesCount != null)
                    {
                        dictShares.Add("shares", sharesCount.InnerText);
                    }
                    else
                        dictShares.Add("shares", "0");

                }
                else
                {
                    dictShares.Add("shares", "0");
                }
            }

            //Getcomments
            string fbCommentsURL = "https://graph.facebook.com/v2.2/{0}/comments?access_token={1}";
            fbCommentsURL = string.Format(fbCommentsURL, postID, access_token);

            HttpWebRequest requestComments = WebRequest.Create(fbCommentsURL) as HttpWebRequest;
            request.Method = "GET";
            Dictionary<string, string> dictComments = new Dictionary<string, string>();
            List<string> ids = new List<string>();
            int commentCounter = 0;
            using (HttpWebResponse response = requestComments.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = string.Empty;
                while (reader.EndOfStream == false)
                {
                    json = reader.ReadLine();

                }
                XmlDocument xDoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json, "data");
                if (xDoc.DocumentElement.HasChildNodes)
                {
                    XmlNodeList comments = xDoc.DocumentElement.SelectNodes("data");
                    
                    foreach(XmlNode node in comments)
                    {
                        XmlNode personNode = node.SelectSingleNode("from/id");
                        if(!ids.Contains(personNode.InnerText))
                        {
                            ids.Add(personNode.InnerText);
                        }
                        commentCounter++;
                    }
                    

                }
            }

            //GetImpressions for post
            string fbImpressionsURL = "https://graph.facebook.com/v2.2/{0}/insights?access_token={1}";
            fbImpressionsURL = string.Format(fbImpressionsURL, postID, access_token);
            HttpWebRequest requestImpressions = WebRequest.Create(fbImpressionsURL) as HttpWebRequest;
            requestImpressions.Method = "GET";
            Dictionary<string, int> dictImpressions = new Dictionary<string, int>();
            using (HttpWebResponse response = requestImpressions.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = string.Empty;
                while (reader.EndOfStream == false)
                {
                    json = reader.ReadLine();

                }
                XmlDocument xDoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json.Replace("other clicks","otherclicks").Replace("link clicks","linkclicks"), "data");
                //dictImpressions = ServiceStack.Text.JsonSerializer.DeserializeFromString<Dictionary<string, string>>(json);
                dictImpressions = GetImpressionsFromXML(xDoc);
            }

            //take individual dicts and combine them into the results dict
            List<string> likes = dictLikes["data"].ToString().Split('{').ToList();
            
            //List<string> shares = dictShares["data"].ToString().Split('{').ToList();

            dictResults.Add("Likes", likes.Count(x => x.Contains("id")));//dictLikes
            dictResults.Add("Unique_Comments", ids.Count);
            dictResults.Add("Total_Comments", commentCounter);
            dictResults.Add("Shares", Convert.ToInt32(dictShares["shares"].ToString()));//dictShares
            dictResults.Add("Unique_Clicks",Convert.ToInt32(dictImpressions["post_consumptions_unique"]));//dictImpressions
            dictResults.Add("Total_Clicks", Convert.ToInt32(dictImpressions["post_consumptions"]));//dictImpressions
            dictResults.Add("Unique_Impressions", Convert.ToInt32(dictImpressions["post_impressions_unique"]));//dictImpressions
            dictResults.Add("Total_Impressions", Convert.ToInt32(dictImpressions["post_impressions"]));//dictImpressions


            return dictResults;
        }

        private static Dictionary<string, int> GetImpressionsFromXML(XmlDocument xDoc)
        {
            Dictionary<string, int> retDict = new Dictionary<string, int>();
            if (xDoc.DocumentElement.HasChildNodes)
            {
                foreach (XmlNode dataNode in xDoc.DocumentElement.ChildNodes)
                {
                    if (dataNode.Name.ToLower().Equals("data"))
                    {

                        XmlNode fbName = dataNode.SelectSingleNode("name");
                        if (fbName != null)
                        {
                            XmlNode fbValue = null;
                            switch (fbName.InnerText)
                            {
                                case "post_impressions":
                                    fbValue = dataNode.SelectSingleNode("values/value");
                                    retDict.Add(fbName.InnerText, Convert.ToInt32(fbValue.InnerText.ToString()));
                                    break;
                                case "post_impressions_unique":
                                    fbValue = dataNode.SelectSingleNode("values/value");
                                    retDict.Add(fbName.InnerText, Convert.ToInt32(fbValue.InnerText.ToString()));
                                    break;
                                case "post_consumptions":
                                    fbValue = dataNode.SelectSingleNode("values/value");
                                    retDict.Add(fbName.InnerText, Convert.ToInt32(fbValue.InnerText.ToString()));
                                    break;
                                case "post_consumptions_unique":
                                    fbValue = dataNode.SelectSingleNode("values/value");
                                    retDict.Add(fbName.InnerText, Convert.ToInt32(fbValue.InnerText.ToString()));
                                    break;
                            }
                        }

                    }
                }
            }
            return retDict;
        }
        [DataContract(Name = "data")]
        [Serializable]
        public class FBAccount
        {
            public FBAccount()
            {
                name = string.Empty;
                id = string.Empty;
                link = string.Empty;
                access_token = string.Empty;
                perms = new List<string>();
            }
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public string id { get; set; }
            [DataMember]
            public string link { get; set; }
            [DataMember]
            public string access_token { get; set; }
            [DataMember]
            public List<string> perms { get; set; }
            [DataMember]
            public string picture { get; set; }
        }

        private static List<FBAccount> GetAccountsFromJSON(string json)
        {
            List<FBAccount> listAccounts = new List<FBAccount>();
            FBAccount fbAccount = new FBAccount();

            string clean = json.Trim();
            clean = clean.Replace("\"", "");

            string[] parse = clean.Split('[');

            foreach (string p in parse)
            {
                if (p.Contains("next:"))
                    break;
                string[] split = p.Split(',');
                foreach (string sp in split)
                {
                    string[] splitFinal = sp.Split(':');
                    if (splitFinal.Length > 2)
                    {
                        if (splitFinal[0] == "picture")
                        {

                            if (p.Contains("http:") || p.Contains("https:"))
                            {
                                fbAccount.picture = p.Substring(p.IndexOf(":") + 1).Replace("\\", "");
                            }
                            else
                            {
                                fbAccount.picture = p.Remove(0, p.IndexOf(":") + 1);

                            }

                            fbAccount = new FBAccount();
                            break;
                        }
                    }
                    else
                    {
                        if (splitFinal[0] == "name")
                        {
                            fbAccount.name = splitFinal[1].Trim().ToString();
                        }
                        else if (splitFinal[0] == "id")
                        {
                            fbAccount.id = splitFinal[1].Trim().ToString();
                        }
                        else if (splitFinal[0] == "link")
                        {
                            fbAccount.link = splitFinal[1].Trim().ToString() + ":" + splitFinal[2].Trim().ToString().Replace("\\", "");
                        }
                        else if (splitFinal[0] == "access_token")
                        {
                            fbAccount.access_token = splitFinal[1].Trim().ToString();
                        }
                        else if (splitFinal[0] == "perms")
                        {
                            string perms = p.Substring(p.IndexOf("[") + 1, p.IndexOf("]", p.IndexOf("[")) - p.IndexOf("[") - 1);
                            string[] splitPerm = perms.Split(',');
                            foreach (string st in splitPerm)
                            {
                                fbAccount.perms.Add(st);
                            }
                            if (fbAccount.perms.Contains("ADMINISTER") || fbAccount.perms.Contains("CREATE_CONTENT"))
                            {
                                listAccounts.Add(fbAccount);

                            }
                            fbAccount = new FBAccount();


                        }

                    }

                }
            }

            return listAccounts;
        }

        private static List<FBAccount> GetAccountsFromXML(XmlDocument xml)
        {
            List<FBAccount> retList = new List<FBAccount>();
            if (xml.DocumentElement.HasChildNodes)
            {

                foreach (XmlNode dataNode in xml.DocumentElement.ChildNodes)
                {
                    if (dataNode.Name.ToLower().Equals("data"))
                    {
                        FBAccount fb = new FBAccount();
                        XmlNode fbName = dataNode.SelectSingleNode("name");
                        fb.name = fbName.InnerText;
                        XmlNode fbID = dataNode.SelectSingleNode("id");
                        fb.id = fbID.InnerText;
                        XmlNode fbAccess_Token = dataNode.SelectSingleNode("access_token");
                        fb.access_token = fbAccess_Token.InnerText;
                        XmlNode fbPicture = dataNode.SelectSingleNode("picture/data/url");
                        fb.picture = fbPicture.InnerText;
                        XmlNodeList perms = dataNode.SelectNodes("perms");
                        foreach (XmlNode currentPerm in perms)
                        {
                            fb.perms.Add(currentPerm.InnerText);
                        }
                        retList.Add(fb);
                    }
                }

            }
            return retList;
        }

        #endregion

        #region LI calls/checks
        public static Dictionary<string, string> GetLIUserProfile(string access_token)
        {
            Dictionary<string, string> retDict = new Dictionary<string, string>();
            string url = string.Format("https://www.linkedin.com/v1/people/~:(id,first-name,last-name,picture-url)?oauth2_access_token={0}", access_token);
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                //this request only returns firstname, lastname, headline, site-standard-profile-request url
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string vals = reader.ReadToEnd();

                    retDict = GetXMLDict(vals);
                }

                //Making this second request to get the full profile
                //HttpWebRequest profileRequest = WebRequest.Create(retDict["site-standard-profile-request"].ToString()) as HttpWebRequest;
                //using(HttpWebResponse response = profileRequest.GetResponse() as HttpWebResponse)
                //{
                //    StreamReader reader = new StreamReader(response.GetResponseStream());
                //    string vals = reader.ReadToEnd();
                //    retDict = GetXMLDict(vals);
                //}
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "SocialMediaHelper.GetLIUserProfile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote(url, access_token, "", request, response));
            }

            return retDict;
        }

        public static List<LIAccount> GetLICompanies(string access_token)
        {
            string url = "https://api.linkedin.com/v1/companies:(id,name,square-logo-url)?is-company-admin=true";
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            List<LIAccount> retList = new List<LIAccount>();
            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "text/xml; encoding='utf-8'";
                request.Headers.Add("x-li-format", "xml");
                request.Headers.Add("Authorization", "Bearer " + access_token);
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string vals = reader.ReadToEnd();

                    retList = GetAccountsList(vals);
                }
            }
            catch (Exception ex)
            {
                LIAccount failAccount = new LIAccount {id = "-1", name = ex.Message};
                retList.Add(failAccount);
                //KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SocialMediaHelper.GetLICompanies", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote(url, access_token, "", request, response));
            }

            return retList;
        }

        public static Dictionary<string, string> PostToLI(string access_token, string title, string subTitle, string comment, bool useThumbnail, string imagePath, string pageID, int blastID, int layoutID, int groupID)
        {
            string LIpostURL = string.Empty;
            StringBuilder sbXML = new StringBuilder();
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Dictionary<string, string> results = new Dictionary<string, string>();
            bool success = false;
            try
            {
                if (!string.IsNullOrEmpty(pageID))
                {
                    LIpostURL = string.Format("https://api.linkedin.com/v1/companies/{0}/shares", pageID);
                }
                else
                {
                    LIpostURL = "https://api.linkedin.com/v1/people/~/shares";
                }
                KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                string previewLink = ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + ConfigurationManager.AppSettings["SocialPreview"].ToString();
                string queryString = "blastID=" + blastID + "&layoutID=" + layoutID + "&m=3&g=" + groupID;
                string encryptedString = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
                previewLink += encryptedString;

                //LIpostURL += "?oauth2_access_token=" + access_token;
                LIShare share = new LIShare();
                share.comment = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(comment);
                share.content.description = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(subTitle);
                share.content.title = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(title);
                share.content.submittedUrl = previewLink;
                share.visibility.code = "anyone";
                if (useThumbnail)
                    share.content.submittedImage = imagePath;
                else
                    share.content.submittedImage = null;


                #region old xml version
                //sbXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                //sbXML.Append("<share>");
                //sbXML.Append("<comment><![CDATA[" + ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(comment) + "]]></comment>");
                //sbXML.Append("<content>");
                //sbXML.Append("<title><![CDATA[" + ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(title) + "]]></title>");
                //sbXML.Append("<description><![CDATA[" + ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(subTitle) + "]]></description>");
                //previewLink = previewLink.Replace("http://", "");
                //previewLink = previewLink.Replace("https://", "");

                //sbXML.Append("<submitted-url>" + previewLink + "</submitted-url>");

                //if (useThumbnail)
                //    sbXML.Append("<submitted-image-url><![CDATA[" + imagePath + "]]></submitted-image-url>");
                //sbXML.Append("</content>");
                //sbXML.Append("<visibility>");
                //sbXML.Append("<code>anyone</code>");
                //sbXML.Append("</visibility>");
                //sbXML.Append("</share>");
                #endregion

                request = WebRequest.Create(LIpostURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("x-li-format", "json");
                request.Headers.Add("Authorization", "Bearer " + access_token);

                string jsonShare = Newtonsoft.Json.JsonConvert.SerializeObject(share);

                byte[] bytes = Encoding.UTF8.GetBytes(jsonShare);
                request.ContentLength = bytes.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);

                try
                {
                    using (response = (HttpWebResponse)request.GetResponse())
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        if (response.StatusCode == HttpStatusCode.Created)
                        {

                            try
                            {
                                results = GetJsonDictLI(reader.ReadToEnd());
                            }
                            catch { }
                            results.Add("success", "true");
                        }
                    }
                }
                catch (Exception ex)
                {
                    results.Add("success", "false");
                    results.Add("LinkedInErrorMsg",ex.Message);
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SocialMediaHelper.PostToLI", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote(LIpostURL, access_token, sbXML.ToString(), request, response));
            }

            return results;
        }

        private static List<LIAccount> GetAccountsList(string dirtyxml)
        {
            List<LIAccount> accounts = new List<LIAccount>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(dirtyxml);
            LIAccount acc = new LIAccount();
            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                if (node.Name.ToLower().Equals("company"))
                {
                    foreach (XmlNode id in node.ChildNodes)
                    {
                        if (id.Name.ToLower().Equals("id"))
                            acc.id = id.InnerText;
                        else if (id.Name.ToLower().Equals("name"))
                            acc.name = id.InnerText;
                        else if (id.Name.ToLower().Equals("square-logo-url"))
                            acc.picture = id.InnerText;
                    }
                    accounts.Add(acc);

                }

            }
            return accounts;
        }

        [Serializable]
        public class LIAccount
        {
            public LIAccount() { }

            public string id { get; set; }
            public string name { get; set; }

            public string picture { get; set; }


        }

        [Serializable]
        public class LIShare
        {
            public LIShare(){
                visibility = new visibility();
                content = new content();
            }

            public visibility visibility { get; set; }

            public string comment { get; set; }

            public content content { get; set; }

        }

        [Serializable]
        public class visibility
        {
            public visibility(){}

            public string code { get; set; }
        }

        [Serializable]
        public class content
        {
            public content(){}

            public string title { get; set; }

            public string description { get; set; }

            [JsonProperty(PropertyName = "submitted-url")]
            public string submittedUrl { get; set; }

            [JsonProperty(PropertyName = "submitted-image-url")]
            public string submittedImage { get; set; }

        }

        #endregion

        #region TW calls/checks

        #endregion

        #region clean results
        public static Dictionary<string, string> GetJSONDict(string jsonDirty)
        {
            Dictionary<string, string> retDict = new Dictionary<string, string>();
            jsonDirty = jsonDirty.Replace("data\":", "");
            foreach (string json in jsonDirty.Split(','))
            {
                string cleanJSON = CleanJSONString(json);
                string[] jsonValues = cleanJSON.Split(':');
                try
                {
                    if (jsonValues.Count() > 2)
                    {
                        if (cleanJSON.Contains("http:") || cleanJSON.Contains("https:"))
                        {
                            retDict.Add(cleanJSON.Substring(0, cleanJSON.IndexOf(":")), cleanJSON.Substring(cleanJSON.IndexOf(":") + 1).Replace("\\", ""));
                        }
                        else
                        {
                            cleanJSON = cleanJSON.Remove(0, cleanJSON.IndexOf(":") + 1);
                            jsonValues = cleanJSON.Split(':');
                            retDict.Add(jsonValues[0].ToString(), jsonValues[1].ToString());
                        }
                    }
                    else
                    {
                        retDict.Add(jsonValues[0].ToString().Replace("\"", ""), jsonValues[1].ToString().Replace("\"", ""));
                    }
                }
                catch { }
            }
            return retDict;
        }

        public static Dictionary<string, string> GetURLParamDict(string url)
        {
            Dictionary<string, string> retDict = new Dictionary<string, string>();
            foreach (string param in url.Split('&'))
            {
                retDict.Add(param.Substring(0, param.IndexOf("=")), param.Substring(param.IndexOf("=") + 1, param.Length - param.IndexOf("=") - 1));
            }
            return retDict;
        }

        public static Dictionary<string, string> GetXMLDict(string dirtyxml)
        {
            Dictionary<string, string> retDict = new Dictionary<string, string>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(dirtyxml);

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {

                retDict.Add(node.Name.ToString(), node.InnerText);

            }

            return retDict;
        }

        public static Dictionary<string,string> GetJsonDictLI(string dirtyJson)
        {
            Dictionary<string, string> retDict = new Dictionary<string, string>();

            LIResponse response = new LIResponse();

            response = Newtonsoft.Json.JsonConvert.DeserializeObject<LIResponse>(dirtyJson);

            retDict.Add("updateKey", response.updateKey);
            retDict.Add("updateUrl", response.updateUrl);

            return retDict;
        }

        public class LIResponse
        {
            public LIResponse() { }

            public string updateKey { get; set; }
            public string updateUrl { get; set; }
        }

        public class FBAccessTokenResponse
        {
            public FBAccessTokenResponse() { }

            public string access_token { get; set; }
            public string expires_in { get; set; }

            public string token_type { get; set; }
        }

        public class FBLongLivedTokenResponse
        {
            FBLongLivedTokenResponse() { }

            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string machine_id { get; set; }
        }


        public static string CleanJSONString(string dirty)
        {
            string clean = dirty.Trim();
            clean = clean.Replace("{", "");
            clean = clean.Replace("}", "");
            clean = clean.Replace("\"", "");
            clean = clean.Replace(",", "").Trim();
            clean = clean.Replace("[", "").Trim();
            return clean;
        }


        #endregion

    }
}
