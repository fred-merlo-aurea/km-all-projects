using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FilterControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FilterWrapper : UserControl, INotifyPropertyChanged
    {
        #region Properties
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IFilter> filterW = FrameworkServices.ServiceClient.UAD_FilterClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        private UserControl _content;
        private FrameworkUAD.Entity.Filter _selectedFilter = new FrameworkUAD.Entity.Filter();
        private ObservableCollection<FrameworkUAD.Entity.Filter> _filters = new ObservableCollection<FrameworkUAD.Entity.Filter>();
        public ObservableCollection<FrameworkUAD.Entity.Filter> Filters { get { return _filters; } }
        public FrameworkUAD.Entity.Filter SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    OnPropertyChanged("SelectedFilter");
                }
            }
        }
        private Framework.FiltersViewModel instance;
        public Framework.FiltersViewModel MyViewModel
        {
            get
            {
                if (instance == null)
                {
                    instance = new Framework.FiltersViewModel();
                    instance.Initialize();
                }
                return instance;
            }
        }
        #endregion

        public FilterWrapper(UserControl uc)
        {
            InitializeComponent();
            Framework.FiltersViewModel vm = new Framework.FiltersViewModel();
            vm.Initialize();
            instance = vm;

            _content = uc;
            dpContent.Children.Clear();
            dpContent.Children.Add(_content);
        }
        public void SetDefaultStandardFilters()
        {
            foreach (FilterControls.Framework.Filters filter in MyViewModel.Filters)
            {
                foreach (FilterControls.Framework.FilterObject fo in filter.Objects)
                    fo.RemoveSelection();
            }

            foreach (Framework.Filters filter in MyViewModel.Filters.Where(x => x.FilterType == Framework.Enums.Filters.Standard))
            {
                try
                {
                    #region Category
                    foreach (FilterControls.Framework.ListObject lo in filter.Objects.Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCodeType).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
                    {
                        if (lo.DisplayValue.Equals("Qualified Free") || lo.DisplayValue.Equals("Qualified Paid"))
                            lo.Selected = true;
                        else
                            lo.Selected = false;
                    }

                    foreach (FilterControls.Framework.ListObject lo in filter.Objects.Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCode).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
                    {
                        if (lo.DisplayValue.Contains("70") || lo.DisplayValue.Contains("71"))
                            lo.Selected = false;
                    }
                    #endregion

                    #region Trans
                    //Framework.FilterObject tranCodeType = filter.Objects.Where(x => x.Name == Framework.Enums.FilterObjects.TransactionCodeType).FirstOrDefault();
                    //Framework.ListFilterObject tranCodes = (Framework.ListFilterObject)tranCodeType;
                    //foreach (Framework.ListObject lo in tranCodes.Options)
                    //{
                    //    if (lo.DisplayValue.Equals("Free Active") || lo.DisplayValue.Equals("Paid Active"))
                    //        lo.Selected = true;
                    //    else
                    //        lo.Selected = false;
                    //}
                    //tranCodeType.SetSelection(tranCodes);
                    foreach (FilterControls.Framework.ListObject lo in filter.Objects.Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.TransactionCodeType).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
                    {
                        if (lo.DisplayValue.Equals("Free Active") || lo.DisplayValue.Equals("Paid Active"))
                            lo.Selected = true;
                        else
                            lo.Selected = false;
                    }
                    #endregion
                }
                catch(Exception ex)
                {
                    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                }

                if (panel.Filter != null && panel.Filter.FilterType == Framework.Enums.Filters.Standard)
                    panel.Filter = filter;
            }
        }
        private void Load_Filters(object sender, RoutedEventArgs e)
        {
            List<Framework.FilterObject> files = new List<Framework.FilterObject>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                files = jf.FromJson<List<FilterControls.Framework.FilterObject>>(_selectedFilter.FilterDetails);
            }
            catch { }

            for (int i = this.MyViewModel.ActiveFilters.Count - 1; i >= 0; i--)
            {
                if (this.MyViewModel.ActiveFilters.ElementAtOrDefault(i) != null)
                {
                    Framework.FilterObject fo = this.MyViewModel.ActiveFilters[i];
                    fo.RemoveSelection();
                }
            }
            List<Framework.FilterObject> catTypeFiles = new List<Framework.FilterObject>();
            List<Framework.FilterObject> catCodeFiles = new List<Framework.FilterObject>();
            List<Framework.FilterObject> tranTypeFiles = new List<Framework.FilterObject>();
            List<Framework.FilterObject> tranCodeFiles = new List<Framework.FilterObject>();
            
            //catTypeFiles = this.MyViewModel.Filters.Where(x => x.FilterType == Framework.Enums.Filters.).FirstOrDefault();
            catTypeFiles.AddRange(files.Where(x => x.Name == Framework.Enums.FilterObjects.CategoryCodeType));
            catCodeFiles.AddRange(files.Where(x => x.Name == Framework.Enums.FilterObjects.CategoryCode));
            tranTypeFiles.AddRange(files.Where(x => x.Name == Framework.Enums.FilterObjects.TransactionCodeType));
            tranCodeFiles.AddRange(files.Where(x => x.Name == Framework.Enums.FilterObjects.TransactionCode));
            catTypeFiles.ForEach(x => files.Remove(x));
            catCodeFiles.ForEach(x => files.Remove(x));
            tranTypeFiles.ForEach(x => files.Remove(x));
            tranCodeFiles.ForEach(x => files.Remove(x));

            foreach (Framework.FilterObject fo in catTypeFiles)
            {
                Framework.Filters filter = this.MyViewModel.Filters.Where(x => x.FilterType == fo.FilterType).FirstOrDefault();
                Framework.FilterObject obj;
                if (filter.FilterType == Framework.Enums.Filters.AdHoc || filter.FilterType == Framework.Enums.Filters.Demographic)
                    obj = filter.Objects.Where(x => x.DisplayName == fo.DisplayName).FirstOrDefault();
                else
                    obj = filter.Objects.Where(x => x.Name == fo.Name).FirstOrDefault();
                obj.SetSelection(fo);
            }
            
            foreach (Framework.FilterObject fo in catCodeFiles)
            {
                Framework.Filters filter = this.MyViewModel.Filters.Where(x => x.FilterType == fo.FilterType).FirstOrDefault();
                Framework.FilterObject obj;
                if (filter.FilterType == Framework.Enums.Filters.AdHoc || filter.FilterType == Framework.Enums.Filters.Demographic)
                    obj = filter.Objects.Where(x => x.DisplayName == fo.DisplayName).FirstOrDefault();
                else
                    obj = filter.Objects.Where(x => x.Name == fo.Name).FirstOrDefault();
                obj.SetSelection(fo, true);
            }

            foreach (Framework.FilterObject fo in tranTypeFiles)
            {
                Framework.Filters filter = this.MyViewModel.Filters.Where(x => x.FilterType == fo.FilterType).FirstOrDefault();
                Framework.FilterObject obj;
                if (filter.FilterType == Framework.Enums.Filters.AdHoc || filter.FilterType == Framework.Enums.Filters.Demographic)
                    obj = filter.Objects.Where(x => x.DisplayName == fo.DisplayName).FirstOrDefault();
                else
                    obj = filter.Objects.Where(x => x.Name == fo.Name).FirstOrDefault();
                obj.SetSelection(fo);
            }
            
            foreach (Framework.FilterObject fo in tranCodeFiles)
            {
                Framework.Filters filter = this.MyViewModel.Filters.Where(x => x.FilterType == fo.FilterType).FirstOrDefault();
                Framework.FilterObject obj;
                if (filter.FilterType == Framework.Enums.Filters.AdHoc || filter.FilterType == Framework.Enums.Filters.Demographic)
                    obj = filter.Objects.Where(x => x.DisplayName == fo.DisplayName).FirstOrDefault();
                else
                    obj = filter.Objects.Where(x => x.Name == fo.Name).FirstOrDefault();
                obj.SetSelection(fo, true);
            }


            foreach (Framework.FilterObject fo in files)
            {
                Framework.Filters filter = this.MyViewModel.Filters.Where(x => x.FilterType == fo.FilterType).FirstOrDefault();
                Framework.FilterObject obj;
                if (filter.FilterType == Framework.Enums.Filters.AdHoc || filter.FilterType == Framework.Enums.Filters.Demographic)
                    obj = filter.Objects.Where(x => x.DisplayName == fo.DisplayName).FirstOrDefault();
                else
                    obj = filter.Objects.Where(x => x.Name == fo.Name).FirstOrDefault();
                obj.SetSelection(fo);
            }

            this.SelectedFilter = null;            
            btnLoadFilters.IsChecked = false;
        }

        private void Save_Filters(object sender, RoutedEventArgs e)
        {
            int result = 0;

            if (MyViewModel.ActiveFilters.Count == 0)
            {
                MessageBox.Show("Please select at least one criteria to save a filter.");
                return;
            }

            if (MyViewModel.SavedFilters.Select(x => x.FilterName).Contains(txtFilterName.Text))
            {
                MessageBoxResult msgResult = MessageBox.Show("A filter with that name already exists. Do you want to overwrite the existing filter?", "Overwrite Filter?", MessageBoxButton.YesNo);
                if (msgResult == MessageBoxResult.Yes)
                {
                    FrameworkUAD.Entity.Filter f = MyViewModel.SavedFilters.Where(x => x.FilterName == txtFilterName.Text).FirstOrDefault();
                    int index = MyViewModel.SavedFilters.IndexOf(f);
                    result = filterW.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, f).Result;
                    if (result > 0)
                    {
                        MyViewModel.SavedFilters.RemoveAt(index);
                        f.FilterDetails = MyViewModel.ActiveFiltersJSON;
                        filterW.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, f);
                        MyViewModel.SavedFilters.Insert(index, f);
                    }
                }
                else
                    return;
            }
            else
            {
                FrameworkUAD.Entity.Filter filter = new FrameworkUAD.Entity.Filter();
                filter.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                filter.DateCreated = DateTime.Now;
                filter.FilterName = txtFilterName.Text;
                filter.FilterDetails = MyViewModel.ActiveFiltersJSON;
                filter.ProductID = MyViewModel.SelectedPubID;
                filter.FilterID = filterW.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, filter).Result;
                result = filter.FilterID;
                if (filter.FilterID > 0)
                {
                    if (!MyViewModel.SavedFilters.Select(x => x.FilterID).Contains(result))
                        MyViewModel.SavedFilters.Add(filter);
                }
            }
            if (result > 0)
                Core_AMS.Utilities.WPF.Message("Filter saved successfully.");
            else
                Core_AMS.Utilities.WPF.MessageError("There was a problem saving your filter.");

            txtFilterName.Text = "";
            btnSaveFilters.IsChecked = false;
        }

        private void Clear_Filters(object sender, RoutedEventArgs e)
        {
            for (int i = this.MyViewModel.ActiveFilters.Count - 1; i >= 0; i--)
            {
                if (this.MyViewModel.ActiveFilters.ElementAtOrDefault(i) != null)
                {
                    Framework.FilterObject fo = this.MyViewModel.ActiveFilters[i];
                    fo.RemoveSelection();
                }
            }
        }

        private void Detach_Filters(object sender, RoutedEventArgs e)
        {
            foreach (Framework.Filters f in MyViewModel.Filters.Where(x => x.Detached == false))
            {
                f.Detached = true;
                PopFilterWindow pop = new PopFilterWindow(f);
                pop.Closed += (o, i) =>
                {
                    pop.Filter.Detached = false;
                };
                pop.Show();
            }
        }

        private void Attach_Filters(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is PopFilterWindow)
                {
                    w.Close();
                }
            }
        }

        private void LoadDefault_Filters(object sender, RoutedEventArgs e)
        {
            //Framework.FiltersViewModel vm = new Framework.FiltersViewModel();
            //vm.Initialize();
            //instance = vm;
            List<Expander> expanders = Core_AMS.Utilities.WPF.FindVisualChildren<Expander>(panel).ToList();
            Dictionary<string, bool> originalExpanders = new Dictionary<string, bool>();
            foreach (Expander ex in expanders)
            {
                if (ex.HasHeader == true)
                    originalExpanders.Add(ex.Header.ToString(), ex.IsExpanded);
            }
            SetDefaultStandardFilters();
            panel.SetExpandersByHeader(originalExpanders);
            //panel.Dispatcher.Invoke(null, System.Windows.Threading.DispatcherPriority.Render);
        }
    }
}
