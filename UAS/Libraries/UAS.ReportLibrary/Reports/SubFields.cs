namespace UAS.ReportLibrary.Reports
{
    using System;

	/// <summary>
	/// Summary description for SubFields.
	/// </summary>
    public partial class SubFields : Telerik.Reporting.Report
	{
		public SubFields()
		{
			InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.Demo.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.DemoTable.DataSource = this.Demo;
            }
            else
            {
                this.DemoTable.DataSource = null;
                this.DemoTable.NeedDataSource += GetSubFieldsParameters;
            }
        }

        private void GetSubFieldsParameters(object sender, System.EventArgs e)
        {
            try
            {
                int issueID = 0;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                //string filters = r.Parameters["Filters"].Value.ToString();
                //string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                string demo = r.Parameters["Demo"].Value.ToString();
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                string filterQuery = r.Parameters["FilterQuery"].Value.ToString();
                mytable.DataSource = ReportUtilities.GetSubFieldData(filterQuery, demo, issueID);
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