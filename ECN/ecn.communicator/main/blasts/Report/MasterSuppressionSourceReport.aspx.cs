using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator.Enums;
using Reports = ECN_Framework_BusinessLayer.Activity.Report;
using ReportEntities = ECN_Framework_Entities.Activity.Report;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.blasts.reports
{
    
    public partial class MasterSuppressionSourceReport : ECN_Framework.WebPageHelper
    {
        private static int RowCount = 0;
        private static int PageCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = CommunicatorEnums.MenuCode.REPORTS;
            Master.SubMenu = "";
            Master.HelpContent = "";
            Master.HelpTitle = "Master Suppression Source Report";
            phError.Visible = false;

            if (!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.MasterSuppressionSourceReport, KMPlatform.Enums.Access.View))
                {
                    txtStartDate.Text = DateTime.Now.AddMonths(-3).ToShortDateString();
                    txtEndDate.Text = DateTime.Now.ToShortDateString();

                    LoadGrid();
                    if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.MasterSuppressionSourceReport, KMPlatform.Enums.Access.DownloadDetails))
                    {
                        pnlDownload.Visible = true;

                    }
                    else
                    {
                        pnlDownload.Visible = false;
                    }

                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();

            }
        }

        private void setECNError(string message)
        {
            phError.Visible = true;
            lblErrorMessage.Text = message;
        }

        private void LoadGrid()
        {
            MasterSuppressionSource.DataSource = null;
            MasterSuppressionSource.DataBind();

            ddlMasterSuppression.Items.Clear();

            MasterSuppressionSourceDetail.DataSource = null;
            MasterSuppressionSourceDetail.DataBind();

            DateTime startDate = new DateTime();
            if (!DateTime.TryParse(txtStartDate.Text, out startDate))
            {
                setECNError("Invalid start date");
                return;
            }

            DateTime endDate = new DateTime();
            if (!DateTime.TryParse(txtEndDate.Text, out endDate))
            {
                setECNError("Invalid end date");
                return;
            }

            //if (endDate < startDate)
            if (DateTime.Compare(startDate, endDate) > 0)
            {
                setECNError("Please enter an end date that occurs after or on your start date");
                return;
            }

            List<ReportEntities.MasterSuppressionSourceReport> msr = Reports.MasterSuppressionSourceReport.GetMaster(Master.UserSession.CurrentUser.CustomerID, startDate, endDate);

            foreach (var item in msr)
            {
                if (item.UnsubscribeCode == null)
                {
                    item.UnsubscribeCodeID = 999;
                }
            }

            MasterSuppressionSource.DataSource = msr;
            MasterSuppressionSource.DataBind();

            
            var allChoice = new ReportEntities.MasterSuppressionSourceReport();
            allChoice.UnsubscribeCode = "All";
            msr.Insert(0, allChoice);

            ddlMasterSuppression.DataSource = msr;
            ddlMasterSuppression.DataBind();

            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.MasterSuppressionSourceReport, KMPlatform.Enums.Access.ViewDetails))
            {
                pnlDetails.Visible = true;
                LoadDetailGridView(MasterSuppressionSourceDetail.SortExpression);
            }
            else
            {
                pnlDetails.Visible = false;
            }
        }

        protected void ddlMasterSuppression_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            MasterSuppressionSourceDetail.PageIndex = 0;
            LoadDetailGridView(MasterSuppressionSourceDetail.SortExpression);
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            RenderReport(drpExport.SelectedItem.Text);
        }

        private void RenderReport(string exportFormat)
        {
            lblRequired.Visible = false;
            if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                DateTime startDate = new DateTime();
                if (!DateTime.TryParse(txtStartDate.Text, out startDate))
                {
                    setECNError("Invalid start date");
                    return;
                }

                DateTime endDate = new DateTime();
                if (!DateTime.TryParse(txtEndDate.Text, out endDate))
                {
                    setECNError("Invalid end date");
                    return;
                }

                if (endDate < startDate)
                {
                    setECNError("Please enter an end date that occurs after or on your start date");
                    return;
                }

                List<ReportEntities.MasterSuppressionSourceReportDetails> lmsrd = Reports.MasterSuppressionSourceReportDetails.GetFilteredRecords(Master.UserSession.CurrentUser.CustomerID, ddlMasterSuppression.Items.Count > 0 ? ddlMasterSuppression.SelectedItem.Text : "", startDate, endDate);

                if (lmsrd != null)
                {
                    string tfile = Master.UserSession.CurrentUser.CustomerID + "-MasterSuppressionSourceReport";

                    if (drpExport.SelectedItem.Value.ToUpper() == "XLSDATA")
                    {
                        Reports.ReportViewerExport.ExportCSV(lmsrd, tfile);
                    }
                    else
                    {
                        List<string> fakeList = null;
                        Reports.ReportViewerExport.ExportToTab(Reports.ReportViewerExport.GetTabDelimited<ReportEntities.MasterSuppressionSourceReportDetails, string>(lmsrd,fakeList), tfile);
                    }
                }
            }
            else
            {
                lblRequired.Visible = true;
            }
        }

        protected void MasterSuppressionSourceDetail_Sorting(object sender, GridViewSortEventArgs e)
        {
            SwapSortDirection();
            MasterSuppressionSourceDetail.PageIndex = 0;
            LoadDetailGridView(e.SortExpression);
            ViewState["SortField"] = e.SortExpression;
        }

        private void LoadDetailGridView(string sortedColumn)
        {
            MasterSuppressionSourceDetail.DataSource = null;
            MasterSuppressionSourceDetail.DataBind();

            PageCount = 0;
            lblRequired.Visible = false;
            if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                DateTime startDate = new DateTime();
                if (!DateTime.TryParse(txtStartDate.Text, out startDate))
                {
                    MasterSuppressionSource.DataSource = null;
                    MasterSuppressionSource.DataBind();

                    ddlMasterSuppression.Items.Clear();

                    setECNError("Invalid start date");
                    return;
                }

                DateTime endDate = new DateTime();
                if (!DateTime.TryParse(txtEndDate.Text, out endDate))
                {
                    MasterSuppressionSource.DataSource = null;
                    MasterSuppressionSource.DataBind();

                    ddlMasterSuppression.Items.Clear();

                    setECNError("Invalid end date");
                    return;
                }

                if (endDate < startDate)
                {
                    MasterSuppressionSource.DataSource = null;
                    MasterSuppressionSource.DataBind();

                    ddlMasterSuppression.Items.Clear();

                    setECNError("Please enter an end date that occurs after or on your start date");
                    return;
                }

                if (string.IsNullOrEmpty(sortedColumn))
                {
                    sortedColumn = "EmailAddress";
                }

                int tempIndex = MasterSuppressionSourceDetail.PageIndex;
                var detailList = Reports.MasterSuppressionSourceReportDetails.GetDetails(Master.UserSession.CurrentUser.CustomerID, ddlMasterSuppression.SelectedItem.Text == string.Empty ? "Blank" : ddlMasterSuppression.SelectedItem.Text, MasterSuppressionSourceDetail.PageIndex + 1, MasterSuppressionSourceDetail.PageSize, SortDirection.ToString(), sortedColumn, startDate, endDate);
                
                int TotalRecordCount = -1;
                if (detailList != null && detailList.Tables[0].Rows.Count>0)
                {
                MasterSuppressionSourceDetail.DataSource = detailList.Tables[0];
                    int.TryParse(detailList.Tables[0].Rows[0]["TotalCount"].ToString(), out TotalRecordCount);
                if (TotalRecordCount > 0)
                {
                    PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TotalRecordCount) / 10));
                }
                }
                RowCount = TotalRecordCount;

                MasterSuppressionSourceDetail.DataBind();
                MasterSuppressionSourceDetail.Visible = true;

                MasterSuppressionSourceDetail.PageIndex = tempIndex;
                lblTotalNumberOfPagesGroup.Text = PageCount <= 1 ? "1" : PageCount.ToString();
                lblCurrentPage.Text = (MasterSuppressionSourceDetail.PageIndex + 1).ToString();

                if (MasterSuppressionSourceDetail.PageIndex >= 1)
                    btnPreviousGroup.Visible = true;
                else
                    btnPreviousGroup.Visible = false;

                if (MasterSuppressionSourceDetail.PageIndex < PageCount-1)
                    btnNextGroup.Visible = true;
                else
                    btnNextGroup.Visible = false;

                plPager.Visible = true;
            }
            else
            {
                lblRequired.Visible = true;
            }
        }

        private void SwapSortDirection()
        {
            if (SortDirection == SortDirection.Ascending)
            {
                SortDirection = SortDirection.Descending;
            }
            else
            {
                SortDirection = SortDirection.Ascending;
            }
        }

        /// <summary>
        /// Gets or sets the grid view sort direction.
        /// </summary>
        /// <value>
        /// The grid view sort direction.
        /// </value>
        public SortDirection SortDirection
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                {
                    ViewState["SortDirection"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["SortDirection"];
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        private string SortField
        {
            get
            {
                try
                {
                    return ViewState["SortField"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["SortField"] = value;
            }
        }

        protected void MasterSuppressionSourceDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                MasterSuppressionSourceDetail.PageIndex = e.NewPageIndex;
            }

            LoadDetailGridView(MasterSuppressionSourceDetail.SortExpression);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblRequired.Visible = false;
            if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                MasterSuppressionSourceDetail.DataSource = null;
                MasterSuppressionSourceDetail.DataBind();
                MasterSuppressionSourceDetail.Visible = false;
                btnPreviousGroup.Visible = false;
                btnNextGroup.Visible = false;
                plPager.Visible = false;
                LoadGrid();
            }
            else
            {
                lblRequired.Visible = true;
            }
        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if(MasterSuppressionSourceDetail.PageIndex > 0)
            {
                MasterSuppressionSourceDetail.PageIndex -= 1;
            }
            else
            {
                MasterSuppressionSourceDetail.PageIndex = 0;
            }
            LoadDetailGridView(SortField);
        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {
            if (MasterSuppressionSourceDetail.PageIndex < PageCount)
            {
                MasterSuppressionSourceDetail.PageIndex += 1;
            }
            else
            {
                MasterSuppressionSourceDetail.PageIndex = Convert.ToInt32(PageCount);
            }
            LoadDetailGridView(SortField);
        }
    }
}