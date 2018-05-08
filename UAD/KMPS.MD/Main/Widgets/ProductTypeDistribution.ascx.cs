using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data;

namespace KMPS.MD.Main.Widgets
{
    public partial class ProductTypeDistribution : System.Web.UI.UserControl, IWidgetControl
    {
        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        DashboardSurface surface;

        #region IWidgetControl Members

        public void Bind(WidgetInstance instance)
        {

        }

        public UpdatePanel[] Command(WidgetInstance instance, Kalitte.Dashboard.Framework.WidgetCommandInfo commandData, ref UpdateMode updateMode)
        {
            if (commandData.CommandName.Equals("Settings"))
            {

            }
            return new UpdatePanel[] { UpdatePanel1 };
        }

        public void InitControl(Kalitte.Dashboard.Framework.WidgetInitParameters parameters)
        {
            surface = parameters.Surface;
        }

        #endregion

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
            try
            {
                if (!IsPostBack)
                {
                    RadHtmlChart1.Visible = true;
                    RadHtmlChart2.Visible = false;

                    RadGridProductType.DataSource = getProductTypeList();
                    RadGridProductType.DataBind();
                    RadGridProductType.MasterTableView.Items[0].Selected = true;

                    int brandID = 0;

                    if (Request.QueryString["brandID"] != null)
                    {
                        try
                        {
                            brandID = int.Parse(Request.QueryString["brandID"].ToString());
                        }
                        catch { }
                    }

                    if (brandID > 0)
                    {
                        rddlDimension.DataSource = MasterGroup.GetActiveByBrandID(clientconnections, brandID).FindAll(m => m.DisplayName.ToUpper() != "PUBCODE").OrderBy(o => o.DisplayName);
                    }
                    else
                    {
                        rddlDimension.DataSource = MasterGroup.GetActiveMasterGroupsSorted(clientconnections).FindAll(m => m.DisplayName.ToUpper() != "PUBCODE").OrderBy(o => o.DisplayName);
                    }

                    rddlDimension.DataBind();
                    rddlDimension.Items.Insert(0, new DropDownListItem("Select Dimension", "0"));
                    loadProductCounts();
                }
            }
            catch
            { }
        }

        protected void RadGridProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RadHtmlChart1.Visible = true;
                RadHtmlChart2.Visible = false;

                loadProductCounts();
            }
            catch
            { }
        }

        public DataTable getProductTypeList()
        {
            int brandID = 0;

            if (Request.QueryString["brandID"] != null)
            {
                try
                {
                    brandID = int.Parse(Request.QueryString["brandID"].ToString());
                }
                catch { }
            }

            SqlCommand cmd = new SqlCommand("Dashboard_getProductTypeDistributionList");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@brandID", brandID);

            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections));
        }

        private void loadProductCounts()
        {
            try
            {
                rddlDimension.ClearSelection();

                int PubTypeID = 0;

                try
                {
                    foreach (GridDataItem item in RadGridProductType.SelectedItems)
                    {
                        PubTypeID = int.Parse(item.GetDataKeyValue("PubTypeID").ToString());
                    }
                }
                catch
                {

                }

                if (PubTypeID > 0)
                {
                    RadHtmlChart1.DataSource = getProductTypeProductCounts(PubTypeID);
                    RadHtmlChart1.DataBind();

                    RadHtmlChart1.Visible = true;
                }
                else
                {

                    RadHtmlChart1.PlotArea.Series.Clear();
                    RadHtmlChart1.Visible = false;
                }
            }
            catch
            {

            }
        }

        public DataTable getProductTypeProductCounts(int PubTypeID)
        {
            int brandID = 0;

            if (Request.QueryString["brandID"] != null)
            {
                try
                {
                    brandID = int.Parse(Request.QueryString["brandID"].ToString());
                }
                catch { }
            }

            SqlCommand cmd = new SqlCommand("Dashboard_getProductCounts_By_PubTypeID");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PubTypeID", PubTypeID);
            cmd.Parameters.AddWithValue("@brandID", brandID);

            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections)); ;
        }

        bool selectFirstItem = false;


        protected void RadGridProductType_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (selectFirstItem)
                {
                    RadGridProductType.MasterTableView.Items[0].Selected = true;
                    loadProductCounts();
                }
            }
            catch
            { }
        }

        protected void RadGridProductType_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            try
            {
                rddlDimension.ClearSelection();

                RadHtmlChart1.Visible = true;
                RadHtmlChart2.Visible = false;

                RadGridProductType.DataSource = getProductTypeList();

                selectFirstItem = true;

                loadProductCounts();
            }
            catch
            { }
        }

        protected void rddlDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int pubtypeID = 0;

                try
                {
                    foreach (GridDataItem item in RadGridProductType.SelectedItems)
                    {
                        pubtypeID = int.Parse(item.GetDataKeyValue("PubTypeID").ToString());
                    }
                }
                catch
                {

                }

                int masterGroupID = Convert.ToInt32(rddlDimension.SelectedValue);

                if (masterGroupID > 0)
                {
                    RadHtmlChart2.ChartTitle.Text = rddlDimension.SelectedText + " Counts";

                    RadHtmlChart2.DataSource = getDimensionCounts(pubtypeID, masterGroupID);
                    RadHtmlChart2.DataBind();

                    RadHtmlChart1.Visible = false;
                    RadHtmlChart2.Visible = true;
                }
                else
                {
                    RadHtmlChart1.Visible = true;
                    RadHtmlChart2.Visible = false;

                    loadProductCounts();
                }
            }
            catch
            {

            }
        }

        public DataTable getDimensionCounts(int pubtypeID,  int masterGroupID)
        {
            int brandID = 0;

            if (Request.QueryString["brandID"] != null)
            {
                try
                {
                    brandID = int.Parse(Request.QueryString["brandID"].ToString());
                }
                catch { }
            }

            SqlCommand cmd = new SqlCommand("Dashboard_getDimensionCounts_By_PubTypeID");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pubtypeID", pubtypeID);
            cmd.Parameters.AddWithValue("@MasterGroupID", masterGroupID);
            cmd.Parameters.AddWithValue("@brandID", brandID);

            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections)); 
        }
    }
}