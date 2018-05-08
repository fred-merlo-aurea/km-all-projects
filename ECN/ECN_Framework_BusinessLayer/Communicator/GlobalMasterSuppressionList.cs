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
    public class GlobalMasterSuppressionList
    {
        public static void Delete(string emailAddress, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.GlobalMasterSuppressionList.Delete(emailAddress, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int gsid, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.GlobalMasterSuppressionList.Delete(gsid, user.UserID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> GetAll(KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList = new List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                globalMasterSuppressionList = ECN_Framework_DataLayer.Communicator.GlobalMasterSuppressionList.GetAll();
                scope.Complete();
            }
            return globalMasterSuppressionList;
        }

        public static List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> GetByEmailAddress(string emailAddress, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList = new List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                globalMasterSuppressionList = ECN_Framework_DataLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(emailAddress);
                scope.Complete();
            }
            return globalMasterSuppressionList;
        }

        public static DataSet GetByEmailAddress_Paging(int pageNo, int pageSize, string searchString, KMPlatform.Entity.User user)
        {
            DataSet dsSuppressionList = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsSuppressionList = ECN_Framework_DataLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress_Paging(pageNo, pageSize, searchString);
                scope.Complete();
            }
            return dsSuppressionList;
        }

        public static List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> Search_Paging(int pageNo, int pageSize, string searchString, string sortColumn, string sortDirection, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> dsSuppressionList = new List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsSuppressionList = ECN_Framework_DataLayer.Communicator.GlobalMasterSuppressionList.Search_Paging(pageNo, pageSize, searchString, sortColumn, sortDirection);
                scope.Complete();
            }
            return dsSuppressionList;
        }
    }
}

