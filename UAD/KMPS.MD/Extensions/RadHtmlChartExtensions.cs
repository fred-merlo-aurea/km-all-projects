using System;
using System.Diagnostics;
using System.Drawing;
using System.Web;
using KMPlatform.Object;
using KMPS.MD.Constants;
using Telerik.Web.UI;
using DashboardCumulativeGrowth = KMPS.MD.Objects.Dashboard.CumulativeGrowth;

namespace KMPS.MD.Extensions
{
    public static class RadHtmlChartExtensions
    {
        private const string NumberFormat = "{0:N0} ";
        private const string DataFieldCounts = "Counts";
        private const string MonthYearLabel = "MonthYearLabel";

        public static void BindCumulativeGrowth(
            this RadHtmlChart chart,
            ClientConnections clientConnection,
            string entityName,
            string type,
            int startMonth,
            int startYear,
            int endMonth,
            int endYear)
        {
            if (chart == null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            try
            {
                chart.PlotArea.Series.Clear();

                var lineSeries = new LineSeries();
                lineSeries.LabelsAppearance.Visible = false;
                lineSeries.LabelsAppearance.DataFormatString = NumberFormat;
                lineSeries.TooltipsAppearance.DataFormatString = NumberFormat;
                lineSeries.TooltipsAppearance.Color = Color.White;
                lineSeries.DataFieldY = DataFieldCounts;

                chart.PlotArea.Series.Add(lineSeries);
                chart.PlotArea.XAxis.DataLabelsField = MonthYearLabel;

                int brandId;
                int.TryParse(HttpContext.Current.Request.QueryString[QueryStringConsts.BrandId], out brandId);
                var dataSource = DashboardCumulativeGrowth.Get(clientConnection, entityName, type, startMonth, startYear, endMonth, endYear, brandId);
                chart.DataSource = dataSource;
                chart.DataBind();
            }
            catch (Exception ex)
            {
                // From legacy code, error ignored. Only log to Debug stream.
                Debug.WriteLine("Unexpected error in {0}: {1}", nameof(BindCumulativeGrowth), ex);
            }
        }
    }
}