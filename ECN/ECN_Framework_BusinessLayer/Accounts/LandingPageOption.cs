using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class LandingPageOption
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LandingPageOption;

        public static bool Exists(int LPOID)
        {
            return ECN_Framework_DataLayer.Accounts.LandingPageOption.Exists(LPOID);
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageOption> GetByLPID(int LPID)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageOption> itemList = new List<ECN_Framework_Entities.Accounts.LandingPageOption>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.LandingPageOption.GetByLPID(LPID);
                scope.Complete();
            }

            return itemList;
        }

        public static ECN_Framework_Entities.Accounts.LandingPageOption GetByLPOID(int LPOID)
        {
            ECN_Framework_Entities.Accounts.LandingPageOption item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.LandingPageOption.GetByLPOID(LPOID);
                scope.Complete();
            }

            return item;
        }
    }
}
