using System.IO;

namespace ECN_Framework.Common.Interfaces
{
    public interface IReportDefinitionProvider
    {
        Stream GetReportDefinitionStream(
            string containerLocation,
            string reportName);
    }
}