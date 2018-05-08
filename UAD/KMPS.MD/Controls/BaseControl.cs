using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using KMPlatform.BusinessLogic;
using KMPlatform.Object;
using KMPS.MD.Objects;
using Enums = KMPS.MD.Objects.Enums;

namespace KMPS.MD.Controls
{
    public class BaseControl : UserControl
    {
        private const string ViewStateBrandID = "BrandID";
        private const string ViewStatePubID = "PubID";
        private const string ViewStateUserID = "UserID";
        private const string ViewStateViewType = "ViewType";

        public int BrandID
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateBrandID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateBrandID] = value;
            }
        }

        public int PubID
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStatePubID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStatePubID] = value;
            }
        }

        public int UserID
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateUserID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateUserID] = value;
            }
        }

        public Enums.ViewType ViewType
        {
            get
            {
                Enums.ViewType viewType;
                var value = ViewState?[ViewStateViewType]?.ToString();

                if (value == null || !Enum.TryParse(value, out viewType))
                {
                    viewType = Enums.ViewType.None;
                }

                return viewType;
            }
            set { ViewState[ViewStateViewType] = value; }
        }

        private ECNSession _usersession = null;
        public ECNSession UserSession
        {
            get
            {
                return _usersession == null
                    ? ECNSession.CurrentSession()
                    : _usersession;
            }
        }

        private ClientConnections _clientconnections = null;
        public ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    var client = new Client().Select(UserSession.ClientID, true);
                    _clientconnections = new ClientConnections(client);
                    return _clientconnections;
                }
                else
                {
                    return _clientconnections;
                }
            }
        }
    }
}