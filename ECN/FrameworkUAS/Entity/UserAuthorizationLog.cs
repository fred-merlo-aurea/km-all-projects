using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class UserAuthorizationLog
    {
        public UserAuthorizationLog() 
        {
            UserAuthLogID = -1;
            AuthSource = string.Empty;
            AuthAttemptDate = DateTimeFunctions.GetMinDate();
            AuthAttemptTime = DateTimeFunctions.GetMinTime();
            IsAuthenticated = false;
            IpAddress = string.Empty;
            AuthUserName = string.Empty;
            ServerVariables = string.Empty;
            AppVersion = string.Empty;
        }
        #region Properties
        [DataMember]
        public int UserAuthLogID { get; set; }
        [DataMember]
        public string AuthSource { get; set; }
        [DataMember]
        public BusinessLogic.Enums.AuthorizationModeTypes AuthMode { get; set; }
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
        public Guid? AuthAccessKey { get; set; }
        [DataMember]
        public string ServerVariables { get; set; }
        [DataMember]
        public string AppVersion { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public DateTime? LogOutDate { get; set; }
        [DataMember]
        public TimeSpan? LogOutTime { get; set; }
        #endregion
    }
}
