using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Bundle : IEntity
    {
        public Bundle()
        {
            country_prices = new List<CountryPrice>();
            subscriptions = new List<Subscription>();
        }
        #region Properties
        [DataMember]
        public int bundle_id { get; set; }
        [DataMember]
        [StringLength(250, ErrorMessage = "The value cannot exceed 250 characters.")]
        public string name { get; set; }
        [DataMember]
        public double price { get; set; }
        [DataMember]
        public bool active { get; set; }
        [DataMember]
        [StringLength(25, ErrorMessage = "The value cannot exceed 25 characters.")]
        public string promo_code { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public Enums.SubscriptionType type { get; set; }
        [DataMember]
        public List<Subscription> subscriptions { get; set; }
        [DataMember]
        public List<CountryPrice> country_prices { get; set; }
        [DataMember]
        public int publication_id { get; set; }
        [DataMember]
        public int issues { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
