using System;
using System.Data;
using System.Web.UI.WebControls;
using ecn.common.classes;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Accounts.Report;
using ECN_Framework.Common;

namespace ecn.accounts.main.reports
{
    public partial class DiskSpace : ReportPageBase
    {
        private IDiskMonitorProxy _diskMonitorProxy;

        public DiskSpace(IDiskMonitorProxy diskMonitor, IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _diskMonitorProxy = diskMonitor;
        }

        public DiskSpace()
        {
            _diskMonitorProxy = new DiskMonitorProxy();;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
            {

                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    DataTable dtChannels = DataFunctions.GetDataTable("select BaseChannelID, BaseChannelName From [BaseChannel] where IsDeleted = 0 order by BaseChannelName");

                    drpChannel.DataSource = dtChannels;
                    drpChannel.DataTextField = "BaseChannelName";
                    drpChannel.DataValueField = "BaseChannelID";
                    drpChannel.DataBind();

                    drpChannel.Items.Insert(0, new ListItem("-- ALL --", "0"));

                    drpMonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
                }
                else
                    Response.Redirect("/ecn.accounts/main/default.aspx");
            }
        }

        protected void Page_Unload(object sender, System.EventArgs e)
        {
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var channel = Convert.ToInt32(drpChannel.SelectedItem.Value);
            var month = Convert.ToInt32(drpMonth.SelectedItem.Value);

            var diskMonitorList = _diskMonitorProxy.Get(channel, month, chkOverLimit.Checked);
            var dataSource = new ReportDataSource("DS_DiskMonitor", diskMonitorList);
            var parameters = new[]
            {
                new ReportParameter("Month", drpMonth.SelectedItem.Value)
            };

            var filePath = Server.MapPath("~/main/reports/rpt_DiskMonitor.rdlc");
            var outputType = drpExport.SelectedItem.Text.ToUpper();
            var outputFileName = string.Format("DiskUsage.{0}", drpExport.SelectedItem.Text);

            ReportViewer1.Visible = false;

            CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
        }
    }
}
