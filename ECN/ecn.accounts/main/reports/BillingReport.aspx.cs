using System;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Accounts.Report;
using ECN_Framework.Common;

namespace ecn.accounts.main.reports
{
    public partial class BillingReport : ReportPageBase
    {
        private IBillingProxy _billingProxy;
        private IBillingNoteProxy _billingNoteProxy;

        public BillingReport(
            IBillingProxy billingProxy, 
            IBillingNoteProxy billingNoteProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _billingProxy = billingProxy;
            _billingNoteProxy = billingNoteProxy;
        }

        public BillingReport()
        {
            _billingProxy = new BillingProxy();
            _billingNoteProxy = new BillingNoteProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
            {
                LoadYear();
                drpMonth.ClearSelection();

                drpMonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    DataTable dtChannels = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select BaseChannelID, BaseChannelName From [BaseChannel] where IsDeleted = 0 order by BaseChannelName", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());

                    drpChannel.DataSource = dtChannels;
                    drpChannel.DataTextField = "BaseChannelName";
                    drpChannel.DataValueField = "BaseChannelID";
                    drpChannel.DataBind();

                    drpChannel.Items.Insert(0, new ListItem("-- ALL --", "0"));
                }
                else
                    Response.Redirect("/ecn.accounts/main/default.aspx");
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

        protected void Page_Unload(object sender, System.EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var channel = Convert.ToInt32(drpChannel.SelectedItem.Value);
            var customerId = getListboxValues(lstCustomer);
            var month = Convert.ToInt32(drpMonth.SelectedItem.Value);
            var year = Convert.ToInt32(drpYear.SelectedItem.Value);

            var billingList = _billingProxy.Get(channel, customerId, month, year);
            var dataSource = new ReportDataSource("DS_Billing", billingList);
            var parameters = new []
            {
                new ReportParameter("month", drpMonth.SelectedItem.Value),
                new ReportParameter("year", drpYear.SelectedItem.Value)
            };

            var filePath = Server.MapPath("~/main/reports/rpt_BillingReport.rdlc");
            var outputType = drpExport.SelectedItem.Text.ToUpper();
            var outputFileName = string.Format(
                "BillingReport_{0}_{1}.{2}",
                drpMonth.SelectedItem.Text, 
                drpYear.SelectedItem.Value,
                drpExport.SelectedItem.Text);

            ReportViewer1.Visible = false;
            CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
        }

        protected void btnBillingNotes_Click(object sender, System.EventArgs e)
        {
            var billingNoteList = _billingNoteProxy.GetAll();
            var dataSource = new ReportDataSource("DS_BillingNote", billingNoteList);

            var filePath = Server.MapPath("~/main/reports/rpt_BillingNotes.rdlc");
            var outputType = drpExport.SelectedItem.Text.ToUpper();
            var outputFileName = string.Format("BillingNotes.{0}", drpExport.SelectedItem.Text);

            ReportViewer1.Visible = false;
            CreateReportResponse(ReportViewer1, filePath, dataSource, null, outputType, outputFileName);
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
            DataTable dtCustomers = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select CustomerID, CustomerName From [Customer] Where basechannelID = " + drpChannel.SelectedItem.Value + " and IsDeleted = 0 order by CustomerName", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());

            lstCustomer.DataSource = dtCustomers;
            lstCustomer.DataTextField = "CustomerName";
            lstCustomer.DataValueField = "CustomerID";
            lstCustomer.DataBind();
        }
    }
}
