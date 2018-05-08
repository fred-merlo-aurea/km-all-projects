using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Application;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using CommunicatorBLL = ECN_Framework_BusinessLayer.Communicator;
using CommunicatorEntity = ECN_Framework_Entities.Communicator;
using System.Web.UI.DataVisualization.Charting;
using ecn.domaintracking.Models;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ecn.domaintracking.HtmlHelpers;
using ecn.domaintracking.Models.Shared;
using ecn.domaintracking.Models.Shared.ControlModels;
using ECN_Framework_Entities.DomainTracker;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using KM.Common;
using KMSite;

namespace ecn.domaintracking.Controllers
{
    public class MainController : Controller
    {
        private const string DateKey = "Date";
        private const string ChartArea1Key = "ChartArea1";
        private const string PageViewsKey = "PageViews";
        private const string KnownKey = "Known";
        private const string UnknownKey = "Unknown";

        private const string ArgumentExceptionMessage = "The property {0} can not be null.";

        private readonly IKMAuthenticationManager _kmAuthenticationManager = new DomainTrackingKMAuthenticationManager();

        #region " Properties "

        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        public ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                if (_usersession == null)
                    _usersession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                return _usersession;
            }
        }

        public bool HasAuthorized(int userID, int clientID)
        {
            return _kmAuthenticationManager.HasAuthorized(clientID);
        }

        #endregion

        #region " Principal Views "

        [Authorize]
        public ActionResult Index(int page = 0)
        {
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            if (KM.Platform.User.IsAdministrator(this.UserSession.CurrentUser) || KMPlatform.BusinessLogic.User.HasAccess(this.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.View))
            {
                TempData["page"] = page;
                ViewBag.TrackAnonymous = KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(UserSession.ClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.TrackAnonymous);


                return View(ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByBaseChannelID(UserSession.CurrentBaseChannel.BaseChannelID, UserSession.CurrentUser));
            }
            else
            {

                return Redirect("/ecn.accounts/main/securityAccessError.aspx");
            }
        }

        public ActionResult _ClientDropDown(Models.ClientDropDown cdd)
        {
            if (ModelState.IsValid)
            {
                if (cdd != null && cdd.CurrentClientGroupID > 0)
                {
                    var baseChannels = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                    cdd.ClientGroups = UserSession.CurrentUser.ClientGroups
                        .Where(x => baseChannels.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true)
                        .OrderBy(x => x.ClientGroupName)
                        .ToList();

                    cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, "ClientGroupID", "ClientGroupName", UserSession.ClientGroupID);
                    var cliets = new List<KMPlatform.Entity.Client>();
                    if (cdd.SelectedClientGroupID != cdd.CurrentClientGroupID)
                    {
                        //Client Group change
                        if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(UserSession.CurrentUser))
                        {
                            cliets = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(cdd.SelectedClientGroupID).OrderBy(x => x.ClientName).ToList();
                        }

                        cdd.SelectedClientID = cliets.First().ClientID;
                        cdd.Clients = cliets;
                        cdd.ClientItems = new SelectList(cdd.Clients, "ClientID", "ClientName", cdd.SelectedClientID);
                        cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
                        cdd.CurrentClientID = cdd.SelectedClientID;
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);
                        return Redirect("/ecn.DomainTracking/Main");
                    }
                    else if (cdd.SelectedClientID != cdd.CurrentClientID)
                    {
                        //Client change
                        cdd = RepopulateDropDowns(cdd);
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);
                        return Redirect("/ecn.DomainTracking/Main");
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
                return PartialView("_ClientDropDown", cdd);
            }
            else
            {
                cdd = RepopulateDropDowns(cdd);

                return PartialView("_ClientDropDown", cdd);
            }
        }

        private Models.ClientDropDown RepopulateDropDowns(Models.ClientDropDown cdd)
        {
            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            cdd.ClientGroups = UserSession.CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName).ToList();
            cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, "ClientGroupID", "ClientGroupName", cdd.SelectedClientGroupID);
            List<KMPlatform.Entity.Client> lstClient = new List<KMPlatform.Entity.Client>();
            if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(UserSession.CurrentUser))
            {
                lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(UserSession.CurrentUser.ClientGroups.First(x => x.ClientGroupID == UserSession.ClientGroupID).ClientGroupID).OrderBy(x => x.ClientName).ToList();
            }
            else
            {
                lstClient = UserSession.CurrentUserClientGroupClients;
            }
            cdd.Clients = lstClient;
            cdd.ClientItems = new SelectList(cdd.Clients, "ClientID", "ClientName", UserSession.ClientID);
            cdd.SelectedClientGroupID = UserSession.ClientGroupID;
            cdd.SelectedClientID = UserSession.ClientID;
            cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
            cdd.CurrentClientID = cdd.SelectedClientID;

            return cdd;
        }

        [Authorize]
        public ActionResult DeleteDomainTracker(int id)
        {
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            if (KM.Platform.User.IsAdministrator(this.UserSession.CurrentUser) || KMPlatform.BusinessLogic.User.HasAccess(this.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.Delete))
            {
                ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.Delete(id, UserSession.CurrentUser);
            }
            TempData["page"] = 0;

            return RedirectToAction("Index", "Main");
        }

        [Authorize]
        public ActionResult DeleteDomainTrackerField(int DomainTrackerID, int FieldsID)
        {
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            List<string> response = new List<string>();
            try
            {
                //if (KM.Platform.User.IsChannelAdministrator(this.UserSession.CurrentUser))
                if (KM.Platform.User.IsAdministrator(this.UserSession.CurrentUser) || KMPlatform.BusinessLogic.User.HasAccess(this.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.Delete))
                {
                    ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.Delete(FieldsID, DomainTrackerID, UserSession.CurrentUser);
                    //DomainTrackerFieldsViewModel Model = new DomainTrackerFieldsViewModel();
                    //Model.DomainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(DomainTrackerID, UserSession.CurrentUser);
                    //Model.DTF = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(DomainTrackerID, UserSession.CurrentUser);
                    response.Add("200");
                    response.Add("/ecn.domaintracking/Main/Edit/" + DomainTrackerID.ToString());
                    return Json(response);
                }
                else
                {
                    response.Add("500");
                    response.Add("You do not have permission to delete this item");
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add(ex.Message);
                return Json(response);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            if (KM.Platform.User.IsAdministrator(this.UserSession.CurrentUser) || KMPlatform.BusinessLogic.User.HasAccess(this.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.Edit))
                return View();
            else
            {
                TempData["page"] = 0;

                return View("Index", ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByBaseChannelID(UserSession.CurrentBaseChannel.BaseChannelID, UserSession.CurrentUser));
            }
        }



        [Authorize]
        [HttpPost]
        public ActionResult Add(DomainTrackerFieldsViewModel Model)
        {
            if (KM.Platform.User.IsAdministrator(this.UserSession.CurrentUser) || KMPlatform.BusinessLogic.User.HasAccess(this.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.Edit))
            {
                List<string> response = new List<string>();
                if (String.IsNullOrEmpty(Model.Domain))
                {

                    response.Add("500");
                    response.Add("The domain name is empty.");
                    return Json(response);
                }

                if (Model.Domain.Contains(@"http://"))
                {
                    Model.Domain = Model.Domain.Substring(7);
                }

                if (Model.Domain.Contains(@"https://"))
                {
                    Model.Domain = Model.Domain.Substring(8);
                }

                Model.DomainTracker.Domain = Model.Domain;
                Model.DomainTracker.TrackerKey = Guid.NewGuid().ToString();
                Model.DomainTracker.CreatedUserID = UserSession.CurrentUser.UserID;
                Model.DomainTracker.BaseChannelID = UserSession.CurrentBaseChannel.BaseChannelID;
                int id = -1;
                try
                {
                    id = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.Save(Model.DomainTracker, UserSession.CurrentUser);
                }
                catch (ECNException ex)
                {
                    response.Add("500");
                    string ecnError = "";
                    foreach (ECNError e in ex.ErrorList)
                    {
                        ecnError += e.ErrorMessage + "\n";
                    }
                    response.Add(ecnError);

                    return Json(response);
                }

                Model.DomainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(id, UserSession.CurrentUser);
                Model.DTF = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(id, UserSession.CurrentUser);
                Model.DomainTrackerFields = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID(id, UserSession.CurrentUser);
                response.Add("200");
                response.Add("/ecn.domaintracking/Main/Edit?id=" + Model.DomainTracker.DomainTrackerID.ToString());
                return Json(response);
            }
            else
            {
                TempData["page"] = 0;
                return View("Index", ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByBaseChannelID(UserSession.CurrentBaseChannel.BaseChannelID, UserSession.CurrentUser));
            }
        }

        [HttpPost]
        public ActionResult Edit(DomainTrackerFieldsViewModel Model)
        {
            if (KM.Platform.User.IsAdministrator(this.UserSession.CurrentUser) || KMPlatform.BusinessLogic.User.HasAccess(this.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.Edit))
            {
                List<string> response = new List<string>();
                Model.DomainTrackerField.DomainTrackerID = Model.DomainTracker.DomainTrackerID;
                Model.ErrorMessage = "";
                StringBuilder sb = new StringBuilder();
                if (String.IsNullOrEmpty(Model.FieldName))
                {
                    sb.Append("The 'Data Point Name' field is required.<br/>");
                }
                if (String.IsNullOrEmpty(Model.SourceID))
                {
                    sb.Append("The 'SourceID' field is required.<br/>");
                }
                if (Model.Source == "0")
                {
                    sb.Append("Please select a value from the 'Source' select list.<br/>");
                }
                int id = -1;
                if (sb.ToString().Length > 0)
                {
                    response.Add("500");
                    response.Add(sb.ToString());
                    return Json(response);
                }
                else
                {
                    if (!ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.Exists(Model.FieldName, Model.DomainTracker.DomainTrackerID))
                    {
                        Model.DomainTrackerField.FieldName = Model.FieldName;
                        Model.DomainTrackerField.Source = Model.Source;
                        Model.DomainTrackerField.SourceID = Model.SourceID;
                        id = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.Save(Model.DomainTrackerField, UserSession.CurrentUser);
                        //Model.DTF = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(Model.DomainTracker.DomainTrackerID, UserSession.CurrentUser);
                        //Model.DomainTrackerFields = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID(id, UserSession.CurrentUser);

                        //Model.FieldName = string.Empty;
                        //Model.Source = string.Empty;
                        //Model.SourceID = string.Empty;
                        ViewData = null;
                        response.Add("200");
                        response.Add("/ecn.domaintracking/Main/Edit?id=" + Model.DomainTracker.DomainTrackerID.ToString());
                    }
                    else
                    {
                        response.Add("500");
                        response.Add("Data Point already exists with that Data Point Name");
                        return Json(response);
                    }
                }

                return Json(response);
            }
            else
            {

                return RedirectToAction("Index", "Main");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (KM.Platform.User.IsAdministrator(this.UserSession.CurrentUser) || KMPlatform.BusinessLogic.User.HasAccess(this.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup, KMPlatform.Enums.Access.Edit))
            {
                DomainTrackerFieldsViewModel Model = new DomainTrackerFieldsViewModel();
                Model.ErrorMessage = "";
                if (id >= 0)
                {
                    Model.DomainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(id, UserSession.CurrentUser);
                    //Model.DTF = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(id, UserSession.CurrentUser);
                    Model.DomainTrackerFields = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID(id, UserSession.CurrentUser);
                }

                return View(Model);
            }
            else
            {
                TempData["page"] = 0;

                return View("Index", ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByBaseChannelID(UserSession.CurrentBaseChannel.BaseChannelID, UserSession.CurrentUser));
            }
        }

        [Authorize]
        public ActionResult Reports(int id, int numDays = 0, string fromDate = null, string toDate = null, string Filter = null, int TopRow = 5)
        {
            fromDate = DateTime.Now.AddDays(-30).ToString();
            toDate = DateTime.Now.ToString();

            DateTime startDate;
            DateTime endDate;
            var model = GetDtReports(id, fromDate, toDate, Filter, TopRow, "Known", out startDate, out endDate);

            ViewBag.fromDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.toDate = endDate.ToString("MM/dd/yyyy");
            ViewBag.Filter = Filter ?? string.Empty;
            ViewBag.TypeFilter = "Known";
            AddDomainTrackerId(id, model.DomainTrackerActivity);

            return View(model);
        }

        [Authorize]
        public ActionResult ReportsSearch(int id, int numDays = 0, string fromDate = null, string toDate = null, string Filter = null, int TopRow = 5, string TypeFilter = "Known")
        {
            bool wasStartDateBlank = false;
            bool wasEndDateBlank = false;

            if (string.IsNullOrWhiteSpace(fromDate))
            {
                wasStartDateBlank = true;
            }
            if (string.IsNullOrWhiteSpace(toDate))
            {
                wasEndDateBlank = true;
            }

            DateTime startDate;
            DateTime endDate;
            var model = GetDtReports(id, fromDate, toDate, Filter, TopRow, TypeFilter, out startDate, out endDate);

            ViewBag.fromDate = wasStartDateBlank ? string.Empty : startDate.ToString("MM/dd/yyyy");
            ViewBag.toDate = wasEndDateBlank ? string.Empty : endDate.ToString("MM/dd/yyyy");
            ViewBag.Filter = Filter ?? string.Empty;
            ViewBag.TypeFilter = TypeFilter;
            AddDomainTrackerId(id, model.DomainTrackerActivity);

            return View("Reports", model);
        }

        private DomainTrackerReportsViewModel GetDtReports(int id, string fromDate, string toDate, string Filter, int TopRow, string TypeFilter, out DateTime startDate, out DateTime endDate)
        {
            TempData["DomainTrackerID"] = id;

            DomainTrackerReportsViewModel Model = new DomainTrackerReportsViewModel();
            startDate = DateTime.Now.AddDays(-31);
            endDate = DateTime.Now;
            SetDates(fromDate, toDate, Filter, TopRow, ref startDate, ref endDate);

            Model.FromDateTime = startDate;
            Model.ToDateTime = endDate;
            Model.DomainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(id, ECNSession.CurrentSession().CurrentUser);
            Model.TypeFilter = TypeFilter;

            Model.KnownPageViews = "0";
            Model.UnknownPageViews = "0";



            if (!KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(ECNSession.CurrentSession().ClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.TrackAnonymous))
            {
                Model.ShowUnknown = false;
                Model.DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats(id, startDate.ToString(), endDate.ToString(), Filter, TopRow, "");
                Model.HeatMapStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetHeatMapDataTableStats(id, startDate.ToString(), endDate.ToString(), Filter, "");
                Model.HeatTbl = RollupCities(Model.HeatMapStats);
                Model.StateCountTbl = RollupStates(Model.HeatMapStats);
                Model.PlatformStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetOSStats(id, startDate.ToString(), endDate.ToString(), Filter, "");
                Model.TotalPageViews = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(id, startDate.ToString(), endDate.ToString(), Filter, "all").Rows[0][0].ToString();
                Model.BrowserStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats(id, startDate.ToString(), endDate.ToString(), Filter, "");

                Model.HeatTblWorld = Model.CountyCountTbl = HeatTblWorld(Model.HeatMapStats);
                Model.TypeFilter = "all";
            }
            else
            {
                Model.ShowUnknown = true;
                if (TypeFilter.ToLower().Equals("known"))
                {
                    Model.KnownPageViews = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(id, startDate.ToString(), endDate.ToString(), Filter, "known").Rows[0][0].ToString();
                    Model.DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats(id, startDate.ToString(), endDate.ToString(), Filter, TopRow, "known");
                    Model.HeatMapStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetHeatMapDataTableStats(id, startDate.ToString(), endDate.ToString(), Filter, "known");
                    Model.PlatformStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetOSStats(id, startDate.ToString(), endDate.ToString(), Filter, "known");
                    Model.BrowserStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats(id, startDate.ToString(), endDate.ToString(), Filter, "known");
                    Model.HeatTblWorld = Model.CountyCountTbl = HeatTblWorld(Model.HeatMapStats);
                    Model.HeatTbl = RollupCities(Model.HeatMapStats);
                    Model.StateCountTbl = RollupStates(Model.HeatMapStats);
                }
                else if (TypeFilter.ToLower().Equals("unknown"))
                {
                    Model.UnknownPageViews = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(id, startDate.ToString(), endDate.ToString(), Filter, "unknown").Rows[0][0].ToString();
                    Model.DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats(id, startDate.ToString(), endDate.ToString(), Filter, TopRow, "unknown");
                    Model.HeatMapStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetHeatMapDataTableStats(id, startDate.ToString(), endDate.ToString(), Filter, "unknown");
                    Model.PlatformStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetOSStats(id, startDate.ToString(), endDate.ToString(), Filter, "unknown");
                    Model.BrowserStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats(id, startDate.ToString(), endDate.ToString(), Filter, "unknown");
                    Model.HeatTblWorld = Model.CountyCountTbl = HeatTblWorld(Model.HeatMapStats);
                    Model.HeatTbl = RollupCities(Model.HeatMapStats);
                    Model.StateCountTbl = RollupStates(Model.HeatMapStats);
                }
                else if (TypeFilter.ToLower().Equals("all"))
                {
                    Model.KnownPageViews = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(id, startDate.ToString(), endDate.ToString(), Filter, "known").Rows[0][0].ToString();
                    Model.UnknownPageViews = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetTotalViews(id, startDate.ToString(), endDate.ToString(), Filter, "unknown").Rows[0][0].ToString();
                    Model.DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats_Known_Unknown(id, startDate.ToString(), endDate.ToString(), Filter, TopRow);
                    Model.HeatMapStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetHeatMapKnown_UnknownStats(id, startDate.ToString(), endDate.ToString(), Filter);
                    Model.PlatformStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetOSStats_Known_Unknown(id, startDate.ToString(), endDate.ToString(), Filter);
                    Model.BrowserStats = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetBrowserStats_Known_Unknown(id, startDate.ToString(), endDate.ToString(), Filter);
                    Model.HeatTblWorld = Model.CountyCountTbl = HeatTblWorld_Known_Unknown(Model.HeatMapStats);
                    Model.HeatTbl = RollupCities_Known_Unknown(Model.HeatMapStats);
                    Model.StateCountTbl = RollupStates_Known_Unknown(Model.HeatMapStats);
                    int known = 0;
                    int.TryParse(Model.KnownPageViews, out known);
                    int unknown = 0;
                    int.TryParse(Model.UnknownPageViews, out unknown);
                    Model.TotalPageViews = (known + unknown).ToString();
                }
            }


            TempData["startDate"] = startDate;
            TempData["endDate"] = endDate;
            TempData["Filter"] = Filter;




            //PageViews(id, new Random().Next(), startDate, endDate);
            ViewBag.ChartFromDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.ChartEndDate = endDate.ToString("MM/dd/yyyy");
            return Model;
        }

        private DataTable HeatTblWorld(DataTable heatTbl)
        {
            DataTable stateCountTbl = new DataTable();
            stateCountTbl.Columns.Add("Country", typeof(string));
            stateCountTbl.Columns.Add("Count", typeof(int));

            if (heatTbl != null && heatTbl.DefaultView.Count > 0)
            {
                if (heatTbl.Rows.Count > 0)
                {
                    DataTable distinctValues = heatTbl.DefaultView.ToTable(true, "Country");
                    foreach (DataRow dtCountry in distinctValues.Rows)
                    {
                        string state = dtCountry["Country"].ToString();
                        int count = heatTbl.AsEnumerable()
                            .Where(y => y.Field<string>("Country") == state)
                            .Sum(x => x.Field<int>("HitCount"));

                        stateCountTbl.Rows.Add(state, count);
                    }

                    return stateCountTbl;
                }
            }

            return stateCountTbl;
        }

        private DataTable HeatTblWorld_Known_Unknown(DataTable heatTbl)
        {
            DataTable stateCountTbl = new DataTable();
            stateCountTbl.Columns.Add("Country", typeof(string));
            stateCountTbl.Columns.Add("Total", typeof(int));
            stateCountTbl.Columns.Add("Known", typeof(int));
            stateCountTbl.Columns.Add("Unknown", typeof(int));

            if (heatTbl != null && heatTbl.DefaultView.Count > 0)
            {
                if (heatTbl.Rows.Count > 0)
                {
                    DataTable distinctValues = heatTbl.DefaultView.ToTable(true, "Country");
                    foreach (DataRow dtCountry in distinctValues.Rows)
                    {
                        string state = dtCountry["Country"].ToString();
                        int totalCount = heatTbl.AsEnumerable()
                            .Where(y => y.Field<string>("Country") == state)
                            .Sum(x => x.Field<int>("Total"));

                        int knownCount = heatTbl.AsEnumerable()
                            .Where(y => y.Field<string>("Country") == state)
                            .Sum(x => x.Field<int>("Known"));

                        int unknownCount = heatTbl.AsEnumerable()
                            .Where(y => y.Field<string>("Country") == state)
                            .Sum(x => x.Field<int>("Unknown"));

                        stateCountTbl.Rows.Add(state, totalCount, knownCount, unknownCount);
                    }

                    return stateCountTbl;
                }
            }

            return stateCountTbl;
        }



        private DataTable RollupCities(DataTable heatTbl)
        {
            DataTable stateCountTbl = new DataTable();
            stateCountTbl.Columns.Add("CityStateCombo", typeof(string));
            stateCountTbl.Columns.Add("City", typeof(string));
            stateCountTbl.Columns.Add("State", typeof(string));
            stateCountTbl.Columns.Add("Count", typeof(int));

            if (heatTbl != null && heatTbl.DefaultView.Count > 0)
            {
                var result = heatTbl.AsEnumerable().Where(r => r.Field<string>("Country") == "United States");
                if (result.Any())
                {
                    DataTable dt = result.CopyToDataTable();
                    dt.Columns.Add(new DataColumn { ColumnName = "CityStateCombo" });
                    foreach (DataRow row in dt.Rows)
                    {
                        row["CityStateCombo"] = row["City"] + "|" + row["State"];
                    }
                    DataTable distinctValues = dt.DefaultView.ToTable(true, "CityStateCombo");


                    foreach (DataRow dtState in distinctValues.Rows)
                    {
                        string csc = dtState["CityStateCombo"].ToString();

                        int count = dt.AsEnumerable()
                            .Where(y => y.Field<string>("CityStateCombo") == csc)
                            .Sum(x => x.Field<int>("HitCount"));

                        string[] parts = csc.Split('|');
                        stateCountTbl.Rows.Add(csc, parts[0], parts[1], count);
                    }

                    return stateCountTbl;
                }
            }

            return stateCountTbl;
        }

        private DataTable RollupCities_Known_Unknown(DataTable heatTblKnown)
        {

            DataTable stateCountTbl = new DataTable();
            stateCountTbl.Columns.Add("CityStateCombo", typeof(string));
            stateCountTbl.Columns.Add("City", typeof(string));
            stateCountTbl.Columns.Add("State", typeof(string));
            stateCountTbl.Columns.Add("Total", typeof(int));
            stateCountTbl.Columns.Add("Known", typeof(int));
            stateCountTbl.Columns.Add("Unknown", typeof(int));


            if (heatTblKnown != null && heatTblKnown.DefaultView.Count > 0)
            {
                var result = heatTblKnown.AsEnumerable().Where(r => r.Field<string>("Country") == "United States");
                if (result.Any())
                {
                    DataTable dt = result.CopyToDataTable();
                    dt.Columns.Add(new DataColumn { ColumnName = "CityStateCombo" });
                    foreach (DataRow row in dt.Rows)
                    {
                        row["CityStateCombo"] = row["City"] + "|" + row["State"];
                    }
                    DataTable distinctValues = dt.DefaultView.ToTable(true, "CityStateCombo");


                    foreach (DataRow dtState in distinctValues.Rows)
                    {
                        DataRow drNew = stateCountTbl.NewRow();
                        string csc = dtState["CityStateCombo"].ToString();

                        int totalCount = dt.AsEnumerable()
                            .Where(y => y.Field<string>("CityStateCombo") == csc)
                            .Sum(x => x.Field<int>("Total"));

                        int knownCount = dt.AsEnumerable()
                            .Where(y => y.Field<string>("CityStateCombo") == csc)
                            .Sum(x => x.Field<int>("Known"));

                        int unknownCount = dt.AsEnumerable()
                            .Where(y => y.Field<string>("CityStateCombo") == csc)
                            .Sum(x => x.Field<int>("Unknown"));

                        string[] parts = csc.Split('|');
                        stateCountTbl.Rows.Add(csc, parts[0], parts[1], totalCount, knownCount, unknownCount);
                    }



                }

            }

            return stateCountTbl;
        }


        private DataTable RollupStates(DataTable heatTbl)
        {
            DataTable stateCountTbl = new DataTable();
            stateCountTbl.Columns.Add("State", typeof(string));
            stateCountTbl.Columns.Add("Count", typeof(int));

            if (heatTbl != null && heatTbl.DefaultView.Count > 0)
            {
                var result = heatTbl.AsEnumerable().Where(r => r.Field<string>("Country") == "United States");
                if (result.Any())
                {
                    DataTable dt = result.CopyToDataTable();
                    DataTable distinctValues = dt.DefaultView.ToTable(true, "State");

                    foreach (DataRow dtState in distinctValues.Rows)
                    {
                        string state = dtState["State"].ToString();

                        int count = dt.AsEnumerable()
                            .Where(y => y.Field<string>("State") == state)
                            .Sum(x => x.Field<int>("HitCount"));


                        stateCountTbl.Rows.Add(state, count);
                    }

                    return stateCountTbl;
                }
            }

            return stateCountTbl;
        }

        private DataTable RollupStates_Known_Unknown(DataTable heatTblKnown)
        {

            DataTable stateCountTbl = new DataTable();
            stateCountTbl.Columns.Add("State", typeof(string));
            stateCountTbl.Columns.Add("Total", typeof(int));
            stateCountTbl.Columns.Add("Known", typeof(int));
            stateCountTbl.Columns.Add("Unknown", typeof(int));


            if (heatTblKnown != null && heatTblKnown.DefaultView.Count > 0)
            {
                var result = heatTblKnown.AsEnumerable().Where(r => r.Field<string>("Country") == "United States");
                if (result.Any())
                {
                    DataTable dt = result.CopyToDataTable();
                    DataTable distinctValues = dt.DefaultView.ToTable(true, "State");

                    foreach (DataRow dtState in distinctValues.Rows)
                    {
                        DataRow drNew = stateCountTbl.NewRow();
                        string state = dtState["State"].ToString();

                        int knownCount = dt.AsEnumerable()
                            .Where(y => y.Field<string>("State") == state)
                            .Sum(x => x.Field<int>("Known"));
                        int UnknownCount = dt.AsEnumerable()
                            .Where(y => y.Field<string>("State") == state)
                            .Sum(x => x.Field<int>("Unknown"));
                        int totalCount = dt.AsEnumerable()
                            .Where(y => y.Field<string>("State") == state)
                            .Sum(x => x.Field<int>("Total"));

                        drNew["Total"] = totalCount;
                        drNew["Known"] = knownCount;
                        drNew["Unknown"] = UnknownCount;
                        drNew["State"] = state;
                        stateCountTbl.Rows.Add(drNew);
                    }


                }

            }

            return stateCountTbl;
        }


        public double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds)
        {
            return degrees + (minutes / 60) + (seconds / 3600);
        }

        private void SetDates(string fromDate, string toDate, string Filter, int TopRow, ref DateTime startDate, ref DateTime endDate)
        {
            ViewBag.TopRow = TopRow;
            if (fromDate != null && toDate != null)
            {
                try
                {
                    DateTime dateTime;
                    if ((DateTime.TryParse(fromDate, out dateTime)) && (DateTime.TryParse(toDate, out dateTime)))
                    {
                        startDate = DateTime.Parse(fromDate);
                        endDate = DateTime.Parse(toDate);
                        endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    }
                    else
                    {
                        startDate = new DateTime(1753, 1, 2);
                        endDate = DateTime.Now;
                    }
                    DateTime sqlMinDate = new DateTime(1753, 1, 1);
                    if (startDate < sqlMinDate)
                    {
                        startDate = sqlMinDate;
                    }
                    if (endDate < sqlMinDate)
                    {
                        endDate = sqlMinDate;
                    }
                }
                catch (ECNException ex)
                {
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Users(int id, int CurrentPage = 0, int PageSize = 15, string StartDate = "", string EndDate = "", string FilterEmail = "Known", string PageURL = "")
        {
            DateTime dt;
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Today.AddMonths(-3).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Today.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (!DateTime.TryParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) ||
                !DateTime.TryParseExact(EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return RedirectToAction("Users", new { id = id });

            ViewData.Add("CurrentPage", CurrentPage);
            TempData["DomainTrackerID"] = id;
            DomainTrackerUsersViewModel Model = new DomainTrackerUsersViewModel();
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            Model.DomainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(id, UserSession.CurrentUser);
            //Model.ProfileList = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.GetByDomainTrackerID(
            //    id,
            //    CurrentPage,
            //    PageSize,
            //    UserSession.CurrentUser,
            //    StartDate,
            //    EndDate,
            //    FilterEmail
            //);
            Model.ShowUnknown = KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(ECNSession.CurrentSession().ClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.TrackAnonymous);


            Model.toDate = EndDate;
            Model.fromDate = StartDate;
            Model.DomainTrackId = id;
            Model.GroupSelectorModel.DomainTrackId = id;
            Model.PageUrl = PageURL;
            Model.FilterType = FilterEmail.ToLower();
            //Model.ProfileCount = Model.ProfileList.Count > 0 ? Model.ProfileList[0].ProfileCount : 19;

            // Model.ProfileList within 3 months
            return View(Model);
        }


        public ActionResult ShowUsers(int id, DateTime? FromDate = null, DateTime? ToDate = null, string PageURL = null, string TypeFilter = "known")
        {
            TempData["DomainTrackerID"] = id;
            TempData["DomainTracker_PageURL"] = PageURL;

            if (ToDate != null)
            {
                var tmp = DateTime.Parse(ToDate.ToString());
                tmp = tmp.AddDays(1);
                ToDate = tmp;
            }

            string pageURL = Server.UrlDecode(PageURL);
            DomainTrackerUsersViewModel model = GetDTUViewModel(id, null, FromDate, ToDate, pageURL, TypeFilter);
            model.GroupSelectorModel.DomainTrackId = id;
            model.GroupSelectorModel.toDate = ToDate.ToString();
            model.GroupSelectorModel.fromDate = FromDate.ToString();
            return View("ShowUsers", model);
        }

        [HttpPost]
        public ActionResult ShowUsers(int id, string FilterEmail = null, DateTime? FromDate = null, DateTime? ToDate = null, string PageURL = null, string TypeFilter = "known")
        {
            TempData["DomainTrackerID"] = id;
            TempData["DomainTracker_PageURL"] = PageURL;

            string pageURL = Server.UrlDecode(PageURL);
            DomainTrackerUsersViewModel model = GetDTUViewModel(id, FilterEmail, FromDate, ToDate, pageURL, TypeFilter);
            return PartialView("_ShowUsers", model);
        }

        #endregion

        #region " Partial Views "

        public ActionResult GetMostVisitedPages(string fromDate = null, string toDate = null, string Filter = null, int TopRow = 5, string TypeFilter = "Known", int DomainTrackerID = 0)
        {
            ViewBag.TopRow = TopRow;

            int id = DomainTrackerID;
            TempData["DomainTrackerID"] = id;

            DateTime startDate = DateTime.Now.AddDays(-31);
            DateTime endDate = DateTime.Now;
            SetDates(fromDate, toDate, Filter, TopRow, ref startDate, ref endDate);
            ViewBag.fromDate = startDate.ToString("MM/dd/yyyy"); 
            ViewBag.toDate = endDate.ToString("MM/dd/yyyy"); 
            DataTable DomainTrackerActivity = null;
            if (!KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(ECNSession.CurrentSession().ClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.TrackAnonymous))
            {
                DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats(id, startDate.ToString(), endDate.ToString(), Filter, TopRow);
            }
            else
            {
                if (TypeFilter == "Known")
                {
                    DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats(id, startDate.ToString(), endDate.ToString(), Filter, TopRow, "known");
                }
                else if (TypeFilter == "Unknown")
                {
                    DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats(id, startDate.ToString(), endDate.ToString(), Filter, TopRow, "unknown");
                }
                else if (TypeFilter == "All")
                {
                    DomainTrackerActivity = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetURLStats_Known_Unknown(id, startDate.ToString(), endDate.ToString(), Filter, TopRow);
                }

            }

            AddDomainTrackerId(id, DomainTrackerActivity);
            ViewBag.TypeFilter = TypeFilter;

            return PartialView("_ReportsMostVisitedPages", DomainTrackerActivity);
        }

        private static void AddDomainTrackerId(int id, DataTable DomainTrackerActivity)
        {
            System.Data.DataColumn newColumn = new System.Data.DataColumn("DomainTrackerID", typeof(System.Int32))
            {
                DefaultValue = id
            };
            DomainTrackerActivity.Columns.Add(newColumn);
        }

        public ActionResult GetGroupListByFolderId(int folderId)
        {
            var session = ECNSession.CurrentSession();
            var currentUser = session.CurrentUser;

            DataTable ECNCurrentGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers(folderId, currentUser, 1, 1000000);
            var GroupList = new List<SelectListItem>();
            for (int j = 0; j < ECNCurrentGroups.Rows.Count; j++)
            {
                var sl = new SelectListItem
                {
                    Value = ECNCurrentGroups.Rows[j]["GroupID"].ToString(),
                    Text = ECNCurrentGroups.Rows[j]["GroupName"].ToString()
                };
                GroupList.Add(sl);
            }

            var model = new DropDownListModel()
            {
                HtmlAttributes = new { @class = "", style = "font-size: 11px" },
                Items = GroupList,
                Name = "ddlGroups",
                Options = DropDownListOptions.None
            };

            return PartialView("Controls/_DropDownListPartial", model);
        }

        [HttpPost]
        public ActionResult UsersFilterByEmailClick(int id, string StartDate = null, string EndDate = null, string FilterEmail = null)
        {
            TempData["DomainTrackerID"] = id;
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            return PartialView("_Users", ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.GetByDomainTrackerID(id, 0, 50, UserSession.CurrentUser, StartDate, EndDate, FilterEmail));
        }

        [HttpPost]
        public ActionResult GetUsersActivityDetails(int domainTrackerId, int profileId, int recordcount, string StartDate, string EndDate)
        {
            UserActivitiesModel model = new UserActivitiesModel();
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            List<DomainTrackerActivity> activities = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetByProfileID(profileId, domainTrackerId, UserSession.CurrentUser, StartDate, EndDate);
            List<ECN_Framework_Entities.DomainTracker.FieldsValuePair> fieldsValuePairList = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetFieldValuesByProfileID(profileId, domainTrackerId, StartDate, EndDate);
            model.Activities = activities;
            model.FieldsValuePairList = fieldsValuePairList;

            return PartialView("_ActivityList", model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetUserActivity([DataSourceRequest]DataSourceRequest request, int domainTrackerId, int profileId, string StartDate, string EndDate, string PageUrl = "")
        {
            DataSourceResult result;
            List<DomainTrackerActivity> activities = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetByProfileID(profileId, domainTrackerId, UserSession.CurrentUser, StartDate, EndDate, PageUrl);


            IQueryable<DomainTrackerActivity> gs = activities.AsQueryable();
            result = gs.ToDataSourceResult(request);

            result.Total = activities.Count;
            return Json(result);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetUserActivity_AdditionalDataPoints([DataSourceRequest]DataSourceRequest request, int domainTrackerId, int profileId, string StartDate, string EndDate, string PageUrl = "")
        {
            DataSourceResult result;
            List<ECN_Framework_Entities.DomainTracker.FieldsValuePair> fieldsValuePairList = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetFieldValuesByProfileID(profileId, domainTrackerId, StartDate, EndDate, PageUrl);



            IQueryable<ECN_Framework_Entities.DomainTracker.FieldsValuePair> gs = fieldsValuePairList.AsQueryable();
            result = gs.ToDataSourceResult(request);

            result.Total = fieldsValuePairList.Count;
            return Json(result);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetProfiles([DataSourceRequest]DataSourceRequest request, string EmailAddress, DateTime StartDate, DateTime EndDate, int DomainTrackerID, int PageNumber, int PageSize, string TypeFilter, string PageUrl)
        {
            DataSourceResult result;
            string sortColumn = "EmailAddress";
            string sortdirection = "ASC";

            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts[0].Member;
                sortdirection = request.Sorts[0].SortDirection == System.ComponentModel.ListSortDirection.Ascending ? "ASC" : "DESC";
            }

            List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> profiles = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.GetByDomainTrackerID(
                DomainTrackerID,
                PageNumber,
                request.PageSize,
                UserSession.CurrentUser,
                StartDate.ToShortDateString(),
                EndDate.ToShortDateString(),
                EmailAddress,
                TypeFilter,
                sortColumn,
                sortdirection,
                PageUrl
            );

            IQueryable<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> gs = profiles.AsQueryable();
            result = profiles.AsQueryable().ToDataSourceResult(request);
            int totalCount = 0;
            if (profiles.Count > 0)
                totalCount = profiles[0].ProfileCount;
            result.Total = totalCount;
            return Json(result);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Response.Redirect("/ecn.accounts/logoff.aspx");
            return View();
        }

        [Authorize]
        public FileResult PageViews(int id, int random, DateTime? startDate = null, DateTime? endDate = null, string Filter = "", string TypeFilter = "known")
        {
            try
            {
                DateTime d1 = (DateTime)startDate;
                DateTime d2 = (DateTime)endDate;

                //TODO: Find out why this method is called twice; the second time w/o an endDate TIME
                if (endDate.ToString().Contains("AM"))
                {
                    d2 = d2.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                TimeSpan t = d2 - d1;
                double NrOfDays = t.TotalDays;
                int AxisXInterval = 3;
                if (NrOfDays >= 31 && NrOfDays <= 60)
                {
                    AxisXInterval = 4;
                }
                else if (NrOfDays >= 61 && NrOfDays <= 90)
                {
                    AxisXInterval = 5;
                }
                else if (NrOfDays >= 91 && NrOfDays <= 180)
                {
                    AxisXInterval = 10;
                }
                else if (NrOfDays > 180)
                {
                    AxisXInterval = 30;
                }

                Chart chtPageViews = new Chart();
                
                chtPageViews.RenderType = RenderType.ImageTag;
                chtPageViews.Series.Clear();
                chtPageViews.ChartAreas.Clear();
                chtPageViews.DataSource = null;

                DataTable dt = new DataTable();
                bool showUnknown = KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(ECNSession.CurrentSession().ClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.TrackAnonymous);
                if (!showUnknown || TypeFilter.ToLower().Equals("known") || TypeFilter.ToLower().Equals("unknown"))
                {
                    dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetPageViewsPerDay(id, d1.ToString(), d2.ToString(), Filter, TypeFilter);
                }
                else
                {
                    dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetPageViewsPerDay_Known(id, d1.ToString(), d2.ToString(), Filter);
                }

                if (dt.Rows.Count > 0 && (!showUnknown || TypeFilter.ToLower().Equals("known") || TypeFilter.ToLower().Equals("unknown")))
                {
                    chtPageViews.DataSource = dt;
                    chtPageViews.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                    chtPageViews.BorderlineDashStyle = ChartDashStyle.Solid;
                    chtPageViews.BorderWidth = 8;

                    SetChartSeries(chtPageViews, PageViewsKey, string.Empty, Color.SteelBlue);
                    SetChartAreas(chtPageViews, ChartArea1Key, AxisXInterval);

                    chtPageViews.Height = 300;
                    chtPageViews.Width = 600;
                    chtPageViews.DataBind();

                    var imgStream = new MemoryStream();
                    chtPageViews.ImageType = ChartImageType.Png;
                    chtPageViews.SaveImage(imgStream, ChartImageFormat.Png);
                    imgStream.Seek(0, SeekOrigin.Begin);

                    return File(imgStream, "image/png");
                }
                else if (dt.Rows.Count > 0 && showUnknown && TypeFilter.ToLower().Equals("all"))
                {
                    //chtPageViews.CustomizeLegend += ChtPageViews_CustomizeLegend;
                    //chtPageViews.DataSource = dt;
                    DataTable dtTotal = new DataTable();
                    DataRow[] drTotal = dt.Select("Type = 'Total'");
                    if (drTotal.Length > 0)
                    {
                        dtTotal = dt.Select("Type = 'Total'").OrderBy(x => x.Field<DateTime>("Date")).CopyToDataTable();
                    }
                    DataTable dtKnown = new DataTable();
                    DataRow[] drKnown = dt.Select("Type = 'Known'");
                    if (drKnown.Length > 0)
                    {
                        dtKnown = dt.Select("Type = 'Known'").OrderBy(x => x.Field<DateTime>("Date")).CopyToDataTable();
                    }
                    DataRow[] drUnknown = dt.Select("Type = 'Unknown'");
                    DataTable dtUnknown = new DataTable();
                    if (drUnknown.Length > 0)
                    {
                        dtUnknown = dt.Select("Type = 'Unknown'").OrderBy(x => x.Field<DateTime>("Date")).CopyToDataTable();
                    }
                    chtPageViews.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                    chtPageViews.BorderlineDashStyle = ChartDashStyle.Solid;
                    chtPageViews.BorderWidth = 8;

                    Legend knownLegend = new Legend("Known");
                    knownLegend.Position.Auto = true;
                    knownLegend.Docking = Docking.Top;
                    knownLegend.Alignment = StringAlignment.Near;
                    knownLegend.LegendStyle = LegendStyle.Column;
                    knownLegend.BackColor = Color.Transparent;
                    knownLegend.DockedToChartArea = "ChartArea1";
                    knownLegend.LegendItemOrder = LegendItemOrder.SameAsSeriesOrder;
                    //knownLegend.Position = new ElementPosition(15, 85, 40, 30);
                    chtPageViews.Legends.Add(knownLegend);

                    SetChartSeries(chtPageViews, KnownKey, KnownKey, Color.Orange);

                    foreach (DataRow dr in dtKnown.Rows)
                    {
                        chtPageViews.Series["Known"].Points.AddXY(dr["Date"], dr["Views"]);
                    }
                    //Unknown

                    Legend unKnownLegend = new Legend("Unknown");
                    unKnownLegend.Position.Auto = true;
                    unKnownLegend.Docking = Docking.Top;
                    unKnownLegend.Alignment = StringAlignment.Near;
                    unKnownLegend.LegendStyle = LegendStyle.Column;
                    unKnownLegend.BackColor = Color.Transparent;
                    unKnownLegend.LegendItemOrder = LegendItemOrder.SameAsSeriesOrder;
                    //unKnownLegend.Position = new ElementPosition(0, 85, 40, 30);
                    unKnownLegend.DockedToChartArea = "ChartArea1";

                    chtPageViews.Legends.Add(unKnownLegend);

                    SetChartSeries(chtPageViews, UnknownKey, UnknownKey, Color.Gray);

                    foreach (DataRow dr in dtUnknown.Rows)
                    {
                        chtPageViews.Series["Unknown"].Points.AddXY(dr["Date"], dr["Views"]);
                    }

                    Legend allLegend = new Legend("All");
                    allLegend.Position.Auto = true;
                    allLegend.Docking = Docking.Top;

                    allLegend.Alignment = StringAlignment.Near;
                    allLegend.LegendStyle = LegendStyle.Column;
                    allLegend.BackColor = Color.Transparent;
                    allLegend.DockedToChartArea = "ChartArea1";
                    allLegend.LegendItemOrder = LegendItemOrder.SameAsSeriesOrder;
                    //allLegend.Position = new ElementPosition(30, 85, 40, 30);
                    chtPageViews.Legends.Add(allLegend);
                    chtPageViews.Series.Add("All");
                    chtPageViews.Series["All"].XValueMember = "Date";
                    chtPageViews.Series["All"].YValueMembers = "Views";
                    chtPageViews.Series["All"].ChartType = SeriesChartType.Line;
                    chtPageViews.Series["All"].IsVisibleInLegend = true;
                    chtPageViews.Series["All"].Legend = "All";
                    chtPageViews.Series["All"].ShadowOffset = 0;
                    chtPageViews.Series["All"].ToolTip = "#VALY{G}";
                    chtPageViews.Series["All"].BorderWidth = 3;
                    chtPageViews.Series["All"].Color = System.Drawing.Color.SteelBlue;
                    chtPageViews.Series["All"].IsValueShownAsLabel = true;
                    chtPageViews.Series["All"].MarkerSize = 10;
                    chtPageViews.Series["All"].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                    foreach (DataRow dr in dtTotal.Rows)
                    {
                        chtPageViews.Series["All"].Points.AddXY(dr["Date"], dr["Views"]);
                    }
                    
                    SetChartAreas(chtPageViews, ChartArea1Key, AxisXInterval);

                    chtPageViews.Height = 400;
                    chtPageViews.Width = 700;
                    //chtPageViews.DataBindCrossTable(dt.AsEnumerable(), "Type", "Date", "Views","");

                    var imgStream = new MemoryStream();
                    chtPageViews.ImageType = ChartImageType.Png;
                    chtPageViews.SaveImage(imgStream, ChartImageFormat.Png);
                    imgStream.Seek(0, SeekOrigin.Begin);

                    return File(imgStream, "image/png");
                }
                else
                {
                    string noRowsImage = Server.MapPath(ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + ConfigurationManager.AppSettings["NoRowsImage"].ToString());
                    FileStream fs = new FileStream(noRowsImage, FileMode.Open);
                    Bitmap bmp = (Bitmap)System.Drawing.Image.FromStream(fs);
                    fs.Dispose();
                    fs.Close();
                    bmp = new Bitmap(bmp);
                    var imgStream = new MemoryStream();
                    bmp.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);
                    imgStream.Seek(0, SeekOrigin.Begin);
                    return File(imgStream, "image/png");

                }
            }
            catch (ECNException ex)
            {
                string exception = String.Empty;
                foreach (ECNError e in ex.ErrorList)
                {
                    exception += e.ErrorMessage;
                }
                return null;
            }
        }

        private void SetChartSeries(Chart chart, string property, string legend, Color selectedColor)
        {
            Guard.NotNull(chart, nameof(chart));

            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentException(ArgumentExceptionMessage, nameof(property));
            }

            chart.Series.Add(property);
            chart.Series[property].XValueMember = DateKey;
            chart.Series[property].YValueMembers = "Views";
            chart.Series[property].ChartType = SeriesChartType.Line;
            chart.Series[property].IsVisibleInLegend = true;
            chart.Series[property].ShadowOffset = 0;
            chart.Series[property].ToolTip = "#VALY{G}";
            chart.Series[property].BorderWidth = 3;
            chart.Series[property].Color = selectedColor;
            chart.Series[property].IsValueShownAsLabel = true;
            chart.Series[property].MarkerSize = 10;
            chart.Series[property].MarkerStyle = MarkerStyle.Circle;

            if (!string.IsNullOrWhiteSpace(legend))
            {
                chart.Series[property].Legend = legend;
            }
        }

        private void SetChartAreas(Chart chart, string property, int axisXInterval)
        {
            Guard.NotNull(chart, nameof(chart));

            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentException(ArgumentExceptionMessage, nameof(property));
            }

            chart.ChartAreas.Add(property);
            chart.ChartAreas[property].AxisX.Title = DateKey;
            chart.ChartAreas[property].AxisY.Title = "Page Views";
            chart.ChartAreas[property].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[property].AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas[property].AxisX.Interval = axisXInterval;
            chart.ChartAreas[property].BackColor = Color.Transparent;
            chart.ChartAreas[property].ShadowColor = Color.Transparent;
            chart.ChartAreas[property].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart.ChartAreas[property].AxisY.MajorGrid.LineColor = Color.LightGray;
        }

        private void ChtPageViews_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
        {
            var items = e.LegendItems;
            var knownItem = items[1];
            var unKnownItem = items[2];
            items.RemoveAt(2);
            items.RemoveAt(1);
            items.Insert(0,unKnownItem);
            items.Insert(1, knownItem);
        }

        [HttpPost]
        public ActionResult AddToGroup(GroupSelectorModel Model)
        {
            try
            {
                var DTUViewModel = DomainTrackerUsersViewModel(Model.DomainTrackId, Model.FilterEmail, Model.pathname, Model.fromDate, Model.toDate, Model.pageUrl, Model.TypeFilter);

                DTUViewModel.ECNGroupId = Model.GroupId;
                ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
                StringBuilder xmlInsert = new StringBuilder();
                xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                for (int i = 0; i < DTUViewModel.ProfileList.Count; i++)
                {
                    string email = DTUViewModel.ProfileList[i].EmailAddress.Trim();
                    xmlInsert.Append("<Emails><emailaddress>" + email + "</emailaddress></Emails>");
                }
                DTUViewModel.emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(UserSession.CurrentUser, UserSession.CurrentUser.CustomerID, DTUViewModel.ECNGroupId, xmlInsert.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "html", "S", true, "", "Ecn.Domaigntracking.controllers.maincontroller.AddToGroup");
                return Content("Import to Group " + Model.GroupName + " was successful");
            }
            catch (Exception ex)
            {
                return Content("An error occurred");
            }

        }

        [HttpPost]
        public ActionResult CreateGroup(GroupSelectorModel Model)
        {
            try
            {
                ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                string gname = string.Empty;
                if (Model != null && Model.GroupName != null) { gname = ECN_Framework_Common.Functions.StringFunctions.CleanString(Model.GroupName.Trim()); }
                string gdesc = string.Empty;
                if (Model != null && Model.GroupDescription != null) { gdesc = ECN_Framework_Common.Functions.StringFunctions.CleanString(Model.GroupDescription.Trim()); }

                int newGroupId = -1;

                try
                {
                    ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                    group.GroupName = gname;
                    group.GroupDescription = gdesc;
                    group.FolderID = Model.FolderId;
                    group.PublicFolder = 0;
                    group.IsSeedList = false;
                    group.AllowUDFHistory = "N";
                    group.OwnerTypeCode = "customer";
                    group.CreatedUserID = UserSession.CurrentUser.UserID;
                    group.CustomerID = UserSession.CurrentUser.CustomerID;
                    newGroupId = ECN_Framework_BusinessLayer.Communicator.Group.Save(group, UserSession.CurrentUser);
                }

                catch (ECNException ex)
                {
                    string exception = ex.ErrorList.Aggregate(String.Empty, (current, e) => current + e.ErrorMessage);
                    return Content(exception);
                }

                if (newGroupId == -1)
                {
                    return Content("Unable to create Group " + Model.GroupName);
                }

                var DTUViewModel = DomainTrackerUsersViewModel(Model.DomainTrackId, Model.FilterEmail, Model.pathname, Model.fromDate, Model.toDate, Model.pageUrl, Model.TypeFilter);

                StringBuilder xmlInsert = new StringBuilder();
                xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                for (int i = 0; i < DTUViewModel.ProfileList.Count; i++)
                {
                    string email = DTUViewModel.ProfileList[i].EmailAddress.Trim();
                    xmlInsert.Append("<Emails><emailaddress>" + email + "</emailaddress></Emails>");
                }
                DTUViewModel.emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(UserSession.CurrentUser, UserSession.CurrentUser.CustomerID, newGroupId, xmlInsert.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "html", "S", true, "", "Ecn.Domaigntracking.controllers.maincontroller.CreateGroup");
                return Content("Group " + Model.GroupName + " has been created");
            }
            catch (Exception ex)
            {
                return Content("An error occurred");
            }
        }

        public static class Extensions
        {
            public static string ToXml(DataSet ds)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (TextWriter streamWriter = new StreamWriter(memoryStream))
                    {
                        var xmlSerializer = new XmlSerializer(typeof(DataSet));
                        xmlSerializer.Serialize(streamWriter, ds);
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }

        public FileContentResult ExportUserActivity(int DomainTrackId, string FilterEmail, string StartDate = null, string EndDate = null, string ExportType = null, string pathname = null, string fromDate = null, string toDate = null, string TypeFilter = "known", string pageUrl = null)  //pageUrl must ALWAYS be last param
        {
            string fileName = "UserActivity";

            var DTUViewModel = DomainTrackerUsersViewModel(DomainTrackId, FilterEmail, pathname, fromDate, toDate, pageUrl, TypeFilter);
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserActivityToExport> userActivityList = new List<DomainTrackerUserActivityToExport>();

            userActivityList = DTUViewModel.GetUserListForExport();
            if (ExportType == "CSV")
            {
                ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportCSV(userActivityList, fileName, StartDate, EndDate);
            }
            else if (ExportType == "XLSX")
            {
                ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToExcel(userActivityList, fileName);
            }
            else
            {
                string strXML = string.Empty;
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(userActivityList.GetType());
                StringWriter strWriter = new StringWriter();
                x.Serialize(strWriter, userActivityList);
                fileName += ".xml";
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strWriter.ToString());
                return File(bytes, "application/text", fileName);
            }


            return null;
        }

        private DomainTrackerUsersViewModel DomainTrackerUsersViewModel(int DomainTrackId, string FilterEmail, string pathname, string fromDate, string toDate, string pageUrl, string TypeFilter)
        {

            //USER Page
            if (fromDate.Length > 0 && toDate.Length > 0)
            {
                DateTime FROMdate = new DateTime();
                DateTime TODate = new DateTime();
                if (DateTime.TryParse(fromDate, out FROMdate))
                {
                    DateTime.TryParse(toDate, out TODate);
                }

                return GetDTUViewModel(DomainTrackId, FilterEmail, FROMdate, TODate, pageUrl, TypeFilter);
            }
            else
            {
                return GetDTUViewModel(DomainTrackId, FilterEmail, null, null, pageUrl, TypeFilter);
            }

            //REPORT-ShowUsers Page
            //var fixedPageUrl = HttpContext.Request.RawUrl.Split(new[] { "pageUrl=" }, StringSplitOptions.None);
            //if (fixedPageUrl.Count() == 2)
            //{
            //    if (string.IsNullOrWhiteSpace(fromDate))
            //    {
            //        fromDate = new DateTime(1753, 1, 1).ToString(CultureInfo.InvariantCulture);
            //    }
            //    if (string.IsNullOrWhiteSpace(toDate))
            //    {
            //        toDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            //    }

            //    return GetDTUViewModel(DomainTrackId, FilterEmail, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), fixedPageUrl[1], TypeFilter);
            //}

            //TempData["DomainTracker_PageURL"] = pageUrl;
            //fixedPageUrl = pageUrl.Split(new[] { "pageUrl=" }, StringSplitOptions.None);
            //return GetDTUViewModel(DomainTrackId, FilterEmail, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), fixedPageUrl[0], TypeFilter);
        }

        DomainTrackerUsersViewModel GetDTUViewModel(int id, string FilterEmail = null, DateTime? FromDate = null, DateTime? ToDate = null, string pageURL = null, string TypeFilter = "known")
        {
            DomainTrackerUsersViewModel Model = new DomainTrackerUsersViewModel();
            DateTime fDate = new DateTime();
            DateTime tDate = new DateTime();
            Model.DomainTrackId = id;

            try
            {
                if (FromDate != null)
                    fDate = DateTime.Parse(FromDate.ToString());

                if (ToDate != null)
                    tDate = DateTime.Parse(ToDate.ToString());

            }
            catch (Exception ex)
            {
                ViewBag.Message += "Error in parsing date:" + ex.Message;
            }

            Model.DomainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(id, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            ECN_Framework_BusinessLayer.Application.ECNSession UserSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            Model.ProfileList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile>();
            Model.ActivityList = new List<List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>>();
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> profileList = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.GetByDomainTrackerID(id, null, null, UserSession.CurrentUser, fDate == new DateTime() ? "" : fDate.ToShortDateString(), tDate == new DateTime() ? "" : tDate.ToShortDateString(), FilterEmail, TypeFilter, "EmailAddress", "ASC", pageURL);
            try
            {
                for (int j = 0; j < profileList.Count; j++)
                {

                    List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity> userActivity =
                        ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetByProfileID(profileList[j].ProfileID, Model.DomainTrackId, UserSession.CurrentUser, fDate == new DateTime() ? "" : fDate.ToShortDateString(), tDate == new DateTime() ? "" : tDate.ToShortDateString(), pageURL);

                    Model.ProfileList.Add(profileList[j]);
                    Model.ActivityList.Add(userActivity);

                }
                return Model;
            }
            catch (Exception e)
            {
                ViewBag.Message += "Exception in getting user activity " + e.ToString();
            }
            return null;
        }
        #endregion
    }
}