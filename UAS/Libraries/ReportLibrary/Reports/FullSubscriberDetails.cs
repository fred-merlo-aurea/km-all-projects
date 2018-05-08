namespace ReportLibrary.Reports
{
    using System;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for FullSubscriberDetails.
    /// </summary>
    public partial class FullSubscriberDetails : Telerik.Reporting.Report
    {
        public FullSubscriberDetails()
        {
            InitializeComponent();

            if (ReportUtilities.Debug)
            {
                this.PubSubData.ConnectionString = ReportUtilities.GetUADConnectionString();
                this.table1.DataSource = PubSubData;
            }
            else
            {
                this.table1.DataSource = null;
                this.table1.NeedDataSource += GetParameters;
            }
        }

        public static Unit GetFontSize(double num)
        {
            double newNum = (num * 4) + 14;
            if (newNum > 100)
                newNum = newNum - (newNum - 100);
            return new Unit(newNum);
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

                mytable.DataSource = ReportUtilities.GetFullSubscriberDetails(filters, adHocFilters, issueID);
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

        //public static bool OpenSubscriber(int id)
        //{
        //    FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        //    FrameworkUAD.Entity.ProductSubscription pub = productSubWorker.Proxy.SelectProductSubscription(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, id,
        //        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientName).Result;

        //    return true;
        //}
    }
}