using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class PaidOrderDetail
    {
        public PaidOrderDetail()
        {
            PaidOrderDetailId = 0;
            PaidOrderId = 0;
            ProductSubscriptionId = 0;
            ProductId = 0;
            RefundDate = null;
            FulfilledDate = null;
            SubTotal = 0;
            TaxTotal = 0;
            GrandTotal = 0;
            SubGenBundleId = 0;
            SubGenOrderItemId = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserId = 0;
            UpdatedByUserId = null;
        }
        #region Properties
        [DataMember]
        public int PaidOrderDetailId { get; set; }
        [DataMember]
        public int PaidOrderId { get; set; }
        [DataMember]
        public int ProductSubscriptionId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public DateTime? RefundDate { get; set; }
        [DataMember]
        public DateTime? FulfilledDate { get; set; }
        [DataMember]
        public double SubTotal { get; set; }
        [DataMember]
        public double TaxTotal { get; set; }
        [DataMember]
        public double GrandTotal { get; set; }
        [DataMember]
        public int SubGenBundleId { get; set; }
        [DataMember]
        public int SubGenOrderItemId { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public int? UpdatedByUserId { get; set; }
        #endregion
    }
}
