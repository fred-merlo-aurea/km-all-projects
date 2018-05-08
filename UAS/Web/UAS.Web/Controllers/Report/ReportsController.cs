using System.IO;
using System.Web;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;

namespace UAS.Web.Controllers.Report
{
   

    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration preservedConfiguration;

        static IReportServiceConfiguration PreservedConfiguration
        {
            get
            {
                if (null == preservedConfiguration)
                {
                    preservedConfiguration = new ReportServiceConfiguration
                    {
                        HostAppId = "UAS.Web",
                        Storage = new FileStorage(),
                        ReportResolver = CreateResolver(),
                        // ReportSharingTimeout = 0,
                        // ClientSessionTimeout = 15,
                    };
                }
                return preservedConfiguration;
            }
        }

        public ReportsController()
        {
            this.ReportServiceConfiguration = PreservedConfiguration;
        }

        static IReportResolver CreateResolver()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/Reports");
            var reportsPath = Path.Combine(appPath, @"Reports");

            return new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());
        }
    }
}