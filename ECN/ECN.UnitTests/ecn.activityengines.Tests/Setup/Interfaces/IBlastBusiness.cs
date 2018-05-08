using System.Data;
using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IBlastBusiness
    {
        BlastAbstract GetByBlastIDNoAccessCheck(int blastId, bool getChildren);

        DataTable GetHTMLPreview(int blastId, int emailId);
    }
}
