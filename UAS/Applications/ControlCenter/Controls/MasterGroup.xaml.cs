using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for MasterGroup.xaml
    /// </summary>
    public partial class MasterGroup : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterGroup> mgWorker = FrameworkServices.ServiceClient.UAD_MasterGroupClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>> svMasterGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>>();
        FrameworkUAS.Service.Response<int> svIntMCS = new FrameworkUAS.Service.Response<int>();
        
        List<FrameworkUAD.Entity.MasterGroup> masterGroups = new List<FrameworkUAD.Entity.MasterGroup>();
        FrameworkUAD.Entity.MasterGroup currentMasterGroup = new FrameworkUAD.Entity.MasterGroup();
        #endregion

        public MasterGroup()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();            
            LoadCombos();
        }

        #region Load Methods
        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svMasterGroup = mgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svMasterGroup.Result != null && svMasterGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    masterGroups = svMasterGroup.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadMasterGroups();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        public void LoadMasterGroups()
        {
            gridDimensions.ItemsSource = null;                        
            gridDimensions.ItemsSource = masterGroups.OrderBy(x => x.DisplayName);                   
        }
        public void LoadCombos()
        {
            cbActive.Items.Clear();
            cbAdhocSearch.Items.Clear();
            cbSearch.Items.Clear();
            cbSubReport.Items.Clear();
            foreach (Core_AMS.Utilities.Enums.YesNo tf in (Core_AMS.Utilities.Enums.YesNo[])Enum.GetValues(typeof(Core_AMS.Utilities.Enums.YesNo)))
            {
                cbActive.Items.Add(tf.ToString().Replace("_", " "));
                cbAdhocSearch.Items.Add(tf.ToString().Replace("_", " "));
                cbSearch.Items.Add(tf.ToString().Replace("_", " "));
                cbSubReport.Items.Add(tf.ToString().Replace("_", " "));
            }            
        }
        #endregion

        #region Button Methods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string Name = "";
            string DisplayName = "";
            string Desc = "";
            bool IsActive = false;
            bool SubReport = false;
            bool Search = false;
            bool AdhocSearch = false;
            int MasterGrpID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out MasterGrpID);

            #region Check Empty Data
            if (string.IsNullOrEmpty(tbxName.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                Name = tbxName.Text;

            if (string.IsNullOrEmpty(tbxDesc.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Description.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                Desc = tbxDesc.Text;

            if (cbActive.SelectedItem != null)
            {
                if (cbActive.SelectedItem.ToString() == Core_AMS.Utilities.Enums.YesNo.Yes.ToString())
                    IsActive = true;
                else
                    IsActive = false;

            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select active value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }

            if (cbSubReport.SelectedItem != null)
            {
                if (cbSubReport.SelectedItem.ToString() == Core_AMS.Utilities.Enums.YesNo.Yes.ToString())
                    SubReport = true;
                else
                    SubReport = false;

            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select sub reporting value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }

            if (cbSearch.SelectedItem != null)
            {
                if (cbSearch.SelectedItem.ToString() == Core_AMS.Utilities.Enums.YesNo.Yes.ToString())
                    Search = true;
                else
                    Search = false;

            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select search value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }

            if (cbAdhocSearch.SelectedItem != null)
            {
                if (cbAdhocSearch.SelectedItem.ToString() == Core_AMS.Utilities.Enums.YesNo.Yes.ToString())
                    AdhocSearch = true;
                else
                    AdhocSearch = false;

            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select adhoc search value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            #endregion

            #region Check value doesn't exist
            if (currentMasterGroup != null)
            {
                if (currentMasterGroup.Name != Name)
                {
                    if (masterGroups.FirstOrDefault(x => x.MasterGroupID == MasterGrpID && x.Name == Name) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                        return;
                    }
                }
            }
            else
            {
                if (masterGroups.FirstOrDefault(x => x.MasterGroupID == MasterGrpID && x.Name == Name) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                    return;
                }
            }
            #endregion

            #region Save|Update
            FrameworkUAD.Entity.MasterGroup mgEntry = new FrameworkUAD.Entity.MasterGroup();
            mgEntry.MasterGroupID = MasterGrpID;
            mgEntry.Name = Name;
            mgEntry.DisplayName = Name;
            mgEntry.Description = Desc;
            mgEntry.IsActive = IsActive;
            mgEntry.EnableSubReporting = SubReport;
            mgEntry.EnableSearching = Search;
            mgEntry.EnableAdhocSearch = AdhocSearch;
            mgEntry.SortOrder = masterGroups.Count + 1;
            svIntMCS = mgWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, mgEntry, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svIntMCS.Result != null && svIntMCS.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                Core_AMS.Utilities.WPF.MessageSaveComplete();
                currentMasterGroup = null;
                tbxName.Text = "";
                tbxDesc.Text = "";
                cbActive.SelectedIndex = -1;
                cbAdhocSearch.SelectedIndex = -1;
                cbSearch.SelectedIndex = -1;
                cbSubReport.SelectedIndex = -1;

                btnSave.Tag = "";
                btnSave.Content = "Save";
                LoadData();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                return;
            }
            #endregion
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            currentMasterGroup = null;
            tbxName.Text = "";
            tbxDesc.Text = "";
            cbActive.SelectedIndex = -1;
            cbAdhocSearch.SelectedIndex = -1;
            cbSearch.SelectedIndex = -1;
            cbSubReport.SelectedIndex = -1;

            btnSave.Tag = "";
            btnSave.Content = "Save";            
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.MasterGroup))
                {
                    FrameworkUAD.Entity.MasterGroup mgItem = (FrameworkUAD.Entity.MasterGroup)b.DataContext;
                    if (mgItem != null)
                    {
                        currentMasterGroup = mgItem;
                        tbxName.Text = currentMasterGroup.Name;
                        tbxDesc.Text = currentMasterGroup.Description;

                        if (currentMasterGroup.IsActive.ToString() == Core_AMS.Utilities.Enums.TrueFalse.True.ToString())
                            cbActive.SelectedItem = Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
                        else
                            cbActive.SelectedItem = Core_AMS.Utilities.Enums.YesNo.No.ToString();

                        if (currentMasterGroup.EnableSubReporting.ToString() == Core_AMS.Utilities.Enums.TrueFalse.True.ToString())
                            cbSubReport.SelectedItem = Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
                        else
                            cbSubReport.SelectedItem = Core_AMS.Utilities.Enums.YesNo.No.ToString();

                        if (currentMasterGroup.EnableSearching.ToString() == Core_AMS.Utilities.Enums.TrueFalse.True.ToString())
                            cbSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
                        else
                            cbSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.No.ToString();

                        if (currentMasterGroup.EnableAdhocSearch.ToString() == Core_AMS.Utilities.Enums.TrueFalse.True.ToString())
                            cbAdhocSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
                        else
                            cbAdhocSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.No.ToString();

                        btnSave.Tag = currentMasterGroup.MasterGroupID.ToString();
                        btnSave.Content = "Update";
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to delete this Master Code Sheet? It will delete all mapping for the Master Code Sheet.", "Warning", MessageBoxButton.YesNo);

            if (areYouSureDelete == MessageBoxResult.Yes)
            {
                if (e.OriginalSource.GetType() == typeof(Button))
                {
                    Button b = (Button)e.OriginalSource;
                    if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.MasterGroup))
                    {
                        FrameworkUAD.Entity.MasterGroup mgItem = (FrameworkUAD.Entity.MasterGroup)b.DataContext;
                        if (mgItem != null)
                        {
                            int MasterGroupID = mgItem.MasterGroupID;
                            #region Delete SubscriptionDetails
                            FrameworkUAS.Service.Response<bool> svBoolMG = new FrameworkUAS.Service.Response<bool>();
                            svBoolMG = mgWorker.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, MasterGroupID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if ((svBoolMG.Result == true || svBoolMG.Result == false) && svBoolMG.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            {
                                LoadData();                                
                                Core_AMS.Utilities.WPF.MessageDeleteComplete();                                
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.Message("Failed to delete. If this problem consists please contact Customer Service.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                return;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                        return;
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                    return;
                }
            }
        }

        private void btnResponses_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.MasterGroup))
                {
                    FrameworkUAD.Entity.MasterGroup mgItem = (FrameworkUAD.Entity.MasterGroup)b.DataContext;
                    if (mgItem != null)
                    {
                        //OPEN MasterCodeSheet pass MasterGroupID
                        StackPanel foundStackPanel = Core_AMS.Utilities.WPF.FindControl<StackPanel>(Application.Current.MainWindow, "spModule");
                        if (foundStackPanel != null)
                        {
                            StackPanel foundStackPanel2 = Core_AMS.Utilities.WPF.FindControl<StackPanel>(foundStackPanel, "spControls");
                            if (foundStackPanel2 != null)
                            {
                                foundStackPanel2.Children.Clear();
                                ControlCenter.Controls.MasterCodeSheet mcs = new MasterCodeSheet(mgItem.MasterGroupID);
                                foundStackPanel2.Children.Add(mcs);
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
        #endregion
    }
}
