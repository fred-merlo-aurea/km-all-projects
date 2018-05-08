using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls.CircCodesheet
{
    /// <summary>
    /// Interaction logic for GroupOrder.xaml
    /// </summary>
    public partial class GroupOrder : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();

        List<FrameworkUAD.Entity.ResponseGroup> allResponseGroup = new List<FrameworkUAD.Entity.ResponseGroup>();

        FrameworkUAD.Entity.ResponseGroup currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();

        int currentPublicationID = 0;
        #endregion
        public GroupOrder(int PublisherID = 0, int PublicationID = 0)
        {
            InitializeComponent();
            currentPublicationID = PublicationID;
            LoadData(PublisherID, PublicationID);
        }

        public void LoadData(int PublisherID = 0, int PublicationID = 0)
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svResponseGroup = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load ResponseGroups
                if (svResponseGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allResponseGroup = svResponseGroup.Result.Where(x => x.PubID == PublicationID).ToList();
                    
                    foreach (FrameworkUAD.Entity.ResponseGroup rt in allResponseGroup.OrderBy(x => x.DisplayOrder))
                    {
                        lbxAvailable.Items.Add(rt.DisplayName);
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
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            List<string> badSaves = new List<string>();
            int i = 1;
            foreach (string s in lbxAvailable.Items)
            {
                FrameworkUAD.Entity.ResponseGroup rt = allResponseGroup.FirstOrDefault(x => x.DisplayName.Equals(s, StringComparison.CurrentCultureIgnoreCase) && x.PubID == currentPublicationID);
                if (rt != null)
                {
                    rt.DisplayOrder = i;
                    rt.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    rt.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    rt.DateCreated = DateTime.Now;
                    rt.DateUpdated = DateTime.Now;

                    FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
                    svSave = rWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, rt);
                    if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        i++;
                    }
                    else
                    {
                        badSaves.Add(s);
                        i++;
                    }
                }
                else
                {
                    badSaves.Add(s);
                    i++;
                }
            }
            if (badSaves.Count > 0)
            {
                Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
                return;
            }
            else
                Core_AMS.Utilities.WPF.MessageSaveComplete();
        }        
    }
}
