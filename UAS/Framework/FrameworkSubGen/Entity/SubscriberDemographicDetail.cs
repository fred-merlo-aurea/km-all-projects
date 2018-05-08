using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberDemographicDetail : IEntity
    {
        public SubscriberDemographicDetail() 
        {
            
        }
        #region Properties
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        public int option_id { get; set; }
        [DataMember]
        public int field_id { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public bool IsProcessed { get; set; }
        [DataMember]
        public DateTime ProcessedDate { get; set; }
        #endregion
    }
}
