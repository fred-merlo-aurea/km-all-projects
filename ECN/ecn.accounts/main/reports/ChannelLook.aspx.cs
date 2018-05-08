using System;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Accounts.Report;
using System.Collections.Generic;
using ECN_Framework.Common;


namespace ecn.accounts.main.reports
{
    public partial class ChannelLook : ReportPageBase
    {
        private const string ReportTypeNoUsage = "2";

        private INoUsageProxy _noUsageProxy;
        private IChannelLookProxy _channelLookProxy;

        public ChannelLook(
            INoUsageProxy billingProxy,
            IChannelLookProxy channelLookProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _noUsageProxy = billingProxy;
            _channelLookProxy = channelLookProxy;
        }

        public ChannelLook()
        {
            _noUsageProxy = new NoUsageProxy();
            _channelLookProxy = new ChannelLookProxy();
        }

        protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
			{
				if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)) 
				{
					//txtstartDate.ReadOnly = true;
					//txtendDate.ReadOnly = true;

                    DataTable dtChannels = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select BaseChannelID, BaseChannelName From [BaseChannel] where IsDeleted = 0 order by BaseChannelName", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());

					drpChannel.DataSource = dtChannels;
					drpChannel.DataTextField = "BaseChannelName";
					drpChannel.DataValueField = "BaseChannelID";
					drpChannel.DataBind();

					drpChannel.Items.Insert(0,new ListItem("-- ALL --","0"));	
				}
				else
					Response.Redirect("/ecn.accounts/main/default.aspx");
			}
		}

        protected void Page_Unload(object sender, System.EventArgs e)
        {
        }

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
		    var channel = Convert.ToInt32(drpChannel.SelectedItem.Value);
            var dateFrom = txtstartDate.Text;
		    var dateTo = txtendDate.Text;
		    var customerId = getListboxValues(lstCustomer);
		    var outputType = drpExport.SelectedItem.Text.ToUpper();
            var parameters = new List<ReportParameter>
		    {
		        new ReportParameter("StartDate", txtstartDate.Text),
		        new ReportParameter("EndDate", txtendDate.Text)
		    };
		    ReportDataSource dataSource;
		    string filePath;
		    string outputFileName;

            if   (rbReportType.SelectedItem.Value == ReportTypeNoUsage) 
			{
			    var noUsageList = _noUsageProxy.Get(channel, customerId, dateFrom, dateTo);
			    dataSource = new ReportDataSource("DS_NoUsage", noUsageList);
			    filePath = Server.MapPath("~/main/reports/rpt_NoUsage.rdlc");
			    outputFileName = string.Format("NoUsage.{0}", drpExport.SelectedItem.Text);
			}
			else
			{
			    var channelLookList = _channelLookProxy.Get(channel, customerId, dateFrom, dateTo, chkTestBlastOnly.Checked);
			    dataSource = new ReportDataSource("DS_ChannelLook", channelLookList);
			    filePath = Server.MapPath("~/main/reports/rpt_ChannelLook.rdlc");
			    outputFileName = string.Format("ChannelLook.{0}", drpExport.SelectedItem.Text);

                parameters.Add(new ReportParameter("BlastDetails", chkIncludeBlstDetails.Checked ? "true" : "false"));
                ReportViewer1.LocalReport.SubreportProcessing += ReportViewer1_SubReport;
			}

		    ReportViewer1.Visible = false;
            CreateReportResponse(ReportViewer1, filePath, dataSource, parameters.ToArray(), outputType, outputFileName);
        }

        private void ReportViewer1_SubReport(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "rpt_ChannelLook_Details")
            {
                List<ChannelLook_Details> cd = ECN_Framework.Accounts.Report.ChannelLook_Details.Get(e.Parameters["CustomerID"].Values[0], txtstartDate.Text, txtendDate.Text, chkTestBlastOnly.Checked ? true : false);
                foreach(ChannelLook_Details cld in cd)
                {
                    cld.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(cld.EmailSubject);
                }
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DS_ChannelLook_Details", cd));
            }
        }

		private string getListboxValues(ListBox lst)
		{
			string selectedvalues = string.Empty;
			foreach (ListItem item in lst.Items)
			{
				if(item.Selected)
				{
					selectedvalues+= selectedvalues==string.Empty?item.Value:","+item.Value;
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
