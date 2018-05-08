using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using Telerik.Windows.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using ControlCenter.Implementation;
using ControlCenter.Interfaces;
using FrameworkUAD.Object;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for ProductCreation.xaml
    /// </summary>
    public partial class ProductCreation : UserControl
    {
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProductGroup> pgWorker = FrameworkServices.ServiceClient.UAD_ProductGroupClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProductTypes> ptWorker = FrameworkServices.ServiceClient.UAD_ProductTypesClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IFrequency> fWorker = FrameworkServices.ServiceClient.UAD_FrequencyClient();

        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProducts = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductGroup>> svProductGroups = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>> svProductTypes = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Frequency>> svFrequency = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Frequency>>();

        FrameworkUAS.Service.Response<int> svIntP = new FrameworkUAS.Service.Response<int>();
        FrameworkUAS.Service.Response<int> svIntPG = new FrameworkUAS.Service.Response<int>();
        FrameworkUAS.Service.Response<int> svIntPT = new FrameworkUAS.Service.Response<int>();

        List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        List<FrameworkUAD.Entity.ProductGroup> productGroups = new List<FrameworkUAD.Entity.ProductGroup>();
        List<FrameworkUAD.Entity.ProductTypes> productTypes = new List<FrameworkUAD.Entity.ProductTypes>();
        List<FrameworkUAD.Entity.Frequency> allFrequency = new List<FrameworkUAD.Entity.Frequency>();

        FrameworkUAD.Entity.Product currentProduct = new FrameworkUAD.Entity.Product();
        Dictionary<int, string> allBaseChannels = new Dictionary<int, string>();
        Dictionary<int, string> allCustomers = new Dictionary<int, string>();
        ObservableCollection<MyGroup> allGroups = new ObservableCollection<MyGroup>();
        ObservableCollection<MyGroup> selectedGroups = new ObservableCollection<MyGroup>();
        ObservableCollection<MyGroup> currGroups = new ObservableCollection<MyGroup>();

        private readonly IAppSettingsProvider _appSettingsprovider;

        private const int MaxItemsToAdd = 25;
        public class MyGroup
        {
            public int GroupID { get; set; }
            public string GroupDisplayName { get; set; }
            public string GroupName { get; set; }

            public MyGroup(int id, string display, string name)
            {
                this.GroupID = id;
                this.GroupDisplayName = display;
                this.GroupName = name;
            }
        }

        public class ProductCreationContainer
        {
            public int PubID { get; set; }
            public string PubName { get; set; }
            public bool istradeshow { get; set; }
            public string PubCode { get; set; }
            public int PubTypeID { get; set; }
            public int GroupID { get; set; }
            public bool EnableSearching { get; set; }
            public int score { get; set; }
            public int SortOrder { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime? DateUpdated { get; set; }
            public int CreatedByUserID { get; set; }
            public int? UpdatedByUserID { get; set; }
            public int ClientID { get; set; }
            public string YearStartDate { get; set; }
            public string YearEndDate { get; set; }
            public DateTime? IssueDate { get; set; }
            public bool? IsImported { get; set; }
            public bool IsActive { get; set; }
            public bool AllowDataEntry { get; set; }
            public string Frequency { get; set; }
            public int? FrequencyID { get; set; }
            public bool? KMImportAllowed { get; set; }
            public bool? ClientImportAllowed { get; set; }
            public bool? AddRemoveAllowed { get; set; }
            public int AcsMailerInfoId { get; set; }
            public bool? IsUAD { get; set; }
            public bool? IsCirc { get; set; }
            public bool? IsOpenCloseLocked { get; set; }
            public bool HasPaidRecords { get; set; }
            public bool UseSubGen { get; set; }
            public string Type { get; set; }

            public ProductCreationContainer(FrameworkUAD.Entity.Product p, List<FrameworkUAD.Entity.Frequency> lf, List<FrameworkUAD.Entity.ProductTypes> lpt)
            {
                this.PubID = p.PubID;
                this.PubName = p.PubName;
                this.istradeshow = p.istradeshow;
                this.PubCode = p.PubCode;
                this.PubTypeID = p.PubTypeID;
                this.GroupID = p.GroupID;
                this.EnableSearching = p.EnableSearching;
                this.score = p.score;
                this.SortOrder = p.SortOrder;
                this.DateCreated = p.DateCreated;
                this.DateUpdated = p.DateUpdated;
                this.CreatedByUserID = p.CreatedByUserID;
                this.UpdatedByUserID = p.UpdatedByUserID;
                this.ClientID = p.ClientID;
                this.YearStartDate = p.YearStartDate;
                this.YearEndDate = p.YearEndDate;
                this.IssueDate = p.IssueDate;
                this.IsImported = p.IsImported;
                this.IsActive = p.IsActive;
                this.AllowDataEntry = p.AllowDataEntry;
                this.Frequency = "";
                FrameworkUAD.Entity.Frequency f;
                f = lf.Find(x => x.FrequencyID == p.FrequencyID);
                if (f != null && f.FrequencyName != null)
                {
                    this.Frequency = f.FrequencyName;
                }
                this.FrequencyID = p.FrequencyID;
                this.KMImportAllowed = p.KMImportAllowed;
                this.ClientImportAllowed = p.ClientImportAllowed;
                this.AddRemoveAllowed = p.AddRemoveAllowed;
                this.AcsMailerInfoId = p.AcsMailerInfoId;
                this.IsUAD = p.IsUAD;
                this.IsCirc = p.IsCirc;
                this.IsOpenCloseLocked = p.IsOpenCloseLocked;
                this.HasPaidRecords = p.HasPaidRecords;
                this.UseSubGen = p.UseSubGen;
                this.Type = "";
                FrameworkUAD.Entity.ProductTypes pt;
                pt = lpt.Find(x => x.PubTypeID == p.PubTypeID);
                if (pt != null && pt.PubTypeDisplayName != null)
                {
                    this.Type = pt.PubTypeDisplayName;
                }
            }
            public FrameworkUAD.Entity.Product ReturnProduct()
            {
                FrameworkUAD.Entity.Product rtnProduct = new FrameworkUAD.Entity.Product();
                rtnProduct.PubID = this.PubID;
                rtnProduct.PubName = this.PubName;
                rtnProduct.istradeshow = this.istradeshow;
                rtnProduct.PubCode = this.PubCode;
                rtnProduct.PubTypeID = this.PubTypeID;
                rtnProduct.GroupID = this.GroupID;
                rtnProduct.EnableSearching = this.EnableSearching;
                rtnProduct.score = this.score;
                rtnProduct.SortOrder = this.SortOrder;
                rtnProduct.DateCreated = this.DateCreated;
                rtnProduct.DateUpdated = this.DateUpdated;
                rtnProduct.CreatedByUserID = this.CreatedByUserID;
                rtnProduct.UpdatedByUserID = this.UpdatedByUserID;
                rtnProduct.ClientID = this.ClientID;
                rtnProduct.YearStartDate = this.YearStartDate;
                rtnProduct.YearEndDate = this.YearEndDate;
                rtnProduct.IssueDate = this.IssueDate;
                rtnProduct.IsImported = this.IsImported;
                rtnProduct.IsActive = this.IsActive;
                rtnProduct.AllowDataEntry = this.AllowDataEntry;
                rtnProduct.FrequencyID = this.FrequencyID;
                rtnProduct.KMImportAllowed = this.KMImportAllowed;
                rtnProduct.ClientImportAllowed = this.ClientImportAllowed;
                rtnProduct.AddRemoveAllowed = this.AddRemoveAllowed;
                rtnProduct.AcsMailerInfoId = this.AcsMailerInfoId;
                rtnProduct.IsUAD = this.IsUAD;
                rtnProduct.IsCirc = this.IsCirc;
                rtnProduct.IsOpenCloseLocked = this.IsOpenCloseLocked;
                rtnProduct.HasPaidRecords = this.HasPaidRecords;
                rtnProduct.UseSubGen = this.UseSubGen;
                return rtnProduct;
            }
        }

        public ProductCreation()
        {
            _appSettingsprovider = new ConfigurationManagerSettingsProvider();

            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();
            LoadData();
            dtpYearStart.Culture = new System.Globalization.CultureInfo("en-US");
            dtpYearStart.Culture.DateTimeFormat.ShortDatePattern = "MM/dd";
            dtpYearEnd.Culture = new System.Globalization.CultureInfo("en-US");
            dtpYearEnd.Culture.DateTimeFormat.ShortDatePattern = "MM/dd";
        }

        public ProductCreation(IAppSettingsProvider appSettingsProvider) : this()
        {
            _appSettingsprovider = appSettingsProvider;
        }

        public void LoadData()
        {
            busy.IsBusy = true;
            var bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svProducts = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svProductGroups = pgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svProductTypes = ptWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svFrequency = fWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svProducts.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    products = svProducts.Result.OrderBy(x => x.PubName).ToList();
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }

                if (svProductGroups.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    productGroups = svProductGroups.Result;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }

                if (svProductTypes.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    productTypes = svProductTypes.Result;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }

                if (svFrequency.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success) { allFrequency = svFrequency.Result; }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }

                LoadProducts();
                LoadCombos();
                LoadBaseChannels();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        public void LoadProducts()
        {
            gridGroups.ItemsSource = null;
            var pcContainer = new List<ProductCreationContainer>();
            foreach (FrameworkUAD.Entity.Product p in products.OrderBy(x => x.PubName))
            {
                pcContainer.Add(new ProductCreationContainer(p, allFrequency, productTypes));
            }
            gridGroups.ItemsSource = pcContainer;
        }

        public void LoadCombos()
        {
            // Search
            cbSearch.Items.Clear();
            foreach (var tf in (Core_AMS.Utilities.Enums.YesNo[])Enum.GetValues(typeof(Core_AMS.Utilities.Enums.YesNo)))
            {
                cbSearch.Items.Add(tf.ToString().Replace("_", " "));
                if (tf.Equals(Core_AMS.Utilities.Enums.YesNo.Yes))
                {
                    cbSearch.SelectedIndex = cbSearch.Items.Count - 1;
                }
            }

            // Score
            cbScore.Items.Clear();
            for (var i = 1; i <= MaxItemsToAdd; i++)
            {
                cbScore.Items.Add(i.ToString());
                if (i.ToString() == "1")
                {
                    cbScore.SelectedIndex = cbScore.Items.Count - 1;
                }
            }

            // Types
            cbType.ItemsSource = null;
            cbType.ItemsSource = productTypes;
            cbType.SelectedValuePath = "PubTypeID";
            cbType.DisplayMemberPath = "PubTypeDisplayName";

            // Frequency
            cbFrequency.ItemsSource = null;
            cbFrequency.ItemsSource = allFrequency;
            cbFrequency.SelectedValuePath = "FrequencyID";
            cbFrequency.DisplayMemberPath = "FrequencyName";
        }

        public void LoadBaseChannels()
        {
            //Call BaseChannel Code            
            allBaseChannels = new Dictionary<int, string>();

            var listOfBaseChannelsAsXmlString = GetListOfBaseChannelsAsXmlString();

            if (!string.IsNullOrEmpty(listOfBaseChannelsAsXmlString))
            {
                const string errorMessage = "An unexpected error occured loading base channels.";
                //Convert string to Xml grab all basechannels
                var processingResult = ProcessXmlForTagName(listOfBaseChannelsAsXmlString, "BaseChannel", errorMessage,
                    nameof(LoadBaseChannels));
                if (processingResult) { return; }

                cbBase.ItemsSource = null;
                cbBase.ItemsSource = allBaseChannels.OrderBy(x => x.Value);
                cbBase.SelectedValuePath = "Key";
                cbBase.DisplayMemberPath = "Value";

                cbBase.SelectedIndex = 0;
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please reload the page. If the problem persists please contact Customer Support.");
                return;
            }
        }

        private void cbBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbBase.SelectedValue == null) return;
            int baseChannelId;
            int.TryParse(cbBase.SelectedValue.ToString(), out baseChannelId);
            var customers = GetListOfCustomersAsXmlStringBasedOnCustomerId(baseChannelId);

            allCustomers = new Dictionary<int, string>();
            if (!string.IsNullOrEmpty(customers))
            {
                const string errorMessage = "An unexpected error occured loading customers.";
                //Convert string to Xml grab all basechannels
                var processingResult =
                    ProcessXmlForTagName(customers, "Customer", errorMessage, nameof(cbBase_SelectionChanged));
                if (processingResult) { return; }

                cbCustomers.ItemsSource = null;
                cbCustomers.ItemsSource = allCustomers;
                cbCustomers.SelectedValuePath = "Key";
                cbCustomers.DisplayMemberPath = "Value";
                cbCustomers.SelectedIndex = 0;
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please reload the page. If the problem persists please contact Customer Support.");
                return;
            }
        }

        private void cbCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCustomers.SelectedValue != null)
            {
                int customerId;
                int.TryParse(cbCustomers.SelectedValue.ToString(), out customerId);
                var list = GetInternalListsBasedOnCustomerId(customerId);
                allGroups.Clear();
                if (!string.IsNullOrEmpty(list))
                {
                    //Convert string to Xml grab all basechannels
                    if (ProcessXmlForTagNameForGroups(list, "Group", ref allGroups)) return;

                    lbxAvailableGroups.ItemsSource = allGroups;
                    lbxAvailableGroups.DisplayMemberPath = "GroupDisplayName";
                    lbxAvailableGroups.SelectedValuePath = "GroupID";
                    lbxSelectedGroups.ItemsSource = selectedGroups;
                    lbxSelectedGroups.DisplayMemberPath = "GroupDisplayName";
                    lbxSelectedGroups.SelectedValuePath = "GroupID";

                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please reload the page. If the problem persists please contact Customer Support.");
                    return;
                }
            }
        }

        private bool ProcessXmlForTagName(string xmlString, string tagName, string errorMessage, string methodName)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xmlString);
            var nodelist = xDoc.GetElementsByTagName(tagName);

            foreach (XmlNode node in nodelist)
            {
                try
                {
                    var id = node?.SelectSingleNode("ID")?.InnerText;
                    var name = node?.SelectSingleNode("Name")?.InnerText;
                    int bcid;
                    int.TryParse(id, out bcid);
                    allBaseChannels.Add(bcid, name);
                }
                catch (Exception ex)
                {
                    LogCriticalErrorViaWcfServiceAndReturn(ex, errorMessage, methodName);
                    return true;
                }
            }

            return false;
        }

        private bool ProcessXmlForTagNameForGroups(string list, string tagName, ref ObservableCollection<MyGroup> groupName)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(list);
            var nodelist = xDoc.GetElementsByTagName(tagName);

            foreach (XmlNode node in nodelist)
            {
                try
                {
                    var id = node?.SelectSingleNode("ID")?.InnerText;
                    var name = node?.SelectSingleNode("Name")?.InnerText;
                    int gid;
                    int.TryParse(id, out gid);
                    groupName.Add(new MyGroup(gid, name + " (" + gid + ")", name));
                }
                catch (Exception ex)
                {
                    return LogCriticalErrorViaWcfServiceAndReturn(ex, "An unexpected error occured loading groups.", nameof(cbCustomers_SelectionChanged));
                }
            }

            return false;
        }

        private string GetListOfBaseChannelsAsXmlString()
        {
            using (var tam = new com.ecn5.webservices.AccountManager.AccountManager())
            {
                var ak = _appSettingsprovider.GetAppSettingsValue("UASMasterAccessKey");
                var bc = tam.GetBaseChanels_Internal(ak);
                return bc;
            }
        }

        private string GetListOfCustomersAsXmlStringBasedOnCustomerId(int baseChannelId)
        {
            using (var tam = new com.ecn5.webservices.AccountManager.AccountManager())
            {
                var ak = _appSettingsprovider.GetAppSettingsValue("UASMasterAccessKey");
                var customers = tam.GetCustomers_Internal(ak, baseChannelId);
                return customers;
            }
        }

        private string GetInternalListsBasedOnCustomerId(int id)
        {
            using (var lmsc = new com.ecn5.webservices.ListManager.ListManager())
            {
                var ak = _appSettingsprovider.GetAppSettingsValue("UASMasterAccessKey");
                var list = lmsc.GetLists_Internal(ak, id);
                return list;
            }
        }

        private bool LogCriticalErrorViaWcfServiceAndReturn(Exception ex, string errorMessage, string eventName)
        {
            Core_AMS.Utilities.WPF.MessageError(errorMessage);
            var alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
            var accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            var app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            var logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            var formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name + "." + eventName, app, string.Empty, logClientId);
            return true;
        }


        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Clear|Reload Groups
            ClearFields();
            lbxAvailableGroups.ItemsSource = allGroups;

            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(ProductCreationContainer))
                {
                    ProductCreationContainer pc = b.DataContext as ProductCreationContainer;
                    FrameworkUAD.Entity.Product pItem = pc.ReturnProduct();
                    // FrameworkUAD.Entity.Product pItem = (FrameworkUAD.Entity.Product)b.DataContext;
                    if (pItem != null)
                    {
                        if (pItem.IsOpenCloseLocked != null && pItem.IsOpenCloseLocked == true)
                        {
                            Core_AMS.Utilities.WPF.Message("The current product is locked and cannot be editted.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Product Locked");
                            return;
                        }
                        currentProduct = pItem;
                        tbxName.Text = currentProduct.PubName;
                        tbxCode.Text = currentProduct.PubCode;

                        if (productTypes.FirstOrDefault(x => x.PubTypeID == currentProduct.PubTypeID) != null)
                            cbType.SelectedValue = pItem.PubTypeID;

                        if (currentProduct.EnableSearching.ToString() == Core_AMS.Utilities.Enums.TrueFalse.True.ToString())
                            cbSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
                        else
                            cbSearch.SelectedItem = Core_AMS.Utilities.Enums.YesNo.No.ToString();

                        cbScore.SelectedItem = currentProduct.score.ToString();

                        btnSave.Tag = currentProduct.PubID.ToString();

                        if (currentProduct.YearStartDate != null)
                            dtpYearStart.CurrentDateTimeText = currentProduct.YearStartDate.ToString();

                        if (currentProduct.YearEndDate != null)
                            dtpYearEnd.CurrentDateTimeText = currentProduct.YearEndDate.ToString();

                        if (currentProduct.IsActive != null && currentProduct.IsActive == true)
                            cbxIsActive.IsChecked = true;
                        else
                            cbxIsActive.IsChecked = false;

                        if (currentProduct.AllowDataEntry != null && currentProduct.AllowDataEntry == true)
                            cbxAllowDataEntry.IsChecked = true;
                        else
                            cbxAllowDataEntry.IsChecked = false;

                        if (currentProduct.FrequencyID != null)
                            cbFrequency.SelectedValue = currentProduct.FrequencyID;

                        if (currentProduct.IsUAD != null && currentProduct.IsUAD == true)
                            cbxUAD.IsChecked = true;
                        else
                            cbxUAD.IsChecked = false;

                        if (currentProduct.IsCirc != null && currentProduct.IsCirc == true)
                            cbxCirc.IsChecked = true;
                        else
                            cbxCirc.IsChecked = false;

                        //Load Selected Groups
                        List<FrameworkUAD.Entity.ProductGroup> selectGroups = productGroups.Where(x => x.PubID == currentProduct.PubID).ToList();
                        foreach (FrameworkUAD.Entity.ProductGroup pg in selectGroups)
                        {
                            //Get each group to add to selected
                            //int item = allGroups.FirstOrDefault(x => x.Key == pg.GroupID).Key;
                            //int item = allGroups.FirstOrDefault(x => x.GroupID == pg.GroupID).GroupID;
                            MyGroup mg = new MyGroup(pg.GroupID, pg.Name + " (" + pg.GroupID + ")", pg.Name);

                            //if (item > 0)
                            //    selectedGroups.Add(allGroups.FirstOrDefault(x => x.GroupID == pg.GroupID));//lbxSelectedGroups.Items.Add(allGroups.FirstOrDefault(x => x.Key == pg.GroupID).Value);
                            if (mg != null)
                            {
                                selectedGroups.Add(mg);
                                currGroups.Add(mg);
                            }

                        }

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
            MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to delete this Product? It will delete all mapping for the product.", "Warning", MessageBoxButton.YesNo);

            if (areYouSureDelete == MessageBoxResult.Yes)
            {
                if (e.OriginalSource.GetType() == typeof(Button))
                {
                    Button b = (Button)e.OriginalSource;
                    if (b.DataContext.GetType() == typeof(ProductCreationContainer))
                    {
                        ProductCreationContainer pc = b.DataContext as ProductCreationContainer;
                        FrameworkUAD.Entity.Product pItem = pc.ReturnProduct();
                        // FrameworkUAD.Entity.Product pItem = (FrameworkUAD.Entity.Product)b.DataContext;
                        if (pItem != null)
                        {
                            if (pItem.IsOpenCloseLocked != null && pItem.IsOpenCloseLocked == true)
                            {
                                Core_AMS.Utilities.WPF.Message("The current product is locked and cannot be deleted.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Product Locked");
                                return;
                            }
                            int PubID = pItem.PubID;
                            FrameworkUAS.Service.Response<int> svBoolP = new FrameworkUAS.Service.Response<int>();
                            pItem.IsActive = false;
                            svBoolP = pWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pItem, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if (svBoolP.Result != null && svBoolP.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            {
                                currentProduct = new FrameworkUAD.Entity.Product();
                                tbxName.Text = "";
                                tbxCode.Text = "";
                                cbType.SelectedIndex = 0;
                                cbSearch.SelectedIndex = 0;
                                cbScore.SelectedIndex = 0;
                                cbBase.SelectedIndex = 0;
                                cbCustomers.SelectedIndex = 0;
                                cbxAllowDataEntry.IsChecked = false;
                                cbxIsActive.IsChecked = false;
                                cbFrequency.SelectedIndex = -1;
                                cbxUAD.IsChecked = false;
                                cbxCirc.IsChecked = false;
                                cbBase.SelectedIndex = 0;
                                cbCustomers.SelectedIndex = 0;
                                selectedGroups.Clear();
                                dtpYearStart.CurrentDateTimeText = "";
                                dtpYearEnd.CurrentDateTimeText = "";

                                btnSave.Tag = "";
                                btnSave.Content = "Save";
                                LoadData();
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.Message("Failed to delete. If this problem consists please contact Customer Service.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                return;
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

        private void btnResponses_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(ProductCreationContainer))
                {
                    ProductCreationContainer pc = b.DataContext as ProductCreationContainer;
                    FrameworkUAD.Entity.Product pItem = pc.ReturnProduct();
                    // FrameworkUAD.Entity.Product pItem = (FrameworkUAD.Entity.Product)b.DataContext;
                    if (pItem != null)
                    {
                        //OPEN MasterCodeSheet pass MasterGroupID
                        //DockPanel foundStackPanel = Core_AMS.Utilities.WPF.FindControl<DockPanel>(Application.Current.MainWindow, "spModule");

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
                                        ControlCenter.Controls.CodeSheet cs = new CodeSheet(pItem.PubID);
                                        sdp.Children.Add(cs);
                                    }
                                }
                            }
                        }
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

        private void btnResponseGroups_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(ProductCreationContainer))
                {
                    ProductCreationContainer pc = b.DataContext as ProductCreationContainer;
                    FrameworkUAD.Entity.Product pItem = pc.ReturnProduct();
                    // FrameworkUAD.Entity.Product pItem = (FrameworkUAD.Entity.Product)b.DataContext;
                    if (pItem != null)
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
                                        ControlCenter.Controls.ResponseGroups rg = new ResponseGroups(pItem.PubID);
                                        sdp.Children.Add(rg);
                                    }
                                }
                            }
                        }
                        //OPEN MasterCodeSheet pass MasterGroupID
                        //StackPanel foundStackPanel = Core_AMS.Utilities.WPF.FindControl<StackPanel>(Application.Current.MainWindow, "spModule");
                        //if (foundStackPanel != null)
                        //{
                        //   StackPanel foundStackPanel2 = Core_AMS.Utilities.WPF.FindControl<StackPanel>(foundStackPanel, "spControls");
                        //    if (foundStackPanel2 != null)
                        //    {
                        //        foundStackPanel2.Children.Clear();
                        //        ControlCenter.Controls.ResponseGroups rg = new ResponseGroups(pItem.PubID);
                        //        foundStackPanel2.Children.Add(rg);
                        //    }
                        //}
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name;
            string code;
            int type;
            var search = false;
            var score = 0;
            var pubId = 0;

            // Assign Values
            var isUad = cbxUAD.IsChecked == true;

            var isCirc = cbxCirc.IsChecked == true;

            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out pubId);

            // Check Empty Data
            if (string.IsNullOrEmpty(tbxName.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                name = tbxName.Text;

            if (string.IsNullOrEmpty(tbxCode.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Description.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                code = tbxCode.Text;

            if (cbType.SelectedValue != null)
                int.TryParse(cbType.SelectedValue.ToString(), out type);
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select a type.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }

            if (cbSearch.SelectedItem != null)
            {
                search = cbSearch.SelectedItem.ToString() == Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
            }
            else
            {
                if (isUad)
                {
                    Core_AMS.Utilities.WPF.Message("Please select enable searching value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                    return;
                }
            }

            if (cbScore.SelectedValue != null)
            {
                int.TryParse(cbScore.SelectedValue.ToString(), out score);
            }
            else
            {
                if (isUad)
                {
                    Core_AMS.Utilities.WPF.Message("Please select a score.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                    return;
                }
            }

            // New Values
            var yrStDate = string.Empty;
            var yrEndDate = string.Empty;
            var frequencyId = 0;

            if (!string.IsNullOrEmpty(dtpYearStart.CurrentDateTimeText))
                yrStDate = dtpYearStart.CurrentDateTimeText;

            if (!string.IsNullOrEmpty(dtpYearEnd.CurrentDateTimeText))
                yrEndDate = dtpYearEnd.CurrentDateTimeText;

            var isActive = cbxIsActive.IsChecked == true;

            if (cbFrequency.SelectedValue != null)
                int.TryParse(cbFrequency.SelectedValue.ToString(), out frequencyId);

            // Validate Circ Product
            if (isCirc)
            {
                if (string.IsNullOrEmpty(yrStDate))
                {
                    Core_AMS.Utilities.WPF.Message("Please supply a Year Start Date.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }

                if (string.IsNullOrEmpty(yrEndDate))
                {
                    Core_AMS.Utilities.WPF.Message("Please supply a Year End Date.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }

            // Check value doesn't exist
            if (currentProduct != null)
            {
                if (currentProduct.PubName != name || currentProduct.PubCode != code)
                {
                    if (products.FirstOrDefault(x => (x.PubName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && x.PubID != pubId) || (x.PubCode.Equals(code, StringComparison.CurrentCultureIgnoreCase) && x.PubID != pubId)) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Product already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                        return;
                    }
                }
            }
            else
            {
                if (products.FirstOrDefault(x => x.PubID == pubId || x.PubName.Equals(name, StringComparison.CurrentCultureIgnoreCase) || x.PubCode.Equals(code, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Product already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                    return;
                }
            }

            // Save|Update
            var pEntry = new FrameworkUAD.Entity.Product
            {
                PubID = pubId,
                ClientID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID,
                IsOpenCloseLocked = false,
                PubName = name,
                PubCode = code,
                istradeshow = false,
                PubTypeID = type,
                GroupID = 0,
                score = score,
                EnableSearching = search,
                SortOrder = 0,
                YearStartDate = yrStDate,
                YearEndDate = yrEndDate,
                IsActive = isActive,
                AllowDataEntry = currentProduct.AllowDataEntry,
                FrequencyID = frequencyId,
                IsUAD = isUad,
                IsCirc = isCirc,
                UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                DateUpdated = DateTime.Now,
                DateCreated = currentProduct.DateCreated,
                CreatedByUserID = currentProduct.CreatedByUserID,
                IssueDate = currentProduct.IssueDate,
                IsImported = currentProduct.IsImported,
                KMImportAllowed = currentProduct.KMImportAllowed,
                ClientImportAllowed = currentProduct.ClientImportAllowed,
                AddRemoveAllowed = currentProduct.AddRemoveAllowed,
                AcsMailerInfoId = currentProduct.AcsMailerInfoId
            };

            svIntP = pWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pEntry, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svIntP.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                pubId = svIntP.Result;
                var pgEntry = new FrameworkUAD.Entity.ProductGroup
                {
                    PubID = pubId
                };

                pgWorker.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pubId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

                foreach (var mg in selectedGroups)
                {
                    var groupId = mg.GroupID;

                    if (groupId > 0)
                    {
                        pgEntry.Name = mg.GroupName;
                        pgEntry.GroupID = groupId;
                        svIntPG = pgWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pgEntry, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("There was an error saving " + mg.GroupID + " in groups data. If this problem persists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                        return;
                    }
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem persists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                return;
            }

            Core_AMS.Utilities.WPF.MessageSaveComplete();

            currentProduct = new FrameworkUAD.Entity.Product();
            ClearFields();

            btnSave.Tag = "";
            btnSave.Content = "Save";

            LoadData();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            currentProduct = new FrameworkUAD.Entity.Product();
            tbxName.Text = "";
            tbxCode.Text = "";
            cbType.SelectedIndex = 0;
            cbSearch.SelectedIndex = 0;
            cbScore.SelectedIndex = 0;
            cbBase.SelectedIndex = 0;
            cbCustomers.SelectedIndex = 0;
            cbxAllowDataEntry.IsChecked = false;
            cbxIsActive.IsChecked = false;
            cbFrequency.SelectedIndex = -1;
            cbxUAD.IsChecked = false;
            cbxCirc.IsChecked = false;
            cbBase.SelectedIndex = 0;
            cbCustomers.SelectedIndex = 0;
            selectedGroups.Clear();
            dtpYearStart.CurrentDateTimeText = "";
            dtpYearEnd.CurrentDateTimeText = "";

            btnSave.Tag = "";
            btnSave.Content = "Save";
        }

        public void ClearFields()
        {
            tbxName.Text = "";
            tbxCode.Text = "";
            cbType.SelectedIndex = -1;
            cbSearch.SelectedIndex = -1;
            cbScore.SelectedIndex = -1;
            cbxAllowDataEntry.IsChecked = false;
            cbxIsActive.IsChecked = false;
            cbFrequency.SelectedIndex = -1;
            cbxUAD.IsChecked = false;
            cbxCirc.IsChecked = false;
            cbBase.SelectedIndex = 0;
            cbCustomers.SelectedIndex = 0;
            selectedGroups.Clear();
            dtpYearStart.CurrentDateTimeText = "";
            dtpYearEnd.CurrentDateTimeText = "";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAvailableGroups.SelectedItems.Count <= 0) return;
            var tempGroups = new ObservableCollection<MyGroup>();
            foreach (MyGroup mg in lbxAvailableGroups.SelectedItems)
            {
                if (!selectedGroups.Select(x => x.GroupID).Contains(mg.GroupID))
                {
                    selectedGroups.Add(mg);
                    tempGroups.Add(mg);
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("You already have this Group ID (" + mg.GroupID + ") selected. Remove existing Group ID first to add.");
                }

            }
            
            foreach (var mg in tempGroups)
            {
                allGroups.Remove(mg);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //if (lbxSelectedGroups.SelectedItems.Count > 0)
            //{
            //    List<string> selectedItems = new List<string>();
            //    foreach (string s in lbxSelectedGroups.SelectedItems)
            //    {
            //        lbxAvailableGroups.Items.Add(s);
            //        selectedItems.Add(s);
            //    }
            //    foreach (string s in selectedItems)
            //    {
            //        lbxSelectedGroups.Items.Remove(s);
            //    }
            //    lbxAvailableGroups.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            //}
            ObservableCollection<MyGroup> tempGroups = new ObservableCollection<MyGroup>();
            if (lbxSelectedGroups.SelectedItems.Count > 0)
            {
                foreach (MyGroup mg in lbxSelectedGroups.SelectedItems)
                {
                    allGroups.Add(mg);
                    tempGroups.Add(mg);
                    //selectedGroups.Remove(mg);
                }
                foreach (MyGroup mg in tempGroups)
                {
                    selectedGroups.Remove(mg);
                }
                //lbxAvailableGroups.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
                //selectedGroups = selectedGroups.Except(tempGroups);
                allGroups = new ObservableCollection<MyGroup>(allGroups.OrderBy(x => x.GroupName));
                lbxAvailableGroups.ItemsSource = allGroups;
            }
        }
        private void gridGroups_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (sender.GetType() == typeof(Telerik.Windows.Controls.RadGridView))
            {
                //Telerik.Windows.Controls.RadGridView rgv = (Telerik.Windows.Controls.RadGridView)sender;                                                      
                if (e.Row.GetType() == typeof(Telerik.Windows.Controls.GridView.GridViewRow))
                {
                    if (e.Row.Cells[1].ParentRow.DataContext.GetType() == typeof(FrameworkUAD.Entity.Product))
                    {
                        FrameworkUAD.Entity.Product p = (FrameworkUAD.Entity.Product)e.Row.Cells[1].ParentRow.DataContext;
                        FrameworkUAD.Entity.ProductTypes pt = productTypes.FirstOrDefault(x => x.PubTypeID == p.PubTypeID);
                        TextBlock tb4 = (TextBlock)e.Row.Cells[4].Content;
                        if (pt != null)
                            tb4.Text = pt.PubTypeDisplayName;

                        FrameworkUAD.Entity.Frequency f = allFrequency.FirstOrDefault(x => x.FrequencyID == p.FrequencyID);
                        //TextBlock tb11 = (TextBlock)e.Row.Cells[].Content;
                        //if (f != null)
                        //    tb11.Text = f.FrequencyName;

                    }
                }
            }
        }
    }
}
