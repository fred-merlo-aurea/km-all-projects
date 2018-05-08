using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Object
{
    [Serializable]
    [DataContract]
    public class CreateOrderLineItem
    {
        public CreateOrderLineItem()
        {
            bundle_id = 0;
            price = 0;
        }
        public CreateOrderLineItem(int liBundleId, double liPrice)
        {
            bundle_id = liBundleId;
            price = liPrice;
        }
        #region Properties
        [DataMember]
        public int bundle_id { get; set; }
        [DataMember]
        public double price { get; set; }
        #endregion
    }
}
