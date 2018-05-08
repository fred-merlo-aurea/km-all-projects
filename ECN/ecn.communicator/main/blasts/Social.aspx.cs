using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;


namespace ecn.communicator.blastsmanager
{
    public partial class Social : ECN_Framework.WebPageHelper
    {
        private const string SortField = "SortField";
        private const string SortDirection = "SortDirection";
        string blastsInGroup = "";
        int RecordCount = 0;
        private int _BlastID = 0;
        private int _CampaignItemID = 0;
        private int _SocialMediaID = 0;

        #region Get Request Variables
        private void GetValuesFromQuerystring(string queryString)
        {
            KM.Common.QueryString qs = KM.Common.QueryString.GetECNParameters(queryString);
            try
            {
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.BlastID).ParameterValue, out _BlastID);
            }
            catch (Exception)
            {
            }
            try
            {
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.CampaignItemID).ParameterValue, out _CampaignItemID);
            }
            catch (Exception)
            {
            }
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.SocialMediaID).ParameterValue, out _SocialMediaID);
        }
        #endregion

        #region Public Properties
        public string sortOrder
        {
            get
            {
                if (ViewState["sortOrder"].ToString() == "DESC")
                {
                    ViewState["sortOrder"] = "ASC";
                }
                else
                {
                    ViewState["sortOrder"] = "DESC";
                }

                return ViewState["sortOrder"].ToString();
            }
            set
            {
                ViewState["sortOrder"] = value;
            }
        }


        public string sortField
        {
            get
            {
                return ViewState["sortField"].ToString();
            }
            set
            {
                ViewState["sortField"] = value;
            }
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, System.EventArgs e)
        {
           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "Social Reporting";
            Master.HelpContent = "<p><b>Social</b><br />Lists all recepients who shared an email message and how many views that share received.";
            Master.HelpTitle = "Blast Manager";

            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
            string querystring = System.Web.HttpUtility.UrlDecode(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            string Decrypted = KM.Common.Encryption.Decrypt(querystring, ec);
            if (Decrypted != string.Empty)
            {
                GetValuesFromQuerystring(Decrypted);
            }

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "blastpriv") || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "viewreport") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))				
            {
                if (!(Page.IsPostBack))
                {
                    ViewState["SortField"] = "DisplayName";
                    ViewState["SortDirection"] = "DESC";
                    LoadSocialGrid();
                    LoadMediaTypes();
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        protected void SocialGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow dr = e.Row;
            if (dr.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink)e.Row.FindControl("hlMediaReporting");
                string txt = hl.Text;
                hl.Text = "<img src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + txt + "' alt='Social(Subscriber) Reporting for the Blast' border='0'>";
            }
        }

        protected void PreviewsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                PreviewsGrid.PageIndex = e.NewPageIndex;
            }
            PreviewsGrid.DataBind();
        }

        protected void PreviewsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecords = (Label)e.Row.FindControl("lblTotalRecords");
                lblTotalRecords.Text = RecordCount.ToString();

                Label lblTotalNumberOfPages = (Label)e.Row.FindControl("lblTotalNumberOfPages");
                lblTotalNumberOfPages.Text = PreviewsGrid.PageCount.ToString();

                TextBox txtGoToPage = (TextBox)e.Row.FindControl("txtGoToPage");
                txtGoToPage.Text = (PreviewsGrid.PageIndex + 1).ToString();

                DropDownList ddlPageSize = (DropDownList)e.Row.FindControl("ddlPageSize");
                ddlPageSize.SelectedValue = PreviewsGrid.PageSize.ToString();
            }
        }

        protected void PreviewsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.PreviewsGrid.PageSize = int.Parse(dropDown.SelectedValue);
        }

        public void PreviewsGrid_sortCommand(Object sender, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
        }

        protected void PreviewsGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortCommand(SortField, SortDirection, e.SortExpression);
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            LoadPreviewsGrid();
            LoadChartShares(false);
            LoadChartPreviews(false);
        }

        protected void GoToPage_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPage = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.PreviewsGrid.PageCount)
            {
                this.PreviewsGrid.PageIndex = pageNumber - 1;
            }
            else
            {
                this.PreviewsGrid.PageIndex = 0;
            }
        }

        protected void PreviewsPager_IndexChanged(object sender, System.EventArgs e)
        {
            LoadPreviewsGrid();
        }

        protected void ExportShares_Click(object sender, EventArgs e)
        {
            //ExportSharesReport(Response, dropdownExportShares.SelectedValue);
        }

        protected void ExportPreviews_Click(object sender, EventArgs e)
        {
            //ExportPreviewsReport(Response, dropdownExportPreviews.SelectedValue);
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (_BlastID > 0 ? "BlastID=" + _BlastID : "CampaignItemID=" + _CampaignItemID));
        }

        protected void ddlMediaType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //PreviewsGrid.CurrentPageIndex = 0;
            //PreviewsPager.CurrentPage = 1;
            //PreviewsPager.CurrentIndex = 0;

            //LoadPreviewsGrid();
        }

        public void DownloadEmails(string filterType, string downloadType)
        {
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");

            string newline = "";
            DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + _BlastID + "-social-share-emails" + downloadType;
            string outfileName = txtoutFilePath + tfile;

            if (!Directory.Exists(txtoutFilePath))
            {
                Directory.CreateDirectory(txtoutFilePath);
            }

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);

            List<ECN_Framework_Entities.Activity.Report.SocialDetail> detailList = null;
            if (_BlastID > 0)
            {
                detailList = ECN_Framework_BusinessLayer.Activity.Report.SocialDetail.GetSocialDetailByBlastID(_BlastID, Master.UserSession.CurrentCustomer.CustomerID);
                if (ddlMediaType.SelectedIndex != 0)
                {
                    detailList = detailList.Where(x => x.SocialMediaID == Convert.ToInt32(ddlMediaType.SelectedValue)).ToList();
                }
            }
            else
            {
                detailList = ECN_Framework_BusinessLayer.Activity.Report.SocialDetail.GetSocialDetailByCampaignItemID(_CampaignItemID, Master.UserSession.CurrentCustomer.CustomerID);
                if (ddlMediaType.SelectedIndex != 0)
                {
                    detailList = detailList.Where(x => x.SocialMediaID == Convert.ToInt32(ddlMediaType.SelectedValue)).ToList();
                }
            }

            if (detailList.Count > 0)
            {
                newline = "BlastID" + (downloadType == ".xls" ? "\t" : ", ");
                newline += "DisplayName" + (downloadType == ".xls" ? "\t" : ", ");
                newline += "EmailAddress" + (downloadType == ".xls" ? "\t" : ", ");
                newline += "Click" + (downloadType == ".xls" ? "\t" : ", ");
                txtfile.WriteLine(newline);

                foreach (ECN_Framework_Entities.Activity.Report.SocialDetail detail in detailList)
                {
                    newline = "";
                    string colData = "";
                    colData = detail.BlastID.ToString();
                    newline += colData + (downloadType == ".xls" ? "\t" : ", ");
                    colData = detail.DisplayName.ToString();
                    newline += colData + (downloadType == ".xls" ? "\t" : ", ");
                    colData = detail.EmailAddress.ToString();
                    newline += colData + (downloadType == ".xls" ? "\t" : ", ");
                    colData = detail.Click.ToString();
                    newline += colData + (downloadType == ".xls" ? "\t" : ", ");
                    txtfile.WriteLine(newline);
                }
                txtfile.Close();

                if (downloadType == ".xls")
                {
                    Response.ContentType = "application/vnd.ms-excel";
                }
                else
                {
                    Response.ContentType = "text/csv";
                }
                Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
                Response.WriteFile(outfileName);
                Response.Flush();
                Response.End();

            }
        }

        public void DownloadButton_Click(object sender, System.EventArgs e)
        {
            DownloadEmails(ddlMediaType.SelectedValue, DownloadType.SelectedItem.Value.ToString());
        }
        #endregion

        #region Methods
        private void InitializeComponent()
        {
            PreviewsGrid.Sorting += new GridViewSortEventHandler(PreviewsGrid_Sorting);
            PreviewsGrid.PageIndexChanging += new GridViewPageEventHandler(PreviewsGrid_PageIndexChanging);
            PreviewsGrid.RowDataBound += new GridViewRowEventHandler(PreviewsGrid_RowDataBound);
        }

        private void LoadSocialGrid()
        {
            if (_BlastID > 0)
            {
                SocialGrid.DataSource = ECN_Framework_BusinessLayer.Activity.Report.SocialSummary.GetSocialSummaryByBlastID(_BlastID, Master.UserSession.CurrentCustomer.CustomerID).Where(x => x.ID != 4 && x.ID != 5).ToList();
                SocialGrid.DataBind();
            }
            else
            {
                SocialGrid.DataSource = ECN_Framework_BusinessLayer.Activity.Report.SocialSummary.GetSocialSummaryByCampaignItemID(_CampaignItemID, Master.UserSession.CurrentCustomer.CustomerID).Where(x => x.ID != 4 && x.ID != 5).ToList();
                SocialGrid.DataBind();
            }
        }

        private void LoadMediaTypes()
        {
            List<ECN_Framework_Entities.Communicator.SocialMedia> socialMediaList= ECN_Framework_BusinessLayer.Communicator.SocialMedia.GetSocialMediaCanShare();
            foreach (ECN_Framework_Entities.Communicator.SocialMedia socialMedia in socialMediaList)
            {
                if(socialMedia.SocialMediaID != 4 && socialMedia.SocialMediaID != 5)
                ddlMediaType.Items.Add(new ListItem(socialMedia.DisplayName, socialMedia.SocialMediaID.ToString()));
            }
            ddlMediaType.Items.Insert(0, new ListItem("ALL", "*"));
            ddlMediaType.Items.FindByValue(_SocialMediaID.ToString()).Selected = true;
        }

        private void LoadPreviewsGrid()
        {
            List<ECN_Framework_Entities.Activity.Report.SocialDetail> detailList = null;
            if (_BlastID > 0)
            {
                detailList = ECN_Framework_BusinessLayer.Activity.Report.SocialDetail.GetSocialDetailByBlastID(_BlastID, Master.UserSession.CurrentCustomer.CustomerID);
                if (ddlMediaType.SelectedIndex != 0)
                {
                    detailList = detailList.Where(x => x.SocialMediaID == Convert.ToInt32(ddlMediaType.SelectedValue)).ToList();
                }
            }
            else
            {
                detailList = ECN_Framework_BusinessLayer.Activity.Report.SocialDetail.GetSocialDetailByCampaignItemID(_CampaignItemID, Master.UserSession.CurrentCustomer.CustomerID);
                if (ddlMediaType.SelectedIndex != 0)
                {
                    detailList = detailList.Where(x => x.SocialMediaID == Convert.ToInt32(ddlMediaType.SelectedValue)).ToList();
                }
            }

            RecordCount = detailList.Count();

            string sortField = ViewState["SortField"].ToString();
            string sortDirection = ViewState["SortDirection"].ToString();
            switch (sortField)
            {
                case "DisplayName":
                    if (sortDirection == "ASC")
                    {
                        detailList = detailList.OrderBy(x => x.DisplayName).ToList();
                    }
                    else
                    {
                        detailList = detailList.OrderByDescending(x => x.DisplayName).ToList();
                    }
                    break;
                case "EmailAddress":
                    if (sortDirection == "ASC")
                    {
                        detailList = detailList.OrderBy(x => x.EmailAddress).ToList();
                    }
                    else
                    {
                        detailList = detailList.OrderByDescending(x => x.EmailAddress).ToList();
                    }
                    break;
                case "Click":
                    if (sortDirection == "ASC")
                    {
                        detailList = detailList.OrderBy(x => x.Click).ToList();
                    }
                    else
                    {
                        detailList = detailList.OrderByDescending(x => x.Click).ToList();
                    }
                    break;
                default:
                    break;
            }

            PreviewsGrid.DataSource = detailList;

            try
            {
                PreviewsGrid.DataBind();
            }
            catch
            {
                PreviewsGrid.PageIndex = 0;
                PreviewsGrid.DataBind();
            }
            RecordCount = detailList.Count();
            PreviewsGrid.ShowEmptyTable = true;
            PreviewsGrid.EmptyTableRowText = "No list to display";
        }

        private void LoadChartShares(bool isExport)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialShareChart> chartList = null;
            if (_BlastID > 0)
            {
                chartList = ECN_Framework_BusinessLayer.Activity.Report.SocialShareChart.GetChartSharesByBlastID(_BlastID, Master.UserSession.CurrentCustomer.CustomerID);
            }
            else
            {
                chartList = ECN_Framework_BusinessLayer.Activity.Report.SocialShareChart.GetChartSharesByCampaignItemID(_CampaignItemID, Master.UserSession.CurrentCustomer.CustomerID);
            }
            try
            {
                chartShares.DataSource = chartList;

                chartShares.Series["Series1"].ChartType = SeriesChartType.Pie;
                chartShares.Series["Series1"].XValueMember = "DisplayName";
                chartShares.Series["Series1"].YValueMembers = "Share";
                chartShares.Series[0].Label = "#VALX";

                chartShares.ChartAreas[0].Area3DStyle.Enable3D = true;

                // Set pie labels to be outside the pie chart
                chartShares.Series[0]["PieLabelStyle"] = "Disabled";

                // Set border width so that labels are shown on the outside
                //chartBounceTypes.Series[0].BorderWidth = 1;
                //chartBounceTypes.Series[0].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);

                // Add a legend to the chart and dock it to the bottom-center
                chartShares.Legends.Add("Legend1");
                chartShares.Legends[0].Enabled = true;
                chartShares.Legends[0].Docking = Docking.Bottom;
                chartShares.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chartShares.Legends[0].IsEquallySpacedItems = true;
                chartShares.Legends[0].TextWrapThreshold = 0;
                chartShares.Legends[0].IsTextAutoFit = true;
                if (!isExport)
                {
                    chartShares.Legends[0].BackColor = System.Drawing.Color.Transparent;
                    chartShares.Legends[0].ShadowColor = System.Drawing.Color.Transparent;
                    chartShares.AntiAliasing = AntiAliasingStyles.Graphics;
                }
                else
                {
                    chartShares.Legends[0].BackColor = System.Drawing.Color.White;
                    chartShares.Legends[0].ShadowColor = System.Drawing.Color.White;
                    chartShares.AntiAliasing = AntiAliasingStyles.All;
                }

                // Show labels in the legend in the format "Name (### %)"
                chartShares.Series[0].LegendText = "#VALX (#PERCENT)";
                chartShares.DataBind();

            }
            catch (Exception ex)
            {
                LabelShares.Text = ex.Message;
            }
        }

        private void LoadChartPreviews(bool isExport)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialShareChart> chartList = null;
            if (_BlastID > 0)
            {
                chartList = ECN_Framework_BusinessLayer.Activity.Report.SocialShareChart.GetChartPreviewsByBlastID(_BlastID, Master.UserSession.CurrentCustomer.CustomerID);
            }
            else
            {
                chartList = ECN_Framework_BusinessLayer.Activity.Report.SocialShareChart.GetChartPreviewsByCampaignItemID(_CampaignItemID, Master.UserSession.CurrentCustomer.CustomerID);
            }
            try
            {
                chartPreviews.DataSource = chartList;

                chartPreviews.Series["Series1"].ChartType = SeriesChartType.Pie;
                chartPreviews.Series["Series1"].XValueMember = "DisplayName";
                chartPreviews.Series["Series1"].YValueMembers = "Share";
                chartPreviews.Series[0].Label = "#VALX";

                chartPreviews.ChartAreas[0].Area3DStyle.Enable3D = true;

                // Set pie labels to be outside the pie chart
                chartPreviews.Series[0]["PieLabelStyle"] = "Disabled";

                // Set border width so that labels are shown on the outside
                //chartBounceTypes.Series[0].BorderWidth = 1;
                //chartBounceTypes.Series[0].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);

                // Add a legend to the chart and dock it to the bottom-center
                chartPreviews.Legends.Add("Legend1");
                chartPreviews.Legends[0].Enabled = true;
                chartPreviews.Legends[0].Docking = Docking.Bottom;
                chartPreviews.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chartPreviews.Legends[0].IsEquallySpacedItems = true;
                chartPreviews.Legends[0].TextWrapThreshold = 0;
                chartPreviews.Legends[0].IsTextAutoFit = true;
                if (!isExport)
                {
                    chartPreviews.Legends[0].BackColor = System.Drawing.Color.Transparent;
                    chartPreviews.Legends[0].ShadowColor = System.Drawing.Color.Transparent;
                    chartPreviews.AntiAliasing = AntiAliasingStyles.Graphics;
                }
                else
                {
                    chartPreviews.Legends[0].BackColor = System.Drawing.Color.White;
                    chartPreviews.Legends[0].ShadowColor = System.Drawing.Color.White;
                    chartPreviews.AntiAliasing = AntiAliasingStyles.All;
                }

                // Show labels in the legend in the format "Name (### %)"
                chartPreviews.Series[0].LegendText = "#VALX (#PERCENT)";
                chartPreviews.DataBind();

            }
            catch (Exception ex)
            {
                LabelPreviews.Text = ex.Message;
            }
        }

        private void ExportSharesReport(HttpResponse response, string type)
        {

        }

        private void ExportPreviewsReport(HttpResponse response, string type)
        {

        }
        #endregion
    }
}