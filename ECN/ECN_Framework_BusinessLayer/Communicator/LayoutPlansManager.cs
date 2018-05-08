using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class LayoutPlansManager : ILayoutPlansManager
    {
        public bool Exists(int layoutPlanId, int customerId)
        {
            return LayoutPlans.Exists(layoutPlanId, customerId);
        }

        public bool Exists(int groupID, string criteria)
        {
            return LayoutPlans.Exists(groupID, criteria);
        }

        
    }
}
