using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IBlastLink
    {
        BlastLink GetByBlastLinkID(int blastId, int blastLinkId);
    }
}
