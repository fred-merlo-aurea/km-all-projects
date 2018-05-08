using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ecn.communicator.mvc.Models;
using ecn.communicator.mvc.Infrastructure;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using KMSite;
using BusinessLayerCommunicator = ECN_Framework_BusinessLayer.Communicator;

namespace ecn.communicator.mvc.Controllers
{
    public class FilterController : BaseController
    {
        private const string SubscribeTypeAll = " 'S', 'U', 'D', 'P', 'B', 'M' ";

        public ActionResult Index(int id)
        {
            int GroupID = 0;
            if (id > 0)
                GroupID = id;
            else
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });

            Session["selectedGroupID"] = GroupID;
            System.Web.HttpCookie archiveFilterCookie = Request.Cookies["archiveFilter"];
            string archiveFilter = "Active";
            if (archiveFilterCookie != null)
                archiveFilter = archiveFilterCookie.Value.ToString();
            /*Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "";
            Master.Heading = "Groups > Manage Groups > Smart Forms > Filters";
            Master.HelpContent = "<B>Adding/Editing Filters</B><div id='par1'><ul><li>Find the Group you want to create a filter for.</li><li>Click on the <em>Funnel</em> icon for that group.</li><li>Enter a title for your filter (for example, pet owners)</li><li>Click <em>Create new filter</em>.</li><li>Under filter names, click on the <em>pencil (Add/Edit Filter Attributes)</em> icon to define the filter attributes.</li><li>In the Compare Field section, use the drop down menu and click on profile field to define attributes of your filter.</li><li>In the Comparator section you have the option of making the field equal to (=), contains, ends with, or starts with.</li><li>In the Compare Value field, enter the information you would want the system to filter (for example, dog).</li><li>The Join Filters allow you to select And, or, Or.</li><li>To add, click <em>Add this Filter</em>.</li><li>Repeat this process several times to fully develop the attributes you are looking for (for example, dog, dogs, cat, cats, dog owners, etc.)</li><li>After all fields and attributes have been selected and added, click <em>Preview filtered e-mails</em> button to view emails in your filtered list.</li><li>When filter is complete, Click on <em>Return to Filters List</em>.</li></ul></div>";
            Master.HelpTitle = "Filters Manager";*/

            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            List<ECN_Framework_Entities.Communicator.Filter> filterList = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(GroupID, false, User, archiveFilter);
            Filters Model = new Filters(filterList, GroupID, ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, User).GroupName);
            Model.ArchiveFilter = archiveFilter;
            //Write Permissions
            if (KM.Platform.User.HasAccess(User, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.Edit))
            {
                Model.canWrite = true;
                /*
                btnAddFilter.Visible = true;
                btnCopyFilter.Visible = true;
                FilterGrid.Columns[2].Visible = true;
                FilterGrid.Columns[3].Visible = true;
                 */
            }
            else
            {
                /*
                btnAddFilter.Visible = false;
                btnCopyFilter.Visible = false;
                FilterGrid.Columns[2].Visible = false;
                FilterGrid.Columns[3].Visible = false;
                 */
            }

            //Read Permissions
            if (KM.Platform.User.HasAccess(User, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.View))
            {
                Model.canRead = true;
                /*
                ViewState["SortField"] = "FilterName";
                ViewState["SortDirection"] = "ASC";*/
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, User);
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
            return View(Model);
        }

        #region Filters
        public ActionResult Edit(int id)
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.Edit))
            {
                ECN_Framework_Entities.Communicator.Filter filter = null;
                int filterID = id;
                if (filterID > 0)
                    filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                else
                    return RedirectToAction("Error", "Error", new { E = "InvalidLink" });

                Session["selectedFilterID"] = filterID;
                if (filter != null && filter.FilterID > 0)
                {
                    return View(filter);
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public ActionResult LoadGroupGrid(int filterID)
        {
            return PartialView("Partials/_FilterGroupGrid", ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser).FilterGroupList);
        }

        public ActionResult LoadConditionGrid(int filterGroupID)
        {
            return PartialView("Partials/_FilterConditionGrid", ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser));
        }

        public ActionResult CreateFilter(ECN_Framework_Entities.Communicator.Filter model)
        {
            ECN_Framework_Entities.Communicator.Filter filter = new ECN_Framework_Entities.Communicator.Filter();
            filter.FilterName = model.FilterName ?? "";
            filter.GroupCompareType = model.GroupCompareType;
            filter.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            filter.GroupID = (int)Session["selectedGroupID"];
            filter.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;

            int FilterID = 0;
            List<string> response = new List<string>();
            try
            {
                FilterID = ECN_Framework_BusinessLayer.Communicator.Filter.Save(filter, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            response.Add("200");
            response.Add(new UrlHelper(Request.RequestContext).Action("Edit", "Filter", new { id = FilterID }));
            return Json(response);
        }

        public ActionResult Update(ECN_Framework_Entities.Communicator.Filter model)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(model.FilterID, User);
            filter.FilterName = model.FilterName ?? "";
            filter.GroupCompareType = model.GroupCompareType;
            filter.UpdatedUserID = User.UserID;

            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Filter.Save(filter, User);
                Session["selectedFilterID"] = filter.FilterID;
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            response.Add("200");
            response.Add(new UrlHelper(Request.RequestContext).Action("Edit", "Filter", new { id = filter.FilterID }));
            return Json(response);
        }

        public ActionResult Preview(int id)
        {
            ECN_Framework_Entities.Communicator.Filter filter = null;
            int filterID = id;

            int GroupID = 0;
            //if (System.Web.HttpContext.Current.Session == null || Session["selectedGroupID"] == null)
            //    return RedirectToAction("Index", "Group");
            //else
            //    GroupID = (int)Session["selectedGroupID"];

            //Session["selectedFilterID"] = filterID;
            //Session["selectedGroupID"] = GroupID;
            List<Models.Email> internalEmails = new List<Models.Email>();

            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.View))
            {
                int CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                if (filterID > 0)
                    filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                else
                    RedirectToAction("Index");

                string whereClause = filter.WhereClause.Trim();
                if (whereClause.Length > 0)
                {
                    whereClause = " AND (" + whereClause + ")";
                }

                System.Data.DataTable emails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.PreviewFilteredEmails_Paging(filter.GroupID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, whereClause.ToString(), "EmailAddress", "ASC", 1, 15);
                internalEmails = emails.DataTableToList<Models.Email>();
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            ViewBag.FilterID = filterID;
            ViewBag.GroupID = filter.GroupID.Value;
            ViewBag.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            return View(internalEmails);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult PreviewEmails_Read([DataSourceRequest]DataSourceRequest request, int filterID, int GroupID, int PageNumber, int PageSize)
        {
            int totalCount = 0;
            DataTable dtGroups;
            DataSourceResult result;
            List<Models.Email> internalEmails = new List<Models.Email>();
            ECN_Framework_Entities.Communicator.Filter filter = null;

            string sortColumn = "GroupName"; // lstgs[0].SortColumnName;
            string sortdirection = "ASC";
            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortdirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

            }

            int CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            if (filterID > 0)
                filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            else
                RedirectToAction("Index");

            string whereClause = filter.WhereClause.Trim();
            if (whereClause.Length > 0)
            {
                whereClause = " AND (" + whereClause + ")";
            }

            System.Data.DataTable emails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.PreviewFilteredEmails_Paging(GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, whereClause.ToString(), sortColumn, sortdirection, PageNumber, request.PageSize);
            internalEmails = emails.DataTableToList<Models.Email>();

            ViewBag.FilterID = filterID;
            ViewBag.GroupID = filter.GroupID.Value;
            ViewBag.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;

            if (emails != null && emails.Rows.Count > 0)
                totalCount = Convert.ToInt32(emails.Rows[0]["TotalCount"].ToString());

            IQueryable<Models.Email> gs = internalEmails.AsQueryable();
            result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result);
        }

        public void downloadFile(string channelID, string customerID, string groupID, string fileType, string subType, string srchType, string srchEm, string filterID)
        {
            string emailAddr = srchEm;
            emailAddr = emailAddr.Replace("_", "[_]");
            string subscribeType = subType;
            string searchType = srchType;
            string downloadType = fileType;

            string profFilter = "ProfilePlusAllUDFs";

            string OSFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/downloads/");

            var delimiter = string.Empty;
            var filterId = 0;
            int.TryParse(filterID, out filterId);
            var filter = ECN_Framework.WebPageHelper.PopulateFilter(downloadType, emailAddr, searchType, filterId, SubscribeTypeAll, ref subscribeType, out delimiter);

            DataTable emailProfilesDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(Convert.ToInt32(groupID), Convert.ToInt32(customerID), filter, subscribeType, profFilter);

            String tfile = customerID + "-" + groupID + "emails" + downloadType;
            string outfileName = OSFilePath + tfile;

            ECN_Framework.WebPageHelper.PopulateResponse(OSFilePath, outfileName, downloadType, emailProfilesDT, delimiter, tfile, Response);
        }

        public ActionResult LoadCopyFilterGroups()
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList =
            ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            var result = (from src in groupList
                          orderby src.GroupName
                          select src).ToList();

            result.Insert(0, new ECN_Framework_Entities.Communicator.Group() { GroupName = "--- Select Group Name ---", GroupID = 0 });

            return PartialView("Partials/Modals/_CopyFilter", result);
        }

        public string LoadCopyFilterList(int groupID)
        {
            int GroupID = groupID;
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            List<ECN_Framework_Entities.Communicator.Filter> sourceFilters = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(GroupID, true, User);
            List<ECN_Framework_Entities.Communicator.Filter> destFilters = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID((int)Session["selectedGroupID"], true, User);
            List<ECN_Framework_Entities.Communicator.Filter> availableFilters = new List<ECN_Framework_Entities.Communicator.Filter>();

            foreach (ECN_Framework_Entities.Communicator.Filter f in sourceFilters)
            {
                if (destFilters.Count(x => x.FilterName.ToLower() == f.FilterName.ToLower()) == 0)
                {
                    availableFilters.Add(f);
                }
            }

            string cb = string.Empty;
            if (availableFilters.Count > 0)
            {
                foreach (var fl in availableFilters)
                {
                    cb += "<input type=\"checkbox\" value=\"" + fl.FilterID + "\" style=\"width: 13px;height: 13px;padding: 0;margin: 0;\" checked=\"checked\" />";
                    cb += "<span>" + fl.FilterName + "</span><br />";
                }
            }
            else
            {
                cb = "No Filters Available";
            }
            return cb;
        }

        public ActionResult CopyFilter(int[] selected, int selectedGroupID)
        {
            List<string> response = new List<string>();
            int GroupID = selectedGroupID;
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, User);
            List<string> missingUDFs = new List<string>();
            List<ECN_Framework_Entities.Communicator.Filter> filtersToCopy = new List<ECN_Framework_Entities.Communicator.Filter>();

            foreach (int filterID in selected)
            {
                ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, User);
                filtersToCopy.Add(filter);
                if (filter.WhereClause.Contains('['))
                {
                    string[] shortnames = filter.WhereClause.Split('[');
                    //looping through where clause to find UDFs and verify they exist, if they don't building a list to display missing udfs to the user
                    foreach (string s in shortnames)
                    {
                        if (s.Contains(']'))
                        {
                            string shortname = s.Substring(0, s.IndexOf(']'));

                            if (!ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Exists(shortname, null, GroupID, User.CustomerID) && !IsProfileField(shortname))
                            {
                                if (!missingUDFs.Contains(shortname))
                                    missingUDFs.Add(shortname);

                            }
                        }
                    }
                }
            }
            if (missingUDFs.Count > 0)
            {
                StringBuilder sbUDFs = new StringBuilder();
                foreach (string s in missingUDFs)
                {
                    sbUDFs.Append(s + ",");
                }
                response.Add("500");
                response.Add("Please add the following UDFs to the group: " + sbUDFs.ToString().TrimEnd(','));
                return Json(response);
            }
            else
            {
                try
                {
                    foreach (ECN_Framework_Entities.Communicator.Filter filter in filtersToCopy)
                    {
                        //Done checking udfs, can copy the filter now
                        ECN_Framework_Entities.Communicator.Filter newFilter = new ECN_Framework_Entities.Communicator.Filter();
                        newFilter.CreatedUserID = User.UserID;
                        newFilter.CustomerID = User.CustomerID;
                        newFilter.DynamicWhere = filter.DynamicWhere;
                        newFilter.FilterGroupList = filter.FilterGroupList;
                        newFilter.FilterName = filter.FilterName;
                        newFilter.GroupCompareType = filter.GroupCompareType;
                        newFilter.GroupID = GroupID;
                        newFilter.IsDeleted = false;
                        newFilter.WhereClause = filter.WhereClause;
                        foreach (ECN_Framework_Entities.Communicator.FilterGroup fg in filter.FilterGroupList)
                        {
                            fg.FilterGroupID = -1;
                            fg.CreatedUserID = User.UserID;
                            foreach (ECN_Framework_Entities.Communicator.FilterCondition fc in fg.FilterConditionList)
                            {
                                fc.FilterConditionID = -1;
                                fc.CreatedUserID = User.UserID;
                            }
                        }
                        ECN_Framework_BusinessLayer.Communicator.Filter.Save(newFilter, User);
                    }

                }
                catch (ECNException ecn)
                {
                    string err = string.Empty;
                    foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecn.ErrorList)
                    {
                        err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                    }
                    response.Add("500");
                    response.Add(err);
                    return Json(response);
                }
            }
            response.Add("200");
            response.Add(Session["selectedGroupID"].ToString());
            return Json(response);
        }

        public ActionResult Delete(int id, int groupID)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Filter.Delete(id, ConvenienceMethods.GetCurrentUser());
                string redirect = "/ecn.communicator.mvc/Filter/Index/" + groupID.ToString();
                response.Add("200");
                response.Add(redirect);
                return Json(response);
            }
            catch (ECNException ecn)
            {
                response.Add("500");
                string err = "";

                int index = 1;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecn.ErrorList)
                {
                    //err += ecnError.Entity + ": " + ecnError.ErrorMessage;
                    //if (index < ecn.ErrorList.Count)
                    //    err += "<br />";
                    //index++;
                    response.Add(ecnError.Entity + ": " + ecnError.ErrorMessage);
                }

                //response.Add(err);
                return Json(response);

            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add(ex.Message);
                return Json(response);
            }
        }

        public string ArchiveFilter(int id)
        {
            ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(id, ConvenienceMethods.GetCurrentUser());
            bool archive = f.Archived.HasValue ? f.Archived.Value : false;
            ECN_Framework_BusinessLayer.Communicator.Filter.ArchiveFilter(id, !archive, ConvenienceMethods.GetCurrentUser());

            return f.GroupID.ToString();
        }

        private bool IsProfileField(string shortname)
        {
            if (BusinessLayerCommunicator.FilterBase.CommonFiltersFields.Contains(shortname)
                || BusinessLayerCommunicator.FilterBase.NonCommonFiltersFields.Contains(shortname))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region FilterGroups
        public ActionResult CreateFilterGroup(ECN_Framework_Entities.Communicator.FilterGroup model)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.FilterGroup group = new ECN_Framework_Entities.Communicator.FilterGroup();
            int filterID = (int)Session["selectedFilterID"];
            group.Name = model.Name.Trim();
            group.ConditionCompareType = model.ConditionCompareType;
            group.FilterID = filterID;
            group.CustomerID = User.CustomerID;
            group.CreatedUserID = User.UserID;

            ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, User);
            if (filter.FilterGroupList.Count == 0)
            {
                group.SortOrder = 1;
            }
            else
            {
                var result = (from src in filter.FilterGroupList
                              group src by src.SortOrder into grp
                              select new
                              {
                                  SortOrder = grp.Max(t => t.SortOrder)
                              }).ToList();

                group.SortOrder = result[0].SortOrder + 1;
            }

            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.FilterGroup.Save(group, User);
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            response.Add("200");
            response.Add(new UrlHelper(Request.RequestContext).Action("Edit", "Filter", new { id = filter.FilterID }));
            return Json(response);
        }

        public ActionResult LoadEditFilterGroup(int filterGroupID)
        {
            return PartialView("Partials/Modals/_EditFilterGroup", ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser));
        }

        public ActionResult EditFilterGroup(ECN_Framework_Entities.Communicator.FilterGroup model)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.FilterGroup group = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(model.FilterGroupID, User);
            group.Name = model.Name;
            group.ConditionCompareType = model.ConditionCompareType;
            group.UpdatedUserID = User.UserID;

            ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(group.FilterID, User);
            if (filter.FilterGroupList.Count == 0)
            {
                group.SortOrder = 1;
            }
            else
            {
                var result = (from src in filter.FilterGroupList
                              group src by src.SortOrder into grp
                              select new
                              {
                                  SortOrder = grp.Max(t => t.SortOrder)
                              }).ToList();

                group.SortOrder = result[0].SortOrder + 1;
            }

            ECN_Framework_BusinessLayer.Communicator.FilterGroup.Save(group, User);
            return JavaScriptRedirectToAction("Edit", "Filter", new { id = filter.FilterID });
        }

        public ActionResult DeleteFilterGroup(int id)
        {
            int filterID = (int)Session["selectedFilterID"];
            ECN_Framework_BusinessLayer.Communicator.FilterGroup.Delete(filterID, id, ConvenienceMethods.GetCurrentUser());

            return JavaScriptRedirectToAction("Edit", "Filter", new { id = filterID });
        }
        #endregion

        #region FilterConditions
        public ActionResult LoadAddFilterCondition(int filterGroupID)
        {
            ECN_Framework_Entities.Communicator.FilterGroup fg =
            ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            ECN_Framework_Entities.Communicator.Filter f =
            ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(fg.FilterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            ECN_Framework_Entities.Communicator.FilterCondition fc = new ECN_Framework_Entities.Communicator.FilterCondition();
            fc.FilterGroupID = filterGroupID;

            // LoadUserFields
            Dictionary<string, string> fields = new Dictionary<string, string>();
            //List<string> fields = new List<string>();
            fields.Add("EmailAddress", "EmailAddress");
            fields.Add("FormatTypeCode", "FormatTypeCode");
            fields.Add("SubscribeTypeCode", "SubscribeTypeCode");
            fields.Add("Title", "Title");
            fields.Add("FirstName", "FirstName");
            fields.Add("LastName", "LastName");
            fields.Add("FullName", "FullName");
            fields.Add("Company", "Company");
            fields.Add("Occupation", "Occupation");
            fields.Add("Address", "Address");
            fields.Add("Address2", "Address2");
            fields.Add("City", "City");
            fields.Add("State", "State");
            fields.Add("Zip", "Zip");
            fields.Add("Country", "Country");
            fields.Add("Voice", "Voice");
            fields.Add("Mobile", "Mobile");
            fields.Add("Fax", "Fax");
            fields.Add("Website", "Website");
            fields.Add("Age", "Age");
            fields.Add("Income", "Income");
            fields.Add("Gender", "Gender");
            fields.Add("User1", "User1");
            fields.Add("User2", "User2");
            fields.Add("User3", "User3");
            fields.Add("User4", "User4");
            fields.Add("User5", "User5");
            fields.Add("User6", "User6");
            fields.Add("Birthdate", "Birthdate [MM/DD/YYYY]");
            fields.Add("UserEvent1", "UserEvent1");
            fields.Add("UserEvent1Date", "UserEvent1Date [MM/DD/YYYY]");
            fields.Add("UserEvent2", "UserEvent2");
            fields.Add("UserEvent2Date", "UserEvent2Date [MM/DD/YYYY]");
            fields.Add("Notes", "Notes");
            fields.Add("CreatedOn", "Profile Added [MM/DD/YYYY]");
            fields.Add("LastChanged", "Profile Updated [MM/DD/YYYY]");
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(f.GroupID.Value);
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
            {
                fields.Add(groupDataFields.ShortName, groupDataFields.ShortName);
            }
            ViewBag.Fields = fields;
            return PartialView("Partials/Modals/_CreateFilterCondition", fc);
        }

        public ActionResult CreateFilterCondition(ECN_Framework_Entities.Communicator.FilterCondition model)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            if (string.IsNullOrEmpty(model.CompareValue))
            {
                model.CompareValue = "";
            }
            if (string.IsNullOrEmpty(model.DatePart))
            {
                model.DatePart = "";
            }
            ECN_Framework_Entities.Communicator.FilterCondition filterCondition = new ECN_Framework_Entities.Communicator.FilterCondition();
            int filterID = (int)Session["selectedFilterID"];
            filterCondition.FilterGroupID = model.FilterGroupID;
            filterCondition.CustomerID = User.CustomerID;
            filterCondition.CreatedUserID = User.UserID;
            filterCondition.Field = model.Field;
            filterCondition.FieldType = model.FieldType;
            filterCondition.DatePart = model.DatePart;
            filterCondition.NotComparator = model.NotComparator;
            filterCondition.Comparator = model.Comparator;
            filterCondition.CompareValue = model.CompareValue;

            if (filterCondition.SortOrder == -1)
            {
                ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterCondition.FilterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                if (filterGroup.FilterConditionList.Count == 0)
                {
                    filterCondition.SortOrder = 1;
                }
                else
                {
                    var result = (from src in filterGroup.FilterConditionList
                                  group src by src.SortOrder into grp
                                  select new
                                  {
                                      SortOrder = grp.Max(t => t.SortOrder)
                                  }).ToList();

                    filterCondition.SortOrder = result[0].SortOrder + 1;
                }
            }
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.FilterCondition.Save(filterCondition, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            response.Add("200");
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_FilterConditionGrid", ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterCondition.FilterGroupID, User)));
            return Json(response);
        }

        public ActionResult LoadEditFilterCondition(int filterConditionID)
        {
            ECN_Framework_Entities.Communicator.FilterCondition fc =
                ECN_Framework_BusinessLayer.Communicator.FilterCondition.GetByFilterConditionID(filterConditionID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            ECN_Framework_Entities.Communicator.FilterGroup fg =
                ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(fc.FilterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            ECN_Framework_Entities.Communicator.Filter f =
                ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(fg.FilterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            // LoadUserFields
            Dictionary<string, string> fields = new Dictionary<string, string>();
            //List<string> fields = new List<string>();
            fields.Add("EmailAddress", "EmailAddress");
            fields.Add("FormatTypeCode", "FormatTypeCode");
            fields.Add("SubscribeTypeCode", "SubscribeTypeCode");
            fields.Add("Title", "Title");
            fields.Add("FirstName", "FirstName");
            fields.Add("LastName", "LastName");
            fields.Add("FullName", "FullName");
            fields.Add("Company", "Company");
            fields.Add("Occupation", "Occupation");
            fields.Add("Address", "Address");
            fields.Add("Address2", "Address2");
            fields.Add("City", "City");
            fields.Add("State", "State");
            fields.Add("Zip", "Zip");
            fields.Add("Country", "Country");
            fields.Add("Voice", "Voice");
            fields.Add("Mobile", "Mobile");
            fields.Add("Fax", "Fax");
            fields.Add("Website", "Website");
            fields.Add("Age", "Age");
            fields.Add("Income", "Income");
            fields.Add("Gender", "Gender");
            fields.Add("User1", "User1");
            fields.Add("User2", "User2");
            fields.Add("User3", "User3");
            fields.Add("User4", "User4");
            fields.Add("User5", "User5");
            fields.Add("User6", "User6");
            fields.Add("Birthdate", "Birthdate [MM/DD/YYYY]");
            fields.Add("UserEvent1", "UserEvent1");
            fields.Add("UserEvent1Date", "UserEvent1Date [MM/DD/YYYY]");
            fields.Add("UserEvent2", "UserEvent2");
            fields.Add("UserEvent2Date", "UserEvent2Date [MM/DD/YYYY]");
            fields.Add("Notes", "Notes");
            fields.Add("CreatedOn", "Profile Added [MM/DD/YYYY]");
            fields.Add("LastChanged", "Profile Updated [MM/DD/YYYY]");
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(f.GroupID.Value);
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
            {
                fields.Add(groupDataFields.ShortName, groupDataFields.ShortName);
            }
            ViewBag.Fields = fields;
            return PartialView("Partials/Modals/_EditFilterCondition", fc);
        }

        public ActionResult EditFilterCondition(ECN_Framework_Entities.Communicator.FilterCondition model)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.FilterCondition filterCondition =
                ECN_Framework_BusinessLayer.Communicator.FilterCondition.GetByFilterConditionID(model.FilterConditionID, User);
            if (string.IsNullOrEmpty(model.CompareValue))
            {
                model.CompareValue = "";
            }
            if (string.IsNullOrEmpty(model.DatePart))
            {
                model.DatePart = "";
            }
            filterCondition.UpdatedUserID = User.UserID;
            filterCondition.Field = model.Field;
            filterCondition.FieldType = model.FieldType;
            filterCondition.DatePart = model.DatePart;
            filterCondition.NotComparator = model.NotComparator;
            filterCondition.Comparator = model.Comparator;
            filterCondition.CompareValue = model.CompareValue;

            if (filterCondition.SortOrder == -1)
            {
                ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterCondition.FilterGroupID, User);
                if (filterGroup.FilterConditionList.Count == 0)
                {
                    filterCondition.SortOrder = 1;
                }
                else
                {
                    var result = (from src in filterGroup.FilterConditionList
                                  group src by src.SortOrder into grp
                                  select new
                                  {
                                      SortOrder = grp.Max(t => t.SortOrder)
                                  }).ToList();

                    filterCondition.SortOrder = result[0].SortOrder + 1;
                }
            }
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.FilterCondition.Save(filterCondition, User);
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            response.Add("200");
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_FilterConditionGrid", ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterCondition.FilterGroupID, User)));
            return Json(response);
        }

        public ActionResult DeleteFilterCondition(int id)
        {
            ECN_Framework_Entities.Communicator.FilterCondition fc =
                ECN_Framework_BusinessLayer.Communicator.FilterCondition.GetByFilterConditionID(id, ConvenienceMethods.GetCurrentUser());

            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.FilterCondition.Delete(fc.FilterGroupID, fc.FilterConditionID, ConvenienceMethods.GetCurrentUser());
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            response.Add("200");
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_FilterConditionGrid", ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(fc.FilterGroupID, ConvenienceMethods.GetCurrentUser())));
            return Json(response);
        }
        #endregion
    }
}