using System;
using System.Data;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using System.Data.SqlClient;
using Telerik.Web.UI;

namespace KMPS.MD.Main.Widgets
{
    public partial class BrandNewSubscriber : System.Web.UI.UserControl, IWidgetControl
    {

        DashboardSurface surface;

        #region IWidgetControl Members

        public void Bind(WidgetInstance instance)
        {

        }

        public UpdatePanel[] Command(WidgetInstance instance, Kalitte.Dashboard.Framework.WidgetCommandInfo commandData, ref UpdateMode updateMode)
        {
            if (commandData.CommandName.Equals("SETTINGS"))
            {
            }
            else if (commandData.CommandName.Equals("REFRESH"))
            {
                RadDateStart.SelectedDate = DateTime.Now.AddMonths(-1);
                RadDateEnd.SelectedDate = DateTime.Now;

                RenderChart();
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
                    RadDateStart.SelectedDate = DateTime.Now.AddMonths(-1);
                    RadDateEnd.SelectedDate = DateTime.Now;

                    RenderChart();
                }
            }
            catch
            { }
        }

        private void RenderChart()
        {
            try
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

                if (brandID == 0)
                {
                    List<KMPS.MD.Objects.Dashboard.BrandGrowthReport> lPtg = KMPS.MD.Objects.Dashboard.BrandGrowthReport.Get(clientconnections, RadDateStart.SelectedDate, RadDateEnd.SelectedDate);

                    RadHtmlChartBNS.ChartTitle.Text = "Brand - New Subscriber Growth";

                    DonutSeries ds = (DonutSeries)RadHtmlChartBNS.PlotArea.Series[0];

                    ds.DataFieldY = "CountsPercentage";
                    ds.NameField = "BrandName";

                    ds.LabelsAppearance.DataField = "CountsPercentage";
                    ds.LabelsAppearance.DataFormatString = "{0} %";

                    ds.TooltipsAppearance.ClientTemplate = "#=dataItem.BrandName#: #=dataItem.Counts#";

                    RadHtmlChartBNS.DataSource = lPtg;
                    RadHtmlChartBNS.DataBind();

                    RadHtmlChartBNS.Legend.Appearance.Visible = chkShowLegend.Checked = (lPtg.Count <= 12);
                }
                else
                {
                    RenderProductChart(brandID);
                }

            }
            catch 
            {

            }
        }

        private void RenderProductChart(int BrandID)
        {
            try
            {
                RadHtmlChartBNS.ChartTitle.Text = "Product Counts";

                SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnections);
                SqlCommand cmd = new SqlCommand("Dashboard_getOriginatedPubcodeCounts_By_BrandID", conn);

                cmd.Parameters.AddWithValue("@BrandID", BrandID);
                cmd.Parameters.AddWithValue("@startdate", RadDateStart.SelectedDate);
                cmd.Parameters.AddWithValue("@enddate", RadDateEnd.SelectedDate);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                DataTable dtdata = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections));

                DonutSeries ds = (DonutSeries)RadHtmlChartBNS.PlotArea.Series[0];

                ds.DataFieldY = "CountsPercentage";
                ds.NameField = "PubName";

                ds.LabelsAppearance.DataField = "CountsPercentage";
                ds.LabelsAppearance.DataFormatString = "{0} %";
                ds.TooltipsAppearance.ClientTemplate = "#=dataItem.PubName# (#=dataItem.Pubcode#): #=dataItem.Counts#";

                RadHtmlChartBNS.DataSource = dtdata;
                RadHtmlChartBNS.DataBind();

                RadHtmlChartBNS.Legend.Appearance.Visible = chkShowLegend.Checked = (dtdata.Rows.Count <= 12);
            }
            catch
            {

            }
        }
        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            RenderChart();
        }


        protected void chkShowLegend_Checked(object sender, EventArgs e)
        {
            RadHtmlChartBNS.Legend.Appearance.Visible = chkShowLegend.Checked;
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                string brandname = e.Argument;

                int BrandID = Brand.GetAll(clientconnections).FirstOrDefault(b => b.BrandName.ToUpper() == brandname.ToUpper()).BrandID;

                RenderProductChart(BrandID);
            }
            catch
            {
                RenderChart();
            }
        }

    }
}