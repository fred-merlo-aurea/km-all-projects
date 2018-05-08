using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkSubGen.Object
{
    [Serializable]
    [DataContract]
    public class SubscriberField
    {
        public SubscriberField()
        {
            field_id = 0;
            text_value = string.Empty;
            option_ids = new List<int>();
        }
        #region Properties
        [DataMember]
        public int field_id { get; set; }
        [DataMember]
        public string text_value { get; set; }
        [DataMember]
        public List<int> option_ids { get; set; }
        #endregion
    }
}
