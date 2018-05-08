using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Batch
    {

        public Entity.Batch StartNewBatch(int userID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Entity.Batch batch = DataAccess.Batch.StartNewBatch(userID, publicationID,client);
            return batch;
        }
        public List<Entity.Batch> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Batch> retList = null;
            retList = DataAccess.Batch.Select(client);
            return retList;
        }
        public List<Entity.Batch> Select(int userID, bool isActive, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Batch> retList = null;
            retList = DataAccess.Batch.Select(userID,isActive,client);
            return retList;
        }
        public bool BatchCheck(Entity.Batch batch)
        {
            if (batch.BatchCount == 100)
            {
                Core_AMS.Utilities.WPF.Message("You have done 100 transactions, you must now close this batch.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop, System.Windows.MessageBoxResult.OK, "Batch Complete");
                return true;
            }
            return false;
        }
        public bool CloseBatches(int userID, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.Batch.CloseBatches(userID, client);
        }
        public int Save(Entity.Batch x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.BatchID = DataAccess.Batch.Save(x,client);
                scope.Complete();
            }

            return x.BatchID;
        }
        public int FinalizeBatchID(int batchID, KMPlatform.Object.ClientConnections client)
        {
            int id = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                id = DataAccess.Batch.FinalizeBatchID(batchID, client);
                scope.Complete();
            }

            return id;
        }
    }
}
