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
    public class LandingPage
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LandingPage;

        public static bool Exists(int LPID)
        {
            return ECN_Framework_DataLayer.Accounts.LandingPage.Exists(LPID);
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPage> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.LandingPage> itemList = new List<ECN_Framework_Entities.Accounts.LandingPage>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.LandingPage.GetAll();
                scope.Complete();
            }

            return itemList;
        }

        public static ECN_Framework_Entities.Accounts.LandingPage GetByLPID(int LPID)
        {
            ECN_Framework_Entities.Accounts.LandingPage item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.LandingPage.GetByLPID(LPID);
                scope.Complete();
            }

            return item;
        }
    }
}
