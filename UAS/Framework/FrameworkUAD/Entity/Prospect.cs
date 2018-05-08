using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Prospect
    {
        public Prospect() { }
        #region Properties
        [DataMember]
        public int ProspectID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int SubscriberID { get; set; }
        [DataMember]
        public bool IsProspect { get; set; }
        [DataMember]
        public bool IsVerifiedProspect { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
