using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class HistoryPaid
    {
        public List<Entity.HistoryPaid> Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryPaid> retList = null;
            retList = DataAccess.HistoryPaid.Select(subscriptionID,client);
            return retList;
        }

        public int Save(FrameworkUAD.Entity.SubscriptionPaid mySubscriptionPaid, int userID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Entity.HistoryPaid hp = new Entity.HistoryPaid();
            hp.SubscriptionPaidID = mySubscriptionPaid.SubscriptionPaidID;
            hp.PubSubscriptionID = mySubscriptionPaid.PubSubscriptionID;
            hp.PriceCodeID = mySubscriptionPaid.PriceCodeID;
            hp.StartIssueDate = mySubscriptionPaid.StartIssueDate;
            hp.ExpireIssueDate = mySubscriptionPaid.ExpireIssueDate;
            hp.CPRate = mySubscriptionPaid.CPRate;
            hp.Amount = mySubscriptionPaid.Amount;
            hp.AmountPaid = mySubscriptionPaid.AmountPaid;
            hp.BalanceDue = mySubscriptionPaid.BalanceDue;
            hp.PaidDate = mySubscriptionPaid.PaidDate;
            hp.TotalIssues = mySubscriptionPaid.TotalIssues;
            hp.CheckNumber = mySubscriptionPaid.CheckNumber;
            hp.CCNumber = mySubscriptionPaid.CCNumber;
            hp.CCExpirationMonth = mySubscriptionPaid.CCExpirationMonth;
            hp.CCExpirationYear = mySubscriptionPaid.CCExpirationYear;
            hp.CCHolderName = mySubscriptionPaid.CCHolderName;
            hp.CreditCardTypeID = mySubscriptionPaid.CreditCardTypeID;
            hp.PaymentTypeID = mySubscriptionPaid.PaymentTypeID;
            hp.DateCreated = DateTime.Now;
            hp.CreatedByUserID = userID;

            return Save(hp,client);
        }
        public int Save(Entity.HistoryPaid x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.HistoryPaidID = DataAccess.HistoryPaid.Save(x,client);
                scope.Complete();
            }

            return x.HistoryPaidID;
        }
    }
}
