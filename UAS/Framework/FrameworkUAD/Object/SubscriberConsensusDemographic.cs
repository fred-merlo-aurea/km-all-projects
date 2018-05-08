using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SubscriberConsensusDemographic
    {
        public SubscriberConsensusDemographic() 
        {
            Name = string.Empty;
            DisplayName = string.Empty;
            Value = string.Empty;
        }
        public SubscriberConsensusDemographic(string name, string displayname, string value)
        {
            Name = name;
            DisplayName = displayname;
            Value = value;
        }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
