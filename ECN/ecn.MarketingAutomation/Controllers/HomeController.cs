using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ecn.MarketingAutomation.Models;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_Common.Objects;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KM.Common.Entity;
using KMSite;
using Newtonsoft.Json;
using BLCommunicator = ECN_Framework_BusinessLayer.Communicator;
using CommonEnum = ECN_Framework_Common.Objects.Enums;
using EntityCommunicator = ECN_Framework_Entities.Communicator;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using PostModelControl = ecn.MarketingAutomation.Models.PostModels.Controls;

namespace ecn.MarketingAutomation.Controllers
{
    public class HomeController : AutomationBaseController
    {
        private const string SuccessResponseCode = "200";
        private const string ErrorResponseCode = "500";
        private const string ActionPause = "Pause";
        private const string ActionUnPause = "UnPause";
        private const string SecurityExceptionMessage = "You do not have permission to resume this automation";
        private const string CommonExceptionMessage = "An error occurred";
        private const string BlastStatusCodePaused = "paused";
        private const string PlanStatusY = "Y";
        private const string PlanStatusP = "P";
        private const string ValidationErrorStr = "Validation Errors: ";
        private const string AutomationMissingMessage = "Automation is missing start or end controls";
        private const string AutomationNotPublishMessage = "Could not Publish automation";
        private const string DiagramPublishMessage = "Diagram published successfully";
        private const string ActionPublish = "Publish";
        private const string StartFromKey = "StartFrom";
        private const string StartToKey = "StartTo";
        private const string GoalKey = "Goal";
        private const string ErrorKey = "Error";
        private const string AutomationPartialViewName = "Partials/_Automation";
        private const string IndexActionName = "Index";
        private const string EditActionName = "Edit";
        private const string DiagramsControllerName= "Diagrams";
        private const string MethodNameSave = "MarketingAutomation.Save";
        private const string AppSettingKmApplication = "KMCommon_Application";
        private const string AutomationNameExistsMessage =
            "Automation Name already exists. Please enter a unique Automation Name.";
        private const string TemplateRequiredMessage = "Please select a template from the list.";
        private const string MethodNameGetGroupAndCampaignItem = "GetGroupAndCampaignItem";

        private readonly IKMAuthenticationManager _kmAuthenticationManager = new KMAuthenticationManager();

        private ECN_Framework_BusinessLayer.Application.ECNSession CurrentSession
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession(); }
        }

        private int CurrentClientGroupID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientGroupID; }
        }

        private int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }

        private int CurrentCustomerID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID; }
        }

        public ActionResult Index()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.MARKETINGAUTOMATION, KMPlatform.Enums.ServiceFeatures.MarketingAutomation, KMPlatform.Enums.Access.FullAccess))
            {
                //List<ECN_Framework_Entities.Communicator.MarketingAutomation> listMA = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
                //listMA = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.SelectByBaseChannelID(CurrentSession.BaseChannelID, CurrentUser);

                //listMA.ForEach(x => CompletedCheckAndStrip(x));

                //HomeModel model = new HomeModel
                //{
                     //ActiveDiagrams = ActiveDiagrams
                //    //Bug 37037:Default sort order of the Automation in the Home Page
                //    ActiveAutomations = listMA.OrderByDescending(x => x.UpdatedDate).ThenByDescending(x => x.CreatedDate)
                //};
                ViewBag.CurrentUser = CurrentUser;
                HomeModel model = new HomeModel();
                return View(model);
            }
            else
            {
                return Redirect("/ecn.accounts/main");
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MAReadToGrid([DataSourceRequest]DataSourceRequest request,string AutomationName="",string State="",string SearchCriteria="", int PageNumber=1)
        {
            KendoGridHelper<MarketingAutomtionViewModel> gh = new KendoGridHelper<MarketingAutomtionViewModel>();
            List<GridSort> lstgs = gh.GetGridSort(request, "UpdatedDate");
            string sortColumn = lstgs[0].SortColumnName;
            string sortdirection = lstgs[0].SortDirection;
            //List of Actual Records
            List<MarketingAutomtionViewModel> ActiveAutomations = new List<MarketingAutomtionViewModel>();
            DataSet ds =  ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetAllMarketingAutomationsbySearch(CurrentSession.BaseChannelID, CurrentUser, AutomationName, State, SearchCriteria, PageNumber,request.PageSize, sortdirection, sortColumn);
            int totalCount = 0;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.AsEnumerable())
            {
               MarketingAutomtionViewModel ma = new MarketingAutomtionViewModel();
               if (totalCount == 0)
                    totalCount = Convert.ToInt32(dr["TotalCount"].ToString());
               ma.TotalRecordCounts = dr["TotalCount"].ToString();
               ma.MarketingAutomationID = Convert.ToInt32(dr["MarketingAutomationID"].ToString());
               ma.Name = dr["Name"].ToString();
               ma.State = dr["State"].ToString();
               ma.StartDate = (DateTime)dr["StartDate"];
               ma.EndDate= (DateTime)dr["EndDate"];
               DateTime dtCreatedDate = (DateTime)dr["CreatedDate"];
               ma.CreatedDate = dtCreatedDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
               if (dr["UpdatedDate"] != DBNull.Value)
                {
                    DateTime dtUpdatedDate = (DateTime)dr["UpdatedDate"];
                    ma.UpdatedDate = dtUpdatedDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                }
               if (dr["LastPublishedDate"] != DBNull.Value)
                {
                    DateTime dtLastPublishedDate = (DateTime)dr["LastPublishedDate"];
                    ma.LastPublishedDate = dtLastPublishedDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                }
                ActiveAutomations.Add(ma);
            }
          
            IQueryable<MarketingAutomtionViewModel> gs = ActiveAutomations.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result);
        }
        private void CompletedCheckAndStrip(ECN_Framework_Entities.Communicator.MarketingAutomation ma)
        {
            if (ma.EndDate.Value.Date < DateTime.Now.Date && ma.State == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Published)
            {
                ma.State = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Completed;
            }
            ma.JSONDiagram = "";
        }



        public ActionResult Archive_UnArchive(int MAID)
        {
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(MAID, true, CurrentUser);
            List<string> response = new List<string>();
            try
            {
                string message = "";
                bool archived = false;
                if (ma.State == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Archived)
                {
                    ma.State = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Published;
                    ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.Save(ma, CurrentUser);
                    ECN_Framework_BusinessLayer.Communicator.MarketingAutomationHistory.Insert(MAID, CurrentUser.UserID, "UnArchive");
                    message = "Automation unarchived";
                    archived = false;
                }
                else if (ma.State == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Published)
                {
                    ma.State = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Archived;
                    ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.Save(ma, CurrentUser);
                    ECN_Framework_BusinessLayer.Communicator.MarketingAutomationHistory.Insert(MAID, CurrentUser.UserID, "Archive");
                    message = "Automation archived";
                    archived = true;
                }
                response.Add("200");
                response.Add(message);
                if (ma.EndDate <= DateTime.Now && !archived)
                    response.Add(((int)ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Completed).ToString());
                else
                    response.Add(((int)ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Archived).ToString());
            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add("Could not update automation");
            }

            return Json(response);
        }

        public ActionResult Pause(int marketingAutomationId)
        {
            var response = TogglePause(marketingAutomationId, true);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnPause(int marketingAutomationId)
            {
            var response = TogglePause(marketingAutomationId, false);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private List<string> TogglePause(int marketingAutomationId, bool isPause)
        {
            var response = new List<string>();
                try
                {
                var marketingAutomation = BLCommunicator.MarketingAutomation.GetByMarketingAutomationID(
                    marketingAutomationId,
                    true,
                    CurrentUser);

                try
                    {
                    using (var scope = new TransactionScope())
                        {
                        foreach (var maControl in marketingAutomation.Controls)
                            {
                            switch (maControl.ControlType)
                                    {
                                case Enums.MarketingAutomationControlType.CampaignItem:
                                case Enums.MarketingAutomationControlType.Click:
                                case Enums.MarketingAutomationControlType.NoClick:
                                case Enums.MarketingAutomationControlType.NoOpen:
                                case Enums.MarketingAutomationControlType.NotSent:
                                case Enums.MarketingAutomationControlType.Open:
                                case Enums.MarketingAutomationControlType.Open_NoClick:
                                case Enums.MarketingAutomationControlType.Sent:
                                case Enums.MarketingAutomationControlType.Suppressed:
                                    TogglePauseBlast(maControl.ECNID, isPause);
                                    break;
                                case Enums.MarketingAutomationControlType.Direct_Click:
                                case Enums.MarketingAutomationControlType.Direct_Open:
                                case Enums.MarketingAutomationControlType.Subscribe:
                                case Enums.MarketingAutomationControlType.Unsubscribe:
                                    TogglePauseForLayoutPlan(maControl.ECNID, isPause);
                                    break;
                                case Enums.MarketingAutomationControlType.Direct_NoOpen:
                                    TogglePauseForTriggerPlan(maControl.ECNID, isPause);
                                    break;
                                case Enums.MarketingAutomationControlType.Start:
                                case Enums.MarketingAutomationControlType.End:
                                case Enums.MarketingAutomationControlType.Wait:
                                    break;
                                case Enums.MarketingAutomationControlType.Group:
                                    break;
                            }
                        }
                        response.Add(SuccessResponseCode);
                        scope.Complete();
                    }
                    
                    //Bug36972:Remove 'Active' from status drop down list in home page 
                    marketingAutomation.State = isPause 
                        ? Enums.MarketingAutomationStatus.Paused
                        : Enums.MarketingAutomationStatus.Published; 
                    marketingAutomation.UpdatedUserID = CurrentUser.UserID;
                    BLCommunicator.MarketingAutomation.Save(marketingAutomation, CurrentUser);
                    var action = isPause ? ActionPause : ActionUnPause;
                    BLCommunicator.MarketingAutomationHistory.Insert(marketingAutomationId, CurrentUser.UserID, action);
                }
                catch (ECNException ecn)
                {
                    response = GetExceptionResponse(ecn);
                }
                catch (SecurityException)
                    {
                    response = GetExceptionResponse<SecurityException>(null);
                    }
                }
            catch (Exception)
                {
                response = GetExceptionResponse<Exception>(null);
                }

            return response;
                }
                
        private List<string> GetExceptionResponse<T>(T exception)
            {
            var response = new List<string>
            {
                ErrorResponseCode
            };

            if (typeof(ECNException).IsAssignableFrom(typeof(T)))
            {
                var ecnException = exception as ECNException;
                if(ecnException != null)
                { 
                    var sbError = new StringBuilder();
                    sbError.AppendLine(ValidationErrorStr);
                    foreach (var error in ecnException.ErrorList)
                    {
                        sbError.AppendLine(error.ErrorMessage);
            }
                    response.Add(sbError.ToString());
        }
            }
            else if (typeof(SecurityException).IsAssignableFrom(typeof(T)))
        {
                response.Add(SecurityExceptionMessage);
            }
            else
            {
                response.Add(CommonExceptionMessage);
            }
                
            return response;
        }

        private void TogglePauseBlast(int campaignItemId, bool isPause)
                {
            var campaignItem = BLCommunicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemId, true);
            foreach (var ciBlast in campaignItem.BlastList)
                    {
                if (isPause)
                        {
                    //pause it
                    if (ciBlast.CustomerID != null 
                        && ciBlast.BlastID != null 
                        && !BLCommunicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                            {
                        BLCommunicator.Blast.Pause_UnPauseBlast(
                            ciBlast.BlastID.Value,
                            true,
                            CurrentUser);
                                        }
                                    }
                else
                                    {
                    //un-pause it
                    if (ciBlast.BlastID != null 
                        && ciBlast.Blast != null
                        && ciBlast.Blast.StatusCode.ToLower().Equals(BlastStatusCodePaused))
                                    {
                        BLCommunicator.Blast.Pause_UnPauseBlast(
                            ciBlast.BlastID.Value,
                            false,
                            CurrentUser);
                                    }
                            }
                        }
                    }

        private void TogglePauseForLayoutPlan(int layoutPlanId, bool isPause)
                {
            var compareStatus = isPause ? PlanStatusY : PlanStatusP;
            var assignStatus = isPause ? PlanStatusP : PlanStatusY;

            var layoutPlans = BLCommunicator.LayoutPlans.GetByLayoutPlanID(layoutPlanId, CurrentUser);
            if (layoutPlans.Status == compareStatus)
                    {
                layoutPlans.Status = assignStatus;
                layoutPlans.UpdatedUserID = CurrentUser.UserID;
                layoutPlans.UpdatedDate = DateTime.Now;
                BLCommunicator.LayoutPlans.Save(layoutPlans, CurrentUser);

                BLCommunicator.BlastSingle.Pause_UnPause_ForLayoutPlanID(
                    layoutPlans.LayoutPlanID, 
                    isPause, 
                    CurrentUser);
                }
                }

        private void TogglePauseForTriggerPlan(int triggerPlanId, bool isPause)
                {
            var compareStatus = isPause ? PlanStatusY : PlanStatusP;
            var assignStatus = isPause ? PlanStatusP : PlanStatusY;

            var triggerPlans = BLCommunicator.TriggerPlans.GetByTriggerPlanID(triggerPlanId, CurrentUser);
            if (triggerPlans.Status == compareStatus)
            {
                triggerPlans.Status = assignStatus;
                triggerPlans.UpdatedUserID = CurrentUser.UserID;
                triggerPlans.UpdatedDate = DateTime.Now;
                BLCommunicator.TriggerPlans.Save(triggerPlans, CurrentUser);

                BLCommunicator.BlastSingle.Pause_Unpause_ForTriggerPlanID(
                    triggerPlans.TriggerPlanID, 
                    isPause, 
                    CurrentUser);
            }
        }

        public ActionResult LoadForm(int? id, bool isCopy = false)
        {
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = new ECN_Framework_Entities.Communicator.MarketingAutomation();

            DiagramPostModel model = new DiagramPostModel();

            if (id.HasValue)
            {
                ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(id.Value, false, CurrentUser);
                if (isCopy)
                {
                    model.Name = "Copy of " + ma.Name;
                    model.State = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Saved;
                }
                else
                {
                    model.Name = ma.Name;
                    //Whenever Edit mode need a state of the Automation
                    model.State = ma.State;
                }
                model.StartFrom = ma.StartDate;
                model.StartTo = ma.EndDate;
                model.Goal = ma.Goal;
                model.Diagram = ma.JSONDiagram;
                model.IsCreate = false;
                model.IsCopy = isCopy;
            }
            else
            {
                model.IsCreate = true;
                model.IsCopy = isCopy;
                //model.StartFrom = DateTime.Now;
            }


            ViewBag.CurrentUser = CurrentUser;
            return PartialView("Partials/_Automation", model);
        }

        public ActionResult LoadPause(int id)
        {
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = new ECN_Framework_Entities.Communicator.MarketingAutomation();

            DiagramPostModel model = new DiagramPostModel();

            if (id > 0)
            {
                ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(id, false, CurrentUser);

                model.Name = ma.Name;
                model.StartFrom = ma.StartDate;
                model.StartTo = ma.EndDate;
                model.Goal = ma.Goal;
                model.Diagram = ma.JSONDiagram;
                model.IsCreate = false;
                model.State = ma.State;
                model.Id = ma.MarketingAutomationID;
            }
            else
                model.IsCreate = true;

            ViewBag.CurrentUser = CurrentUser;
            return PartialView("Partials/_AutomationPause", model);
        }

        public ActionResult Create(DiagramPostModel model)
        {
            return ProcessCreate(model);
        }

        public ActionResult Delete(int id)
        {
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(id, false, CurrentUser);
            ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.Delete(ma, CurrentUser);
            ECN_Framework_BusinessLayer.Communicator.MarketingAutomationHistory.Insert(id, CurrentUser.UserID, "Delete");
            return JavaScriptRedirectToAction("Index");
        }

        public ActionResult Copy(DiagramPostModel model)
        {
            return ProcessCreate(model, true);
        }

        private ActionResult ProcessCreate(DiagramPostModel model, bool isCopy = false)
        {
            var diagramId = 0;
            var isValid = true;
            model.Name = (model.Name ?? string.Empty).Trim();
            if (!model.IsCopy && !model.IsCreate)
            {
                //While Disable Datepicker we need a StartFrom from DB
                var marketingAutomation = BLCommunicator.MarketingAutomation.GetByMarketingAutomationID(
                    model.Id,
                    false,
                    CurrentUser);
                model.State = marketingAutomation.State;
                if (model.State != Enums.MarketingAutomationStatus.Saved)
                {
                    model.StartFrom = marketingAutomation.StartDate;
                    //View not iline ModelState Error Cleared related to StartFrom
                    if (ModelState.ContainsKey(StartFromKey))
                    {
                        ModelState[StartFromKey].Errors.Clear();
                    }
                    model.Goal = marketingAutomation.Goal;
                    //View not iline ModelState Error Cleared related to Goal
                    if (ModelState.ContainsKey(GoalKey))
                    {
                        ModelState[GoalKey].Errors.Clear();
                    }
                    if (model.State == Enums.MarketingAutomationStatus.Archived)
                    {
                        model.StartTo = marketingAutomation.EndDate;
                        //View not inline with ModelState ,Error should be Cleared related to StartTo
                        if (ModelState.ContainsKey(StartToKey))
                        {
                            ModelState[StartToKey].Errors.Clear();
                        }
                    }
                    else
                    {
                        isValid = ValidateProcess(marketingAutomation, model);
                    }
                }
            }

            //36919:Validation of Start and End Date for Automation
            //Start Date Validation Required for New and Copy Automation.
            //Rules:Start Date should not be greater than End Date. Start Date Should not be Past date
            var validDate = CheckValidDates(model);
            if (!string.IsNullOrWhiteSpace(validDate))
            {
                ModelState.AddModelError(ErrorKey, validDate);
                return PartialView(AutomationPartialViewName, model);
            }

            var isDiagramSave = ValidateAndSaveDiagramPost(isCopy, model, ref diagramId, ref isValid);

            if (isValid)
            {
                if (!model.IsCopy && !model.IsCreate)
                {
                    return JavaScriptRedirectToAction(IndexActionName);
                }

                return JavaScriptRedirectToAction(EditActionName, DiagramsControllerName, new { id = diagramId });
            }

            if (!isDiagramSave)
            {
                return PartialView(AutomationPartialViewName, model);
            }

            return JavaScriptRedirectToAction(IndexActionName);
        }

        private bool ValidateProcess(
            EntityCommunicator.MarketingAutomation marketingAutomation,
            DiagramPostModel model)
        {
            var isValid = true;
            marketingAutomation.EndDate = model.StartTo;
            marketingAutomation.Name = model.Name;
            try
            {
                var diagramJsonObject = JsonConvert.DeserializeObject<DiagramJsonObject>(
                    marketingAutomation.JSONDiagram,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                AllControls = diagramJsonObject.shapes;
                AllConnectors = diagramJsonObject.connections;

                //this is for validating and collecting issues to return to the screen
                var ecnExForValidate = new ECNException(new List<ECNError>());

                ValidatePublish(diagramJsonObject.shapes, marketingAutomation, ref ecnExForValidate);

                if (ecnExForValidate.ErrorList.Count > 0)
                {
                    throw ecnExForValidate;
                }

                if (marketingAutomation.State == Enums.MarketingAutomationStatus.Paused)
                {
                    model.State = Enums.MarketingAutomationStatus.Published;

                    BLCommunicator.MarketingAutomationHistory.Insert(
                        marketingAutomation.MarketingAutomationID,
                        CurrentUser.UserID,
                        ActionPublish);
                }
            }
            catch (ECNException ecnException)
            {
                //throw popup error saying we can't save certain objects
                var stringBuilder = new StringBuilder();
                foreach (var error in ecnException.ErrorList)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                ModelState.AddModelError(ErrorKey, stringBuilder.ToString());
                isValid = false;
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(ErrorKey, CommonExceptionMessage);
                isValid = false;
                ApplicationLog.LogCriticalError(
                    exception,
                    MethodNameSave,
                    Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]));
            }

            return isValid;
        }

        private bool ValidateAndSaveDiagramPost(
            bool isCopy,
            DiagramPostModel model,
            ref int diagramId,
            ref bool isValid)
        {
            var isDiagramSave = false;
            if (ModelState.IsValid)
            {
                if (model.Id <= 0)
                {
                    if ((model.FromTemplate == DiagramFromTemplate.Yes && model.TemplateId > 0) ||
                        model.FromTemplate == DiagramFromTemplate.No)
                    {
                        if (CheckNameIsUnique(model.Name, -1))
                        {
                            diagramId = Save(model, isCopy);
                        }
                        else
                        {
                            //validation messages when input for fields is invalid
                            ModelState.AddModelError(ErrorKey, AutomationNameExistsMessage);
                            isValid = false;
                        }
                    }
                    else
                    {
                        //validation messages when input template not selected
                        ModelState.AddModelError(ErrorKey, TemplateRequiredMessage);
                        isValid = false;
                    }
                }
                else
                {
                    if (!model.IsCopy)
                    {
                        if (CheckNameIsUnique(model.Name, model.Id))
                        {
                            diagramId = Save(model);
                            isDiagramSave = true;
                        }
                        else
                        {
                            //validation messages when input for fields is invalid
                            ModelState.AddModelError(ErrorKey, AutomationNameExistsMessage);
                            isValid = false;
                        }
                    }
                    else
                    {
                        model.Id = -1;
                        if (CheckNameIsUnique(model.Name, model.Id))
                        {
                            diagramId = Save(model, true);
                            isDiagramSave = true;
                        }
                        else
                        {
                            //validation messages when input for fields is invalid
                            ModelState.AddModelError(ErrorKey, AutomationNameExistsMessage);
                            isValid = false;
                        }
                    }
                }
            }
            else
            {
                isValid = false;
            }

            return isDiagramSave;
        }

        public ActionResult Publish(int id)
        {
            var response = new List<string>();
            var marketingAutomation = BLCommunicator.MarketingAutomation.GetByMarketingAutomationID(
                id, 
                true,
                CurrentUser);
            try
            {
                var diagramObject = JsonConvert.DeserializeObject<DiagramJsonObject>(
                    marketingAutomation.JSONDiagram,
                    new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

                var diagramShapes = diagramObject.shapes;
                AllControls = diagramShapes;
                AllConnectors = diagramObject.connections;

                if (!string.IsNullOrWhiteSpace(marketingAutomation.JSONDiagram))
                {
                    if (diagramShapes.Exists(shape =>
                            shape.ControlType == Enums.MarketingAutomationControlType.Start) &&
                        diagramShapes.Exists(shape => 
                            shape.ControlType == Enums.MarketingAutomationControlType.End))
                    {
                        response = SaveAutomationForStartEndType(diagramObject, marketingAutomation);
                        return Json(response);
                    }

                    response.AddRange(AddStrToResponse(ErrorResponseCode, AutomationMissingMessage));
                    return Json(response);
                }

                response.AddRange(AddStrToResponse(ErrorResponseCode, AutomationNotPublishMessage));
                return Json(response);
            }
            catch (Exception)
            {
                SavePublishExceptionResponse<Exception>(null, ref response,
                    marketingAutomation.JSONDiagram, null);
                return Json(response);
            }
        }

        private List<string> SaveAutomationForStartEndType(
            DiagramJsonObject diagramObject,
            EntityCommunicator.MarketingAutomation marketingAutomation)
        {
            var response = new List<string>();
            var automationId = marketingAutomation.MarketingAutomationID;
            var diagramShapes = diagramObject.shapes;
            try
            {
                //this is for validating and collecting issues to return to the screen
                var ecnExForValidate = new ECNException(new List<ECNError>(), CommonEnum.ExceptionLayer.Business);
                var startObject = diagramShapes.OfType<Start>()
                    .First(shape => shape.ControlType == Enums.MarketingAutomationControlType.Start);
                //find connectors from start object
                var startConn = diagramObject.connections.Where(conn => conn.from.shapeId == startObject.ControlID)
                    .ToList();
                //start looping through it's children
                var children = diagramShapes.Where(shape => startConn.Any(conn => conn.to.shapeId == shape.ControlID))
                    .ToList();
                ValidatePublish(diagramShapes, marketingAutomation, ref ecnExForValidate);

                if (ecnExForValidate.ErrorList.Count > 0)
                {
                    throw ecnExForValidate;
                }

                //find start object
                using (var scopeMaster = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TimeSpan(0, 6, 0)))
                {
                    //save start object
                    SaveStartObject(marketingAutomation, automationId, startObject);

                    //Delete existing controls that don't exist in the models list of controls/connectors
                    var existingControls = marketingAutomation.Controls.Where(control => 
                        !diagramShapes.Any(shape => shape.ControlID == control.ControlID)).ToList();

                    DeleteControl(existingControls, marketingAutomation, ValidateDelete, DeleteECNObject);

                    //Putting in global variables so we don't have to pass them around when doing the recursive save
                    AllConnectors = diagramObject.connections;
                    AllControls = diagramShapes;

                    //Start the recursive save
                    SaveChildren(startObject, children, automationId);
                    marketingAutomation.State = Enums.MarketingAutomationStatus.Published;
                    AutomationSaveAndAddHistory(marketingAutomation, CurrentUser, ActionPublish);
                    scopeMaster.Complete();
                }
            }
            catch (SecurityException)
            {
                SavePublishExceptionResponse<SecurityException>(
                    null, 
                    ref response,
                    marketingAutomation.JSONDiagram, 
                    null);
                return response;
            }
            catch (ECNException ex) //throw popup error saying we can't save certain objects
            {
                SavePublishExceptionResponse(ex, ref response, marketingAutomation.JSONDiagram, null);
                return response;
            }
            catch (Exception ex)
            {
                SavePublishExceptionResponse(ex, ref response, marketingAutomation.JSONDiagram, null);
                return response;
            }

            var savedControls = BLCommunicator.MAControl.GetByMarketingAutomationID(automationId);
            var savedConnectors = BLCommunicator.MAConnector.GetByMarketingAutomationID(automationId);
            var connectors = Connector.GetPostModelFromObject(savedConnectors);
            var controls = ControlBase.GetModelsFromObject(savedControls, savedConnectors, diagramShapes, CurrentUser);
            var respControls = ControlBase.Serialize(controls, connectors, automationId);
            marketingAutomation.JSONDiagram = respControls;
            BLCommunicator.MarketingAutomation.Save(marketingAutomation, CurrentUser);
            response.AddRange(AddStrToResponse(SuccessResponseCode, DiagramPublishMessage, respControls));
            return response;
        }

        public ActionResult ValidateCampaignItem(Models.PostModels.Controls.CampaignItem ci)
        {
            List<string> response = new List<string>();
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(ci.MarketingAutomationID, false, CurrentUser);
            try
            {

                ci.Validate(CurrentUser);
                response.Add("200");
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                response.Add("500");
                response.Add(GetErrorString(ecn));

            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add("An error occurred");

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ValidateWait(Models.PostModels.MarketingAutomationPostModel automation, Models.PostModels.Controls.Wait wait)
        {
            List<string> response = new List<string>();

            automation.Controls = automation.GetAllControls();
            AllConnectors = automation.Connectors;
            AllControls = automation.Controls;

            DateTime parentSendTime = new DateTime();
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(automation.MarketingAutomationID, false, CurrentUser);
            decimal totalWait = GetTotalWaitTime(wait, automation.Controls, automation.Connectors, ref parentSendTime);

            try
            {
                //Models.PostModels.Controls.Wait wait = (Models.PostModels.Controls.Wait)controls.First(x => x.ControlType == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Wait);
                wait.Validate(FindParent(wait), ma, CurrentUser);
                if (parentSendTime.AddDays(Convert.ToDouble(totalWait)).Date > automation.EndDate.Date)
                {
                    response.Add("500");
                    response.Add("Wait time is outside of the Automation date range");
                }
                else
                {
                    response.Add("200");
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                response.Add("500");
                response.Add(GetErrorString(ecn));

            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add("An error occurred");

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateGroupEmail(Models.PostModels.MarketingAutomationPostModel automation, Models.PostModels.Controls.Wait wait, string controlID)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(automation.MarketingAutomationID, false, CurrentUser);

                automation.Controls = automation.GetAllControls();
                AllConnectors = automation.Connectors;
                AllControls = automation.Controls;
                //DateTime parentSendTime = new DateTime();
                //decimal totalSendTime = GetTotalWaitTime(wait, AllControls, AllConnectors, ref parentSendTime);               

                switch (AllControls.First(x => x.ControlID == controlID).ControlType)
                {
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click:
                        Models.PostModels.Controls.Click click = (Models.PostModels.Controls.Click)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.CampaignItem ciClick = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        click.Validate(wait, ciClick, ma, CurrentUser);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoClick:
                        Models.PostModels.Controls.NoClick noClick = (Models.PostModels.Controls.NoClick)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.CampaignItem ciNoClick = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        noClick.Validate(wait, ciNoClick, ma, CurrentUser);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoOpen:
                        Models.PostModels.Controls.NoOpen noOpen = (Models.PostModels.Controls.NoOpen)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.CampaignItem ciNoOpen = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        noOpen.Validate(wait, ciNoOpen, ma, CurrentUser);

                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NotSent:
                        Models.PostModels.Controls.NotSent notSent = (Models.PostModels.Controls.NotSent)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.CampaignItem ciNotSent = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        notSent.Validate(wait, ciNotSent, ma, CurrentUser);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open:
                        Models.PostModels.Controls.Open open = (Models.PostModels.Controls.Open)AllControls.First(x => x.ControlID == controlID);

                        Models.PostModels.Controls.CampaignItem ciOpen = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        open.Validate(wait, ciOpen, ma, CurrentUser);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open_NoClick:
                        Models.PostModels.Controls.Open_NoClick open_noClick = (Models.PostModels.Controls.Open_NoClick)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.CampaignItem ciOpen_NoClick = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        open_noClick.Validate(wait, ciOpen_NoClick, ma, CurrentUser);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Sent:
                        Models.PostModels.Controls.Sent sent = (Models.PostModels.Controls.Sent)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.CampaignItem ciSent = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        sent.Validate(wait, ciSent, ma, CurrentUser);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Suppressed:
                        Models.PostModels.Controls.Suppressed suppressed = (Models.PostModels.Controls.Suppressed)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.CampaignItem ciSuppressed = (Models.PostModels.Controls.CampaignItem)FindParentCampaignItem(wait);
                        suppressed.Validate(wait, ciSuppressed, ma, CurrentUser);
                        break;
                }
                response.Add("200");
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                response.Add("500");
                response.Add(GetErrorString(ecn));

            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add("An error occurred");
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateDirectEmail(
            MarketingAutomationPostModel automationModel, 
            Wait wait, 
            string controlId)
        {
            var response = new List<string>();
            try
            {
                automationModel.Controls = automationModel.GetAllControls();
                AllConnectors = automationModel.Connectors;
                AllControls = automationModel.Controls;

                var automation = BLCommunicator.MarketingAutomation.GetByMarketingAutomationID(
                    automationModel.MarketingAutomationID,
                    false, 
                    CurrentUser);
                var automationControl = AllControls.First(control => control.ControlID == controlId);
                var campaignItem = default(CampaignItem);
                var groupControl = default(Group);
                var hasItem = false;
                switch (automationControl.ControlType)
                {
                    case Enums.MarketingAutomationControlType.Direct_Click:
                        var directClick = automationControl as Direct_Click;
                        hasItem = GetGroupAndCampaignItem(ref campaignItem, ref groupControl, wait);
                        directClick?.Validate(wait, hasItem, campaignItem, groupControl, FindParent(wait), CurrentUser);
                        break;
                    case Enums.MarketingAutomationControlType.Direct_NoOpen:
                        var directNoOpen = automationControl as Direct_NoOpen;
                        hasItem = GetGroupAndCampaignItem(ref campaignItem, ref groupControl, wait);
                        directNoOpen?.Validate(wait, hasItem, campaignItem, groupControl, automation, CurrentUser);
                        break;
                    case Enums.MarketingAutomationControlType.Direct_Open:
                        var directOpen = automationControl as Direct_Open;
                        hasItem = GetGroupAndCampaignItem(ref campaignItem, ref groupControl, wait);
                        directOpen?.Validate(wait, hasItem, campaignItem, groupControl, automation, CurrentUser);
                        break;
                    case Enums.MarketingAutomationControlType.FormAbandon:
                        var formAbandon = automationControl as FormAbandon;
                        hasItem = GetGroupAndCampaignItem(ref campaignItem, ref groupControl, wait);
                        formAbandon?.Validate(
                            wait, 
                            hasItem, 
                            campaignItem, 
                            groupControl, 
                            FindParent(FindParent(wait)),
                            CurrentUser);
                        break;
                    case Enums.MarketingAutomationControlType.FormSubmit:
                        var formSubmit = automationControl as FormSubmit;
                        hasItem = GetGroupAndCampaignItem(ref campaignItem, ref groupControl, wait);
                        formSubmit?.Validate(
                            wait, 
                            hasItem, 
                            campaignItem, 
                            groupControl, 
                            FindParent(FindParent(wait)), 
                            CurrentUser);
                        break;
                }

                response.AddRange(AddStrToResponse(SuccessResponseCode));
            }
            catch (ECNException ecnException)
            {
                response.AddRange(AddStrToResponse(ErrorResponseCode, GetErrorString(ecnException)));
            }
            catch (Exception)
            {
                response.AddRange(AddStrToResponse(ErrorResponseCode, CommonExceptionMessage));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private bool GetGroupAndCampaignItem(ref CampaignItem campaignItem, ref Group groupControl, Wait wait)
        {
            var hasItem = false;
            try
            {
                campaignItem = FindParentCampaignItem(wait);
                if (campaignItem != null)
                {
                    hasItem = true;
                }
            }
            catch(Exception exception)
            {
                ApplicationLog.LogNonCriticalError(
                    exception, 
                    MethodNameGetGroupAndCampaignItem, 
                    Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication].ToString()));
            }

            try
            {
                groupControl = FindParentGroup(wait);
            }
            catch (Exception exception)
            {
                ApplicationLog.LogNonCriticalError(
                    exception,
                    MethodNameGetGroupAndCampaignItem,
                    Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication].ToString()));
            }

            return hasItem;
        }

        public ActionResult ValidateFormControl(Models.PostModels.MarketingAutomationPostModel automation, Models.PostModels.Controls.Form form, string controlID)
        {
            List<string> response = new List<string>();
            try
            {
                automation.Controls = automation.GetAllControls();
                AllConnectors = automation.Connectors;
                AllControls = automation.Controls;

                ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(automation.MarketingAutomationID, false, CurrentUser);
                Models.PostModels.Controls.Form fc= (Models.PostModels.Controls.Form)AllControls.First(x => x.ControlID == controlID);
                //Models.PostModels.Controls.CampaignItem ciForm = FindParentCampaignItem(form);
                fc.Validate(FindParent(form), CurrentUser);
                response.Add("200");
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                response.Add("500");
                response.Add(GetErrorString(ecn));

            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add("An error occurred");
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateGroup(Models.PostModels.Controls.Group control)
        {
            List<string> response = new List<string>();
            try
            {
                control.Validate();
                response.Add("200");
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                response.Add("500");
                response.Add(GetErrorString(ecn));

            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add("An error occurred");
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateGroupTrigger(Models.PostModels.MarketingAutomationPostModel automation, Models.PostModels.Controls.Wait wait, string controlID)
        {
            List<string> response = new List<string>();
            try
            {
                automation.Controls = automation.GetAllControls();
                AllConnectors = automation.Connectors;
                AllControls = automation.Controls;


                switch (AllControls.First(x => x.ControlID == controlID).ControlType)
                {
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:
                        Models.PostModels.Controls.Subscribe sub = (Models.PostModels.Controls.Subscribe)AllControls.First(x => x.ControlID == controlID);

                        Models.PostModels.Controls.Group subGroup = FindParentGroup(wait);
                        sub.Validate(subGroup, CurrentUser);
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe:
                        Models.PostModels.Controls.Unsubscribe unSub = (Models.PostModels.Controls.Unsubscribe)AllControls.First(x => x.ControlID == controlID);
                        Models.PostModels.Controls.Group unSubGroup = FindParentGroup(wait);
                        unSub.Validate(unSubGroup, CurrentUser);
                        break;

                }
                response.Add("200");
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                response.Add("500");
                response.Add(GetErrorString(ecn));

            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add("An error occurred");
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private string GetErrorString(ECN_Framework_Common.Objects.ECNException ecn)
        {
            string result = "";
            foreach (ECN_Framework_Common.Objects.ECNError error in ecn.ErrorList)
            {
                result += error.ErrorMessage + "<br />";
            }
            return result;
        }

        private void DeleteECNObject(EntityCommunicator.MAControl maControl)
        {
            BaseDeleteEcnObject(maControl, true);
        }

        private bool CheckNameIsUnique(string name, int MAID)
        {
            return !ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.Exists(CurrentSession.BaseChannelID, name, MAID);
        }

        private string CheckValidDates(DiagramPostModel model)
        {
            string valid = string.Empty;
            //36919:Validation of Start and End Date for Automation
            //Start Date Validation Required for New and Copy Automation.
            //Rules •	Start Date should not be greater than End Date
            //•	Start Date Should not be Past date
            if ((model.IsCopy) || (model.Id <= 0))
            {
                if (DateTime.Now.AddDays(-1) > model.StartFrom)
                    valid = "Automation Start Date must be greater than or equal to today’s date.";
            }
            if (model.StartFrom > model.StartTo)
                valid = "Automation End Date must be greater than the Automation Start Date.";

            return valid;
        }

        private int Save(DiagramPostModel model, bool isCopy = false)
        {

            ECN_Framework_Entities.Communicator.MarketingAutomation ma = new ECN_Framework_Entities.Communicator.MarketingAutomation();

            ma.MarketingAutomationID = isCopy ? -1 : model.Id;
            ma.Name = model.Name;
            ma.Type = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationType.Simple;
            ma.State = model.State; //Bug 36990:Editing and saving Automation Properties for Published automation should not change status
            ma.StartDate = model.StartFrom;
            ma.EndDate = model.StartTo;
            ma.Goal = model.Goal;
            ma.CreatedUserID = CurrentUser.UserID;
            ma.CustomerID = CurrentSession.CurrentCustomer.CustomerID;
            int MAID = -1;
            if (model.IsCreate)
            {
                if (model.FromTemplate == DiagramFromTemplate.No)
                {
                    ma.JSONDiagram = "{\"shapes\":[],\"connections\":[]}";
                }
                else
                {
                    TemplateViewModel tvm = new TemplateViewModel();
                    tvm = tvm.getSingleDiagram(model.TemplateId.Value);
                    ma.JSONDiagram = tvm.Diagram;
                 
                }
            }
            else
            {
                if (isCopy)
                {
                    Models.PostModels.DiagramJsonObject djo = new Models.PostModels.DiagramJsonObject();

                    djo = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.PostModels.DiagramJsonObject>(model.Diagram, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

                    AllControls = djo.shapes;
                    AllConnectors = djo.connections;


                    List<ECN_Framework_Entities.Communicator.MAControl> controlsToCopy = ECN_Framework_BusinessLayer.Communicator.MAControl.GetByMarketingAutomationID(model.Id);
                    List<ECN_Framework_Entities.Communicator.MAConnector> connectorsToCopy = ECN_Framework_BusinessLayer.Communicator.MAConnector.GetByMarketingAutomationID(model.Id);
                    List<Models.PostModels.Connector> connectors = new List<Models.PostModels.Connector>();
                    List<Models.PostModels.ControlBase> controls = Models.PostModels.ControlBase.GetModelsForCopyFromControlBase(AllControls, Models.PostModels.Connector.GetConnectorsForCopy(AllConnectors, MAID), ref connectors, - 1);
                    ma.JSONDiagram = Models.PostModels.ControlBase.Serialize(controls, connectors, MAID);
                }
                else
                {
                    ma.JSONDiagram = model.Diagram;
                }

            }
            MAID = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.Save(ma, CurrentUser);
            model.Id = MAID;
            ECN_Framework_BusinessLayer.Communicator.MarketingAutomationHistory.Insert(MAID, CurrentUser.UserID, model.IsCreate ? "Save" : "Copy");
            return MAID;
        }



        private int FullCopy(DiagramPostModel model)
        {
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = new ECN_Framework_Entities.Communicator.MarketingAutomation();

            ma.MarketingAutomationID = model.Id;
            ma.Name = model.Name;
            ma.Type = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationType.Simple;
            ma.State = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Saved;
            ma.StartDate = model.StartFrom;
            ma.EndDate = model.StartTo;
            ma.Goal = model.Goal;
            ma.JSONDiagram = model.Diagram;
            ma.CreatedUserID = CurrentUser.UserID;
            ma.CustomerID = CurrentSession.CurrentCustomer.CustomerID;
            int diagId = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.Save(ma, CurrentUser);
            ECN_Framework_BusinessLayer.Communicator.MarketingAutomationHistory.Insert(diagId, CurrentUser.UserID, "Copy");
            return diagId;
        }

        public ActionResult GetTemplateView(int id)
        {
            TemplateViewModel tvm = new TemplateViewModel();
            return PartialView("Partials/_TemplateViewer", tvm.getSingleDiagram(id));
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/EmailMarketing.Site/Login/Logout");
        }

        #region Publishing/creating ECN objects
        private List<Models.PostModels.ControlBase> GetChildren(Models.PostModels.Connector connector, List<Models.PostModels.ControlBase> children)
        {
            return children.Where(x => x.ControlID.ToString() == connector.to.shapeId).ToList();
        }

        
        private bool IsParentDirectOpen(Models.PostModels.Controls.Wait wait)
        {
            Models.PostModels.Connector startConn = AllConnectors.First(x => x.to.shapeId == wait.ControlID);
            if (startConn != null)
            {
                //start looping through it's children
                return AllControls.Exists(x => startConn.from.shapeId == x.ControlID && x.ControlType == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open);

            }
            else
            {
                return false;
            }
        }
        
        private void SaveChildren(ControlBase parent, List<ControlBase> children, int automationId)
        {
            BaseSaveChildren(parent, children, automationId, false, false);
        }

        #endregion

        #region dropdown forms auth stuff
        [HttpGet]
        public ActionResult _ClientDropDown(ClientDropDown cdd, bool temp = false)
        {
            cdd.SelectedClientGroupID = CurrentClientGroupID;
            cdd.SelectedClientID = CurrentClientID;
            cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
            cdd.CurrentClientID = cdd.SelectedClientID;
            cdd = RepopulateDropDowns(cdd);


            return PartialView("~/Views/Shared/Partials/_ClientDropDown.cshtml", cdd);

        }
        
        [HttpPost]
        public ActionResult _ClientDropDown(ClientDropDown cdd)
        {
            if (ModelState.IsValid)
            {
                if (cdd != null && cdd.CurrentClientGroupID > 0)
                {
                    var baseChannels = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                    cdd.ClientGroups = CurrentUser.ClientGroups
                        .Where(x => baseChannels.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true)
                        .OrderBy(x => x.ClientGroupName)
                        .ToList();

                    cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, "ClientGroupID", "ClientGroupName", cdd.SelectedClientGroupID);
                    var clients = new List<KMPlatform.Entity.Client>();
                    if (cdd.SelectedClientGroupID != cdd.CurrentClientGroupID)
                    {
                        //Client Group change
                        if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(CurrentUser))
                        {
                            clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(cdd.SelectedClientGroupID).OrderBy(x => x.ClientName).ToList();
                        }
                        cdd.SelectedClientID = clients.First().ClientID;
                        cdd.Clients = clients;
                        cdd.ClientItems = new SelectList(cdd.Clients, "ClientID", "ClientName", cdd.SelectedClientID);
                        cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
                        cdd.CurrentClientID = cdd.SelectedClientID;
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);

                        return RedirectToAction("Index", "Home");
                    }
                    else if (cdd.SelectedClientID != cdd.CurrentClientID)
                    {
                        //Client change
                        cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
                        cdd.CurrentClientID = cdd.SelectedClientID;
                        cdd = RepopulateDropDowns(cdd);
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);

                        return RedirectToAction("Index", "Home");
                    }
                    else//Post from different view???
                    {
                        cdd = RepopulateDropDowns(cdd);
                    }
                }
                else
                {
                    cdd = RepopulateDropDowns(cdd);
                }

                return PartialView("~/Views/Shared/Partials/_ClientDropDown.cshtml", cdd);
            }
            else
            {
                cdd = RepopulateDropDowns(cdd);
                return PartialView("~/Views/Shared/Partials/_ClientDropDown.cshtml", cdd);
            }
        }

        private ClientDropDown RepopulateDropDowns(ClientDropDown cdd)
        {

            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            cdd.ClientGroups = CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName).ToList();
            cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, "ClientGroupID", "ClientGroupName", cdd.SelectedClientGroupID);
            List<KMPlatform.Entity.Client> lstClient = new List<KMPlatform.Entity.Client>();
            if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(CurrentUser))
            {
                lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(CurrentUser.ClientGroups.First(x => x.ClientGroupID == CurrentClientGroupID).ClientGroupID).OrderBy(x => x.ClientName).ToList();
            }
            else
            {
                lstClient = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUserClientGroupClients;
            }
            cdd.Clients = lstClient;
            cdd.ClientItems = new SelectList(cdd.Clients, "ClientID", "ClientName", CurrentClientID);

            cdd.SelectedClientGroupID = CurrentClientGroupID;
            cdd.SelectedClientID = CurrentClientID;
            cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
            cdd.CurrentClientID = cdd.SelectedClientID;


            return cdd;
        }
        
        public bool HasAuthorized(int userID, int clientID)
        {
            return _kmAuthenticationManager.HasAuthorized(clientID);
        }


        #endregion
    }
}