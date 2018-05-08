using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class HistoryPaid
    {
        public HistoryPaid() { }
        #region Properties
        [DataMember]
        public int HistoryPaidID { get; set; }
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
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        #endregion
    }
}
