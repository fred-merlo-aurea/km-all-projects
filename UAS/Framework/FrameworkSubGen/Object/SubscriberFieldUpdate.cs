using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkSubGen.Object
{
    [Serializable]
    [DataContract]
    public class SubscriberFieldUpdate
    {
        public SubscriberFieldUpdate()
        {
            subscriber_id = 0;
            fields = new List<SubscriberField>();
        }
        #region Properties
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        public List<SubscriberField> fields { get; set; }
        #endregion
    }
}
