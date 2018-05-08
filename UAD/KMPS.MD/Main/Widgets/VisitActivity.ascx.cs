using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using System.Data.SqlClient;
using System.Data;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using Telerik.Web.UI;
using System.Text;

namespace KMPS.MD.Main.Widgets
{
    public partial class VisitActivity : System.Web.UI.UserControl, IWidgetControl
    {
        DashboardSurface surface;

        #region IWidgetControl Members

        public void Bind(WidgetInstance instance)
        {
            
        }

        public UpdatePanel[] Command(WidgetInstance instance, Kalitte.Dashboard.Framework.WidgetCommandInfo commandData, ref UpdateMode updateMode)
        {
            if (commandData.CommandName.ToUpper().Equals("SETTINGS"))
            {
            }
            else if (commandData.CommandName.ToUpper().Equals("REFRESH"))
            {
                rcbDomains.Items.Clear();

                rcbDomains.DataSource = DomainTracking.Get(clientconnections);
                rcbDomains.DataBind();

                RadDateStart.SelectedDate = DateTime.Now.AddDays(-7);
                RadDateEnd.SelectedDate = DateTime.Now;
                getdata();
            }
            return new UpdatePanel[] { UpdatePanelBrandTrends };
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
                    rcbDomains.Items.Clear();

                    rcbDomains.DataSource = DomainTracking.Get(clientconnections);
                    rcbDomains.DataBind();

                    RadDateStart.SelectedDate = DateTime.Now.AddDays(-7);
                    RadDateEnd.SelectedDate = DateTime.Now;
                    getdata();
                }
            }
            catch
            { }
        }

        private void getdata()
        {
            var sbDomains = new StringBuilder();

            try
            {
                if (rcbDomains.CheckedItems.Count > 0)
                {
                    foreach (var item in rcbDomains.CheckedItems)
                    {
                        if (sbDomains.ToString() == string.Empty)
                            sbDomains.Append(item.Value);
                        else
                            sbDomains.Append("," + item.Value);
                    }
                }

                int brandID = 0;

                if (Request.QueryString["brandID"] != null)
                {
                    try
                    {
                        brandID = int.Parse(Request.QueryString["brandID"].ToString());
                    }
                    catch { }
                }

                SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnections);
                SqlCommand cmd = new SqlCommand("Dashboard_GetVisitActivity_By_Domain_By_Range", conn);

                cmd.Parameters.AddWithValue("@startdate", RadDateStart.SelectedDate);
                cmd.Parameters.AddWithValue("@endDate", RadDateEnd.SelectedDate);
                cmd.Parameters.AddWithValue("@DomainIDs", sbDomains.ToString());
                cmd.Parameters.AddWithValue("@brandID", brandID);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                DataTable dtdata = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnections));

                DataColumn pivotColumn = dtdata.Columns["DomainTrackingID"];
                DataColumn pivotValue = dtdata.Columns["Counts"];

                DataTable dtPivotdata = Pivot(dtdata, pivotColumn, pivotValue);

                foreach (DomainTracking dt in DomainTracking.Get(clientconnections))
                {
                    try
                    {
                        DataColumn pvtColumn = dtPivotdata.Columns["dt" + dt.DomainTrackingID.ToString()];

                        if (pvtColumn != null)
                        {
                            LineSeries ls1 = new LineSeries();
                            ls1.Name = dt.DomainName;
                            ls1.LabelsAppearance.Visible = false;
                            ls1.TooltipsAppearance.DataFormatString = dt.DomainName + " - " + "{0:N0} ";
                            ls1.TooltipsAppearance.Color = System.Drawing.Color.White;
                            ls1.DataFieldY = "dt" + dt.DomainTrackingID.ToString();
                            RadHtmlChart1.PlotArea.Series.Add(ls1);
                        }
                    }
                    catch { }
                }

                RadHtmlChart1.DataSource = dtPivotdata;
                RadHtmlChart1.DataBind();
            }
            catch
            {

            }
        }

        DataTable Pivot(DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToArray();

            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();
            dt.AsEnumerable()
                .Select(r => r[pivotColumn.ColumnName].ToString())
                .Distinct().ToList()
                .ForEach(c => result.Columns.Add(c, pivotColumn.DataType));

            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                    pkColumnNames
                        .Select(c => row[c])
                        .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];
            }

            return result;
        }

        protected void lbtnApplyFilters_Click(object sender, EventArgs e)
        {

        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            RadHtmlChart1.PlotArea.Series.Clear();
            getdata();
        }
    }
}