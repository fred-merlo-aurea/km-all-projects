using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class UserLog
    {
        public UserLog() { }
        public UserLog(int userID, int applicationID,string obj,int userLogTypeID,string fromValues = "", string toValues="", int clientID = 0, string groupTransactionCode = "")
        {
            KMPlatform.Entity.UserLog log = new KMPlatform.Entity.UserLog();
            log.ApplicationID = applicationID;
            log.DateCreated = DateTime.Now;
            log.FromObjectValues = fromValues;
            log.Object = obj;
            log.ToObjectValues = toValues;
            log.UserID = userID;
            log.UserLogTypeID = userLogTypeID;
            log.ClientID = clientID;
            log.GroupTransactionCode = groupTransactionCode;
        }
        #region Properties
        [DataMember]
        public int UserLogID { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public int UserLogTypeID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string Object { get; set; }
        [DataMember]
        public string FromObjectValues { get; set; }
        [DataMember]
        public string ToObjectValues { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int? ClientID { get; set; }
        [DataMember]
        public string GroupTransactionCode { get; set; }
        #endregion
    }
}
