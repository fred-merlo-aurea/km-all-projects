using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using KM.Common;
using KM.Common.Extensions;
using UAS.Web.Controllers.Common;
using UAS.Web.Models.Circulations;
using AccessEnum = KMPlatform.Enums.Access;
using FeatureEnum = KMPlatform.Enums.ServiceFeatures;
using ServiceEnum = KMPlatform.Enums.Services;

namespace UAS.Web.Controllers.Circulations
{
   
    public class AddKillController : BaseController
    {
        private const string NameAdd = "Add";
        private const string NameRemove = "Remove";

        // GET: AddKill
        public ActionResult Index(int ProductID = 0)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                AddKillProductViewModel model = new AddKillProductViewModel();
                var product = new FrameworkUAD.BusinessLogic.Product().Select(ProductID, CurrentClient.ClientConnections);
                bool isAddRemoveOpened = product.AddRemoveAllowed.HasValue ? (bool) product.AddRemoveAllowed : false;
                if (product != null && isAddRemoveOpened)
                {
                    model.ProductID = ProductID;
                    model.RecordCounts = new FrameworkUAD.Object.Counts();

                    int paidRecords = 0;
                    int freeRecords = 0;
                    int paidCopies = 0;
                    int freeCopies = 0;

                    FrameworkUAD.BusinessLogic.ProductSubscription subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                    List<FrameworkUAD.Entity.ActionProductSubscription> subscriptionList = subscriptionWorker.SelectProductID(model.ProductID, CurrentClient.ClientConnections);
                    subscriptionList = subscriptionList.Where(x => x.CategoryCodeValue != 70 && x.CategoryCodeValue != 71 &&
                                    x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ") &&
                                    x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") &&
                                    x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " ") &&
                                    x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " ")).ToList();
                    //List<FrameworkUAD.Entity.ActionProductSubscription> pubs = this.SubscriptionList.Where(x => this.PendingSubscriptionList.Contains(x.PubSubscriptionID)).ToList();
                    subscriptionList.ForEach(x =>
                    {
                        if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ") ||
                            x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " "))
                        {
                            freeRecords++;
                            freeCopies += x.Copies;
                        }
                        else if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ") ||
                            x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))
                        {
                            paidRecords++;
                            paidCopies += x.Copies;
                        }
                    });

                    model.RecordCounts.FreeCopies = freeCopies;
                    model.RecordCounts.FreeRecords = freeRecords;
                    model.RecordCounts.PaidCopies = paidCopies;
                    model.RecordCounts.PaidRecords = paidRecords;

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { errorType = "AddRemoveNotAllowed" });
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }

        [HttpPost]
        public ActionResult CreateAddKillContainer(string filters, int ProductID, string AddRemove, string AddKillList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                FrameworkUAD.Object.FilterMVC filter = new FrameworkUAD.Object.FilterMVC();
                AddKillProductViewModel model = new AddKillProductViewModel();
                List<int> currentUsedSubscriberIDs = new List<int>();

                #region Create Filter
                filters = filters.Replace("\\", "").TrimStart('"').TrimEnd('"');

                string removeString = "{\"filters\":";
                int index = filters.IndexOf(removeString);
                string cleanPath = (index < 0) ? filters : filters.Remove(index, removeString.Length);

                filters = cleanPath;
                filters = filters.Substring(0, filters.Length - 1);

                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                var mappings = jf.FromJson<FrameworkUAD.Object.FilterMVC[]>(filters);

                filter = mappings[0];
                #endregion

                #region Build AddKillContainer(s) 
                if (AddKillList != "{\"AddKillList\":null}")
                {
                    AddKillList = AddKillList.Replace("\\", "").TrimStart('"').TrimEnd('"');

                    string removeString2 = "{\"AddKillList\":";
                    int index2 = AddKillList.IndexOf(removeString2);
                    AddKillList = (index2 < 0) ? AddKillList : AddKillList.Remove(index2, removeString2.Length);
                    AddKillList = AddKillList.Substring(0, AddKillList.Length - 1);

                    var mappings2 = jf.FromJson<AddKillContainer[]>(AddKillList);

                    List<AddKillContainer> listAddKillContainer = new List<AddKillContainer>();
                    foreach (var m in mappings2)
                    {
                        listAddKillContainer.Add(m);
                    }

                    foreach (AddKillContainer akc in listAddKillContainer)
                    {
                        currentUsedSubscriberIDs.AddRange(akc.SubscriberIDs);
                    }
                }
                #endregion

                //int ProductID = model.ProductID;
                //string AddRemove = model.AddRemove;
                var product = new FrameworkUAD.BusinessLogic.Product().Select(ProductID, CurrentClient.ClientConnections);
                model.PubID = product.PubID;
                model.PubName = product.PubName;

                //model.subscriptionList = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectAllActiveIDs(model.PubID, CurrentClient.ClientConnections);            
                //model.RemainingPool = new List<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));
                //var activeSubscriptionIDs = new HashSet<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));      

                FrameworkUAD.BusinessLogic.ProductSubscription subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                List<FrameworkUAD.Entity.ActionProductSubscription> subscriptionList = subscriptionWorker.SelectProductID(model.PubID, CurrentClient.ClientConnections);
                List<FrameworkUAD.Entity.ActionProductSubscription> activeSubsList = subscriptionList.Where(x => x.CategoryCodeValue != 70 && x.CategoryCodeValue != 71 &&
                    x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ") &&
                    x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") &&
                    x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " ") &&
                    x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " ")).ToList();
                List<int> activeSubscriptionIDs = activeSubsList.Select(x => x.PubSubscriptionID).ToList();
                List<int> subscriptionPool = subscriptionList.Select(x => x.PubSubscriptionID).ToList();

                model.subscriptionList = subscriptionList.Select(x => new FrameworkUAD.Entity.CopiesProductSubscription() { PubSubscriptionID = x.PubSubscriptionID, Copies = x.Copies }).ToList();
                model.RemainingPool = new List<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));
                //var activeSubscriptionIDs = new HashSet<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));

                string filterQuery = "";
                filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);

                int count = 0;
                List<int> subIDs = new List<int>();
                var subCountResponse = new FrameworkUAD.BusinessLogic.Report().SelectSubscriberCountMVC(filterQuery, CurrentClient.ClientConnections);
                if (subCountResponse != null && subCountResponse.Count > 0)
                {
                    subIDs = subCountResponse;
                    //Remove SubscriberIDs already used
                    subIDs = subIDs.Except(currentUsedSubscriberIDs).ToList();
                }

                if (AddRemove.Equals("Add", StringComparison.CurrentCultureIgnoreCase))
                {
                    subIDs = subIDs.Except(activeSubscriptionIDs).ToList();
                    subIDs = subIDs.Intersect(subscriptionPool).ToList();
                    count = subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                }
                else if (AddRemove.Equals("Remove", StringComparison.CurrentCultureIgnoreCase))
                {
                    subIDs = subIDs.Intersect(activeSubscriptionIDs).ToList();
                    subIDs = subIDs.Intersect(subscriptionPool).ToList();
                    count = subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                }

                filter.SubscriberIDs = subIDs;

                if (count > 0)
                {
                    string containerId = "";
                    if (filter.FilterName == null || filter.FilterName == "")
                        containerId = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(Core_AMS.Utilities.StringFunctions.GenerateProcessCode());
                    else
                        containerId = filter.FilterName;

                    filter.FilterName = containerId;
                    AddKillContainer akc = new AddKillContainer(containerId, count, filter, subIDs, model.PubID, AddRemove);
                    return Json(new { Complete = true, AddKillObject = akc, AddRemoveRecords = subIDs.Count, AddRemoveCopies = count }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AddKillContainer akc = new AddKillContainer("", count, filter, subIDs, model.PubID, AddRemove);
                    return Json(new { Complete = false, AddKillObject = akc, AddRemoveRecords = 0, AddRemoveCopies = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpPost]
        public ActionResult CombineIntoGrid(string AddKillList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                #region Build AddKillContainer(s)
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                AddKillList = AddKillList.Replace("\\", "").TrimStart('"').TrimEnd('"');

                string removeString = "{\"AddKillList\":";
                int index = AddKillList.IndexOf(removeString);
                AddKillList = (index < 0) ? AddKillList : AddKillList.Remove(index, removeString.Length);
                AddKillList = AddKillList.Substring(0, AddKillList.Length - 1);

                var mappings = jf.FromJson<AddKillContainer[]>(AddKillList);

                List<AddKillContainer> listAddKillContainer = new List<AddKillContainer>();
                foreach (var m in mappings)
                {
                    listAddKillContainer.Add(m);
                }
                #endregion

                return PartialView("_addKillGrid", listAddKillContainer);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpPost]
        public ActionResult UpdateAddKillSelection(string addKillList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, ServiceEnum.FULFILLMENT, FeatureEnum.OC, AccessEnum.FullAccess))
            {
                const string removeString = "{\"AddKillList\":";
                var args = new UpdateAddKillArgs();

                addKillList = addKillList.Replace("\\", "").TrimStart('"').TrimEnd('"');
                var index = addKillList.IndexOf(removeString, StringComparison.Ordinal);
                addKillList = index < 0 ? addKillList : addKillList.Remove(index, removeString.Length);
                addKillList = addKillList.Substring(0, addKillList.Length - 1);

                var mappings = new Core_AMS.Utilities.JsonFunctions().FromJson<AddKillContainer[]>(addKillList);
                args.AddKills.AddRange(mappings);

                const int codeTypeId = 1;
                const int codeValue = 10;
                var catCodeWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                var catCode = catCodeWorker.Select(codeTypeId, codeValue);
                if (catCode != null)
                {
                    args.AddCatCode = catCode.CategoryCodeID;
                }

                var transCodeWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                var tranCode = transCodeWorker.SelectTransactionCodeValue(codeValue);
                if (tranCode != null)
                {
                    args.AddTransCode = tranCode.TransactionCodeID;
                }

                const int transCodeValue = 38;
                var killTranCode = transCodeWorker.SelectTransactionCodeValue(transCodeValue);
                if (killTranCode != null)
                {
                    args.KillTransCode = killTranCode.TransactionCodeID;
                }

                var singleAkc = args.AddKills.FirstOrDefault();
                if (singleAkc != null)
                {
                    args.PubId = args.AddKills.First().ProductID;
                }

                UpdateAddKillContainers(args);

                return PartialView("_addKillGrid", args.AddKills);
            }

            return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
        }

        private void UpdateAddKillContainers(UpdateAddKillArgs args)
        {
            Guard.NotNull(args, nameof(args));

            var subscriptionList = args.SubscriptionWorker.SelectProductID(args.PubId, CurrentClient.ClientConnections);
            var addKillToUpdate = args.AddKills.Where(x => x.Update)
                .ToList();

            foreach (var addKill in addKillToUpdate)
            {
                var editList = addKill.Filter.SubscriberIDs;
                if (editList.Count > 0)
                {
                    var pendingSubscriptionList = GetPendingSubscriptionList(addKill.Filter.SubscriberIDs, addKill.DesiredCount);

                    SaveSubAddRemove(args, pendingSubscriptionList, subscriptionList, addKill);
                    UpdateAddKillSubscriptions(args, pendingSubscriptionList, subscriptionList, addKill);

                    // Update For Grid Refresh
                    addKill.NotUpdated = false;
                    var oldAddKill = args.AddKills.FirstOrDefault(x => x.ContainerId.EqualsIgnoreCase(addKill.ContainerId));
                    if (oldAddKill != null)
                    {
                        args.AddKills.Remove(oldAddKill);
                        args.AddKills.Add(addKill);
                    }
                }
            }
        }

        private void SaveSubAddRemove(
            UpdateAddKillArgs args,
            IEnumerable<int> pendingSubscriptionList,
            IList<ActionProductSubscription> subscriptionList,
            AddKillContainer addKill)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(pendingSubscriptionList, nameof(pendingSubscriptionList));
            Guard.NotNull(subscriptionList, nameof(subscriptionList));
            Guard.NotNull(addKill, nameof(addKill));

            var subAddKill = new SubscriberAddKill
            {
                CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                PublicationID = addKill.ProductID,
                AddKillCount = addKill.DesiredCount,
                Count = addKill.ActualCount,
                Type = addKill.Type
            };

            var addRemoveId = args.SubAddKillWorker.Save(subAddKill, CurrentClient.ClientConnections);
            var details = new List<SubscriberAddKillDetail>();

            if (addKill.Type.EqualsIgnoreCase(NameAdd))
            {
                details.AddRange(pendingSubscriptionList.Select(id => new SubscriberAddKillDetail(addRemoveId, args.AddCatCode, args.AddTransCode, id)));
            }
            else
            {
                foreach (var id in pendingSubscriptionList)
                {
                    var categoryId = 0;
                    var actionPubSub = subscriptionList.FirstOrDefault(x => x.PubSubscriptionID == id);
                    if (actionPubSub != null)
                    {
                        categoryId = actionPubSub.PubCategoryID;
                    }

                    details.Add(new SubscriberAddKillDetail(addRemoveId, categoryId, args.KillTransCode, id));
                }
            }

            args.SubAddKillWorker.BulkInsertDetail(details, addRemoveId, CurrentClient.ClientConnections);
        }

        private void UpdateAddKillSubscriptions(
            UpdateAddKillArgs args,
            IEnumerable<int> pendingSubscriptionList,
            IList<ActionProductSubscription> subscriptionList,
            AddKillContainer addKill)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(pendingSubscriptionList, nameof(pendingSubscriptionList));
            Guard.NotNull(subscriptionList, nameof(subscriptionList));
            Guard.NotNull(addKill, nameof(addKill));

            var idHashSet = new HashSet<int>(pendingSubscriptionList);
            var workingList = subscriptionList.Where(x => idHashSet.Contains(x.PubSubscriptionID)).ToList();
            var total = workingList.Count;
            var counter = 0;
            var processedCount = 0;
            const int batchSize = 1000;
            var xml = "<XML>";

            if (addKill.Type.EqualsIgnoreCase(NameAdd))
            {
                foreach (var item in workingList)
                {
                    xml =
                        $"{xml}<Subscription><SubscriptionID>{item.PubSubscriptionID}</SubscriptionID><PubCategoryID>{args.AddCatCode}</PubCategoryID><PubTransactionID>{args.AddTransCode}</PubTransactionID></Subscription>";
                    counter++;
                    processedCount++;
                    if (processedCount == total || counter == batchSize)
                    {
                        xml = $"{xml}</XML>";
                        args.SubscriptionWorker.SaveBulkActionIDUpdate(xml, CurrentClient.ClientConnections);
                        counter = 0;
                        xml = "<XML>";
                    }
                }
            }
            else if (addKill.Type.EqualsIgnoreCase(NameRemove))
            {
                foreach (var item in workingList)
                {
                    xml =
                        $"{xml}<Subscription><SubscriptionID>{item.PubSubscriptionID}</SubscriptionID><PubCategoryID>{item.PubCategoryID}</PubCategoryID><PubTransactionID>{args.KillTransCode}</PubTransactionID></Subscription>";
                    counter++;
                    processedCount++;
                    if (processedCount == total || counter == batchSize)
                    {
                        xml = $"{xml}</XML>";
                        args.SubscriptionWorker.SaveBulkActionIDUpdate(xml, CurrentClient.ClientConnections);
                        counter = 0;
                        xml = "<XML>";
                    }
                }
            }
        }

        public ActionResult DesiredCountPopup(string ContainerID, int CurrentCount)
        {
            Models.Circulations.DesiredCount dcModel = new DesiredCount();
            dcModel.ID = ContainerID;
            dcModel.Count = CurrentCount;

            return PartialView("_desiredCount", dcModel);
        }

        public ActionResult DownloadAddKillFile(string Detail, string AddKillList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                FrameworkUAD.BusinessLogic.ProductSubscription subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();

                #region Build Download Columns            
                Detail = Detail.Replace("\\", "").TrimStart('"').TrimEnd('"');

                string removeString = "{\"Detail\":[";
                int index = Detail.IndexOf(removeString);
                Detail = (index < 0) ? Detail : Detail.Remove(index, removeString.Length);
                Detail = Detail.Substring(0, Detail.Length - 1);

                var mappings = jf.FromJson<DownloadColumns[]>(Detail);
                List<DownloadColumns> downloadColumns = new List<DownloadColumns>();
                foreach (var m in mappings)
                {
                    downloadColumns.Add(m);
                }
                #endregion

                #region Build AddKillContainer(s)            
                AddKillList = AddKillList.Replace("\\", "").TrimStart('"').TrimEnd('"');

                string removeString2 = "{\"AddKillList\":";
                int index2 = AddKillList.IndexOf(removeString2);
                AddKillList = (index2 < 0) ? AddKillList : AddKillList.Remove(index2, removeString2.Length);
                AddKillList = AddKillList.Substring(0, AddKillList.Length - 1);

                var mappings2 = jf.FromJson<AddKillContainer[]>(AddKillList);

                List<AddKillContainer> listAddKillContainer = new List<AddKillContainer>();
                foreach (var m in mappings2)
                {
                    listAddKillContainer.Add(m);
                }
                #endregion

                #region Variables     
                int myPubID = 0;
                AddKillContainer singleAkc = listAddKillContainer.FirstOrDefault();
                if (singleAkc != null)
                    myPubID = listAddKillContainer.FirstOrDefault().ProductID;

                System.Data.DataTable dt = new System.Data.DataTable();
                string columns = "";

                columns = String.Join(",", downloadColumns.Select(x => x.DownloadName).ToList());

                List<int> subIDs = new List<int>();
                foreach (AddKillContainer akc in listAddKillContainer)
                {
                    List<int> PendingSubscriptionList = GetPendingSubscriptionList(akc.Filter.SubscriberIDs, akc.DesiredCount);
                    subIDs.AddRange(PendingSubscriptionList);
                }
                HashSet<int> hs = new HashSet<int>(subIDs);
                System.Data.DataTable master = new System.Data.DataTable();
                #endregion

                #region GetSubscribers
                int rowProcessedCount = 0;
                int index3 = 0;
                int size = 2500;
                while (master.Rows.Count < hs.Count)
                {
                    if ((index3 + 2500) > subIDs.Count)
                        size = subIDs.Count - index3;
                    List<int> temp = subIDs.GetRange(index3, size);
                    index3 += 2500;
                    dt = subscriptionWorker.Select_For_Export_Static(myPubID, columns, temp, CurrentClient.ClientConnections);
                    rowProcessedCount += dt.Rows.Count;

                    dt.AcceptChanges();
                    master.Merge(dt);
                }
                #endregion

                #region Create File And Return
                if (master.Columns.Contains("PubSubscriptionID"))
                    master.Columns.Remove("PubSubscriptionID");
                master.AcceptChanges();


                var str = UAS.Web.Helpers.IEnumerableExtention.DataTableToCSV(master, ',');
                string sanitizedName = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(Core_AMS.Utilities.StringFunctions.GenerateProcessCode());
                CreateCSVFile(sanitizedName, str, master);

                try
                {
                    var archive = Server.MapPath("../addkilldownloads/main/" + sanitizedName + ".csv");

                    return Json(new { error = false, successmessage = "File exported successfully", filedownloadpath = archive }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { error = true, errrormessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                #endregion
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult DownLoadFile(string fileloc)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileloc);
            string contentType = "";
            contentType = MimeMapping.GetMimeMapping(fileloc);
            System.IO.File.Delete(fileloc);
            return File(fileBytes, contentType, "Export.tsv");
        }

        private void CreateCSVFile(string splitName, string str, System.Data.DataTable masterClone)
        {
            string path = Server.MapPath("../addkilldownloads/main/");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

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
        }

        public ActionResult SetupPreviewData(string AddKillList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                #region BusinessLogic
                FrameworkUAD.BusinessLogic.SubscriberAddKill subAddKillWorker = new FrameworkUAD.BusinessLogic.SubscriberAddKill();
            FrameworkUAD.BusinessLogic.ProductSubscription subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
            FrameworkUAD_Lookup.BusinessLogic.CategoryCode catCodeWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
            FrameworkUAD_Lookup.BusinessLogic.TransactionCode transCodeWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
            #endregion            

                #region Build AddKillContainer(s)
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                AddKillList = AddKillList.Replace("\\", "").TrimStart('"').TrimEnd('"');

                string removeString = "{\"AddKillList\":";
                int index = AddKillList.IndexOf(removeString);
                AddKillList = (index < 0) ? AddKillList : AddKillList.Remove(index, removeString.Length);
                AddKillList = AddKillList.Substring(0, AddKillList.Length - 1);

                var mappings = jf.FromJson<AddKillContainer[]>(AddKillList);

                List<AddKillContainer> listAddKillContainer = new List<AddKillContainer>();
                foreach (var m in mappings)
                {
                    listAddKillContainer.Add(m);
                }
                #endregion

                #region Variable Setup
                bool hasError = false;
                string Message = "";
                int addCatCode = 0;
                int addTransCode = 0;
                int killTransCode = 0;

                FrameworkUAD_Lookup.Entity.CategoryCode catCode = catCodeWorker.Select(1, 10);
                if (catCode != null)
                    addCatCode = catCode.CategoryCodeID;

                FrameworkUAD_Lookup.Entity.TransactionCode tranCode = transCodeWorker.SelectTransactionCodeValue(10);
                if (tranCode != null)
                    addTransCode = tranCode.TransactionCodeID;

                FrameworkUAD_Lookup.Entity.TransactionCode killTranCode = transCodeWorker.SelectTransactionCodeValue(38);
                if (killTranCode != null)
                    killTransCode = killTranCode.TransactionCodeID;

                List<FrameworkUAD.Entity.ActionProductSubscription> subscriptionList = new List<FrameworkUAD.Entity.ActionProductSubscription>();
                int myPubID = 0;
                AddKillContainer singleAkc = listAddKillContainer.FirstOrDefault();
                if (singleAkc != null)
                    myPubID = listAddKillContainer.FirstOrDefault().ProductID;

                subscriptionList = subscriptionWorker.SelectProductID(myPubID, CurrentClient.ClientConnections);
                #endregion

                #region PreviewData
                bool boolResponse = subAddKillWorker.ClearDetails(myPubID, CurrentClient.ClientConnections);

                foreach (AddKillContainer akc in listAddKillContainer)
                {
                    int addRemoveID = 0;
                    FrameworkUAD.Entity.SubscriberAddKill s = new FrameworkUAD.Entity.SubscriberAddKill();
                    s.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    s.PublicationID = myPubID;
                    s.AddKillCount = akc.DesiredCount;
                    s.Count = akc.ActualCount;
                    s.Type = akc.Type.ToString();

                    List<FrameworkUAD.Entity.SubscriberAddKillDetail> details = new List<FrameworkUAD.Entity.SubscriberAddKillDetail>();

                    List<int> PendingSubscriptionList = GetPendingSubscriptionList(akc.Filter.SubscriberIDs, akc.DesiredCount);

                    if (akc.Type.Equals("Add", StringComparison.CurrentCultureIgnoreCase))
                    {
                        foreach (int i in PendingSubscriptionList)
                            details.Add(new FrameworkUAD.Entity.SubscriberAddKillDetail(addRemoveID, addCatCode, addTransCode, i));
                    }
                    else
                    {
                        foreach (int i in PendingSubscriptionList)
                        {
                            int cat = 0;
                            FrameworkUAD.Entity.ActionProductSubscription a = subscriptionList.Where(x => x.PubSubscriptionID == i).FirstOrDefault();
                            if (a != null)
                                cat = a.PubCategoryID;
                            details.Add(new FrameworkUAD.Entity.SubscriberAddKillDetail(addRemoveID, cat, killTransCode, i));
                        }
                    }

                    boolResponse = subAddKillWorker.BulkInsertDetail(details, addRemoveID, CurrentClient.ClientConnections);
                    if (boolResponse == false)
                    {
                        hasError = true;
                        Message = "There was an issue loading the preview data.";
                        break;
                    }
                }

                if (!hasError)
                    Message = "Add Remove preview data has finished loading. Please use the 'Include Add Removes' parameter on the desired reports to view how the preview data will affect your reports.";

                #endregion

                return Json(new { Error = hasError, retMessage = Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public List<int> GetPendingSubscriptionList(List<int> subscriptionList, int desiredCount)
        {
            List<int> pendingSubscriptionList = new List<int>();

            pendingSubscriptionList = GetNthSubscribers(subscriptionList.Count, desiredCount, subscriptionList.ToList());

            return pendingSubscriptionList;
        }

        public List<int> GetNthSubscribers(int totalRecords, int requestedRecords, List<int> idSourceList)
        {
            List<int> LNth = getNth(totalRecords, requestedRecords);
            List<int> ids = new List<int>();

            foreach (int n in LNth)
            {
                var id = idSourceList.Skip(n).Take(1);
                ids.Add(id.ToList().FirstOrDefault());
            }

            return ids;
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

        public ActionResult GetAddDefaultFilters()
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                #region CatType
                List<SelectListItem> selectCatTypeList = new List<SelectListItem>();
                #endregion
                #region CatCode
                FrameworkUAD_Lookup.Entity.CategoryCode catCode = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select(1, 70);
                List<SelectListItem> selectListcatCode = new List<SelectListItem>();
                selectListcatCode.Add(new SelectListItem()
                {
                    Text = catCode.CategoryCodeValue + ". " + catCode.CategoryCodeName.ToUpper(),
                    Value = catCode.CategoryCodeID.ToString(),
                    Selected = true

                });
                #endregion
                #region TranType
                List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xCodeTypeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select();
                FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActiveFree = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == true).First();
                List<SelectListItem> selectListXCodeType = new List<SelectListItem>();
                selectListXCodeType.Add(new SelectListItem()
                {
                    Text = xCodeTypeActiveFree.TransactionCodeTypeName.ToUpper(),
                    Value = xCodeTypeActiveFree.TransactionCodeTypeID.ToString(),
                    Selected = true
                });
                #endregion
                #region TranCode
                var selectListxCodeList = GenerateXCodeList(xCodeTypeActiveFree);
                #endregion

                return Json(new { cc = selectListcatCode, ct = selectCatTypeList, xc = selectListxCodeList, xt = selectListXCodeType }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult GetRemoveDefaultFilters()
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                #region CatType
                List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select();
                var QualFreeCatType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select("Qualified Free");
                List<SelectListItem> selectCatTypeList = new List<SelectListItem>();
                selectCatTypeList.Add(new SelectListItem()
                {
                    Text = QualFreeCatType.CategoryCodeTypeName.ToUpper(),
                    Value = QualFreeCatType.CategoryCodeTypeID.ToString(),
                    Selected = true
                });
                #endregion
                #region CatCode
                List<FrameworkUAD_Lookup.Entity.CategoryCode> catCode = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select().Where(x => (x.CategoryCodeTypeID == QualFreeCatType.CategoryCodeTypeID) && x.CategoryCodeValue != 70).ToList();
                List<SelectListItem> selectListcatCode = new List<SelectListItem>();
                catCode.ForEach(c => selectListcatCode.Add(new SelectListItem()
                {
                    Text = c.CategoryCodeValue + ". " + c.CategoryCodeName.ToUpper(),
                    Value = c.CategoryCodeID.ToString(),
                    Selected = true

                }));
                #endregion
                #region TranType
                List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xCodeTypeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select();
                FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActiveFree = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == true).First();
                List<SelectListItem> selectListXCodeType = new List<SelectListItem>();
                selectListXCodeType.Add(new SelectListItem()
                {
                    Text = xCodeTypeActiveFree.TransactionCodeTypeName.ToUpper(),
                    Value = xCodeTypeActiveFree.TransactionCodeTypeID.ToString(),
                    Selected = true
                });
                #endregion
                #region TranCode
                var selectListxCodeList = GenerateXCodeList(xCodeTypeActiveFree);
                #endregion

                return Json(new { cc = selectListcatCode, ct = selectCatTypeList, xc = selectListxCodeList, xt = selectListXCodeType }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public List<SelectListItem> GenerateXCodeList(TransactionCodeType xCodeTypeActiveFree)
        {
            if (xCodeTypeActiveFree == null)
            {
                throw new ArgumentNullException(nameof(xCodeTypeActiveFree));
            }

            var xCodeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select()
                .Where(x => x.TransactionCodeTypeID == xCodeTypeActiveFree.TransactionCodeTypeID)
                .ToList();

            var selectListxCodeList = new List<SelectListItem>();
            xCodeList.ForEach(c => selectListxCodeList.Add(new SelectListItem()
            {
                Text = string.Format("{0}. {1}", c.TransactionCodeValue, c.TransactionCodeName.ToUpper()),
                Value = c.TransactionCodeID.ToString(),
                Selected = true
            }));

            return selectListxCodeList;
        }

        public ActionResult GetUpdateTotals(string Filter, string AddRemove, int DesireCount, bool IsDelete = false)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                FrameworkUAD.Object.FilterMVC filter = new FrameworkUAD.Object.FilterMVC();
                AddKillContainer model = new AddKillContainer();

                #region Create Filter
                Filter = Filter.Replace("\\", "").TrimStart('"').TrimEnd('"');

                string removeString = "{\"Filter\":";
                int index = Filter.IndexOf(removeString);
                string cleanPath = (index < 0) ? Filter : Filter.Remove(index, removeString.Length);

                Filter = cleanPath;
                Filter = Filter.Substring(0, Filter.Length - 1);

                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                var mappings = jf.FromJson<AddKillContainer[]>(Filter);

                model = mappings[0];
                filter = mappings[0].Filter;
                #endregion

                FrameworkUAD.BusinessLogic.ProductSubscription subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                List<FrameworkUAD.Entity.ActionProductSubscription> subscriptionList = subscriptionWorker.SelectProductID(model.ProductID, CurrentClient.ClientConnections);

                string filterQuery = "";
                filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);

                int returnDesireCount = DesireCount;

                int count = 0;
                List<int> PendingSubscriptionList = GetPendingSubscriptionList(filter.SubscriberIDs, DesireCount);
                count = subscriptionList.Where(x => PendingSubscriptionList.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                int returnCount = count;

                if (IsDelete)
                {
                    //DesireCount = model.ActualCount;
                    if (AddRemove.Equals("Add", StringComparison.CurrentCultureIgnoreCase))
                        returnDesireCount = DesireCount * -1;
                    else
                        returnDesireCount = DesireCount * -1;

                    returnCount = count * -1;
                }


                int records = returnDesireCount;
                int copies = returnCount;
                string type = AddRemove;

                return Json(new { AddRemoveRecords = records, AddRemoveCopies = copies, ContainerType = type }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }

        public ActionResult RecalculateTotals(string Filter, string AddRemove, int NewDesireCount, int OldDesireCount)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                FrameworkUAD.Object.FilterMVC filter = new FrameworkUAD.Object.FilterMVC();
                AddKillContainer model = new AddKillContainer();

                #region Create Filter
                Filter = Filter.Replace("\\", "").TrimStart('"').TrimEnd('"');

                string removeString = "{\"Filter\":";
                int index = Filter.IndexOf(removeString);
                string cleanPath = (index < 0) ? Filter : Filter.Remove(index, removeString.Length);

                Filter = cleanPath;
                Filter = Filter.Substring(0, Filter.Length - 1);

                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                var mappings = jf.FromJson<AddKillContainer[]>(Filter);

                model = mappings[0];
                filter = mappings[0].Filter;
                #endregion

                FrameworkUAD.BusinessLogic.ProductSubscription subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                List<FrameworkUAD.Entity.ActionProductSubscription> subscriptionList = subscriptionWorker.SelectProductID(model.ProductID, CurrentClient.ClientConnections);

                string filterQuery = "";
                filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);

                int newCount = 0;
                int oldCount = 0;
                int count = 0;
                List<int> PendingSubscriptionList = GetPendingSubscriptionList(filter.SubscriberIDs, NewDesireCount);
                List<int> OldPendingSubscriptionList = GetPendingSubscriptionList(filter.SubscriberIDs, OldDesireCount);
                newCount = subscriptionList.Where(x => PendingSubscriptionList.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                oldCount = subscriptionList.Where(x => OldPendingSubscriptionList.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();

                int returnRecords = 0;
                int returnCopies = 0;

                if (AddRemove.Equals("Add", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (OldDesireCount > NewDesireCount)
                    {
                        //Negative
                        returnRecords = NewDesireCount - OldDesireCount;
                        returnCopies = newCount - oldCount;
                    }
                    else
                    {
                        //Positive
                        returnRecords = NewDesireCount - OldDesireCount;
                        returnCopies = newCount - oldCount;
                    }
                }
                else if (AddRemove.Equals("Remove", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (OldDesireCount > NewDesireCount)
                    {
                        //Positive
                        returnRecords = NewDesireCount - OldDesireCount;
                        returnCopies = newCount - oldCount;
                    }
                    else
                    {
                        //Negative
                        returnRecords = NewDesireCount - OldDesireCount;
                        returnCopies = newCount - oldCount;
                    }
                }

                int records = returnRecords;
                int copies = returnCopies;
                string type = AddRemove;

                return Json(new { AddRemoveRecords = records, AddRemoveCopies = copies, ContainerType = type }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult ExportColumnLists(int pubID, bool includeSplitsColumns = false)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
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
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult GetAddKill(FrameworkUAD.Object.FilterMVC filter, List<FrameworkUAD.Object.FilterMVC> filterscol, AddKillProductViewModel model)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                var product = new FrameworkUAD.BusinessLogic.Product().Select(model.PubID, CurrentClient.ClientConnections);
                model.PubID = product.PubID;
                model.PubName = product.PubName;

                model.subscriptionList = new FrameworkUAD.BusinessLogic.ProductSubscription().SelectAllActiveIDs(model.PubID, CurrentClient.ClientConnections);
                model.RemainingPool = new List<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));

                #region idk
                //var issueResponse = new FrameworkUAD.BusinessLogic.Issue().SelectPublication(model.PubID, CurrentClient.ClientConnections);
                //model.Issue = issueResponse.Where(x => x.IssueId == model.IssueID).FirstOrDefault();
                //if (model.Issue.IsComplete == false)
                //    model.ReadOnly = false;
                //else
                //    model.ReadOnly = true;

                //List<int> compSubIDs = new List<int>();
                //var issueCompResponse = new FrameworkUAD.BusinessLogic.IssueComp().SelectIssue(model.Issue.IssueId, CurrentClient.ClientConnections);
                //List<FrameworkUAD.Entity.IssueCompDetail> comps = new List<FrameworkUAD.Entity.IssueCompDetail>();
                //if (issueCompResponse != null && issueCompResponse.Count > 0)
                //{
                //    int id = issueCompResponse.Where(x => x.IsActive == true).LastOrDefault().IssueCompId;
                //    var myComp = issueCompResponse.Where(x => x.IsActive == true).FirstOrDefault();
                //    var myIssueCompID = id;
                //    var issueCompDetailResponse = new FrameworkUAD.BusinessLogic.IssueCompDetail().Select(id, CurrentClient.ClientConnections);
                //    if (issueCompDetailResponse != null && issueCompDetailResponse.Count > 0)
                //    {
                //        int tempSubID = model.RemainingPool.Max();
                //        tempSubID += 500000;

                //        comps = issueCompDetailResponse;

                //        foreach (FrameworkUAD.Entity.IssueCompDetail comp in comps)
                //        {
                //            FrameworkUAD.Entity.ProductSubscription sp = new FrameworkUAD.Entity.ProductSubscription(comp);
                //            tempSubID++;
                //            sp.PubSubscriptionID = tempSubID;
                //            comp.PubSubscriptionID = tempSubID;
                //            //allProductSubscriptions.Add(sp);
                //            model.subscriptionList.Add(new FrameworkUAD.Entity.CopiesProductSubscription() { PubSubscriptionID = tempSubID, Copies = comp.Copies });
                //            //activeSubscriptionIDs.Add(tempSubID);
                //            model.RemainingPool.Add(tempSubID);
                //            compSubIDs.Add(tempSubID);
                //        }
                //    }
                //}
                #endregion
                var activeSubscriptionIDs = new HashSet<int>(model.subscriptionList.Select(x => x.PubSubscriptionID));

                model.FilterCollection = new FrameworkUAD.Object.FilterCollection(CurrentClient.ClientConnections, CurrentUser.UserID);
                foreach (var f in filterscol)
                {
                    f.FilterNo = model.FilterCollection.Count() + 1;
                    model.FilterCollection.Add(f);
                }
                var tempSplitsList = GetSplitList(model, activeSubscriptionIDs);

                //if (tempSplitsList != null && tempSplitsList.Count == 0)
                //{
                //    return Json(new { error = true, errormessage = "No results to display." });
                //}
                #region Genrate Script                
                #endregion
                model.AddKillList = tempSplitsList;
                return PartialView("_addKillGrid", model.AddKillList);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        private List<AddKillContainer> GetSplitList(AddKillProductViewModel model, HashSet<int> activeSubscriptionIDs)
        {
            List<AddKillContainer> tempSplitsList = new List<AddKillContainer>();
            foreach (var filter in model.FilterCollection)
            {
                //FrameworkUAD.Entity.IssueSplit split = new FrameworkUAD.Entity.IssueSplit();
                if (filter.FilterDescription != model.AddRemove)
                    model.AddRemove = filter.FilterDescription;

                string filterQuery = "";
                filterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);

                int count = 0;
                List<int> subIDs = new List<int>();
                var subCountResponse = new FrameworkUAD.BusinessLogic.Report().SelectSubscriberCountMVC(filterQuery, CurrentClient.ClientConnections);
                if (subCountResponse != null && subCountResponse.Count > 0)
                {
                    subIDs = subCountResponse.Where(x => activeSubscriptionIDs.Contains(x)).ToList();
                }
                filter.SubscriberIDs = subIDs;
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
                count = model.subscriptionList.Select(x => x.PubSubscriptionID).Intersect(subIDs).Count(); //Perfomence improvement on Below
                //count = model.subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                if (count > 0)
                {
                    string containerId = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(Core_AMS.Utilities.StringFunctions.GenerateProcessCode());
                    tempSplitsList.Add(new AddKillContainer(containerId, count, filter, subIDs, model.PubID, model.AddRemove));
                }
            }

            return tempSplitsList;
        }

        public ActionResult GetReports(int pubID)
        {
            List<FrameworkUAD.Entity.Report> reportList = new List<FrameworkUAD.Entity.Report>();
            var ReportType = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Report, "Cross Tab");
            List<string> rptAddRmvLst = new List<string>() { "3b Qualification Source Breakdown", "3b - Qualification Source Breakdown", "", "Geographical Breakdown - Domestic", "SubSource Summary" ,"BUSINESS X ASSOCIATIONS", "BUSINESS X FUNCTION" };
            reportList.Add(new FrameworkUAD.Entity.Report() { ReportID = -1, ReportName = "--Select Report--", ProductID = pubID });
            var reports = new FrameworkUAD.BusinessLogic.Report().Select(CurrentClient.ClientConnections);
            var rptList = reports.Where(x => x.ProductID == pubID && (x.ReportTypeID== ReportType.CodeId || rptAddRmvLst.Contains(x.ReportName)) ).OrderBy(x => x.ReportName).ToList();
            reportList.AddRange(rptList);
            return Json(reportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewTotalsAfterUpdate(int ProductID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                int paidRecords = 0;
                int freeRecords = 0;
                int paidCopies = 0;
                int freeCopies = 0;

                FrameworkUAD.BusinessLogic.ProductSubscription subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
                List<FrameworkUAD.Entity.ActionProductSubscription> subscriptionList = subscriptionWorker.SelectProductID(ProductID, CurrentClient.ClientConnections);
                subscriptionList = subscriptionList.Where(x => x.CategoryCodeValue != 70 && x.CategoryCodeValue != 71 &&
                                x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ") &&
                                x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") &&
                                x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " ") &&
                                x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " ")).ToList();
                //List<FrameworkUAD.Entity.ActionProductSubscription> pubs = this.SubscriptionList.Where(x => this.PendingSubscriptionList.Contains(x.PubSubscriptionID)).ToList();
                subscriptionList.ForEach(x =>
                {
                    if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ") ||
                        x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " "))
                    {
                        freeRecords++;
                        freeCopies += x.Copies;
                    }
                    else if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ") ||
                        x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))
                    {
                        paidRecords++;
                        paidCopies += x.Copies;
                    }
                });
               
                return Json(new { FreeCopies = freeCopies, FreeRecords = freeRecords, PaidCopies = paidCopies, PaidRecords = paidRecords }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult GetSubIdsForDownload(string AddKillList)
        {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();

            #region Build AddKillContainer(s)            
            AddKillList = AddKillList.Replace("\\", "").TrimStart('"').TrimEnd('"');

            string removeString2 = "{\"AddKillList\":";
            int index2 = AddKillList.IndexOf(removeString2);
            AddKillList = (index2 < 0) ? AddKillList : AddKillList.Remove(index2, removeString2.Length);
            AddKillList = AddKillList.Substring(0, AddKillList.Length - 1);

            var mappings2 = jf.FromJson<AddKillContainer[]>(AddKillList);

            List<AddKillContainer> listAddKillContainer = new List<AddKillContainer>();
            foreach (var m in mappings2)
            {
                listAddKillContainer.Add(m);
            }
            #endregion

            List<int> subIDs = new List<int>();
            foreach (AddKillContainer akc in listAddKillContainer)
            {
                List<int> PendingSubscriptionList = GetPendingSubscriptionList(akc.Filter.SubscriberIDs, akc.DesiredCount);
                subIDs.AddRange(PendingSubscriptionList);
            }

            Session["AddRemoveSubscriberIds"] = subIDs;

            return Json(new { Complete = true }, JsonRequestBehavior.AllowGet);
        }
    }
}