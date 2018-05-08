namespace ReportLibrary.Reports
{
    using System;
    using System.Data;

    /// <summary>
    /// Summary description for GeoSingleCountry.
    /// </summary>
    public partial class GeoSingleCountry : Telerik.Reporting.Report
    {
        public GeoSingleCountry()
        {
            InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.Country.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.Countries.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.ReportParameters["CountryID"].AvailableValues.DataSource = this.Countries;
                this.GeoTable.DataSource = this.Country;
            }
            else
            {
                DataTable dt = ReportUtilities.GetCountries();
                this.ReportParameters["CountryID"].AvailableValues.DataSource = dt;
                this.ReportParameters["CountryID"].AvailableValues.DisplayMember = "ShortName";
                this.ReportParameters["CountryID"].AvailableValues.ValueMember = "CountryID";
                this.GeoTable.DataSource = null;
                this.GeoTable.NeedDataSource += GetParameters;
            }
        }

        private void GetParameters(object sender, System.EventArgs e)
        {
            try
            {
                int issueID = 0;
                int countryID = 0;
                Telerik.Reporting.Processing.DataItem mytable = sender as Telerik.Reporting.Processing.DataItem;
                Telerik.Reporting.Processing.Report r = mytable.Report;
                string filters = r.Parameters["Filters"].Value.ToString();
                string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                int.TryParse(r.Parameters["CountryID"].Value.ToString(), out countryID);

                mytable.DataSource = ReportUtilities.GetGeoBreakDownSingleCountry(filters, adHocFilters, issueID, countryID);
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