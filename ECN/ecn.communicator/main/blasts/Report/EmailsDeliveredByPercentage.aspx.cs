using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using Microsoft.Reporting.WebForms;

namespace ecn.communicator.blasts.reports
{
    public partial class EmailsDeliveredByPercentage : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailDeliveredByPercentageReport, KMPlatform.Enums.Access.View))
                {
                    Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
                    Master.SubMenu = "email delivered";
                    Master.Heading = "";
                    Master.HelpContent = "";
                    Master.HelpTitle = "";

                    List<ECN_Framework_Entities.Accounts.Customer> customerList = new List<ECN_Framework_Entities.Accounts.Customer>();
                    //if (!(KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser)))
                    //if (!KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                    //{
                    //    throw new ECN_Framework_Common.Objects.SecurityException();
                    //}

                    LoadYear();

                    if (rbRange.SelectedItem.Value == "Week")
                    {
                        pnlWeek.Visible = true;
                    }
                    else
                    {
                        pnlMonth.Visible = true;
                    }
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        private void LoadYear()
        {
            //Year list can be changed by changing the lower and upper 
            //limits of the For statement    
            for (int intYear = DateTime.Now.Year - 10; intYear <= DateTime.Now.Year + 10; intYear++)
            {
                drpYear.Items.Add(intYear.ToString());
            }

            //Make the current year selected item in the list
            drpYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            if (rbRange.SelectedValue.Equals("Week"))
            {
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                DateTime.TryParse(txtFrom.Text, out startDate);
                DateTime.TryParse(txtTo.Text, out endDate);

                if (validateDateRange(startDate, endDate))
                {
                    lblDateError.Visible = false;
                    LoadReport();
                }
                else
                {
                    lblDateError.Visible = true;
                }
            }
            else if(rbRange.SelectedValue.Equals("Month"))
            {
                lblDateError.Visible = false;
                LoadReport();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtFrom.Text = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1)).ToString("MM/dd/yyyy");
            //rbRange.ClearSelection();
            pnlWeek.Visible = true;
            pnlMonth.Visible = false;
            pnlReport.Visible = false;
            lblDateError.Visible = false;
        }

        private void LoadReport()
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
           
            if (rbRange.SelectedItem.Value == "Week")
            {
                DateTime.TryParse(txtFrom.Text, out startDate);
                DateTime.TryParse(txtTo.Text, out endDate);
                     
            }
            else
            {
                DateTime.TryParse((drpMonth.SelectedItem.Value + "/01/" + drpYear.SelectedItem.Value), out startDate);
                DateTime.TryParse((drpMonth.SelectedItem.Value + "/" + DateTime.DaysInMonth(Convert.ToInt32(drpYear.SelectedItem.Value), Convert.ToInt32(drpMonth.SelectedItem.Value)) + "/" + drpYear.SelectedItem.Value), out endDate);
               
            }

            grdEmailsDelivered.DataSource = ECN_Framework_BusinessLayer.Activity.Report.EmailsDeliveredByPercentage.Get(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), startDate, endDate);
            grdEmailsDelivered.DataBind();

            pnlReport.Visible = true;

            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailDeliveredByPercentageReport, KMPlatform.Enums.Access.Download))
            {
                drpExport.Visible = true;
                btnDownload.Visible = true;
            }
            else
            {
                drpExport.Visible = false;
                btnDownload.Visible = false;
            }
        }

        protected void rbRange_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (rbRange.SelectedItem.Value == "Week")
            {
                pnlWeek.Visible = true;
                pnlMonth.Visible = false;
            }
            else
            {
                pnlMonth.Visible = true;
                pnlWeek.Visible = false;
            }
        }

        protected void grdEmailsDelivered_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdEmailsDelivered.PageIndex = e.NewPageIndex;
            }
            LoadReport();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            //DateTime.TryParse(txtstartDate.Text, out startDate);
            //DateTime.TryParse(txtendDate.Text, out endDate);

           // string fromDate = "";
            //string toDate = "";

            if (rbRange.SelectedItem.Value == "Week")
            {
                DateTime.TryParse(txtFrom.Text, out fromDate);
                DateTime.TryParse(txtTo.Text, out toDate);
             }
            else
            {
                string tempDateTo = drpMonth.SelectedItem.Value + "/" + DateTime.DaysInMonth(Convert.ToInt32(drpYear.SelectedItem.Value), Convert.ToInt32(drpMonth.SelectedItem.Value)) + "/" + drpYear.SelectedItem.Value;

                DateTime.TryParse((drpMonth.SelectedItem.Value + "/01/" + drpYear.SelectedItem.Value), out fromDate);
                DateTime.TryParse(tempDateTo, out toDate);

                //fromDate = drpMonth.SelectedItem.Value + "/01/" + drpYear.SelectedItem.Value;
                //toDate = drpMonth.SelectedItem.Value + "/" + DateTime.DaysInMonth(Convert.ToInt32(drpYear.SelectedItem.Value), Convert.ToInt32(drpMonth.SelectedItem.Value)) + "/" + drpYear.SelectedItem.Value;
            }

            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DS_EmailsDeliveredByPercentage", ECN_Framework_BusinessLayer.Activity.Report.EmailsDeliveredByPercentage.Get(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), fromDate, toDate));
            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("rpt_EmailsDeliveredByPercentage.rdlc");
            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("fromDate", Convert.ToString(fromDate));
            parameters[1] = new ReportParameter("toDate", Convert.ToString(toDate));
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportViewer1.LocalReport.Refresh();

            Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;

            switch (drpExport.SelectedItem.Value.ToUpper())
            {
                case "PDF":
                    bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/pdf";
                    break;
                case "XLS":
                    bytes = ReportViewer1.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/vnd.ms-excel";
                    break;
                case "DOC":
                    Response.ContentType = "application/ms-word";
                    bytes = ReportViewer1.LocalReport.Render("WORD", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    break;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=EmailsDelivered." + drpExport.SelectedItem.Value);
            Response.BinaryWrite(bytes);
            Response.End();
        }

        private static bool validateDateRange(DateTime startDate, DateTime endDate)
        {
            DateTime minDate = new DateTime(2000, 1, 1);

            if (startDate <= endDate && startDate > minDate && endDate > minDate)
            {
                return true;
            }
            else
                return false;

        }

    }
}