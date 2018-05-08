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
    public class RequestQueryValue
    {
        public RequestQueryValue() { }

        public int RequestQueryValue_Seq_ID { get; set; }

        public int Control_ID { get; set; }

        public int Rule_Seq_ID { get; set; }

        public string Name { get; set; }
    }
}
