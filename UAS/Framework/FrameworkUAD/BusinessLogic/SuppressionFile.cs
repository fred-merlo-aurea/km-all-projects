using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SuppressionFile
    {
        public List<Entity.SuppressionFile> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SuppressionFile> x = null;
            x = DataAccess.SuppressionFile.Select(client).ToList();
            return x;
        }
        public int Save(Entity.SuppressionFile x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = Core_AMS.Utilities.TransactionUtilities.CreateTransactionScope())
            {
                x.FileDateModified = DateTime.Now;
                x.SuppressionFileId = DataAccess.SuppressionFile.Save(x, client);
                scope.Complete();
            }

            return x.SuppressionFileId;
        }
        public int RunSuppression(KMPlatform.Object.ClientConnections client, string processcode)
        {
            int count = 0;
            using (TransactionScope scope = Core_AMS.Utilities.TransactionUtilities.CreateTransactionScope())
            {
                count = DataAccess.SuppressionFile.RunSuppression(client, processcode);
                scope.Complete();
            }
            return count;
        }
    }
}
