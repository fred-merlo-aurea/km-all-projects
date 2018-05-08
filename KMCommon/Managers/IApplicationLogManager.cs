using System;
using KM.Common.Entity;

namespace KM.Common.Managers
{
    public interface IApplicationLogManager
    {
        void LogCriticalError(
            Exception exception,
            string sourceMethod,
            Application.Applications application,
            string note = "",
            int gdCharityId = -1,
            int ecnCustomerId = -1);

        int LogCriticalError(
            Exception exception,
            string sourceMethod,
            int applicationId,
            string note = "",
            int gdCharityId = -1,
            int ecnCustomerId = -1);
    }
}