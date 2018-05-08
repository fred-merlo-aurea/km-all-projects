using System;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Accounts.Report;
using ECN_Framework.Common;

namespace ecn.accounts.main.reports
{
    public partial class ECNToday : ReportPageBase
    {
        private const string ReportTypeNewCustomer = "2";
        private const string ReportTypeNewUser = "3";

        private INewCustomerProxy _newCustomerProxy;
        private INewUserProxy _newUserProxy;
        private IECNTodayProxy _ecnTodayProxy;

        public ECNToday(
            INewCustomerProxy newCustomerProxy,
            INewUserProxy newUserProxy,
            IECNTodayProxy ecnTodayProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _newCustomerProxy = newCustomerProxy;
            _newUserProxy = newUserProxy;
            _ecnTodayProxy = ecnTodayProxy;
        }

        public ECNToday()
        {
            _newCustomerProxy = new NewCustomerProxy();
            _newUserProxy = new NewUserProxy();
            _ecnTodayProxy = new ECNTodayProxy();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
            {
                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    LoadYear();
                    drpMonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
                }
                else
                    Response.Redirect("/ecn.accounts/main/default.aspx");
            }
        }

        protected void Page_Unload(object sender, System.EventArgs e)
        {
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var month = Convert.ToInt32(drpMonth.SelectedItem.Value);
            var year = Convert.ToInt32(drpYear.SelectedItem.Value);
            var outputType = drpExport.SelectedItem.Text.ToUpper();
            var parameters = new []
            {
                new ReportParameter("Month", drpMonth.SelectedItem.Value),
                new ReportParameter("Year", drpYear.SelectedItem.Value)
            };
            ReportDataSource dataSource;
            string filePath;
            string outputFileName;

            if (rbReportType.SelectedItem.Value == ReportTypeNewCustomer)
            {
                var newCustomerList = _newCustomerProxy.Get(month, year, chkTestBlastOnly.Checked);
                dataSource = new ReportDataSource("DS_NewCustomer", newCustomerList);
                filePath = Server.MapPath("~/main/reports/rpt_NewCustomer.rdlc");
                outputFileName = string.Format("NewCustomer.{0}", drpExport.SelectedItem.Text);
            }
            else if (rbReportType.SelectedItem.Value == ReportTypeNewUser)
            {
                var newUserList = _newUserProxy.Get(month, year, chkTestBlastOnly.Checked);
                dataSource = new ReportDataSource("DS_NewUser", newUserList);
                filePath = Server.MapPath("~/main/reports/rpt_NewUser.rdlc");
                outputFileName = string.Format("NewUser.{0}", drpExport.SelectedItem.Text);
            }
            else
            {
                var newUserList = _ecnTodayProxy.Get(month, year, chkTestBlastOnly.Checked);
                dataSource = new ReportDataSource("DS_ECNToday", newUserList);
                filePath = Server.MapPath("~/main/reports/rpt_ECNToday.rdlc");
                outputFileName = string.Format("ECNToday.{0}", drpExport.SelectedItem.Text);
            }

            ReportViewer1.Visible = false;
            CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
        }
    }
}
