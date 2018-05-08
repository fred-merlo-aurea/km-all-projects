using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class OrderTotalLineItem : IEntity
    {
        public OrderTotalLineItem() { }
        #region Properties
        [DataMember]
        public int product_id { get; set; }
        [DataMember]
        public int bundle_id { get; set; }
        [DataMember]
        public double price { get; set; }
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
