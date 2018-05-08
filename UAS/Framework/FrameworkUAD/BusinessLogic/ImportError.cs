using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ImportError
    {
        public List<Entity.ImportError> Select(string processCode,int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ImportError> x = null;
            x = DataAccess.ImportError.Select(processCode,sourceFileID, client).ToList();
            return x;
        }

        public int Save(Entity.ImportError x,KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.ImportErrorID = DataAccess.ImportError.Save(x, client);
                scope.Complete();
            }

            return x.ImportErrorID;
        }
        public bool SaveBulkSqlInsert(List<Entity.ImportError> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            done = DataAccess.ImportError.SaveBulkSqlInsert(list, client);
            return done;
        }
        public bool SaveBulkSqlInsert(Entity.ImportError x, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.ImportError.SaveBulkSqlInsert(x, client);
                scope.Complete();
                done = true;
            }

            return done;
        }

       
    }
}
