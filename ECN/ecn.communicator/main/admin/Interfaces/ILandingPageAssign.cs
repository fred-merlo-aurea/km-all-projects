using System.Data;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;

namespace Ecn.Communicator.Main.Admin.Interfaces
{
    public interface ILandingPageAssign
    {
        void Save(LandingPageAssign lpa, User user);
        DataTable GetPreviewParameters(int LPAID, int customerID);
        LandingPageAssign GetByBaseChannelID(int baseChannelID, int LPID);
    }
}
