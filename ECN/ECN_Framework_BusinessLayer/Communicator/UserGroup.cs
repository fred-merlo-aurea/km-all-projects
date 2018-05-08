using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class UserGroup
    {
        public static bool Exists(int userID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.UserGroup.Exists(userID);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Communicator.UserGroup> Get(int userID)
        {
            List<ECN_Framework_Entities.Communicator.UserGroup> ugList = new List<ECN_Framework_Entities.Communicator.UserGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ugList = ECN_Framework_DataLayer.Communicator.UserGroup.GetByUserID(userID);
                scope.Complete();
            }
            return ugList;
        }

        public static ECN_Framework_Entities.Communicator.UserGroup GetSingle(int userID, int groupID)
        {
            ECN_Framework_Entities.Communicator.UserGroup ug = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ug = ECN_Framework_DataLayer.Communicator.UserGroup.GetSingle(userID, groupID);
                scope.Complete();
            }
            return ug;
        }

        public static int Save(ECN_Framework_Entities.Communicator.UserGroup userGroup, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                userGroup.UGID = ECN_Framework_DataLayer.Communicator.UserGroup.Save(userGroup);
                scope.Complete();
            }
            return userGroup.UGID;
        }

        public static void DeleteByUserID(int userID, KMPlatform.Entity.User loggingUser)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.UserGroup.DeleteByUserID(userID, loggingUser.UserID);
                scope.Complete();
            }
        }

        public static void DeleteByUserID_CustomerID(int userID,int customerID, KMPlatform.Entity.User loggingUser)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.UserGroup.DeleteByUserID_CustomerID(userID,customerID, loggingUser.UserID);
                scope.Complete();
            }
        }
    }
}
