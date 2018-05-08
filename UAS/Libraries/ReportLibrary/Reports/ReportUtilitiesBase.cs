using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;

namespace ReportLibrary.Reports
{
    public abstract class ReportUtilitiesBase
    {
        public static int ClientId { get; set; } = 24;

        public static Report CurrentReport { get; set; } = new Report();

        public static ReportingXML ReportFilters { get; set; } = new ReportingXML();

        public static Code ReportCode { get; set; } = new Code();

        public static int ProductId { get; set; } = 0;

        public static bool Debug { get; set; } = false;
    }
}
