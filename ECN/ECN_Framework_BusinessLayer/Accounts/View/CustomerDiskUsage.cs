using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ECN_Framework_BusinessLayer.Accounts.View
{
    [Serializable]
    public class CustomerDiskUsage
    {
        public static ECN_Framework_Entities.Accounts.View.CustomerDiskUsage GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.View.CustomerDiskUsage usage = ECN_Framework_DataLayer.Accounts.View.CustomerDiskUsage.GetByCustomerID(customerID);
            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(usage, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            return usage;
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Accounts.View.CustomerDiskUsage usage, KMPlatform.Entity.User user)
        {
            if (usage != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (usage.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
