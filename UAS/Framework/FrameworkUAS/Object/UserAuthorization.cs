using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAS.Object
{
    /// <summary>
    /// This class contains items that are enabled/active for the user
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserAuthorization : KMPlatform.Object.UserAuthorization
    {
        public UserAuthorization() 
        {
            AuthSource = string.Empty;
            AuthAttemptDate = DateTimeFunctions.GetMinDate();
            AuthAttemptTime = DateTimeFunctions.GetMinTime();
            ClientAdditionalProperties = new Dictionary<int, Object.ClientAdditionalProperties>();
            IsAuthenticated = false;
            IpAddress = string.Empty;
            AuthUserName = string.Empty;
            AuthPassword = string.Empty;
            AuthAccessKey = Guid.Empty;
            SubGenLoginToken = string.Empty;
            AuthorizationMode = KMPlatform.BusinessLogic.Enums.AuthorizationModeTypes.Access_Key;
            IsKmUser = false;
            //ServerVariables = new KMPlatform.Object.ServerVariable();
            User = new KMPlatform.Entity.User();
            //UserAuthLog = new KMPlatform.Entity.UserAuthorizationLog();  
            UserAuthLogId = 0;
        }
        public UserAuthorization(KMPlatform.Object.UserAuthorization u)
        {
            AuthSource = u.AuthSource;
            AuthAttemptDate = u.AuthAttemptDate;
            AuthAttemptTime = u.AuthAttemptTime;
            ClientAdditionalProperties = new Dictionary<int, Object.ClientAdditionalProperties>();
            IsAuthenticated = u.IsAuthenticated;
            IpAddress = u.IpAddress;
            AuthUserName = u.AuthUserName;
            AuthPassword = u.AuthPassword;
            AuthAccessKey = u.AuthAccessKey;
            SubGenLoginToken = u.SubGenLoginToken;
            AuthorizationMode = u.AuthorizationMode;
            IsKmUser = u.IsKmStaff;// (u.IsKmStaff ?? false);
            //ServerVariables = u.ServerVariables;
            User = u.User;
            //UserAuthLog = u.UserAuthLog;
            UserAuthLogId = u.UserAuthLogId;
        }
        #region Properties
        [DataMember]
        public bool IsKmUser { get; set; }
        [DataMember]
        public Dictionary<int, ClientAdditionalProperties> ClientAdditionalProperties { get; set; }
        #endregion

        public KMPlatform.Object.UserAuthorization GetPlatformUserEntity()
        {
            KMPlatform.Object.UserAuthorization u = new KMPlatform.Object.UserAuthorization();
            u.AuthAccessKey = this.AuthAccessKey;
            u.AuthAttemptDate = this.AuthAttemptDate;
            u.AuthAttemptTime = this.AuthAttemptTime;
            u.AuthorizationMode = this.AuthorizationMode;
            u.AuthPassword = this.AuthPassword;
            u.AuthSource = this.AuthSource;
            u.AuthUserName = this.AuthUserName;
            u.SubGenLoginToken = this.SubGenLoginToken;
            u.IpAddress = this.IpAddress;
            u.IsAuthenticated = this.IsAuthenticated;
            u.IsKmStaff = this.IsKmUser;
            //u.ServerVariables = this.ServerVariables;
            u.User = this.User;
            u.UserAuthLogId = this.UserAuthLogId;

            return u;
        }
    }
}
