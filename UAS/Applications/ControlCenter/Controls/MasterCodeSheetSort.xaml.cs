using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for MasterCodeSheetSort.xaml
    /// </summary>
    public partial class MasterCodeSheetSort : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterCodeSheet> mcsWorker = FrameworkServices.ServiceClient.UAD_MasterCodeSheetClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterGroup> mgWorker = FrameworkServices.ServiceClient.UAD_MasterGroupClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>> svMasterCodeSheet = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>> svMasterGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>>();
        FrameworkUAS.Service.Response<int> svIntMCS = new FrameworkUAS.Service.Response<int>();

        List<FrameworkUAD.Entity.MasterCodeSheet> masterCodeSheets = new List<FrameworkUAD.Entity.MasterCodeSheet>();
        List<FrameworkUAD.Entity.MasterGroup> masterGroups = new List<FrameworkUAD.Entity.MasterGroup>();
        #endregion
        public MasterCodeSheetSort()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();            
        }

        #region Load Methods
        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svMasterGroup = mgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svMasterCodeSheet = mcsWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svMasterGroup.Result != null && svMasterGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    masterGroups = svMasterGroup.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svMasterCodeSheet.Result != null && svMasterCodeSheet.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)            
                    masterCodeSheets = svMasterCodeSheet.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadMasterGroups();
                LoadMasterCodeSheets();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        public void LoadMasterGroups()
        {
            cbMasterGroup.ItemsSource = null;                        
            cbMasterGroup.ItemsSource = masterGroups.OrderBy(x => x.SortOrder);
            cbMasterGroup.DisplayMemberPath = "DisplayName";
            cbMasterGroup.SelectedValuePath = "MasterGroupID";            
        }
        public void LoadMasterCodeSheets()
        {                                    
            lbxAvailable.Items.Clear();
            if (cbMasterGroup.SelectedValue != null)
            {
                int mg = 0;
                int.TryParse(cbMasterGroup.SelectedValue.ToString(), out mg);
                foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in masterCodeSheets.Where(x => x.MasterGroupID == mg).OrderBy(x => x.SortOrder).ToList())
                {
                    lbxAvailable.Items.Add(mcs.MasterDesc);
                } 
            }            
        }
        #endregion

        private void cbMasterGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMasterGroup.SelectedValue != null)
            {
                lbxAvailable.Items.Clear();
                int mGrpID = 0;
                int.TryParse(cbMasterGroup.SelectedValue.ToString(), out mGrpID);
                if (mGrpID > 0)
                {
                    List<FrameworkUAD.Entity.MasterCodeSheet> distMasterCodeSheets = new List<FrameworkUAD.Entity.MasterCodeSheet>();
                    distMasterCodeSheets = masterCodeSheets.Where(x => x.MasterGroupID == mGrpID).OrderBy(x => x.SortOrder).ToList();
                    foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMasterCodeSheets)
                    {
                        lbxAvailable.Items.Add(mcs.MasterDesc);
                    }
                }                                
            }
        }

        #region Button Methods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            List<FrameworkUAD.Entity.MasterCodeSheet> failedSaves = new List<FrameworkUAD.Entity.MasterCodeSheet>();
            foreach (string item in lbxAvailable.Items)
            {
                if (cbMasterGroup.SelectedValue != null)
                {
                    int mGrpID = 0;
                    int.TryParse(cbMasterGroup.SelectedValue.ToString(), out mGrpID);
                    FrameworkUAD.Entity.MasterCodeSheet thisMCS = masterCodeSheets.FirstOrDefault(x => x.MasterGroupID == mGrpID && x.MasterDesc.Equals(item, StringComparison.CurrentCultureIgnoreCase));
                    count++;
                    thisMCS.SortOrder = count;

                    svIntMCS = mcsWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisMCS, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (svIntMCS.Result != null && svIntMCS.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        continue;
                    }
                    else
                    {
                        failedSaves.Add(thisMCS);
                    }
                }
            }
            if (failedSaves.Count > 0)
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                return;
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageSaveComplete();
                LoadData();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {                        
            //LoadMasterGroups();
            //LoadMasterCodeSheets();
            lbxAvailable.Items.Clear();
            int mg = 0;
            int.TryParse(cbMasterGroup.SelectedValue.ToString(), out mg);
            foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in masterCodeSheets.Where(x => x.MasterGroupID == mg).OrderBy(x => x.SortOrder).ToList())
            {
                lbxAvailable.Items.Add(mcs.MasterDesc);
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAvailable.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbxAvailable.Items.Count; i++)
                {
                    if (lbxAvailable.SelectedItems.Contains(lbxAvailable.Items[i]))
                    {
                        if (i > 0 && !lbxAvailable.SelectedItems.Contains(lbxAvailable.Items[i - 1]))
                        {
                            var item = lbxAvailable.Items[i];
                            lbxAvailable.Items.Remove(item);
                            lbxAvailable.Items.Insert(i - 1, item);
                            lbxAvailable.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAvailable.SelectedItems.Count > 0)
            {
                int startindex = lbxAvailable.Items.Count - 1;

                for (int i = startindex; i > -1; i--)
                {
                    if (lbxAvailable.SelectedItems.Contains(lbxAvailable.Items[i]))
                    {
                        if (i < startindex && !lbxAvailable.SelectedItems.Contains(lbxAvailable.Items[i + 1]))
                        {
                            var item = lbxAvailable.Items[i];
                            lbxAvailable.Items.Remove(item);
                            lbxAvailable.Items.Insert(i + 1, item);
                            lbxAvailable.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
