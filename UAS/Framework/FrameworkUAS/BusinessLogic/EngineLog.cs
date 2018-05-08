using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class EngineLog
    {
        /// <summary>
        /// * This is the select all - there will be one EngineLog item for each Engine for each client
        /// * Primary Key is ClientId / Engine
        /// * You can call this to get all engines status for all clients
        /// * Additional select methods in this class will call this method to retrieve data then filter returned object
        /// </summary>
        /// <returns></returns>
        public List<Entity.EngineLog> Select()
        {
            List<Entity.EngineLog> x = null;
            x = DataAccess.EngineLog.Select().ToList();

            return x;
        }
        public List<Entity.EngineLog> Select(int clientId)
        {
            return Select().Where(x => x.ClientId == clientId).ToList();
        }
        public List<Entity.EngineLog> Select(bool isRunning)
        {
            return Select().Where(x => x.IsRunning == isRunning).ToList();
        }
        public Entity.EngineLog Select(int clientId, string engine)
        {
            //if(!Select().Exists(x => x.ClientId == clientId && x.Engine.Equals(engine, StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    Entity.EngineLog el = new Entity.EngineLog
            //    {
            //        EngineLogId = 0,
            //        ClientId = clientId,
            //        Engine = engine
            //    };
            //    Save(el);
            //}

            //return Select().SingleOrDefault(x => x.ClientId == clientId && x.Engine.Equals(engine, StringComparison.CurrentCultureIgnoreCase));

            Entity.EngineLog el = Select().SingleOrDefault(x => x.ClientId == clientId && x.Engine.Equals(engine, StringComparison.CurrentCultureIgnoreCase));
            if (el == null || el.EngineLogId == 0)
            {
                el = new Entity.EngineLog
                {
                    EngineLogId = 0,
                    ClientId = clientId,
                    Engine = engine
                };
                Save(el);
                el = Select().SingleOrDefault(x => x.ClientId == clientId && x.Engine.Equals(engine, StringComparison.CurrentCultureIgnoreCase));
            }
            return el;
        }
        public Entity.EngineLog Select(int clientId, Enums.Engine engine)
        {
            //if (!Select().Exists(x => x.ClientId == clientId && x.Engine.Equals(engine.ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    Entity.EngineLog el = new Entity.EngineLog
            //    {
            //        EngineLogId = 0,
            //        ClientId = clientId,
            //        Engine = engine.ToString()
            //    };
            //    Save(el);
            //}

            //return Select().Single(x => x.ClientId == clientId && x.Engine.Equals(engine.ToString(), StringComparison.CurrentCultureIgnoreCase));

            Entity.EngineLog el = Select().SingleOrDefault(x => x.ClientId == clientId && x.Engine.Equals(engine.ToString(), StringComparison.CurrentCultureIgnoreCase));
            if (el == null || el.EngineLogId == 0)
            {
                el = new Entity.EngineLog
                {
                    EngineLogId = 0,
                    ClientId = clientId,
                    Engine = engine.ToString()
                };
                Save(el);
                el = Select().SingleOrDefault(x => x.ClientId == clientId && x.Engine.Equals(engine.ToString(), StringComparison.CurrentCultureIgnoreCase));
            }
            return el;
        }
        public bool Save(Entity.EngineLog x)
        {
            x.DateUpdated = DateTime.Now;
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.EngineLog.Save(x);
                scope.Complete();
            }

            return done;
        }
        public bool UpdateRefresh(int engineLogId, string currentStatus = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.EngineLog.UpdateRefresh(engineLogId,currentStatus);
                scope.Complete();
            }
            return done;
        }
        public bool UpdateRefresh(int clientId, string engine, string currentStatus = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.EngineLog.UpdateRefresh(clientId, engine, currentStatus);
                scope.Complete();
            }
            return done;
        }
        public bool UpdateRefresh(int clientId, Enums.Engine engine, string currentStatus = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.EngineLog.UpdateRefresh(clientId, engine.ToString(), currentStatus);
                scope.Complete();
            }
            return done;
        }
        public bool UpdateIsRunning(int engineLogId, bool isRunning, string currentStatus = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.EngineLog.UpdateIsRunning(engineLogId, isRunning, currentStatus);
                scope.Complete();
            }
            return done;
        }
        public bool UpdateIsRunning(int clientId, string engine, bool isRunning, string currentStatus = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.EngineLog.UpdateIsRunning(clientId, engine, isRunning);
                scope.Complete();
            }
            return done;
        }
        public bool UpdateIsRunning(int clientId, Enums.Engine engine, bool isRunning, string currentStatus = "")
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.EngineLog.UpdateIsRunning(clientId, engine.ToString(), isRunning, currentStatus);
                scope.Complete();
            }
            return done;
        }
        public void SaveEngineLog(string message, int clientId, Enums.Engine engine, FrameworkUAS.Entity.EngineLog engineLog = null)
        {
            if (engineLog == null)
                engineLog = Select(clientId, engine);
            engineLog.CurrentStatus = message;
            engineLog.DateUpdated = DateTime.Now;
            engineLog.IsRunning = true;
            engineLog.LastRunningCheckDate = DateTime.Now;
            engineLog.LastRunningCheckTime = DateTime.Now.TimeOfDay;
            Save(engineLog);
        }
    }
}
