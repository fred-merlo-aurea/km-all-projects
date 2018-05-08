using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;


namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for CodeSheet.xaml
    /// </summary>
    public partial class CodeSheet : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> csWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheetMasterCodeSheetBridge> csmcsbWorker = FrameworkServices.ServiceClient.UAD_CodeSheetMasterCodeSheetBridgeClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterCodeSheet> mcsWorker = FrameworkServices.ServiceClient.UAD_MasterCodeSheetClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterData> mdWorker = FrameworkServices.ServiceClient.UAD_MasterDataClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterGroup> mgWorker = FrameworkServices.ServiceClient.UAD_MasterGroupClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> prodWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> psWorker = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient(); 
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rgWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IReportGroups> rpgWorker = FrameworkServices.ServiceClient.UAD_ReportGroupsClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> svCodeSheets = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>> svMasterCodeSheets = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Object.MasterData>> svMasterData = new FrameworkUAS.Service.Response<List<FrameworkUAD.Object.MasterData>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>> svMasterGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProduct = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();        
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>> svReportGroups = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>>();
        FrameworkUAS.Service.Response<bool> svBoolCS = new FrameworkUAS.Service.Response<bool>();
        FrameworkUAS.Service.Response<bool> svBoolCSMSSB = new FrameworkUAS.Service.Response<bool>();
        FrameworkUAS.Service.Response<bool> svBoolPSD = new FrameworkUAS.Service.Response<bool>();
        
        List<FrameworkUAD.Entity.CodeSheet> codeSheets = new List<FrameworkUAD.Entity.CodeSheet>();
        List<FrameworkUAD.Entity.MasterCodeSheet> allMasterCodeSheets = new List<FrameworkUAD.Entity.MasterCodeSheet>();
        List<FrameworkUAD.Object.MasterData> allMasterData = new List<FrameworkUAD.Object.MasterData>();
        List<FrameworkUAD.Entity.MasterGroup> allMasterGroup = new List<FrameworkUAD.Entity.MasterGroup>();
        List<FrameworkUAD.Entity.Product> allProducts = new List<FrameworkUAD.Entity.Product>();        
        List<FrameworkUAD.Entity.ResponseGroup> allResponseGroup = new List<FrameworkUAD.Entity.ResponseGroup>();
        List<FrameworkUAD.Entity.ReportGroups> allReportGroups = new List<FrameworkUAD.Entity.ReportGroups>();

        FrameworkUAD.Entity.CodeSheet currentCodeSheet = new FrameworkUAD.Entity.CodeSheet();
        FrameworkUAD.Entity.MasterGroup currentMasterGroup = new FrameworkUAD.Entity.MasterGroup();
        FrameworkUAD.Entity.Product currentProduct = new FrameworkUAD.Entity.Product();                        
        FrameworkUAD.Entity.ResponseGroup currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();
        int selectedProductID = 0;
        int selectedResponseGroupID = 0;
        int pageResponseGrpID = 0;
        bool isFromProduct = false;
        bool isFromResponseGroup = false;
        int returnPubID;
        #endregion        

        public class CodeSheetContainer
        {
            public int CodeSheetID { get; set; }
            public int PubID { get; set; }
            public string ResponseGroup { get; set; }
            public string ReportGroupDescription { get; set; }
            public string ResponseValue { get; set; }
            public string ResponseDesc { get; set; }
            public int ResponseGroupID { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime? DateUpdated { get; set; }
            public int CreatedByUserID { get; set; }
            public int? UpdatedByUserID { get; set; }
            public int? DisplayOrder { get; set; }
            public int? ReportGroupID { get; set; }
            public bool? IsActive { get; set; }
            public int? WQT_ResponseID { get; set; }
            public bool? IsOther { get; set; }
            public List<FrameworkUAD.Object.MasterData> MasterData { get; set; }

            public CodeSheetContainer(FrameworkUAD.Entity.CodeSheet cs, List<FrameworkUAD.Object.MasterData> lst, List<FrameworkUAD.Entity.ReportGroups> rgs)
            {
                this.CodeSheetID = cs.CodeSheetID;
                this.PubID = cs.PubID;
                this.ResponseGroup = cs.ResponseGroup;
                this.ResponseValue = cs.ResponseValue;
                this.ResponseDesc = cs.ResponseDesc;
                this.ResponseGroupID = cs.ResponseGroupID;
                this.DateCreated = cs.DateCreated;
                this.DateUpdated = cs.DateUpdated;
                this.CreatedByUserID = cs.CreatedByUserID;
                this.UpdatedByUserID = cs.UpdatedByUserID;
                this.DisplayOrder = cs.DisplayOrder;
                this.ReportGroupID = cs.ReportGroupID;
                this.IsActive = cs.IsActive;
                this.WQT_ResponseID = cs.WQT_ResponseID;
                this.IsOther = cs.IsOther;
                this.MasterData = lst;
                this.ReportGroupDescription = "";
                FrameworkUAD.Entity.ReportGroups rg;
                rg = rgs.Find(x => x.ReportGroupID == cs.ReportGroupID);
                if (rg != null && rg.DisplayName != null)
                {
                    this.ReportGroupDescription = rgs.Find(x => x.ReportGroupID == cs.ReportGroupID).DisplayName;
                }
            }

            public FrameworkUAD.Entity.CodeSheet ReturnCodeSheet()
            {
                FrameworkUAD.Entity.CodeSheet rtnObject = new FrameworkUAD.Entity.CodeSheet();

                rtnObject.CodeSheetID = this.CodeSheetID;
                rtnObject.PubID = this.PubID;
                rtnObject.ResponseGroup = this.ResponseGroup;
                rtnObject.ResponseValue = this.ResponseValue;
                rtnObject.ResponseDesc = this.ResponseDesc;
                rtnObject.ResponseGroupID = this.ResponseGroupID;
                rtnObject.DateCreated = this.DateCreated;
                rtnObject.DateUpdated = this.DateUpdated;
                rtnObject.CreatedByUserID = this.CreatedByUserID;
                rtnObject.UpdatedByUserID = this.UpdatedByUserID;
                rtnObject.DisplayOrder = this.DisplayOrder;
                rtnObject.ReportGroupID = this.ReportGroupID;
                rtnObject.IsActive = this.IsActive;
                rtnObject.WQT_ResponseID = this.WQT_ResponseID;
                rtnObject.IsOther = this.IsOther;

                return rtnObject;
            }
        }

        public CodeSheet(int PubID = 0, int ResponseGroupID = 0)
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            pageResponseGrpID = ResponseGroupID;
            LoadData(PubID, ResponseGroupID);
            cbResponseGroup.SelectedValue = ResponseGroupID;         
            
            if(PubID != 0 && ResponseGroupID == 0)
            {
                isFromProduct = true;
            }
            else if(PubID != 0 && ResponseGroupID != 0)
            {
                isFromResponseGroup = true;
            }
            returnPubID = PubID;
        }

        public void LoadData(int PubID = 0, int ResponseGroupID = 0)
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svProduct = prodWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svResponseGroup = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svMasterGroup = mgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svCodeSheets = csWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svMasterCodeSheets = mcsWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svMasterData = mdWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svReportGroups = rpgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svProduct.Result != null && svProduct.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)                
                    allProducts = svProduct.Result;                
                else                
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svResponseGroup.Result != null && svResponseGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)                
                    allResponseGroup = svResponseGroup.Result;                
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();  
              
                if (svMasterGroup.Result != null && svMasterGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)            
                    allMasterGroup = svMasterGroup.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError(); 

                if (svCodeSheets.Result != null && svCodeSheets.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)                
                    codeSheets = svCodeSheets.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError(); 

                if (svMasterCodeSheets.Result != null && svMasterCodeSheets.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)            
                    allMasterCodeSheets = svMasterCodeSheets.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svMasterData.Result != null && svMasterData.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)                
                    allMasterData = svMasterData.Result;

                if (svReportGroups.Result != null && svReportGroups.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    allReportGroups = svReportGroups.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadPubCombo(PubID);
                if (ResponseGroupID > 0)
                {
                    LoadGroupCombo(PubID, ResponseGroupID);
                }
                LoadTypeCombo();
                LoadReportGroupCombo();
                LoadAvailableList();
                LoadSubGrids(ResponseGroupID, PubID);

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        #region Comboboxes
        private void LoadPubCombo(int PubID)
        {
            cbProduct.ItemsSource = null;                       
            cbProduct.ItemsSource = allProducts.OrderBy(x => x.PubName);
            cbProduct.DisplayMemberPath = "PubName";
            cbProduct.SelectedValuePath = "PubID";
            if (PubID > 0)
            {
                cbProduct.SelectedItem = allProducts.FirstOrDefault(x => x.PubID == PubID);                    
            }                   
        }
        private void LoadGroupCombo(int pubID, int groupID = 0)
        {
            List<FrameworkUAD.Entity.ResponseGroup> responsesTemp = new List<FrameworkUAD.Entity.ResponseGroup>();
            cbResponseGroup.ItemsSource = null;
            responsesTemp = allResponseGroup.Where(x => x.PubID == pubID).ToList();
            cbResponseGroup.ItemsSource = responsesTemp;

            cbResponseGroup.DisplayMemberPath = "DisplayName";
            cbResponseGroup.SelectedValuePath = "ResponseGroupID";

            if(groupID > 0)
                cbResponseGroup.SelectedValue = groupID;   
        }
        private void LoadTypeCombo()
        {
            cbTypes.ItemsSource = null;                        
            cbTypes.ItemsSource = allMasterGroup;
            cbTypes.DisplayMemberPath = "DisplayName";
            cbTypes.SelectedValuePath = "MasterGroupID";  
        }
        private void LoadReportGroupCombo()
        {
            cbReportGroup.ItemsSource = null;
            cbReportGroup.ItemsSource = allReportGroups;
            cbReportGroup.DisplayMemberPath = "DisplayName";
            cbReportGroup.SelectedValuePath = "ReportGroupID";  
        }
        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProduct.SelectedValue != null)
            {
                selectedProductID = 0;
                int.TryParse(cbProduct.SelectedValue.ToString(), out selectedProductID);
                gridResponses.ItemsSource = null;

                if (selectedProductID > 0)
                    LoadGroupCombo(selectedProductID);
                else
                {
                    Core_AMS.Utilities.WPF.Message("Issue with ProductID for the selected Product.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Product Error");
                    return;
                }
            }
        }
        private void cbResponseGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbResponseGroup.SelectedValue != null)
            {
                selectedResponseGroupID = 0;
                int.TryParse(cbResponseGroup.SelectedValue.ToString(), out selectedResponseGroupID);
                LoadSubGrids(selectedResponseGroupID, selectedProductID);

                cbReportGroup.ItemsSource = allReportGroups.Where(x=> x.ResponseGroupID == selectedResponseGroupID);
                //gridResponses.ItemsSource = null;
                //if (selectedProductID > 0 && selectedResponseGroupID > 0)
                //{
                //    List<CodeSheetContainer> csContainer = new List<CodeSheetContainer>();
                //    foreach(FrameworkUAD.Entity.CodeSheet cs in codeSheets.Where(x => x.ResponseGroupID == selectedResponseGroupID && x.PubID == selectedProductID))
                //    {
                //        List<FrameworkUAD.Object.MasterData> distinct = allMasterData.Where(x => x.CodeSheetID == cs.CodeSheetID).ToList();
                //        csContainer.Add(new CodeSheetContainer(cs, distinct));
                //    }
                //    gridResponses.ItemsSource = csContainer;
                //    //gridResponses.ItemsSource = codeSheets.Where(x => x.ResponseGroupID == selectedResponseGroupID && x.PubID == selectedProductID);
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageError("Invalid Product and/or Response Group.");
                //    return;
                //}
            }
        }
        private void cbTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTypes.SelectedValue != null)
            {
                #region Listbox Available Load
                int mGrpID = 0;
                int.TryParse(cbTypes.SelectedValue.ToString(), out mGrpID);
                lbxAvailable.Items.Clear();

                if (mGrpID > 0)
                {
                    List<FrameworkUAD.Entity.MasterCodeSheet> distMCS = allMasterCodeSheets.Where(x => x.MasterGroupID == mGrpID).OrderBy(x => x.MasterValue).ToList();
                    foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMCS)
                    {
                        FrameworkUAD.Entity.MasterGroup mg = allMasterGroup.FirstOrDefault(x => x.MasterGroupID == mGrpID);
                        if (mg != null)
                        {
                            int id = mcs.MasterID;
                            string dn = mg.DisplayName;
                            string mv = mcs.MasterValue;
                            string md = mcs.MasterDesc;
                            Dictionary<int, string> listItem = new Dictionary<int, string>();
                            listItem.Add(id, dn + " - " + mv + " - " + md);
                            if (!lbxSelected.Items.Contains(listItem))
                            {
                                lbxAvailable.Items.Add(listItem);
                                lbxAvailable.SelectedValuePath = "Key";
                                lbxAvailable.DisplayMemberPath = "Value";
                            }
                        }
                    }
                }
                #endregion
                #region Listbox Selected Load
                //if (currentCodeSheet != null)
                //{
                //    List<FrameworkUAD.Object.MasterData> distinct = allMasterData.Where(x => x.CodeSheetID == currentCodeSheet.CodeSheetID).ToList();
                //    foreach (FrameworkUAD.Object.MasterData md in distinct)
                //    {
                //        FrameworkUAD.Entity.MasterGroup selMG = (FrameworkUAD.Entity.MasterGroup)cbTypes.SelectedItem;
                //        if (md.DisplayName == selMG.DisplayName)
                //        {
                //            List<FrameworkUAD.Entity.MasterCodeSheet> distMCS = allMasterCodeSheets.Where(x => x.MasterGroupID == mGrpID && x.MasterID == md.MasterID).ToList();
                //            foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMCS)
                //            {
                //                if (mcs.MasterID == md.MasterID && mcs.MasterDesc == md.MasterDesc && mcs.MasterValue == md.MasterValue)
                //                {
                //                    FrameworkUAD.Entity.MasterGroup mg = allMasterGroup.FirstOrDefault(x => x.MasterGroupID == mGrpID);
                //                    if (mg != null)
                //                    {
                //                        List<MasterDataSelectedItems> items = new List<MasterDataSelectedItems>();
                //                        foreach (MasterDataSelectedItems mdsi in gridSelectedItems.Items)
                //                        {
                //                            items.Add(mdsi);
                //                        }

                //                        if (items.SingleOrDefault(x => x.ID == mcs.MasterID) != null)
                //                        {
                //                            int id = mcs.MasterID;
                //                            string dn = mg.DisplayName;
                //                            string mv = mcs.MasterValue;
                //                            string mcsmd = mcs.MasterDesc;
                //                            Dictionary<int, string> listItem = new Dictionary<int, string>();
                //                            listItem.Add(id, dn + " - " + mv + " - " + mcsmd);
                //                            lbxSelected.Items.Add(listItem);
                //                            lbxSelected.SelectedValuePath = "Key";
                //                            lbxSelected.DisplayMemberPath = "Value";
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
            }
        }

        private void LoadSubGrids(int responseGroupID, int pubID)
        {
            gridResponses.ItemsSource = null;
            if (responseGroupID > 0 && pubID > 0)
            {
                List<CodeSheetContainer> csContainer = new List<CodeSheetContainer>();
                foreach (FrameworkUAD.Entity.CodeSheet cs in codeSheets.Where(x => x.ResponseGroupID == responseGroupID && x.PubID == pubID))
                {
                    List<FrameworkUAD.Object.MasterData> distinct = allMasterData.Where(x => x.CodeSheetID == cs.CodeSheetID).ToList();
                    csContainer.Add(new CodeSheetContainer(cs, distinct, allReportGroups));
                }
                gridResponses.ItemsSource = csContainer;
            }
        }
        #endregion

        #region Listboxes
        public void LoadAvailableList()
        {                        
            if (cbTypes.SelectedValue != null)
            {
                int mGrpID = 0;
                int.TryParse(cbTypes.SelectedValue.ToString(), out mGrpID);

                if (mGrpID > 0)
                {
                    List<FrameworkUAD.Entity.MasterCodeSheet> distMCS = allMasterCodeSheets.Where(x => x.MasterGroupID == mGrpID).OrderBy(x => x.MasterValue).ToList();
                    foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMCS)
                    {
                        FrameworkUAD.Entity.MasterGroup mg = allMasterGroup.FirstOrDefault(x => x.MasterGroupID == mGrpID);
                        if (mg != null)
                        {
                            int id = mcs.MasterID;
                            string dn = mg.DisplayName;
                            string mv = mcs.MasterValue;
                            string md = mcs.MasterDesc;
                            Dictionary<int, string> listItem = new Dictionary<int, string>();
                            listItem.Add(id, dn + " - " + mv + " - " + md);
                            lbxAvailable.Items.Add(listItem);
                            lbxAvailable.SelectedValuePath = "Key";
                            lbxAvailable.DisplayMemberPath = "Value";
                        }
                    }
                }
            }            
        }
        #endregion

        #region Button Clicks
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Variable Initialize 
            int CodeSheetID = 0;
            if (btnSave.Tag != null)
            {
                int.TryParse(btnSave.Tag.ToString(), out CodeSheetID);
            }
            List<string> uniqueNames = new List<string>();
            List<System.ComponentModel.ListSortDirection> sortingStates = new List<System.ComponentModel.ListSortDirection>();
            foreach (Telerik.Windows.Controls.GridView.ColumnSortDescriptor sd in gridResponses.SortDescriptors)
            {
                uniqueNames.Add(sd.Column.UniqueName);
                sortingStates.Add(sd.SortDirection);
            }
            int PubID = 0;
            int RespGrpID = 0;
            string RespGrp = "";
            string RespValue = "";
            string RespDesc = "";
            int display = 0;
            bool active = true;
            bool other = false;
            int repGrpID = 0;
            #endregion

            #region Checks
            int.TryParse(tbxDisplay.Text, out display);            

            if (cbxActive.IsChecked == true)
                active = true;              
            else
                active = false;

            if (cbxOther.IsChecked == true)
                other = true;              
            else
                other = false;

            if (cbReportGroup.SelectedValue != null)
                int.TryParse(cbReportGroup.SelectedValue.ToString(), out repGrpID);
            #endregion

            #region Data Empty Checks
            #region Product Check
            if (cbProduct.SelectedValue != null)
                int.TryParse(cbProduct.SelectedValue.ToString(), out PubID);

            if (!(PubID > 0))
            {
                Core_AMS.Utilities.WPF.Message("Please select a Product.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            #endregion
            #region Response Group Check
            if (cbResponseGroup.SelectedValue != null)
                int.TryParse(cbResponseGroup.SelectedValue.ToString(), out RespGrpID);
            
            if (!(RespGrpID > 0))
            {
                Core_AMS.Utilities.WPF.Message("Please select a Response Group.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            #endregion
            #region Type Check
            if (cbResponseGroup.SelectedValue != null)
                RespGrp = allResponseGroup.Single(x => x.ResponseGroupID == RespGrpID).ResponseGroupName;//cbResponseGroup.Text.ToString();

            //if (string.IsNullOrEmpty(RespGrp))
            //{
            //    Core_AMS.Utilities.WPF.Message("Please select a Type.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
            //    return;
            //}
            #endregion
            #region Value Check
            if (string.IsNullOrEmpty(tbxValue.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
            {
                RespValue = tbxValue.Text;
            }
            #endregion
            #region Description Check
            if (string.IsNullOrEmpty(tbxDesc.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Description.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
            {
                RespDesc = tbxDesc.Text;
            }
            #endregion            
            #endregion

            #region Check Duplicate Entry
            if (CodeSheetID > 0)
            {
                //Update. Gets existing checks if RespValue or RespGroupID changed if so checks if values already exist
                FrameworkUAD.Entity.CodeSheet soloCS = codeSheets.FirstOrDefault(x => x.CodeSheetID == CodeSheetID);
                if (soloCS != null)
                {
                    if (soloCS.ResponseValue != RespValue || soloCS.ResponseGroupID != RespGrpID)
                    {
                        if (codeSheets.FirstOrDefault(x => x.ResponseValue == RespValue && x.ResponseGroupID == RespGrpID) != null)
                        {
                            Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                            return;
                        }
                    }
                }
            }
            else
            {
                //Insert checks for duplicate entry
                if (codeSheets.FirstOrDefault(x => x.ResponseValue == RespValue && x.ResponseGroupID == RespGrpID) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                    return;
                }
            }
            #endregion
            
            #region Save CodeSheet
            FrameworkUAD.Entity.CodeSheet csEntry = new FrameworkUAD.Entity.CodeSheet();
            csEntry.CodeSheetID = CodeSheetID;
            csEntry.PubID = PubID;
            csEntry.ResponseGroupID = RespGrpID;
            csEntry.ResponseGroup = RespGrp;
            csEntry.ResponseValue = RespValue;
            csEntry.ResponseDesc = RespDesc;
            csEntry.DisplayOrder = display;
            csEntry.IsActive = active;
            csEntry.IsOther = other;
            csEntry.ReportGroupID = repGrpID;

            FrameworkUAS.Service.Response<int> svIntCS = csWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, csEntry, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svIntCS.Result != null && svIntCS.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                CodeSheetID = svIntCS.Result;
                btnSave.Tag = CodeSheetID.ToString();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                return;
            }
            #endregion

            #region Delete CodeSheetMasterCodeSheetBridge
            svBoolCSMSSB = csmcsbWorker.Proxy.DeleteCodeSheetID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, CodeSheetID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if ((svBoolCSMSSB.Result == true || svBoolCSMSSB.Result == false) && svBoolCSMSSB.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                //string pass;
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                return;
            }
            #endregion

            #region Save CodeSheetMasterCodeSheetBridge
            List<MasterDataSelectedItems> mdsiList = new List<MasterDataSelectedItems>();
            foreach (MasterDataSelectedItems mdsi in gridSelectedItems.Items)
            {
                int MasterID = mdsi.ID;
                if (MasterID > 0)
                {
                    FrameworkUAD.Entity.CodeSheetMasterCodeSheetBridge csmcsbEntry = new FrameworkUAD.Entity.CodeSheetMasterCodeSheetBridge();
                    csmcsbEntry.CodeSheetID = CodeSheetID;
                    csmcsbEntry.MasterID = MasterID;

                    FrameworkUAS.Service.Response<int> svIntCSMCSB = csmcsbWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, csmcsbEntry, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (svIntCSMCSB.Result != null && svIntCSMCSB.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        //CodeSheetID = svIntCSMCSB.Result;
                    }
                    else
                    {
                        mdsiList.Add(mdsi);
                    }
                }
                else
                {
                    mdsiList.Add(mdsi);
                }
            }
            if (mdsiList.Count > 0)
            {
                Core_AMS.Utilities.WPF.Message("Issue saving CodeSheet values.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error Saving");
                return;
            }
            #endregion

            #region Reload Page
            currentCodeSheet = null;
            tbxValue.Text = "";
            tbxDesc.Text = "";
            lbxSelected.Items.Clear();
            cbReportGroup.SelectedItem = null;
            gridSelectedItems.ItemsSource = null;
            btnSave.Tag = "0";
            btnSave.Content = "Save";
            #region Main Grid Reload
            gridResponses.ItemsSource = null;
            svMasterData = mdWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svMasterData.Result != null && svMasterData.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                allMasterData = svMasterData.Result;
            svCodeSheets = csWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svCodeSheets.Result != null && svCodeSheets.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                codeSheets = svCodeSheets.Result;
                if (selectedProductID > 0 && selectedResponseGroupID > 0)
                {
                    gridResponses.ItemsSource = codeSheets.Where(x => x.ResponseGroupID == selectedResponseGroupID && x.PubID == selectedProductID);
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("Invalid Product and/or Response Group.");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageServiceError();
                return;
            }
            //LoadGroupCombo(PubID, RespGrpID);
            LoadSubGrids(RespGrpID, PubID);
            for (int i = 0; i < uniqueNames.Count; i++)
            {
                Telerik.Windows.Data.SortDescriptor descriptor = new Telerik.Windows.Data.SortDescriptor();
                descriptor.Member = uniqueNames[i];
                descriptor.SortDirection = sortingStates[i];
                gridResponses.SortDescriptors.Add(descriptor);
            }
            Core_AMS.Utilities.WPF.Message("Code Sheet saved successfully.", "Save Complete");
            #endregion           
            #endregion
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            currentCodeSheet = null;
            tbxValue.Text = "";
            tbxDesc.Text = "";
            lbxSelected.Items.Clear();         
            gridSelectedItems.ItemsSource = null;
            btnSave.Tag = "0";
            btnSave.Content = "Save";

            if(isFromProduct)
            {
                
                List<DockPanel> dps = Application.Current.MainWindow.ChildrenOfType<DockPanel>().ToList();
                foreach (DockPanel dp in dps)
                {
                    if (dp.Name == "spModule")
                    {
                        List<DockPanel> dpControls = dp.ChildrenOfType<DockPanel>().ToList();
                        foreach (DockPanel sdp in dpControls)
                        {
                            if (sdp.Name == "spControls")
                            {
                                sdp.Children.Clear();
                                ControlCenter.Controls.ProductCreation pc = new ProductCreation();
                                sdp.Children.Add(pc);
                            }
                        }
                    }
                }
            }
            else if (isFromResponseGroup)
            {

                List<DockPanel> dps = Application.Current.MainWindow.ChildrenOfType<DockPanel>().ToList();
                foreach (DockPanel dp in dps)
                {
                    if (dp.Name == "spModule")
                    {
                        List<DockPanel> dpControls = dp.ChildrenOfType<DockPanel>().ToList();
                        foreach (DockPanel sdp in dpControls)
                        {
                            if (sdp.Name == "spControls")
                            {
                                sdp.Children.Clear();
                                ControlCenter.Controls.ResponseGroups rg = new ResponseGroups(returnPubID);
                                sdp.Children.Add(rg);
                            }
                        }
                    }
                }
            }
        }
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAvailable.SelectedItems.Count > 0)
            {
                List<MasterDataSelectedItems> mdsiList = new List<MasterDataSelectedItems>();
                foreach(MasterDataSelectedItems m in gridSelectedItems.Items)
                {
                    mdsiList.Add(m);
                }
                foreach(Dictionary<int, string> d in lbxAvailable.SelectedItems)
                {                    
                    int masterID = 0;
                    int.TryParse(d.FirstOrDefault().Key.ToString(), out masterID);
                    if (masterID > 0)
                    {
                        List<FrameworkUAD.Entity.MasterCodeSheet> distMCS = allMasterCodeSheets.Where(x => x.MasterID == masterID).ToList();
                        foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMCS)
                        {
                            FrameworkUAD.Entity.MasterGroup mg = allMasterGroup.FirstOrDefault(x => x.MasterGroupID == mcs.MasterGroupID);
                            if (mg != null)
                            {
                                int id = mcs.MasterID;
                                string dn = mg.DisplayName;
                                string mv = mcs.MasterValue;
                                string mcsmd = mcs.MasterDesc;                                                                

                                MasterDataSelectedItems mdsi = new MasterDataSelectedItems();
                                mdsi.ID = id;
                                mdsi.Type = dn;
                                mdsi.Entries = dn + " - " + mv + " - " + mcsmd;
                                if (mdsiList.FirstOrDefault(x => x.ID == id && x.Type == dn && x.Entries == mdsi.Entries) == null)
                                {
                                    mdsiList.Add(mdsi);

                                    Dictionary<int, string> listItem = new Dictionary<int, string>();
                                    listItem.Add(id, dn + " - " + mv + " - " + mcsmd);
                                    lbxSelected.Items.Add(listItem);
                                    lbxSelected.SelectedValuePath = "Key";
                                    lbxSelected.DisplayMemberPath = "Value";
                                }
                            }                                    
                        }                            
                    }                                            
                }
                gridSelectedItems.ItemsSource = mdsiList;
            }
        }
        
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //if (lbxAvailable.SelectedItems.Count > 0)
            //{
            //    List<int> indexes = new List<int>();
            //    List<MasterDataSelectedItems> mdsiList = new List<MasterDataSelectedItems>();
            //    foreach (MasterDataSelectedItems m in gridSelectedItems.Items)
            //    {
            //        mdsiList.Add(m);
            //    }
            //    foreach (Dictionary<int, string> d in lbxSelected.SelectedItems)
            //    {                    
            //        int masterID = 0;
            //        int.TryParse(d.FirstOrDefault().Key.ToString(), out masterID);
            //        if (masterID > 0)
            //        {                        
            //            List<FrameworkUAD.Entity.MasterCodeSheet> distMCS = allMasterCodeSheets.Where(x => x.MasterID == masterID).ToList();
            //            foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMCS)
            //            {
            //                FrameworkUAD.Entity.MasterGroup mg = allMasterGroup.FirstOrDefault(x => x.MasterGroupID == mcs.MasterGroupID);
            //                if (mg != null)
            //                {
            //                    int id = mcs.MasterID;
            //                    string dn = mg.DisplayName;
            //                    string mv = mcs.MasterValue;
            //                    string mcsmd = mcs.MasterDesc;

            //                    MasterDataSelectedItems mdsi = new MasterDataSelectedItems();
            //                    mdsi.ID = id;
            //                    mdsi.Type = dn;
            //                    mdsi.Entries = dn + " - " + mv + " - " + mcsmd;
            //                    if (mdsiList.FirstOrDefault(x => x.ID == id && x.Type == dn && x.Entries == mdsi.Entries) != null)
            //                    {
            //                        mdsiList.Remove(mdsi);                                    
            //                    }
            //                }
            //            }
            //        }
            //        indexes.Add(lbxSelected.Items.IndexOf(d));                    
            //    }
            //    gridSelectedItems.ItemsSource = mdsiList;
            //    foreach(int i in indexes)
            //    {
            //        lbxSelected.Items.RemoveAt(i);
            //    }
            //}
        }
        
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(CodeSheetContainer))
                {
                    CodeSheetContainer cs = b.DataContext as CodeSheetContainer;
                    FrameworkUAD.Entity.CodeSheet csItem = cs.ReturnCodeSheet();
                    //FrameworkUAD.Entity.CodeSheet csItem = (FrameworkUAD.Entity.CodeSheet)b.DataContext;
                    if (csItem != null)
                    {                        
                        currentCodeSheet = csItem;
                        tbxValue.Text = csItem.ResponseValue;
                        tbxDesc.Text = csItem.ResponseDesc;

                        if (csItem.DisplayOrder != null)
                            tbxDisplay.Text = csItem.DisplayOrder.ToString();

                        if (csItem.IsActive != null && csItem.IsActive == true)
                            cbxActive.IsChecked = true;
                        else
                            cbxActive.IsChecked = false;

                        if (csItem.IsOther != null && csItem.IsOther == true)
                            cbxOther.IsChecked = true;
                        else
                            cbxOther.IsChecked = false;

                        if (csItem.ReportGroupID != null)
                        {
                            cbReportGroup.SelectedValue = csItem.ReportGroupID.ToString();
                        }

                        #region Load Selected Listbox

                        if (currentCodeSheet != null)
                        {
                            lbxSelected.Items.Clear();
                            List<FrameworkUAD.Object.MasterData> distinct = allMasterData.Where(x => x.CodeSheetID == currentCodeSheet.CodeSheetID).ToList();
                            foreach (FrameworkUAD.Object.MasterData md in distinct)
                            {
                                FrameworkUAD.Entity.MasterGroup selMG = (FrameworkUAD.Entity.MasterGroup)cbTypes.SelectedItem;
                                List<FrameworkUAD.Entity.MasterCodeSheet> distMCS = allMasterCodeSheets.Where(x => x.MasterID == md.MasterID).ToList();
                                foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMCS)
                                {
                                    if (mcs.MasterID == md.MasterID && mcs.MasterDesc == md.MasterDesc && mcs.MasterValue == md.MasterValue)
                                    {
                                        FrameworkUAD.Entity.MasterGroup mg = allMasterGroup.FirstOrDefault(x => x.MasterGroupID == mcs.MasterGroupID);
                                        if (1 == 1)
                                        {
                                            int id = mcs.MasterID;
                                            string dn = mg.DisplayName;
                                            string mv = mcs.MasterValue;
                                            string mcsmd = mcs.MasterDesc;
                                            Dictionary<int, string> listItem = new Dictionary<int, string>();
                                            listItem.Add(id, dn + " - " + mv + " - " + mcsmd);
                                            lbxSelected.Items.Add(listItem);
                                            lbxSelected.SelectedValuePath = "Key";
                                            lbxSelected.DisplayMemberPath = "Value";
                                        }
                                    }
                                }      
                            }                                
                        }
       
                        #endregion
                        #region Load Selected Rad Grid
                        List<MasterDataSelectedItems> mdsiList = new List<MasterDataSelectedItems>();
                        List<FrameworkUAD.Object.MasterData> selDist = allMasterData.Where(x => x.CodeSheetID == currentCodeSheet.CodeSheetID).ToList();
                        foreach (FrameworkUAD.Object.MasterData md in selDist)
                        {
                            List<FrameworkUAD.Entity.MasterCodeSheet> distMCS = allMasterCodeSheets.Where(x => x.MasterID == md.MasterID).ToList();
                            foreach (FrameworkUAD.Entity.MasterCodeSheet mcs in distMCS)
                            {
                                if (mcs.MasterID == md.MasterID && mcs.MasterDesc == md.MasterDesc && mcs.MasterValue == md.MasterValue)
                                {
                                    FrameworkUAD.Entity.MasterGroup mg = allMasterGroup.FirstOrDefault(x => x.MasterGroupID == mcs.MasterGroupID);
                                    if (mg != null)
                                    {
                                        int id = mcs.MasterID;
                                        string dn = mg.DisplayName;
                                        string mv = mcs.MasterValue;
                                        string mcsmd = mcs.MasterDesc;
                                        //listItem.Add(id, dn + " - " + mv + " - " + mcsmd);

                                        MasterDataSelectedItems mdsi = new MasterDataSelectedItems();
                                        mdsi.ID = id;
                                        mdsi.Type = dn;
                                        mdsi.Entries = dn + " - " + mv + " - " + mcsmd;
                                        mdsiList.Add(mdsi);
                                    }
                                }
                            }
                        }
                        gridSelectedItems.ItemsSource = mdsiList;
                        #endregion

                        btnSave.Tag = csItem.CodeSheetID.ToString();                        
                        btnSave.Content = "Update";
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS10.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS9.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to delete this response?", "Warning", MessageBoxButton.YesNo);

            if (areYouSureDelete == MessageBoxResult.Yes)
            {
                if (e.OriginalSource.GetType() == typeof(Button))
                {
                    Button b = (Button)e.OriginalSource;
                    if (b.DataContext.GetType() == typeof(CodeSheetContainer))
                    {
                        CodeSheetContainer cs = b.DataContext as CodeSheetContainer;
                        FrameworkUAD.Entity.CodeSheet csItem = cs.ReturnCodeSheet();
                        //FrameworkUAD.Entity.CodeSheet csItem = (FrameworkUAD.Entity.CodeSheet)b.DataContext;
                        if (csItem != null)
                        {
                            currentCodeSheet = csItem;
                            //Delete CodeSheet_MasterCodeSheet_Bridge on CodeSheetID
                            svBoolCSMSSB = csmcsbWorker.Proxy.DeleteCodeSheetID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, csItem.CodeSheetID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            //Delete PubSubscriptionDetail on CodeSheetID
                            svBoolPSD = psWorker.Proxy.DeleteCodeSheetID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, csItem.CodeSheetID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            //Delete CodeSheet on CodeSheetID
                            svBoolCS = csWorker.Proxy.DeleteCodeSheetID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, csItem.CodeSheetID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if ((svBoolCSMSSB.Result == true || svBoolCSMSSB.Result == false) && svBoolCSMSSB.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success
                                && (svBoolPSD.Result == true || svBoolPSD.Result == false) && svBoolPSD.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success
                                && (svBoolCS.Result == true || svBoolCS.Result == false) && svBoolCS.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            {
                                if (svBoolCS.Result == true)
                                {
                                    Core_AMS.Utilities.WPF.MessageDeleteComplete();
                                    currentCodeSheet = null;
                                    tbxValue.Text = "";
                                    tbxDesc.Text = "";
                                    lbxSelected.Items.Clear();
                                    gridSelectedItems.ItemsSource = null;
                                    btnSave.Tag = "0";
                                    btnSave.Content = "Save";
                                    LoadData(selectedProductID, selectedResponseGroupID);
                                }
                                else
                                {
                                    Core_AMS.Utilities.WPF.Message("There was an error deleting the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Deletion Error");
                                    return;
                                }
                            }
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
        
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(MasterDataSelectedItems))
                {
                    MasterDataSelectedItems csItem = (MasterDataSelectedItems)b.DataContext;
                    if (csItem != null)
                    {                        
                        //Remove selected item gridSelectedItems                        
                        List<MasterDataSelectedItems> items = new List<MasterDataSelectedItems>();
                        foreach (MasterDataSelectedItems mdsi in gridSelectedItems.Items)
                        {
                            items.Add(mdsi);
                        }
                        items.Remove(csItem);
                        gridSelectedItems.ItemsSource = null;
                        gridSelectedItems.ItemsSource = items;
                        //Remove select list item add back to available  
                        if (lbxSelected.Items.Count > 0)
                        {
                            Dictionary<int, object> removeItems = new Dictionary<int, object>();
                            foreach (Dictionary<int, string> kvp in lbxSelected.Items)
                            {
                               int index = lbxSelected.Items.IndexOf(kvp);
                               if (kvp.Keys.Contains(csItem.ID))
                               {
                                   object kvp2 = kvp.FirstOrDefault(x => x.Key == csItem.ID);
                                   removeItems.Add(index, kvp2);
                               }
                            }
                            foreach (KeyValuePair<int, object> kvp in removeItems)
                            {
                                lbxSelected.Items.RemoveAt(kvp.Key);                                
                            }
                        }
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS10.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS9.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }
        #endregion        
        
        private void gridMasterData_Loaded(object sender, RoutedEventArgs e)
        {
            //if (sender.GetType() == typeof(Telerik.Windows.Controls.RadGridView))
            //{
            //    Telerik.Windows.Controls.RadGridView rgv = (Telerik.Windows.Controls.RadGridView)sender;
            //    if (rgv.ParentRow.DataContext.GetType() == typeof(FrameworkUAD.Entity.CodeSheet))
            //    {
            //        FrameworkUAD.Entity.CodeSheet cs = (FrameworkUAD.Entity.CodeSheet)rgv.ParentRow.DataContext;
            //        if (cs != null)
            //        {
            //            if (cs.CodeSheetID > 0)
            //            {
            //                List<FrameworkUAD.Object.MasterData> distinct = allMasterData.Where(x => x.CodeSheetID == cs.CodeSheetID).ToList();
            //                if(cs.CodeSheetID == 13604)                            
            //                {
                                
            //                }
            //                rgv.ItemsSource = distinct;
            //                rgv.Rebind();
            //            }
            //        }
            //    }
            //}
        }
    }

    public class MasterDataSelectedItems
    {
        public MasterDataSelectedItems() { }
        public int ID { get; set; }
        public string Type { get; set; }
        public string Entries { get; set; }
    }
}
