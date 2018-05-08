using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for MasterCodeSheet.xaml
    /// </summary>
    public partial class MasterCodeSheet : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterCodeSheet> mcsWorker = FrameworkServices.ServiceClient.UAD_MasterCodeSheetClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterGroup> mgWorker = FrameworkServices.ServiceClient.UAD_MasterGroupClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionDetail> sdWorker = FrameworkServices.ServiceClient.UAD_SubscriptionDetailClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheetMasterCodeSheetBridge> csmcsbWorker = FrameworkServices.ServiceClient.UAD_CodeSheetMasterCodeSheetBridgeClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberMasterValue> smvWorker = FrameworkServices.ServiceClient.UAD_SubscriberMasterValueClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>> svMasterCodeSheet = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>> svMasterGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>>();
        FrameworkUAS.Service.Response<int> svIntMCS = new FrameworkUAS.Service.Response<int>();        

        List<FrameworkUAD.Entity.MasterCodeSheet> masterCodeSheets = new List<FrameworkUAD.Entity.MasterCodeSheet>();
        List<FrameworkUAD.Entity.MasterGroup> masterGroups = new List<FrameworkUAD.Entity.MasterGroup>();

        FrameworkUAD.Entity.MasterCodeSheet currentMasterCodeSheet = new FrameworkUAD.Entity.MasterCodeSheet();
        #endregion
        public MasterCodeSheet(int MasterGroupID = 0)
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData(MasterGroupID);                       
        }

        #region Loads
        public void LoadData(int MasterGroupID = 0)
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svMasterCodeSheet = mcsWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svMasterGroup = mgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svMasterCodeSheet.Result != null && svMasterCodeSheet.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)                
                    masterCodeSheets = svMasterCodeSheet.Result;                
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svMasterGroup.Result != null && svMasterGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    masterGroups = svMasterGroup.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadMasterCodeSheets();
                LoadMasterGroups(MasterGroupID);
                LoadSearch(); 

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        public void RefreshData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            int currentPage = radDataPager.PageIndex;
            bw.DoWork += (o, ea) =>
            {
                svMasterCodeSheet = mcsWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svMasterGroup = mgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svMasterCodeSheet.Result != null && svMasterCodeSheet.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    masterCodeSheets = svMasterCodeSheet.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svMasterGroup.Result != null && svMasterGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    masterGroups = svMasterGroup.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadMasterCodeSheets();
                LoadSearch();
                radDataPager.PageIndex = currentPage;

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        public void LoadMasterCodeSheets()
        {                                        
            if (cbMasterGroup.SelectedValue != null)
            {
                int mg = 0;
                int.TryParse(cbMasterGroup.SelectedValue.ToString(), out mg);
                gridDimensions.ItemsSource = masterCodeSheets.Where(x => x.MasterGroupID == mg).ToList();
            }            
        }
        #endregion

        #region Comboboxes Methods
        public void LoadMasterGroups(int MasterGroupID)
        {
            cbMasterGroup.ItemsSource = null;
                        
            List<FrameworkUAD.Entity.MasterGroup> mgResults = new List<FrameworkUAD.Entity.MasterGroup>();
            if (MasterGroupID > 0)
            {
                mgResults = masterGroups.Where(x => x.MasterGroupID == MasterGroupID).ToList();
            }
            else
            {
                mgResults = masterGroups;
            }                
            cbMasterGroup.ItemsSource = mgResults.OrderBy(x => x.SortOrder);
            cbMasterGroup.DisplayMemberPath = "DisplayName";
            cbMasterGroup.SelectedValuePath = "MasterGroupID";   
            if (MasterGroupID > 0)
            {
                cbMasterGroup.SelectedItem = masterGroups.FirstOrDefault(x => x.MasterGroupID == MasterGroupID);
            }               
        }
        public void LoadSearch()
        {
            cbSearch.Items.Clear();
            foreach (Core_AMS.Utilities.Enums.YesNo tf in (Core_AMS.Utilities.Enums.YesNo[])Enum.GetValues(typeof(Core_AMS.Utilities.Enums.YesNo)))
            {
                cbSearch.Items.Add(tf.ToString().Replace("_", " "));
            }
        }
        private void cbMasterGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMasterGroup.SelectedValue != null)
            {
                int MasterGrpID = 0;
                int.TryParse(cbMasterGroup.SelectedValue.ToString(), out MasterGrpID);

                gridDimensions.ItemsSource = null;
                if (masterCodeSheets != null)
                {
                    List<FrameworkUAD.Entity.MasterCodeSheet> distMasterCodeSheets = new List<FrameworkUAD.Entity.MasterCodeSheet>();
                    distMasterCodeSheets = masterCodeSheets.Where(x => x.MasterGroupID == MasterGrpID).ToList();
                    gridDimensions.ItemsSource = distMasterCodeSheets;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("No data to select from. Reload page and please contact Customer Support if problem persists.");
                    return;
                }
            }
            //else
            //{
            //    Core_AMS.Utilities.WPF.MessageError("Issue occurred with Master Group. Please contact Customer Supper if problem persists.");
            //    return;
            //}
        }
        #endregion

        #region Button Methods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {            
            string Value = "";
            string Desc = "";
            string Desc1 = tbxDesc1.Text;
            bool search = false;
            int MasterGrpID = 0;
            int MasterID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out MasterID);                

            #region Check Empty Data
            if (string.IsNullOrEmpty(tbxValue.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                Value = tbxValue.Text;

            if (string.IsNullOrEmpty(tbxDesc.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Description.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                Desc = tbxDesc.Text;

            if (string.IsNullOrEmpty(tbxDesc1.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Description1.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                Desc1 = tbxDesc1.Text;

            if (cbSearch.SelectedItem != null)
            {
                if (cbSearch.SelectedItem.ToString() == Core_AMS.Utilities.Enums.YesNo.Yes.ToString())
                    search = true;
                else
                    search = false;
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select enable searching value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }

            if (cbMasterGroup.SelectedValue != null)
                int.TryParse(cbMasterGroup.SelectedValue.ToString(), out MasterGrpID);
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select a Master Group value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            #endregion

            #region Check value doesn't exist
            if (currentMasterCodeSheet != null)
            {
                if (currentMasterCodeSheet.MasterValue != Value)
                {
                    if (masterCodeSheets.FirstOrDefault(x => x.MasterGroupID == MasterGrpID && x.MasterValue == Value) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                        return;
                    }
                }
            }
            else
            {
                if (masterCodeSheets.FirstOrDefault(x => x.MasterGroupID == MasterGrpID && x.MasterValue == Value) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                    return;
                }
            }
            #endregion

            #region Save|Update
            FrameworkUAD.Entity.MasterCodeSheet mcsEntry = new FrameworkUAD.Entity.MasterCodeSheet();
            mcsEntry.MasterID = MasterID;
            mcsEntry.MasterValue = Value;
            mcsEntry.MasterDesc = Desc;
            mcsEntry.MasterGroupID = MasterGrpID;
            mcsEntry.MasterDesc1 = Desc1;
            mcsEntry.EnableSearching = search;
            mcsEntry.SortOrder = 0;
            svIntMCS = mcsWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, mcsEntry, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svIntMCS.Result != null && svIntMCS.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                Core_AMS.Utilities.WPF.MessageSaveComplete();
                currentMasterCodeSheet = null;
                tbxValue.Text = "";
                tbxDesc.Text = "";
                tbxDesc1.Text = "";
                cbSearch.SelectedIndex = -1;

                btnSave.Tag = "";
                btnSave.Content = "Save";         
                RefreshData();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                return;
            }
            #endregion
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            currentMasterCodeSheet = null;
            tbxValue.Text = "";
            tbxDesc.Text = "";
            tbxDesc1.Text = "";
            cbSearch.SelectedIndex = -1;

            btnSave.Tag = "";
            btnSave.Content = "Save";
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.MasterCodeSheet))
                {
                    FrameworkUAD.Entity.MasterCodeSheet mcsItem = (FrameworkUAD.Entity.MasterCodeSheet)b.DataContext;
                    if (mcsItem != null)
                    {
                        currentMasterCodeSheet = mcsItem;
                        tbxValue.Text = currentMasterCodeSheet.MasterValue;
                        tbxDesc.Text = currentMasterCodeSheet.MasterDesc;
                        tbxDesc1.Text = currentMasterCodeSheet.MasterDesc1;
                        if (currentMasterCodeSheet.EnableSearching.ToString() == Core_AMS.Utilities.Enums.TrueFalse.True.ToString())                        
                            cbSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
                        else
                            cbSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.No.ToString();

                        btnSave.Tag = currentMasterCodeSheet.MasterID.ToString();
                        btnSave.Content = "Update";
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int MasterGroupID = 0;
            if(cbMasterGroup.SelectedItem != null)
            {
                int.TryParse(cbMasterGroup.SelectedItem.ToString(), out MasterGroupID);
            }
            MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to delete this Master Code Sheet? It will delete all mapping for the Master Code Sheet.", "Warning", MessageBoxButton.YesNo);

            if (areYouSureDelete == MessageBoxResult.Yes)
            {
                if (e.OriginalSource.GetType() == typeof(Button))
                {
                    Button b = (Button)e.OriginalSource;
                    if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.MasterCodeSheet))
                    {
                        FrameworkUAD.Entity.MasterCodeSheet mcsItem = (FrameworkUAD.Entity.MasterCodeSheet)b.DataContext;
                        if (mcsItem != null)
                        {
                            int MasterID = mcsItem.MasterID;
                            #region Delete SubscriptionDetails 
                            FrameworkUAS.Service.Response<bool> svBoolSD = new FrameworkUAS.Service.Response<bool>();
                            svBoolSD = sdWorker.Proxy.DeleteMasterID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, MasterID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if ((svBoolSD.Result == true || svBoolSD.Result == false) && svBoolSD.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            {
                                #region Delete SubscriberMasterValues
                                FrameworkUAS.Service.Response<bool> svBoolSMV = new FrameworkUAS.Service.Response<bool>();
                                svBoolSMV = smvWorker.Proxy.DeleteMasterID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, MasterID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                if ((svBoolSMV.Result == true || svBoolSMV.Result == false) && svBoolSMV.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                                {
                                    #region Delete CodeSheet_Mastercodesheet_Bridge
                                    FrameworkUAS.Service.Response<bool> svBoolCSMCSB = new FrameworkUAS.Service.Response<bool>();
                                    svBoolCSMCSB = csmcsbWorker.Proxy.DeleteMasterID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, MasterID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                    if ((svBoolCSMCSB.Result == true || svBoolCSMCSB.Result == false) && svBoolCSMCSB.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                                    {                                    
                                        #region Delete Mastercodesheet
                                        FrameworkUAS.Service.Response<bool> svBoolMCS = new FrameworkUAS.Service.Response<bool>();
                                        svBoolMCS = mcsWorker.Proxy.DeleteMasterID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, MasterID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                        if ((svBoolMCS.Result == true || svBoolMCS.Result == false) && svBoolMCS.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                                        {
                                            Core_AMS.Utilities.WPF.MessageDeleteComplete();
                                            currentMasterCodeSheet = null;
                                            tbxValue.Text = "";
                                            tbxDesc.Text = "";
                                            tbxDesc1.Text = "";
                                            cbSearch.SelectedIndex = -1;

                                            btnSave.Tag = "";
                                            btnSave.Content = "Save";       
                                            RefreshData();
                                        }
                                        else
                                        {
                                            Core_AMS.Utilities.WPF.Message("Failed at Step 4 of 4. If this problem consists please contact Customer Service.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                            return;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        Core_AMS.Utilities.WPF.Message("Failed at Step 3 of 4. If this problem consists please contact Customer Service.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                        return;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    Core_AMS.Utilities.WPF.Message("Failed at Step 2 of 4. If this problem consists please contact Customer Service.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                    return;
                                }
                                #endregion                                
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.Message("Failed at Step 1 of 4. If this problem consists please contact Customer Service.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                return;
                            }
                            #endregion                            
                        }
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                        return;
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                    return;
                }
            }
        }       
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            int masterGrpID = 0;
            if (cbMasterGroup.SelectedValue != null)
                int.TryParse(cbMasterGroup.SelectedValue.ToString(), out masterGrpID);

            if (!(masterGrpID > 0))
            {
                Core_AMS.Utilities.WPF.Message("Select master group before proceeding.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                return;
            }

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Recognized Files(*.csv)|*.csv"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {                
                if (ValidateUploadCsvHeaders(dlg.FileName))
                {
                    Stream stream = File.OpenRead(dlg.FileName);            
                    List<XDocument> xDocList = SerializeCsvFile(stream, "mastergrouplist", "mastergroup", 5000);

                    try
                    {
                        if (!UploadedMasterValuesContainCommas(xDocList))
                        {                                                        
                            foreach (XDocument xDoc in xDocList)
                            {                                
                                FrameworkUAS.Service.Response<int> mcsBool = new FrameworkUAS.Service.Response<int>();
                                mcsBool = mcsWorker.Proxy.ImportSubscriber(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 1, xDoc, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                if (mcsBool != null && mcsBool.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                                {
                                    continue;
                                }
                                else
                                {
                                    Core_AMS.Utilities.WPF.Message("Error while uploading data from file. If the problem persists please contact customer support.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                    return;
                                }
                            }
                            Core_AMS.Utilities.WPF.Message("File successfully uploaded.", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, "Action Completed");

                            LoadData();
                            LoadMasterCodeSheets();                           
                        }
                        else
                        {
                            Core_AMS.Utilities.WPF.Message("Values in MASTERVALUE cannot contain commas.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                            return;                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Core_AMS.Utilities.WPF.Message("An error has occured uploading your file. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                        FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                        Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                        int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".btnUpload_Click", app, string.Empty, logClientId);
                        return;                        
                    }    
                }
            }
            else
                return;

        }
        #endregion

        #region Upload Methods
        private bool ValidateUploadCsvHeaders(string file)
        {
            bool isValid = false;
            try
            {
                Stream stream = File.OpenRead(file);            

                MemoryStream memoryStream = new MemoryStream();

                try
                {
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;

                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                        string headerString = reader.ReadLine();
                        isValid = string.Compare(headerString, "IGROUPNO,MASTERVALUE,MASTERDESC", true) == 0;
                    }
                }
                finally
                {
                    stream.Position = 0;

                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                    }

                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                Core_AMS.Utilities.WPF.Message("There was an error working with the file. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Upload CSV");
                return false;
            }

            return isValid;
        }

        private bool UploadedMasterValuesContainCommas(List<XDocument> xDocList)
        {
            bool containsValue = false;

            foreach (var xDoc in xDocList)
            {
                containsValue = (from c in xDoc.Descendants("MasterGroup").Elements("MASTERVALUE")
                                 where c.Value.Contains(',')
                                 select c).Count() > 0;

                if (containsValue)
                {
                    break;
                }
            }

            return containsValue;
        }

        public static XDocument SerializeCsvFile(Stream stream, string rootNode, string itemNode)
        {
            return SerializeCsvFile(stream, rootNode, itemNode, 0).First();
        }

        public static List<XDocument> SerializeCsvFile(Stream stream, string rootNode, string itemNode, int maxCollectionSize)
        {
            List<XDocument> documentList = new List<XDocument>();
            stream.Position = 0;

            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(stream))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");

                string[] headerValues = parser.ReadFields();

                string[] headerValuesLower = (from str in headerValues
                                              select str.ToLower()).ToArray();

                while (!parser.EndOfData)
                {
                    XDocument xDoc = new XDocument(new XElement(rootNode));
                    int collectionItemCount = 0;

                    while ((collectionItemCount < maxCollectionSize || maxCollectionSize <= 0)
                        && !parser.EndOfData)
                    {
                        string[] csvData = parser.ReadFields();
                        xDoc.Root.Add(BuildElement(headerValuesLower, csvData, itemNode));
                        collectionItemCount++;
                    }

                    documentList.Add(xDoc);
                }
            }

            return documentList;
        }

        private static XElement BuildElement(string[] headerValues, string[] csvData, string itemNode)
        {
            XElement newItemElement = new XElement(itemNode);
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";

            if (csvData.Length == headerValues.Length)
            {
                for (int i = 0; i < headerValues.Length; i++)
                {
                    newItemElement.Add(new XElement(headerValues[i], Regex.Replace(csvData[i], re, "")));
                }
            }

            return newItemElement;
        }
        #endregion
    }
}
