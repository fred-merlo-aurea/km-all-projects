using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Circulation.Helpers;
using Telerik.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for ResponseNew.xaml
    /// </summary>
    public partial class ResponseNew : UserControl, INotifyPropertyChanged
    {
        #region Entity / List
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private List<FrameworkUAD.Entity.ResponseGroup> questions = new List<FrameworkUAD.Entity.ResponseGroup>();
        private List<FrameworkUAD.Entity.CodeSheet> answers = new List<FrameworkUAD.Entity.CodeSheet>();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> TempDetails = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private List<FrameworkUAD_Lookup.Entity.Code> mlist = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> parList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> qSourceList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        
        private int catCodeTypeId = 0;
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private ObservableCollection<Question> questionCollection = new ObservableCollection<Question>();
        #endregion
        #region Worker
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> p3Worker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> qsWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> aWorker = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> mWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.IMarketingMap> mmWorker = FrameworkServices.ServiceClient.UAD_MarketingMapClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> pubSubW = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> codeSheetW = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseGroupW = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        #endregion
        #region Service Response
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qualSouResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> par3cResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> marketingResponse;
        //private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MarketingMap>> mmResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> psdResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rtResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> rResponse;
        #endregion
        #region Properties
        public event Action<int> CopiesChanged;
        //private List<FrameworkUAD.Entity.MarketingMap> _marketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();
        //private List<FrameworkUAD.Entity.MarketingMap> _origMarketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> _productResponseList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> _origProductResponseList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private int _copies;
        private int _qSourceID;
        private int _par3C;
        private DateTime _qDate;
        private bool qualDateChanged;
        private bool _enabled;
        private bool _madeResponseChange;
        private bool _triggerQualDateChange;
        private bool _isCopiesEnabled;
        private bool? _mailPermission;
        private bool? _faxPermission;
        private bool? _phonePermission;
        private bool? _otherProductPermission;
        private bool? _emailRenewPermission;
        private bool? _thirdPartyPermission;
        private bool? _textPermission;
        public List<FrameworkUAD.Entity.ProductSubscriptionDetail> ProductResponseList
        {
            get
            {
                foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd in _productResponseList)
                {
                    int grpID = answers.Where(x => x.CodeSheetID == psd.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
                    if (grpID > 0)
                        psd.ResponseOther = questionCollection.Where(x => x.GroupID == grpID).Select(x => x.OtherValue).FirstOrDefault();
                }
                return _productResponseList;
            }
            set
            {
                _productResponseList = value;
            }
        }
        public List<FrameworkUAD.Entity.ProductSubscriptionDetail> OriginalProductResponseList
        {
            get { return _origProductResponseList; }
        }
        //public List<FrameworkUAD.Entity.MarketingMap> MarketingMapList
        //{
        //    get
        //    {
        //        if (_marketingMapList != null) return _marketingMapList;
        //        else return null;
        //    }
        //}
        //public List<FrameworkUAD.Entity.MarketingMap> OriginalMarketingMapList
        //{
        //    get
        //    {
        //        return _origMarketingMapList;
        //    }
        //}
        public int? QSourceID
        {
            get { return _qSourceID; }
            set
            {
                if (_qSourceID != (value ?? 0))
                {
                    //this.TriggerQualDateChange = true;
                    _qSourceID = (value ?? 0);
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("QSourceID"));
                    }
                }
            }
        }
        public int? Par3C
        {
            get { return _par3C; }
            set
            {
                if (_par3C != (value ?? 0))
                {
                    //this.TriggerQualDateChange = true;
                    _par3C = (value ?? 0);
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Par3C"));
                    }
                }
            }
        }
        public DateTime QDate
        {
            get { return _qDate; }
            set
            {
                if (_qDate != value)
                {
                    _qDate = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("QDate"));
                    }
                }
            }
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
        public bool MadeResponseChange
        {
            get { return _madeResponseChange; }
            set
            {
                if (_madeResponseChange != value)
                {
                    _madeResponseChange = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("MadeResponseChange"));
                    }
                }
            }
        }
        public int Copies
        {
            set
            {
                if (value > 0 && _copies != value)
                {
                    _copies = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Copies"));
                    }
                }
            }
            get { return _copies; }
        }
        public bool TriggerQualDateChange
        {           
            get { return _triggerQualDateChange; }
            set
            {
                if (_triggerQualDateChange != value)
                {
                    if (qualDateChanged == true)
                        _triggerQualDateChange = false;
                    else
                        _triggerQualDateChange = value;
                }

                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TriggerQualDateChange"));
                }
            }
        }
        public bool? MailPermission
        {
            get { return _mailPermission; }
            set
            {
                _mailPermission = value;
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
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TextPermission"));
                }
            }
        }
        public bool IsCopiesEnabled
        {
            get { return _isCopiesEnabled; }
            set
            {
                _isCopiesEnabled = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsCopiesEnabled"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<bool> QualDateChanged;
        #endregion
        #region Classes
        public class Question : INotifyPropertyChanged
        {
            private string _OtherValue;
            private bool _OtherChanged;
            private string _OriginalOtherValue;
            private bool _ShowOther;
            private bool _isRequired;
            public string DisplayName { get; set; }
            public List<Answer> Answers { get; set; }
            public bool IsRequired
            {
                get { return _isRequired; }
                set
                {
                    _isRequired = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsRequired"));
                    }
                }
            }
            public bool IsMultiple { get; set; }
            public int GroupID { get; set; }
            public bool OtherChanged
            {
                get { return _OtherChanged; }
                set
                {
                    _OtherChanged = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("OtherChanged"));
                    }
                }
            }
            public string OtherValue
            {
                get { return _OtherValue; }
                set
                {   
                    _OtherValue = value;
                    if (_OtherValue != _OriginalOtherValue)
                        OtherChanged = true;
                    else
                        OtherChanged = false;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("OtherValue"));
                    }
                }
            }
            public bool ShowOther
            {
                get { return _ShowOther; }
                set
                {
                    _ShowOther = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ShowOther"));
                    }
                }
            }
            public Question(string name, int id, List<Answer> answers, bool required, bool multiple, string other)
            {
                this.DisplayName = name;
                this.GroupID = id;
                this.Answers = answers.OrderBy(x=> x.DisplayOrder).ToList();
                this.IsRequired = required;
                this.IsMultiple = multiple;
                this.OtherValue = other;
                _OriginalOtherValue = other;
                if (this.Answers.Where(x => x.IsOther == true && x.IsSelected == true).Count() > 0)
                    this.ShowOther = true;
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        private void UpdateOtherChanges(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("OtherChanged"))
            {
                if (!this.ProductResponseList.OrderBy(x => x.CodeSheetID).Select(x => x.ResponseOther).SequenceEqual(_origProductResponseList.OrderBy(x => x.CodeSheetID).Select(x => x.ResponseOther)))
                    this.MadeResponseChange = true;
                else
                {
                    this.MadeResponseChange = false;
                    this.TriggerQualDateChange = false;
                }
                //else
                //{
                //    var t = from b in _marketingMapList
                //            join a in _origMarketingMapList on b.MarketingID equals a.MarketingID
                //            where b.IsActive != a.IsActive
                //            select b;
                //    if (t.Count() == 0)
                //    {
                //        this.MadeResponseChange = false;
                //        this.TriggerQualDateChange = false;
                //    }
                //}
            }
        }

        public class Answer: INotifyPropertyChanged
        {
            private bool _IsSelected;
            public int CodeSheetID { get; set; }
            public int PubID { get; set; }
            public string ResponseDesc { get; set; }          
            public int ResponseGroupID { get; set; }           
            public int? DisplayOrder { get; set; }            
            public bool? IsActive { get; set; }          
            public bool? IsOther { get; set; }
            public bool IsSelected
            {
                get { return _IsSelected; }
                set
                {
                    _IsSelected = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                    }
                }
            }

            public Answer(int id, int pubID, string description, string value, int respGrpID, int? order, bool? active, bool? other, bool selected)
            {
                this.CodeSheetID = id;
                this.PubID = pubID;
                this.ResponseDesc = value + ". " + description;
                this.ResponseGroupID = respGrpID;
                this.DisplayOrder = order;
                this.IsActive = active;
                this.IsOther = other;
                this.IsSelected = selected;
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public class MarketingAnswer : INotifyPropertyChanged
        {
            private bool _IsSelected;
            public int MarketingID { get; set; }
            public string MarketingName { get; set; }
            public bool IsActive { get; set; }
            public bool IsSelected
            {
                get { return _IsSelected; }
                set
                {
                    _IsSelected = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                    }
                }
            }

            public MarketingAnswer(FrameworkUAD_Lookup.Entity.Code market, bool selected)
            {
                this.MarketingID = market.CodeId;
                this.MarketingName = market.CodeName;
                this.IsActive = market.IsActive;
                this.IsSelected = selected;
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        #endregion
        public ResponseNew(KMPlatform.Entity.Client publisher, FrameworkUAD.Entity.Product product,
            FrameworkUAD.Entity.ProductSubscription prdSubscription, List<FrameworkUAD.Entity.ResponseGroup> rgList, List<FrameworkUAD.Entity.CodeSheet> csList)
        {
            InitializeComponent();
            _madeResponseChange = false;
            myClient = publisher;
            myProduct = product;
            myProductSubscription = prdSubscription;
            this.MailPermission = myProductSubscription.MailPermission;
            this.FaxPermission = myProductSubscription.FaxPermission;
            this.PhonePermission = myProductSubscription.PhonePermission;
            this.OtherProductsPermission = myProductSubscription.OtherProductsPermission;
            this.EmailRenewPermission = myProductSubscription.EmailRenewPermission;
            this.ThirdPartyPermission = myProductSubscription.ThirdPartyPermission;
            this.TextPermission = myProductSubscription.TextPermission;
            questions = rgList;
            answers = csList;
            actionList = Home.Actions;
            catTypeList = Home.CategoryCodeTypes;
            Home.CategoryCodes.ForEach(x=> catCodeList.Add(Core_AMS.Utilities.ObjectFunctions.DeepCopy(x)));
            catCodeTypeId = 0;
            transCodeList = Home.TransactionCodes;
            codeList = Home.Codes;
            codeTypeList = Home.CodeTypes;
            transCodeTypeList = Home.TransactionCodeTypes;
            parList = Home.Par3CCodes;
            qSourceList = Home.QSourceCodes;
            

            this.Copies = prdSubscription.Copies;

            //mmResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MarketingMap>>();
            //if (myProductSubscription.PubSubscriptionID > 0)
            //{
            //    mmResponse = mmWorker.Proxy.SelectSubscriber(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
            //    if (Common.CheckResponse(mmResponse.Result, mmResponse.Status))
            //    {
            //        _marketingMapList = mmResponse.Result.Where(a => a.IsActive == true).ToList();
            //    }
            //}
            //if (_marketingMapList == null)
            //    _marketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();
            LoadData();
        }
        private void LoadData()
        {
            #region Populate Lists if Empty & Selected Values
            cbQSource.ItemsSource = qSourceList.OrderBy(x=> x.CodeId);
            cbQSource.DisplayMemberPath = "DisplayName";
            cbQSource.SelectedValuePath = "CodeId";

            cbPar3C.ItemsSource = parList.OrderBy(x => x.CodeId);
            cbPar3C.DisplayMemberPath = "DisplayName";
            cbPar3C.SelectedValuePath = "CodeId";

            // ResponseType/Question
            if (questions == null || questions.Count == 0)
            {
                rtResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
                rtResponse = responseGroupW.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, myProduct.PubID);
                //int responseGroupTypeID = codeTypeList.Where(x=> x.CodeTypeName == FrameworkUAS.BusinessLogic.Enums.ResponseGroupTypes.Circ_and_UAD.ToString().Replace("_", " ")).Select(x=> x.CodeTypeId).FirstOrDefault();
                int circAndUAD = codeList.Where(x=> x.CodeName == FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_and_UAD.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault();
                if (rtResponse != null && rtResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    questions = rtResponse.Result.Where(x => x.IsActive == true && x.ResponseGroupTypeId == circAndUAD).OrderBy(y => y.DisplayOrder).ThenBy(z => z.DisplayName).ToList();
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            // Response/Answer
            if (answers == null || answers.Count == 0)
            {
                rResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
                rResponse = codeSheetW.Proxy.Select(accessKey, myProduct.PubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (rResponse != null && rResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    answers = rResponse.Result.Where(x => x.IsActive == true).ToList();
                    //answers.ForEach(x => x.ResponseDesc = x.ResponseValue + "." + x.ResponseDesc);
                }
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }


            if (myProductSubscription.ProductMapList != null)
                _productResponseList = DeepCopy(myProductSubscription.ProductMapList);
            else
                _productResponseList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();

            if (_productResponseList == null || _productResponseList.Count == 0)
            {
                pubSubW = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
                psdResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>>();
                psdResponse = pubSubW.Proxy.Select(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
                if (Common.CheckResponse(psdResponse.Result, psdResponse.Status) == true)
                {
                    //_productResponseList = DeepCopy(myProductSubscription.ProductMapList);
                    #region Validate List to ensure there is only one instance of each ResponseGroupID
                    List<FrameworkUAD.Entity.ProductSubscriptionDetail> deletes = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                    foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd in _productResponseList)
                    {
                        int rGroupID = answers.Where(x => x.CodeSheetID == psd.CodeSheetID).Select(x=> x.ResponseGroupID).FirstOrDefault();
                        foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd2 in _productResponseList)
                        {
                            int rGroupID2 = answers.Where(x => x.CodeSheetID == psd2.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
                            if(rGroupID == rGroupID2 && psd.CodeSheetID != psd2.CodeSheetID)
                            {
                                if (psd.DateCreated > psd2.DateCreated)
                                    deletes.Add(psd2);
                                else
                                    deletes.Add(psd);
                            }
                        }
                    }
                    _productResponseList = _productResponseList.Except(deletes).ToList();
                    #endregion
                }
            }
            else
            {
                #region Validate List to ensure there is only one instance of each ResponseGroupID
                List<FrameworkUAD.Entity.ProductSubscriptionDetail> deletes = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd in _productResponseList)
                {
                    int rGroupID = answers.Where(x => x.CodeSheetID == psd.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
                    bool? isMulti = questions.Where(x => x.ResponseGroupID == rGroupID).Select(x=> x.IsMultipleValue).FirstOrDefault();
                    if (isMulti == false)
                    {
                        foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd2 in _productResponseList)
                        {
                            int rGroupID2 = answers.Where(x => x.CodeSheetID == psd2.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
                            if (rGroupID == rGroupID2 && psd.CodeSheetID != psd2.CodeSheetID)
                            {
                                if (psd.DateCreated > psd2.DateCreated)
                                    deletes.Add(psd2);
                                else
                                    deletes.Add(psd);
                            }
                        }
                    }
                }
                _productResponseList = _productResponseList.Except(deletes).ToList();
                #endregion
            }

            if (myProductSubscription.QualificationDate != null)
                this.QDate = myProductSubscription.QualificationDate ?? DateTime.Today;
            else
                this.QDate = DateTime.Today;

            this.QSourceID = myProductSubscription.PubQSourceID;
            this.Par3C = myProductSubscription.Par3CID;
            #endregion
            #region Populate Collections (MarketingMap, ResponseGroup + CodeSheet)
            mWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            //List<FrameworkUAD_Lookup.Entity.Code> marketingList = mWorker.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeTypes.Marketing).Result;
            //List<MarketingAnswer> markets = new List<MarketingAnswer>();

            //foreach (FrameworkUAD_Lookup.Entity.Code m in marketingList)
            //{
            //    if (_marketingMapList.Select(x => x.MarketingID).Contains(m.CodeId))
            //    {
            //        if(m.IsActive == true)
            //            markets.Add(new MarketingAnswer(m, true));
            //        else
            //            markets.Add(new MarketingAnswer(m, false));
            //    }
            //    else
            //    {
            //        markets.Add(new MarketingAnswer(m, false));
            //        _marketingMapList.Add(new FrameworkUAD.Entity.MarketingMap() {MarketingID = m.CodeId, IsActive = false, PublicationID = myProduct.PubID,
            //                                                                      PubSubscriptionID = myProductSubscription.PubSubscriptionID,
            //                                                                      CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
            //                                                                      DateCreated = DateTime.Now });
            //    }
            //}

            //Is there a better way to build these lists? Seems like a lot of iteration - Nick 7/17/15
            foreach(FrameworkUAD.Entity.ResponseGroup rg in questions)
            {
                List<Answer> a = new List<Answer>();
                List<FrameworkUAD.Entity.CodeSheet> cs = answers.Where(x => x.ResponseGroupID == rg.ResponseGroupID).OrderBy(x => x.ResponseValue).ToList();
                string other = "";
                foreach(FrameworkUAD.Entity.CodeSheet c in cs)
                {
                    FrameworkUAD.Entity.ProductSubscriptionDetail psd = _productResponseList.Where(x=> x.CodeSheetID == c.CodeSheetID).FirstOrDefault();
                    if (psd != null)
                        other = (psd.ResponseOther ?? "");
                    if (_productResponseList.Select(x => x.CodeSheetID).Contains(c.CodeSheetID))
                        a.Add(new Answer(c.CodeSheetID, c.PubID, c.ResponseDesc, c.ResponseValue, c.ResponseGroupID, c.DisplayOrder, c.IsActive, c.IsOther, true));
                    else
                        a.Add(new Answer(c.CodeSheetID, c.PubID, c.ResponseDesc, c.ResponseValue, c.ResponseGroupID, c.DisplayOrder, c.IsActive, c.IsOther, false));
                }
                if(a != null && a.Count > 0)
                    questionCollection.Add(new Question(rg.DisplayName, rg.ResponseGroupID, a, (rg.IsRequired ?? false), (rg.IsMultipleValue ?? false), other));
            }

            _productResponseList.ForEach(x => { if (x.ResponseOther == null) x.ResponseOther = ""; });
            //foreach (FrameworkUAD.Entity.MarketingMap mm in _marketingMapList)
            //{
            //    _origMarketingMapList.Add(Core_AMS.Utilities.WPF.DeepClone<FrameworkUAD.Entity.MarketingMap>(mm));
            //}
            foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd in _productResponseList)
            {
                _origProductResponseList.Add(Core_AMS.Utilities.WPF.DeepClone<FrameworkUAD.Entity.ProductSubscriptionDetail>(psd));
            }

            foreach(Question q in questionCollection)
            {
                q.PropertyChanged += UpdateOtherChanges;
            }

            icQuestions.ItemsSource = questionCollection;
            //lbContactMethod.ItemsSource = markets;
            #endregion
            this.MadeResponseChange = false;
            qualDateChanged = false;
        }
        #region UI Events
        public void spQualDate_LostFocus(object sender, RoutedEventArgs e)
        {
            RadDatePicker dp = sender as RadDatePicker;
            if (dp.SelectedDate != null)
            {
                if (QualDateChanged != null)
                    QualDateChanged(true);
                //this.TriggerQualDateChange = false;
            }
        }
        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            RadButton me = sender as RadButton;
            RadDropDownButton dp = me.ParentOfType<RadDropDownButton>();
            dp.IsOpen = false;
        }
        private void lbDemo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadListBox rlb = sender as RadListBox;
            foreach (Answer a in e.AddedItems)
            {
                if (!ProductResponseList.Select(x => x.CodeSheetID).Contains(a.CodeSheetID)) //Don't need to add if Subscriber has this value currently saved in their MarketingMaps on first load of UserControl.
                {
                    FrameworkUAD.Entity.ProductSubscriptionDetail psd = new FrameworkUAD.Entity.ProductSubscriptionDetail()
                    {
                        CodeSheetID = a.CodeSheetID,
                        DateCreated = DateTime.Now,
                        PubSubscriptionID = myProductSubscription.PubSubscriptionID,
                        ResponseOther = "",
                        SubscriptionID = myProductSubscription.SubscriptionID,
                        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID
                    };
                    if (a.IsOther == true)
                    {
                        Question q = (Question)rlb.DataContext;
                        q.ShowOther = true;
                    }
                    this.ProductResponseList.Add(psd);
                }
            }
            foreach (Answer a in e.RemovedItems)
            {
                this.ProductResponseList.RemoveAll(x => x.CodeSheetID == a.CodeSheetID);
                if (a.IsOther == true)
                {
                    Question q = (Question)rlb.DataContext;
                    q.ShowOther = false;
                    q.OtherValue = "";    
                }
                a.IsSelected = false;
            }

            if (!this.ProductResponseList.OrderBy(x => x.CodeSheetID).Select(x => x.CodeSheetID).SequenceEqual(_origProductResponseList.OrderBy(x => x.CodeSheetID).Select(x => x.CodeSheetID)))
            {
                this.MadeResponseChange = true;
                //this.TriggerQualDateChange = true;
            }
            else
            {
                this.MadeResponseChange = false;
                if (this.myProductSubscription.IsNewSubscription)
                {
                    this.TriggerQualDateChange = true;
                }
                else
                {
                    this.TriggerQualDateChange = false;
                }
                //var t = from b in _marketingMapList
                //        join a in _origMarketingMapList on b.MarketingID equals a.MarketingID
                //        where b.IsActive != a.IsActive
                //        select b;
                //if (t.Count() == 0)
                //{
                //    this.MadeResponseChange = false;
                //    this.TriggerQualDateChange = false;
                //}
            }
            //else if(this.MarketingMapList.OrderBy(x=> x.MarketingID).Select(x=> x.MarketingID).SequenceEqual(_origMarketingMapList.OrderBy(x=> x.MarketingID).Select(x=> x.MarketingID)))
            //{
            //    this.MadeResponseChange = false;
            //    //this.TriggerQualDateChange = false;
            //}
        }
        private void lbContactMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //RadListBox rlb = sender as RadListBox;
            //foreach (MarketingAnswer m in e.AddedItems)
            //{
            //    //if (!_marketingMapList.Select(x => x.MarketingID).Contains(m.MarketingID)) //Same condition as lbDemos above.
            //    //{
            //    //    FrameworkUAD.Entity.MarketingMap mm = new FrameworkUAD.Entity.MarketingMap()
            //    //    {
            //    //        MarketingID = m.MarketingID,
            //    //        DateCreated = DateTime.Now,
            //    //        PubSubscriptionID = 1,
            //    //        IsActive = true,
            //    //        PublicationID = 3,
            //    //        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID
            //    //    };
            //    //    _marketingMapList.Add(mm);
            //    //}
            //    //else
            //        _marketingMapList.Where(x => x.MarketingID == m.MarketingID).FirstOrDefault().IsActive = true;
            //}
            //foreach (MarketingAnswer m in e.RemovedItems)
            //{
            //    //_marketingMapList.RemoveAll(x => x.MarketingID == m.MarketingID);
            //    _marketingMapList.Where(x => x.MarketingID == m.MarketingID).FirstOrDefault().IsActive = false;
            //}

            //var t = from b in _marketingMapList
            //        join a in _origMarketingMapList on b.MarketingID equals a.MarketingID
            //        where b.IsActive != a.IsActive
            //        select b;
            //if (t.Count() > 0)
            //{
            //    this.MadeResponseChange = true;
            //    //this.TriggerQualDateChange = true;
            //}
            ////if (!this.MarketingMapList.OrderBy(x => x.MarketingID).Select(x => x.MarketingID).SequenceEqual(_origMarketingMapList.OrderBy(x => x.MarketingID).Select(x => x.MarketingID)))
            ////{

            ////}
            //else if (this.ProductResponseList.OrderBy(x => x.CodeSheetID).Select(x => x.CodeSheetID).SequenceEqual(_origProductResponseList.OrderBy(x => x.CodeSheetID).Select(x => x.CodeSheetID)))
            //{
            //    this.MadeResponseChange = false;
            //    //this.TriggerQualDateChange = false;
            //}
        }
        #endregion
        #region Helpers
        public string RequiredAnswered()
        {
            StringBuilder sb = new StringBuilder();
            List<string> questionNotAnswered = new List<string>();

            foreach (var q in questions)
            {
                if (q.IsRequired == true)
                {
                    var x = (from rml in ProductResponseList
                             join r in answers on rml.CodeSheetID equals r.CodeSheetID
                             join rt in questions on r.ResponseGroupID equals rt.ResponseGroupID
                             where rt.IsRequired == true && rt.ResponseGroupID == q.ResponseGroupID
                             select rml.SubscriptionID).ToList();
                    if (x.Count == 0)
                    {
                        if (!questionNotAnswered.Contains(q.DisplayName))
                        {
                            questionNotAnswered.Add(q.DisplayName);
                        }
                    }
                }
            }

            if (questionNotAnswered.Count != 0)
            {
                foreach (string s in questionNotAnswered)
                {
                    if (sb.Length == 0)
                        sb.Append(s);
                    else
                        sb.Append("," + s);
                }
            }
            string ua = sb.ToString();
            return ua;
        }
        public void UpdateRequiredQuestions(bool isRequired)
        {
            foreach(Question q in questionCollection)
            {
                FrameworkUAD.Entity.ResponseGroup rg = questions.Where(x => x.ResponseGroupID == q.GroupID).FirstOrDefault();
                if (rg.IsRequired == true)
                    q.IsRequired = isRequired;
            }
        }
        public List<FrameworkUAD.Entity.ProductSubscriptionDetail> DeepCopy(List<FrameworkUAD.Entity.ProductSubscriptionDetail> list)
        {
            List<FrameworkUAD.Entity.ProductSubscriptionDetail> copy = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
            foreach(FrameworkUAD.Entity.ProductSubscriptionDetail psd in list)
            {
                FrameworkUAD.Entity.ProductSubscriptionDetail newpsd = new FrameworkUAD.Entity.ProductSubscriptionDetail();
                newpsd.CodeSheetID = psd.CodeSheetID;
                newpsd.CreatedByUserID = psd.CreatedByUserID;
                newpsd.DateCreated = psd.DateCreated;
                newpsd.DateUpdated = psd.DateUpdated;
                newpsd.PubSubscriptionDetailID = psd.PubSubscriptionDetailID;
                newpsd.PubSubscriptionID = psd.PubSubscriptionID;
                newpsd.ResponseOther = psd.ResponseOther;
                newpsd.SubscriptionID = psd.SubscriptionID;
                newpsd.UpdatedByUserID = psd.UpdatedByUserID;
                copy.Add(newpsd);
            }
            return copy;
        }
        #endregion
    }
}
