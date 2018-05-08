using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class OrderItem : IEntity
    {
        public OrderItem()
        {
            refund_date = DateTimeFunctions.GetMinDate();
            fulfilled_date = DateTimeFunctions.GetMinDate();
        }
        #region Properties
        [DataMember]
        public int order_item_id { get; set; }
        [DataMember]
        public int bundle_id { get; set; }
        [DataMember]
        public DateTime refund_date { get; set; }
        [DataMember]
        public DateTime fulfilled_date { get; set; }
        [DataMember]
        public double sub_total { get; set; }
        [DataMember]
        public double tax_total { get; set; }
        [DataMember]
        public double grand_total { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
