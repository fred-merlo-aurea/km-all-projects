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
    public class Condition
    {
        public Condition() { }

        public int Condition_Seq_ID { get; set; }

        public int Control_ID { get; set; }

        public int ConditionGroup_Seq_ID { get; set; }

        public int Operation_ID { get; set; }

        public string Value { get; set; }
    }
}
