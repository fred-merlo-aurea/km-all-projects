using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    public class License
    {
        public static ECN_Framework_Entities.Accounts.License GetCurrentLicensesByCustomerID(int customerID, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeCode licensetypecode)
        {
            ECN_Framework_Entities.Accounts.License lic = new ECN_Framework_Entities.Accounts.License();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lic = ECN_Framework_DataLayer.Accounts.CustomerLicense.GetCurrentLicensesByCustomerID(customerID, licensetypecode);
                scope.Complete();
            }
            return lic;
        }
    }
}
