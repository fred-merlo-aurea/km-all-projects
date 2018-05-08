using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using System.IO;
using CommonEnums = KM.Common.Enums;

namespace FrameworkUAD.BusinessLogic
{
    public class ImportErrorSummary
    {
        public List<Object.ImportErrorSummary> Select(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.ImportErrorSummary> x = null;
            x = DataAccess.ImportErrorSummary.Select(sourceFileID,processCode,client).ToList();
            return x;
        }
        public List<Object.ImportErrorSummary> FileValidatorSelect(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.ImportErrorSummary> x = null;
            x = DataAccess.ImportErrorSummary.FileValidatorSelect(sourceFileID, processCode, client).ToList();
            return x;
        }
        public string CreateDimensionErrorsSummaryReport(int sourceFileID, string processCode, string clientName, KMPlatform.Object.ClientConnections client, string clientArchiveDir)
        {
            FrameworkUAD.BusinessLogic.ImportErrorSummary rptWorker = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
            List<FrameworkUAD.Object.ImportErrorSummary> summary = rptWorker.Select(sourceFileID, processCode, client);
            foreach(var s in summary)
            {
                s.ClientMessage = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.ClientMessage.Trim());
                s.MAFField = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.MAFField.Trim());
                s.PubCode = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.PubCode.Trim());
                s.Value = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.Value.Trim());
            }

            // Attempt to remove bad "FAKE" dimensions created from Multiple Split into rows
            var tsWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
            var tsList = tsWorker.SelectObject(sourceFileID);
            var clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);
            foreach (var tsi in tsList)
            {
                if (!clientPubCodes.ContainsKey(tsi.PubID))
                {
                    continue;
                }

                var pubcode = clientPubCodes[tsi.PubID];


                if (string.IsNullOrEmpty(pubcode))
                {
                    continue;
                }

                var delimiter = CommonEnums.GetDelimiterSymbol(tsi.Delimiter).GetValueOrDefault(',');
                summary.RemoveAll(x => x.PubCode.Equals(pubcode, StringComparison.CurrentCultureIgnoreCase) &&
                                       x.MAFField.Equals(tsi.MAFField, StringComparison.CurrentCultureIgnoreCase) &&
                                       x.Value.Contains(delimiter));
            }

            var dtSummary = Core_AMS.Utilities.DataTableFunctions.ToDataTable(summary);
            string dir = clientArchiveDir + @"\" + clientName + @"\Reports\";
            System.IO.Directory.CreateDirectory(dir);

            string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(processCode);
            string fileName = dir + cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DimensionErrorsSummaryReport.ToString() + ".xlsx";//csv
            //fWorker.CreateCSVFromDataTable(dtSummary, fileName);
            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
            Workbook wb = ef.GetWorkbook(dtSummary, "Dimension Errors");
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            using (FileStream output = new FileStream(fileName, FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
            return fileName;
        }
       
    }
}
