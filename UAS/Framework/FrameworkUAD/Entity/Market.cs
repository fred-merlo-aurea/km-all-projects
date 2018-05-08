using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Market
    {
        public Market()
        {
            MarketID = 0;
            MarketName = string.Empty;
            MarketXML = string.Empty;
            BrandID = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember]
        public int MarketID { get; set; }
        [DataMember]
        public string MarketName { get; set; }
        [DataMember]
        public string MarketXML { get; set; }
        [DataMember]
        public int BrandID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
