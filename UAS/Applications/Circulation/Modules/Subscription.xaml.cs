using Circulation.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for Subscription.xaml
    /// </summary>
    public partial class Subscription : UserControl, INotifyPropertyChanged
    {
        //Subscription exists within a tab in SubscriptionContainer module. Controls Cat/Trans data and Subscribing/Unsubscribing.
        #region Entity
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList;
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList;
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList;
        private List<FrameworkUAD_Lookup.Entity.Action> actions;
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList;
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sstList;
        private List<FrameworkUAD_Lookup.Entity.Code> codeList;
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList;
        #endregion
        #region Worker
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> tWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> ccWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> aWorker = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catCodeTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> subscriptionWorker = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> sstWorker = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusClient();
        #endregion
        #region Response
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> catTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> sstResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>>();
        #endregion
        #region Properties
        private string _onBehalfOf;
        private int? _categoryID;
        private int _transactionID;
        private int _subscriptionStatus;
        private bool _isSubscribed;
        private bool _isPaid;
        private bool _isNewSubscription;
        private bool _isActive;
        private string _transactionName;
        private bool _enabled;
        private bool _reactivateEnabled;
        private bool firstLoad;
        private bool _isCountryEnabled;
        private bool _requalOnlyChange;
        private bool _triggerQualDate;
        public string OnBehalfOf
        {
            get { return _onBehalfOf; }
            set
            {
                _onBehalfOf = value;
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
                if (_categoryID != null)
                {
                    FrameworkUAD_Lookup.Entity.CategoryCode cc = catCodeList.Where(x => x.CategoryCodeID == _categoryID).FirstOrDefault();
                    ModifySubscriptionStatus();
                    if (cc != null)
                    {
                        if (cc.CategoryCodeValue == 11 || cc.CategoryCodeValue == 21 || cc.CategoryCodeValue == 25 || cc.CategoryCodeValue == 28 || cc.CategoryCodeValue == 31 ||
                            cc.CategoryCodeValue == 35 || cc.CategoryCodeValue == 51 || cc.CategoryCodeValue == 56 || cc.CategoryCodeValue == 62)
                            UpdateCopiesControl(true);
                        else
                            UpdateCopiesControl(false);
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
                _transactionID = value;
                if (_transactionID != null)
                {
                    ModifySubscriptionStatus();
                    string name = transCodeList.Where(x => x.TransactionCodeID == _transactionID).Select(x => x.TransactionCodeName).FirstOrDefault();
                    this.TransactionName = name;
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TransactionCodeID"));
                }
            }
        }
        public int SubscriptionStatus
        {
            get { return _subscriptionStatus; }
            set
            {
                _subscriptionStatus = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SubscriptionStatus"));
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
        public bool RequalOnlyChange
        {
            get { return _requalOnlyChange; }
            set
            {
                _requalOnlyChange = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RequalOnlyChange"));
                }
            }
        }
        public bool IsPaid
        {
            get { return _isPaid; }
            set
            {
                _isPaid = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPaid"));
                }
            }
        }
        public bool IsNewSubscription
        {
            get { return _isNewSubscription; }
            set
            {
                _isNewSubscription = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsNewSubscription"));
                }
            }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsActive"));
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
        public bool ReactivateEnabled
        {
            get { return _reactivateEnabled; }
            set
            {
                _reactivateEnabled = value;
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
        public string TransactionName
        {
            get { return _transactionName; }
            set
            {
                _transactionName = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TransactionName"));
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

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<bool> UpdateRequiredQuestions;
        public event Action<bool> UpdateCopiesEnabled;
        #endregion

        public Subscription(KMPlatform.Entity.Client publisher, FrameworkUAD.Entity.Product product, FrameworkUAD.Entity.ProductSubscription prdSubscription)
        {
            InitializeComponent();

            this.DataContext = this;
            firstLoad = true;

            myClient = publisher;
            myProduct = product;
            myProductSubscription = prdSubscription;
            actions = Home.Actions;
            transCodeList = Home.TransactionCodes;
            catCodeList = Home.CategoryCodes;
            sstList = Home.SubscriptionStatuses;
            catTypeList = Home.CategoryCodeTypes;
            codeList = Home.Codes;
            codeTypeList = Home.CodeTypes;
            transCodeTypeList = Home.TransactionCodeTypes;

            cbCat.IsEnabled = false;

            if (sstList == null || sstList.Count == 0)
            {
                sstResponse = sstWorker.Proxy.Select(accessKey);
                if (Common.CheckResponse(sstResponse.Result, sstResponse.Status) == true)
                    sstList = sstResponse.Result;
            }
            if (actions == null || actions.Count == 0)
            {
                actionResponse = aWorker.Proxy.Select(accessKey);
                if (actionResponse != null && actionResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    actions = actionResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            if (myProductSubscription.IsNewSubscription == true)
            {
                this.IsSubscribed = true;
                cbFreePaid.IsEnabled = true;
                cbCat.IsEnabled = true;
            }
            else
            {
                cbFreePaid.IsEnabled = false;
                cbCat.IsEnabled = false;
            }

            if (myProductSubscription.IsNewSubscription == false)
                this.TransactionCodeID = myProductSubscription.PubTransactionID;

            BindCatTranCodes();

            if (!string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf))
                this.OnBehalfOf = myProductSubscription.OnBehalfOf;

            if (myProductSubscription.IsLocked == true)
                btnReactivate.IsEnabled = false;

            this.CategoryCodeID = myProductSubscription.PubCategoryID;
            this.IsPaid = myProductSubscription.IsPaid;
            this.IsSubscribed = myProductSubscription.IsSubscribed;
            this.SubscriptionStatus = myProductSubscription.SubscriptionStatusID;
            this.OnBehalfOf = myProductSubscription.OnBehalfOf;
            this.IsPaid = myProductSubscription.IsPaid;
            this.IsNewSubscription = myProductSubscription.IsNewSubscription;

            firstLoad = false;
        }

        #region Subscription Kill/Activate
        private void btnReactivate_Click(object sender, RoutedEventArgs e)
        {
            btnReactivate.IsChecked = true;
            btnPOKill.IsEnabled = true;
            btnPersonKill.IsEnabled = true;
            btnOnBehalfKill.IsEnabled = true;
            tbOnBehalfName.IsEnabled = true;
            tbOnBehalfName.Text = string.Empty;
            myProductSubscription.IsActive = true;
            this.IsCountryEnabled = true;
            this.IsSubscribed = true;
            this.Enabled = true;

            cbFreePaid.IsEnabled = true;
            cbCat.IsEnabled = true;

            this.RequalOnlyChange = true;
            this.TriggerQualDate = true;

            int freePaidvalue = Convert.ToInt32(cbFreePaid.SelectedValue);
            if (freePaidvalue == catTypeList.SingleOrDefault(c => c.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")).CategoryCodeTypeID ||
                freePaidvalue == catTypeList.SingleOrDefault(c => c.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")).CategoryCodeTypeID)
                AddRemoveAsteriskControl(sender, true);
            else
                AddRemoveAsteriskControl(sender, false);

            ModifySubscriptionStatus();
        }
        private void btnPOKill_Click(object sender, RoutedEventArgs e)
        {
            //this.IsActive = false;
            //PromptUnsubscribe();
            cbFreePaid.IsEnabled = false;
            cbCat.IsEnabled = false;
            if (myClient.HasPaid == true && this.IsPaid == true)
                TransactionCodeID = transCodeList.Where(x => x.TransactionCodeValue == 61).Select(c => c.TransactionCodeID).SingleOrDefault();
            else
                TransactionCodeID = transCodeList.Where(x => x.TransactionCodeValue == 31 && x.IsKill == true).Select(c => c.TransactionCodeID).SingleOrDefault();
        }
        private void btnPersonKill_Click(object sender, RoutedEventArgs e)
        {
            //this.IsActive = false;
            //PromptUnsubscribe();
            cbFreePaid.IsEnabled = false;
            cbCat.IsEnabled = false;
            TransactionCodeID = transCodeList.Where(x => x.TransactionCodeValue == 32 && x.IsKill == true).Select(c => c.TransactionCodeID).SingleOrDefault();
        }
        private void btnOnBehalfKill_Click(object sender, RoutedEventArgs e)
        {
            //this.IsActive = false;
            //PromptUnsubscribe();
            cbFreePaid.IsEnabled = false;
            cbCat.IsEnabled = false;
            if (myProductSubscription.IsSubscribed == true)
            {
                if (string.IsNullOrEmpty(tbOnBehalfName.Text))
                {
                    Core_AMS.Utilities.WPF.Message("Please enter the On Behalf Request name before saving.", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, "On Behalf Request");
                }
            }
            TransactionCodeID = transCodeList.Where(x => x.TransactionCodeValue == 32 && x.IsKill == true).Select(c => c.TransactionCodeID).SingleOrDefault();
        }
        private void btnCreditCancel_Click(object sender, RoutedEventArgs e)
        {
            cbFreePaid.IsEnabled = false;
            cbCat.IsEnabled = false;
            TransactionCodeID = transCodeList.Where(x => x.TransactionCodeValue == 65).Select(c => c.TransactionCodeID).SingleOrDefault();
        }
        private void btnPaidExpire_Click(object sender, RoutedEventArgs e)
        {
            cbFreePaid.IsEnabled = false;
            cbCat.IsEnabled = false;
            TransactionCodeID = transCodeList.Where(x => x.TransactionCodeValue == 64).Select(c => c.TransactionCodeID).SingleOrDefault();
        }
        private void cbFreePaid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            if (firstLoad == false) //This stops the event from running when the page first loads.
            {
                if (rcb.SelectedItem != null && e.RemovedItems.Count > 0 && !myProductSubscription.IsNewSubscription)
                {
                    FrameworkUAD_Lookup.Entity.CategoryCodeType prevType = e.RemovedItems[0] as FrameworkUAD_Lookup.Entity.CategoryCodeType;
                    FrameworkUAD_Lookup.Entity.CategoryCodeType ccType = rcb.SelectedItem as FrameworkUAD_Lookup.Entity.CategoryCodeType;
                    if (prevType != null && ccType != null && prevType.CategoryCodeTypeID != ccType.CategoryCodeTypeID)
                    {          
                        //Paid to Free
                        if (((prevType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")) ||
                            (prevType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))) &&
                            ((ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")) ||
                            (ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " "))))
                        {
                            this.IsPaid = false;
                        }
                        //Free to Paid
                        else if (((prevType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")) ||
                            (prevType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " "))) &&
                            ((ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")) ||
                            (ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))))
                        {
                            this.IsPaid = true;
                            //SUB GEN CODE
                            //NEED TO CHANGE THIS TO TAKE IN NEW ID FIELD FROM PUBSUBSCRIPTION - TEST 2893945
                            if (myProduct.UseSubGen)
                            {
                                if (myProductSubscription.SubGenSubscriberID != null && myProductSubscription.SubGenSubscriberID > 0)
                                {
                                    Modules.SubscriptionGenius sg = new SubscriptionGenius(KMPlatform.BusinessLogic.Enums.SubGenControls.Subscriber, myProductSubscription.SubGenSubscriberID);
                                    Windows.PlainPopout pop = new Windows.PlainPopout(sg);
                                    pop.Show();
                                }
                                else
                                    Core_AMS.Utilities.WPF.MessageError("This subscriber has not yet been connected to paid subscription services. Please try again later.");
                                Window home = Core_AMS.Utilities.WPF.GetParentWindow(this);
                                Windows.Popout homeWin = (Windows.Popout)home;
                                homeWin.IgnoreSaveMessage = true;
                                if (home != null)
                                    home.Close();
                            }
                        }
                        if (ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") ||
                            ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))
                        {
                            if (UpdateRequiredQuestions != null)
                                UpdateRequiredQuestions(false);
                        }
                        else if (UpdateRequiredQuestions != null)
                            UpdateRequiredQuestions(true);

                        cbCat.ItemsSource = catCodeList.Where(c => c.CategoryCodeTypeID == Convert.ToInt32(cbFreePaid.SelectedValue) && c.IsActive == true);
                        cbCat.DisplayMemberPath = "CategoryCodeName";
                        cbCat.SelectedValuePath = "CategoryCodeID";
                    }
                }
                else if (rcb.SelectedItem != null && myProductSubscription.IsNewSubscription)
                {
                    FrameworkUAD_Lookup.Entity.CategoryCodeType ccType = rcb.SelectedItem as FrameworkUAD_Lookup.Entity.CategoryCodeType;
                    if (ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ") ||
                            ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " "))
                    {
                        FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 10 && x.TransactionCodeTypeID == 1).FirstOrDefault();
                        if (tc != null)
                        {
                            this.TransactionCodeID = tc.TransactionCodeID;
                            this.IsPaid = false;
                        }
                    }
                    else if (ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ") ||
                        ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))
                    {
                        FrameworkUAD_Lookup.Entity.TransactionCode tc = transCodeList.Where(x => x.TransactionCodeValue == 13 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                        if (tc != null)
                        {
                            this.TransactionCodeID = tc.TransactionCodeID;
                            this.IsPaid = true;
                        }
                    }
                    if (ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") ||
                        ccType.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))
                    {
                        if (UpdateRequiredQuestions != null)
                            UpdateRequiredQuestions(false);
                    }
                    else if (UpdateRequiredQuestions != null)
                        UpdateRequiredQuestions(true);

                    cbCat.ItemsSource = catCodeList.Where(c => c.CategoryCodeTypeID == Convert.ToInt32(cbFreePaid.SelectedValue) && c.IsActive == true);
                    cbCat.DisplayMemberPath = "CategoryCodeName";
                    cbCat.SelectedValuePath = "CategoryCodeID";
                }
            }
        }
        private void PromptUnsubscribe()
        {
            if (myProductSubscription.IsSubscribed == false)
            {
                Core_AMS.Utilities.WPF.Message("This subscription has already been unsubscribed.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Unsubscribed Notice.");
                cbFreePaid.IsEnabled = false;
                cbCat.IsEnabled = false;
                btnReactivate.IsChecked = false;
                btnPOKill.IsChecked = false;
                btnPersonKill.IsChecked = false;
                btnOnBehalfKill.IsChecked = false;
                return;
            }
        }
        #endregion
        #region Helpers
        private void BindCatTranCodes()
        {
            if (catCodeList == null || catCodeList.Count == 0)
            {
                ccWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
                catCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
                catCodeResponse = ccWorker.Proxy.Select(accessKey);
                if (catCodeResponse != null && catCodeResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    catCodeList = catCodeResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            if (catTypeList == null || catTypeList.Count == 0)
            {
                catCodeTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
                catTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
                catTypeResponse = catCodeTypeWorker.Proxy.Select(accessKey);
                if (catTypeResponse != null && catTypeResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    catTypeList = catTypeResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            if (transCodeList == null || transCodeList.Count == 0)
            {
                tWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
                transCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
                transCodeResponse = tWorker.Proxy.Select(accessKey);
                if (transCodeResponse != null && transCodeResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    transCodeList = transCodeResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            if (myClient.HasPaid == false)
            {
                cbFreePaid.ItemsSource = catTypeList.Where(f => f.IsFree != myClient.HasPaid && f.IsActive == true);
                cbFreePaid.DisplayMemberPath = "CategoryCodeTypeName";
                cbFreePaid.SelectedValuePath = "CategoryCodeTypeID";
            }
            else
            {
                cbFreePaid.ItemsSource = catTypeList;
                cbFreePaid.DisplayMemberPath = "CategoryCodeTypeName";
                cbFreePaid.SelectedValuePath = "CategoryCodeTypeID";
            }

            FrameworkUAD_Lookup.Entity.Action soloAction = new FrameworkUAD_Lookup.Entity.Action();

            if (myProductSubscription.PubTransactionID > 0)
            {
                if (myProductSubscription.IsNewSubscription == false)
                {                    
                    int codeTypeId = codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Action.ToString())).CodeTypeId;
                    soloAction = actions.SingleOrDefault(a => a.CategoryCodeID == myProductSubscription.PubCategoryID && a.TransactionCodeID == myProductSubscription.PubTransactionID && a.ActionTypeID == codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId && x.CodeName.Equals(FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace("_", " "))).CodeId);
                }

                if (soloAction != null && myProductSubscription.IsNewSubscription == false)
                {
                    int catCodeType = -1;
                    catCodeType = catCodeList.Where(s => s.CategoryCodeID == soloAction.CategoryCodeID).Select(ct => ct.CategoryCodeTypeID).SingleOrDefault();

                    cbFreePaid.SelectedValue = catCodeType;

                    cbCat.ItemsSource = (from cc in catCodeList
                                         join ct in catTypeList on cc.CategoryCodeTypeID equals ct.CategoryCodeTypeID
                                         where cc.IsActive == true && cc.CategoryCodeTypeID == catCodeType
                                         select cc);
                    cbCat.DisplayMemberPath = "CategoryCodeName";
                    cbCat.SelectedValuePath = "CategoryCodeID";

                    int freePOHold = transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Free_PO_Hold.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int freeRequest = transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Free_Request.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int paidRefund = transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Paid_Refund.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int paidPOHold = transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Paid_PO_Hold.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();

                    if (soloAction.TransactionCodeID == freePOHold || soloAction.TransactionCodeID == paidPOHold)
                    {
                        btnPOKill.IsChecked = true;
                    }
                    else if (string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf) && (soloAction.TransactionCodeID == paidRefund || soloAction.TransactionCodeID == freeRequest))
                    {
                        btnPersonKill.IsChecked = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf) && (soloAction.TransactionCodeID == paidRefund || soloAction.TransactionCodeID == freeRequest))
                    {
                        btnOnBehalfKill.IsChecked = true;
                    }

                }
            }
        }
        private void ModifySubscriptionStatus()
        {
            int catValue = catCodeList.Where(x => x.CategoryCodeID == _categoryID).Select(x => x.CategoryCodeValue).FirstOrDefault();
            int catTypeID = catCodeList.Where(x => x.CategoryCodeID == _categoryID).Select(x => x.CategoryCodeTypeID).FirstOrDefault();
            int xactTypeID = transCodeList.Where(x => x.TransactionCodeID == _transactionID).Select(x => x.TransactionCodeTypeID).FirstOrDefault();

            if ((catValue == 70 || catValue == 71)) //Verified Prospect & Unverified Prospect
            {
                if (xactTypeID == 1) //Active
                {
                    this.SubscriptionStatus = sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.AProsp.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = true;
                    this.IsActive = true;
                }
                else //InActive
                {
                    this.SubscriptionStatus = sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAProsp.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = false;
                    this.IsActive = false;
                }
            }
            else if (catTypeID == 1 || catTypeID == 2) //Qualified, Non-Qualified Free
            {
                if (xactTypeID == 1) //Free Active
                {
                    this.SubscriptionStatus = sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.AFree.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = true;
                    this.IsActive = true;
                }
                else //Free Inactive
                {
                    this.SubscriptionStatus = sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAFree.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = false;
                    this.IsActive = false;
                }
            }
            else if (catTypeID == 3 || catTypeID == 4) //Qualified, Non-Qualified Paid
            {
                if (xactTypeID == 3) //Paid Active
                {
                    this.SubscriptionStatus = sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.APaid.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = true;
                    this.IsActive = true;
                }
                else //Paid Inactive
                {
                    this.SubscriptionStatus = sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAPaid.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = false;
                    this.IsActive = false;
                }
            }
        }
        public void UpdateCopiesControl(bool enable)
        {
            if (UpdateCopiesEnabled != null)
                UpdateCopiesEnabled(enable);
        }
        public void AddRemoveAsteriskControl(object sender, bool addAsterisk)
        {
            if (UpdateRequiredQuestions != null)
                UpdateRequiredQuestions(addAsterisk);
        }
        #endregion
    }
}
