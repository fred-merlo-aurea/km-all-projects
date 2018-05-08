using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class AcsShippingDetail
    {
        public AcsShippingDetail() 
        {
            AcsShippingDetailId = 0;
            CustomerNumber = 0;
            AcsDate = DateTime.Now;
            ShipmentNumber = 0;
            AcsTypeId = 0;
            AcsId = 0;
            AcsName = string.Empty;
            ProductCode = string.Empty;
            Description = string.Empty;
            Quantity = 1;
            UnitCost = 0;
            TotalCost = 0;
            DateCreated = DateTime.Now;
            IsBilled = false;
            ProcessCode = string.Empty;
        }
        #region Properties
        [DataMember]
        public int AcsShippingDetailId { get; set; }
        [DataMember]
        public int CustomerNumber { get; set; }
        [DataMember]
        public DateTime AcsDate { get; set; }
        [DataMember]
        public Int64 ShipmentNumber { get; set; }
        [DataMember]
        public int AcsTypeId { get; set; }
        [DataMember]
        public int AcsId { get; set; }
        [DataMember]
        public string AcsName { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal UnitCost { get; set; }
        [DataMember]
        public decimal TotalCost { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public bool IsBilled { get; set; }
        [DataMember]
        public DateTime? BilledDate { get; set; }
        [DataMember]
        public int? BilledByUserID { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        #endregion
    }
}
