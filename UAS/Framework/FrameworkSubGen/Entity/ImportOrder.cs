using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class ImportOrder
    {
        public ImportOrder()
        {
            OrderDate = null;
            FulfilledDate = null;
            RefundedDate = null;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            IsMergedToUAD = false;
            DateMergedToUAD = null;
        }
        #region Properties
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int OrderItemID { get; set; }
        [DataMember]
        public int SubscriberID { get; set; }
        [DataMember]
        public string SubscriberFirstName { get; set; }
        [DataMember]
        public string SubscriberLastName { get; set; }
        [DataMember]
        public string ShippingFirstName { get; set; }
        [DataMember]
        public string ShippingLastName { get; set; }
        [DataMember]
        public string ShippingAddressLine1 { get; set; }
        [DataMember]
        public string ShippingCity { get; set; }
        [DataMember]
        public string ShippingState { get; set; }
        [DataMember]
        public string ShippingPostalCode { get; set; }
        [DataMember]
        public string ShippingCountry { get; set; }
        [DataMember]
        public string BillingFirstName { get; set; }
        [DataMember]
        public string BillingLastName { get; set; }
        [DataMember]
        public string BillingAddressLine1 { get; set; }
        [DataMember]
        public string BillingCity { get; set; }
        [DataMember]
        public string BillingState { get; set; }
        [DataMember]
        public string BillingPostalCode { get; set; }
        [DataMember]
        public string BillingCountry { get; set; }
        [DataMember]
        public string CreatedbyRep { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string PublicationName { get; set; }
        [DataMember]
        public int PublicationProductCode { get; set; }
        [DataMember]
        public int PublicationRevenueCode { get; set; }
        [DataMember]
        public string SubscriptionOfferName { get; set; }
        [DataMember]
        public bool StarterIssues { get; set; }
        [DataMember]
        public int TotalIssues { get; set; }
        [DataMember]
        public int Copies { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int ParentOrderItemID { get; set; }
        [DataMember]
        public DateTime? FulfilledDate { get; set; }
        [DataMember]
        public DateTime? RefundedDate { get; set; }
        [DataMember]
        public double SubTotal { get; set; }
        [DataMember]
        public double TaxTotal { get; set; }
        [DataMember]
        public double GrandTotal { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int account_id { get; set; }

        //columns for sending to UAD
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public bool IsMergedToUAD { get; set; }
        [DataMember]
        public DateTime? DateMergedToUAD { get; set; }

        #endregion
    }
}
