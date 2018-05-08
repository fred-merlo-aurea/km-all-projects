using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Campaign
    {                                                      
        public Campaign() 
        {
            CampaignID = 0;
            CampaignName = string.Empty;                                   
            AddedBy = 0;
            DateAdded = DateTime.Now;
            UpdatedBy = 0;
            DateUpdated = null;
            BrandID =null;
        }
        #region Properties
        [DataMember]
        public int CampaignID { get; set; }
        [DataMember]
        public string CampaignName { get; set; }
        [DataMember]
        public int AddedBy { get; set; }
        [DataMember]
        public DateTime DateAdded { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int? BrandID { get; set; }                
        #endregion
    }
}
