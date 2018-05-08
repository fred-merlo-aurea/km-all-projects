namespace UAS.ReportLibrary.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data;

	/// <summary>
	/// Summary description for DemoXQualification.
	/// </summary>
    public partial class DemoXQualification : Telerik.Reporting.Report
	{
		public DemoXQualification()
		{
			InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.Main.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.Fields.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.IssueDates.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.ReportParameters["YearStart"].AvailableValues.DataSource = this.IssueDates;
                this.ReportParameters["YearEnd"].AvailableValues.DataSource = this.IssueDates;
                this.ReportParameters["Row"].AvailableValues.DataSource = this.Fields;
                this.crosstab1.DataSource = this.Main;
            }
            else
            {
                DataTable dt = ReportUtilities.GetIssueDates(ReportUtilities.ProductId);
                this.ReportParameters["YearStart"].AvailableValues.DataSource = dt;
                this.ReportParameters["YearStart"].AvailableValues.DisplayMember = "YearStartDate";
                this.ReportParameters["YearStart"].AvailableValues.ValueMember = "MonthDayStartDate";
                this.ReportParameters["YearStart"].Value = dt.Rows[0]["MonthDayStartDate"];
                this.ReportParameters["YearEnd"].AvailableValues.DataSource = dt;
                this.ReportParameters["YearEnd"].AvailableValues.DisplayMember = "YearEndDate";
                this.ReportParameters["YearEnd"].AvailableValues.ValueMember = "MonthDayEndDate";
                this.ReportParameters["YearEnd"].Value = dt.Rows[0]["MonthDayEndDate"];
                List<FrameworkUAD.Entity.ResponseGroup> responses = ReportUtilities.GetResponses(ReportUtilities.ProductId);
                this.ReportParameters["Row"].AvailableValues.DataSource = responses;
                this.ReportParameters["Row"].AvailableValues.DisplayMember = "DisplayName";
                this.ReportParameters["Row"].AvailableValues.ValueMember = "ResponseGroupName";
                this.crosstab1.DataSource = null;
                this.crosstab1.NeedDataSource += GetParameters;
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
                string filterQuery = r.Parameters["FilterQuery"].Value.ToString();
              
                string row = r.Parameters["Row"].Value.ToString();
                int.TryParse(r.Parameters["ProductID"].Value.ToString(), out productID);
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                bool.TryParse(r.Parameters["ReportGroups"].Value.ToString(), out includeReportGroups);

                mytable.DataSource = ReportUtilities.GetDemoXQualData(filterQuery, issueID, row, includeReportGroups, productID);
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