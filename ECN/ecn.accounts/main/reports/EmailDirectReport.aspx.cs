using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Accounts.Report;

namespace ecn.accounts.main.reports
{
    public partial class EmailDirectReport : ReportPageBase
    {
        private IEmailDirectReportProxy _emailDirectReportProxy;

        public EmailDirectReport(
            IEmailDirectReportProxy listSizeOvertimeReportProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _emailDirectReportProxy = listSizeOvertimeReportProxy;
        }

        public EmailDirectReport()
        {
            _emailDirectReportProxy = new EmailDirectReportProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
            {
                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    //txtstartDate.ReadOnly = true;
                    //txtendDate.ReadOnly = true;

                    List<ECN_Framework_Entities.Accounts.BaseChannel> bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                    drpChannel.DataSource = bc.OrderBy(x => x.BaseChannelName).ToList();
                    drpChannel.DataTextField = "BaseChannelName";
                    drpChannel.DataValueField = "BaseChannelID";
                    drpChannel.DataBind();

                    drpChannel.Items.Insert(0, new ListItem("-- ALL --", "0"));
                }
                else
                    Response.Redirect("/ecn.accounts/main/default.aspx");
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                DateTime start = new DateTime();
                DateTime.TryParse(txtstartDate.Text, out start);
                DateTime end = new DateTime();
                DateTime.TryParse(txtendDate.Text, out end);

                DataTable dt = _emailDirectReportProxy.Get(
                    Convert.ToInt32(drpChannel.SelectedValue),
                    getListboxValues(lstCustomer),
                    start,
                    end);
      
                var dataSource = new ReportDataSource("DS_EmailDirect", dt);
                var filePath = Server.MapPath("~/main/reports/rpt_EmailDirectReport.rdlc");
                var outputType = ReportConsts.OutputTypeXLS;
                var outputFileName = "EmailDirectReport.xls";
                var parameters = new[]
                {
                    new ReportParameter("StartDate", start.ToShortDateString()),
                    new ReportParameter("EndDate", end.ToShortDateString()),
                };

                ReportViewer1.Visible = true;
                CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
                
                HttpContext.Current.ApplicationInstance.CompleteRequest();
               
            }
            catch(Exception ex) 
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ecn.accounts.main.reports.EmailDirectReport.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Email Direct Report Failure");
            }
        }

        private string getListboxValues(ListBox lst)
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                {
                    selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
                }
            }
            return selectedvalues;
        }

        protected void drpChannel_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            List<ECN_Framework_Entities.Accounts.Customer> listC = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Convert.ToInt32(drpChannel.SelectedValue.ToString()));
            lstCustomer.DataSource = listC;
            lstCustomer.DataTextField = "CustomerName";
            lstCustomer.DataValueField = "CustomerID";
            lstCustomer.DataBind();
        }
    }
}