using System;
using Ecn.Communicator.Main.Admin.Interfaces;
using KM.Common.Entity;

namespace Ecn.Communicator.Main.Admin.Helpers
{
    public class ApplicationLogAdapter : IApplicationLog
    {
        public int LogCriticalError(
            Exception exception, 
            string sourceMethod, 
            int applicationId, 
            string note = "", 
            int charityId = -1, 
            int customerId = -1)
        {
            return ApplicationLog.LogCriticalError(exception, sourceMethod, applicationId, note, charityId, customerId);
        }
    }
}
