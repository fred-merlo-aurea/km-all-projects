using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FileLog
    {
        #region File Log Entity
        public  List<Entity.FileLog> SelectClient(int clientID)
        {
            List<Entity.FileLog> x = null;
            x = DataAccess.FileLog.SelectClient(clientID).ToList();

            return x;
        }
        public List<Entity.FileLog> SelectProcessCode(string processCode)
        {
            List<Entity.FileLog> x = null;
            x = DataAccess.FileLog.SelectProcessCode(processCode).ToList();

            return x;
        }
        public  Entity.FileLog SelectTopOneProcessCode(string processCode)
        {
            Entity.FileLog x = null;
            x = DataAccess.FileLog.SelectTopOneProcessCode(processCode);

            return x;
        }

        public List<Entity.FileLog> SelectFileLog(int sourceFileID, string processCode)
        {
            List<Entity.FileLog> x = null;
            x = DataAccess.FileLog.SelectFileLog(sourceFileID, processCode);

            return x;
        }
      
        public  bool Save(Entity.FileLog x)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DataAccess.FileLog.Save(x);
                scope.Complete();
                done = true;
            }

            return done;
        }
        #endregion

        #region File Log Object
        public List<Object.FileLog> SelectDistinctProcessCodePerSourceFile(int clientID)
        {
            List<Object.FileLog> retList = null;
            retList = DataAccess.FileLog.SelectDistinctProcessCodePerSourceFile(clientID);
            return retList;
        }
        #endregion
    }
}
