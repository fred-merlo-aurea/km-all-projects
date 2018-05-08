using System;
using System.Data;
using Ecn.Communicator.Main.Admin.Interfaces;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;

namespace Ecn.Communicator.Main.Admin.Helpers
{
    public class LandingPageAssignAdapter : ILandingPageAssign
    {
        public LandingPageAssign GetByBaseChannelID(int baseChannelID, int LPID)
        {
            return ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(baseChannelID, LPID);
        }

        public DataTable GetPreviewParameters(int LPAID, int customerID)
        {
            return ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetPreviewParameters(LPAID, customerID);
        }

        public void Save(LandingPageAssign lpa, User user)
        {
            ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.Save(lpa, user);
        }
    }
}
