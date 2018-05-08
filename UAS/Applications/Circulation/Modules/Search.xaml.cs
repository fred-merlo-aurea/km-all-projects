using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Circulation.Helpers;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Collections.ObjectModel;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        #region Entity/List
        private List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        private List<FrameworkUAD_Lookup.Entity.Region> regions = new List<FrameworkUAD_Lookup.Entity.Region>();
        private List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
        private List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();
        private List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        private FrameworkUAD.Entity.ProductSubscription waveMailSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private FrameworkUAD.Entity.WaveMailingDetail myWMDetail = new FrameworkUAD.Entity.WaveMailingDetail();
        private FrameworkUAD_Lookup.Entity.Country selCountryPhonePrefix = new FrameworkUAD_Lookup.Entity.Country();
        private FrameworkUAD.Entity.ProductSubscription waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription();
        private List<FrameworkUAD.Entity.Subscription> subscriptionList = new List<FrameworkUAD.Entity.Subscription>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> categoryCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sstList = new List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>();
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> ssmList = new List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>();
        private List<FrameworkUAD_Lookup.Entity.Code> qSourceList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> parList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> marketingList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> addressTypeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> productSubDetailList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private List<FrameworkUAD_Lookup.Entity.ZipCode> zipList = new List<FrameworkUAD_Lookup.Entity.ZipCode>();
        public ObservableCollection<FrameworkUAD.Entity.ProductSubscription> productSubscriptions = new ObservableCollection<FrameworkUAD.Entity.ProductSubscription>();
        private BackgroundWorker bw;
        private int paidID = 0;
        private int freeID = 0;
        private bool saveWaveMailing = false;
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private int applicationID = 0;
        #endregion
        #region Workers
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> cWorker = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> rWorker = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionSearchResult> ssrWorker = FrameworkServices.ServiceClient.UAD_SubscriptionSearchResultClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IHistory> hWorker = FrameworkServices.ServiceClient.UAD_HistoryClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> bWorker = FrameworkServices.ServiceClient.UAD_BatchClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IWaveMailingDetail> wMDetailWorker = FrameworkServices.ServiceClient.UAD_WaveMailingDetailClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> aWorker = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catCodeTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> tcWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> tcTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> ccWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> sstWorker = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatusMatrix> ssmWorker = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusMatrixClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> productSubDetailWorker = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IZipCode> zipCodeW = FrameworkServices.ServiceClient.UAD_Lookup_ZipCodeClient();
        private FrameworkServices.ServiceClient<SubGen_WS.Interface.ISubGenUtils> subgenW = FrameworkServices.ServiceClient.SubGen_SubGenUtilsClient();
        #endregion
        #region Service Response
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Object.SubscriptionSearchResult>> ssrResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Object.SubscriptionSearchResult>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.History>> historyResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.History>>();
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> prodSubscriptionResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>> subscriptionListResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>>();
        private FrameworkUAS.Service.Response<int> batchIDResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> sstResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>> ssmResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> catTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transCodeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qSourceResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> parResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> marketingResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> productSubResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> prodSubResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> productSubDetailResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.ZipCode>> zipResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.ZipCode>>();
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product> singProdResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product>();
        #endregion
        #region Classes & Properties
        public class Subscriber : INotifyPropertyChanged
        {
            #region Private
            private string _firstName = "";
            private string _lastName = "";
            private string _title = "";
            private string _company = "";
            private int _addressType = 0;
            private string _address1 = "";
            private string _address2 = "";
            private string _address3 = "";
            private string _city = "";
            private int _countryID = 0;
            private string _country = "";
            private int _regionID = 0;
            private string _regionCode = "";
            private string _fullZip = "";
            private string _zip = "";
            private string _plus4 = "";
            private string _county = "";
            private string _phone = "";
            private int _phoneCode = 0;
            private string _phoneExt = "";
            private string _mobile = "";
            private string _fax = "";
            private string _email = "";
            private int _emailStatusID = 0;
            private string _website = "";
            private string _clientName = "";
            private string _pubCode = "";
            private string _fullName = "";
            private string _fullAddress = "";
            private string _account = "";
            private int _subscriptionStatus = 0;
            private int _sequenceID = 0;
            private bool _infoChanged = false;
            private bool _addressOnlyChange = false;
            private bool _isSubscribed = false;
            private bool _isOpen = false;
            private bool _isPaid = false;
            private bool _isEnabled = false;
            private bool _isInActiveWaveMailing = false;
            private int _paidAddressChangeOnly = 0;
            private int _freeAddressChangeOnly = 0;
            private int _subscriptionID = 0;
            #endregion
            public string FirstName
            {
                get { return _firstName; }
                set
                {
                    _firstName = value;
                    this.FullName = _firstName + " " + _lastName;
                    CurrentSubscriber.FirstName = _firstName;
                    if (_firstName != OriginalSubscriber.FirstName)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                    }
                }
            }
            public string LastName
            {
                get { return _lastName; }
                set
                {
                    _lastName = value;
                    this.FullName = _firstName + " " + _lastName;
                    CurrentSubscriber.LastName = _lastName;
                    if (_lastName != OriginalSubscriber.LastName)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                    }
                }
            }
            public string Title
            {
                get { return _title; }
                set
                {
                    _title = value;
                    CurrentSubscriber.Title = _title;
                    if (_title != OriginalSubscriber.Title)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                    }
                }
            }
            public string Company
            {
                get { return _company; }
                set
                {
                    _company = value;
                    CurrentSubscriber.Company = _company;
                    if (_company != OriginalSubscriber.Company)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Company"));
                    }
                }
            }
            public int? AddressType
            {
                get { return _addressType; }
                set
                {
                    _addressType = (value ?? 0);
                    CurrentSubscriber.AddressTypeCodeId = _addressType;
                    if (_addressType != OriginalSubscriber.AddressTypeCodeId)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AddressType"));
                    }
                }
            }
            public string Address1
            {
                get { return _address1; }
                set
                {
                    _address1 = value;
                    this.FullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + _fullZip + ", " + _country;
                    CurrentSubscriber.Address1 = _address1;
                    if (_address1 != OriginalSubscriber.Address1)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;

                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Address1"));
                    }
                }
            }
            public string Address2
            {
                get { return _address2; }
                set
                {
                    _address2 = value;
                    this.FullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + _fullZip + ", " + _country;
                    CurrentSubscriber.Address2 = _address2;
                    if (_address2 != OriginalSubscriber.Address2)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;

                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Address2"));
                    }
                }
            }
            public string Address3
            {
                get { return _address3; }
                set
                {
                    _address3 = value;
                    this.FullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + _fullZip + ", " + _country;
                    CurrentSubscriber.Address3 = _address3;
                    if (_address3 != OriginalSubscriber.Address3)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;

                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Address3"));
                    }
                }
            }
            public string City
            {
                get { return _city; }
                set
                {
                    _city = value;
                    this.FullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + _fullZip + ", " + _country;
                    CurrentSubscriber.City = _city;
                    if (_city != OriginalSubscriber.City)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("City"));
                    }
                }
            }
            public int? CountryID
            {
                get { return _countryID; }
                set
                {
                    _countryID = (value ?? 0);
                    if (_countryID != OriginalSubscriber.CountryID)
                    {
                        this.InfoChanged = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CountryID"));
                    }
                }
            }
            public string Country
            {
                get { return _country; }
                set
                {
                    _country = value;
                    this.FullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + _fullZip + ", " + _country;
                    CurrentSubscriber.Country = _country;
                }
            }
            public int? RegionID
            {
                get { return _regionID; }
                set
                {
                    _regionID = (value ?? 0);
                    CurrentSubscriber.RegionID = _regionID;
                    if (_regionID != OriginalSubscriber.RegionID)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                        if (_regionID <= 0 && CurrentSubscriber.RegionCode == null)
                        {
                            CurrentSubscriber.RegionCode = string.Empty;
                            this.RegionCode = string.Empty;
                        }
                        else if (Regions != null)
                        {
                            CurrentSubscriber.RegionCode = Regions.Where(r => r.RegionID == _regionID).Select(s => s.RegionCode).SingleOrDefault();
                            this.RegionCode = CurrentSubscriber.RegionCode;
                        }
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("RegionID"));
                    }
                }
            }
            public string RegionCode
            {
                get { return _regionCode; }
                set
                {
                    _regionCode = value;
                    this.FullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + _fullZip + ", " + _country;
                    CurrentSubscriber.RegionCode = _regionCode;
                }
            }
            public string FullZip
            {
                get { return _fullZip; }
                set
                {
                    _fullZip = (value ?? "").Replace("-", "").Replace(" ", "");
                    this.FullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + _fullZip + ", " + _country;
                    if (string.IsNullOrEmpty(_fullZip.Trim()))
                    {
                        this.Zip = "";
                        this.Plus4 = "";
                    }
                    else if (_country.ToUpper() == "UNITED STATES")
                    {
                        string zip = "";
                        string plus4 = "";
                        for (int i = 0; i < _fullZip.Length; i++)
                        {
                            if (i < 5)
                                zip += _fullZip[i].ToString();
                            else
                                plus4 += _fullZip[i].ToString();
                        }
                        this.Zip = zip;
                        this.Plus4 = plus4;
                    }
                    else
                    {
                        this.Zip = _fullZip;
                        this.Plus4 = "";
                    }
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FullZip"));
                    }
                }
            }
            public string Zip
            {
                get { return _zip; }
                set
                {
                    _zip = value;
                    CurrentSubscriber.ZipCode = _zip;
                    if (_zip != OriginalSubscriber.ZipCode)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Zip"));
                    }
                }
            }
            public string Plus4
            {
                get { return _plus4; }
                set
                {
                    _plus4 = value;
                    CurrentSubscriber.Plus4 = _plus4;
                    if (_plus4 != OriginalSubscriber.Plus4)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Plus4"));
                    }
                }
            }
            public string County
            {
                get { return _county; }
                set
                {
                    _county = (value.Trim() ?? "");
                    CurrentSubscriber.County = _county;
                    if (_county != OriginalSubscriber.County)
                    {
                        this.InfoChanged = true;
                        this.AddressOnlyChange = true;
                    }
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("County"));
                    }
                }
            }
            public string Phone
            {
                get { return _phone; }
                set
                {
                    _phone = (value.Trim() ?? "");
                    CurrentSubscriber.Phone = _phone;
                    if (_phone != OriginalSubscriber.Phone.Trim().Replace("-", ""))
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
                    }
                }
            }
            public int PhoneCode
            {
                get { return _phoneCode; }
                set
                {
                    _phoneCode = value;
                    CurrentSubscriber.PhoneCode = _phoneCode;
                    if (_phoneCode != OriginalSubscriber.PhoneCode)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PhoneCode"));
                    }
                }
            }
            public string PhoneExt
            {
                get { return _phoneExt; }
                set
                {
                    _phoneExt = (value.Trim() ?? "");
                    CurrentSubscriber.PhoneExt = _phoneExt;
                    if (_phoneExt != OriginalSubscriber.PhoneExt)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PhoneExt"));
                    }
                }
            }
            public string Mobile
            {
                get { return _mobile; }
                set
                {
                    _mobile = (value.Trim() ?? "");
                    CurrentSubscriber.Mobile = _mobile;
                    if (_mobile != OriginalSubscriber.Mobile.Trim().Replace("-", ""))
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Mobile"));
                    }
                }
            }
            public string Fax
            {
                get { return _fax; }
                set
                {
                    _fax = (value.Trim() ?? "");
                    CurrentSubscriber.Fax = _fax;
                    if (_fax != OriginalSubscriber.Fax.Trim().Replace("-", ""))
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Fax"));
                    }
                }
            }
            public string Email
            {
                get { return _email; }
                set
                {
                    _email = (value.Trim() ?? "");
                    if (_email != "")
                    {
                        if (_email != this.OriginalSubscriber.Email)
                            this.EmailStatusID = 1;
                        else
                            this.EmailStatusID = this.OriginalSubscriber.EmailStatusID;
                    }
                    else
                        this.EmailStatusID = 0;
                    CurrentSubscriber.Email = _email;
                    if (_email != OriginalSubscriber.Email)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                    }
                }
            }
            public int EmailStatusID
            {
                get { return _emailStatusID; }
                set
                {
                    _emailStatusID = value;
                    CurrentSubscriber.EmailStatusID = _emailStatusID;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("EmailStatusID"));
                    }
                }
            }
            public string Website
            {
                get { return _website; }
                set
                {
                    _website = (value.Trim() ?? "");
                    CurrentSubscriber.Website = _website;
                    if (_website != OriginalSubscriber.Website)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Website"));
                    }
                }
            }
            public int SubscriptionStatus
            {
                get { return _subscriptionStatus; }
                set
                {
                    _subscriptionStatus = value;
                    CurrentSubscriber.SubscriptionStatusID = _subscriptionStatus;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SubscriptionStatus"));
                    }
                }
            }
            public int SequenceID
            {
                get { return _sequenceID; }
                set
                {
                    _sequenceID = value;
                    CurrentSubscriber.SequenceID = _sequenceID;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SequenceID"));
                    }
                }
            }
            public string ClientName
            {
                get { return _clientName; }
                set
                {
                    _clientName = (value.Trim() ?? "");
                    CurrentSubscriber.ClientName = _clientName;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ClientName"));
                    }
                }
            }
            public string FullName
            {
                get { return _fullName; }
                set
                {
                    _fullName = (value.Trim() ?? "");
                    CurrentSubscriber.FullName = _fullName;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FullName"));
                    }
                }
            }
            public string FullAddress
            {
                get { return _fullAddress; }
                set
                {
                    _fullAddress = (value.Trim() ?? "");
                    CurrentSubscriber.FullAddress = _fullAddress;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FullAddress"));
                    }
                }
            }
            public string Account
            {
                get { return _account; }
                set
                {
                    _account = (value.Trim() ?? "");
                    CurrentSubscriber.AccountNumber = _account;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Account"));
                    }
                }
            }
            public string PubCode
            {
                get { return _pubCode; }
                set
                {
                    _pubCode = (value.Trim() ?? "");
                    CurrentSubscriber.PubCode = _pubCode;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PubCode"));
                    }
                }
            }
            public bool IsInActiveWaveMailing
            {
                get { return _isInActiveWaveMailing; }
                set
                {
                    _isInActiveWaveMailing = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsInActiveWaveMailing"));
                    }
                }
            }
            public bool InfoChanged
            {
                get { return _infoChanged; }
                set
                {
                    _infoChanged = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("InfoChanged"));
                    }
                }
            }
            public bool AddressOnlyChange
            {
                get { return _addressOnlyChange; }
                set
                {
                    if (_addressOnlyChange != value)
                    {
                        _addressOnlyChange = value;
                        if (_addressOnlyChange == true)
                        {
                            if (_isPaid == true)
                            {
                                this.CurrentSubscriber.PubTransactionID = _paidAddressChangeOnly;
                            }
                            else
                            {
                                this.CurrentSubscriber.PubTransactionID = _freeAddressChangeOnly;
                            }
                        }
                        else
                            this.CurrentSubscriber.PubTransactionID = this.OriginalSubscriber.PubTransactionID;
                        if (null != this.PropertyChanged)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("AddressOnlyChange"));
                        }
                    }
                }
            }
            public bool IsSubscribed
            {
                get { return _isSubscribed; }
                set
                {
                    _isSubscribed = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSubscribed"));
                    }
                }
            }
            public bool IsEnabled
            {
                get { return _isEnabled; }
                set
                {
                    if (_isEnabled != value)
                    {
                        _isEnabled = value;
                        if (null != this.PropertyChanged)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("IsEnabled"));
                        }
                    }
                }
            }
            public bool IsOpen
            {
                get { return _isOpen; }
                set
                {
                    if (_isOpen != value)
                    {
                        _isOpen = value;
                        //this.IsLocked = _isOpen;
                        if (null != this.PropertyChanged)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("IsOpen"));
                        }
                    }
                }
            }
            public bool IsPaid
            {
                get { return _isPaid; }
                set
                {
                    if (_isPaid != value)
                    {
                        _isPaid = value;
                        if (null != this.PropertyChanged)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("IsPaid"));
                        }
                    }
                }
            }
            public List<FrameworkUAD_Lookup.Entity.Region> Regions { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Country> Countries { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Code> AddressTypes { get; set; }
            public FrameworkUAD.Entity.ProductSubscription OriginalSubscriber { get; set; }
            public FrameworkUAD.Entity.ProductSubscription CurrentSubscriber { get; set; }
             public int SubscriptionID
            {
                get { return _subscriptionID; }
                set
                {
                    _sequenceID = value;
                    CurrentSubscriber.SubscriptionID = _sequenceID;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SubscriptionID"));
                    }
                }
            }
            private bool CheckChanges()
            {
                bool infoChanged = false;
                bool addressChanged = false;
                if (this.FirstName != OriginalSubscriber.FirstName)
                    infoChanged = true;
                if (this.LastName != OriginalSubscriber.LastName)
                    infoChanged = true;
                if (this.Title != OriginalSubscriber.Title)
                    infoChanged = true;
                if (this.Company != OriginalSubscriber.Company)
                    infoChanged = true;
                if (this.AddressType != OriginalSubscriber.AddressTypeCodeId)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.Address1 != OriginalSubscriber.Address1)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.Address2 != OriginalSubscriber.Address2)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.Address3 != OriginalSubscriber.Address3)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.City != OriginalSubscriber.City)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.CountryID != OriginalSubscriber.CountryID)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.RegionID != OriginalSubscriber.RegionID)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.Zip != OriginalSubscriber.ZipCode)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.Plus4 != OriginalSubscriber.Plus4)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (this.County != OriginalSubscriber.County)
                {
                    infoChanged = true;
                    addressChanged = true;
                }
                if (Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(this.Phone) != Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(OriginalSubscriber.Phone))
                    infoChanged = true;
                if (this.PhoneExt != OriginalSubscriber.PhoneExt)
                    infoChanged = true;
                if (this.PhoneCode != OriginalSubscriber.PhoneCode)
                    infoChanged = true;
                if (Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(this.Mobile) != Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(OriginalSubscriber.Mobile))
                    infoChanged = true;
                if (Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(this.Fax) != Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(OriginalSubscriber.Fax))
                    infoChanged = true;
                if (this.Email != OriginalSubscriber.Email)
                    infoChanged = true;
                if (this.Website != OriginalSubscriber.Website)
                    infoChanged = true;

                this.AddressOnlyChange = addressChanged;

                return infoChanged;
            }
            public event PropertyChangedEventHandler PropertyChanged;

            public Subscriber(FrameworkUAD.Entity.ProductSubscription ps, List<FrameworkUAD_Lookup.Entity.Region> regions, List<FrameworkUAD_Lookup.Entity.Country> countries, 
                List<FrameworkUAD_Lookup.Entity.Code> addressTypes, int addressChangePaid, int addressChangeFree)
            {
                if (ps.CountryID == 0)
                    ps.CountryID = 1;
                this.OriginalSubscriber = new FrameworkUAD.Entity.ProductSubscription(ps);
                this.CurrentSubscriber = new FrameworkUAD.Entity.ProductSubscription(ps);
                this.IsPaid = ps.IsPaid;
                if(ps.CountryID != 1 && ps.CountryID != 2 && ps.CountryID != 429)
                    Regions = regions.Where(x => x.CountryID == ps.CountryID || x.RegionCode == "FO").ToList();
                else
                    Regions = regions.Where(x=> x.CountryID == ps.CountryID).ToList();
                Countries = countries;
                AddressTypes = addressTypes;
                _regionCode = ps.RegionCode;
                _firstName = ps.FirstName;
                _lastName = ps.LastName;
                _title = ps.Title;
                _company = ps.Company;
                _addressType = ps.AddressTypeCodeId;
                _address1 = ps.Address1;
                _address2 = ps.Address2;
                _address3 = ps.Address3;
                _city = ps.City;
                _countryID = ps.CountryID;
                _country = ps.Country;
                _regionID = ps.RegionID;
                _regionCode = ps.RegionCode;
                _fullZip = ps.FullZip;
                _county = ps.County;
                _phone = ps.Phone;
                _phoneExt = ps.PhoneExt;
                _phoneCode = ps.PhoneCode;
                if (ps.CountryID == 2) //Scrub Canadian Zip Codes. Not using Plus4.
                {
                    if (this.OriginalSubscriber.ZipCode.Length <= 3)
                    {
                        this.OriginalSubscriber.ZipCode = this.OriginalSubscriber.ZipCode + this.OriginalSubscriber.Plus4;
                        this.CurrentSubscriber.ZipCode = this.CurrentSubscriber.ZipCode + this.CurrentSubscriber.Plus4;
                    }
                    this.OriginalSubscriber.Plus4 = "";
                    this.CurrentSubscriber.Plus4 = "";
                }
                _zip = ps.ZipCode;
                _plus4 = ps.Plus4;
                _mobile = ps.Mobile;
                _fax = ps.Fax;
                _emailStatusID = ps.EmailStatusID;
                this.Email = ps.Email;
                _website = ps.Website;
                _sequenceID = ps.SequenceID;
                _clientName = ps.ClientName;
                _account = ps.AccountNumber;
                _isSubscribed = ps.IsSubscribed;
                _pubCode = ps.PubCode;
                _fullName = ps.FullName;
                _fullAddress = _address1 + ", " + _address2 + ", " + _address3 + ", " + _city + ", " + _regionCode + " " + ps.FullZip + ", " + _country;
                _isOpen = false;
                _subscriptionStatus = ps.SubscriptionStatusID;
                _isInActiveWaveMailing = ps.IsInActiveWaveMailing;
                _freeAddressChangeOnly = addressChangeFree;
                _paidAddressChangeOnly = addressChangePaid;
                _subscriptionID = ps.SubscriptionID;
            }
        }
        private class SearchDataModel : INotifyPropertyChanged
        {
            #region Private
            private string _firstName = "";
            private string _lastName = "";
            private string _title = "";
            private string _company = "";
            private string _address = "";
            private string _city = "";
            private string _state = "";
            private string _zip = "";
            private string _country = "";
            private string _email = "";
            private string _phone = "";
            private int _sequenceID = 0;
            private string _account = "";
            private int _publisherID = 0;
            private int _publicationID = 0;
            private int _subscriptionID = 0;
            #endregion
            public string FirstName
            {
                get { return _firstName; }
                set
                {
                    _firstName = value;
                    MySearchData.firstName = _firstName;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                    }
                }
            }
            public string LastName
            {
                get { return _lastName; }
                set
                {
                    _lastName = value;
                    MySearchData.lastName = _lastName;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                    }
                }
            }
            public string Company
            {
                get { return _company; }
                set
                {
                    _company = value;
                    MySearchData.Company = _company;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Company"));
                    }
                }
            }
            public string Title
            {
                get { return _title; }
                set
                {
                    _title = value;
                    MySearchData.Title = _title;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                    }
                }
            }
            public string Address
            {
                get { return _address; }
                set
                {
                    _address = value;
                    MySearchData.Address1 = _address;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                    }
                }
            }
            public string City
            {
                get { return _city; }
                set
                {
                    _city = value;
                    MySearchData.City = _city;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("City"));
                    }
                }
            }
            public string State
            {
                get { return _state; }
                set
                {
                    _state = (value ?? "");
                    if (_state == "-1")
                        _state = "";
                    _state = _state.Trim();
                    MySearchData.State = _state;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("State"));
                    }
                }
            }
            public string Zip
            {
                get { return _zip; }
                set
                {
                    _zip = value;
                    MySearchData.Zip = _zip;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Zip"));
                    }
                }
            }
            public string Country
            {
                get { return _country; }
                set
                {
                    _country = (value ?? "");
                    MySearchData.Country = _country;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Country"));
                    }
                }
            }
            public string Email
            {
                get { return _email; }
                set
                {
                    _email = value;
                    MySearchData.Email = _email;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                    }
                }
            }
            public string Phone
            {
                get { return _phone; }
                set
                {
                    _phone = value.Trim();
                    if (!string.IsNullOrEmpty(_phone))
                        _phone = _phone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace(".", "").Replace("-", "").Replace("+", "").Replace("'","");
                    MySearchData.Phone = _phone;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
                    }
                }
            }
            public int? SequenceID
            {
                get { return _sequenceID; }
                set
                {
                    _sequenceID = (value ?? 0);
                    MySearchData.SequenceID = _sequenceID;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SequenceID"));
                    }
                }
            }
            public string Account
            {
                get { return _account; }
                set
                {
                    _account = value.Trim();
                    MySearchData.Account = _account;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Account"));
                    }
                }
            }
            public int PublisherID
            {
                get { return _publisherID; }
                set
                {
                    _publisherID = value;
                    MySearchData.PublisherID = _publisherID;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PublisherID"));
                    }
                }
            }
            public int? PublicationID
            {
                get { return _publicationID; }
                set
                {
                    _publicationID = (value ?? 0);
                    MySearchData.PublicationID = _publicationID;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PublicationID"));
                    }
                }
            }
            public int? SubscriptionID
            {
                get { return _subscriptionID; }
                set
                {
                    _subscriptionID = (value ?? 0);
                    MySearchData.SubscriptionID = _subscriptionID;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SubscriptionID"));
                    }
                }
            }
            public FrameworkUAD.Object.SearchData MySearchData { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;

            public SearchDataModel(FrameworkUAD.Object.SearchData sd)
            {
                this.MySearchData = new FrameworkUAD.Object.SearchData(sd);
                this.FirstName = sd.firstName;
                this.LastName = sd.lastName;
                this.Company = sd.Company;
                this.Title = sd.Title;
                this.Address = sd.Address1;
                this.City = sd.City;
                this.State = sd.State;
                this.Zip = sd.Zip;
                this.Country = sd.Country;
                this.Email = sd.Email;
                this.Phone = sd.Phone;
                this.SequenceID = sd.SequenceID;
                this.Account = sd.Account;
                this.PublisherID = sd.PublisherID;
                this.PublicationID = sd.PublicationID;
                this.SubscriptionID = sd.SubscriptionID;
            }
        }
        SearchDataModel MySearchData { get; set; }
        //private FrameworkUAS.Object.Product TempProduct { get; set; }
        public ObservableCollection<Subscriber> SubscriberList { get; set; }
        private void UpdateProduct(object sender, PropertyChangedEventArgs e)
        {
            string pubID = Core_AMS.Utilities.WPF.GetPropertyName(() => MySearchData.PublicationID);
         
            if (e.PropertyName.Equals(pubID))
                myProduct = products.Where(x => x.PubID == MySearchData.PublicationID).FirstOrDefault();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Search
        public Search()
        {
            InitializeComponent();

            clientList = new List<KMPlatform.Entity.Client>();
            clientList.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients);

            foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.Products.Where(x => x.IsCirc == true))
                productList.Add(cp);

            productResponse = productWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Common.CheckResponse(productResponse.Result, productResponse.Status))
                products = productResponse.Result.Where(x => x.IsCirc == true).OrderBy(x => x.PubCode).ToList();

            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null)
                LoadPublication(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);

            LoadCountry();
            LoadLists();

            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsReadOnly)
                cbPublication.IsEnabled = false;

            SubscriberList = new ObservableCollection<Subscriber>();
            tbSequenceID.IsEnabledChanged += (s, e) => tbSequenceID.Focus();
            MySearchData = new SearchDataModel(new FrameworkUAD.Object.SearchData());
            //MySearchData.PropertyChanged += UpdateProduct;
            grdSearchOptions.DataContext = MySearchData;
            this.DataContext = this;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            busy.IsBusy = true;
            bw = new BackgroundWorker();

            string firstName = MySearchData.FirstName;
            string lastName = MySearchData.LastName;
            string company = MySearchData.Company;
            string title = MySearchData.Title;
            string email = MySearchData.Email;
            string address = MySearchData.Address;
            string city = MySearchData.City;
            string state = MySearchData.State;
            string country = MySearchData.Country;

            #region Set Fields & Check Conditions
            #region OLD
            //if (MySearchData.FirstName.Contains("'"))
            //    MySearchData.FirstName = MySearchData.FirstName.Replace("'", "'+char(39)+'");

            //if (MySearchData.LastName.Contains("'"))
            //    MySearchData.LastName = MySearchData.LastName.Replace("'", "'+char(39)+'");

            //if (MySearchData.Company.Contains("'"))
            //    MySearchData.Company = MySearchData.Company.Replace("'", "'+char(39)+'");

            //if (MySearchData.Title.Contains("'"))
            //    MySearchData.Title = MySearchData.Title.Replace("'", "'+char(39)+'");

            //if (MySearchData.Email.Contains("'"))
            //    MySearchData.Email = MySearchData.Email.Replace("'", "'+char(39)+'");
            #endregion

            //Need to escape apostrophe and underscore
            if (firstName.Contains("'") || firstName.Contains("_"))
                firstName = firstName.Replace("'", "''").Replace("_", "[_]");

            if (lastName.Contains("'") || lastName.Contains("_"))
                lastName = lastName.Replace("'", "''").Replace("_", "[_]");

            if (company.Contains("'") || company.Contains("_"))
                company = company.Replace("'", "''").Replace("_", "[_]");

            if (title.Contains("'") || title.Contains("_"))
                title = title.Replace("'", "''").Replace("_", "[_]");

            if (email.Contains("'") || email.Contains("_"))
                email = email.Replace("'", "''").Replace("_", "[_]");

            if (address.Contains("'") || address.Contains("_"))
                address = address.Replace("'", "''").Replace("_", "[_]");            

            if (city.Contains("'") || city.Contains("_"))
                city = city.Replace("'", "''").Replace("_", "[_]");

            if (state.Contains("'") || state.Contains("_"))
                state = state.Replace("'", "''").Replace("_", "[_]");

            if (country.Contains("'") || country.Contains("_"))
                country = country.Replace("'", "''").Replace("_", "[_]");

            if (!string.IsNullOrEmpty(MySearchData.Phone))
                MySearchData.Phone = MySearchData.Phone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace(".", "").Replace("-", "").Replace("+", "");

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(company) && string.IsNullOrEmpty(title)
                && string.IsNullOrEmpty(address) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(state)
                && string.IsNullOrEmpty(country) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(MySearchData.Zip) && string.IsNullOrEmpty(MySearchData.Account)
                && string.IsNullOrEmpty(MySearchData.Phone) && string.IsNullOrEmpty(firstName) && MySearchData.SequenceID == 0 && MySearchData.PublicationID == 0
                && MySearchData.PublisherID == 0 && MySearchData.SubscriptionID == 0)
            {
                busy.IsBusy = false;
                Core_AMS.Utilities.WPF.Message("At least one search condition must exist to perform a search.", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Add Condition");
                return;
            }
            #endregion

            #region Search - Do Work
            List<FrameworkUAD.Entity.ProductSubscription> results = new List<FrameworkUAD.Entity.ProductSubscription>();
            bw.DoWork += (o, ea) =>
            {
                FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>> bwResp = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>>();
                bwResp = productSubWorker.Proxy.Search(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName, firstName, lastName,
                    company, title, address, city, state, MySearchData.Zip, country, email,
                    MySearchData.Phone, (MySearchData.SequenceID ?? 0), MySearchData.Account, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, (MySearchData.PublicationID ?? 0), (MySearchData.SubscriptionID ?? 0));
                if (bwResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && bwResp.Result != null)
                    results = bwResp.Result;

                productSubscriptions = new ObservableCollection<FrameworkUAD.Entity.ProductSubscription>(results);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                LoadIndividuals();
                grdSearchResults.Visibility = Visibility.Visible;
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
            #endregion
        }
        #endregion
        #region Loading
        public void LoadCountry()
        {
            if (Home.Countries.Count == 0)
            {
                cWorker = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
                countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
                countryResponse = cWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(countryResponse.Result, countryResponse.Status) == true)
                    countryList = countryResponse.Result.Where(x => x.SortOrder != 0).ToList();
            }
            else
                countryList = Home.Countries.Where(x => x.SortOrder != 0).ToList();

            if (Home.Regions.Count == 0)
            {
                rWorker = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
                regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
                regionResponse = rWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(regionResponse.Result, regionResponse.Status) == true)
                    regions = regionResponse.Result.ToList();
            }
            else
                regions = Home.Regions;

            if (countryList != null)
            {
                cbCountry.ItemsSource = countryList.OrderByDescending(o => o.CountryID == 1).ThenByDescending(s => s.CountryID == 2).ThenByDescending(s => s.CountryID == 429).ThenBy(x => x.ShortName);
                cbCountry.SelectedValuePath = "ShortName";
                cbCountry.DisplayMemberPath = "ShortName";
            }

            if (regions != null)
            {
                cbState.ItemsSource = regions.OrderBy(x => x.RegionName);
                cbState.SelectedValuePath = "RegionCode";
                cbState.DisplayMemberPath = "RegionName";
            }
        }
        private void LoadLists()
        {
            busy.IsBusy = true;

            bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                if (Home.CodeTypes.Count == 0)
                    codeTypeResponse = codeTypeWorker.Proxy.Select(accessKey);
                if (Home.Codes.Count == 0)
                    codeResponse = codeWorker.Proxy.Select(accessKey);
                if (Home.TransactionCodes.Count == 0)
                    transResponse = tcWorker.Proxy.Select(accessKey);
                if (Home.TransactionCodeTypes.Count == 0)
                    transCodeTypeResponse = tcTypeWorker.Proxy.Select(accessKey);
                if (Home.CategoryCodes.Count == 0)
                    catResponse = ccWorker.Proxy.Select(accessKey);
                if (Home.CategoryCodeTypes.Count == 0)
                    catTypeResponse = catCodeTypeWorker.Proxy.Select(accessKey);
                if (Home.Actions.Count == 0)
                    actionResponse = aWorker.Proxy.Select(accessKey);
                if (Home.SubscriptionStatuses.Count == 0)
                    sstResponse = sstWorker.Proxy.Select(accessKey);
                if (Home.SubscriptionStatusMatrices.Count == 0)
                    ssmResponse = ssmWorker.Proxy.Select(accessKey);
                //if (Home.ZipCodes.Count == 0)
                //    zipResponse = zipCodeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Code Types and Codes
                if (codeTypeResponse.Result != null)
                    codeTypeList = codeTypeResponse.Result;
                else
                    codeTypeList = Home.CodeTypes;
                if (codeResponse.Result != null || Home.Codes.Count > 0)
                {
                    if (Home.Codes.Count == 0)
                        codeList = codeResponse.Result;
                    else
                        codeList = Home.Codes;

                    if (Home.AddressCodes.Count == 0)
                    {
                        int addrType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Address.ToString())).CodeTypeId;
                        addressTypeList = codeList.Where(x => x.CodeTypeId == addrType).ToList();
                        if (addressTypeList.Where(x => x.CodeId == 0).Count() == 0)
                            addressTypeList.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, DisplayName = "", IsActive = true });
                    }
                    else
                        addressTypeList = Home.AddressCodes;

                    if (Home.QSourceCodes.Count == 0)
                    {
                        int qSourceType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "))).CodeTypeId;
                        qSourceList = codeList.Where(x => x.CodeTypeId == qSourceType).ToList();
                    }
                    else
                        qSourceList = Home.QSourceCodes;

                    if (Home.Par3CCodes.Count == 0)
                    {
                        int par3cType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Par3c.ToString())).CodeTypeId;
                        parList = codeList.Where(x => x.CodeTypeId == par3cType).ToList();
                    }
                    else
                        parList = Home.Par3CCodes;

                    if (Home.MarketingCodes.Count == 0)
                    {
                        int marketingType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Marketing.ToString())).CodeTypeId;
                        marketingList = codeList.Where(x => x.CodeTypeId == marketingType).ToList();
                    }
                    else
                        marketingList = Home.MarketingCodes;
                }
                #endregion
                if (transResponse.Result != null)
                    transCodeList = transResponse.Result;
                else
                    transCodeList = Home.TransactionCodes;
                if (transCodeTypeResponse.Result != null)
                    transCodeTypeList = transCodeTypeResponse.Result;
                else
                    transCodeTypeList = Home.TransactionCodeTypes;
                if (catResponse.Result != null)
                    categoryCodeList = catResponse.Result;
                else
                    categoryCodeList = Home.CategoryCodes;
                if (catTypeResponse.Result != null)
                    catTypeList = catTypeResponse.Result;
                else
                    catTypeList = Home.CategoryCodeTypes;
                if (actionResponse.Result != null)
                    actionList = actionResponse.Result.Where(a => a.IsActive == true).ToList();
                else
                    actionList = Home.Actions.Where(a=> a.IsActive == true).ToList();
                if (sstResponse.Result != null)
                    sstList = sstResponse.Result.Where(a => a.IsActive == true).ToList();
                else
                    sstList = Home.SubscriptionStatuses.Where(a=> a.IsActive == true).ToList();
                if (ssmResponse.Result != null)
                    ssmList = ssmResponse.Result.Where(x => x.IsActive == true).ToList();
                else
                    ssmList = Home.SubscriptionStatusMatrices.Where(x=> x.IsActive == true).ToList();
                //if (zipResponse.Result != null)
                //    zipList = zipResponse.Result;
                //else
                //    zipList = Home.ZipCodes;

                FrameworkUAD_Lookup.Entity.TransactionCodeType freeType = transCodeTypeList.Where(x => x.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Active.ToString().Replace("_", " "))).FirstOrDefault();
                FrameworkUAD_Lookup.Entity.TransactionCodeType paidType = transCodeTypeList.Where(x => x.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Active.ToString().Replace("_", " "))).FirstOrDefault();

                if (freeType != null && paidType != null)
                {
                    FrameworkUAD_Lookup.Entity.TransactionCode paid = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == paidType.TransactionCodeTypeID).FirstOrDefault();
                    FrameworkUAD_Lookup.Entity.TransactionCode free = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == freeType.TransactionCodeTypeID).FirstOrDefault();
                    if (paid != null)
                        paidID = paid.TransactionCodeID;
                    if (free != null)
                        freeID = free.TransactionCodeID;
                }

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        public void LoadPublication(int clientId)
        {
            ReLoadProduct();
            cbSPublication.ItemsSource = products;
            cbSPublication.DisplayMemberPath = "PubCode";
            cbSPublication.SelectedValuePath = "PubID";
            cbPublication.ItemsSource = products;
            cbPublication.DisplayMemberPath = "PubCode";
            cbPublication.SelectedValuePath = "PubID";
        }
        public void LoadIndividuals()
        {
            SubscriberList.Clear();
            foreach (FrameworkUAD.Entity.ProductSubscription s in productSubscriptions)
            {
                if (s.CountryID != 0 && (s.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID
                    || s.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID
                    || s.CountryID == countryList.SingleOrDefault(x => x.ShortName.Replace("-", "").Replace("_", " ").Trim() == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.MEXICO.ToString().Replace("_", " ")).CountryID))
                {
                    if (!string.IsNullOrEmpty(s.ZipCode) && !string.IsNullOrEmpty(s.Plus4)
                        && s.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID)
                    {
                        if (s.ZipCode.Length > 5)
                            s.FullZip = s.ZipCode.Replace("-", "");
                        else
                            s.FullZip = s.ZipCode + s.Plus4;
                    }
                    else if (!string.IsNullOrEmpty(s.ZipCode)
                        && s.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
                    {
                        if (s.ZipCode.Length > 3)
                            s.FullZip = s.ZipCode.Replace(" ", "");
                        else
                            s.FullZip = s.ZipCode + s.Plus4;
                    }
                    else
                        s.FullZip = s.ZipCode + s.Plus4;
                }
                else
                {
                    s.FullZip = s.ZipCode + s.Plus4;
                }

                if (!string.IsNullOrEmpty(s.Phone) && s.Phone.Trim().Length == 10)
                    s.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(s.Phone.Trim());
                if (!string.IsNullOrEmpty(s.Mobile) && s.Mobile.Trim().Length == 10)
                    s.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(s.Mobile.Trim());
                if (!string.IsNullOrEmpty(s.Fax) && s.Fax.Trim().Length == 10)
                    s.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(s.Fax.Trim());

                Subscriber sub = new Subscriber(s, regions, countryList, addressTypeList, paidID, freeID);
                SubscriberList.Add(sub);
            }
        }
        #endregion
        #region Saving
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Subscriber mySubscriber = btn.DataContext as Subscriber;
            InitiateSave(mySubscriber);
        }
        private void InitiateSave(Subscriber mySubscriber)
        {
            ReLoadProduct();
            if (myProduct.AllowDataEntry == true && mySubscriber != null)
            {
                saveWaveMailing = false;
                myWMDetail = new FrameworkUAD.Entity.WaveMailingDetail();

                foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.SecurityGroups)
                {
                    foreach (var s in cp.Services)
                    {
                        foreach (var a in s.Applications)
                        {
                            if (a.ApplicationName.Contains(FrameworkUAS.BusinessLogic.Enums.Applications.Circulation.ToString()))
                            {
                                applicationID = a.ApplicationID;
                                break;
                            }
                        }
                    }
                }

                if (mySubscriber.InfoChanged == true)
                {
                    int id = 0;
                    busy.IsBusy = true;
                    bw = new BackgroundWorker();
                    bw.DoWork += (o, ea) =>
                    {
                        //SaveSubscriber(mySubscriber);
                        id = SaveSubscriberNew(mySubscriber);
                    };
                    bw.RunWorkerCompleted += (o, ea) =>
                    {
                        busy.IsBusy = false;
                        if(id > 0)
                            Core_AMS.Utilities.WPF.Message("Record Saved", MessageBoxButton.OK, MessageBoxImage.Information, "Record Saved");
                    };
                    bw.RunWorkerAsync();
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("Subscriber change is required to save.", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, "No Changes");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("This publication is currently locked to process lists. Data can not be saved.", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, "Data Entry Locked");
                return;
            }
        }
        private void SaveSubscriber(Subscriber subscriber)
        {
            #region Set Product Subscription object values

            if (subscriber.Phone.Length == 10)
                subscriber.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(subscriber.Phone);
            if (subscriber.Mobile.Length == 10)
                subscriber.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(subscriber.Mobile);
            if (subscriber.Fax.Length == 10)
                subscriber.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(subscriber.Fax);

            KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            if (myProductSubscription.IsNewSubscription == true)
            {
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
                subscriber.CurrentSubscriber.DateCreated = DateTime.Now;
                subscriber.CurrentSubscriber.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            }
            else
            {
                myProductSubscription.DateUpdated = DateTime.Now;
                myProductSubscription.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            }

            waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription(subscriber.CurrentSubscriber);
            if (subscriber.OriginalSubscriber.IsInActiveWaveMailing == true)
            {
                CompareSubscriber(subscriber);
            }
            if (subscriber.CurrentSubscriber.CountryID == 0)
                subscriber.CurrentSubscriber.CountryID = 1;
            #endregion
            FrameworkUAS.Service.Response<int> saveResp = productSubWorker.Proxy.Save(accessKey, subscriber.CurrentSubscriber, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            #region Write to Logs

            if (saveResp.Result > 0 && saveResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                myProductSubscription.PubSubscriptionID = saveResp.Result;
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                FrameworkServices.ServiceClient<UAS_WS.Interface.IUserLog> ulWorker = FrameworkServices.ServiceClient.UAS_UserLogClient();
                FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog> ulResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog>();

                if (subscriber.OriginalSubscriber.IsInActiveWaveMailing == true && saveWaveMailing == true)
                {
                    ulResponse = ulWorker.Proxy.CreateLog(accessKey,
                                            applicationID,
                                            ult,
                                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                                            "ProductSubscription",
                                            jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(subscriber.OriginalSubscriber),
                                            jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(waveMailSubscriber));

                    wMDetailWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myWMDetail, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                }
                else
                {
                    ulResponse = ulWorker.Proxy.CreateLog(accessKey,
                                                            applicationID,
                                                            ult,
                                                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                                                            "ProductSubscription",
                                                            jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(subscriber.OriginalSubscriber),
                                                            jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(subscriber.CurrentSubscriber));
                }

                int userLogID = 0;
                if (ulResponse.Result != null && ulResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    userLogID = ulResponse.Result.UserLogID;

                if (userLogID > 0)
                {
                    int historySubscriptionID = 0;
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IHistorySubscription> bwhsWorker = FrameworkServices.ServiceClient.UAD_HistorySubscriptionClient();

                    FrameworkUAS.Service.Response<int> historyResponse = new FrameworkUAS.Service.Response<int>();
                    historyResponse = bwhsWorker.Proxy.SaveForSubscriber(accessKey, subscriber.CurrentSubscriber, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, 
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

                    if (historyResponse.Result != null && historyResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        historySubscriptionID = historyResponse.Result;

                    FrameworkUAD.Entity.Batch batch = Common.CurrentBatch(myProduct.PubID);
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IHistory> bwhWorker = FrameworkServices.ServiceClient.UAD_HistoryClient();
                    FrameworkUAD.Entity.History history = bwhWorker.Proxy.AddHistoryEntry(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, batch.BatchID,
                                                                                                                    batch.BatchCount,
                                                                                                                    myProduct.PubID,
                                                                                                                    subscriber.CurrentSubscriber.PubSubscriptionID,
                                                                                                                    subscriber.CurrentSubscriber.SubscriptionID,
                                                                                                                    historySubscriptionID,
                                                                                                                    0,
                                                                                                                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                                                                                                                    0).Result;
                    //UserLog HistoryID - HistoryToUserLog
                    if (userLogID > 0)
                        bwhWorker.Proxy.Insert_History_To_UserLog(accessKey, history.HistoryID, userLogID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                }
                int index = SubscriberList.IndexOf(subscriber);
                subscriber.OriginalSubscriber = new FrameworkUAD.Entity.ProductSubscription(subscriber.CurrentSubscriber);
                subscriber.InfoChanged = false;
                SubscriberList.RemoveAt(index);
                SubscriberList.Insert(index, subscriber);
            }
            #endregion
        }
        private int SaveSubscriberNew(Subscriber subscriber)
        {
            if (subscriber.Phone.Length == 10)
                subscriber.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(subscriber.Phone);
            if (subscriber.Mobile.Length == 10)
                subscriber.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(subscriber.Mobile);
            if (subscriber.Fax.Length == 10)
                subscriber.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(subscriber.Fax);

            KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            if (myProductSubscription.IsNewSubscription == true)
            {
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
                subscriber.CurrentSubscriber.DateCreated = DateTime.Now;
                subscriber.CurrentSubscriber.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            }
            else
            {
                myProductSubscription.DateUpdated = DateTime.Now;
                myProductSubscription.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                subscriber.CurrentSubscriber.DateUpdated = DateTime.Now;
                subscriber.CurrentSubscriber.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            }

            waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription(subscriber.CurrentSubscriber);
            if (subscriber.OriginalSubscriber.IsInActiveWaveMailing == true)
            {
                CompareSubscriber(subscriber);
            }
            if (subscriber.CurrentSubscriber.CountryID == 0)
                subscriber.CurrentSubscriber.CountryID = 1;

            FrameworkUAS.Object.Batch b = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.PublicationID == myProduct.PubID
                && x.UserID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID && x.IsActive == true).FirstOrDefault();

            FrameworkUAS.Service.Response<int> saveResp = productSubWorker.Proxy.ProfileSave(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, subscriber.CurrentSubscriber, subscriber.OriginalSubscriber,
                saveWaveMailing, applicationID, ult, b, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, waveMailSubscriber, myWMDetail);
            int id = saveResp.Result;

            if(b != null && b.BatchCount >= 100)
                Core_AMS.Utilities.WPF.Message("Batch #" + b.BatchNumber + " has reached 100 Transactions. The next batch will open automatically.");

            Common.RefreshBatches();

            if (id > 0)
            {
                int index = SubscriberList.IndexOf(subscriber);
                subscriber.OriginalSubscriber = new FrameworkUAD.Entity.ProductSubscription(subscriber.CurrentSubscriber);
                subscriber.InfoChanged = false;
                SubscriberList.RemoveAt(index);
                SubscriberList.Insert(index, subscriber);
            }
            else
                Core_AMS.Utilities.WPF.MessageError("There was a problem saving your subscriber. Please try again.");

            return id;
        }
        #endregion
        #region UI Events
        //
        // This is the code handling not allowing certain special characters
        // Per Sunil on 11-02-2016 we will allow apostrophe TFS Ticket 36227
        //
        public void AlphanumericOnlySpecial(object sender, TextCompositionEventArgs e)
        {
            // Tyring to avoid allowing users to enter apostrophy 
            e.Handled = new Regex("[^0-9a-zA-Z '.,\\-_!@#$%^&*()<>;:{}?[\\]|/]").IsMatch(e.Text);
        }
        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            RadButton thisBtn = ((RadButton)sender);
            Subscriber s = thisBtn.DataContext as Subscriber;
            CancelSubscriberEdit(s);
        }
        private void tbBFName_Loaded_1(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.CaretIndex = tb.Text.Length;
            tb.Focus();
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (typeof(TextBox) == sender.GetType())
            {
                TextBox tb = (TextBox)sender;
                if (e.Key == Key.Tab)
                {
                    tb.SelectAll();
                }
            }
            else if(typeof(RadWatermarkTextBox) == sender.GetType())
            {
                RadWatermarkTextBox tb = (RadWatermarkTextBox)sender;
                if (e.Key == Key.Tab)
                {
                    tb.SelectAll();
                }
            }
        }
        private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sv = cbCountry.SelectedValue;

            if (sv == null)
            {
                return;
            }

            if (regions == null)
            {
                rWorker = new FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion>();
                regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
                regionResponse = rWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(regionResponse.Result, regionResponse.Status) == true)
                    regions = regionResponse.Result;
            }

            if (sv.Equals(FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")))
            {
                FrameworkUAD_Lookup.Entity.Country c = countryList.Where(x => x.ShortName == sv.ToString()).FirstOrDefault();
                if (c != null)
                {
                    cbState.ItemsSource = regions.Where(x => x.CountryID == Convert.ToInt32(c.CountryID)).ToList();
                    cbState.SelectedValuePath = "RegionCode";
                    cbState.DisplayMemberPath = "RegionName";

                    if (!string.IsNullOrEmpty(tbZipPlus4.Text) && tbZipPlus4.Text.Length.Equals(9))
                    {
                        tbZipPlus4.Text = tbZipPlus4.Text.Substring(0, 5) + "-" + tbZipPlus4.Text.Substring(5, 4);
                    }
                }
            }
            else if (sv.Equals(FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")))
            {
                FrameworkUAD_Lookup.Entity.Country c = countryList.Where(x => x.ShortName == sv.ToString()).FirstOrDefault();
                if (c != null)
                {
                    cbState.SelectedValue = -1;
                    cbState.ItemsSource = regions.Where(x => x.CountryID == Convert.ToInt32(c.CountryID)).ToList();
                    cbState.SelectedValuePath = "RegionCode";
                    cbState.DisplayMemberPath = "RegionName";
                }
            }
            else if (sv.Equals(FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.MEXICO.ToString().Replace("_", " ")))
            {
                cbState.SelectedValue = -1;
                cbState.ItemsSource = regions.Where(x => x.CountryID == 429).OrderBy(x => x.CountryID).ThenBy(x => x.RegionName).ToList();
                cbState.SelectedValuePath = "RegionCode";
                cbState.DisplayMemberPath = "RegionName";
            }
            else if (sv.Equals(""))
            {
                cbState.ItemsSource = null;
            }
            else
            {
                cbState.ItemsSource = regions.Where(x => x.CountryID == 0).ToList();
                cbState.SelectedValuePath = "RegionCode";
                cbState.DisplayMemberPath = "RegionName";
                cbState.SelectedValue = "FO";
            }
        }
        private void tbAddress_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!string.IsNullOrEmpty(MySearchData.Address) && MySearchData.Address.Length < 4)
            {
                Core_AMS.Utilities.WPF.Message("Address field must be longer than 4 characters.", "Warning");
                tb.CaretIndex = tb.Text.Length;
                e.Handled = true;
            }
        }
        private void cbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPublication.SelectedItem != null)
            {
                FrameworkUAD.Entity.Product myProduct = (FrameworkUAD.Entity.Product)cbPublication.SelectedItem;
                List<FrameworkUAD.Entity.History> maxBatches = new List<FrameworkUAD.Entity.History>();

                hWorker = FrameworkServices.ServiceClient.UAD_HistoryClient();
                historyResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.History>>();
                historyResponse = hWorker.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, myProduct.PubID, 
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (Common.CheckResponse(historyResponse.Result, historyResponse.Status) == true)
                    maxBatches = historyResponse.Result;

                if (maxBatches.Count > 0)
                {
                    Core_AMS.Utilities.WPF.Message("There are batches that need to be finalized.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Finalize Batch");
                    cbPublication.SelectedValue = null;
                    return;
                }
                else if (myProduct.HasPaidRecords && myProduct.UseSubGen)
                {
                    //if (myProduct.AllowDataEntry == false)
                    //{
                    //    Core_AMS.Utilities.WPF.Message("Data Entry for this product is currently locked.", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Product Locked");
                    //}
                    //else 
                    //{
                    //    //open subgen to create new subscription
                    //    Modules.SubscriptionGenius sg = new SubscriptionGenius(KMPlatform.BusinessLogic.Enums.SubGenControls.Subscriber, currRow.SubGenSubscriberID);
                    //    Windows.PlainPopout pop = new Windows.PlainPopout(sg);
                    //    pop.Show();
                    //}

                    //will stop cdc data from coming in if product is locked - adding to SubGen is fine
                    //open subgen to create new subscription
                    Modules.SubscriptionGenius sg = new SubscriptionGenius(KMPlatform.BusinessLogic.Enums.SubGenControls.New_Subscriber);
                    Windows.PlainPopout pop = new Windows.PlainPopout(sg);
                    pop.Show();
                }
                else
                {
                    cbPublication.SelectedIndex = -1;
                    ReLoadProduct();
                    SubscriptionContainer sc = new SubscriptionContainer(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient, myProduct, null, products);
                    Common.OpenCircPopoutWindow(sc);
                }
            }
        }
        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            ClearSearch();
        }
        private void Subscriber_Unloaded(object sender, RoutedEventArgs e)
        {
            Grid grd = sender as Grid;
            Subscriber mySubscriber = grd.DataContext as Subscriber;
            if (mySubscriber != null && mySubscriber.IsOpen == true)
            {
                mySubscriber.IsOpen = false;
                productSubWorker.Proxy.UpdateLock(accessKey, mySubscriber.CurrentSubscriber.PubSubscriptionID, false, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            }
        }
        private void btnOpenWindow_Click(object sender, RoutedEventArgs e)
        {
            busy.IsBusy = true;
            Button thisBtn = (Button)sender;
            Subscriber selectedSub = thisBtn.DataContext as Subscriber;
            prodSubResponse = productSubWorker.Proxy.SelectProductSubscription(accessKey, selectedSub.CurrentSubscriber.PubSubscriptionID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName);
            if (prodSubResponse.Result != null && prodSubResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                //selectedSub.CurrentSubscriber.IsLocked = prodSubResponse.Result.IsLocked;
                //selectedSub.CurrentSubscriber.LockedByUserID = prodSubResponse.Result.LockedByUserID;
                selectedSub.CurrentSubscriber = new FrameworkUAD.Entity.ProductSubscription(prodSubResponse.Result);
            }            
            CancelSubscriberEdit(selectedSub);
            OpenSubscriberRecord(selectedSub.CurrentSubscriber);
        }
        private void btn_ToggleRowDetails(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Subscriber s = btn.DataContext as Subscriber;
            if (s != null)
            {
                ToggleSubscriberVisibility(s);
            }
        }
        private void GridAmaze_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.OriginalSource.GetType().FullName.ToString() == typeof(GridViewCell).FullName.ToString())
            {
                RadGridView grdView = sender as RadGridView;
                Subscriber s = grdView.SelectedItem as Subscriber;
                if (s != null && e.Key == Key.Space)
                {
                    ToggleSubscriberVisibility(s);
                    e.Handled = true;
                }
                else if (s != null && e.Key == Key.O)
                {
                    CancelSubscriberEdit(s);
                    OpenSubscriberRecord(s.CurrentSubscriber);
                    e.Handled = true;
                }
            }
        }
        private void GridAmaze_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Source.ToString() == typeof(RadGridView).FullName.ToString())
            {
                RadGridView grdView = sender as RadGridView;
                Subscriber s = grdView.SelectedItem as Subscriber;
                if (s!= null && s.IsOpen)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) // Is Ctrl key pressed
                    {
                        if (Keyboard.IsKeyDown(Key.S))
                        {
                            InitiateSave(s);
                            e.Handled = true;
                        }
                        else if (Keyboard.IsKeyDown(Key.C))
                        {
                            CancelSubscriberEdit(s);
                            e.Handled = true;
                        }
                    }
                }
            }
        }
        #endregion
        #region Helpers
        private void ReLoadProduct()
        {
            if (myProduct != null)
            {
                singProdResponse = productWorker.Proxy.Select(accessKey, myProduct.PubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (singProdResponse.Result != null && singProdResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    FrameworkUAD.Entity.Product pub = singProdResponse.Result;
                    if (pub != null)
                    {
                        myProduct = pub;
                    }
                }
            }
        }
        public void ClearSearch()
        {
            MySearchData = new SearchDataModel(new FrameworkUAD.Object.SearchData());
            grdSearchOptions.DataContext = MySearchData;

            grdSearchResults.Visibility = Visibility.Hidden;
        }
        public void ClearPublication()
        {
            cbPublication.ItemsSource = null;
            cbSPublication.ItemsSource = null;
        }
        private void CancelSubscriberEdit(Subscriber s)
        {
            s.FirstName = s.OriginalSubscriber.FirstName;
            s.LastName = s.OriginalSubscriber.LastName;
            s.Title = s.OriginalSubscriber.Title;
            s.Company = s.OriginalSubscriber.Company;
            s.AddressType = s.OriginalSubscriber.AddressTypeCodeId;
            s.Address1 = s.OriginalSubscriber.Address1;
            s.Address2 = s.OriginalSubscriber.Address2;
            s.Address3 = s.OriginalSubscriber.Address3;
            s.City = s.OriginalSubscriber.City;
            s.Country = s.OriginalSubscriber.Country;
            s.CountryID = s.OriginalSubscriber.CountryID;
            s.RegionCode = s.OriginalSubscriber.RegionCode;
            s.RegionID = s.OriginalSubscriber.RegionID;
            s.FullZip = s.OriginalSubscriber.FullZip;
            s.County = s.OriginalSubscriber.County;
            s.Phone = s.OriginalSubscriber.Phone.Trim().Replace("-", "");
            s.PhoneCode = s.OriginalSubscriber.PhoneCode;
            s.PhoneExt = s.OriginalSubscriber.PhoneExt;
            s.Mobile = s.OriginalSubscriber.Mobile.Trim().Replace("-", "");
            s.Fax = s.OriginalSubscriber.Fax.Trim().Replace("-", "");
            s.Email = s.OriginalSubscriber.Email;
            s.Website = s.OriginalSubscriber.Website;

            if (s.CurrentSubscriber.LockedByUserID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID)
                productSubWorker.Proxy.UpdateLock(accessKey, s.CurrentSubscriber.PubSubscriptionID, false, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, 
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            s.IsOpen = false;
        }
        private void CompareSubscriber(Subscriber sub)
        {
            FrameworkUAD.Entity.ProductSubscription origSub = sub.OriginalSubscriber;
            myProductSubscription.IsInActiveWaveMailing = true;
            myProductSubscription.WaveMailingID = origSub.WaveMailingID;
            myWMDetail.PubSubscriptionID = origSub.PubSubscriptionID;
            myWMDetail.SubscriptionID = origSub.SubscriptionID;
            myWMDetail.WaveMailingID = origSub.WaveMailingID;
            waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription(sub.CurrentSubscriber);
            if (origSub.FirstName != sub.CurrentSubscriber.FirstName)
            {
                myWMDetail.FirstName = sub.CurrentSubscriber.FirstName;
                sub.CurrentSubscriber.FirstName = origSub.FirstName;
                saveWaveMailing = true;
            }
            if (origSub.LastName != sub.CurrentSubscriber.LastName)
            {
                myWMDetail.LastName = sub.CurrentSubscriber.LastName;
                sub.CurrentSubscriber.LastName = origSub.LastName;
                saveWaveMailing = true;
            }
            if (origSub.Title != sub.CurrentSubscriber.Title)
            {
                myWMDetail.Title = sub.CurrentSubscriber.Title;
                sub.CurrentSubscriber.Title = origSub.Title;
                saveWaveMailing = true;
            }
            if (origSub.Company != sub.CurrentSubscriber.Company)
            {
                myWMDetail.Company = sub.CurrentSubscriber.Company;
                sub.CurrentSubscriber.Company = origSub.Company;
                saveWaveMailing = true;
            }
            if (origSub.Address1 != sub.CurrentSubscriber.Address1)
            {
                myWMDetail.Address1 = sub.CurrentSubscriber.Address1;
                sub.CurrentSubscriber.Address1 = origSub.Address1;
                saveWaveMailing = true;
            }
            if (origSub.Address2 != sub.CurrentSubscriber.Address2)
            {
                myWMDetail.Address2 = sub.CurrentSubscriber.Address2;
                sub.CurrentSubscriber.Address2 = origSub.Address2;
                saveWaveMailing = true;
            }
            if (origSub.Address3 != sub.CurrentSubscriber.Address3)
            {
                myWMDetail.Address3 = sub.CurrentSubscriber.Address3;
                waveMailSubscriber.Address3 = sub.CurrentSubscriber.Address3;
                sub.CurrentSubscriber.Address3 = origSub.Address3;
                saveWaveMailing = true;
            }
            if (origSub.AddressTypeCodeId != sub.CurrentSubscriber.AddressTypeCodeId)
            {
                myWMDetail.AddressTypeID = sub.CurrentSubscriber.AddressTypeCodeId;
                sub.CurrentSubscriber.AddressTypeCodeId = origSub.AddressTypeCodeId;
                saveWaveMailing = true;
            }
            if (origSub.City != sub.CurrentSubscriber.City)
            {
                myWMDetail.City = sub.CurrentSubscriber.City;
                sub.CurrentSubscriber.City = origSub.City;
                saveWaveMailing = true;
            }
            if (origSub.RegionCode != sub.CurrentSubscriber.RegionCode)
            {
                myWMDetail.RegionCode = sub.CurrentSubscriber.RegionCode;
                sub.CurrentSubscriber.RegionCode = origSub.RegionCode;
                saveWaveMailing = true;
            }
            if (origSub.RegionID != sub.CurrentSubscriber.RegionID)
            {
                myWMDetail.RegionID = sub.CurrentSubscriber.RegionID;
                sub.CurrentSubscriber.RegionID = origSub.RegionID;
                saveWaveMailing = true;
            }
            if (origSub.ZipCode != sub.CurrentSubscriber.ZipCode)
            {
                myWMDetail.ZipCode = sub.CurrentSubscriber.ZipCode;
                sub.CurrentSubscriber.ZipCode = origSub.ZipCode;
                saveWaveMailing = true;
            }
            if (origSub.Plus4 != sub.CurrentSubscriber.Plus4)
            {
                myWMDetail.Plus4 = sub.CurrentSubscriber.Plus4;
                sub.CurrentSubscriber.Plus4 = origSub.Plus4;
                saveWaveMailing = true;
            }
            if (origSub.County != sub.CurrentSubscriber.County)
            {
                myWMDetail.County = sub.CurrentSubscriber.County;
                sub.CurrentSubscriber.County = origSub.County;
                saveWaveMailing = true;
            }
            if (origSub.Country != sub.CurrentSubscriber.Country)
            {
                myWMDetail.Country = sub.CurrentSubscriber.Country;
                sub.CurrentSubscriber.Country = origSub.Country;
                saveWaveMailing = true;
            }
            if (origSub.CountryID != sub.CurrentSubscriber.CountryID)
            {
                myWMDetail.CountryID = sub.CurrentSubscriber.CountryID;
                sub.CurrentSubscriber.CountryID = origSub.CountryID;
                saveWaveMailing = true;
            }
            if (origSub.Email != sub.CurrentSubscriber.Email)
            {
                myWMDetail.Email = sub.CurrentSubscriber.Email;
                sub.CurrentSubscriber.Email = origSub.Email;
                saveWaveMailing = true;
            }
            if (origSub.Phone != sub.CurrentSubscriber.Phone)
            {
                myWMDetail.Phone = sub.CurrentSubscriber.Phone;
                sub.CurrentSubscriber.Phone = origSub.Phone;
                saveWaveMailing = true;
            }
            if (origSub.Fax != sub.CurrentSubscriber.Fax)
            {
                myWMDetail.Fax = sub.CurrentSubscriber.Fax;
                sub.CurrentSubscriber.Fax = origSub.Fax;
                saveWaveMailing = true;
            }
            if (origSub.Mobile != sub.CurrentSubscriber.Mobile)
            {
                myWMDetail.Mobile = sub.CurrentSubscriber.Mobile;
                sub.CurrentSubscriber.Mobile = origSub.Mobile;
                saveWaveMailing = true;
            }
            if (origSub.Demo7 != sub.CurrentSubscriber.Demo7)
            {
                myWMDetail.Demo7 = sub.CurrentSubscriber.Demo7;
                sub.CurrentSubscriber.Demo7 = origSub.Demo7;
                saveWaveMailing = true;
            }
            if (origSub.PubCategoryID != sub.CurrentSubscriber.PubCategoryID)
            {
                myWMDetail.PubCategoryID = sub.CurrentSubscriber.PubCategoryID;
                sub.CurrentSubscriber.PubCategoryID = origSub.PubCategoryID;
                saveWaveMailing = true;
            }
            if (origSub.PubTransactionID != sub.CurrentSubscriber.PubTransactionID)
            {
                myWMDetail.PubTransactionID = sub.CurrentSubscriber.PubTransactionID;
                sub.CurrentSubscriber.PubTransactionID = origSub.PubTransactionID;
                saveWaveMailing = true;
            }
            if (origSub.IsSubscribed != sub.CurrentSubscriber.IsSubscribed)
            {
                myWMDetail.IsSubscribed = sub.CurrentSubscriber.IsSubscribed;
                sub.CurrentSubscriber.IsSubscribed = origSub.IsSubscribed;
                saveWaveMailing = true;
            }
            if (origSub.SubscriptionStatusID != sub.CurrentSubscriber.SubscriptionStatusID)
            {
                myWMDetail.SubscriptionStatusID = sub.CurrentSubscriber.SubscriptionStatusID;
                sub.CurrentSubscriber.SubscriptionStatusID = origSub.SubscriptionStatusID;
                saveWaveMailing = true;
            }
            if (origSub.Copies != sub.CurrentSubscriber.Copies)
            {
                myWMDetail.Copies = sub.CurrentSubscriber.Copies;
                sub.CurrentSubscriber.Copies = origSub.Copies;
                saveWaveMailing = true;
            }
            if (origSub.PhoneExt != sub.CurrentSubscriber.PhoneExt)
            {
                myWMDetail.PhoneExt = sub.CurrentSubscriber.PhoneExt;
                sub.CurrentSubscriber.PhoneExt = origSub.PhoneExt;
                saveWaveMailing = true;
            }
            if (origSub.IsPaid != sub.CurrentSubscriber.IsPaid)
            {
                myWMDetail.IsPaid = sub.CurrentSubscriber.IsPaid;
                sub.CurrentSubscriber.IsPaid = origSub.IsPaid;
                saveWaveMailing = true;
            }
        }
        private void ToggleSubscriberVisibility(Subscriber s)
        {
            if (s.IsOpen == false)
            {
                s.IsOpen = true;
                grdSearchResults.ScrollIntoView(s);
                grdSearchResults.UpdateLayout();
                FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> currPsResp = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
                currPsResp = productSubWorker.Proxy.SelectProductSubscription(accessKey, s.CurrentSubscriber.PubSubscriptionID,
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName);
                if (currPsResp.Result != null && currPsResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    s.IsSubscribed = currPsResp.Result.IsSubscribed;
                    s.CurrentSubscriber = new FrameworkUAD.Entity.ProductSubscription(currPsResp.Result);
                    s.OriginalSubscriber = new FrameworkUAD.Entity.ProductSubscription(currPsResp.Result);
                }
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                myProduct = products.Where(x => x.PubID == s.OriginalSubscriber.PubID).SingleOrDefault();

                ReLoadProduct();
                if (myProduct != null && myProduct.AllowDataEntry == true && !s.CurrentSubscriber.IsLocked && s.CurrentSubscriber.IsActive && s.CurrentSubscriber.IsSubscribed &&
                    !FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsReadOnly)
                {
                    productSubWorker.Proxy.UpdateLock(accessKey, s.CurrentSubscriber.PubSubscriptionID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    s.CurrentSubscriber.LockedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    s.IsEnabled = true;
                }
                else if (myProduct != null && (myProduct.AllowDataEntry == false || s.CurrentSubscriber.IsLocked || !s.CurrentSubscriber.IsActive || !s.CurrentSubscriber.IsSubscribed ||
                                               FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsReadOnly))
                {
                    s.IsEnabled = false;
                    if (!FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsReadOnly)
                    {
                        if (s.CurrentSubscriber.IsLocked)
                            Core_AMS.Utilities.WPF.Message("This subscription is currently locked.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Subscription Locked.");
                        else if (myProduct.AllowDataEntry == false)
                            Core_AMS.Utilities.WPF.Message("Data entry is currently locked for this product.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Data Entry Locked");
                        else
                            Core_AMS.Utilities.WPF.Message("This subscription is inactive or unsubscribed. Please open their record to reactivate.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Inactive Subscription");
                    }
                }
            }
            else
            {
                CancelSubscriberEdit(s);
            }
            
        }
        private void OpenSubscriberRecord(FrameworkUAD.Entity.ProductSubscription currRow)
        {
            SubscriptionContainer sc;
            FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> prodSubResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
            productSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
            prodSubResponse = productSubWorker.Proxy.SelectProductSubscription(accessKey, currRow.PubSubscriptionID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName);
            if (prodSubResponse.Result != null && prodSubResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                currRow = prodSubResponse.Result;
            if (currRow != null && currRow.PubSubscriptionID > 0)
            {
                if(currRow.CountryID == 2) //Scrub Canadian Zip Code
                {
                    currRow.ZipCode = currRow.ZipCode + currRow.Plus4;
                    currRow.Plus4 = "";
                }
                myProductSubscription = currRow;
                myProduct = products.Where(x => x.PubID == myProductSubscription.PubID).SingleOrDefault();
                if (currRow.IsLocked == true)
                {
                    sc = new SubscriptionContainer(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient, myProduct, myProductSubscription, products);
                    Common.OpenCircPopoutWindow(sc);
                    busy.IsBusy = false;
                }
                else
                {
                    if (currRow.IsPaid && myProduct.UseSubGen)
                    {
                        if (myProduct.AllowDataEntry == false)
                        {
                            Core_AMS.Utilities.WPF.Message("Data Entry for this product is currently locked.", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Product Locked");
                        }
                        else if (currRow.SubGenSubscriberID != null && currRow.SubGenSubscriberID > 0)
                        {
                            //NEED TO CHANGE THIS TO TAKE IN NEW ID FIELD FROM PUBSUBSCRIPTION
                            Modules.SubscriptionGenius sg = new SubscriptionGenius(KMPlatform.BusinessLogic.Enums.SubGenControls.Subscriber, currRow.SubGenSubscriberID);
                            Windows.PlainPopout pop = new Windows.PlainPopout(sg);
                            pop.Show();
                        }
                        else
                            Core_AMS.Utilities.WPF.MessageError("This subscriber has not yet been connected to paid subscription services. Please try again later.");
                    }
                    else
                    {
                        productSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
                        productSubWorker.Proxy.UpdateLock(accessKey, myProductSubscription.PubSubscriptionID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        myProductSubscription.LockedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        sc = new SubscriptionContainer(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient, myProduct, myProductSubscription, products);

                        sc.ReloadSubscriber += value =>
                        {
                            if (value > 0)
                            {
                                prodSubResponse = productSubWorker.Proxy.SelectProductSubscription(accessKey, value, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName);
                                if (prodSubResponse.Result != null && prodSubResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                                {
                                    Subscriber s = SubscriberList.Where(x => x.CurrentSubscriber.PubSubscriptionID == value).FirstOrDefault();                                
                                    int index = SubscriberList.IndexOf(s);
                                    if (s != null)
                                    {
                                        s = new Subscriber(prodSubResponse.Result, regions, countryList, addressTypeList, paidID, freeID);
                                            s.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(s.Phone.Trim());
                                            s.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(s.Mobile.Trim());
                                            s.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(s.Fax.Trim());
                                        SubscriberList.RemoveAt(index);
                                        SubscriberList.Insert(index, s);
                                    }
                                }
                            }
                        };
                        Common.OpenCircPopoutWindow(sc);
                    }
                    busy.IsBusy = false;
                    //else
                    //{
                    //    if (currRow.SubGenSubscriberID != null && currRow.SubGenSubscriberID > 0)
                    //    {
                    //        //NEED TO CHANGE THIS TO TAKE IN NEW ID FIELD FROM PUBSUBSCRIPTION
                    //        Modules.SubscriptionGenius sg = new SubscriptionGenius(KMPlatform.BusinessLogic.Enums.SubGenControls.Subscriber, currRow.SubGenSubscriberID);
                    //        Windows.PlainPopout pop = new Windows.PlainPopout(sg);
                    //        pop.Show();
                    //    }
                    //    else
                    //        Core_AMS.Utilities.WPF.MessageError("This subscriber has not yet been connected to paid record services. Please try again later.");
                    //}
                    //busy.IsBusy = false;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Cannot view this record.", MessageBoxButton.OK, MessageBoxImage.Error, "Record Not Found");
                busy.IsBusy = false;
            }
        }
        #endregion
    }
}

