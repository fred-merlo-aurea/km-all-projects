namespace ReportLibrary.Reports
{
    using System;
    using System.Collections.Generic;

	/// <summary>
	/// Summary description for CrossTab.
	/// </summary>
    public partial class CrossTab : Telerik.Reporting.Report
	{
		public CrossTab()
		{
			InitializeComponent();
            if (ReportUtilities.Debug)
            {
                this.BusinessXFunction.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.SubReport.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.Fields.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.crosstab1.DataSource = this.BusinessXFunction;
                this.CrossSubReport.DataSource = this.SubReport;
                this.ReportParameters["Row"].AvailableValues.DataSource = this.Fields;
                this.ReportParameters["Col"].AvailableValues.DataSource = this.Fields;
            }
            else
            {
                List<FrameworkUAD.Entity.ResponseGroup> responses = ReportUtilities.GetResponses(ReportUtilities.ProductId);
                this.ReportParameters["Row"].AvailableValues.DataSource = responses;
                this.ReportParameters["Col"].AvailableValues.DataSource = responses;

                //when needed to set NeedDataSource be sure to hook up 1-1 methods
                this.crosstab1.DataSource = null;
                this.crosstab1.NeedDataSource += GetParameters;

                this.CrossSubReport.DataSource = null;
                this.CrossSubReport.NeedDataSource += GetParametersSubReport;
            }
		}

        private void GetParameters(object sender, System.EventArgs e)
        {
            try
            {
                bool includeAddRemove = false;
                bool includeReportGroups = false;
                int productID = 0;
                int issueID = 0;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                string filters = r.Parameters["Filters"].Value.ToString();
                string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                string row = r.Parameters["Row"].Value.ToString();
                string col = r.Parameters["Col"].Value.ToString();
                int.TryParse(r.Parameters["ProductID"].Value.ToString(), out productID);
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                bool.TryParse(r.Parameters["IncludeAddRemove"].Value.ToString(), out includeAddRemove);
                bool.TryParse(r.Parameters["ReportGroups"].Value.ToString(), out includeReportGroups);

                mytable.DataSource = ReportUtilities.GetCrossTabData(filters, adHocFilters, issueID, col, row, includeAddRemove, includeReportGroups, productID);
                //this.CrossSubReport.DataSource = ReportUtilities.GetDemoSubReportData(filters, adHocFilters, issueID, row, includeAddRemove, productID);
            }
            catch(Exception ex) 
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> appLogW = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                appLogW.Proxy.LogCriticalError(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, formatException, this.GetType().Name.ToString() + ".GetParameters", app, "Reports",
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
            }
        }

        private void GetParametersSubReport(object sender, System.EventArgs e)
        {
            try
            {
                bool includeAddRemove = false;
                bool includeReportGroups = false;
                int productID = 0;
                int issueID = 0;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                string filters = r.Parameters["Filters"].Value.ToString();
                string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                string row = r.Parameters["Row"].Value.ToString();
                string col = r.Parameters["Col"].Value.ToString();
                int.TryParse(r.Parameters["ProductID"].Value.ToString(), out productID);
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                bool.TryParse(r.Parameters["IncludeAddRemove"].Value.ToString(), out includeAddRemove);
                bool.TryParse(r.Parameters["ReportGroups"].Value.ToString(), out includeReportGroups);

                //mytable.DataSource = ReportUtilities.GetCrossTabData(filters, adHocFilters, issueID, col, row, includeAddRemove, includeReportGroups, productID);
                mytable.DataSource = ReportUtilities.GetDemoSubReportData(filters, adHocFilters, issueID, row, includeAddRemove, productID);
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> appLogW = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                appLogW.Proxy.LogCriticalError(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, formatException, this.GetType().Name.ToString() + ".GetParameters", app, "Reports",
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
            }
        }
    }
}