using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_Entities.FormDesigner
{
    [Serializable]
    [DataContract]
    public class ConditionGroup
    {
        public ConditionGroup() {
            Conditions = new List<Condition>();
            ConditionGroup1 = new List<ConditionGroup>();
            ConditionGroup_Seq_ID = -1;
        }

        public int ConditionGroup_Seq_ID { get; set; }

        public int? MainGroup_ID { get; set; }

        public bool LogicGroup { get; set; }

        public List<Condition> Conditions { get; set; }
        public List<ConditionGroup> ConditionGroup1 { get; set; }
    }
}
