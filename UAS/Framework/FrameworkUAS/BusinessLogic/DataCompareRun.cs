using System;
using System.Collections.Generic;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareRun
    {
        public List<Entity.DataCompareRun> SelectForSourceFile(int sourceFileId)
        {
            List<Entity.DataCompareRun> x = null;
            x = DataAccess.DataCompareRun.SelectForSourceFile(sourceFileId);
            foreach (Entity.DataCompareRun r in x)
                SetCustomProperties(r);
            return x;
        }
        public List<Entity.DataCompareRun> SelectForClient(int clientId)
        {
            List<Entity.DataCompareRun> x = null;
            x = DataAccess.DataCompareRun.SelectForClient(clientId);
            foreach (Entity.DataCompareRun r in x)
                SetCustomProperties(r);
            return x;
        }
        public List<Entity.DataCompareRun> SelectForUser(int userId)
        {
            List<Entity.DataCompareRun> x = null;
            x = DataAccess.DataCompareRun.SelectForUser(userId);
            foreach (Entity.DataCompareRun r in x)
                SetCustomProperties(r);
            return x;
        }
        public int Save(Entity.DataCompareRun x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.DcRunId = DataAccess.DataCompareRun.Save(x);
                scope.Complete();
            }

            return x.DcRunId;
        }
        private void SetCustomProperties(Entity.DataCompareRun dcr)
        {
            DataCompareView cWorker = new DataCompareView();
            dcr.DcViews = cWorker.SelectForRun(dcr.DcRunId);
        }

        //public bool SetStarted(int dcRunId, string processCode)
        //{
        //    bool done = false;
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        done = DataAccess.DataCompareRun.SetStarted(dcRunId, processCode);
        //        scope.Complete();
        //    }

        //    return done;
        //}     
    }
}
