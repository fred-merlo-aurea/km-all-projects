using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for AdHocs.xaml
    /// </summary>
    public partial class AdHocs : UserControl, INotifyPropertyChanged
    {
        //AdHocs exists in a tab within SubscriptionContainer. This data is pulled from PubSubscriptionsExtension and PubSubscriptionsExtensionMapper.
        private bool _adHocChanged = false;
        private bool _enabled = false;
        private ObservableCollection<AdHocContainer> _adhocs = new ObservableCollection<AdHocContainer>();
        private ObservableCollection<AdHocContainer> originalAdHocs = new ObservableCollection<AdHocContainer>();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubscriptionW = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkUAS.Service.Response<List<string>> stringResponse;
        public ObservableCollection<AdHocContainer> AdHocFields { get { return _adhocs; } }
        public bool AdHocChanged
        {
            get { return _adHocChanged; }
            set
            {
                _adHocChanged = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AdHocChanged"));
                }
            }
        }
        private bool MadeChange()
        {
            foreach (AdHocContainer ahc in _adhocs)
            {
                AdHocContainer a = originalAdHocs.Where(x => x.AdHocField == ahc.AdHocField).FirstOrDefault();
                if (a != null)
                {
                    if (a.Value != ahc.Value)
                        return true;
                }
            }
            return false;
        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Enabled"));
                }
            }
        }
        public class AdHocContainer : INotifyPropertyChanged
        {
            private string _adHocField;
            private string _value;
            public string AdHocField
            {
                get { return _adHocField; }
                set
                {
                    _adHocField = value;
                }
            }
            public string Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                    }
                }
            }

            public AdHocContainer(string field, string value)
            {
                this.AdHocField = field;
                this.Value = value;
            }

            public FrameworkUAD.Object.PubSubscriptionAdHoc GetModel()
            {
                FrameworkUAD.Object.PubSubscriptionAdHoc mod = new FrameworkUAD.Object.PubSubscriptionAdHoc(this.AdHocField, this.Value);
                return mod;
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (AdHocContainer newItem in e.NewItems)
                {
                    //Add listener for each item on PropertyChanged event
                    newItem.PropertyChanged += this.OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (AdHocContainer oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= this.OnItemPropertyChanged;
                }
            }
        }
        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AdHocContainer item = sender as AdHocContainer;
            if (item != null)
                this.AdHocChanged = MadeChange();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public FrameworkUAD.Entity.ProductSubscription MyProductSubscription;
        public AdHocs(FrameworkUAD.Entity.ProductSubscription prdSubscription)
        {
            InitializeComponent();
            MyProductSubscription = prdSubscription;
            _adhocs.CollectionChanged += OnCollectionChanged;
            stringResponse = productSubscriptionW.Proxy.Get_AdHocs(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, prdSubscription.PubID, 
                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            List<string> adhocs = new List<string>();
            if(Helpers.Common.CheckResponse(stringResponse.Result, stringResponse.Status))
            {
                adhocs = stringResponse.Result;
            }
            if (MyProductSubscription.AdHocFields != null)
            {
                List<string> missing = adhocs.Except(MyProductSubscription.AdHocFields.Select(x => x.AdHocField)).ToList();
                missing.ForEach(x => MyProductSubscription.AdHocFields.Add(new FrameworkUAD.Object.PubSubscriptionAdHoc(x, "")));
                MyProductSubscription.AdHocFields.ForEach(x => this.AdHocFields.Add(new AdHocContainer(x.AdHocField, x.Value)));
                MyProductSubscription.AdHocFields.ForEach(x => originalAdHocs.Add(new AdHocContainer(x.AdHocField, x.Value)));
                this.icAdHocFields.ItemsSource = AdHocFields;
            }
        }
    }
}
