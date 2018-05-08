using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class DomainTracking
    {
        public DomainTracking() 
        {
            DomainTrackingID = 0;
            DomainName = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember]
        public int DomainTrackingID { get; set; }
        [DataMember]
        public string DomainName { get; set; }
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
