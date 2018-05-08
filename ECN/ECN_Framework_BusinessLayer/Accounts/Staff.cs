//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Transactions;

//namespace ECN_Framework_BusinessLayer.Accounts
//{
//    public class Staff
//    {
//        private static readonly string CacheName = "CACHE_STAFF";

//        public static List<ECN_Framework_Entities.Accounts.Staff> GetAll()
//        {
//            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled())
//            {
//                List<ECN_Framework_Entities.Accounts.Staff> lStaff = null;

//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    lStaff = ECN_Framework_DataLayer.Accounts.Staff.GetAll();
//                    scope.Complete();
//                }

//                return lStaff;
//            }
//            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName) == null)
//            {
//                List<ECN_Framework_Entities.Accounts.Staff> lStaff = null;
//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    lStaff = ECN_Framework_DataLayer.Accounts.Staff.GetAll();
//                    scope.Complete();
//                }
//                ECN_Framework_Common.Functions.CacheHelper.AddToCache(CacheName, lStaff);
//                return lStaff;
//            }
//            else
//            {
//                return (List<ECN_Framework_Entities.Accounts.Staff>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName);
//            }
//        }

//        public static ECN_Framework_Entities.Accounts.Staff GetStaffByID(int staffID)
//        {
//            return GetAll().Find(x => x.StaffID == staffID);
//        }

//        public static ECN_Framework_Entities.Accounts.Staff GetStaffByUserID(int userID)
//        {
//            return GetAll().Find(x => x.UserID == userID);
//        }

//        public static List<ECN_Framework_Entities.Accounts.Staff> GetStaffByRole(ECN_Framework_Common.Objects.Accounts.Enums.StaffRoleEnum role)
//        {
//            return (List<ECN_Framework_Entities.Accounts.Staff>)GetAll().Where(x => x.Roles == (Int16)role).ToList();
//        }

//        private static ECN_Framework_Entities.Accounts.Staff GetStaff(DataRow row)
//        {
//            ECN_Framework_Entities.Accounts.Staff staff = new ECN_Framework_Entities.Accounts.Staff(Convert.ToString(row["FirstName"]), Convert.ToString(row["LastName"]), Convert.ToString(row["Email"]), (ECN_Framework_Common.Objects.Accounts.Enums.StaffRoleEnum)Convert.ToInt16(row["Roles"]));
//            staff.StaffID = Convert.ToInt32(row["StaffID"]);
//            return staff;
//        }
//    }
//}
