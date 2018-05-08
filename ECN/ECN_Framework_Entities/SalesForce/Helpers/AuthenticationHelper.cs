using System.Net;
using System.Text;
using ECN_Framework_Entities.Salesforce.Interfaces;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    public class AuthenticationHelper
    {
        private const string AuthorizeTypeParamValue = "?grant_type=authorization_code";
        private const string RefreshTokenTypeParamValue = "?grant_type=refresh_token";
        private const string RefreshTokenParam = "&refresh_token=";
        private const string ClientIdParam = "&client_id=";
        private const string ClientSecretParam = "&client_secret=";
        private const string RedirectUrlParam = "&redirect_uri=";
        private const string CodeParam = "&code=";
        private const string FormatJsonParamValue = "&format=json";
        private const string ResponseTypeParamValue = "?response_type=code";
        private const string DisplayParamValue = "&display=popup-Compact";
        private const string AuthContentType = "application/x-www-form-urlencoded";

        private static ISFUtilities _sFUtilities;

        static AuthenticationHelper()
        {
            _sFUtilities = new SFUtilitiesAdapter();
        }

        public static void InitUtilities(ISFUtilities utilities)
        {
            _sFUtilities = utilities;
        }

        public static string GetTokenUrl(string endpoint, string consumerKey, string consumerSecret, string redirectURL, string authCode)
        {
            var sfOAuth = new StringBuilder();
            sfOAuth.Append(endpoint);
            sfOAuth.Append(AuthorizeTypeParamValue);
            sfOAuth.Append(ClientIdParam + consumerKey);
            sfOAuth.Append(ClientSecretParam + consumerSecret);
            sfOAuth.Append(RedirectUrlParam + redirectURL);
            sfOAuth.Append(CodeParam + authCode);

            return sfOAuth.ToString();
        }

        public static string GetRefreshTokenUrl(string endpoint, string refreshToken, string consumerKey, string consumerSecret)
        {
            var sfOAuth = new StringBuilder();
            sfOAuth.Append(endpoint);
            sfOAuth.Append(RefreshTokenTypeParamValue);
            sfOAuth.Append(RefreshTokenParam + refreshToken);
            sfOAuth.Append(ClientIdParam + consumerKey);
            sfOAuth.Append(ClientSecretParam + consumerSecret);
            sfOAuth.Append(FormatJsonParamValue);

            return sfOAuth.ToString();
        }

        public static string GetLoginUrl(string endpoint, string consumerKey, string redirectUrl)
        {
            var sfOAuth = new StringBuilder();
            sfOAuth.Append(endpoint);
            sfOAuth.Append(ResponseTypeParamValue);
            sfOAuth.Append(ClientIdParam + consumerKey);
            sfOAuth.Append(RedirectUrlParam + redirectUrl);
            sfOAuth.Append(DisplayParamValue);

            return sfOAuth.ToString();
        }

        public static T GetToken<T>(string tokenEndPoint, string consumerKey, string consumerSecret, string redirectURL, string authCode)
        {
            //PARAMETERS - TOKEN
            // grant_type    ---  Value must be authorization_code for this flow.
            // client_id     ---  The Consumer Key from the remote access application definition.
            // client_secret ---  The Consumer Secret from the remote access application definition.
            // redirect_uri  ---  The Callback URL from the remote access application definition.
            // code          ---  Authorization code the consumer must use to obtain the access and refresh tokens.

            //OPTIONAL PARAMETERS - TOKEN
            //format        ---  Expected return format. The default is json. Values are:
            //• urlencoded
            //• json
            //• xml
            //The return format can also be specified in the header of the request using one of the following:
            //• Accept: application/x-www-form-urlencoded
            //• Accept: application/json
            //• Accept: application/xml

            //Example:
            //POST /services/oauth2/token HTTP/1.1
            //Host: login.salesforce.com
            //grant_type=authorization_code&code=aPrxsmIEeqM9PiQroGEWx1UiMQd95_5JUZ
            //VEhsOFhS8EVvbfYBBJli2W5fn3zbo.8hojaNW_1g%3D%3D&client_id=3MVG9lKcPoNI
            //NVBIPJjdw1J9LLM82HnFVVX19KY1uA5mu0QqEWhqKpoW3svG3XHrXDiCQjK1mdgAvhCs
            //cA9GE&client_secret=1955279925675241571&
            //redirect_uri=https%3A%2F%2Fwww.mysite.com%2Fcode_callback.jsp

            //do post .NET 4.5
            //System.Net.HttpClient httpclient = new HttpClient();
            //PostMethod post = new PostMethod(environment);
            //post.addParameter("code", code);
            //post.addParameter("grant_type", "authorization_code");
            ///** For session ID instead of OAuth 2.0, use "grant_type", "password" **/
            //post.addParameter("client_id", clientId);
            //post.addParameter("client_secret", clientSecret);
            //post.addParameter("redirect_uri", redirectUri);

            if (!string.IsNullOrWhiteSpace(authCode))
            {
                var url = GetTokenUrl(tokenEndPoint, consumerKey, consumerSecret, redirectURL, authCode);
                return GetToken<T>(url);
            }

            return default(T);
        }

        public static T GetRefreshToken<T>(string tokenEndPoint, string refreshToken, string consumerKey, string consumerSecret)
        {
            // grant_type    --- Value must be refresh_token.
            // refresh_token --- The refresh token the client application already received.
            // client_id     --- The Consumer Key from the remote access application definition.
            // client_secret --- The Consumer Secret from the remote access application definition. This parameter is optional.
            // format        --- Expected return format. The default is json. Values are:
            //• urlencoded
            //• json
            //• xml
            // The return format can also be specified in the header of the request using one of the following:
            //• Accept: application/x-www-form-urlencoded
            //• Accept: application/json
            //• Accept: application/xml
            //This parameter is optional.

            //Example POST
            //POST /services/oauth2/token HTTP/1.1
            //Host: https://login.salesforce.com/
            //grant_type=refresh_token&client_id=3MVG9lKcPoNINVBIPJjdw1J9LLM82HnFVVX19KY1uA5mu0
            //QqEWhqKpoW3svG3XHrXDiCQjK1mdgAvhCscA9GE&client_secret=1955279925675241571
            //&refresh_token=your token here
            //User Name: justin.wagner@teamkm.com
            //Password: ECN_2_SalesForce
            //https://localhost:1572/ECN_SF_Integration.aspx

            // response_type  ---  Must be code for this authentication flow.
            // client_id      ---  The Consumer Key from the remote access application definition.
            // redirect_url   ---  The Callback URL from the remote access application definition.

            //The following parameters are optional:
            // display      ---   Changes the login page’s display type. Valid values are:
            //• page—Full-page authorization screen. This is the default value if none is specified.
            //• popup—Compact dialog optimized for modern Web browser popup windows.
            //• touch—Mobile-optimized dialog designed for modern smartphones such as Android and iPhone.
            //• mobile—Mobile optimized dialog designed for smartphones such as BlackBerry OS 5 that don’t support touch screens.

            var url = GetRefreshTokenUrl(tokenEndPoint, refreshToken, consumerKey, consumerSecret);
            return GetToken<T>(url);
        }

        public static string Login(string authorizeEndPoint, string consumerKey, string redirectUrl)
        {
            var url = GetLoginUrl(authorizeEndPoint, consumerKey, redirectUrl);
            var req = CreateRequest(url);

            return SFUtilitiesAdapter.SafeExecute<WebException, string>(
                 () =>
                 {
                     using (WebResponse resp = req.GetResponse())
                     {
                         return resp.ResponseUri.ToString();
                     }
                 },
                  ex => _sFUtilities.LogWebException(ex, url));
        }

        private static WebRequest CreateRequest(string url)
        {
            var req = _sFUtilities.CreateWebRequest(url);
            req.Method = Method.POST.ToString();
            req.ContentType = AuthContentType;
            return req;
        }

        private static T GetToken<T>(string url)
        {
            var req = CreateRequest(url);

            return SFUtilitiesAdapter.SafeExecute<WebException, T>(
                () => _sFUtilities.ReadToken<T>(req),
                ex => _sFUtilities.LogWebException(ex, url));
        }
    }
}
