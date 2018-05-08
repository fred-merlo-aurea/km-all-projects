using System;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IApplicationLog
    {
        int LogNonCriticalError(
            string error,
            string sourceMethod,
            int applicationId,
            string note,
            int gdCharityId,
            int ecnCustomerId);

        int LogCriticalError(
            Exception exception,
            string sourceMethod,
            int applicationId,
            string note,
            int gdCharityId,
            int ecnCustomerId);
    }
}
