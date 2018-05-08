using System;
using KM.Common.Entity;

namespace KM.Common.Managers
{
    [Serializable]
    public class ApplicationLogManager : IApplicationLogManager
    {
        public void LogCriticalError(
            Exception exception,
            string sourceMethod,
            Application.Applications application,
            string note = "",
            int gdCharityId = -1,
            int ecnCustomerId = -1)
        {
            ApplicationLog.LogCriticalError(
                exception,
                sourceMethod,
                application,
                note,
                gdCharityId,
                ecnCustomerId);
        }

        public int LogCriticalError(
            Exception exception,
            string sourceMethod,
            int applicationId,
            string note = "",
            int gdCharityId = -1,
            int ecnCustomerId = -1)
        {
            return ApplicationLog.LogCriticalError(
                exception,
                sourceMethod,
                applicationId,
                note,
                gdCharityId,
                ecnCustomerId);
        }
    }
}
