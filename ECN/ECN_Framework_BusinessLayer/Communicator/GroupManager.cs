using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class GroupManager : IGroupManager
    {
        public bool Exists(int groupId, int customerId)
        {
            return Group.Exists(groupId, customerId);
        }

        public bool IsArchived(int groupId, int customerId)
        {
            return Group.IsArchived(groupId, customerId);
        }

        public void ValidateDynamicTags(int groupId, int layoutId, User user)
        {
            Group.ValidateDynamicTags(groupId, layoutId, user);
        }
    }
}
