using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class ClientCustomProcedure
    {
        public List<Entity.ClientCustomProcedure> Select()
        {
            List<Entity.ClientCustomProcedure> x = null;
            x = DataAccess.ClientCustomProcedure.Select().ToList();

            return x;
        }

        public List<Entity.ClientCustomProcedure> SelectClient(int clientID)
        {
            List<Entity.ClientCustomProcedure> x = null;
            x = DataAccess.ClientCustomProcedure.SelectClient(clientID).ToList();

            return x;
        }


        public bool Save(Entity.ClientCustomProcedure x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.ClientCustomProcedure.Save(x);
                scope.Complete();
                done = true;
            }

            return done;
        }
        public int SaveReturnID(Entity.ClientCustomProcedure x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            int ClientCustomProcedureID = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                ClientCustomProcedureID = DataAccess.ClientCustomProcedure.Save(x);
                scope.Complete();
            }

            return ClientCustomProcedureID;
        }
        public bool ExecuteSproc(string sproc, int sourceFileID, string fileName, KMPlatform.Entity.Client client, string processCode = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.ClientCustomProcedure.ExecuteSproc(sproc, sourceFileID, fileName, client, processCode);
                scope.Complete();
                done = true;
            }

            return done;
        }
        public bool ExecuteSproc(string sproc, int sourceFileID, string fileName, KMPlatform.Entity.Client client, string xml, string processCode = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.ClientCustomProcedure.ExecuteSproc(sproc, sourceFileID, fileName, client, xml, processCode);
                scope.Complete();
                done = true;
            }

            return done;
        }
    }
}
