using System.Collections.Generic;
using Ecn.Communicator.Main.Lists.Interfaces;
using KMPlatform.Entity;

namespace Ecn.Communicator.Main.Lists.Helpers
{
    public class SmartFormsHistoryAdapter : ISmartFormsHistory
    {
        public int Save(ECN_Framework_Entities.Communicator.SmartFormsHistory history, User user)
        {
            return ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.Save(history, user);
        }

        public IList<ECN_Framework_Entities.Communicator.SmartFormsHistory> GetByGroupID(int groupID, User user)
        {
            return ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.GetByGroupID(groupID, user);
        }

        public ECN_Framework_Entities.Communicator.SmartFormsHistory GetBySmartFormID(int smartFormID, int groupID, User user)
        {
            return ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.GetBySmartFormID(smartFormID, groupID, user);
        }
    }
}
