using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberAddKill
    {
        public SubscriberAddKill()
        {
            AddKillID = 0;
            PublicationID = 0;
            Count = 0;
            AddKillCount = 0;
            Type = String.Empty;
            IsActive = true;
            DateCreated = DateTime.Now;
        }

        #region Properties
        [DataMember]
        public int AddKillID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public int AddKillCount { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        #endregion
    }
}
