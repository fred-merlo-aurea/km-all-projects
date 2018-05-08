using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for FileLogViewer.xaml
    /// </summary>
    public partial class FileLogViewer : UserControl
    {
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sourceFileWorker = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFileLog> fileLogWorker = FrameworkServices.ServiceClient.UAS_FileLogClient();

        private FrameworkUAS.Object.AppData myAppData = new FrameworkUAS.Object.AppData();
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();        
        private List<FrameworkUAS.Entity.SourceFile> sourceList = new List<FrameworkUAS.Entity.SourceFile>();
        private List<FrameworkUAS.Entity.FileLog> fileLogList = new List<FrameworkUAS.Entity.FileLog>();
        private List<FrameworkUAS.Object.FileLog> processCodeToSourceFileList = new List<FrameworkUAS.Object.FileLog>();
        private int oldSourceFileID;
        private string oldProcessCode;

        public FileLogViewer(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            myAppData = appData;
            myClient = myAppData.AuthorizedUser.User.CurrentClient;
            oldSourceFileID = -1;
            oldProcessCode = "";
            LoadData();
        }

        #region Local Method
        private void LoadData()
        {
            BackgroundWorker bw = new BackgroundWorker();
            busyIcon.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                sourceList = sourceFileWorker.Proxy.Select(myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.ToList();
                processCodeToSourceFileList = fileLogWorker.Proxy.SelectDistinctProcessCodePerSourceFile(myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID).Result.ToList();                
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Set ItemsSource
                List<FrameworkUAS.Entity.SourceFile> admsRanSources = new List<FrameworkUAS.Entity.SourceFile>();
                admsRanSources = sourceList.Where(x => processCodeToSourceFileList.Select(y => y.SourceFileID).Contains(x.SourceFileID)).ToList();
                cbFiles.ItemsSource = admsRanSources.OrderBy(x => x.FileName);
                cbFiles.SelectedValuePath = "SourceFileID";
                cbFiles.DisplayMemberPath = "FileName";
                #endregion                
                busyIcon.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        #endregion

        #region Combobox
        private void cbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFiles.SelectedValue != null)
            {
                int sfID = 0;
                int.TryParse(cbFiles.SelectedValue.ToString(), out sfID);
                #region
                cbProcessCode.ItemsSource = processCodeToSourceFileList.Where(x => x.SourceFileID == sfID).ToList();
                cbProcessCode.SelectedValuePath = "ProcessCode";
                cbProcessCode.DisplayMemberPath = "ProcessCode";
                #endregion
            }
        }
        #endregion

        #region Button
        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            #region Set and Error
            int SourceFileID = -1;
            if (cbFiles.SelectedValue != null)
                int.TryParse(cbFiles.SelectedValue.ToString(), out SourceFileID);

            string ProcessCode = "";
            if (cbProcessCode.SelectedValue != null)
                ProcessCode = cbProcessCode.SelectedValue.ToString();

            if (!(SourceFileID > -1) && string.IsNullOrEmpty(ProcessCode))
            {
                Core_AMS.Utilities.WPF.Message("Insufficent display criteria. Select a file and/or process code to continue.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Required Data");
                return;
            }
            #endregion

            BackgroundWorker bw = new BackgroundWorker();
            busyIcon.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                fileLogList = fileLogWorker.Proxy.SelectFileLog(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, SourceFileID, ProcessCode).Result;                
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Set ItemsSource
                grdFileLog.ItemsSource = fileLogList;
                #endregion                
                busyIcon.IsBusy = false;
            };
            bw.RunWorkerAsync();           
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbFiles.SelectedIndex = -1;
            cbProcessCode.SelectedIndex = -1;
            oldSourceFileID = -1;
            oldProcessCode = "";
            grdFileLog.ItemsSource = null;
        }
        #endregion
    }
}
