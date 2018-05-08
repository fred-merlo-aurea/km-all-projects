using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using KM.Common;
using KM.Common.Import;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using ComputerInfo = Microsoft.VisualBasic.Devices.ComputerInfo;
using AMSFileWorker = Core_AMS.Utilities.FileWorker;
using AmsWpf = Core_AMS.Utilities.WPF;
using UasAppData = FrameworkUAS.Object.AppData;
using FtpFunctions = Core_AMS.Utilities.FtpFunctions;
using EntityClient = KMPlatform.Entity.Client;

namespace DQM.Modules
{
    /// <summary>
    /// Interaction logic for UAD_FileValidator.xaml
    /// </summary>
    public partial class UAD_FileValidator : UserControl
    {
        public List<FrameworkUAS.Entity.SourceFile> PossibleFiles { get; set; }

        public static readonly ulong BytesInKb = 1024;
        private const int MemoryThresholdInsuficient = 2;
        private const int MemoryThresholdLocalFiles = 4;
        private const int MemoryThresholdAnyFiles = 8;
        private const string MessageInsuficient = 
            "Your computer does not have enough RAM for local file validation.  " +
            "Off-Line validation is your only option.";
        private const string MessageMemoryLocalFiles = 
            "Your computer meets the bare minimum RAM requirements for local file validation of SMALL files only.  " +
            "If you file is over 5k records, has more than 2 transformations " +
            "or over 40 columns please use Off-Line Validation.";
        private const string MessageMemoryEnough = 
            "Your computer has enough RAM for local file validation for most files.  " +
            "If you file is over 50k records, has more than 4 transformations " +
            "or over 100 columns please use off-line validation.";
        private const string MessageFileValidationSupported = "File validation fully supported.";
        private const string AdmsValidatorPath = "/ADMS/FileValidator";
        private FrameworkUAS.Entity.SourceFile sfCheck;
        private KMPlatform.Entity.Client client;
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sfData = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        public UAD_FileValidator()
        {
            InitializeComponent();
            LoadClients();
            MemoryCheck();
        }
        private void MemoryCheck()
        {
            CheckMemory(new ComputerInfo().TotalPhysicalMemory, rbLocal, rbOffline, spProcess);
        }

        public static void CheckMemory(
            ulong totalPhysicalMemory,
            RadioButton rbLocal, 
            RadioButton rbOffline, 
            StackPanel spProcess)
        {
            Guard.NotNull(rbLocal, nameof(rbLocal));
            Guard.NotNull(rbOffline, nameof(rbOffline));
            Guard.NotNull(spProcess, nameof(spProcess));

            var sizeKb = totalPhysicalMemory;
            var sizeMb = sizeKb / BytesInKb;
            var sizeGb = sizeMb / BytesInKb;

            if (sizeGb <= MemoryThresholdInsuficient)
            {
                rbLocal.IsChecked = false;
                rbOffline.IsChecked = true;
                spProcess.IsEnabled = false;
                AmsWpf.Message(MessageInsuficient, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (sizeGb > MemoryThresholdInsuficient && sizeGb <= MemoryThresholdLocalFiles)
            {
                rbLocal.IsChecked = true;
                rbOffline.IsChecked = false;
                AmsWpf.Message(MessageMemoryLocalFiles, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (sizeGb > MemoryThresholdLocalFiles && sizeGb <= MemoryThresholdAnyFiles)
            {
                rbLocal.IsChecked = true;
                rbOffline.IsChecked = false;
                AmsWpf.Message(MessageMemoryEnough, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                rbLocal.IsChecked = true;
                rbOffline.IsChecked = false;
                AmsWpf.Message(MessageFileValidationSupported, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadClients()
        {
            KMPlatform.Entity.Client SelectedClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            if (SelectedClient.ClientID == 1)
            {                
                cbMyClient.ItemsSource = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients;
                cbMyClient.DisplayMemberPath = "DisplayName";
                cbMyClient.SelectedValuePath = "ClientID";
            }
            else
            {
                KMPlatform.Entity.Client sc = SelectedClient;
                if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.Exists(x => x.Clients.Exists(y => y.ClientID == SelectedClient.ClientID)) == true)
                {
                    if (sc == null)
                    {
                        foreach (var cg in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups)
                        {
                            if (sc != null)
                                break;

                            foreach (var c in cg.Clients)
                            {
                                if (c.ClientID == SelectedClient.ClientID)
                                {
                                    sc = c;
                                    break;
                                }
                            }
                        }
                    }
                }

                cbMyClient.Items.Add(SelectedClient);
                cbMyClient.DisplayMemberPath = "DisplayName";
                cbMyClient.SelectedValuePath = "ClientID";
                cbMyClient.SelectedItem = sc;
            }
        }
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
            }
        }

        private bool isFileUploaded(KMPlatform.Entity.Client client, FileInfo file)
        {
            bool canOpen;
            canOpen = false;

            try
            {
                using (File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    canOpen = true;
                }
            }
            catch
            {
                canOpen = false;
                Core_AMS.Utilities.WPF.MessageError(System.IO.Path.GetFileName(file.FullName) + " could not be opened. Is the file currently open? If so, please close and try again.");
            }

            return canOpen;
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            if (cbMyClient.SelectedIndex != null)
            {
                if (!string.IsNullOrEmpty(tbFile.Text))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(tbFile.Text);
                    KMPlatform.Entity.Client client = (KMPlatform.Entity.Client)cbMyClient.SelectedItem;
                    List<FrameworkUAS.Entity.SourceFile> sourceFilesList = new List<FrameworkUAS.Entity.SourceFile>();
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.ContainsKey(client.ClientID))
                        sourceFilesList = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].SourceFilesList.Where(x => x.IsDeleted == false).ToList();
                    else
                    {
                        sourceFilesList = sfData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, client.ClientID, false).Result.Where(x => x.IsDeleted == false).ToList();
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.Add(client.ClientID, new FrameworkUAS.Object.ClientAdditionalProperties());
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].SourceFilesList = sourceFilesList;
                    }

                    //check for Partial matching file name
                    if (sfCheck == null)
                    {
                        string incomingFile = fileInfo.Name.Replace(fileInfo.Extension, "").ToLower();
                        if (sourceFilesList.Exists(x => incomingFile.StartsWith(x.FileName.ToLower())))
                        {
                            if (sourceFilesList.Exists(x => incomingFile.Equals(x.FileName, StringComparison.CurrentCultureIgnoreCase)))
                                sfCheck = sourceFilesList.FirstOrDefault(x => incomingFile.Equals(x.FileName, StringComparison.CurrentCultureIgnoreCase));
                            else
                                sfCheck = sourceFilesList.FirstOrDefault(x => incomingFile.StartsWith(x.FileName.ToLower()));
                            DoValidation();
                        }
                        else if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => x.FileName.ToLower().StartsWith(incomingFile.ToLower()) && x.IsDeleted == false))
                        {
                            PossibleFiles = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].SourceFilesList.Where(x => x.FileName.ToLower().StartsWith(incomingFile) && x.IsDeleted == false).ToList();
                            //this.Background = System.Windows.Media.Brushes.DarkGray;
                            btnFile.IsEnabled = false;
                            cbMyClient.IsEnabled = false;
                            btnValidate.IsEnabled = false;
                            pwMultiFile.Visibility = System.Windows.Visibility.Visible;

                            this.rcbFiles.ItemsSource = PossibleFiles;
                            this.rcbFiles.DisplayMemberPath = "FileName";
                            this.rcbFiles.SelectedValuePath = "SourceFileID";
                        }
                        else
                            Core_AMS.Utilities.WPF.Message("No matching files found.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        DoValidation(); 
                }
                else
                    Core_AMS.Utilities.WPF.MessageError("You must select a file.");
            }
            else
                Core_AMS.Utilities.WPF.MessageError("You must select a client.");
        }
        private void DoValidation()
        {
            GC.Collect();
            FrameworkUAS.Entity.SourceFile sf = sfCheck;
            Helpers.Validation.FileValidator fv = new Helpers.Validation.FileValidator();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(tbFile.Text);

            #region File format check
            if (!isFileUploaded(client, fileInfo))
                return;

            //check headers for unicode characters - If found will immediately stop processing and request user redo column header and attempt File Validation again.
            var fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = sf.Delimiter,
                IsQuoteEncapsulated = sf.IsTextQualifier,
            };

            if (CheckUnicode(fileInfo, fileConfig))
            {
                return;
            }

            #endregion

            if (sf != null && sf.SourceFileID > 0)
            {
                if (rbLocal.IsChecked == true)
                {
                    #region Local file processing
                    if (!isFileUploaded(client, fileInfo))
                    {
                        sfCheck = null;
                        return;
                    }
                    Dictionary<string, string> downloadFiles = new Dictionary<string, string>();
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (o, ea) =>
                    {
                        downloadFiles = fv.ValidateFileAsObject(fileInfo, client, sf);
                    };

                    worker.RunWorkerCompleted += (o, ea) =>
                    {
                        tbResults.Inlines.Clear();
                        foreach (KeyValuePair<string, string> kvp in downloadFiles)
                        {
                            if (!kvp.Key.Equals("Exception"))
                            {
                                tbResults.Inlines.Add("File: " + kvp.Key + "  Location: " + kvp.Value + Environment.NewLine);
                            }
                            else
                                tbResults.Inlines.Add(kvp.Value);
                        }
                        busy.IsBusy = false;

                        cbMyClient.SelectedIndex = -1;
                        btnFile.IsEnabled = false;
                        btnValidate.IsEnabled = false;
                        sfCheck = null;
                        string dir = Core.ADMS.BaseDirs.getAppsDir() + "\\DQM\\";
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.InitialDirectory = dir;
                        ofd.ShowDialog();
                    };

                    busy.IsBusy = true;
                    worker.RunWorkerAsync();
                    #endregion
                }
                else
                {
                    client = cbMyClient.SelectedItem as EntityClient;
                    ValidateOffline(fileInfo, client, busy);
                }
            }
        }

        public static void ValidateOffline(
            FileSystemInfo fileInfo,
            EntityClient client, 
            RadBusyIndicator busy)
        {
            var clientFtp = UasAppData
                .myAppData
                .AuthorizedUser
                .ClientAdditionalProperties[client.ClientID]
                .ClientFtpDirectoriesList
                .FirstOrDefault();
            if (clientFtp != null)
            {
                var host = $"{clientFtp.Server}{AdmsValidatorPath}";

                var ftp = new FtpFunctions(host, clientFtp.UserName, clientFtp.Password);

                var uploadSuccess = false;
                var worker = new BackgroundWorker();
                worker.DoWork += (_, __) => { uploadSuccess = ftp.Upload(fileInfo.Name, fileInfo.FullName); };

                worker.RunWorkerCompleted += (_, __) =>
                {
                    if (uploadSuccess)
                    {
                        AmsWpf.MessageFileUploadComplete();
                    }
                    else
                    {
                        AmsWpf.MessageFileUploadError();
                    }
                    busy.IsBusy = false;
                };

                busy.IsBusy = true;
                worker.RunWorkerAsync();
            }
            else
            {
                AmsWpf.MessageError(
                    "FTP site is not configured for the selected client.  Please contact customer support.");
            }
        }

        public static bool CheckUnicode(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            var fileWorker = new AMSFileWorker();
            var unicodeHeaders = fileWorker.CheckHeadersForUnicodeChars(fileInfo, fileConfig);
            if (unicodeHeaders.Count > 0)
            {
                if (unicodeHeaders.ContainsValue(true))
                {
                    return true;
                }
            }
            else
            {
                AmsWpf.MessageError(
                    "I’m sorry, but we are unable to process your file due to an invalid file signature, " +
                    "or due to your file being corrupt.  Please review your file signature, " +
                    "and confirm your file’s validity, and resubmit for processing. " +
                    "If you need further assistance, please contact your Knowledge Marketing representative.");
                return true;
            }

            return false;
        }

        private void cbMyClient_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (cbMyClient.SelectedIndex != null)
            {
                btnFile.IsEnabled = true;
                btnValidate.IsEnabled = true;

                client = (KMPlatform.Entity.Client)cbMyClient.SelectedItem;
            }
        }

        private void rcbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FrameworkUAS.Entity.SourceFile sf = (FrameworkUAS.Entity.SourceFile)rcbFiles.SelectedItem;
            sfCheck = sf;

            //this.Background = System.Windows.Media.Brushes.Transparent;
            btnValidate.IsEnabled = true;
            btnFile.IsEnabled = true;
            cbMyClient.IsEnabled = true;
            pwMultiFile.Visibility = System.Windows.Visibility.Collapsed;

            DoValidation();
        }
    }
}
