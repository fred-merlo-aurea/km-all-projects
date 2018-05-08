using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KM.Common.Functions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Telerik.Pivot.Core;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Pivot.Export;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for DataFetcher.xaml
    /// </summary>
    public partial class DataFetcher : UserControl
    {
        #region Resources
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueData = FrameworkServices.ServiceClient.UAD_IssueClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> subscriptionData = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportsData = FrameworkServices.ServiceClient.UAD_ReportsClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();

        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>> reportResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>>();
        private FrameworkUAS.Service.Response<int> intResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Issue>> issueResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Issue>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<int>> subCountResponse = new FrameworkUAS.Service.Response<List<int>>();
        private FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code> codeSingleResponse = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code>();

        private Guid accessKey;
        private int myProductID;
        private FrameworkUAD.Entity.Report myReport = new FrameworkUAD.Entity.Report();
        public List<FrameworkUAD.Entity.Issue> issueList;
        private List<FrameworkUAD.Entity.Report> reports = new List<FrameworkUAD.Entity.Report>();
        private RadBusyIndicator busy = new RadBusyIndicator();
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        private List<RowDemoTotals> demoTotals = new List<RowDemoTotals>();
        private int uniqueCopiesCount = 0;
        private TelerikReportViewer ReportViewer;
        public class RowDemoTotals
        {
            public int RowNumber { get; set; }
            public int Print { get; set; }
            public int Digital { get; set; }
            public int Both { get; set; }
            public int Total { get; set; }
        }
        public List<FrameworkUAD.Entity.Product> myProducts { get; set; }
        public List<FrameworkUAD.Entity.Product> displayProducts { get; set; }
        #endregion

        public DataFetcher(bool isCirc, bool isUad)
        {
            InitializeComponent();
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            if(issueList == null)
                issueList = new List<FrameworkUAD.Entity.Issue>();

            if (myProducts == null || myProducts.Count == 0)
            {
                productResponse = productData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (Helpers.Common.CheckResponse(productResponse.Result, productResponse.Status))
                {
                    myProducts = productResponse.Result.OrderBy(x => x.PubName).ToList(); 
                }
            }
            displayProducts = new List<FrameworkUAD.Entity.Product>();
            if (isCirc == true)
                displayProducts.AddRange(productResponse.Result.Where(x => x.IsCirc == true).ToList());
            if (isUad == true)
                displayProducts.AddRange(productResponse.Result.Where(x => x.IsUAD == true).ToList());
            rcbProducts.ItemsSource = displayProducts.OrderBy(x => x.PubName).ToList();
        }

        private void Initialize(int productID)
        {
            myProductID = productID;
            if (issueList == null)
                issueList = new List<FrameworkUAD.Entity.Issue>();

            if (myProducts == null || myProducts.Count == 0)
            {
                productResponse = productData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (Helpers.Common.CheckResponse(productResponse.Result, productResponse.Status))
                {
                    myProducts = productResponse.Result.OrderBy(x => x.PubName).ToList();
                }
            }

            myProduct = myProducts.Where(x => x.PubID == productID).SingleOrDefault();
            if (myProduct == null)
                myProduct = new FrameworkUAD.Entity.Product();

            reportResponse = reportsData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(reportResponse.Result, reportResponse.Status))
            {
                //Uses the reports in database
                reports = reportResponse.Result.Where(x => x.ProductID == myProductID).OrderBy(x => x.ReportName).ToList();
                //reports.Add(new FrameworkUAD.Entity.Report() { ReportName = "Add Remove", URL = "Add Remove", IsActive = true, IsCrossTabReport = false, Status = true });
                reports.Add(new FrameworkUAD.Entity.Report() { ReportName = "BPA", URL = "BPA Report", IsActive = true, IsCrossTabReport = false, Status = true, ReportTypeID = 0 });
                rcbReports.ItemsSource = reports;
                rcbReports.SelectedValuePath = "ReportID";
                rcbReports.DisplayMemberPath = "ReportName";
            }
            Window w = Window.GetWindow(this);
            FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
            FilterControls.Framework.FiltersViewModel vm = fw.MyViewModel;
            vm.Initialize(myProductID);
            fw.SetDefaultStandardFilters();

            ReportViewer = new TelerikReportViewer(myProductID);
            ReportViewer.DataContext = vm;
            spView.Children.Clear();
            spView.Children.Add(ReportViewer);

            issueResponse = issueData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(issueResponse.Result, issueResponse.Status))
            {
                issueList = issueResponse.Result.Where(x => x.PublicationId == myProductID).ToList();
                rcbIssues.ItemsSource = issueList;
                FrameworkUAD.Entity.Issue currIssue = issueList.Where(x => x.IsComplete == false).FirstOrDefault();
                if (currIssue != null)
                {
                    currIssue.IssueId = 0;
                    rcbIssues.SelectedIndex = issueList.IndexOf(currIssue);
                }
                else
                    Core_AMS.Utilities.WPF.MessageError("There is no issue created for this publication. Please go to Open Close and open an issue before running reports.");
            }
        }

        #region UI Events
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnGenerate.IsEnabled = false;
                Generatereport((RadButton) sender);
                //btnGenerate.IsEnabled = true;
            }
            catch(Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
        }

        private void Generatereport(RadButton sender = null)
        {
            if (rcbReports.SelectedItem != null)
            {
                if (rcbIssues.SelectedValue != null)
                {
                    int id = 0;
                    FrameworkUAD.Entity.Issue selectedIssue = rcbIssues.SelectedItem as FrameworkUAD.Entity.Issue;
                    int.TryParse(rcbIssues.SelectedValue.ToString(), out id);
                    FrameworkUAD.Entity.Report item = rcbReports.SelectedItem as FrameworkUAD.Entity.Report;
                    if (item.ReportName.ToString() == FrameworkUAD_Lookup.Enums.ReportTypes.BPA.ToString())
                    {
                        WindowsAndDialogs.BPACustomization bpa = new WindowsAndDialogs.BPACustomization(reports.Where(x => x.ReportTypeID != 0).ToList());
                        ObservableCollection<FrameworkUAD.Entity.Report> selected = new ObservableCollection<FrameworkUAD.Entity.Report>();
                        bpa.Answer += value => selected = value;
                        bpa.ShowDialog();
                        if (selected != null && selected.Count > 0)
                        {
                            ReportViewer.ExportBPA(selected.ToList(), id, selectedIssue.IssueName);
                            sender.IsEnabled = true;
                        }
                    }
                    else
                        ReportViewer.SetReport(item, id, selectedIssue.IssueName, chkDownload.IsChecked.Value, sender);
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("Please select an Issue.");
                    btnGenerate.IsEnabled = true;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageError("Please select a Report.");
                btnGenerate.IsEnabled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserControl uc = this.ParentOfType<UserControl>();
            busy = uc.FindChildByType<RadBusyIndicator>();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            #region Export
            List<RadGridView> grds = new List<RadGridView>();
            ExportFormat ef = new ExportFormat();
            string extension = "";
            string reportName = "";
            //if (rcbFileExport.Text != null)
            //{
            //    extension = rcbFileExport.SelectedItem.ToString().ToLower();
            //    switch (extension)
            //    {
            //        case "csv":
            //            ef = ExportFormat.Csv;
            //            break;
            //        case "html":
            //            ef = ExportFormat.Html;
            //            break;
            //        case "xls":
            //            ef = ExportFormat.Xlsx;
            //            break;
            //        case "pdf":
            //            ef = ExportFormat.Pdf;
            //            break;
            //        case "text":
            //            ef = ExportFormat.Text;
            //            break;
            //    }
            //}
            if (rcbReports.SelectedItem != null)
            {
                FrameworkUAD.Entity.Report r = rcbReports.SelectedItem as FrameworkUAD.Entity.Report;
                reportName = r.ReportName;
            }
            grds = this.ChildrenOfType<RadGridView>().ToList();
            RadPivotGrid crossTabGrid = this.ChildrenOfType<RadPivotGrid>().FirstOrDefault();
            if (grds.Count > 0 && extension != "" && crossTabGrid.Visibility == Visibility.Collapsed)
            {
                #region Other Extensions
                if (extension != "pdf")
                {
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        DefaultExt = extension,
                        Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, extension.ToUpper().ToString()),
                        FilterIndex = 1,
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        using (Stream stream = dialog.OpenFile())
                        {
                            foreach (RadGridView grd in grds)
                            {
                                grd.Export(stream,
                                new GridViewExportOptions()
                                {
                                    Format = ef,
                                    ShowColumnHeaders = true,
                                    ShowColumnFooters = true,
                                    ShowGroupFooters = false
                                });
                            }
                        }
                    }
                }
                #endregion
                #region PDF
                else if (extension == "pdf")
                {
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        DefaultExt = extension,
                        Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, extension.ToUpper().ToString()),
                        FilterIndex = 1,
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        int grdCount = grds.Where(x => x.ItemsSource != null).Count();
                        List<string> files = new List<string>();
                        int count = 1;
                        string ext = "";
                        if (grdCount > 1)
                        {
                            foreach (RadGridView grd in grds)
                            {
                                if (grd.ItemsSource != null)
                                {
                                    DataTable src = grd.ItemsSource as DataTable;
                                    string noExt = dialog.FileName.Remove(dialog.FileName.Count() - 4);
                                    ext = count.ToString();
                                    if (grd.Name == "grdBottom")
                                        files.Add(ExportToPdf(src, noExt + ext + ".pdf", reportName, false));
                                    else
                                        files.Add(ExportToPdf(src, noExt + ext + ".pdf", reportName));
                                    count++;
                                }
                            }
                            if (files.Count > 1)
                            {
                                CombinePDFs(files, dialog.SafeFileName, dialog.FileName);
                            }
                        }
                        else
                        {
                            foreach (RadGridView grd in grds)
                            {
                                if (grd.ItemsSource != null)
                                {
                                    DataTable src = grd.ItemsSource as DataTable;
                                    files.Add(ExportToPdf(src, dialog.FileName, reportName));
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            else if (crossTabGrid != null && crossTabGrid.Visibility == Visibility.Visible)
            {
                LocalDataSourceProvider provider = crossTabGrid.DataProvider as LocalDataSourceProvider;
                DataTable dt = (DataTable)provider.ItemsSource;

                if (dt != null)
                {
                    var query = from row in dt.AsEnumerable()
                                group row by new { Key = row.Field<int>("PubSubscriptionID") } into g
                                select new
                                {
                                    GroupDescription = g.Key,
                                    Copies = (from value in g.ToList()
                                              select value.Field<int>("Copies")).FirstOrDefault()
                                };

                    uniqueCopiesCount = query.Select(x => x.Copies).Sum();

                    List<string> uniqueRows = (from r in dt.AsEnumerable()
                                               select (string)r[myReport.Row]).Distinct().ToList();

                    int rowIndex = dt.Columns.IndexOf(myReport.Row);
                    int demoIndex = dt.Columns.IndexOf("Demo7");

                    for (int i = 0; i < uniqueRows.Count; i++)
                    {
                        RowDemoTotals total = new RowDemoTotals();
                        total.RowNumber = i;
                        dt.Rows.Cast<DataRow>().Where(r => r.ItemArray[rowIndex].Equals(uniqueRows[i])).ToList().ForEach(r =>
                        {
                            if (r.ItemArray[demoIndex].Equals("A"))
                                total.Print += (int)r["Copies"];
                            else if (r.ItemArray[demoIndex].Equals("B"))
                                total.Digital += (int)r["Copies"];
                            else if (r.ItemArray[demoIndex].Equals("C"))
                                total.Both += (int)r["Copies"];
                        });
                        demoTotals.Add(total);
                    }
                }
                ExportToExcel();
                Core_AMS.Utilities.WPF.Message("Export Complete.");
            }
            #endregion
        }

        private void btnDownloadDetails_Click(object sender, RoutedEventArgs e)
        {
            if (rcbProducts.SelectedIndex >= 0)
            {
                #region prep datacall
                DataTable master = new DataTable();
                HashSet<int> hs = new HashSet<int>();
                Window w = Window.GetWindow(this);
                FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
                FilterControls.Framework.FiltersViewModel vm = fw.MyViewModel;
                FrameworkUAD.Object.ReportingXML xml = new FrameworkUAD.Object.ReportingXML();
                if (vm != null)
                {
                    xml = vm.ActiveFiltersXML;
                }
                //Helpers.FilterOperations.FilterContainer fc = new Helpers.FilterOperations.FilterContainer();
                //Helpers.FilterOperations fo = new Helpers.FilterOperations();
                //fc.Filter.ProductId = myProductID;
                //fc.Filter.IsActive = true;

                //Window w = Window.GetWindow(this);
                //StandardDemographics filters = w.FindChildByType<StandardDemographics>();
                //DynamicDemographics reportFilters = w.FindChildByType<DynamicDemographics>();
                //AdHocFilters adHocFilters = w.FindChildByType<AdHocFilters>();

                //fc.FilterDetails.AddRange(filters.GetSelection());
                //fc.FilterDetails.Add(reportFilters.GetSelection());
                //fc.FilterDetails.AddRange(adHocFilters.GetSelection());

                string fields = "";
                DownloadDetails dd = new DownloadDetails(myProductID);
                dd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dd.Check += value => fields = value;
                dd.ShowDialog();

                if (fields == "")
                    return;
                busy.IsBusy = true;
                busy.IsIndeterminate = true;
                BackgroundWorker bw = new BackgroundWorker();
                int issueId = 0;
                if (rcbIssues.SelectedValue != null)
                    issueId = (int)rcbIssues.SelectedValue;
                #endregion

                bw.DoWork += (o, ea) =>
                {
                    FrameworkUAD.Entity.Issue selectedIssue = issueList.SingleOrDefault(x => x.IssueId == issueId);
                    FrameworkUAD.Object.Reporting obj = new FrameworkUAD.Object.Reporting();
                    List<int> subIDs = new List<int>();
                    if (selectedIssue.IsComplete == false)
                        subCountResponse = reportsData.Proxy.SelectSubscriberCount(accessKey, xml.Filters, xml.AdHocFilters, false, false, 0, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    else
                        subCountResponse = reportsData.Proxy.SelectSubscriberCount(accessKey, xml.Filters, xml.AdHocFilters, false, true, issueId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

                    if (Helpers.Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
                    {
                        hs = new HashSet<int>(subCountResponse.Result);
                        subIDs = subCountResponse.Result;
                    }

                    #region GetSubscribers
                    DataTable dt = new DataTable();
                    int rowProcessedCount = 0;
                    int index = 0;
                    int size = 2500;
                    while (master.Rows.Count < hs.Count)
                    {
                        if ((index + 2500) > subIDs.Count)
                            size = subIDs.Count - index;
                        List<int> temp = subIDs.GetRange(index, size);
                        index += 2500;
                        if (selectedIssue != null)
                        {
                            if (selectedIssue.IsComplete == false)
                                dt = subscriptionData.Proxy.SelectForExportStatic(accessKey, myProductID, fields, temp, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                            else
                                dt = subscriptionData.Proxy.SelectForExportStatic(accessKey, myProductID, issueId, fields, temp, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                        }
                        else
                        {
                            dt = subscriptionData.Proxy.SelectForExportStatic(accessKey, myProductID, fields, temp, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                        }
                        rowProcessedCount += dt.Rows.Count;

                        dt.AcceptChanges();
                        master.Merge(dt);
                    }
                    if (master.Columns.Contains("PubSubscriptionID"))
                        master.Columns.Remove("PubSubscriptionID");
                    master.AcceptChanges();
                    #endregion
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    #region completed work
                    busy.IsBusy = false;
                    RadGridView grd = new RadGridView();
                    grd.AutoGenerateColumns = true;
                    grd.ItemsSource = master;
                    string extension = "csv";
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        DefaultExt = extension,
                        Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "CSV"),
                        FilterIndex = 1
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        using (Stream stream = dialog.OpenFile())
                        {
                            string test =  grd.ToCsv();
                            StreamWriter sw = new StreamWriter(stream);
                            
                            test = HtmlFunctions.StripTextFromHtml(test);

                            sw.Write(test);
                            sw.Flush();
                            sw.Close();
                            
                        }
                    }
                    Core_AMS.Utilities.WPF.Message("Download Complete.");
                    #endregion
                };
                bw.RunWorkerAsync();
            }
            else
                Core_AMS.Utilities.WPF.Message("Please select a product.", MessageBoxButton.OK, MessageBoxImage.Warning, "Product required");
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FilterControls.Framework.FiltersViewModel vm = e.NewValue as FilterControls.Framework.FiltersViewModel;
            if (vm != null)
            {
                //Initialize(vm.SelectedPubID);
                //vm.PropertyChanged += (o, i) =>
                //{
                //    if (myProductID != vm.SelectedPubID)
                //        Initialize(vm.SelectedPubID);
                //};
            }
        }

        private void rcbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int pubid = (int)rcbProducts.SelectedValue;
            //BackgroundWorker bw = new BackgroundWorker();
            Initialize(pubid);

            //does not work - UI not updated
            //bw.DoWork += (o, ea) =>
            //{
            //    Initialize(pubid);
            //};
            //bw.RunWorkerCompleted += (o, ea) =>
            //{
            //    //this.Dispatcher.InvokeAsync(null, System.Windows.Threading.DispatcherPriority.Render);
            //    Window w = Window.GetWindow(this);
            //    FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
            //    fw.Dispatcher.InvokeAsync(null, System.Windows.Threading.DispatcherPriority.Render);
            //};

            //bw.RunWorkerAsync();
            btnDownloadDetails.IsEnabled = true;
            //did not help - rcbProducts.Dispatcher.InvokeAsync(null, System.Windows.Threading.DispatcherPriority.Render);
            //FilterControls.Framework.FiltersViewModel vm = ReportViewer.DataContext as FilterControls.Framework.FiltersViewModel;
            //vm.Initialize(pubid);
        }

        #endregion

        #region Exporting
        public string ExportToPdf(DataTable dt, string fileName, string reportName, bool includeHeader = true)
        {
            string filePath = "";
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = System.IO.Path.Combine(pathUser, "Downloads");
                Directory.CreateDirectory(pathDownload);

                //string filePath = pathDownload + "\\" + fileName;
                filePath = fileName;

                Document document = new Document();
                //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);
                iTextSharp.text.Font font7 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 7);

                Chunk chunk1 = new Chunk(reportName + "\n", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD));
                Chunk chunk2 = new Chunk("For Product:   " + myProduct.PubName + "\n" + "Issue Date:    " + myProduct.IssueDate + "\n" + "As of Date:    " + DateTime.Now.ToShortDateString() + "\n" + " ", font7);
                Paragraph para = new Paragraph();
                Phrase p = new Phrase();
                p.AddRange(new List<Chunk>() { chunk1, chunk2 });
                para.AddRange(p);
                para.SetLeading(10, 0);
                para.Alignment = Element.ALIGN_LEFT;

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.WidthPercentage = 100;

                foreach (DataColumn c in dt.Columns)
                {

                    table.AddCell(new Phrase(c.ColumnName, font5));
                }

                foreach (DataRow r in dt.Rows)
                {
                    for (int i = 0; i < r.ItemArray.Count(); i++)
                    {
                        table.AddCell(new Phrase(r[i].ToString(), font5));
                    }
                }

                if (includeHeader == true)
                    document.Add(para);
                document.Add(table);
                document.Close();
            }
            catch
            {
                Core_AMS.Utilities.WPF.Message("An error occurred while exporting your file. Please contact support if error keeps recurring.");
            }

            return filePath;
        }

        private void CombinePDFs(List<string> files, string outputName, string filepath)
        {
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = System.IO.Path.Combine(pathUser, "Downloads");
            pathDownload += "\\" + outputName;

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

        #region Export CrossTabNew
        private void Export_Report(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
            Core_AMS.Utilities.WPF.Message("Export complete.");
        }
        private void ExportToExcel()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "xlsx";
            dialog.Filter = "Excel Workbook (xlsx) | *.xlsx |All Files (*.*) | *.*";

            var result = dialog.ShowDialog();
            if ((bool)result)
            {
                try
                {
                    using (var stream = dialog.OpenFile())
                    {
                        var workbook = GenerateWorkbook();

                        XlsxFormatProvider provider = new XlsxFormatProvider();
                        provider.Export(workbook, stream);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private Workbook GenerateWorkbook()
        {
            #region Basic Formatting
            RadPivotGrid radPivotGrid = this.ChildrenOfType<RadPivotGrid>().FirstOrDefault();
            var export = radPivotGrid.GenerateExport();

            Workbook workbook = new Workbook();
            workbook.History.IsEnabled = false;

            var worksheet = workbook.Worksheets.Add();

            workbook.SuspendLayoutUpdate();
            int rowCount = export.RowCount;
            int columnCount = export.ColumnCount;

            var allCells = worksheet.Cells[0, 0, rowCount - 1, columnCount - 1];
            allCells.SetFontFamily(new ThemableFontFamily(radPivotGrid.FontFamily));
            allCells.SetFontSize(12);
            allCells.SetFill(GenerateFill(radPivotGrid.Background));

            foreach (var cellInfo in export.Cells)
            {
                int rowStartIndex = cellInfo.Row;
                int rowEndIndex = rowStartIndex + cellInfo.RowSpan - 1;
                int columnStartIndex = cellInfo.Column;
                int columnEndIndex = columnStartIndex + cellInfo.ColumnSpan - 1;

                CellSelection cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex];

                var value = cellInfo.Value;
                if (value != null)
                {
                    cellSelection.SetValue(Convert.ToString(value));
                    cellSelection.SetVerticalAlignment(RadVerticalAlignment.Center);
                    cellSelection.SetHorizontalAlignment(GetHorizontalAlignment(cellInfo.TextAlignment));
                    int indent = cellInfo.Indent;
                    if (indent > 0)
                    {
                        cellSelection.SetIndent(indent);
                    }
                }

                cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex];

                SetCellProperties(cellInfo, cellSelection);
            }

            for (int i = 0; i < columnCount; i++)
            {
                var columnSelection = worksheet.Columns[i];
                columnSelection.AutoFitWidth();

                //NOTE: workaround for incorrect autofit.
                var newWidth = worksheet.Columns[i].GetWidth().Value.Value + 15;
                columnSelection.SetWidth(new ColumnWidth(newWidth, false));
            }
            #endregion
            #region Formatting Demo7 Counts
            worksheet.Columns.Insert(1); // 1 Print
            worksheet.Columns.Insert(1); // 2 Digital
            worksheet.Columns.Insert(1); // 3 Both
            columnCount += 3;
            CellSelection printH = worksheet.Cells[0, 1];
            printH.SetValue("Print");
            CellSelection digitalH = worksheet.Cells[0, 2];
            digitalH.SetValue("Digital");
            CellSelection bothH = worksheet.Cells[0, 3];
            bothH.SetValue("Both");

            foreach (RowDemoTotals t in demoTotals)
            {
                CellSelection print = worksheet.Cells[t.RowNumber + 1, 1];
                print.SetValue(t.Print);
                CellSelection digital = worksheet.Cells[t.RowNumber + 1, 2];
                digital.SetValue(t.Digital);
                CellSelection both = worksheet.Cells[t.RowNumber + 1, 3];
                both.SetValue(t.Both);
            }
            ColumnSelection columns = worksheet.Columns[1, 3];
            columns.AutoFitWidth();
            #endregion
            #region Formatting the Title and Unique Total
            worksheet.Rows.Insert(0); // 0 Title
            worksheet.Rows.Insert(0); // 1 Pub Info
            worksheet.Rows.Insert(0); // 2 Issue Date
            worksheet.Rows.Insert(0); // 3 Current Date
            worksheet.Rows.Insert(0); // Space
            rowCount += 6;
            worksheet.Rows.Insert(rowCount - 1); //Unique Count Totals
            #region Header Info - Title, Issue Date, etc.
            CellSelection corner = worksheet.Cells[0, 0];
            corner.SetValue(myReport.Row + " x " + myReport.Column);
            corner.SetFontSize(18);
            corner.SetIsBold(true);
            corner.SetVerticalAlignment(RadVerticalAlignment.Bottom);
            corner.SetHorizontalAlignment(RadHorizontalAlignment.Left);
            corner.SetIndent(2);
            CellSelection title = worksheet.Cells[1, 0];
            title.SetValue("For Magazine: ");
            title.SetIsBold(true);
            title.SetVerticalAlignment(RadVerticalAlignment.Bottom);
            title.SetHorizontalAlignment(RadHorizontalAlignment.Left);
            title.SetIndent(2);
            if (myProduct != null)
            {
                CellSelection pub = worksheet.Cells[1, 1];
                pub.SetValue(myProduct.PubName);
                pub.SetVerticalAlignment(RadVerticalAlignment.Bottom);
                pub.SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
            CellSelection issueTitle = worksheet.Cells[2, 0];
            issueTitle.SetValue("Issue Date: ");
            issueTitle.SetIsBold(true);
            issueTitle.SetVerticalAlignment(RadVerticalAlignment.Bottom);
            issueTitle.SetHorizontalAlignment(RadHorizontalAlignment.Left);
            issueTitle.SetIndent(2);
            if (myProduct != null)
            {
                CellSelection issueDate = worksheet.Cells[2, 1];
                FrameworkUAD.Entity.Issue currIssue = issueList.Where(x => x.PublicationId == myProduct.PubID && x.IsComplete == false).FirstOrDefault();
                if (currIssue != null)
                {
                    issueDate.SetValue(currIssue.DateCreated.ToShortDateString());
                    issueDate.SetVerticalAlignment(RadVerticalAlignment.Bottom);
                    issueDate.SetHorizontalAlignment(RadHorizontalAlignment.Left);
                }
            }
            CellSelection dateTitle = worksheet.Cells[3, 0];
            dateTitle.SetValue("As of Date: ");
            dateTitle.SetIsBold(true);
            dateTitle.SetVerticalAlignment(RadVerticalAlignment.Bottom);
            dateTitle.SetHorizontalAlignment(RadHorizontalAlignment.Left);
            dateTitle.SetIndent(2);
            CellSelection date = worksheet.Cells[3, 1];
            date.SetValue(DateTime.Now.ToShortDateString());
            date.SetVerticalAlignment(RadVerticalAlignment.Bottom);
            date.SetHorizontalAlignment(RadHorizontalAlignment.Left);
            #endregion
            #region Bottom Row - Unique Total
            CellSelection bottomCorner = worksheet.Cells[rowCount, 0];
            bottomCorner.SetIsBold(true);
            bottomCorner.SetValue("Unique Total");
            bottomCorner.SetVerticalAlignment(RadVerticalAlignment.Bottom);
            bottomCorner.SetHorizontalAlignment(RadHorizontalAlignment.Left);
            CellSelection total = worksheet.Cells[rowCount, columnCount - 1];
            total.SetIsBold(true);
            total.SetValue(uniqueCopiesCount);
            total.SetVerticalAlignment(RadVerticalAlignment.Bottom);
            total.SetHorizontalAlignment(RadHorizontalAlignment.Right);
            var bottomRom = worksheet.Cells[rowCount, 0, rowCount, columnCount - 1];
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#B2B2B2");
            bottomRom.SetFill(GenerateFill(brush));
            #endregion
            #endregion
            workbook.ResumeLayoutUpdate();
            return workbook;
        }
        private RadHorizontalAlignment GetHorizontalAlignment(TextAlignment textAlignment)
        {
            switch (textAlignment)
            {
                case TextAlignment.Center:
                    return RadHorizontalAlignment.Center;

                case TextAlignment.Left:
                    return RadHorizontalAlignment.Left;

                case TextAlignment.Right:
                    return RadHorizontalAlignment.Right;

                case TextAlignment.Justify:
                default:
                    return RadHorizontalAlignment.Justify;
            }
        }
        private static void SetCellProperties(PivotExportCellInfo cellInfo, CellSelection cellSelection)
        {
            var fill = GenerateFill(cellInfo.Background);
            if (fill != null)
            {
                cellSelection.SetFill(fill);
            }

            SolidColorBrush solidBrush = cellInfo.Foreground as SolidColorBrush;
            if (solidBrush != null)
            {
                cellSelection.SetForeColor(new ThemableColor(solidBrush.Color));
            }

            if (cellInfo.FontWeight.HasValue && cellInfo.FontWeight.Value != FontWeights.Normal)
            {
                cellSelection.SetIsBold(true);
            }

            SolidColorBrush solidBorderBrush = cellInfo.BorderBrush as SolidColorBrush;
            if (solidBorderBrush != null && cellInfo.BorderThickness.HasValue)
            {
                var borderThickness = cellInfo.BorderThickness.Value;
                var color = new ThemableColor(solidBorderBrush.Color);
                var leftBorder = new CellBorder(GetBorderStyle(borderThickness.Left), color);
                var topBorder = new CellBorder(GetBorderStyle(borderThickness.Top), color);
                var rightBorder = new CellBorder(GetBorderStyle(borderThickness.Right), color);
                var bottomBorder = new CellBorder(GetBorderStyle(borderThickness.Bottom), color);
                var insideBorder = cellInfo.Background != null ? new CellBorder(CellBorderStyle.None, color) : null;
                cellSelection.SetBorders(new CellBorders(leftBorder, topBorder, rightBorder, bottomBorder, insideBorder, insideBorder, null, null));
            }
        }
        private static CellBorderStyle GetBorderStyle(double thickness)
        {
            if (thickness < 1)
            {
                return CellBorderStyle.None;
            }
            else if (thickness < 2)
            {
                return CellBorderStyle.Thin;
            }
            else if (thickness < 3)
            {
                return CellBorderStyle.Medium;
            }
            else
            {
                return CellBorderStyle.Thick;
            }
        }
        private static IFill GenerateFill(Brush brush)
        {
            if (brush != null)
            {
                SolidColorBrush solidBrush = brush as SolidColorBrush;
                if (solidBrush != null)
                {
                    return PatternFill.CreateSolidFill(solidBrush.Color);
                }
            }

            return null;
        }
        #endregion

        private void btnRest_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            btnGenerate.IsEnabled = true;
        }
    }
}
