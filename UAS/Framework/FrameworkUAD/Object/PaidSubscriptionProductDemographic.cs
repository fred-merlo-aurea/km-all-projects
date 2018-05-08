using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class PaidSubscriptionProductDemographic
    {
        public PaidSubscriptionProductDemographic() 
        {
            ProductCode = string.Empty; 
            DemographicName = string.Empty;
            OtherTextValue = string.Empty;
            Values = new List<string>();
        }
        #region Properties
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string DemographicName { get; set; }
        [DataMember]
        public string OtherTextValue { get; set; }
        [DataMember]
        public List<string> Values { get; set; }
        #endregion
    }
}
