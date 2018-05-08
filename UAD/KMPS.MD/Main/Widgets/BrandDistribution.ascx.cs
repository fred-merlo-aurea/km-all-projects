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
    public partial class BrandDistribution : System.Web.UI.UserControl, IWidgetControl
    {
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

        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        public int brandID
        {
            get
            {
                if (Request.QueryString["brandID"] != null)
                {
                    try
                    {
                        return int.Parse(Request.QueryString["brandID"].ToString());
                    }
                    catch {

                    }
                }

                return 0;
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
            try
            {
                if (!IsPostBack)
                {
                    RadHtmlChart1.Visible = true;
                    RadHtmlChart2.Visible = false;

                    RadGridBrand.DataSource = getBrandList();
                    RadGridBrand.DataBind();

                    if (brandID > 0)
                    {
                        try
                        {
                            foreach (GridDataItem item in RadGridBrand.MasterTableView.Items)
                            {
                                if (item.GetDataKeyValue("BrandID").ToString() == brandID.ToString())
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        RadGridBrand.MasterTableView.Items[0].Selected = true;
                    }

                    loadProductCounts();
                }
            }
            catch
            { }
        }

        protected void RadGridBrand_SelectedIndexChanged(object sender, EventArgs e)
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

        public DataTable getBrandList()
        {
            SqlCommand cmd = new SqlCommand("Dashboard_getBrandDistributionList");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userID", UserSession.UserID);
            cmd.Parameters.AddWithValue("@brandID", brandID);


            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections));
        }

        private void loadProductCounts()
        {
            try
            {
                rddlDimension.ClearSelection();

                int brandID = 0;

                try
                {
                    foreach (GridDataItem item in RadGridBrand.SelectedItems)
                    {
                        brandID = int.Parse(item.GetDataKeyValue("BrandID").ToString());
                    }
                }
                catch
                {

                }

                if (brandID > 0)
                {
                    rddlDimension.DataSource = MasterGroup.GetActiveByBrandID(clientconnections, brandID).FindAll(m => m.DisplayName.ToUpper() != "PUBCODE").OrderBy(o => o.DisplayName);
                    rddlDimension.DataBind();
                    rddlDimension.Items.Insert(0, new DropDownListItem("Select Dimension", "0"));
                    rddlDimension.SelectedIndex = -1;

                    RadHtmlChart1.DataSource = getBrandProductCounts(brandID);
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

        public DataTable getBrandProductCounts(int brandID)
        {
            SqlCommand cmd = new SqlCommand("Dashboard_getProductCounts_By_BrandID");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@brandID", brandID);
            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections)); ;
        }

        bool selectFirstItem = false;

        protected void RadGridBrand_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                rddlDimension.ClearSelection();

                RadHtmlChart1.Visible = true;
                RadHtmlChart2.Visible = false;

                RadGridBrand.CurrentPageIndex = e.NewPageIndex;

                RadGridBrand.DataSource = getBrandList();
                RadGridBrand.DataBind();

                selectFirstItem = true;
            }
            catch
            {

            }
        }

        protected void RadGridBrand_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (selectFirstItem)
                {
                    RadGridBrand.MasterTableView.Items[0].Selected = true;
                    loadProductCounts();
                }
            }
            catch
            { }
        }

        protected void RadGridBrand_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    Label lbl = (Label)e.Item.FindControl("lblLogo");
                    Image img = (Image)e.Item.FindControl("imgLogo");

                    if (lbl.Text == string.Empty)
                    {
                        img.Visible = false;
                    }
                    else
                    {
                        img.ImageUrl = "~/images/Logo/" + UserSession.CurrentCustomer.CustomerID.ToString() + "/" + lbl.Text;
                        img.CssClass = "smalllogo";
                    }
                }
            }
            catch
            { }
        }

        protected void rddlDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedbrandID = 0;

                try
                {
                    foreach (GridDataItem item in RadGridBrand.SelectedItems)
                    {
                        selectedbrandID = int.Parse(item.GetDataKeyValue("BrandID").ToString());
                    }
                }
                catch
                {

                }

                int masterGroupID = Convert.ToInt32(rddlDimension.SelectedValue);

                if (masterGroupID > 0 && selectedbrandID > 0)
                {
                    RadHtmlChart2.ChartTitle.Text = rddlDimension.SelectedText + " Counts";

                    RadHtmlChart2.DataSource = getBrandDimensionCounts(selectedbrandID, masterGroupID);
                    RadHtmlChart2.DataBind();

                    RadHtmlChart1.Visible = false;
                    RadHtmlChart2.Visible = true;
                }
                else
                {
                    if (selectedbrandID > 0)
                    {
                        RadHtmlChart1.DataSource = getBrandProductCounts(selectedbrandID);
                        RadHtmlChart1.DataBind();

                        RadHtmlChart1.Visible = true;
                        RadHtmlChart2.Visible = false;
                    }
                    else
                    {
                        RadHtmlChart1.PlotArea.Series.Clear();
                    }
                }
            }
            catch
            {

            }
        }

        public DataTable getBrandDimensionCounts(int brandID, int masterGroupID)
        {
            SqlCommand cmd = new SqlCommand("Dashboard_getDimensionCounts_By_BrandID_By_MasterGroupID");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.Parameters.AddWithValue("@MasterGroupID", masterGroupID);
            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections)); ;
        }
    }
}