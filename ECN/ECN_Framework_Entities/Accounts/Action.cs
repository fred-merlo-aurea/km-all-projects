using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class Action
    {
        public Action() 
        {
            ActionID = -1;
            ProductID = null;
            DisplayName = string.Empty;
            ActionCode = string.Empty;
            DisplayOrder = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int ActionID { get; set; }
        [DataMember]
        public int? ProductID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string ActionCode { get; set; }
        [DataMember]
        public int? DisplayOrder { get; set; }
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
