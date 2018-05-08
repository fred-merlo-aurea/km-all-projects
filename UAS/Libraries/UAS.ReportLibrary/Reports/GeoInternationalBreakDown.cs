namespace UAS.ReportLibrary.Reports
{
    using System;

    /// <summary>
    /// Summary description for GeoInternationalBreakDown.
    /// </summary>
    public partial class GeoInternationalBreakDown : Telerik.Reporting.Report
    {
        public GeoInternationalBreakDown()
        {
            InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.International.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.GeoTable.DataSource = this.International;
            }
            else
            {
                this.GeoTable.DataSource = null;
                this.GeoTable.NeedDataSource += GetParameters;
            }
        }

        private void GetParameters(object sender, System.EventArgs e)
        {
            try
            {
                int issueID = 0, ProductID = 0;
                bool includeCustomRegion = false;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                string filterQuery = r.Parameters["FilterQuery"].Value.ToString();
                bool.TryParse(r.Parameters["IsCustomRegionReport"].Value.ToString(), out includeCustomRegion);
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                int.TryParse(r.Parameters["ProductID"].Value.ToString(), out ProductID);
                mytable.DataSource = ReportUtilities.GetGeoBreakDownInternational(filterQuery, issueID, ProductID, includeCustomRegion);
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