using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Circulation.Helpers;
using MessageBox = System.Windows.MessageBox;
using System.ComponentModel;
using System.Windows.Interop;
using System.Data;
using Telerik.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for IndividualTasks.xaml
    /// </summary>

    public partial class SubscriptionContainer : UserControl, INotifyPropertyChanged
    {
        private const string AddressOnlyChangeKey = "AddressOnlyChange";
        private const string RequalOnlyChangeKey = "RequalOnlyChange";

        private const int AddressOnlyChangeCode = 21;
        private const int RequalOnlyChangeCode = 22;
        private const int RenewalPaymentCode = 40;
        private const int IsPaidFalse = 1;
        private const int IsPaidTrue = 3;

        #region Entity/List
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        private FrameworkUAD.Entity.ProductSubscription waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription();
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription;
        private FrameworkUAD.Entity.ProductSubscription originalSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> myProductSubscriptionDetail = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private FrameworkUAD.Entity.ProductSubscription waveMailSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private FrameworkUAD.Entity.WaveMailingDetail myWMDetail = new FrameworkUAD.Entity.WaveMailingDetail();
        private FrameworkUAD.Entity.SubscriptionPaid myProductSubscriptionPaid = new FrameworkUAD.Entity.SubscriptionPaid();
        private FrameworkUAD.Entity.PaidBillTo myPaidBillTo = new FrameworkUAD.Entity.PaidBillTo();
        private FrameworkUAD.Entity.PaidBillTo originalBillTo = new FrameworkUAD.Entity.PaidBillTo();
        private FrameworkUAD.Entity.SubscriptionPaid originalPaid = new FrameworkUAD.Entity.SubscriptionPaid();

        private List<FrameworkUAD_Lookup.Entity.Region> regions = new List<FrameworkUAD_Lookup.Entity.Region>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> categoryCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> addressTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private FrameworkUAD_Lookup.Entity.Action action = new FrameworkUAD_Lookup.Entity.Action();
        private FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix sStatus = new FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix();
        private FrameworkUAD_Lookup.Entity.SubscriptionStatus myStatus = new FrameworkUAD_Lookup.Entity.SubscriptionStatus();
        private FrameworkUAD_Lookup.Entity.Country selCountryPhonePrefix = new FrameworkUAD_Lookup.Entity.Country();
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sstList = new List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>();
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> ssmList = new List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>();
        private List<FrameworkUAD.Entity.MarketingMap> marketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();
        private FrameworkUAD_Lookup.Entity.Action soloAction = new FrameworkUAD_Lookup.Entity.Action();
        private List<FrameworkUAD_Lookup.Entity.Code> qSourceList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> parList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private List<FrameworkUAD_Lookup.Entity.Code> marketingList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD.Entity.CodeSheet> codeSheetList = new List<FrameworkUAD.Entity.CodeSheet>();
        private List<FrameworkUAD.Entity.ResponseGroup> responseGroupList = new List<FrameworkUAD.Entity.ResponseGroup>();
        private List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        private List<FrameworkUAD_Lookup.Entity.ZipCode> zipList = new List<FrameworkUAD_Lookup.Entity.ZipCode>();

        private bool saveWaveMailing = false;
        private bool billToInfoChanged = false;
        private List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
        private List<FrameworkUAD.Entity.Product> productList = new List<FrameworkUAD.Entity.Product>();
        private FrameworkUAD_Lookup.Entity.CategoryCode selectedCat = new FrameworkUAD_Lookup.Entity.CategoryCode();
        private FrameworkUAD_Lookup.Entity.TransactionCode selectedTran = new FrameworkUAD_Lookup.Entity.TransactionCode();
        private int applicationID = 0;
        private int demo7CodeTypeID = 0;
        private bool firstLoad = true;
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private ChildrenContainer MyChildren { get; set; }
        public FrameworkUAD.Entity.Product MyProduct { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        #region Workers
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> tcWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> tcTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> ccWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> subWorker = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> productSubDetailWorker = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> sstWorker = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatusMatrix> ssmWorker = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusMatrixClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionPaid> spWorker = FrameworkServices.ServiceClient.UAD_SubscriptionPaidClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionSearchResult> ssrWorker = FrameworkServices.ServiceClient.UAD_SubscriptionSearchResultClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> aWorker = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> rWorker = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> uasc = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IHistory> hWorker = FrameworkServices.ServiceClient.UAD_HistoryClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catCodeTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IMarketingMap> mmWorker = FrameworkServices.ServiceClient.UAD_MarketingMapClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUserLog> ulWorker = FrameworkServices.ServiceClient.UAS_UserLogClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IWaveMailingDetail> wMDetailWorker = FrameworkServices.ServiceClient.UAD_WaveMailingDetailClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IHistorySubscription> hsWorker = FrameworkServices.ServiceClient.UAD_HistorySubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPaidBillTo> pbtWorker = FrameworkServices.ServiceClient.UAD_PaidBillToClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IHistoryResponseMap> hrmWorker = FrameworkServices.ServiceClient.UAD_HistoryResponseMapClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IHistoryMarketingMap> hmmWorker = FrameworkServices.ServiceClient.UAD_HistoryMarketingMapClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUserLog> userLogW = FrameworkServices.ServiceClient.UAS_UserLogClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IZipCode> zipCodeW = FrameworkServices.ServiceClient.UAD_Lookup_ZipCodeClient();
        private Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
        #endregion
        #region Service Response
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>> prodSubResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> prodSubDetailResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transTypeResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> sstResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>> ssmResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> catTypeResponse;
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.SubscriptionPaid> spResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MarketingMap>> mmResponse;
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.UserLog>> ulIDResponse;
        //private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.QualificationSource>> qSourceResponse;
        //private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Par3c>> parResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse;
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product> productResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> srmResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> psdResponse;
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.PaidBillTo> pbtResponse;
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product> singProdResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.ZipCode>> zipResponse;
        #endregion
        #region Enums
        string IAFree = FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAFree.ToString();
        string AFree = FrameworkUAD_Lookup.Enums.SubscriptionStatus.AFree.ToString();
        string IAPaid = FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAPaid.ToString();
        string APaid = FrameworkUAD_Lookup.Enums.SubscriptionStatus.APaid.ToString();
        string IAProsp = FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAProsp.ToString();
        string AProsp = FrameworkUAD_Lookup.Enums.SubscriptionStatus.AProsp.ToString();
        #endregion
        #region Properties & Classes
        #region Private
        private string _firstName;
        private string _lastName;
        private string _title;
        private string _company;
        private int _addressType;
        private string _address1;
        private string _address2;
        private string _address3;
        private string _city;
        private int _countryID;
        private int _regionID;
        private string _fullZip;
        private string _zip;
        private string _plus4;
        private string _county;
        private string _phone;
        private string _phoneExt;
        private string _mobile;
        private string _fax;
        private string _email;
        private string _website;
        private bool _isPaid;
        private bool _isFreeToPaid;
        private bool _isActive;
        private bool _isNewSubscription;
        private bool _isInActiveWaveMailing;
        private bool _isSubscribed;
        private bool _isProspect;
        private bool _isLocked;
        private int _lockedbyUser;
        private string _onBehalfOf;
        private int _subscriptionStatus;
        private int? _categoryID;
        private int _transactionID;
        private int _pubSubscriptionID;
        private int _qSourceID;
        private int _par3C;
        private int _copies;
        private string _accountNumber;
        private string _memberGroup;
        private string _origSubSrc;
        private string _subSrc;
        private int _subSrcID;
        private string _deliverability;
        private string _verify;
        private DateTime _qDate;
        private int _emailStatusID;
        private bool? _mailPermission;
        private bool? _faxPermission;
        private bool? _phonePermission;
        private bool? _otherProductPermission;
        private bool? _emailRenewPermission;
        private bool? _thirdPartyPermission;
        private bool? _textPermission;
        private string _statusUpdateReason;
        private bool _isEnabled;
        private bool _reactivateEnabled;
        private bool _isCountryEnabled;
        private bool _infoChanged;
        private bool _responsesChanged;
        private bool _adHocsChanged;
        private bool _addressOnlyChange;
        private bool _requalOnlyChange;
        private bool _completeChange;
        private bool _renewalPayment;
        private bool _triggerQualDate;
        private bool qualDateTriggered;
        private bool _copyProfileInfo;
        #endregion
        #region Subscriber
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.FirstName = _firstName;
                myProductSubscription.FirstName = _firstName;
                if (_firstName != originalSubscription.FirstName)
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
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.LastName = _lastName;
                myProductSubscription.LastName = _lastName;
                if (_lastName != originalSubscription.LastName)
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
                myProductSubscription.Title = _title;
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Title = _title;
                if (_title != originalSubscription.Title)
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
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Company = _company;
                myProductSubscription.Company = _company;
                if (_company != originalSubscription.Company)
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
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.AddressTypeCodeId = _addressType;
                myProductSubscription.AddressTypeCodeId = _addressType;
                if (_addressType != originalSubscription.AddressTypeCodeId)
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
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Address = _address1;
                myProductSubscription.Address1 = _address1;
                if (_address1 != originalSubscription.Address1)
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
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Address2 = _address2;
                myProductSubscription.Address2 = _address2;
                if (_address2 != originalSubscription.Address2)
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
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Address3 = _address3;
                myProductSubscription.Address3 = _address3;
                if (_address3 != originalSubscription.Address3)
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
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.City = _city;
                myProductSubscription.City = _city;
                if (_city != originalSubscription.City)
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
                #region Validate Address Info From Zip
                //FrameworkUAD_Lookup.Entity.ZipCode zip = zipList.Where(x => x.PrimaryCity.ToUpper() == _city.ToUpper()).FirstOrDefault();
                //if (zip != null)
                //{
                //    if (this.RegionID <= 0)
                //        this.RegionID = zip.RegionId;
                //    if (string.IsNullOrEmpty(this.County))
                //        this.County = zip.County;
                //    if (string.IsNullOrEmpty(this.FullZip))
                //        this.FullZip = zip.Zip;
                //}
                #endregion
            }
        }
        public int? CountryID
        {
            get { return _countryID; }
            set
            {
                _countryID = (value ?? 0);
                if (_countryID != null && _countryID != 0 && _countryID != myProductSubscription.CountryID)
                    this.FullZip = "";
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.CountryID = _countryID;
                LoadCountryChanges(_countryID);
                myProductSubscription.CountryID = _countryID;
                if (_countryID != originalSubscription.CountryID)
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
        public int? RegionID
        {
            get { return _regionID; }
            set
            {
                _regionID = (value ?? 0);
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.RegionID = _regionID;
                myProductSubscription.RegionID = _regionID;
                if (_regionID != originalSubscription.RegionID)
                {
                    this.InfoChanged = true;
                    if (_regionID <= 0 && myProductSubscription.RegionCode == null)
                        myProductSubscription.RegionCode = string.Empty;
                    else if (regions != null)
                        myProductSubscription.RegionCode = regions.Where(r => r.RegionID == _regionID).Select(s => s.RegionCode).SingleOrDefault();
                }
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RegionID"));
                }
            }
        }
        public string FullZip
        {
            get { return _fullZip; }
            set
            {
                _fullZip = (value ?? "");
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.FullZip = _fullZip;
                if (string.IsNullOrEmpty(_fullZip.Trim()))
                {
                    this.Zip = "";
                    this.Plus4 = "";
                }
                else if (_fullZip.Length == 5)
                {
                    this.Zip = _fullZip.Trim();
                    this.Plus4 = string.Empty;
                }
                else if (_fullZip.Length == 7)
                {
                    // Canada zip
                    //this.Zip = _fullZip.Trim().Substring(0, 3);
                    //this.Plus4 = _fullZip.Trim().Substring(4, 3);
                    this.Zip = _fullZip.Trim();//.Replace(" ", "");  -- removed the space replace on 6/3/16 by JW per MS
                    this.Plus4 = "";
                }
                else if (_fullZip.Length == 9)
                {
                    this.Zip = _fullZip.Trim().Substring(0, 5);
                    this.Plus4 = _fullZip.Trim().Substring(5, 4);
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
                myProductSubscription.ZipCode = _zip;
                if (_zip != originalSubscription.ZipCode)
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
                #region Validate Address Info From Zip
                //FrameworkUAD_Lookup.Entity.ZipCode zip = zipList.Where(x => x.Zip.ToUpper() == _zip.ToUpper()).FirstOrDefault();
                //if (zip != null)
                //{
                //    if (this.RegionID <= 0)
                //        this.RegionID = zip.RegionId;
                //    if (string.IsNullOrEmpty(this.County))
                //        this.County = zip.County;
                //    if (string.IsNullOrEmpty(this.City))
                //        this.City = zip.PrimaryCity;
                //}
                #endregion
            }
        }
        public string Plus4
        {
            get { return _plus4; }
            set
            {
                _plus4 = value;
                myProductSubscription.Plus4 = _plus4;
                if (_plus4 != originalSubscription.Plus4)
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
                _county = (value ?? "").Trim();
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.County = _county;
                myProductSubscription.County = _county;
                if (_county != originalSubscription.County)
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
                _phone = (value ?? "").Trim();
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Phone = _phone;
                myProductSubscription.Phone = _phone;
                if (_phone != originalSubscription.Phone.Trim().Replace("-", ""))
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
                }
            }
        }
        public string PhoneExt
        {
            get { return _phoneExt; }
            set
            {
                _phoneExt = (value.Trim() ?? "");
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.PhoneExt = _phoneExt;
                myProductSubscription.PhoneExt = _phoneExt;
                if (_phoneExt != originalSubscription.PhoneExt)
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
                _mobile = (value ?? "").Trim();
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Mobile = _mobile;
                myProductSubscription.Mobile = _mobile;
                if (_mobile != originalSubscription.Mobile.Trim().Replace("-", ""))
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
                _fax = (value ?? "").Trim();
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Fax = _fax;
                myProductSubscription.Fax = _fax;
                if (_fax != originalSubscription.Fax.Trim().Replace("-", ""))
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
                    if (_email != originalSubscription.Email)
                        this.EmailStatusID = 1;
                    else
                        this.EmailStatusID = originalSubscription.EmailStatusID;
                }
                else
                    this.EmailStatusID = 0;
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Email = _email;
                myProductSubscription.Email = _email;
                if (_email != originalSubscription.Email)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                }
            }
        }
        public string Website
        {
            get { return _website; }
            set
            {
                _website = (value.Trim() ?? "");
                if (_copyProfileInfo == true)
                    MyChildren.BillTo.Website = _website;
                myProductSubscription.Website = _website;
                if (_website != originalSubscription.Website)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Website"));
                }
            }
        }
        public bool IsPaid
        {
            get { return _isPaid; }
            set
            {
                _isPaid = value;
                myProductSubscription.IsPaid = _isPaid;
                if (_isPaid != originalSubscription.IsPaid)
                {
                    this.InfoChanged = true;
                    this.IsFreeToPaid = true;
                }
                else if (CheckChanges() == false)
                {
                    this.InfoChanged = false;
                    this.IsFreeToPaid = false;
                }
                else
                    this.IsFreeToPaid = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPaid"));
                }
            }
        }
        public bool IsFreeToPaid
        {
            get { return _isFreeToPaid; }
            set
            {
                if (!_isNewSubscription)
                {
                    //_isFreeToPaid = value;
                    if (value && !_isPaid)
                    {
                        FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 10 && x.TransactionCodeTypeID == 1).FirstOrDefault();
                        if (tc != null)
                            this.TransactionCodeID = tc.TransactionCodeID;
                        //this.AddressOnlyChange = false;
                        //this.RequalOnlyChange = false;
                        _isFreeToPaid = value;
                    }
                    else if (value)
                    {
                        //this.AddressOnlyChange = false;
                        //this.RequalOnlyChange = false;
                        FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 13 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                        if (tc != null)
                            this.TransactionCodeID = tc.TransactionCodeID;
                        _isFreeToPaid = value;
                    }
                    else
                    {
                        this.AddressOnlyChange = false;
                        this.RequalOnlyChange = false;
                        _isFreeToPaid = value;
                        this.TransactionCodeID = originalSubscription.PubTransactionID;
                        CheckChanges();
                    }
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsFreeToPaid"));
                }
            }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                myProductSubscription.IsActive = _isActive;
                if (_isActive != originalSubscription.IsActive)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsActive"));
                }
            }
        }
        public string OnBehalfOf
        {
            get { return _onBehalfOf; }
            set
            {
                _onBehalfOf = value;
                myProductSubscription.OnBehalfOf = _onBehalfOf;
                if (_onBehalfOf != originalSubscription.OnBehalfOf)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("OnBehalfOf"));
                }
            }
        }
        public int? CategoryCodeID
        {
            get { return _categoryID; }
            set
            {
                _categoryID = value;
                int catValue = categoryCodeList.Where(x => x.CategoryCodeID == _categoryID).Select(x => x.CategoryCodeValue).FirstOrDefault();
                string catName = categoryCodeList.Where(x => x.CategoryCodeID == _categoryID).Select(x => x.CategoryCodeName).FirstOrDefault();
                if (_categoryID != null)
                {
                    lbCategory.Text = catName;
                    myProductSubscription.PubCategoryID = _categoryID.Value;
                    if (catValue == 70 || catValue == 71)
                        this.IsProspect = true;
                    else
                        this.IsProspect = false;
                    if (_categoryID != originalSubscription.PubCategoryID)
                    {
                        this.InfoChanged = true;
                        this.TriggerQualDate = true;
                    }
                    else if (CheckChanges() == false)
                    {
                        this.InfoChanged = false;
                        this.TriggerQualDate = CheckQDateTrigger();
                    }
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CategoryCodeID"));
                }
            }
        }
        public int TransactionCodeID
        {
            get { return _transactionID; }
            set
            {
                int val = transCodeList.Where(x => x.TransactionCodeID == value).Select(x => x.TransactionCodeValue).FirstOrDefault();
                if (this.IsFreeToPaid == false || (val == 64 || val == 31 || val == 32 || val == 65)) //Only allow changes if Record is not F2P/P2F or a Kill.
                {
                    _transactionID = value;
                    myProductSubscription.PubTransactionID = _transactionID;
                    if (MyChildren.Subscription != null)
                    {
                        if (MyChildren.Subscription.TransactionCodeID != _transactionID)
                            MyChildren.Subscription.TransactionCodeID = _transactionID;
                    }
                    if (_transactionID != originalSubscription.PubTransactionID)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TransactionCodeID"));
                    }
                }
            }
        }
        public int PubSubscriptionID
        {
            get { return _pubSubscriptionID; }
            set
            {
                _pubSubscriptionID = value;
            }
        }
        public int LockedByUser
        {
            get { return _lockedbyUser; }
            set
            {
                _lockedbyUser = value;
            }
        }
        public int SubscriptionStatus
        {
            get { return _subscriptionStatus; }
            set
            {
                _subscriptionStatus = value;
                myProductSubscription.SubscriptionStatusID = _subscriptionStatus;
                if (_subscriptionStatus != originalSubscription.SubscriptionStatusID)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SubscriptionStatus"));
                }
            }
        }
        public int QSourceID
        {
            get { return _qSourceID; }
            set
            {
                _qSourceID = value;
                myProductSubscription.PubQSourceID = _qSourceID;
                if (_qSourceID != originalSubscription.PubQSourceID)
                {
                    this.InfoChanged = true;
                    this.TriggerQualDate = true;
                    this.RequalOnlyChange = true;
                }
                else if (CheckChanges() == false)
                {
                    this.InfoChanged = false;
                    this.TriggerQualDate = CheckQDateTrigger();
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("QSourceID"));
                }
            }
        }
        public int Par3C
        {
            get { return _par3C; }
            set
            {
                _par3C = value;
                myProductSubscription.Par3CID = _par3C;
                if (_par3C != originalSubscription.Par3CID)
                {
                    this.InfoChanged = true;
                    this.TriggerQualDate = true;
                    this.RequalOnlyChange = true;
                }
                else if (CheckChanges() == false)
                {
                    this.InfoChanged = false;
                    this.TriggerQualDate = CheckQDateTrigger();
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Par3C"));
                }
            }
        }
        public int Copies
        {
            get { return _copies; }
            set
            {
                _copies = value;
                myProductSubscription.Copies = _copies;
                if (_copies != originalSubscription.Copies)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (MyChildren.Paid != null && _isPaid == true)
                    MyChildren.Paid.Copies = _copies;
                if (MyChildren.Responses != null && MyChildren.Responses.Copies != _copies)
                    MyChildren.Responses.Copies = _copies;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Copies"));
                }
            }
        }
        public string AccountNumber
        {
            get { return _accountNumber; }
            set
            {
                _accountNumber = value;
                myProductSubscription.AccountNumber = _accountNumber;
                if (_accountNumber != originalSubscription.AccountNumber)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AccountNumber"));
                }
            }
        }
        public string MemberGroup
        {
            get { return _memberGroup; }
            set
            {
                _memberGroup = value;
                myProductSubscription.MemberGroup = _memberGroup;
                if (_memberGroup != originalSubscription.MemberGroup)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MemberGroup"));
                }
            }
        }
        public string OriginalSubscriberSourceCode
        {
            get { return _origSubSrc; }
            set
            {
                _origSubSrc = value;
                myProductSubscription.OrigsSrc = _origSubSrc;
                if (_origSubSrc != originalSubscription.OrigsSrc)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("OriginalSubscriberSourceCode"));
                }
            }
        }
        public string SubscriberSourceCode
        {
            get { return _subSrc; }
            set
            {
                _subSrc = value;
                myProductSubscription.SubscriberSourceCode = _subSrc;
                if (_subSrc != originalSubscription.SubscriberSourceCode)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SubscriberSourceCode"));
                }
            }
        }
        public int SubSrcID
        {
            get { return _subSrcID; }
            set
            {
                _subSrcID = value;
                myProductSubscription.SubSrcID = _subSrcID;
                if (_subSrcID != originalSubscription.SubSrcID)
                {
                    this.InfoChanged = true;
                    this.TriggerQualDate = true;
                }
                else if (CheckChanges() == false)
                {
                    this.InfoChanged = false;
                    this.TriggerQualDate = CheckQDateTrigger();
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SubSrcID"));
                }
            }
        }
        public string Deliverability
        {
            get { return _deliverability; }
            set
            {
                _deliverability = value;
                myProductSubscription.Demo7 = _deliverability;
                int val = codeList.Where(x => x.CodeValue == _deliverability && x.CodeTypeId == demo7CodeTypeID).Select(x => x.CodeId).FirstOrDefault();
                if (val > 0)
                    MyChildren.SubscriptionInfo.Deliverability = val;
                if (_deliverability != originalSubscription.Demo7)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Deliverability"));
                }
            }
        }
        public string Verify
        {
            get { return _verify; }
            set
            {
                _verify = value;
                myProductSubscription.Verify = _verify;
                if (_verify != originalSubscription.Verify)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Verify"));
                }
            }
        }
        public DateTime QDate
        {
            get { return _qDate; }
            set
            {
                _qDate = value;
                myProductSubscription.QualificationDate = _qDate;
                if (_qDate != originalSubscription.QualificationDate)
                {
                    this.InfoChanged = true;
                    this.RequalOnlyChange = true;
                }
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("QDate"));
                }
            }
        }
        public int EmailStatusID
        {
            get { return _emailStatusID; }
            set
            {
                _emailStatusID = value;
                myProductSubscription.EmailStatusID = _emailStatusID;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EmailStatusID"));
                }
            }
        }
        public bool? MailPermission
        {
            get { return _mailPermission; }
            set
            {
                _mailPermission = value;
                myProductSubscription.MailPermission = _mailPermission;
                if (_mailPermission != originalSubscription.MailPermission)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MailPermission"));
                }
            }
        }
        public bool? FaxPermission
        {
            get { return _faxPermission; }
            set
            {
                _faxPermission = value;
                myProductSubscription.FaxPermission = _faxPermission;
                if (_faxPermission != originalSubscription.FaxPermission)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FaxPermission"));
                }
            }
        }
        public bool? PhonePermission
        {
            get { return _phonePermission; }
            set
            {
                _phonePermission = value;
                myProductSubscription.PhonePermission = _phonePermission;
                if (_phonePermission != originalSubscription.PhonePermission)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PhonePermission"));
                }
            }
        }
        public bool? OtherProductsPermission
        {
            get { return _otherProductPermission; }
            set
            {
                _otherProductPermission = value;
                myProductSubscription.OtherProductsPermission = _otherProductPermission;
                if (_otherProductPermission != originalSubscription.OtherProductsPermission)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("OtherProductsPermission"));
                }
            }
        }
        public bool? EmailRenewPermission
        {
            get { return _emailRenewPermission; }
            set
            {
                _emailRenewPermission = value;
                myProductSubscription.EmailRenewPermission = _emailRenewPermission;
                if (_emailRenewPermission != originalSubscription.EmailRenewPermission)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EmailRenewPermission"));
                }
            }
        }
        public bool? ThirdPartyPermission
        {
            get { return _thirdPartyPermission; }
            set
            {
                _thirdPartyPermission = value;
                myProductSubscription.ThirdPartyPermission = _thirdPartyPermission;
                if (_thirdPartyPermission != originalSubscription.ThirdPartyPermission)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ThirdPartyPermission"));
                }
            }
        }
        public bool? TextPermission
        {
            get { return _textPermission; }
            set
            {
                _textPermission = value;
                myProductSubscription.TextPermission = _textPermission;
                if (_textPermission != originalSubscription.TextPermission)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TextPermission"));
                }
            }
        }
        public string StatusUpdatedReason
        {
            get { return _statusUpdateReason; }
            set
            {
                _statusUpdateReason = value;
                myProductSubscription.StatusUpdatedReason = _statusUpdateReason;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StatusUpdatedReason"));
                }
            }
        }
        public bool IsSubscribed
        {
            get { return _isSubscribed; }
            set
            {
                _isSubscribed = value;
                myProductSubscription.IsSubscribed = _isSubscribed;
                if (_isSubscribed != originalSubscription.IsSubscribed)
                    this.InfoChanged = true;
                else if (CheckChanges() == false)
                    this.InfoChanged = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSubscribed"));
                }
            }
        }
        public bool IsProspect
        {
            get { return _isProspect; }
            set
            {
                _isProspect = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsProspect"));
                }
            }
        }
        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                _isLocked = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsLocked"));
                }
            }
        }
        public bool IsNewSubscription
        {
            get { return _isNewSubscription; }
            set
            {
                _isNewSubscription = value;
                myProductSubscription.IsNewSubscription = _isNewSubscription;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsNewSubscription"));
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
        public bool Enabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                if (MyChildren != null)
                {
                    if (MyChildren.BillTo != null && MyChildren.BillTo.Enabled != _isEnabled)
                        MyChildren.BillTo.Enabled = _isEnabled;
                    if (MyChildren.Paid != null && MyChildren.Paid.Enabled != _isEnabled)
                        MyChildren.Paid.Enabled = _isEnabled;
                    if (MyChildren.Subscription.Enabled != _isEnabled)
                        MyChildren.Subscription.Enabled = _isEnabled;
                    if (MyChildren.SubscriptionInfo.Enabled != _isEnabled)
                        MyChildren.SubscriptionInfo.Enabled = _isEnabled;
                    if (MyChildren.Responses.Enabled != _isEnabled)
                        MyChildren.Responses.Enabled = _isEnabled;
                    if (MyChildren.Adhocs.Enabled != _isEnabled)
                        MyChildren.Adhocs.Enabled = _isEnabled;
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Enabled"));
                }
            }
        }
        public bool ReactivateEnabled
        {
            get { return _reactivateEnabled; }
            set
            {
                _reactivateEnabled = value;
                if (MyChildren != null)
                {
                    MyChildren.Subscription.ReactivateEnabled = _reactivateEnabled;
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ReactivateEnabled"));
                }
            }
        }
        public bool IsCountryEnabled
        {
            get { return _isCountryEnabled; }
            set
            {
                _isCountryEnabled = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsCountryEnabled"));
                }
            }
        }
        public bool Saved { get; set; }
        #endregion
        #region Changes & Events
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
        public bool ResponsesChanged
        {
            get { return _responsesChanged; }
            set
            {
                _responsesChanged = value;
                if (_responsesChanged == true)
                {
                    this.InfoChanged = true;
                    this.TriggerQualDate = true;
                    this.RequalOnlyChange = true;
                }
                else
                {
                    this.InfoChanged = CheckChanges();
                    this.TriggerQualDate = CheckQDateTrigger();
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("InfoChanged"));
                }
            }
        }
        public bool AdHocsChanged
        {
            get { return _adHocsChanged; }
            set
            {
                _adHocsChanged = value;
                if (_adHocsChanged == true)
                {
                    this.InfoChanged = true;
                    //this.TriggerQualDate = true;
                    //this.RequalOnlyChange = true;
                }
                else
                {
                    this.InfoChanged = CheckChanges();
                    //this.TriggerQualDate = CheckQDateTrigger();
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AdHocsChanged"));
                }
            }
        }
        public bool AddressOnlyChange
        {
            get { return _addressOnlyChange; }
            set
            {
                if (_addressOnlyChange == value || _isNewSubscription)
                {
                    return;
                }

                _addressOnlyChange = value;
                PropertyChangedHandler(_renewalPayment, _isPaid, _addressOnlyChange, _requalOnlyChange);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(AddressOnlyChangeKey));
            }
        }
        public bool RequalOnlyChange
        {
            get { return _requalOnlyChange; }
            set
            {
                if (_requalOnlyChange == value || _isNewSubscription)
                {
                    return;
                }

                _requalOnlyChange = value;
                PropertyChangedHandler(_renewalPayment, _isPaid, _addressOnlyChange, _requalOnlyChange);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(RequalOnlyChangeKey));
            }
        }

        private void PropertyChangedHandler(
            bool renewalPayment, 
            bool isPaid, 
            bool addressOnlyChange, 
            bool requalOnlyChange)
        {
            if (renewalPayment && isPaid)
            {
                SetTransactionCodeId(RenewalPaymentCode, IsPaidTrue);
            }
            else if (addressOnlyChange)
            {
                if (requalOnlyChange == false)
                {
                    CompleteChange = false;
                    SetTransactionCodeId(AddressOnlyChangeCode, isPaid ? IsPaidTrue : IsPaidFalse);
                }
                else
                {
                    CompleteChange = true;
                }
            }
            else if (requalOnlyChange)
            {
                CompleteChange = false;
                SetTransactionCodeId(RequalOnlyChangeCode, isPaid ? IsPaidTrue : IsPaidFalse);
            }
            else
            {
                CompleteChange = false;
                TransactionCodeID = originalSubscription.PubTransactionID;
            }
        }

        private void SetTransactionCodeId(int codeValue, int typeId)
        {
            var transactionCode = transCodeList.FirstOrDefault(item => 
                item.TransactionCodeValue == codeValue && item.TransactionCodeTypeID == typeId);

            if (transactionCode != null)
            {
                TransactionCodeID = transactionCode.TransactionCodeID;
            }
        }

        public bool CompleteChange
        {
            get { return _completeChange; }
            set
            {
                _completeChange = value;
                if (_completeChange == true)
                {
                    if (_isPaid == true)
                    {
                        FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 27 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                        if (tc != null)
                            this.TransactionCodeID = tc.TransactionCodeID;
                    }
                    else
                    {
                        FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 27 && x.TransactionCodeTypeID == 1).FirstOrDefault();
                        if (tc != null)
                            this.TransactionCodeID = tc.TransactionCodeID;
                    }
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CompleteChange"));
                }
            }
        }
        public bool RenewalPayment
        {
            get { return _renewalPayment; }
            set
            {
                if (!_isNewSubscription)
                {
                    _renewalPayment = value;
                    if (_renewalPayment == true && _isPaid == true)
                    {
                        FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 40 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                        if (tc != null)
                            this.TransactionCodeID = tc.TransactionCodeID;
                    }
                    else if (_addressOnlyChange == true && _requalOnlyChange == false)
                    {
                        if (_isPaid == true)
                        {
                            FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                            if (tc != null)
                                this.TransactionCodeID = tc.TransactionCodeID;
                        }
                        else
                        {
                            FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 21 && x.TransactionCodeTypeID == 1).FirstOrDefault();
                            if (tc != null)
                                this.TransactionCodeID = tc.TransactionCodeID;
                        }
                        this.CompleteChange = false;
                    }
                    else if (_addressOnlyChange == true && _requalOnlyChange == true)
                    {
                        this.CompleteChange = true;
                    }
                    else if (_requalOnlyChange == true)
                    {
                        this.CompleteChange = false;
                        if (_isPaid == true)
                        {
                            FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 22 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                            if (tc != null)
                                this.TransactionCodeID = tc.TransactionCodeID;
                        }
                        else
                        {
                            FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 22 && x.TransactionCodeTypeID == 1).FirstOrDefault();
                            if (tc != null)
                                this.TransactionCodeID = tc.TransactionCodeID;
                        }
                    }
                    else
                    {
                        this.CompleteChange = false;
                        this.TransactionCodeID = originalSubscription.PubTransactionID;
                    }
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("RenewalPayment"));
                    }
                }
            }
        }
        public bool TriggerQualDate
        {
            get { return _triggerQualDate; }
            set
            {
                if (_triggerQualDate != value)
                {
                    if (qualDateTriggered == false) //If we have already touched the QDate control, we don't need to make it required any more.
                    {
                        if (this.MyChildren.Subscription.TriggerQualDate == true)
                        {
                            _triggerQualDate = true;
                            MyChildren.Responses.TriggerQualDateChange = true;
                        }
                        else
                        {
                            _triggerQualDate = value;
                            if (MyChildren.Responses.TriggerQualDateChange != _triggerQualDate)
                                MyChildren.Responses.TriggerQualDateChange = _triggerQualDate;
                        }
                    }
                    else
                    {
                        _triggerQualDate = false;
                        MyChildren.Responses.TriggerQualDateChange = false;
                        MyChildren.Subscription.TriggerQualDate = false;
                    }
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TriggerQualDate"));
                }
            }
        }
        public bool CopyProfileInfo
        {
            get { return _copyProfileInfo; }
            set
            {
                _copyProfileInfo = value;
                if (_copyProfileInfo == true)
                {
                    MyChildren.BillTo.FirstName = myProductSubscription.FirstName;
                    MyChildren.BillTo.LastName = myProductSubscription.LastName;
                    MyChildren.BillTo.Company = myProductSubscription.Company;
                    MyChildren.BillTo.Title = myProductSubscription.Title;
                    MyChildren.BillTo.AddressTypeCodeId = myProductSubscription.AddressTypeCodeId;
                    MyChildren.BillTo.Address = myProductSubscription.Address1;
                    MyChildren.BillTo.Address2 = myProductSubscription.Address2;
                    MyChildren.BillTo.Address3 = myProductSubscription.Address3;
                    MyChildren.BillTo.City = myProductSubscription.City;
                    MyChildren.BillTo.CountryID = myProductSubscription.CountryID;
                    MyChildren.BillTo.RegionID = myProductSubscription.RegionID;
                    MyChildren.BillTo.Zip = myProductSubscription.ZipCode;
                    MyChildren.BillTo.FullZip = this.FullZip;
                    MyChildren.BillTo.Plus4 = myProductSubscription.Plus4;
                    MyChildren.BillTo.County = myProductSubscription.County;
                    MyChildren.BillTo.Email = myProductSubscription.Email;
                    MyChildren.BillTo.Phone = myProductSubscription.Phone;
                    MyChildren.BillTo.PhoneExt = myProductSubscription.PhoneExt;
                    MyChildren.BillTo.Fax = myProductSubscription.Fax;
                    MyChildren.BillTo.Mobile = myProductSubscription.Mobile;
                    MyChildren.BillTo.Website = myProductSubscription.Website;
                }
                else
                {
                    MyChildren.BillTo.FirstName = "";
                    MyChildren.BillTo.LastName = "";
                    MyChildren.BillTo.Company = "";
                    MyChildren.BillTo.Title = "";
                    MyChildren.BillTo.AddressTypeCodeId = 0;
                    MyChildren.BillTo.Address = "";
                    MyChildren.BillTo.Address2 = "";
                    MyChildren.BillTo.Address3 = "";
                    MyChildren.BillTo.City = "";
                    MyChildren.BillTo.CountryID = 0;
                    MyChildren.BillTo.RegionID = 0;
                    MyChildren.BillTo.Zip = "";
                    MyChildren.BillTo.Plus4 = "";
                    MyChildren.BillTo.FullZip = "";
                    MyChildren.BillTo.County = "";
                    MyChildren.BillTo.Email = "";
                    MyChildren.BillTo.Phone = "";
                    MyChildren.BillTo.PhoneExt = "";
                    MyChildren.BillTo.Fax = "";
                    MyChildren.BillTo.Mobile = "";
                    MyChildren.BillTo.Website = "";
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CopyProfileInfo"));
                }
            }
        }
        private bool CheckChanges()
        {
            bool infoChanged = false;
            bool addressChanged = false;
            bool requalChanged = false;
            if (this.IsNewSubscription == true)
                infoChanged = true;
            else if (firstLoad == false)
            {
                ValidateGeneralProperties(ref infoChanged);
                ValidateAddressProperties(ref infoChanged, ref addressChanged);

                if (this.Phone.Replace("-", "") != originalSubscription.Phone.Replace("-", ""))
                    infoChanged = true;
                if (this.Mobile.Replace("-", "") != originalSubscription.Mobile.Replace("-", ""))
                    infoChanged = true;
                if (this.Fax.Replace("-", "") != originalSubscription.Fax.Replace("-", ""))
                    infoChanged = true;

                if (MyChildren.BillTo != null && MyChildren.BillTo.MadePaidBillToChange == true && _isPaid == true)
                    infoChanged = true;
                if (MyChildren.Paid != null && MyChildren.Paid.MadePaidChange == true && _isPaid == true)
                    infoChanged = true;
                if (MyChildren.Responses.MadeResponseChange == true)
                {
                    infoChanged = true;
                    requalChanged = true;
                }
                if (MyChildren.Subscription.RequalOnlyChange == true)
                {
                    requalChanged = true;
                }
                this.AddressOnlyChange = addressChanged;
                this.RequalOnlyChange = requalChanged;
            }

            return infoChanged;
        }

        private void ValidateGeneralProperties(ref bool infoChanged)
        {
            SetInfoChanged(FirstName, originalSubscription.FirstName, ref infoChanged);
            SetInfoChanged(LastName, originalSubscription.LastName, ref infoChanged);
            SetInfoChanged(Title, originalSubscription.Title, ref infoChanged);
            SetInfoChanged(Company, originalSubscription.Company, ref infoChanged);
            SetInfoChanged(PhoneExt, originalSubscription.PhoneExt, ref infoChanged);
            SetInfoChanged(Email, originalSubscription.Email, ref infoChanged);
            SetInfoChanged(Website, originalSubscription.Website, ref infoChanged);
            SetInfoChanged(OnBehalfOf, originalSubscription.OnBehalfOf, ref infoChanged);
            SetInfoChanged(Copies.ToString(), originalSubscription.Copies.ToString(), ref infoChanged);
            SetInfoChanged(IsActive.ToString(), originalSubscription.IsActive.ToString(), ref infoChanged);
            SetInfoChanged(AccountNumber, originalSubscription.AccountNumber, ref infoChanged);
            SetInfoChanged(MemberGroup, originalSubscription.MemberGroup, ref infoChanged);
            SetInfoChanged(OriginalSubscriberSourceCode, originalSubscription.OrigsSrc, ref infoChanged);
            SetInfoChanged(SubscriberSourceCode, originalSubscription.SubscriberSourceCode, ref infoChanged);
            SetInfoChanged(SubSrcID.ToString(), originalSubscription.SubSrcID.ToString(), ref infoChanged);
            SetInfoChanged(Deliverability, originalSubscription.Demo7, ref infoChanged);
            SetInfoChanged(Verify, originalSubscription.Verify, ref infoChanged);
            SetInfoChanged(TransactionCodeID.ToString(), originalSubscription.PubTransactionID.ToString(), ref infoChanged);
            SetInfoChanged(CategoryCodeID.ToString(), originalSubscription.PubCategoryID.ToString(), ref infoChanged);
        }

        private void ValidateAddressProperties(ref bool infoChanged, ref bool addressChanged)
        {
            SetInfoChangedAndAddressChanged(AddressType.ToString(), originalSubscription.AddressTypeCodeId.ToString(), ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(Address1, originalSubscription.Address1, ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(Address2, originalSubscription.Address2, ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(Address3, originalSubscription.Address3, ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(City, originalSubscription.City, ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(CountryID.ToString(), originalSubscription.CountryID.ToString(), ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(RegionID.ToString(), originalSubscription.RegionID.ToString(), ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(Zip, originalSubscription.ZipCode, ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(Plus4, originalSubscription.Plus4, ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(County, originalSubscription.County, ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(QSourceID.ToString(), originalSubscription.PubQSourceID.ToString(), ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(Par3C.ToString(), originalSubscription.Par3CID.ToString(), ref infoChanged, ref addressChanged);
            SetInfoChangedAndAddressChanged(QDate.ToString(), originalSubscription.QualificationDate.ToString(), ref infoChanged, ref addressChanged);
        }
        
        private void SetInfoChanged(string propValue1, string propValue2, ref bool infoChanged)
        {
            if (propValue1 != propValue2)
            {
                infoChanged = true;
            }
        }

        private void SetInfoChangedAndAddressChanged(string propValue1, string propValue2, ref bool infoChanged, ref bool addressChanged)
        {
            if (propValue1 != propValue2)
            {
                infoChanged = true;
                addressChanged = true;
            }
        }

        private bool CheckQDateTrigger()
        {
            var infoChanged = false;

            SetInfoChanged(QSourceID.ToString(), myProductSubscription.PubQSourceID.ToString(), ref infoChanged);
            SetInfoChanged(Par3C.ToString(), myProductSubscription.Par3CID.ToString(), ref infoChanged);
            SetInfoChanged(CategoryCodeID.ToString(), myProductSubscription.PubCategoryID.ToString(), ref infoChanged);
            SetInfoChanged(SubSrcID.ToString(), myProductSubscription.SubSrcID.ToString(), ref infoChanged);

            if (MyChildren.Responses.MadeResponseChange)
            {
                infoChanged = true;
            }

            return infoChanged;
        }
        private void UpdateSubscriptionChanges(object sender, PropertyChangedEventArgs e)
        {
            string isPaid = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.IsPaid);
            string isSubscribed = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.IsSubscribed);
            string xactId = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.TransactionCodeID);
            string onBehalfOf = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.OnBehalfOf);
            string catID = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.CategoryCodeID);
            string subStatus = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.SubscriptionStatus);
            string isActive = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.IsActive);
            string isCountryEnabled = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.IsCountryEnabled);
            string requalChange = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.RequalOnlyChange);
            string triggerQualDate = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.TriggerQualDate);
            string isEnabled = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Subscription.Enabled);

            if (e.PropertyName.Equals(isPaid) && _isPaid != MyChildren.Subscription.IsPaid)
                this.IsPaid = MyChildren.Subscription.IsPaid;
            else if (e.PropertyName.Equals(isSubscribed) && _isSubscribed != MyChildren.Subscription.IsSubscribed)
                this.IsSubscribed = MyChildren.Subscription.IsSubscribed;
            else if (e.PropertyName.Equals(xactId) && _transactionID != MyChildren.Subscription.TransactionCodeID)
                this.TransactionCodeID = MyChildren.Subscription.TransactionCodeID;
            else if (e.PropertyName.Equals(onBehalfOf) && _onBehalfOf != MyChildren.Subscription.OnBehalfOf)
                this.OnBehalfOf = MyChildren.Subscription.OnBehalfOf;
            else if (e.PropertyName.Equals(catID) && _categoryID != MyChildren.Subscription.CategoryCodeID)
                this.CategoryCodeID = MyChildren.Subscription.CategoryCodeID;
            else if (e.PropertyName.Equals(subStatus) && _subscriptionStatus != MyChildren.Subscription.SubscriptionStatus)
                this.SubscriptionStatus = MyChildren.Subscription.SubscriptionStatus;
            else if (e.PropertyName.Equals(isActive) && _isActive != MyChildren.Subscription.IsActive)
                this.IsActive = MyChildren.Subscription.IsActive;
            else if (e.PropertyName.Equals(isCountryEnabled) && _isCountryEnabled != MyChildren.Subscription.IsCountryEnabled)
                this.IsCountryEnabled = MyChildren.Subscription.IsCountryEnabled;
            else if (e.PropertyName.Equals(requalChange) && _requalOnlyChange != MyChildren.Subscription.RequalOnlyChange)
                this.RequalOnlyChange = MyChildren.Subscription.RequalOnlyChange;
            else if (e.PropertyName.Equals(triggerQualDate) && _triggerQualDate != MyChildren.Subscription.TriggerQualDate)
                this.TriggerQualDate = MyChildren.Subscription.TriggerQualDate;
            else if (e.PropertyName.Equals(isEnabled) && _isEnabled != MyChildren.Subscription.Enabled)
                this.Enabled = MyChildren.Subscription.Enabled;
        }
        private void UpdateResponseChanges(object sender, PropertyChangedEventArgs e)
        {
            string qSource = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.QSourceID);
            string par3C = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.Par3C);
            string copies = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.Copies);
            string qDate = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.QDate);
            string madeResponseChange = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.MadeResponseChange);
            string triggerQual = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.TriggerQualDateChange);
            string mail = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.MailPermission);
            string fax = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.FaxPermission);
            string phone = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.PhonePermission);
            string other = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.OtherProductsPermission);
            string email = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.EmailRenewPermission);
            string thirdParty = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.ThirdPartyPermission);
            string text = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Responses.TextPermission);

            if (e.PropertyName.Equals(qSource) && _qSourceID != (MyChildren.Responses.QSourceID ?? 0))
                this.QSourceID = (MyChildren.Responses.QSourceID ?? 0);
            else if (e.PropertyName.Equals(par3C) && _par3C != (MyChildren.Responses.Par3C ?? 0))
                this.Par3C = (MyChildren.Responses.Par3C ?? 0);
            else if (e.PropertyName.Equals(copies) && _copies != MyChildren.Responses.Copies)
                this.Copies = MyChildren.Responses.Copies;
            else if (e.PropertyName.Equals(qDate) && _qDate != MyChildren.Responses.QDate)
                this.QDate = MyChildren.Responses.QDate;
            else if (e.PropertyName.Equals(madeResponseChange) && _responsesChanged != MyChildren.Responses.MadeResponseChange)
                this.ResponsesChanged = MyChildren.Responses.MadeResponseChange;
            else if (e.PropertyName.Equals(triggerQual) && _triggerQualDate != MyChildren.Responses.TriggerQualDateChange)
                this.TriggerQualDate = MyChildren.Responses.TriggerQualDateChange;
            else if (e.PropertyName.Equals(mail) && _mailPermission != MyChildren.Responses.MailPermission)
                this.MailPermission = MyChildren.Responses.MailPermission;
            else if (e.PropertyName.Equals(fax) && _faxPermission != MyChildren.Responses.FaxPermission)
                this.FaxPermission = MyChildren.Responses.FaxPermission;
            else if (e.PropertyName.Equals(phone) && _phonePermission != MyChildren.Responses.PhonePermission)
                this.PhonePermission = MyChildren.Responses.PhonePermission;
            else if (e.PropertyName.Equals(other) && _otherProductPermission != MyChildren.Responses.OtherProductsPermission)
                this.OtherProductsPermission = MyChildren.Responses.OtherProductsPermission;
            else if (e.PropertyName.Equals(email) && _emailRenewPermission != MyChildren.Responses.EmailRenewPermission)
                this.EmailRenewPermission = MyChildren.Responses.EmailRenewPermission;
            else if (e.PropertyName.Equals(thirdParty) && _thirdPartyPermission != MyChildren.Responses.ThirdPartyPermission)
                this.ThirdPartyPermission = MyChildren.Responses.ThirdPartyPermission;
            else if (e.PropertyName.Equals(text) && _textPermission != MyChildren.Responses.TextPermission)
                this.TextPermission = MyChildren.Responses.TextPermission;
        }
        private void UpdateSubscriptionInfoChanges(object sender, PropertyChangedEventArgs e)
        {
            string accountNumber = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.SubscriptionInfo.AccountNumber);
            string memberGroup = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.SubscriptionInfo.MemberGroup);
            string origSubSrc = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.SubscriptionInfo.OriginalSubscriberSourceCode);
            string subSrc = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.SubscriptionInfo.SubscriberSourceCode);
            string subSrcID = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.SubscriptionInfo.SubSrcID);
            string deliver = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.SubscriptionInfo.Deliverability);
            string verify = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.SubscriptionInfo.Verify);

            if (e.PropertyName.Equals(accountNumber) && _accountNumber != MyChildren.SubscriptionInfo.AccountNumber)
                this.AccountNumber = MyChildren.SubscriptionInfo.AccountNumber;
            else if (e.PropertyName.Equals(memberGroup) && _memberGroup != MyChildren.SubscriptionInfo.MemberGroup)
                this.MemberGroup = MyChildren.SubscriptionInfo.MemberGroup;
            else if (e.PropertyName.Equals(origSubSrc) && _origSubSrc != MyChildren.SubscriptionInfo.OriginalSubscriberSourceCode)
                this.OriginalSubscriberSourceCode = MyChildren.SubscriptionInfo.OriginalSubscriberSourceCode;
            else if (e.PropertyName.Equals(subSrc) && _subSrc != MyChildren.SubscriptionInfo.SubscriberSourceCode)
                this.SubscriberSourceCode = MyChildren.SubscriptionInfo.SubscriberSourceCode;
            else if (e.PropertyName.Equals(subSrcID) && _subSrcID != (MyChildren.SubscriptionInfo.SubSrcID ?? 0))
                this.SubSrcID = (MyChildren.SubscriptionInfo.SubSrcID ?? 0);
            else if (e.PropertyName.Equals(deliver))
            {
                string val = (codeList.Where(x => x.CodeId == MyChildren.SubscriptionInfo.Deliverability).Select(x => x.CodeValue).FirstOrDefault() ?? "");
                if (this.Deliverability != val)
                    this.Deliverability = val;
            }
            else if (e.PropertyName.Equals(verify) && _verify != MyChildren.SubscriptionInfo.Verify)
                this.Verify = MyChildren.SubscriptionInfo.Verify;
        }
        private void UpdateAdHocChanges(object sender, PropertyChangedEventArgs e)
        {
            string adhocChanged = Core_AMS.Utilities.WPF.GetPropertyName(() => MyChildren.Adhocs.AdHocChanged);

            if (e.PropertyName.Equals(adhocChanged) && _adHocsChanged != MyChildren.Adhocs.AdHocChanged)
                this.AdHocsChanged = MyChildren.Adhocs.AdHocChanged;
        }

        public event Action<int> ReloadSubscriber;
        #endregion

        //ChildrenContainer holds the different modules within the TabViewer.
        private class ChildrenContainer : INotifyPropertyChanged
        {
            private bool _areQuestionsRequired;
            private bool _copiesEnabled;
            public bool AreQuestionsRequired
            {
                get { return _areQuestionsRequired; }
                set
                {
                    _areQuestionsRequired = value;
                    if (this.Responses != null)
                        this.Responses.UpdateRequiredQuestions(_areQuestionsRequired);
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AreQuestionsRequired"));
                    }
                }
            }
            public bool CopiesEnabled
            {
                get { return _areQuestionsRequired; }
                set
                {
                    _copiesEnabled = value;
                    if (this.Responses != null)
                        this.Responses.IsCopiesEnabled = _copiesEnabled;
                    if (this.Paid != null)
                        this.Paid.IsCopiesEnabled = _copiesEnabled;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CopiesEnabled"));
                    }
                }
            }
            public ResponseNew Responses { get; set; }
            public PaidNew Paid { get; set; }
            public PaidBillTo BillTo { get; set; }
            public Subscription Subscription { get; set; }
            public SubscriptionInfo SubscriptionInfo { get; set; }
            public BatchNew History { get; set; }
            public AdHocs Adhocs { get; set; }

            public ChildrenContainer(ResponseNew responses, Subscription subscription, SubscriptionInfo subInfo, BatchNew history, AdHocs ah)
            {
                this.Responses = responses;
                this.Subscription = subscription;
                this.SubscriptionInfo = subInfo;
                this.History = history;
                this.Adhocs = ah;
            }

            public ChildrenContainer(ResponseNew responses, Subscription subscription, SubscriptionInfo subInfo, BatchNew history, PaidNew paid, PaidBillTo billTo, AdHocs ah)
            {
                this.Responses = responses;
                this.Subscription = subscription;
                this.SubscriptionInfo = subInfo;
                this.History = history;
                this.Paid = paid;
                this.BillTo = billTo;
                this.Adhocs = ah;
            }

            public ChildrenContainer() { }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        private class ProductSubscriptionDetailChange
        {
            public FrameworkUAD.Entity.ProductSubscriptionDetail OriginalValues { get; set; }
            public FrameworkUAD.Entity.ProductSubscriptionDetail NewValues { get; set; }

            public ProductSubscriptionDetailChange(FrameworkUAD.Entity.ProductSubscriptionDetail origValues, FrameworkUAD.Entity.ProductSubscriptionDetail newValues)
            {
                this.OriginalValues = origValues;
                this.NewValues = newValues;
            }
        }
        #endregion

        public SubscriptionContainer(KMPlatform.Entity.Client publisher,
            FrameworkUAD.Entity.Product product,
            FrameworkUAD.Entity.ProductSubscription prodSubscription,
            List<FrameworkUAD.Entity.Product> products,
            List<FrameworkUAD.Entity.MarketingMap> mmList = null)
        {
            InitializeComponent();

            categoryCodeList = Home.CategoryCodes;
            catTypeList = Home.CategoryCodeTypes;
            transCodeList = Home.TransactionCodes;
            actionList = Home.Actions;
            codeList = Home.Codes;
            codeTypeList = Home.CodeTypes;
            sstList = Home.SubscriptionStatuses;
            ssmList = Home.SubscriptionStatusMatrices;
            countryList = Home.Countries;
            regions = Home.Regions;
            transCodeTypeList = Home.TransactionCodeTypes;
            marketingMapList = mmList;
            qSourceList = Home.QSourceCodes;
            marketingList = Home.MarketingCodes;
            parList = Home.Par3CCodes;
            zipList = Home.ZipCodes;

            clientList = new List<KMPlatform.Entity.Client>();
            clientList.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients);

            productList = products;

            MyProduct = product;

            this.InfoChanged = false;
            this.Saved = false;

            //Publisher and Publication CAN NOT BE NULL
            if (publisher != null && publisher.ClientID > 0 && product != null && product.PubID > 0)
            {
                myClient = publisher;

                if (prodSubscription != null)
                {
                    if (string.IsNullOrEmpty(prodSubscription.PubCode))
                        prodSubscription.PubCode = product.PubCode;

                    myProductSubscription = prodSubscription;

                    if (myProductSubscription.ProductMapList != null)
                        myProductSubscriptionDetail = myProductSubscription.ProductMapList;
                }
                Setup();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("You MUST have a Publisher and Publication selected. The current process will be stopped.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #region Loading
        private void Setup()
        {
            lbPublisher.Text = myClient.DisplayName;
            lbPublication.Text = MyProduct.PubCode;

            #region Load Lists

            KMPlatform.Entity.Client client = clientList.SingleOrDefault(c => c.ClientID == myClient.ClientID);

            // Use this for retrieve Pubs from specific UAD Databases
            int prodID = -1;
            prodID = MyProduct.PubID;
            //prodID = productList.SingleOrDefault(w => w.IsCirc == true && w.ProductCode.Equals(myUASProduct.ProductCode, StringComparison.CurrentCultureIgnoreCase) && w.ClientID == myUASProduct.ClientID).ProductID;


            if (myProductSubscription != null && (myProductSubscriptionDetail == null || myProductSubscriptionDetail.Count == 0))
            {
                psdResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>>();
                psdResponse = productSubDetailWorker.Proxy.Select(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
                if (Common.CheckResponse(psdResponse.Result, psdResponse.Status) == true)
                {
                    myProductSubscription.ProductMapList = psdResponse.Result;
                }
            }

            //if ((marketingMapList == null || marketingMapList.Count == 0) && myProductSubscription != null && myProductSubscription.IsNewSubscription == false)
            //{
            //    mmResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MarketingMap>>();
            //    mmResponse = mmWorker.Proxy.SelectSubscriber(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
            //    if (Common.CheckResponse(mmResponse.Result, mmResponse.Status))
            //    {
            //        marketingMapList = mmResponse.Result.Where(a => a.IsActive == true).ToList();
            //    }
            //}

            if (codeSheetList == null || codeSheetList.Count == 0)
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> rtWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
                FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> rRespList = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
                rRespList = rtWorker.Proxy.Select(accessKey, prodID, client.ClientConnections);
                if (Common.CheckResponse(rRespList.Result, rRespList.Status) == true)
                    codeSheetList = rRespList.Result.Where(x => x.IsActive == true && !x.ResponseGroup.Equals("Pubcode", StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (codeTypeList == null || codeTypeList.Count == 0)
            {
                codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
                codeTypeResponse = codeTypeWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(codeTypeResponse.Result, codeTypeResponse.Status) == true)
                    codeTypeList = codeTypeResponse.Result;
            }
            if (codeList == null || codeList.Count == 0)
            {
                codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                codeResponse = codeWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(codeResponse.Result, codeResponse.Status) == true)
                    codeList = codeResponse.Result;
            }

            if (responseGroupList == null || responseGroupList.Count == 0)
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rgWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
                FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rGroupList = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
                rGroupList = rgWorker.Proxy.Select(accessKey, client.ClientConnections, prodID);

                if (Common.CheckResponse(rGroupList.Result, rGroupList.Status) == true)
                    responseGroupList = rGroupList.Result.Where(x => x.IsActive == true && (!x.ResponseGroupName.Equals("Pubcode", StringComparison.CurrentCultureIgnoreCase) || !x.DisplayName.Equals("Pubcode", StringComparison.CurrentCultureIgnoreCase))
                        && (x.ResponseGroupTypeId == codeList.SingleOrDefault(r => r.CodeName.Equals(FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_and_UAD.ToString().Replace("_", " "))).CodeId ||
                         x.ResponseGroupTypeId == codeList.SingleOrDefault(r => r.CodeName.Equals(FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_Only.ToString().Replace("_", " "))).CodeId))
                        .OrderBy(y => y.DisplayOrder).ThenBy(z => z.DisplayName).ToList();
            }
            if (countryList == null || countryList.Count == 0)
            {
                countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
                countryResponse = uasc.Proxy.Select(accessKey);
                if (Common.CheckResponse(countryResponse.Result, countryResponse.Status) == true)
                    countryList = countryResponse.Result.Where(f => f.SortOrder != 0).ToList();
            }

            if (regions == null || regions.Count == 0)
            {
                regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
                regionResponse = rWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(regionResponse.Result, regionResponse.Status) == true)
                    regions = regionResponse.Result.Where(x => !x.RegionCode.Equals("MX")).ToList();
            }

            if (transCodeList == null || transCodeList.Count == 0)
            {
                transResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
                transResponse = tcWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(transResponse.Result, transResponse.Status) == true)
                    transCodeList = transResponse.Result;
            }

            if (transCodeTypeList == null || transCodeTypeList.Count == 0)
            {
                transTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
                transTypeResponse = tcTypeWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(transTypeResponse.Result, transTypeResponse.Status) == true)
                    transCodeTypeList = transTypeResponse.Result;
            }

            if (categoryCodeList == null || categoryCodeList.Count == 0)
            {
                catResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
                catResponse = ccWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(catResponse.Result, catResponse.Status) == true)
                    categoryCodeList = catResponse.Result;
            }

            if (catTypeList == null || catTypeList.Count == 0)
            {
                catTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
                catTypeResponse = catCodeTypeWorker.Proxy.Select(accessKey);
                if (catTypeResponse != null && catTypeResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    catTypeList = catTypeResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            if (actionList == null || actionList.Count == 0)
            {
                actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
                actionResponse = aWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(actionResponse.Result, actionResponse.Status) == true)
                    actionList = actionResponse.Result.Where(a => a.IsActive == true).ToList();
            }

            if (sstList == null || sstList.Count == 0)
            {
                sstResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>>();
                sstResponse = sstWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(sstResponse.Result, sstResponse.Status) == true)
                    sstList = sstResponse.Result.Where(a => a.IsActive == true).ToList();
            }

            if (ssmList == null || ssmList.Count == 0)
            {
                ssmResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>>();
                ssmResponse = ssmWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(ssmResponse.Result, ssmResponse.Status) == true)
                    ssmList = ssmResponse.Result.Where(x => x.IsActive == true).ToList();
            }

            if (qSourceList == null || qSourceList.Count == 0)
            {
                int type = codeTypeList.Where(x => x.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " ")).FirstOrDefault().CodeTypeId;
                if (type > 0)
                {
                    qSourceList = codeList.Where(x => x.CodeTypeId == type).ToList();
                }
            }
            if (parList == null || parList.Count == 0)
            {
                int type = codeTypeList.Where(x => x.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Par3c.ToString()).FirstOrDefault().CodeTypeId;
                if (type > 0)
                {
                    parList = codeList.Where(x => x.CodeTypeId == type).ToList();
                }
            }
            if (marketingList == null || marketingList.Count == 0)
            {
                int type = codeTypeList.Where(x => x.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Marketing.ToString()).FirstOrDefault().CodeTypeId;
                if (type > 0)
                {
                    marketingList = codeList.Where(x => x.CodeTypeId == type).ToList();
                }
            }
            //if(zipList == null || zipList.Count == 0)
            //{
            //    zipResponse = zipCodeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            //    if(Helpers.Common.CheckResponse(zipResponse.Result, zipResponse.Status))
            //    {
            //        zipList = zipResponse.Result;
            //    }
            //}

            #endregion

            int addrType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Address.ToString())).CodeTypeId;
            addressTypes = codeList.Where(x => x.CodeTypeId == addrType).ToList();
            if (addressTypes.Where(x => x.CodeId == 0).Count() == 0)
                addressTypes.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, DisplayName = "", IsActive = true });

            FrameworkUAD_Lookup.Entity.CodeType ct = codeTypeList.Where(x => x.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString()).FirstOrDefault();
            if (ct != null)
                demo7CodeTypeID = ct.CodeTypeId;

            cbAddressType.ItemsSource = addressTypes.OrderBy(x => x.CodeId);
            cbAddressType.DisplayMemberPath = "DisplayName";
            cbAddressType.SelectedValuePath = "CodeId";

            if (countryList.Where(x => x.CountryID == 0).Count() == 0)
                countryList.Add(new FrameworkUAD_Lookup.Entity.Country() { CountryID = 0, ShortName = "", FullName = "", SortOrder = 0 });

            cbCountry.ItemsSource = countryList.OrderByDescending(o => o.CountryID == 0).ThenByDescending(o => o.CountryID == 1).ThenByDescending(s => s.CountryID == 2).ThenByDescending(s => s.CountryID == 429).ThenBy(x => x.ShortName);
            cbCountry.SelectedValuePath = "CountryID";
            cbCountry.DisplayMemberPath = "ShortName";

            if (regions.Where(x => x.RegionID == 0).Count() == 0)
                regions.Add(new FrameworkUAD_Lookup.Entity.Region() { RegionID = 0, RegionCode = "", RegionName = "", RegionGroupID = 0, CountryID = 0 });
            if (myProductSubscription != null)
            {
                if (myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID ||
                    myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
                    cbState.ItemsSource = regions.Where(x => x.CountryID == myProductSubscription.CountryID).OrderBy(x => x.RegionName);
                else if (myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.MEXICO.ToString().Replace("_", " ")).CountryID)
                    cbState.ItemsSource = regions.Where(x => x.CountryID == 0 || x.CountryID == myProductSubscription.CountryID).OrderBy(x => x.RegionName);
                else if (myProductSubscription.CountryID <= 0 && myProductSubscription.RegionID >= 1)
                {
                    int id = regions.Where(x => x.RegionID == myProductSubscription.RegionID).FirstOrDefault().CountryID;
                    cbState.ItemsSource = regions.Where(x => x.CountryID == id).OrderBy(x => x.RegionName);
                }
                else
                    cbState.ItemsSource = regions.OrderByDescending(r => r.CountryID == 0).OrderBy(r => r.RegionName).ToList();
            }
            else
                cbState.ItemsSource = regions.OrderByDescending(r => r.CountryID == 0).OrderBy(r => r.RegionName).ToList();

            cbState.SelectedValuePath = "RegionID";
            cbState.DisplayMemberPath = "RegionName";

            if (myProductSubscription != null && myProductSubscription.PubSubscriptionID > 0)
            {
                BindProfile(myProductSubscription);
                this.IsNewSubscription = false;
            }
            else
            {
                myProductSubscription = new FrameworkUAD.Entity.ProductSubscription();
                myProductSubscription.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                this.IsNewSubscription = true;
                myProductSubscription.PubID = MyProduct.PubID;
                LoadModules();
                this.Enabled = true;
                this.IsLocked = false;
                this.CountryID = 1;
            }
            qualDateTriggered = false;
        }
        private void LoadModules()
        {
            SubscriptionInfo si = new SubscriptionInfo(myClient, MyProduct, myProductSubscription);
            si.Name = "modSI";
            tabSubInfo.Content = si;

            ResponseNew resNew = new ResponseNew(myClient, MyProduct, myProductSubscription, responseGroupList, codeSheetList);
            resNew.Name = "modResp";
            tabQual.Content = resNew;

            Subscription sub = new Subscription(myClient, MyProduct, myProductSubscription);
            sub.Name = "modSub";
            tabSub.Content = sub;

            BatchNew hisNew = new BatchNew(responseGroupList, codeSheetList, myClient, MyProduct, myProductSubscription);

            AdHocs ah = new AdHocs(myProductSubscription);
            tabAdHoc.Content = ah;

            hisNew.Name = "modHistory";
            tabHistory.Content = hisNew;

            MyChildren = new ChildrenContainer(resNew, sub, si, hisNew, ah);

            MyChildren.Subscription.UpdateRequiredQuestions += value => MyChildren.AreQuestionsRequired = value;
            MyChildren.Subscription.UpdateCopiesEnabled += value =>
            {
                if (value == false)
                    this.Copies = 1;
                MyChildren.CopiesEnabled = value;
            };
            MyChildren.Subscription.PropertyChanged += UpdateSubscriptionChanges;
            MyChildren.Responses.PropertyChanged += UpdateResponseChanges;
            MyChildren.Responses.QualDateChanged += value =>
            {
                qualDateTriggered = value;
                if (qualDateTriggered == true)
                {
                    this.MyChildren.Subscription.TriggerQualDate = false;
                }
            };
            MyChildren.SubscriptionInfo.PropertyChanged += UpdateSubscriptionInfoChanges;
            MyChildren.Adhocs.PropertyChanged += UpdateAdHocChanges;

            SetUpPaidFunctions();

            if (myProductSubscription.IsNewSubscription == true)
                tbSeqSearch.IsEnabledChanged += (s, e) => tbSeqSearch.Focus();
        }
        public void BindProfile(FrameworkUAD.Entity.ProductSubscription subscriber)
        {
            myProductSubscription = subscriber;
            originalSubscription = new FrameworkUAD.Entity.ProductSubscription(myProductSubscription);
            firstLoad = true;

            myProductSubscription.AdHocFields = productSubWorker.Proxy.Get_AdHocs_PubSubscription(accessKey, myProductSubscription.PubID, myProductSubscription.PubSubscriptionID, myClient.ClientConnections).Result;

            LoadModules();

            #region Set Profile fields

            this.FirstName = myProductSubscription.FirstName;
            this.LastName = myProductSubscription.LastName;
            this.Title = myProductSubscription.Title;
            this.Company = myProductSubscription.Company;
            this.IsSubscribed = myProductSubscription.IsSubscribed;
            this.QSourceID = myProductSubscription.PubQSourceID;
            this.CategoryCodeID = myProductSubscription.PubCategoryID;
            this.SubscriptionStatus = myProductSubscription.SubscriptionStatusID;
            this.SubscriberSourceCode = myProductSubscription.SubscriberSourceCode;
            this.SubSrcID = myProductSubscription.SubSrcID;
            this.OriginalSubscriberSourceCode = myProductSubscription.OrigsSrc;
            this.IsActive = myProductSubscription.IsActive;
            this.OnBehalfOf = myProductSubscription.OnBehalfOf;
            this.TransactionCodeID = myProductSubscription.PubTransactionID;
            this.AccountNumber = myProductSubscription.AccountNumber;
            this.MemberGroup = myProductSubscription.MemberGroup;
            this.Deliverability = myProductSubscription.Demo7;
            this.Verify = myProductSubscription.Verify;
            this.County = myProductSubscription.County;
            this.Par3C = myProductSubscription.Par3CID;
            this.QDate = myProductSubscription.QualificationDate ?? DateTime.Today;
            this.IsInActiveWaveMailing = myProductSubscription.IsInActiveWaveMailing;
            this.IsLocked = myProductSubscription.IsLocked;
            this.LockedByUser = myProductSubscription.LockedByUserID;
            this.PubSubscriptionID = myProductSubscription.PubSubscriptionID;
            this.IsNewSubscription = myProductSubscription.IsNewSubscription;
            this.MailPermission = myProductSubscription.MailPermission;
            this.FaxPermission = myProductSubscription.FaxPermission;
            this.PhonePermission = myProductSubscription.PhonePermission;
            this.OtherProductsPermission = myProductSubscription.OtherProductsPermission;
            this.EmailRenewPermission = myProductSubscription.EmailRenewPermission;
            this.ThirdPartyPermission = myProductSubscription.ThirdPartyPermission;
            this.TextPermission = myProductSubscription.TextPermission;

            if (myProductSubscription.RegionID == null || myProductSubscription.RegionID == 0)
            {
                if (myProductSubscription.RegionCode != null && myProductSubscription.RegionCode != "")
                {
                    int regionID = regions.Where(x => x.RegionCode == myProductSubscription.RegionCode).Select(x => x.RegionID).FirstOrDefault();
                    if (regionID > 0)
                        this.RegionID = regionID;
                }
            }
            else
                this.RegionID = myProductSubscription.RegionID;

            if (myProductSubscription.CountryID == null || myProductSubscription.CountryID == 0)
            {
                if (myProductSubscription.Country != null && myProductSubscription.Country != "")
                {
                    int countryID = countryList.Where(x => x.ShortName.Equals(myProductSubscription.Country, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.CountryID).FirstOrDefault();
                    if (countryID > 0)
                        this.CountryID = countryID;
                }
                else
                {
                    this.CountryID = 1;
                }
            }
            else
                this.CountryID = myProductSubscription.CountryID;

            this.AddressType = myProductSubscription.AddressTypeCodeId;
            this.RegionID = myProductSubscription.RegionID;
            this.City = myProductSubscription.City;

            this.Address1 = myProductSubscription.Address1;
            this.Address2 = myProductSubscription.Address2;
            this.Address3 = myProductSubscription.Address3;
            if (myProductSubscription.ZipCode == null)
                myProductSubscription.ZipCode = "";
            if (myProductSubscription.FullZip == null)
                myProductSubscription.FullZip = "";

            if (myProductSubscription.CountryID != 0 && (myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID
                || myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID
                || myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName.Replace("-", "").Replace("_", " ").Trim() == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.MEXICO.ToString().Replace("_", " ")).CountryID))
            {
                if (!string.IsNullOrEmpty(myProductSubscription.ZipCode) && !string.IsNullOrEmpty(myProductSubscription.Plus4)
                    && myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID)
                {
                    if (myProductSubscription.ZipCode.Length > 5)
                        myProductSubscription.FullZip = myProductSubscription.ZipCode.Replace("-", "");
                    else
                        myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
                }
                else if (!string.IsNullOrEmpty(myProductSubscription.ZipCode)
                    && myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
                {
                    if (myProductSubscription.ZipCode.Length > 3)
                        myProductSubscription.FullZip = myProductSubscription.ZipCode.Replace(" ", "");
                    else
                        myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
                }
                else
                    myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
            }
            else
            {
                myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
            }

            if (string.IsNullOrEmpty(this.FullZip) && myProductSubscription.FullZip.Length > 0)
                this.FullZip = myProductSubscription.FullZip;

            if (!string.IsNullOrEmpty(myProductSubscription.Phone))
            {
                this.Phone = myProductSubscription.Phone.Trim().Replace("-", "");
            }
            else
                this.Phone = myProductSubscription.Phone;

            this.PhoneExt = myProductSubscription.PhoneExt;

            if (!string.IsNullOrEmpty(myProductSubscription.Mobile))
            {
                this.Mobile = myProductSubscription.Mobile.Trim().Replace("-", "");
            }
            else
                this.Mobile = myProductSubscription.Mobile;

            if (!string.IsNullOrEmpty(myProductSubscription.Fax))
            {
                this.Fax = myProductSubscription.Fax.Trim().Replace("-", "");
            }
            else
                this.Fax = myProductSubscription.Fax;

            this.Email = myProductSubscription.Email;
            this.EmailStatusID = myProductSubscription.EmailStatusID;
            this.Website = myProductSubscription.Website;
            tbSequence.Text = myProductSubscription.SequenceID.ToString();
            tbSubID.Text = myProductSubscription.SubscriptionID.ToString();
            
            if (myProductSubscription.PubID == 0)
                myProductSubscription.PubID = MyProduct.PubID;

            #endregion

            tbFname.CaretIndex = tbFname.Text.Length;
            tbFname.Focus();

            #region Set Remainig Fields

            MyChildren.AreQuestionsRequired = false;
            IsPaid = myProductSubscription.IsPaid;

            if (myProductSubscription != null && myProductSubscription.IsNewSubscription == false)
            {
                myStatus = sstList.SingleOrDefault(s => s.SubscriptionStatusID == myProductSubscription.SubscriptionStatusID);

                FrameworkUAD_Lookup.Entity.CategoryCode soloCat = categoryCodeList.SingleOrDefault(v => v.CategoryCodeID == myProductSubscription.PubCategoryID);
                selectedCat = soloCat;

                if (myProductSubscription.PubTransactionID > 0)
                {
                    int codeTypeId = codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Action.ToString())).CodeTypeId;
                    int codeid = codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId && x.CodeName.Equals(FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace("_", " "))).CodeId;
                    FrameworkUAD_Lookup.Entity.Action action = new FrameworkUAD_Lookup.Entity.Action();
                    int actionId = 0;
                    action = actionList.SingleOrDefault(a => a.CategoryCodeID == soloCat.CategoryCodeID && a.TransactionCodeID == myProductSubscription.PubTransactionID && a.ActionTypeID == codeid);
                    if (action != null)
                    {
                        actionId = action.ActionID;
                    }
                    else
                    {
                        busy.IsBusy = false;
                        this.Enabled = false;
                        Core_AMS.Utilities.WPF.MessageError("There was a problem loading this Subscriber. Please contact customer service if this problem continues.");
                        return;
                    }

                    if (actionId != null && actionId > 0)
                    {
                        soloAction = actionList.SingleOrDefault(x => x.ActionID == actionId);
                    }

                    if (myProductSubscription != null && myProductSubscription.IsSubscribed == true && soloAction != null &&
                        (soloCat.CategoryCodeValue == 11 || soloCat.CategoryCodeValue == 21 || soloCat.CategoryCodeValue == 25 || soloCat.CategoryCodeValue == 28 || soloCat.CategoryCodeValue == 31 ||
                        soloCat.CategoryCodeValue == 35 || soloCat.CategoryCodeValue == 51 || soloCat.CategoryCodeValue == 56 || soloCat.CategoryCodeValue == 62))
                    {
                        MyChildren.CopiesEnabled = true;
                    }
                    else
                    {
                        MyChildren.CopiesEnabled = false;
                    }

                    this.Copies = myProductSubscription.Copies;

                    // Get CatType to determine if asterisk should be added after the question

                    if (soloAction != null)
                    {
                        FrameworkUAD_Lookup.Entity.CategoryCodeType ccType = new FrameworkUAD_Lookup.Entity.CategoryCodeType();
                        FrameworkUAD_Lookup.Entity.CategoryCode cc = categoryCodeList.FirstOrDefault(x => x.CategoryCodeID == soloAction.CategoryCodeID);
                        if (cc != null)
                        {
                            int cctId = cc.CategoryCodeTypeID;
                            ccType = catTypeList.SingleOrDefault(z => z.CategoryCodeTypeID == cctId);
                            if (ccType.CategoryCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")) ||
                               ccType.CategoryCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")))
                            {
                                MyChildren.AreQuestionsRequired = true;
                            }
                        }
                    }
                }
            }

            // Default phone prefix to 1 for U.S. and Canada
            if (myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID
                || myProductSubscription.CountryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
            {
                tbPhoneCode.Text = "1";
            }
            else if (selCountryPhonePrefix != null)
            {
                selCountryPhonePrefix = countryList.SingleOrDefault(x => x.CountryID == myProductSubscription.CountryID);
                tbPhoneCode.Text = selCountryPhonePrefix.PhonePrefix.ToString();
            }
            else
                tbPhoneCode.Text = "";

            myProductSubscription.Phone = myProductSubscription.Phone.Replace("-", "");
            myProductSubscription.Fax = myProductSubscription.Fax.Replace("-", "");
            myProductSubscription.Mobile = myProductSubscription.Mobile.Replace("-", "");
            originalSubscription = new FrameworkUAD.Entity.ProductSubscription(myProductSubscription);
            FrameworkUAD_Lookup.Entity.SubscriptionStatus sst = sstList.Where(x => x.SubscriptionStatusID == SubscriptionStatus).FirstOrDefault();

            ReLoadProduct();
            if (MyProduct.AllowDataEntry == false || (myProductSubscription.IsLocked == true && myProductSubscription.LockedByUserID != FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID))
            {
                this.Enabled = false;
                this.ReactivateEnabled = false;
                //if (MyProduct.AllowDataEntry == false)
                //    Core_AMS.Utilities.WPF.Message("Data Entry is currently locked for this publication. Edits can not be made.");
                //else
                //    Core_AMS.Utilities.WPF.Message("Another user is currently editing this Subscriber. Edits can not be made.");
            }
            else if (sst != null && (sst.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAFree.ToString() ||
                sst.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAPaid.ToString() ||
                sst.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAProsp.ToString()))
            {
                this.Enabled = false;
                this.ReactivateEnabled = true;
            }
            else
            {
                this.Enabled = true;
                this.ReactivateEnabled = true;
            }

            #endregion
        }
        #endregion
        #region Suggest Match
        string lnameSuggestMatch = string.Empty;
        string fnameSuggestMatch = string.Empty;
        string emailSuggestMatch = string.Empty;

        private Core_AMS.Utilities.Enums.DialogResponses? DoSuggestMatch()
        {
            #region Find Potential Matches
            DataTable result = new DataTable();

            FrameworkUAS.Service.Response<DataTable> subResponse = new FrameworkUAS.Service.Response<DataTable>();
            Core_AMS.Utilities.Enums.DialogResponses? response = null;

            if (myProductSubscription.SubscriptionID <= 0 || myProductSubscription.IsNewSubscription == true)
            {
                #region Suggest Matches
                subResponse = subWorker.Proxy.FindMatches(accessKey, myProductSubscription.PubID, myProductSubscription.FirstName, myProductSubscription.LastName, myProductSubscription.Company,
                    myProductSubscription.Address1, myProductSubscription.RegionCode, myProductSubscription.ZipCode, myProductSubscription.Phone, myProductSubscription.Email, myProductSubscription.Title,
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

                if (subResponse.Result != null && subResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    result = subResponse.Result;
                    result.Rows.Cast<DataRow>().Where(r => (int)r["SubscriptionID"] == myProductSubscription.SubscriptionID).ToList().ForEach(r => r.Delete());
                    result.AcceptChanges();
                    if (result.Rows.Count > 0)
                    {
                        Windows.SuggestMatch sm = new Windows.SuggestMatch(result);
                        sm.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                        sm.Width = 800;

                        if (System.Windows.Forms.SystemInformation.MonitorCount > 1)
                        {
                            System.Drawing.Rectangle workingArea = GetActiveScreen(sm);
                            sm.Left = (workingArea.Width - sm.Width) / 2 + workingArea.Left;
                            sm.Top = (workingArea.Height - sm.Height) / 2 + workingArea.Top;
                        }
                        else
                        {
                            sm.Left = (SystemParameters.WorkArea.Width - sm.Width) / 2 + SystemParameters.WorkArea.Left;
                            sm.Top = (SystemParameters.WorkArea.Height - sm.Height) / 2 + SystemParameters.WorkArea.Top;
                        }

                        DoubleAnimation animationWidth = new DoubleAnimation
                        {
                            To = 310,
                            Duration = TimeSpan.FromSeconds(1),
                        };
                        sm.BeginAnimation(Rectangle.HeightProperty, animationWidth);
                        sm.DialogResponse += value => response = value;
                        sm.ShowDialog();
                    }
                }
                #endregion
            }
            #endregion
            return response;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // This method is for suggest match and search
            List<FrameworkUAD.Entity.ProductSubscription> subs = new List<FrameworkUAD.Entity.ProductSubscription>();
            List<FrameworkUAD.Entity.ProductSubscription> checkSubs = new List<FrameworkUAD.Entity.ProductSubscription>();
            FrameworkUAD.Entity.ProductSubscription soloSubscription = new FrameworkUAD.Entity.ProductSubscription();

            KMPlatform.Entity.Client soloPublisher = new KMPlatform.Entity.Client();
            KMPlatform.Object.Product soloPublication = new KMPlatform.Object.Product();
            SubscriptionContainer sc;
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> prodSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
            FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>> prodSubListResposne = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>>();
            productSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();

            BackgroundWorker bw = new BackgroundWorker();
            int seq = 0;
            int.TryParse(tbSeqSearch.Text, out seq);
            busy.IsBusy = true;

            if (seq > 0)
            {
                #region Search on Sequence
                bw.DoWork += (o, ea) =>
                {
                    prodSubListResposne = prodSubWorker.Proxy.Search(accessKey, myClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName, "", "", "", "", "", "", "", "", "", "", "", seq);
                    if (Common.CheckResponse(prodSubListResposne.Result, prodSubListResposne.Status) == true)
                        subs = prodSubListResposne.Result;

                    // Need to do a bulk select here for subs by SubscriptionID

                    checkSubs = subs.Where(x => x.PubID == MyProduct.PubID).ToList();
                    if (checkSubs.Count > 0)
                    {
                        if (checkSubs.Count > 0)
                            soloSubscription = subs.OrderByDescending(x => x.DateUpdated).FirstOrDefault(x => x.PubID == MyProduct.PubID);
                        else
                            soloSubscription = subs.FirstOrDefault(x => x.PubID == MyProduct.PubID);
                    }
                    else
                        soloSubscription = subs.SingleOrDefault(x => x.PubID == MyProduct.PubID);
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    if (soloSubscription != null)
                    {
                        soloPublisher = myClient;
                        //soloPublication = myUASProduct;

                        sc = new SubscriptionContainer(soloPublisher, MyProduct, soloSubscription, productList);

                        if (soloSubscription.IsLocked == true)
                            Core_AMS.Utilities.WPF.Message("This subscription is currently locked.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Subscription Locked.");
                        else
                            prodSubWorker.Proxy.UpdateLock(accessKey, soloSubscription.SubscriptionID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, myClient.ClientConnections);

                        ReturnToSearch();
                        Circulation.Helpers.Common.OpenCircPopoutWindow(sc);
                    }
                    else
                        Core_AMS.Utilities.WPF.Message("No search results found.", MessageBoxButton.OK, MessageBoxImage.Information, "No Results Found");
                    busy.IsBusy = false;
                };
                #endregion
            }
            else if (!string.IsNullOrEmpty(tbFname.Text) || !string.IsNullOrEmpty(tbLname.Text) || !string.IsNullOrEmpty(tbEmail.Text))
            {
                busy.IsBusy = false;
                fnameSuggestMatch = string.Empty;
                lnameSuggestMatch = string.Empty;
                emailSuggestMatch = string.Empty;

                DoSuggestMatch();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("At least one search condition must exist to perform a search.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Search");
                busy.IsBusy = false;
            }

            bw.RunWorkerAsync();
            busy.IsBusy = false;
        }
        #endregion
        #region Saving
        private void Save()
        {
            //FrameworkServices.ServiceClient<UAD_WS.Interface.IHistorySubscription> bwhsWorker = FrameworkServices.ServiceClient.UAD_HistorySubscriptionClient();
            //FrameworkServices.ServiceClient<UAD_WS.Interface.IHistoryPaid> hpWorker = FrameworkServices.ServiceClient.UAD_HistoryPaidClient();
            //FrameworkServices.ServiceClient<UAD_WS.Interface.IHistoryPaidBillTo> hpbtWorker = FrameworkServices.ServiceClient.UAD_HistoryPaidBillToClient();
            //FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog> ulResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog>();
            //KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            //int historyPaidID = -1;
            //int historyPaidBillToID = -1;
            //int userLogIdPaid = -1;
            //int userLogIdPaidBillTo = -1;
            //int userLogID = 0;

            //if (myProductSubscription.IsNewSubscription == false && originalSubscription.IsInActiveWaveMailing == true)
            //    CompareSubscriber();

            //#region Save PubSubscription/Subscription And Log
            //bool isNew = myProductSubscription.IsNewSubscription;
            //if (myProductSubscription.CountryID == 0)
            //    myProductSubscription.CountryID = 1;
            //if (isNew == true)
            //    ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
            //else
            //{
            //    myProductSubscription.DateUpdated = DateTime.Now;
            //    myProductSubscription.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //}

            //FrameworkUAS.Service.Response<int> saveResp = productSubWorker.Proxy.Save(accessKey, myProductSubscription, myClient.ClientConnections);

            //if (saveResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //{
            //    We need to select the Product Subscription we just saved to get PubSubscriptionID and SubscriptionID (in case either were added)
            //    FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> prodSubResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
            //    prodSubResponse = productSubWorker.Proxy.SelectProductSubscription(accessKey, saveResp.Result, myClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientName);
            //    if (prodSubResponse.Result != null && prodSubResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //    {
            //        myProductSubscription = prodSubResponse.Result;
            //    }
            //}

            //if (this.IsInActiveWaveMailing == true && saveWaveMailing == true)
            //{
            //    ulResponse = ulWorker.Proxy.CreateLog(accessKey,
            //                            applicationID,
            //                            ult,
            //                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //                            "ProductSubscription",
            //                            jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(originalSubscription),
            //                            jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(waveMailSubscriber));

            //    wMDetailWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myWMDetail, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            //}
            //else
            //{
            //ulResponse = ulWorker.Proxy.CreateLog(accessKey,
            //                                        applicationID,
            //                                        ult,
            //                                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //                                        "ProductSubscription",
            //                                        jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(originalSubscription),
            //                                        jf.ToJson<FrameworkUAD.Entity.ProductSubscription>(myProductSubscription));
            //}

            //if (ulResponse.Result != null && ulResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //    userLogID = ulResponse.Result.UserLogID;
            //#endregion

            //#region Save Paid Fields
            //if (myClient.HasPaid == true && myProductSubscription.IsPaid == true)
            //{
            //    userLogIdPaid = SavePaid(applicationID, myClient);
            //    if(myProductSubscriptionPaid != null && myProductSubscriptionPaid.SubscriptionPaidID > 0)
            //        myPaidBillTo.SubscriptionPaidID = myProductSubscriptionPaid.SubscriptionPaidID;

            //    userLogIdPaidBillTo = SavePaidBillTo(applicationID, myClient);
            //}
            //#endregion

            // Only create history and increment batch if something actually saved
            //if ((userLogID > 0 || userLogIdPaid > 0 || userLogIdPaidBillTo > 0) && this.InfoChanged)
            //{
            //    FrameworkUAD.Entity.Batch batch = Common.CurrentBatch(MyProduct.PubID);

            //    int historySubscriptionID = 0;

            //    FrameworkUAS.Service.Response<int> historyResponse = new FrameworkUAS.Service.Response<int>();
            //    historyResponse = bwhsWorker.Proxy.SaveForSubscriber(accessKey, myProductSubscription, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, 
            //        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            //    if (historyResponse.Result != null && historyResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //        historySubscriptionID = historyResponse.Result;

            //    if (myProductSubscriptionPaid != null && myProductSubscriptionPaid.SubscriptionPaidID > 0)
            //        historyPaidID = hpWorker.Proxy.Save(accessKey, myProductSubscriptionPaid, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            //    if (myPaidBillTo != null && myPaidBillTo.SubscriptionPaidID > 0)
            //        historyPaidBillToID = hpbtWorker.Proxy.Save(accessKey, myPaidBillTo, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            //    History
            //    FrameworkServices.ServiceClient<UAD_WS.Interface.IHistory> bwhWorker = FrameworkServices.ServiceClient.UAD_HistoryClient();
            //    FrameworkUAS.Service.Response<FrameworkUAD.Entity.History> respHistory = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.History>();
            //    respHistory = bwhWorker.Proxy.AddHistoryEntry(accessKey,
            //                                                                        AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
            //                                                                        batch.BatchID,
            //                                                                        batch.BatchCount,
            //                                                                        MyProduct.PubID,
            //                                                                        myProductSubscription.PubSubscriptionID,
            //                                                                        myProductSubscription.SubscriptionID,
            //                                                                        historySubscriptionID,
            //                                                                        historyPaidID,
            //                                                                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //                                                                        historyPaidBillToID);

            //    if (respHistory.Result != null && respHistory.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //    {
            //        FrameworkUAD.Entity.History history = respHistory.Result;
            //        if (history != null)
            //        {
            //            HistoryMarketing
            //            List<int> marketingUserLogIDs = new List<int>();
            //            try
            //            {
            //                if(MyChildren.Responses.MadeResponseChange)
            //                    marketingUserLogIDs = SaveMarketing(history.HistoryID);
            //            }
            //            catch (Exception ex)
            //            {
            //                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
            //                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            //                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            //                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".SaveSubscriberAndLogs", app, string.Empty, logClientId);
            //            }
            //            HistoryResponse
            //            List<int> responseUserLogIDs = new List<int>();
            //            try
            //            {
            //                if(MyChildren.Responses.MadeResponseChange == true)
            //                    responseUserLogIDs = SaveResponses(history.HistoryID);
            //            }
            //            catch (Exception ex)
            //            {
            //                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
            //                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            //                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            //                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".SaveSubscriberAndLogs", app, string.Empty, logClientId);
            //            }

            //            UserLog HistoryID - HistoryToUserLog
            //            if (userLogID > 0)
            //                bwhWorker.Proxy.Insert_History_To_UserLog(accessKey, history.HistoryID, userLogID, AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            //            if (historySubscriptionID > 0)
            //                bwhWorker.Proxy.Insert_History_To_UserLog(accessKey, history.HistoryID, historySubscriptionID, AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            //            if (userLogIdPaid > 0)
            //                bwhWorker.Proxy.Insert_History_To_UserLog(accessKey, history.HistoryID, userLogIdPaid, AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            //            if (userLogIdPaidBillTo > 0)
            //                bwhWorker.Proxy.Insert_History_To_UserLog(accessKey, history.HistoryID, userLogIdPaidBillTo, AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            //            foreach (int i in marketingUserLogIDs)
            //                bwhWorker.Proxy.Insert_History_To_UserLog(accessKey, history.HistoryID, i, AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            //            foreach (int i in responseUserLogIDs)
            //                bwhWorker.Proxy.Insert_History_To_UserLog(accessKey, history.HistoryID, i, AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

            //            batch.HistoryList.Add(history);
            //        }
            //    }
            //}
            //if (ReloadSubscriber != null)
            //    ReloadSubscriber(myProductSubscription.PubSubscriptionID);
        }
        private int SaveNew()
        {
            int result = 0;
            try
            {
                KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
                if (myProductSubscription.IsNewSubscription == false && originalSubscription.IsInActiveWaveMailing == true)
                    CompareSubscriber();

                if (MyChildren.Adhocs != null && MyChildren.Adhocs.AdHocFields != null)
                {
                    List<FrameworkUAD.Object.PubSubscriptionAdHoc> adhocs = new List<FrameworkUAD.Object.PubSubscriptionAdHoc>();
                    MyChildren.Adhocs.AdHocFields.ToList().ForEach(x => adhocs.Add(x.GetModel()));
                    myProductSubscription.AdHocFields = new List<FrameworkUAD.Object.PubSubscriptionAdHoc>(adhocs);
                }

                bool isNew = myProductSubscription.IsNewSubscription;
                if (myProductSubscription.CountryID == 0)
                    myProductSubscription.CountryID = 1;
                if (isNew == true)
                    ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
                else
                {
                    myProductSubscription.DateUpdated = DateTime.Now;
                    myProductSubscription.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                }

                FrameworkUAS.Object.Batch b = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.PublicationID == MyProduct.PubID
                                              && x.UserID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID && x.IsActive == true).FirstOrDefault();

                foreach (var r in MyChildren.Responses.ProductResponseList)
                {
                    if (r.ResponseOther != null)
                    {
                        if (r.ResponseOther.Contains("&") || r.ResponseOther.Contains("<"))
                        {
                            Core_AMS.Utilities.WPF.MessageError("Special Character(s) not allowed: & <");
                            return 0;
                        }
                    }
                }
                foreach (var a in myProductSubscription.AdHocFields)
                {
                    if (a.Value != null)
                    {
                        if (a.Value.Contains("<") || a.Value.Contains("&") || a.Value.Contains("'"))
                        {
                            Core_AMS.Utilities.WPF.MessageError("Special Character(s) not allowed: & < '");
                            return 0;
                        }
                    }
                }
                bool madeResponseChange, madePaidChange, madePaidBillToChange;
                if (MyChildren.Responses == null || MyChildren.Responses.MadeResponseChange == null)
                {
                    madeResponseChange = false;
                }
                else
                {
                    madeResponseChange = MyChildren.Responses.MadeResponseChange;
                }

                if (MyChildren.Paid == null || MyChildren.Paid.MadePaidChange == null)
                {
                    madePaidChange = false;
                }
                else
                {
                    madePaidChange = MyChildren.Paid.MadePaidChange;
                }

                if (MyChildren.BillTo == null || MyChildren.BillTo.MadePaidBillToChange == null)
                {
                    madePaidBillToChange = false;
                }
                else
                {
                    madePaidBillToChange = MyChildren.BillTo.MadePaidBillToChange;
                }

                myWMDetail.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                myWMDetail.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                FrameworkUAS.Service.Response<int> saveResp = productSubWorker.Proxy.FullSave(accessKey, myProductSubscription, this.originalSubscription, saveWaveMailing, applicationID, ult,
                    b, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, madeResponseChange, madePaidChange,
                    madePaidBillToChange, MyChildren.Responses.ProductResponseList, waveMailSubscriber, myWMDetail, myProductSubscriptionPaid,
                    myPaidBillTo, MyChildren.Responses.ProductResponseList);

                result = saveResp.Result;

                if (result > 0)
                {
                    Common.RefreshBatches();
                    if (ReloadSubscriber != null)
                        ReloadSubscriber(myProductSubscription.PubSubscriptionID);
                }
                else
                    Core_AMS.Utilities.WPF.MessageError("There was a problem saving your subscriber. Please try again.");
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".SaveNew", app, string.Empty, logClientId);
            }

            return result;
        }
        private bool ValidateSubscriber()
        {
            sStatus = new FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix();
            if (myProductSubscription.IsNewSubscription == true)
            {
                if (myProductSubscription.IsPaid == true)
                {
                    FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 13 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                    if (tc != null)
                        myProductSubscription.PubTransactionID = tc.TransactionCodeID;
                }
                else
                {
                    FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 10 && x.TransactionCodeTypeID == 1).FirstOrDefault();
                    if (tc != null)
                        myProductSubscription.PubTransactionID = tc.TransactionCodeID;
                }

                if (myProductSubscription.QualificationDate == null)
                {
                    Core_AMS.Utilities.WPF.Message("Please select a qualification date.");
                    return false;
                }
            }
            if (myProductSubscription.PubCategoryID > 0 && myProductSubscription.PubTransactionID > 0)
            {
                int codeTypeId = codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Action.ToString())).CodeTypeId;
                action = actionList.SingleOrDefault(a => a.CategoryCodeID == myProductSubscription.PubCategoryID && a.TransactionCodeID == myProductSubscription.PubTransactionID &&
                        a.ActionTypeID == codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId &&
                        x.CodeName.Equals(FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace("_", " "))).CodeId);
                if (action != null)
                {
                    sStatus = ssmList.Where(s => s.CategoryCodeID == action.CategoryCodeID && s.TransactionCodeID == action.TransactionCodeID && s.IsActive == true &&
                        s.SubscriptionStatusID == myProductSubscription.SubscriptionStatusID).FirstOrDefault();
                    if (sStatus == null)
                    {
                        Core_AMS.Utilities.WPF.Message("This combination of Transaction/Category Codes and Subscription Status are not valid. Subscriber could not be saved.");
                        return false;
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("This combination of Transaction and Category Codes are not valid. Subscriber could not be saved.");
                    return false;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("This combination of Transaction and Category Codes are not valid. Subscriber could not be saved.");
                return false;
            }

            return true;
        }
        private int SavePaid(int bwApplicationId, KMPlatform.Entity.Client myClient)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionPaid> bwspWorker = FrameworkServices.ServiceClient.UAD_SubscriptionPaidClient();
            KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            FrameworkUAD.Entity.SubscriptionPaid spOriginal = bwspWorker.Proxy.Select(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections).Result;
            FrameworkUAS.Service.Response<int> spIntID = new FrameworkUAS.Service.Response<int>();
            FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog> ulResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog>();
            FrameworkServices.ServiceClient<UAS_WS.Interface.IUserLog> bwulWorker = FrameworkServices.ServiceClient.UAS_UserLogClient();
            Core_AMS.Utilities.JsonFunctions bwjf = new Core_AMS.Utilities.JsonFunctions();

            int userLogID = -1;
            myProductSubscriptionPaid.PubSubscriptionID = myProductSubscription.PubSubscriptionID;
            if (myProductSubscriptionPaid.SubscriptionPaidID > 0)
            {
                myProductSubscriptionPaid.DateUpdated = DateTime.Now;
                myProductSubscriptionPaid.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            }
            else
            {
                myProductSubscriptionPaid.DateCreated = DateTime.Now;
                myProductSubscriptionPaid.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
            }
            if (MyChildren.Paid.MadePaidChange == true)
            {
                spIntID = bwspWorker.Proxy.Save(accessKey, myProductSubscriptionPaid, myClient.ClientConnections);
                if (spIntID.Result != null && spIntID.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    myProductSubscriptionPaid.SubscriptionPaidID = spIntID.Result;
                }
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                ulResponse = bwulWorker.Proxy.CreateLog(accessKey,
                                                        bwApplicationId,
                                                        ult,
                                                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                                                        "SubscriptionPaid",
                                                        bwjf.ToJson<FrameworkUAD.Entity.SubscriptionPaid>(spOriginal),
                                                        bwjf.ToJson<FrameworkUAD.Entity.SubscriptionPaid>(myProductSubscriptionPaid));
                if (ulResponse.Result != null && ulResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    userLogID = ulResponse.Result.UserLogID;
            }

            return userLogID;
        }
        private int SavePaidBillTo(int bwApplicationId, KMPlatform.Entity.Client myClient)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IPaidBillTo> pbtWorker = FrameworkServices.ServiceClient.UAD_PaidBillToClient();
            FrameworkUAD.Entity.PaidBillTo spOriginal = pbtWorker.Proxy.SelectSubscription(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections).Result;
            KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            FrameworkUAS.Service.Response<int> pbtIDResponse = new FrameworkUAS.Service.Response<int>();
            FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog> ulResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.UserLog>();
            FrameworkServices.ServiceClient<UAS_WS.Interface.IUserLog> bwulWorker = FrameworkServices.ServiceClient.UAS_UserLogClient();

            myPaidBillTo.PubSubscriptionID = myProductSubscription.PubSubscriptionID;
            if (myPaidBillTo.PaidBillToID > 0)
            {
                myPaidBillTo.DateUpdated = DateTime.Now;
                myPaidBillTo.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
            }
            else
            {
                myPaidBillTo.DateCreated = DateTime.Now;
                myPaidBillTo.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
            }
            if (myProductSubscriptionPaid != null)
                myPaidBillTo.SubscriptionPaidID = myProductSubscriptionPaid.SubscriptionPaidID;

            int userLogID = -1;

            if (MyChildren.BillTo.MadePaidBillToChange == true)
            {
                pbtIDResponse = pbtWorker.Proxy.Save(accessKey, myPaidBillTo, myClient.ClientConnections);
                if (pbtIDResponse.Result != null && pbtIDResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    myPaidBillTo.PaidBillToID = pbtIDResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                jf = new Core_AMS.Utilities.JsonFunctions();
                ulResponse = bwulWorker.Proxy.CreateLog(accessKey,
                                                        bwApplicationId,
                                                        ult,
                                                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                                                        "PaidBillTo",
                                                        jf.ToJson<FrameworkUAD.Entity.PaidBillTo>(spOriginal),
                                                        jf.ToJson<FrameworkUAD.Entity.PaidBillTo>(myPaidBillTo));
                if (ulResponse.Result != null && ulResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    userLogID = ulResponse.Result.UserLogID;
            }

            return userLogID;
        }
        private List<int> SaveMarketing(int historyID)
        {
            List<int> userLogIDs = new List<int>();
            //#region Save the transaction
            //List<KMPlatform.Entity.UserLog> userLogList = new List<KMPlatform.Entity.UserLog>();
            //List<FrameworkUAD.Entity.HistoryMarketingMap> hmmList = new List<FrameworkUAD.Entity.HistoryMarketingMap>();
            //List<KMPlatform.Entity.UserLog> ulList = new List<KMPlatform.Entity.UserLog>();
            //int userLogTypeId = codeList.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.UserLogTypes.Edit.ToString())).CodeId;

            //MyChildren.Responses.MarketingMapList.ForEach(x =>
            //{
            //    x.PubSubscriptionID = myProductSubscription.PubSubscriptionID;                
            //});

            //var addedList = from b in MyChildren.Responses.MarketingMapList join a in MyChildren.Responses.OriginalMarketingMapList on b.MarketingID equals a.MarketingID
            //                where b.IsActive != a.IsActive && b.IsActive == true 
            //                select b;
            //var removedList = from b in MyChildren.Responses.MarketingMapList
            //                    join a in MyChildren.Responses.OriginalMarketingMapList on b.MarketingID equals a.MarketingID
            //                    where b.IsActive != a.IsActive && b.IsActive == false
            //                    select b;

            //addedList = MyChildren.Responses.MarketingMapList.Except(MyChildren.Responses.OriginalMarketingMapList).ToList();
            //removedList = MyChildren.Responses.OriginalMarketingMapList.Except(MyChildren.Responses.OriginalMarketingMapList).ToList();

            //#region Removed Responses
            //foreach (FrameworkUAD.Entity.MarketingMap mm in removedList)
            //{
            //    ulList.Add(new KMPlatform.Entity.UserLog()
            //    {
            //        ApplicationID = applicationID,
            //        UserLogTypeID = userLogTypeId,
            //        UserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //        Object = "MarketingMap",
            //        FromObjectValues = jf.ToJson<FrameworkUAD.Entity.MarketingMap>(mm),
            //        ToObjectValues = "",
            //        DateCreated = DateTime.Now
            //    });

            //    hmmList.Add(new FrameworkUAD.Entity.HistoryMarketingMap()
            //    {
            //        MarketingID = mm.MarketingID,
            //        PubSubscriptionID = myProductSubscription.PubSubscriptionID,
            //        PublicationID = mm.PublicationID,
            //        IsActive = mm.IsActive,
            //        DateCreated = DateTime.Now,
            //        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID
            //    });
            //}
            //#endregion
            //#region New Responses
            //foreach (FrameworkUAD.Entity.MarketingMap mm in addedList)
            //{
            //    ulList.Add(new KMPlatform.Entity.UserLog()
            //    {
            //        ApplicationID = applicationID,
            //        UserLogTypeID = userLogTypeId,
            //        UserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //        Object = "MarketingMap",
            //        FromObjectValues = "",
            //        ToObjectValues = jf.ToJson<FrameworkUAD.Entity.MarketingMap>(mm),
            //        DateCreated = DateTime.Now
            //    });

            //    hmmList.Add(new FrameworkUAD.Entity.HistoryMarketingMap()
            //    {
            //        MarketingID = mm.MarketingID,
            //        PubSubscriptionID = myProductSubscription.PubSubscriptionID,
            //        PublicationID = mm.PublicationID,
            //        IsActive = mm.IsActive,
            //        DateCreated = DateTime.Now,
            //        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID
            //    });
            //}
            //#endregion

            //mmWorker.Proxy.SaveBulkUpdate(AppData.myAppData.AuthorizedUser.AuthAccessKey, MyChildren.Responses.MarketingMapList, myClient.ClientConnections);

            //foreach (var x in ulList)
            //{
            //    if (x.FromObjectValues == null)
            //        x.FromObjectValues = "";
            //    if (x.ToObjectValues == null)
            //        x.ToObjectValues = "";
            //}

            //FrameworkUAS.Service.Response<List<KMPlatform.Entity.UserLog>> bwulIDResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.UserLog>>();
            //bwulIDResponse = userLogW.Proxy.SaveBulkInsert(AppData.myAppData.AuthorizedUser.AuthAccessKey, ulList, myClient);
            //if (Common.CheckResponse(bwulIDResponse.Result, bwulIDResponse.Status) == true)
            //    userLogList = bwulIDResponse.Result;

            //foreach (var x in userLogList)
            //{
            //    userLogIDs.Add(x.UserLogID);
            //}

            //FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.HistoryMarketingMap>> hmmResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.HistoryMarketingMap>>();
            //List<FrameworkUAD.Entity.HistoryMarketingMap> hmmID = new List<FrameworkUAD.Entity.HistoryMarketingMap>();
            //hmmResponse = hmmWorker.Proxy.SaveBulkUpdate(AppData.myAppData.AuthorizedUser.AuthAccessKey, hmmList, myClient.ClientConnections);
            //if (Common.CheckResponse(hmmResponse.Result, hmmResponse.Status) == true)
            //    hmmID = hmmResponse.Result;

            //FrameworkServices.ServiceClient<UAD_WS.Interface.IHistory> bwhWorker = FrameworkServices.ServiceClient.UAD_HistoryClient();
            //bwhWorker.Proxy.Insert_History_To_HistoryMarketingMap_List(AppData.myAppData.AuthorizedUser.AuthAccessKey, historyID, hmmID, myClient.ClientConnections);
            //#endregion
            return userLogIDs;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Core_AMS.Utilities.Enums.DialogResponses? response = DoSuggestMatch();
            if (response == Core_AMS.Utilities.Enums.DialogResponses.Cancel || response == Core_AMS.Utilities.Enums.DialogResponses.Copy)
                return;
            else if (response == Core_AMS.Utilities.Enums.DialogResponses.Save || response == null)
            {
                if (ValidateSubscriber() == false)
                    return;
                ReLoadProduct();
                if (MyProduct.AllowDataEntry == true)
                {
                    if (this.InfoChanged == true)
                    {
                        string required = RequiredFields();
                        if (required.Length == 0)
                        {
                            int result = 0;
                            this.Saved = true;
                            busy.IsBusy = true;
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.DoWork += (o, ea) =>
                            {
                                //Save();
                                result = SaveNew();
                            };
                            bw.RunWorkerCompleted += (o, ea) =>
                            {
                                if (result > 0)
                                {
                                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                                    ReturnToSearch();
                                }
                                else
                                    busy.IsBusy = false;
                            };
                            bw.RunWorkerAsync();
                        }
                        else
                        {
                            required = required.TrimEnd(' ');
                            required = required.TrimEnd(',');
                            MessageBox.Show("Please update or provide answers/selections for the following fields: " + required, "Required Fields", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.MessageSaveComplete();
                        ReturnToSearch();
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("This publication is currently locked to process lists. Data can not be saved.", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, "Data Entry Locked");
                    return;
                }
            }
        }
        private void btnSaveNew_Click(object sender, RoutedEventArgs e)
        {
            DoSuggestMatch();
            if (ValidateSubscriber() == false)
                return;
            ReLoadProduct();
            if (MyProduct.AllowDataEntry == true)
            {
                if (this.InfoChanged == true)
                {
                    string required = RequiredFields();
                    if (required.Length == 0)
                    {
                        busy.IsBusy = true;
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += (o, ea) =>
                        {
                            Save();
                        };
                        bw.RunWorkerCompleted += (o, ea) =>
                        {
                            Core_AMS.Utilities.WPF.MessageSaveComplete();
                            ReturnToSearch();
                        };
                        bw.RunWorkerAsync();
                    }
                    else
                    {
                        required = required.TrimEnd(' ');
                        required = required.TrimEnd(',');
                        MessageBox.Show("Please update or provide answers/selections for the following fields: " + required, "Required Fields", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                    ReturnToSearch();
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("This publication is currently locked to process lists. Data can not be saved.", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, "Data Entry Locked");
                return;
            }
        }
        #endregion
        #region UI Events
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window home = Core_AMS.Utilities.WPF.GetParentWindow(this);
            if (home != null)
                home.Close();
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Any unsaved data will be lost by resetting the page. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                this.CopyProfileInfo = false;
                tabSub.IsSelected = true;
                FrameworkUAD.Entity.ProductSubscription origCopy = new FrameworkUAD.Entity.ProductSubscription(originalSubscription);
                BindProfile(origCopy);
            }
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
            else if (typeof(RadWatermarkTextBox) == sender.GetType())
            {
                RadWatermarkTextBox tb = (RadWatermarkTextBox)sender;
                if (e.Key == Key.Tab)
                {
                    tb.SelectAll();
                }
            }
        }
        private void tbProfile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (typeof(TextBox) == sender.GetType())
            {
                TextBox tb = (TextBox)sender;
                tb.SelectAll();
            }
        }
        #endregion
        #region Helpers
        public static System.Drawing.Rectangle GetActiveScreen(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            return screen.WorkingArea;
        }
        private void SetUpPaidFunctions()
        {
            if (myClient.HasPaid == true || myProductSubscription.IsNewSubscription == true)
            {
                FrameworkUAD.Entity.PaidBillTo pbt = new FrameworkUAD.Entity.PaidBillTo();

                if (myProductSubscription.IsNewSubscription == false)
                {
                    spResponse = spWorker.Proxy.Select(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
                    if (spResponse.Result != null && spResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        myProductSubscriptionPaid = spResponse.Result;
                        //originalPaid = spResponse.Result;

                        pbtResponse = pbtWorker.Proxy.Select(accessKey, myProductSubscriptionPaid.SubscriptionPaidID, myClient.ClientConnections);
                        if (pbtResponse.Result != null && pbtResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        {
                            pbt = pbtResponse.Result;
                            //originalBillTo = pbtResponse.Result;
                        }
                    }
                }

                if (myProductSubscriptionPaid.SubscriptionPaidID == 0 || myProductSubscription.IsNewSubscription == true)
                {
                    myProductSubscriptionPaid = new FrameworkUAD.Entity.SubscriptionPaid();
                    myProductSubscriptionPaid.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    myProductSubscriptionPaid.DateCreated = DateTime.Now;
                    myProductSubscriptionPaid.PubSubscriptionID = myProductSubscription.PubSubscriptionID;
                    myProductSubscriptionPaid.PaidDate = DateTime.Now;
                    myProductSubscriptionPaid.StartIssueDate = DateTime.Now;
                    myProductSubscriptionPaid.ExpireIssueDate = DateTime.Now;
                    myProductSubscriptionPaid.CCExpirationMonth = string.Empty;
                    myProductSubscriptionPaid.CCExpirationYear = string.Empty;
                    myProductSubscriptionPaid.CCHolderName = string.Empty;
                    myProductSubscriptionPaid.CCNumber = string.Empty;
                    myProductSubscriptionPaid.CheckNumber = string.Empty;
                }

                PaidNew paidNew = new PaidNew(myProductSubscription, myProductSubscriptionPaid);
                paidNew.Name = "modPaid";
                tabPaid.Content = paidNew;

                MyChildren.Paid = paidNew;
                MyChildren.Paid.CopiesChanged += value => this.Copies = value;
                MyChildren.Paid.InfoChanged += value =>
                {
                    if (value == true && _isPaid)
                    {
                        this.InfoChanged = true;
                        this.RenewalPayment = true;
                    }
                    else if (_isPaid && !value)
                    {
                        this.RenewalPayment = false;
                        if (CheckChanges() == false)
                            this.InfoChanged = false;
                    }
                };

                if (pbt.PaidBillToID == 0)
                    pbt = new FrameworkUAD.Entity.PaidBillTo();

                myPaidBillTo = pbt;

                PaidBillTo bill = new PaidBillTo(pbt);
                bill.Name = "modBill";
                tabBillTo.Content = bill;

                MyChildren.BillTo = bill;
                MyChildren.BillTo.CopyProfileInfo += value => this.CopyProfileInfo = value;
                MyChildren.BillTo.InfoChanged += value =>
                {
                    if (value == true)
                        this.InfoChanged = true;
                    else if (CheckChanges() == false)
                        this.InfoChanged = false;
                };
            }
        }
        private void LoadCountryChanges(int countryID)
        {
            #region States

            if (countryID.Equals(1) || countryID.Equals(2) || countryID.Equals(3))
            {
                if (countryID.Equals(1))
                {
                    cbState.ItemsSource = regions.Where(x => x.CountryID == 1).ToList();
                    cbState.SelectedValuePath = "RegionID";
                    cbState.DisplayMemberPath = "RegionName";
                }
                if (countryID.Equals(2))
                {
                    //cbState.SelectedValue = -1;
                    cbState.ItemsSource = regions.Where(x => x.CountryID == 2).ToList();
                    cbState.SelectedValuePath = "RegionID";
                    cbState.DisplayMemberPath = "RegionName";
                }
                if (countryID.Equals(3))
                {
                    cbState.ItemsSource = regions.Where(x => x.CountryID == 1 || x.CountryID == 2).OrderBy(x => x.CountryID).ThenBy(x => x.RegionName).ToList();
                    cbState.SelectedValuePath = "RegionID";
                    cbState.DisplayMemberPath = "RegionName";
                }

            }
            else if (countryID.Equals(429))
            {
                cbState.ItemsSource = regions.Where(x => x.CountryID == 0 || x.CountryID == 429).OrderBy(x => x.CountryID).ThenBy(x => x.RegionName).ToList();
                cbState.SelectedValuePath = "RegionID";
                cbState.DisplayMemberPath = "RegionName";
                FrameworkUAD_Lookup.Entity.Region r = regions.Where(x => x.RegionCode == "MX").FirstOrDefault();
                if (r != null && this.IsNewSubscription == true)
                    cbState.SelectedItem = r;
            }
            else
            {
                cbState.ItemsSource = regions.Where(x => x.CountryID == 0).ToList();
                cbState.SelectedValuePath = "RegionID";
                cbState.DisplayMemberPath = "RegionName";
                FrameworkUAD_Lookup.Entity.Region r = regions.Where(x => x.RegionCode == "FO").FirstOrDefault();
                if (r != null && this.IsNewSubscription == true)
                    cbState.SelectedItem = r;
            }
            #endregion
            #region Phone Prefix
            selCountryPhonePrefix = countryList.SingleOrDefault(x => x.CountryID == countryID);
            if (countryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.UNITED_STATES.ToString().Replace("_", " ")).CountryID
                || countryID == countryList.SingleOrDefault(x => x.ShortName == FrameworkUAS.BusinessLogic.Enums.CountriesWithRegions.CANADA.ToString().Replace("_", " ")).CountryID)
            {
                tbPhoneCode.Text = "1";
            }
            else if (selCountryPhonePrefix != null)
            {
                selCountryPhonePrefix = countryList.SingleOrDefault(x => x.CountryID == countryID);
                tbPhoneCode.Text = selCountryPhonePrefix.PhonePrefix.ToString();
            }
            else
            {
                tbPhoneCode.Text = "";
            }
            #endregion
            #region ZipCode
            //if (!string.IsNullOrEmpty(tbZipPlus4.Text) && (countryID.Equals(1) || countryID.Equals(2)))
            //{
            //    Regex usZipRegEx = new Regex("(^[0-9]{5}$)|(^[0-9]{5}-[0-9]{4}$)");
            //    Regex caZipRegEx = new Regex("(^[A-Z]{1}[0-9]{1}[A-Z]{1}\\s[0-9]{1}[A-Z]{1}[0-9]$)");

            //    bool validZipCode;
            //    if ((usZipRegEx.IsMatch(tbZipPlus4.Text)) || (caZipRegEx.IsMatch(tbZipPlus4.Text)))
            //        validZipCode = true;
            //    else
            //        validZipCode = false;


            //    if (!validZipCode)
            //    {
            //        if (countryID.Equals(1))
            //        {
            //            //USA                        
            //            if (!tbZipPlus4.Text.Contains("-") & tbZipPlus4.Text.Length == 9)
            //                tbZipPlus4.Text = tbZipPlus4.Text.Substring(0, 5) + "-" + tbZipPlus4.Text.Substring(5);

            //        }
            //        else if (countryID.Equals(2))
            //        {
            //            //CANADA                        
            //            if (!tbZipPlus4.Text.Contains(" ") & tbZipPlus4.Text.Length == 6)
            //                tbZipPlus4.Text = tbZipPlus4.Text.Substring(0, 3) + " " + tbZipPlus4.Text.Substring(3);

            //        }
            //    }

            //    if ((usZipRegEx.IsMatch(tbZipPlus4.Text)) || (caZipRegEx.IsMatch(tbZipPlus4.Text)))
            //        validZipCode = true;
            //    else
            //        Core_AMS.Utilities.WPF.Message("Postal Code is invalid.", MessageBoxButton.OK,
            //            MessageBoxImage.Information, "Invalid Postal Code");
            //}
            #endregion
            #region Load Country Name
            string countryName = countryList.Where(x => x.CountryID == countryID).Select(x => x.ShortName).FirstOrDefault();
            if (countryName != "" && countryName != null)
                myProductSubscription.Country = countryName;
            #endregion
        }
        private void ReturnToSearch()
        {
            Window home = Core_AMS.Utilities.WPF.GetParentWindow(this);
            home.Close();
        }
        private string RequiredFields()
        {
            string required = "";
            if (myProductSubscription.IsNewSubscription == false)
            {
                if (this.TriggerQualDate == true)
                {
                    required += "Qualification Date, ";
                }
                if (myProductSubscription.IsSubscribed == true && (myProductSubscription.PubCategoryID == null || myProductSubscription.PubCategoryID == 0))
                {
                    required += "Category Type, ";
                }
                if (string.IsNullOrEmpty(myProductSubscription.OnBehalfOf) && MyChildren.Subscription.btnOnBehalfKill.IsChecked == true)
                {
                    required += "On Behalf Request, ";
                }
            }
            else if (myProductSubscription.IsNewSubscription == true)
            {
                if (this.TriggerQualDate == true)
                {
                    required += "Qualification Date, ";
                }
                if (myProductSubscription.IsSubscribed == true && (myProductSubscription.PubCategoryID == null || myProductSubscription.PubCategoryID == 0))
                {
                    required += "Category Type, ";
                }
            }
            if (string.IsNullOrEmpty(myProductSubscription.Demo7))
            {
                required += "Media Type, ";
            }

            if (MyChildren.AreQuestionsRequired == true)
            {
                if (MyChildren.Responses != null)
                {
                    string unAnswered = MyChildren.Responses.RequiredAnswered();

                    if (!string.IsNullOrEmpty(unAnswered) && myProductSubscription.IsSubscribed == true)
                    {
                        required += unAnswered + ", ";
                    }
                }
            }
            return required;
        }
        private void ReLoadProduct()
        {
            if (MyProduct != null)
            {
                singProdResponse = productWorker.Proxy.Select(accessKey, MyProduct.PubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (singProdResponse.Result != null && singProdResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    FrameworkUAD.Entity.Product pub = singProdResponse.Result;
                    if (pub != null)
                    {
                        MyProduct = pub;
                    }
                }
            }
        }
        private void CompareSubscriber()
        {
            waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription(myProductSubscription);
            myWMDetail.SubscriptionID = originalSubscription.SubscriptionID;
            myWMDetail.PubSubscriptionID = originalSubscription.PubSubscriptionID;
            myWMDetail.WaveMailingID = originalSubscription.WaveMailingID;
            if (originalSubscription.FirstName != myProductSubscription.FirstName)
            {
                myWMDetail.FirstName = myProductSubscription.FirstName;
                //waveMailSubscriber.FirstName = myProductSubscription.FirstName;
                myProductSubscription.FirstName = originalSubscription.FirstName;
                saveWaveMailing = true;
            }
            if (originalSubscription.LastName != myProductSubscription.LastName)
            {
                myWMDetail.LastName = myProductSubscription.LastName;
                //waveMailSubscriber.LastName = myProductSubscription.LastName;
                myProductSubscription.LastName = originalSubscription.LastName;
                saveWaveMailing = true;
            }
            if (originalSubscription.Title != myProductSubscription.Title)
            {
                myWMDetail.Title = myProductSubscription.Title;
                //waveMailSubscriber.Title = myProductSubscription.Title;
                myProductSubscription.Title = originalSubscription.Title;
                saveWaveMailing = true;
            }
            if (originalSubscription.Company != myProductSubscription.Company)
            {
                myWMDetail.Company = myProductSubscription.Company;
                //waveMailSubscriber.Company = myProductSubscription.Company;
                myProductSubscription.Company = originalSubscription.Company;
                saveWaveMailing = true;
            }
            if (originalSubscription.Address1 != myProductSubscription.Address1)
            {
                myWMDetail.Address1 = myProductSubscription.Address1;
                //waveMailSubscriber.Address1 = myProductSubscription.Address1;
                myProductSubscription.Address1 = originalSubscription.Address1;
                saveWaveMailing = true;
            }
            if (originalSubscription.Address2 != myProductSubscription.Address2)
            {
                myWMDetail.Address2 = myProductSubscription.Address2;
                //waveMailSubscriber.Address2 = myProductSubscription.Address2;
                myProductSubscription.Address2 = originalSubscription.Address2;
                saveWaveMailing = true;
            }
            if (originalSubscription.Address3 != myProductSubscription.Address3)
            {
                myWMDetail.Address3 = myProductSubscription.Address3;
                //waveMailSubscriber.Address3 = myProductSubscription.Address3;
                myProductSubscription.Address3 = originalSubscription.Address3;
                saveWaveMailing = true;
            }
            if (originalSubscription.AddressTypeCodeId != myProductSubscription.AddressTypeCodeId)
            {
                myWMDetail.AddressTypeID = myProductSubscription.AddressTypeCodeId;
                //waveMailSubscriber.AddressTypeCodeId = myProductSubscription.AddressTypeCodeId;
                myProductSubscription.AddressTypeCodeId = originalSubscription.AddressTypeCodeId;
                saveWaveMailing = true;
            }
            if (originalSubscription.City != myProductSubscription.City)
            {
                myWMDetail.City = myProductSubscription.City;
                //waveMailSubscriber.City = myProductSubscription.City;
                myProductSubscription.City = originalSubscription.City;
                saveWaveMailing = true;
            }
            if (originalSubscription.RegionCode != myProductSubscription.RegionCode)
            {
                myWMDetail.RegionCode = myProductSubscription.RegionCode;
                //waveMailSubscriber.RegionCode = myProductSubscription.RegionCode;
                myProductSubscription.RegionCode = originalSubscription.RegionCode;
                saveWaveMailing = true;
            }
            if (originalSubscription.RegionID != myProductSubscription.RegionID)
            {
                myWMDetail.RegionID = myProductSubscription.RegionID;
                //waveMailSubscriber.RegionID = myProductSubscription.RegionID;
                myProductSubscription.RegionID = originalSubscription.RegionID;
                saveWaveMailing = true;
            }
            if (originalSubscription.ZipCode != myProductSubscription.ZipCode)
            {
                myWMDetail.ZipCode = myProductSubscription.ZipCode;
                //waveMailSubscriber.ZipCode = myProductSubscription.ZipCode;
                myProductSubscription.ZipCode = originalSubscription.ZipCode;
                saveWaveMailing = true;
            }
            if (originalSubscription.Plus4 != myProductSubscription.Plus4)
            {
                myWMDetail.Plus4 = myProductSubscription.Plus4;
                //waveMailSubscriber.Plus4 = myProductSubscription.Plus4;
                myProductSubscription.Plus4 = originalSubscription.Plus4;
                saveWaveMailing = true;
            }
            if (originalSubscription.County != myProductSubscription.County)
            {
                myWMDetail.County = myProductSubscription.County;
                //waveMailSubscriber.County = myProductSubscription.County;
                myProductSubscription.County = originalSubscription.County;
                saveWaveMailing = true;
            }
            if (originalSubscription.Country != myProductSubscription.Country)
            {
                myWMDetail.Country = myProductSubscription.Country;
                //waveMailSubscriber.Country = myProductSubscription.Country;
                myProductSubscription.Country = originalSubscription.Country;
                saveWaveMailing = true;
            }
            if (originalSubscription.CountryID != myProductSubscription.CountryID)
            {
                myWMDetail.CountryID = myProductSubscription.CountryID;
                //waveMailSubscriber.CountryID = myProductSubscription.CountryID;
                myProductSubscription.CountryID = originalSubscription.CountryID;
                saveWaveMailing = true;
            }
            if (originalSubscription.Email != myProductSubscription.Email)
            {
                myWMDetail.Email = myProductSubscription.Email;
                //waveMailSubscriber.Email = myProductSubscription.Email;
                myProductSubscription.Email = originalSubscription.Email;
                saveWaveMailing = true;
            }
            if (originalSubscription.Phone != myProductSubscription.Phone)
            {
                myWMDetail.Phone = myProductSubscription.Phone;
                waveMailSubscriber.Phone = myProductSubscription.Phone;
                myProductSubscription.Phone = originalSubscription.Phone;
                saveWaveMailing = true;
            }
            if (originalSubscription.Fax != myProductSubscription.Fax)
            {
                myWMDetail.Fax = myProductSubscription.Fax;
                //waveMailSubscriber.Fax = myProductSubscription.Fax;
                myProductSubscription.Fax = originalSubscription.Fax;
                saveWaveMailing = true;
            }
            if (originalSubscription.Mobile != myProductSubscription.Mobile)
            {
                myWMDetail.Mobile = myProductSubscription.Mobile;
                //waveMailSubscriber.Mobile = myProductSubscription.Mobile;
                myProductSubscription.Mobile = originalSubscription.Mobile;
                saveWaveMailing = true;
            }
            if (originalSubscription.Demo7 != myProductSubscription.Demo7)
            {
                myWMDetail.Demo7 = myProductSubscription.Demo7;
                myProductSubscription.Demo7 = originalSubscription.Demo7;
                saveWaveMailing = true;
            }
            if (originalSubscription.PubCategoryID != myProductSubscription.PubCategoryID)
            {
                myWMDetail.PubCategoryID = myProductSubscription.PubCategoryID;
                myProductSubscription.PubCategoryID = originalSubscription.PubCategoryID;
                saveWaveMailing = true;
            }
            if (originalSubscription.PubTransactionID != myProductSubscription.PubTransactionID)
            {
                myWMDetail.PubTransactionID = myProductSubscription.PubTransactionID;
                myProductSubscription.PubTransactionID = originalSubscription.PubTransactionID;
                saveWaveMailing = true;
            }
            if (originalSubscription.IsSubscribed != myProductSubscription.IsSubscribed)
            {
                myWMDetail.IsSubscribed = myProductSubscription.IsSubscribed;
                myProductSubscription.IsSubscribed = originalSubscription.IsSubscribed;
                saveWaveMailing = true;
            }
            if (originalSubscription.SubscriptionStatusID != myProductSubscription.SubscriptionStatusID)
            {
                myWMDetail.SubscriptionStatusID = myProductSubscription.SubscriptionStatusID;
                myProductSubscription.SubscriptionStatusID = originalSubscription.SubscriptionStatusID;
                saveWaveMailing = true;
            }
            if (originalSubscription.Copies != myProductSubscription.Copies)
            {
                myWMDetail.Copies = myProductSubscription.Copies;
                myProductSubscription.Copies = originalSubscription.Copies;
                saveWaveMailing = true;
            }
            if (originalSubscription.PhoneExt != myProductSubscription.PhoneExt)
            {
                myWMDetail.PhoneExt = myProductSubscription.PhoneExt;
                myProductSubscription.PhoneExt = originalSubscription.PhoneExt;
                saveWaveMailing = true;
            }
            if (originalSubscription.IsPaid != myProductSubscription.IsPaid)
            {
                myWMDetail.IsPaid = myProductSubscription.IsPaid;
                myProductSubscription.IsPaid = originalSubscription.IsPaid;
                saveWaveMailing = true;
            }
        }
        public void CheckSubscriptionStatus()
        {
            FrameworkUAD_Lookup.Entity.SubscriptionStatus sst = new FrameworkUAD_Lookup.Entity.SubscriptionStatus();
            if (sstList != null)
            {
                sst = sstList.Where(x => x.SubscriptionStatusID == SubscriptionStatus).FirstOrDefault();
            }

            ReLoadProduct();
            if (MyProduct.AllowDataEntry == false || (myProductSubscription.IsLocked == true && myProductSubscription.LockedByUserID != FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID) ||
                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsReadOnly)
            {
                this.Enabled = false;
                this.ReactivateEnabled = false;
                if (!FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsReadOnly)
                {
                    grdMain.Opacity = 0.5;
                    if (MyProduct.AllowDataEntry == false)
                        txtPopUp.Text = "Data Entry is currently locked for this publication. Edits can not be made.";
                    else
                        txtPopUp.Text = "Another user is currently editing this Subscriber. Edits can not be made.";
                    brdPopUp.Visibility = System.Windows.Visibility.Visible;
                    btnOk.Focus();
                }
            }
            else if (sst != null && (sst.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAFree.ToString() ||
                sst.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAPaid.ToString() ||
                sst.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAProsp.ToString()))
            {
                this.Enabled = false;
                this.ReactivateEnabled = true;
            }
            else
            {
                this.Enabled = true;
                this.ReactivateEnabled = true;
            }

            this.RenewalPayment = false;
            qualDateTriggered = false;
            this.TriggerQualDate = false;

            firstLoad = false;
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CheckSubscriptionStatus();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            brdPopUp.Visibility = Visibility.Collapsed;
            grdMain.Opacity = 1;
        }
    }
}
