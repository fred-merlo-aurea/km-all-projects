namespace ReportLibrary.Reports
{
    using System;

	/// <summary>
	/// Summary description for Par3C.
	/// </summary>
    public partial class Par3C : Telerik.Reporting.Report
	{
		public Par3C()
		{
			InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.Main.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.crosstab1.DataSource = this.Main;
            }
            else
            {
                this.crosstab1.DataSource = null;
                this.crosstab1.NeedDataSource += GetParameters;
            }
		}

        private void GetParameters(object sender, System.EventArgs e)
        {
            try
            {
                int issueID = 0;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                string filters = r.Parameters["Filters"].Value.ToString();
                string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);

                mytable.DataSource = ReportUtilities.GetPar3CData(filters, adHocFilters, issueID);
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