using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KMPS.MD.Objects;
using System.Web.UI.DataVisualization.Charting;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using Telerik.Web.UI;

namespace KMPS.MD.Main.Widgets
{
    public partial class OpenGrowth : System.Web.UI.UserControl, IWidgetControl
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

        private readonly string EntityName = "SubscriberOpenActivity";
        private readonly string Type = "Net";

        protected void Page_Load(object sender, EventArgs e)
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

        private void getdata(int startmonth, int startyear, int endmonth, int endyear)
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

            RadHtmlChart1.DataSource = KMPS.MD.Objects.Dashboard.CumulativeGrowth.Get(EntityName, Type, startmonth, startyear, endmonth, endyear);
            RadHtmlChart1.DataBind();

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