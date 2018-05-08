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
    public class Role
    {
        public Role() 
        {
            RoleID = -1;
            CustomerID = -1;
            RoleName = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            actionList = null;
        }

        [DataMember]
        public int RoleID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string RoleName { get; set; }
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
        //optional
        public List<RoleAction> actionList { get; set; }

    }
}
