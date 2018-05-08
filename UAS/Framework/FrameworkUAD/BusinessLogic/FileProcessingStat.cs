using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class FileProcessingStat
    {
        public bool NightlyInsert(DateTime processDate, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
                try
                {
                    DataAccess.FileProcessingStat.NightlyInsert(processDate, client);
                    done = true;
                }
                catch (Exception ex)
                {
                    done = false;
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                    alWorker.LogCriticalError(message, "NightlyInsert", KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations, "ProcessDate: " + processDate.ToShortDateString());
                }
            return done;
        }
        public Entity.FileProcessingStat Select(DateTime processDate, KMPlatform.Object.ClientConnections client)
        {
            Entity.FileProcessingStat x = null;
            x = DataAccess.FileProcessingStat.Select(processDate, client);
            return x;
        }
        public List<Entity.FileProcessingStat> SelectDateRange(DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FileProcessingStat> x = null;
            x = DataAccess.FileProcessingStat.SelectDateRange(startDate, endDate, client);
            return x;
        }



        public FrameworkUAS.Entity.FileProcessingStat GetFileProcessingStats(DateTime processDate, int clientId, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAS.Entity.FileProcessingStat x = null;
            x = DataAccess.FileProcessingStat.GetFileProcessingStats(processDate, clientId, client);
            return x;
        }
    }
}
