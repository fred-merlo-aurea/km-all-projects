using System;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Accounts.Report;
using ECN_Framework.Common;

namespace ecn.accounts.main.reports
{
    public partial class DigitalEdition : ReportPageBase
	{
	    private IDigitalEditionBillingProxy _billingProxy;

	    public DigitalEdition(IDigitalEditionBillingProxy billingProxy, IReportContentGenerator reportContentGenerator)
	        : base(reportContentGenerator)
	    {
	        _billingProxy = billingProxy;
	    }

	    public DigitalEdition()
	    {
	        _billingProxy = new DigitalEditionBillingProxy();
	    }

        protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
			{
				if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)) 
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

		private void  LoadYear()
		{
			//Year list can be changed by changing the lower and upper 
			//limits of the For statement    
			for(int intYear=DateTime.Now.Year-10;intYear<=DateTime.Now.Year+10;intYear++)
			{
				drpYear.Items.Add(intYear.ToString());    
			}

			//Make the current year selected item in the list
			drpYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
		}

        protected void btnSubmit_Click(object sender, EventArgs e)
	    {
	        int month;
	        int year;

            if (!Int32.TryParse(drpMonth.SelectedItem.Value, out month))
            {
                var exceptionMessage = string.Format(
                    "DEReport.btnSubmit_Click: Value '{0}' can not be converted to a proper month number.",
                    drpMonth.SelectedItem.Value);
                throw new InvalidOperationException(exceptionMessage);
	        }

	        if (!Int32.TryParse(drpYear.SelectedItem.Value, out year))
	        {
	            var exceptionMessage = string.Format(
	                "DEReport.btnSubmit_Click: Value '{0}' can not be converted to a proper year.",
	                drpYear.SelectedItem.Value);
                throw new InvalidOperationException(exceptionMessage);
	        }

            var billingList = _billingProxy.Get(month, year);
	        var dataSource = new ReportDataSource("DS_DigitalEditionBilling", billingList);
	        var parameters = new[]
	        {
	            new ReportParameter("Year", drpYear.SelectedItem.Value),
	            new ReportParameter("Month", drpMonth.SelectedItem.Value)
	        };

	        var filePath = Server.MapPath("~/main/reports/rpt_DigitalEdition.rdlc");
	        var outputType = drpExport.SelectedItem.Text.ToUpper();
	        var outputFileName = string.Format("DigitalEdition.{0}", drpExport.SelectedItem.Text);

	        ReportViewer1.Visible = false;

            CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
	    }
	}
}
