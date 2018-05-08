namespace UAS.ReportLibrary.Reports
{
    using System;

	/// <summary>
	/// Summary description for SubSourceSummary.
	/// </summary>
    public partial class SubSourceSummary : Telerik.Reporting.Report
	{
		public SubSourceSummary()
		{
			InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.SubSource.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.table1.DataSource = this.SubSource;
            }
            else
            {
                this.table1.DataSource = null;
                this.table1.NeedDataSource += new EventHandler(GetParameters);
            }
        }

        private void GetParameters(object sender, System.EventArgs e)
        {

            try
            {
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;

                int issueID = 0;
                bool includeAddRemove = false;
                //string filters = r.Parameters["Filters"].Value.ToString();
                //string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                bool.TryParse(r.Parameters["IncludeAddRemove"].Value.ToString(), out includeAddRemove);
                string filterQuery = r.Parameters["FilterQuery"].Value.ToString();
                mytable.DataSource = ReportUtilities.GetSubSrcData(filterQuery, includeAddRemove, issueID);
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