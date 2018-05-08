using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkSubGen.Object
{
    [Serializable]
    [DataContract]
    public class CreateOrder
    {
        public CreateOrder()
        {
            billing_address_id = 0;
            mailing_address_id = 0;
            subscriber_id = 0;
            order_date = DateTime.Now.ToString("yyyy-MM-dd");
            is_gift = false;
            payment = new Entity.Payment();
            line_items = new List<CreateOrderLineItem>();
        }
        #region Properties
        [DataMember]
        public int billing_address_id { get; set; }
        [DataMember]
        public int mailing_address_id { get; set; }
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        public string order_date { get; set; }
        [DataMember]
        public bool is_gift { get; set; }
        [DataMember]
        public Entity.Payment payment { get; set; }
        [DataMember]
        public List<CreateOrderLineItem> line_items { get; set; }
        #endregion
    }
}
