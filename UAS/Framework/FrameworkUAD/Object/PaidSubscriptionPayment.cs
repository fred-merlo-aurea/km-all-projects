using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class PaidSubscriptionPayment
    {
        public PaidSubscriptionPayment() 
        {
            Amount = 0;
            Note = string.Empty;
            TransactionId = string.Empty;
            PaymentType = FrameworkUAD_Lookup.Enums.PaymentType.Credit_Card;
        }
        #region Properties
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string TransactionId { get; set; }	
        [DataMember]
        public FrameworkUAD_Lookup.Enums.PaymentType PaymentType { get; set; }
        #endregion
    }
}
