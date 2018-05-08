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
    public class CustomerDepartment
    {
        public CustomerDepartment() 
        {
            DepartmentID = -1;
            CustomerID = null;
            DepartmentName = string.Empty;
            DepartmentDesc = string.Empty;
            udList = null;
            ErrorList = new List<ECNError>();
        }
        
        public int DepartmentID { get; set; }
        public int? CustomerID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDesc { get; set; }
        //optional
        public List<UserDepartment> udList { get; set; }
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
