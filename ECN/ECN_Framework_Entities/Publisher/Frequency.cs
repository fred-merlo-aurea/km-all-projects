using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class Frequency
    {
        public Frequency()
        {
            FrequencyID = -1;
            FrequencyName = string.Empty;
            IsDeleted = false;
        }

        [DataMember]
        public int FrequencyID { get; set; }
        [DataMember]
        public string FrequencyName { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
