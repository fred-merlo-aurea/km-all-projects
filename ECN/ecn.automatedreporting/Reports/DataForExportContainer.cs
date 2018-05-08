using System.Data;

namespace ecn.automatedreporting.Reports
{
    public struct DataForExportContainer
    {
        public DataTable DataToBeExported { get; set; }
        public string NameOfReport { get; set; }
    }
}