using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]    
    public class CampaignItemBlastFilter
    {
        public CampaignItemBlastFilter() { }

        #region properties
        [DataMember]
        public int CampaignItemBlastFilterID { get; set; }
        [DataMember]
        public int? CampaignItemBlastID { get; set; }
        [DataMember]
        public int? CampaignItemSuppresionID { get; set; }
        [DataMember]
        public int? SuppressionGroupID { get; set; }
        [DataMember]
        public int? CampaignItemTestBlastID { get; set; }
        [DataMember]
        public int? SmartSegmentID { get; set; }
        [DataMember]
        public string RefBlastIDs { get; set; }
        [DataMember]
        public int? FilterID { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }

        #endregion
    }
}
