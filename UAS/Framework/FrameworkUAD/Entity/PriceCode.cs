using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class PriceCode
    {
        public PriceCode() { }
        #region Properties
        [DataMember]
        public int PriceCodeID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string PriceCodes { get; set; }
        [DataMember]
        public int Term { get; set; }
        [DataMember]
        public decimal US_CopyRate { get; set; }
        [DataMember]
        public decimal CAN_CopyRate { get; set; }
        [DataMember]
        public decimal FOR_CopyRate { get; set; }
        [DataMember]
        public decimal US_Price { get; set; }
        [DataMember]
        public decimal CAN_Price { get; set; }
        [DataMember]
        public decimal FOR_Price { get; set; }
        [DataMember]
        public string QFOfferCode { get; set; }
        [DataMember]
        public string FoxProPriceCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int DeliverabilityID { get; set; }
        [DataMember]
        public int TotalIssues { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
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
