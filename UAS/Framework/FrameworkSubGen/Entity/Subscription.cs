using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Subscription : IEntity
    {
        public Subscription()
        {
            date_created = DateTimeFunctions.GetMinDate();
            date_expired = DateTimeFunctions.GetMinDate();
            date_last_renewed = DateTimeFunctions.GetMinDate();
        }
        #region Properties
        [DataMember]
        public int subscription_id { get; set; }
        [DataMember]
        public int publication_id { get; set; }
        [DataMember]
        public int mailing_address_id { get; set; }
        [DataMember]
        public int billing_address_id { get; set; }
        [DataMember]
        public int issues { get; set; }
        [DataMember]
        public int copies { get; set; }
        [DataMember]
        public int paid_issues_left { get; set; }
        [DataMember]
        public double unearned_revenue { get; set; }
        //[DataMember]
        //public int remaining_issues { get; set; }

        [DataMember]
        public Enums.SubscriptionType type { get; set; }
        [DataMember]
        public DateTime date_created { get; set; }
        [DataMember]
        public DateTime date_expired { get; set; }
        [DataMember]
        public DateTime date_last_renewed { get; set; }
        [DataMember]
        public int last_purchase_bundle_id { get; set; }
        //[DataMember]
        //public int last_purchase_id { get; set; }
        [DataMember]
        public bool renew_campaign_active { get; set; }
        [DataMember]
        public string audit_classification { get; set; }
        [DataMember]
        public string audit_request_type { get; set; }

        [DataMember]
        public int subscriber_id { get; set; }//only populated for CDC
        //[DataMember]
        //public List<Issue> issues { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
