using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KM.Common.Functions;
using UAS.Web.Controllers.Common;
using UAS.Web.Models.Circulations;

namespace UAS.Web.Controllers.Circulations
{
    public class IssueSplitController : BaseController
    {
        // GET: IssueSplit
        [HttpGet]
        public ActionResult Index(int ProductID = 0)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT) && ProductID > 0)
            {
                FrameworkUAD.Entity.Product product = new FrameworkUAD.Entity.Product();
                if (ProductID > 0)
                {
                    product = new FrameworkUAD.BusinessLogic.Product().Select(ProductID, CurrentClient.ClientConnections);
                }
                int lockedbyproduct = product.UpdatedByUserID.HasValue ? (int) product.UpdatedByUserID : 0;
                bool isOpenClosedLocked = product.IsOpenCloseLocked.HasValue ? (bool) product.IsOpenCloseLocked : false;

                bool isAddRemoveAllowed = product.AddRemoveAllowed.HasValue ? (bool)product.AddRemoveAllowed : false;
                bool isClientImportAllowed = product.ClientImportAllowed.HasValue ? (bool) product.ClientImportAllowed : false;
                bool isAllowDataEntry = product.AllowDataEntry;
                bool isKMImportAllowed = product.KMImportAllowed.HasValue ? (bool) product.ClientImportAllowed : false; 

                if (isOpenClosedLocked && (lockedbyproduct != CurrentUser.UserID) && lockedbyproduct > 0)
                {
                    KMPlatform.Entity.User productlockedby = new KMPlatform.BusinessLogic.User().SelectUser(lockedbyproduct);
                    return RedirectToAction("Error", "Error", new { errorType = "IssueOpenClosedLocked", errorMessage = "IssueSplit is locked by '" + productlockedby.UserName + "'." });
                    //return Json(new { error = true, errormessage = "IssueSplit is locked by '" + productlockedby.UserName + "'." },JsonRequestBehavior.AllowGet);
                }
                else if (isAddRemoveAllowed|| isClientImportAllowed|| isAllowDataEntry|| isKMImportAllowed)
                {
                    return RedirectToAction("Error", "Error", new { errorType = "CloseOther", errorMessage = "Please close Data Entry,Internal Import, External Import and Add Remove  ." });
                }
                else
                {
                    IssueSplitProductViewModel model = new IssueSplitProductViewModel();
                    model.PubID = ProductID;

                    var mailerInfoResponse = new FrameworkUAD.BusinessLogic.AcsMailerInfo().SelectByID(product.AcsMailerInfoId, CurrentClient.ClientConnections);
                    if (mailerInfoResponse != null)
                    {
                        if (mailerInfoResponse.ImbSeqCounter == 0)
                            model.ImbSeqCounter = 1;
                        else
                            model.ImbSeqCounter = mailerInfoResponse.ImbSeqCounter;
                    }

                    var countsResponse = new FrameworkUAD.BusinessLogic.Report().SelectActiveIssueSplitsCounts(model.PubID, CurrentClient.ClientConnections);
                    if (countsResponse != null)
                    {
                        model.RecordCounts = countsResponse;
                        model.QualDict = new Dictionary<int, string>(model.RecordCounts.FreeRecords + model.RecordCounts.PaidRecords);
                        model.ImbSeqs = new Dictionary<int, string>(model.RecordCounts.FreeRecords + model.RecordCounts.PaidRecords);
                        model.CompImbSeqs = new Dictionary<int, string>(model.RecordCounts.FreeRecords + model.RecordCounts.PaidRecords);
                    }
                    var qualResponse = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);
                    if (qualResponse != null && qualResponse.Count > 0)
                    {
                        model.QualDict = qualResponse.ToDictionary(x => x.CodeId, x => x.CodeValue);
                    }
                    Session["myAcsMailerInfo"] = mailerInfoResponse;
                    Session["maxImbID"] = model.ImbSeqCounter;
                    Session["ImbSeqs"] = model.ImbSeqs;
                    Session["CompImbSeqs"] = model.CompImbSeqs;

                    var allissuelist = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);
                    model.IssueList = allissuelist.Where(x => x.PublicationId == ProductID).OrderByDescending(x => x.IssueId).ToList();

                    var currentissue = model.IssueList.Where(x => x.IsComplete == false).First();
                    model.Issue = currentissue;
                    model.IssueID = currentissue.IssueId;

                    List<FrameworkUAD.Entity.WaveMailing> waveMailingList = new List<FrameworkUAD.Entity.WaveMailing>();
                    var allwavelist = new FrameworkUAD.BusinessLogic.WaveMailing().Select(CurrentClient.ClientConnections);
                    if (currentissue != null)
                        waveMailingList = allwavelist.Where(x => x.IssueID == currentissue.IssueId).ToList();
                    model.WaveList = waveMailingList;

                    model.RecordCounts = new FrameworkUAD.BusinessLogic.Report().SelectActiveIssueSplitsCounts(ProductID, CurrentClient.ClientConnections);

                    model = GetSplitsModel(model);

                    int subCount = 0;
                    subCount = model.subscriptionList.Select(x => x.Copies).Sum();
                    ViewBag.RemainingCount = subCount - model.SplitsList.Select(x => x.IssueSplitCount).Sum();
                    if (model.SplitsList.Count > 0)
                        ViewBag.LastModifiedDate = model.lastmodifieddates.Max();
                    else
                        ViewBag.LastModifiedDate = "";
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }
        public ActionResult GetSplits(List<FrameworkUAD.Object.FilterMVC> filterscol, IssueSplitProductViewModel model)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {

                model = GetSplitList(model, filterscol);
                if (model.TempSplitsList == null || model.TempSplitsList.Count == 0 || model.TempSplitsList.Count < model.FilterCollection.Count)
                {
                    return Json(new { error = true, errormessage = "No results to display." });
                }
                else
                {
                    return PartialView("_issueSplitGrid", model.TempSplitsList);
                }

            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult SaveSplits(List<FrameworkUAD.Object.FilterMVC> filterscollection, IssueSplitProductViewModel model)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {
                model = GetSplitList(model, filterscollection);

                return Json(new { error = false, errormessage = "" });
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult GenerteSplitList(List<FrameworkUAD.Object.FilterMVC> filterscollection, IssueSplitProductViewModel model)
        {

            model = GetSplitList(model, filterscollection);
            var activeSubscriptionIDs = new HashSet<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));
            List<IssueSplitContainer> tempSplitsList = new List<IssueSplitContainer>();
            #region Genrate Script
            model.SplitsList.Clear();
            List<int> dictionaryKeys = new List<int>();
            int counter = 0;
            List<DateTime> lastmodifieddates = new List<DateTime>();
            List<IssueSplitContainer> StagingSplitsList = new List<IssueSplitContainer>();
            Dictionary<int, IssueSplitContainer> filterDictionary = new Dictionary<int, IssueSplitContainer>();
            List<int> subPool = new List<int>();
            List<int> totalPool = new List<int>();
            try
            {
                foreach (IssueSplitContainer isc in model.TempSplitsList)
                {
                    filterDictionary.Add(counter, isc);
                    dictionaryKeys.Add(counter);
                    subPool.AddRange(isc.SubscriberIDs);
                    counter++;
                }
                subPool = subPool.Distinct().ToList();
                totalPool = subPool.Distinct().ToList();
                //Finds all possible combinations between a number of objects.
                List<FilterCombinations> combos = GetCombination(dictionaryKeys);
                //Given all possible combinations, we see if there is any intersection between SubscriberIDs in those combinations.
                //Example: With 2 splits, we have 3 possible combinations: 1, 2, 1 + 2
                //Given those combinations, starting with the largest combination (1 + 2), we find all possible SubscriberIDs that intersect and remove them from remaining combinations as we
                //work our way down the list.
                foreach (FilterCombinations fc in combos)
                {
                    List<FrameworkUAD.Object.FilterDetails> filterdetails = new List<FrameworkUAD.Object.FilterDetails>();
                    List<int> subscriberPool = subPool;
                    List<int> subscriberTotalPool = totalPool;
                    String name = "";
                    foreach (int i in fc.Criteria)
                    {
                        IssueSplitContainer ctnr = filterDictionary[i];
                        filterdetails.AddRange(ctnr.Filter.Fields);
                        name += ctnr.IssueSplitName + " + ";
                        subscriberPool = subscriberPool.Intersect(ctnr.SubscriberIDs).ToList();
                        subscriberTotalPool = subscriberTotalPool.Intersect(ctnr.SubscriberIDs).ToList();
                    }
                    name = name.TrimEnd(' ');
                    name = name.TrimEnd('+');
                    name = name.TrimEnd(' ');
                    subPool = subPool.Except(subscriberPool).ToList();
                    if (subscriberPool.Count > 0)
                    {
                        //List<FilterControls.Framework.FilterObject> finalFilters = GetCombinedDetail(filters);
                        filterdetails = GetCombinedDetail(filterdetails);
                        var finalfilter = new FrameworkUAD.Object.FilterMVC();
                        finalfilter.Fields.AddRange(filterdetails);
                        finalfilter.FilterName = name;
                        finalfilter.SubscriberIDs = subscriberPool;
                        FrameworkUAD.Entity.IssueSplit newSplit = new FrameworkUAD.Entity.IssueSplit();
                        //Calculates Copies Count
                        DataTable dtSubIDs = CreateIdTable();
                        finalfilter.SubscriberIDs.ForEach(x => dtSubIDs.Rows.Add(x));
                        int subcount = new FrameworkUAD.BusinessLogic.IssueSplit().GetSubscriberCopiesCount(dtSubIDs, CurrentClient.ClientConnections);

                        //int subcount = model.subscriptionList.Select(x => x.PubSubscriptionID).Intersect(subscriberPool).Count();
                        //int subcount = model.subscriptionList.Where(x => subscriberPool.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();

                        newSplit.IssueSplitCount = subcount;
                        newSplit.IssueSplitRecords = subscriberPool.Count;
                        newSplit.IssueSplitId = 0;
                        newSplit.IsActive = true;
                        newSplit.IssueId = model.IssueID;
                        newSplit.IssueSplitName = name;
                        IssueSplitContainer newContainer = new IssueSplitContainer(newSplit, finalfilter, subscriberPool, model.PubID);
                        newContainer.Save = true;
                        StagingSplitsList.Add(newContainer);
                    }
                }
            }
            catch (Exception ex)
            {
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                return Json(new { error = true, errormessage = formatException });
            }
            new FrameworkUAD.BusinessLogic.IssueSplit().ClearIssue(model.IssueID, CurrentClient.ClientConnections);

            foreach (IssueSplitContainer isc in StagingSplitsList)
            {
                try
                {
                    var f = new FrameworkUAD.Entity.IssueSplitFilter();
                    f.FilterName = isc.Filter.FilterName;
                    f.FilterID = isc.Filter.FilterID;
                    f.PubId = isc.ProductID;
                    f.CreatedByUserID = CurrentUser.UserID;

                    DataTable dtFilterDetails = CreateFilterDetailsTable();
                    isc.Filter.Fields.ForEach(x => dtFilterDetails.Rows.Add(x.Name, x.Values, x.SearchCondition, x.Group, x.Text));
                    isc.Filter.FilterID = new FrameworkUAD.BusinessLogic.IssueSplitFilter().Save(f, dtFilterDetails, CurrentClient.ClientConnections);

                    DataTable dtIssueSplitPubs = CreateIdTable();
                    isc.Filter.SubscriberIDs.ForEach(x => dtIssueSplitPubs.Rows.Add(x));
                    isc.IssueSplit.FilterId = isc.Filter.FilterID;
                    int issuesplitID = new FrameworkUAD.BusinessLogic.IssueSplit().SaveNew(isc.IssueSplit, dtIssueSplitPubs, CurrentClient.ClientConnections);

                    lastmodifieddates.Add(DateTime.Now);
                    model.SplitsList.Add(isc);
                }
                catch (Exception ex)
                {
                    return Json(new { error = true, errormessage = ex.Message });
                }
            }

            //new FrameworkUAD.BusinessLogic.ProductSubscription().Update_Requester_Flags(model.PubID, model.IssueID, CurrentClient.ClientConnections);
            int subCount = 0;
            subCount = model.subscriptionList.Select(x => x.Copies).Sum();
            ViewBag.RemainingCount = subCount - model.SplitsList.Select(x => x.IssueSplitCount).Sum();
            if (model.SplitsList.Count > 0)
                ViewBag.LastModifiedDate = lastmodifieddates.Max();
            else
                ViewBag.LastModifiedDate = "";
            #endregion
            return PartialView("_issueSplitGrid", model.SplitsList);
        }
        public ActionResult MoveRecordCounts(DesiredCountTrnsferViewModel movedata)
        {
            List<int> MovedSubID = new List<int>();
            var parentIssueSplitSubIdMapping = new FrameworkUAD.BusinessLogic.IssueSplitArchivePubSubscriptionMap().SelectIssueSplitPubsMapping(movedata.FromIssuePlitId, CurrentClient.ClientConnections);
            List<int> movedSubIndexes = getNth(movedata.TotalRecordCount, movedata.MovedRecordCount);
            movedSubIndexes.ForEach(x => MovedSubID.Add(parentIssueSplitSubIdMapping[x].IssueSplitPubSubscriptionId));
            DataTable dtPubsIds = CreateIdTable();
            MovedSubID.ForEach(x => dtPubsIds.Rows.Add(x));
            int result = new FrameworkUAD.BusinessLogic.IssueSplitArchivePubSubscriptionMap().MoveSplitRecords(movedata.ToIssueSplitId, movedata.FromIssuePlitId, movedata.MovedRecordCount, dtPubsIds, CurrentClient.ClientConnections);
            return Json(new { error = false, successmessage = "Records has been moved sucessfully" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIssues(int PubID = 0)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {
                var allissuelist = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);
                List<FrameworkUAD.Entity.Issue> issueList = allissuelist.Where(x => x.PublicationId == PubID).OrderByDescending(x => x.IssueId).ToList();
                return Json(issueList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult GetWaveMailingList(int issueId = 0)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {
                var currentissue = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections).Where(x => x.IsComplete == false).First();
                List<FrameworkUAD.Entity.WaveMailing> waveMailingList = new List<FrameworkUAD.Entity.WaveMailing>();
                var allwavelist = new FrameworkUAD.BusinessLogic.WaveMailing().Select(CurrentClient.ClientConnections);
                if (issueId > 0)
                    waveMailingList = allwavelist.Where(x => x.IssueID == issueId).ToList();
                else
                {
                    if (currentissue != null)
                        waveMailingList = allwavelist.Where(x => x.IssueID == currentissue.IssueId).ToList();
                }

                return Json(waveMailingList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        [HttpPost]
        public ActionResult ExportSplits(ExportModel exportmodel)
        {
            IssueSplitProductViewModel model = new IssueSplitProductViewModel();
            int ProductId = exportmodel.ProductID;
            int IssueID = exportmodel.IssueID;
            // var splitstring = splitIds.Split(',');
            List<int> SplitIDs = new List<int>();
            SplitIDs = exportmodel.IssueSplitIDs;
            //foreach (var s in splitstring)
            //{
            //    SplitIDs.Add(Convert.ToInt16(s));
            //}
            model.PubID = ProductId;
            model.IssueID = IssueID;
            model = GetSplitsModel(model);
            List<IssueSplitContainer> splitsList = new List<IssueSplitContainer>();
            splitsList = model.SplitsList.Where(x => SplitIDs.Contains(x.IssueSplitId)).ToList();
            List<string> defaultCols = new List<string>();
            //Special columns have extra processing that needs to happen before export. They are also in the default export format (if conditions are met).
            List<string> specialCols = new List<string>() { "ps.[ACSCode]", "ps.[MailerID]", "ps.[Keyline]", "ps.[Split Name]", "ps.[Split Description]", "ps.[KeyCode]" };
            List<string> removeCols = new List<string>();
            List<string> filenames = new List<string>();
            DataTable master = new DataTable();
            DataTable masterComps = new DataTable();

            if (exportmodel.DownloadFields != null && exportmodel.DownloadFields.Count > 0)
            {
                defaultCols = exportmodel.DownloadFields.Select(x => x.DownloadName).ToList();
                List<string> remove = defaultCols.Intersect(specialCols).ToList();
                defaultCols.RemoveAll(x => remove.Contains(x));
                specialCols.RemoveAll(x => !remove.Contains(x));
            }
            else
            {
                defaultCols = new List<string>() {
                "p.[Pubcode]","ps.[SequenceID]", "ps.[PubCategoryID]", "ps.[PubTransactionID]"," '' as [TempACSCode] "," '' as [tempkeyline]"," '' as [tempmailerid]",
                "ps.[IMBSeq]","ps.[Copies]", "ps.[FirstName]", "ps.[LastName]", "ps.[Title]", "ps.[Company]","ps.[Address1]", "ps.[Address2]", "ps.[Address3]", "ps.[City]", "ps.[RegionCode]", "ps.[ZipCode]", "ps.[Plus4]", "ps.[Country]", "ps.[reqflag]",
                "sp.[ExpireIssueDate]","ps.[Qualificationdate]","ps.[Exp_Qdate]"," '' as [tempkeycode] ",
                "ps.[QSource]"," '' as [tempsplit] "," '' as [tempsplitdesc] ",
                "ps.[Phone]","ps.[Fax]","ps.[Email]"};
            }

            {
                #region GetSubscribers
                string tempCols = "";
                HashSet<int> hs = new HashSet<int>();
                HashSet<int> hsComp = new HashSet<int>();
                List<int> subIDs = new List<int>();
                List<int> compSubs = new List<int>();
                foreach (string s in defaultCols)
                {
                    tempCols += s + ",";
                }
                tempCols = tempCols.TrimEnd(',');
                foreach (IssueSplitContainer isc in splitsList)
                {
                    subIDs.AddRange(isc.SubscriberIDs);
                }


                List<int> compSubIDs = new List<int>();
                List<FrameworkUAD.Entity.IssueCompDetail> comps = new List<FrameworkUAD.Entity.IssueCompDetail>();
                compSubIDs = (List<int>) Session["compSubIDs"];
                comps = (List<FrameworkUAD.Entity.IssueCompDetail>) Session["comps"];
                compSubs = compSubIDs.Intersect(subIDs).ToList();
                subIDs = subIDs.Except(compSubs).ToList();
                hs = new HashSet<int>(subIDs);
                hsComp = new HashSet<int>(comps.Where(x => compSubs.Contains(x.PubSubscriptionID)).Select(x => x.IssueCompDetailId));
                if (tempCols == "")
                {
                    //return Json(new { error = true, errormessage = "No columns were selected for export." });

                }
                #region ProductSubscription
                DataTable dt = new DataTable();
                int rowProcessedCount = 0;
                int index = 0;
                int size = 2500;
                int total = subIDs.Count;
                tempCols = "ps.[PubSubscriptionID]," + tempCols;
                if (total > 0)
                {
                    while (master.Rows.Count < hs.Count)
                    {
                        if ((index + 2500) > subIDs.Count)
                            size = subIDs.Count - index;
                        List<int> temp = subIDs.GetRange(index, size);
                        index += 2500;
                        DataTable wmProductSubscriptionResponse = new DataTable();
                        wmProductSubscriptionResponse = new FrameworkUAD.BusinessLogic.ProductSubscription().Select_For_Export_Static(ProductId, tempCols, temp, CurrentClient.ClientConnections);
                        if (wmProductSubscriptionResponse != null)
                        {
                            dt = wmProductSubscriptionResponse;
                            rowProcessedCount += dt.Rows.Count;
                            dt.AcceptChanges();
                            master.Merge(dt);
                        }
                    }
                    master.AcceptChanges();
                }
                #endregion

                foreach (DataRow dr in master.Rows)
                {
                    if (master.Columns.Contains("StatusUpdatedDate") && !string.IsNullOrEmpty(dr["StatusUpdatedDate"].ToString()))
                    {
                        dr["StatusUpdatedDate"] = DateTime.Parse(dr["StatusUpdatedDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("ExpireIssueDate") && !string.IsNullOrEmpty(dr["ExpireIssueDate"].ToString()))
                    {
                        dr["ExpireIssueDate"] = DateTime.Parse(dr["ExpireIssueDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("QualificationDate") && !string.IsNullOrEmpty(dr["QualificationDate"].ToString()))
                    {
                        dr["QualificationDate"] = String.Format("{0:MM/dd/yy}", DateTime.Parse(dr["QualificationDate"].ToString()));
                    }
                    if (master.Columns.Contains("Exp_QDate") && !string.IsNullOrEmpty(dr["Exp_QDate"].ToString()))
                    {
                        dr["Exp_QDate"] = DateTime.Parse(dr["Exp_QDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("StartIssueDate") && !string.IsNullOrEmpty(dr["StartIssueDate"].ToString()))
                    {
                        dr["StartIssueDate"] = DateTime.Parse(dr["StartIssueDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("Birthdate") && !string.IsNullOrEmpty(dr["Birthdate"].ToString()))
                    {
                        dr["Birthdate"] = DateTime.Parse(dr["Birthdate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("SubGenSubscriptionRenewDate") && !string.IsNullOrEmpty(dr["SubGenSubscriptionRenewDate"].ToString()))
                    {
                        dr["SubGenSubscriptionRenewDate"] = DateTime.Parse(dr["SubGenSubscriptionRenewDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("SubGenSubscriptionExpireDate") && !string.IsNullOrEmpty(dr["SubGenSubscriptionExpireDate"].ToString()))
                    {
                        dr["SubGenSubscriptionExpireDate"] = DateTime.Parse(dr["SubGenSubscriptionExpireDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("SubGenSubscriptionLastQualifiedDate") && !string.IsNullOrEmpty(dr["SubGenSubscriptionLastQualifiedDate"].ToString()))
                    {
                        dr["SubGenSubscriptionLastQualifiedDate"] = DateTime.Parse(dr["SubGenSubscriptionLastQualifiedDate"].ToString()).ToShortDateString();
                    }

                }
                master.AcceptChanges();

                #region Comps
                if (compSubs.Count > 0)
                {
                    FrameworkUAD.Entity.IssueCompDetail icd = new FrameworkUAD.Entity.IssueCompDetail();
                    List<System.Reflection.PropertyInfo> props = icd.GetType().GetProperties().ToList();
                    size = 2500;
                    index = 0;
                    total = hsComp.Count();
                    rowProcessedCount = 0;
                    tempCols = "";
                    foreach (string s in defaultCols)
                    {
                        //tempCols += s + ",";
                        string baseProp = s.Replace("ps.", "").Replace("[", "").Replace("]", "").Replace("sp.", "").Replace("demos.", "");
                        if (s.Contains("ps."))
                        {
                            if (props.Select(x => x.Name.ToLower()).Contains(baseProp.ToLower()))
                                tempCols += s + ",";
                            else if (baseProp.ToLower().Equals("exp_qdate"))
                            {
                                tempCols += s + ",";
                            }
                            else if (baseProp.ToLower().Equals("qsource"))
                            {
                                tempCols += @" '''' as QSource,";
                            }
                            else if (baseProp.ToLower().Equals("categorycode") || baseProp.ToLower().Equals("transactioncode"))
                            {
                                tempCols += s + ",";
                            }
                        }
                        else if (s.Contains("Pubcode"))
                        {
                            tempCols += s + ",";
                        }
                        else if (s.StartsWith(@" '' as"))
                        {
                            tempCols += baseProp.Replace("''", "''''") + ",";
                        }
                        else
                        {
                            if (baseProp.Equals("expireissuedate"))
                            {
                                tempCols += @" '''' as ExpireIssueDate,";
                            }
                            else
                            {
                                tempCols += @" '''' as [" + baseProp + "],";
                            }
                        }

                    }
                    tempCols = tempCols.TrimEnd(',');
                    tempCols = "IssueCompDetailId," + tempCols;
                    List<int> compIDs = new List<int>(hsComp);
                    while (rowProcessedCount < total)
                    {
                        if ((index + 2500) > compIDs.Count)
                            size = compIDs.Count - index;
                        List<int> temp = compIDs.GetRange(index, size);
                        index += 2500;
                        dt = new FrameworkUAD.BusinessLogic.IssueCompDetail().Select_For_Export(IssueID, tempCols, temp, CurrentClient.ClientConnections);
                        rowProcessedCount += dt.Rows.Count;

                        dt.AcceptChanges();
                        masterComps.Merge(dt);
                    }
                    masterComps.AcceptChanges();
                    //Switch below to try parse
                    foreach (DataRow dr in masterComps.Rows)
                    {
                        try
                        {
                            if (masterComps.Columns.Contains("StatusUpdatedDate") && dr["StatusUpdatedDate"] != null)
                                dr["StatusUpdatedDate"] = DateTime.Parse(dr["StatusUpdatedDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("ExpireIssueDate") && dr["ExpireIssueDate"] != null)
                                dr["ExpireIssueDate"] = DateTime.Parse(dr["ExpireIssueDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("QualificationDate") && dr["QualificationDate"] != null)
                                dr["QualificationDate"] = DateTime.Parse(dr["QualificationDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("Exp_QDate") && dr["Exp_QDate"] != null)
                                dr["Exp_QDate"] = DateTime.Parse(dr["Exp_QDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("StartIssueDate") && dr["StartIssueDate"] != null)
                                dr["StartIssueDate"] = DateTime.Parse(dr["StartIssueDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("Birthdate") && dr["Birthdate"] != null)
                                dr["Birthdate"] = DateTime.Parse(dr["Birthdate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("SubGenSubscriptionRenewDate") && dr["SubGenSubscriptionRenewDate"] != null)
                                dr["SubGenSubscriptionRenewDate"] = DateTime.Parse(dr["SubGenSubscriptionRenewDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("SubGenSubscriptionExpireDate") && dr["SubGenSubscriptionExpireDate"] != null)
                                dr["SubGenSubscriptionExpireDate"] = DateTime.Parse(dr["SubGenSubscriptionExpireDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("SubGenSubscriptionLastQualifiedDate") && dr["SubGenSubscriptionLastQualifiedDate"] != null)
                                dr["SubGenSubscriptionLastQualifiedDate"] = DateTime.Parse(dr["SubGenSubscriptionLastQualifiedDate"].ToString()).ToShortDateString();
                        }
                        catch
                        { }
                    }
                    masterComps.AcceptChanges();
                }
                #endregion

                #endregion

                #region ChangeColumns
                //If user combined/renamed any columns, that work happens here.
                if (exportmodel.NewColumnsFields != null && exportmodel.NewColumnsFields.Count > 0)
                {
                    foreach (NewColumn n in exportmodel.NewColumnsFields)
                    {
                        string delimiter = "";

                        switch (n.Delimiter)
                        {
                            case "Comma":
                                delimiter = ",";
                                break;
                            case "Space":
                                delimiter = " ";
                                break;
                            case "None":
                                break;
                        }

                        string expression = "";
                        foreach (string s in n.Columns)
                        {
                            if (s != n.Columns.Last())
                                expression = expression + "IsNull(" + s + ", '' )" + " + '" + delimiter + "' + ";
                            else
                                expression = expression + "IsNull(" + s + ", '' )";

                            removeCols.Add(s);
                        }
                        if (subIDs.Count > 0)
                        {
                            master.Columns.Add(n.Name, typeof(string), expression);
                            master.AcceptChanges();
                        }
                    }
                }
                #endregion

                master = SetSpecialColumns(master, specialCols);
                masterComps = SetSpecialColumns(masterComps, specialCols);
            }
            {


                foreach (IssueSplitContainer split in splitsList)
                {
                    var compSubIDs = (List<int>) Session["compSubIDs"];
                    var comps = (List<FrameworkUAD.Entity.IssueCompDetail>) Session["comps"];
                    DataTable masterClone = master.Copy();
                    DataTable masterCompClone = masterComps.Copy();
                    HashSet<int> hsTemp = new HashSet<int>(split.SubscriberIDs);
                    List<int> idTemp = new List<int>(compSubIDs.Intersect(split.SubscriberIDs));
                    HashSet<int> hsComp = new HashSet<int>(comps.Where(x => idTemp.Contains(x.PubSubscriptionID)).Select(x => x.IssueCompDetailId));

                    if (specialCols.Contains("ps.[Split Name]"))
                    {
                        DataColumn newColumn = new DataColumn("Split Name");
                        DataColumn newCompColumn = new DataColumn("Split Name");
                        newColumn.DefaultValue = split.IssueSplitName;
                        newCompColumn.DefaultValue = split.IssueSplitName;
                        masterClone.Columns.Add(newColumn);
                        masterCompClone.Columns.Add(newCompColumn);

                        if (masterClone.Columns.Contains("tempsplit"))
                        {
                            int keycodeindex = masterClone.Columns.IndexOf("tempsplit");
                            int createdIndex = masterClone.Columns.IndexOf("Split Name");
                            foreach (DataRow dr in masterClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterClone.Columns.Remove("Split Name");
                            masterClone.Columns["tempsplit"].ColumnName = "Split Name";
                        }

                        if (masterCompClone.Columns.Contains("tempsplit"))
                        {
                            int keycodeindex = masterCompClone.Columns.IndexOf("tempsplit");
                            int createdIndex = masterCompClone.Columns.IndexOf("Split Name");
                            foreach (DataRow dr in masterCompClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterCompClone.Columns.Remove("Split Name");
                            masterCompClone.Columns["tempsplit"].ColumnName = "Split Name";
                        }
                    }
                    if (specialCols.Contains("ps.[Split Description]"))
                    {
                        DataColumn newColumn = new DataColumn("Split Description");
                        DataColumn newCompColumn = new DataColumn("Split Description");
                        newColumn.DefaultValue = split.IssueSplitDescription;
                        newCompColumn.DefaultValue = split.IssueSplitDescription;
                        masterClone.Columns.Add(newColumn);
                        masterCompClone.Columns.Add(newCompColumn);

                        if (masterClone.Columns.Contains("tempsplitdesc"))
                        {
                            int keycodeindex = masterClone.Columns.IndexOf("tempsplitdesc");
                            int createdIndex = masterClone.Columns.IndexOf("Split Description");
                            foreach (DataRow dr in masterClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterClone.Columns.Remove("Split Description");
                            masterClone.Columns["tempsplitdesc"].ColumnName = "Split Description";
                        }

                        if (masterCompClone.Columns.Contains("tempsplitdesc"))
                        {
                            int keycodeindex = masterCompClone.Columns.IndexOf("tempsplitdesc");
                            int createdIndex = masterCompClone.Columns.IndexOf("Split Description");
                            foreach (DataRow dr in masterCompClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterCompClone.Columns.Remove("Split Description");
                            masterCompClone.Columns["tempsplitdesc"].ColumnName = "Split Description";
                        }
                    }
                    if (specialCols.Contains("ps.[KeyCode]"))
                    {
                        DataColumn newColumn = new DataColumn("KeyCode");
                        DataColumn newCompColumn = new DataColumn("KeyCode");
                        newColumn.DefaultValue = split.KeyCode;
                        newCompColumn.DefaultValue = split.KeyCode;
                        masterClone.Columns.Add(newColumn);
                        masterCompClone.Columns.Add(newCompColumn);

                        if (masterClone.Columns.Contains("tempkeycode"))
                        {
                            int keycodeindex = masterClone.Columns.IndexOf("tempkeycode");
                            int createdIndex = masterClone.Columns.IndexOf("KeyCode");
                            foreach (DataRow dr in masterClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterClone.Columns.Remove("KeyCode");
                            masterClone.Columns["tempkeycode"].ColumnName = "KeyCode";
                        }
                        if (masterCompClone.Columns.Contains("tempkeycode"))
                        {
                            int keycodeindex = masterCompClone.Columns.IndexOf("tempkeycode");
                            int createdIndex = masterCompClone.Columns.IndexOf("KeyCode");
                            foreach (DataRow dr in masterCompClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterCompClone.Columns.Remove("KeyCode");
                            masterCompClone.Columns["tempkeycode"].ColumnName = "KeyCode";
                        }
                    }
                    //Remove unneeded columns.
                    masterClone.Rows.Cast<DataRow>().Where(r => !hsTemp.Contains((int) r["PubSubscriptionID"])).ToList().ForEach(r => r.Delete());
                    if (masterClone.Columns.Contains("PubSubscriptionID"))
                        masterClone.Columns.Remove("PubSubscriptionID");
                    masterCompClone.Rows.Cast<DataRow>().Where(r => !hsComp.Contains((int) r["IssueCompDetailId"])).ToList().ForEach(r => r.Delete());
                    masterClone.AcceptChanges();
                    if (masterClone.Rows.Count >= 1)
                    {
                        masterClone.Merge(masterCompClone, false, MissingSchemaAction.Add);
                    }
                    else
                    {
                        masterClone = masterCompClone;
                    }

                    if (masterClone.Columns.Contains("IssueCompDetailId"))
                        masterClone.Columns.Remove("IssueCompDetailId");
                    if (masterClone.Columns.Contains("tempACSCode"))
                        masterClone.Columns["tempACSCode"].ColumnName = "ACSCode";
                    if (masterClone.Columns.Contains("tempkeyline"))
                        masterClone.Columns["tempkeyline"].ColumnName = "Keyline";
                    if (masterClone.Columns.Contains("tempmailerid"))
                        masterClone.Columns["tempmailerid"].ColumnName = "MailerID";
                    masterClone.AcceptChanges();

                    foreach (DataRow dr in masterClone.Rows)
                    {
                        if (String.IsNullOrEmpty(dr.ToString()))
                        {
                            masterClone.Rows.Remove(dr);
                        }
                    }
                    masterClone.AcceptChanges();
                    var str = UAS.Web.Helpers.IEnumerableExtention.DataTableToCSV(masterClone, ',');
                    string sanitizedName = Core_AMS.Utilities.StringFunctions.MakeValidFileName(split.IssueSplitName);
                    CreateCSVFile(sanitizedName, str, masterClone);
                    filenames.Add(sanitizedName + ".csv");
                    split.NotExported = false;
                    split.Save = false;
                }

                //MessageBox.Show("Exports completed and saved to User's Downloads folder.");
            }
            try
            {
                var files = Directory.GetFiles(Server.MapPath("../main/issueexport/"));
                var archive = Server.MapPath("../main/archive.zip");
                var temp = Server.MapPath("../main/temp/");
                var issueexportfolder = Server.MapPath("../main/issueexport/");
                // clear any existing archive
                if (System.IO.File.Exists(archive))
                {
                    System.IO.File.Delete(archive);
                }
                // empty the temp folder
                Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

                // copy the selected files to the temp folder
                files.ToList().ForEach(f => System.IO.File.Copy(f, Path.Combine(temp, Path.GetFileName(f))));

                // create a new archive
                ZipFile.CreateFromDirectory(temp, archive);

                // Delete files from Temp and IssueExport folder
                Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));
                Directory.EnumerateFiles(issueexportfolder).ToList().ForEach(f => System.IO.File.Delete(f));

                return Json(new { error = false, successmessage = "File exported successfully", filedownloadpath = archive, maximb = (int) Session["maxImbID"] }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { error = true, errrormessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DownLoadFile(string fileloc)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileloc);
            System.IO.File.Delete(fileloc);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, "Export.zip");
        }
        public ActionResult ExportColumnLists(int pubID, bool includeSplitsColumns = false)
        {
            int uadOnlyID = 0;
            List<DownloadField> downLoadFields = new List<DownloadField>();
            var codeResponse = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Response_Group);
            if (codeResponse != null && codeResponse.Count > 0)
            {
                FrameworkUAD_Lookup.Entity.Code c = codeResponse.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.ResponseGroupTypes.UAD_Only.ToString().Replace("_", " ")).FirstOrDefault();
                if (c != null)
                    uadOnlyID = c.CodeId;
            }
            var response = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(CurrentClient.ClientConnections);
            List<string> responses = response.Where(x => x.IsActive == true && x.ResponseGroupTypeId != uadOnlyID && x.PubID == pubID).OrderBy(x => x.DisplayOrder).Select(x => x.DisplayName).ToList();
            //responses.Add("Permissions");
            List<string> adHocs = new List<string>();
            var adHocResponses = new FrameworkUAD.BusinessLogic.ProductSubscription().Get_AdHocs(pubID, CurrentClient.ClientConnections);
            if (adHocResponses != null && adHocResponses.Count > 0)
            {
                adHocs = adHocResponses;
            }
            List<string> paidProperties = new List<string>()
            {
                "TotalIssues", "Term", "Frequency", "StartIssueDate", "ExpireIssueDate", "Amount", "AmountPaid", "PaidDate",
                "PaymentType", "CreditCardType", "CheckNumber", "CCNumber", "CCExpirationMonth", "CCExpirationYear", "Payor Name"
            };
            List<string> properties = new List<string>()
            {
                "Pubcode","SequenceID","SubscriptionID", "Batch", "Email","FirstName","LastName","Company","Title","Address1","Address2","Address3","City","RegionCode","ZipCode","Plus4","Country","County","Phone","Mobile","Fax",
                "Website","CategoryCode", "TransactionCode", "QSource","Qualificationdate","Par3C", "Copies","Demo7","SubscriberSourceCode", "OrigsSrc", "WaveMailingID",
                "IMBSeq", "ReqFlag", "Verify", "MailPermission", "FaxPermission", "PhonePermission", "OtherProductsPermission", "EmailRenewPermission", "ThirdPartyPermission","TextPermission","DateCreated","DateUpdated"
            };
            if (includeSplitsColumns == true)
            {
                properties.Add("ACSCode");
                properties.Add("Keyline");
                properties.Add("MailerID");
                properties.Add("Split Name");
                properties.Add("Split Description");
                properties.Add("KeyCode");
                properties.Remove("SubscriptionID");
            }
            foreach (string s in properties)
            {
                if (s.Equals("Pubcode"))
                {
                    downLoadFields.Add(new DownloadField(s, "p", "Profile"));
                }
                else
                {
                    downLoadFields.Add(new DownloadField(s, "ps", "Profile"));
                }
            }
            foreach (string s in paidProperties)
                downLoadFields.Add(new DownloadField(s, "sp", "Paid"));
            foreach (string s in responses)
                downLoadFields.Add(new DownloadField(s, "demos", "Demo"));
            foreach (string s in adHocs)
                downLoadFields.Add(new DownloadField(s, "adhoc", "AdHoc"));

            var profilefields = downLoadFields.Where(x => x.Type == "Profile").ToList();
            var demofields = downLoadFields.Where(x => x.Type == "Demo").ToList();
            var paidfields = downLoadFields.Where(x => x.Type == "Paid").ToList();
            var adhocfields = downLoadFields.Where(x => x.Type == "AdHoc").ToList();
            var model = new ExportFieldsViewModel() { ProfileFields = profilefields, DemoFields = demofields, AdHocFields = adhocfields, PaidFields = paidfields };
            return PartialView("_downLoadFields", model);
        }
        public ActionResult GetFinalizeWavePopup()
        {
            return PartialView("_issueWaveMailingDetails");
        }
        public ActionResult GetNewIssuePopup()
        {
            return PartialView("_nextIssueDetails");
        }
        public ActionResult ResetImbSequenceCounter(int pubID)
        {
            FrameworkUAD.Entity.Product product = new FrameworkUAD.Entity.Product();
            if (pubID > 0)
            {
                product = new FrameworkUAD.BusinessLogic.Product().Select(pubID, CurrentClient.ClientConnections);
            }
            var mailerInfoResponse = new FrameworkUAD.BusinessLogic.AcsMailerInfo().SelectByID(product.AcsMailerInfoId, CurrentClient.ClientConnections);
            if (mailerInfoResponse != null)
            {
                if (mailerInfoResponse.ImbSeqCounter == 0)
                    Session["maxImbID"] = 1;
                else
                    Session["maxImbID"] = mailerInfoResponse.ImbSeqCounter;
            }
            return Json(new { error = false }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Finalize(IssueFinalizeViewModel model)
        {
            bool flag = false;
            if (model.FinalizeOperation.Equals("Finalize Issue", StringComparison.InvariantCultureIgnoreCase))
            {
                flag = FinalizeIssues(model);
                if (flag)
                    return Json(new { error = false, successmessage = "Issue has been archived successfully." }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { error = true, errormessage = "There was a problem finalizing the Issue.  If this problem persists, please contact Product Support." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                flag = FinalizeWave(model);
                if (flag)
                    return Json(new { error = false, succcessmessage = "Wave Mailing saved successfully." }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { error = false, errormessage = "Error occurred while saving the Wave." }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateNewIssue(IssueFinalizeViewModel model)
        {
            int myIssueID = model.CurrentIssueID;
            int myProductID = model.CurrentProductID;
            string name = model.NextIssueName;
            string code = model.NextIssueCode;
            var myProduct = new FrameworkUAD.BusinessLogic.Product().Select(myProductID, CurrentClient.ClientConnections);
            FrameworkUAD.Entity.IssueComp myComp = new FrameworkUAD.Entity.IssueComp();
            var issueResponse = new FrameworkUAD.BusinessLogic.Issue().SelectPublication(myProductID, CurrentClient.ClientConnections);
            if (issueResponse != null && issueResponse.Count > 0)
            {
                FrameworkUAD.Entity.Issue i = issueResponse.Where(x => x.IssueId == myIssueID).FirstOrDefault();
                if (i != null)
                {
                    i.IsComplete = true;
                    i.DateComplete = DateTime.Now;
                    i.CompleteByUserID = CurrentUser.UserID;
                    new FrameworkUAD.BusinessLogic.Issue().Save(i, CurrentClient.ClientConnections);
                }
                myComp.IsActive = false;
                if (myComp.IssueCompId > 0)
                {
                    new FrameworkUAD.BusinessLogic.IssueComp().Save(myComp, CurrentClient.ClientConnections);
                    new FrameworkUAD.BusinessLogic.IssueCompDetail().Clear(myComp.IssueCompId, CurrentClient.ClientConnections);
                }

                FrameworkUAD.Entity.Issue nextIssue = new FrameworkUAD.Entity.Issue();
                nextIssue.PublicationId = myProductID;
                nextIssue.IssueName = name;
                nextIssue.IssueCode = code;
                nextIssue.DateOpened = DateTime.Now;
                nextIssue.OpenedByUserID = CurrentUser.UserID;
                nextIssue.DateCreated = DateTime.Now;
                nextIssue.CreatedByUserID = CurrentUser.UserID;
                new FrameworkUAD.BusinessLogic.Issue().Save(nextIssue, CurrentClient.ClientConnections);
                myProduct.AllowDataEntry = true;
                myProduct.DateUpdated = DateTime.Now;
                myProduct.UpdatedByUserID = CurrentUser.UserID;
                new FrameworkUAD.BusinessLogic.Product().Save(myProduct, CurrentClient.ClientConnections);
                return Json(new { error = false, successmessage = "The new issue has been opened." }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { error = true, errormessage = "Failed to load the next issue." }, JsonRequestBehavior.AllowGet);
        }
        private IssueSplitProductViewModel GetSplitsModel(IssueSplitProductViewModel model)
        {
            model.subscriptionList = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectAllActiveIDs(model.PubID, CurrentClient.ClientConnections);
            model.RemainingPool = new List<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));

            List<int> compSubIDs = new List<int>();
            var issueCompResponse = new FrameworkUAD.BusinessLogic.IssueComp().SelectIssue(model.IssueID, CurrentClient.ClientConnections);
            List<FrameworkUAD.Entity.IssueCompDetail> comps = new List<FrameworkUAD.Entity.IssueCompDetail>();
            if (issueCompResponse != null && issueCompResponse.Count > 0)
            {
                int id = issueCompResponse.Where(x => x.IsActive == true).LastOrDefault().IssueCompId;
                var myComp = issueCompResponse.Where(x => x.IsActive == true).FirstOrDefault();
                var myIssueCompID = id;
                var issueCompDetailResponse = new FrameworkUAD.BusinessLogic.IssueCompDetail().Select(id, CurrentClient.ClientConnections);
                if (issueCompDetailResponse != null && issueCompDetailResponse.Count > 0)
                {
                    int tempSubID = model.RemainingPool.Max();
                    tempSubID += 500000;

                    comps = issueCompDetailResponse;

                    foreach (FrameworkUAD.Entity.IssueCompDetail comp in comps)
                    {
                        FrameworkUAD.Entity.ProductSubscription sp = new FrameworkUAD.Entity.ProductSubscription(comp);
                        tempSubID++;
                        sp.PubSubscriptionID = tempSubID;
                        comp.PubSubscriptionID = tempSubID;
                        //allProductSubscriptions.Add(sp);
                        model.subscriptionList.Add(new FrameworkUAD.Entity.CopiesProductSubscription() { PubSubscriptionID = tempSubID, Copies = comp.Copies });
                        //activeSubscriptionIDs.Add(tempSubID);
                        model.RemainingPool.Add(tempSubID);
                        compSubIDs.Add(tempSubID);
                    }
                }
            }
            Session["compSubIDs"] = compSubIDs;
            Session["comps"] = comps;
            var activeSubscriptionIDs = new HashSet<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));

            model.SplitsList = new List<IssueSplitContainer>();
            List<FrameworkUAD.Entity.IssueSplit> issuesplitList = new List<FrameworkUAD.Entity.IssueSplit>();
            var templist = new FrameworkUAD.BusinessLogic.IssueSplit().SelectIssueID(model.IssueID, CurrentClient.ClientConnections);
            if (model.WaveID == 0)
            {
                issuesplitList = templist.Where(x => x.WaveMailingID.HasValue == false).ToList();
            }
            else
            {
                issuesplitList = templist.Where(x => x.WaveMailingID.HasValue == true && x.WaveMailingID==model.WaveID ).ToList();
            }
           
            foreach (var i in issuesplitList)
            {
                model.lastmodifieddates.Add(i.DateCreated);
                var SubIds = new List<int>();
                var f = new FrameworkUAD.BusinessLogic.IssueSplitFilter().SelectFilterID(i.FilterId, CurrentClient.ClientConnections);
                var fd = new FrameworkUAD.BusinessLogic.IssueSplitFilterDetails().SelectFilterID(i.FilterId, CurrentClient.ClientConnections);
                var filter = new FrameworkUAD.Object.FilterMVC();
                filter.FilterName = f.FilterName;
                filter.FilterID = f.FilterID;
                filter.PubID = f.PubId;
                fd.ForEach(x => filter.Fields.Add(new FrameworkUAD.Object.FilterDetails() { Name = x.Name, FilterID = x.FilterID, Values = x.Values, SearchCondition = x.SearchCondition, Group = x.Group, Text = x.Text }));
                var mapping = new FrameworkUAD.BusinessLogic.IssueSplitArchivePubSubscriptionMap().SelectIssueSplitPubsMapping(i.IssueSplitId, CurrentClient.ClientConnections);

                mapping.ForEach(x => SubIds.Add(x.IssueSplitPubSubscriptionId));

                var container = new IssueSplitContainer(i, filter, SubIds, model.PubID);
                container.Save = true;
                model.SplitsList.Add(container);
            }

            return model;
        }
        private static DataTable CreateIdTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            return dt;
        }
        private static DataTable CreateFilterDetailsTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Values", typeof(string));
            dt.Columns.Add("SearchCondition", typeof(string));
            dt.Columns.Add("Group", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            return dt;
        }
        private static List<int> getNth(int TotalRecords, int RequestedRecords)
        {
            List<int> listNth = new List<int>();

            if (RequestedRecords == 0)
                RequestedRecords = TotalRecords;

            double inccounter = (double) TotalRecords / RequestedRecords;

            double y = inccounter;

            for (; Math.Round(y, 2) <= TotalRecords; y = (y + inccounter))
            {
                listNth.Add(Convert.ToInt32(y - 1));

                if (listNth.Count == RequestedRecords)
                {
                    break;
                }
            }
            return listNth;
        }
        private IssueSplitProductViewModel GetSplitList(IssueSplitProductViewModel model, List<FrameworkUAD.Object.FilterMVC> filterscol)
        {

            //var dt = filtercol.ExecuteForInterSection();
            //var combinations= filtercol.FilterComboList;
            //model.Issue = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections).Where(x=>x.IssueId== model.IssueID).FirstOrDefault();
            var product = new FrameworkUAD.BusinessLogic.Product().Select(model.PubID, CurrentClient.ClientConnections);
            model.PubID = product.PubID;
            model.PubName = product.PubName;
            //var mailerInfoResponse =new FrameworkUAD.BusinessLogic.AcsMailerInfo().SelectByID(product.AcsMailerInfoId,CurrentClient.ClientConnections);
            //if (mailerInfoResponse != null)
            //{
            //    if (mailerInfoResponse.ImbSeqCounter == 0)
            //        model.ImbSeqCounter = 1;
            //    else
            //        model.ImbSeqCounter = mailerInfoResponse.ImbSeqCounter;
            //}
            //var countsResponse =new FrameworkUAD.BusinessLogic.Report().SelectActiveIssueSplitsCounts(model.PubID, CurrentClient.ClientConnections);
            //if (countsResponse != null)
            //{
            //    model.RecordCounts = countsResponse;
            //    model.QualDict = new Dictionary<int, string>(model.RecordCounts.FreeRecords + model.RecordCounts.PaidRecords);
            //    model.ImbSeqs = new Dictionary<int, string>(model.RecordCounts.FreeRecords + model.RecordCounts.PaidRecords);
            //    model.CompImbSeqs = new Dictionary<int, string>(model.RecordCounts.FreeRecords + model.RecordCounts.PaidRecords);
            //}
            //var qualResponse =new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);
            //if (qualResponse!=null && qualResponse.Count>0)
            //{
            //    model.QualDict = qualResponse.ToDictionary(x => x.CodeId, x => x.CodeValue);
            //}

            model.subscriptionList = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectAllActiveIDs(model.PubID, CurrentClient.ClientConnections);
            model.RemainingPool = new List<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));

            var issueResponse = new FrameworkUAD.BusinessLogic.Issue().SelectPublication(model.PubID, CurrentClient.ClientConnections);
            model.Issue = issueResponse.Where(x => x.IssueId == model.IssueID).FirstOrDefault();
            if (model.Issue.IsComplete == false)
                model.ReadOnly = false;
            else
                model.ReadOnly = true;

            List<int> compSubIDs = new List<int>();
            var issueCompResponse = new FrameworkUAD.BusinessLogic.IssueComp().SelectIssue(model.Issue.IssueId, CurrentClient.ClientConnections);
            List<FrameworkUAD.Entity.IssueCompDetail> comps = new List<FrameworkUAD.Entity.IssueCompDetail>();
            if (issueCompResponse != null && issueCompResponse.Count > 0)
            {
                int id = issueCompResponse.Where(x => x.IsActive == true).LastOrDefault().IssueCompId;
                var myComp = issueCompResponse.Where(x => x.IsActive == true).FirstOrDefault();
                var myIssueCompID = id;
                var issueCompDetailResponse = new FrameworkUAD.BusinessLogic.IssueCompDetail().Select(id, CurrentClient.ClientConnections);
                if (issueCompDetailResponse != null && issueCompDetailResponse.Count > 0)
                {
                    int tempSubID = model.RemainingPool.Max();
                    tempSubID += 500000;

                    comps = issueCompDetailResponse;

                    foreach (FrameworkUAD.Entity.IssueCompDetail comp in comps)
                    {
                        FrameworkUAD.Entity.ProductSubscription sp = new FrameworkUAD.Entity.ProductSubscription(comp);
                        tempSubID++;
                        sp.PubSubscriptionID = tempSubID;
                        comp.PubSubscriptionID = tempSubID;
                        //allProductSubscriptions.Add(sp);
                        model.subscriptionList.Add(new FrameworkUAD.Entity.CopiesProductSubscription() { PubSubscriptionID = tempSubID, Copies = comp.Copies });
                        //activeSubscriptionIDs.Add(tempSubID);
                        model.RemainingPool.Add(tempSubID);
                        compSubIDs.Add(tempSubID);
                    }
                }
            }
            var activeSubscriptionIDs = new HashSet<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));
            #region  Add Filters Start
            model.FilterCollection = new FrameworkUAD.Object.FilterCollection(CurrentClient.ClientConnections, CurrentUser.UserID);
            foreach (var f in filterscol)
            {
                f.FilterNo = model.FilterCollection.Count() + 1;
                model.FilterCollection.Add(f);
            }

            #endregion

            List<IssueSplitContainer> tempSplitsList = new List<IssueSplitContainer>();
            foreach (var filter in model.FilterCollection)
            {
                FrameworkUAD.Entity.IssueSplit split = new FrameworkUAD.Entity.IssueSplit();
                string filterQuery = "";
                if (model.Issue != null && model.Issue.IsComplete)
                    filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductArchiveFilterQuery(filter, "distinct ps.PubSubscriptionID", "", model.IssueID, CurrentClient.ClientConnections);
                else
                    filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);

                int count = 0;
                List<int> subIDs = new List<int>();
                var subCountResponse = new FrameworkUAD.BusinessLogic.Report().SelectSubscriberCountMVC(filterQuery, CurrentClient.ClientConnections);
                if (subCountResponse != null && subCountResponse.Count > 0)
                {
                    subIDs = subCountResponse.Where(x => activeSubscriptionIDs.Contains(x)).ToList();
                }
                //This will be provided by Sunil
                //if (comps.Count > 0 && !obj.Filters.Contains("Responses") && !obj.AdHocFilters.Contains("<FilterObjectType>AdHoc</FilterObjectType>")) //IssueCompDetails can not have Response or PubSubExtension data.
                //{
                //    var subCountResponse = new FrameworkUAD.BusinessLogic.IssueCompDetail().GetByFilter(accessKey, obj.Filters, obj.AdHocFilters, myIssueCompID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                //    if (subCountResponse!=null && subCountResponse.Count>0)
                //    {
                //        List<int> temp = subCountResponse;
                //        List<int> ids = comps.Where(x => temp.Contains(x.IssueCompDetailId)).Select(x => x.PubSubscriptionID).ToList();
                //        subIDs.AddRange(ids);
                //    }
                //}
                filter.SubscriberIDs = subIDs;
                DataTable dtSubIDs = CreateIdTable();
                subIDs.ForEach(x => dtSubIDs.Rows.Add(x));
                count = new FrameworkUAD.BusinessLogic.IssueSplit().GetSubscriberCopiesCount(dtSubIDs, CurrentClient.ClientConnections);
                //count = model.subscriptionList.Select(x => x.PubSubscriptionID).Intersect(subIDs).Count(); //Perfomence improvement on Below
                //count = model.subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                if (count > 0)
                {
                    split.IssueSplitCount = count;
                    split.IssueSplitName = filter.FilterName;
                    split.IssueSplitDescription = filter.FilterDescription;
                    tempSplitsList.Add(new IssueSplitContainer(split, filter, subIDs, model.PubID));

                }
                model.TempSplitsList = tempSplitsList;
            }

            return model;

        }
        private List<FilterCombinations> GetCombination(List<int> list)
        {
            //Algorithm to find all possible combinations of a list of values.
            List<FilterCombinations> combos = new List<FilterCombinations>();
            double count = Math.Pow(2, list.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                List<int> criteria = new List<int>();
                string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '1')
                    {
                        criteria.Add(list[j]);
                    }
                }
                combos.Add(new FilterCombinations(criteria));
            }
            combos = combos.OrderByDescending(x => x.Criteria.Count).ToList();
            return combos;
        }
        private List<FrameworkUAD.Object.FilterDetails> GetCombinedDetail(List<FrameworkUAD.Object.FilterDetails> deets)
        {

            List<FrameworkUAD.Object.FilterDetails> returnDetails = new List<FrameworkUAD.Object.FilterDetails>();
            foreach (FrameworkUAD.Object.FilterDetails fo in deets)
            {
                if (!fo.Name.Equals("Adhoc", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!returnDetails.Select(x => x.Name).Contains(fo.Name))
                    {
                        var dups = deets.Where(x => x.Name == fo.Name);
                        string[] arrValues = { };
                        string[] arrText = { };
                        if (dups.Count() > 1)
                        {
                            foreach (var f in dups)
                            {
                                arrValues = arrValues.Union(f.Values.Split(',')).ToArray();
                                arrText = arrText.Union(f.Text.Split(',')).ToArray();
                            }
                            fo.Values = string.Join(",", arrValues);
                            fo.Text = string.Join(",", arrText);
                            returnDetails.Add(fo);
                        }
                        else
                        {
                            returnDetails.AddRange(dups);
                        }

                    }

                }


            }
            return returnDetails;
        }
        private DataTable SetSpecialColumns(DataTable dt, List<string> cols)
        {
            var myAcsMailer = (FrameworkUAD.Entity.AcsMailerInfo) Session["myAcsMailerInfo"];
            //Session["maxImbID"] = model.ImbSeqCounter;
            //Session["ImbSeqs"] = model.ImbSeqs;
            //Session["CompImbSeqs"] = model.CompImbSeqs;
            var imbSeqs = (Dictionary<int, string>) Session["ImbSeqs"];
            var compImbSeqs = (Dictionary<int, string>) Session["CompImbSeqs"];
            int mailerID = myAcsMailer.MailerID;
            string acsCode = myAcsMailer.AcsCode;

            if (acsCode != null && acsCode != string.Empty)
            {
                if (cols.Contains("ps.[ACSCode]"))
                {
                    DataColumn newColumn = new DataColumn("ACSCode");
                    newColumn.DefaultValue = acsCode;
                    dt.Columns.Add(newColumn);

                    if (dt.Columns.Contains("TempACSCode"))
                    {
                        int keycodeindex = dt.Columns.IndexOf("TempACSCode");
                        int createdIndex = dt.Columns.IndexOf("ACSCode");
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr[keycodeindex] = dr[createdIndex];
                        }
                        dt.Columns.Remove("ACSCode");
                        dt.Columns["TempACSCode"].ColumnName = "ACSCode";
                    }

                }
            }

            if (acsCode != null && acsCode != "")
            {
                if (cols.Contains("ps.[Keyline]") && dt.Columns.Contains("SequenceID"))
                {
                    dt.Columns.Add("Keyline");
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["SequenceID"].ToString() != "0")
                        {
                            var key = KeyLineComputation.Compute(dr["SequenceID"].ToString());
                            dr["Keyline"] = key;
                        }
                        if (dt.Columns.Contains("tempkeyline"))
                        {
                            int keycodeindex = dt.Columns.IndexOf("tempkeyline");
                            int createdIndex = dt.Columns.IndexOf("Keyline");
                            foreach (DataRow dr2 in dt.Rows)
                            {
                                dr2[keycodeindex] = dr2[createdIndex];
                            }
                            dt.Columns.Remove("Keyline");
                            dt.Columns["tempkeyline"].ColumnName = "Keyline";
                        }
                    }
                    if (dt.Columns.Contains("tempkeyline"))
                    {
                        int keycodeindex = dt.Columns.IndexOf("tempkeyline");
                        int createdIndex = dt.Columns.IndexOf("Keyline");
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr[keycodeindex] = dr[createdIndex];
                        }
                        dt.Columns.Remove("Keyline");
                        dt.Columns["tempkeyline"].ColumnName = "Keyline";
                    }
                }
            }
            if (mailerID > 0)
            {
                if (dt.Columns.Contains("IMBSeq"))
                {
                    if (dt.Columns.Contains("PubSubscriptionID"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string imb = CreateIMBSeq((int) Session["maxImbID"], mailerID.ToString());
                            int id = 0;
                            dr["IMBSeq"] = imb;
                            if (int.TryParse(dr["PubSubscriptionID"].ToString(), out id) == true && !imbSeqs.ContainsKey(id) && !imbSeqs.ContainsValue(imb))
                                imbSeqs.Add(id, imb);
                        }
                    }
                    else if (dt.Columns.Contains("IssueCompDetailId"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string imb = CreateIMBSeq((int) Session["maxImbID"], mailerID.ToString());
                            int id = 0;
                            dr["IMBSeq"] = imb;
                            if (int.TryParse(dr["IssueCompDetailId"].ToString(), out id) == true && !compImbSeqs.ContainsKey(id) && !compImbSeqs.ContainsValue(imb))
                                compImbSeqs.Add(id, imb);
                        }
                    }
                }
            }
            if (cols.Contains("ps.[MailerID]"))
            {
                DataColumn newColumn = new DataColumn("MailerID");
                newColumn.DefaultValue = mailerID;
                dt.Columns.Add(newColumn);

                if (dt.Columns.Contains("tempmailerid"))
                {
                    int keycodeindex = dt.Columns.IndexOf("tempmailerid");
                    int createdIndex = dt.Columns.IndexOf("MailerID");
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[keycodeindex] = dr[createdIndex];
                    }
                    dt.Columns.Remove("MailerID");
                    dt.Columns["tempmailerid"].ColumnName = "MailerID";
                }
            }
            if (dt.Columns.Contains("ExpireIssueDate"))
            {
                int Index = dt.Columns.IndexOf("ExpireIssueDate");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[Index].ToString() == "'")
                        dr[Index] = "";
                }
            }
            if (dt.Columns.Contains("QSource"))
            {
                int Index = dt.Columns.IndexOf("QSource");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[Index].ToString() == "'")
                        dr[Index] = "";
                }
            }

            Session["ImbSeqs"] = imbSeqs;
            Session["CompImbSeqs"] = compImbSeqs;
            return dt;
        }
        private void CreateCSVFile(string splitName, string str, DataTable masterClone)
        {
            string path = Server.MapPath("../main/issueexport/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = path + "\\" + splitName + ".csv";
            if (System.IO.File.Exists(filePath))
            {
                int i = 1;
                while (System.IO.File.Exists(filePath))
                {
                    filePath = path + "\\" + splitName + "_" + i + ".csv";
                    i++;
                }
            }
            new Core_AMS.Utilities.FileFunctions().CreateCSVFromDataTable(masterClone, filePath, true);
            //using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            //{
            //    string test = str;
            //    StreamWriter sw = new StreamWriter(stream);

            //    test = Core_AMS.Utilities.HTMLFunctions.StripTextFromHtmlForExport(test);

            //    sw.Write(test);
            //    sw.Flush();
            //    sw.Close();
            //}
        }
        private string CreateIMBSeq(int imb, string id)
        {
            int count = id.Length;
            int fill = 15 - count;
            int maxIMB = (int) Session["maxImbID"];
            string retIMB = imb.ToString();
            if (retIMB.Length > fill)
            {
                maxIMB = 2;
                retIMB = "1";
            }
            else
                maxIMB++;

            for (int i = retIMB.Length; i < fill; i++)
            {
                retIMB = "0" + retIMB;
            }
            Session["maxImbID"] = maxIMB;
            return retIMB;
        }
        private bool FinalizeIssues(IssueFinalizeViewModel model)
        {
            var myAcsMailerInfo = (FrameworkUAD.Entity.AcsMailerInfo) Session["myAcsMailerInfo"];
            int origIMB = myAcsMailerInfo.ImbSeqCounter;
            try
            {
                //Save_Splits_And_Filters();
                foreach (var isc in model.Splits)
                {
                    isc.UpdatedByUserID = CurrentUser.UserID;
                    Update_Splits(isc);
                }
                UpdateAcsMailerInfo();
                ArchiveAll(model.CurrentIssueID, model.CurrentProductID);
                UpdateOriginalRecords(model.CurrentProductID);
            }
            catch (Exception ex)
            {

                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication("AMS_Web");
                int logClientId = CurrentUser.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                new KMPlatform.BusinessLogic.ApplicationLog().LogCriticalError(formatException, this.GetType().Name.ToString() + ".IssueSplits", app, string.Empty, logClientId);
            }
            if (ValidateArchive(model.CurrentIssueID, model.CurrentProductID))
            {
                return true;
            }
            else
            {
                ////rollback, display error, and send email
                RollBackIssue(model);
                return false;
            }
        }
        private bool FinalizeWave(IssueFinalizeViewModel model)
        {
            string name = model.WaveMailingName;
            int waveNumber = 0;
            var allwavelist = new FrameworkUAD.BusinessLogic.WaveMailing().Select(CurrentClient.ClientConnections);
            var wave =allwavelist.Where(x => x.IssueID == model.CurrentIssueID);
            if (wave != null)
            {
                waveNumber =wave.Count()+1;
            }
            int myIssueID = model.CurrentIssueID;
            int myProductID = model.CurrentProductID;
            var compSubIDs = (List<int>) Session["compSubIDs"];
            var imbSeqs = (Dictionary<int, string>) Session["ImbSeqs"];
            var compImbSeqs = (Dictionary<int, string>) Session["CompImbSeqs"];
            var myAcsMailerInfo = (FrameworkUAD.Entity.AcsMailerInfo) Session["myAcsMailerInfo"];
            IssueSplitProductViewModel vmmodel = new IssueSplitProductViewModel();
            vmmodel.PubID = myProductID;
            vmmodel.IssueID = myIssueID;
            vmmodel = GetSplitsModel(vmmodel);
            List<IssueSplitContainer> splitsList = new List<IssueSplitContainer>();
            splitsList = vmmodel.SplitsList.Where(x => model.Splits.Select(y=>y.IssueSplitId).Contains(x.IssueSplitId)).ToList();
            var myProduct = new FrameworkUAD.BusinessLogic.Product().Select(myProductID, CurrentClient.ClientConnections);
            int origIMB = myAcsMailerInfo.ImbSeqCounter;
            #region Finalize Wave
            List<int> workingList = new List<int>();
            foreach (IssueSplitContainer split in splitsList)
            {
                workingList.AddRange(split.SubscriberIDs);
            }
            {

                try
                {
                    UpdateAcsMailerInfo();
                    workingList = workingList.Except(compSubIDs).ToList();
                   
                    FrameworkUAD.Entity.WaveMailing wm = new FrameworkUAD.Entity.WaveMailing();
                    wm.WaveMailingName = name;
                    wm.WaveNumber = waveNumber + 1;
                    wm.IssueID = myIssueID;
                    wm.PublicationID = myProductID;
                    wm.DateCreated = DateTime.Now;
                    wm.CreatedByUserID = CurrentUser.UserID;
                    int intResponse = new FrameworkUAD.BusinessLogic.WaveMailing().Save(wm, CurrentClient.ClientConnections);
                    foreach (var isc in model.Splits)
                    {
                        isc.WaveMailingID = intResponse;
                        isc.UpdatedByUserID = CurrentUser.UserID;
                        Update_Splits(isc);
                    }
                    int total = workingList.Count;
                    int counter = 0;
                    int processedCount = 0;
                    int batchSize = 1000;
                    string xml = "<XML>";
                    if (intResponse > 0)
                    {
                        foreach (int i in workingList)
                        {
                            //We need to save the IMBSEQ numbers somewhere when doing a Wave Mailing because we do not Archive them until later. 
                            //So we use the PubSubscription table to store them temporarily.
                            string imb = "";
                            if (imbSeqs.Keys.Where(x => x == i).Count() > 0)
                            {
                                KeyValuePair<int, string> kv = imbSeqs.Where(x => x.Key == i).First();
                                if (!kv.Equals(new KeyValuePair<int, string>()))
                                    imb = kv.Value;
                            }
                            xml = xml + "<S><ID>" + i + "</ID><IMB>" + imb + "</IMB></S>";

                            counter++;
                            processedCount++;
                            if (processedCount == total || counter == batchSize)
                            {
                                xml = xml + "</XML>";
                                new FrameworkUAD.BusinessLogic.ProductSubscription().SaveBulkWaveMailing(xml, intResponse, CurrentClient.ClientConnections);
                                counter = 0;
                                xml = "<XML>";

                            }
                        }
                        var issueResponse = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);
                        if (issueResponse != null && issueResponse.Count > 0)
                        {
                            FrameworkUAD.Entity.Issue myIssue = issueResponse.Where(x => x.IssueId == myIssueID).FirstOrDefault();
                            if (myIssue != null)
                            {
                                myIssue.DateOpened = DateTime.Now;
                                myIssue.OpenedByUserID = CurrentUser.UserID;
                                myIssue.IsClosed = false;
                                myIssue.DateUpdated = DateTime.Now;
                                new FrameworkUAD.BusinessLogic.Issue().Save(myIssue, CurrentClient.ClientConnections);
                            }
                        }

                    }
                    {
                        myProduct.AllowDataEntry = true;
                        myProduct.DateUpdated = DateTime.Now;
                        myProduct.UpdatedByUserID = CurrentUser.UserID;
                        new FrameworkUAD.BusinessLogic.Product().Save(myProduct, CurrentClient.ClientConnections);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            #endregion

        }
        private void Update_Splits(FrameworkUAD.Entity.IssueSplit split)
        {
            new FrameworkUAD.BusinessLogic.IssueSplit().UpdateIssueSplit(split, CurrentClient.ClientConnections);
        }
        private void UpdateAcsMailerInfo()
        {
            var myAcsMailer = (FrameworkUAD.Entity.AcsMailerInfo) Session["myAcsMailerInfo"];
            myAcsMailer.ImbSeqCounter = (int) Session["maxImbID"];
            myAcsMailer.DateUpdated = DateTime.Now;
            myAcsMailer.UpdatedByUserID = CurrentUser.UserID;
            new FrameworkUAD.BusinessLogic.AcsMailerInfo().Save(myAcsMailer, CurrentClient.ClientConnections);
        }
        private void ArchiveAll(int CurrentIssueID, int CurrentProductID)
        {
            int myIssueID = CurrentIssueID;
            int myProductID = CurrentProductID;
            var compSubIDs = (List<int>) Session["compSubIDs"];
            var imbSeqs = (Dictionary<int, string>) Session["ImbSeqs"];
            var compImbSeqs = (Dictionary<int, string>) Session["CompImbSeqs"];
            new FrameworkUAD.BusinessLogic.Issue().ArchiveAll(myProductID, myIssueID, imbSeqs, compImbSeqs, CurrentClient.ClientConnections);

        }
        private void UpdateOriginalRecords(int productID)
        {
            int myProductID = productID;
            int counter = 0;
            while (counter < 2)
            {
                try
                {
                    new FrameworkUAD.BusinessLogic.WaveMailingDetail().UpdateOriginalSubInfo(myProductID, CurrentUser.UserID, CurrentClient.ClientConnections);
                    counter = 2;
                }
                catch (Exception)
                {
                    counter++;
                }
            }
        }
        private bool ValidateArchive(int CurrentIssueID, int CurrentProductID)
        {
            int myIssueID = CurrentIssueID;
            int myProductID = CurrentProductID;
            bool result = new FrameworkUAD.BusinessLogic.Issue().ValidateArchive(myProductID, myIssueID, CurrentClient.ClientConnections);
            return result;
        }
        private void RollBackIssue(IssueFinalizeViewModel model)
        {
            int myIssueID = model.CurrentIssueID;
            int myProductID = model.CurrentProductID;
            int origIMB = model.OrginalIMB;
            new FrameworkUAD.BusinessLogic.Issue().RollBackIssue(myProductID, myIssueID, origIMB, CurrentClient.ClientConnections);

        }
        private string CreateIMBSeq(int imb, string id, int maxIMB)
        {
            int count = id.Length;
            int fill = 15 - count;

            string retIMB = imb.ToString();
            if (retIMB.Length > fill)
            {
                maxIMB = 2;
                retIMB = "1";
            }
            else
                maxIMB++;

            for (int i = retIMB.Length; i < fill; i++)
            {
                retIMB = "0" + retIMB;
            }

            return retIMB;
        }
    }

}