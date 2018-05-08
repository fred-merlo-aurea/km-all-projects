using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;

namespace KMPS.MD.Main.Widgets
{
    public partial class ClientProd : System.Web.UI.UserControl, IWidgetControl
    {
        DashboardSurface surface;
       
        #region IWidgetControl Members

        public void Bind(WidgetInstance instance)
        {
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }

        public UpdatePanel[] Command(WidgetInstance instance, Kalitte.Dashboard.Framework.WidgetCommandInfo commandData, ref UpdateMode updateMode)
        {
            if (commandData.CommandName.Equals("Settings"))
            {
                if (PnlChart.Visible)
                {
                    PnlSettings.Visible = true;
                    PnlChart.Visible = false;
                }
            }
            return new UpdatePanel[] { UpdatePanelClientProd };
        }

        public void InitControl(Kalitte.Dashboard.Framework.WidgetInitParameters parameters)
        {
            surface = parameters.Surface;
        }

        #endregion

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
            if (!IsPostBack)
            {
                lbProductType.DataSource = PubTypes.GetActive(clientconnections) ;
                lbProductType.DataBind();

                List<PubTypes> pubTypes = PubTypes.GetActive(clientconnections) .FindAll(x => x.SortOrder == 1 || x.SortOrder == 2 || x.SortOrder == 3);
                
                foreach (PubTypes pt in pubTypes)
                {
                    foreach (ListItem mylistvalue in lbProductType.Items)
                    {
                        if (Convert.ToInt32(mylistvalue.Value) == pt.PubTypeID)
                        {
                           mylistvalue.Selected = true;
                        }
                    }
                }
                CreateChart();
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (lbProductType.GetSelectedIndices().Count() != 3)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select any 3 Product Types";
                return;
            }

            KMPS.MD.Objects.ClientProds.DeleteCache(clientconnections);

            CreateChart();
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }

        private void CreateChart()
        {
            try
            {
               List<int> pubTypeID = new List<int>();

                foreach (ListItem mylistvalue in lbProductType.Items)
                {
                    if (mylistvalue.Selected)
                    {
                        pubTypeID.Add(Convert.ToInt32(mylistvalue.Value));
                    }
                }

                List<ClientProds> cplist = ClientProds.Get(clientconnections, pubTypeID[0], pubTypeID[1], pubTypeID[2]);
                
                if (cplist.Count > 0)
                {
                    string ImageUrl = "";

                    string F1name = cplist[0].Product;
                    string F2name = cplist[1].Product;
                    string F3name = cplist[2].Product;
                    int F1count = cplist[0].SubscriberCount;
                    int F2count = cplist[1].SubscriberCount;
                    int F3count = cplist[2].SubscriberCount;

                    string F1F2name = cplist[3].Product;
                    string F2F3name = cplist[4].Product;
                    string F1F3name = cplist[5].Product;
                    int F1F2 = cplist[3].SubscriberCount;
                    int F2F3 = cplist[4].SubscriberCount;
                    int F1F3 = cplist[5].SubscriberCount;

                    string F1F2F3name = cplist[6].Product;
                    int F1F2F3 = cplist[6].SubscriberCount;

                    int mxF1F2 = (F2count > F1count ? F2count : F1count);
                    int max = (mxF1F2 > F3count ? mxF1F2 : F3count);
                    if (max == 0)
                        max = 1;

                    int F1F2P = Convert.ToInt32((F1F2 * 100) / max);
                    int F2F3P = Convert.ToInt32((F2F3 * 100) / max);
                    int F1F3P = Convert.ToInt32((F1F3 * 100) / max);
                    int F1F2F3P = Convert.ToInt32((F1F2F3 * 100) / max);

                    //string height= (Convert.ToInt32((Int32.Parse(Request.QueryString["_height"].ToString()) / 2.8))).ToString();
                    //string width=  (Convert.ToInt32((Int32.Parse(Request.QueryString["_width"].ToString()) / 3.8))).ToString(); 
                    string height = "300";
                    string width = "500";


                    ImageUrl = "http://chart.googleapis.com/chart?cht=v&chd=t:" + Convert.ToInt32((F1count * 100) / max) + "," + Convert.ToInt32((F2count * 100) / max) + "," + Convert.ToInt32((F3count * 100) / max) + "," + F1F2P + "," + F1F3P + "," + F2F3P + "," + F1F2F3P + "&chs=" + width + "x" + height + "&chts=0000FF,25&chdl=" + F1name + " (" + F1count + ")|" + F2name + " (" + F2count + ")|" + F3name + " (" + F3count + ")|" + F1F2name + " (" + F1F2 + " - " + F1F2P + "%)|" + F2F3name + " (" + F2F3 + " - " + F2F3P + "%)|" + F1F3name + " (" + F1F3 + " - " + F1F3P + "%)|" + F1F2F3name + " (" + F1F2F3 + " - " + F1F2F3P + "%)&chdlp=b";

                    imgVenn.ImageUrl = ImageUrl;
                    imgVenn.Visible = true;
                    Label1.Visible = false;
                    lblMsg.Visible = false;
                }
                else
                {
                    imgVenn.Visible = false;
                    Label1.Text = "No Data";
                }
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        }
    }
}