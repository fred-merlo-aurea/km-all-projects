using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class BlastPlans
    {
        public BlastPlans()
        {
            BlastPlanID = -1;
            BlastID = null;
            CustomerID = null;
            GroupID = null;
            EventType = string.Empty;
            Period = null;
            BlastDay = null;
            PlanType = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int BlastPlanID { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public string EventType { get; set; }
        [DataMember]
        public decimal? Period { get; set; }
        [DataMember]
        public int? BlastDay { get; set; }
        [DataMember]
        public string PlanType { get; set; }
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
    }
}
