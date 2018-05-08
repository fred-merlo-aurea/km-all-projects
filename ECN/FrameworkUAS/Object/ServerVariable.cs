using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Object
{
    [Serializable]
    [DataContract]
    public class ServerVariable
    {
        public ServerVariable()
        {
            ALL_HTTP = string.Empty;
            ALL_RAW = string.Empty;
            APPL_MD_PATH = string.Empty;
            APPL_PHYSICAL_PATH = string.Empty;
            AUTH_PASSWORD = string.Empty;
            AUTH_TYPE = string.Empty;
            AUTH_USER = string.Empty;
            CERT_COOKIE = string.Empty;
            CERT_FLAGS = string.Empty;
            CERT_ISSUER = string.Empty;
            CERT_KEYSIZE = string.Empty;
            CERT_SECRETKEYSIZE = string.Empty;
            CERT_SERIALNUMBER = string.Empty;
            CERT_SERVER_ISSUER = string.Empty;
            CERT_SERVER_SUBJECT = string.Empty;
            CERT_SUBJECT = string.Empty;
            CONTENT_LENGTH = string.Empty;
            CONTENT_TYPE = string.Empty;
            GATEWAY_INTERFACE = string.Empty;
            HTTP_ = string.Empty;
            HTTP_ACCEPT = string.Empty;
            HTTP_ACCEPT_LANGUAGE = string.Empty;
            HTTP_COOKIE = string.Empty;
            HTTP_REFERER = string.Empty;
            HTTP_USER_AGENT = string.Empty;
            HTTPS = string.Empty;
            HTTPS_KEYSIZE = string.Empty;
            HTTPS_SECRETKEYSIZE = string.Empty;
            HTTPS_SERVER_ISSUER = string.Empty;
            HTTPS_SERVER_SUBJECT = string.Empty;
            INSTANCE_ID = string.Empty;
            INSTANCE_META_PATH = string.Empty;
            LOCAL_ADDR = string.Empty;
            LOGON_USER = string.Empty;
            PATH_INFO = string.Empty;
            PATH_TRANSLATED = string.Empty;
            QUERY_STRING = string.Empty;
            REMOTE_ADDR = string.Empty;
            REMOTE_HOST = string.Empty;
            REMOTE_USER = string.Empty;
            REQUEST_METHOD = string.Empty;
            SCRIPT_NAME = string.Empty;
            SERVER_NAME = string.Empty;
            SERVER_PORT = string.Empty;
            SERVER_PORT_SECURE = string.Empty;
            SERVER_PROTOCOL = string.Empty;
            SERVER_SOFTWARE = string.Empty;
            URL = string.Empty;
        }
        #region Properties
        [DataMember]
        /// <summary>
        /// Returns all HTTP headers sent by the client. Always prefixed with HTTP_ and capitalized
        /// </summary>
        public string ALL_HTTP { get; set; }

        [DataMember]
        /// <summary>
        /// Returns all headers in raw form
        /// </summary>
        public string ALL_RAW { get; set; }

        [DataMember]
        /// <summary>
        /// Returns the meta base path for the application for the ISAPI DLL
        /// </summary>
        public string APPL_MD_PATH { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the physical path corresponding to the meta base path
        /// </summary>
        public string APPL_PHYSICAL_PATH { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the value entered in the client's authentication dialog
        /// </summary>
        public string AUTH_PASSWORD { get; set; }
        [DataMember]
        /// <summary>
        /// he authentication method that the server uses to validate users
        /// </summary>
        public string AUTH_TYPE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the raw authenticated user name
        /// </summary>
        public string AUTH_USER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the unique ID for client certificate as a string
        /// </summary>
        public string CERT_COOKIE { get; set; }
        [DataMember]
        /// <summary>
        /// bit0 is set to 1 if the client certificate is present and bit1 is set to 1 if the cCertification authority of the client certificate is not valid
        /// </summary>
        public string CERT_FLAGS { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the issuer field of the client certificate
        /// </summary>
        public string CERT_ISSUER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the number of bits in Secure Sockets Layer connection key size
        /// </summary>
        public string CERT_KEYSIZE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the number of bits in server certificate private key
        /// </summary>
        public string CERT_SECRETKEYSIZE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the serial number field of the client certificate
        /// </summary>
        public string CERT_SERIALNUMBER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the issuer field of the server certificate
        /// </summary>
        public string CERT_SERVER_ISSUER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the subject field of the server certificate
        /// </summary>
        public string CERT_SERVER_SUBJECT { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the subject field of the client certificate
        /// </summary>
        public string CERT_SUBJECT { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the length of the content as sent by the client
        /// </summary>
        public string CONTENT_LENGTH { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the data type of the content
        /// </summary>
        public string CONTENT_TYPE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the revision of the CGI specification used by the server
        /// </summary>
        public string GATEWAY_INTERFACE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the value stored in the header HeaderName
        /// </summary>
        public string HTTP_ { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the value of the Accept header
        /// </summary>
        public string HTTP_ACCEPT { get; set; }
        [DataMember]
        /// <summary>
        /// Returns a string describing the language to use for displaying content
        /// </summary>
        public string HTTP_ACCEPT_LANGUAGE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the cookie string included with the request
        /// </summary>
        public string HTTP_COOKIE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns a string containing the URL of the page that referred the request to the current page using an <a> tag. If the page is redirected, HTTP_REFERER is empty
        /// </summary>
        public string HTTP_REFERER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns a string describing the browser that sent the request
        /// </summary>
        public string HTTP_USER_AGENT { get; set; }
        [DataMember]
        /// <summary>
        /// Returns ON if the request came in through secure channel or OFF if the request came in through a non-secure channel
        /// </summary>
        public string HTTPS { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the number of bits in Secure Sockets Layer connection key size
        /// </summary>
        public string HTTPS_KEYSIZE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the number of bits in server certificate private key
        /// </summary>
        public string HTTPS_SECRETKEYSIZE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the issuer field of the server certificate
        /// </summary>
        public string HTTPS_SERVER_ISSUER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the subject field of the server certificate
        /// </summary>
        public string HTTPS_SERVER_SUBJECT { get; set; }
        [DataMember]
        /// <summary>
        /// The ID for the IIS instance in text format
        /// </summary>
        public string INSTANCE_ID { get; set; }
        [DataMember]
        /// <summary>
        /// The meta base path for the instance of IIS that responds to the request
        /// </summary>
        public string INSTANCE_META_PATH { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the server address on which the request came in
        /// </summary>
        public string LOCAL_ADDR { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the Windows account that the user is logged into
        /// </summary>
        public string LOGON_USER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns extra path information as given by the client
        /// </summary>
        public string PATH_INFO { get; set; }
        [DataMember]
        /// <summary>
        /// A translated version of PATH_INFO that takes the path and performs any necessary virtual-to-physical mapping
        /// </summary>
        public string PATH_TRANSLATED { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the query information stored in the string following the question mark (?) in the HTTP request
        /// </summary>
        public string QUERY_STRING { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the IP address of the remote host making the request
        /// </summary>
        public string REMOTE_ADDR { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the name of the host making the request
        /// </summary>
        public string REMOTE_HOST { get; set; }
        [DataMember]
        /// <summary>
        /// Returns an unmapped user-name string sent in by the user
        /// </summary>
        public string REMOTE_USER { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the method used to make the request
        /// </summary>
        public string REQUEST_METHOD { get; set; }
        [DataMember]
        /// <summary>
        /// Returns a virtual path to the script being executed
        /// </summary>
        public string SCRIPT_NAME { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the server's host name, DNS alias, or IP address as it would appear in self-referencing URLs
        /// </summary>
        public string SERVER_NAME { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the port number to which the request was sent
        /// </summary>
        public string SERVER_PORT { get; set; }
        [DataMember]
        /// <summary>
        /// Returns a string that contains 0 or 1. If the request is being handled on the secure port, it will be 1. Otherwise, it will be 0
        /// </summary>
        public string SERVER_PORT_SECURE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the name and revision of the request information protocol
        /// </summary>
        public string SERVER_PROTOCOL { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the name and version of the server software that answers the request and runs the gateway
        /// </summary>
        public string SERVER_SOFTWARE { get; set; }
        [DataMember]
        /// <summary>
        /// Returns the base portion of the URL
        /// </summary>
        public string URL { get; set; }

        #endregion
    }
}
