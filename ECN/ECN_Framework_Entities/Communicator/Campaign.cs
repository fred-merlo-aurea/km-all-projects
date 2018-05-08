using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class Campaign
    {
        public Campaign()
        {
            CampaignID = -1;
            CustomerID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CampaignName = string.Empty;
            DripDesign = string.Empty;
            ItemList = null;
            IsArchived = false;
        }

        [DataMember]
        public int CampaignID { get; set; }
        [DataMember]
        public string DripDesign { get; set; }
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
        public string CampaignName { get; set; }
        [DataMember]
        public bool? IsArchived { get; set; }
        //optional
        public List<CampaignItem> ItemList { get; set; }
    }
}
