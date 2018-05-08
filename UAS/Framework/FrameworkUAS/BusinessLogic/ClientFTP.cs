using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class ClientFTP
    {
        public List<Entity.ClientFTP> SelectClient(int clientID)
        {
            List<Entity.ClientFTP> x = null;
            x = DataAccess.ClientFTP.SelectClient(clientID).ToList();

            return x;
        }
      
        public int Save(Entity.ClientFTP x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.FTPID = DataAccess.ClientFTP.Save(x);
                scope.Complete();
            }

            return x.FTPID;
        }
    }
}
