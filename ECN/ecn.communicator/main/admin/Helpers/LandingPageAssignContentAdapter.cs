using System.Collections.Generic;
using Ecn.Communicator.Main.Admin.Interfaces;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using BusinessLayerAccounts = ECN_Framework_BusinessLayer.Accounts;

namespace Ecn.Communicator.Main.Admin.Helpers
{
    public class LandingPageAssignContentAdapter : ILandingPageAssignContent
    {
        public void Delete(int LPAID, User user)
        {
            BusinessLayerAccounts.LandingPageAssignContent.Delete(LPAID, user);
        }

        public void Save(LandingPageAssignContent lpa, User user)
        {
            BusinessLayerAccounts.LandingPageAssignContent.Save(lpa, user);
        }

        public IList<LandingPageAssignContent> GetByLPAID(int lpaID)
        {
            return BusinessLayerAccounts.LandingPageAssignContent.GetByLPAID(lpaID);
        }
    }
}
