using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class PaidOrder
    {
        public PaidOrder() 
        {
            PaidOrderId = 0;
            SubscriptionId = 0;
            ImportName = string.Empty;
            OrderDate = DateTime.Now;
            IsGift = false;
            SubTotal = 0;
            TaxTotal = 0;
            GrandTotal = 0;
            PaymentAmount = 0;
            PaymentNote = string.Empty;
            PaymentTransactionId = string.Empty;
            PaymentTypeCodeId = 0;
            UserId = 0;
            SubGenOrderId = 0;
            SubGenSubscriberId = 0;
            SubGenUserId = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserId = 0;
            UpdatedByUserId = null;
            PaidOrderDetails = new List<PaidOrderDetail>();
        }

        #region Properties
        [DataMember]
        public int PaidOrderId {get;set;}
        [DataMember]
	    public int SubscriptionId {get;set;}
        [DataMember]
	    public string ImportName {get;set;}
        [DataMember]
	    public DateTime OrderDate {get;set;}
        [DataMember]
	    public bool IsGift {get;set;}
        [DataMember]
	    public double SubTotal {get;set;}
        [DataMember]
	    public double TaxTotal {get;set;}
        [DataMember]
	    public double GrandTotal {get;set;}
        [DataMember]
	    public double PaymentAmount {get;set;}
        [DataMember]
	    public string PaymentNote {get;set;}
        [DataMember]
	    public string PaymentTransactionId {get;set;}
        [DataMember]
	    public int PaymentTypeCodeId {get;set;}
        [DataMember]
	    public int UserId  {get;set;}
        [DataMember]
	    public int SubGenOrderId  {get;set;}
        [DataMember]
	    public int SubGenSubscriberId  {get;set;}
        [DataMember]
	    public int SubGenUserId {get;set;}
        [DataMember]
	    public DateTime DateCreated {get;set;}
        [DataMember]
	    public DateTime? DateUpdated {get;set;}
        [DataMember]
	    public int CreatedByUserId {get;set;}
        [DataMember]
        public int? UpdatedByUserId { get; set; }
        #endregion

        [DataMember]
        public FrameworkUAD_Lookup.Enums.PaymentType PaymentType { get; set; }
        [DataMember]
        public List<PaidOrderDetail> PaidOrderDetails { get; set; }
    }
}
