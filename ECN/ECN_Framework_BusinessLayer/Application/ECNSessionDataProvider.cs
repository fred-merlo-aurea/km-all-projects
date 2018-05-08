using ECN_Framework_BusinessLayer.Application.Interfaces;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Application
{
    public class ECNSessionDataProvider : ISessionDataProvider
    {
        private ECNSession _ecnSession;

        public ECNSessionDataProvider(ECNSession ecnSession)
        {
            _ecnSession = ecnSession;
        }

        public int GetCustomerId()
        {
            return _ecnSession.CurrentUser.CustomerID;
        }

        public User GetCurrentUser()
        {
            return _ecnSession.CurrentUser;
        }
    }
}
