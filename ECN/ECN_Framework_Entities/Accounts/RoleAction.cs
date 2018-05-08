using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects.Accounts;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class RoleAction
    {
        public RoleAction() 
        {
            RoleActionID = -1;
            RoleID = null;
            ActionID = null;
            Active = string.Empty;
            CustomerID = -1;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int RoleActionID { get; set; }
        [DataMember]
        public int? RoleID { get; set; }
        [DataMember]
        public int? ActionID { get; set; }
        [DataMember]
        public string Active { get; set; }
        //optional
        [DataMember]
        public int CustomerID { get; set; }
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
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
