using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class RoleAction
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.RoleAction;

        public static List<ECN_Framework_Entities.Accounts.RoleAction> GetByRoleID(int roleID, int customerID)
        {
            List<ECN_Framework_Entities.Accounts.RoleAction> lroleAction = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lroleAction = ECN_Framework_DataLayer.Accounts.RoleAction.GetByRoleID(roleID, customerID);
                scope.Complete();
            }

            return lroleAction;
        }

        public static int Save(ECN_Framework_Entities.Accounts.RoleAction roleAction, KMPlatform.Entity.User user)
        {
            Validate(roleAction, user);
            return ECN_Framework_DataLayer.Accounts.RoleAction.Save(roleAction);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.RoleAction roleAction, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                errorList.Add(new ECNError(Entity, Method, ""));
                throw new ECNException(errorList);
            }

            if (roleAction.RoleActionID <= 0 && roleAction.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (roleAction.RoleActionID > 0 && roleAction.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }

        }
    }
}
