using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace FilterControls.Framework
{
    public class FiltersViewModel : INotifyPropertyChanged
    {
        //Viewmodel is used as a DataContext throughout the Filters module. It instantiates a list of Filters and listens to changes on their FilterObjects so it can
        //correctly add and remove them from the ActiveFilters collection. It also keeps track of the number of Detached Filters, for use with a MultiConverter.
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        private int _selectedPubID;
        private int _detachedFiltersCount;
        public int SelectedPubID
        {
            get { return _selectedPubID; }
            set
            {
                if (_selectedPubID != value)
                {
                    _selectedPubID = value;
                    if (_selectedPubID > 0)
                    {
                        Initialize(_selectedPubID);
                    }
                    OnPropertyChanged("SelectedPubID");
                }
            }
        }
        public int DetachedFiltersCount
        {
            get { return _detachedFiltersCount; }
            set
            {
                if (_detachedFiltersCount != value)
                {
                    _detachedFiltersCount = value;
                    OnPropertyChanged("DetachedFiltersCount");
                }
            }
        }
        private ObservableCollection<Framework.Filters> _filters = new ObservableCollection<Filters>();
        private ObservableCollection<Framework.FilterObject> _activeFilters = new ObservableCollection<FilterObject>();
        private ObservableCollection<FrameworkUAD.Entity.Filter> _savedFilters = new ObservableCollection<FrameworkUAD.Entity.Filter>();
        public ObservableCollection<Framework.Filters> Filters { get { return _filters; } }
        public ObservableCollection<Framework.FilterObject> ActiveFilters { get { return _activeFilters; } }
        public ObservableCollection<FrameworkUAD.Entity.Filter> SavedFilters { get { return _savedFilters; } }
        
        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Framework.FilterObject item = sender as Framework.FilterObject;
            if (item != null && item.Active == true && !ActiveFilters.Contains(item))
                ActiveFilters.Add(item);
            else if (item != null && item.Active == false)
                ActiveFilters.Remove(item);
        }
        private void OnFilterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Framework.Filters item = sender as Framework.Filters;
            if (item != null && item.Detached)
                this.DetachedFiltersCount++;
            else if (item != null && !item.Detached)
                this.DetachedFiltersCount--;
        }
        public void Initialize()
        {
            ClearFilters();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is PopFilterWindow)
                    w.Close();
            }
            _savedFilters.Clear();
            _filters.Clear();
            _activeFilters.Clear();
            _filters.Add(new StandardFilters());
            _filters.Add(new ContactFilters());
            _filters.Add(new PermissionFilters());
            _filters.Add(new AdHocFilters());

            foreach(Framework.Filters f in _filters)
            {
                f.PropertyChanged += OnFilterPropertyChanged;
                foreach (Framework.FilterObject fo in f.Objects)
                    fo.PropertyChanged += OnItemPropertyChanged;
            }

            FrameworkServices.ServiceClient<UAD_WS.Interface.IFilter> filterW = FrameworkServices.ServiceClient.UAD_FilterClient();
            FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>>();
            filterResponse = filterW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
                filterResponse.Result.ForEach(x => _savedFilters.Add(x));
        }
        public void Initialize(int productID)
        {
            ClearFilters();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is PopFilterWindow)
                    w.Close();
            }
            _savedFilters.Clear();
            _filters.Clear();
            _activeFilters.Clear();
            _filters.Add(new StandardFilters());
            _filters.Add(new ContactFilters());
            _filters.Add(new PermissionFilters());
            _filters.Add(new DemographicFilters(productID));
            _filters.Add(new AdHocFilters(productID));
            _selectedPubID = productID;

            foreach (Framework.Filters f in _filters)
            {
                f.PropertyChanged += OnFilterPropertyChanged;
                foreach (Framework.FilterObject fo in f.Objects)
                    fo.PropertyChanged += OnItemPropertyChanged;
            }

            FrameworkServices.ServiceClient<UAD_WS.Interface.IFilter> filterW = FrameworkServices.ServiceClient.UAD_FilterClient();
            FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>>();
            filterResponse = filterW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
            {
                filterResponse.Result.Where(x => x.ProductID == productID).ToList().ForEach(x => SavedFilters.Add(x));
            }
        }
        //ActiveFiltersXML is used for sending Filters to stored procedures to get lists of Subscribers back (Reports, Splits, etc).
        public FrameworkUAD.Object.ReportingXML ActiveFiltersXML
        {
            get
            {
                FrameworkUAD.Object.ReportingXML obj = new FrameworkUAD.Object.ReportingXML();
                string xml = "<XML><Filters><ProductID>" + _selectedPubID + "</ProductID>";
                this.ActiveFilters.Where(x=> x.FilterType != Enums.Filters.Demographic && x.FilterType != Enums.Filters.AdHoc).ToList().ForEach(x => xml += x.GetSelectionXml());
                if (this.ActiveFilters.Where(x => x.FilterType == Enums.Filters.Demographic).ToList().Count > 0)
                {
                    xml += "<Responses>";
                    this.ActiveFilters.Where(x => x.FilterType == Enums.Filters.Demographic).ToList().ForEach(x => xml += x.GetSelectionXml() + ",");
                    xml = xml.TrimEnd(',');
                    xml += "</Responses>";
                }
                xml += "</Filters></XML>";
                string adHocXml = "<XML>";
                this.ActiveFilters.Where(x => x.FilterType == Enums.Filters.AdHoc).ToList().ForEach(x => adHocXml += x.GetSelectionXml());
                adHocXml += "</XML>";
                obj.Filters = xml;
                obj.AdHocFilters = adHocXml;
                return obj;
            }
        }
        //ActiveFitlersJSON is the format we use to save our filters to the DB.
        public string ActiveFiltersJSON
        {
            get
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string json = jf.ToJson<List<Framework.FilterObject>>(this.ActiveFilters.ToList());
                return json;
            }
        }
        public void ClearFilters()
        {
            foreach (FilterControls.Framework.Filters filter in this.Filters)
            {
                foreach (FilterControls.Framework.FilterObject fo in filter.Objects)
                    fo.RemoveSelection();
            }
        }
    }
}
