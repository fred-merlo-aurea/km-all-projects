using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrameworkUAS.Object;

namespace DQM.Modules
{
    /// <summary>
    /// Interaction logic for ADMS_FTP_FileUpload.xaml
    /// </summary>
    public partial class ADMS_FTP_FileUpload : UserControl
    {
        public ADMS_FTP_FileUpload()
        {
            InitializeComponent();
            LoadClients();
            btnFile.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }

        private void LoadClients()
        {
            cbClient.ItemsSource = AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients;//clients;
            cbClient.SelectedValuePath = "ClientID";
            cbClient.DisplayMemberPath = "DisplayName";
        }

        #region Button
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                tbFile.Text = filename;
                btnUpload.IsEnabled = true;
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (cbClient.SelectedValue != null)
            {
                if (!string.IsNullOrEmpty(tbFile.Text))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(tbFile.Text.Trim());
                    //upload to FTP
                    KMPlatform.Entity.Client client = (KMPlatform.Entity.Client)cbClient.SelectedItem;
                    FrameworkUAS.Entity.ClientFTP cFTP = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].ClientFtpDirectoriesList.FirstOrDefault();// cftpData.SelectClient(client.ClientID).FirstOrDefault();
                    if (cFTP != null)
                    {
                        string host = "";
                        if (useSuppression.IsChecked.Value.Equals(true))
                        {
                            host = cFTP.Server + "/ADMS/Suppression/";
                        }
                        else
                        {
                            host = cFTP.Server + "/ADMS/";
                        }

                        Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                        bool uploadSuccess = false;
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += (o, ea) =>
                        {
                            uploadSuccess = ftp.Upload(file.Name, file.FullName);
                        };

                        worker.RunWorkerCompleted += (o, ea) =>
                        {
                            if (uploadSuccess == true)
                            {
                                Core_AMS.Utilities.WPF.MessageFileUploadComplete();
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.MessageFileUploadError();
                            }
                            busyIcon.IsBusy = false;
                        };

                        busyIcon.IsBusy = true;
                        worker.RunWorkerAsync();

                    }
                    else
                        Core_AMS.Utilities.WPF.MessageError("FTP site is not configured for the selected client.  Please contact customer support.");
                }
                else
                    Core_AMS.Utilities.WPF.MessageError("You must select a file.");
            }
            else
                Core_AMS.Utilities.WPF.MessageError("You must select a client.");
        }
        #endregion
        #region Combobox
        private void cbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbClient.SelectedValue != null)
            {
                btnFile.IsEnabled = true;
            }
        }
        #endregion
    }
}
