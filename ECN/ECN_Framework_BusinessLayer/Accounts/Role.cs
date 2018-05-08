using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Role
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Role;

        public static bool Exists(int roleID, int customerID)
        {
            return ECN_Framework_DataLayer.Accounts.Role.Exists(roleID, customerID);
        }

        public static List<ECN_Framework_Entities.Accounts.Role> GetByCustomerID(int customerID, bool getChildren)
        {
            List<ECN_Framework_Entities.Accounts.Role> roleList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                roleList = ECN_Framework_DataLayer.Accounts.Role.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (roleList != null && getChildren)
            {
                foreach (ECN_Framework_Entities.Accounts.Role role in roleList)
                {
                    role.actionList = ECN_Framework_BusinessLayer.Accounts.RoleAction.GetByRoleID(role.RoleID, customerID);
                }
            }
            return roleList;
        }

        public static int Save(ECN_Framework_Entities.Accounts.Role role, KMPlatform.Entity.User user)
        {
            Validate(role, user);
            return role.RoleID = ECN_Framework_DataLayer.Accounts.Role.Save(role);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.Role role, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
              if (!KM.Platform.User.IsSystemAdministrator(user) && KM.Platform.User.IsChannelAdministrator(user))
                {
                    using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        if (ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(role.CustomerID, false).BaseChannelID != ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false).BaseChannelID)
                        {
                            throw new ECN_Framework_Common.Objects.SecurityException();
                        }
                        supressscope.Complete();
                    }
                }
                
            if (role.RoleID <= 0 && role.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (role.RoleID > 0 && role.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}
