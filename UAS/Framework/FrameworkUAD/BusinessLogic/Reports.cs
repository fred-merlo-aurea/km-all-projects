using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using System.IO;

namespace FrameworkUAD.BusinessLogic
{
    public class Reports
    {
        public List<FrameworkUAD.Report.SourceFileSummary> GetSourceFileSummary(int sourceFileID, string processCode, string fileName, KMPlatform.Object.ClientConnections client)
        {
            List<FrameworkUAD.Report.SourceFileSummary> x = null;
            x = DataAccess.Reports.GetSourceFileSummary(sourceFileID,processCode,fileName,client).ToList();
            return x;
        }
        public List<FrameworkUAD.Report.SourceFilePubCodeSummary> GetSourceFilePubCodeSummary(int sourceFileID, string processCode, string fileName, KMPlatform.Object.ClientConnections client)
        { 
            List<FrameworkUAD.Report.SourceFilePubCodeSummary> x = null;
            x = DataAccess.Reports.GetSourceFilePubCodeSummary(sourceFileID, processCode, fileName, client).ToList();
            return x;
        }
        public string CreateFileSummaryReport(int sourceFileID, string processCode, string sourceFileName, string clientName, KMPlatform.Object.ClientConnections client, string clientArchiveDir)
        {
            FrameworkUAD.BusinessLogic.Reports rptWorker = new FrameworkUAD.BusinessLogic.Reports();
            List<FrameworkUAD.Report.SourceFileSummary> summary = rptWorker.GetSourceFileSummary(sourceFileID, processCode, sourceFileName, client);
            DataTable dtSummary = Core_AMS.Utilities.DataTableFunctions.ToDataTable<FrameworkUAD.Report.SourceFileSummary>(summary);

            string dir = clientArchiveDir + @"\" + clientName + @"\Reports\";
            System.IO.Directory.CreateDirectory(dir);
            //Core_AMS.Utilities.FileFunctions fWorker = new Core_AMS.Utilities.FileFunctions();
            DateTime time = DateTime.Now;
            string format = "MMddyyyy_HH-mm-ss";
            string fileName = dir + time.ToString(format) + "_" + sourceFileID.ToString() + "_SummaryReport.xlsx";
            //fWorker.CreateCSVFromDataTable(dtSummary, fileName);
            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
            Workbook wb = ef.GetWorkbook(dtSummary, "File Summary");
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            using (FileStream output = new FileStream(fileName, FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
            return fileName;
        }
        public string CreatePubCodeSummaryReport(int sourceFileID, string processCode, string sourceFileName, string clientName, KMPlatform.Object.ClientConnections client, string clientArchiveDir)
        {
            FrameworkUAD.BusinessLogic.Reports rptWorker = new FrameworkUAD.BusinessLogic.Reports();
            List<FrameworkUAD.Report.SourceFilePubCodeSummary> summary = rptWorker.GetSourceFilePubCodeSummary(sourceFileID, processCode, sourceFileName, client);
            DataTable dtSummary = Core_AMS.Utilities.DataTableFunctions.ToDataTable<FrameworkUAD.Report.SourceFilePubCodeSummary>(summary);

            string dir = clientArchiveDir + @"\" + clientName + @"\Reports\";
            System.IO.Directory.CreateDirectory(dir);
            //Core_AMS.Utilities.FileFunctions fWorker = new Core_AMS.Utilities.FileFunctions();
            DateTime time = DateTime.Now;
            string format = "MMddyyyy_HH-mm-ss";
            string fileName = dir + time.ToString(format) + "_" + sourceFileID.ToString() + "_PubCodeSummaryReport.xlsx";
            //fWorker.CreateCSVFromDataTable(dtSummary, fileName);
            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
            Workbook wb = ef.GetWorkbook(dtSummary, "PubCode Summary");
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            using (FileStream output = new FileStream(fileName, FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
            return fileName;
        }
    }
}
