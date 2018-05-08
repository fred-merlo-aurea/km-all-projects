using System;
using System.Collections.Generic;
using System.Linq;

namespace KMPlatform.BusinessLogic
{
    public class ServerVariable
    {
        public Object.ServerVariable GetServerVariables()
        {
            Object.ServerVariable sv = new Object.ServerVariable();

            Core_AMS.Utilities.HTTPFunctions http = new Core_AMS.Utilities.HTTPFunctions();
            Dictionary<string, string> dict = http.GetServerVariables();

            foreach(KeyValuePair<string,string> kvp in dict)
            {
                if (kvp.Key.Equals("ALL_HTTP")) sv.ALL_HTTP = kvp.Value;
                else if (kvp.Key.Equals("ALL_RAW")) sv.ALL_RAW = kvp.Value;
                else if (kvp.Key.Equals("APPL_MD_PATH")) sv.APPL_MD_PATH = kvp.Value;
                else if (kvp.Key.Equals("APPL_PHYSICAL_PATH")) sv.APPL_PHYSICAL_PATH = kvp.Value;
                else if (kvp.Key.Equals("AUTH_PASSWORD")) sv.AUTH_PASSWORD = kvp.Value;
                else if (kvp.Key.Equals("AUTH_TYPE")) sv.AUTH_TYPE = kvp.Value;
                else if (kvp.Key.Equals("AUTH_USER")) sv.AUTH_USER = kvp.Value;
                else if (kvp.Key.Equals("CERT_COOKIE")) sv.CERT_COOKIE = kvp.Value;
                else if (kvp.Key.Equals("CERT_FLAGS")) sv.CERT_FLAGS = kvp.Value;
                else if (kvp.Key.Equals("CERT_ISSUER")) sv.CERT_ISSUER = kvp.Value;
                else if (kvp.Key.Equals("CERT_KEYSIZE")) sv.CERT_KEYSIZE = kvp.Value;
                else if (kvp.Key.Equals("CERT_SECRETKEYSIZE")) sv.CERT_SECRETKEYSIZE = kvp.Value;
                else if (kvp.Key.Equals("CERT_SERIALNUMBER")) sv.CERT_SERIALNUMBER = kvp.Value;
                else if (kvp.Key.Equals("CERT_SERVER_ISSUER")) sv.CERT_SERVER_ISSUER = kvp.Value;
                else if (kvp.Key.Equals("CERT_SERVER_SUBJECT")) sv.CERT_SERVER_SUBJECT = kvp.Value;
                else if (kvp.Key.Equals("CERT_SUBJECT")) sv.CERT_SUBJECT = kvp.Value;
                else if (kvp.Key.Equals("CONTENT_LENGTH")) sv.CONTENT_LENGTH = kvp.Value;
                else if (kvp.Key.Equals("CONTENT_TYPE")) sv.CONTENT_TYPE = kvp.Value;
                else if (kvp.Key.Equals("GATEWAY_INTERFACE")) sv.GATEWAY_INTERFACE = kvp.Value;
                else if (kvp.Key.Equals("HTTP_")) sv.HTTP_ = kvp.Value;
                else if (kvp.Key.Equals("HTTP_ACCEPT")) sv.HTTP_ACCEPT = kvp.Value;
                else if (kvp.Key.Equals("HTTP_ACCEPT_LANGUAGE")) sv.HTTP_ACCEPT_LANGUAGE = kvp.Value;
                else if (kvp.Key.Equals("HTTP_COOKIE")) sv.HTTP_COOKIE = kvp.Value;
                else if (kvp.Key.Equals("HTTP_REFERER")) sv.HTTP_REFERER = kvp.Value;
                else if (kvp.Key.Equals("HTTP_USER_AGENT")) sv.HTTP_USER_AGENT = kvp.Value;
                else if (kvp.Key.Equals("HTTPS")) sv.HTTPS = kvp.Value;
                else if (kvp.Key.Equals("HTTPS_KEYSIZE")) sv.HTTPS_KEYSIZE = kvp.Value;
                else if (kvp.Key.Equals("HTTPS_SECRETKEYSIZE")) sv.HTTPS_SECRETKEYSIZE = kvp.Value;
                else if (kvp.Key.Equals("HTTPS_SERVER_ISSUER")) sv.HTTPS_SERVER_ISSUER = kvp.Value;
                else if (kvp.Key.Equals("HTTPS_SERVER_SUBJECT")) sv.HTTPS_SERVER_SUBJECT = kvp.Value;
                else if (kvp.Key.Equals("INSTANCE_ID")) sv.INSTANCE_ID = kvp.Value;
                else if (kvp.Key.Equals("INSTANCE_META_PATH")) sv.INSTANCE_META_PATH = kvp.Value;
                else if (kvp.Key.Equals("LOCAL_ADDR")) sv.LOCAL_ADDR = kvp.Value;
                else if (kvp.Key.Equals("LOGON_USER")) sv.LOGON_USER = kvp.Value;
                else if (kvp.Key.Equals("PATH_INFO")) sv.PATH_INFO = kvp.Value;
                else if (kvp.Key.Equals("PATH_TRANSLATED")) sv.PATH_TRANSLATED = kvp.Value;
                else if (kvp.Key.Equals("QUERY_STRING")) sv.QUERY_STRING = kvp.Value;
                else if (kvp.Key.Equals("REMOTE_ADDR")) sv.REMOTE_ADDR = kvp.Value;
                else if (kvp.Key.Equals("REMOTE_HOST")) sv.REMOTE_HOST = kvp.Value;
                else if (kvp.Key.Equals("REMOTE_USER")) sv.REMOTE_USER = kvp.Value;
                else if (kvp.Key.Equals("REQUEST_METHOD")) sv.REQUEST_METHOD = kvp.Value;
                else if (kvp.Key.Equals("SCRIPT_NAME")) sv.SCRIPT_NAME = kvp.Value;
                else if (kvp.Key.Equals("SERVER_NAME")) sv.SERVER_NAME = kvp.Value;
                else if (kvp.Key.Equals("SERVER_PORT")) sv.SERVER_PORT = kvp.Value;
                else if (kvp.Key.Equals("SERVER_PORT_SECURE")) sv.SERVER_PORT_SECURE = kvp.Value;
                else if (kvp.Key.Equals("SERVER_PROTOCOL")) sv.SERVER_PROTOCOL = kvp.Value;
                else if (kvp.Key.Equals("SERVER_SOFTWARE")) sv.SERVER_SOFTWARE = kvp.Value;
                else if (kvp.Key.Equals("URL")) sv.URL = kvp.Value;

            }
            return sv;
        }
    }
}
