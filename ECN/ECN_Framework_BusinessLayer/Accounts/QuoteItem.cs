using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class QuoteItem
    {
        public static bool Exists(int quoteID, int customerID)
        {
            return ECN_Framework_DataLayer.Accounts.QuoteItem.Exists(quoteID, customerID);
        }

        public static List<ECN_Framework_Entities.Accounts.QuoteItem> GetByQuoteID(int quoteID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.QuoteItem> lQuoteItem = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lQuoteItem = ECN_Framework_DataLayer.Accounts.QuoteItem.GetByQuoteID(quoteID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(lQuoteItem, user))
            throw new ECN_Framework_Common.Objects.SecurityException();

            return lQuoteItem;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.QuoteItem> lQuoteItem, KMPlatform.Entity.User user)
        {
            if (lQuoteItem != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from l in lQuoteItem
                                        join c in custList on l.CustomerID equals c.CustomerID
                                        select new { l.QuoteItemID };

                    if (securityCheck.Count() != lQuoteItem.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from l in lQuoteItem
                                        where l.CustomerID != user.CustomerID
                                        select new { l.QuoteItemID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Delete(int quoteID, int customerID, int userID)
        {
            ECN_Framework_DataLayer.Accounts.Quote.Delete(quoteID, customerID, userID);
        }
    }    
}
