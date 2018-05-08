using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.ComponentModel;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for SubscriptionInfo.xaml
    /// </summary>
    public partial class SubscriptionInfo : UserControl, INotifyPropertyChanged
    {
        //SubscriptionInfo exists within a tab in SubscriptionContainer module.
        #region Entity/Lists
        private KMPlatform.Entity.Client myPublisher = new KMPlatform.Entity.Client();
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private FrameworkUAD.Entity.ProductSubscriptionDetail myProductSubscriptionDetail = new FrameworkUAD.Entity.ProductSubscriptionDetail();
        private string originalSubSrc = "";
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        public List<FrameworkUAD.Entity.MarketingMap> MarketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> subSourceList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        #endregion
        #region Worker
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> sstWorker;
        #endregion
        #region Service Resp
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> ssTypeResponse;
        #endregion
        #region Properties
        private string _accountNumber;
        private string _memberGroup;
        private string _origSubSrc;
        private string _subSrc;
        private int _subSrcID;
        private int _deliverability;
        private string _verify;
        private bool _triggerQualDate;
        private bool _enabled;
        public string AccountNumber
        {
            get { return _accountNumber; }
            set
            {
                _accountNumber = value;
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
                if (string.IsNullOrEmpty(originalSubSrc))
                    this.OriginalSubscriberSourceCode = _subSrc;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SubscriberSourceCode"));
                }
            }
        }
        public int? SubSrcID
        {
            get { return _subSrcID; }
            set
            {
                _subSrcID = (value ?? 0);
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SubSrcID"));
                }
            }
        }
        public int? Deliverability
        {
            get { return _deliverability; }
            set
            {
                if(_deliverability != (value ?? 0))
                    _deliverability = (value ?? 0);
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
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Verify"));
                }
            }
        }
        public bool TriggerQualDate
        {
            get { return _triggerQualDate; }
            set
            {
                _triggerQualDate = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TriggerQualDate"));
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

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public SubscriptionInfo(KMPlatform.Entity.Client publisher, FrameworkUAD.Entity.Product product, FrameworkUAD.Entity.ProductSubscription productSubscription)
        {
            InitializeComponent();
            myPublisher = publisher;
            myProduct = product;
            myProductSubscription = productSubscription;
            codeList = Home.Codes;
            codeTypeList = Home.CodeTypes;
            actionList = Home.Actions;
            catCodeList = Home.CategoryCodes;
            catCodeTypeList = Home.CategoryCodeTypes;
            transCodeList = Home.TransactionCodes;
            transCodeTypeList = Home.TransactionCodeTypes;
            originalSubSrc = myProductSubscription.OrigsSrc;

            LoadData();

            this.DataContext = this;
        }
        private void LoadData()
        {
            #region Deliverability & SubSource

            int deliverCodeTypeId = codeTypeList.SingleOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString())).CodeTypeId;

            cbDeliver.ItemsSource = codeList.Where(x => x.CodeTypeId == deliverCodeTypeId).ToList();
            cbDeliver.SelectedValuePath = "CodeId";
            cbDeliver.DisplayMemberPath = "DisplayName";

            if (myProductSubscription.IsNewSubscription == false && !string.IsNullOrEmpty(myProductSubscription.Demo7))
            {
                int demo7 = codeList.SingleOrDefault(x => x.CodeTypeId == deliverCodeTypeId && x.CodeValue.Equals(myProductSubscription.Demo7)).CodeId;
                Deliverability = demo7;
            }

            int subSourceID = codeTypeList.SingleOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Subscriber_Source.ToString().Replace("_"," "))).CodeTypeId;

            subSourceList = codeList.Where(x => x.CodeTypeId == subSourceID).ToList();

            if (subSourceList.Where(x => x.CodeTypeId == subSourceID && x.CodeId == 0).Count() == 0) //CodeIds have to be unique, so we make sure this isn't there first.
                subSourceList.Add(new FrameworkUAD_Lookup.Entity.Code() { IsActive = true, CodeName = "", CodeTypeId = subSourceID, CodeId = 0 });

            cbSubscriberSourceType.ItemsSource = subSourceList.OrderBy(x=> x.CodeId);
            cbSubscriberSourceType.DisplayMemberPath = "DisplayName";
            cbSubscriberSourceType.SelectedValuePath = "CodeId";

            #endregion

            if (myProductSubscription != null)
            {
                if (!string.IsNullOrEmpty(myProductSubscription.OrigsSrc))
                {
                    OriginalSubscriberSourceCode = myProductSubscription.OrigsSrc;
                }
                else if (string.IsNullOrEmpty(myProductSubscription.OrigsSrc) && !string.IsNullOrEmpty(myProductSubscription.SubscriberSourceCode))
                {
                    OriginalSubscriberSourceCode = myProductSubscription.SubscriberSourceCode;
                }

                SubscriberSourceCode = myProductSubscription.SubscriberSourceCode;
                SubSrcID = myProductSubscription.SubSrcID;
                AccountNumber = myProductSubscription.AccountNumber;
                MemberGroup = myProductSubscription.MemberGroup;
                Verify = myProductSubscription.Verify;

                #region DatePickers
                if (myProductSubscription.DateCreated != null && !myProductSubscription.IsNewSubscription)
                    tbCreatedDate.Text = myProductSubscription.DateCreated.ToString();
                else
                    tbCreatedDate.Text = "";

                if (myProductSubscription.DateUpdated != null)
                    tbUpdatedDate.Text = myProductSubscription.DateUpdated.ToString();
                else
                    tbUpdatedDate.Text = "";
                #endregion
            }
        }
    }
}
