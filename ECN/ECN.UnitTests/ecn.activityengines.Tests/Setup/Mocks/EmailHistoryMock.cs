using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Moq;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class EmailHistoryMock : Mock<IEmailHistory>
    {
        public EmailHistoryMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimEmailHistory.FindMergedEmailIDInt32 = FindMergedEmailID;
        }

        private int FindMergedEmailID(int oldEmailId)
        {
            return Object.FindMergedEmailID(oldEmailId);
        }
    }
}
