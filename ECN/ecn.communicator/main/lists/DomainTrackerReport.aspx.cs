using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using System.Data;

namespace ecn.communicator.main.lists
{
    public partial class DomainTrackerReport : System.Web.UI.Page
    {
        private int getDomainTrackerID()
        {
            if (Request.QueryString["domainTrackerID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["domainTrackerID"]);
            }
            else
                return -1;
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(getDomainTrackerID(), Master.UserSession.CurrentUser);
                lblDomainTracker.Text = "Domain Tracking Report (" + domainTracker.Domain + ")";
                GetTotalViews(); 
                GetURLStats();
                GetOSStats();
                GetBrowserStats();
                CreateChart();
            }
        }

        private void GetURLStats()
        {
            DateTime startDate = new DateTime();
            DateTime endDate = DateTime.Now;
            if (ddlMostVisitedPagesRange.SelectedValue == "-1")
            {
                startDate = DateTime.MinValue;
            }
            else if (ddlMostVisitedPagesRange.SelectedValue == "7")
            {
                startDate = endDate.AddDays(-7);
            }
            else if (ddlMostVisitedPagesRange.SelectedValue == "30")
            {
                startDate = endDate.AddDays(-30);
            }
            else if (ddlMostVisitedPagesRange.SelectedValue == "60")
            {
                startDate = endDate.AddDays(-60);
            }
            else if (ddlMostVisitedPagesRange.SelectedValue == "90")
            {
                startDate = endDate.AddDays(-90);
            }
            else if (ddlMostVisitedPagesRange.SelectedValue == "365")
            {
                startDate = endDate.AddDays(-365);
            }
            DataTable dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats(getDomainTrackerID(), startDate.ToString(), endDate.ToString(),"",0,"known");
            if (dt.Rows.Count > 0)
            {
                ddlMostVisitedPagesRange.Visible = true;
                lblMostVisitedPagesRange.Visible = true;
            }
            gvPageViews.DataSource = dt;
            gvPageViews.DataBind();
            CreateChart();

        }

        private void GetOSStats()
        {
            DataTable dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetOSStats(getDomainTrackerID());
            gvPlatformStats.DataSource = dt;
            gvPlatformStats.DataBind();
        }

        private void GetBrowserStats()
        {
            DataTable dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats(getDomainTrackerID());
            gvBrowserStats.DataSource = dt;
            gvBrowserStats.DataBind();
        }

        private void GetTotalViews()
        {
            DataTable dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(getDomainTrackerID());
            lblTotalViews.Text = "Total Page Views: " + dt.Rows[0][0].ToString();
            if (Convert.ToInt32(dt.Rows[0][0].ToString()) == 0)
            {
                lblPlatformStats.Visible = false;
                lblMostVisitedPages.Visible = false;
                lblBrowserStats.Visible = false;
            }
        }

        public void CreateChart()
        {
            try
            {
                chtPageViews.Series.Clear();
                chtPageViews.ChartAreas.Clear();
                chtPageViews.DataSource = null;
                DataTable dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetPageViewsPerDay(getDomainTrackerID());
                chtPageViews.DataSource = dt;
                chtPageViews.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                chtPageViews.BorderlineDashStyle = ChartDashStyle.Solid;
                chtPageViews.BorderWidth = 8;

                chtPageViews.Series.Add("PageViews");
                chtPageViews.Series["PageViews"].XValueMember = "Date";
                chtPageViews.Series["PageViews"].YValueMembers = "Views";
                chtPageViews.Series["PageViews"].ChartType = SeriesChartType.Spline;
                chtPageViews.Series["PageViews"].IsVisibleInLegend = true;
                chtPageViews.Series["PageViews"].ShadowOffset = 0;
                chtPageViews.Series["PageViews"].ToolTip = "#VALY{G}";
                chtPageViews.Series["PageViews"].BorderWidth = 3;
                chtPageViews.Series["PageViews"].Color = System.Drawing.Color.SteelBlue;
                chtPageViews.Series["PageViews"].IsValueShownAsLabel = true;
                chtPageViews.Series["PageViews"].MarkerSize = 10;
                chtPageViews.Series["PageViews"].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;

                chtPageViews.ChartAreas.Add("ChartArea1");
                chtPageViews.ChartAreas["ChartArea1"].AxisX.Title = "Date";
                chtPageViews.ChartAreas["ChartArea1"].AxisY.Title = "Page Views";
                chtPageViews.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                chtPageViews.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                chtPageViews.ChartAreas["ChartArea1"].AxisX.Interval = 3;
                chtPageViews.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.Transparent;
                chtPageViews.ChartAreas["ChartArea1"].ShadowColor = System.Drawing.Color.Transparent;
                chtPageViews.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtPageViews.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;

                chtPageViews.Height = 300;
                chtPageViews.Width = 600;
                chtPageViews.DataBind();
            }
            catch (Exception ex)
            {
                throwECNException(ex.Message);
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Blast, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void gvPlatformStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img = (Image)e.Row.FindControl("imgPlatform");
                Label lblOS = (Label)e.Row.FindControl("lblOS");
                if (lblOS.Text.ToLower().Contains("linux"))
                {
                    img.ImageUrl = "~/images/imgLinux.png";
                }
                else if (lblOS.Text.ToLower().Contains("windows"))
                {
                    img.ImageUrl = "~/images/imgWindows.png";
                }
                else if (lblOS.Text.ToLower().Contains("android"))
                {
                    img.ImageUrl = "~/images/imgAndroid.png";
                }
                else if (lblOS.Text.ToLower().Contains("ipad") || lblOS.Text.ToLower().Contains("mac") || lblOS.Text.ToLower().Contains("iphone"))
                {
                    img.ImageUrl = "~/images/imgApple.png";
                }
                else 
                {
                    img.ImageUrl = "~/images/imgOther.png";
                }
            }
        }

        protected void gvBrowserStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img = (Image)e.Row.FindControl("imgBrowser");
                Label lblBrowser = (Label)e.Row.FindControl("lblBrowser");
                if (lblBrowser.Text.ToLower().Contains("chrome"))
                {
                    img.ImageUrl = "~/images/imgChrome.png";
                }
                else if (lblBrowser.Text.ToLower().Contains("ie"))
                {
                    img.ImageUrl = "~/images/imgIE.png";
                }
                else if (lblBrowser.Text.ToLower().Contains("firefox"))
                {
                    img.ImageUrl = "~/images/imgFirefox.png";
                }
                else if (lblBrowser.Text.ToLower().Contains("safari"))
                {
                    img.ImageUrl = "~/images/imgSafari.png";
                }
                else if (lblBrowser.Text.ToLower().Contains("opera"))
                {
                    img.ImageUrl = "~/images/imgOpera.png";
                }
                else
                {
                    lblBrowser.Text = "Other";
                    img.ImageUrl = "~/images/imgOther.png";
                }
            }
        }

        protected void ddlMostVisitedPagesRange_IndexChanged(object sender, EventArgs e)
        {
            GetURLStats();
        }
    }
}