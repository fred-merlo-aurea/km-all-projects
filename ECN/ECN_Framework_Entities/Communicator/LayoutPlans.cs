using System;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class LayoutPlans : IUserValidate
    {
        public LayoutPlans()
        {
            LayoutPlanID = -1;
            LayoutID = null;
            EventType = string.Empty;
            CustomerID = null;
            BlastID = null;
            GroupID = null;
            Period = null;
            Criteria = string.Empty;
            ActionName = string.Empty;
            Status = string.Empty;
            SmartFormID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            TokenUID = null;
            CampaignItemID = null;
        }

        public bool HasValidID
        {
            get { return LayoutPlanID > 0; }
        }
        [DataMember]
        public int LayoutPlanID { get; set; }
        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public string EventType { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public decimal? Period { get; set; }
        [DataMember]
        public string Criteria { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string ActionName { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int? SmartFormID { get; set; }
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
        public int? CampaignItemID { get; set; }
        [DataMember]
        public Guid? TokenUID { get; set; }
    
    }
}
