using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.BusinessLogic;
using KM.Common;
using KMPlatform.Object;
using UAS.Web.Models.Circulations.Helpers;
using static FrameworkUAD_Lookup.Enums;
using Batch = FrameworkUAS.Object.Batch;
using Enums = FrameworkUAS.BusinessLogic.Enums;
using KmEnums = KMPlatform.BusinessLogic.Enums;
using BoEnums = FrameworkUAD_Lookup.Enums;

namespace UAS.Web.Models.Circulations
{
    public class SubscriptionManager
    {
        #region Entities 
        private KMPlatform.Entity.Client myClient;
        private FrameworkUAD.Entity.Product MyProduct;
        private EntityLists CommonList;
      
        KMPlatform.Entity.User currentuser;
        private FrameworkUAD.Entity.ProductSubscription originalSubscription;
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription;
        private FrameworkUAD.Entity.SubscriptionPaid myProductSubscriptionPaid;
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> myProductSubscriptionDetail;
        private FrameworkUAD.Entity.PaidBillTo myPaidBillTo;
        private FrameworkUAD_Lookup.Entity.SubscriptionStatus myStatus = new FrameworkUAD_Lookup.Entity.SubscriptionStatus();
        private FrameworkUAD_Lookup.Entity.CategoryCode selectedCat = new FrameworkUAD_Lookup.Entity.CategoryCode();
        private FrameworkUAD_Lookup.Entity.Action soloAction = new FrameworkUAD_Lookup.Entity.Action();
        private FrameworkUAD_Lookup.Entity.Country selCountryPhonePrefix = new FrameworkUAD_Lookup.Entity.Country();
        private FrameworkUAD.Entity.WaveMailingDetail myWMDetail = new FrameworkUAD.Entity.WaveMailingDetail();
        private FrameworkUAD.Entity.ProductSubscription waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription();
        private FrameworkUAD_Lookup.Entity.Action action = new FrameworkUAD_Lookup.Entity.Action();
        private FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix sStatus = new FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix();
        private List<FrameworkUAD.Entity.ResponseGroup> questions = new List<FrameworkUAD.Entity.ResponseGroup>();
        private List<FrameworkUAD.Entity.CodeSheet> answers = new List<FrameworkUAD.Entity.CodeSheet>();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> _productResponseList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> _origProductResponseList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private List<FrameworkUAD.Entity.Product> productList = new List<FrameworkUAD.Entity.Product>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.Code> qSourceList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> parList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> marketingList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> addressTypeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        private List<FrameworkUAD_Lookup.Entity.Region> regions = new List<FrameworkUAD_Lookup.Entity.Region>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> categoryCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sstList = new List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>();
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> ssmList = new List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>();

        private KMPlatform.BusinessLogic.Client _clientBO;
        private FrameworkUAD.BusinessLogic.ProductSubscription _productSubBO;
        private FrameworkUAD_Lookup.BusinessLogic.Country _countryBO;
        private FrameworkUAD_Lookup.BusinessLogic.Region _regionBO;
        private FrameworkUAD_Lookup.BusinessLogic.CodeType _codeTypeBO;
        private FrameworkUAD_Lookup.BusinessLogic.Code _codeBO;
        private FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType _transactionTypeCodeBO;
        private FrameworkUAD_Lookup.BusinessLogic.TransactionCode _transactionCodeBO;
        private FrameworkUAD_Lookup.BusinessLogic.CategoryCode _categoryCodeBO;
        private FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType _categoryCodeTypeBO;
        private FrameworkUAD_Lookup.BusinessLogic.Action _actionBO;
        private FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus _subscriptionStatusBO;
        private FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix _subscriptionStatusMatrixBO;
        private FrameworkUAD.BusinessLogic.SubscriptionPaid _subPaidBO;
        FrameworkUAD.BusinessLogic.PaidBillTo _subPaidBillTo;
        private SubscriberViewModel subVM;
        #endregion

        private bool saveWaveMailing = false;
        private int applicationID = 0;

        private const int SaveErrorResult = 0;

        public SubscriptionManager()
        {

        }
        public SubscriptionManager(KMPlatform.Entity.Client publisher, KMPlatform.Entity.User user, FrameworkUAD.Entity.Product product, FrameworkUAD.Entity.ProductSubscription prodSubscription,
            EntityLists entlist , FrameworkUAD.Entity.ProductSubscription orgSubscription =null)
        {
            this.subVM = new SubscriberViewModel();
            this.MyProduct = product;
            this.CommonList = entlist;
            this.currentuser = user;
            if (publisher != null && publisher.ClientID > 0 && product != null && product.PubID > 0)
            {
                this.myClient = publisher;

                if (prodSubscription != null)
                {
                    if (string.IsNullOrEmpty(prodSubscription.PubCode))
                        prodSubscription.PubCode = product.PubCode;

                    this.originalSubscription = orgSubscription;
                    this.myProductSubscription = prodSubscription;
                    this.myProductSubscription.DateCreated = orgSubscription.DateCreated;

                    if (myProductSubscription.ProductMapList != null)
                        myProductSubscriptionDetail = myProductSubscription.ProductMapList;
                }

            }

        }

        internal SubscriberViewModel GetViewModel()
        {
            subVM.ClientName = myClient.DisplayName;
            FrameworkUAD.BusinessLogic.ProductSubscriptionDetail productSubDetailWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionDetail();
            FrameworkUAD_Lookup.BusinessLogic.TransactionCode tcWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
            transCodeList = tcWorker.Select();

            // Use this for retrieve Pubs from specific UAD Databases
            try
            {
                int prodID = -1;
                prodID = MyProduct.PubID;

                if (myProductSubscription != null && (myProductSubscriptionDetail == null || myProductSubscriptionDetail.Count == 0))
                {
                    myProductSubscription.ProductMapList = productSubDetailWorker.Select(myProductSubscription.PubSubscriptionID, myClient.ClientConnections);

                }

                if (myProductSubscription != null && myProductSubscription.PubSubscriptionID > 0)
                {
                    BindValues();
                    subVM.TransactionName = transCodeList.Where(x => x.TransactionCodeID == myProductSubscription.PubTransactionID).Select(x => x.TransactionCodeName).FirstOrDefault();
                    myProductSubscription.IsNewSubscription = false;
                    originalSubscription = new FrameworkUAD.Entity.ProductSubscription(myProductSubscription);
                }
                else
                {
                    myProductSubscription = new FrameworkUAD.Entity.ProductSubscription(myProductSubscription);
                    myProductSubscription.CreatedByUserID = currentuser.UserID;
                    myProductSubscription.IsNewSubscription = true;
                    myProductSubscription.PubID = MyProduct.PubID;
                    myProductSubscription.IsLocked = false;
                    myProductSubscription.CountryID = 1;
                    BindValues();
                    originalSubscription = new FrameworkUAD.Entity.ProductSubscription(myProductSubscription);
                    subVM.Enabled = true;
                    subVM.IsCountryEnabled = true;

                }
                subVM.Product = MyProduct;
                subVM.PubSubscription = myProductSubscription;
                subVM.OriginalPubSubscription = myProductSubscription;
                subVM.PubSubscription.IsLocked = false;

                KMPlatform.Entity.User creadedbyuser = new KMPlatform.BusinessLogic.User().SelectUser(myProductSubscription.CreatedByUserID, false);
                if (creadedbyuser != null)
                    subVM.CreatedByUser = creadedbyuser.FullName;
                else
                    subVM.CreatedByUser = "";

                KMPlatform.Entity.User lastModifiedByUser = new KMPlatform.BusinessLogic.User().SelectUser((int)myProductSubscription.UpdatedByUserID, false);
                if (lastModifiedByUser != null)
                    subVM.LastModifiedByUser = lastModifiedByUser.FullName;
                else
                    subVM.LastModifiedByUser = "";

                if (MyProduct != null && MyProduct.AllowDataEntry == true && (!subVM.PubSubscription.IsLocked || subVM.PubSubscription.LockedByUserID == currentuser.UserID)
                    && subVM.PubSubscription.IsActive && subVM.PubSubscription.IsSubscribed && !currentuser.IsReadOnly)
                {
                    FrameworkUAD.BusinessLogic.ProductSubscription pubSubSubWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                    pubSubSubWorker.UpdateLock(subVM.PubSubscription.PubSubscriptionID, false, currentuser.UserID, myClient.ClientConnections);
                    subVM.PubSubscription.LockedByUserID = currentuser.UserID;
                    subVM.Enabled = true;
                }
                else if (MyProduct != null && (MyProduct.AllowDataEntry == false || subVM.PubSubscription.IsLocked || !subVM.PubSubscription.IsActive || !subVM.PubSubscription.IsSubscribed || currentuser.IsReadOnly))
                {
                    subVM.Enabled = false;

                }
            }
            catch (Exception ex)
            {
                subVM.ErrorList.Add("FailedToGetModel", Core_AMS.Utilities.StringFunctions.FormatException(ex));
            }
            return subVM;
        }

        private void BindValues()
        {
            BindModules();

            if (myProductSubscription.PubID == 0)
            {
                myProductSubscription.PubID = MyProduct.PubID;
            }

            BindZip();
            BindPhone(selCountryPhonePrefix);

            if (myProductSubscription != null && !myProductSubscription.IsNewSubscription)
            {
                myStatus = CommonList.sstList.SingleOrDefault(s => s.SubscriptionStatusID == myProductSubscription.SubscriptionStatusID);

                var soloCat = CommonList.categoryCodeList.SingleOrDefault(v => v.CategoryCodeID == myProductSubscription.PubCategoryID);
                selectedCat = soloCat;

                if (myProductSubscription.PubTransactionID > 0)
                {
                    BindSoloAction(soloCat);
                    SetIsCopiesEnabledStatus(soloCat);
                    SetAreQuestionsRequiredStatus();
                }
            }

            myProductSubscription.Phone = myProductSubscription.Phone.RemoveDash();
            myProductSubscription.Fax = myProductSubscription.Fax.RemoveDash();
            myProductSubscription.Mobile = myProductSubscription.Mobile.RemoveDash();

            ReLoadProduct();
            SetSubscriberViewModelEnableStatus();
        }

        private void SetAreQuestionsRequiredStatus()
        {
            subVM.AreQuestionsRequired = false;
            if (soloAction != null)
            {
                var categoryCode = CommonList.categoryCodeList.FirstOrDefault(x => x.CategoryCodeID == soloAction.CategoryCodeID);
                if (categoryCode != null)
                {
                    var ccType = CommonList.catTypeList.SingleOrDefault(z => z.CategoryCodeTypeID == categoryCode.CategoryCodeTypeID);

                    subVM.AreQuestionsRequired = ccType.CategoryCodeTypeName.Equals(BoEnums.CategoryCodeType.Qualified_Free.ToString().SpaceInstedOfUnderscore()) ||
                                                        ccType.CategoryCodeTypeName.Equals(BoEnums.CategoryCodeType.Qualified_Paid.ToString().SpaceInstedOfUnderscore());

                }
            }
        }

        private void SetIsCopiesEnabledStatus(FrameworkUAD_Lookup.Entity.CategoryCode soloCategory)
        {
            Guard.NotNull(soloCategory, nameof(soloCategory));

            subVM.IsCopiesEnabled = myProductSubscription != null
                                        && myProductSubscription.IsSubscribed
                                        && soloAction != null
                                        && (soloCategory.CategoryCodeValue == 11
                                            || soloCategory.CategoryCodeValue == 21
                                            || soloCategory.CategoryCodeValue == 25
                                            || soloCategory.CategoryCodeValue == 28
                                            || soloCategory.CategoryCodeValue == 31
                                            || soloCategory.CategoryCodeValue == 35
                                            || soloCategory.CategoryCodeValue == 51
                                            || soloCategory.CategoryCodeValue == 56
                                            || soloCategory.CategoryCodeValue == 62);
        }

        private void BindSoloAction(FrameworkUAD_Lookup.Entity.CategoryCode soloCategory)
        {
            Guard.NotNull(soloCategory, nameof(soloCategory));

            var codeTypeId = CommonList.codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(BoEnums.CodeType.Action.ToString())).CodeTypeId;
            var codeId = CommonList.codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId
                                                            && x.CodeName.Equals(ActionTypes.Data_Entry.ToString().SpaceInstedOfUnderscore())).CodeId;
            var actionEntity = CommonList.actionList.SingleOrDefault(a => a.CategoryCodeID == soloCategory.CategoryCodeID
                                                                        && a.TransactionCodeID == myProductSubscription.PubTransactionID
                                                                        && a.ActionTypeID == codeId);

            var actionId = 0;
            if (actionEntity != null)
            {
                actionId = actionEntity.ActionID;
            }
            else
            {
                subVM.ErrorList.Add("LoadingIssue", "There was a problem loading this Subscriber. Please contact customer service if this problem continues.");
            }

            if (actionId > 0)
            {
                soloAction = CommonList.actionList.SingleOrDefault(x => x.ActionID == actionId);
            }
        }

        private void BindPhone(FrameworkUAD_Lookup.Entity.Country countryPhonePrefix)
        {
            Guard.NotNull(countryPhonePrefix, nameof(countryPhonePrefix));

            if (myProductSubscription.CountryID == GetUsaCountryId()
               || myProductSubscription.CountryID == GetCanadaCountryID())
            {
                myProductSubscription.PhoneCode = 1;
            }
            else if (countryPhonePrefix != null && myProductSubscription.CountryID > 0)
            {
                countryPhonePrefix = CommonList.countryList.SingleOrDefault(x => x.CountryID == myProductSubscription.CountryID);
                myProductSubscription.PhoneCode = countryPhonePrefix.PhonePrefix;
            }
            else
            {
                myProductSubscription.PhoneCode = 1;
            }
        }

        private void BindZip()
        {
            if (myProductSubscription.ZipCode == null)
            {
                myProductSubscription.ZipCode = string.Empty;
            }

            if (myProductSubscription.FullZip == null)
            {
                myProductSubscription.FullZip = string.Empty;
            }

            if (myProductSubscription.CountryID != 0
                && (myProductSubscription.CountryID == GetUsaCountryId()
                    || myProductSubscription.CountryID == GetCanadaCountryID()
                    || myProductSubscription.CountryID == GetMexicoCountryID()))
            {
                if (!string.IsNullOrEmpty(myProductSubscription.ZipCode)
                    && !string.IsNullOrEmpty(myProductSubscription.Plus4)
                    && myProductSubscription.CountryID == GetUsaCountryId())
                {
                    if (myProductSubscription.ZipCode.Length > 5)
                    {
                        myProductSubscription.FullZip = myProductSubscription.ZipCode.RemoveDash();
                    }
                    else
                    {
                        myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
                    }
                }
                else if (!string.IsNullOrEmpty(myProductSubscription.ZipCode)
                    && myProductSubscription.CountryID == CommonList.countryList.SingleOrDefault(x => x.IsCanada()).CountryID)
                {
                    if (myProductSubscription.ZipCode.Length > 3)
                    {
                        myProductSubscription.FullZip = myProductSubscription.ZipCode.Replace(" ", "");
                    }
                    else
                    {
                        myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
                    }
                }
                else
                {
                    myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
                }
            }
            else
            {
                myProductSubscription.FullZip = myProductSubscription.ZipCode + myProductSubscription.Plus4;
            }
        }

        private int GetCanadaCountryID()
        {
            return CommonList.countryList.SingleOrDefault(x => x.IsCanada()).CountryID;
        }

        private int GetUsaCountryId()
        {
            return CommonList.countryList.SingleOrDefault(x => x.IsUsa()).CountryID;
        }

        private int GetMexicoCountryID()
        {
            return CommonList.countryList.SingleOrDefault(x => x.IsMexico()).CountryID;
        }

        private void SetSubscriberViewModelEnableStatus()
        {
            var subscriptionStatus = CommonList.sstList.FirstOrDefault(x => x.SubscriptionStatusID == myProductSubscription.SubscriptionStatusID);
            if (!MyProduct.AllowDataEntry || (myProductSubscription.IsLocked && myProductSubscription.LockedByUserID != currentuser.UserID))
            {
                subVM.Enabled = false;
                subVM.ReactivateButtonEnabled = false;
            }
            else if (subscriptionStatus != null &&
                (subscriptionStatus.StatusCode == BoEnums.SubscriptionStatus.IAFree.ToString() ||
                subscriptionStatus.StatusCode == BoEnums.SubscriptionStatus.IAPaid.ToString() ||
                subscriptionStatus.StatusCode == BoEnums.SubscriptionStatus.IAProsp.ToString()))
            {
                subVM.Enabled = false;
                subVM.ReactivateButtonEnabled = true;
            }
            else
            {
                subVM.Enabled = true;
                subVM.ReactivateButtonEnabled = true;
            }
        }

        private void BindModules()
        {
             _productSubBO = new FrameworkUAD.BusinessLogic.ProductSubscription();
            FrameworkUAD.BusinessLogic.ProductSubscriptionDetail productSubDetailWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionDetail();
            FrameworkUAD.BusinessLogic.ResponseGroup _responseGroupBO = new FrameworkUAD.BusinessLogic.ResponseGroup();
            FrameworkUAD.BusinessLogic.CodeSheet _codeSheetBO = new FrameworkUAD.BusinessLogic.CodeSheet();
            List<Question> questionCollection = new List<Question>();

            //Status

            if (myProductSubscription.IsLocked == true)
                subVM.ReactivateButtonEnabled = false;
            if (myProductSubscription.IsNewSubscription == true)
            {
                myProductSubscription.IsSubscribed = true;
                subVM.CategoryFreePaidEnabled = true;
                subVM.CategoryCodeEnabled = true;
            }
            else
            {
                subVM.CategoryFreePaidEnabled = false;
                subVM.CategoryCodeEnabled = false;
            }
            BindCatTransactionValues();

            //Marketing Map module
            if (string.IsNullOrEmpty(myProductSubscription.OrigsSrc))
            {
                myProductSubscription.OrigsSrc = myProductSubscription.SubscriberSourceCode;
            }

            //AdHocs
            myProductSubscription.AdHocFields = _productSubBO.Get_AdHocs(myProductSubscription.PubID, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);

           List<string> adhocsforpub = _productSubBO.Get_AdHocs(myProductSubscription.PubID,  myClient.ClientConnections);
            
            if (myProductSubscription.AdHocFields != null)
            {
                List<string> missing = adhocsforpub.Except(myProductSubscription.AdHocFields.Select(x => x.AdHocField)).ToList();
                missing.ForEach(x => myProductSubscription.AdHocFields.Add(new FrameworkUAD.Object.PubSubscriptionAdHoc(x, "")));
             
            }

            //Response New Module

            if (myProductSubscription.ProductMapList != null)
                _productResponseList = DeepCopy(myProductSubscription.ProductMapList);
            else
                _productResponseList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();

            questions = _responseGroupBO.Select(MyProduct.PubID, myClient.ClientConnections);
            if (questions != null)
            {
                questions = questions.Where(x => x.IsActive == true && (!x.ResponseGroupName.Equals("Pubcode", StringComparison.CurrentCultureIgnoreCase) || !x.DisplayName.Equals("Pubcode", StringComparison.CurrentCultureIgnoreCase))
                   && (x.ResponseGroupTypeId == CommonList.codeList.SingleOrDefault(r => r.CodeName.Equals(FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_and_UAD.ToString().Replace("_", " "))).CodeId ||
                    x.ResponseGroupTypeId == CommonList.codeList.SingleOrDefault(r => r.CodeName.Equals(FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_Only.ToString().Replace("_", " "))).CodeId))
                   .OrderBy(y => y.DisplayOrder).ThenBy(z => z.DisplayName).ToList();
            }

            var responselist = _codeSheetBO.Select(MyProduct.PubID, myClient.ClientConnections);

           
            if (responselist!=null  && responselist.Count > 0)
            {
                answers = responselist.Where(x => x.IsActive == true).ToList();
            }
            if (_productResponseList == null || _productResponseList.Count == 0)
            {
                List<FrameworkUAD.Entity.ProductSubscriptionDetail> psdResponse = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                psdResponse = productSubDetailWorker.Select(myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
                if (psdResponse != null)
                {
                    //_productResponseList = DeepCopy(myProductSubscription.ProductMapList);
                    #region Validate List to ensure there is only one instance of each ResponseGroupID
                    List<FrameworkUAD.Entity.ProductSubscriptionDetail> deletes = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                    foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd in _productResponseList)
                    {
                        int rGroupID = answers.Where(x => x.CodeSheetID == psd.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
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
                    bool? isMulti = questions.Where(x => x.ResponseGroupID == rGroupID).Select(x => x.IsMultipleValue).FirstOrDefault();
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
            //Selected Answers Codes
            foreach (FrameworkUAD.Entity.ResponseGroup rg in questions)
            {
                List<Answer> a = new List<Answer>();
                List<Answer> selectedAnswers = new List<Answer>();
                List<FrameworkUAD.Entity.CodeSheet> cs = answers.Where(x => x.ResponseGroupID == rg.ResponseGroupID).OrderBy(x => x.ResponseValue).ToList();
                string other = "";
                foreach (FrameworkUAD.Entity.CodeSheet c in cs)
                {
                    FrameworkUAD.Entity.ProductSubscriptionDetail psd = _productResponseList.Where(x => x.CodeSheetID == c.CodeSheetID).FirstOrDefault();
                    if (psd != null)
                        other = (psd.ResponseOther ?? "");
                    if (_productResponseList.Select(x => x.CodeSheetID).Contains(c.CodeSheetID))
                    {
                        selectedAnswers.Add(new Answer(c.CodeSheetID, c.PubID, c.ResponseDesc, c.ResponseValue, c.ResponseGroupID, c.DisplayOrder, c.IsActive, c.IsOther, true, psd.DateCreated));
                        a.Add(new Answer(c.CodeSheetID, c.PubID, c.ResponseDesc, c.ResponseValue, c.ResponseGroupID, c.DisplayOrder, c.IsActive, c.IsOther, true, psd.DateCreated));
                    }
                    else
                    {
                        a.Add(new Answer(c.CodeSheetID, c.PubID, c.ResponseDesc, c.ResponseValue, c.ResponseGroupID, c.DisplayOrder, c.IsActive, c.IsOther, false, c.DateCreated));
                    }
                }
                if (a != null && a.Count > 0)
                    questionCollection.Add(new Question(rg.DisplayName, rg.ResponseGroupID, a, selectedAnswers, (rg.IsRequired ?? false), (rg.IsMultipleValue ?? false), other));
            }

            _productResponseList.ForEach(x => { if (x.ResponseOther == null) x.ResponseOther = ""; });

            //Add Pubcode To ResponseGroup


            subVM.QuestionList = questionCollection;

            //Load Batch History Module.
            BatchNew his = new BatchNew(questions, answers, myClient, MyProduct, myProductSubscription, CommonList);
            subVM.BatchHistoryList = his.GetHistoryList(myProductSubscription);

            //Setup Paid Functions

            SetUpPaidFunctions();


        }
        private void BindCatTransactionValues()
        {
            //Bind Category Types
            FrameworkUAD_Lookup.Entity.Action soloAction = new FrameworkUAD_Lookup.Entity.Action();
            if (myProductSubscription.PubTransactionID > 0)
            {
                if (myProductSubscription.IsNewSubscription == false)
                {
                    int codeTypeId = CommonList.codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Action.ToString())).CodeTypeId;
                    soloAction = CommonList.actionList.SingleOrDefault(a => a.CategoryCodeID == myProductSubscription.PubCategoryID && a.TransactionCodeID == myProductSubscription.PubTransactionID && a.ActionTypeID == CommonList.codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId && x.CodeName.Equals(FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace("_", " "))).CodeId);
                }

                if (soloAction != null && myProductSubscription.IsNewSubscription == false)
                {
                    int catCodeType = -1;
                    catCodeType = CommonList.categoryCodeList.Where(s => s.CategoryCodeID == soloAction.CategoryCodeID).Select(ct => ct.CategoryCodeTypeID).SingleOrDefault();

                    //Ctagory Types
                    subVM.CategoryCodeTypeID = catCodeType;

                    int freePOHold = CommonList.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Free_PO_Hold.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int freeRequest = CommonList.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Free_Request.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int paidRefund = CommonList.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Paid_Refund.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();
                    int paidPOHold = CommonList.transCodeList.Where(t => t.TransactionCodeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCode.Paid_PO_Hold.ToString().Replace("_", " "))).Select(c => c.TransactionCodeID).SingleOrDefault();

                    if (soloAction.TransactionCodeID == freePOHold || soloAction.TransactionCodeID == paidPOHold)
                    {
                        subVM.btnPOKillChecked = true;
                    }
                    else if (string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf) && (soloAction.TransactionCodeID == paidRefund || soloAction.TransactionCodeID == freeRequest))
                    {
                        subVM.btnPersonKillChecked = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(myProductSubscription.OnBehalfOf) && (soloAction.TransactionCodeID == paidRefund || soloAction.TransactionCodeID == freeRequest))
                    {
                        subVM.btnOnBehalfKillChecked = true;
                    }

                }
            }

        }
        private void SetUpPaidFunctions()
        {
            if (myClient.HasPaid == true || myProductSubscription.IsNewSubscription == true)
            {
                FrameworkUAD.Entity.PaidBillTo pbt = new FrameworkUAD.Entity.PaidBillTo();
                myProductSubscriptionPaid = new FrameworkUAD.Entity.SubscriptionPaid();
                _subPaidBO = new FrameworkUAD.BusinessLogic.SubscriptionPaid();
                _subPaidBillTo = new FrameworkUAD.BusinessLogic.PaidBillTo();
                if (myProductSubscription.IsNewSubscription == false)
                {
                    myProductSubscriptionPaid = _subPaidBO.Select(myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
                    if (myProductSubscriptionPaid != null)
                    {
                        pbt = _subPaidBillTo.Select(myProductSubscriptionPaid.SubscriptionPaidID, myClient.ClientConnections);
                        if (pbt == null)
                        {
                            pbt = new FrameworkUAD.Entity.PaidBillTo();
                        }
                    }
                    else
                    {
                        myProductSubscriptionPaid = new FrameworkUAD.Entity.SubscriptionPaid();
                        pbt = new FrameworkUAD.Entity.PaidBillTo();
                    }
                }
                if (myProductSubscriptionPaid.SubscriptionPaidID == 0 || myProductSubscription.IsNewSubscription == true)
                {
                    
                    myProductSubscriptionPaid.CreatedByUserID = currentuser.UserID;
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
                if (pbt.PaidBillToID == 0)
                    pbt = new FrameworkUAD.Entity.PaidBillTo();

                myPaidBillTo = pbt;

                subVM.MySubscriptionPaid = myProductSubscriptionPaid;
                subVM.MyPaidBillTo = myPaidBillTo;

            }
        }
        private void ReLoadProduct()
        {
            FrameworkUAD.BusinessLogic.Product _productBO = new FrameworkUAD.BusinessLogic.Product();
            if (this.MyProduct != null)
            {
                FrameworkUAD.Entity.Product pub = _productBO.Select(this.MyProduct.PubID, myClient.ClientConnections);
                if (pub != null)
                {
                    this.MyProduct = pub;
                }

            }
        }
        internal SubscriberViewModel SaveSubscription(SubscriberViewModel model)
        {
            subVM = model;
            subVM.ClientName = myClient.ClientName;
            int result = 0;
            if (!ValidateSubscriber())
            {
                return subVM;
            }
            else
            {
                ReLoadProduct();
                if (MyProduct.AllowDataEntry == true)
                {
                    result = SaveNew();
                    if (result > 0)
                    {
                        subVM.ErrorList.Add("Success_Complete", "Record has been updated successfully.");
                    }
                    
                }
                else
                {
                    subVM.ErrorList.Add("Error_DataEntryLocked", "This publication is currently locked to process lists. Data can not be saved.");
                    return subVM;
                }
            }

            return subVM;
        }

        private int SaveNew()
        {
            var appName = string.Empty;
            _productSubBO = new FrameworkUAD.BusinessLogic.ProductSubscription();

            try
            {
                var application = currentuser.CurrentClientGroup.SecurityGroups
                    .SelectMany(sg => sg.Services)
                    .SelectMany(s => s.Applications)
                    .FirstOrDefault(a => a.ApplicationName.Contains(Enums.Applications.Circulation.ToString()));

                if (application != null)
                {
                    applicationID = application.ApplicationID;
                    appName = application.ApplicationName;
                }

                return FullSaveSubscription();
            }
            catch (Exception ex)
            {
                var worker = new KMPlatform.BusinessLogic.ApplicationLog();
                var accessKey = currentuser.AccessKey;
                var app = KmEnums.GetApplication(appName);
                var logClientId = myClient.ClientID;
                var formatException = StringFunctions.FormatException(ex);
                worker.LogCriticalError(accessKey.ToString(), formatException, app, $"{GetType().Name}.SaveNew", logClientId);
                subVM.ErrorList.Add("Error_SavingIssue", "There was a problem saving your subscriber. Please try again.");
                return SaveErrorResult;
            }
        }

        private int FullSaveSubscription()
        {
            var userLogType = GetUserLogType();
            var userLogTypeId = CommonList.codeList
                .First(c => c.CodeName.Equals(userLogType.ToString()))
                .CodeId;

            if (!myProductSubscription.IsNewSubscription && originalSubscription.IsInActiveWaveMailing)
            {
                CompareSubscriber();
            }

            if (subVM.PubSubscription.AdHocFields != null)
            {
                myProductSubscription.AdHocFields = subVM.PubSubscription.AdHocFields;
            }

            var containsNotAllowedChars = myProductSubscription.AdHocFields
                .Where(f => f.Value != null)
                .Any(f => f.Value.Contains("<") || f.Value.Contains("&") || f.Value.Contains("'"));

            if (containsNotAllowedChars)
            {
                subVM.ErrorList.Add("Error_SpecialChar", "Special Character(s) not allowed: & < '");
                return SaveErrorResult;
            }

            FillSubscriberAddress();

            var madeResponseChange = GetMadeResponseChange();

            myProductSubscriptionPaid = subVM.MySubscriptionPaid ?? new SubscriptionPaid();
            myPaidBillTo = subVM.MyPaidBillTo ?? new PaidBillTo();

            var batch = GetBatch();

            return DoFullSave(userLogType, userLogTypeId, batch, madeResponseChange);
        }

        private int DoFullSave(KmEnums.UserLogTypes userLogType, int userLogTypeId, Batch batch, bool madeResponseChange)
        {
            var result = _productSubBO.FullSave(
                myProductSubscription,
                originalSubscription,
                saveWaveMailing,
                applicationID,
                userLogType,
                userLogTypeId,
                batch,
                myClient.ClientID,
                madeResponseChange,
                subVM.MadePaidChange,
                subVM.MadePaidBillToChange,
                myProductSubscription.ProductMapList,
                waveMailSubscriber,
                myWMDetail,
                subVM.MySubscriptionPaid,
                subVM.MyPaidBillTo,
                myProductSubscription.ProductMapList);

            if (result != SaveErrorResult)
            {
                subVM.ErrorList = new Dictionary<string, string>();
                subVM.PubSubscription.PubSubscriptionID = result;
            }
            else
            {
                subVM.ErrorList.Add("Error_SavingIssue", "There was a problem saving your subscriber. Please try again.");
            }

            return result;
        }

        private void FillSubscriberAddress()
        {
            const int unitedStatesCountryId = 1;
            const int defaultId = 0;

            if (myProductSubscription.CountryID == defaultId)
            {
                myProductSubscription.CountryID = unitedStatesCountryId;
                myProductSubscription.Country = "UNITED STATES";
            }
            else
            {
                var country = new Country().Select().FirstOrDefault(c => c.CountryID == myProductSubscription.CountryID);
                myProductSubscription.Country = country?.ShortName;
                myProductSubscription.PhoneCode = country?.PhonePrefix ?? defaultId;
            }

            if (myProductSubscription.RegionID != defaultId)
            {
                var regionsList = new Region().Select();
                myProductSubscription.RegionCode = regionsList.FirstOrDefault(r => r.RegionID == myProductSubscription.RegionID)?.RegionCode;
            }

            myProductSubscription.FullName = $"{myProductSubscription.FirstName} {myProductSubscription.LastName}";

            myProductSubscription.ZipCode = string.Empty;
            myProductSubscription.Plus4 = string.Empty;

            var fullZip = myProductSubscription.FullZip
                .Replace("_", "")
                .Replace("-", "")
                .Replace(" ", "");

            if (!string.IsNullOrWhiteSpace(myProductSubscription.FullZip))
            {
                if (myProductSubscription.CountryID == unitedStatesCountryId)
                {
                    const int shortZipLength = 5;

                    myProductSubscription.ZipCode = string.Concat(fullZip.Take(shortZipLength));
                    myProductSubscription.Plus4 = string.Concat(fullZip.Skip(shortZipLength));                    
                }
                else
                {
                    myProductSubscription.ZipCode = fullZip;
                }
            }
            
            myProductSubscription.FullAddress = $"{myProductSubscription.Address1}, {myProductSubscription.Address2}, {myProductSubscription.Address3}, {myProductSubscription.City}, {myProductSubscription.RegionCode} {myProductSubscription.FullZip}, {myProductSubscription.Country}";
        }

        private bool GetMadeResponseChange()
        {
            var madeResponseChange = subVM.MadeResponseChange;
            if (!madeResponseChange || subVM.ProductResponseList == null)
            {
                return madeResponseChange;
            }

            myProductSubscription.ProductMapList = subVM.ProductResponseList;
            var responses = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(MyProduct.PubID, myClient.ClientConnections);
            if (responses != null)
            {
                var pubcodeResponseGroupId = responses
                    .Where(t => t.ResponseGroupName.Equals("PUBCODE", StringComparison.InvariantCultureIgnoreCase))
                    .Select(t => t.ResponseGroupID)
                    .FirstOrDefault();

                var codesheets = new FrameworkUAD.BusinessLogic.CodeSheet().Select(MyProduct.PubID, myClient.ClientConnections);
                var pubcodeSheetId = codesheets.Where(t => t.ResponseGroupID == pubcodeResponseGroupId)
                    .Select(t => t.CodeSheetID).FirstOrDefault();
                if (pubcodeSheetId != 0)
                    myProductSubscription.ProductMapList.Add(new ProductSubscriptionDetail()
                    {
                        CodeSheetID = pubcodeSheetId,
                        DateCreated = (DateTime) myProductSubscription.QualificationDate,
                        PubSubscriptionID = myProductSubscription.PubSubscriptionID,
                        SubscriptionID = myProductSubscription.SubscriptionID,
                        CreatedByUserID = myProductSubscription.CreatedByUserID,
                        ResponseOther = string.Empty
                    });
            }

            foreach (var response in subVM.ProductResponseList.Where(r => r.ResponseOther != null))
            {
                if (response.ResponseOther.Contains("&") || response.ResponseOther.Contains("<"))
                {
                    subVM.ErrorList.Add("Error_SpecialChar", "Special Character(s) not allowed: & <");
                }
            }

            return madeResponseChange;
        }

        private Batch GetBatch()
        {            
            var clientConnections = new ClientConnections(myClient);
            var batchBusinessObject = new FrameworkUAD.BusinessLogic.Batch()
                .Select(clientConnections)
                .FirstOrDefault(x => x.PublicationID == myProductSubscription.PubID && x.UserID == currentuser.UserID && x.IsActive);

            var batch = new Batch();
            if (batchBusinessObject != null)
            {
                batch.BatchCount = batchBusinessObject.BatchCount;
                batch.BatchID = batchBusinessObject.BatchID;
                batch.BatchNumber = batchBusinessObject.BatchNumber;
                batch.DateCreated = batchBusinessObject.DateCreated;
                batch.DateFinalized = batchBusinessObject.DateFinalized;
                batch.IsActive = batchBusinessObject.IsActive;
                batch.PublicationID = batchBusinessObject.PublicationID;
                batch.UserID = batchBusinessObject.UserID;
            }

            return batch;
        }

        private KmEnums.UserLogTypes GetUserLogType()
        {
            if (myProductSubscription.IsNewSubscription)
            {
                myProductSubscription.DateCreated = DateTime.Now;
                myProductSubscription.CreatedByUserID = currentuser.UserID;
                return KmEnums.UserLogTypes.Add;
            }
            else
            {
                myProductSubscription.DateUpdated = DateTime.Now;
                myProductSubscription.UpdatedByUserID = currentuser.UserID;
                return KmEnums.UserLogTypes.Edit;
            }
        }

        private bool ValidateSubscriber()
        {
            sStatus = new FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix();
            if (myProductSubscription.IsNewSubscription == true)
            {
                if (myProductSubscription.IsPaid == true)
                {
                    FrameworkUAD_Lookup.Entity.TransactionCode tc = this.CommonList.transCodeList.Where(x => x.TransactionCodeValue == 13 && x.TransactionCodeTypeID == 3).FirstOrDefault();
                    if (tc != null)
                        myProductSubscription.PubTransactionID = tc.TransactionCodeID;
                }
                else
                {
                    FrameworkUAD_Lookup.Entity.TransactionCode tc = this.CommonList.transCodeList.Where(x => x.TransactionCodeValue == 10 && x.TransactionCodeTypeID == 1).FirstOrDefault();
                    if (tc != null)
                        myProductSubscription.PubTransactionID = tc.TransactionCodeID;
                }

                if (myProductSubscription.QualificationDate == null)
                {
                    subVM.ErrorList.Add("Error_EmptyQualDate", "Please select a qualification date.");
                    return false;
                }
            }
            if (myProductSubscription.PubCategoryID > 0 && myProductSubscription.PubTransactionID > 0)
            {
                int codeTypeId = this.CommonList.codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Action.ToString())).CodeTypeId;
                action = this.CommonList.actionList.SingleOrDefault(a => a.CategoryCodeID == myProductSubscription.PubCategoryID && a.TransactionCodeID == myProductSubscription.PubTransactionID &&
                        a.ActionTypeID == this.CommonList.codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId &&
                        x.CodeName.Equals(FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace("_", " "))).CodeId);
                if (action != null)
                {

                    sStatus = this.CommonList.ssmList.Where(s => s.CategoryCodeID == action.CategoryCodeID && s.TransactionCodeID == action.TransactionCodeID && s.IsActive == true &&
                        s.SubscriptionStatusID == myProductSubscription.SubscriptionStatusID).FirstOrDefault();
                    if (sStatus == null)
                    {

                        subVM.ErrorList.Add("Error_TCCSS", "This combination of Transaction/Category Codes and Subscription Status are not valid. Subscriber could not be saved.");
                        return false;
                    }
                }
                else
                {
                    subVM.ErrorList.Add("Error_TCC1", "This combination of Transaction and Category Codes are not valid. Subscriber could not be saved.");
                    return false;
                }
            }
            else
            {
                subVM.ErrorList.Add("Error_TCC1", "This combination of Transaction and Category Codes are not valid. Subscriber could not be saved.");
                return false;
            }

            return true;
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
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> DeepCopy(List<FrameworkUAD.Entity.ProductSubscriptionDetail> list)
        {
            List<FrameworkUAD.Entity.ProductSubscriptionDetail> copy = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
            foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd in list)
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

        
    }
}