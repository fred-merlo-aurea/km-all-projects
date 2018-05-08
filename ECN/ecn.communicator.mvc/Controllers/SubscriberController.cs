using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecn.communicator.mvc.Models;
using ecn.communicator.mvc.Infrastructure;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using System.Configuration;
using System.Data;
using System.Xml;
using System.IO;
using System.Collections;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using KMSite;
using System.Text;
using System.Web.UI;
using System.Text.RegularExpressions;
using BusinessEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup;

namespace ecn.communicator.mvc.Controllers
{
    public class SubscriberController : BaseController
    {
        private const string BlastIdColumnName = "BlastID";
        private const string EmailFilterPrefix = " AND (EmailAddress like '%";
        private const string FilterSuffix = "%' ) ";
        private const string FilterSuffixNormal = "' ) ";
        private const string Like = "like";
        private const string EqualsString = "equals";
        private const string Ends = "ends";
        private const string Starts = "starts";
        private const string Xml = ".xml";
        private const string Csv = ".csv";
        private const string Txt = ".txt";
        private const string Xls = ".xls";
        private const string Star = "*";
        private const string SubscribeTypes = " 'S', 'U', 'D', 'P', 'B', 'M','A','F','?','E' ";
        private const string DoubleQuote = "\"";
        private const string DualDoubleQuote = @"""";
        private const string NewLineR = "\r";
        private const string NewLineN = "\n";
        private const string TabChar = "\t";
        private const string Comma = ",";
        private const string TextXml = "text/xml";
        private const string TextCsv = "text/csv";
        private const string ApplicationMSExcel = "application/vnd.ms-excel";
        private const string ContentDisposition = "content-disposition";
        private const string AttachmentFilename = "attachment; filename={0}";

        private KMPlatform.Entity.User CurrentUser
        {
            get { return ConvenienceMethods.GetCurrentUser(); }
        }

        public ActionResult Index(int id, string comparator = "like", string value = "")
        {
            int GroupID = 0;
            if (id > 0)
                GroupID = id;
            else
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });

            //Session["selectedGroupID"] = GroupID;
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();

            GroupWrapper Model = new GroupWrapper();

            if (KM.Platform.User.HasAccess(User, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
            {
                Model.SubscribeTypeCodes = LoadDropDowns(User, GroupID);

                if (KM.Platform.User.HasAccess(User, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.View))
                {
                    //LoadFormData(getGroupID());
                    ECN_Framework_Entities.Communicator.Group dbGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, User);
                    if (dbGroup != null && dbGroup.GroupID > 0)
                    {
                        Models.Group group = new Models.Group(dbGroup);

                        //IEnumerable<ECN_Framework_Entities.Communicator.Email> emails = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(GroupID, User);
                        group.Emails = new List<ecn.communicator.mvc.Models.Email>();
                        //foreach (ECN_Framework_Entities.Communicator.Email email in emails)
                        //{
                        //    group.Emails.Add(new Models.Email(email, GroupID));
                        //}
                        string emailAdd = ECN_Framework_Common.Functions.StringFunctions.CleanString(value);
                        emailAdd = emailAdd.Replace("_", "[_]").Replace("'", "''");
                        string filter = "";
                        switch (comparator)
                        {
                            case "like":
                                filter = " AND EmailAddress like '%" + emailAdd + "%' ";
                                break;
                            case "starts":
                                filter = " AND EmailAddress like '" + emailAdd + "%' ";
                                break;
                            case "ends":
                                filter = " AND EmailAddress like '%" + emailAdd + "' ";
                                break;
                            case "equals":
                                filter = " AND EmailAddress = '" + emailAdd + "' ";
                                break;

                        }
                        DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, GroupID, 1, 15, new DateTime(), new DateTime(), false, filter, "EmailAddress", "ASC");

                        //DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, GroupID, 1, 20000, filter);
                        DataTable emailstable = emailsListDS.Tables[1];
                        int totalCount = Convert.ToInt32(emailsListDS.Tables[0].Rows[0][0].ToString());
                        Model.Comparator = comparator;
                        Model.SearchValue = value;
                        group.Emails = emailstable.DataTableToList<ecn.communicator.mvc.Models.Email>();
                        //Model.FolderList = LoadFolders(User);
                        Model.group = group;
                        ViewBag.xsdDownloadLbl = "<img src='" + System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/images/note.gif'>&nbsp;<font face='verdana' color='orange' size=1>For Export as XML,&nbsp;<a href='" + System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/emails.xsd' target='_blank'>Click here</a> to Download XML Schema</font>";
                        return View(Model);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Group");
                    }
                }
                else
                {
                    return RedirectToAction("SecurityAccess", "Error");
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SubLogReadToGrid([DataSourceRequest]DataSourceRequest request, int emailID, int groupID, int pageNumber, int pageSize)
        {

            int EmailID = 0;
            if (emailID > 0)
                EmailID = emailID;
            else
                return RedirectToAction("Index", "Group");

            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            int GroupID = 0;
            if (groupID < 0)
                return RedirectToAction("Index", "Group");
            else
                GroupID = groupID;

            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID(EmailID, GroupID, User);
            if (email != null && email.EmailID > 0)
            {
                List<ECN_Framework_Entities.Activity.View.ActivitylogSearch> logSearches = new List<ECN_Framework_Entities.Activity.View.ActivitylogSearch>();
                DataTable logs = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.ActivityLogs(CurrentUser, email.EmailID, pageNumber, pageSize);
                logSearches = ConvertToClass(logs);
                IQueryable<ECN_Framework_Entities.Activity.View.ActivitylogSearch> gs = logSearches.AsQueryable();
                DataSourceResult result = gs.ToDataSourceResult(request);
                return Json(result);
            }
            else
            {
                return RedirectToAction("Index", "Group");
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EmailsSubsReadToGrid([DataSourceRequest]DataSourceRequest request, int GroupID, string SubscribeTypeCodes, string SearchCriterion, string ProfileName, string FromDate, string ToDate, bool WithActivity, int PageNumber, int PageSize)
        {
            if (string.IsNullOrEmpty(ProfileName))
                ProfileName = "";
            string emailAdd = ECN_Framework_Common.Functions.StringFunctions.CleanString(ProfileName);
            emailAdd = emailAdd.Replace("_", "[_]").Replace("'", "''");
            string searchEmailLike = SearchCriterion;
            string filter = "";

            string sortColumn = "EmailAddress"; // lstgs[0].SortColumnName;
            string sortdirection = "ASC";
            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortdirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

            }

            if (emailAdd.Length > 0)
            {
                if (searchEmailLike.Equals("like"))
                {
                    filter = " AND EmailAddress like '%" + emailAdd + "%'";
                }
                else if (searchEmailLike.Equals("equals"))
                {
                    filter = " AND EmailAddress like '" + emailAdd + "'";
                }
                else if (searchEmailLike.Equals("ends"))
                {
                    filter = " AND EmailAddress like '%" + emailAdd + "'";
                }
                else if (searchEmailLike.Equals("starts"))
                {
                    filter = " AND EmailAddress like '" + emailAdd + "%'";
                }
                else
                {
                    filter = " AND EmailAddress like '%" + emailAdd + "%'";
                }
            }

            string subscribeType = SubscribeTypeCodes;
            if (!(subscribeType.Equals("*")))
            {
                filter += " AND SubscribeTypeCode = '" + subscribeType + "'";
            }

            try
            {
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                DateTime.TryParse(FromDate, out fromDate);
                DateTime.TryParse(ToDate, out toDate);
                DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, GroupID, PageNumber, PageSize, fromDate, toDate, WithActivity, filter, sortColumn, sortdirection);

                //DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, GroupID, 1, 20000, filter);
                DataTable emailstable = emailsListDS.Tables[1];
                int totalCount = Convert.ToInt32(emailsListDS.Tables[0].Rows[0][0].ToString());
                List<Models.Email> list = new List<Models.Email>();

                list = emailstable.DataTableToList<Models.Email>();

                IQueryable<Models.Email> gs = list.AsQueryable();
                DataSourceResult result = gs.ToDataSourceResult(request);
                result.Total = totalCount;
                return Json(result);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ecn.communicator.mvc.Controllers.GroupController.EmailsSubsReadToGrid", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                return PartialView("Partials/_ErrorNotification");
            }
        }

        public ActionResult Edit(int id, int groupID)
        {
            int EmailID = 0;
            if (id > 0)
                EmailID = id;
            else
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });

            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            int GroupID = 0;
            if (groupID < 0)
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
            else
                GroupID = groupID;

            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID(EmailID, GroupID, User);
            if (email != null && email.EmailID > 0)
            {
                EmailWrapper emailWrapperModel = new EmailWrapper(email, GroupID);

                if (KMPlatform.BusinessLogic.Client.HasServiceFeature(User.CurrentClient.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email))
                {
                    if (KM.Platform.User.HasAccess(User, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit))
                    {
                        emailWrapperModel.SubscribeTypeCodes = LoadDropDowns(User, GroupID);
                        emailWrapperModel.SubscribeTypeCodes.RemoveAll(item => item.Item2 == "*");

                        if (EmailID > 0)
                        {
                            LoadLog(emailWrapperModel, User, EmailID);
                        }

                        emailWrapperModel.UDFURL = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailProfileManager.aspx?action=edit&ead=" + email.EmailAddress + "&eid=" + EmailID + "&gid=" + GroupID + "&cid=" + User.CustomerID;
                        //ProfileManagerButton.Attributes.Add("Onclick", "return popManagerWindow('" + profileURL + "');");
                    }

                    else
                    {
                        throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                    }
                }

                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
                }
                emailWrapperModel.Errors = this.GetTempData("ECNErrors") as List<ECNError>;
                return View(emailWrapperModel);
            }
            else
            {
                return RedirectToAction("Index", "Group");
            }
        }

        [HttpPost]
        public ActionResult Update(ecn.communicator.mvc.Models.Email externalEmail)
        {
            KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.Email internalEmail = null;
            List<string> response = new List<string>();
            try
            {
                internalEmail = externalEmail.ToEmail_Internal(user);

                if (ECN_Framework_BusinessLayer.Communicator.Email.Exists(externalEmail.EmailAddress, user.CustomerID, externalEmail.EmailID))
                {
                    Email email1 = externalEmail;
                    Email email2 = new Email(ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(externalEmail.EmailAddress, user.CustomerID), externalEmail.CurrentGroupID);
                    //Session["email1"] = email1;
                    //Session["email2"] = email2;
                    ////ECN_Framework_BusinessLayer.Communicator.Email.Save(user, internalEmail, externalEmail.CurrentGroupID, "ecn.communicator.mvc.controllers.GroupController.UpdateEmail");
                    //return RedirectToAction("MergeEmails", new { emailid1 = email1.EmailID, emailid2 = email2.EmailID, groupid = externalEmail.CurrentGroupID });
                    ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
                    ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.EmailGroup;
                    response.Add("300");
                    response.Add(email1.EmailID.ToString());
                    response.Add(email2.EmailID.ToString());
                    //errorList.Add(new ECNError(Entity, Method, "This email address already exists for this Customer Account. Would you like to merge these two profiles? <a href=\"../MergeEmails?oldemailid=" + email1.EmailID.ToString() + "&newemailid=" + email2.EmailID.ToString() + "\">MERGE</a>"));
                    //throw new ECNException(errorList);
                    return Json(response);
                }
                string isEmailSuppressedString = isEmailSuppressed(externalEmail.EmailAddress.Trim(), user);
                if (!string.IsNullOrEmpty(isEmailSuppressedString))
                {
                    response.Add("500");
                    response.Add(isEmailSuppressedString);
                    return Json(response);
                }

                ECN_Framework_BusinessLayer.Communicator.Email.Save(user, internalEmail, externalEmail.CurrentGroupID, "ecn.communicator.mvc.controllers.GroupController.UpdateEmail");
                response.Add("200");
            }
            catch (ECNException ex)
            {
                //this.SetTempData("ECNErrors", ex.ErrorList);
                //return RedirectToAction("Edit", "Subscriber", new { id = externalEmail.EmailID });
                response.Add("500");
                StringBuilder sbError = new StringBuilder();
                foreach (ECNError r in ex.ErrorList)
                {
                    sbError.Append(r.ErrorMessage + "<br />");
                }
                response.Add(sbError.ToString());
            }
            catch (Exception e)
            {
                response.Add("500");
                response.Add("An error occurred");
            }
            return Json(response);
        }

        [HttpGet]
        public ActionResult MergeEmails(int oldemailid, int newemailid, int groupID)
        {
            KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();

            ECN_Framework_Entities.Communicator.Email externalEmail1 = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(oldemailid, user); // Session["email1"] as Email;
            ECN_Framework_Entities.Communicator.Email externalEmail2 = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(newemailid, user); // Session["email2"] as Email;
            Session["email1"] = externalEmail1;
            Session["email2"] = externalEmail2;
            if (externalEmail1 == null || externalEmail2 == null)
                throw new ECN_Framework_Common.Objects.SecurityException("Attempting to merge non-conflicting emails");
            Tuple<ECN_Framework_Entities.Communicator.Email, ECN_Framework_Entities.Communicator.Email> Model = new Tuple<ECN_Framework_Entities.Communicator.Email, ECN_Framework_Entities.Communicator.Email>(externalEmail1, externalEmail2);
            ViewBag.GroupID = groupID;
            return View(Model);
        }

        [HttpPost]
        public ActionResult MergeEmailsProcess(int id, int remove, int groupID)
        {
            List<string> response = new List<string>();


            try
            {
                KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();
                ECN_Framework_Entities.Communicator.Email email1 = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(id, user); // Session["email1"] as Email;
                ECN_Framework_Entities.Communicator.Email email2 = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(remove, user); // Session["email1"] as Email;
                if (email1 == null || email2 == null)
                {
                    response.Add("500");
                    response.Add("Attempted email merge with no recorded conflict");
                    return Json(response);
                    //throw new SecurityException("Attempted email merge with no recorded conflict");
                }

                // Can't merge and make changes simultaneously
                if (email1.EmailID == id)
                {
                    ECN_Framework_BusinessLayer.Communicator.Email.MergeProfiles(email2.EmailID, id, user);
                }
                else if (email2.EmailID == id)
                {
                    ECN_Framework_BusinessLayer.Communicator.Email.MergeProfiles(email1.EmailID, id, user);
                }
                else
                {
                    response.Add("500");
                    response.Add("EmailID does not match the conflicting ones");
                    return Json(response);
                    //throw new ArgumentException("EmailID does not match the conflicting ones");
                }
            }
            catch (ECNException ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ecn.communicator.mvc.Controllers.GroupController.MergeProfiles(int)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
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
            response.Add(new UrlHelper(Request.RequestContext).Action("Index", "Subscriber", new { id = groupID }));
            return Json(response);
        }

        public ActionResult EditUDF(int id, int groupID)
        {
            int EmailID = 0;
            if (id > 0)
                EmailID = id;
            else
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.Edit))
            {
                Session["selectedEmailID"] = EmailID;
                KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
                int GroupID = 0;
                if (groupID < 0)
                    return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
                else
                    GroupID = groupID;

                List<DataRow> dt = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetStandaloneUDFDataValues(GroupID, EmailID, User).AsEnumerable().ToList();
                EmailUDFData model = new EmailUDFData();
                model.EmailID = EmailID;
                model.GroupID = GroupID;
                ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(EmailID, User);
                if (email.CustomerID != ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID)
                {
                    return RedirectToAction("SecurityAccess", "Error");
                }
                model.Email = email.EmailAddress;

                model.datafieldSets = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupID(GroupID);
                foreach (var data in dt)
                {
                    model.eudfdv.Add(new EmailUDFDataValue { GroupDataFieldsID = (int)data.ItemArray[0], UDFName = data.ItemArray[1].ToString(), Data = data.ItemArray[2].ToString() });
                }
                return View(model);
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public ActionResult LoadUDFHistoryData(int DataFieldSetID, int emailID, int groupID)
        {
            List<string> response = new List<string>();
            int EmailID = 0;
            if (emailID < 0)
            {
                response.Add("500");
                response.Add("Session timedout");
                return Json(response);
            }
            else
                EmailID = emailID;

            int GroupID = 0;
            if (groupID < 0)
            {
                response.Add("500");
                response.Add("Session timedout");
                return Json(response);
            }
            else
                GroupID = groupID;

            DataTable historyData = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetTransUDFDataValues(CurrentUser.CustomerID, GroupID, EmailID.ToString(), DataFieldSetID, CurrentUser);
            historyData.DefaultView.Sort = "LastModifiedDate DESC";
            historyData.Columns.Remove("EmailID");
            if (historyData.Columns.Count > 0)
            {
                historyData.Columns.Add("FixedSortDate", typeof(DateTime));
                foreach (DataRow row in historyData.Rows)
                {
                    string date = row["LastModifiedDate"].ToString();
                    if (!string.IsNullOrWhiteSpace(date))
                    {
                        row["FixedSortDate"] = Convert.ToDateTime(date);
                    }
                }

                historyData.DefaultView.Sort = "FixedSortDate DESC";
                historyData.Columns.Remove("FixedSortDate");

                response.Add("200");
                response.Add(ConvertDataTableToHTML(historyData.DefaultView.ToTable()));
                return Json(response);
            }
            else
            {
                {
                    response.Add("500");
                    response.Add("No Transactional Data");
                    return Json(response);
                }
            }
        }
        protected List<ECN_Framework_Entities.Activity.View.ActivitylogSearch> ConvertToClass(DataTable logs)
        {
            List<ECN_Framework_Entities.Activity.View.ActivitylogSearch> logSearches = new List<ECN_Framework_Entities.Activity.View.ActivitylogSearch>();
            foreach (DataRow log in logs.Rows)
            {
                var blastId = 0;
                var blastIdString = Convert.ToString(log[BlastIdColumnName]);

                ECN_Framework_Entities.Activity.View.ActivitylogSearch ls = new ECN_Framework_Entities.Activity.View.ActivitylogSearch();
                ls.TotalRowsCount = Convert.ToInt32(log["TotalCount"]);
                ls.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(log["EmailSubject"].ToString());
                
                if (int.TryParse(blastIdString, out blastId) && blastId > 0)
                {
                    ls.BlastID = blastIdString;
                }
                if (log["ActionTypeCode"].ToString().ToLower().Equals("subscribe"))
                    ls.ActionTypeCode = "unsubscribe";
                else
                    ls.ActionTypeCode = log["ActionTypeCode"].ToString();
                ls.ActionValue = log["ActionValue"].ToString();
                if (!string.IsNullOrWhiteSpace(log["ActionDate"].ToString()))
                {
                    try
                    {
                        ls.ActionDate = DateTime.Parse(log["ActionDate"].ToString());
                    }
                    catch { }
                }

                logSearches.Add(ls);
            }
            return logSearches;
        }
        private static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td style=\"font-weight: bold; font-size: 14px;\">" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        public ActionResult DeleteSubscriber(int Id, int groupID)
        {
            List<string> response = new List<string>();
            int GroupID = 0;
            if (groupID < 0)
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
            else
                GroupID = groupID;
            try
            {
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.Delete(GroupID, Id, ConvenienceMethods.GetCurrentUser());
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
            //response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_SubscribersGrid", GroupID));
            return Json(response);
        }

        public ActionResult LoadAddUDFData(int groupID, int emailID)
        {
            int GroupID = 0;
            if (groupID < 0)
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
            else
                GroupID = groupID;
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.Edit))
            {
                Models.EditUDFData model = new Models.EditUDFData();
                model.GroupID = groupID;
                model.GroupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, ConvenienceMethods.GetCurrentUser()).Where(x => !x.DatafieldSetID.HasValue).ToList();
                model.EmailID = emailID;

                return PartialView("Partials/Modals/_AddUDFData", model);
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public ActionResult LoadEditUDFData(int groupDataFieldsID, int emailID, int groupID)
        {
            int GroupID = 0;
            if (groupID < 0)
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
            else
                GroupID = groupID;
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.Edit))
            {
                List<DataRow> dt = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetStandaloneUDFDataValues(GroupID, emailID, ConvenienceMethods.GetCurrentUser()).AsEnumerable().ToList();
                EmailUDFDataValue model = new EmailUDFDataValue();
                foreach (var data in dt)
                {
                    if ((int)data.ItemArray[0] == groupDataFieldsID)
                    {
                        model.GroupDataFieldsID = groupDataFieldsID;
                        model.UDFName = data.ItemArray[1].ToString();
                        model.Data = data.ItemArray[2].ToString();
                        model.GroupID = GroupID;
                        model.EmailID = emailID;
                    }
                }
                return PartialView("Partials/Modals/_EditUDFData", model);
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public ActionResult AddEditUDFData(int groupDataFieldsID, string value, int emailID, int groupID)
        {
            int GroupID = 0;
            if (groupID < 0)
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
            else
                GroupID = groupID;
            List<string> response = new List<string>();
            try
            {
                updateUDFValue(groupDataFieldsID, value, groupID, emailID);
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
            List<DataRow> dt = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetStandaloneUDFDataValues(GroupID, emailID, ConvenienceMethods.GetCurrentUser()).AsEnumerable().ToList();
            List<EmailUDFDataValue> list = new List<EmailUDFDataValue>();
            foreach (var data in dt)
            {
                list.Add(new EmailUDFDataValue { GroupDataFieldsID = (int)data.ItemArray[0], UDFName = data.ItemArray[1].ToString(), Data = data.ItemArray[2].ToString() });
            }
            response.Add("200");
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_UDFsDataGrid", list));
            return Json(response);
        }

        private List<Tuple<string, string>> LoadDropDowns(KMPlatform.Entity.User user, int GroupID)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, user);
            List<Tuple<string, string>> SubscribeTypeCodes = new List<Tuple<string, string>>();
            if (group != null)
            {
                if (group.MasterSupression == null || group.MasterSupression.Value == 0)
                {
                    SubscribeTypeCodes.Clear();
                    SubscribeTypeCodes.Add(new Tuple<string, string>("All Types", "*"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Subscribes", "S"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("UnSubscribes", "U"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Master Suppressed", "M"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Marked as Bad Records", "D"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Pending Subscribes", "P"));
                }
                else
                {
                    SubscribeTypeCodes.Clear();
                    SubscribeTypeCodes.Add(new Tuple<string, string>("All Types", "*"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("UnSubscribes", "U"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Bounce", "B"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Abuse Complaint", "A"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Manual Upload", "M"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Feedback Loop(or Spam Complaint)", "F"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Email Address Change", "E"));
                    SubscribeTypeCodes.Add(new Tuple<string, string>("Unknown User", "?"));
                }
            }
            return SubscribeTypeCodes;
        }

        private void LoadLog(ecn.communicator.mvc.Models.EmailWrapper wrapper, KMPlatform.Entity.User user, int EmailID)
        {
            List<ECN_Framework_Entities.Activity.View.BlastActivity> blastActivity = new List<ECN_Framework_Entities.Activity.View.BlastActivity>();
            if (EmailID > 0)
            {
                wrapper.BlastActivity = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetByEmailID(EmailID, user);
            }
        }

        private void updateUDFValue(int groupDataFieldsID, string value, int groupID, int emailID)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(emailID, User);
            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailIDGroupID(email.EmailID, groupID, User);
            StringBuilder xmlUDF = new StringBuilder("");
            xmlUDF.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><row><ea>" + email.EmailAddress + "</ea><udf id=\"" + groupDataFieldsID.ToString() + "\"><v><![CDATA[" + value + "]]></v></udf></row></XML>");
            StringBuilder xmlProfile = new StringBuilder("");
            xmlProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails>");
            xmlProfile.Append("<emailaddress>" + email.EmailAddress + "</emailaddress>");
            xmlProfile.Append("</Emails></XML>");
            ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(User, User.CustomerID, groupID, xmlProfile.ToString(), xmlUDF.ToString(), emailGroup.FormatTypeCode, emailGroup.SubscribeTypeCode, false, "", "ecn.communicator.mvc.Controllers.SubscriberController.updateUDFValue");
        }

        private string isEmailSuppressed(string email, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(user.CustomerID, user);
            ECN_Framework_Entities.Communicator.EmailGroup msEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(email, MSGroup.GroupID, user);
            if (msEmailGroup != null && msEmailGroup.CustomerID.HasValue)
                return "New email address is Master Suppressed. Updating is not allowed.";

            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, email.Replace("'", "''"), user);
            if (channelMasterSuppressionList_List.Count > 0)
                return "New email address is Channel Master Suppressed. Updating is not allowed.";

            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(email.Replace("'", "''"), user);
            if (globalMasterSuppressionList_List.Count > 0)
                return "New email address is Global Master Suppressed. Updating is not allowed.";

            return string.Empty;
        }

        #region Dates Validation
        public ActionResult ValidateDates(string DateFromFilter, string DateToFilter, bool WithActivity)
        {
            List<string> response = new List<string>();

            if (WithActivity && (string.IsNullOrWhiteSpace(DateFromFilter) || string.IsNullOrWhiteSpace(DateToFilter)))
            {
                response.Add("500");
                response.Add("Dates are required fields");
                return Json(response);
            }

            DateTime startDateOut;
            DateTime.TryParse(DateFromFilter, out startDateOut);
            DateTime endDateOut;
            DateTime.TryParse(DateToFilter, out endDateOut);

            if (!string.IsNullOrWhiteSpace(DateFromFilter) && (startDateOut == DateTime.MinValue || !IsValidDate(DateFromFilter)))
            {
                response.Add("500");
                response.Add("Start Date is invalid");
                return Json(response);
            }
            else if (!string.IsNullOrWhiteSpace(DateToFilter) && (endDateOut == DateTime.MinValue || !IsValidDate(DateToFilter)))
            {
                response.Add("500");
                response.Add("End Date is invalid");
                return Json(response);
            }
            else if (!string.IsNullOrWhiteSpace(DateFromFilter) && startDateOut < DateTime.Now.AddYears(-2))
            {
                response.Add("500");
                response.Add("Start Date is outside of the archive window of " + 2 + " year(s)");
                return Json(response);
            }
            else if (!string.IsNullOrWhiteSpace(DateFromFilter) && !string.IsNullOrWhiteSpace(DateToFilter) && startDateOut > endDateOut)
            {
                response.Add("500");
                response.Add("Start Date can not come after the End Date");
                return Json(response);
            }
            else
            {
                response.Add("200");
                return Json(response);
            }
        }

        private bool IsValidDate(string date)
        {
            string[] dateParts = date.Split('/');
            //if ((dateParts[2].Count() == 2 || dateParts[2].Count() == 4) && (IsRecentYear(dateParts[2])))
            if (dateParts[2].Count() == 2 || dateParts[2].Count() == 4)
            {
                bool tmp = Regex.IsMatch(date, @"(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>(?:\d{4}|\d{2}))");
                return tmp;
            }
            return false;
        }
        #endregion

        #region Export
        public void ExportData(
            string groupId, 
            string subscribeType, 
            string emailAddress, 
            string searchType, 
            string downloadType, 
            string profFilter, 
            string strFromDate, 
            string strToDate,
            bool withActivity)
        {
            var customerId = CurrentUser.CustomerID.ToString();
            var columnHeadings = new ArrayList();
            var delimiter = string.Empty;
            var filter = string.Empty;

            delimiter = GetDelimitor(downloadType);
            subscribeType = GetSubscribeType(subscribeType);
            filter = GetFilter(emailAddress, searchType);

            var fromDate = new DateTime();
            var toDate = new DateTime();
            DateTime.TryParse(strFromDate, out fromDate);
            DateTime.TryParse(strToDate, out toDate);

            int iGroupId, iCustomerId;
            int.TryParse(groupId, out iGroupId);
            int.TryParse(customerId, out iCustomerId);

            var emailProfilesDataTable = BusinessEmailGroup.GetGroupEmailProfilesWithUDF(iGroupId, iCustomerId, subscribeType, fromDate, 
                                            toDate, withActivity, filter, profFilter);

            var osFilePath = string.Format("{0}/customers/{1}/downloads/", ConfigurationManager.AppSettings["Images_VirtualPath"], customerId);
            var fileName = string.Format("{0}-{1}emails{2}", customerId, groupId, downloadType);
            var outfileName = osFilePath + fileName;

            if (!Directory.Exists(osFilePath))
            {
                Directory.CreateDirectory(osFilePath);
            }

            if (System.IO.File.Exists(outfileName))
            {
                System.IO.File.Delete(outfileName);
            }

            WriteTextFile(downloadType, outfileName, columnHeadings, emailProfilesDataTable, delimiter);

            if (downloadType == Xls)
            {
                Response.ContentType = ApplicationMSExcel;
            }
            else if (downloadType == Csv)
            {
                Response.ContentType = TextCsv;
            }
            else if (downloadType == Txt)
            {
                Response.ContentType = TextCsv;
            }
            else if (downloadType == Xml)
            {
                Response.ContentType = TextXml;
            }

            Response.AddHeader(ContentDisposition, string.Format(AttachmentFilename, fileName));
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }

        private string GetDelimitor(string downloadType)
        {
            var delimiter = string.Empty;
            if (downloadType.Equals(Xls) || downloadType.Equals(Txt))
            {
                delimiter = TabChar;
            }
            else
            {
                delimiter = Comma;
            }

            return delimiter;
        }

        private string GetSubscribeType(string subscribeType)
        {
            if (subscribeType.Equals(Star))
            {
                subscribeType = SubscribeTypes;
            }
            else
            {
                subscribeType = string.Format(" '{0}' ", subscribeType);
            }

            return subscribeType;
        }

        private void WriteTextFile(string downloadType, string outfileName, ArrayList columnHeadings, DataTable emailProfilesDataTable, string delimiter)
        {
            var txtfile = System.IO.File.AppendText(outfileName);

            if (downloadType.Equals(Xls) || downloadType.Equals(Csv) || downloadType.Equals(Txt))
            {
                var newline = new StringBuilder();
                var quotes = downloadType.Equals(Csv) ? DoubleQuote : string.Empty;
                //var newLineContent = string.Empty;

                columnHeadings = GetDataTableColumns(emailProfilesDataTable);
                var aListEnum = columnHeadings.GetEnumerator();
                while (aListEnum.MoveNext())
                {
                    newline.Append(quotes)
                        .Append(aListEnum.Current.ToString())
                        .Append(quotes)
                        .Append(delimiter);
                }
                txtfile.WriteLine(newline.ToString());

                var columnText = string.Empty;
                foreach (DataRow dataRow in emailProfilesDataTable.Rows)
                {
                    newline = new StringBuilder();
                    aListEnum.Reset();
                    while (aListEnum.MoveNext())
                    {
                        columnText = string.Empty;
                        columnText = dataRow[aListEnum.Current.ToString()].ToString().Replace(NewLineN, string.Empty);
                        columnText = columnText.Replace(NewLineR, string.Empty);
                        columnText = columnText.Replace(DualDoubleQuote, string.Empty);
                        newline.Append(quotes)
                            .Append(columnText)
                            .Append(quotes)
                            .Append(delimiter);
                    }
                    txtfile.WriteLine(newline.ToString());
                }
            }
            else if (downloadType.Equals(Xml))
            {
                var emailProfilesDS = new DataSet();
                emailProfilesDS.Tables.Add(emailProfilesDataTable);
                emailProfilesDS.WriteXml(txtfile);
            }
            txtfile.Close();
        }

        private string GetFilter(string emailAddr, string searchType)
        {
            var filter = string.Empty;

            if (emailAddr.Length > 0)
            {
                switch (searchType)
                {
                    case Like:
                        filter = EmailFilterPrefix + emailAddr + FilterSuffix;
                        break;
                    case EqualsString:
                        filter = EmailFilterPrefix + emailAddr + FilterSuffixNormal;
                        break;
                    case Ends:
                        filter = EmailFilterPrefix + emailAddr + FilterSuffixNormal;
                        break;
                    case Starts:
                        filter = EmailFilterPrefix + emailAddr + FilterSuffix;
                        break;
                    default:
                        filter = EmailFilterPrefix + emailAddr + FilterSuffix;
                        break;
                }
            }
            else
            {
                filter = string.Empty;
            }

            return filter;
        }

        public ArrayList GetDataTableColumns(DataTable dataTable)
        {
            int nColumns = dataTable.Columns.Count;
            ArrayList columnHeadings = new ArrayList();
            DataColumn dataColumn = null;
            for (int i = 0; i < nColumns; i++)
            {
                dataColumn = dataTable.Columns[i];
                columnHeadings.Add(dataColumn.ColumnName.ToString());
            }
            return columnHeadings;
        }
        #endregion
    }
}