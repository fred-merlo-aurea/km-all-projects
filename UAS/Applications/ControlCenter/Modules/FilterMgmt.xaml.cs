using FrameworkUAS.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using System.Windows.Media;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for FilterMgmt.xaml
    /// </summary>
    public partial class FilterMgmt : UserControl
    {
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IFilter> filterWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; }
        private List<FrameworkUAD.Entity.Product> products { get; set; }
        public List<FrameworkUAD.Entity.Filter> filters { get; set; }
        public List<FrameworkUAD.Entity.FilterCategory> filterCategory { get; set; }
        public List<FrameworkUAD.Entity.QuestionCategory> questionCategory { get; set; }
        public List<FrameworkUAD.Entity.Brand> brands { get; set; }
        private List<FilterContainer> filterContainer = new List<FilterContainer>();
        private FrameworkUAD.Entity.Filter originalFilter = new FrameworkUAD.Entity.Filter();
        private FilterContainer currRow = new FilterContainer();

        public FilterMgmt()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > -1))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    LoadData();
                    this.DataContext = this;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
            }
        }
        private void LoadData()
        {
            //startUp = false;
            SortDescriptor sort = new SortDescriptor();
            sort.Member = "Name";
            sort.SortDirection = ListSortDirection.Ascending;
            grdFilter.SortDescriptors.Add(sort);
            filterWorker = FrameworkServices.ServiceClient.UAD_FilterClient();

            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));

            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result;
            filters = filterWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            #region Filter Category
            FrameworkServices.ServiceClient<UAD_WS.Interface.IFilterCategory> filterCategoryWorker = FrameworkServices.ServiceClient.UAD_FilterCategoryClient();
            filterCategory = filterCategoryWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (filterCategory != null)
            {
                cbFilterCategory.ItemsSource = filterCategory;
                cbFilterCategory.SelectedValuePath = "FilterCategoryID";
                cbFilterCategory.DisplayMemberPath = "CategoryName";
            }
            #endregion
            #region Question Category
            FrameworkServices.ServiceClient<UAD_WS.Interface.IQuestionCategory> questionCategoryWorker = FrameworkServices.ServiceClient.UAD_QuestionCategoryClient();
            questionCategory = questionCategoryWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (questionCategory != null)
            {
                cbQuestionCategory.ItemsSource = questionCategory;
                cbQuestionCategory.SelectedValuePath = "QuestionCategoryID";
                cbQuestionCategory.DisplayMemberPath = "CategoryName";
            }
            #endregion
            #region Product
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
            products = productWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (products != null)
            {
                cbPub.ItemsSource = products;
                cbPub.SelectedValuePath = "PubID";
                cbPub.DisplayMemberPath = "PubName";
            }
            #endregion
            #region Brand
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBrand> brandWorker = FrameworkServices.ServiceClient.UAD_BrandClient();
            brands = brandWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (brands != null)
            {
                cbBrand.ItemsSource = brands;
                cbBrand.SelectedValuePath = "BrandID";
                cbBrand.DisplayMemberPath = "BrandName";
            }
            #endregion

            foreach (FrameworkUAD.Entity.Filter filter in filters)
            {
                //FilterContainer fc = new FilterContainer(filter);
                //KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == filter.CreatedUserID);
                //if (user != null)
                //    fc.CreatedByName = user.FullName;

                //user = kmUsers.FirstOrDefault(x => x.UserID == filter.UpdatedUserID);
                //if (user != null)
                //    fc.UpdatedByName = user.FullName;

                //FrameworkUAD.Entity.Product prod = products.FirstOrDefault(x => x.PubID == filter.PubID);
                //if (prod != null)
                //    fc.ProductName = prod.PubName;

                //FrameworkUAD.Entity.Brand brand = brands.FirstOrDefault(x => x.BrandID == filter.BrandID);
                //if (brand != null)
                //    fc.BrandName = brand.BrandName;

                //FrameworkUAD.Entity.FilterCategory filterCat = filterCategory.FirstOrDefault(x => x.FilterCategoryID == filter.FilterCategoryID);
                //if (filterCat != null)
                //    fc.FilterCategoryName = filterCat.CategoryName;

                //FrameworkUAD.Entity.QuestionCategory questionCat = questionCategory.FirstOrDefault(x => x.QuestionCategoryID == filter.QuestionCategoryID);
                //if (questionCat != null)
                //    fc.QuestionCategoryName = questionCat.CategoryName;

                //filterContainer.Add(fc);
            }

            grdFilter.ItemsSource = filterContainer;
        }
        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdFilter != null && grdFilter.Columns.Count > 0)
            {
                Telerik.Windows.Controls.GridViewColumn isEnabledColumn = grdFilter.Columns["IsDeleted"];

                string isEnabled = lstEnabled.SelectedItem.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "");
                //All, Enabled, Not Enabled

                if (!isEnabled.Equals("All"))
                {
                    if (isEnabled.Equals("Deleted")) isEnabled = "true";
                    else isEnabled = "false";

                    Telerik.Windows.Controls.GridView.IColumnFilterDescriptor isEnabledFilter = isEnabledColumn.ColumnFilterDescriptor;
                    // Suspend the notifications to avoid multiple data engine updates
                    isEnabledFilter.SuspendNotifications();

                    // This is the same as the end user configuring the upper field filter.
                    isEnabledFilter.FieldFilter.Filter1.Operator = Telerik.Windows.Data.FilterOperator.IsEqualTo;
                    isEnabledFilter.FieldFilter.Filter1.Value = isEnabled;
                    isEnabledFilter.FieldFilter.Filter1.IsCaseSensitive = false;

                    // This is the same as the end user changing the logical operator between the two field filters.
                    //countryFilter.FieldFilter.LogicalOperator = Telerik.Windows.Data.FilterCompositionLogicalOperator.Or;

                    // This is the same as the end user configuring the lower field filter.
                    //countryFilter.FieldFilter.Filter2.Operator = Telerik.Windows.Data.FilterOperator.Contains;
                    //countryFilter.FieldFilter.Filter2.Value = "stan";
                    //countryFilter.FieldFilter.Filter2.IsCaseSensitive = true;

                    // Resume the notifications to force the data engine to update the filter.
                    isEnabledFilter.ResumeNotifications();
                }
                else
                {
                    grdFilter.FilterDescriptors.SuspendNotifications();
                    isEnabledColumn.ClearFilters();
                    grdFilter.FilterDescriptors.ResumeNotifications();
                }
            }

        }

        private string GetControlValue(List<ControlObject> obj, string ctrlName)
        {
            var item = obj.SingleOrDefault(i => i.Name == ctrlName);

            if (item == null || item.Value == null)
                return "";
            else
                return item.Value;

        }
        public class ControlObject
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        private Boolean infoChanged(List<ControlObject> o)
        {
            Boolean infoChanged = false;
            //if (!originalFilter.Name.ToString().Equals(GetControlValue(o, "tbBFilterName").Trim()))
            //{
            //    infoChanged = true;
            //}

            //if (!originalFilter.BrandID.ToString().Equals(GetControlValue(o, "cbBBrand").Trim()))
            //{
            //    infoChanged = true;
            //}

            return infoChanged;
        }

        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdFilter.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            //var rows = this.grdFilter.ChildrenOfType<GridViewRow>();
            //foreach (GridViewRow row in rows)
            //{
            //    if (row.DetailsVisibility == Visibility.Visible)
            //    {
            //        row.DetailsVisibility = Visibility.Collapsed;
            //        break;
            //    }
            //}
        }
        private void grdFilter_RowDetailsVisibilityChanged(object sender, GridViewRowDetailsEventArgs e)
        {
            FilterContainer tmpFilter = (FilterContainer)e.DetailsElement.DataContext;
            Grid grd = e.DetailsElement.FindName("ItemDetails") as Grid;
            grdFilter.ItemsSource = filterContainer;

            List<RadComboBox> rcbList = grd.ChildrenOfType<RadComboBox>().ToList();

            foreach (RadComboBox rcb in rcbList)
            {
                if (rcb.Name == "cbBBrand")
                {
                    rcb.ItemsSource = brands;
                    rcb.SelectedValuePath = "BrandID";
                    rcb.DisplayMemberPath = "BrandName";
                }
                else if (rcb.Name == "cbBFilterCategory")
                {
                    rcb.ItemsSource = filterCategory;
                    rcb.SelectedValuePath = "FilterCategoryID";
                    rcb.DisplayMemberPath = "CategoryName";
                }
                else if (rcb.Name == "cbBQuestionCategory")
                {
                    rcb.ItemsSource = questionCategory;
                    rcb.SelectedValuePath = "QuestionCategoryID";
                    rcb.DisplayMemberPath = "CategoryName";
                }
                else
                {
                    rcb.ItemsSource = products;
                    rcb.SelectedValuePath = "PubID";
                    rcb.DisplayMemberPath = "PubName";
                }
            }

            if (e.Visibility == Visibility.Visible)
            {
                #region Get Campagin Variables
                //originalFilter.FilterID = tmpFilter.FilterID;
                //originalFilter.Name = tmpFilter.Name;
                //originalFilter.FilterXML = tmpFilter.FilterXML;
                //originalFilter.CreatedDate = tmpFilter.CreatedDate;
                //originalFilter.CreatedUserID = tmpFilter.CreatedUserID;
                //originalFilter.FilterType = tmpFilter.FilterType;
                //originalFilter.PubID = tmpFilter.PubID;
                //originalFilter.IsDeleted = tmpFilter.IsDeleted;
                //originalFilter.UpdatedDate = tmpFilter.UpdatedDate;
                //originalFilter.UpdatedUserID = tmpFilter.UpdatedUserID;
                //originalFilter.BrandID = tmpFilter.BrandID;
                //originalFilter.AddtoSalesView = tmpFilter.AddtoSalesView;
                //originalFilter.FilterCategoryID = tmpFilter.FilterCategoryID;
                //originalFilter.QuestionCategoryID = tmpFilter.QuestionCategoryID;
                //originalFilter.QuestionName = tmpFilter.QuestionName;
                #endregion

                currRow = tmpFilter;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var test = e.Source;
            RadButton thisBtn = ((RadButton)sender);
            List<ControlObject> objects = new List<ControlObject>();
            objects.AddRange(RadGridInformation(thisBtn));
            var testing = thisBtn.GetParents();

            if (!infoChanged(objects))
            {
                Core_AMS.Utilities.WPF.MessageError("No changes were made.");
                grdFilter.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            else
            {
                //EditAction == Commit
                //save changes
                FilterContainer myFilter = currRow;
                #region Check Values
                if (string.IsNullOrEmpty(myFilter.Name))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current name cannot be blank.");
                    return;
                }
                //if (filters != null && filters.FirstOrDefault(x => x.FilterID == myFilter.FilterID).Name != myFilter.Name)
                //{
                //    if (filters.FirstOrDefault(x => x.Name.Equals(myFilter.Name, StringComparison.CurrentCultureIgnoreCase) && x.FilterID != myFilter.FilterID) != null)
                //    {
                //        Core_AMS.Utilities.WPF.MessageError("Current name has been used. Please provide a unique service name.");
                //        return;
                //    }
                //}                
                #endregion

                FrameworkUAD.Entity.Filter saveFilter = new FrameworkUAD.Entity.Filter();
                #region Get Filter Variables
                //saveFilter.FilterID = myFilter.FilterID;
                //saveFilter.Name = myFilter.Name;
                //saveFilter.FilterXML = myFilter.FilterXML;
                //saveFilter.CreatedDate = myFilter.CreatedDate;
                //saveFilter.CreatedUserID = myFilter.CreatedUserID;
                //saveFilter.FilterType = myFilter.FilterType;
                //saveFilter.PubID = myFilter.PubID;
                //saveFilter.IsDeleted = myFilter.IsDeleted;
                //saveFilter.UpdatedDate = myFilter.UpdatedDate;
                //saveFilter.UpdatedUserID = myFilter.UpdatedUserID;
                //saveFilter.BrandID = myFilter.BrandID;
                //saveFilter.AddtoSalesView = myFilter.AddtoSalesView;
                //saveFilter.FilterCategoryID = myFilter.FilterCategoryID;
                //saveFilter.QuestionCategoryID = myFilter.QuestionCategoryID;
                //saveFilter.QuestionName = myFilter.QuestionName;
                #endregion

                filterWorker = FrameworkServices.ServiceClient.UAD_FilterClient();
                myFilter.UpdatedUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                //myFilter.UpdatedDate = DateTime.Now;
                //saveFilter.UpdatedUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                //saveFilter.UpdatedDate = DateTime.Now;
                filterWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, saveFilter);

                //KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == saveFilter.UpdatedUserID);
                //if (user != null)
                //    myFilter.UpdatedByName = user.FullName;

                //FrameworkUAD.Entity.Product prod = products.FirstOrDefault(x => x.PubID == myFilter.PubID);
                //if (prod != null)
                //    myFilter.ProductName = prod.PubName;

                //FrameworkUAD.Entity.Brand brand = brands.FirstOrDefault(x => x.BrandID == myFilter.BrandID);
                //if (brand != null)
                //    myFilter.BrandName = brand.BrandName;

                //FrameworkUAD.Entity.FilterCategory filterCat = filterCategory.FirstOrDefault(x => x.FilterCategoryID == myFilter.FilterCategoryID);
                //if (filterCat != null)
                //    myFilter.FilterCategoryName = filterCat.CategoryName;

                //FrameworkUAD.Entity.QuestionCategory questionCat = questionCategory.FirstOrDefault(x => x.QuestionCategoryID == myFilter.QuestionCategoryID);
                //if (questionCat != null)
                //    myFilter.QuestionCategoryName = questionCat.CategoryName;

                //filters = filterWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                
                //var rows = this.grdFilter.ChildrenOfType<GridViewRow>();
                //foreach (GridViewRow row in rows)
                //{
                //    if (row.DetailsVisibility == Visibility.Visible)
                //    {
                //        row.DetailsVisibility = Visibility.Collapsed;
                //        break;
                //    }
                //}
                //grdFilter.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
                //grdFilter.Rebind();
            }
        }
        //private void grdFilter_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        //{
        //    if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
        //    {
        //        FrameworkUAD.Entity.Filter filter = e.DataElement as FrameworkUAD.Entity.Filter;
        //        KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == filter.CreatedUserID);

        //        TextBlock tbCreatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbCreatedByName");
        //        TextBlock tbUpdatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbUpdatedByName");
        //        TextBlock tbFilterType = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbFilterType");
        //        TextBlock tbProduct = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbProduct");
        //        TextBlock tbBrand = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbBrand");
        //        TextBlock tbFilterCategory = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbFilterCategory");
        //        TextBlock tbQuestionCategoryID = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbQuestionCategoryID");

        //        #region User
        //        if (tbCreatedBy != null)
        //        {
        //            if (user != null)
        //            {
        //                if (filter.UpdatedUserID.HasValue == true && filter.CreatedUserID == filter.UpdatedUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = user.FullName;
        //                }
        //                else if (filter.UpdatedUserID.HasValue == false && filter.CreatedUserID != filter.UpdatedUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = string.Empty;
        //                }
        //                else if (filter.UpdatedUserID.HasValue == true || filter.CreatedUserID != filter.UpdatedUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    KMPlatform.Entity.User updatedUser = kmUsers.Single(x => x.UserID == filter.UpdatedUserID);
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = updatedUser.FullName;
        //                }
        //            }
        //        }
        //        #endregion
        //        #region Product
        //        if (tbProduct != null && products != null)
        //        {
        //            FrameworkUAD.Entity.Product prod = products.FirstOrDefault(x => x.PubID == filter.PubID);
        //            if (prod != null)
        //                tbProduct.Text = prod.PubCode;

        //        }
        //        #endregion
        //        #region Brand
        //        if (tbBrand != null && brands != null)
        //        {
        //            FrameworkUAD.Entity.Brand brand = brands.FirstOrDefault(x => x.BrandID == filter.BrandID);
        //            if (brand != null)
        //                tbProduct.Text = brand.BrandName;

        //        }
        //        #endregion
        //        #region FilterCategory
        //        if (tbFilterCategory != null && filterCategory != null)
        //        {
        //            FrameworkUAD.Entity.FilterCategory fc = filterCategory.FirstOrDefault(x => x.FilterCategoryID == filter.FilterCategoryID);
        //            if (fc != null)
        //                tbProduct.Text = fc.CategoryName;

        //        }
        //        #endregion
        //        #region QuestionCategory
        //        if (tbQuestionCategoryID != null && questionCategory != null)
        //        {
        //            FrameworkUAD.Entity.QuestionCategory qc = questionCategory.FirstOrDefault(x => x.QuestionCategoryID == filter.QuestionCategoryID);
        //            if (qc != null)
        //                tbProduct.Text = qc.CategoryName;

        //        }
        //        #endregion
        //    }
        //}
        //private void rdForm_Loaded(object sender, RoutedEventArgs e)
        //{
        //    RadDataForm rdForm = sender as RadDataForm;

        //    FilterContainer myFilter = (FilterContainer)rdForm.DataContext;

        //    #region Pub
        //    DataFormComboBoxField cbPub = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbPub");
        //    if (cbPub != null)
        //    {
        //        cbPub.ItemsSource = Products;
        //        cbPub.SelectedValuePath = "PubID";
        //        cbPub.DisplayMemberPath = "PubName";
        //        FrameworkUAD.Entity.Product myProd = Products.FirstOrDefault(x => x.PubID == myFilter.PubID);
        //        if (myProd != null)
        //            cbPub.SelectedIndex = Products.IndexOf(myProd);

        //    }
        //    #endregion
        //    #region Brand
        //    DataFormComboBoxField cbBrand = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbBrand");
        //    if (cbBrand != null)
        //    {
        //        cbBrand.ItemsSource = brands;
        //        cbBrand.SelectedValuePath = "BrandID";
        //        cbBrand.DisplayMemberPath = "BrandName";
        //        FrameworkUAD.Entity.Brand myBrand = brands.FirstOrDefault(x => x.BrandID == myFilter.BrandID);
        //        if (myBrand != null)
        //            cbBrand.SelectedIndex = brands.IndexOf(myBrand);

        //    }
        //    #endregion
        //    #region Filter Category
        //    DataFormComboBoxField cbFilterCategory = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbFilterCategory");
        //    if (cbFilterCategory != null)
        //    {
        //        cbFilterCategory.ItemsSource = filterCategory;
        //        cbFilterCategory.SelectedValuePath = "FilterCategoryID";
        //        cbFilterCategory.DisplayMemberPath = "CategoryName";
        //        FrameworkUAD.Entity.FilterCategory myFC = filterCategory.FirstOrDefault(x => x.FilterCategoryID == myFilter.FilterCategoryID);
        //        if (myFC != null)
        //            cbFilterCategory.SelectedIndex = filterCategory.IndexOf(myFC);

        //    }
        //    #endregion
        //    #region Question Category
        //    DataFormComboBoxField cbQuestionCategory = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbQuestionCategory");
        //    if (cbQuestionCategory != null)
        //    {
        //        cbQuestionCategory.ItemsSource = questionCategory;
        //        cbQuestionCategory.SelectedValuePath = "QuestionCategoryID";
        //        cbQuestionCategory.DisplayMemberPath = "CategoryName";
        //        FrameworkUAD.Entity.QuestionCategory myQC = questionCategory.FirstOrDefault(x => x.QuestionCategoryID == myFilter.QuestionCategoryID);
        //        if (myQC != null)
        //            cbQuestionCategory.SelectedIndex = questionCategory.IndexOf(myQC);

        //    }
        //    #endregion
        //}

        private void rbNewFilter_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;
            lstEnabled.IsEnabled = false;
            rbNewFilter.IsEnabled = false;
            grdFilter.IsEnabled = false;
            grdFilter.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;            
        }
        //private void grdFilter_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        //{
        //    FrameworkUAD.Entity.Filter myFilter = (FrameworkUAD.Entity.Filter)e.NewObject;
        //}
        #region Add new service

        private void ResetNewWindow()
        {
            tbName.Text = "";
            tbFilterXML.Text = "";
            tbFilterType.Text = "";
            cbPub.SelectedIndex = -1;
            cbIsDeleted.IsChecked = false;
            cbBrand.SelectedIndex = -1;
            cbAddtoSalesView.IsChecked = false;
            cbFilterCategory.SelectedIndex = -1;
            cbQuestionCategory.SelectedIndex = -1;
            tbQuestionName.Text = "";            
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            FrameworkUAD.Entity.Filter newFilter = new FrameworkUAD.Entity.Filter();

            //newFilter.Name = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbName.Text.Trim());
            //newFilter.FilterXML = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbFilterXML.Text.Trim());
            //newFilter.FilterType = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbFilterType.Text.Trim());
            //int pubID = 0;
            //int.TryParse(cbPub.SelectedValue.ToString(), out pubID);
            //newFilter.PubID = pubID;
            //newFilter.IsDeleted = cbIsDeleted.IsChecked.Value;
            //int brandID = 0;
            //int.TryParse(cbBrand.SelectedValue.ToString(), out brandID);
            //newFilter.BrandID = brandID;
            //newFilter.AddtoSalesView = cbAddtoSalesView.IsChecked.Value;
            //int fcID = 0;
            //int.TryParse(cbFilterCategory.SelectedValue.ToString(), out fcID);
            //newFilter.FilterCategoryID = cbFilterCategory.SelectedIndex = -1;
            //int qcID = 0;
            //int.TryParse(cbQuestionCategory.SelectedValue.ToString(), out qcID);
            //newFilter.QuestionCategoryID = cbQuestionCategory.SelectedIndex = -1;
            //newFilter.QuestionName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbQuestionName.Text.Trim());            

            #region Check Values
            //if (string.IsNullOrEmpty(newFilter.Name))
            //{
            //    Core_AMS.Utilities.WPF.MessageError("Must provide a name.");
            //    return;
            //}
            //else
            //{
            //    if (filters.FirstOrDefault(x => x.Name.Equals(newFilter.Name, StringComparison.CurrentCultureIgnoreCase)) != null)
            //    {
            //        Core_AMS.Utilities.WPF.MessageError("Must provide a unique name that hasn't been used.");
            //        return;
            //    }
            //}
           
            #endregion
            
            //newFilter.CreatedUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //newFilter.CreatedDate = DateTime.Now;

            newFilter.FilterID = filterWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, newFilter).Result;
            if (newFilter.FilterID > 0)
            {
                ResetNewWindow();

                //FilterContainer fc = new FilterContainer(newFilter);
                //KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == newFilter.CreatedUserID);
                //if (user != null)
                //    fc.CreatedByName = user.FullName;

                //FrameworkUAD.Entity.Product prod = products.FirstOrDefault(x => x.PubID == newFilter.PubID);
                //if (prod != null)
                //    fc.ProductName = prod.PubName;

                //FrameworkUAD.Entity.Brand brand = brands.FirstOrDefault(x => x.BrandID == newFilter.BrandID);
                //if (brand != null)
                //    fc.BrandName = brand.BrandName;

                //FrameworkUAD.Entity.FilterCategory filterCat = filterCategory.FirstOrDefault(x => x.FilterCategoryID == newFilter.FilterCategoryID);
                //if (filterCat != null)
                //    fc.FilterCategoryName = filterCat.CategoryName;

                //FrameworkUAD.Entity.QuestionCategory questionCat = questionCategory.FirstOrDefault(x => x.QuestionCategoryID == newFilter.QuestionCategoryID);
                //if (questionCat != null)
                //    fc.QuestionCategoryName = questionCat.CategoryName;

                //filterContainer.Add(fc);
                grdFilter.ItemsSource = null;
                grdFilter.ItemsSource = filterContainer;

                filters = filterWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                CloseWindow();
                this.grdFilter.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();
            
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstEnabled.IsEnabled = true;
            rbNewFilter.IsEnabled = true;
            grdFilter.IsEnabled = true;
            grdFilter.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            CloseWindow();
            this.grdFilter.SelectedItem = null;
        }
        #endregion

    private DependencyObject FindParentControl<T>(DependencyObject control)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(control);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        private List<ControlObject> RadGridInformation(DependencyObject control)
        {
            List<ControlObject> objects = new List<ControlObject>();
            object a = FindParentControl<Grid>(control);
            if (a != null && a.GetType() == typeof(Grid))
            {
                Grid aa = (Grid)a;
                foreach (object dobj in aa.Children)
                {
                    //object o = FindChildControl<StackPanel>(aa);
                    if (dobj != null && dobj.GetType() == typeof(StackPanel))
                    {
                        StackPanel sp = (StackPanel)dobj;
                        //object z;

                        foreach (object spDObj in sp.Children)
                        {
                            //z = FindChildControl<Object>(spDObj);
                            if (spDObj != null && spDObj.GetType() == typeof(TextBox))
                            {
                                TextBox t = (TextBox)spDObj;
                                objects.Add(new ControlObject() { Name = t.Name, Value = t.Text });
                            }
                            else if (spDObj != null && spDObj.GetType() == typeof(RadComboBox))
                            {
                                string value = "";
                                RadComboBox cb = (RadComboBox)spDObj;
                                if (cb.SelectedValue == null)
                                    value = "";
                                else
                                    value = cb.SelectedValue.ToString();

                                objects.Add(new ControlObject() { Name = cb.Name, Value = value });
                            }
                        }
                    }
                }
            }

            return objects;
        }
    }

    [Serializable]
    [DataContract]
    public class FilterContainer
    {
        public FilterContainer() { }
        public FilterContainer(FrameworkUAD.Entity.Filter filter)
        {
            //FilterID = filter.FilterID;
            //Name = filter.Name;
            //FilterXML = filter.FilterXML;
            //CreatedDate = filter.CreatedDate;
            //CreatedUserID = filter.CreatedUserID;
            //FilterType = filter.FilterType;
            //PubID = filter.PubID;
            //IsDeleted = filter.IsDeleted;
            //UpdatedDate = filter.UpdatedDate;
            //UpdatedUserID = filter.UpdatedUserID;
            //BrandID = filter.BrandID;
            //AddtoSalesView = filter.AddtoSalesView;
            //FilterCategoryID = filter.FilterCategoryID;
            //QuestionCategoryID = filter.QuestionCategoryID;
            //QuestionName = filter.QuestionName;
            //CreatedByName = string.Empty;
            //FilterTypeName = string.Empty;
            //ProductName = string.Empty;
            //UpdatedByName = string.Empty;
            //BrandName = string.Empty;
            //FilterCategoryName = string.Empty;
            //QuestionCategoryName = string.Empty;

        }
        #region Properties
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string FilterXML { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public string FilterType { get; set; }
        [DataMember]
        public int? PubID { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        [DataMember]
        public bool? AddtoSalesView { get; set; }
        [DataMember]
        public int? FilterCategoryID { get; set; }
        [DataMember]
        public int? QuestionCategoryID { get; set; }
        [DataMember]
        public string QuestionName { get; set; }
        [DataMember]
        public string CreatedByName { get; set; }
        [DataMember]
        public string FilterTypeName { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public string FilterCategoryName { get; set; }
        [DataMember]
        public string QuestionCategoryName { get; set; }
        #endregion
    }
}
