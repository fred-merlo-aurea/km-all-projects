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
    public class Rule
    {
        public Rule() {
            OverwritePostValue = new List<FormDesigner.OverwriteDataPost>();
            RequestQueryValue = new List<FormDesigner.RequestQueryValue>();
            Rule_Seq_ID = -1;
        }

        public int Rule_Seq_ID { get; set; }

        public int? Form_Seq_ID { get; set; }

        public int? Control_ID { get; set; }

        public int ConditionGroup_Seq_ID { get; set; }

        public int Type { get; set; }

        public string Action { get; set; }

        public string ActionJs { get; set; }

        public string UrlToRedirect { get; set; }

        public int Order { get; set; }

        public int ResultType { get; set; }

        public int NonQualify { get; set; }

        public int SuspendpostDB { get; set; }

        public int Overwritedatapost { get; set; }

        public List<OverwriteDataPost> OverwritePostValue { get; set; }

        public ConditionGroup ConditionGroup { get; set; }

        public List<RequestQueryValue> RequestQueryValue { get; set; }
    }
}
