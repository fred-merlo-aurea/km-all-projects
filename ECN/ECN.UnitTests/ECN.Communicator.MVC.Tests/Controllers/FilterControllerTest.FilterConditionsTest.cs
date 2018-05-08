using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Fakes;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using System.Web.Routing;
using ecn.communicator.mvc.Controllers;
using ecn.communicator.mvc.Controllers.Fakes;
using ecn.communicator.mvc.Infrastructure.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using KMSite;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using BusinessAccountsLayer = ECN_Framework_BusinessLayer.Accounts.Fakes;
using BusinessLayerCommunicatorFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Fakes;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;
using EntityCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.MVC.Tests
{
    [TestFixture]
    public partial class FilterControllerTest
    {
        private const int FilterGroupId = 10;
        private const int PageNumber = 1;
        private const int Zero = 0;
        private const string CurrentUser = "CurrentUser";
        private const string SelectedGroupID = "selectedGroupID";
        private const string SelectedFilterID = "selectedFilterID";
        private const string EmailAddress = "EmailID";
        private const string Value = "value";
        private const string Filter = "Filter";
        private const string FilterWhere = "[Filter]";
        private const string ArchiveFilter = "archiveFilter";
        private const string Edit = "Edit";
        private const string PartialsModals = "Partials/Modals/_CreateFilterCondition";
        private const string PartialsModalsEditFilter = "Partials/Modals/_EditFilterCondition";
        private static DataTable _dataTable;

        [TearDown]
        public void TearDown()
        {
            if (_dataTable != null)
            {
                _dataTable.Dispose();
            }
        }

        [Test]
        public void LoadAddFilterCondition_ForFilterGroupId_ReturnActionResult()
        {
            // Arrange
            InitializeFilterCondition();
            var controller = new FilterController();

            // Act
            var result = controller.LoadAddFilterCondition(FilterGroupId) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(PartialsModals));
        }

        [Test]
        public void LoadEditFilterCondition_ForFilterCondition_ReturnActionResult()
        {
            // Arrange
            InitializeLoadEditFilter();
            var controller = new FilterController();
            var filterCondition = new FilterCondition()
            {
                FilterGroupID = FilterGroupId
            };

            // Act
            var result = controller.LoadEditFilterCondition(FilterGroupId) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(PartialsModalsEditFilter));
        }

        [Test]
        public void EditFilterCondition_ForSaveError_ReturnActionResult()
        {
            // Arrange
            InitializeEditFilter();
            var controller = new FilterController();
            var filterCondition = new FilterCondition()
            {
                FilterConditionID = FilterGroupId
            };

            // Act
            var result = controller.EditFilterCondition(filterCondition) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void DeleteFilterCondition_ForDelete_ReturnActionResult()
        {
            // Arrange
            InitializeDeleteFilter();
            var controller = new FilterController();

            // Act
            var result = controller.DeleteFilterCondition(FilterGroupId) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void EditFilterGroup_ForEmptyFilterGroupList_ReturnActionResult()
        {
            // Arrange
            InitializeEditFilterGroup();
            var controller = new FilterController();
            var filterGroup = new FilterGroup() { FilterGroupID = FilterGroupId };

            // Act
            var result = controller.EditFilterGroup(filterGroup) as JavaScriptRedirectResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ActionName.ShouldBe(Edit),
                () => result.ControllerName.ShouldBe(Filter),
                () => result.RouteValues.ShouldNotBeNull());
        }

        [Test]
        public void Index_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeIndex();
            var controller = new FilterController();

            // Act
            var result = controller.Index(FilterGroupId) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void Edit_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeEdit();
            var controller = new FilterController();

            // Act
            var result = controller.Edit(FilterGroupId) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void Update_ForFilter_ResultActionResult()
        {
            // Arrange
            InitializeUpdate();
            var controller = new FilterController();
            var filter = new EntityCommunicator.Filter()
            {
                FilterID = FilterGroupId
            };

            // Act
            var result = controller.Update(filter) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void Preview_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializePreview();
            var controller = new FilterController();

            // Act
            var result = controller.Preview(FilterGroupId) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void PreviewEmailsRead_DataSourceRequest_ReturnActionResult()
        {
            // Arrange
            InitializePreview();
            var controller = new FilterController();
            var dataSourceRequest = new DataSourceRequest()
            {
                PageSize = PageNumber,
                Sorts = CreateSortsList()
            };

            // Act
            var result = controller.PreviewEmails_Read(dataSourceRequest, 
                FilterGroupId, FilterGroupId, PageNumber, PageNumber) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        [TestCase(Filter)]
        [TestCase(FilterWhere)]
        public void CopyFilter_ForCurrentUser_ReturnActionResult(string param)
        {
            // Arrange
            InitializeCopyFilter(param);
            var controller = new FilterController();
            var selected = new int[] { FilterGroupId, PageNumber };

            // Act
            var result = controller.CopyFilter(selected, FilterGroupId) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        private static void InitializeCopyFilter(string param)
        {
            CreateCurrentUser();
            BusinessLayerCommunicatorFakes.ShimGroupDataFields.GetByGroupIDInt32UserBoolean = 
                (x, y, z) => new List<GroupDataFields> { new GroupDataFields() };
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter()
                {
                    GroupID = FilterGroupId,
                    FilterID = FilterGroupId,
                    CustomerID = FilterGroupId,
                    FilterGroupList = new List<FilterGroup>
                    {
                        new FilterGroup()
                        {
                            SortOrder = FilterGroupId,
                            FilterConditionList = new List<FilterCondition> { new FilterCondition() }
                        }
                    },
                    WhereClause = param
                };
            ShimFilterController.AllInstances.IsProfileFieldString = (x, y) => false;
            BusinessAccountsLayer.ShimCustomer.ExistsInt32 = (x) => true;
            BusinessLogicFakes.ShimUser.ExistsInt32Int32 = (x, y) => true;
            BusinessLayerCommunicatorFakes.ShimGroup.ExistsInt32Int32 = (x, y) => false;
            DataLayerFakes.ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => Zero;
            ShimUser.IsSystemAdministratorUser = (x) => true;
        }

        private static IList<SortDescriptor> CreateSortsList()
        {
            var email = new Email() { EmailAddress = EmailAddress };
            var sortDescriptor = new SortDescriptor(email.EmailAddress, ListSortDirection.Ascending);
            return new List<SortDescriptor> { sortDescriptor };
        }

        private static void InitializePreview()
        {
            CreateCurrentUser();
            CreateEcnSession();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter()
                {
                    GroupID = FilterGroupId,
                    FilterID = FilterGroupId,
                    CustomerID = FilterGroupId,
                    FilterGroupList = new List<FilterGroup> { new FilterGroup() { SortOrder = FilterGroupId } },
                    WhereClause = Filter
                };
            _dataTable = new DataTable()
            {
                Columns = { "TotalCount", "EmailID" },
                Rows = { "10", FilterGroupId }
            };
            DataLayerFakes.ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => _dataTable;
        }

        private static void InitializeUpdate()
        {
            CreateCurrentUser();
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter()
                {
                    GroupID = FilterGroupId,
                    FilterID = FilterGroupId,
                    CustomerID = FilterGroupId,
                    FilterGroupList = new List<FilterGroup> { new FilterGroup() { SortOrder = FilterGroupId } }
                };
            var httpSessionBase = new Moq.Mock<HttpSessionStateBase>();
            httpSessionBase.Setup(x => x.Add(SelectedFilterID, Value));
            BusinessLayerCommunicatorFakes.ShimFilter.SaveFilterUser = (x, y) => FilterGroupId;
            ShimController.AllInstances.SessionGet = (x) => httpSessionBase.Object;
            var httpRequest = new Moq.Mock<HttpRequestBase>();
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpRequest.Setup(x => x.RequestContext).Returns(new RequestContext(httpContext.Object, new RouteData()));
            ShimController.AllInstances.RequestGet = (x) => httpRequest.Object;
            ShimUrlHelper.AllInstances.Action = (x) => Filter;
        }

        private static void InitializeEdit()
        {
            CreateEcnSession();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter()
                {
                    GroupID = FilterGroupId,
                    FilterID = FilterGroupId,
                    FilterGroupList = new List<FilterGroup> { new FilterGroup() { SortOrder = FilterGroupId } }
                };
            var httpSessionBase = new Moq.Mock<HttpSessionStateBase>();
            httpSessionBase.Setup(x => x.Add(SelectedFilterID, Value));
            ShimController.AllInstances.SessionGet = (x) => httpSessionBase.Object;
            BusinessLogicFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
        }

        private static void InitializeIndex()
        {
            CreateCurrentUser();
            var httpSessionBase = new Moq.Mock<HttpSessionStateBase>();
            httpSessionBase.Setup(x => x.Add(SelectedGroupID, Value));
            ShimController.AllInstances.SessionGet = (x) => httpSessionBase.Object;
            var httpRequestBase = new Moq.Mock<HttpRequestBase>();
            var httpCoolieCollection = new HttpCookieCollection();
            httpCoolieCollection.Add(new HttpCookie(ArchiveFilter, Filter));
            httpRequestBase.Setup(x => x.Cookies).Returns(httpCoolieCollection);
            ShimController.AllInstances.RequestGet = (x) => httpRequestBase.Object;
            DataLayerCommunicatorFakes.ShimFilter.GetListSqlCommand = 
                (x) => new List<EntityCommunicator.Filter>
                {
                    new EntityCommunicator.Filter()
                    {
                        CustomerID = FilterGroupId,
                        FilterID = FilterGroupId
                    }
                };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter()
                {
                    GroupID = FilterGroupId,
                    FilterID = FilterGroupId,
                    FilterGroupList = new List<FilterGroup> { new FilterGroup() { SortOrder = FilterGroupId } }
                };
            BusinessLayerCommunicatorFakes.ShimGroup.GetByGroupIDInt32User = 
                (x, y) => new EntityCommunicator.Group() { GroupName = Filter };
            BusinessLayerCommunicatorFakes.ShimFilterGroup.GetByFilterIDInt32User = 
                (x, y) => new List<FilterGroup> { new EntityCommunicator.FilterGroup() };
        }

        private static void InitializeEditFilterGroup()
        {
            CreateCurrentUser();
            BusinessLayerCommunicatorFakes.ShimFilterGroup.GetByFilterGroupIDInt32User =
                (x, y) => new FilterGroup()
                {
                    FilterID = FilterGroupId,
                    FilterConditionList = new List<FilterCondition> { new FilterCondition() }
                };
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter()
                {
                    GroupID = FilterGroupId,
                    FilterID = FilterGroupId,
                    FilterGroupList = new List<FilterGroup> { new FilterGroup() { SortOrder = FilterGroupId } }
                };
            BusinessLayerCommunicatorFakes.ShimFilterGroup.SaveFilterGroupUser = (x, y) => FilterGroupId;
        }

        private static void InitializeDeleteFilter()
        {
            CreateCurrentUser();
            BusinessLayerCommunicatorFakes.ShimFilterCondition.GetByFilterConditionIDInt32User =
                (x, y) => new FilterCondition()
                {
                    FilterGroupID = FilterGroupId,
                    FilterConditionID = FilterGroupId
                };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            DataLayerFakes.ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            BusinessLayerCommunicatorFakes.ShimFilterGroup.GetByFilterGroupIDInt32User =
                (x, y) => new FilterGroup()
                {
                    FilterID = FilterGroupId,
                    FilterConditionList = new List<FilterCondition> { new FilterCondition() }
                };
            BusinessLayerCommunicatorFakes.ShimFilterCondition.GetByFilterGroupIDInt32User = 
                (x, y) => new List<FilterCondition> { new FilterCondition() };
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter() { GroupID = FilterGroupId };
            BusinessLayerCommunicatorFakes.ShimFilter.CreateWhereClauseFilter = (x) => Filter;
            ShimHtmlHelperMethods.RenderViewToStringControllerContextStringObject = (x, y, z) => Filter;
        }

        private static void InitializeEditFilter()
        {
            CreateCurrentUser();
            BusinessLayerCommunicatorFakes.ShimFilterCondition.GetByFilterConditionIDInt32User = 
                (x, y) => new FilterCondition() { FilterGroupID = FilterGroupId };
            BusinessLayerCommunicatorFakes.ShimFilterGroup.GetByFilterGroupIDInt32User =
                (x, y) => new FilterGroup()
                {
                    FilterID = FilterGroupId,
                    FilterConditionList = new List<FilterCondition> { new FilterCondition() }
                };
        }

        private static void InitializeLoadEditFilter()
        {
            CreateEcnSession();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            DataLayerCommunicatorFakes.ShimFilterCondition.GetSqlCommand = 
                (x) => new FilterCondition() { CustomerID = FilterGroupId, FilterGroupID = FilterGroupId };
            BusinessLayerCommunicatorFakes.ShimFilterGroup.GetByFilterGroupIDInt32User =
                (x, y) => new FilterGroup() { FilterID = FilterGroupId };
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User =
                (x, y) => new EntityCommunicator.Filter() { GroupID = FilterGroupId };
            var groupDataFields = new Moq.Mock<GroupDataFields>();
            DataLayerCommunicatorFakes.ShimGroupDataFields.GetListSqlCommand = 
                (x) => new List<GroupDataFields> { groupDataFields.Object };
        }

        private static void InitializeFilterCondition()
        {
            CreateEcnSession();
            BusinessLayerCommunicatorFakes.ShimFilterGroup.GetByFilterGroupIDInt32User = 
                (x, y) => new FilterGroup() { FilterID = FilterGroupId };
            BusinessLayerCommunicatorFakes.ShimFilter.GetByFilterIDInt32User = 
                (x, y) => new EntityCommunicator.Filter() { GroupID = FilterGroupId };
            var groupDataFields = new Moq.Mock<GroupDataFields>();
            DataLayerCommunicatorFakes.ShimGroupDataFields.GetListSqlCommand = 
                (x) => new List<GroupDataFields> { groupDataFields.Object };
        }

        private static void CreateCurrentUser()
        {
            var user = new User()
            {
                CurrentClient = new Client() { ClientID = FilterGroupId },
                CustomerID = FilterGroupId,
                UserID = FilterGroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
        }

        private static void CreateEcnSession()
        {
            var shimECNSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimECNSession;
            var fieldCurrentUser = typeof(ECNSession).GetField(CurrentUser);
            if (fieldCurrentUser == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentCustomer should not be null");
            }
            fieldCurrentUser.SetValue(shimECNSession.Instance, new User
            {
                UserID = FilterGroupId,
                CustomerID = FilterGroupId
            });
        }
    }
}
