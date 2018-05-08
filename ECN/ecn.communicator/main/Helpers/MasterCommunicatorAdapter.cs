using System;
using Ecn.Communicator.Main.Interfaces;
using ECN_Framework_Common.Objects.Communicator;

namespace Ecn.Communicator.Main.Helpers
{
    public class MasterCommunicatorAdapter : IMasterCommunicator
    {
        private ecn.communicator.MasterPages.Communicator _master;

        public string SubMenu
        {
            get
            {
                return _master.SubMenu;
            }
            set
            {
                _master.SubMenu = value;
            }
        }

        public string Heading
        {
            set
            {
                _master.Heading = value;
            }
        }

        public Enums.MenuCode CurrentMenuCode
        {
            get
            {
                return _master.CurrentMenuCode;
            }
            set
            {
                _master.CurrentMenuCode = value;
            }
        }

        public MasterCommunicatorAdapter(ecn.communicator.MasterPages.Communicator master)
        {
            _master = master;
        }

        public int? GetBaseChannelID()
        {
            return _master.UserSession.CurrentCustomer.BaseChannelID;
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
