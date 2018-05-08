using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls.CircCodesheet
{
    /// <summary>
    /// Interaction logic for Magazines.xaml
    /// </summary>
    public partial class Magazines : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IFrequency> fWorker = FrameworkServices.ServiceClient.UAD_FrequencyClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPub = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublication = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Frequency>> svFrequency = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Frequency>>();

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();
        List<FrameworkUAD.Entity.Product> allPublications = new List<FrameworkUAD.Entity.Product>();
        List<FrameworkUAD.Entity.Product> allCurrentPublisherPublications = new List<FrameworkUAD.Entity.Product>();
        List<FrameworkUAD.Entity.Frequency> allFrequency = new List<FrameworkUAD.Entity.Frequency>();

        KMPlatform.Entity.Client currentPublisher = new KMPlatform.Entity.Client();
        FrameworkUAD.Entity.Product currentPublication = new FrameworkUAD.Entity.Product();

        List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();

        #endregion
        public Magazines()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
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
                //svPublication = pubWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                svFrequency = fWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publishers
                //if (svPub.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                    //allPublishers = svPub.Result;  
                    allPublishers = new List<KMPlatform.Entity.Client>();
                    allPublishers.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients);
  
                    cbPublisher.ItemsSource = allPublishers.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
                    cbPublisher.SelectedValuePath = "ClientID";
                    cbPublisher.DisplayMemberPath = "DisplayName"; 
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null)                    
                        currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);                        
                    
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load Publications
                //if (svPublication.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                foreach (var x in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                {
                    foreach (var y in x.Products)
                    {
                        allPublications.Add(new FrameworkUAD.Entity.Product
                        {
                            PubID = y.ProductID,
                            PubCode = y.ProductCode,
                            PubName = y.ProductName,
                            ClientID = y.ClientID,
                            IsUAD = y.IsUAD,
                            IsCirc = y.IsCirc
                        });
                    }
                }

                //allPublications = svPublication.Result;
                if (currentPublisher != null)
                {
                    allCurrentPublisherPublications = allPublications.Where(x => x.ClientID == currentPublisher.ClientID).ToList();
                    gridMagazines.ItemsSource = allCurrentPublisherPublications;
                        
                    cbPublisher.SelectedItem = allPublishers.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);                    
                }
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load Frequency
                if (svFrequency.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allFrequency = svFrequency.Result;                    
                    cbFrequency.ItemsSource = allFrequency;
                    cbFrequency.SelectedValuePath = "FrequencyID";
                    cbFrequency.DisplayMemberPath = "FrequencyName";  
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

        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridMagazines.ItemsSource = null;
            if (cbPublisher.SelectedValue != null)
            {
                int PublisherID = 0;
                int.TryParse(cbPublisher.SelectedValue.ToString(), out PublisherID);
                allCurrentPublisherPublications = allPublications.Where(x => x.ClientID == PublisherID).ToList();                
                gridMagazines.ItemsSource = allCurrentPublisherPublications;
                currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == PublisherID);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.Product))
                {
                    FrameworkUAD.Entity.Product pItem = (FrameworkUAD.Entity.Product)b.DataContext;
                    if (pItem != null)
                    {
                        currentPublication = pItem;

                        tbxMagazineID.Text = pItem.PubID.ToString();
                        tbxMagazineName.Text = pItem.PubName;
                        tbxMagazineCode.Text = pItem.PubCode;
                        tbxYearStartDate.Text = pItem.YearStartDate;
                        tbxYearEndDate.Text = pItem.YearEndDate;
                        if (pItem.AllowDataEntry == true)
                            cbxAllowDataEntry.IsChecked = true;
                        else
                            cbxAllowDataEntry.IsChecked = false;

                        cbFrequency.SelectedValue = pItem.FrequencyID;

                        btnSave.Tag = pItem.PubID.ToString();
                        btnSave.Content = "Update";
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
            btnSave.Content = "Save";

            //Set currentItem to null
            currentPublication = null;

            //Clear control
            tbxMagazineID.Text = "";
            tbxMagazineName.Text = "";
            tbxMagazineCode.Text = "";
            tbxYearStartDate.Text = "";
            tbxYearEndDate.Text = "";
            cbxAllowDataEntry.IsChecked = false;
            cbFrequency.SelectedIndex = -1;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Initialize Data
            int PublicationID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out PublicationID);

            string Name = tbxMagazineName.Text;
            string Code = tbxMagazineCode.Text;
            string Start = tbxYearStartDate.Text;
            string End = tbxYearEndDate.Text;
            bool AllowDataEntry = false;
            if (cbxAllowDataEntry.IsChecked == true)
                AllowDataEntry = true;

            int FrequencyID = 0;
            if (cbFrequency.SelectedValue != null)
                int.TryParse(cbFrequency.SelectedValue.ToString(), out FrequencyID);

            #endregion
            #region Check Blank Data
            if (string.IsNullOrEmpty(Name))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            if (string.IsNullOrEmpty(Code))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a code.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            if (string.IsNullOrEmpty(Start))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a year start date.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            if (string.IsNullOrEmpty(End))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a year end date.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            if (!(FrequencyID > 0))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a frequency.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            if (currentPublisher == null || !(currentPublisher.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a publisher.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            #endregion
            #region Check Duplicate Submission
            if (PublicationID > 0)
            {
                //Check not value existence name and code based by client
                if (allPublications.FirstOrDefault(x => x.PubName.Equals(Name, StringComparison.CurrentCultureIgnoreCase) && x.PubID != PublicationID) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
                else if (allPublications.FirstOrDefault(x => x.PubCode.Equals(Code, StringComparison.CurrentCultureIgnoreCase) && x.PubID != PublicationID) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Code currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }
            else
            {
                //Check not value existence name and code
                if (allPublications.FirstOrDefault(x => x.PubName.Equals(Name, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
                else if (allPublications.FirstOrDefault(x => x.PubCode.Equals(Code, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Code currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }
            #endregion

            #region Save|Update
            if (PublicationID > 0)
            {
                currentPublication.PubID = PublicationID;
                currentPublication.PubName = Name;
                currentPublication.PubCode = Code;
                currentPublication.YearStartDate = Start;
                currentPublication.YearEndDate = End;
                currentPublication.AllowDataEntry = AllowDataEntry;
                currentPublication.FrequencyID = FrequencyID;
                currentPublication.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentPublication.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentPublication.DateCreated = DateTime.Now;
                currentPublication.DateUpdated = DateTime.Now;
            }
            else
            {
                currentPublication = new FrameworkUAD.Entity.Product();
                currentPublication.PubID = PublicationID;
                currentPublication.PubName = Name;
                currentPublication.PubCode = Code;
                currentPublication.ClientID = currentPublisher.ClientID;
                currentPublication.YearStartDate = Start;
                currentPublication.YearEndDate = End;
                currentPublication.IssueDate = new DateTime();
                currentPublication.IsImported = false;
                currentPublication.IsActive = true;
                currentPublication.AllowDataEntry = AllowDataEntry;
                currentPublication.FrequencyID = FrequencyID;
                currentPublication.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentPublication.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentPublication.DateCreated = DateTime.Now;
                currentPublication.DateUpdated = DateTime.Now;
            }

            FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svSave = pubWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentPublication, currentPublisher.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Refresh|Clear
                if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    //Set currentItem to null
                    currentPublication = null;

                    //Change Button tag to zero content back to save
                    btnSave.Tag = "0";
                    btnSave.Content = "Save";

                    //Clear control
                    tbxMagazineID.Text = "";
                    tbxMagazineName.Text = "";
                    tbxMagazineCode.Text = "";
                    tbxYearStartDate.Text = "";
                    tbxYearEndDate.Text = "";
                    cbxAllowDataEntry.IsChecked = false;
                    cbFrequency.SelectedIndex = -1;

                    //Refresh Grid
                    LoadData();

                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                }
                else
                    Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");

                #endregion

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
            #endregion
        }
        private void btnResponses_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.Product))
                {
                    FrameworkUAD.Entity.Product pItem = (FrameworkUAD.Entity.Product)b.DataContext;
                    if (pItem != null)
                    {
                        //OPEN MasterCodeSheet pass MasterGroupID
                        StackPanel foundStackPanel = Core_AMS.Utilities.WPF.FindControl<StackPanel>(Application.Current.MainWindow, "spModule");
                        if (foundStackPanel != null)
                        {
                            StackPanel foundStackPanel2 = Core_AMS.Utilities.WPF.FindControl<StackPanel>(foundStackPanel, "spControls");
                            if (foundStackPanel2 != null)
                            {
                                foundStackPanel2.Children.Clear();
                                ControlCenter.Controls.CircCodesheet.Response cs = new Response(pItem.PubID, pItem.ClientID);
                                foundStackPanel2.Children.Add(cs);
                            }
                        }
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.Product))
                {
                    FrameworkUAD.Entity.Product pItem = (FrameworkUAD.Entity.Product)b.DataContext;
                    if (pItem != null)
                    {
                        //OPEN MasterCodeSheet pass MasterGroupID
                        StackPanel foundStackPanel = Core_AMS.Utilities.WPF.FindControl<StackPanel>(Application.Current.MainWindow, "spModule");
                        if (foundStackPanel != null)
                        {
                            StackPanel foundStackPanel2 = Core_AMS.Utilities.WPF.FindControl<StackPanel>(foundStackPanel, "spControls");
                            if (foundStackPanel2 != null)
                            {
                                foundStackPanel2.Children.Clear();
                                ControlCenter.Controls.Reports cs = new Reports(pItem.PubID, pItem.ClientID);
                                foundStackPanel2.Children.Add(cs);
                            }
                        }
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }                            
    }
}
