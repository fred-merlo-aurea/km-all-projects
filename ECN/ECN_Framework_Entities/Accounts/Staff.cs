//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ECN_Framework_Common.Objects.Accounts;

//namespace ECN_Framework_Entities.Accounts
//{
//    public class Staff
//    {
//        public Staff()
//        {

//        }
//        public Staff(string firstName, string lastName, string email, ECN_Framework_Common.Objects.Accounts.Enums.StaffRoleEnum role)
//        {
//            FirstName = firstName;
//            LastName = lastName;
//            Email = email;
//            Roles = (Int16)role;
//        }

//        public Staff(int staffid)
//        {
//            StaffID = staffid;
//        }

//        public int StaffID { get; set; }
//        public int BaseChannelID { get; set; }
//        public string FirstName { get; set; }
//        public string LastName { get; set; }
//        public string Email { get; set; }
//        public int UserID { get; set; }
//        public int Roles { get; set; }
//        public bool LicenseUpdateFlag { get; set; }
//        public bool FeatureUpdateFlag { get; set; }
//    }
//}
