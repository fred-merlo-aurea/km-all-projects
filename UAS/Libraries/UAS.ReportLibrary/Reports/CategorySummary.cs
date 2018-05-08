namespace UAS.ReportLibrary.Reports
{
    using System;

	/// <summary>
	/// Summary description for CategorySummary.
	/// </summary>
    public partial class CategorySummary : Telerik.Reporting.Report
	{
        
        public CategorySummary()
		{
            

            InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.CatSummary.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.table1.DataSource = this.CatSummary;
               
            }
            else
            {
                this.table1.DataSource = null;
                this.table1.NeedDataSource += GetParameters;
            }
        }

        private void GetParameters(object sender, System.EventArgs e)
        {
            try
            {
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                string filterQuery = r.Parameters["FilterQuery"].Value.ToString();
                int issueID = 0;
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                mytable.DataSource = ReportUtilities.GetCategorySummaryData(filterQuery, issueID);
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