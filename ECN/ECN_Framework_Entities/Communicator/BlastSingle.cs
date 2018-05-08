using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class BlastSingle
    {
        public BlastSingle()
        {
            BlastSingleID = -1;
            BlastID = null;
            EmailID = null;
            SendTime = null;
            Processed = string.Empty;
            LayoutPlanID = null;
            RefBlastID = null;
            CustomerID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int BlastSingleID { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public int? EmailID { get; set; }
        [DataMember]
        public DateTime? SendTime { get; set; }
        [DataMember]
        public string Processed { get; set; }
        [DataMember]
        public int? LayoutPlanID { get; set; }
        [DataMember]
        public int? RefBlastID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }

        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }
    }
}

