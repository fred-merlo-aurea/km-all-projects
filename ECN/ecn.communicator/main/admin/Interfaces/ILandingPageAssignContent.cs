using System.Collections.Generic;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;

namespace Ecn.Communicator.Main.Admin.Interfaces
{
    public interface ILandingPageAssignContent
    {
        void Save(LandingPageAssignContent lpa, User user);
        void Delete(int LPAID, User user);
        IList<LandingPageAssignContent> GetByLPAID(int LPAID);
    }
}
