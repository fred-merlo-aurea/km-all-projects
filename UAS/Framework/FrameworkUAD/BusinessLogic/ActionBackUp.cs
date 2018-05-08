using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ActionBackUp
    {
        public bool Restore(int productID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.ActionBackUp.Restore(productID, client);
                scope.Complete();
            }

            return complete;
        }

        public bool Bulk_Insert(int productID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.ActionBackUp.Bulk_Insert(productID, client);
                scope.Complete();
            }

            return complete;
        }
    }
}
