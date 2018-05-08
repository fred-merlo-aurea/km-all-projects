using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for MasterGroupSort.xaml
    /// </summary>
    public partial class MasterGroupSort : UserControl
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

        public MasterGroupSort()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();            
        }

        #region Loads
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
            lbxAvailable.Items.Clear();                        
            foreach (FrameworkUAD.Entity.MasterGroup mg in masterGroups.OrderBy(x => x.SortOrder))
            {
                lbxAvailable.Items.Add(mg.DisplayName);
            }            
        }
        #endregion

        #region Button Methods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            List<FrameworkUAD.Entity.MasterGroup> failedSaves = new List<FrameworkUAD.Entity.MasterGroup>();
            int count = 0;
            foreach (string item in lbxAvailable.Items)
            {                
                FrameworkUAD.Entity.MasterGroup thisMCS = masterGroups.FirstOrDefault(x => x.DisplayName.Equals(item, StringComparison.CurrentCultureIgnoreCase));
                count++;

                if (thisMCS != null)
                {
                    thisMCS.SortOrder = count;

                    svIntMCS = mgWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisMCS, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
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
                Core_AMS.Utilities.WPF.Message("There was an error saving the data order. If this problem persists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
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
            LoadMasterGroups();
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
