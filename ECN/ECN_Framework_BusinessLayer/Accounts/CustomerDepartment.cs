using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerDepartment
    {
        public static bool Exists(int departmentID)
        {
            return ECN_Framework_DataLayer.Accounts.CustomerDepartment.Exists(departmentID);
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerDepartment> GetByCustomerID(int customerID, bool getChildren)
        {
            List<ECN_Framework_Entities.Accounts.CustomerDepartment> cdList = new List<ECN_Framework_Entities.Accounts.CustomerDepartment>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                cdList = ECN_Framework_DataLayer.Accounts.CustomerDepartment.GetByCustomerID(customerID);
                scope.Complete();
            }
            if (cdList != null && getChildren)
            {
                foreach (ECN_Framework_Entities.Accounts.CustomerDepartment cd in cdList)
                {
                    cd.udList = UserDepartment.GetByDepartmentID(cd.DepartmentID, cd.CustomerID.Value);
                }
            }
            return cdList;
        }

    }
}
