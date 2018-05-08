using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class BatchHistoryDetail
    {
        public BatchHistoryDetail() { }
        #region Properties
        [DataMember]
        public int BatchID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int BatchCount { get; set; }
        [DataMember]
        public int BatchNumber { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime BatchDateCreated { get; set; }
        [DataMember]
        public DateTime? BatchDateFinalized { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string PublicationName { get; set; }
        [DataMember]
        public string PubCode { get; set; }
        [DataMember]
        public int PublisherID { get; set; }
        [DataMember]
        public string PublisherName { get; set; }
        [DataMember]
        public int BatchCountItem { get; set; }
        [DataMember]
        public int SubscriberID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int HistorySubscriptionID { get; set; }
        [DataMember]
        public DateTime HistoryDateCreated { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        //public int TaskID { get; set; }
        //public string TaskName { get; set; }
        [DataMember]
        public int UserLogTypeID { get; set; }
        [DataMember]
        public string UserLogTypeName { get; set; }
        [DataMember]
        public string Object { get; set; }
        [DataMember]
        public string FromObjectValues { get; set; }
        [DataMember]
        public string ToObjectValues { get; set; }
        [DataMember]
        public DateTime? UserLogDateCreated { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public int UserLogID { get; set; }
        #endregion
    }
}
