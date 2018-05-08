using KMPlatform.Entity;

namespace Ecn.Communicator.Main.Lists.Interfaces
{
    public interface IMasterCommunicator
    {
        int GetCustomerID();
        int GetUserID();
        User GetCurrentUser();
    }
}