using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KMEnums;
using KMManagers;
using KMModels;
using KMModels.Controls;
using KMModels.PostModels;
using KMModels.ViewModels;
using KMPlatform;
using KMSite;
using KMWeb.Models.Forms;
using KMWeb.Services;

namespace KMWeb.Controllers
{
    public class FormsController : BaseController
    {
        private readonly IUserSelfServicing _userSelfServicing;
        private readonly IKMAuthenticationManager _kmAuthenticationManager;

        #region Constants
        private const string ErrorKey = "error";
        private const string AlreadyExist = "This name is already exists, please, enter another one";
        private const string ProblemWithFields = "Problem with fields for new group";
        private const string DOI_NoMacros = "You must use '%url%' macros in double opt-in message";
        private const string InvalidLinkError = "Invalid link";
        #endregion

        #region Managers

        private readonly FormManager FM;
        private readonly ControlManager CM;
        private readonly NotificationManager NM;
        private readonly CssFileManager CssM;
        private readonly RuleManager RM;
        private readonly ControlTypeManager CTM;
        private readonly SubscriberLoginManager SL;
       
        #endregion

        public FormsController(
            IApplicationManagersFactory applicationManagersFactory,
            IUserSelfServicing userSelfServicing,
            IKMAuthenticationManager authenticationManager)
        {
            FM = applicationManagersFactory.CreateFormManager();
            CM = applicationManagersFactory.CreateControlManager();
            NM = applicationManagersFactory.CreateNotificationManager();
            CssM = applicationManagersFactory.CreateCssFileManager();
            RM = applicationManagersFactory.CreateRuleManager();
            CTM = applicationManagersFactory.CreateControlTypeManager();
            SL = applicationManagersFactory.CreateSubscriberLoginManager();

            _userSelfServicing = userSelfServicing;

            _kmAuthenticationManager = authenticationManager;
        }

        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }

        private int CurrentClientGroupID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientGroupID; }
        }

        private int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }

        private bool IsViewStateField(string value)
        {
            return Regex.Match(value, ECN_Framework_BusinessLayer.Utils.ViewStateRegex).Success;
        }

        private void AddViewStateFieldErrorToModelState()
        {
            ModelState.AddModelError("InvalidInput", "Content contains a ViewState field, which is not supported");
        }

        #region Main form & actions

        public ActionResult Index()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(CurrentUser, Enums.Services.FORMSDESIGNER, Enums.ServiceFeatures.FormsDesigner, Enums.Access.FullAccess))
            {
                FM.CheckIfNeedPublishByCustomer(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID);
                //List<FormViewModel> ActiveForms = FM.GetActiveByChannel<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID).ToList();
                //List<FormViewModel> InactiveForms = FM.GetInactiveByChannel<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID).ToList();
                //foreach (FormViewModel a in ActiveForms)
                //{
                //    a.UpdatedBy = a.UpdatedBy != null && a.UpdatedBy.Length > 27 ? a.UpdatedBy.Substring(0, 27) + "..." : a.UpdatedBy;
                //}
                //foreach (FormViewModel a in InactiveForms)
                //{
                //    a.UpdatedBy = a.UpdatedBy != null && a.UpdatedBy.Length > 27 ? a.UpdatedBy.Substring(0, 27) + "..." : a.UpdatedBy;
                //}
                //var model = new HomeModel
                //            {
                //                ActiveForms = ActiveForms,
                //                InactiveForms = InactiveForms
                //            };
                List<ECN_Framework_Entities.Accounts.Customer> customers = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID);
                ViewBag.Customers = customers;
                HomeModel model = new HomeModel();
                return View(model);
            }
            else
            {
                return Redirect("/ecn.accounts/main");
            }
        }

        public ActionResult LoadForm(int? id) 
        {
            FormPostModel model;
            bool IsCreate = true;

            if (id.HasValue)
            {
                model = FM.GetByID<FormPostModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id.Value);

                ViewBag.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(model.GroupId.Value);// FM.GetGroupByCustomerIDAndID(ApiKey, model.CustomerId.Value, model.GroupId.Value);
                IsCreate = false;

                // Loads any Newsletter Control Group
                var allControls = new FormControlsPostModel { Id = model.Id };
                CM.FillControls(allControls, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
                List<int> nlGroupIds = new List<int>();
                foreach (var nls in allControls.NewsLetter)
                {
                    foreach (var group in nls.Groups)
                    {
                        nlGroupIds.Add(group.GroupID);
                    }
                }
                ViewBag.nlGroupIds = nlGroupIds;
            }
            else
            {
                model = new FormPostModel();
                List<int> nlGroupIds = new List<int>();
                ViewBag.nlGroupIds = nlGroupIds;
            }

            ViewBag.IsCreate = IsCreate;
            return PartialView("Partials/Modals/_Form", model);
        }

        public ActionResult Create(FormPostModel model)
        {
            return ProcessCreate(model, true);
        }

        public ActionResult Copy(FormPostModel model)
        {
            return ProcessCreate(model, false);
        }

        private ActionResult ProcessCreate(FormPostModel model, bool isCreate)
        {
            if (model.GroupId.HasValue && model.CustomerId.HasValue) 
            {
                ViewBag.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(model.GroupId.Value);// FM.GetGroupByCustomerIDAndID(ApiKey, model.CustomerId.Value, model.GroupId.Value);
            }

            int formId = 0;
            model.Name = (model.Name ?? string.Empty).Trim();
            bool valid = true;
            if (ModelState.IsValid)
            {
                if (FM.CheckNameIsUnique(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.Name))
                {
                    Dictionary<string, string> ControlField = Session != null ? (Dictionary<string, string>)Session["ControlField"] : null;
                    Session.Remove("ControlField");
                    formId = isCreate ? FM.Save(UserID, model, ApiKey, CM) : FM.FullCopy(UserID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model, ApiKey, ControlField);
                }
                else
                {
                    ModelState.AddModelError(ErrorKey, AlreadyExist);
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
            if (valid)
            {
                if (isCreate)
                {
                    return JavaScriptRedirectToAction("Edit", new { id = formId });
                }
                else
                {
                    return JavaScriptRedirectToAction("Index");
                }
            }
            else
            {
                if (isCreate)
                {
                    model = new FormPostModel();
                    List<int> nlGroupIds = new List<int>();
                    ViewBag.nlGroupIds = nlGroupIds;
                }
                else
                {
                    // Loads any Newsletter Control Group
                    var allControls = new FormControlsPostModel { Id = model.Id };
                    CM.FillControls(allControls, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
                    List<int> nlGroupIds = new List<int>();
                    foreach (var nls in allControls.NewsLetter)
                    {
                        foreach (var group in nls.Groups)
                        {
                            nlGroupIds.Add(group.GroupID);
                        }
                    }
                    ViewBag.nlGroupIds = nlGroupIds;
                }
            }
            ViewBag.IsCreate = isCreate;
            return PartialView("Partials/Modals/_Form", model);
        }

        public ActionResult Edit(int id)
        {
            if (id != null)
            {
            var model = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            if (model == null)
            {
                throw new InvalidLinkException();
            }
            else
            {
                int newId = id;
                //if (FM.IsActiveByID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id))
                //{
                if (model.Status != FormStatus.Saved.ToString() && !model.HasChild)
                {
                    newId = FM.FullCopyByID(UserID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
                }
                if (model.Status == FormStatus.Updated.ToString() && model.HasChild)
                {
                    newId = model.ChildId.Value;
                }
                //}
                if (newId != id)
                {
                    return RedirectToAction("Edit", new { id = newId });
                }
            }

                ViewBag.OptInType = model.OptInType;
                ViewBag.FormType = model.Type;
                ViewBag.ViewOnly = false;
                return View(id);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult ViewOnly(int id)
        {
            if (id != null)
            {
                var model = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
                if (model == null)
                {
                    throw new InvalidLinkException();
                }

                ViewBag.OptInType = model.OptInType;
                ViewBag.FormType = model.Type;
                ViewBag.ViewOnly = true;
                return View("Edit", id);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult Delete(int id) 
        {
            FM.DeleteByID(UserID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            return JavaScriptRedirectToAction("Index");
        }
        public ActionResult ValidateDelete(int id)
        {
            List<string> response = new List<string>();
            var model = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            if (model == null)
            {
                throw new InvalidLinkException();
            }
            if (ECN_Framework_BusinessLayer.Communicator.MAControl.ExistsByECNID(id, 
                ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Form.ToString(),
                ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Published.ToString()))
            {
                response.Add("500");
                response.Add( model.Name + " is being used in a Published Automation. Deleting is not allowed.");
            }
            else
            {
                response.Add("200");
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Publish(int id, DateTime? publishDate)
        {
            if (publishDate.HasValue && DateTime.Now < publishDate.Value)
            {
                FM.PublishAfterByID(UserID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id, publishDate);
            }
            else
            {
                FM.PublishFormByID(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            }

            return JavaScriptRedirectToAction("Index");
        }

        public ActionResult ChangeActiveState(ActivateFormModel model) 
        {
            if (ModelState.IsValid)
            {
                
                ECN_Framework_Entities.Communicator.Group g = FM.GetGroupByFormID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.Id);
                bool badNWGroup = false;

                var controls = new FormControlsPostModel
                {
                    Id = model.Id
                };

                CM.FillControls(controls, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);

                foreach (var n in controls.NewsLetter)
                {
                    foreach (var group in n.Groups)
                    {
                        ECN_Framework_Entities.Communicator.Group gNews = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(group.GroupID);
                        if (gNews.Archived.Value)
                        {
                            badNWGroup = true;
                            break;
                        }
                    }
                }


                if ((g.Archived == false && !badNWGroup) || model.State == FormActive.Inactive)
                {
                    FM.ChangeActiveStateByID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.Id, model.State, model.From, model.To);

                    return JavaScriptRedirectToAction("Index");
                }
                else if (g.Archived == true && (model.State == FormActive.Active || model.State == FormActive.UseActivationDates))
                {
                    model.State = FormActive.Inactive;
                    ModelState.AddModelError("Error", "Group associated with the form is archived");
                    return PartialView("Partials/Modals/_FormActivate", model);
                }
                else if (badNWGroup)
                {
                    model.State = FormActive.Inactive;
                    ModelState.AddModelError("Error", "Group associated with newsletter control is archived");
                    return PartialView("Partials/Modals/_FormActivate", model);
                }
            }

            return PartialView("Partials/Modals/_FormActivate", model);
        }

        public ActionResult GetFormPath(int id) 
        {
            var form = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            var group = FM.GetGroupByFormID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, ApiKey, id);

            var model = new FormPath 
                        {
                            CustomerName = form.CustomerName,
                            FormName = form.Name,
                            GroupName = group.GroupName
                        };

            return PartialView("Partials/_FormPathContent", model);
        }

        public ActionResult AddGroup(AddGroupModel model) 
        {
            if (ModelState.IsValid) 
            {
                
                ECN_Framework_Entities.Communicator.Group g = new ECN_Framework_Entities.Communicator.Group();
                g.GroupName = model.GroupName;
                g.FolderID = model.FolderId.HasValue ? model.FolderId.Value : 0;
                g.CustomerID = model.CustomerId;
                g.CreatedUserID = CurrentUser.UserID;
                g.IsSeedList = false;
                g.OwnerTypeCode = "customer";
                g.MasterSupression = 0;
                g.PublicFolder = 1;
                g.AllowUDFHistory = "N";

                try
                {
                    ECN_Framework_BusinessLayer.Communicator.Group.Save(g, CurrentUser);
                    return Json(new { success = true, group = g });
                }
                catch (ECN_Framework_Common.Objects.ECNException ecn)
                {
                    StringBuilder errormessage = new StringBuilder();
                    foreach (ECN_Framework_Common.Objects.ECNError e in ecn.ErrorList)
                    {
                        errormessage.AppendLine(e.ErrorMessage);
                    }
                    ModelState.AddModelError("Error", errormessage.ToString());
                }
               
            }
            return PartialView("Partials/Modals/_AddGroupContent", model);
        }

        public ActionResult AddField(AddFieldModel model)
        {
            if (ModelState.IsValid)
            {
                ECN_Framework_Entities.Communicator.GroupDataFields gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf.GroupID = model.GroupId;
                gdf.ShortName = model.FieldName;
                gdf.LongName = model.FieldName;
                gdf.CustomerID = model.CustomerId;
                gdf.IsPublic = "Y";
                
                gdf.CreatedUserID = CurrentUser.UserID;

                try
                {
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, CurrentUser);
                    return Json(new { success = true, field = gdf });
                }
                catch (ECN_Framework_Common.Objects.ECNException ecn)
                {
                    string errormessage = "";
                    foreach (ECN_Framework_Common.Objects.ECNError e in ecn.ErrorList)
                    {
                        errormessage += e.ErrorMessage + "<br />";
                    }
                    ModelState.AddModelError("Error", errormessage);
                }
               
            }
            return PartialView("Partials/Modals/_AddFieldContent", model);
        }

        public ActionResult _ClientDropDown(KMWeb.Models.Forms.ClientDropDown cdd)
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
                        return Redirect(ConfigurationManager.AppSettings["Forms_VirtualPath"].ToString() + "/Forms");
                    }
                    else if (cdd.SelectedClientID != cdd.CurrentClientID)
                    {
                        //Client change
                        cdd = RepopulateDropDowns(cdd);
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);
                        return Redirect(ConfigurationManager.AppSettings["Forms_VirtualPath"].ToString() + "/Forms");
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

        [HttpPost]
        public ActionResult ChangeClientGroup(int clientGroupID = 0)
        {

            return null;
        }

        private KMWeb.Models.Forms.ClientDropDown RepopulateDropDowns(KMWeb.Models.Forms.ClientDropDown cdd)
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

        [AllowAnonymous]
        public ActionResult GetFormQueryString(string token)
        {
            FormSubmitter submitter = new FormSubmitter();
            return Json(submitter.GetQueryString(token));
        }

        public bool validateHtmlHref (string html)
        {
            bool valid = true;
            Regex expression = new Regex(@"<\s*([^>]*)(href\s*=\s*)([""'])([^>]*?([^>]*?))\3([^>]*>)");
            var results = expression.Matches(html);
            string url = string.Empty;
            foreach (Match match in results)
            {
                url = match.Groups[4].Value;
            }
            if (!string.IsNullOrEmpty(url))
            {
                valid = false;
                if (url.ToLower().StartsWith("http://") || url.ToLower().StartsWith("https://") || url.ToLower().StartsWith("mailto:"))
                {
                    valid = true;
                }
            }

            return valid;
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ActiveFormsReadToGrid([DataSourceRequest]DataSourceRequest request, int CustomerID = -1, string FormType = "", string FormStatus = "", string FormName = "", string SearchCriterion = "", int PageNumber = 1)
        {
            KendoGridHelper<FormViewModel> gh = new KendoGridHelper<FormViewModel>();
            List<GridSort> lstgs = gh.GetGridSort(request, "Form_Seq_ID");
            List<GridFilter> lstgf = gh.GetGridFilter(request);
            if (lstgf.Count > 0)
            {
                FormName = lstgf.Where(x => x.FilterColumnName == "FormName").Select(x => x.FilterColumnValue).FirstOrDefault().ToString();
            }
            string sortColumn = lstgs[0].SortColumnName;
            string sortDirection = lstgs[0].SortDirection;
            List<FormViewModel> listRange = new List<FormViewModel>();

            DataSet formsListDS = ECN_Framework_BusinessLayer.FormDesigner.Form.GetBySearchStringPaging(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CustomerID, FormType, FormStatus, FormName, SearchCriterion, 0, PageNumber , request.PageSize, sortDirection, sortColumn);
            int totalCount = 0;
            DataTable dt = formsListDS.Tables[0];
            foreach (DataRow dr in dt.AsEnumerable())
            {
                FormViewModel content = new FormViewModel();
                if (totalCount == 0)
                    totalCount = Convert.ToInt32(dr["TotalCount"].ToString());
                content = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, Convert.ToInt32(dr["Form_Seq_ID"].ToString()));
                content.UpdatedBy = content.UpdatedBy != null && content.UpdatedBy.Length > 27 ? content.UpdatedBy.Substring(0, 27) + "..." : content.UpdatedBy;
                content.Type = content.Type == "AutoSubmit" ? "Auto-Submit" : content.Type;
                content.TotalRecordCounts = dr["TotalCount"].ToString();
                listRange.Add(content);
            }

            //List<FormViewModel> ActiveForms = FM.GetActiveByChannel<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID).ToList();
            IQueryable<FormViewModel> gs = listRange.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult InactiveFormsReadToGrid([DataSourceRequest]DataSourceRequest request, int CustomerID = -1,string FormType = "", string FormStatus = "", string FormName = "", string SearchCriterion = "", int PageNumber = 1)
        {
            KendoGridHelper<FormViewModel> gh = new KendoGridHelper<FormViewModel>();
            List<GridSort> lstgs = gh.GetGridSort(request, "Form_Seq_ID");
            List<GridFilter> lstgf = gh.GetGridFilter(request);
            if (lstgf.Count > 0)
            {
                FormName = lstgf.Where(x => x.FilterColumnName == "FormName").Select(x => x.FilterColumnValue).FirstOrDefault().ToString();
            }
            string sortColumn = lstgs[0].SortColumnName;
            string sortDirection = lstgs[0].SortDirection;
            List<FormViewModel> listRange = new List<FormViewModel>();

            DataSet formsListDS = ECN_Framework_BusinessLayer.FormDesigner.Form.GetBySearchStringPaging(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CustomerID, FormType, FormStatus, FormName, SearchCriterion, 1, PageNumber, request.PageSize, sortDirection, sortColumn);
            int totalCount = 0;
            DataTable dt = formsListDS.Tables[0];
            foreach (DataRow dr in dt.AsEnumerable())
            {
                FormViewModel content = new FormViewModel();
                if (totalCount == 0)
                    totalCount = Convert.ToInt32(dr["TotalCount"].ToString());
                content = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, Convert.ToInt32(dr["Form_Seq_ID"].ToString()));
                content.UpdatedBy = content.UpdatedBy != null && content.UpdatedBy.Length > 27 ? content.UpdatedBy.Substring(0, 27) + "..." : content.UpdatedBy;
                content.Type = content.Type == "AutoSubmit" ? "Auto-Submit" : content.Type;
                content.TotalRecordCounts = dr["TotalCount"].ToString();
                listRange.Add(content);
            }

            //List<FormViewModel> InactiveForms = FM.GetInactiveByChannel<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID).ToList();
            IQueryable<FormViewModel> gs = listRange.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Form Properties

        public ActionResult GetProperties(int id, bool viewOnly = false) 
        {
            var fmod = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            ViewBag.FormType = fmod.Type;
            ViewBag.Group = FM.GetGroupByFormID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            var allControls = new FormControlsPostModel { Id = id };
            CM.FillControls(allControls, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            List<int> nlGroupIds = new List<int>();
            foreach(var nls in allControls.NewsLetter)
            {
                foreach (var group in nls.Groups)
                {
                    nlGroupIds.Add(group.GroupID);
                }
            }
            ViewBag.nlGroupIds = nlGroupIds;
            ViewBag.ViewOnly = viewOnly;
            FormPropertiesPostModel properties = FM.GetByID<FormPropertiesPostModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);

            return PartialView(properties.GetPartialViewName(), properties);
        }

        public ActionResult SaveProperties(FormPropertiesPostModel model) 
        {
            var fmod = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.Id);
            ViewBag.FormType = fmod.Type;
            ViewBag.Group = FM.GetGroupByFormID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.Id);
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(model.Id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            var allControls = new FormControlsPostModel { Id = model.Id };
            CM.FillControls(allControls, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            List<int> nlGroupIds = new List<int>();
            foreach (var nls in allControls.NewsLetter)
            {
                foreach (var group in nls.Groups)
                {
                    nlGroupIds.Add(group.GroupID);
                }
            }
            ViewBag.nlGroupIds = nlGroupIds;

            if (ModelState.IsValid)
            {
                switch (model.ConfirmationPageType)
                {
                    case ResultType.Message:
                        if (HttpUtility.HtmlDecode(model.ConfirmationPageMessage).Trim() == "")
                        {
                            ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");
                        }
                        else if (!validateHtmlHref(HttpUtility.HtmlDecode(model.ConfirmationPageMessage)))
                        {
                            ModelState.AddModelError("BadHref", "Missing protocol on links. Links must begin with a protocol (like http:// or mailto:).");
                        }
                        else if (IsViewStateField(model.ConfirmationPageMessage))
                        {
                            AddViewStateFieldErrorToModelState();
                        }
                        break;
                    case ResultType.URL:
                        if (HttpUtility.HtmlDecode(model.ConfirmationPageUrl).Trim() == "")
                        {
                            ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");
                        }
                        break;
                    case ResultType.MessageAndURL:
                        if (HttpUtility.HtmlDecode(model.ConfirmationPageMAUMessage).Trim() == "" || HttpUtility.HtmlDecode(model.ConfirmationPageMAUUrl).Trim() == "")
                        {
                            ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");
                        }
                        else if (!validateHtmlHref(HttpUtility.HtmlDecode(model.ConfirmationPageMAUMessage)))
                        {
                            ModelState.AddModelError("BadHref", "Missing protocol on links. Links must begin with a protocol (like http:// or mailto:).");
                        }
                        else if (IsViewStateField(model.ConfirmationPageMAUMessage))
                        {
                            AddViewStateFieldErrorToModelState();
                        }
                        break;
                }
                switch (model.InactiveRedirectType)
                {
                    case ResultType.Message:
                        if (HttpUtility.HtmlDecode(model.InactiveRedirectMessage).Trim() == "")
                        {
                            ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");
                        }
                        else if (!validateHtmlHref(HttpUtility.HtmlDecode(model.InactiveRedirectMessage)))
                        {
                            ModelState.AddModelError("BadHref", "Missing protocol on links. Links must begin with a protocol (like http:// or mailto:).");
                        }
                        else if (IsViewStateField(model.InactiveRedirectMessage))
                        {
                            AddViewStateFieldErrorToModelState();
                        }
                        break;
                    case ResultType.URL:
                        if (HttpUtility.HtmlDecode(model.InactiveRedirectUrl).Trim() == "")
                        {
                            ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");
                        }
                        break;
                }
            }

            var saved = false;
            if (ModelState.IsValid)
            {
                if (FM.CheckNameIsUnique(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.Name, model.Id))
                {
                    FM.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model);
                    saved = true;
                }
                else
                {
                    ModelState.AddModelError(ErrorKey, AlreadyExist); 
                }
            }

            Response.AddHeader("Saved", saved.ToString());

            return PartialView("Partials/_FormPropertiesContent", model);
        }

        public ActionResult ChangeGroupContent(int formId, int customerId, int groupId, bool changeFormGroup)
        {
            var fields = FM.GetFieldsByGroupID(groupId);

            var fieldModels = fields.Select(x => new FieldModel
                {
                    Id = x.GroupDataFieldsID,
                    Name = x.ShortName
                }).ToList();

            var controls = CM.GetAllCustomWithFieldByFormIDWithFieldNames(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, ApiKey, formId).ToList();
            FormSubscriberLoginPostModel login = SL.GetByID<FormSubscriberLoginPostModel>(formId);
            if (login != null)
            {
                if(login.OtherIdentificationSelection)
                {
                    int fID;
                    if (int.TryParse(login.OtherIdentification, out fID))
                    {
                        ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(fID, CurrentUser);
                        if(gdf != null)
                        {
                            controls.Add(new ControlFieldModel
                            {
                                ControlId = 0,
                                ControlLabel = "Subscriber Identification",
                                FieldId = null,
                                FieldName = gdf.ShortName
                            });
                        }
                    }
                }
            }

            foreach (var control in controls)
            {
                if (!fieldModels.Any(x => x.Name == control.FieldName))
                {
                    fieldModels.Add(new FieldModel { Id = null, Name = control.FieldName });
                }
            }

            ViewBag.Fields = fieldModels;

            if (controls.Count() > 0)
            {
                Response.AddHeader("controls", true.ToString());
            }

            var model = new ChangeGroupPostModel
            {
                FormId = formId,
                CustomerId = customerId,
                GroupId = groupId,
                ChangeFormGroup = changeFormGroup,
                Fields = controls.Select(x => new ControlFieldModel
                {
                    ControlId = x.ControlId,
                    ControlLabel = x.ControlLabel,
                    FieldId = null,
                    FieldName = x.FieldName
                })
            };

            return PartialView("Partials/Modals/_ChangeGroup", model);
        }

        public ActionResult ChangeGroup(ChangeGroupPostModel model)
        {
            string error = null;
            var response = FM.SaveGroups(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model, ApiKey, CurrentUser, out error);
            if (ModelState.IsValid && Convert.ToBoolean(response["result"]))
            {
                Session.Add("ControlField", response);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        #endregion Form Properties

        #region Form Subscriber Login

        public ActionResult GetSubscriberLogin(int id, bool viewOnly = false)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> otherDDList = FM.GetFieldsByFormID(id, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            foreach (var udf in otherDDList)
            {
                udf.LongName = udf.GroupDataFieldsID.ToString();
            }
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User6", LongName = "User6" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User5", LongName = "User5" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User4", LongName = "User4" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User3", LongName = "User3" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User2", LongName = "User2" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User1", LongName = "User1" });
            ViewBag.Fields = otherDDList;
            ViewBag.Group = FM.GetGroupByFormID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            FormSubscriberLoginPostModel login = SL.GetByID<FormSubscriberLoginPostModel>(id);

            if (login == null)
            {
                login = new FormSubscriberLoginPostModel();
                login.FormID = id;
                login.LoginRequired = false;
                login.OtherIdentificationSelection = false;
                login.OtherIdentification = "";
                login.PasswordRequired = false;
                login.AutoLoginAllowed = false;
                login.LoginModalHTML = "If you are interested in signing up to receive Pub 1, please select New Subscriber. If you are an existing Subscriber and want to manage your subscription, please select Current Subscriber.";
                login.LoginButtonText = "Login";
                login.SignUpButtonText = "Sign Up";
                login.ForgotPasswordButtonText = "Forgot Password";
                login.NewSubscriberButtonText = "New Subscription";
                login.ExistingSubscriberButtonText = "Existing Subscription";
                login.ForgotPasswordMessageHTML = "Your password has been sent to the email specified. Please check your email inbox and click the button below to return to the login page.";
                login.ForgotPasswordNotificationHTML = "The Password you requested is %%Password%%";
                login.ForgotPasswordFromName = "";
                login.ForgotPasswordSubject = "";
                login.EmailAddressQuerystringName = "emailaddress";
                login.OtherQuerystringName = "";
                login.PasswordQuerystringName = "";
            }

            ViewBag.ViewOnly = viewOnly;
            return PartialView(login.GetPartialViewName(), login);
        }

        public ActionResult SaveSubscriberLogin(FormSubscriberLoginPostModel model)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> otherDDList = FM.GetFieldsByFormID(model.FormID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            foreach (var udf in otherDDList)
            {
                udf.LongName = udf.GroupDataFieldsID.ToString();
            }
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User6", LongName = "User6" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User5", LongName = "User5" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User4", LongName = "User4" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User3", LongName = "User3" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User2", LongName = "User2" });
            otherDDList.Insert(0, new ECN_Framework_Entities.Communicator.GroupDataFields { ShortName = "User1", LongName = "User1" });
            ViewBag.Fields = otherDDList;
            ViewBag.Group = FM.GetGroupByFormID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.FormID);

            if (ModelState.IsValid)
            {                
                if (model.LoginModalHTML == null || HttpUtility.HtmlDecode(model.LoginModalHTML).Trim() == "")
                    ModelState.AddModelError("BlankSpaces", "Login Modal Literal HTML Blank Spaces not allowed");
                else if (!validateHtmlHref(HttpUtility.HtmlDecode(model.LoginModalHTML)))
                    ModelState.AddModelError("BadHref", "Missing protocol on links. Links must begin with a protocol (like http:// or mailto:).");
                  
                if (model.ForgotPasswordMessageHTML == null || HttpUtility.HtmlDecode(model.ForgotPasswordMessageHTML).Trim() == "")
                    ModelState.AddModelError("BlankSpaces", "Forgot Password Message Blank Spaces not allowed");
                else if (!validateHtmlHref(HttpUtility.HtmlDecode(model.ForgotPasswordMessageHTML)))
                    ModelState.AddModelError("BadHref", "Missing protocol on links. Links must begin with a protocol (like http:// or mailto:).");

                if (model.ForgotPasswordNotificationHTML == null || HttpUtility.HtmlDecode(model.ForgotPasswordNotificationHTML).Trim() == "")
                    ModelState.AddModelError("BlankSpaces", "Forgot Password Email Notification Blank Spaces not allowed");
                else if (!validateHtmlHref(HttpUtility.HtmlDecode(model.ForgotPasswordNotificationHTML)))
                    ModelState.AddModelError("BadHref", "Missing protocol on links. Links must begin with a protocol (like http:// or mailto:).");

                Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");
                Match m = regex.Match(model.EmailAddressQuerystringName);
                if (!m.Success)
                    ModelState.AddModelError("Validation", "Invalid EmailAddress QueryString Name. Only allowed a-z, A-Z, 0-9 and _ characters.");

                if (model.OtherIdentificationSelection)
                {
                    if (string.IsNullOrEmpty(model.OtherQuerystringName))
                        ModelState.AddModelError("BlankSpaces", "Other Querystring Name Required");
                    else
                    {
                        m = regex.Match(model.OtherQuerystringName);
                        if (!m.Success)
                            ModelState.AddModelError("Validation", "Invalid Other QueryString Name. Only allowed a-z, A-Z, 0-9 and _ characters.");
                    }
                }

                if (model.PasswordRequired && model.AutoLoginAllowed)
                {
                    if (string.IsNullOrEmpty(model.PasswordQuerystringName))
                        ModelState.AddModelError("BlankSpaces", "Password Querystring Name Required");
                    else
                    {
                        m = regex.Match(model.PasswordQuerystringName);
                        if (!m.Success)
                            ModelState.AddModelError("Validation", "Invalid Password QueryString Name. Only allowed a-z, A-Z, 0-9 and _ characters.");
                    }
                }

                if (model.PasswordRequired && string.IsNullOrEmpty(model.ForgotPasswordFromName))
                    ModelState.AddModelError("BlankSpaces", "From Name Required");

                if (model.PasswordRequired && string.IsNullOrEmpty(model.ForgotPasswordSubject))
                    ModelState.AddModelError("BlankSpaces", "Subject Line Required");
            }

            var saved = false;
            if (ModelState.IsValid)
            {
                SL.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model);
                saved = true;
            }

            Response.AddHeader("Saved", saved.ToString());

            return PartialView("Partials/_FormSubscriberLoginContent", model);
        }

        [AllowAnonymous]
        public ActionResult PublicFormLogin(string emailAddress, string other, string password, bool? passReq, int? groupID, string OtherIdentification)
        {
            try
            {
                if (passReq == null || groupID == null)
                    throw new Exception("Empty parameters for passReq or groupID");
            }
            catch (Exception e)
            {
                string note = "passReq: " + passReq + ", groupID:" + groupID + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "Forms.PublicFormLogin", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }

            List<string> response = new List<string>();
            var userIdentificationData = new UserIdentificationData();
            if (passReq.Value && string.IsNullOrEmpty(password))
            {
                response.Add("500");
                response.Add("Password cannot be empty.");
                return Json(response);
            }
            if (!passReq.Value)
                password = string.Empty;
            if (!string.IsNullOrEmpty(other))
            {
                if (string.IsNullOrEmpty(OtherIdentification))
                {
                    response.Add("404");
                    response.Add("The SubscriberID you entered doesn't exist.");
                    return Json(response);
                }

                userIdentificationData.Initialize(other, OtherIdentification);
            }

            try
            {
                int emailID = ECN_Framework_BusinessLayer.Communicator.EmailGroup.FDSubscriberLogin(groupID.Value, 
                    emailAddress, userIdentificationData.UdfId, userIdentificationData.UdfValue, password,
                    userIdentificationData.User1, userIdentificationData.User2, userIdentificationData.User3,
                    userIdentificationData.User4, userIdentificationData.User5, userIdentificationData.User6);
                if (emailID == 0)
                {
                    response.Add("500");
                    response.Add("The Email address or password you entered is invalid.");
                }
                else
                {
                    ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(emailID);
                    response.Add("200");
                    response.Add(email.EmailAddress);
                    response.Add(email.EmailID.ToString());
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "FormsController.PublicFormLogin", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                response.Add("500");
                response.Add(ex.Message);
            }            

            return Json(response);
        }

        [AllowAnonymous]
        public ActionResult PublicFormSignUp(string emailAddress, int? groupID)
        {
            try
            {
                if (groupID == null)
                    throw new Exception("Empty parameter for groupID");
            }
            catch (Exception e)
            {
                string note = "groupID:" + groupID + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "Forms.PublicFormSignUp", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }

            List<string> response = new List<string>();
            KMPlatform.Entity.User _User;
            if (HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["MasterAccessKey"].ToString())] == null)
            {
                _User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["MasterAccessKey"].ToString(), true);
                HttpRuntime.Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["MasterAccessKey"].ToString()), _User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                _User = (KMPlatform.Entity.User)HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["MasterAccessKey"].ToString())];
            }

            ECN_Framework_Entities.Communicator.EmailGroup emailAddressObj = null;
            try
            {
                emailAddressObj = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(emailAddress, groupID.Value);
                if (emailAddressObj == null)
                {
                    response.Add("404");
                    return Json(response);
                }

                //ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
                //ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(emailAddress, group.CustomerID);

                //if (email != null)
                //{
                //    //email exists
                //    ECN_Framework_Entities.Communicator.EmailGroup eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(emailAddress, groupID);
                //    if (eg == null)
                //    {
                //        //email exists but doesn't belong to the group, adding it to the group
                //        StringBuilder sbProfileXML = new StringBuilder();
                //        sbProfileXML.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                //        if (passReq)
                //            sbProfileXML.Append("<Emails><emailaddress>" + emailAddress + "</emailaddress><password>" + password + "</password></Emails>");
                //        else
                //            sbProfileXML.Append("<Emails><emailaddress>" + emailAddress + "</emailaddress></Emails>");
                //        ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(_User, group.CustomerID, group.GroupID, sbProfileXML.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "HTML", "S", false, "", "Ecn.FormDesigner.KMWeb.Controllers.FormsController.PublicFormSignUp");
                //        response.Add("200");
                //    }
                //    else
                //    {
                //        response.Add("500");
                //        response.Add("The Email address already exist.");
                //    }
                //}
                //else
                //{
                //    //email doesn't exist, adding it
                //    StringBuilder sbProfileXML = new StringBuilder();
                //    sbProfileXML.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                //    if (passReq)
                //        sbProfileXML.Append("<Emails><emailaddress>" + emailAddress + "</emailaddress><password>" + password + "</password></Emails>");
                //    else
                //        sbProfileXML.Append("<Emails><emailaddress>" + emailAddress + "</emailaddress></Emails>");
                //    ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(_User, group.CustomerID, group.GroupID, sbProfileXML.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "HTML", "S", false, "", "Ecn.FormDesigner.KMWeb.Controllers.FormsController.PublicFormSignUp");
                //    response.Add("200");
                //}
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "FormsController.PublicFormSignUp", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                response.Add("500");
                response.Add(ex.Message);
            }

            response.Add("200");
            response.Add("The Email Address you entered is already registered. Please Login.");
            return Json(response);
        }

        enum SuppressValue
        {
            None = 0,
            ReceiveOnlyThis = 1,
            ReceiveAll = 2,
            Suppressed = 3
        };

        static void ModifySuppressionForEmailChange(
            string oldEmailAddress,
            string newEmailAddress,
            int groupID,
            int customerID,
            int formID,
            KMPlatform.Entity.User user,
            bool isMasterSuppressed,
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> suppressedChannels,
            string oldSTC,
            string newSTC)
        {
            var newEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(newEmailAddress, customerID);
            if (isMasterSuppressed)
            {
                var comments = "Remove from Master Suppression Thru Form: " + formID;
                ECN_Framework_BusinessLayer.Activity.EmailActivityUpdate.UpdateEmailActivity_NoAccessCheck(oldEmailAddress, newEmailAddress, groupID, customerID, formID, comments, user);
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.DeleteFromMasterSuppressionGroup(groupID, newEmail.EmailID, user);
            }
            foreach (var c in suppressedChannels)
            {
                var comments = "Remove from Channel Master Suppression Thru Form: " + formID;
                ECN_Framework_BusinessLayer.Activity.EmailActivityUpdate.UpdateEmailActivity_NoAccessCheck(oldEmailAddress, newEmailAddress, groupID, customerID, formID, comments, user);
                ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.Delete(c.BaseChannelID, newEmailAddress, user);
            }
            var egs = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailID(newEmail.EmailID, user);
            egs = egs.Where(X => X.SubscribeTypeCode.ToLower() == oldSTC.ToLower()).ToList();
            foreach (var eg in egs)
            {
                ECN_Framework_DataLayer.Communicator.EmailGroup.SetSTC(newSTC, newEmail.EmailID, eg.GroupID);
            }
        }

        static void ModifyEmailAddress(
            string oldEmailAddress,
            string newEmailAddress,
            int groupID,
            int customerID,
            int formID,
            KMPlatform.Entity.User user)
        {
            var comments = "Email Address Update Thru Form: " + formID;
            ECN_Framework_BusinessLayer.Activity.EmailActivityUpdate.UpdateEmailActivity_NoAccessCheck(oldEmailAddress, newEmailAddress, groupID, customerID, formID, comments, user);
            ECN_Framework_BusinessLayer.Communicator.Email.UpdateEmailAddress(groupID, customerID, newEmailAddress, oldEmailAddress, user, "Forms.PublicFormChangeEmail.UpdateEmailAddressForGroup");
        }

        [AllowAnonymous]
        public ActionResult PublicFormChangeEmail(string newEmailAddress, string oldEmailAddress, int? groupID, int? formID, int suppressValue)
        {
            var suppress = (SuppressValue)suppressValue;
            try
            {
                if (formID == null || groupID == null)
                {
                    throw new ArgumentException("Empty parameters for formID or groupID");
                }
            }
            catch (Exception e)
            {
                var note = "formID: " + formID + ", groupID:" + groupID + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "Forms.PublicFormChangeEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }
            var response = new List<string>();
            var user = (KMPlatform.Entity.User)null;
            var masterAccessKey = ConfigurationManager.AppSettings["MasterAccessKey"].ToString();
            var cacheKey = string.Format("cache_user_by_AccessKey_{0}", masterAccessKey);
            if (HttpRuntime.Cache[cacheKey] == null)
            {
                user = KMPlatform.BusinessLogic.User.GetByAccessKey(masterAccessKey, true);
                HttpRuntime.Cache.Add(cacheKey, user, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                user = (KMPlatform.Entity.User)HttpRuntime.Cache[cacheKey];
            }
            try
            {
                if (!APIRunnerBase.CheckEmail(ref newEmailAddress))
                {
                    throw new ArgumentException("Email invalid");
                }
                if (ECN_Framework_BusinessLayer.Communicator.Email.ExistsByGroup(newEmailAddress.Trim(), groupID.Value))
                {
                    throw new ArgumentException("Email exists");
                }
                var group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID.Value);
                var msGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(group.CustomerID, user);
                var msEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(newEmailAddress.Trim(), msGroup.GroupID, user);
                var msMatch = msEmailGroup != null && msEmailGroup.CustomerID != null && msEmailGroup.CustomerID.HasValue;
                var customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(group.CustomerID, false);
                if (customer == null)
                {
                    throw new ArgumentException("Invalid Customer");
                }
                if (customer.BaseChannelID == null || customer.BaseChannelID == -1)
                {
                    throw new ArgumentException("Invalid Channel");
                }
                var channelID = customer.BaseChannelID.Value;
                var cms = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress
                (
                    channelID,
                    newEmailAddress.Replace("'", "''"),
                    user
                );
                var cmsMatch = cms.Count > 0;
                var form = ECN_Framework_BusinessLayer.FormDesigner.Form.GetByFormID_NoAccessCheck(formID.Value);
                if (suppress == SuppressValue.None)
                {
                    if (msMatch)
                    {
                        throw new ArgumentException("Suppressed Master");
                    }
                    if (cmsMatch)
                    {
                        throw new ArgumentException("Suppressed Channel");
                    }
                    ModifyEmailAddress(oldEmailAddress, newEmailAddress, groupID.Value, group.CustomerID, formID.Value, user);
                    response.Add("200");
                    response.Add("success");
                    return Json(response);
                }
                if (suppress == SuppressValue.ReceiveAll)
                {
                    ModifyEmailAddress(oldEmailAddress, newEmailAddress, groupID.Value, group.CustomerID, formID.Value, user);
                    // remove all suppression
                    ModifySuppressionForEmailChange
                    (
                        oldEmailAddress,
                        newEmailAddress,
                        groupID.Value,
                        group.CustomerID,
                        formID.Value,
                        user,
                        msMatch,
                        cms,
                        "M",
                        "S"
                    );
                    response.Add("200");
                    response.Add("success");
                    return Json(response);
                }
                if (suppress == SuppressValue.ReceiveOnlyThis)
                {
                    ModifyEmailAddress(oldEmailAddress, newEmailAddress, groupID.Value, group.CustomerID, formID.Value, user);
                    // remove only this channel suppression
                    ModifySuppressionForEmailChange
                    (
                        oldEmailAddress,
                        newEmailAddress,
                        groupID.Value,
                        group.CustomerID,
                        formID.Value,
                        user,
                        msMatch,
                        cms.Where(X => X.BaseChannelID == channelID).ToList(),
                        "M",
                        "U"
                    );
                    response.Add("200");
                    response.Add("success");
                    return Json(response);
                }
            }
            catch (ArgumentException ex)
            {
                response.Add("500");
                response.Add(ex.Message);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Forms.PublicFormChangeEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                response.Add("500");
                response.Add("An error has occurred.");
            }

            return Json(response);
        }

        [AllowAnonymous]
        public ActionResult PublicFormSendPassword(string emailAddress, string other, int? groupId, int? formId, string OtherIdentification)
        {
            if (formId == null || groupId == null)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Empty parameters for formId or groupId", "Forms.PublicFormSendPassword",
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                    $"formId: {(formId?.ToString() ?? "")}, groupId: {(groupId?.ToString() ?? "")}, User Agent: {Request.UserAgent}");

                return null;
            }

            string result;
            try
            {
                result = _userSelfServicing.ResendPassword(emailAddress, other, groupId.Value, formId.Value, OtherIdentification, SL);
            }
            catch (SelfServicingException exception)
            {
                return Json(new List<string> {exception.Code, exception.Message});
            }

            return Json(new List<string> { "200", result });
        }

        [AllowAnonymous]
        public ActionResult PublicFormUpdateProfileAndSendPassword(string newEmailAddress, string emailAddress, 
            string other, int? groupId, int? formId, string OtherIdentification)
        {
            if (formId == null || groupId == null)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Empty parameters for formId or groupId", "Forms.PublicFormUpdateProfileAndSendPassword",
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                    $"formId: {(formId?.ToString() ?? "")}, groupId: {(groupId?.ToString() ?? "")}, User Agent: {Request.UserAgent}");

                return null;
            }

            string result;
            try
            {
                result = _userSelfServicing.ChangeEmailAndResendPassword(newEmailAddress, emailAddress, other, groupId.Value, formId.Value, OtherIdentification, SL);
            }
            catch (SelfServicingException exception)
            {
                return Json(new List<string> { exception.Code, exception.Message });
            }

            return Json(new List<string> { "200", result });
        }

        private bool ValidatePassword(bool ValidatePassword, string emailPassword, string modelPassword)
        {
            if (ValidatePassword == false) { return true; }
            return emailPassword == modelPassword;
        }
        #endregion Form Subscriber Login

        #region Form Styles

        public ActionResult GetStyles(int id, bool viewOnly = false) 
        {
            FormStylesPostModel styles = CssM.GetByFormID<FormStylesPostModel>(UserID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            if (styles == null)
            {
                styles = new FormStylesPostModel()
                                {
                                    Id = id,
                                    StylingType = StylingType.Custom
                                };
            }

            CustomStyles defaultStyles = CssM.GetDefaultStyles();
            ViewBag.DefaultStyles = defaultStyles;
            ViewBag.ViewOnly = viewOnly;

            // On Form creation, there wont be a users custom style. That means styles.CustomStyles = null
            // If user make custom css modifications through Form Designer, on save a record will be inserted in DB and a file will be created.
            // On GetStyles(int id) method, we will compare user's minimal css file wirh KM Standard css file, and add any item found to a temp css file.
            // Then we will send the merged "styles file" render the CSS.
            // On save we will get the differences with defaultStyle and the new customStyle obj to save only the altered elements as a new lite css file.

            return PartialView(styles.GetPartialViewName(), styles);
        }

        public ActionResult SaveStyles(FormStylesPostModel model) 
        {
            if (ModelState.IsValid)
            {
                CssM.Update(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetCss(int id)
        {
            return File(Server.MapPath("~/Content/Generator/KM_styles.css"), "text/css", "KM_styles.css");
        }

        public ActionResult MatchKMStandardCSS(string[] arrayOfValues)
        {
            if (CssM.MatchKMStandardCSS(arrayOfValues[0], arrayOfValues[1]))
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion Form Styles

        #region Form Controls

        public ActionResult GetControls(int id, bool viewOnly = false)
        {
            ViewBag.Fields = FM.GetFieldsByFormID(id, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            ViewBag.Group = FM.GetGroupByFormID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            ViewBag.Rules = RM.GetAllByFormID<RuleModel>(id);
            ViewBag.SNotifications = NM.GetAllSubscriberNotificationsByFormID(id);
            ViewBag.IUNotifications = NM.GetAllUserNotificationsByFormID(id);
            ViewBag.ViewOnly = viewOnly;

            var controls = new FormControlsPostModel
                            {
                                Id = id
                            };

            CM.FillControls(controls, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            controls.CustomerID = CurrentUser.CustomerID;
            return PartialView(controls.GetPartialViewName(), controls);
        }

        public ActionResult SaveControls(FormControlsPostModel model)
        {
            IEnumerable<Control> controls = model.GetAllControls();
            List<string> OldGrids = new List<string>();

            // Validation for Email and PageBreak
            List<Control> validCtrls = controls.ToList();
            bool haveEmail = false;
            bool havePageB = false;
            foreach (Control ctrl in validCtrls)
            {
                if (ctrl.Type == ControlType.Email)
                    haveEmail = true;
                if (ctrl.Type == ControlType.PageBreak)
                    havePageB = true;
            }

            if (haveEmail && havePageB)
            {
                if (ModelState.IsValid && CM.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.AccessKey.ToString(), model, controls, ref OldGrids))
                {
                    return Json(OldGrids);
                }
            }
            else
            {
                OldGrids.Add("505");
                if (!haveEmail) OldGrids.Add("No Email Control.");
                if (!havePageB) OldGrids.Add("No PageBreak Control.");
            }

            return Json(OldGrids);
        }

        public ActionResult UpdateRules(FormControlsPostModel model)
        {
            var Controls = CM.GetAllValuedByFormID<ControlModel>(model.Id);
            var rulesModel = new FormRulesPostModel { Id = model.Id };
            rulesModel.Rules = RM.GetAllByFormID<RuleModel>(model.Id);
            List<RuleModel> rules = rulesModel.Rules.ToList();
            string dataText = string.Empty;
            foreach (RuleModel rule in rules)
            {
                List<ConditionGroupModel> cgoList = rule.ConditionGroup.ToList();
                foreach (ConditionGroupModel cgp in cgoList)
                {
                    List<ConditionModel> condiList = cgp.Conditions.ToList();
                    foreach (ConditionModel condi in condiList)
                    {
                        ControlModel control = null;
                        foreach (var ctrl in Controls)
                        {
                            if (ctrl.Id == condi.ControlId)
                                control = ctrl;
                        }
                        if (control != null && control.DataType == DataType.Selection)
                        {
                            if (model.OldGrids.Contains("country") || model.OldGrids.Contains("state"))
                            {
                                foreach (string element in model.OldGrids)
                                {
                                    string[] ele = element.Split(',');
                                    if (ele[0] == condi.Value)
                                        dataText = ele[1];
                                }
                                foreach (SelectableItem item in control.SelectableItems)
                                {
                                    if (item.Label == dataText)
                                        condi.Value = item.Id.ToString();                                    
                                }
                            }
                        }
                    }
                    cgp.Conditions = condiList;
                }
                rule.ConditionGroup = cgoList;
            }
            rulesModel.Rules = rules;
            RM.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, rulesModel);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult GetCountriesFromDB()
        {
            return Json(CM.GetCountriesFromDB());
        }

        [AllowAnonymous]
        public ActionResult GetStatesByCountryId(int? key)
        {
            try
            {
                if (key == null)
                    throw new Exception("Empty parameter for key");

                return Json(CM.GetStatesByCountryId(key.Value));
            }
            catch (Exception e)
            {
                string note = "key: " + key + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "Forms.GetStatesByCountryId", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }            
        }

        [AllowAnonymous]
        public ActionResult GetStatesAll()
        {
            return Json(CM.GetStatesAll());
        }

        #endregion Form Controls

        #region Form Rules

        public ActionResult GetRules(int id, bool viewOnly = false)
        {
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            ViewBag.VisibleControls = CM.GetAllVisibleByFormID<ControlModel>(id);
            ViewBag.PageBreaks = CM.GetPageBreaksByFormID<ControlModel>(id);
            var form = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            ViewBag.FormType = form.Type;
            if (ViewBag.FormType == Enum.GetName(typeof(FormType),FormType.Subscription))
            {
                ViewBag.SubVisibleControls = CM.GetAllVisibleOverwriteDataFormID<ControlModel>(id);
            }
            List<ControlModel> controlModels = CM.GetAllRequestQueryByFormID<ControlModel>(id).ToList();
            foreach(ControlModel cm in controlModels)
            {
                ControlTypeModel cType = CTM.GetPaidQueryStringByName<ControlTypeModel>(cm.Control_Type.Name);
                if(cType != null)
                    cm.KMPaidQueryString = cType.KMPaidQueryString;
            }
            var fields = FM.GetFieldsByFormID(id, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            var fieldModels = fields.Select(x => new FieldModel
            {
                Id = x.GroupDataFieldsID,
                Name = x.ShortName
            });
            foreach (ControlModel cm in controlModels)
            {
                foreach (FieldModel fm in fieldModels)
                {
                    if(cm.FieldID == fm.Id)
                    {
                        cm.KMPaidQueryString = "user_"+ fm.Name;
                    }
                }
            }
            ViewBag.URLControls = controlModels;
            var model = new FormRulesPostModel
            {
                Id = id
            };
            model.Rules = RM.GetAllByFormID<RuleModel>(id);
            ViewBag.ViewOnly = viewOnly;
            return PartialView(model.GetPartialViewName(), model);
        }

        public ActionResult SaveRules(FormRulesPostModel model)
        {
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(model.Id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            ViewBag.VisibleControls = CM.GetAllVisibleByFormID<ControlModel>(model.Id);
            ViewBag.PageBreaks = CM.GetPageBreaksByFormID<ControlModel>(model.Id);
            IEnumerable<int> controlIDs = CM.GetAllByFormID<ControlModel>(model.Id).Select(x => x.Id);
            var form = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model.Id);
            ViewBag.FormType = form.Type;
            if (ViewBag.FormType == Enum.GetName(typeof(FormType), FormType.Subscription))
            {
                ViewBag.SubVisibleControls = CM.GetAllVisibleOverwriteDataFormID<ControlModel>(model.Id);
            }
            List<ControlModel> controlModels = CM.GetAllRequestQueryByFormID<ControlModel>(model.Id).ToList();
            foreach (ControlModel cm in controlModels)
            {
                ControlTypeModel cType = CTM.GetPaidQueryStringByName<ControlTypeModel>(cm.Control_Type.Name);
                if (cType != null)
                    cm.KMPaidQueryString = cType.KMPaidQueryString;
            }
            var fields = FM.GetFieldsByFormID(model.Id, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CurrentUser);
            var fieldModels = fields.Select(x => new FieldModel
            {
                Id = x.GroupDataFieldsID,
                Name = x.ShortName
            });
            foreach (ControlModel cm in controlModels)
            {
                foreach (FieldModel fm in fieldModels)
                {
                    if (cm.FieldID == fm.Id)
                    {
                        cm.KMPaidQueryString = "user_" + fm.Name;
                    }
                }
            }
            ViewBag.URLControls = controlModels;
            var saved = false;

            if (ModelState.IsValid)
            {
                bool controlsIsValid = true;
                if (model.Rules != null)
                {
                    foreach (var r in model.Rules)
                    {
                        if (r.Type == RuleTypes.Field && model.Rules.Where(p => p.ControlId == r.ControlId).Count() > 1) 
                        {
                            controlsIsValid = false;
                            ModelState.AddModelError(ErrorKey, "Field Rules Duplicates Fields");
                            ViewBag.selectedTab = 0;
                            break;
                        }

                        if (r.Type == RuleTypes.Form)
                        {
                            if(r.OverwritePostValue !=null && r.IsOverWriteDataPost)
                            {
                                var query = r.OverwritePostValue.GroupBy(x => x.FormField)
                                              .Where(g => g.Count() > 1)
                                              .Select(y => new { Element = y.Key, Counter = y.Count() })
                                              .ToList();
                                if(query.Count>0)
                                {
                                    controlsIsValid = false;
                                    ModelState.AddModelError(ErrorKey, "Form Rules Duplicates Fields not allowed in Overwrite Data Post");
                                }
                                foreach (var ow in r.OverwritePostValue)
                                {
                                    if (string.IsNullOrEmpty(ow.Value))
                                    {
                                        controlsIsValid = false;
                                        ModelState.AddModelError(ErrorKey, "Form Rules Fill in Value for Overwrite Data");
                                    }
                                    if (!string.IsNullOrEmpty(ow.Value) && ow.Value.Length > 100)
                                    {
                                        controlsIsValid = false;
                                        ModelState.AddModelError(ErrorKey, "Form Rules Value is too long");
                                    }
                                }
                            }
                            if (r.RequestQueryValue != null && (r.ResultOnSubmit ==ResultType.KMPaidPage || r.ResultOnSubmit == ResultType.URL))
                            {
                                var query = r.RequestQueryValue.GroupBy(x => x.Value)
                                              .Where(g => g.Count() > 1)
                                              .Select(y => new { Element = y.Key, Counter = y.Count() })
                                              .ToList();
                                if (query.Count > 0)
                                {
                                    controlsIsValid = false;
                                    ModelState.AddModelError(ErrorKey, "Form Rules Duplicates Fields not allowed in Request Query String");
                                }
                                foreach (var re in r.RequestQueryValue)
                                {
                                    if (string.IsNullOrEmpty(re.Name))
                                    {
                                        controlsIsValid = false;
                                        ModelState.AddModelError(ErrorKey, "Form Rules Fill in Name for Querystring Parameter");
                                    }
                                    if (!string.IsNullOrEmpty(re.Name) && re.Name.Length > 100)
                                    {
                                        controlsIsValid = false;
                                        ModelState.AddModelError(ErrorKey, "Form Rules Name is too long");
                                    }
                                    if (!string.IsNullOrEmpty(re.Name))
                                    {
                                        Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");
                                        Match m = regex.Match(re.Name);
                                        if (!m.Success)
                                        {
                                            controlsIsValid = false;
                                            ModelState.AddModelError(ErrorKey, "Invalid QueryString Name. Only allowed a-z, A-Z, 0-9 and _ characters.");
                                        }
                                    }
                                }
                            }
                            if (r.ResultOnSubmit == ResultType.Message && string.IsNullOrEmpty(r.Action))
                            {
                                controlsIsValid = false;
                                ModelState.AddModelError(ErrorKey, "Form Rules empty message not allowed");
                            }
                            else if (r.ResultOnSubmit == ResultType.Message && !validateHtmlHref(HttpUtility.HtmlDecode(r.Action)))
                            {
                                controlsIsValid = false;
                                ModelState.AddModelError(ErrorKey, "Form Rules Missing protocol on links. Links must begin with a protocol (like http:// or mailto:).");
                            }
                            else if (r.ResultOnSubmit == ResultType.Message && IsViewStateField(r.Action))
                            {
                                AddViewStateFieldErrorToModelState();
                            }
                            else if (r.ResultOnSubmit == ResultType.KMPaidPage && string.IsNullOrEmpty(r.UrlToRedirectKM))
                            {
                                controlsIsValid = false;
                                ModelState.AddModelError(ErrorKey, "Form Rules empty Url To Redirect not allowed");
                            }
                            else if (r.ResultOnSubmit == ResultType.URL && string.IsNullOrEmpty(r.UrlToRedirect))
                            {
                                controlsIsValid = false;
                                ModelState.AddModelError(ErrorKey, "Form Rules empty Url To Redirect not allowed");
                            }

                            if (!controlsIsValid)
                            {
                                ViewBag.selectedTab = 2;
                                break;
                            }                            
                        }
                      
                        if (r.ControlId.HasValue && r.ControlId > -1 && !controlIDs.Contains(r.ControlId.Value))
                        {
                            controlsIsValid = false;
                            break;
                        }
                        if (r.MainConditionGroup == null)
                        {
                            r.MainConditionGroup = new ConditionGroupModel { LogicGroup = r.ConditionGroup.First().LogicGroup, Id = 0, ConditionGroups = r.ConditionGroup, Conditions = null };
                        }
                        foreach (var gr in r.ConditionGroup)
                        {
                            foreach (var c in gr.Conditions)
                            {
                                if (!controlIDs.Contains(c.ControlId))
                                {
                                    controlsIsValid = false;
                                    break;
                                }
                            }
                            if (!controlsIsValid)
                            {
                                break;
                            }
                        }
                        if (!controlsIsValid)
                        {
                            break;
                        }
                    }
                }
                if (controlsIsValid)
                {
                    RM.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model);

                    saved = true;
                }
            }

            Response.AddHeader("Saved", saved.ToString());

            return PartialView("Partials/_FormRulesContent", model);
        }

        #endregion Form Rules

        #region Form Notifications

        public ActionResult GetNotifications(int id, bool viewOnly = false) 
        {
            var model = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            ViewBag.FormType = model.Type;
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            ViewBag.ViewOnly = viewOnly;

            var notifications = new FormNotificationsPostModel
                                {
                                    SubscriberNotifications = NM.GetAllSubscriberNotificationsByFormID(id),
                                    InternalUserNotifications = NM.GetAllUserNotificationsByFormID(id)
                                };

            int CustomerID = CurrentUser.CustomerID;
            foreach(KMModels.SubscriberNotificationModel snm in notifications.SubscriberNotifications.ToList())
            {
                snm.CustomerID = CustomerID;
            }

            foreach(KMModels.InternalUserNotificationModel iunm in notifications.InternalUserNotifications.ToList())
            {
                iunm.CustomerID = CustomerID;
            }
            notifications.CustomerID = CurrentUser.CustomerID;

            // Set Error Notification if not found
            if (model.Type == Enum.GetName(typeof(FormType), FormType.AutoSubmit))
            {
                bool errorNot = false;
                int errorNotCount = 0;
                foreach (KMModels.InternalUserNotificationModel iunm in notifications.InternalUserNotifications.ToList())
                {
                    if (iunm.Message == "SubmissionErrorNotification")
                    {
                        errorNot = true;
                        errorNotCount++;
                    }
                }
                if (errorNotCount > 1)
                {
                    List<InternalUserNotificationModel> newInternalNoti = notifications.InternalUserNotifications.ToList();
                    var iunmToRemove = newInternalNoti.FirstOrDefault(x => x.Message == "SubmissionErrorNotification");
                    if (iunmToRemove != null)
                    {
                        newInternalNoti.Remove(iunmToRemove);
                        notifications.InternalUserNotifications = newInternalNoti;
                        notifications.Id = id;
                        NM.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, notifications);
                    }
                }
                if (!errorNot)
                {
                    InternalUserNotificationModel iunm = new InternalUserNotificationModel();
                    iunm.Message = "SubmissionErrorNotification";
                    notifications.Id = id;
                    notifications.InternalUserNotifications = new[] { iunm };
                    NM.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, notifications);
                }
            }

            return PartialView(notifications.GetPartialViewName(), notifications);
        }

        public ActionResult SaveNotifications(FormNotificationsPostModel model) 
        {
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(model.Id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            var saved = false;

            if (model.SubscriberNotifications != null)
            {
                foreach (var sn in model.SubscriberNotifications)
                {
                    if (!string.IsNullOrEmpty(sn.Message))
                        if (HttpUtility.HtmlDecode(sn.Message).Trim() == "")
                            ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");

                    if (sn.Conditions != null)
                    {
                        foreach (var cond in sn.Conditions)
                        {
                            cond.Type = sn.ConditionType;
                        }
                    }
                }
            }

            if (model.InternalUserNotifications != null)
            {
                foreach (var iun in model.InternalUserNotifications)
                {
                    if (!string.IsNullOrEmpty(iun.ToEmail))
                        if (iun.ToEmail.Trim().EndsWith(","))
                            ModelState.AddModelError("InternalNotificationTrailingCommas", "Please remove Emails trailing commas");

                    if (!string.IsNullOrEmpty(iun.Message))
                        if (HttpUtility.HtmlDecode(iun.Message).Trim() == "")
                            ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");

                    if (iun.Conditions != null)
                    {
                        foreach (var cond in iun.Conditions)
                        {
                            cond.Type = iun.ConditionType;
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                NM.Save(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model);

                saved = true;
            }

            Response.AddHeader("Saved", saved.ToString());

            return PartialView("Partials/_FormNotificationsContent", model);
        }

        #endregion Form Notifications

        #region Form Output

        public ActionResult GetOutput(int id, bool viewOnly = false)
        {
            ViewBag.ViewOnly = viewOnly;
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            var output = new FormResultManager().GetThirdpartyOutputResultByFormID<FormOutputPostModel>(id);
            output = output ?? new FormOutputPostModel { Id = id, ExternalUrl = "", ThirdPartyQueryValue = null };

            return PartialView(output.GetPartialViewName(), output);
        }

        public ActionResult SaveOutput(FormOutputPostModel model)
        {
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(model.Id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;
            var saved = false;

            if (model.ThirdPartyQueryValue == null)
                ModelState.Remove("ExternalUrl");

            if (ModelState.IsValid)
            {
                FormResultManager FRM = new FormResultManager();
                if (string.IsNullOrEmpty(model.ExternalUrl))
                {
                    FormOutputPostModel output = FRM.GetThirdpartyOutputResultByFormID<FormOutputPostModel>(model.Id);
                    if (output != null)
                    {
                        FRM.DeleteByID(output.ResultId);
                        model.ThirdPartyQueryValue = null;
                    }
                }
                else
                {
                    FRM.Update(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, model);
                }

                saved = true;
            }

            Response.AddHeader("Saved", saved.ToString());

            return PartialView("Partials/_FormOutputContent", model);
        }

        #endregion Form Output

        #region DoubleOptIn

        public ActionResult GetDoubleOptIn(int id, bool viewOnly = false)
        {
            ViewBag.ViewOnly = viewOnly;
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;

            var model = new FormDoubleOptInPostModel 
                        {
                            Notification = NM.GetDOINotificationByFormID(id)
                        };

            return PartialView(model.GetPartialViewName(), model);
        }

        public ActionResult SaveDoubleOptIn(FormDoubleOptInPostModel model)
        {
            List<ControlModel> ctrls = CM.GetAllValuedByFormID<ControlModel>(model.Id).ToList();
            ViewBag.Controls = ctrls as IEnumerable<ControlModel>;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Notification.Message))
                    if (HttpUtility.HtmlDecode(model.Notification.Message).Trim() == "")
                        ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");

                if (!string.IsNullOrEmpty(model.Page))
                    if (HttpUtility.HtmlDecode(model.Page).Trim() == "")
                        ModelState.AddModelError("BlankSpaces", "Blank Spaces not allowed");
            }

            var saved = false;
            if (ModelState.IsValid)
            {
                if (model.Notification.Message.IndexOf(HTMLGenerator.urlMacros) > -1)
                {
                    //Save
                    NM.Save(model);
                    saved = true;
                    
                }
                else
                {
                    ModelState.AddModelError(ErrorKey, DOI_NoMacros); 
                }
            }

            Response.AddHeader("Saved", saved.ToString());

            return PartialView("Partials/_FormDoubleOptInContent", model);
        }

        #endregion DoubleOptIn

        #region Form Summary

        public ActionResult GetSummary(int id, bool viewOnly = false)
        {
            ViewBag.ViewOnly = viewOnly;
            var model = FM.GetByID<FormViewModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);
            ViewBag.FormType = model.Type;
            var properties = FM.GetByID<FormPropertiesPostModel>(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);

            ViewBag.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(properties.GroupId.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);// FM.GetGroupByCustomerIDAndID(ApiKey, properties.CustomerId.Value, properties.GroupId.Value);

            return PartialView("Partials/_FormSummary", properties);
        }

        public ActionResult PublishForm(int id)
        {
            FM.PublishFormByID(CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion Form Summary

        #region FormTranslate
        private const string ApplicationName = "GoogleAppName";
        private const string ApiKey = "GoogleApiKey";
        private string Translate(string text, string target)
        {
            TranslateService ts = null;

            //load languages
            ts = new TranslateService(new BaseClientService.Initializer()
            {
                ApplicationName = ConfigurationManager.AppSettings[ApplicationName],
                ApiKey = ConfigurationManager.AppSettings[ApiKey]
            });
            string res = text;
            if (target != null && res != string.Empty)
            {
                List<string> data = new List<string>();
                data.Add(res);
                try
                {
                    res = ts.Translations.List(data, target).Execute().Translations[0].TranslatedText;
                }
                catch
                { }
                if (string.IsNullOrEmpty(res))
                {
                    res = text;
                }
            }

            return res;
        }
        [AllowAnonymous]
        public ActionResult TranslateNotification(string text, string target)
        {
            return Json(Translate(text, target));
        }
        #endregion

        [AllowAnonymous]
        public ActionResult PublicFormTimeout()
        {
            if (Request.UrlReferrer != null)
            {
                string note = "URL: " + Request.UrlReferrer.AbsoluteUri + " User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(new Exception("PublicForm Timeout"), "Forms.PublicFormTimeout", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
            }
            return null;            
        }

        [AllowAnonymous]
        public ActionResult UpdateKMPSEmail(string kmpsemail, string newemail, int? groupID, bool? update, int? emailID)
        {
            try
            {
                if (groupID == null || update == null || emailID == null)
                    throw new Exception("Empty parameters for formID or groupID");
            }
            catch (Exception e)
            {
                string note = "User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "Forms.UpdateKMPSEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }

            List<string> response = new List<string>();
            try
            {
                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID.Value);
                bool exist = ECN_Framework_BusinessLayer.Communicator.Email.Exists(newemail, g.CustomerID);
                if (!exist && update.Value)
                {
                    ECN_Framework_Entities.Communicator.Email kmpsE = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(emailID.Value);
                    kmpsE.EmailAddress = newemail;
                    ECN_Framework_BusinessLayer.Communicator.Email.Save(kmpsE);
                    response.Add("200");
                    response.Add("All good.");
                }
                if (exist)
                {
                    response.Add("500");
                    response.Add("The Email address you entered already exist.");
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "FormsController.UpdateKMPSEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                response.Add("500");
                response.Add(ex.Message);
            }

            return Json(response);
        }

    }
}