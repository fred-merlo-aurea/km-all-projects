using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Quote
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Quote;

        public static bool Exists(int quoteID, int customerID)
        {
            return ECN_Framework_DataLayer.Accounts.Quote.Exists(quoteID, customerID);
        }

        public static ECN_Framework_Entities.Accounts.Quote GetByQuoteID(int quoteID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Accounts.Quote quote = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                quote = ECN_Framework_DataLayer.Accounts.Quote.GetByQuoteID(quoteID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != quote.CustomerID && !SecurityCheck(quote, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (quote.QuoteID > 0 && getChildren)
            {
                quote.ItemList = ECN_Framework_BusinessLayer.Accounts.QuoteItem.GetByQuoteID(quote.QuoteID, user);
            }

            return quote;
        }

        public static List<ECN_Framework_Entities.Accounts.Quote> GetByCustomerID(int customerID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Accounts.Quote> lQuote = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lQuote = ECN_Framework_DataLayer.Accounts.Quote.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lQuote, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (lQuote.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Accounts.Quote quote in lQuote)
                {
                    quote.ItemList = ECN_Framework_BusinessLayer.Accounts.QuoteItem.GetByQuoteID(quote.QuoteID, user); 
                }
            }
            return lQuote;
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Accounts.Quote quote, KMPlatform.Entity.User user)
        {
            if (quote != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (quote.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.Quote> lQuote, KMPlatform.Entity.User user)
        {
            if (lQuote != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from l in lQuote
                                        join c in custList on l.CustomerID equals c.CustomerID
                                        select new { l.QuoteID };

                    if (securityCheck.Count() != lQuote.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from l in lQuote
                                        where l.CustomerID != user.CustomerID
                                        select new { l.QuoteID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Delete(int quoteID, int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(quoteID, customerID))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (ECN_Framework_BusinessLayer.Accounts.QuoteItem.Exists(quoteID, customerID))
                        ECN_Framework_BusinessLayer.Accounts.QuoteItem.Delete(quoteID, customerID, user.UserID);

                    ECN_Framework_DataLayer.Accounts.Quote.Delete(quoteID, customerID, user.UserID);
                    scope.Complete();
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Item does not exist"));
                throw new ECNException(errorList);
            }
        }
    }
}
