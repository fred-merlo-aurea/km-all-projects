using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using System.IO;

namespace FrameworkUAD.BusinessLogic
{
    public class CircIntegration
    {
        public bool ApplyTelemarketingRules(string processCode, FrameworkUAD_Lookup.Enums.FileTypes dft, KMPlatform.Object.ClientConnections client, int clientId)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyTelemarketingRules(processCode, dft, client, clientId);
                scope.Complete();
            }

            return complete;
        }

        public bool CircFileTypeUpdateIGrpBySequence(string processCode, FrameworkUAD_Lookup.Enums.FileTypes dft, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.CircFileTypeUpdateIGrpBySequence(processCode, dft, client);
                scope.Complete();
            }

            return complete;
        }

        public bool ApplyWebFormRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyWebFormRules(processCode, client, clientId);
                scope.Complete();
            }

            return complete;
        }

        public bool ApplyListSource2YRRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyListSource2YRRules(processCode, client, clientId);
                scope.Complete();
            }

            return complete;
        }

        public bool ApplyListSource3YRRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyListSource3YRRules(processCode, client, clientId);
                scope.Complete();
            }

            return complete;
        }

        public bool ApplyListSourceOtherRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyListSourceOtherRules(processCode, client, clientId);
                scope.Complete();
            }

            return complete;
        }

        public bool ApplyFieldUpdateRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId, bool qdateOverRide, bool mailPermissionOverRide, bool faxPermissionOverRide, bool phonePermissionOverRide, bool otherProductsPermissionOverRide, bool thirdPartyPermissionOverRide, bool emailRenewPermissionOverRide, bool textPermissionOverRide)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyFieldUpdateRules(processCode, client, clientId, qdateOverRide, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide);
                scope.Complete();
            }

            return complete;
        }

        public bool ApplyQuickFillRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId, bool qdateOverRide, bool mailPermissionOverRide, bool faxPermissionOverRide, bool phonePermissionOverRide, bool otherProductsPermissionOverRide, bool thirdPartyPermissionOverRide, bool emailRenewPermissionOverRide, bool textPermissionOverRide)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyQuickFillRules(processCode, client, clientId, qdateOverRide, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide);
                scope.Complete();
            }

            return complete;
        }
        public bool ApplyPaidTransactionLogic(string processCode, KMPlatform.Object.ClientConnections client, int clientId, bool qdateOverRide, bool mailPermissionOverRide, bool faxPermissionOverRide, bool phonePermissionOverRide, bool otherProductsPermissionOverRide, bool thirdPartyPermissionOverRide, bool emailRenewPermissionOverRide, bool textPermissionOverRide)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.CircIntegration.ApplyPaidTransactionLogic(processCode, client, clientId, qdateOverRide, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide);
                scope.Complete();
            }

            return complete;
        }

        public bool FieldUpdateDataMatching(KMPlatform.Object.ClientConnections client, string processCode)
        {
            bool done = false;
            done = DataAccess.CircIntegration.FieldUpdateDataMatching(client, processCode);
            return done;
        }

        public bool QuickFillDataMatching(KMPlatform.Object.ClientConnections client, string processCode)
        {
            bool done = false;
            done = DataAccess.CircIntegration.QuickFillDataMatching(client, processCode);
            return done;
        }

        public string SelectCircImportSummaryOne(string ProcessCode, KMPlatform.Object.ClientConnections client, string clientName, int sourceFileID, string clientArchiveDir)
        {
            DataTable dtSummary = new DataTable();
            dtSummary = DataAccess.CircIntegration.SelectCircImportSummaryOne(ProcessCode, client);

            string dir = clientArchiveDir + @"\" + clientName + @"\Reports\";
            System.IO.Directory.CreateDirectory(dir);
            //Core_AMS.Utilities.FileFunctions fWorker = new Core_AMS.Utilities.FileFunctions();
            DateTime time = DateTime.Now;
            string format = "MMddyyyy_HH-mm-ss";
            string fileName = dir + time.ToString(format) + "_" + sourceFileID.ToString() + "_QDateBreakdown.xlsx";
            // fWorker.CreateCSVFromDataTable(dtSummary, fileName);
            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
            Workbook wb = ef.GetWorkbook(dtSummary, "Circ Import Summary");
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            using (FileStream output = new FileStream(fileName, FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
            return fileName;
        }

        public string SelectCircImportSummaryTwo(string ProcessCode, KMPlatform.Object.ClientConnections client, string clientName, int sourceFileID, string clientArchiveDir)
        {
            DataTable dtSummary = new DataTable();
            dtSummary = DataAccess.CircIntegration.SelectCircImportSummaryTwo(ProcessCode, client);

            string dir = clientArchiveDir + @"\" + clientName + @"\Reports\";
            System.IO.Directory.CreateDirectory(dir);
            //Core_AMS.Utilities.FileFunctions fWorker = new Core_AMS.Utilities.FileFunctions();
            DateTime time = DateTime.Now;
            string format = "MMddyyyy_HH-mm-ss";
            string fileName = dir + time.ToString(format) + "_" + sourceFileID.ToString() + "_CatTransBreakdown.xlsx";
            //fWorker.CreateCSVFromDataTable(dtSummary, fileName);
            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
            Workbook wb = ef.GetWorkbook(dtSummary, "Circ Import Summary");
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            using (FileStream output = new FileStream(fileName, FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
            return fileName;
        }

        public string SelectCircACSSummary(string ProcessCode, KMPlatform.Object.ClientConnections client, string clientName, int sourceFileID, string clientArchiveDir)
        {
            DataTable dtSummary = new DataTable();
            dtSummary = DataAccess.CircIntegration.SelectCircACSSummary(ProcessCode, client);

            string dir = clientArchiveDir + @"\" + clientName + @"\Reports\";
            System.IO.Directory.CreateDirectory(dir);
            //Core_AMS.Utilities.FileFunctions fWorker = new Core_AMS.Utilities.FileFunctions();
            DateTime time = DateTime.Now;
            string format = "MMddyyyy_HH-mm-ss";
            string fileName = dir + time.ToString(format) + "_" + sourceFileID.ToString() + "_ACSBreakdown.xlsx";
            //fWorker.CreateCSVFromDataTable(dtSummary, fileName);
            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
            Workbook wb = ef.GetWorkbook(dtSummary, "ACS Summary");
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            using (FileStream output = new FileStream(fileName, FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
            return fileName;
        }
    }
}
