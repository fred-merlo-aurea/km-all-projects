using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for ReportGroupOrder.xaml
    /// </summary>
    public partial class ReportGroupOrder : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> rWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rtWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IReportGroups> rgWorker = FrameworkServices.ServiceClient.UAD_ReportGroupsClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPub = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublication = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> svResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>> svReportGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>>();    
        FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();    

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> allPublications = new List<KMPlatform.Object.Product>();
        List<FrameworkUAD.Entity.CodeSheet> allResponse = new List<FrameworkUAD.Entity.CodeSheet>();
        List<FrameworkUAD.Entity.ResponseGroup> allResponseGroup = new List<FrameworkUAD.Entity.ResponseGroup>();
        List<FrameworkUAD.Entity.ReportGroups> allReportGroups = new List<FrameworkUAD.Entity.ReportGroups>();
        ObservableCollection<FrameworkUAD.Entity.ReportGroups> currentReports = new ObservableCollection<FrameworkUAD.Entity.ReportGroups>();
        List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();

        KMPlatform.Entity.Client currentPublisher = new KMPlatform.Entity.Client();
        KMPlatform.Object.Product currentPublication = new KMPlatform.Object.Product();
        FrameworkUAD.Entity.ResponseGroup currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();
        #endregion
        public ReportGroupOrder()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            currentPublisher = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svPub = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                svResponseGroup = rtWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svResponse = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svReportGroup = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
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
                            if (!allPublications.Contains(a))
                                allPublications.Add(a);
                        }
                    }
                }

                if (currentPublisher != null)
                {
                    cbMagazine.ItemsSource = allPublications.Where(x => x.ClientID == currentPublisher.ClientID).ToList(); ;
                    cbMagazine.SelectedValuePath = "ProductID";
                    cbMagazine.DisplayMemberPath = "ProductCode";

                    if (cbMagazine.SelectedValue != null)
                    {
                        int PublicationID = 0;
                        int.TryParse(cbMagazine.SelectedValue.ToString(), out PublicationID);
                        currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PublicationID);
                    }
                }
                #endregion
                #region Load ResponseTypes
                if (svResponseGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allResponseGroup = svResponseGroup.Result;
                    if (currentPublication != null)
                    {
                        cbGroup.ItemsSource = null;
                        cbGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == currentPublication.ProductID).ToList();
                        cbGroup.SelectedValuePath = "ResponseGroupID";
                        cbGroup.DisplayMemberPath = "ResponseGroupName";
                    }

                    //lbxResponse.Items.Clear();
                    //foreach (FrameworkUAD.Entity.ResponseGroup rt in allResponseGroup)
                    //{
                    //    lbxResponse.Items.Add(rt.DisplayName);
                    //}
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                #region Load Response
                if (svResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allResponse = svResponse.Result;
                    if (currentPublication != null)
                    {
                        cbGroup.ItemsSource = allResponse.Where(x => x.PubID == currentPublication.ProductID).ToList();

                        //if (currentResponseGroup != null)
                        //{
                        //    lbxResponse.Items.Clear();
                        //    foreach (FrameworkUAD.Entity.CodeSheet rg in allResponse.Where(x => x.PubID == currentPublication.ProductID && x.ResponseGroupID == currentResponseGroup.ResponseGroupID))
                        //    {
                        //        lbxResponse.Items.Add(rg.ResponseDesc);
                        //    }
                        //}
                    }              
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                #region Load Report Groups
                if (svReportGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allReportGroups = svReportGroup.Result.ToList();
                    lbxReport.ItemsSource = currentReports;
                    lbxReport.SelectedValuePath = "ReportGroupID";
                    lbxReport.DisplayMemberPath = "DisplayName";
                    //if (currentResponseGroup != null)
                    //{
                    //    lbxReport.Items.Clear();
                    //    foreach (FrameworkUAD.Entity.ReportGroups rg in allReportGroups.Where(x => x.ResponseTypeID == currentResponseGroup.ResponseGroupID))
                    //    {
                    //        lbxReport.Items.Add(rg.DisplayName);
                    //    }
                    //}
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                busy.IsBusy = false;

                cbMagazine.SelectedIndex = 0;
                cbGroup.SelectedIndex = 0;
            };
            bw.RunWorkerAsync();
        }        

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int irep = 1;
            foreach (FrameworkUAD.Entity.ReportGroups rg in lbxReport.Items)
            {
                rg.DisplayOrder = irep;
                svSave = rgWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, rg);
                if (svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    irep++;
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
                    return;
                }
            }

            //int ires = 1;
            //foreach (string response in lbxResponse.Items)
            //{
            //    FrameworkUAD.Entity.CodeSheet r = allResponse.FirstOrDefault(x => x.ResponseValue.Equals(response, StringComparison.CurrentCultureIgnoreCase));
            //    r.DisplayOrder = ires;
            //    svSave = rWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, r, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                
            //    if (svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //    {
            //        ires++;
            //    }
            //    else
            //    {
            //        Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
            //        return;
            //    }
            //}
            Core_AMS.Utilities.WPF.MessageSaveComplete();
        }
        private void btnUpReport_Click(object sender, RoutedEventArgs e)
        {
            if (lbxReport.SelectedItems.Count > 0)
            {
                for (int i = 0; i < currentReports.Count; i++)
                {
                    if (lbxReport.SelectedItems.Contains(currentReports[i]))
                    {
                        if (i > 0 && !lbxReport.SelectedItems.Contains(currentReports[i - 1]))
                        {
                            var item = currentReports[i];
                            currentReports.Remove(item);
                            currentReports.Insert(i - 1, item);
                            lbxReport.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }
        private void btnDownReport_Click(object sender, RoutedEventArgs e)
        {
            if (lbxReport.SelectedItems.Count > 0)
            {
                int startindex = currentReports.Count - 1;

                for (int i = startindex; i > -1; i--)
                {
                    if (lbxReport.SelectedItems.Contains(currentReports[i]))
                    {
                        if (i < startindex && !lbxReport.SelectedItems.Contains(currentReports[i + 1]))
                        {
                            var item = currentReports[i];
                            currentReports.Remove(item);
                            currentReports.Insert(i + 1, item);
                            lbxReport.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }
        private void btnUpResponse_Click(object sender, RoutedEventArgs e)
        {
            if (lbxResponse.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbxResponse.Items.Count; i++)
                {
                    if (lbxResponse.SelectedItems.Contains(lbxResponse.Items[i]))
                    {
                        if (i > 0 && !lbxResponse.SelectedItems.Contains(lbxResponse.Items[i - 1]))
                        {
                            var item = lbxResponse.Items[i];
                            lbxResponse.Items.Remove(item);
                            lbxResponse.Items.Insert(i - 1, item);
                            lbxResponse.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }
        private void btnDownResponse_Click(object sender, RoutedEventArgs e)
        {
            if (lbxResponse.SelectedItems.Count > 0)
            {
                int startindex = lbxResponse.Items.Count - 1;

                for (int i = startindex; i > -1; i--)
                {
                    if (lbxResponse.SelectedItems.Contains(lbxResponse.Items[i]))
                    {
                        if (i < startindex && !lbxResponse.SelectedItems.Contains(lbxResponse.Items[i + 1]))
                        {
                            var item = lbxResponse.Items[i];
                            lbxResponse.Items.Remove(item);
                            lbxResponse.Items.Insert(i + 1, item);
                            lbxResponse.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }
        private void cbMagazine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMagazine.SelectedValue != null)
            {
                //lbxReport.Items.Clear();
                currentReports.Clear();
                lbxResponse.Items.Clear();
                int PID = 0;
                int.TryParse(cbMagazine.SelectedValue.ToString(), out PID);
                currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PID);
                cbGroup.ItemsSource = null;
                cbGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == PID).ToList();
                cbGroup.SelectedValuePath = "ResponseGroupID";
                cbGroup.DisplayMemberPath = "ResponseGroupName";
            }
        }
        private void cbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbGroup.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(cbGroup.SelectedValue.ToString(), out PID);
                currentResponseGroup = allResponseGroup.FirstOrDefault(x => x.PubID == currentPublication.ProductID && x.ResponseGroupID == PID);

                //foreach (FrameworkUAD.Entity.ReportGroups rg in allReportGroups.Where(x => x.ResponseGroupID == currentResponseGroup.ResponseGroupID))
                //{
                //    lbxReport.Items.Add(rg.DisplayName);
                //}       
                currentReports.Clear();
                foreach (FrameworkUAD.Entity.ReportGroups rg in allReportGroups.Where(x => x.ResponseGroupID == currentResponseGroup.ResponseGroupID).OrderBy(x=> x.DisplayOrder))
                {
                    currentReports.Add(rg);
                }    
   
                lbxResponse.Items.Clear();
                //foreach (FrameworkUAD.Entity.CodeSheet rg in allResponse.Where(x => x.PubID == currentPublication.ProductID && x.ResponseGroupID == currentResponseGroup.ResponseGroupID))
                //{
                //    lbxResponse.Items.Add(rg.ResponseDesc);
                //}
            }
        }
        private void lbxReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            foreach (FrameworkUAD.Entity.ReportGroups rg in e.AddedItems)
            {
                List<FrameworkUAD.Entity.CodeSheet> responses = allResponse.Where(x => x.ReportGroupID == rg.ReportGroupID).OrderBy(x=> x.DisplayOrder).ToList();
                foreach (FrameworkUAD.Entity.CodeSheet cs in responses)
                {
                    lbxResponse.Items.Add(cs.ResponseDesc);
                }
            }
            foreach (FrameworkUAD.Entity.ReportGroups rg in e.RemovedItems)
            {
                List<FrameworkUAD.Entity.CodeSheet> responses = allResponse.Where(x => x.ReportGroupID == rg.ReportGroupID).OrderBy(x=> x.DisplayOrder).ToList();
                foreach (FrameworkUAD.Entity.CodeSheet cs in responses)
                {
                    lbxResponse.Items.Remove(cs.ResponseDesc);
                }
            }
        }                                            
    }
}
