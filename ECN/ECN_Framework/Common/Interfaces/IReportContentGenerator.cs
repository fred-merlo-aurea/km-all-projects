using System;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace ECN_Framework.Common
{
    public interface IReportContentGenerator
    {
        Byte[] CreateReportContent(
            string path,
            ReportDataSource dataSource,
            ReportParameter[] parameters,
            string outputType,
            out string responseContentType);

        Byte[] CreateReportContent(
            Stream definitionStream,
            ReportDataSource dataSource,
            ReportParameter[] parameters,
            string outputType,
            out string responseContentType);
    }
}
