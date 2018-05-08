using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class TriggerPlans
    {
        public TriggerPlans()
        {
            TriggerPlanID = -1;
            refTriggerID = null;
            CustomerID = null;
            GroupID = null;
            EventType = string.Empty;
            Period = null;
            BlastID = null;
            Criteria = string.Empty;
            ActionName = string.Empty;
            Status = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int TriggerPlanID { get; set; }
        [DataMember]
        public int? refTriggerID { get; set; }
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
        public string Criteria { get; set; }
        [DataMember]
        public string ActionName { get; set; }
        [DataMember]
        public string Status { get; set; }
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
