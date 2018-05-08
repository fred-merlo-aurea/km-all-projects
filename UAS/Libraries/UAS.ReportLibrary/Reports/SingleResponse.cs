namespace UAS.ReportLibrary.Reports
{
    using System;
    using System.Collections.Generic;

	/// <summary>
	/// Summary description for SingleResponse.
	/// </summary>
    public partial class SingleResponse : Telerik.Reporting.Report
	{
		public SingleResponse()
		{
			InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.Fields.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.DemoData.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.ReportParameters["Row"].AvailableValues.DataSource = this.Fields;
                this.CrossTabReport.DataSource = this.DemoData;
            }
            else
            {
                List<FrameworkUAD.Entity.ResponseGroup> responses = ReportUtilities.GetResponses(ReportUtilities.ProductId);
                this.ReportParameters["Row"].AvailableValues.DataSource = responses;
                this.ReportParameters["Row"].AvailableValues.DisplayMember = "DisplayName";
                this.ReportParameters["Row"].AvailableValues.ValueMember = "ResponseGroupName";

                this.CrossTabReport.DataSource = null;
                this.CrossTabReport.NeedDataSource += GetParameters;
            }
        }

        private void GetParameters(object sender, System.EventArgs e)
        {
            try
            {
                bool includeReportGroups = false;
                int productID = 0;
                int issueID = 0;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                //string filters = r.Parameters["Filters"].Value.ToString();
                string filterQuery = r.Parameters["FilterQuery"].Value.ToString();
                //string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                string row = r.Parameters["Row"].Value.ToString();
                int.TryParse(r.Parameters["ProductID"].Value.ToString(), out productID);
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                bool.TryParse(r.Parameters["ReportGroups"].Value.ToString(), out includeReportGroups);
                mytable.DataSource = ReportUtilities.GetSingleResponseData(filterQuery, issueID, row, includeReportGroups, productID);
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".GetParameters", app, "Reports");
            }
        }
    }
}