namespace ReportLibrary.Reports
{
    using System;

	/// <summary>
	/// Summary description for GeoDomesticBreakDown.
	/// </summary>
    public partial class GeoDomesticBreakDown : Telerik.Reporting.Report
	{
		public GeoDomesticBreakDown()
		{
			InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.Domestic.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.GeoTable.DataSource = this.Domestic;
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
                bool includeAddRemove = false;
                int issueID = 0;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                string filters = r.Parameters["Filters"].Value.ToString();
                string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                bool.TryParse(r.Parameters["IncludeAddRemove"].Value.ToString(), out includeAddRemove);

                mytable.DataSource = ReportUtilities.GetGeoBreakDownDomestic(filters, adHocFilters, issueID, includeAddRemove);
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