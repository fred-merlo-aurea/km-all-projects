using System.IO;
using System.Reflection;
using ECN_Framework.Common.Interfaces;

namespace ECN_Framework.Common
{
    public class AssemblyReportDefinitionProvider : IReportDefinitionProvider
    {
        public Stream GetReportDefinitionStream(
            string containerLocation,
            string reportName)
        {
            var assembly = Assembly.LoadFrom(containerLocation);
            var stream = assembly.GetManifestResourceStream(reportName);
            return stream;
        }
    }
}
