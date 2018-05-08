using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberDemographic : IEntity
    {
        public SubscriberDemographic() 
        {
            subDemoDetails = new List<SubscriberDemographicDetail>();
        }
        #region Properties
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public int field_id { get; set; }
        [DataMember]
        public string text_value { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public bool IsProcessed { get; set; }
        [DataMember]
        public DateTime ProcessedDate { get; set; }
        [DataMember]
        public List<FrameworkSubGen.Entity.SubscriberDemographicDetail> subDemoDetails { get; set; }
        #endregion
    }
}
