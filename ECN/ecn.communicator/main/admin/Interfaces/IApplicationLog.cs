using System;

namespace Ecn.Communicator.Main.Admin.Interfaces
{
    public interface IApplicationLog
    {
        int LogCriticalError(Exception exception, string sourceMethod, int applicationId, string note = "", int charityId = -1, int customerId = -1);
    }
}
