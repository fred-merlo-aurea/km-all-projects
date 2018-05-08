using System;
using System.Web.UI;
using Kalitte.Dashboard.Framework;
using Kalitte.Dashboard.Framework.Types;
using KMPS.MD.Objects.Dashboard;
using Telerik.Web.UI;

namespace KMPS.MD.Main.Widgets
{
    public partial class ClickSubscriberGrowth : WidgetBase, IWidgetControl
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

        private readonly string EntityName = "SubscriberClickActivity";
        private readonly string Type = "UniqueSubscriber";

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

        private void getdata(int startmonth, int startyear, int endmonth, int endyear)
        {
            try
            {
                RadHtmlChart1.PlotArea.Series.Clear();

                LineSeries cs1 = new LineSeries();
                cs1.LabelsAppearance.Visible = false;
                cs1.LabelsAppearance.DataFormatString = "{0:N0} ";
                cs1.TooltipsAppearance.DataFormatString = "{0:N0} ";
                cs1.TooltipsAppearance.Color = System.Drawing.Color.White;
                cs1.DataFieldY = "Counts";
                RadHtmlChart1.PlotArea.Series.Add(cs1);

                RadHtmlChart1.PlotArea.XAxis.DataLabelsField = "MonthYearLabel";

                int brandID = 0;

                if (Request.QueryString["brandID"] != null)
                {
                    try
                    {
                        brandID = int.Parse(Request.QueryString["brandID"].ToString());
                    }
                    catch { }
                }

                RadHtmlChart1.DataSource = CumulativeGrowth.Get(ClientConnections, EntityName, Type, startmonth, startyear, endmonth, endyear, brandID);
                RadHtmlChart1.DataBind();
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        }

        protected void lbtnApplyFilters_Click(object sender, EventArgs e)
        {

        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            getdata(RadMonthYearPickerStart.SelectedDate.Value.Month, RadMonthYearPickerStart.SelectedDate.Value.Year, RadMonthYearPickerEnd.SelectedDate.Value.Month, RadMonthYearPickerEnd.SelectedDate.Value.Year);
        }
    }
}