using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Blast
    {
        public Blast()
        {
            BlastID = 0;
            CustomerID = 0;
            EmailSubject = string.Empty;
            EmailFrom = string.Empty;
            EmailFromName = string.Empty;
            StatusCode = string.Empty;
            BlastType = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string EmailFrom { get; set; }
        [DataMember]
        public string EmailFromName { get; set; }
        [DataMember]
        public DateTime? SendTime { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string BlastType { get; set; }
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
