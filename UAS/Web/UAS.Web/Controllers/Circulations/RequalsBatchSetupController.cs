using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAS.Web.Controllers.Common;
using UAS.Web.Models.Circulations;

namespace UAS.Web.Controllers.Circulations
{
    public class RequalsBatchSetupController : BaseController
    {
        #region Public Action Methods
        public ActionResult Index(int PubID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                if (PubID > 0)
                {
                    var Product = new FrameworkUAD.BusinessLogic.Product().Select(PubID, CurrentClient.ClientConnections);
                    if (Product.AllowDataEntry)
                    {
                        RequalBatchDetailsViewModel RqBatchVM = new RequalBatchDetailsViewModel();
                        RqBatchVM.ProductID = PubID;
                        RqBatchVM.ProductList = GetProductList();
                        RqBatchVM.Par3CList = GetPar3CList();
                        RqBatchVM.QSourceList = GetQSourceList();
                        RqBatchVM.ResponseGroupList = GetProductResponseGroups(PubID);
                        return View(RqBatchVM);
                    }
                    return RedirectToAction("Error", "Error", new { errorType = "DataEntryNotAllowed"});
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { errorType = "ProductEmpty" });
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult GetSubscriberRecord(int PubID, int SeqNumber)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                var pubsub = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectSequenceIDPubID(SeqNumber, PubID, CurrentClient.ClientConnections);
                if (pubsub != null)
                {
                    if (pubsub.SubscriptionStatusID == 1 || pubsub.SubscriptionStatusID == 3 || pubsub.SubscriptionStatusID == 4)
                    {
                        DateTime dateOnly = pubsub.QualificationDate.HasValue? (DateTime) pubsub.QualificationDate:new DateTime();
                        var date = dateOnly.Date;
                        List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select();
                        List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypes = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select();
                        var CatCode = catCodes.Where(x => x.CategoryCodeID == pubsub.PubCategoryID).First();
                        var CatType = catTypes.Where(x => x.CategoryCodeTypeID == CatCode.CategoryCodeTypeID).First();

                        if (CatType.CategoryCodeTypeName == "Qualified Paid" || CatType.CategoryCodeTypeName == "NonQualified Paid")
                            return Json(new { error = true, errormessage = "Subscriber is Paid and not allowed to update from Quick Data Entry." }, JsonRequestBehavior.AllowGet);
                        else
                        {
                            string phoneext = string.IsNullOrEmpty(pubsub.PhoneExt) ? "" : " X " + pubsub.PhoneExt;
                            string phone = "("+pubsub.Phone.Substring(0,3)+")-"+ pubsub.Phone.Substring(3,3)+"-"+ pubsub.Phone.Substring(6, 4);
                            string dd = dateOnly.Day.ToString();
                            string mm = dateOnly.Month.ToString();
                            string yyyy = dateOnly.Year.ToString();
                            string Details = pubsub.FirstName + " " + pubsub.LastName + "<br/>" +
                                             pubsub.Title + "<br/>" +
                                             pubsub.Company + "<br/>" +
                                             pubsub.Address1 + "<br/>" +
                                             pubsub.City + ", " + pubsub.RegionCode + "  " + pubsub.ZipCode + "-" + pubsub.Plus4 + "<br/>" +
                                             "Email: " + pubsub.Email + "<br/>" +
                                             "Phone: " + phone + phoneext;
                            return Json(new { error = false, Details= Details, PubSubscriptionID = pubsub.PubSubscriptionID, SubscriptionID = pubsub.SubscriptionID, QualDate = date.ToString("MM/dd/yyyy"), DD=dd, MM=mm, YYYY=yyyy }, JsonRequestBehavior.AllowGet);
                        }
                            

                    }
                    else
                        return Json(new { error = true, errormessage = "Subscriber is inactive or not subscribed." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = true, errormessage = "Subscriber not found. Please enter another sequence#." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult SaveResponses(RequalBatchDetailsViewModel RqBatchVM)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                int PubSubID = RqBatchVM.PubSubscriptionID;
                #region Create Current and Modified PubSub
                FrameworkUAD.Entity.ProductSubscription OriginalPubSub = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectProductSubscription(PubSubID, CurrentClient.ClientConnections, CurrentClient.ClientName);
                FrameworkUAD.Entity.ProductSubscription CurrentPubSub = new FrameworkUAD.Entity.ProductSubscription(OriginalPubSub);
                CurrentPubSub.ProductMapList = new FrameworkUAD.BusinessLogic.ProductSubscriptionDetail().Select(PubSubID, CurrentClient.ClientConnections);
                var responseGroupIds = RqBatchVM.PubSubDetails.Select(x => x.ResponseGroupID);
                //Loo over all responsegroup IDs
                foreach (var rgid in responseGroupIds)
                {
                    FrameworkUAD.Entity.ResponseGroup rg = new FrameworkUAD.BusinessLogic.ResponseGroup().SelectByID(rgid, CurrentClient.ClientConnections);
                    bool IsMultipleAllowed = rg.IsMultipleValue.HasValue ? (bool) rg.IsMultipleValue : false;
                    var CurrentPubSubCodeSheetIDs = CurrentPubSub.ProductMapList.Select(x => x.CodeSheetID);
                    var modifiedCodeSheets = RqBatchVM.PubSubDetails.Where(x => x.ResponseGroupID == rgid);
                    var CodeSheetIDs = new FrameworkUAD.BusinessLogic.CodeSheet().SelectByResponseGroupID(rg.ResponseGroupID, CurrentClient.ClientConnections).Select(x => x.CodeSheetID);
                    foreach (int i in CodeSheetIDs)
                    {
                        var item = CurrentPubSub.ProductMapList.Where(x => x.CodeSheetID == i).FirstOrDefault();
                        if (item != null)
                            CurrentPubSub.ProductMapList.Remove(item);
                    }
                    foreach (var psdvm in modifiedCodeSheets)
                    {
                        if (IsMultipleAllowed)
                        {
                            if (psdvm.DemoChecked) //If Demochecked Remove existing ITem with Codesheetid and Add new Item
                            {
                                CurrentPubSub.ProductMapList.Add(new FrameworkUAD.Entity.ProductSubscriptionDetail()
                                {
                                    PubSubscriptionID = psdvm.PubSubscriptionID,
                                    SubscriptionID = psdvm.SubscriptionID,
                                    CodeSheetID = psdvm.CodeSheetID,
                                    ResponseOther = psdvm.ResponseOther,
                                    DateCreated = psdvm.DateCreated,
                                    CreatedByUserID = CurrentUser.UserID
                                });
                            }
                            else if (CurrentPubSubCodeSheetIDs.Contains(psdvm.CodeSheetID)) //If Demo is not checked but PubSub already has entry but the other response changed
                            {
                                //If response other changed
                                var selectcurrentproductMap = CurrentPubSub.ProductMapList.Where(x => x.CodeSheetID == psdvm.CodeSheetID).First();
                                if (!string.IsNullOrEmpty(psdvm.ResponseOther) && !psdvm.ResponseOther.Equals(selectcurrentproductMap.ResponseOther, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    CurrentPubSub.ProductMapList.Where(x => x.CodeSheetID == psdvm.CodeSheetID).First().ResponseOther = psdvm.ResponseOther;
                                    CurrentPubSub.ProductMapList.Where(x => x.CodeSheetID == psdvm.CodeSheetID).First().CreatedByUserID = CurrentUser.UserID;
                                }
                            }
                            else //If Codesheet ID is not present 
                            {
                                CurrentPubSub.ProductMapList.Add(new FrameworkUAD.Entity.ProductSubscriptionDetail()
                                {
                                    PubSubscriptionID = psdvm.PubSubscriptionID,
                                    SubscriptionID = psdvm.SubscriptionID,
                                    CodeSheetID = psdvm.CodeSheetID,
                                    ResponseOther = psdvm.ResponseOther,
                                    DateCreated = psdvm.DateCreated,
                                    CreatedByUserID = CurrentUser.UserID
                                });
                            }

                        }
                        else
                        {
                             if (psdvm.DemoChecked) //If Demochecked Remove existing ITem with Codesheetid and Add new Item
                            {
                                CurrentPubSub.ProductMapList.Add(new FrameworkUAD.Entity.ProductSubscriptionDetail()
                                {
                                    PubSubscriptionID = psdvm.PubSubscriptionID,
                                    SubscriptionID = psdvm.SubscriptionID,
                                    CodeSheetID = psdvm.CodeSheetID,
                                    ResponseOther = psdvm.ResponseOther,
                                    DateCreated = psdvm.DateCreated,
                                    CreatedByUserID = CurrentUser.UserID
                                });
                            }
                            else if (CurrentPubSubCodeSheetIDs.Contains(psdvm.CodeSheetID))
                            {
                                //If response other changed
                                CurrentPubSub.ProductMapList.RemoveAll(x => CodeSheetIDs.Contains(x.CodeSheetID) && x.CodeSheetID != psdvm.CodeSheetID);
                                var selectcurrentproductMap = CurrentPubSub.ProductMapList.Where(x => x.CodeSheetID == psdvm.CodeSheetID).First();
                                if (!string.IsNullOrEmpty(psdvm.ResponseOther) && !psdvm.ResponseOther.Equals(selectcurrentproductMap.ResponseOther, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    CurrentPubSub.ProductMapList.Where(x => x.CodeSheetID == psdvm.CodeSheetID).First().ResponseOther = psdvm.ResponseOther;
                                    CurrentPubSub.ProductMapList.Where(x => x.CodeSheetID == psdvm.CodeSheetID).First().CreatedByUserID = CurrentUser.UserID;
                                }
                            }
                        }

                    }
                }
                CurrentPubSub.Par3CID = RqBatchVM.Par3CID;
                CurrentPubSub.PubQSourceID = RqBatchVM.QSourceID;
                CurrentPubSub.SubscriberSourceCode = string.IsNullOrEmpty(RqBatchVM.SubSrc) ? CurrentPubSub.OrigsSrc : RqBatchVM.SubSrc;
                CurrentPubSub.QualificationDate = RqBatchVM.QDate;
                CurrentPubSub.DateUpdated = DateTime.Now;
                CurrentPubSub.UpdatedByUserID = CurrentUser.UserID;

                List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select();
                List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypes = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select();
                FrameworkUAD_Lookup.Entity.TransactionCode transaction = new FrameworkUAD_Lookup.Entity.TransactionCode();
                var CatCode = catCodes.Where(x => x.CategoryCodeID == CurrentPubSub.PubCategoryID).First();
                var CatType = catTypes.Where(x => x.CategoryCodeTypeID == CatCode.CategoryCodeTypeID).First();
                if (CatCode.CategoryCodeValue != 70 && CatCode.CategoryCodeValue != 71)
                {
                    if (CatType.CategoryCodeTypeName == "Qualified Free" || CatType.CategoryCodeTypeName == "NonQualified Free")
                    {
                        transaction = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().SelectActiveIsFree(true).Where(x => x.TransactionCodeValue == 22).First();
                        CurrentPubSub.PubTransactionID = transaction.TransactionCodeID;
                    }
                    else if (CatType.CategoryCodeTypeName == "Qualified Paid" || CatType.CategoryCodeTypeName == "NonQualified Paid")
                    {
                        transaction = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().SelectActiveIsFree(false).Where(x => x.TransactionCodeValue == 22).First();
                        CurrentPubSub.PubTransactionID = transaction.TransactionCodeID;
                    }
                }
               
                bool saveWaveMailing = CurrentPubSub.IsInActiveWaveMailing;
                #endregion
                #region Compare Subscriptions for Wave mailing
                var myWMDetail = new FrameworkUAD.Entity.WaveMailingDetail();
                var waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription(CurrentPubSub);
                if (saveWaveMailing)
                {
                    myWMDetail.PubSubscriptionID = OriginalPubSub.PubSubscriptionID;
                    myWMDetail.SubscriptionID = OriginalPubSub.SubscriptionID;
                    myWMDetail.WaveMailingID = OriginalPubSub.WaveMailingID;
                    waveMailSubscriber = new FrameworkUAD.Entity.ProductSubscription(CurrentPubSub);
                    if (OriginalPubSub.IsInActiveWaveMailing == true)
                    {
                        OriginalPubSub.IsInActiveWaveMailing = true;
                    }
                }
                #endregion
                #region User Log Type 
                KMPlatform.BusinessLogic.Enums.UserLogTypes ult = KMPlatform.BusinessLogic.Enums.UserLogTypes.Edit;
                int userLogTypeId = new FrameworkUAD_Lookup.BusinessLogic.Code().Select().FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.UserLogTypes.Edit.ToString())).CodeId;
                #endregion

                #region Application
                KMPlatform.Entity.Application app = new KMPlatform.BusinessLogic.Application().Select().Where(x => x.ApplicationName == "AMSCircMVC").FirstOrDefault();
                #endregion

                #region Batch Creation
                KMPlatform.Object.ClientConnections clientObj = new KMPlatform.Object.ClientConnections(CurrentClient);
                FrameworkUAD.Entity.Batch batch = new FrameworkUAD.BusinessLogic.Batch().Select(clientObj)
                    .Where(x => x.PublicationID == CurrentPubSub.PubID && x.UserID == CurrentUser.UserID && x.IsActive == true)
                    .FirstOrDefault();
                FrameworkUAS.Object.Batch b = new FrameworkUAS.Object.Batch();
                if (batch != null)
                {
                    b.BatchCount = batch.BatchCount;
                    b.BatchID = batch.BatchID;
                    b.BatchNumber = batch.BatchNumber;
                    b.DateCreated = batch.DateCreated;
                    b.DateFinalized = batch.DateFinalized;
                    b.IsActive = batch.IsActive;
                    b.PublicationID = batch.PublicationID;
                    b.UserID = batch.UserID;
                }
                #endregion

                int result = 0;
                result = new FrameworkUAD.BusinessLogic.ProductSubscription().FullSave(CurrentPubSub, OriginalPubSub, saveWaveMailing, app.ApplicationID, ult, userLogTypeId, b, CurrentClient.ClientID, true, false, false, CurrentPubSub.ProductMapList,waveMailSubscriber,myWMDetail, new FrameworkUAD.Entity.SubscriptionPaid(), new FrameworkUAD.Entity.PaidBillTo(), CurrentPubSub.ProductMapList);
                //DataTable dtPubSubDetails = CreatePubSubDetailsTable();
                //PubSubDetails.ForEach(x => dtPubSubDetails.Rows.Add(x.PubSubscriptionID, x.SubscriptionID, x.CodeSheetID, x.ResponseOther, x.DateCreated,CurrentUser.UserID));
                //bool savedone = new FrameworkUAD.BusinessLogic.ProductSubscriptionDetail().SaveQuickEntries(CurrentClient.ClientConnections, dtPubSubDetails);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }
        public ActionResult GetBatchEntryForm(RequalBatchDetailsViewModel RqBatchVM)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                var reslist = GetProductResponseGroups(RqBatchVM.ProductID);
                RqBatchVM.ResponseGroupList = new List<RequalDemos>();
                var responses = reslist.Where(x => RqBatchVM.SelectedResponseGroups.Contains(x.Value)).ToList();
                foreach(var r in RqBatchVM.SelectedResponseGroups)
                {
                    RqBatchVM.ResponseGroupList.Add(responses.Where(x => x.Value == r).First());
                }
                return PartialView("_requalsBatchEntry", RqBatchVM);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult GetResponseGroupCodeSheetAjax(int rgId,List<string> codeSheetIDs)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                 if (rgId > 0 && codeSheetIDs.Count > 0)
                 {
                    List<FrameworkUAD.Entity.CodeSheet> mastercodesheet = new List<FrameworkUAD.Entity.CodeSheet>();
                    var responsGroup = new FrameworkUAD.BusinessLogic.ResponseGroup().SelectByID(rgId, CurrentClient.ClientConnections);
                    mastercodesheet = new FrameworkUAD.BusinessLogic.CodeSheet().SelectByResponseGroupID(rgId, CurrentClient.ClientConnections);
                    bool IsMultiple = responsGroup.IsMultipleValue.HasValue ? (bool) responsGroup.IsMultipleValue : false;
                    var responsesAllowed = new List<int>();
                    if (IsMultiple)
                    {
                        mastercodesheet = mastercodesheet.Where(x => codeSheetIDs.Contains(x.ResponseValue)).ToList();
                    }
                    else
                    {
                        mastercodesheet = mastercodesheet.Where(x => x.ResponseValue.Equals(codeSheetIDs.First(),StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    List<dynamic> selectList = new List<dynamic>();
                    mastercodesheet.Where(x=>x.IsActive==true).ToList().ForEach(c => selectList.Add(new { Text = c.ResponseValue+". " + c.ResponseDesc, Value = c.CodeSheetID.ToString() , IsOther = c.IsOther.HasValue ? (bool) c.IsOther : false}));
                    if (selectList.Count > 0)
                    {
                        return Json(selectList, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }
        public ActionResult GetProductResponseGroupsAjax(int PubID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.SRCH, KMPlatform.Enums.Access.FullAccess))
            {
                List<SelectListItem> selectList = new List<SelectListItem>();
                var responseGroup = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(PubID, CurrentClient.ClientConnections).Where(x => x.DisplayName.ToUpper() != "PUBCODE");
                responseGroup.OrderBy(c => c.DisplayOrder).ToList().ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName.ToUpper(), Value = c.ResponseGroupID.ToString() }));
                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
           
        }
        #endregion

        #region Private Methods
        private static DataTable CreatePubSubDetailsTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PubSubscriptionID", typeof(int));
            dt.Columns.Add("SubscriptionID", typeof(int));
            dt.Columns.Add("CodeSheetID", typeof(int));
            dt.Columns.Add("ResponseOther", typeof(string));
            dt.Columns.Add("DateCreated", typeof(DateTime));
            dt.Columns.Add("CreatedByUserID", typeof(int));
            return dt;
        }
        private List<RequalDemos> GetProductResponseGroups(int PubID)
        {
            List<RequalDemos> selectList = new List<RequalDemos>();
            var responseGroup = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(PubID, CurrentClient.ClientConnections).Where(x => x.DisplayName.ToUpper() != "PUBCODE");
            responseGroup.OrderBy(c => c.DisplayOrder).ToList().ForEach(c => selectList.Add(new RequalDemos { Text = c.DisplayName.ToUpper(), Value = c.ResponseGroupID, IsMultiple = c.IsMultipleValue.HasValue ? (bool) c.IsMultipleValue : false, IsRequired=c.IsRequired.HasValue ? (bool) c.IsRequired : false }));
            return selectList;
        }
        private List<SelectListItem> GetProductList()
        {
            List<FrameworkUAD.Entity.Product> prdList = new FrameworkUAD.BusinessLogic.Product().Select(CurrentClient.ClientConnections);
            List<SelectListItem> selectList = new List<SelectListItem>();
            var prdAMS = prdList.Where(x => x.IsCirc == true).OrderBy(x=>x.PubCode).ToList();
            prdAMS.ForEach(c => selectList.Add(new SelectListItem() { Text = c.PubCode.ToUpper(), Value = c.PubID.ToString() }));
            return selectList;
        }
        private List<SelectListItem> GetQSourceList()
        {
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);
            List<SelectListItem> selectList = new List<SelectListItem>();
            codeList.OrderBy(c => c.DisplayOrder).ToList().ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName.ToUpper(), Value = c.CodeId.ToString() }));
            return selectList;
        }
        private  List<SelectListItem> GetPar3CList()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Par3c);
            codeList.OrderBy(c => c.DisplayOrder).ToList().ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName.ToUpper(), Value = c.CodeId.ToString() }));
            return selectList;
        }
        #endregion

    }
}