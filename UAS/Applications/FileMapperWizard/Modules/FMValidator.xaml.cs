using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DQM.Modules;
using KM.Common.Import;
using Microsoft.VisualBasic.Devices;
using EntityClient = KMPlatform.Entity.Client;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for FMValidator.xaml
    /// </summary>
    public partial class FMValidator : UserControl
    {
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> blc = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IService> bls = FrameworkServices.ServiceClient.UAS_ServiceClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sfData = FrameworkServices.ServiceClient.UAS_SourceFileClient();

        public List<KMPlatform.Entity.Client> AllClients = new List<KMPlatform.Entity.Client>();
        DQM.Helpers.Validation.FileValidator fv = new DQM.Helpers.Validation.FileValidator();
        public DoubleAnimation animateWindow { get; set; }
        public DoubleAnimation animateWindowReverse { get; set; }

        public FMValidator(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            KMPlatform.Entity.Client SelectedClient = appData.AuthorizedUser.User.CurrentClient;
            if (SelectedClient.ClientID == 1)
            {
                AllClients = appData.AuthorizedUser.User.CurrentClientGroup.Clients;
                rcbClients.ItemsSource = AllClients;
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
            }
            else
            {
                KMPlatform.Entity.Client sc = SelectedClient;
                if (appData.AuthorizedUser.User.ClientGroups.Exists(x => x.Clients.Exists(y => y.ClientID == SelectedClient.ClientID)) == true)
                {
                if (sc == null)
                    {
                        foreach (var cg in appData.AuthorizedUser.User.ClientGroups)
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

                rcbClients.Items.Add(SelectedClient);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
                rcbClients.SelectedItem = sc;
                if (!appData.AuthorizedUser.ClientAdditionalProperties.ContainsKey(sc.ClientID))
                {

                    FrameworkUAS.Object.ClientAdditionalProperties cap = blc.Proxy.GetClientAdditionalProperties(appData.AuthorizedUser.AuthAccessKey, sc.ClientID, false).Result;
                    if (cap != null)
                        appData.AuthorizedUser.ClientAdditionalProperties.Add(sc.ClientID, cap);
                }

            }
            Style s = this.FindResource("roundedComboBoxes") as Style;
            rcbClients.Style = s;
            MemoryCheck();
        }
        private void MemoryCheck()
        {
            UAD_FileValidator.CheckMemory(new ComputerInfo().TotalPhysicalMemory, rbLocal, rbOffline, spProcess);
        }
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
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
                txtFile.Text = filename;
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

        #region ProgressChanges
        private void OverallProgress(object sender, RoutedEventArgs e)
        {
            int percent = 0;
            int.TryParse(fv.overallProgress.Text.ToString(), out percent);
            overallProgressBar.Value = percent;
            overallPercent.Text = percent.ToString();
        }
        private void CurrentProgress(object sender, RoutedEventArgs e)
        {
            int percent = 0;
            int.TryParse(fv.currentProgress.Text.ToString(), out percent);
            currentProgressBar.Value = percent;
            currentPercent.Text = percent.ToString();
        }
        private void CurrentOperation(object sender, RoutedEventArgs e)
        {
            //txtCurrentOp.Text = txtCurrentOp.Text + "\n" + fv.currentOperation.Text;
            //txtCurrentOp.Inlines.Clear();
            if (fv.currentOperation.Text.Contains("error"))
                txtCurrentOp.Inlines.Add(new Run(fv.currentOperation.Text + "\n") { Foreground = Brushes.Red });
            else
                txtCurrentOp.Inlines.Add(new Run(fv.currentOperation.Text + "\n"));
            svCurrentOp.ScrollToBottom();
        }
        #endregion
        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            #region animation
            GC.Collect();
            double originalHeight = rectValidator.ActualHeight;
            rectValidator.Height = originalHeight;

            animateWindow = new DoubleAnimation
            {
                To = 10,
                Duration = TimeSpan.FromSeconds(.4)
            };

            animateWindowReverse = new DoubleAnimation
            {
                To = originalHeight,
                Duration = TimeSpan.FromSeconds(.4)
            };

            animateWindow.Completed += (s, _) =>
            {
                rectValidator.BeginAnimation(Rectangle.HeightProperty, animateWindowReverse);
            };
            #endregion
            if (rcbClients.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(txtFile.Text))
                {
                    #region SourceFile check
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(txtFile.Text);
                    KMPlatform.Entity.Client client = (KMPlatform.Entity.Client)rcbClients.SelectedItem;
                    List<FrameworkUAS.Entity.SourceFile> sourceFilesList = new List<FrameworkUAS.Entity.SourceFile>();
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.ContainsKey(client.ClientID))
                        sourceFilesList = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].SourceFilesList.Where(x => x.IsDeleted == false).ToList();
                    else
                    {
                        sourceFilesList = sfData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, client.ClientID, false).Result.Where(x => x.IsDeleted == false).ToList();
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.Add(client.ClientID, new FrameworkUAS.Object.ClientAdditionalProperties());
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].SourceFilesList = sourceFilesList;
                    }
                    FrameworkUAS.Entity.SourceFile sf = null;
                    string incomingFile = fileInfo.Name.Replace(fileInfo.Extension, "").ToLower();
                    if (sourceFilesList.Exists(x => incomingFile.StartsWith(x.FileName.ToLower())))
                    {
                        if (sourceFilesList.Exists(x => incomingFile.Equals(x.FileName, StringComparison.CurrentCultureIgnoreCase)))
                            sf = sourceFilesList.FirstOrDefault(x => incomingFile.Equals(x.FileName, StringComparison.CurrentCultureIgnoreCase));
                        else
                            sf = sourceFilesList.FirstOrDefault(x => incomingFile.StartsWith(x.FileName.ToLower()));
                    }

                    fv.overallProgress.TextChanged += new TextChangedEventHandler(OverallProgress);
                    fv.currentProgress.TextChanged += new TextChangedEventHandler(CurrentProgress);
                    fv.currentOperation.TextChanged += new TextChangedEventHandler(CurrentOperation);
                    #endregion
                    if (sf != null && sf.SourceFileID > 0)
                    {
                        #region File format check
                        if (!isFileUploaded(client, fileInfo))
                            return;

                        //check headers for unicode characters - If found will immediately stop processing and request user redo column header and attempt File Validation again.
                        var fileConfig = new FileConfiguration()
                        {
                            FileColumnDelimiter = sf.Delimiter,
                            IsQuoteEncapsulated = sf.IsTextQualifier,
                        };

                        if (UAD_FileValidator.CheckUnicode(fileInfo, fileConfig))
                        {
                            return;
                        }

                        #endregion
                        if (rbLocal.IsChecked == true)
                        {
                            #region run local
                            grdValidatorButtons.Visibility = Visibility.Hidden;
                            rectValidator.BeginAnimation(Rectangle.HeightProperty, animateWindow);
                            animateWindowReverse.Completed += (s, _) =>
                            {
                                grdValidatorFeedback.Visibility = Visibility.Visible;

                                Dictionary<string, string> downloadFiles = new Dictionary<string, string>();
                                BackgroundWorker worker = new BackgroundWorker();
                                worker.DoWork += (o, ea) =>
                                {
                                    downloadFiles = fv.ValidateFileAsObject(fileInfo, client, sf);
                                };

                                worker.RunWorkerCompleted += (o, ea) =>
                                {
                                    foreach (KeyValuePair<string, string> kvp in downloadFiles)
                                    {
                                        if (!kvp.Key.Equals("Exception"))
                                        {
                                            txtCurrentOp.Inlines.Add(new Run("File: " + kvp.Key + "  Location: " + kvp.Value + Environment.NewLine) { Foreground = Brushes.Gray });
                                        }
                                        overallProgressBar.Value = 100;
                                        currentProgressBar.Value = 0;
                                        overallPercent.Text = "100";
                                        currentPercent.Text = "0";
                                        btnReValidate.Visibility = System.Windows.Visibility.Visible;
                                    }
                                    Core_AMS.Utilities.WPF.MessageTaskComplete("Done!");
                                    string dir = Core.ADMS.BaseDirs.getAppsDir() + "\\DQM\\";
                                    if (!Directory.Exists(dir))
                                        Directory.CreateDirectory(dir);
                                    OpenFileDialog ofd = new OpenFileDialog();
                                    ofd.InitialDirectory = dir;
                                    ofd.ShowDialog();
                                };
                                worker.RunWorkerAsync();
                            };
                            #endregion
                        }
                        else
                        {
                            client = rcbClients.SelectedItem as EntityClient;
                            UAD_FileValidator.ValidateOffline(fileInfo, client, busy);
                        }
                    }
                    else
                        Core_AMS.Utilities.WPF.MessageError("The selected file is not setup as a valid Source File for client " + client.FtpFolder);
                }
                else
                    Core_AMS.Utilities.WPF.MessageError("You must select a file.");

            }
            else
                Core_AMS.Utilities.WPF.MessageError("You must select a client.");
        }

        private void btnReValidate_Click(object sender, RoutedEventArgs e)
        {
            btnReValidate.Visibility = System.Windows.Visibility.Collapsed;
            grdValidatorFeedback.Visibility = Visibility.Collapsed;
            grdValidatorButtons.Visibility = Visibility.Visible;
            txtCurrentOp.Inlines.Clear();
            currentPercent.Text = "0";
            overallPercent.Text = "0";
            currentProgressBar.Value = 0;
            overallProgressBar.Value = 0;
            fv = new DQM.Helpers.Validation.FileValidator();
        }

        private void rcbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KMPlatform.Entity.Client sc = (KMPlatform.Entity.Client)rcbClients.SelectedItem;
            if (sc != null)
            {
                if (!FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.ContainsKey(sc.ClientID))
                {
                    FrameworkUAS.Object.ClientAdditionalProperties cap = blc.Proxy.GetClientAdditionalProperties(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sc.ClientID, false).Result;
                    if (cap != null)
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.Add(sc.ClientID, cap);
                }
            }
        }
    }
}
