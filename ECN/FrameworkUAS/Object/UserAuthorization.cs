using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace KMPlatform.Object
{
    /// <summary>
    /// This class contains items that are enabled/active for the user
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserAuthorization
    {
        public UserAuthorization() 
        {
            AuthSource = string.Empty;
            AuthAttemptDate = DateTimeFunctions.GetMinDate();
            AuthAttemptTime = DateTimeFunctions.GetMinTime();
            IsAuthenticated = false;
            IpAddress = string.Empty;
            AuthUserName = string.Empty;
            AuthPassword = string.Empty;
            AuthAccessKey = Guid.Empty;
            SubGenLoginToken = string.Empty;
            AuthorizationMode = BusinessLogic.Enums.AuthorizationModeTypes.Access_Key;
            IsKmStaff = false;
            //ServerVariables = new ServerVariable();
            User = new Entity.User();
            //UserAuthLog = new Entity.UserAuthorizationLog();
            UserAuthLogId = 0;
        }
        #region Properties
        [DataMember]
        public string AuthSource { get; set; }
        [DataMember]
        public DateTime AuthAttemptDate { get; set; }
        [DataMember]
        public TimeSpan AuthAttemptTime { get; set; }
        [DataMember]
        public bool IsAuthenticated { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public string AuthUserName { get; set; }
        [DataMember]
        public string AuthPassword { get; set; }
        [DataMember]
        public Guid AuthAccessKey { get; set; }
        [DataMember]
        public string SubGenLoginToken { get; set; }
        [DataMember]
        public BusinessLogic.Enums.AuthorizationModeTypes AuthorizationMode { get; set; }
        [DataMember]
        public bool IsKmStaff { get; set; }
        //[DataMember]
        //public ServerVariable ServerVariables { get; set; }
        [DataMember]
        public Entity.User User { get; set; }
        //[DataMember]
        //public Entity.UserAuthorizationLog UserAuthLog { get; set; }
        [DataMember]
        public int UserAuthLogId { get; set; }
        #endregion
    }
}
