using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SubscriberProductDemographic
    {
        public SubscriberProductDemographic() 
        {
            Name = string.Empty;
            Value = string.Empty;
            DemoUpdateAction = FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace;
        }
        public SubscriberProductDemographic(string name, string value, FrameworkUAD_Lookup.Enums.DemographicUpdate demoUpdateAction)
        {
            Name = name;
            Value = value;
            DemoUpdateAction = demoUpdateAction;
        }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
        [IgnoreDataMember]
        public FrameworkUAD_Lookup.Enums.DemographicUpdate DemoUpdateAction { get; set; }
    }
}
