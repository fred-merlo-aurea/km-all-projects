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
    public class UserDepartment
    {
        public UserDepartment() 
        {
            UserDepartmentID = -1;
            UserID = -1;
            DepartmentID = -1;
            IsDefaultDept = false;
            CustomerID = null;
        }

        [DataMember]
        public int UserDepartmentID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public int? DepartmentID { get; set; }

        [DataMember]
        public bool IsDefaultDept { get; set; }

        [DataMember] //optional - not in Table - loading to Validate Customer
        public int? CustomerID { get; set; }
    }
}
