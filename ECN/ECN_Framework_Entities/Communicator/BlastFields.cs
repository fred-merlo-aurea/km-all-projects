using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class BlastFields
    {
        public BlastFields()
        {
            BlastID = -1;
            Field1 = string.Empty;
            Field2 = string.Empty;
            Field3 = string.Empty;
            Field4 = string.Empty;
            Field5 = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public string Field1 { get; set; }
        [DataMember]
        public string Field2 { get; set; }
        [DataMember]
        public string Field3 { get; set; }
        [DataMember]
        public string Field4 { get; set; }
        [DataMember]
        public string Field5 { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
    }
}
