using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IGroupManager
    {
        bool Exists(int groupId, int customerId);
        bool IsArchived(int groupId, int customerId);
        void ValidateDynamicTags(int groupId, int layoutId, User user);
    }
}