namespace UAS.ReportLibrary.Reports
{
    using System;

    /// <summary>
    /// Summary description for SubscriberPaidDetails.
    /// </summary>
    public partial class SubscriberPaidDetails : Telerik.Reporting.Report
    {
        public SubscriberPaidDetails()
        {
            InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.Paid.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.table1.DataSource = this.Paid;
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
                string filterQuery= r.Parameters["FilterQuery"].Value.ToString();
                //string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();

                mytable.DataSource = ReportUtilities.GetSubscriberPaidDetails(filterQuery);
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