using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for Review.xaml
    /// </summary>
    public partial class Review : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fieldMappingWorker = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformation> transformationWorker = FrameworkServices.ServiceClient.UAS_TransformationClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sfWorker = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        #endregion
        #region Variables
        FileMapperWizard.Modules.FMUniversal thisContainer;
        List<FrameworkUAS.Entity.SourceFile> SourceFilesList = new List<FrameworkUAS.Entity.SourceFile>();
        #endregion

        public Review(FileMapperWizard.Modules.FMUniversal container)
        {
            InitializeComponent();
            thisContainer = container;
            LoadData();
        }

        private void LoadData()
        {
            busyIcon.IsBusy = true;

            txbMessage.Text = "File Review - " + System.IO.Path.GetFileName(thisContainer.FileName);
                        
            BackgroundWorker bw = new BackgroundWorker();
            List<FrameworkUAS.Entity.FieldMapping> fieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();            
            List<FrameworkUAS.Entity.Transformation> transformations = new List<FrameworkUAS.Entity.Transformation>();
            List<FrameworkUAS.Entity.TransformationFieldMultiMap> alltfmm = new List<FrameworkUAS.Entity.TransformationFieldMultiMap>();
              
            bw.DoWork += (o, ea) =>
            {                                
                fieldMappings = fieldMappingWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID, true).Result.OrderBy(x => x.ColumnOrder).ToList();                    
                transformations = transformationWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID, thisContainer.sourceFileID, true).Result;
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                foreach (FrameworkUAS.Entity.FieldMapping fm in fieldMappings)
                {
                    List<FrameworkUAS.Entity.Transformation> currentTransformations = new List<FrameworkUAS.Entity.Transformation>();
                    currentTransformations = transformations.Where(x => x.FieldMap.Any(y => y.FieldMappingID == fm.FieldMappingID)).ToList();
                    FileMapperWizard.Controls.FileReview fileReview = new Controls.FileReview(thisContainer, fm.IncomingField, fm.MAFField, fm, currentTransformations);
                    spReview.Children.Add(fileReview);
                }
                busyIcon.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        private void btnFinishReview_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messBox = MessageBox.Show("Are you sure you have completed work on this file?", "Warning", MessageBoxButton.YesNo);

            if (messBox == MessageBoxResult.No)
                return;

            #region UPLOAD CIRC FILE
            if (thisContainer.isCirculation && !string.IsNullOrEmpty(thisContainer.filePath))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to import this file now?", "Warning", MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    System.IO.FileInfo fInfo = new System.IO.FileInfo(thisContainer.filePath);
                    if (Core_AMS.Utilities.FileFunctions.IsFileLocked(fInfo))
                    {
                        Core_AMS.Utilities.WPF.Message("File is currently locked. This could be because the file is opened. Please close and import again.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                        return;
                    }
                    else
                    {
                        #region Upload to FTP
                        System.IO.FileInfo file = new System.IO.FileInfo(thisContainer.filePath);
                        KMPlatform.Entity.Client client = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                        List<FrameworkUAS.Entity.ClientFTP> ClientFtpDirectoriesList = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].ClientFtpDirectoriesList;
                        if (ClientFtpDirectoriesList.Where(x => x.IsActive == true).ToList().Count > 1)
                        {
                            Core_AMS.Utilities.WPF.Message("Client has more than one active FTP settings. Please contact customer service to have this fixed before proceeding.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                            return;
                        }

                        FrameworkUAS.Entity.ClientFTP cFTP = ClientFtpDirectoriesList.FirstOrDefault();
                        if (cFTP != null)
                        {
                            string host = "";
                            host = cFTP.Server + "/ADMS/";

                            Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                            SourceFilesList = sfWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID, false, false).Result.ToList();
                            FrameworkUAS.Entity.SourceFile sf = SourceFilesList.FirstOrDefault(x => thisContainer.fileInfo.Name.ToLower().StartsWith(x.FileName.ToLower()) && x.SourceFileID != thisContainer.sourceFileID);
                            if (sf == null)
                            {
                                bool uploadSuccess = false;
                                Telerik.Windows.Controls.RadBusyIndicator busy = Core_AMS.Utilities.WPF.FindControl<Telerik.Windows.Controls.RadBusyIndicator>(thisContainer, "busyIcon");
                                busy.IsBusy = true;
                                BackgroundWorker worker = new BackgroundWorker();
                                worker.DoWork += (o, ea) =>
                                {
                                    uploadSuccess = ftp.Upload(file.Name, file.FullName);
                                };

                                worker.RunWorkerCompleted += (o, ea) =>
                                {
                                    if (uploadSuccess == true)
                                    {
                                        Core_AMS.Utilities.WPF.Message("Your file has been imported. View the Import Status page for file progress updates and import confirmation.", MessageBoxButton.OK, MessageBoxImage.Information, "File Uploaded");

                                        //Close old window with import but cannot close main window
                                        var oo = Core_AMS.Utilities.WPF.GetWindow(this);
                                        if (oo.GetType().ToString().Replace(".", "_") != Core_AMS.Utilities.Enums.Windows.AMS_Desktop_Windows_Home.ToString())
                                        {
                                            oo.Close();
                                        }
                                        else
                                        {
                                            CloseWindow();
                                        }
                                    }
                                    else
                                    {
                                        Core_AMS.Utilities.WPF.MessageFileUploadError();
                                    }
                                    busy.IsBusy = false;
                                };

                                busy.IsBusy = true;
                                worker.RunWorkerAsync();
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.Message("There is a file with a similar name, please wait for a engine refresh before dropping your file.", MessageBoxButton.OK, MessageBoxImage.Information, "Upload not available");
                                var oo = Core_AMS.Utilities.WPF.GetWindow(this);
                                if (oo.GetType().ToString().Replace(".", "_") != Core_AMS.Utilities.Enums.Windows.AMS_Desktop_Windows_Home.ToString())
                                {
                                    oo.Close();
                                }
                                else
                                {
                                    CloseWindow();
                                }
                            }
                        }
                        else
                            Core_AMS.Utilities.WPF.MessageError("FTP site is not configured for the selected client.  Please contact Customer Support.");

                        #endregion
                    }
                }
                else
                {
                    CloseWindow();
                }
            }
            //else if (this.thisContainer.isCirculation == false && this.thisContainer.myUADFeature == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
            //{
            //    GoToDataCompareStep();
            //}
            else
            {
                CloseWindow();
            }
            #endregion         
        }
        //private void GoToDataCompareStep()
        //{
        //    thisContainer.TransformationsToDataCompareOptions();
        //    //need new window
        //    //foreach (Window window in Application.Current.Windows.OfType<FileMapperWizard.Modules.FMLiteWindow>())
        //    //    ((FileMapperWizard.Modules.FMLiteWindow)window).Close();
        //    CloseWindow();

        //    Windows.FMWindow newMappingWindow = new Windows.FMWindow("Data Compare");
        //    StackPanel stackPanel = new StackPanel();
        //    stackPanel.Children.Clear();
        //    FileMapperWizard.Modules.DataCompareSteps dc = new FileMapperWizard.Modules.DataCompareSteps(thisContainer);
        //    stackPanel.Children.Add(dc);
        //    newMappingWindow.spContent.Children.Add(stackPanel);
        //    newMappingWindow.Title = "Data Compare";
        //    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
        //    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    newMappingWindow.ShowDialog();
        //    newMappingWindow.Activate();
        //}
        private void btnPrevReview_Click(object sender, RoutedEventArgs e)
        {
            thisContainer.ReviewToRules();
        }

        private void CloseWindow()
        {            
            object win = Core_AMS.Utilities.WPF.GetWindow(this);           
            if (win != null && win.GetType() == typeof (Windows.FMWindow))
            {
                Windows.FMWindow fmWin = (Windows.FMWindow) win;
                fmWin.Close();
            }
            else
            {
                object pwin = Core_AMS.Utilities.WPF.GetWindow(this);
                if (pwin != null)
                {
                    if (pwin.GetType() == typeof(WpfControls.WindowsAndDialogs.PopOut))
                    {
                        WpfControls.WindowsAndDialogs.PopOut p = (WpfControls.WindowsAndDialogs.PopOut)pwin;
                        p.Close();
                    }                    
                }
            }
        }  
    }
}
