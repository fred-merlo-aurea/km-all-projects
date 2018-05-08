using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class APILoggingManager : IAPILoggingManager
    {
        public int Insert(CommunicatorEntities.APILogging log)
        {
            return APILogging.Insert(log);
        }

        public void UpdateLog(int apiLogId, int? logId)
        {
            APILogging.UpdateLog(apiLogId, logId);
        }
    }
}
