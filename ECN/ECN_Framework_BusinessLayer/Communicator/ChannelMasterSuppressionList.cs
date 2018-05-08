using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class ChannelMasterSuppressionList
    {
        public static void Delete(int baseChannelID, string emailAddress, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ChannelMasterSuppressionList.Delete(baseChannelID, emailAddress, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int baseChannelID, int cmsid, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.ChannelMasterSuppressionList.Delete(baseChannelID, cmsid, user.UserID);
                scope.Complete();
            }
        }

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList, KMPlatform.Entity.User user)
        //{
        //    if (channelMasterSuppressionList != null)
        //    {
        //        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {                 

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in channelMasterSuppressionList
        //                                join c in custList on ct.BaseChannelID equals c.BaseChannelID.Value
        //                                select new { ct.CMSID };

        //            if (securityCheck.Count() != channelMasterSuppressionList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in channelMasterSuppressionList
        //                                where ct.BaseChannelID != customer.BaseChannelID.Value
        //                                select new { ct.CMSID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> GetByBaseChannelID(int baseChannelID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channelMasterSuppressionList = ECN_Framework_DataLayer.Communicator.ChannelMasterSuppressionList.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }
            return channelMasterSuppressionList;
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> GetByBaseChannelID_Paging(int baseChannelID,string email, int pageIndex, int pageSize, string sortColumn, string sortDirection, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channelMasterSuppressionList = ECN_Framework_DataLayer.Communicator.ChannelMasterSuppressionList.GetByBaseChannelID_Paging(baseChannelID, email,pageIndex, pageSize, sortColumn, sortDirection);
                scope.Complete();
            }
            return channelMasterSuppressionList;
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> GetByEmailAddress(int baseChannelID, string emailAddress, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channelMasterSuppressionList = ECN_Framework_DataLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(baseChannelID, emailAddress);
                scope.Complete();
            }
            return channelMasterSuppressionList;
        }

        public static DataSet GetByEmailAddress_Paging(int baseChannelID, int pageNo, int pageSize, string searchString, KMPlatform.Entity.User user)
        {
            DataSet dsSuppressionList = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsSuppressionList = ECN_Framework_DataLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress_Paging(baseChannelID, pageNo, pageSize, searchString);
                scope.Complete();
            }
            return dsSuppressionList;
        }
    }
}

