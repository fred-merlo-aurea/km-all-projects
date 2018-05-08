using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class CustomerPlan
    {
        public CustomerPlan() 
        { 
        }
        
        public int PlanID { get; set; }
        public int CustomerID { get; set; }
        public int QuoteOptionID { get; set; }
        public DateTime ActivationDate { get; set; }
        public string CardOwnerName { get; set; }
        private string _cardNumber;
        public string CardNumber
        {
            get
            {
                //KM.Common.Entity.Encryption ec = new KM.Common.Entity.Encryption();
                KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                return KM.Common.Encryption.Encrypt(_cardNumber, ec);
            }
            set
            {
                //KM.Common.Entity.Encryption ec = new KM.Common.Entity.Encryption();
                KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                _cardNumber = KM.Common.Encryption.Encrypt(value, ec);
            }
        }
        public ECN_Framework_Common.Objects.Accounts.Enums.CardType CardType { get; set; }
        public string CardExpiration { get; set; }
        public string CardVerificationNumber { get; set; }
        public ECN_Framework_Common.Objects.Accounts.Enums.SubscriptionType SubscriptionType { get; set; }
        public bool IsPhoneSupportIncluded { get; set; }
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
