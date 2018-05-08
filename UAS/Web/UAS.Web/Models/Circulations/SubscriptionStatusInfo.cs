using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class SubscriptionStatusInfo
    {

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
        private EntityLists entlst { get; set; }
        private KMPlatform.Entity.Client myClient { get; set; }
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription { get; set; }
        public string OnBehalfOf
        {
            get { return _onBehalfOf; }
            set
            {
                _onBehalfOf = value;
               
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
                    FrameworkUAD_Lookup.Entity.CategoryCode cc = entlst.categoryCodeList.Where(x => x.CategoryCodeID == _categoryID).FirstOrDefault();
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
               
            }
        }
        public int CategoryCodeValue { get; set; }
        public int TransactionCodeID
        {
            get { return _transactionID; }
            set
            {
                _transactionID = value;
                if (_transactionID != null)
                {
                    ModifySubscriptionStatus();
                    string name = entlst.transCodeList.Where(x => x.TransactionCodeID == _transactionID).Select(x => x.TransactionCodeName).FirstOrDefault();
                    this.TransactionName = name;
                }
                
            }
        }
        public int SubscriptionStatus
        {
            get { return _subscriptionStatus; }
            set
            {
                _subscriptionStatus = value;
                
            }
        }
        public bool IsSubscribed
        {
            get { return _isSubscribed; }
            set
            {
                _isSubscribed = value;
               
            }
        }
        public bool RequalOnlyChange
        {
            get { return _requalOnlyChange; }
            set
            {
                _requalOnlyChange = value;
               
            }
        }
        public bool IsPaid
        {
            get { return _isPaid; }
            set
            {
                _isPaid = value;
               
            }
        }
        public bool IsNewSubscription
        {
            get { return _isNewSubscription; }
            set
            {
                _isNewSubscription = value;
                
            }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                
            }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
               
            }
        }
        public bool ReactivateEnabled
        {
            get { return _reactivateEnabled; }
            set
            {
                _reactivateEnabled = value;
                
            }
        }
        public bool IsCountryEnabled
        {
            get { return _isCountryEnabled; }
            set
            {
                _isCountryEnabled = value;
                
            }
        }
        public string TransactionName
        {
            get { return _transactionName; }
            set
            {
                _transactionName = value;
                
            }
        }
        public bool TriggerQualDate
        {
            get { return _triggerQualDate; }
            set
            {
                _triggerQualDate = value;
               
            }
        }
        public bool btnPersonKill { get; set; }
        public bool btnPOKill { get; set; }
        public bool btnOnBehalfKill { get; set; }
        public int? catCodeTypeID { get; set; }
        
        public bool categoryDropdownsEnabled { get; set; }
        public List<SelectListItem> catTypeSelectList { get; set; }
        public List<SelectListItem> catCodeSelectList { get; set; }
        public event Action<bool> UpdateRequiredQuestions;
        public event Action<bool> UpdateCopiesEnabled;
        public SubscriptionStatusInfo()
        {

        }

        public SubscriptionStatusInfo(KMPlatform.Entity.Client _myClient, FrameworkUAD.Entity.ProductSubscription _myProductSubscription, EntityLists _entList)
        {
            entlst= _entList;
            myProductSubscription = _myProductSubscription;
            myClient = _myClient;

            categoryDropdownsEnabled = false;

            if (myProductSubscription.IsNewSubscription == false)
                this.TransactionCodeID = myProductSubscription.PubTransactionID;

            if (myProductSubscription.IsNewSubscription == true)
            {
                this.IsSubscribed = true;
                this.categoryDropdownsEnabled = true;
                
            }
            else
            {
                this.categoryDropdownsEnabled = false;
                
            }

            if (!string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf))
                this.OnBehalfOf = myProductSubscription.OnBehalfOf;

            if (myProductSubscription.IsLocked == true)
                this.ReactivateEnabled = false;//btnReactivate.IsEnabled = false;

            this.CategoryCodeID = myProductSubscription.PubCategoryID;
            this.IsPaid = myProductSubscription.IsPaid;
            this.IsSubscribed = myProductSubscription.IsSubscribed;
            this.SubscriptionStatus = myProductSubscription.SubscriptionStatusID;
            this.OnBehalfOf = myProductSubscription.OnBehalfOf;
            this.IsPaid = myProductSubscription.IsPaid;
            this.IsNewSubscription = myProductSubscription.IsNewSubscription;

            this.CategoryCodeValue = entlst.categoryCodeList.Where(x => x.CategoryCodeID == myProductSubscription.PubCategoryID).Select(x => x.CategoryCodeValue).FirstOrDefault();
            if (myClient.HasPaid == false)
            {
                this.catTypeSelectList = new List<SelectListItem>();
                var catSelectList = entlst.catTypeList.Where(f => f.IsFree != myClient.HasPaid && f.IsActive == true);
                foreach (var ct in catSelectList)
                {
                    this.catTypeSelectList.Add(new SelectListItem { Text = ct.CategoryCodeTypeName, Value = ct.CategoryCodeTypeID.ToString() });
                }
                //cbFreePaid.DisplayMemberPath = "CategoryCodeTypeName";
                //cbFreePaid.SelectedValuePath = "CategoryCodeTypeID";
            }
            else
            {
                this.catTypeSelectList = entlst.CategoryTypesSelectList;

            }

            FrameworkUAD_Lookup.Entity.Action soloAction = new FrameworkUAD_Lookup.Entity.Action();
            if (myProductSubscription.PubTransactionID > 0)
            {
                if (myProductSubscription.IsNewSubscription == false)
                {
                    int codeTypeId = entlst.codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Action.ToString())).CodeTypeId;
                    soloAction = entlst.actionList.SingleOrDefault(a => a.CategoryCodeID == myProductSubscription.PubCategoryID && a.TransactionCodeID == myProductSubscription.PubTransactionID && a.ActionTypeID == entlst.codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId && x.CodeName.Equals(FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace("_", " "))).CodeId);
                }

                if (soloAction != null && myProductSubscription.IsNewSubscription == false)
                {
                    int catCodeType = -1;
                    catCodeType = entlst.categoryCodeList.Where(s => s.CategoryCodeID == soloAction.CategoryCodeID).Select(ct => ct.CategoryCodeTypeID).SingleOrDefault();

                    this.catCodeTypeID = catCodeType;
                    //Check for selected Item
                    foreach (var ct in catTypeSelectList)
                    {
                        if(ct.Value == catCodeType.ToString())
                        {
                            ct.Selected = true;
                        }
                    }

                    this.catCodeSelectList = new List<SelectListItem>();

                    var catCodeList = (from cc in entlst.categoryCodeList
                                         join ct in entlst.catTypeList on cc.CategoryCodeTypeID equals ct.CategoryCodeTypeID
                                         where cc.IsActive == true && cc.CategoryCodeTypeID == catCodeType
                                         select cc);

                    foreach (var cc in catCodeList)
                    {
                        this.catCodeSelectList.Add(new SelectListItem { Text = cc.CategoryCodeName, Value = cc.CategoryCodeID.ToString() });
                    }

                   
                    int freePOHold = entlst.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Free_PO_Hold.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int freeRequest = entlst.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Free_Request.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int paidRefund = entlst.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Paid_Refund.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int paidPOHold = entlst.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Paid_PO_Hold.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();

                    if (soloAction.TransactionCodeID == freePOHold || soloAction.TransactionCodeID == paidPOHold)
                    {
                        this.btnPOKill = true;
                    }
                    else if (string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf) && (soloAction.TransactionCodeID == paidRefund || soloAction.TransactionCodeID == freeRequest))
                    {
                        this.btnPersonKill = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf) && (soloAction.TransactionCodeID == paidRefund || soloAction.TransactionCodeID == freeRequest))
                    {
                        this.btnOnBehalfKill = true;
                    }

                }
            }

        }
        #region Ajax Calls
        
        #endregion
        #region Helpers
        private void ModifySubscriptionStatus()
        {
            int catValue = entlst.categoryCodeList.Where(x => x.CategoryCodeID == _categoryID).Select(x => x.CategoryCodeValue).FirstOrDefault();
            int catTypeID = entlst.categoryCodeList.Where(x => x.CategoryCodeID == _categoryID).Select(x => x.CategoryCodeTypeID).FirstOrDefault();
            int xactTypeID = entlst.transCodeList.Where(x => x.TransactionCodeID == _transactionID).Select(x => x.TransactionCodeTypeID).FirstOrDefault();

            if ((catValue == 70 || catValue == 71)) //Verified Prospect & Unverified Prospect
            {
                if (xactTypeID == 1) //Active
                {
                    this.SubscriptionStatus = entlst.sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.AProsp.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = true;
                    this.IsActive = true;
                }
                else //InActive
                {
                    this.SubscriptionStatus = entlst.sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAProsp.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = false;
                    this.IsActive = false;
                }
            }
            else if (catTypeID == 1 || catTypeID == 2) //Qualified, Non-Qualified Free
            {
                if (xactTypeID == 1) //Free Active
                {
                    this.SubscriptionStatus = entlst.sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.AFree.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = true;
                    this.IsActive = true;
                }
                else //Free Inactive
                {
                    this.SubscriptionStatus = entlst.sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAFree.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = false;
                    this.IsActive = false;
                }
            }
            else if (catTypeID == 3 || catTypeID == 4) //Qualified, Non-Qualified Paid
            {
                if (xactTypeID == 3) //Paid Active
                {
                    this.SubscriptionStatus = entlst.sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.APaid.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
                    this.IsSubscribed = true;
                    this.IsActive = true;
                }
                else //Paid Inactive
                {
                    this.SubscriptionStatus = entlst.sstList.Where(x => x.StatusCode == FrameworkUAD_Lookup.Enums.SubscriptionStatus.IAPaid.ToString()).Select(x => x.SubscriptionStatusID).FirstOrDefault();
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