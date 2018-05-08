using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using ecn.communicator.mvc.Models;
using ecn.communicator.mvc.Infrastructure;
using Ecn.Communicator.Mvc.Interfaces;
using Ecn.Communicator.Mvc.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Accounts;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using KM.Common;
using KM.Common.UserControls;
using Microsoft.Reporting.WebForms;
using KMSite;
using KMPlatform.Entity;
using CommChannelNoThresholdList = ECN_Framework_Entities.Communicator.ChannelNoThresholdList;
using DataColumn = System.Data.DataColumn;
using DataRow = System.Data.DataRow;
using Email = ecn.communicator.mvc.Models.Email;
using Enums = ECN_Framework_Common.Objects.Enums;
using Group = ecn.communicator.mvc.Models.Group;
using SystemFile = System.IO.File;
using static ECN_Framework.WebPageHelper;

namespace ecn.communicator.mvc.Controllers
{
    public class GroupController : BaseController
    {
        private const string LikeSearchSyntax = " AND (EmailAddress like '%{0}%' ) ";
        private const string EqualsSearchSyntax = " AND (EmailAddress like '{0}' ) ";
        private const string EndsWithSearchSyntax = " AND (EmailAddress like '%{0}' ) ";
        private const string StartsWithSearchSyntax = " AND (EmailAddress like '{0}%' ) ";
        private const string EqualsSearchCriteria = "equals";
        private const string EndsWithSearchCriteria = "ends";
        private const string StartsWithSearchCriteria = "starts";
        private const string ExcelFileExtension = ".xls";
        private const string TextFileExtension = ".txt";
        private const string CsvFileExtension = ".csv";
        private const string XmlFileExtensions = ".xml";
        private const string CsvFileContentType = "text/csv";
        private const string XmlFileContentType = "text/xml";
        private const string ExcelFileContentType = "application/vnd.ms-excel";
        private const string DefaultSubscribeType = " 'S', 'U', 'D', 'P', 'B', 'M' ";
        private const string ImagesVirtualPathConfigurationKey = "Images_VirtualPath";
        private const string DefaultCustomersDownloadsPaths = "{0}/customers/{1}/downloads/";
        private const string TabChar = "\t";
        private const string NewLineChar = "\n";
        private const string CarriageReturnChar = "\r";
        private const string CommaChar = ",";
        private const string WildcardChar = "*";
        private const string DoubleQuoteChar = @"""";
        private const string ContentDispositionHeaderName = "content-disposition";
        private const string AttachmentHeaderName = "attachment; filename=";
        private const string SubscribeTypeTemplate = " '{0}' ";
        private const string CsvWrapperTemplate = "\"{0}\"";
        private const string Placeholder = "{0}";
        private const string NotValidNumberErrorMessage = "Is not a valid number.";
        private static string NoThresholdEmailCSVFileName = "_NoThreshold_Emails.CSV";
        private static string MasterSuppressedEmailCSVFileName = "_MasterSuppressed_Emails.CSV";

        private const string DefaultXmlHeader = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>";
        private const string ImportEmailsToGlobalMs = "ImportEmailsToGlobalMS";
        private const string ImportEmailsToNoThreshold = "ImportEmailsToNoThreshold";
        private const string SuppressionThresholdGridView = "Partials/_Suppresion_Threshold_Grid";
        private const string SuppressionGlobalGridView = "Partials/_Suppresion_Global_Grid";

        private const string DefaultSuccessCode = "200";
        private const string DefaultSortOrder = "sortorder asc";
        private const string DescriptionKey = "Description";
        private const string SortOrderKey = "sortOrder";
        private const string TotalsKey = "Totals";

        private const string DuplicateDescription = "Duplicate(s)";
        private const string ChangedDescription = "Changed";
        private const string NewDescription = "New";
        private const string SkippedDescription = "Skipped";
        private const string TotalRecordsDescription = "Total Records in the File";

        private const string ChangedCode = "U";
        private const string DuplicateCode = "D";
        private const string NewCode = "I";
        private const string SkippedCode = "S";
        private const string TotalRecordsCode = "T";

        private User _currentUser;
        private User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = ConvenienceMethods.GetCurrentUser();
                }
                return _currentUser;
            }
        }

        private BaseChannel _currentBaseChannel;
        private BaseChannel CurrentBaseChannel
        {
            get
            {
                if (_currentBaseChannel == null)
                {
                    _currentBaseChannel = ECNSession.CurrentSession().CurrentBaseChannel;
                }
                return _currentBaseChannel;
            }
        }

        private IFileSystem _fileSystem;
        private HttpServerUtilityBase _server;
        private IChannelMasterSuppressionList _channelMasterSuppressionList;
        private IChannelNoThresholdList _channelNoThresholdList;
        private HttpResponseBase _httpResponse;
        public GroupController()
        {
            _fileSystem = new FileSystemAdapter();
            _channelNoThresholdList = new ChannelNoThresholdListAdapter();
            _httpResponse = new HttpResponseAdapter(Response);
            _server = new ServerAdapter(Server);
            _channelMasterSuppressionList = new ChannelMasterSuppressionListAdapter();
        }

        public GroupController(
            IFileSystem fileSystem,
            HttpServerUtilityBase server,
            IChannelNoThresholdList channelNoThresholdList,
            HttpResponseBase httpResponse,
            BaseChannel baseChannel,
            User user,
            IChannelMasterSuppressionList channelMasterSuppressionList)
        {
            _fileSystem = fileSystem;
            _channelNoThresholdList = channelNoThresholdList;
            _httpResponse = httpResponse;
            _currentBaseChannel = baseChannel;
            _server = server;
            _currentUser = user;
            _channelMasterSuppressionList = channelMasterSuppressionList;
        }

        #region Group Manager
        public ActionResult Index()
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

            List<ECN_Framework_Entities.Communicator.Folder> folderList =
                ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), CurrentUser);

            var sortedFolderList = (from src in folderList
                                    orderby src.FolderName
                                    select src).ToList();
            IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> folderTree = sortedFolderList.ToKendoTree();
            FolderExplorer Model = new FolderExplorer(folderTree);
            Model.GroupsGrid = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName("", "like", CurrentUser, 0, 1, 10, false, "active", "GroupName", "ASC").DataTableToListGroups();//.GetSubscribers(0, CurrentUser, 1, 10000, false, "active").DataTableToListGroups();
            var Errors = this.GetTempData("ECNErrors") as List<ECNError>;
            if (Errors != null)
            {
                Model.Errors = Errors;
            }
            return View(Model);
        }

        public ActionResult Edit(int id)
        {
            int GroupID = id;
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
            {
                if (GroupID > 0)
                {
                    try
                    {
                        ECN_Framework_Entities.Communicator.Group dbGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, CurrentUser);

                        if (dbGroup != null && dbGroup.GroupID > 0)
                        {
                            Models.Group group = new Group(dbGroup);
                            Models.GroupWrapper Model = new GroupWrapper(group);
                            Model.FolderList = LoadFolders(CurrentUser);
                            List<ECN_Framework_Entities.Communicator.Folder> folderList =
                                ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                            var sortedFolderList = (from src in folderList
                                                    orderby src.FolderName
                                                    select src).ToList();

                            Model.KendoFolders = sortedFolderList.ToKendoTree();
                            Model.Errors = this.GetTempData("ECNErrors") as List<ECNError>;
                            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SeedList))
                            {
                                Model.SeedListAllowed = true;
                            }
                            else
                            {
                                Model.SeedListAllowed = false;
                            }
                            return View(Model);

                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    catch (ECN_Framework_Common.Objects.SecurityException se)
                    {
                        return RedirectToAction("SecurityAccess", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public ActionResult Emails(int id)
        {
            ecn.communicator.mvc.Models.GroupWrapper Model = new GroupWrapper();
            int GroupID = id;

            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
            {
                if (GroupID > 0)
                {
                    LoadDropDowns(Model, CurrentUser, GroupID);

                    if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.View))
                    {
                        return RedirectToAction("Index", "Group"); // Security Error
                    }
                    else
                    {
                        //LoadFormData(getGroupID());
                        Models.Group group = new Group(ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, CurrentUser));
                        IEnumerable<ECN_Framework_Entities.Communicator.Email> emails = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(GroupID, CurrentUser);
                        group.Emails = new List<ecn.communicator.mvc.Models.Email>();
                        foreach (ECN_Framework_Entities.Communicator.Email email in emails)
                        {
                            group.Emails.Add(new Models.Email(email, GroupID));
                        }
                        Model.group = group;
                        Model.FolderList = LoadFolders(CurrentUser);
                        return View(Model);
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public ActionResult GroupGrid(int folderID)
        {
            return PartialView("Partials/_GroupGrid", ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers(folderID, CurrentUser, 1, 10000, false, "active").DataTableToListGroups());
        }

        public ActionResult GroupGridObject(int folderID)
        {
            //return PartialView("Partials/_GroupGrid", ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers(folderID, CurrentUser, 1, 10000, false, "active").DataTableToListGroups());

            return Json(ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers(folderID, CurrentUser, 1, 10000, false, "active").DataTableToListGroups());
        }

        [HttpPost]
        public ActionResult Search(SearchParams searchParams)
        {
            int pageSize = 1000000; // max out page size. We don't need this paging anymore.
            DataTable groupList = new DataTable();

            if (string.IsNullOrEmpty(searchParams.profileName))
                searchParams.profileName = "";
            if (searchParams.searchType.Equals("Group"))
            {
                groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(searchParams.profileName, searchParams.searchCriterion, CurrentUser.CustomerID, CurrentUser.UserID, searchParams.folderID, 1, pageSize, searchParams.allFolders, searchParams.archiveFilter);
            }
            else if (searchParams.searchType.Equals("Profile"))
            {
                groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(searchParams.profileName, searchParams.searchCriterion, CurrentUser.CustomerID, CurrentUser.UserID, searchParams.folderID, 1, pageSize, searchParams.allFolders, searchParams.archiveFilter);
            }

            List<Group> groupListFound = groupList.DataTableToListGroups();
            if (searchParams.allFolders)
            {
                foreach (var group in groupListFound)
                {
                    ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(group.GroupID);
                    if (g.FolderID.HasValue && g.FolderID.Value > 0)
                    {
                        ECN_Framework_Entities.Communicator.Folder f = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID_NoAccessCheck(g.FolderID.Value);
                        group.FolderName = f.FolderName;
                    }
                    else
                        group.FolderName = "root";
                }
            }
            ViewBag.AllFolders = searchParams.allFolders;
            return PartialView("Partials/_SearchGroupGrid", groupListFound);
        }

        public void Download()
        {
            DataTable emailGroupView = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByUserID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID);

            string filePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID + "/downloads/");
            string fileName = string.Format("subscribercounts_{0}_{1}_{2}_{3}.xml", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Second);


            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fileDestination = Path.Combine(filePath, fileName);
            XmlTextWriter xmlWriter = null;
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(fileDestination, FileMode.OpenOrCreate, FileAccess.Write);
                xmlWriter = new XmlTextWriter(fileStream, System.Text.Encoding.Unicode);

                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument(true);
                xmlWriter.WriteStartElement("Lists");

                string oFolderID = string.Empty;
                int count = 0;
                foreach (DataRow emailGroup in emailGroupView.AsEnumerable())
                {
                    if (oFolderID != emailGroup["FolderID"].ToString())
                    {
                        if (oFolderID != string.Empty)
                            xmlWriter.WriteEndElement();

                        oFolderID = emailGroup["FolderID"].ToString();
                        xmlWriter.WriteStartElement("Folder");
                        xmlWriter.WriteAttributeString("name", emailGroup["FolderName"].ToString());
                    }

                    xmlWriter.WriteStartElement("Group");
                    xmlWriter.WriteAttributeString("name", emailGroup["GroupName"].ToString());
                    xmlWriter.WriteRaw(emailGroup["Subscribers"].ToString());
                    xmlWriter.WriteEndElement();

                    if (count == emailGroupView.Rows.Count - 1)
                    {
                        xmlWriter.WriteEndElement();
                    }
                    count++;
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            catch
            {
            }
            finally
            {
                if (xmlWriter != null)
                {
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            Response.ContentType = "application/xml";
            Response.AddHeader("content-disposition", "attachment; filename=subscribercount.xml");
            Response.WriteFile(Path.Combine(filePath, fileName));
            Response.Flush();
            Response.End();
        }

        public ActionResult Delete(int Id)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Group.Delete(Id, CurrentUser);
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
            response.Add("redirect to Index");
            return Json(response);
        }

        public string ArchiveGroup(int id)
        {
            ECN_Framework_Entities.Communicator.Group f = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(id, CurrentUser);
            bool archive = f.Archived.HasValue ? f.Archived.Value : false;
            ECN_Framework_BusinessLayer.Communicator.Group.Archive(id, !archive, CurrentUser.CustomerID, CurrentUser);

            return f.GroupID.ToString();
        }

        public ActionResult Add()
        {
            string GroupName = this.GetTempData("gn") as string;
            string GroupDescription = this.GetTempData("gd") as string;
            int? FolderID = this.GetTempData("fid") as int?;
            List<ECNError> Errors = this.GetTempData("ECNErrors") as List<ECN_Framework_Common.Objects.ECNError>;

            Models.GroupWrapper Model = new GroupWrapper();
            if (GroupName != null)
                Model.group.GroupName = GroupName;
            if (GroupDescription != null)
                Model.group.GroupDescription = GroupDescription;
            if (Errors != null)
                Model.Errors = Errors;
            if (FolderID != null)
                Model.group.FolderID = FolderID;

            Model.FolderList = LoadFolders(CurrentUser);

            List<ECN_Framework_Entities.Communicator.Folder> folderList =
                ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            var sortedFolderList = (from src in folderList
                                    orderby src.FolderName
                                    select src).ToList();

            Model.KendoFolders = sortedFolderList.ToKendoTree();

            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
            {
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SeedList))
                {
                    Model.SeedListAllowed = true;
                }
                else
                {
                    Model.SeedListAllowed = false;
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult Add(ecn.communicator.mvc.Models.Group group)
        {
            List<string> response = new List<string>();

            if (group.GroupDescription == null)
                group.GroupDescription = string.Empty;
            if (group.GroupName == null)
                group.GroupName = string.Empty;


            // I don't know what this is.
            //int pub_folder = 0;
            //if (PublicFolder.Checked)
            //{
            //    pub_folder = 1;
            //}

            string gname = ECN_Framework_Common.Functions.StringFunctions.CleanString(group.GroupName.ToString().Trim());
            string gdesc = ECN_Framework_Common.Functions.StringFunctions.CleanString(group.GroupDescription.ToString().Trim());
            try
            {
                //ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                ECN_Framework_Entities.Communicator.Group internalGroup = group.ToGroup_InternalModel();

                internalGroup.PublicFolder = 0; //pub_folder;
                internalGroup.AllowUDFHistory = "N";
                internalGroup.OwnerTypeCode = "customer";
                internalGroup.CreatedUserID = CurrentUser.UserID;
                internalGroup.CustomerID = CurrentUser.CustomerID;
                ECN_Framework_BusinessLayer.Communicator.Group.Save(internalGroup, CurrentUser);
                //return Json(new { Url = new UrlHelper(Request.RequestContext).Action("Index") });
                response.Add("200");
                response.Add(new UrlHelper(Request.RequestContext).Action("Index"));
                return Json(response);
            }
            catch (ECNException ex)
            {
                response.Add("500");

                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    //err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                    response.Add(ecnError.Entity + ": " + ecnError.ErrorMessage);
                }


                return Json(response);
                //this.SetTempData("ECNErrors", ex.ErrorList);
                //this.SetTempData("gn", group.GroupName);
                //this.SetTempData("gd", group.GroupDescription);
                //this.SetTempData("fid", group.FolderID);
                //return Json(new { Url = new UrlHelper(Request.RequestContext).Action("Add") });
            }
        }

        [HttpPost]
        public ActionResult Update(Models.Group group)
        {
            List<string> response = new List<string>();
            string gname = ECN_Framework_Common.Functions.StringFunctions.CleanString(group.GroupName.Trim());
            string gdesc = ECN_Framework_Common.Functions.StringFunctions.CleanString(group.GroupDescription == null ? "" : group.GroupDescription.Trim());
            ECN_Framework_Entities.Communicator.Group internalGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(group.GroupID, CurrentUser);
            internalGroup.GroupName = gname;
            internalGroup.GroupDescription = gdesc;
            internalGroup.FolderID = group.FolderID;
            internalGroup.UpdatedUserID = CurrentUser.UserID;
            //internalGroup.PublicFolder = pub_folder;
            //internalGroup.OwnerTypeCode = "customer";
            internalGroup.IsSeedList = group.IsSeedList.Value;
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Group.Save(internalGroup, CurrentUser);
                //return Json(new { Url = new UrlHelper(Request.RequestContext).Action("Index") });
                response.Add("200");
                response.Add(new UrlHelper(Request.RequestContext).Action("Index"));
                return Json(response);
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
                //this.SetTempData("ECNErrors", ex.ErrorList);
                //return Json(new { Url = new UrlHelper(Request.RequestContext).Action("Edit", new { id = group.GroupID }) });
            }
        }

        public List<ECN_Framework_Entities.Communicator.Folder> LoadFolders(KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(user.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), user);
            ECN_Framework_Entities.Communicator.Folder root = new ECN_Framework_Entities.Communicator.Folder();
            root.FolderID = 0;
            root.FolderName = "root";
            folderList.Insert(0, root);
            return folderList;
        }

        private void LoadDropDowns(ecn.communicator.mvc.Models.GroupWrapper wrapper, KMPlatform.Entity.User user, int GroupID)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, user);
            if (group != null)
            {
                if (group.MasterSupression == null || group.MasterSupression.Value == 0)
                {
                    wrapper.SubscribeTypeCodes.Clear();
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Subscribes", "S"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("UnSubscribes", "U"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Master Suppressed", "M"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Marked as Bad Records", "D"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Pending Subscribes", "P"));
                }
                else
                {
                    wrapper.SubscribeTypeCodes.Clear();
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("UnSubscribes", "U"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Bounce", "B"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Abuse Complaint", "A"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Manual Upload", "M"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Feedback Loop(or Spam Complaint)", "F"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Email Address Change", "E"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Unknown User", "?"));
                }
            }
        }

        [HttpPost]
        public ActionResult FilterEmails(int GroupID, string searchString, string searchType = "", string searchCriterion = "")
        {
            try
            {
                int LARGE_NUMBER = 10000;
                DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, GroupID, 1, LARGE_NUMBER, searchString);
                DataTable emailstable = emailsListDS.Tables[1];
                List<ECN_Framework_Entities.Communicator.Email> internalEmails = emailstable.DataTableToList<ECN_Framework_Entities.Communicator.Email>();
                List<ecn.communicator.mvc.Models.Email> externalEmails = new List<Email>();
                foreach (var e in internalEmails)
                    externalEmails.Add(new Models.Email(e, GroupID));
                return PartialView("Partials/_GroupEditor_Subscribers", externalEmails);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ecn.communicator.mvc.Controllers.GroupController.FilterEmails", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                return PartialView("Partials/_ErrorNotification");
            }
        }

        public void Export(
            string groupId,
            string subscribeType,
            string emailAddress,
            string searchType,
            string downloadType,
            string profFilter)
        {
            Guard.NotNull(CurrentUser, nameof(CurrentUser));

            var customerId = CurrentUser.CustomerID.ToString();
            var fileName = $"{customerId}-{groupId}emails{downloadType}";
            var outputFilePath = GetOutputFilePath(fileName, customerId);
            using (var outputFile = SystemFile.AppendText(outputFilePath))
            {
                var emailProfiles =
                    GetEmailProfiles(groupId, subscribeType, profFilter, customerId, emailAddress, searchType);

                if (ExcelFileExtension.Equals(downloadType, StringComparison.OrdinalIgnoreCase) ||
                    TextFileExtension.Equals(downloadType, StringComparison.OrdinalIgnoreCase))
                {
                    var regularLineWrapperTemplate = $"{Placeholder}{TabChar}";
                    WriteDataTableToFile(emailProfiles, outputFile, regularLineWrapperTemplate);

                    Response.ContentType = ExcelFileExtension.Equals(downloadType, StringComparison.OrdinalIgnoreCase)
                        ? ExcelFileContentType
                        : CsvFileContentType;
                }
                else if (CsvFileExtension.Equals(downloadType, StringComparison.OrdinalIgnoreCase))
                {
                    var csvLineWrapperTemplate = $"{CsvWrapperTemplate}{CommaChar}";
                    WriteDataTableToFile(emailProfiles, outputFile, csvLineWrapperTemplate);
                    Response.ContentType = CsvFileContentType;
                }
                else if (XmlFileExtensions.Equals(downloadType, StringComparison.OrdinalIgnoreCase))
                {
                    var emailDataSet = new DataSet();
                    emailDataSet.Tables.Add(emailProfiles);
                    emailDataSet.WriteXml(outputFile);
                    Response.ContentType = XmlFileContentType;
                }
            }

            Response.AddHeader(ContentDispositionHeaderName, $"{AttachmentHeaderName}{fileName}");
            Response.WriteFile(outputFilePath);
            Response.Flush();
            Response.End();
        }

        private void WriteDataTableToFile(DataTable emailProfiles, StreamWriter outputFile, string lineWrapperTemplate)
        {
            Guard.NotNull(outputFile, nameof(outputFile));
            Guard.NotNull(emailProfiles, nameof(emailProfiles));

            var columnHeadings = GetDataTableColumns(emailProfiles);
            var columnHeadingsEnumerator = columnHeadings.GetEnumerator();

            var text = new StringBuilder();
            while (columnHeadingsEnumerator.MoveNext())
            {
                text.AppendFormat(lineWrapperTemplate, columnHeadingsEnumerator.Current);
            }
            outputFile.WriteLine(text.ToString());

            foreach (var row in emailProfiles.Rows)
            {
                var dataRow = row as DataRow;
                if (dataRow == null)
                {
                    continue;
                }

                text.Clear();
                columnHeadingsEnumerator.Reset();
                while (columnHeadingsEnumerator.MoveNext())
                {
                    if (columnHeadingsEnumerator.Current == null)
                    {
                        continue;
                    }

                    var columnText = dataRow[columnHeadingsEnumerator.Current.ToString()]
                        .ToString()
                        .Replace(NewLineChar, string.Empty)
                        .Replace(CarriageReturnChar, string.Empty)
                        .Replace(DoubleQuoteChar, string.Empty);

                    text.Append(string.Format(lineWrapperTemplate, columnText));
                }

                outputFile.WriteLine(text.ToString());
            }
        }

        private string GetOutputFilePath(string fileName, string customerId)
        {
            var relativeFilePath = String.Format(
                DefaultCustomersDownloadsPaths,
                ConfigurationManager.AppSettings[ImagesVirtualPathConfigurationKey],
                customerId);

            var physicalFilePath = Server.MapPath(relativeFilePath);
            var outputFilePath = Path.Combine(physicalFilePath ?? string.Empty, fileName);

            if (!Directory.Exists(physicalFilePath))
            {
                Directory.CreateDirectory(physicalFilePath);
            }

            if (SystemFile.Exists(outputFilePath))
            {
                SystemFile.Delete(outputFilePath);
            }

            return outputFilePath;
        }

        private static DataTable GetEmailProfiles(
            string groupId,
            string subscribeType,
            string profFilter,
            string customerId,
            string emailAddress,
            string searchType)
        {
            var groupIdValue = 0;
            var customerIdValue = 0;

            Guard.NotNull(subscribeType, nameof(subscribeType));
            Guard.For(() => !int.TryParse(groupId, out groupIdValue), () => new ArgumentException(NotValidNumberErrorMessage, nameof(groupId)));
            Guard.For(() => !int.TryParse(customerId, out customerIdValue), () => new ArgumentException(NotValidNumberErrorMessage, nameof(customerIdValue)));

            var filter = GetEmailAddressFilter(emailAddress, searchType);

            subscribeType = subscribeType.Equals(WildcardChar)
                ? DefaultSubscribeType
                : string.Format(SubscribeTypeTemplate, subscribeType);

            return EmailGroup.GetGroupEmailProfilesWithUDF(
                groupIdValue,
                customerIdValue,
                filter,
                subscribeType,
                profFilter);
        }

        private static string GetEmailAddressFilter(string emailAddress, string searchType)
        {
            Guard.NotNull(emailAddress, nameof(emailAddress));
            Guard.NotNull(searchType, nameof(searchType));

            if (emailAddress.Length <= 0)
            {
                return string.Empty;
            }

            if (searchType.Equals(EqualsSearchCriteria))
            {
                return string.Format(EqualsSearchSyntax, emailAddress);
            }

            if (searchType.Equals(EndsWithSearchCriteria))
            {
                return string.Format(EndsWithSearchSyntax, emailAddress);
            }

            if (searchType.Equals(StartsWithSearchCriteria))
            {
                return string.Format(StartsWithSearchSyntax, emailAddress);
            }

            return string.Format(LikeSearchSyntax, emailAddress);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GroupsReadToGrid([DataSourceRequest]DataSourceRequest request, string searchType, string searchCriterion, string profileName, string archiveFilter, bool allFolders, int FolderID, int PageNumber)
        {
            List<Group> groups = new List<Group>();
            int totalCount = 0;
            DataTable dtGroups;
            DataSourceResult result;


            string sortColumn = "GroupName"; // lstgs[0].SortColumnName;
            string sortdirection = "ASC";
            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortdirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

            }



            if (searchType.ToLower().Equals("profile"))
            {
                dtGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName(profileName.Replace("'", "\'\'"), searchCriterion, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, FolderID, PageNumber, request.PageSize, allFolders, archiveFilter, sortColumn, sortdirection);
                groups = dtGroups.DataTableToListGroups();

            }
            else
            {
                dtGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName(profileName, searchCriterion, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, FolderID, PageNumber, request.PageSize, allFolders, archiveFilter, sortColumn, sortdirection);
                groups = dtGroups.DataTableToListGroups();

            }

            if (dtGroups != null && dtGroups.Rows.Count > 0)
                totalCount = Convert.ToInt32(dtGroups.Rows[0]["TotalCount"].ToString());

            IQueryable<Group> gs = groups.AsQueryable();
            result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result);
        }
        #endregion

        #region Add Emails
        public ActionResult EmailLoader()
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.AddEmails))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

            return View();
        }

        public ActionResult AddEmails(string GL, int groupId, int customerId, string ST, string FT, string addresses)
        {
            List<string> response = new List<string>();
            List<ECN_Framework_Entities.Communicator.Group> Groups = GetSelectedGroups(GL, groupId);

            if (Groups.Any())
            {
                Guid lblGuid = Guid.NewGuid();
                string fileName = lblGuid.ToString();
                int emailsAdded = 0;
                string emailAddressToAdd = addresses;
                StringBuilder xmlInsert = new StringBuilder();

                DateTime startDateTime = DateTime.Now;

                Hashtable hUpdatedRecords = new Hashtable();
                DataTable emailRecordsDT = new DataTable();

                ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(CurrentUser.CustomerID, CurrentUser);

                DataTable mergedDT = new DataTable();
                if (emailAddressToAdd.Length > 0)
                {
                    emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",").Replace("\n", ",");
                    if (emailAddressToAdd.LastIndexOf(",") == (emailAddressToAdd.Length - 1))
                        emailAddressToAdd = emailAddressToAdd.Substring(0, emailAddressToAdd.Length - 1);
                    StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                    //Subscribe/Unsubscribe from one or all Groups
                    if (ST.Equals("U"))
                    {
                        foreach (ECN_Framework_Entities.Communicator.Group group in Groups)
                        {
                            st = new StringTokenizer(emailAddressToAdd, ',');
                            ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(group.GroupID), CurrentUser);
                            if ((!g.IsSeedList.HasValue || !g.IsSeedList.Value) && g.GroupID != MSGroup.GroupID)
                            {
                                xmlInsert = new StringBuilder();
                                xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                                while (st.HasMoreTokens())
                                {
                                    string email = st.NextToken().Trim();
                                    ECN_Framework_Entities.Communicator.EmailGroup eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(email, g.GroupID, CurrentUser);
                                    if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(email))
                                    {
                                        if (eg != null && eg.SubscribeTypeCode != "U")
                                        {
                                            xmlInsert.Append("<Emails><emailaddress>" + email + "</emailaddress></Emails>");
                                        }
                                    }
                                    else
                                    {
                                        response.Add("500");
                                        response.Add("Invalid email address: " + email);
                                        return Json(response);
                                    }
                                }
                                DataTable resultsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(CurrentUser, CurrentUser.CustomerID, g.GroupID, xmlInsert.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", FT, ST, true, fileName, "Ecn.communicator.main.lists.addressloader.AddEmails - SubscribeType U");
                                if (mergedDT.Columns.Count == 0)
                                {
                                    mergedDT = resultsDT.Clone();
                                }
                                mergedDT.Merge(resultsDT);
                            }
                        }

                    }//Subscribe to all groups they aren't currently unsubscribed from
                    else if (ST.Equals("S"))
                    {
                        foreach (ECN_Framework_Entities.Communicator.Group group in Groups)
                        {
                            st = new StringTokenizer(emailAddressToAdd, ',');
                            xmlInsert = new StringBuilder();
                            xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");

                            int currentGroupID = Convert.ToInt32(group.GroupID);
                            ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(currentGroupID, CurrentUser);

                            //if (!g.IsSeedList.HasValue || !g.IsSeedList.Value)
                            //{
                            while (st.HasMoreTokens())
                            {
                                string email = st.NextToken().Trim();

                                if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(email))
                                {
                                    xmlInsert.Append("<Emails><emailaddress>" + email + "</emailaddress></Emails>");
                                }
                                else
                                {
                                    response.Add("500");
                                    response.Add("Invalid email address: " + email);
                                    return Json(response);
                                }
                            }

                            DataTable resultsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(CurrentUser, CurrentUser.CustomerID, currentGroupID, xmlInsert.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", FT, ST, true, fileName, "Ecn.communicator.main.lists.addressloader.AddEmails - SubscribeType S");
                            if (mergedDT.Columns.Count == 0)
                            {
                                mergedDT = resultsDT.Clone();
                            }
                            mergedDT.Merge(resultsDT);
                            //}
                        }
                    }

                    CalculateCounts(mergedDT, hUpdatedRecords);


                    if (hUpdatedRecords.Count > 0)
                    {
                        DataTable dtRecords = new DataTable();

                        dtRecords.Columns.Add("Description");
                        dtRecords.Columns.Add("ActionCode");
                        dtRecords.Columns.Add("Totals");
                        dtRecords.Columns.Add("sortOrder");

                        DataRow row;

                        foreach (DictionaryEntry de in hUpdatedRecords)
                        {
                            row = dtRecords.NewRow();

                            if (de.Key.ToString() == "T")
                            {
                                row["Description"] = "Total Records in the File";
                                row["ActionCode"] = "T";
                                row["sortOrder"] = 1;
                            }
                            else if (de.Key.ToString() == "I")
                            {
                                row["Description"] = "New";
                                row["ActionCode"] = "I";
                                row["sortOrder"] = 2;
                            }
                            else if (de.Key.ToString() == "U")
                            {
                                row["Description"] = "Changed";
                                row["ActionCode"] = "U";
                                row["sortOrder"] = 3;
                            }
                            else if (de.Key.ToString() == "D")
                            {
                                row["Description"] = "Duplicate(s)";
                                row["ActionCode"] = "D";
                                row["sortOrder"] = 4;
                            }
                            else if (de.Key.ToString() == "S")
                            {
                                row["Description"] = "Skipped";
                                row["ActionCode"] = "S";
                                row["sortOrder"] = 5;
                            }
                            else if (de.Key.ToString() == "M")
                            {
                                row["Description"] = "Skipped (Emails in Master Suppression)";
                                row["ActionCode"] = "M";
                                row["sortOrder"] = 6;
                            }
                            row["Totals"] = de.Value;
                            dtRecords.Rows.Add(row);
                        }

                        row = dtRecords.NewRow();
                        row["Description"] = "&nbsp;";
                        row["Totals"] = " ";
                        row["sortOrder"] = 8;
                        dtRecords.Rows.Add(row);

                        TimeSpan duration = DateTime.Now - startDateTime;

                        row = dtRecords.NewRow();
                        row["Description"] = "Time to Import";
                        row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                        row["ActionCode"] = "time";
                        row["sortOrder"] = 9;
                        dtRecords.Rows.Add(row);

                        DataView dv = dtRecords.DefaultView;
                        dv.Sort = "sortorder asc";
                        DataTable sortedDT = dv.ToTable();
                        //sortedDT.Columns.RemoveAt(1);
                        //sortedDT.Columns.RemoveAt(2);

                        string dtHtml = "<table border=\"1\" cellpadding=\"5\"><thead><tr>";
                        foreach (System.Data.DataColumn col in sortedDT.Columns)
                        {
                            if (col.Caption.ToLower().Equals("description") || col.Caption.ToLower().Equals("totals"))
                            {
                                dtHtml += "<th>" + col.Caption + "</th>";
                            }
                        }
                        dtHtml += "</tr></thead><tbody>";
                        foreach (System.Data.DataRow r in sortedDT.Rows)
                        {
                            dtHtml += "<tr>";

                            dtHtml += "<td>" + r["Description"].ToString() + "</td>";
                            if (r["ActionCode"].ToString().Equals("U") || r["ActionCode"].ToString().Equals("S") || r["ActionCode"].ToString().Equals("M") || r["ActionCode"].ToString().Equals("D") || r["ActionCode"].ToString().Equals("I"))
                            {
                                dtHtml += "<td><a href='/ecn.communicator.mvc/Group/DownloadImportedEmails?type=" + r["ActionCode"].ToString() + "&file=&guid=" + lblGuid.ToString() + "'>" + r["Totals"].ToString() + "</a></td>";
                            }
                            else
                            {
                                dtHtml += "<td>" + r["Totals"].ToString() + "</td>";
                            }

                            dtHtml += "</tr>";
                        }
                        dtHtml += "</tbody></table>";
                        response.Add("200");
                        response.Add(dtHtml);
                        return Json(response);
                    }
                    response.Add("200");
                    response.Add("<font face=verdana size=2 color=#000000>&nbsp;" + emailsAdded.ToString() + " rows updated/inserted </font>");
                    return Json(response);
                }
                else
                {
                    response.Add("200");
                    response.Add("<font face=verdana size=2 color=#000000>&nbsp;" + emailsAdded.ToString() + " rows updated/inserted </font>");
                    return Json(response);
                }
            }
            else
            {
                response.Add("500");
                response.Add("Please select a Group.");
                return Json(response);
            }
        }

        private List<ECN_Framework_Entities.Communicator.Group> GetSelectedGroups(string GL, int groupId)
        {
            if (GL == "A")
            {
                //All groups
                List<ECN_Framework_Entities.Communicator.Group> groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(CurrentUser.CustomerID, CurrentUser);
                var result = (from src in groupList
                              orderby src.GroupName
                              select src).ToList();

                ECN_Framework_Entities.Communicator.Group masterSuppressionGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(CurrentUser.CustomerID, CurrentUser);
                result.RemoveAll(x => x.GroupID == masterSuppressionGroup.GroupID);
                return result;
            }
            if (GL == "M")
            {
                //Master Suppression Group
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(CurrentUser.CustomerID, CurrentUser);
                List<ECN_Framework_Entities.Communicator.Group> groups = new List<ECN_Framework_Entities.Communicator.Group>();
                if (group != null) { groups.Add(@group); }
                return groups;
            }
            else
            {
                //One group
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupId, CurrentUser);
                List<ECN_Framework_Entities.Communicator.Group> groups = new List<ECN_Framework_Entities.Communicator.Group>();
                if (group != null) { groups.Add(@group); }
                return groups;
            }
        }

        private static void CalculateCounts(DataTable tempDT, Hashtable hUpdatedRecords)
        {
            if (tempDT.Rows.Count <= 0) return;
            foreach (DataRow dr in tempDT.Rows)
            {
                if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                {
                    hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                }
                else
                {
                    int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                    hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                }
            }
        }
        #endregion

        #region Import Data
        public ActionResult ImportManager()
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

            string DataFilePath = "/customers/" + CurrentUser.CustomerID + "/data";

            if (!Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath)))
                Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath));

            List<DataFile> files = createDataSource(DataFilePath);
            Session["sFiles" + CurrentUser.UserID] = null;
            return View(files);
        }

        public ActionResult AddFiles()
        {
            List<string> response = new List<string>();
            ArrayList aFiles = new ArrayList();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        aFiles = (ArrayList)Session["sFiles" + CurrentUser.UserID];

                        if (aFiles == null)
                            aFiles = new ArrayList();

                        aFiles.Add(file);
                        Session["sFiles" + CurrentUser.UserID] = aFiles;
                    }
                }
                response.Add("200");
                foreach (HttpPostedFileBase f in aFiles)
                {
                    response.Add(f.FileName);
                }
            }
            catch (Exception ex)
            {
                response.Add("500");
                response.Add(ex.Message);
            }
            return Json(response);
        }

        public ActionResult RemoveFiles(int fileIndex)
        {
            List<string> response = new List<string>();
            if (fileIndex >= 0)
            {

                ArrayList aFiles = new ArrayList();
                aFiles = (ArrayList)Session["sFiles" + CurrentUser.UserID];
                if (aFiles == null)
                    aFiles = new ArrayList();
                if (aFiles.Count > fileIndex && aFiles.Count > 0)
                {
                    aFiles.RemoveAt(fileIndex);
                    Session["sFiles" + CurrentUser.UserID] = aFiles;
                }
                response.Add("200");
                foreach (HttpPostedFileBase f in aFiles)
                {
                    response.Add(f.FileName);
                }
                return Json(response);
            }
            else
            {
                response.Add("400");
                response.Add("Please select a file to remove");
                return Json(response);
            }
        }

        public ActionResult UploadFilesToServer()
        {
            string DataFilePath = "/customers/" + CurrentUser.CustomerID + "/data";
            string baseLocation = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath);
            List<string> response = new List<string>();
            int filesUploaded = 0;
            string status = "";
            bool bError = false;
            if (!string.IsNullOrWhiteSpace(baseLocation))
            {
                baseLocation = System.Web.HttpUtility.UrlDecode(baseLocation);
            }
            ArrayList aFiles = (ArrayList)Session["sFiles" + CurrentUser.UserID];
            if (aFiles != null)
            {
                foreach (HttpPostedFileBase f in aFiles)
                {
                    string fn = string.Empty;
                    try
                    {
                        fn = System.IO.Path.GetFileNameWithoutExtension(f.FileName);
                        fn = ECN_Framework_Common.Functions.StringFunctions.ReplaceNonAlphaNumeric(fn, "_");
                        f.SaveAs(baseLocation + "\\" + fn + System.IO.Path.GetExtension(f.FileName));
                        filesUploaded++;
                        status += fn + "<br>";

                        try
                        {
                            ecn.common.classes.DataFunctions.Execute("insert into Uploadlog (UserID,CustomerID,Path,FileName,uploaddate,PageSource) values (" + CurrentUser.UserID + "," + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID + ",'" + baseLocation + "','" + fn.Replace("'", "''") + "',getdate(),'" + Request.ServerVariables["PATH_INFO"].ToString() + "')");
                        }
                        catch
                        { }

                    }
                    catch (Exception err)
                    {
                        bError = true;
                        response.Add("500");
                        response.Add("Error File Save " + fn + "<br>" + err.Message.ToString());
                        return Json(response);
                    }
                }
                if (!bError)
                {
                    response.Add("200");
                    response.Add("These " + filesUploaded + " file(s) were uploaded:<br>" + status);
                    Session["sFiles" + CurrentUser.UserID] = new ArrayList();
                    aFiles.Clear();
                    // refreshFileDD
                    List<DataFile> files = createDataSource(DataFilePath);
                    string options = "<option value=\"\">- Select File -</option>";
                    foreach (DataFile f in files)
                    {
                        options += "<option value=\"" + f.FileName + "\">" + f.FileName + "</option>";
                    }
                    response.Add(options);
                    // refreshFileGrid
                    response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_FileLibraryGrid", files));
                }
            }
            else
            {
                response.Add("500");
                response.Add("Cannot upload. Empty files.");
            }

            return Json(response);
        }

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetFiles()
        {
            string DataFilePath = "/customers/" + CurrentUser.CustomerID + "/data";
            List<DataFile> files = createDataSource(DataFilePath);
            files.Insert(0, new DataFile() { Date = DateTime.Now, FileName = "- Select File -", Size = 0 });
            var sl = new List<SelectListItem>(files.Select(item => new SelectListItem() { Text = item.FileName, Value = item.FileName, Selected = item.FileName.ToLower().Equals("- select file -") }));

            return Json(sl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDataFile(string thefile)
        {
            string DataFilePath = "/customers/" + CurrentUser.CustomerID + "/data";
            string baseLocation = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath);
            List<string> response = new List<string>();

            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(baseLocation + "//" + thefile);
                file.Delete();

                List<DataFile> files = createDataSource(DataFilePath);
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_FileLibraryGrid", files));
                return Json(response);
            }
            catch (Exception err)
            {
                response.Add("500");
                response.Add("Error File Delete " + thefile + "<br>" + err.Message.ToString());
                return Json(response);
            }
        }

        public List<DataFile> createDataSource(string datapath)
        {
            List<DataFile> dtFiles = new List<DataFile>();
            DataFile drFiles;

            System.IO.FileInfo file = null;
            string[] files = null;
            string filename = "";
            files = System.IO.Directory.GetFiles(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + datapath), "*.*");

            for (int i = 0; i <= files.Length - 1; i++)
            {
                file = new System.IO.FileInfo(files[i]);
                filename = file.Name.ToString();
                if (filename.ToLower().EndsWith(".xml") ||
                    filename.ToLower().EndsWith(".txt") ||
                    filename.ToLower().EndsWith(".csv") ||
                    filename.ToLower().EndsWith(".xlsx") ||
                    filename.ToLower().EndsWith(".xls"))
                {
                    drFiles = new DataFile();
                    drFiles.FileName = file.Name;
                    drFiles.Size = (file.Length / 1000);
                    drFiles.Date = file.LastWriteTime;
                    dtFiles.Add(drFiles);
                }
            }

            return dtFiles;
        }

        public ActionResult ImportDataFromFile(string file, string ftc, string stc, string gid, string ft, string line, string sheet, string dl)
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
            Session["selectedGroupID"] = gid;
            ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(gid), CurrentUser);
            ImportDataOptions ido = new ImportDataOptions();
            ido.file = file;
            ido.ftc = ftc;
            ido.stc = stc;
            ido.gid = gid;
            ido.fileType = ft;
            ido.lineStart = line;
            ido.sheetName = sheet;
            ido.dl = dl;
            ido.lblGUID = Guid.NewGuid().ToString();
            ViewBag.GroupName = grp.GroupName;
            try
            {
                DataTable dtFile = GetDataTableByFileType(ido, 5);
                ido.numOfColumns = dtFile.Columns.Count;
                ViewBag.TableToImport = BuildEmailImportForm(dtFile, new ImportMapper(), ido.fileType, Convert.ToInt32(ido.gid));
            }
            catch (System.Data.OleDb.OleDbException oledbEx)
            {
                if (oledbEx.Message.Contains("'" + ido.sheetName + "$' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long"))
                {
                    ViewBag.TableToImport = "ERROR - Please check the sheet name.";
                }
                else
                {
                    ViewBag.TableToImport = "ERROR - Issue importing file, please check format and contents.  If you are still having problems, please contact customer service.";
                }
            }
            return View(ido);
        }

        public ActionResult CheckImportDataFromFile(string file, string ftc, string stc, string gid, string ft, string line, string sheet, string dl)
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
            List<string> response = new List<string>();
            //Session["selectedGroupID"] = gid;
            ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(gid), CurrentUser);
            ImportDataOptions ido = new ImportDataOptions();
            ido.file = file;
            ido.ftc = ftc;
            ido.stc = stc;
            ido.gid = gid;
            ido.fileType = ft;
            ido.lineStart = line;
            ido.sheetName = sheet;
            ido.dl = dl;
            ido.lblGUID = Guid.NewGuid().ToString();
            ViewBag.GroupName = grp.GroupName;
            try
            {
                DataTable dtFile = GetDataTableByFileType(ido, 5);
                ido.numOfColumns = dtFile.Columns.Count;
                string formHTML = BuildEmailImportForm(dtFile, new ImportMapper(), ido.fileType, Convert.ToInt32(ido.gid));
                response.Add("200");
            }
            catch (System.Data.OleDb.OleDbException oledbEx)
            {
                response.Add("500");
                if (oledbEx.Message.Contains("'" + ido.sheetName + "$' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long"))
                {
                    response.Add("ERROR - Please check the sheet name.");
                }
                else
                {
                    response.Add("ERROR - Issue importing file, please check format and contents.  If you are still having problems, please contact customer service.");
                }
            }
            catch (ECNException ex)
            {
                response.Add("500");
                foreach (var er in ex.ErrorList)
                {
                    response.Add(er.ErrorMessage);
                }
            }
            return Json(response);
        }

        public ActionResult ImportDataAction(ImportDataOptions ido)
        {
            List<string> response = new List<string>();
            int duplicationColumnCount = 0;
            bool UDFExists = false;
            StringBuilder xmlUDF = new StringBuilder("");
            StringBuilder xmlProfile = new StringBuilder("");
            ImportMapper mapper = new ImportMapper();
            ArrayList colRemove = new ArrayList();
            bool EmailAddressOnly = true;
            Hashtable hUpdatedRecords = new Hashtable();

            DataTable dtFile;
            DateTime startDateTime = DateTime.Now;
            try
            {

                #region Validate Mapping
                dtFile = GetDataTableByFileType(ido, 1);

                StringBuilder duplicatedColumns = new StringBuilder();

                for (int i = 0; i < dtFile.Columns.Count; i++)
                {
                    string selectedColumnName = ido.dropDownValues[i]; //Request.Params.Get(CurrentUser.CurrentClient.ClientID + "$ContentPlaceHolder1$ColumnHeaderSelect" + i);
                    if (selectedColumnName.ToLower() == "ignore")
                    {
                        continue;
                    }
                    if (!mapper.AddMapping(i, selectedColumnName))
                    {
                        duplicationColumnCount++;
                        duplicatedColumns.Append(string.Format("{0}{1}", duplicatedColumns.Length > 0 ? "/" : "", selectedColumnName));
                    }
                }

                if (duplicationColumnCount > 0)
                {
                    response.Add("500");
                    response.Add(string.Format("You have selected duplicate field names.<BR><BR>{0}.", duplicatedColumns.ToString()));
                    //BuildEmailImportForm(GetDataTableByFileType(ido, 5), mapper, ido.fileType, Convert.ToInt32(ido.gid));
                    return Json(response);
                }

                if (mapper.MappingCount == 0)
                {
                    response.Add("500");
                    response.Add("Email Address is required to import data.");
                    //BuildEmailImportForm(GetDataTableByFileType(ido, 5), mapper, ido.fileType, Convert.ToInt32(ido.gid));
                    return Json(response);
                }

                if (!mapper.HasEmailAddress)
                {
                    response.Add("500");
                    response.Add("Email Address is required to import data.");
                    //BuildEmailImportForm(GetDataTableByFileType(ido, 5), mapper, ido.fileType, Convert.ToInt32(ido.gid));
                    return Json(response);
                }
                #endregion
            }
            catch
            {
                response.Add("500");
                response.Add("Email Address is required to import data.");
                //BuildEmailImportForm(GetDataTableByFileType(ido, 5), mapper, ido.fileType, Convert.ToInt32(ido.gid));
                return Json(response);
            }

            try
            {
                try
                {
                    dtFile = GetDataTableByFileType(ido, 0);
                }
                catch (System.Data.OleDb.OleDbException oledbEx)
                {
                    if (oledbEx.Message.Contains("'" + ido.sheetName + "$' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long"))
                    {
                        response.Add("500");
                        response.Add("Please check the sheet name.");
                        return Json(response);
                    }
                    else
                        throw oledbEx;

                }
                for (int i = 0; i < dtFile.Columns.Count; i++)
                {
                    string selectedColumnName = ido.dropDownValues[i]; //Request.Params.Get(CurrentUser.CurrentClient.ClientID + "$ContentPlaceHolder1$ColumnHeaderSelect" + i);

                    if (selectedColumnName.ToLower() == "ignore")
                    {
                        colRemove.Add("delete" + i);
                        dtFile.Columns[i].ColumnName = "delete" + i;
                    }
                    else
                    {
                        if (selectedColumnName.ToLower().ToString().IndexOf("user_") > -1)
                            UDFExists = true;
                        else if (selectedColumnName.ToLower() != "emailaddress")
                            EmailAddressOnly = false;

                        dtFile.Columns[i].ColumnName = selectedColumnName.ToLower().ToString();
                    }
                }

                for (int j = 0; j < colRemove.Count; j++)
                {
                    dtFile.Columns.Remove(colRemove[j].ToString());
                }
                colRemove.Clear();

                Hashtable hGDFFields = GetGroupDataFields(Convert.ToInt32(ido.gid));

                bool bRowCreated = false;

                int iTotalRecords = dtFile.Rows.Count;
                int iProgressInc = iTotalRecords / 50;
                int iProgressCount = iProgressInc;
                int iProgressPercent = 0;

                for (int cnt = 0; cnt < dtFile.Rows.Count; cnt++)
                {
                    //if (cnt == 0 && dtFile.Rows.Count >= 100)
                    //    initNotify("Starting Import!");

                    DataRow drFile = dtFile.Rows[cnt];

                    bRowCreated = false;

                    xmlProfile.Append("<Emails>");

                    foreach (DataColumn dcFile in dtFile.Columns)
                    {
                        if (dcFile.ColumnName.IndexOf("user_") == -1 && dcFile.ColumnName.IndexOf("delete") == -1)
                        {
                            if (drFile[dcFile.ColumnName].ToString().Trim().Length > 0)
                            {
                                xmlProfile.Append("<" + dcFile.ColumnName + ">" + CleanXMLString(drFile[dcFile.ColumnName].ToString()) + "</" + dcFile.ColumnName + ">");
                            }
                        }

                        if (UDFExists)
                        {
                            if (hGDFFields.Count > 0)
                            {
                                if (dcFile.ColumnName.IndexOf("user_") > -1)
                                {
                                    if (!bRowCreated)
                                    {
                                        xmlUDF.Append("<row>");
                                        xmlUDF.Append("<ea>" + CleanXMLString(drFile["emailaddress"].ToString()) + "</ea>");
                                        bRowCreated = true;
                                    }

                                    if (drFile[dcFile.ColumnName].ToString().Trim().Length > 0)
                                    {
                                        xmlUDF.Append("<udf id=\"" + hGDFFields[dcFile.ColumnName].ToString() + "\">");

                                        xmlUDF.Append("<v><![CDATA[" + CleanXMLString(drFile[dcFile.ColumnName].ToString()) + "]]></v>");

                                        xmlUDF.Append("</udf>");
                                    }
                                }
                            }
                        }
                    }
                    xmlProfile.Append("</Emails>");

                    if (bRowCreated)
                        xmlUDF.Append("</row>");

                    if ((cnt != 0) && (cnt % 5000 == 0) || (cnt == dtFile.Rows.Count - 1))
                    {
                        UpdateToDB(Convert.ToInt32(CurrentUser.CustomerID),
                            Convert.ToInt32(ido.gid),
                            "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>",
                            "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>",
                            EmailAddressOnly,
                            hUpdatedRecords,
                            ido.ftc,
                            ido.stc,
                            ido.file,
                            ido.lblGUID);

                        xmlProfile = new StringBuilder("");
                        xmlUDF = new StringBuilder("");
                        GC.Collect();
                    }

                    if (((cnt == iProgressCount) || (cnt == dtFile.Rows.Count - 1)) && (dtFile.Rows.Count >= 100))
                    {
                        iProgressPercent = iProgressPercent + 2;

                        //Notify(iProgressPercent <= 100 ? iProgressPercent.ToString() : "101", "Importing (" + iProgressCount.ToString() + " / " + iTotalRecords.ToString() + ")");

                        iProgressCount = iProgressCount + iProgressInc;

                        if (cnt == dtFile.Rows.Count - 1)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                }
                hGDFFields.Clear();

                if (hUpdatedRecords.Count > 0)
                {
                    DataTable dtRecords = new DataTable();

                    dtRecords.Columns.Add("Description");
                    dtRecords.Columns.Add("ActionCode");
                    dtRecords.Columns.Add("Totals");
                    dtRecords.Columns.Add("sortOrder");

                    DataRow row;

                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        row = dtRecords.NewRow();

                        if (de.Key.ToString() == "T")
                        {
                            row["Description"] = "Total Records in the File";
                            row["ActionCode"] = "";
                            row["sortOrder"] = 1;
                        }
                        else if (de.Key.ToString() == "I")
                        {
                            row["Description"] = "New";
                            row["ActionCode"] = "I";
                            row["sortOrder"] = 2;
                        }
                        else if (de.Key.ToString() == "U")
                        {
                            row["Description"] = "Changed";
                            row["sortOrder"] = 3;
                            row["ActionCode"] = "U";
                        }
                        else if (de.Key.ToString() == "D")
                        {
                            row["Description"] = "Duplicate(s)";
                            row["ActionCode"] = "D";
                            row["sortOrder"] = 4;
                        }
                        else if (de.Key.ToString() == "S")
                        {
                            row["Description"] = "Skipped";
                            row["ActionCode"] = "S";
                            row["sortOrder"] = 5;
                        }
                        else if (de.Key.ToString() == "M")
                        {
                            row["Description"] = "Skipped (Emails in Master Suppression)";
                            row["sortOrder"] = 6;
                            row["ActionCode"] = "M";
                        }
                        row["Totals"] = de.Value;
                        dtRecords.Rows.Add(row);
                    }

                    row = dtRecords.NewRow();
                    row["Description"] = "";
                    row["Totals"] = " ";
                    row["sortOrder"] = 8;
                    dtRecords.Rows.Add(row);

                    TimeSpan duration = DateTime.Now - startDateTime;

                    row = dtRecords.NewRow();
                    row["Description"] = "Time to Import";
                    row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                    row["sortOrder"] = 9;
                    dtRecords.Rows.Add(row);

                    DataView dv = dtRecords.DefaultView;
                    dv.Sort = "sortorder asc";
                    DataTable sortedDT = dv.ToTable();
                    //sortedDT.Columns.RemoveAt(1);
                    //sortedDT.Columns.RemoveAt(2);

                    string dtHtml = "<table border=\"1\" cellpadding=\"5\"><thead><tr>";
                    foreach (System.Data.DataColumn col in sortedDT.Columns)
                    {
                        if (col.Caption.ToLower().Equals("description") || col.Caption.ToLower().Equals("totals"))
                        {
                            dtHtml += "<th>" + col.Caption + "</th>";
                        }
                    }
                    dtHtml += "</tr></thead><tbody>";
                    foreach (System.Data.DataRow r in sortedDT.Rows)
                    {
                        dtHtml += "<tr>";

                        dtHtml += "<td>" + r["Description"].ToString() + "</td>";
                        if (r["ActionCode"].ToString().Equals("U") || r["ActionCode"].ToString().Equals("S") || r["ActionCode"].ToString().Equals("M") || r["ActionCode"].ToString().Equals("D") || r["ActionCode"].ToString().Equals("I"))
                        {
                            dtHtml += "<td><a href='/ecn.communicator.mvc/Group/DownloadImportedEmails?type=" + r["ActionCode"].ToString() + "&file=" + ido.file + "&guid=" + ido.lblGUID + "'>" + r["Totals"].ToString() + "</a></td>";
                        }
                        else
                        {
                            dtHtml += "<td>" + r["Totals"].ToString() + "</td>";
                        }

                        dtHtml += "</tr>";
                    }
                    dtHtml += "</tbody></table>";
                    response.Add("200");
                    response.Add(dtHtml);
                    return Json(response);
                }
                else
                {
                    response.Add("200");
                    response.Add("<font face=verdana size=2 color=#000000>0 rows updated/inserted </font>");
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ImportDataFromFile.ImportData", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote(ido.gid, ido.file));
                response.Add("500");
                response.Add("An error occurred and was logged.  Please try again.  If it continues, please contact your digital services specialist.");
                //BuildEmailImportForm(GetDataTableByFileType(ido, 5), mapper, ido.fileType, Convert.ToInt32(ido.gid));
                return Json(response);
            }
        }
        public FileContentResult DownloadImportedEmails(string type, string file, string guid)
        {
            string actionCode = type;
            if (actionCode.Equals("U") || actionCode.Equals("S") || actionCode.Equals("M") || actionCode.Equals("D") || actionCode.Equals("I"))
            {

                string downloadType = ".xls";
                ArrayList columnHeadings = new ArrayList();
                IEnumerator aListEnum = null;
                string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + CurrentUser.CustomerID + "/downloads/");

                string newline = "";
                DateTime date = DateTime.Now;
                string filename = actionCode + "-" + file;
                String tfile = "emails_" + filename + downloadType;
                string outfileName = txtoutFilePath + tfile;

                if (!Directory.Exists(txtoutFilePath))
                {
                    Directory.CreateDirectory(txtoutFilePath);
                }

                if (System.IO.File.Exists(outfileName))
                {
                    System.IO.File.Delete(outfileName);
                }

                TextWriter txtfile = System.IO.File.AppendText(outfileName);
                DataTable emailstable = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ExportFromImportEmails(CurrentUser, (file + " " + guid).Trim(), actionCode);

                for (int i = 0; i < emailstable.Columns.Count; i++)
                {
                    columnHeadings.Add(emailstable.Columns[i].ColumnName.ToString());
                }

                aListEnum = columnHeadings.GetEnumerator();
                while (aListEnum.MoveNext())
                {
                    newline += aListEnum.Current.ToString() + (downloadType == ".xls" ? "\t" : ", ");
                }
                txtfile.WriteLine(newline);


                foreach (DataRow dr in emailstable.Rows)
                {
                    newline = "";
                    aListEnum.Reset();
                    while (aListEnum.MoveNext())
                    {
                        newline += dr[aListEnum.Current.ToString()].ToString() + (downloadType == ".xls" ? "\t" : ", ");
                    }
                    txtfile.WriteLine(newline);
                }
                txtfile.Close();

                if (downloadType == ".xls")
                {
                    Response.ContentType = "application/vnd.ms-excel";
                }
                else
                {
                    Response.ContentType = "text/csv";
                }

                Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
                Response.WriteFile(outfileName);
                Response.Flush();
                Response.End();

            }
            return null;
        }
        //Import Helpers
        private DataTable GetDataTableByFileType(ImportDataOptions ido, int maxRecordsToRetrieve)
        {
            int startLine = (Convert.ToInt32(ido.lineStart) - 1 >= 0) ? (Convert.ToInt32(ido.lineStart) - 1) : 0;
            string physicalDataPath = getPhysicalPath();
            return FileImporter.GetDataTableByFileType(physicalDataPath, ido.fileType, ido.file, ido.sheetName, startLine, maxRecordsToRetrieve, ido.dl.ToUpper());
        }
        private string BuildEmailImportForm(DataTable dt, ImportMapper mapper, string fileType, int gid)
        {
            if (dt == null)
            {
                return "";
            }
            HtmlTable dataCollectionTable = new HtmlTable();
            HtmlTableRow tableRows = null;
            HtmlTableCell headerColumn = null;	// <td> to hold the header which is the emails table dropdown columns
            HtmlTableCell dataColumn = null;		// <td> to hold the data from the file
            int countColumns = dt.Columns.Count;


            for (int i = 0; i < countColumns; i++)
            {
                string columnname = string.Empty; //Grab data from 1st row ; match with fieldname if matches preselect column.
                string textData = "";
                int lineStartCheck = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (lineStartCheck == 0)
                    {

                        if (fileType.Equals("C"))
                            columnname = dt.Columns[i].ToString();// dr[i].ToString();
                        else
                            columnname = dr[i].ToString();
                    }
                    textData += dr[i].ToString() + ", ";
                    lineStartCheck++;
                    if (lineStartCheck == 10)
                        break;
                }

                HtmlSelect tableColumnHeadersSelectbox = buildColumnHeaderDropdowns("ColumnHeaderSelect" + i, columnname, gid);
                if (tableColumnHeadersSelectbox.SelectedIndex < 0)
                    tableColumnHeadersSelectbox.SelectedIndex = tableColumnHeadersSelectbox.Items.IndexOf(tableColumnHeadersSelectbox.Items.FindByText(mapper.GetColumnName(i)));
                tableRows = new HtmlTableRow();
                headerColumn = new HtmlTableCell();
                headerColumn.Controls.Add(tableColumnHeadersSelectbox);
                headerColumn.VAlign = "middle";
                headerColumn.Align = "middle";
                headerColumn.Height = "32px";
                headerColumn.Style.Add("background-color", "#AED6F8");
                tableRows.Cells.Add(headerColumn);

                dataColumn = new HtmlTableCell();
                dataColumn.Style.Add("font-family", "Verdana, Arial, Helvetica, sans-serif");
                dataColumn.Style.Add("font-size", "10px");
                dataColumn.Style.Add("background-color", "#AED6F8");
                Label tableDataColumnLabel = new Label();


                tableDataColumnLabel.Text =
                string.Format("<font color='black'>{0}</font>",
                ECN_Framework_Common.Functions.StringFunctions.Left(textData, textData.Length > 100 ? 100 : textData.Length) + ((textData.Length > 100) ? "<font color=orange><b>&nbsp;&nbsp;&nbsp;more...</b></font>" : " "));
                dataColumn.Controls.Add(tableDataColumnLabel);
                tableRows.Cells.Add(dataColumn);

                dataCollectionTable.Rows.Add(tableRows);
            }
            StringWriter writer = new StringWriter();
            Html32TextWriter htmlWriter = new Html32TextWriter(writer);
            dataCollectionTable.RenderControl(htmlWriter);
            return writer.ToString();
        }
        private HtmlSelect buildColumnHeaderDropdowns(string selectBoxName, string colName, int gid)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(gid, CurrentUser);

            EmailTableColumnManager ColumnManager = new EmailTableColumnManager();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in GroupDataFieldsList)
            {
                ColumnManager.AddGroupDataFields(groupDataFields.ShortName);
            }
            ArrayList columnHeaderSelect = new ArrayList();
            for (int i = 0; i < ColumnManager.ColumnCount; i++)
            {
                columnHeaderSelect.Insert(i, ColumnManager.GetColumnNameByIndex(i));
            }
            columnHeaderSelect.Sort();

            return new SelectBuilder(selectBoxName)
                .Bind(columnHeaderSelect)
                .SelectItems(colName)
                .Build();
        }
        private string getPhysicalPath()
        {
            string DataPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + CurrentUser.CustomerID + "/data");

            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
            return DataPath;
        }
        private Hashtable GetGroupDataFields(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, CurrentUser);
            Hashtable fields = new Hashtable();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
                fields.Add("user_" + groupDataFields.ShortName.ToLower(), groupDataFields.GroupDataFieldsID);
            return fields;
        }
        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            return text;
        }
        private void UpdateToDB(int CustomerID, int GroupID, string xmlProfile, string xmlUDF, bool EmailaddressOnly, Hashtable hUpdatedRecords, string ftc, string stc, string file, string lblGUID)
        {
            DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(CurrentUser, CurrentUser.CustomerID, GroupID, xmlProfile, xmlUDF, ftc, stc, EmailaddressOnly, file + " " + lblGUID, "Ecn.communicator.main.lists.importDatafromFile.UpdateToDB");
            if (dtRecords.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRecords.Rows)
                {
                    if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                        hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                    else
                    {
                        int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                        hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                    }
                }

            }
        }
        private string CreateNote(string gid, string file)
        {
            StringBuilder sbNote = new StringBuilder();
            sbNote.AppendLine("Current Customer: " + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID.ToString());
            sbNote.AppendLine("Current User: " + CurrentUser.UserID.ToString());
            sbNote.AppendLine("Group ID: " + gid);
            sbNote.AppendLine("File Name: " + file);

            return sbNote.ToString();
        }
        #endregion

        #region Clean Emails
        public ActionResult EmailVerifier()
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.CleanEmails))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

            return View();
        }

        public ActionResult CleanEmails(string GL, int groupId, string VT)
        {
            int gid = 0;
            List<string> response = new List<string>();

            if (GL.Equals("M"))
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Convert.ToInt32(CurrentUser.CustomerID), CurrentUser);
                gid = group.GroupID;
            }
            else
            {
                gid = groupId;
            }

            if (gid < 1)
            {
                response.Add("500");
                response.Add("Please select a Group.");
                return Json(response);
            }
            else
            {
                int bademails = 0;
                Server.ScriptTimeout = 360;
                string res = string.Empty;

                if (VT == "syntax")
                {
                    bademails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidateEmails(gid, CurrentUser.UserID);
                    res = "<span class=errormsg>Process complete. " + bademails.ToString() + " emails marked as bad</span>";
                }

                if (VT == "delete")
                {
                    bademails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.DeleteBadEmails(gid, CurrentUser.UserID, CurrentUser);
                    res = "<span class=errormsg>Process Complete. " + bademails.ToString() + " emails deleted</span>";
                }

                response.Add("200");
                response.Add(res);
                return Json(response);
            }
        }
        #endregion

        #region Suppression 
        public ActionResult Suppression()
        {
            GroupWrapper Model = new GroupWrapper();

            ECN_Framework_Entities.Communicator.Group group =
                ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(CurrentUser.CustomerID, CurrentUser);

            Model.group = new Group(group);
            Model.group.Emails = new List<Email>();
            string ProfileName = "";
            string emailAdd = ECN_Framework_Common.Functions.StringFunctions.CleanString(ProfileName);
            emailAdd = emailAdd.Replace("_", "[_]").Replace("'", "''");
            string searchEmailLike = "contains";
            string filter = "";

            string sortColumn = "EmailAddress"; // lstgs[0].SortColumnName;
            string sortdirection = "ASC";


            string subscribeType = "*";


            try
            {
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();

                DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, group.GroupID, 1, 15, fromDate, toDate, false, filter, sortColumn, sortdirection);

                //DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, GroupID, 1, 20000, filter);
                DataTable emailstable = emailsListDS.Tables[1];
                List<Models.Email> list = new List<Models.Email>();

                list = emailstable.DataTableToList<Models.Email>();

                Model.group.Emails = list;
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ecn.communicator.mvc.Controllers.GroupController.Suppression", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                return PartialView("Partials/_ErrorNotification");
            }
            Model.SubscribeTypeCodes = LoadDropDowns(CurrentUser, Model.group.GroupID);

            return View(Model);
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
        // Domain Suppresion
        public ActionResult AddDomainSuppression(int DomainSuppressionID, string RbType, string TxtDomain)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_Entities.Communicator.DomainSuppression domainSuppression = new ECN_Framework_Entities.Communicator.DomainSuppression();
                if (RbType == "Channel")
                {
                    domainSuppression.BaseChannelID = Convert.ToInt32(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID);
                }
                else
                {
                    domainSuppression.CustomerID = Convert.ToInt32(CurrentUser.CustomerID);
                }
                domainSuppression.DomainSuppressionID = DomainSuppressionID;
                domainSuppression.IsActive = true;
                domainSuppression.Domain = TxtDomain;
                if (DomainSuppressionID > 0)
                    domainSuppression.UpdatedUserID = CurrentUser.UserID;
                else
                    domainSuppression.CreatedUserID = CurrentUser.UserID;

                ECN_Framework_BusinessLayer.Communicator.DomainSuppression.Save(domainSuppression, CurrentUser);

                List<ECN_Framework_Entities.Communicator.DomainSuppression> domainSuppressionList = new List<ECN_Framework_Entities.Communicator.DomainSuppression>();
                var CurrentCustomer = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer;
                if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DomainSuppression, KMPlatform.Enums.Access.Edit))
                {
                    domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain("", CurrentCustomer.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, CurrentUser);
                }
                else
                {
                    domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain("", CurrentCustomer.CustomerID, null, CurrentUser);
                }

                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Domain_Grid", domainSuppressionList));
                return Json(response);
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
        }

        public ActionResult LoadAddDomainSuppression()
        {
            return PartialView("Partials/Modals/_AddDomainSuppression", new ECN_Framework_Entities.Communicator.DomainSuppression());
        }

        public ActionResult LoadEditDomainSuppression(int DomainSuppressionID)
        {
            ECN_Framework_Entities.Communicator.DomainSuppression ds = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomainSuppressionID(DomainSuppressionID, CurrentUser);
            return PartialView("Partials/Modals/_EditDomainSuppression", ds);
        }
        public ActionResult SetEditDomainSuppression(int DomainSuppressionID)
        {
            List<string> response = new List<string>();
            ECN_Framework_Entities.Communicator.DomainSuppression domainSupression = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomainSuppressionID(DomainSuppressionID, CurrentUser);
            response.Add("200");
            response.Add(domainSupression.Domain);
            if (domainSupression.BaseChannelID != null)
            {
                response.Add("Channel");
            }
            else
            {
                response.Add("Customer");
            }
            response.Add(DomainSuppressionID.ToString());
            response.Add("Edit Domain Suppression");

            return Json(response);
        }

        public ActionResult DeleteDomainSuppression(int Id)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.DomainSuppression.Delete(Id, CurrentUser);

                List<ECN_Framework_Entities.Communicator.DomainSuppression> domainSuppressionList = new List<ECN_Framework_Entities.Communicator.DomainSuppression>();
                var CurrentCustomer = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer;
                if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DomainSuppression, KMPlatform.Enums.Access.Edit))
                {
                    domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain("", CurrentCustomer.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, CurrentUser);
                }
                else
                {
                    domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain("", CurrentCustomer.CustomerID, null, CurrentUser);
                }
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Domain_Grid", domainSuppressionList));
                return Json(response);
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
        }

        public ActionResult SearchDomainSuppression(string SearchDomain)
        {
            List<string> response = new List<string>();
            try
            {
                List<ECN_Framework_Entities.Communicator.DomainSuppression> domainSuppressionList = new List<ECN_Framework_Entities.Communicator.DomainSuppression>();
                var CurrentCustomer = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer;
                if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DomainSuppression, KMPlatform.Enums.Access.Edit))
                {
                    domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain(SearchDomain.Replace("'", "''"), CurrentCustomer.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, CurrentUser);
                }
                else
                {
                    domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain(SearchDomain.Replace("'", "''"), CurrentCustomer.CustomerID, null, CurrentUser);
                }
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Domain_Grid", domainSuppressionList));
                return Json(response);
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
        }

        // Channel Master Suppresion
        public ActionResult AddChannelSuppression(string emailAddressToAdd)
        {
            List<string> response = new List<string>();
            int emailsAdded = 0;
            StringBuilder xmlInsert = new StringBuilder();
            xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
            DateTime startDateTime = DateTime.Now;

            Hashtable hUpdatedRecords = new Hashtable();

            if (emailAddressToAdd.Length > 0)
            {
                emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",");
                emailAddressToAdd = emailAddressToAdd.Replace("\n", ",");
                StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                while (st.HasMoreTokens())
                {
                    xmlInsert.Append("<ea>" + st.NextToken().Trim() + "</ea>");
                }

                xmlInsert.Append("</XML>");
                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsToCS(CurrentUser, CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());
                if (emailRecordsDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in emailRecordsDT.Rows)
                    {
                        if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                            hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                        else
                        {
                            int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                            hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                        }
                    }
                }

                if (hUpdatedRecords.Count > 0)
                {
                    DataTable dtRecords = new DataTable();

                    dtRecords.Columns.Add("Description");
                    dtRecords.Columns.Add("Totals");
                    dtRecords.Columns.Add("sortOrder");

                    DataRow row;

                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        row = dtRecords.NewRow();

                        if (de.Key.ToString() == "T")
                        {
                            row["Description"] = "Total Records in the File";
                            row["sortOrder"] = 1;
                        }
                        else if (de.Key.ToString() == "I")
                        {
                            row["Description"] = "New";
                            row["sortOrder"] = 2;
                        }
                        else if (de.Key.ToString() == "U")
                        {
                            row["Description"] = "Changed";
                            row["sortOrder"] = 3;
                        }
                        else if (de.Key.ToString() == "D")
                        {
                            row["Description"] = "Duplicate(s)";
                            row["sortOrder"] = 4;
                        }
                        else if (de.Key.ToString() == "S")
                        {
                            row["Description"] = "Skipped";
                            row["sortOrder"] = 5;
                        }
                        row["Totals"] = de.Value;
                        dtRecords.Rows.Add(row);
                    }

                    row = dtRecords.NewRow();
                    row["Description"] = "&nbsp;";
                    row["Totals"] = " ";
                    row["sortOrder"] = 8;
                    dtRecords.Rows.Add(row);

                    TimeSpan duration = DateTime.Now - startDateTime;

                    row = dtRecords.NewRow();
                    row["Description"] = "Time to Import";
                    row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                    row["sortOrder"] = 9;
                    dtRecords.Rows.Add(row);

                    DataView dv = dtRecords.DefaultView;
                    dv.Sort = "sortorder asc";
                    DataTable sortedDT = dv.ToTable();
                    sortedDT.Columns.RemoveAt(2);

                    string dtHtml = "<table border=\"1\" cellpadding=\"5\"><thead><tr>";
                    foreach (System.Data.DataColumn col in sortedDT.Columns)
                    {
                        dtHtml += "<th>" + col.Caption + "</th>";
                    }
                    dtHtml += "</tr></thead><tbody>";
                    foreach (System.Data.DataRow r in sortedDT.Rows)
                    {
                        dtHtml += "<tr>";
                        foreach (var cell in r.ItemArray)
                        {
                            dtHtml += "<td>" + cell.ToString() + "</td>";
                        }
                        dtHtml += "</tr>";
                    }
                    dtHtml += "</tbody></table>";
                    response.Add("200");
                    response.Add(dtHtml);
                }
                else
                {
                    response.Add("200");
                    response.Add("<font face=verdana size=2 color=#000000>&nbsp;" + emailsAdded.ToString() + " rows updated/inserted </font>");
                }
            }
            else
            {
                response.Add("200");
                response.Add("<font face=verdana size=2 color=#000000>&nbsp;" + emailsAdded.ToString() + " rows updated/inserted </font>");
            }

            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
            channelSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByBaseChannelID(CurrentBaseChannel.BaseChannelID, CurrentUser);
            channelSuppressionList = channelSuppressionList.OrderBy(o => o.EmailAddress).ToList();
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Channel_Grid", channelSuppressionList));
            return Json(response);
        }

        public ActionResult LoadAddChannelSuppression()
        {
            return PartialView("Partials/Modals/_AddChannelSuppression");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CS_ReadToGrid([DataSourceRequest]DataSourceRequest request, string ProfileName, int PageNumber, int PageSize)
        {
            DataSourceResult result;
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
            string sortColumn = "EmailAddress";
            string sortDirection = "ASC";

            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortDirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

            }

            channelSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByBaseChannelID_Paging(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, ProfileName, PageNumber, PageSize, sortColumn, sortDirection, CurrentUser);
            //channelSuppressionList = channelSuppressionList.OrderBy(o => o.EmailAddress).ToList();

            IQueryable<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> gs = channelSuppressionList.AsQueryable();
            result = gs.ToDataSourceResult(request);
            if (channelSuppressionList.Count > 0)
                result.Total = channelSuppressionList[0].TotalCount;
            else
            {
                result.Total = 0;
            }
            return Json(result);
        }
        public ActionResult DeleteChannelSuppression(int Id)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.Delete(CurrentBaseChannel.BaseChannelID, Id, CurrentUser);

                List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
                channelSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByBaseChannelID(CurrentBaseChannel.BaseChannelID, CurrentUser);
                channelSuppressionList = channelSuppressionList.OrderBy(o => o.EmailAddress).ToList();
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Channel_Grid", channelSuppressionList));
                return Json(response);
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
        }

        public ActionResult SearchChannelSuppression(string SearchChannel)
        {
            List<string> response = new List<string>();
            try
            {
                List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelSuppressionList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();
                var CurrentCustomer = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer;
                channelSuppressionList = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(CurrentBaseChannel.BaseChannelID, SearchChannel, CurrentUser);
                channelSuppressionList = channelSuppressionList.OrderBy(o => o.EmailAddress).ToList();
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Channel_Grid", channelSuppressionList));
                return Json(response);
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
        }

        public FileContentResult ExportChannelSuppresion(string searchChannel)
        {
            var channelMasterSuppressionList = _channelMasterSuppressionList.GetByEmailAddress(
                CurrentBaseChannel.BaseChannelID,
                searchChannel.Replace("'", "''"),
                CurrentUser);

            return ExportSupression(channelMasterSuppressionList, MasterSuppressedEmailCSVFileName);
        }

        // No Threshold Suppresion
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult NoT_ReadToGrid([DataSourceRequest]DataSourceRequest request, string ProfileName, int PageNumber, int PageSize)
        {
            DataSourceResult result;
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = new List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>();
            string sortColumn = "EmailAddress";
            string sortDirection = "ASC";

            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortDirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

            }

            channelNoThresholdList = ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByEmailAddress_Paging(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, ProfileName, PageNumber, PageSize, sortColumn, sortDirection, CurrentUser);

            IQueryable<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> gs = channelNoThresholdList.AsQueryable();
            result = gs.ToDataSourceResult(request);
            if (channelNoThresholdList.Count > 0)
                result.Total = channelNoThresholdList[0].TotalCount;
            else
            {
                result.Total = 0;
            }
            return Json(result);
        }
        public ActionResult LoadAddThresholdSuppression()
        {
            return PartialView("Partials/Modals/_AddThresholdSuppression");
        }

        public ActionResult AddNoThresholdSuppression(string emailAddressToAdd)
        {
            var hUpdatedRecords = new Hashtable();
            var response = new List<string>();
            var xmlInsert = new StringBuilder();
            xmlInsert.Append(DefaultXmlHeader);
            var startDateTime = DateTime.Now;
            var emailsAdded = 0;

            if (emailAddressToAdd.Length > 0)
            {
                SetEmailAddress(emailAddressToAdd, hUpdatedRecords, xmlInsert, ImportEmailsToNoThreshold);

                if (hUpdatedRecords.Count > 0)
                {
                    var dtRecords = AddDatableRows(hUpdatedRecords, startDateTime);
                    var dtHtml = ComposeJsonResponse(dtRecords);
                    response.Add(DefaultSuccessCode);
                    response.Add(dtHtml);
                }
                else
                {
                    AddRowsUpdatedInfoToResponse(response, emailsAdded);
                }
            }
            else
            {
                AddRowsUpdatedInfoToResponse(response, emailsAdded);
            }

            var baseChannelId = ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;
            var channelNoThresholdList = new List<CommChannelNoThresholdList>();
            channelNoThresholdList = ChannelNoThresholdList.GetByBaseChannelID(baseChannelId, CurrentUser);
            channelNoThresholdList = channelNoThresholdList.OrderBy(o => o.EmailAddress).ToList();

            response.Add(HtmlHelperMethods.RenderViewToString(ControllerContext, SuppressionThresholdGridView, channelNoThresholdList));
            return Json(response);
        }

        private static void AddRowsUpdatedInfoToResponse(List<string> response, int emailsAdded)
        {
            response.Add(DefaultSuccessCode);
            response.Add($"<font face=verdana size=2 color=#000000>&nbsp;{emailsAdded} rows updated/inserted </font>");
        }

        public ActionResult DeleteNoThresholdSuppression(int CNTID)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.DeleteByCNTID(CurrentBaseChannel.BaseChannelID, CNTID, CurrentUser);

                List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = new List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>();
                channelNoThresholdList = ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByBaseChannelID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, CurrentUser);
                channelNoThresholdList = channelNoThresholdList.OrderBy(o => o.EmailAddress).ToList();
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Threshold_Grid", channelNoThresholdList));
                return Json(response);
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
        }

        public ActionResult SearchNoThresholdSuppression(string SearchNoThreshold)
        {
            List<string> response = new List<string>();
            try
            {
                List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList = new List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>();
                var CurrentCustomer = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer;
                channelNoThresholdList = ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByEmailAddress(CurrentBaseChannel.BaseChannelID, SearchNoThreshold, CurrentUser);
                channelNoThresholdList = channelNoThresholdList.OrderBy(o => o.EmailAddress).ToList();
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Threshold_Grid", channelNoThresholdList));
                return Json(response);
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
        }

        public FileContentResult ExportNoThresholdSuppresion(string searchNoThreshold)
        {
            var channelNoThresholdList = _channelNoThresholdList.GetByEmailAddress(
                CurrentBaseChannel.BaseChannelID,
                searchNoThreshold.Replace("'", "''"),
                CurrentUser);

            var orderedChannelList = (from src in channelNoThresholdList
                                      orderby src.EmailAddress
                                      group src by src.EmailAddress into grp
                                      select new
                                      {
                                          EmailAddress = grp.Key,
                                          CreatedDate = grp.Max(t => t.CreatedDate)
                                      }).ToList();

            return ExportSupression(orderedChannelList, NoThresholdEmailCSVFileName);
        }

        private FileContentResult ExportSupression(IEnumerable<dynamic> result, string csvFileName)
        {
            var clientID = _currentBaseChannel.BaseChannelID.ToString();
            var fileName = string.Format("{0}{1}", clientID, csvFileName);

            var txtoutFilePath = _server.MapPath(
                string.Format("{0}/customers/{1}/downloads/", ConfigurationManager.AppSettings["Images_VirtualPath"], clientID));

            if (!_fileSystem.DirectoryExists(txtoutFilePath))
            {
                _fileSystem.CreateDirectory(txtoutFilePath);
            }

            var date = DateTime.Now;
            var outfileName = txtoutFilePath + fileName;

            if (_fileSystem.FileExists(outfileName))
            {
                _fileSystem.FileDelete(outfileName);
            }

            var txtfile = _fileSystem.FileAppendText(outfileName);
            txtfile.WriteLine("EmailAddress, DateAdded");

            var newline = string.Empty;
            if (result.Any())
            {
                foreach (var res in result)
                {
                    newline = string.Empty;
                    newline += res.EmailAddress + ", " + res.CreatedDate.ToString();
                    txtfile.WriteLine(newline);
                }
            }
            _fileSystem.CloseTextWriter(txtfile);
            _httpResponse.ContentType = "text/csv";
            _httpResponse.AddHeader("content-disposition", "attachment; filename=" + fileName);
            _httpResponse.WriteFile(outfileName);
            _httpResponse.Flush();
            _httpResponse.End();
            return null;
        }

        // Global Suppresion
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GS_ReadToGrid([DataSourceRequest]DataSourceRequest request, string searchTermValue, int PageNumber, int PageSize)
        {
            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalSuppresionList = new List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>();
            string sortColumn = "EmailAddress";
            string sortDirection = "ASC";

            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortDirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

            }


            globalSuppresionList = ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.Search_Paging(PageNumber, PageSize, searchTermValue, sortColumn, sortDirection, CurrentUser);



            IQueryable<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> gs = globalSuppresionList.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            if (globalSuppresionList.Count > 0)
                result.Total = globalSuppresionList[0].TotalCount;
            else
                result.Total = 0;
            return Json(result);
        }

        public ActionResult AddGlobalSuppression(string emailAddressToAdd)
        {
            var hUpdatedRecords = new Hashtable();
            var response = new List<string>();
            var xmlInsert = new StringBuilder();
            xmlInsert.Append(DefaultXmlHeader);
            var startDateTime = DateTime.Now;
            var emailsAdded = 0;

            if (emailAddressToAdd.Length > 0)
            {
                SetEmailAddress(emailAddressToAdd, hUpdatedRecords, xmlInsert, ImportEmailsToGlobalMs);

                if (hUpdatedRecords.Count > 0)
                {
                    var dtRecords = AddDatableRows(hUpdatedRecords, startDateTime);
                    var dtHtml = ComposeJsonResponse(dtRecords);
                    response.Add(DefaultSuccessCode);
                    response.Add(dtHtml);
                }
                else
                {
                    response.Add(DefaultSuccessCode);
                    response.Add($"<font face=verdana size=2 color=#000000>&nbsp;{emailsAdded} rows updated/inserted </font>");
                }
            }
            else
            {
                response.Add(DefaultSuccessCode);
                response.Add($"<font face=verdana size=2 color=#000000>&nbsp;{emailsAdded} rows updated/inserted </font>");
            }

            response.Add(HtmlHelperMethods.RenderViewToString(ControllerContext, SuppressionGlobalGridView, string.Empty));
            return Json(response);
        }

        private static string ComposeJsonResponse(DataTable dtRecords)
        {
            Guard.NotNull(dtRecords, nameof(dtRecords));

            var defaultView = dtRecords.DefaultView;
            defaultView.Sort = DefaultSortOrder;
            var sortedDt = defaultView.ToTable();
            sortedDt.Columns.RemoveAt(2);

            var dtHtml = new StringBuilder("<table border=\"1\" cellpadding=\"5\"><thead><tr>");
            foreach (DataColumn col in sortedDt.Columns)
            {
                dtHtml.Append($"<th>{col.Caption}</th>");
            }

            dtHtml.Append("</tr></thead><tbody>");
            foreach (DataRow row in sortedDt.Rows)
            {
                dtHtml.Append("<tr>");

                foreach (var cell in row.ItemArray)
                {
                    dtHtml.Append($"<td>{cell}</td>");
                }

                dtHtml.Append("</tr>");
            }

            dtHtml.Append("</tbody></table>");
            return dtHtml.ToString();
        }

        private static DataTable AddDatableRows(IEnumerable updatedRecords, DateTime startDateTime)
        {
            Guard.NotNull(updatedRecords, nameof(updatedRecords));
            Guard.NotNull(startDateTime, nameof(startDateTime));

            var dtRecords = new DataTable();
            dtRecords.Columns.Add(DescriptionKey);
            dtRecords.Columns.Add(TotalsKey);
            dtRecords.Columns.Add(SortOrderKey);

            DataRow row;
            var duration = DateTime.Now - startDateTime;

            foreach (DictionaryEntry de in updatedRecords)
            {
                row = dtRecords.NewRow();

                switch (de.Key.ToString())
                {
                    case TotalRecordsCode:
                        row[DescriptionKey] = TotalRecordsDescription;
                        row[SortOrderKey] = 1;
                        break;
                    case NewCode:
                        row[DescriptionKey] = NewDescription;
                        row[SortOrderKey] = 2;
                        break;
                    case ChangedCode:
                        row[DescriptionKey] = ChangedDescription;
                        row[SortOrderKey] = 3;
                        break;
                    case DuplicateCode:
                        row[DescriptionKey] = DuplicateDescription;
                        row[SortOrderKey] = 4;
                        break;
                    case SkippedCode:
                        row[DescriptionKey] = SkippedDescription;
                        row[SortOrderKey] = 5;
                        break;
                }
                row[TotalsKey] = de.Value;
                dtRecords.Rows.Add(row);
            }

            row = dtRecords.NewRow();
            row[DescriptionKey] = "&nbsp;";
            row[TotalsKey] = " ";
            row[SortOrderKey] = 8;
            dtRecords.Rows.Add(row);

            row = dtRecords.NewRow();
            row[DescriptionKey] = "Time to Import";
            row[TotalsKey] = $"{duration.Hours}:{duration.Minutes}:{duration.Seconds}";
            row[SortOrderKey] = 9;
            dtRecords.Rows.Add(row);

            return dtRecords;
        }

        private string SetEmailAddress(string emailAddressToAdd, Hashtable hUpdatedRecords, StringBuilder xmlInsert, string importType)
        {
            emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",");
            emailAddressToAdd = emailAddressToAdd.Replace("\n", ",");
            var stringTokenizer = new StringTokenizer(emailAddressToAdd, ',');

            while (stringTokenizer.HasMoreTokens())
            {
                xmlInsert.Append($"<ea>{stringTokenizer.NextToken().Trim()}</ea>");
            }

            xmlInsert.Append("</XML>");

            var emailRecordsDt = SetEmailRecordDataTable(xmlInsert, importType);
            if (emailRecordsDt.Rows.Count > 0)
            {
                foreach (DataRow row in emailRecordsDt.Rows)
                {
                    if (!hUpdatedRecords.Contains(row["Action"].ToString()))
                    {
                        hUpdatedRecords.Add(row["Action"].ToString().ToUpper(), Convert.ToInt32(row["Counts"]));
                    }
                    else
                    {
                        var eTotal = Convert.ToInt32(hUpdatedRecords[row["Action"].ToString().ToUpper()]);
                        hUpdatedRecords[row["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(row["Counts"]);
                    }
                }
            }

            return emailAddressToAdd;
        }

        private DataTable SetEmailRecordDataTable(StringBuilder xmlInsert, string importType)
        {
            if (importType.Equals(ImportEmailsToGlobalMs))
            {
                return EmailGroup.ImportEmailsToGlobalMS(CurrentUser, CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());
            }
            if (importType.Equals(ImportEmailsToNoThreshold))
            {
                return EmailGroup.ImportEmailsToNoThreshold(CurrentUser, CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());
            }

            return null;
        }

        public ActionResult DeleteGlobalSuppression(int Id)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.Delete(Id, CurrentUser);

                List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalSuppresionList = new List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>();
                globalSuppresionList = ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetAll(CurrentUser);
                globalSuppresionList = globalSuppresionList.OrderBy(o => o.EmailAddress).ToList();
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Global_Grid", ""));
                return Json(response);
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
        }

        public ActionResult DeleteMasterSuppression(int Id)
        {
            List<string> response = new List<string>();
            GroupWrapper Model = new GroupWrapper();

            try
            {
                ECN_Framework_Entities.Communicator.Group group =
                    ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(CurrentUser.CustomerID, CurrentUser);

                ECN_Framework_BusinessLayer.Communicator.EmailGroup.DeleteFromMasterSuppressionGroup(group.GroupID, Id, ConvenienceMethods.GetCurrentUser());

                //Model.group = new Group(group);
                //Model.group.Emails = new List<Email>();
                //string ProfileName = "";
                //string emailAdd = ECN_Framework_Common.Functions.StringFunctions.CleanString(ProfileName);
                //emailAdd = emailAdd.Replace("_", "[_]").Replace("'", "''");
                //string searchEmailLike = "contains";
                //string filter = "";

                //string sortColumn = "EmailAddress"; // lstgs[0].SortColumnName;
                //string sortdirection = "ASC";
                //string subscribeType = "*";

                //try
                //{
                //    DateTime fromDate = new DateTime();
                //    DateTime toDate = new DateTime();

                //    DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, group.GroupID, 1, 15, fromDate, toDate, false, filter, sortColumn, sortdirection);

                //    //DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(CurrentUser.CustomerID, GroupID, 1, 20000, filter);
                //    DataTable emailstable = emailsListDS.Tables[1];
                //    List<Models.Email> list = new List<Models.Email>();

                //    list = emailstable.DataTableToList<Models.Email>();

                //    Model.group.Emails = list;
                //}
                //catch (Exception ex)
                //{
                //    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ecn.communicator.mvc.Controllers.GroupController.Suppression", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                //    return PartialView("Partials/_ErrorNotification");
                //}
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
            //response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Master_Grid", Model.group));
            return Json(response);
        }

        public ActionResult SearchGlobalSuppression(string SearchGlobal)
        {
            List<string> response = new List<string>();
            try
            {
                //List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalSuppresionList = new List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>();
                //var CurrentCustomer = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer;
                //globalSuppresionList = ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(SearchGlobal, CurrentUser);
                //globalSuppresionList = globalSuppresionList.OrderBy(o => o.EmailAddress).ToList();
                response.Add("200");
                response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_Suppresion_Global_Grid", SearchGlobal));
                return Json(response);
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
        }

        public FileContentResult ExportGlobalSuppresion(string SearchGlobal)
        {
            string newline = "";
            string clientID = CurrentBaseChannel.BaseChannelID.ToString();
            string fileName = clientID + "_MasterSuppressedGlobal_Emails.CSV";

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + clientID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            DateTime date = DateTime.Now;
            string outfileName = txtoutFilePath + fileName;

            if (System.IO.File.Exists(outfileName))
            {
                System.IO.File.Delete(outfileName);
            }

            TextWriter txtfile = System.IO.File.AppendText(outfileName);
            txtfile.WriteLine("EmailAddress, DateAdded");
            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
            ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(SearchGlobal.Replace("'", "''"), CurrentUser);
            if (globalMasterSuppressionList_List.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList GlobalMasterSuppressionList in globalMasterSuppressionList_List)
                {
                    newline = "";
                    newline += GlobalMasterSuppressionList.EmailAddress + ", " + GlobalMasterSuppressionList.CreatedDate;
                    txtfile.WriteLine(newline);
                }
            }
            txtfile.Close();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
            return null;
        }
        #endregion

        #region Group Config
        public ActionResult GroupConfig()
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupConfig, KMPlatform.Enums.Access.View))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

            return View(ECN_Framework_BusinessLayer.Communicator.GroupConfig.GetByCustomerID(CurrentUser.CustomerID, CurrentUser));
        }

        public ActionResult DeleteGroupConfig(int Id)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.GroupConfig.Delete(Id, CurrentUser);
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
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_GroupConfigGrid",
                ECN_Framework_BusinessLayer.Communicator.GroupConfig.GetByCustomerID(CurrentUser.CustomerID, CurrentUser)));
            return Json(response);
        }

        public ActionResult AddGroupConfig(string ShortName, string IsPublic)
        {
            ECN_Framework_Entities.Communicator.GroupConfig grpConfig = new ECN_Framework_Entities.Communicator.GroupConfig();
            grpConfig.CreatedUserID = CurrentUser.UserID;
            grpConfig.CustomerID = CurrentUser.CustomerID;
            grpConfig.ShortName = ShortName.Trim().Replace(" ", "_");
            grpConfig.IsPublic = IsPublic;

            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.GroupConfig.Save(grpConfig, CurrentUser);
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
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_GroupConfigGrid",
                ECN_Framework_BusinessLayer.Communicator.GroupConfig.GetByCustomerID(CurrentUser.CustomerID, CurrentUser)));
            return Json(response);
        }
        #endregion

        #region Email Search 
        public ActionResult EmailSearch()
        {
            if (!KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailSearch, KMPlatform.Enums.Access.FullAccess))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

            return View();
        }

        public ActionResult LoadEmailGrid(string FilterTypeValue, string searchTermValue)
        {
            List<string> searchParams = new List<string> { FilterTypeValue, searchTermValue };
            return PartialView("Partials/_EmailSearchGrid");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EmailSearchReadToGrid([DataSourceRequest]DataSourceRequest request, string FilterTypeValue, string searchTermValue, int pageNumber, int pageSize)
        {
            StringBuilder sbChannels = new StringBuilder();
            StringBuilder sbCustomers = new StringBuilder();
            List<ECN_Framework_Entities.Communicator.EmailSearch> emailSearches = new List<ECN_Framework_Entities.Communicator.EmailSearch>();

            if (KM.Platform.User.IsSystemAdministrator(CurrentUser)) { sbChannels.Append("0"); sbCustomers.Append("0"); }
            else if (KM.Platform.User.IsChannelAdministrator(CurrentUser))
            {
                KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();

                foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in CurrentUser.UserClientSecurityGroupMaps)
                {
                    KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false, false);
                    if (sg.AdministrativeLevel == KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator)
                    {
                        sbChannels.Append(ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(sg.ClientGroupID).BaseChannelID.ToString() + ",");
                    }
                }
                sbCustomers.Append("0");
            }
            else if (CurrentUser.CurrentSecurityGroup.ClientGroupID > 0)
            {
                sbChannels.Append(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID.ToString());
                sbCustomers.Append("0");
            }
            else
            {
                sbChannels.Append("0");
                sbCustomers.Append(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID.ToString());
            }


            string sortColumn = "EmailAddress";
            string sortDirection = "ASC";
            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortDirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

                if (sortColumn.ToLower().Equals("datecreated"))
                    sortColumn = "datecreated";
                else if (sortColumn.ToLower().Equals("datemodified"))
                    sortColumn = "datemodified";
            }
            DataTable emails = ECN_Framework_BusinessLayer.Communicator.Email.EmailSearch(FilterTypeValue, searchTermValue, CurrentUser, sbChannels.ToString().TrimEnd(','), sbCustomers.ToString(), pageNumber, pageSize, sortColumn, sortDirection);

            emailSearches = ConvertToClass(emails);


            IQueryable<ECN_Framework_Entities.Communicator.EmailSearch> gs = emailSearches.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            return Json(result);
        }

        public FileContentResult ExportReport(string filterType, string searchfor, string exportFormat)
        {
            StringBuilder sbChannels = new StringBuilder();
            StringBuilder sbCustomers = new StringBuilder();
            if (KM.Platform.User.IsSystemAdministrator(CurrentUser)) { sbChannels.Append("0"); sbCustomers.Append("0"); }
            else if (KM.Platform.User.IsChannelAdministrator(CurrentUser))
            {
                KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();

                foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in CurrentUser.UserClientSecurityGroupMaps)
                {
                    KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false, false);
                    if (sg.AdministrativeLevel == KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator)
                    {
                        sbChannels.Append(ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(sg.ClientGroupID).BaseChannelID.ToString() + ",");
                    }
                }
                sbCustomers.Append("0");
            }
            else if (CurrentUser.CurrentSecurityGroup.ClientGroupID > 0)
            {
                sbChannels.Append(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID.ToString());
                sbCustomers.Append("0");
            }
            else
            {
                sbChannels.Append("0");
                sbCustomers.Append(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID.ToString());
            }

            int currentPage = 1;
            int pageSize = 1000000;
            string sortColumn = "EmailAddress";
            string sortDirection = "ASC";

            DataTable emails = ECN_Framework_BusinessLayer.Communicator.Email.EmailSearch(filterType, searchfor, CurrentUser, sbChannels.ToString().TrimEnd(','), sbCustomers.ToString(), currentPage, pageSize, sortColumn, sortDirection);
            List<ECN_Framework_Entities.Communicator.EmailSearch> emailSearches = ConvertToClass(emails);

            List<ECN_Framework_Entities.Communicator.EmailSearchCSV> EmailSearchCSV_List = new List<ECN_Framework_Entities.Communicator.EmailSearchCSV>();
            foreach (ECN_Framework_Entities.Communicator.EmailSearch email in emailSearches)
            {
                ECN_Framework_Entities.Communicator.EmailSearchCSV eCSV = new ECN_Framework_Entities.Communicator.EmailSearchCSV();
                eCSV.EmailAddress = email.EmailAddress;
                eCSV.BaseChannelName = email.BaseChannelName;
                eCSV.CustomerName = email.CustomerName;
                eCSV.GroupName = email.GroupName;
                eCSV.Subscribe = email.Subscribe;
                eCSV.DateAdded = email.DateCreated.HasValue ? email.DateCreated.Value.ToShortDateString() + " " + email.DateCreated.Value.ToShortTimeString() : "";
                eCSV.DateModified = email.DateModified.HasValue ? email.DateModified.Value.ToShortDateString() + " " + email.DateModified.Value.ToShortTimeString() : "";
                EmailSearchCSV_List.Add(eCSV);
            }

            string tfile = CurrentUser.CustomerID + "_EmailSearch";

            switch (exportFormat.ToUpper())
            {
                case "CSV":
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportCSV(EmailSearchCSV_List, tfile);
                    break;
                case "XLS":
                case "TXT":
                    ReportViewer ReportViewer1 = new ReportViewer();
                    string mappath = Server.MapPath("~/bin/ECN_Framework_Common.dll");
                    Assembly assembly = Assembly.LoadFrom(Server.MapPath("~/bin/ECN_Framework_Common.dll"));
                    Stream stream = assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_EmailSearch.rdlc");
                    ReportViewer1.LocalReport.LoadReportDefinition(stream);
                    Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DS_EmailSearch", emailSearches);
                    ReportViewer1.Visible = false;
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.Refresh();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    String mimeType = null;
                    String encoding = null;
                    String extension = null;
                    Byte[] bytes = null;

                    switch (exportFormat.ToUpper())
                    {
                        case "PDF":
                            bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                            Response.ContentType = "application/pdf";
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment; filename=EmailSearch." + exportFormat);
                            Response.BinaryWrite(bytes);
                            Response.End();
                            break;
                        case "XLS":
                            bytes = ReportViewer1.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                            Response.ContentType = "application/vnd.ms-excel";
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment; filename=EmailSearch." + exportFormat);
                            Response.BinaryWrite(bytes);
                            Response.End();
                            break;
                        case "TXT":
                            string attachment = "attachment; filename=" + CurrentUser.CustomerID + "_EmailSearch.txt";

                            Response.Clear();
                            Response.ClearHeaders();
                            Response.ClearContent();
                            Response.AddHeader("content-disposition", attachment);
                            Response.ContentType = "text/csv";
                            Response.AddHeader("Pragma", "public");
                            Response.Write(ConvertListToString(emailSearches));
                            Response.End();
                            break;
                    }
                    break;
            }
            return null;
        }

        protected List<ECN_Framework_Entities.Communicator.EmailSearch> ConvertToClass(DataTable emails)
        {
            List<ECN_Framework_Entities.Communicator.EmailSearch> emailSearches = new List<ECN_Framework_Entities.Communicator.EmailSearch>();
            foreach (DataRow email in emails.Rows)
            {
                ECN_Framework_Entities.Communicator.EmailSearch es = new ECN_Framework_Entities.Communicator.EmailSearch();
                es.TotalRowsCount = Convert.ToInt32(email["TotalCount"]);
                es.BaseChannelName = email["BaseChannelName"].ToString();
                es.CustomerName = email["CustomerName"].ToString();
                es.GroupName = email["GroupName"].ToString();
                es.EmailAddress = email["EmailAddress"].ToString();
                es.Subscribe = email["SubscribeTypeCode"].ToString();
                if (!string.IsNullOrWhiteSpace(email["DateCreated"].ToString()))
                {
                    try
                    {
                        es.DateCreated = DateTime.Parse(email["DateCreated"].ToString());
                    }
                    catch { }
                }
                if (!string.IsNullOrWhiteSpace(email["DateModified"].ToString()))
                {
                    try
                    {
                        es.DateModified = DateTime.Parse(email["DateModified"].ToString());
                    }
                    catch { }
                }
                emailSearches.Add(es);
            }
            return emailSearches;
        }

        protected static string ConvertListToString(IEnumerable<ECN_Framework_Entities.Communicator.EmailSearch> emails)
        {
            const char DataSeparator = ',';
            string LineSeparator = Environment.NewLine;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Base Channel Name");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Customer Name");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Group Name");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Email Address");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Subscribe");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Date Added");
            stringBuilder.Append(DataSeparator);
            stringBuilder.Append("Date Modified");
            stringBuilder.Append(LineSeparator);

            // Build the usesrs string.
            foreach (ECN_Framework_Entities.Communicator.EmailSearch EmailSearch in emails)
            {
                stringBuilder.Append(EmailSearch.BaseChannelName);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.CustomerName);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.GroupName);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.EmailAddress);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.Subscribe);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.DateCreated);
                stringBuilder.Append(DataSeparator);
                stringBuilder.Append(EmailSearch.DateModified);
                stringBuilder.Append(LineSeparator);
            }

            // Remove trailing separator.
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }
        #endregion

        #region Update Email
        public ActionResult UpdateEmailAddress()
        {
            return View();
        }

        public ActionResult UpdateEmail(string oldEmailAddress, string newEmailAddress)
        {
            List<string> response = new List<string>();

            try
            {
                if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(oldEmailAddress.Trim()))
                {
                    if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(newEmailAddress.Trim()))
                    {
                        string isEmailSuppressedString = isEmailSuppressed(newEmailAddress.Trim(), CurrentUser);
                        if (!string.IsNullOrEmpty(isEmailSuppressedString))
                        {
                            response.Add("500");
                            response.Add(isEmailSuppressedString);
                            return Json(response);
                        }

                        ECN_Framework_BusinessLayer.Communicator.Email.UpdateEmail_BaseChannel(oldEmailAddress.Trim(), newEmailAddress.Trim(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, CurrentUser.UserID, "ecn.communicator.UpdateEmailAddress");
                        response.Add("200");
                        response.Add("Email Address updated");
                    }
                    else
                    {
                        response.Add("500");
                        response.Add("New Email Address is not a valid Email");
                    }
                }
                else
                {
                    response.Add("500");
                    response.Add("Old Email Address is not a valid Email");
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
            }
            return Json(response);
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
        #endregion

        #region Group Explorer        
        public ActionResult GetCustomers()
        {
            List<ECN_Framework_Entities.Accounts.Customer> listCust = new List<ECN_Framework_Entities.Accounts.Customer>();

            ECN_Framework_Entities.Accounts.Customer cust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CurrentUser.CustomerID, false);

            listCust.Add(cust);

            string AccessKey = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.AccessKey.ToString();

            IEnumerable<KMManagers.APITypes.Customer> customers = from c in listCust
                                                                  where c.IsDeleted.HasValue ? c.IsDeleted.Value == false : c.IsDeleted.HasValue == false && c.ActiveFlag.ToLower().Equals("y")
                                                                  select new KMManagers.APITypes.Customer { CustomerID = c.CustomerID.ToString(), CustomerName = "Root", AccessKey = AccessKey };

            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGroups(int customerId, int? folderId, string searchType = "contains", string search = "", bool allFolders = false)
        {
            List<ECN_Framework_Entities.Communicator.Group> groups;

            if (folderId.HasValue)
            {
                groups = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupSearch(search, allFolders ? null : folderId, customerId, CurrentUser, false);
            }
            else
                groups = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupSearch(search, 0, customerId, CurrentUser, false); // manager.GetRootGroupsByCustomerID(ApiKey, customerId);

            IEnumerable<Group> GroupIEnum = from g in groups
                                            where g.Archived.Value == false
                                            select new Group { GroupID = g.GroupID, CustomerID = g.CustomerID, FolderID = g.FolderID ?? 0, GroupName = g.GroupName, GroupDescription = g.GroupDescription, FolderName = g.FolderName }
                                           ;

            return Json(GroupIEnum.OrderBy(g => g.GroupName), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GE_ReadToGrid([DataSourceRequest]DataSourceRequest request, string GroupName, string searchType, int PageNumber, int PageSize, int folderID, string archiveFilter, string groupOrProfile, bool allFolders = false)
        {
            DataSourceResult result;

            string sortColumn = "GroupName";
            string sortDirection = "ASC";

            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                sortColumn = request.Sorts.First().Member;
                sortDirection = request.Sorts.First().SortDirection.ToString().ToLower().Equals("ascending") ? "ASC" : "DESC";// lstgs[0].SortDirection;

            }

            DataTable dtGroups = null;
            if (groupOrProfile.Equals("group"))
                dtGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(GroupName, searchType, CurrentUser.CustomerID, CurrentUser.UserID, folderID, PageNumber, PageSize, allFolders, archiveFilter, null, sortColumn, sortDirection);
            else if (groupOrProfile.Equals("profile"))
                dtGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(GroupName, searchType, CurrentUser.CustomerID, CurrentUser.UserID, folderID, PageNumber, PageSize, allFolders, archiveFilter, null, sortColumn, sortDirection);


            IQueryable<Group> gs = dtGroups.DataTableToListGroups().AsQueryable();// channelSuppressionList.AsQueryable();
            result = gs.ToDataSourceResult(request);
            if (dtGroups.Rows.Count > 0)
                result.Total = Convert.ToInt32(dtGroups.Rows[0]["TotalCount"].ToString());
            else
            {
                result.Total = 0;
            }
            return Json(result);
        }

        public ActionResult GetFolders(int customerId, string type, int? folderId)
        {
            //IEnumerable<ECN_Framework_Entities.Communicator.Folder> folders;
            List<ECN_Framework_Entities.Communicator.Folder> tempFolders = new List<ECN_Framework_Entities.Communicator.Folder>();
            if (folderId.HasValue && folderId.Value != 0)
                tempFolders = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(customerId, type, CurrentUser).Where(x => x.ParentID == folderId.Value).ToList();// manager.GetFoldersByCustomerIDAndParentID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.AccessKey.ToString(), customerId, folderId.Value);
            else
            {

                tempFolders = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(customerId, type, CurrentUser).Where(x => x.ParentID == 0).ToList();// manager.GetFoldersByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.AccessKey.ToString(), customerId);
            }


            var iEnumFolders = from f in tempFolders
                               select new KMManagers.APITypes.Folder { CreatedDate = f.CreatedDate, CreatedUserID = f.CreatedUserID.HasValue ? f.CreatedUserID.Value : 0, CustomerID = f.CustomerID.Value, CustomerName = "", FolderDescription = f.FolderDescription, FolderID = f.FolderID, FolderName = f.FolderName, FolderType = f.FolderType, ParentID = f.ParentID, UpdatedDate = f.UpdatedDate, UpdatedUserID = f.UpdatedUserID.HasValue ? f.UpdatedUserID.Value : -1 };
            return Json(iEnumFolders, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSingleCustomer(int customerID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> listCust = new List<ECN_Framework_Entities.Accounts.Customer>();

            ECN_Framework_Entities.Accounts.Customer cust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false);

            listCust.Add(cust);

            IEnumerable<KMManagers.APITypes.Customer> customerIEnum = from c in listCust
                                                                      where c.IsDeleted.HasValue ? c.IsDeleted.Value == false : c.IsDeleted.HasValue == false && c.ActiveFlag.ToLower().Equals("y")
                                                                      select new KMManagers.APITypes.Customer { CustomerID = c.CustomerID.ToString(), CustomerName = "Root" };

            return Json(customerIEnum, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}