using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for FileStatus.xaml
    /// </summary>
    public partial class FileStatus : UserControl
    {
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> blc = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> blsf = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IServiceFeature> sf = FrameworkServices.ServiceClient.UAS_ServiceFeatureClient();
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IFileStatus> fsData = FrameworkServices.ServiceClient.UAS_FileStatusClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> fsTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();//FileStatus
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFileLog> logData = FrameworkServices.ServiceClient.UAS_FileLogClient();

        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        private List<KMPlatform.Entity.Client> AllClients = new List<KMPlatform.Entity.Client>();
        private FrameworkUAS.Entity.SourceFile me = new FrameworkUAS.Entity.SourceFile();
        //private FrameworkUAS.Entity.FileStatus myStatus = new FrameworkUAS.Entity.FileStatus();
        private FrameworkUAD_Lookup.Entity.Code myfsType = new FrameworkUAD_Lookup.Entity.Code();//FileStatusType
        private List<FrameworkUAS.Entity.FileLog> myLog = new List<FrameworkUAS.Entity.FileLog>();

        public FileStatus(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();

            myAppData = appData;
            LoadData(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient);
        }

        public void LoadData(KMPlatform.Entity.Client SelectedClient)
        {
            if (SelectedClient.ClientID == 1)
            {
                AllClients = blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.OrderBy(x => x.ClientName).ToList();
                rcbClients.ItemsSource = AllClients.Where(x => x.IsActive);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
            }
            else
            {
                AllClients = blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.OrderBy(x => x.ClientName).ToList();
                rcbClients.ItemsSource = AllClients.Where(x => x.IsActive && x.ClientID == SelectedClient.ClientID);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
                KMPlatform.Entity.Client sc = AllClients.SingleOrDefault(x => x.ClientID == SelectedClient.ClientID);
                if (sc != null)
                    rcbClients.SelectedItem = sc;

            }

            Style s = this.FindResource("roundedComboBoxes") as Style;
            rcbClients.Style = s;
        }

        private void rcbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            int clientID = 0;
            if (rcb.SelectedValue != null)
                int.TryParse(rcb.SelectedValue.ToString(), out clientID);

            if (clientID > 0)
            {
                myClient = blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientID).Result;
                List<FrameworkUAS.Entity.SourceFile> files = blsf.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientID, false).Result.Where(x => x.IsDeleted == false).ToList();
                rlbFiles.ItemsSource = files;
                rlbFiles.DisplayMemberPath = "FileName";
                rlbFiles.SelectedValuePath = "SourceFileID";
                rlbFiles.Visibility = Visibility.Visible;
                txtSelectFile.Visibility = Visibility.Visible;
                Hide_Options();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
        }
        private void rlbFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int sfID = 0;
            if (rlbFiles.SelectedValue != null)
            {
                int.TryParse(rlbFiles.SelectedValue.ToString(), out sfID);
                me = blsf.Proxy.SelectForSourceFile(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sfID, false).Result;
                txtFileNameBig.Text = me.FileName;
                txtDateCreatedBig.Text = me.DateCreated.ToString();
                txtDateUpdated.Text = me.DateUpdated.ToString();
                txtExtension.Text = me.Extension;
                if (me.Extension.ToLower() == ".csv" || me.Extension.ToLower() == ".txt")
                {
                    txtDelimiter.Text = me.Delimiter;
                    spDelimiter.Visibility = Visibility.Visible;
                }

                //myStatus = fsData.Proxy.SelectForFile(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sfID).Result.LastOrDefault();
                //if(myStatus != null)
                //{
                //    myfsType = fsTypeData.Proxy.SelectCodeId(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myStatus.FileStatusTypeID).Result;
                //    txtStatus.Text = myfsType.CodeName.ToString();//FileStatusName
                //    txtFileRun.Text = "Yes";
                //    txtFileRun.Foreground = System.Windows.Media.Brushes.Green;
                //}
                //else
                //{
                //    txtFileRun.Text = "No";
                //    txtFileRun.Foreground = System.Windows.Media.Brushes.Red;
                //    txtStatus.Text = "N/A";
                //}
                myLog = logData.Proxy.SelectClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID).Result.Where(x=> x.SourceFileID == sfID).ToList();
                if(myLog.Count > 0)
                {
                    //txtLog.Inlines.Add("\n \n");
                    foreach(FrameworkUAS.Entity.FileLog fl in myLog)
                    {
                        txtLog.Inlines.Add(fl.LogDate + ": " + fl.Message);
                    }
                }

                spFileInfoBig.Visibility = Visibility.Visible;
                spFileRun.Visibility = Visibility.Visible;
                spStatus.Visibility = Visibility.Visible;
                svLog.Visibility = Visibility.Visible;
                rlbFiles.Visibility = Visibility.Hidden;
                txtSelectFile.Visibility = Visibility.Hidden;
                spFileInfo.Visibility = Visibility.Collapsed;
                btnSelectDifferentFile.Visibility = Visibility.Visible;
            }
        }
        private void Select_Different_File(object sender, RoutedEventArgs e)
        {
            Hide_Options();
        }
        private void Hide_Options()
        {
            rlbFiles.Visibility = Visibility.Visible;
            txtSelectFile.Visibility = Visibility.Visible;
            btnSelectDifferentFile.Visibility = Visibility.Hidden;
            spFileInfo.Visibility = Visibility.Hidden;
            spDelimiter.Visibility = Visibility.Hidden;
            spFileInfoBig.Visibility = Visibility.Hidden;
            spFileRun.Visibility = Visibility.Hidden;
            spStatus.Visibility = Visibility.Hidden;
            svLog.Visibility = Visibility.Hidden;
            txtFileRun.Text = "";
            txtLog.Inlines.Clear();
            txtStatus.Text = "";
        }

        private void rlbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sfID = 0;
            if (rlbFiles.SelectedValue != null)
            {
                spFileInfo.Visibility = Visibility.Visible;
                int.TryParse(rlbFiles.SelectedValue.ToString(), out sfID);
                me = blsf.Proxy.SelectForSourceFile(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sfID, false).Result;
                txtFileName.Text = me.FileName;
                txtDateCreated.Text = me.DateCreated.ToString();
            }
        }
    }
}
