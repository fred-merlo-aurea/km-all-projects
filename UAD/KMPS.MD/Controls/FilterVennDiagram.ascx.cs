using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Controls
{
    public partial class FilterVennDiagram : System.Web.UI.UserControl
    {
        public string VennParams
        {
            get
            {
                    return hidvennparam.Value;
            }
            set
            {
                hidvennparam.Value = value;
            }
        }

        public string VennDivID
        {
            get
            {
                return venn.ClientID;
            }
            
        }
        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        public KMPlatform.Object.ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(UserSession.ClientID, true);
                    _clientconnections = new KMPlatform.Object.ClientConnections(client);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void CreateVenn(Filters fc)
        {
            VennParams = fc.VennCords;
        }

        public void CreateVennForFilterSegmentation(FilterViews fv)
        {
            VennParams = fv.VennCords;
        }

        public void Clear()
        {
            VennParams = null;
        }

    }
}