using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace KM.Common
{
    public class WebFunctions
    {
        public static Control FindControlRecursive(Control controlToSearch, string id)
        {
            if (null == controlToSearch)
                throw new ArgumentNullException("controlToSearch");

            if (string.IsNullOrEmpty("id"))
                throw new ArgumentNullException("id");

            Control result = controlToSearch.FindControl(id);
            if (null == result)
            {
                if (null != controlToSearch.Controls && controlToSearch.Controls.Count > 0)
                {
                    foreach (Control c in controlToSearch.Controls)
                    {
                        result = FindControlRecursive(c, id);
                        if (null != result)
                            break;
                    }
                }
            }
            return result;
        }
         /// <summary>
        /// Finds a control recursive
        /// </summary>
        /// <typeparam name="T">Control class</typeparam>
        /// <param name="controls">Input control collection</param>
        /// <returns>Found control</returns>
        public static T FindControlRecursive<T>(ControlCollection controls) where T : class
        {
            T found = default(T);

            if (controls != null && controls.Count > 0)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i] is T)
                    {
                        found = controls[i] as T;
                        break;
                    }
                    else
                    {
                        found = FindControlRecursive<T>(controls[i].Controls);
                        if (found != null)
                            break;
                    }
                }
            }

            return found;
        }

        public static void DeleteCache(string cacheName)
        {
            if (System.Web.HttpContext.Current != null)
            {
                //delete cache
                System.Web.HttpContext.Current.Cache.Remove(cacheName);
            }
        }
        public static DateTime GetMinDate()
        {
            DateTime minDate = Convert.ToDateTime("1/1/1900");
            return minDate;
        }
        public static TimeSpan GetMinTime()
        {
            TimeSpan minTime;
            TimeSpan.TryParse("00:00:00.0000000", out minTime);
            return minTime;
        }

        //public static string Encrypt_UrlEncode_QueryString(string qstring)
        //{
        //    KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
        //    if (ec != null && ec.ID > 0)
        //    {
        //        return System.Web.HttpContext.Current.Server.UrlEncode(KM.Common.Encryption.Encrypt(qstring, ec));
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
        //public static string DeCrypt_DeCode_EncryptedQueryString(string encrypted_Qstring)
        //{
        //    KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));

        //    string encryptedQuery = System.Web.HttpContext.Current.Server.UrlDecode(encrypted_Qstring);
        //    string decryptedQuery = KM.Common.Encryption.Decrypt(encryptedQuery, ec);
        //    return decryptedQuery;
        //}

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            bool result = false;
            if (String.IsNullOrEmpty(email))
                return result;
            email = email.Trim();
            result = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z|0-9]{2,63})(\]?)$");
            return result;
        }

        /// <summary>
        /// Gets query string value by name
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static string QueryString(string name)
        {
            string result = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString[name] != null)
                result = HttpContext.Current.Request.QueryString[name].ToString();
            return result;
        }

        /// <summary>
        /// Gets boolean value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static bool QueryStringBool(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            return (resultStr == "YES" || resultStr == "TRUE" || resultStr == "1");
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            int result;
            Int32.TryParse(resultStr, out result);
            return result;
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string name, int defaultValue)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            if (resultStr.Length > 0)
            {
                return Int32.Parse(resultStr);
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets GUID value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static Guid? QueryStringGuid(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            Guid? result = null;
            try
            {
                result = new Guid(resultStr);
            }
            catch
            {
            }
            return result;
        }
        
        /// <summary>
        /// Gets Form String
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Result</returns>
        public static string GetFormString(string name)
        {
            string result = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request[name] != null)
                result = HttpContext.Current.Request[name].ToString();
            return result;
        }

        /// <summary>
        /// Set meta http equiv (eg. redirects)
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="httpEquiv">Http Equiv</param>
        /// <param name="content">Content</param>
        public static void SetMetaHttpEquiv(Page page, string httpEquiv, string content)
        {
            if (page.Header == null)
                return;

            HtmlMeta meta = new HtmlMeta();
            if (page.Header.FindControl("meta" + httpEquiv) != null)
            {
                meta = (HtmlMeta)page.Header.FindControl("meta" + httpEquiv);
                meta.Content = content;
            }
            else
            {
                meta.ID = "meta" + httpEquiv;
                meta.HttpEquiv = httpEquiv;
                meta.Content = content;
                page.Header.Controls.Add(meta);
            }
        }

        /// <summary>
        /// Selects item
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="value">Value to select</param>
        public static void SelectListItem(DropDownList list, object value)
        {
            if (list.Items.Count != 0)
            {
                var selectedItem = list.SelectedItem;
                if (selectedItem != null)
                    selectedItem.Selected = false;
                if (value != null)
                {
                    selectedItem = list.Items.FindByValue(value.ToString());
                    if (selectedItem != null)
                        selectedItem.Selected = true;
                }
            }
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string name)
        {
            string tmpS = string.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables[name] != null)
                {
                    tmpS = HttpContext.Current.Request.ServerVariables[name].ToString();
                }
            }
            catch
            {
                tmpS = string.Empty;
            }
            return tmpS;
        }

        /// <summary>
        /// Disable browser cache
        /// </summary>
        public static void DisableBrowserCache()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Cache.SetExpires(new DateTime(1995, 5, 6, 12, 0, 0, DateTimeKind.Utc));
                HttpContext.Current.Response.Cache.SetNoStore();
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");

            }
        }

       
        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public static bool IsCurrentConnectionSecured()
        {
            bool useSSL = false;
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                useSSL = HttpContext.Current.Request.IsSecureConnection;
                //when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true, use the statement below
                //just uncomment it
                //useSSL = HttpContext.Current.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSSL;
        }


        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <returns></returns>
        public static string GetThisPageUrl(bool includeQueryString, string appURL)
        {
            string URL = string.Empty;
            if (HttpContext.Current == null)
                return URL;

            if (includeQueryString)
            {
                bool useSSL = IsCurrentConnectionSecured();
                if (appURL.EndsWith("/"))
                    appURL = appURL.Substring(0, appURL.Length - 1);
                URL = appURL + HttpContext.Current.Request.RawUrl;
            }
            else
            {
                URL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            }
            URL = URL.ToLowerInvariant();
            return URL;
        }
        /// <summary>
        /// Modifies query string
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryStringModification">Query string modification</param>
        /// <param name="targetLocationModification">Target location modification</param>
        /// <returns>New url</returns>
        public static string ModifyQueryString(string url, string queryStringModification, string targetLocationModification)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();
            
            if (queryStringModification == null)
                queryStringModification = string.Empty;
            queryStringModification = queryStringModification.ToLowerInvariant();
            
            if (targetLocationModification == null)
                targetLocationModification = string.Empty;
            targetLocationModification = targetLocationModification.ToLowerInvariant();


            string str = string.Empty;
            string str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in queryStringModification.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new char[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }
                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }
            if (!string.IsNullOrEmpty(targetLocationModification))
            {
                str2 = targetLocationModification;
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2))).ToLowerInvariant();
        }

        /// <summary>
        /// Remove query string from url
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryString">Query string to remove</param>
        /// <returns>New url</returns>
        public static string RemoveQueryString(string url, string queryString)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (queryString == null)
                queryString = string.Empty;
            queryString = queryString.ToLowerInvariant();
            

            string str = string.Empty;
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    dictionary.Remove(queryString);

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        /// <summary>
        /// Sets cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie value</param>
        /// <param name="ts">Timespan</param>
        public static void SetCookie(string cookieName, string cookieValue, TimeSpan ts)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Value = HttpContext.Current.Server.UrlEncode(cookieValue);
                DateTime dt = DateTime.Now;
                cookie.Expires = dt.Add(ts);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        /// <summary>
        /// Gets cookie string
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="decode">Decode cookie</param>
        /// <returns>Cookie string</returns>
        public static string GetCookieString(string cookieName, bool decode)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
            {
                return string.Empty;
            }
            try
            {
                string tmp = HttpContext.Current.Request.Cookies[cookieName].Value.ToString();
                if (decode)
                    tmp = HttpContext.Current.Server.UrlDecode(tmp);
                return tmp;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets boolean value from cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <returns>Result</returns>
        public static bool GetCookieBool(string cookieName)
        {
            string str1 = GetCookieString(cookieName, true).ToUpperInvariant();
            return (str1 == "TRUE" || str1 == "YES" || str1 == "1");
        }

        /// <summary>
        /// Gets integer value from cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <returns>Result</returns>
        public static int GetCookieInt(string cookieName)
        {
            string str1 = GetCookieString(cookieName, true);
            if (!String.IsNullOrEmpty(str1))
                return Convert.ToInt32(str1);
            else
                return 0;
        }

        /// <summary>
        /// Gets boolean value from NameValue collection
        /// </summary>
        /// <param name="config">NameValue collection</param>
        /// <param name="valueName">Name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Result</returns>
        public static bool ConfigGetBooleanValue(NameValueCollection config, 
            string valueName, bool defaultValue)
        {
            bool result;
            string str1 = config[valueName];
            if (str1 == null)
                return defaultValue;
            if (!bool.TryParse(str1, out result))
                throw new Exception(string.Format("Value must be boolean {0}", valueName));
            return result;
        }

        /// <summary>
        /// Gets integer value from NameValue collection
        /// </summary>
        /// <param name="config">NameValue collection</param>
        /// <param name="valueName">Name</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="zeroAllowed">Zero allowed</param>
        /// <param name="maxValueAllowed">Max value allowed</param>
        /// <returns>Result</returns>
        public static int ConfigGetIntValue(NameValueCollection config, 
            string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
        {
            int result;
            string str1 = config[valueName];
            if (str1 == null)
                return defaultValue;
            if (!int.TryParse(str1, out result))
            {
                if (zeroAllowed)
                {
                    throw new Exception(string.Format("Value must be non negative integer {0}", valueName));
                }
                throw new Exception(string.Format("Value must be positive integer {0}", valueName));
            }
            if (zeroAllowed && (result < 0))
                throw new Exception(string.Format("Value must be non negative integer {0}", valueName));
            if (!zeroAllowed && (result <= 0))
                throw new Exception(string.Format("Value must be positive integer {0}", valueName));
            if ((maxValueAllowed > 0) && (result > maxValueAllowed))
                throw new Exception(string.Format("Value too big {0}", valueName));
            return result;
        }

        /// <summary>
        /// Write XML to response
        /// </summary>
        /// <param name="xml">XML</param>
        /// <param name="fileName">Filename</param>
        public static void WriteResponseXml(string xml, string fileName)
        {
            if(!String.IsNullOrEmpty(xml))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlDeclaration decl = document.FirstChild as XmlDeclaration;
                if(decl != null)
                {
                    decl.Encoding = "utf-8";
                }
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xml";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                response.BinaryWrite(Encoding.UTF8.GetBytes(document.InnerXml));
                response.End();
            }
        }

        /// <summary>
        /// Write text to response
        /// </summary>
        /// <param name="txt">text</param>
        /// <param name="fileName">Filename</param>
        public static void WriteResponseTxt(string txt, string fileName)
        {
            if (!String.IsNullOrEmpty(txt))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/plain";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                response.BinaryWrite(Encoding.UTF8.GetBytes(txt));
                response.End();
            }
        }

        /// <summary>
        /// Write XLS file to response
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponseXls(string filePath, string targetFileName)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xls";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(File.ReadAllBytes(filePath));
                response.End();
            }
        }
        
        /// <summary>
        /// Write PDF file to response
        /// </summary>
        /// <param name="filePath">File napathme</param>
        /// <param name="targetFileName">Target file name</param>
        /// <remarks>For BeatyStore project</remarks>
        public static void WriteResponsePdf(string filePath, string targetFileName)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/pdf";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(File.ReadAllBytes(filePath));
                response.End();
            }
        }
        
        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        /// <summary>
        /// Fills drop down list with values of enumaration
        /// </summary>
        /// <param name="list">Dropdownlist</param>
        /// <param name="enumType">Enumeration</param>
        public static void FillDropDownWithEnum(DropDownList list, Type enumType)
        {
            FillDropDownWithEnum(list, enumType, true);
        }

        /// <summary>
        /// Fills drop down list with values of enumaration
        /// </summary>
        /// <param name="list">Dropdownlist</param>
        /// <param name="enumType">Enumeration</param>
        /// <param name="clearListItems">Clear list of exsisting items</param>
        public static void FillDropDownWithEnum(DropDownList list, Type enumType, bool clearListItems)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType must be enum type");
            }

            if (clearListItems)
            {
                list.Items.Clear();
            }
            string[] strArray = Enum.GetNames(enumType);
            foreach (string str2 in strArray)
            {
                int enumValue = (int)Enum.Parse(enumType, str2, true);
                ListItem ddlItem = new ListItem(WebFunctions.ConvertEnum(str2), enumValue.ToString());
                list.Items.Add(ddlItem);
            }
        }

        /// <summary>
        /// Set response NoCache
        /// </summary>
        /// <param name="response">Response</param>
        public static void SetResponseNoCache(HttpResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            //response.Cache.SetCacheability(HttpCacheability.NoCache) 

            response.CacheControl = "private";
            response.Expires = 0;
            response.AddHeader("pragma", "no-cache");
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
                return str.Substring(0, maxLength);
            else
                return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();

            // Loop is faster than RegEx
            //return Regex.Replace(str, "\\D", "");
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }

        /// <summary>
        /// Get a value indicating whether content page is requested
        /// </summary>
        /// <returns>Result</returns>
        public static bool IsContentPageRequested()
        {
            HttpContext context = HttpContext.Current;
            HttpRequest request = context.Request;

            if (!request.Url.LocalPath.ToLower().EndsWith(".aspx") &&
                !request.Url.LocalPath.ToLower().EndsWith(".asmx") &&
                !request.Url.LocalPath.ToLower().EndsWith(".ashx"))
            {
                return false;
            }

            return true;
        }



        public class ControlFinder<T> where T : Control
        {
            private readonly List<T> _foundControls = new List<T>();
            public IEnumerable<T> FoundControls
            {
                get { return _foundControls; }
            }
            public void FindChildControlsRecursive(Control control)
            {
                foreach (Control childControl in control.Controls)
                {
                    if (childControl.GetType() == typeof(T))
                    {
                        _foundControls.Add((T)childControl);
                    }
                    else
                    {
                        FindChildControlsRecursive(childControl);
                    }
                }
            }
        } 
    }
}
