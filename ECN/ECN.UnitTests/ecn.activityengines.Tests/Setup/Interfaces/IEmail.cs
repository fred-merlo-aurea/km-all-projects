using System.Data;
using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IEmail
    {
        DataTable GetColumnNames();

        Email GetByEmailIDNoAccessCheck(int emailId);
    }
}
