using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastFieldsValue
    {
        public static DataTable GetByBlastFieldID(int BlastFieldID, KMPlatform.Entity.User user)
        {
            DataTable dt;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.BlastFieldsValue.GetByBlastFieldID(BlastFieldID, user.CustomerID);
                scope.Complete();
            }
            return dt;
        }
        
        public static DataTable GetByBlastFieldIDCustomerID(int BlastFieldID,int CustomerID, KMPlatform.Entity.User user)
        {
            DataTable dt;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.BlastFieldsValue.GetByBlastFieldID(BlastFieldID, CustomerID);
                scope.Complete();
            }
            return dt;
        }
        public static void Save(ECN_Framework_Entities.Communicator.BlastFieldsValue blastFieldsValue, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                blastFieldsValue.BlastFieldsValueID = ECN_Framework_DataLayer.Communicator.BlastFieldsValue.Save(blastFieldsValue);
                scope.Complete();
            }
        }

        public static void Delete(int BlastFieldsValueID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastFieldsValue.Delete(BlastFieldsValueID, user.UserID);
                scope.Complete();
            }
        }

    }
}
