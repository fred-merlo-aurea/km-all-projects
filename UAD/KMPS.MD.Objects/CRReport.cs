using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ExportOptions = CrystalDecisions.Shared.ExportOptions;

namespace KMPS.MD.Objects
{
    public enum CRExportEnum
    {
        PDF = 1,
        XLS = 2,
        DOC = 3,
        HTML = 4
    }

    public class CRReport
    {
        private const string ContentDispositionHeaderName = "content-disposition";
        private const string ContentTypeExcel = "application/x-msexcel";
        private const string ContentTypePdf = "application/pdf";
        private const string ContentTypeWord = "application/msword";
        private const string AttachmentFilenameFormatString = "attachment; filename={0}";

        public CRReport()
        {
        }

        public static ReportDocument GetReportWithoutParams(string reportName)
        {

            ReportDocument report = new ReportDocument();
            report.Load(reportName);

            //ConnectionInfo ci = new ConnectionInfo();

            //ci.ServerName = ConfigurationManager.AppSettings["Server"].ToString();
            //ci.DatabaseName = ConfigurationManager.AppSettings["Database"].ToString();
            //ci.UserID = ConfigurationManager.AppSettings["UserID"].ToString();
            //ci.Password = ConfigurationManager.AppSettings["Password"].ToString();

            ConnectionInfo ci = GetConnectInfo();

            if (!ApplyLogon(report, ci))
                throw new Exception("Crystal Report Login Failed");

            return report;

        }

        public static ConnectionInfo GetConnectInfo()
        {
            string server = (string)System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            ConnectionInfo ci = new ConnectionInfo();

            string subDomain = "";
            int length = server.Split('.').Length;
            if (length > 2)
            {
                int first = server.IndexOf(".");
                subDomain = server.Substring(0, first);
            }
            else if (server.Trim().ToUpper() == "LOCALHOST")
            {
                subDomain = "localhost";
            }
            ci.ServerName = ConfigurationManager.AppSettings[subDomain + "_Server"].ToString();
            ci.DatabaseName = ConfigurationManager.AppSettings[subDomain + "_Database"].ToString();
            ci.UserID = ConfigurationManager.AppSettings[subDomain + "_UserID"].ToString();
            ci.Password = ConfigurationManager.AppSettings[subDomain + "_Password"].ToString();

            return ci;
        }

        public static ReportDocument GetReport(string reportName, Hashtable cParams)
        {

            ReportDocument report = new ReportDocument();

            report.Load(reportName);

            ConnectionInfo ci = GetConnectInfo();

            //ConnectionInfo ci = new ConnectionInfo();

            //ci.ServerName = ConfigurationManager.AppSettings["Server"].ToString();
            //ci.DatabaseName = ConfigurationManager.AppSettings["Database"].ToString();
            //ci.UserID = ConfigurationManager.AppSettings["UserID"].ToString();
            //ci.Password = ConfigurationManager.AppSettings["Password"].ToString();

            if (!ApplyLogon(report, ci))
                throw new Exception("Crystal Report Login Failed");

            //foreach (ReportObject obj in report.ReportDefinition.ReportObjects)
            //{
            //    if (obj.Kind == ReportObjectKind.SubreportObject)
            //    {
            //        subObj = (SubreportObject)obj;

            //        if (!ApplyLogon(report.OpenSubreport(subObj.SubreportName), ci))
            //            throw new Exception("Crystal Subreport Report Login");
            //    }
            //}

            IDictionaryEnumerator en = cParams.GetEnumerator();

            while (en.MoveNext())
            {
                report.SetParameterValue(en.Key.ToString(), en.Value.ToString());
            }

            return report;
        }

        // Helper method that iterates through all tables // in a report document 
        private static bool ApplyLogon(ReportDocument cr, ConnectionInfo ci)
        {
            TableLogOnInfo li;
            // for each table apply connection info 
            foreach (Table tbl in cr.Database.Tables)
            {
                li = tbl.LogOnInfo;
                li.ConnectionInfo = ci;
                tbl.ApplyLogOnInfo(li);

                //// check if logon was successful 
                //// if TestConnectivity returns false, check 
                //// logon credentials 
                //if (tbl.TestConnectivity())
                //{
                //    // drop fully qualified table location 
                //    if (tbl.Location.IndexOf(".") > 0)
                //    {
                //        tbl.Location = tbl.Location.Substring(tbl.Location.LastIndexOf(".") + 1);
                //    }
                //    else
                //        tbl.Location = tbl.Location;
                //}
                //else return (false);
            }
            return (true);
        }



        public static void Export(ReportDocument report, CRExportEnum exportFormat, string fileName)
        {
            if (exportFormat == CRExportEnum.HTML)
            {
                return;
            }

            var tempFileName = GetTempFileName(exportFormat);

            var diskFileDestinationOptions = new DiskFileDestinationOptions { DiskFileName = tempFileName };
            SetExportOptions(report, exportFormat, diskFileDestinationOptions);
            ExportReport(report);

            SetHttpContext(exportFormat, fileName, tempFileName);
            File.Delete(tempFileName);
        }

        private static void SetHttpContext(CRExportEnum exportFormat, string fileName, string tempFileName)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            switch (exportFormat)
            {
                case CRExportEnum.XLS:
                    HttpContext.Current.Response.ContentType = ContentTypeExcel;
                    HttpContext.Current.Response.AddHeader(ContentDispositionHeaderName, string.Format(AttachmentFilenameFormatString, fileName));
                    break;
                case CRExportEnum.PDF:
                    HttpContext.Current.Response.ContentType = ContentTypePdf;
                    HttpContext.Current.Response.AddHeader(ContentDispositionHeaderName, string.Format(AttachmentFilenameFormatString, fileName));
                    break;
                case CRExportEnum.DOC:
                    HttpContext.Current.Response.ContentType = ContentTypeWord;
                    HttpContext.Current.Response.AddHeader(ContentDispositionHeaderName, string.Format(AttachmentFilenameFormatString, fileName));
                    break;
            }

            HttpContext.Current.Response.WriteFile(tempFileName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private static void SetExportOptions(
            ReportDocument report,
            CRExportEnum exportFormat,
            DiskFileDestinationOptions diskFileDestinationOptions)
        {
            var exportOptions = ReportExportOptions(report);
            exportOptions.DestinationOptions = diskFileDestinationOptions;
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;

            switch (exportFormat)
            {
                case CRExportEnum.XLS:
                    exportOptions.ExportFormatType = ExportFormatType.Excel;
                    var objExcelOptions = new ExcelFormatOptions { ExcelUseConstantColumnWidth = false };
                    exportOptions.FormatOptions = objExcelOptions;
                    break;
                case CRExportEnum.PDF:
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    break;
                case CRExportEnum.DOC:
                    exportOptions.ExportFormatType = ExportFormatType.RichText;
                    break;
            }
        }

        private static string GetTempFileName(CRExportEnum exportFormat)
        {
            var tempFileName = Path.GetTempFileName(); 
            var extension = exportFormat.ToString().ToLower();

            return Path.ChangeExtension(tempFileName, extension);
        }

        private static void ExportReport(ReportDocument report)
        {
            report.Export();
        }

        private static ExportOptions ReportExportOptions(ReportDocument report)
        {
            return report.ExportOptions;
        }
    }
}