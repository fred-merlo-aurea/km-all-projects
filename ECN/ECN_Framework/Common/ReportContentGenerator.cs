using System;
using ECN_Framework.Consts;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace ECN_Framework.Common
{
    public class ReportContentGenerator : IReportContentGenerator
    {
        private ReportViewer _reportViewer;

        public ReportContentGenerator(ReportViewer reportViewer)
        {
            _reportViewer = reportViewer;
        }

        public Byte[] CreateReportContent(
            string path,
            ReportDataSource dataSource,
            ReportParameter[] parameters,
            string outputType,
            out string responseContentType)
        {
            UpdateReportViewer(null, path, dataSource, parameters);
            return GetReportContent(outputType, out responseContentType);
        }

        public Byte[] CreateReportContent(
            Stream definitionStream,
            ReportDataSource dataSource,
            ReportParameter[] parameters,
            string outputType,
            out string responseContentType)
        {
            UpdateReportViewer(definitionStream, string.Empty, dataSource, parameters);
            return GetReportContent(outputType, out responseContentType);
        }

        private void UpdateReportViewer(Stream definitionStream, string path, ReportDataSource dataSource, ReportParameter[] parameters)
        {
            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.DataSources.Add(dataSource);
            if (definitionStream != null)
            {
                _reportViewer.LocalReport.LoadReportDefinition(definitionStream);
            }

            if (!string.IsNullOrWhiteSpace(path))
            {
                _reportViewer.LocalReport.ReportPath = path;
            }

            if (parameters != null)
            {
                _reportViewer.LocalReport.SetParameters(parameters);
            }

            _reportViewer.LocalReport.Refresh();
        }

        private byte[] GetReportContent(string outputType, out string responseContentType)
        {
            responseContentType = string.Empty;

            Warning[] warnings;
            string[] streamids;
            String mimeType;
            String encoding;
            String extension;
            Byte[] bytes = null;

            var renderFormat = GetRenderFormatForReportOutputType(outputType);
            responseContentType = GetResponseContentTypeForReportOutputType(outputType);
            
            if (!string.IsNullOrEmpty(renderFormat))
            {
                bytes = _reportViewer.LocalReport.Render(
                    renderFormat,
                    string.Empty,
                    out mimeType,
                    out encoding,
                    out extension,
                    out streamids,
                    out warnings);
            }

            return bytes;
        }

        private string GetRenderFormatForReportOutputType(string outputType)
        {
            if (outputType.Equals(ReportConsts.OutputTypePDF))
            {
                return ReportConsts.RenderFormatPDF;
            }

            if (outputType.Equals(ReportConsts.OutputTypeXLS))
            {
                return ReportConsts.RenderFormatEXCEL;
            }

            return string.Empty;
        }

        private string GetResponseContentTypeForReportOutputType(string outputType)
        {
            if (outputType.Equals(ReportConsts.OutputTypePDF))
            {
                return ResponseConsts.ContentTypePDF;
            }

            if (outputType.Equals(ReportConsts.OutputTypeXLS))
            {
                return ResponseConsts.ContentTypeExcel;
            }

            return string.Empty;
        }
    }
}
