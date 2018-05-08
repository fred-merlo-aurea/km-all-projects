using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class ChannelNoThresholdList
    {
        public static void Delete(int baseChannelID, string emailAddress, KMPlatform.Entity.User user)
        {
            //this does the security check
            //GetByEmailAddress(baseChannelID, emailAddress, user);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ChannelNoThresholdList.Delete(baseChannelID, emailAddress, user.UserID);
                scope.Complete();
            }
        }
        public static void DeleteByCNTID(int baseChannelID, int CNTID, KMPlatform.Entity.User user)
        {
            //this does the security check
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ChannelNoThresholdList.DeleteByCNTID(baseChannelID, CNTID, user.UserID);
                scope.Complete();
            }
        }
        public static List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> GetByBaseChannelID(int baseChannelID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = new List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channelNoThresholdList = ECN_Framework_DataLayer.Communicator.ChannelNoThresholdList.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }

            return channelNoThresholdList;
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> GetByEmailAddress(int baseChannelID, string emailAddress, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = new List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channelNoThresholdList = ECN_Framework_DataLayer.Communicator.ChannelNoThresholdList.GetByEmailAddress(baseChannelID, emailAddress);
                scope.Complete();
            }

            return channelNoThresholdList;
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> GetByEmailAddress_Paging(int baseChannelID, string emailAddress, int pageIndex, int pageSize, string sortColumn, string sortDirection, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = new List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channelNoThresholdList = ECN_Framework_DataLayer.Communicator.ChannelNoThresholdList.GetByEmailAddress_Paging(baseChannelID, emailAddress, pageIndex, pageSize, sortColumn, sortDirection);
                scope.Complete();
            }

            return channelNoThresholdList;
        }

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> noThresholdList, KMPlatform.Entity.User user)
        //{
        //    if (noThresholdList != null)
        //    {
        //        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {                    

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in noThresholdList
        //                                join c in custList on ct.BaseChannelID equals c.BaseChannelID.Value
        //                                select new { ct.CNTID };

        //            if (securityCheck.Count() != noThresholdList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in noThresholdList
        //                                where ct.BaseChannelID != customer.BaseChannelID.Value
        //                                select new { ct.CNTID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
