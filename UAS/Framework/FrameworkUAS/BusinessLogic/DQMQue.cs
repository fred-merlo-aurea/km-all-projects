using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DQMQue
    {
        public List<Entity.DQMQue> Select(int clientID, bool isDemo, bool isADMS)
        {
            List<Entity.DQMQue> x = null;
            x = DataAccess.DQMQue.Select(clientID, isDemo, isADMS).ToList();

            return x;
        }
        public Entity.DQMQue Select(string processCode)
        {
            Entity.DQMQue x = null;
            x = DataAccess.DQMQue.Select(processCode);

            return x;
        }
        public List<Entity.DQMQue> Select(bool isDemo, bool isADMS, bool isQued = false)
        {
            List<Entity.DQMQue> x = null;
            x = DataAccess.DQMQue.Select(isDemo, isADMS, isQued).ToList();

            return x;
        }
        public List<Entity.DQMQue> Select(int clientID, bool isDemo, bool isADMS, bool isQued = false)
        {
            List<Entity.DQMQue> x = null;
            x = DataAccess.DQMQue.Select(clientID, isDemo, isADMS, isQued).ToList();

            return x;
        }

        public List<Entity.DQMQue> Select(bool isDemo, bool isADMS, bool isQued = false, bool isCompleted = false)
        {
            List<Entity.DQMQue> x = null;
            x = DataAccess.DQMQue.Select(isDemo, isADMS, isQued, isCompleted).ToList();

            return x;
        }
        public bool Save(Entity.DQMQue x)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.DQMQue.Save(x);
                scope.Complete();
            }

            return done;
        }
        public bool UpdateComplete(string processCode, bool createLog = false, string msg = "", int sourceFileId = -1)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.DQMQue.UpdateComplete(processCode,msg,createLog);
                scope.Complete();
            }

            if(createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = msg;
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return done;
        }
    }
}
