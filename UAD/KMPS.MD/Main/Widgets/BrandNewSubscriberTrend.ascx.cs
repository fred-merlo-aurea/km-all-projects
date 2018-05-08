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

namespace KMPS.MD.Main.Widgets
{
    public partial class BrandNewSubscriberTrend : System.Web.UI.UserControl, IWidgetControl
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
                    int startmonth = System.DateTime.Now.AddMonths(-12).Month;
                    int startyear = System.DateTime.Now.AddMonths(-12).Year;
                    int endmonth = System.DateTime.Now.Month;
                    int endyear = System.DateTime.Now.Year;

                    RadMonthYearPickerStart.SelectedDate = new DateTime(startyear, startmonth, 1);
                    RadMonthYearPickerEnd.SelectedDate = new DateTime(endyear, endmonth, 1);

                    getdata(startmonth, startyear, endmonth, endyear);
                }
            }
            catch
            { }
        }

        private void getdata(int StartMonth, int StartYear, int EndMonth, int EndYear)
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

                SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnections);
                SqlCommand cmd = new SqlCommand("Dashboard_BrandNewSubscriberTrend_By_Range", conn);

                cmd.Parameters.AddWithValue("@startmonth", StartMonth);
                cmd.Parameters.AddWithValue("@startyear", StartYear);
                cmd.Parameters.AddWithValue("@endmonth", EndMonth);
                cmd.Parameters.AddWithValue("@endyear", EndYear);
                cmd.Parameters.AddWithValue("@brandID", brandID);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                DataTable dtdata = DataFunctions.getDataTable(cmd, conn);

                DataColumn pivotColumn = dtdata.Columns["BrandID"];
                DataColumn pivotValue = dtdata.Columns["Counts"];

                DataTable dtPivotdata = Pivot(dtdata, pivotColumn, pivotValue);

                List<Brand> brands = new List<Brand>();

                if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser))
                {
                    brands = Brand.GetByUserID(clientconnections, UserSession.CurrentUser.UserID);
                }

                if (brands.Count == 0)
                {
                    brands = Brand.GetAll(clientconnections).Where(b => b.IsDeleted == false).ToList();
                }

                foreach (Brand b in Brand.GetAll(clientconnections).Where(b => b.IsDeleted == false))
                {
                    try
                    {
                        DataColumn pvtColumn = dtPivotdata.Columns["b" + b.BrandID.ToString()];

                        if (pvtColumn != null)
                        {
                            LineSeries ls1 = new LineSeries();
                            ls1.Name = b.BrandName;
                            ls1.LabelsAppearance.Visible = false;
                            ls1.LabelsAppearance.DataFormatString = b.BrandName + " - {0:N0}";
                            ls1.TooltipsAppearance.DataFormatString = "{0:N0}";
                            ls1.TooltipsAppearance.Color = System.Drawing.Color.White;
                            ls1.DataFieldY = "b" + b.BrandID.ToString();
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
            try
            {
                RadHtmlChart1.PlotArea.Series.Clear();
                getdata(RadMonthYearPickerStart.SelectedDate.Value.Month, RadMonthYearPickerStart.SelectedDate.Value.Year, RadMonthYearPickerEnd.SelectedDate.Value.Month, RadMonthYearPickerEnd.SelectedDate.Value.Year);
            }
            catch
            {

            }
        }
    }
}