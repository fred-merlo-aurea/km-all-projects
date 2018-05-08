using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataImportExport.Modules
{
    /// <summary>
    /// Interaction logic for CircImport.xaml
    /// </summary>
    public partial class CircImport : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sfWorker = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        //FrameworkServices.ServiceClient<Circulation_WS.Interface.IPublication> publicationWorker = FrameworkServices.ServiceClient.Circ_PublicationClient();
        #endregion

        #region Variables
        FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>> svSF = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>>();        
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPUB = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();

        List<FrameworkUAS.Entity.SourceFile> allSourceFiles = new List<FrameworkUAS.Entity.SourceFile>();                
        List<FrameworkUAD.Entity.Product> allUADPublications = new List<FrameworkUAD.Entity.Product>();

        KMPlatform.Entity.Client thisClient = new KMPlatform.Entity.Client();

        string FileName;
        #endregion

        public CircImport(FrameworkUAD.Entity.Product selectedProduct = null, string filePath = "")
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > -1))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            else
                thisClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;

            InitializeComponent();                
            LoadData(selectedProduct, filePath);
        }

        private void LoadData(FrameworkUAD.Entity.Product selectedProduct, string filePath)
        {
            radBusy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svSF = sfWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisClient.ClientID, false);                
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load SourceFiles
                if (svSF.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && svSF.Result != null)
                {
                    allSourceFiles = svSF.Result.Where(x => x.IsDeleted == false).ToList().Where(x => x.ClientID == thisClient.ClientID).ToList();
                    int clientID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientID].SourceFilesList = allSourceFiles;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                #region Load Publications
                svPUB = pubWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisClient.ClientConnections);

                if (svPUB.Result != null && svPUB.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    allUADPublications = svPUB.Result;
                
                FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> pubResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
                pubResponse = pubData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisClient.ClientConnections);

                if (pubResponse.Result != null && pubResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    cbPublication.ItemsSource = pubResponse.Result.Where(x => x.IsCirc == true);
                    cbPublication.DisplayMemberPath = "PubCode";
                    cbPublication.SelectedValuePath = "PubID";

                    if (selectedProduct != null)
                        cbPublication.SelectedValue = pubResponse.Result.FirstOrDefault(x => x.PubCode == selectedProduct.PubCode).PubID; //selectedProduct.PubID;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                    return;
                }                

                #endregion

                #region File Selected
                if (!string.IsNullOrEmpty(filePath))
                {
                    string fileName = filePath;
                    tbxName.Text = System.IO.Path.GetFileName(fileName);
                    lblFilePath.Content = fileName.ToString();

                    FileName = fileName;
                }
                #endregion
                radBusy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }        

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            tbxName.Text = "";
            lblFilePath.Content = "";

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Recognized Files(*.txt;*.csv;*.xls;*.xlsx;*.dbf;*.zip;*.xml;*.json)|*.txt;*.csv;*.xls;*.xlsx;*.dbf;*.zip;*.xml;*.json"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                string fileName = dlg.FileName;
                tbxName.Text = System.IO.Path.GetFileName(fileName);
                lblFilePath.Content = fileName.ToString();

                FileName = fileName;
            }
            else
                return;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            FrameworkUAD.Entity.Product prod = new FrameworkUAD.Entity.Product();
            if (!string.IsNullOrEmpty(FileName))
            {
                int PublicationID = 0;
                #region Check File is under right publication
                if (cbPublication.SelectedValue != null)
                {                    
                    int.TryParse(cbPublication.SelectedValue.ToString(), out PublicationID); 
                    prod = (FrameworkUAD.Entity.Product)cbPublication.SelectedItem;              
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("Please select a publication before you continue.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                    return;
                }
                #endregion

                #region Check SourceFile is in selected Publication
                List<FrameworkUAS.Entity.SourceFile> sourcesInPublication = new List<FrameworkUAS.Entity.SourceFile>();
                sourcesInPublication = allSourceFiles.Where(x => x.PublicationID == PublicationID).ToList();

                if (sourcesInPublication.FirstOrDefault(x => x.FileName.Equals(System.IO.Path.GetFileNameWithoutExtension(FileName), StringComparison.CurrentCultureIgnoreCase)) == null)
                {
                    if (sourcesInPublication.Exists(x => (System.IO.Path.GetFileNameWithoutExtension(FileName).ToLower()).StartsWith(x.FileName.ToLower())) == false)
                    {                    
                        if (allSourceFiles.Exists(x => x.FileName.Equals(System.IO.Path.GetFileNameWithoutExtension(FileName), StringComparison.CurrentCultureIgnoreCase)) == false)
                        {
                            MessageBoxResult messBox = MessageBox.Show("This file was not recognized as a circulation file for the selected product. This file needs to be mapped before proceeding. Do you want to map the file now?", "Warning", MessageBoxButton.YesNo);

                            if (messBox == MessageBoxResult.No)
                                return;                        

                            //Close old window with import but cannot close main window
                            var CircImportWindow = Core_AMS.Utilities.WPF.GetWindow(this);
                            if (CircImportWindow.GetType().ToString().Replace(".", "_") != Core_AMS.Utilities.Enums.Windows.AMS_Desktop_Windows_Home.ToString())
                            {
                                CircImportWindow.Close();
                            }

                            //Open File Mapper Wizard window
                            FileMapperWizard.Modules.FMUniversal fmLite = new FileMapperWizard.Modules.FMUniversal(false, true, thisClient, prod, FileName);
                            PopOutWindow(KMPlatform.BusinessLogic.Enums.Applications.Circulation, fmLite, "File Mapper Wizard");                        
                            return;
                        }
                        else
                        {
                            MessageBoxResult messBox = MessageBox.Show("This file is mapped in a different publication. If you wish to import it, you must first delete the mapping in the other publication.", "Warning");
                            return;
                        }
                    }
                }
                #endregion
                
                FileInfo fInfo = new FileInfo(FileName);
                if (Core_AMS.Utilities.FileFunctions.IsFileLocked(fInfo))
                {
                    Core_AMS.Utilities.WPF.Message("File is currently locked. This could be because the file is opened. Please close and import again.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                    return;
                }
                else
                {
                    #region Upload to FTP
                    System.IO.FileInfo file = new System.IO.FileInfo(FileName);                    
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisClient.ClientID].ClientFtpDirectoriesList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count > 1)
                    {
                        Core_AMS.Utilities.WPF.Message("Client has more than one active FTP settings. Please contact customer service to have this fixed before proceeding.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                        return;
                    }

                    FrameworkUAS.Entity.ClientFTP cFTP = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisClient.ClientID].ClientFtpDirectoriesList.FirstOrDefault();
                    if (cFTP != null)
                    {
                        string host = "";
                        host = cFTP.Server + "/ADMS/";

                        Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                        bool uploadSuccess = false;
                        radBusy.IsBusy = true;
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
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.MessageFileUploadError();
                            }
                            radBusy.IsBusy = false;
                        };

                        radBusy.IsBusy = true;
                        worker.RunWorkerAsync();
                        
                    }
                    else
                        Core_AMS.Utilities.WPF.MessageError("FTP site is not configured for the selected client.  Please contact Customer Support.");

                    #endregion
                }
            }
            else
                Core_AMS.Utilities.WPF.MessageError("You must select a file.");

        }

        private void PopOutWindow(KMPlatform.BusinessLogic.Enums.Applications app, System.Windows.Controls.UserControl uc, string windowTitle)
        {
            WpfControls.WindowsAndDialogs.PopOut win = new WpfControls.WindowsAndDialogs.PopOut(app, uc);
            win.Title = windowTitle;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
        }
    }
}
