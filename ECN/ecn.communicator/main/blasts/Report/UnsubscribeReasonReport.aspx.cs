using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.blasts.Report
{
    public partial class UnsubscribeReasonReport : System.Web.UI.Page
    {
        private static int TotalRowCount = 0;
        private static int PageCount = 0;
        private static int PageSize = 0;
        private static int CurrentPage;
        private static DateTime FromDate;
        private static DateTime ToDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;

            if (!Page.IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.UnsubscribeReasonReport, KMPlatform.Enums.Access.View))
                    throw new SecurityException();
                txtFromDate.Text = DateTime.Now.AddMonths(-3).ToShortDateString();
                txtToDate.Text = DateTime.Now.ToShortDateString();
                DateTime.TryParse(txtFromDate.Text, out FromDate);
                DateTime.TryParse(txtToDate.Text, out ToDate);

                PageSize = gvReasonDetails.PageSize;
                CurrentPage = gvReasonDetails.PageIndex;

                LoadPage();
            }
            else
            {
                DateTime.TryParse(txtFromDate.Text, out FromDate);
                DateTime.TryParse(txtToDate.Text, out ToDate);
            }

        }

        private List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason> LoadSummary()
        {
            gvSummary.ShowEmptyTable = true;
            gvSummary.EmptyTableRowText = "No data to display";

            string searchField = ddlSearchBy.SelectedValue.ToString();
            string searchCriteria = txtSearchCriteria.Text.Trim();

            List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason> listUnSub = ECN_Framework_BusinessLayer.Activity.Report.UnsubscribeReason.Get(searchField, searchCriteria, Master.UserSession.CurrentCustomer.CustomerID, FromDate, ToDate);

            gvSummary.DataSource = listUnSub;
            gvSummary.DataBind();
            return listUnSub;
        }

        private void LoadReasonDropDown(List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason> listUnSub)
        {
            DataTable dtReasons = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetReasons(Master.UserSession.CurrentCustomer.CustomerID, FromDate, ToDate);
            ECN_Framework_Entities.Accounts.LandingPageAssign lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(1, Master.UserSession.CurrentCustomer.CustomerID, true);
            List<string> listReasons = new List<string>();

            foreach (ECN_Framework_Entities.Activity.Report.UnsubscribeReason ur in listUnSub)
            {
                if (!listReasons.Contains(ur.SelectedReason))
                {
                    listReasons.Add(ur.SelectedReason);
                }
            }

            listReasons.Sort();
            ddlReason.DataSource = listReasons;
            ddlReason.DataBind();
            if (ddlReason.Items.Count > 0)
                ddlReason.Items.Insert(0, new ListItem() { Text = "All", Value = "", Selected = true });
            else
                ddlReason.Items.Insert(0, new ListItem() { Text = "No Reasons Found", Value = "", Selected = true });

        }

        private void LoadDetail()
        {
            gvReasonDetails.ShowEmptyTable = true;
            gvReasonDetails.EmptyTableRowText = "No data to display";

            int CurrentPage = gvReasonDetails.PageIndex + 1;
            string searchField = ddlSearchBy.SelectedValue.ToString();
            string searchCriteria = txtSearchCriteria.Text.Trim();
            string Reason = ("All".Equals(ddlReason.SelectedItem.Text, StringComparison.InvariantCultureIgnoreCase) || 
                             "No Reasons Found".Equals(ddlReason.SelectedItem.Text, StringComparison.InvariantCultureIgnoreCase) ? "" : ddlReason.SelectedItem.Text);

            var detailList = ECN_Framework_BusinessLayer.Activity.Report.UnsubscribeReasonDetail.GetPaging(
                searchField, searchCriteria, FromDate, ToDate, Master.UserSession.CurrentCustomer.CustomerID,
                Reason, PageSize, CurrentPage);

            foreach(DataRow dr in detailList.Tables[0].Rows)
            {
                dr["EmailSubject"] = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString());
            }

            TotalRowCount = 0;
            PageCount = 0;
            if (detailList != null && detailList.Tables[0] != null)
            {
                gvReasonDetails.DataSource = detailList.Tables[0];
                gvReasonDetails.DataBind();
                if (detailList.Tables[0].Rows.Count > 0)
                {
                    int.TryParse(detailList.Tables[0].Rows[0]["TotalCount"].ToString(), out TotalRowCount);
                    if (TotalRowCount > 0)
                    {
                        PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TotalRowCount) / PageSize));
                    }
                }
            }

            lblTotalRecords.Text = TotalRowCount.ToString();
            lblTotalNumberOfPagesGroup.Text = PageCount <= 1 ? "1" : PageCount.ToString();
            lblCurrentPage.Text = (gvReasonDetails.PageIndex + 1).ToString();
            //enable/disable prev/next buttons
            btnPreviousGroup.Enabled = false;
            btnNextGroup.Enabled = false;
            if (TotalRowCount > 0)
            {
                if (CurrentPage > 1)
                    btnPreviousGroup.Enabled = true;
                if (PageCount > CurrentPage)
                    btnNextGroup.Enabled = true;
            }
        }

        private void LoadPage()
        {
            gvReasonDetails.PageIndex = 0;
            List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason> listUnSub = LoadSummary();
            LoadReasonDropDown(listUnSub);
            LoadDetail();
        }

        private string Validate()
        {
            string error = string.Empty;
            System.Text.StringBuilder sbError = new System.Text.StringBuilder();
            if (FromDate.Year < 2000)
            {
                if (sbError.Length > 0)
                    sbError.Append("<br/>");
                sbError.Append("From Date is invalid");
            }
            if (ToDate.Year < 2000)
            {
                if (sbError.Length > 0)
                    sbError.Append("<br/>");
                sbError.Append("To Date is invalid");
            }
            if (FromDate > ToDate)
            {
                if (sbError.Length > 0)
                    sbError.Append("<br/>");
                sbError.Append("Date range is invalid");
            }
            if (ddlSearchBy.SelectedValue == "" && txtSearchCriteria.Text.Length > 0)
            {
                if (sbError.Length > 0)
                    sbError.Append("<br/>");
                sbError.Append("Search By selection is invalid for Search Text");
            }
            if (ddlSearchBy.SelectedValue != "" && txtSearchCriteria.Text.Length == 0)
            {
                if (sbError.Length > 0)
                    sbError.Append("<br/>");
                sbError.Append("Search Text is invalid for Search By");
            }
            return sbError.ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string error = Validate();
            if (error.Length == 0)
                LoadPage();
            else
            {
                setECNError(error);
                return;
            }
        }

        protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvReasonDetails.PageIndex = 0;
            DateTime fromDate = new DateTime();
            DateTime.TryParse(txtFromDate.Text, out fromDate);
            DateTime toDate = new DateTime();
            DateTime.TryParse(txtToDate.Text, out toDate);
            LoadDetail();
        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (gvReasonDetails.PageIndex > 0)
            {
                gvReasonDetails.PageIndex -= 1;
            }
            else
            {
                gvReasonDetails.PageIndex = 0;
            }
            LoadDetail();
        }
        
        protected void btnNextGroup_Click(object sender, EventArgs e)
        {
            if (gvReasonDetails.PageIndex < PageCount)
            {
                gvReasonDetails.PageIndex += 1;



            }
            else
            {
                gvReasonDetails.PageIndex = Convert.ToInt32(PageCount);
            }
            LoadDetail();
        }
        
        private void setECNError(string message)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";

            lblErrorMessage.Text = message;


        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string error = Validate();
            if (error.Length > 0)
            {
                setECNError(error);
                return;
            }
            else
            {

                gvReasonDetails.PageIndex = 0;
                int CurrentPage = gvReasonDetails.PageIndex + 1;
                string searchField = ddlSearchBy.SelectedValue.ToString();
                string searchCriteria = txtSearchCriteria.Text.Trim();
                string Reason = ("All".Equals(ddlReason.SelectedItem.Text, StringComparison.InvariantCultureIgnoreCase) ? "" : ddlReason.SelectedItem.Text);

                List<ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail> listUnsubDetail = ECN_Framework_BusinessLayer.Activity.Report.UnsubscribeReasonDetail.GetReport(
               searchField, searchCriteria, FromDate, ToDate, Master.UserSession.CurrentCustomer.CustomerID,
               Reason);

                foreach(var urd in listUnsubDetail)
                {
                    urd.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(urd.EmailSubject);
                }

                string tfile = Master.UserSession.CurrentUser.CustomerID + "-UnsubscribeReasonReport";

                if (ddlFormat.SelectedValue.ToUpper().Equals("XLSDATA"))
                {
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportCSV(listUnsubDetail, tfile);
                }                
                else if (ddlFormat.SelectedValue.ToUpper().Equals("PDF") || ddlFormat.SelectedValue.ToUpper().Equals("XLS"))
                {

                    Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", listUnsubDetail);
                    report.Visible = true;
                    report.LocalReport.DataSources.Clear();
                    report.LocalReport.DataSources.Add(rds);
                    report.LocalReport.ReportPath = Server.MapPath("UnsubscribeReasonDetail.rdlc");
                    report.LocalReport.Refresh();

                    Warning[] warnings = null;
                    string[] streamids = null;
                    String mimeType = null;
                    String encoding = null;
                    String extension = null;
                    Byte[] bytes = null;

                    if (ddlFormat.SelectedValue.ToUpper().Equals("PDF"))
                    {
                        bytes = report.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                        Response.ContentType = "application/pdf";
                    }
                    else
                    {
                        bytes = report.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                        Response.ContentType = "application/octet-stream";
                    }
                    

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=UnsubscribeReasonDetail." + ddlFormat.SelectedValue.ToLower());
                    Response.BinaryWrite(bytes);
                    Response.End();

                }
            }
        }
    }
}
