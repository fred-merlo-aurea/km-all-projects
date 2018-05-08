using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data;

namespace ECN_Framework_Entities.Salesforce
{
    [DataContract]
    public class SF_Token
    {
        /// <summary>
        /// access_token     ---   Access token that acts as a session ID that the application
        ///                          uses for making requests. This token should be protected as though it were user credentials.
        /// refresh_token    ---   Token that can be used in the future to obtain new access tokens.
        ///                          Warning: This value is a secret. You should treat it like the user's password and use appropriate measures to protect it.
        /// instance_url     ---   Identifies the Salesforce instance to which API calls should be sent.
        /// id               ---   Identity URL that can be used to both identify the user as well as query for more information about the user. Can be
        ///                          used in an HTTP request to get more information about the end user.
        /// issued_at        ---   When the signature was created, represented as the number of seconds since the Unix epoch (00:00:00 UTC on 1 January 1970).
        /// signature        ---   Base64-encoded HMAC-SHA256 signature signed with the consumer's private key containing the concatenated ID
        ///                          and issued_at value. The signature can be used to verify that the identity URL wasn’t modified because it was sent by the server.
        /// </summary>
        public SF_Token() { }
        #region Properties
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string refresh_token { get; set; }
        [DataMember]
        public string instance_url { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string issued_at { get; set; }
        [DataMember]
        public string signature { get; set; }
        #endregion

    }
    
}