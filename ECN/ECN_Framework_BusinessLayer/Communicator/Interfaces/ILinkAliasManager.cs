namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ILinkAliasManager
    {
        bool Exists(int customerId);
        bool Exists(int contentId, int customerId);
        bool Exists(int layoutId, string link);
    }
}