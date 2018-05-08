using System.Collections.Generic;
using KMPlatform.Entity;
using ECN_Framework_Entities.Communicator;

namespace Ecn.Communicator.Main.Lists.Interfaces
{
    public interface ISmartFormsHistory
    {
        int Save(SmartFormsHistory history, User user);

        IList<SmartFormsHistory> GetByGroupID(int groupID, User user);
        SmartFormsHistory GetBySmartFormID(int smartFormID, int groupID, User user);
    }
}