using System;
using System.IO;
using System.Net.Mail;
using System.Xml.Serialization;
using ECN_Framework_Entities.Communicator;
using Microsoft.Reporting.WebForms;

namespace ecn.automatedreporting.Reports.Helpers
{
    public class ReportProcessor
    {
        private const string FileTypePdf = "PDF";
        private const string FileTypeXls = "XLS";
        private const string FileTypeXml = "XML";
        private const string FileTypeCsv = "CSV";
        private const string FileTypeExcel = "Excel";
        private const string FileTypeExcelUpperCase = "EXCEL";
        private const string FileExtensionPdf = ".pdf";
        private const string FileExtensionExcel = ".xls";
        private const string FileExtensionXml = ".xml";
        private const string RenderedLogMessage = "Rendered report.";
        private const string AttachingLogMessage = "Attaching report.";

        public bool ExportFormatPdf(ReportSchedule schedule)
        {
            return ExportFormatEquals(schedule, FileTypePdf);
        }

        public bool ExportFormatXls(ReportSchedule schedule)
        {
            return ExportFormatEquals(schedule, FileTypeXls);
        }

        public bool ExportFormatPdfOrExcel(ReportSchedule schedule)
        {
            return ExportFormatEquals(schedule, FileTypePdf) || ExportFormatEquals(schedule, FileTypeXls);
        }

        public bool ExportFormatXml(ReportSchedule schedule)
        {
            return ExportFormatEquals(schedule, FileTypeXml);
        }

        public bool ExportFormatCsv(ReportSchedule schedule)
        {
            return ExportFormatEquals(schedule, FileTypeCsv);
        }

        public bool ExportFormatEquals(ReportSchedule schedule, string fileTypeExtension)
        {
            return schedule.ExportFormat.Equals(fileTypeExtension, StringComparison.OrdinalIgnoreCase);
        }

        public void RenderAndWritePdf(ReportViewer reportViewer, string filePath)
        {
            RenderAndWriteFile(reportViewer, FileTypePdf, filePath);
        }

        public void RenderAndWriteXls(ReportViewer reportViewer, string filePath)
        {
            RenderAndWriteFile(reportViewer, FileTypeExcel, filePath);
        }

        public void RenderAndWriteFile(ReportViewer reportViewer, string fileType, string filePath)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            var bytes = reportViewer.LocalReport.Render(fileType, string.Empty, out mimeType, out encoding, out extension, out streamids, out warnings);
            File.WriteAllBytes(filePath, bytes);
        }

        public void RenderAndAttachPdf(
            EmailDirect message,
            ReportViewer reportViewer, 
            string fileNamePrefix,
            DateTime startDate,
            DateTime endDate, 
            string fileNameId = "")
        {
            RenderAndAttach(message, reportViewer, FileTypePdf, fileNamePrefix, startDate, endDate, FileExtensionPdf, fileNameId);
        }

        public void RenderAndAttachXls(
            EmailDirect message, 
            ReportViewer reportViewer, 
            string fileNamePrefix, 
            DateTime startDate,
            DateTime endDate,
            string fileNameId = "")
        {
            RenderAndAttach(message, reportViewer, FileTypeExcelUpperCase, fileNamePrefix, startDate, endDate, FileExtensionExcel, fileNameId);
        }

        public void RenderAndAttach(
            EmailDirect message,
            ReportViewer reportViewer, 
            string fileType, 
            string fileNamePrefix,
            DateTime startDate,
            DateTime endDate, 
            string fileExtension,
            string fileNameId = "")
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            var bytes = reportViewer.LocalReport.Render(fileType, string.Empty, out mimeType, out encoding, out extension, out streamids, out warnings);
            ReportsHelper.WriteToLog(RenderedLogMessage);
            var fileName = BuildFilePath(string.Empty, fileNamePrefix, startDate, endDate, fileExtension);
            var attachXls = new Attachment(new MemoryStream(bytes), fileName);
            ReportsHelper.WriteToLog(AttachingLogMessage);
            message.Attachments.Add(attachXls);
        }

        public void WriteAndAttachXml(
            ReportSchedule reportSchedule,
            EmailDirect message,
            string fileNamePrefix,
            DateTime startDate, 
            DateTime endDate,
            Type serializerType,
            object serializeObject, 
            string fileNameId = "")
        {
            if (reportSchedule == null)
            {
                throw new ArgumentNullException(nameof(reportSchedule));
            }

            if (reportSchedule.CustomerID == null)
            {
                throw new ArgumentException("CustomerID is null", nameof(reportSchedule));
            }

            var filepath = ReportsHelper.GetFilePath(reportSchedule.CustomerID.Value);
            var fileName = BuildFilePath(filepath, fileNamePrefix, startDate, endDate, FileExtensionXml, fileNameId);
            var file = new FileInfo(fileName);
            using (var sw = file.AppendText())
            {
                var writer = new XmlSerializer(serializerType);
                writer.Serialize(sw, serializeObject);
                ReportsHelper.WriteToLog(RenderedLogMessage);
                sw.Close();
            }

            ReportsHelper.WriteToLog(AttachingLogMessage);
            message.Attachments.Add(new Attachment(file.FullName));
        }

        public string BuildFilePath(string filePath, string fileNamePrefix, DateTime startDate, DateTime endDate, string extension, string fileNameId = "")
        {
            return $"{filePath}{fileNamePrefix}{startDate.ToShortDateString().Replace("/", "_")}_{endDate.ToShortDateString().Replace("/", "_")}{fileNameId}{extension}";
        }
    }
}
