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
    public class Code
    {
        public Code()
        {
            CodeID = -1;
            CustomerID = null;
            CodeType = string.Empty;
            CodeValue = string.Empty;
            CodeDisplay = string.Empty;
            SortOrder = null;
            DisplayFlag = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int CodeID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string CodeType { get; set; }
        [DataMember]
        public string CodeValue { get; set; }
        [DataMember]
        public string CodeDisplay { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public string DisplayFlag { get; set; }
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
    }
}
