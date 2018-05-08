using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriptionPaid
    {
        public SubscriptionPaid()
        {
            this.SubscriptionPaidID = 0;
            this.PubSubscriptionID = 0;
            this.PriceCodeID = 0;
            this.StartIssueDate = DateTime.Now;
            this.ExpireIssueDate = DateTime.Now;
            this.CPRate = 0;
            this.Amount = 0;
            this.AmountPaid = 0;
            this.BalanceDue = 0;
            this.PaidDate = DateTime.Now;
            this.TotalIssues = 0;
            this.CheckNumber = "";
            this.CCNumber = "";
            this.CCExpirationMonth = "";
            this.CCExpirationYear = "";
            this.CCHolderName = "";
            this.CreditCardTypeID = 0;
            this.PaymentTypeID = 0;
            this.DeliverID = 0;
            this.GraceIssues = 0;
            this.WriteOffAmount = 0;
            this.OtherType = "";
            this.DateCreated = DateTime.Now;
            this.Frequency = 0;
            this.Term = 0;      
        }
        public SubscriptionPaid(SubscriptionPaid sp)
        {
            this.SubscriptionPaidID = sp.SubscriptionPaidID;          
            this.PubSubscriptionID = sp.PubSubscriptionID;            
            this.PriceCodeID = sp.PriceCodeID;   
            this.StartIssueDate = sp.StartIssueDate;
            this.ExpireIssueDate = sp.ExpireIssueDate;
            this.CPRate = sp.CPRate;
            this.Amount = sp.Amount;
            this.AmountPaid = sp.AmountPaid;   
            this.BalanceDue = sp.BalanceDue;
            this.PaidDate = sp.PaidDate;
            this.TotalIssues = sp.TotalIssues;
            this.CheckNumber = sp.CheckNumber;
            this.CCNumber = sp.CCNumber;
            this.CCExpirationMonth = sp.CCExpirationMonth;     
            this.CCExpirationYear = sp.CCExpirationYear;
            this.CCHolderName = sp.CCHolderName;
            this.CreditCardTypeID = sp.CreditCardTypeID;
            this.PaymentTypeID  = sp.PaymentTypeID;
            this.DeliverID  = sp.DeliverID;
            this.GraceIssues = sp.GraceIssues;
            this.WriteOffAmount = sp.WriteOffAmount;
            this.OtherType  = sp.OtherType;
            this.DateCreated = sp.DateCreated;
            this.DateUpdated = sp.DateUpdated;
            this.CreatedByUserID = sp.CreatedByUserID;
            this.UpdatedByUserID = sp.UpdatedByUserID;
            this.Frequency = sp.Frequency;
            this.Term = sp.Term;       
        }

        #region Properties
        [DataMember]
        public int SubscriptionPaidID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int PriceCodeID { get; set; }
        [DataMember]
        public DateTime StartIssueDate { get; set; }
        [DataMember]
        public DateTime ExpireIssueDate { get; set; }
        [DataMember]
        public decimal CPRate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal AmountPaid { get; set; }
        [DataMember]
        public decimal BalanceDue { get; set; }
        [DataMember]
        public DateTime PaidDate { get; set; }
        [DataMember]
        public int TotalIssues { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public string CCNumber { get; set; }
        [DataMember]
        public string CCExpirationMonth { get; set; }
        [DataMember]
        public string CCExpirationYear { get; set; }
        [DataMember]
        public string CCHolderName { get; set; }
        [DataMember]
        public int CreditCardTypeID { get; set; }
        [DataMember]
        public int PaymentTypeID { get; set; }
        [DataMember]
        public int DeliverID { get; set; }
        [DataMember]
        public int GraceIssues { get; set; }
        [DataMember]
        public decimal WriteOffAmount { get; set; }
        [DataMember]
        public string OtherType { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int Frequency { get; set; }
        [DataMember]
        public int Term { get; set; }

        #endregion
    }
}
