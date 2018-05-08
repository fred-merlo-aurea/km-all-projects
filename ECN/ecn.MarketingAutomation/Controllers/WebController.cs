using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Kendo.Mvc.UI;

using Kendo.Mvc.Extensions;
using ecn.MarketingAutomation.Models;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;

namespace ecn.MarketingAutomation.Controllers
{
    public class WebController : Controller
    {
        private const string UTCDateStringFormat = "M/d/yyyy h:mm:ss tt";
        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }
        private ECN_Framework_BusinessLayer.Application.ECNSession CurrentSession
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession(); }
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
                               select new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FolderSelect { CreatedDate = f.CreatedDate, CreatedUserID = f.CreatedUserID.HasValue ? f.CreatedUserID.Value : 0, CustomerID = f.CustomerID.Value, CustomerName = "", FolderDescription = f.FolderDescription, FolderID = f.FolderID, FolderName = f.FolderName, FolderType = f.FolderType, ParentID = f.ParentID, UpdatedDate = f.UpdatedDate, UpdatedUserID = f.UpdatedUserID.HasValue ? f.UpdatedUserID.Value : -1 };
            return Json(iEnumFolders, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetGroups(int customerId, int? folderId, string search = "", bool allFolders = false)
        {
            List<ECN_Framework_Entities.Communicator.Group> groups;

            if (folderId.HasValue)
            {
                groups = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupSearch(search, allFolders ? null : folderId, customerId, CurrentUser, false);
            }
            else
                groups = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupSearch(search, 0, customerId, CurrentUser, false); // manager.GetRootGroupsByCustomerID(ApiKey, customerId);

            IEnumerable<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect> GroupIEnum = from g in groups
                                                                                                        where g.Archived.Value == false && (!g.IsSeedList.HasValue || g.IsSeedList.Value == false)
                                                                                                        select new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect { GroupID = g.GroupID, CustomerID = g.CustomerID, FolderID = g.FolderID ?? 0, GroupName = g.GroupName, GroupDescription = g.GroupDescription }
                                           ;

            return Json(GroupIEnum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMessages(int customerId, int? folderId, string search = "", bool allFolders = false)
        {
            List<ECN_Framework_Entities.Communicator.Layout> messages;

            if (folderId.HasValue)
            {
                messages = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutSearch(search, allFolders ? null : folderId, null, customerId, null, null, CurrentUser, false);
            }
            else
            {
                messages = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutSearch(search, 0, null, customerId, null, null, CurrentUser, false);
            }

            IEnumerable<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.MessageSelect> MessageIEnum = from g in messages
                                                                                                          where g.Archived.Value == false || g.Archived == null
                                                                                                            select new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.MessageSelect {
                                                                                                            MessageID = g.LayoutID,
                                                                                                            CustomerID = g.CustomerID.Value,
                                                                                                            FolderID = g.FolderID ?? 0,
                                                                                                            MessageName = g.LayoutName

                                                                                                            };
            return Json(MessageIEnum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMessageEnvelope(int messageID)
        {
            ECN_Framework_Entities.Communicator.Blast blast =
                ECN_Framework_BusinessLayer.Communicator.Blast.GetTopOneByLayoutID_NoAccessCheck(messageID, false);
            ecn.MarketingAutomation.Models.PostModels.ECN_Objects.MessageSelect ms = new Models.PostModels.ECN_Objects.MessageSelect();
            if (blast != null)
            {

                ms.FromEmail = blast.EmailFrom;
                ms.EmailSubject = blast.EmailSubject;
                ms.FromName = blast.EmailFromName;
                ms.ReplyTo = blast.ReplyTo;
            }
            return Json(ms, JsonRequestBehavior.AllowGet);
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

            DataSet formsListDS = ECN_Framework_BusinessLayer.FormDesigner.Form.GetBySearchStringPaging(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, CustomerID, FormType, FormStatus, FormName, SearchCriterion, 0, PageNumber, request.PageSize, sortDirection, sortColumn);
            int totalCount = 0;
            DataTable dt = formsListDS.Tables[0];
            foreach (DataRow dr in dt.AsEnumerable())
            {
                FormViewModel content = new FormViewModel();
                if (totalCount == 0)
                    totalCount = Convert.ToInt32(dr["TotalCount"].ToString());

                ECN_Framework_Entities.FormDesigner.Form  form = ECN_Framework_BusinessLayer.FormDesigner.Form.GetByID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID, Convert.ToInt32(dr["Form_Seq_ID"].ToString()));
                if(form!=null)
                { 
                    content.Name = form.Name;
                    content.Id = form.Form_Seq_ID;
                    content.FormType = form.FormType;
                    content.Status = form.Status;
                    content.CustomerName = form.CustomerName;
                    content.FormType = content.FormType == "AutoSubmit" ? "Auto-Submit" : content.FormType;
                    content.TotalRecordCounts = dr["TotalCount"].ToString();
                    listRange.Add(content);
                }
            }
            
            IQueryable<FormViewModel> gs = listRange.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCampaignItemTemplates(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> citemplates;
            citemplates = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCustomerID(customerID, CurrentUser, true, "active");
            Dictionary<int, List <ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect>> selectedGroupList = new Dictionary<int,List< ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect>>();
            Dictionary<int, List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect>> selectedGroupFilterList = new Dictionary<int, List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect>>();
            Dictionary<int, List <ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect>> SuppressionGroupList = new Dictionary<int, List< ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect>>();
            Dictionary<int, List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect>> SuppressionGroupFilterList = new Dictionary<int, List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect>>();
            Dictionary<int, string> MessageNameList = new Dictionary<int, string>();
            Dictionary<int, string> CampaignNameList = new Dictionary<int, string>();
            foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplate cit in citemplates)
            {
                List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect>  GroupList = new List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect>();
                List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect> GroupFilterList = new List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect>();
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup citg in cit.SelectedGroupList)
                {
                    if (!GroupList.Exists(x => x.GroupID == citg.GroupID))
                    {
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(citg.GroupID, CurrentUser);
                        if (group != null  && (!group.IsSeedList.HasValue || group.IsSeedList.Value == false))
                        {
                            GroupList.Add(new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect() { GroupID = group.GroupID, CustomerID = group.CustomerID, FolderID = group.FolderID ?? 0, GroupName = group.GroupName, GroupDescription = group.GroupDescription });
                            if (citg.Filters.Count > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in citg.Filters)
                                {
                                    if (cibf.FilterID.HasValue)
                                    {
                                        ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                                        GroupFilterList.Add(new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect() { FilterID = f.FilterID, CustomerID = f.CustomerID.Value, FilterName = f.FilterName, GroupID = f.GroupID.Value });
                                    }
                                }
                            }

                        }
                    }
                }
                selectedGroupList.Add(cit.CampaignItemTemplateID, GroupList);
                if (GroupFilterList==null)
                    selectedGroupFilterList.Add(cit.CampaignItemTemplateID, null);
                else
                {
                    selectedGroupFilterList.Add(cit.CampaignItemTemplateID, GroupFilterList);
                }
                if ((cit.LayoutID.HasValue) && (cit.LayoutID > 0))
                {
                    ECN_Framework_Entities.Communicator.Layout lp = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(cit.LayoutID ?? 0, false);
                    if (lp != null)
                    {
                        if (string.IsNullOrWhiteSpace(lp.LayoutName))
                        {
                            MessageNameList.Add(cit.CampaignItemTemplateID, "");
                        }
                        else
                        {
                            MessageNameList.Add(cit.CampaignItemTemplateID, lp.LayoutName);
                        }
                    }
                    else
                    {
                        MessageNameList.Add(cit.CampaignItemTemplateID, "");
                    }
                }
                else
                {
                    MessageNameList.Add(cit.CampaignItemTemplateID, "");
                }
                if ((cit.CampaignID.HasValue) && (cit.CampaignID > 0))
                {
                    ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID_NoAccessCheck(cit.CampaignID ?? 0, false);
                    if (c != null)
                    {
                        if (string.IsNullOrWhiteSpace(c.CampaignName))
                        {
                            CampaignNameList.Add(cit.CampaignItemTemplateID, "");
                        }
                        else
                        {
                            CampaignNameList.Add(cit.CampaignItemTemplateID, c.CampaignName);
                        }
                    }
                    else
                    {
                        CampaignNameList.Add(cit.CampaignItemTemplateID, "");
                    }
                }
                else
                {
                    CampaignNameList.Add(cit.CampaignItemTemplateID, "");
                }
                List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect> sGroupList = new List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect>();
                List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect> sGroupFilterList = new List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect>();
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup citg in cit.SuppressionGroupList)
                {
                    if (!sGroupList.Exists(x => x.GroupID == citg.GroupID))
                    {
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(citg.GroupID.Value, CurrentUser);
                        if (group != null  && (!group.IsSeedList.HasValue || group.IsSeedList.Value == false))
                        {
                            sGroupList.Add(new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.GroupSelect() { GroupID = group.GroupID, CustomerID = group.CustomerID, FolderID = group.FolderID ?? 0, GroupName = group.GroupName, GroupDescription = group.GroupDescription });
                            if (citg.Filters.Count > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in citg.Filters)
                                {
                                    if (cibf.FilterID.HasValue)
                                    {
                                        ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                                        sGroupFilterList.Add(new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect() { FilterID = f.FilterID, CustomerID = f.CustomerID.Value, FilterName = f.FilterName, GroupID = f.GroupID.Value });
                                    }
                                }
                            }
                        }
                    }
                  
                }
              SuppressionGroupList.Add(cit.CampaignItemTemplateID, sGroupList);
              if (sGroupFilterList != null)
                  SuppressionGroupFilterList.Add(cit.CampaignItemTemplateID, sGroupFilterList);
              else
              {
                 SuppressionGroupFilterList.Add(cit.CampaignItemTemplateID,null );
              }
            }
           
            IEnumerable<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.CampaignItemTemplateSelect> CampaignItemTemplateIEnum = from cit in citemplates
                                                                                                where cit.Archived.Value == false
                                                                                                select new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.CampaignItemTemplateSelect
                                                                                                {
                                                                                                    CITemplateID = cit.CampaignItemTemplateID,
                                                                                                    MessageID = cit.LayoutID ?? 0,
                                                                                                    MessageName = MessageNameList[cit.CampaignItemTemplateID],
                                                                                                    TemplateName = cit.TemplateName,
                                                                                                    CustomerID = cit.CustomerID.Value,
                                                                                                    UpdatedDate = cit.UpdatedDate,
                                                                                                    CreatedDate = cit.CreatedDate,
                                                                                                    FromEmail = cit.FromEmail,
                                                                                                    FromName = cit.FromName,
                                                                                                    ReplyTo = cit.ReplyTo,
                                                                                                    Subject = cit.Subject,
                                                                                                    BlastField1 = cit.BlastField1,
                                                                                                    BlastField2 = cit.BlastField2,
                                                                                                    BlastField3 = cit.BlastField3,
                                                                                                    BlastField4 = cit.BlastField4,
                                                                                                    BlastField5 = cit.BlastField5,
                                                                                                    SelectedGroupList = selectedGroupList[cit.CampaignItemTemplateID],
                                                                                                    SelectedGroupFilterList=selectedGroupFilterList[cit.CampaignItemTemplateID],
                                                                                                    SuppressionGroupList = SuppressionGroupList[cit.CampaignItemTemplateID],
                                                                                                    SuppressionGroupFilterList = SuppressionGroupFilterList[cit.CampaignItemTemplateID],
                                                                                                    CampaignID = cit.CampaignID ?? 0,
                                                                                                    CampaignName = CampaignNameList[cit.CampaignItemTemplateID]
                                                                                                };

             return Json(CampaignItemTemplateIEnum, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBlastFieldInformation(int customerID)
        {
            List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.BlastFieldSelect> bf = new List<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.BlastFieldSelect>();
            for (int i = 1; i <= 5; i++)
            {
                ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldIDCustomerID(i, customerID, CurrentUser);
                DataTable blastFieldsValue = ECN_Framework_BusinessLayer.Communicator.BlastFieldsValue.GetByBlastFieldIDCustomerID(i, customerID, CurrentUser); 
                if(blastFieldsName!=null)
                {
                    bf.Add(new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.BlastFieldSelect() { BlastFieldName = blastFieldsName.Name, BlastFieldValues = blastFieldsValue.AsEnumerable()
                                                                                                                                                .Select(r => r.Field<string>("Value"))
                                                                                                                                                .ToList() });                    
                }
                
            }
          
            return Json(bf, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomers()
        {
            List<ECN_Framework_Entities.Accounts.Customer> customers = new List<ECN_Framework_Entities.Accounts.Customer>();
            List<KMPlatform.Entity.UserClientSecurityGroupMap> ucsgm = new List<KMPlatform.Entity.UserClientSecurityGroupMap>();
            if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(CurrentUser))
            {
                List<ECN_Framework_Entities.Accounts.Customer> tempCust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(CurrentSession.CurrentBaseChannel.BaseChannelID).OrderBy(x => x.CustomerName).ToList();
                KMPlatform.BusinessLogic.ClientGroupClientMap cgcmWorker = new KMPlatform.BusinessLogic.ClientGroupClientMap();
                foreach (ECN_Framework_Entities.Accounts.Customer c in tempCust)
                {
                    List<KMPlatform.Entity.ClientGroupClientMap> cgcm = cgcmWorker.SelectForClientID(c.PlatformClientID);
                    if (cgcm != null && cgcm.Count > 0 && cgcm.FirstOrDefault().ClientGroupID == CurrentSession.ClientGroupID)
                    {
                        if (KMPlatform.BusinessLogic.Client.HasServiceFeature(cgcm.FirstOrDefault().ClientID, KMPlatform.Enums.Services.MARKETINGAUTOMATION, KMPlatform.Enums.ServiceFeatures.MarketingAutomation))
                        {
                            if (!customers.Exists(x => x.PlatformClientID == c.PlatformClientID))
                            {
                                try
                                {
                                    customers.Add(c);
                                }
                                catch (Exception ex)
                                {
                                    //couldn't get customer becaues they don't exist
                                }
                            }
                        }
                    }


                }
            }
            else
            {
                ucsgm = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap().SelectForUser(CurrentUser.UserID);
                List<int> clientGroupIDs = new List<int>();
                KMPlatform.BusinessLogic.ClientGroupClientMap cgcmWorker = new KMPlatform.BusinessLogic.ClientGroupClientMap();
                foreach (KMPlatform.Entity.UserClientSecurityGroupMap sg in ucsgm)
                {
                    List<KMPlatform.Entity.ClientGroupClientMap> cgcm = cgcmWorker.SelectForClientID(sg.ClientID);
                    if (cgcm != null && cgcm.Count > 0 && cgcm.FirstOrDefault().ClientGroupID == CurrentSession.ClientGroupID)
                    {
                        if (KMPlatform.BusinessLogic.Client.HasServiceFeature(cgcm.FirstOrDefault().ClientID, KMPlatform.Enums.Services.MARKETINGAUTOMATION, KMPlatform.Enums.ServiceFeatures.MarketingAutomation))
                        {
                            if (!customers.Exists(x => x.PlatformClientID == sg.ClientID))
                            {
                                try
                                {
                                    customers.Add(ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(sg.ClientID, false));
                                }
                                catch (Exception ex)
                                {
                                    //couldn't get customer becaues they don't exist
                                }
                            }
                        }
                    }


                }
                customers = customers.OrderBy(x => x.CustomerName).ToList();
            }


            IEnumerable<Models.PostModels.ECN_Objects.CustomerSelect> customerIEnum = from c in customers
                                                                                      where c.IsDeleted.HasValue ? c.IsDeleted.Value == false : c.IsDeleted.HasValue == false && c.ActiveFlag.ToLower().Equals("y")
                                                                                      select new Models.PostModels.ECN_Objects.CustomerSelect { CustomerID = c.CustomerID, CustomerName = c.CustomerName, PlatformClientID = c.PlatformClientID };
            return Json(customerIEnum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSingleCustomer(int customerID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> listCust = new List<ECN_Framework_Entities.Accounts.Customer>();

            ECN_Framework_Entities.Accounts.Customer cust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false);

            listCust.Add(cust);

            IEnumerable<Models.PostModels.ECN_Objects.CustomerSelect> customerIEnum = from c in listCust
                                                                                      where c.IsDeleted.HasValue ? c.IsDeleted.Value == false : c.IsDeleted.HasValue == false && c.ActiveFlag.ToLower().Equals("y")
                                                                                      select new Models.PostModels.ECN_Objects.CustomerSelect { CustomerID = c.CustomerID, CustomerName = "Root", PlatformClientID = c.PlatformClientID };

            return Json(customerIEnum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCampaigns(int customerID, string search = "")
        {
            List<ECN_Framework_Entities.Communicator.Campaign> campaigns;

            campaigns = ECN_Framework_BusinessLayer.Communicator.Campaign.Search(customerID, search, false);

            IEnumerable<Models.PostModels.ECN_Objects.CampaignSelect> campaignEnum = from c in campaigns
                                                                                     where c.IsDeleted == false && c.IsArchived == false
                                                                                     select new Models.PostModels.ECN_Objects.CampaignSelect { CampaignID = c.CampaignID, CampaignName = c.CampaignName, CustomerID = c.CustomerID.Value };
            return Json(campaignEnum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCampaignItems(int customerID, int MAID)
        {
            DataTable campaignItems;
            ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(MAID, false, CurrentUser);
            campaignItems = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetPendingCampaignItems_NONRecurring(customerID);

            IEnumerable<Models.PostModels.ECN_Objects.CampaignItemSelect> campaignItemEnum = from myRow in campaignItems.AsEnumerable()
                                                                                             where myRow.Field<DateTime>("SendTime").Date >= ma.StartDate.Value.Date && myRow.Field<DateTime>("SendTime").Date <= ma.EndDate.Value.Date
                                                                                             select new Models.PostModels.ECN_Objects.CampaignItemSelect()
                                                                                             {
                                                                                                 CampaignItemID = myRow.Field<int>("CampaignItemID"),
                                                                                                 CampaignItemName = myRow.Field<string>("CampaignItemName"),
                                                                                                 CustomerID = myRow.Field<int>("CustomerID"),
                                                                                                 EmailSubject = myRow.Field<string>("EmailSubject"),
                                                                                                 Group = myRow.Field<string>("Group"),
                                                                                                 CampaignItemType = myRow.Field<string>("CampaignItemType"),
                                                                                                 UpdatedDate = myRow.Field<DateTime>("UpdatedDate"),
                                                                                                 SendTime = myRow.Field<DateTime>("SendTime"),
                                                                                                 SendTimeUTC = myRow.Field<DateTime>("SendTime").ToString(UTCDateStringFormat),
                                                                                                 LayoutID = myRow.Field<int>("LayoutID"),
                                                                                                 LayoutName = myRow.Field<string>("LayoutName")
                                                                                             };
            return Json(campaignItemEnum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFilters(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.Filter> filters;

            filters = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID_NoAccessCheck(groupID, true, "active");

            IEnumerable<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect> FiltersIEnum = from f in filters
                                                                                                           where f.IsDeleted == false && f.Archived == false
                                                                                                           select new ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect { FilterID = f.FilterID, CustomerID = f.CustomerID.Value, FilterName = f.FilterName, GroupID = f.GroupID.Value };

            if (FiltersIEnum == null || FiltersIEnum.Count() == 0)
            {
                FiltersIEnum = Enumerable.Empty<ecn.MarketingAutomation.Models.PostModels.ECN_Objects.FilterSelect>();
            }

            return Json(FiltersIEnum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLinkAlias(int layoutID, int customerID)
        {
            DataTable dtLinkAlias = new DataTable();

            dtLinkAlias = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetLinkAliasDR(customerID, layoutID, CurrentUser);

            IEnumerable<Models.PostModels.ECN_Objects.LinkAliasSelect> LinkAliasIEnum = from l in dtLinkAlias.AsEnumerable()
                                                                                        select new Models.PostModels.ECN_Objects.LinkAliasSelect { Alias = l.Field<string>("Alias"), Link = l.Field<string>("Link") };

            return Json(LinkAliasIEnum, JsonRequestBehavior.AllowGet);
        }
    }
}