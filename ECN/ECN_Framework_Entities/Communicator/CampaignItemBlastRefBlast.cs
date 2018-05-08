using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemBlastRefBlast
    {
        public CampaignItemBlastRefBlast()
        {
            CampaignItemBlastRefBlastID = 0;
            CampaignItemBlastID = null; 
            RefBlastID = null;            
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int CampaignItemBlastRefBlastID { get; set; }
        [DataMember]
        public int? CampaignItemBlastID { get; set; }
        [DataMember]
        public int? RefBlastID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }

    }
}
