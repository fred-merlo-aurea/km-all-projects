using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class FileProcessingStat
    {
        public bool Save(Entity.FileProcessingStat fps)
        {
            bool done = false;
            try
            {
                DataAccess.FileProcessingStat.Save(fps);
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                alWorker.LogCriticalError(message, "FrameworkUAS.BusinessLogic.FileProcessingStat.Save", KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations);
            }
            return done;
        }
        public Entity.FileProcessingStat Select(DateTime processDate)
        {
            Entity.FileProcessingStat x = null;
            x = DataAccess.FileProcessingStat.Select(processDate);
            return x;
        }
        public List<Entity.FileProcessingStat> SelectDateRange(DateTime startDate, DateTime endDate)
        {
            List<Entity.FileProcessingStat> x = null;
            x = DataAccess.FileProcessingStat.SelectDateRange(startDate, endDate);
            return x;
        }
    }
}
