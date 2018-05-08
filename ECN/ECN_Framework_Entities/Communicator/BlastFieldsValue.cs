using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class BlastFieldsValue
    {
        public BlastFieldsValue()
        {
            BlastFieldsValueID = -1;
            BlastFieldID = -1;
            CustomerID = -1;
            Value = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int BlastFieldsValueID { get; set; }
        [DataMember]
        public int BlastFieldID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
