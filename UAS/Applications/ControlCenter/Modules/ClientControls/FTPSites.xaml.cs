using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.IO;
using FrameworkUAS.Object;

namespace ControlCenter.Modules.ClientControls
{
    /// <summary>
    /// Interaction logic for FTPSites.xaml
    /// </summary>
    public partial class FTPSites : UserControl
    {
        private KMPlatform.Entity.Client currentClient { get; set; }
        private FrameworkUAS.Entity.ClientFTP clientFTP { get; set; }
        private List<FrameworkUAS.Entity.ClientFTP> clientFTPList { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClientFTP> clientWorker { get; set; }

        private bool insertCheck = false;
        private bool cancelCheck = false;

        public FTPSites(KMPlatform.Entity.Client client)
        {
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    LoadData(client);           
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
            }
        }        

        private void LoadData(KMPlatform.Entity.Client client)
        {
            currentClient = client;
            clientWorker = FrameworkServices.ServiceClient.UAS_ClientFTPClient();
            clientFTP = new FrameworkUAS.Entity.ClientFTP();            
            clientFTPList = clientWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentClient.ClientID).Result;
            
            grdFTP.ItemsSource = null;
            grdFTP.ItemsSource = clientFTPList;
            grdFTP.Rebind();
        }

        #region GridWork
        public void grdFTP_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grdFTP.SelectedItem != null)
            {
                grdFTP.ScrollIntoView(this.grdFTP.SelectedItem);
                grdFTP.UpdateLayout();
                grdFTP.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }      
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Cancel)
                grdFTP.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            else
            {
                //EditAction == Commit
                //save changes
                FrameworkUAS.Entity.ClientFTP myCFTP = (FrameworkUAS.Entity.ClientFTP)grdFTP.SelectedItem;
                #region Check Values
                if (string.IsNullOrEmpty(myCFTP.Server) || string.IsNullOrEmpty(myCFTP.UserName) || string.IsNullOrEmpty(myCFTP.Password) || string.IsNullOrEmpty(myCFTP.Folder))
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a server, username, password, and folder.");
                    grdFTP.Rebind();
                    return;
                }
                #endregion                
                myCFTP.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                clientWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myCFTP);

                grdFTP.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            this.grdFTP.SelectedItem = null;
        }
        private void rdForm_Loaded(object sender, RoutedEventArgs e)
        {
            //RadDataForm rdForm = sender as RadDataForm;

            //KMPlatform.Entity.Service myServ = (KMPlatform.Entity.Service)rdForm.DataContext;

            //DataFormComboBoxField cbEditApplications = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbEditApplications");

            //if (cbEditApplications != null)
            //{
            //    cbEditApplications.ItemsSource = applications;
            //    KMPlatform.Entity.Application myApp = applications.Single(x => x.ApplicationID == myServ.DefaultApplicationID);
            //    cbEditApplications.SelectedIndex = applications.IndexOf(myApp);
            //}
        }
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdFTP.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            this.grdFTP.SelectedItem = null;
        }
        private void rbNewFTP_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;
            rbNewFTP.IsEnabled = false;
            grdFTP.IsEnabled = false;
            grdFTP.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;            
        }
        #endregion

        #region ClientMethods
        private bool FTPCheck(string url, string user, string pass)
        {
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(url);
            FtpWebResponse res;
            StreamReader reader;

            ftp.Timeout = 60000;
            ftp.Credentials = new NetworkCredential(user, pass);
            ftp.Credentials = new NetworkCredential(user.Normalize(), pass.Normalize());
            ftp.KeepAlive = false;
            ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            try
            {
                using (res = (FtpWebResponse)ftp.GetResponse())
                {
                    reader = new StreamReader(res.GetResponseStream());
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            string server = tbServer.Text.Trim();
            string username = tbUserName.Text.Trim();
            string password = tbPassword.Text.Trim();
            string folder = tbFolder.Text.Trim();           
            bool isDeleted = false;
            if (cbxIsDeleted.IsChecked.HasValue)
                isDeleted = cbxIsDeleted.IsChecked.Value;

            bool isExternal = false;
            if (cbxIsExternal.IsChecked.HasValue)
                isExternal = cbxIsExternal.IsChecked.Value;

            bool isActive = false;
            if (cbxIsActive.IsChecked.HasValue)
                isActive = cbxIsActive.IsChecked.Value;

            bool FTPConnectionValidated = false;
            if (cbxFTPConnectionValidated.IsChecked.HasValue)
                FTPConnectionValidated = cbxFTPConnectionValidated.IsChecked.Value;

            DateTime todayDate = DateTime.Now;
            #region Check Values
            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(folder))
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a server, username, password, and folder.");
                return;
            }
            #endregion
            FrameworkUAS.Entity.ClientFTP newClientFTP = new FrameworkUAS.Entity.ClientFTP();            
            newClientFTP.ClientID = currentClient.ClientID;
            newClientFTP.Server = server;
            newClientFTP.UserName = username;
            newClientFTP.Password = password;
            newClientFTP.Folder = folder;
            newClientFTP.IsDeleted = isDeleted;
            newClientFTP.IsExternal = isExternal;
            newClientFTP.IsActive = isActive;
            newClientFTP.FTPConnectionValidated = FTPConnectionValidated;
            newClientFTP.DateCreated = todayDate;
            newClientFTP.DateUpdated = todayDate;
            newClientFTP.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newClientFTP.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newClientFTP.FTPID = clientWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newClientFTP).Result;
            
            if (newClientFTP.FTPID > 0)
            {
                ResetNewWindow();

                List<FrameworkUAS.Entity.ClientFTP> clientFTPs = (List<FrameworkUAS.Entity.ClientFTP>)grdFTP.ItemsSource;
                clientFTPs.Add(newClientFTP);
                grdFTP.ItemsSource = null;
                grdFTP.ItemsSource = clientFTPs;

                CloseWindow();
                this.grdFTP.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            List<KMPlatform.Entity.Service> services = (List<KMPlatform.Entity.Service>)grdFTP.ItemsSource;
            grdFTP.ItemsSource = services;
            CloseWindow();
            this.grdFTP.SelectedItem = null;
        }
        private void ResetNewWindow()
        {
            tbServer.Clear();
            tbUserName.Clear();
            tbPassword.Clear();
            tbFolder.Clear();
            cbxIsDeleted.IsChecked = false;
            cbxIsExternal.IsChecked = false;
            cbxIsActive.IsChecked = false;
            cbxFTPConnectionValidated.IsChecked = false;
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;            
            rbNewFTP.IsEnabled = true;
            grdFTP.IsEnabled = true;
            grdFTP.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
