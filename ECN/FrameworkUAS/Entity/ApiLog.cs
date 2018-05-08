using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ApiLog
    {
        public ApiLog() 
        {
            ApiLogId = 0;
            ClientID = 0;
            AccessKey = Guid.Empty;
            RequestFromIP = string.Empty;
            ApiId = 0;
            Entity = string.Empty;
            Method = string.Empty;
            ErrorMessage = string.Empty;
            RequestData = string.Empty;
            ResponseData = string.Empty;
            RequestStartDate = DateTime.Now;
            RequestStartTime = DateTime.Now.TimeOfDay;
            RequestEndDate = null;
            RequestEndTime = null;
        }
        #region Properties
        [DataMember]
        public int ApiLogId { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public Guid AccessKey { get; set; }        
        [DataMember]
        public string RequestFromIP { get; set; }
        [DataMember]
        public int ApiId { get; set; }
        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public string Method { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string RequestData { get; set; }
        [DataMember]
        public string ResponseData { get; set; }
        [DataMember]
        public DateTime RequestStartDate { get; set; }
        [DataMember]
        public TimeSpan RequestStartTime { get; set; }
        [DataMember]
        public DateTime? RequestEndDate { get; set; }
        [DataMember]
        public TimeSpan? RequestEndTime { get; set; }
        #endregion
    }
}
