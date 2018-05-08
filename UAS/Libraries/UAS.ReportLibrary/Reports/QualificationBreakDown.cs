namespace UAS.ReportLibrary.Reports
{
    using System;
    using System.Data;

	/// <summary>
	/// Summary description for QualificationBreakDown.
	/// </summary>
    public partial class QualificationBreakDown : Telerik.Reporting.Report
	{
		public QualificationBreakDown()
		{
			InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.PubIssueDates.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.QualBreakDown.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.table1.DataSource = this.QualBreakDown;
                this.ReportParameters["YearStart"].AvailableValues.DataSource = this.PubIssueDates;
                this.ReportParameters["YearStart"].AvailableValues.DisplayMember = "YearStartDate";
                this.ReportParameters["YearStart"].AvailableValues.ValueMember = "MonthDayStartDate";
                this.ReportParameters["YearEnd"].AvailableValues.DataSource = this.PubIssueDates;
                this.ReportParameters["YearEnd"].AvailableValues.DisplayMember = "YearEndDate";
                this.ReportParameters["YearEnd"].AvailableValues.ValueMember = "MonthDayEndDate";
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

                //NeedDataSource needs to be set individually for the report to work
                this.table1.DataSource = null;
                this.table1.NeedDataSource += GetParameters;
                this.QualificationChart.DataSource = null;
                this.QualificationChart.NeedDataSource += GetParameters;

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
                int.TryParse(r.Parameters["ProductID"].Value.ToString(), out productID);
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                bool.TryParse(r.Parameters["IncludeAddRemove"].Value.ToString(), out includeReportGroups);

                DataTable dt = ReportUtilities.GetQSourceBreakDown(filterQuery, issueID, includeReportGroups, productID);
                mytable.DataSource = dt;
                //QualificationChart.DataSource = dt;
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