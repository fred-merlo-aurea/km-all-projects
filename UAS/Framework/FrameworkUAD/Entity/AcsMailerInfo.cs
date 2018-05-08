using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    public class AcsMailerInfo
    {
        public AcsMailerInfo()
        {
            AcsMailerInfoId = 0;
            AcsCode = string.Empty;
            MailerID = 0;
            ImbSeqCounter = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
        }

        [DataMember]
        public int AcsMailerInfoId { get; set; }
        [DataMember]
        public string AcsCode { get; set; }
        [DataMember]
        public int MailerID { get; set; }
        [DataMember]
        public int ImbSeqCounter { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
    }
}
