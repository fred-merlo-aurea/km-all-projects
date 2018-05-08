using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for AdhocSetup.xaml
    /// </summary>
    public partial class AdhocSetup : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IAdhocCategory> acWorker = FrameworkServices.ServiceClient.UAD_AdhocCategoryClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IAdhoc> aWorker = FrameworkServices.ServiceClient.UAD_AdhocClient();
        #endregion
        #region VARIABLES
        //FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>> svAvailable = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>>();
        //FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>> svSelected = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>> svAdhocs = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>> svAdhocsOther = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.AdhocCategory>> svAdhocCats = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.AdhocCategory>>();
        FrameworkUAS.Service.Response<int> saveAdhocCategory = new FrameworkUAS.Service.Response<int>();
        FrameworkUAS.Service.Response<int> saveAdhocs = new FrameworkUAS.Service.Response<int>();
        FrameworkUAS.Service.Response<bool> deleteAdhocs = new FrameworkUAS.Service.Response<bool>();

        FrameworkUAD.Entity.AdhocCategory currentAdhocCategory = new FrameworkUAD.Entity.AdhocCategory();
        List<FrameworkUAD.Entity.AdhocCategory> allAdhocCategories = new List<FrameworkUAD.Entity.AdhocCategory>();
        List<FrameworkUAD.Entity.Adhoc> allAdhocs = new List<FrameworkUAD.Entity.Adhoc>();
        List<FrameworkUAD.Entity.Adhoc> otherAdhocs = new List<FrameworkUAD.Entity.Adhoc>();
        List<FrameworkUAD.Entity.Adhoc> allSelected = new List<FrameworkUAD.Entity.Adhoc>();
        #endregion
        #region ENUMS
        public enum IsSaveUpdate
        {
            Save,
            Update
        };
        #endregion
        public AdhocSetup()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();                
            currentAdhocCategory = null;            
        }

        #region Loading Methods
        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svAdhocCats = acWorker.Proxy.SelectAll(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svAdhocs = aWorker.Proxy.SelectCategoryID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, 0);
                svAdhocsOther = aWorker.Proxy.SelectAll(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svAdhocCats.Result != null && svAdhocCats.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    allAdhocCategories = svAdhocCats.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svAdhocs.Result != null && svAdhocs.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    allAdhocs = svAdhocs.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svAdhocsOther.Result != null && svAdhocsOther.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    otherAdhocs = svAdhocsOther.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadGrid();
                LoadAvailable();
                LoadCombos(IsSaveUpdate.Save);

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        public void LoadGrid()
        {                                  
            gridCategory.ItemsSource = null;
            if (allAdhocCategories != null)
            {
                if (allAdhocs != null)
                {
                    foreach (FrameworkUAD.Entity.AdhocCategory ac in allAdhocCategories)
                    {
                        ac.AdhocList = new List<FrameworkUAD.Entity.Adhoc>();
                        ac.AdhocList = otherAdhocs.Where(x => x.CategoryID == ac.CategoryID).OrderBy(x => x.SortOrder).ToList();
                    }
                }
                gridCategory.ItemsSource = allAdhocCategories.OrderBy(x => x.SortOrder);                    
            }
        }        
        
        public void LoadAvailable()
        {
            lbxAvailable.Items.Clear();            
            if (allAdhocs.Count > 0)
            {
                List<FrameworkUAD.Entity.Adhoc> distinctList = allAdhocs.Where(x => x.CategoryID == 0).ToList();
                foreach (FrameworkUAD.Entity.Adhoc a in distinctList.OrderBy(x => x.DisplayName))
                {
                    lbxAvailable.Items.Add(a.DisplayName);
                }
            }
        }

        public void LoadSelected(int CategoryID)
        {
            lbxSelected.Items.Clear();
            if (allAdhocs.Count > 0)
            {
                List<FrameworkUAD.Entity.Adhoc> distinctList = otherAdhocs.Where(x => x.CategoryID == CategoryID).ToList();
                foreach (FrameworkUAD.Entity.Adhoc a in distinctList.OrderBy(x => x.SortOrder))
                {
                    lbxSelected.Items.Add(a.AdhocName);
                }
            }            
        }

        private void LoadCombos(IsSaveUpdate e)
        {
            int sortCount = 1;
            Dictionary<int, int> sortList = new Dictionary<int, int>();
            if (allAdhocCategories != null)
            {
                sortCount = allAdhocCategories.Count;
                if (e == IsSaveUpdate.Save)
                    sortCount++;

                for (int i = 1; i <= sortCount; i++)
                {
                    sortList.Add(i, i);
                }
            }

            cbSortOrder.ItemsSource = null;
            if (sortList.Count > 0)
            {
                cbSortOrder.ItemsSource = sortList;
                cbSortOrder.SelectedValuePath = "Key";
                cbSortOrder.DisplayMemberPath = "Value";
            }
        }
        #endregion

        #region Button Methods
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.AdhocCategory))
                {
                    FrameworkUAD.Entity.AdhocCategory acItem = (FrameworkUAD.Entity.AdhocCategory)b.DataContext;
                    if (acItem != null)
                    {
                        LoadCombos(IsSaveUpdate.Update);
                        currentAdhocCategory = acItem;
                        tbxName.Text = acItem.CategoryName.ToString();
                        cbSortOrder.SelectedValue = acItem.SortOrder;
                        btnSave.Tag = acItem.CategoryID.ToString();
                        LoadSelected(acItem.CategoryID);
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {            
            int adhocCategoryID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out adhocCategoryID);

            string name = "";
            int sortOrder = 0;

            if (allAdhocCategories == null)
            {
                Core_AMS.Utilities.WPF.Message("Previous Adhoc data wasn't provided and save cannot continue. Please refresh and contact Customer Support if problem persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }

            #region Check Name
            if (!string.IsNullOrEmpty(tbxName.Text))
            {                
                if (currentAdhocCategory != null)
                {
                    //Edit
                    if (currentAdhocCategory.CategoryName.Equals(tbxName.Text, StringComparison.CurrentCultureIgnoreCase) == false
                        && allAdhocCategories.SingleOrDefault(x => x.CategoryName.Equals(tbxName.Text, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Category Name already used. Please provide new Category Name before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Data");
                        return;
                    }
                    else
                        name = tbxName.Text;
                }
                else
                {
                    //New
                    if (allAdhocCategories.SingleOrDefault(x => x.CategoryName.Equals(tbxName.Text, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Category Name already used. Please provide new Category Name before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Data");
                        return;
                    }
                    else
                        name = tbxName.Text;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Category Name not provided. Please provide Category Name before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion

            #region Check Sort
            if (cbSortOrder.SelectedValue != null)
                int.TryParse(cbSortOrder.SelectedValue.ToString(), out sortOrder);
            else
            {
                Core_AMS.Utilities.WPF.Message("Sort Order not provided. Please provide a sort order before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Data");
                return;
            }
            #endregion

            #region Create Save AdhocCategory
            FrameworkUAD.Entity.AdhocCategory acItem = new FrameworkUAD.Entity.AdhocCategory();
            acItem.CategoryID = adhocCategoryID;
            acItem.CategoryName = name;
            acItem.SortOrder = sortOrder;
            #endregion

            #region Save Adhoc Category
            saveAdhocCategory = acWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, acItem);            
            int categoryID = 0;
            if (saveAdhocCategory != null && saveAdhocCategory.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                categoryID = saveAdhocCategory.Result;

            #endregion

            //Delete old Adhocs and Save new Adhocs
            if (categoryID > 0)
            {
                #region Delete
                bool delete = false;
                deleteAdhocs = aWorker.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, categoryID);                
                if (deleteAdhocs != null && deleteAdhocs.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    delete = deleteAdhocs.Result;
                else
                {
                    Core_AMS.Utilities.WPF.Message("Failed to remove old adhocs. Please contact Customer Support if problem persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                    return;
                }
                #endregion

                #region Save Adhoc
                int order = 1;
                List<string> failedSaves = new List<string>();
                if (lbxSelected.Items.Count > 0)
                {
                    foreach (string s in lbxSelected.Items)
                    {
                        FrameworkUAD.Entity.Adhoc aItem = new FrameworkUAD.Entity.Adhoc();
                        aItem.AdhocID = 0;
                        aItem.AdhocName = s.ToString();
                        aItem.CategoryID = categoryID;
                        aItem.SortOrder = order;

                        order++;

                        saveAdhocs = aWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, aItem);                        
                        if (saveAdhocs != null && saveAdhocs.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            continue;
                        else
                            failedSaves.Add(s.ToString());
                    }

                    if (failedSaves.Count > 0)
                    {
                        Core_AMS.Utilities.WPF.Message("Failed to save following Adhocs to Category Name: " + acItem.CategoryName + ". Please contact Customer Support if problem persists. Items that failed to save: " + String.Join(",", failedSaves).ToString().TrimEnd(','), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                        return;
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.MessageSaveComplete();
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                }
                #endregion
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Failed to save Category Name " + acItem.CategoryName + ". Please contact Customer Support if problem persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }

            #region Refresh|Clear
            //Set currentItem to null
            currentAdhocCategory = null;

            //Change Button tag to zero content back to save
            btnSave.Tag = "0";
            btnSave.Content = "Save";

            //Clear control
            tbxName.Text = "";
            cbSortOrder.SelectedIndex = -1;            

            //Refresh Grid  
            LoadData();            
            lbxSelected.Items.Clear();
            #endregion
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Change Button tag to zero content back to save
            btnSave.Tag = "0";
            btnSave.Content = "Save";

            //Set currentItem to null
            currentAdhocCategory = null;

            //Clear control
            tbxName.Text = "";
            cbSortOrder.SelectedIndex = -1;
            LoadAvailable();
            LoadCombos(IsSaveUpdate.Save);
            lbxSelected.Items.Clear();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAvailable.SelectedItems.Count > 0)
            {
                List<string> selectedItems = new List<string>(); 
                foreach (string s in lbxAvailable.SelectedItems)
                {
                    lbxSelected.Items.Add(s);
                    selectedItems.Add(s);
                }                
                foreach (string s in selectedItems)
                {
                    lbxAvailable.Items.Remove(s);
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {                                
                List<string> selectedItems = new List<string>();
                foreach (string s in lbxSelected.SelectedItems)
                {
                    lbxAvailable.Items.Add(s);
                    selectedItems.Add(s);
                }
                foreach (string s in selectedItems)
                {
                    lbxSelected.Items.Remove(s);
                }
                lbxAvailable.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbxSelected.Items.Count; i++)
                {
                    if (lbxSelected.SelectedItems.Contains(lbxSelected.Items[i]))
                    {
                        if (i > 0 && !lbxSelected.SelectedItems.Contains(lbxSelected.Items[i - 1]))
                        {
                            var item = lbxSelected.Items[i];
                            lbxSelected.Items.Remove(item);
                            lbxSelected.Items.Insert(i - 1, item);
                            lbxSelected.SelectedItem = item;
                        }
                    }
                }
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                int startindex = lbxSelected.Items.Count - 1;

                for (int i = startindex; i > -1; i--)
                {
                    if (lbxSelected.SelectedItems.Contains(lbxSelected.Items[i]))
                    {
                        if (i < startindex && !lbxSelected.SelectedItems.Contains(lbxSelected.Items[i + 1]))
                        {
                            var item = lbxSelected.Items[i];
                            lbxSelected.Items.Remove(item);
                            lbxSelected.Items.Insert(i + 1, item);
                            lbxSelected.SelectedItem = item;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
