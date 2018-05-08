using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class OrderTotal
    {
        public OrderTotal()
        {
            line_items = new List<OrderTotalLineItem>();
        }
        #region Properties
        [DataMember]
        public double sub_total { get; set; }
        [DataMember]
        public double tax_total { get; set; }
        [DataMember]
        public double grand_total { get; set; }
        [DataMember]
        public List<OrderTotalLineItem> line_items { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
