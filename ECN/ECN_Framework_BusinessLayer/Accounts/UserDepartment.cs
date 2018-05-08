using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class UserDepartment
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.UserDepartment;

        public static bool Exists(int userDepartmentID)
        {
            return ECN_Framework_DataLayer.Accounts.UserDepartment.Exists(userDepartmentID);
        }

        public static List<ECN_Framework_Entities.Accounts.UserDepartment> GetByDepartmentID(int departmentID, int customerID)
        {
            List<ECN_Framework_Entities.Accounts.UserDepartment> luserDept = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                luserDept = ECN_Framework_DataLayer.Accounts.UserDepartment.GetByDepartmentID(departmentID, customerID);
            }

            return luserDept;
        }

        public static List<ECN_Framework_Entities.Accounts.UserDepartment> GetByUserID(int userID, int customerID)
        {
            List<ECN_Framework_Entities.Accounts.UserDepartment> luserDept = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                luserDept = ECN_Framework_DataLayer.Accounts.UserDepartment.GetByUserID(userID, customerID);
            }

            return luserDept;
        }

        public static bool Delete(ECN_Framework_Entities.Accounts.UserDepartment userDept)
        {
            ECN_Framework_DataLayer.Accounts.UserDepartment.Delete(userDept.UserID);
            return true;
        }

        public static bool Validate(ECN_Framework_Entities.Accounts.UserDepartment userDept)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> ErrorList = new List<ECNError>();

            ECNError error = null;
            if (userDept.UserID == null || userDept.CustomerID == null || !KMPlatform.BusinessLogic.User.Exists(userDept.UserID, userDept.CustomerID.Value))
            {
                error = new ECNError(Entity, Method, "User Department user is invalid");
                ErrorList.Add(error);
            }
            if (userDept.DepartmentID == null || (!ECN_Framework_BusinessLayer.Accounts.CustomerDepartment.Exists(userDept.DepartmentID.Value)))
            {
                error = new ECNError(Entity, Method, "User Department id is invalid");
                ErrorList.Add(error);
            }
            if (userDept.IsDefaultDept == null)
            {
                error = new ECNError(Entity, Method, "User Department default is invalid");
                ErrorList.Add(error);
            }

            if (ErrorList.Count > 0)
            {
                throw new ECNException(ErrorList, Enums.ExceptionLayer.Business);
            }
            else
            {
                return true;
            }
        }

        public static bool Save(ECN_Framework_Entities.Accounts.UserDepartment userDept)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            List<ECNError> ErrorList = new List<ECNError>();

            if (Validate(userDept))
            {
                if (userDept.UserDepartmentID > 0)
                {

                    if (Exists(userDept.UserDepartmentID))
                    {
                        ECN_Framework_DataLayer.Accounts.UserDepartment.Update(ref userDept);
                        return true;
                    }
                    else
                    {

                        ErrorList.Add(new ECNError(Entity, Method, "Invalid UserDepartmentID"));
                        throw new ECNException(ErrorList, Enums.ExceptionLayer.Business);
                    }

                }
                else
                {

                    ECN_Framework_DataLayer.Accounts.UserDepartment.Insert(ref userDept);
                    return true;

                }
            }
            else
            {
                return false;
            }
        }

        public static void DefaultAll(int userID)
        {
            ECN_Framework_DataLayer.Accounts.UserDepartment.DefaultAll(userID);
        }

    }
}
