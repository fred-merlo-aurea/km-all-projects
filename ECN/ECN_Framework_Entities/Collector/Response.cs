using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_Entities.Collector
{
    [Serializable]
    [DataContract]
    public class Response
    {
        public Response()
        {
            ID = -1;
            Value = string.Empty;
        }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
