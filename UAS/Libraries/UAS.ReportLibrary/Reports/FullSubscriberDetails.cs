namespace UAS.ReportLibrary.Reports
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
                string filterQuery = r.Parameters["FilterQuery"].Value.ToString();
                //string adHocFilters = r.Parameters["AdHocFilters"].Value.ToString();
                int.TryParse(r.Parameters["IssueID"].Value.ToString(), out issueID);
                mytable.DataSource = ReportUtilities.GetFullSubscriberDetails(filterQuery,issueID);
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".GetParameters", app, "Reports");
            }
        }

        public static bool OpenSubscriber(int id)
        {
            var client = new KMPlatform.BusinessLogic.Client().Select(ReportUtilities.ClientId);
            FrameworkUAD.Entity.ProductSubscription pub = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectProductSubscription(id,client.ClientConnections,client.DisplayName);
            return true;
        }
    }
}