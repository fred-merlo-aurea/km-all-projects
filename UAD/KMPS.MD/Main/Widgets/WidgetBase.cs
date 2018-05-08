using System;
using System.Web.UI;
using KMPlatform.BusinessLogic;
using KMPlatform.Object;
using KMPS.MD.Objects;

namespace KMPS.MD.Main.Widgets
{
    public abstract class WidgetBase : UserControl
    {
        protected ECNSession _userSession = null;
        public ECNSession UserSession
        {
            get
            {
                return _userSession == null ? ECNSession.CurrentSession() : _userSession;
            }
        }

        protected ClientConnections _clientConnections = null;
        public ClientConnections ClientConnections
        {
            get
            {
                if (_clientConnections == null)
                {
                    var clientBusiness = new Client();
                    var client = clientBusiness.Select(UserSession.ClientID, true);
                    _clientConnections = new ClientConnections(client);
                }

                return _clientConnections;
            }
        }
    }
}