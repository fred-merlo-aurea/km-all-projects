using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class CampaignFilter
    {
        public CampaignFilter()
        {

        }
        #region Properties
        public int CampaignFilterID { get; set; }
        public int CampaignID { get; set; }
        public string FilterName { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Count { get; set; }
        public string PromoCode { get; set; }
        #endregion
    }
}
