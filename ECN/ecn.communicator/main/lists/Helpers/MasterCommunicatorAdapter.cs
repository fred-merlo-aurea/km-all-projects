using Ecn.Communicator.Main.Lists.Interfaces;

namespace Ecn.Communicator.Main.Lists.Helpers
{
    public class MasterCommunicatorAdapter : IMasterCommunicator
    {
        private ecn.communicator.MasterPages.Communicator _master;
        public MasterCommunicatorAdapter(ecn.communicator.MasterPages.Communicator master)
        {
            _master = master;
        }

        public KMPlatform.Entity.User GetCurrentUser()
        {
            return _master.UserSession.CurrentUser;
        }

        public int GetCustomerID()
        {
            return _master.UserSession.CurrentUser.CustomerID;
        }

        public int GetUserID()
        {
            return _master.UserSession.CurrentUser.UserID;
        }
    }
}
