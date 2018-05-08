using iTextSharp.text;
using iTextSharp.text.pdf;
using ReportLibrary.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for TelerikReportViewer.xaml
    /// </summary>
    public partial class TelerikReportViewer : UserControl
    {
        #region Service Call/Responses
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code> codeSingleResponse = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code>();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productW = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryW = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catCodeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transCodeW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> transCodeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catCodeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> rptCodeResp = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
            
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        private int myProductID;
        private int groupID;
        private RadBusyIndicator busy;
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        private List<FrameworkUAD_Lookup.Entity.Country> countries = new List<FrameworkUAD_Lookup.Entity.Country>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypes = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.Code> reportCodes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypes = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodes = new List<FrameworkUAD_Lookup.Entity.TransactionCode>(); 
        RadButton btnGenerater;
        #endregion

        public TelerikReportViewer(int productID)
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            myProductID = productID;
            codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Group);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                groupID = codeResponse.Result.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterGroupTypes.Circulation.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
            }
            myProduct = productW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            countries = countryW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            catCodes = catCodeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            transCodeTypes = transCodeTypeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            reportCodes = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Report).Result;
            transCodes = transCodeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            catCodeTypes = catCodeTypeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
        }
        public void ExportReport(Telerik.Reporting.ReportSource rptSource, FrameworkUAD.Entity.Report report)
        {
            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
            System.Collections.Hashtable deviceInfo = new System.Collections.Hashtable();
            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("XLS", rptSource, deviceInfo);

            string fileName = myProduct.PubCode + "_" + report.ReportName.Replace(" ", "") + "_" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xls";
            string dir = "C:\\ADMS\\Reports\\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (System.IO.File.Exists(dir + fileName))
                System.IO.File.Delete(dir + fileName);
            string fullPath = System.IO.Path.Combine(dir, fileName);

            using (System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }
        }
        public void SetReport(FrameworkUAD.Entity.Report report, int issueID, string issueName, bool download, RadButton sender = null)
        {
            FrameworkUAD_Lookup.Entity.Code c = new FrameworkUAD_Lookup.Entity.Code();
            codeSingleResponse = codeData.Proxy.SelectCodeId(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, report.ReportTypeID);
            if (codeSingleResponse.Result != null && codeSingleResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                c = codeSingleResponse.Result;
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

            FrameworkUAD.Object.ReportingXML objFilters = GetFilter();

            if (c != null)
            {
                try
                {
                    GC.Collect();
                    Telerik.Reporting.ReportSource rptSource = ReportUtilities.GetReportSource(c, report, countries, objFilters, myProductID, myProduct.PubName, issueID, issueName);
                    ReportUtilities.ReportFilters = objFilters;
                    ReportUtilities.ReportCode = c;
                    ReportUtilities.CurrentReport = report;
                    ReportUtilities.ProductId = myProductID;
                    if (download == true)
                    {
                        busy.IsBusy = true;
                        busy.IsIndeterminate = true;
                        busy.BusyContent = "Generating Report";
                        GC.Collect();
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += (o, ea) =>
                        {
                            ExportReport(rptSource, report);
                        };
                        bw.RunWorkerCompleted += (o, ea) =>
                        {
                            busy.IsBusy = false;
                            busy.IsIndeterminate = true;
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.InitialDirectory = "C:\\ADMS\\Reports";
                            ofd.ShowDialog();
                            sender.IsEnabled = true;
                        };
                        bw.RunWorkerAsync();
                    }
                    else
                    {
                        btnGenerater = sender;
                        rvReport.ReportSource = rptSource;
                        rvReport.RefreshReport();
                    }

                }
                catch (Exception ex)
                {
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alWorker.LogCriticalError(formatException, "SetReport", app, string.Empty, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                }

            }
        }

        public void ExportBPA(List<FrameworkUAD.Entity.Report> reports, int issueID, string issueName)
        {
            Dictionary<int, string> files = new Dictionary<int, string>();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            ReportUtilities.ProductId = myProductID;
            dlg.DefaultExt = "PDF";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf";
            Nullable<bool> dialResult = dlg.ShowDialog();
            if (dialResult == false)
            {
                return;
            }
            string filePath = Path.GetDirectoryName(dlg.FileName);
            Helpers.FilterOperations fo = new Helpers.FilterOperations();

            
            FrameworkUAD.Object.ReportingXML objFilters = GetBPAFilter();
            busy.IsBusy = true;
            busy.IsIndeterminate = false;
            int k = 0;
            double increment = Math.Truncate(Convert.ToDouble(100 / reports.Count()));
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                try
                {
                    int reportCount = 0;
                    foreach (FrameworkUAD.Entity.Report report in reports)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (k == 0)
                                busy.ProgressValue = 0;
                            else
                                busy.ProgressValue = busy.ProgressValue + increment;

                            busy.BusyContent = "Generating " + report.ReportName;

                            k++;
                        }));
                        FrameworkUAD_Lookup.Entity.Code c = reportCodes.SingleOrDefault(x => x.CodeId == report.ReportTypeID);
                        if (c != null)
                        {
                            Telerik.Reporting.ReportSource rptSource = ReportUtilities.GetReportSource(c, report, countries, objFilters, myProductID, myProduct.PubName, issueID, issueName);
                            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

                            System.Collections.Hashtable deviceInfo =
                                new System.Collections.Hashtable();

                            Telerik.Reporting.Processing.RenderingResult result =
                                reportProcessor.RenderReport("PDF", rptSource, deviceInfo);

                            string fileName = report.ReportName.Replace(" ", "");
                            string fullPath = System.IO.Path.Combine(filePath, fileName);

                            using (System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
                            {
                                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                            }
                            //files.Add(report.ReportID, fullPath);
                            files.Add(reportCount, fullPath);
                            reportCount++;
                        }
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        busy.ProgressValue = 100;
                        busy.BusyContent = "Generating BPA";
                    }));

                    if (files.Count == reports.Count)
                        CombinePDFs(files, dlg.FileName);
                    else
                    {
                        foreach (var r in reports)
                        {
                            try
                            {
                                if (files.ContainsKey(r.ReportID) == false)
                                {
                                    FrameworkUAD_Lookup.Entity.Code c = reportCodes.SingleOrDefault(x => x.CodeId == r.ReportTypeID);
                                    if (c != null)
                                    {
                                        Telerik.Reporting.ReportSource rptSource = ReportUtilities.GetReportSource(c, r, countries, objFilters, myProductID, myProduct.PubName, issueID, issueName);
                                        Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

                                        System.Collections.Hashtable deviceInfo =
                                            new System.Collections.Hashtable();

                                        Telerik.Reporting.Processing.RenderingResult result =
                                            reportProcessor.RenderReport("PDF", rptSource, deviceInfo);

                                        string fileName = r.ReportName.Replace(" ", "");
                                        string fullPath = System.IO.Path.Combine(filePath, fileName);

                                        using (System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
                                        {
                                            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                                        }
                                        files.Add(r.ReportID, fullPath);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Core_AMS.Utilities.WPF.Message("Some of your requested reports failed to generate. Please contact support if error keeps recurring.");
                                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExportBPA", app, "Reports");
                            }
                        }
                        CombinePDFs(files, dlg.FileName);
                    }
                }
                catch (Exception ex)
                {
                    Core_AMS.Utilities.WPF.Message("An error occurred while exporting your file. Please contact support if error keeps recurring.");
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExportBPA", app, "Reports");
                }
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                busy.IsBusy = false;
                busy.IsIndeterminate = true;
                busy.BusyContent = "Loading...";
                MessageBox.Show("Export complete. File has been saved to User's Folder");
            };
            bw.RunWorkerAsync();
        }

        public FrameworkUAD.Object.ReportingXML GetFilter()
        {
            FilterControls.Framework.FiltersViewModel vm = this.DataContext as FilterControls.Framework.FiltersViewModel;
            FrameworkUAD.Object.ReportingXML xml = new FrameworkUAD.Object.ReportingXML();
            xml = vm.ActiveFiltersXML;

            return xml;
        }

        public FrameworkUAD.Object.ReportingXML GetBPAFilter()
        {
            Helpers.FilterOperations fo = new Helpers.FilterOperations();

            string catIDs = "";
            string transIDs = "";

            int catQFreeID = catCodeTypes.SingleOrDefault(x => x.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")).CategoryCodeTypeID;
            int catQPaidID = catCodeTypes.SingleOrDefault(x => x.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")).CategoryCodeTypeID;
            int tranActiveFreeid = transCodeTypes.SingleOrDefault(x => x.TransactionCodeTypeName == FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Active.ToString().Replace("_", " ")).TransactionCodeTypeID;
            int tranActivePaidid = transCodeTypes.SingleOrDefault(x => x.TransactionCodeTypeName == FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Active.ToString().Replace("_", " ")).TransactionCodeTypeID;


            foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodes)
            {
                if ((cc.CategoryCodeTypeID == catQFreeID || cc.CategoryCodeTypeID == catQPaidID) && cc.CategoryCodeValue != 70)
                    catIDs += cc.CategoryCodeID + ",";
            }
            catIDs = catIDs.TrimEnd(',');

            foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in transCodes)
            {
                if (tc.TransactionCodeTypeID == tranActiveFreeid || tc.TransactionCodeTypeID == tranActivePaidid)
                {
                    transIDs += tc.TransactionCodeID + ",";
                }
            }
            transIDs = transIDs.TrimEnd(',');

            FrameworkUAD.Object.ReportingXML xml = fo.GetDefaultXMLFilter(myProductID, catIDs, transIDs);

            return xml;
        }

        private void CombinePDFs(Dictionary<int, string> files, string filepath)
        {
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(filepath, System.IO.FileMode.Create));

            //Open the output file
            sourceDocument.Open();

            try
            {
                //Loop through the files list
                for (int f = 0; f < files.Count; f++)
                {
                    int pages = Get_Page_Count(files[f]);

                    reader = new PdfReader(files[f]);
                    //Add pages of current file
                    for (int i = 1; i <= pages; i++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                        pdfCopyProvider.AddPage(importedPage);
                    }

                    reader.Close();
                }
                //At the end save the output file
                sourceDocument.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            for (int i = 0; i < files.Count; i++)
            {
                File.Delete(files[i]);
            }
        }
        private int Get_Page_Count(string file)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            busy = this.ParentOfType<RadBusyIndicator>();
        }

        private void rvReport_RenderingEnd(object sender, EventArgs e)
        {
            btnGenerater.IsEnabled = true;
        }
        private void rvReport_RenderingBegin(object sender, CancelEventArgs e)
        {
            btnGenerater.IsEnabled = false;
        }
        private void rvReport_Error(object sender, Telerik.Reporting.ErrorEventArgs eventArgs)
        {
            btnGenerater.IsEnabled = true;
        }
    }
}
