using System.Collections.Generic;
using ECN_Framework_Entities.Accounts;

namespace Ecn.Communicator.Main.Admin.Interfaces
{
    public interface ILandingPageOption
    {
        IList<LandingPageOption> GetByLPID(int LPID);
    }
}
