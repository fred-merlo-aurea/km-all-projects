using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlastLink;
using Moq;
using ECN_Framework_Entities.Communicator;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class BlastLinkMock : Mock<IBlastLink>
    {
        public BlastLinkMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByBlastLinkIDInt32Int32 = GetByBlastLinkID;
        }

        private BlastLink GetByBlastLinkID(int blastId, int blastLinkId)
        {
            return Object.GetByBlastLinkID(blastId, blastLinkId);
        }
    }
}
