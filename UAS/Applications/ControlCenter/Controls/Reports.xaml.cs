using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> rWorker = FrameworkServices.ServiceClient.UAD_ReportsClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPub = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublication = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>> svReport = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>>();

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> allPublications = new List<KMPlatform.Object.Product>();
        List<FrameworkUAD.Entity.Report> allReports = new List<FrameworkUAD.Entity.Report>();

        KMPlatform.Entity.Client currentPublisher = new KMPlatform.Entity.Client();
        KMPlatform.Object.Product currentPublication = new KMPlatform.Object.Product();
        FrameworkUAD.Entity.Report currentReport = new FrameworkUAD.Entity.Report();
        #endregion
        public Reports(int PublicationID = 0, int PublisherID = 0)
        {
            InitializeComponent();
            btnSave.IsEnabled = false;
            LoadData();
        }

        public void LoadData(int PublicationID = 0)
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svPub = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                //svPublication = pubWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                svReport = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publications
                allPublications = new List<KMPlatform.Object.Product>();
                foreach (var p in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups)
                {
                    foreach (var c in p.Clients)
                    {
                        foreach (var a in c.Products)
                        {
                            if (!allPublications.Select(x=> x.ProductCode).Contains(a.ProductCode))
                                allPublications.Add(a);
                        }
                    }
                }
                cbMagazine.ItemsSource = null;
                cbMagazine.ItemsSource = allPublications.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).OrderBy(x=> x.ProductCode).ToList();
                cbMagazine.SelectedValuePath = "ProductID";
                cbMagazine.DisplayMemberPath = "ProductCode";

                #endregion                
                #region Load Reports
                if (svReport.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allReports = svReport.Result;
                    gridReport.ItemsSource = null;
                    //if(PublicationID > 0)
                    //    gridReport.ItemsSource = allReports.Where(x => x.ProductID == PublicationID);

                    if (PublicationID > 0)
                    {
                        currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PublicationID);
                        cbMagazine.SelectedValue = currentPublication.ProductID;
                        gridReport.ItemsSource = allReports.Where(x => x.ProductID == PublicationID);
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.Report))
                {
                    FrameworkUAD.Entity.Report rItem = (FrameworkUAD.Entity.Report)b.DataContext;
                    if (rItem != null)
                    {
                        btnSave.IsEnabled = true;

                        currentReport = rItem;

                        tbxReportName.Text = rItem.ReportName;

                        btnSave.Tag = rItem.ReportID.ToString();                        
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS10.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS9.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Change Button tag to zero content back to save
            btnSave.Tag = "0";
            btnSave.IsEnabled = false;

            //Set currentItem to null
            currentReport = null;

            //Clear control
            tbxReportName.Text = "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int ID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out ID);

            FrameworkUAD.Entity.Report myReport = allReports.Where(x => x.ReportID == ID).FirstOrDefault();

            string Name = tbxReportName.Text;

            if (string.IsNullOrEmpty(Name))
            {
                Core_AMS.Utilities.WPF.Message("No report name was provided. Please provide a report name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }

            if (ID > 0 && myReport != null)
            {
                //Check not value existence name and code based by client
                if (allReports.FirstOrDefault(x => x.ReportName.Equals(Name, StringComparison.CurrentCultureIgnoreCase) && x.ReportID != ID && x.ProductID == myReport.ProductID) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Report name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }

            currentReport.ReportName = Name;

            #region Save|Update
            FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svSave = rWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, currentReport);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Refresh|Clear
                if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    //Set currentItem to null
                    currentReport = null;

                    //Change Button tag to zero content back to save
                    btnSave.Tag = "0";                    

                    //Clear control
                    tbxReportName.Text = "";                    

                    //Refresh Grid
                    LoadData(currentPublication.ProductID);

                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                }
                else
                    Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");

                #endregion

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
            #endregion

            btnSave.IsEnabled = false;
        }

        private void cbMagazine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMagazine.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(cbMagazine.SelectedValue.ToString(), out PID);
                currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PID);
                gridReport.ItemsSource = null;
                gridReport.ItemsSource = allReports.Where(x=> x.ProductID == currentPublication.ProductID);//.Where(x => x.MagazineID == PublicationID);
            }
        }
    }
}
