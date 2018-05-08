using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator.View
{
    [Serializable]
    [DataContract]
    public class LayoutPlanSummary
    {
        public LayoutPlanSummary(int id, string name, int triggerCount)
        {
            ID = id;
            Name = name;
            TriggerCount = triggerCount;
        }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int TriggerCount { get; set; }
    }
}

