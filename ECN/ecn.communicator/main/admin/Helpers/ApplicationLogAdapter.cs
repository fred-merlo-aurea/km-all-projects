using System.Collections.Generic;
using Ecn.Communicator.Main.Admin.Interfaces;
using ECN_Framework_Entities.Accounts;
using BusinessLayerAccounts = ECN_Framework_BusinessLayer.Accounts;

namespace Ecn.Communicator.Main.Admin.Helpers
{
    public class LandingPageOptionAdapter : ILandingPageOption
    {
        public IList<LandingPageOption> GetByLPID(int landingPageID)
        {
            return BusinessLayerAccounts.LandingPageOption.GetByLPID(landingPageID);
        }
    }
}
