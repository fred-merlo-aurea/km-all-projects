using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = KM.Common.Entity.Fakes.ShimApplicationLog;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ApplicationLogMock : Mock<IApplicationLog>
    {
        public ApplicationLogMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.LogNonCriticalErrorStringStringInt32StringInt32Int32 = LogNonCriticalError;
            Shim.LogCriticalErrorExceptionStringInt32StringInt32Int32 = LogCriticalError;
        }

        private int LogCriticalError(
            Exception exception,
            string sourceMethod,
            int applicationId,
            string note,
            int gdCharityId,
            int ecnCustomerId)
        {
            return Object.LogCriticalError(exception, sourceMethod, applicationId, note, gdCharityId, ecnCustomerId);
        }

        private int LogNonCriticalError(
            string error,
            string sourceMethod,
            int applicationId,
            string note,
            int gdCharityId,
            int ecnCustomerId)
        {
            return Object.LogNonCriticalError(error, sourceMethod, applicationId, note, gdCharityId, ecnCustomerId);
        }

        public void VerifyLogNonCriticalError(string error, string sourceMethod, int applicationId)
        {
            Verify(log => log.LogNonCriticalError(error, sourceMethod, applicationId, It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>()));
        }
    }
}
