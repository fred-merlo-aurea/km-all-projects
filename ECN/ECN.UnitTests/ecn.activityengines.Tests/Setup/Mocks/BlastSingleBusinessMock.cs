using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlastSingle;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class BlastSingleBusinessMock : Mock<IBlastSingleBusiness>
    {
        public BlastSingleBusinessMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetRefBlastIDInt32Int32Int32String = GetRefBlastID;
        }

        private int GetRefBlastID(
            int blastId,
            int emailId,
            int customerId,
            string blastType)
        {
            return Object.GetRefBlastID(blastId, emailId, customerId, blastType);
        }
    }
}
