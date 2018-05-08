using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Order : IEntity
    {
        public Order()
        {
            order_items = new List<OrderItem>();
        }
        #region Properties
        [DataMember]
        public int order_id { get; set; }//yes
        [DataMember]
        public int mailing_address_id { get; set; }//yes
        [DataMember]
        public int billing_address_id { get; set; }//yes
        [DataMember]
        public int subscriber_id { get; set; }//yes
        [DataMember]//30 char limit
        [StringLength(30, ErrorMessage = "The value cannot exceed 30 characters.")]
        public string import_name { get; set; }//yes
        [DataMember]
        public int user_id { get; set; }//yes
        [DataMember]
        public int channel_id { get; set; }//yes
        [DataMember]
        public DateTime order_date { get; set; }//yes
        [DataMember]
        public bool is_gift { get; set; }//yes
        [DataMember]
        public double sub_total { get; set; }//yes
        [DataMember]
        public double tax_total { get; set; }//yes
        [DataMember]
        public double grand_total { get; set; }//yes
        [DataMember]
        public List<OrderItem> order_items { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
