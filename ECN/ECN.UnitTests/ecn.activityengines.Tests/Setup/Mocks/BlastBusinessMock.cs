using System.Data;
using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_Entities.Communicator;
using Moq;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlast;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class BlastBusinessMock : Mock<IBlastBusiness>
    {
        public BlastBusinessMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByBlastID_NoAccessCheckInt32Boolean = GetByBlastIDNoAccessCheck;
            Shim.GetHTMLPreviewInt32Int32 = GetHTMLPreview;
        }

        private DataTable GetHTMLPreview(int blastId, int emailId)
        {
            return Object.GetHTMLPreview(blastId, emailId);
        }

        private BlastAbstract GetByBlastIDNoAccessCheck(int blastId, bool getChildren)
        {
            return Object.GetByBlastIDNoAccessCheck(blastId, getChildren);
        }
    }
}
