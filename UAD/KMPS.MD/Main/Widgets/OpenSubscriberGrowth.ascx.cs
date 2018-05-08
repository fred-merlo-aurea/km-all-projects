using System;
using System.Web.UI;
using Kalitte.Dashboard.Framework;
using Kalitte.Dashboard.Framework.Types;
using KMPS.MD.Extensions;

namespace KMPS.MD.Main.Widgets
{
    public partial class OpenSubscriberGrowth : WidgetBase, IWidgetControl
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

                    GetData(startmonth, startyear, endmonth, endyear);
                }
            }
            catch
            { }

        }

        private void GetData(int startMonth, int startYear, int endMonth, int endYear)
        {
            RadHtmlChart1.BindCumulativeGrowth(ClientConnections, EntityName, Type, startMonth, startYear, endMonth, endYear);
        }

        protected void lbtnApplyFilters_Click(object sender, EventArgs e)
        {

        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            GetData(RadMonthYearPickerStart.SelectedDate.Value.Month, RadMonthYearPickerStart.SelectedDate.Value.Year, RadMonthYearPickerEnd.SelectedDate.Value.Month, RadMonthYearPickerEnd.SelectedDate.Value.Year);
        }
    }
}