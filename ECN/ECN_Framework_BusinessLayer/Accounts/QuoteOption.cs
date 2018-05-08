using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class QuoteOption
    {
        public static List<ECN_Framework_Entities.Accounts.QuoteOption> GetByLicenseType(int baseChannelID, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum licenseType, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.QuoteOption> lQuoteOption = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lQuoteOption = ECN_Framework_DataLayer.Accounts.QuoteOption.GetByLicenseType(baseChannelID, licenseType);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(lQuoteOption, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return lQuoteOption;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.QuoteOption> lQuoteOption, KMPlatform.Entity.User user)
        {
            if (lQuoteOption != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from l in lQuoteOption
                                        join c in custList on l.CustomerID equals c.CustomerID
                                        select new { l.QuoteOptionID };

                    if (securityCheck.Count() != lQuoteOption.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from l in lQuoteOption
                                        where l.CustomerID != user.CustomerID
                                        select new { l.QuoteOptionID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

    }
}
