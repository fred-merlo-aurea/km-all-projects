using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Xml;
using ECN_Framework_Entities.Salesforce.Helpers;

namespace ECN_Framework_Entities.Salesforce
{
    public static class SF_Authentication
    {
        public enum Method { GET, POST, PUT, DELETE };

        private static string _tokenEndPoint = "";
        private static string _authorizeEndPoint = "";
        private static string _redirectURL = "";
        private static string _tokenEndPoint_Sandbox = "";
        private static string _authorizeEndPoint_Sandbox = "";
        public static string TokenEndPoint
        {
            get
            {
                if (_tokenEndPoint.Length == 0)
                    _tokenEndPoint = System.Configuration.ConfigurationManager.AppSettings["SF_TokenEndPoint"].ToString();
                return _tokenEndPoint;
            }
            set { _tokenEndPoint = value; }

        }
        public static string AuthorizeEndPoint
        {
            get
            {
                if (_authorizeEndPoint.Length == 0)
                {
                    _authorizeEndPoint = System.Configuration.ConfigurationManager.AppSettings["SF_AuthEndPoint"].ToString();
                }
                return _authorizeEndPoint;
            }
            set { _authorizeEndPoint = value; }
        }
        public static string RedirectURL
        {
            get
            {
                if (_redirectURL.Length == 0)
                {
                    _redirectURL = HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["SF_RedirectURL"].ToString(), System.Text.Encoding.UTF8);
                }
                return _redirectURL;
            }
            set { _redirectURL = value; }
        }
        private static string _consumerKey = "";
        private static string _consumerSecret = "";
        public static string ConsumerKey
        {
            get
            {
                if (_consumerKey.Length == 0)
                {
                    //_consumerKey = System.Configuration.ConfigurationManager.AppSettings["SF_ConsumerKey"].ToString();
                }
                return _consumerKey;
            }
            set { _consumerKey = value; }
        }
        public static string ConsumerSecret
        {
            get
            {
                if (_consumerSecret.Length == 0)
                {
                    // _consumerSecret = System.Configuration.ConfigurationManager.AppSettings["SF_ConsumerSecret"].ToString();
                }
                return _consumerSecret;
            }
            set { _consumerSecret = value; }
        }

        private static bool _LoginAttempted = false;
        public static bool LoginAttempted
        {
            get
            {
                if (_LoginAttempted != null)
                    return _LoginAttempted;
                else
                    return false;
            }
            set
            {
                _LoginAttempted = value;
            }
        }

        private static bool _LoggedIn = false;
        public static bool LoggedIn
        {
            get
            {
                if (_LoggedIn != null)
                    return _LoggedIn;
                else
                    return false;
            }
            set
            {
                _LoggedIn = value;
            }
        }

        private static SF_Token _token = null;
        public static SF_Token Token
        {
            get
            {
                if (_token != null)
                    return _token;
                else
                    return new SF_Token();
            }
            set
            {
                _token = value;
            }
        }
        public static string AuthCode
        {
            get
            {
                if (HttpContext.Current.Session["SF_AuthCode"] != null)
                    return HttpContext.Current.Session["SF_AuthCode"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["SF_AuthCode"] = value;
            }
        }

        public static string SandboxRedirect
        {
            get
            {
                if (HttpContext.Current.Session["SF_SandboxURL"] != null)
                    return HttpContext.Current.Session["SF_SandboxURL"].ToString();
                else
                {
                    return ConfigurationManager.AppSettings["SF_RedirectURL_Sandbox"].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session["SF_SandboxURL"] = value;
            }
        }

        public static void SF_Login(string consumerKey, string consumerSecret, bool useSandbox)
        {
            _authorizeEndPoint = "";
            _consumerKey = "";
            _consumerSecret = "";
            _redirectURL = "";
            _tokenEndPoint = "";
            _tokenEndPoint_Sandbox = "";

            AuthorizeEndPoint = "";
            RedirectURL = "";
            TokenEndPoint = "";
            SandboxRedirect = "";
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;

            LoginAttempted = true;

            // response_type  ---  Must be code for this authentication flow.
            // client_id      ---  The Consumer Key from the remote access application definition.
            // redirect_url   ---  The Callback URL from the remote access application definition.

            //The following parameters are optional:
            // display      ---   Changes the login page’s display type. Valid values are:
            //• page—Full-page authorization screen. This is the default value if none is specified.
            //• popup—Compact dialog optimized for modern Web browser popup windows.
            //• touch—Mobile-optimized dialog designed for modern smartphones such as Android and iPhone.
            //• mobile—Mobile optimized dialog designed for smartphones such as BlackBerry OS 5 that don’t support touch screens.

            if (useSandbox)
            {
                TokenEndPoint = ConfigurationManager.AppSettings["SF_TokenEndPoint_Sandbox"].ToString();
                AuthorizeEndPoint = ConfigurationManager.AppSettings["SF_AuthEndPoint_Sandbox"].ToString();
                RedirectURL = ConfigurationManager.AppSettings["SF_RedirectURL_Sandbox"].ToString();
            }
            var redirectTo = AuthenticationHelper.Login(AuthorizeEndPoint, ConsumerKey, RedirectURL);
            HttpContext.Current.Response.Redirect(redirectTo);

        }

        public static void SetSalesForceToken(string ConsumerKey, string ConsumerSecret, bool useSandbox)
        {
            if (useSandbox)
            {
                TokenEndPoint = ConfigurationManager.AppSettings["SF_TokenEndPoint_Sandbox"].ToString();
                AuthorizeEndPoint = ConfigurationManager.AppSettings["SF_AuthEndPoint_Sandbox"].ToString();
                RedirectURL = ConfigurationManager.AppSettings["SF_RedirectURL_Sandbox"].ToString();
            }

            Token = AuthenticationHelper.GetToken<SF_Token>(TokenEndPoint, ConsumerKey, ConsumerSecret, RedirectURL, AuthCode);
            LoggedIn = Token != null;
        }

        public static void RefreshSalesForceToken(string RefreshToken, string consumerSecret, string consumerKey, bool useSandbox)
        {
            ConsumerSecret = consumerSecret;
            ConsumerKey = consumerKey;

            if (useSandbox)
            {
                TokenEndPoint = ConfigurationManager.AppSettings["SF_TokenEndPoint_Sandbox"].ToString();
                AuthorizeEndPoint = ConfigurationManager.AppSettings["SF_AuthEndPoint_Sandbox"].ToString();
                RedirectURL = ConfigurationManager.AppSettings["SF_RedirectURL_Sandbox"].ToString();
            }

            var refreshed = AuthenticationHelper.GetRefreshToken<SF_Token>(TokenEndPoint, RefreshToken, ConsumerKey, ConsumerSecret);
            if (refreshed != null)
            {
                refreshed.refresh_token = Token.refresh_token;
                Token = refreshed;
                LoggedIn = true;
            }
            else
            {
                LoggedIn = false;
            }
        }
    }
}