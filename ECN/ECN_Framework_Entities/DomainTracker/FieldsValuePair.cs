using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.DomainTracker
{
    [Serializable]
    [DataContract]
    public class FieldsValuePair
    {
        [DataMember]
        public int DomainTrackerActivityID { get; set; }
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ReferralURL { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}
