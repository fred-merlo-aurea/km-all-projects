using System;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using System.Web.Routing;
using System.Collections.Generic;
using ecn.communicator.mvc.Controllers;
using ecn.communicator.mvc.Infrastructure.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using KMPlatform.Entity;
using KM.Platform.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessCommunicatorFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using EcnCommunicatorModels = ecn.communicator.mvc.Models;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;
using KMCommonFakes = KM.Common.Fakes;
using KMPlatformBusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using Models = ecn.communicator.mvc.Models;

namespace ECN.Communicator.MVC.Tests
{
    [TestFixture]
    public partial class GroupControllerTest
    {
        private const string FolderName = "dir";
        private const string Null = "null";
        private const string TypeGroup = "Group";
        private const string TypeProfile = "Profile";
        private const string GroupName = "GroupName";
        private const string TypeGroupName = "gn";
        private const string TypeGroupDisc = "gd";
        private const string EcnErrors = "ECNErrors";
        private const string UdfHistory = "Y";
        private const string CurrentUser = "CurrentUser";
        private const string CurrentBaseChannel = "CurrentBaseChannel";
        private const string CurrentCustomer = "CurrentCustomer";
        private const string PartialsSearchGroupGrid = "Partials/_SearchGroupGrid";
        private const int Id = 0;
        private const int GroupId = 10;
        private const int PageNumber = 1;

        [Test]
        public void Index_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeIndex();
            var controller = new GroupController();

            // Act
            var result = controller.Index() as ViewResult;

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
            var controller = new GroupController();

            // Act
            var result = controller.Edit(GroupId) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Emails_ForAccessedUser_ReturnActionResult(bool param)
        {
            // Arrange
            InitializeEmails(param);
            var controller = new GroupController();

            // Act
            var result = controller.Emails(GroupId) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        [TestCase(TypeGroup)]
        [TestCase(TypeProfile)]
        public void Search_ForDifferentSearchTypes_ReturnActionResult(string param)
        {
            // Arrange
            InitializeSearch(param);
            var controller = new GroupController();
            var searchParams = new EcnCommunicatorModels.SearchParams()
            {
                searchType = param,
                folderID = GroupId,
                allFolders = true,
                archiveFilter = FolderName,
                searchCriterion = param
            };

            // Act
            var result = controller.Search(searchParams) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(PartialsSearchGroupGrid));
        }

        [Test]
        public void Add_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeAdd();
            var controller = new GroupController();

            // Act
            var result = controller.Add() as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void AddHttpPost_ForGroup_ReturnActionResult()
        {
            // Arrange
            InitializeHttpAdd();
            var controller = new GroupController();
            var group = new Models.Group()
            {
                CustomerID = GroupId,
                UpdatedUserID = GroupId,
                FolderID = GroupId,
                GroupName = FolderName,
                MasterSupression = 0,
                CreatedUserID = GroupId,
                PublicFolder = 1,
                AllowUDFHistory = UdfHistory,
                IsSeedList = false
            };

            // Act
            var result = controller.Add(group) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void Update_ForGroup_ReturnActionResult()
        {
            // Arrange
            InitializeHttpUpdate();
            var controller = new GroupController();
            var group = new Models.Group()
            {
                CustomerID = GroupId,
                UpdatedUserID = GroupId,
                FolderID = GroupId,
                GroupName = FolderName,
                MasterSupression = 0,
                CreatedUserID = GroupId,
                PublicFolder = 1,
                AllowUDFHistory = UdfHistory,
                IsSeedList = false
            };

            // Act
            var result = controller.Update(group) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        [TestCase(TypeProfile)]
        [TestCase(TypeGroup)]
        public void GroupsReadToGrid_ForSearchTypeProfile_ReturnActionResult(string param)
        {
            // Arrange
            InitializeGroups(param);
            var controller = new GroupController();
            var dataSource = new DataSourceRequest()
            {
                Sorts = CreateSortDescriptor()
            };

            // Act
            var result = controller.GroupsReadToGrid(dataSource, param, param, FolderName, FolderName, true, GroupId, PageNumber) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        private static void InitializeGroups(string param)
        {
            CreateEcnSession();
            if (param.Equals(TypeProfile))
            {
                BusinessCommunicatorFakes.ShimGroup.GetByProfileNameStringStringUserInt32Int32Int32BooleanStringStringString = 
                    (x, y, z, n, q, w, e, r, t, p) => new DataTable();
            }
            else
            {
                BusinessCommunicatorFakes.ShimGroup.GetByGroupNameStringStringUserInt32Int32Int32BooleanStringStringString = 
                    (x, y, z, q, w, e, r, t, u, i) => new DataTable();
            }
            ShimConversionMethods.DataTableToListGroupsDataTable = (x) => new List<Models.Group> { new Models.Group() };
        }

        private static IList<SortDescriptor> CreateSortDescriptor()
        {
            var group = new Models.Group() { GroupName = GroupName };
            var sortDescriptor = new SortDescriptor(group.GroupName, ListSortDirection.Ascending);
            return new List<SortDescriptor> { sortDescriptor };
        }

        private void InitializeHttpUpdate()
        {
            CreateCurrentUser();
            BusinessCommunicatorFakes.ShimGroup.GetByGroupIDInt32User =
                (x, y) => new Group() { GroupID = GroupId, MasterSupression = GroupId };
            BusinessCommunicatorFakes.ShimGroup.SaveGroupUser = (x, y) => GroupId;
            var request = new Moq.Mock<HttpRequestBase>();
            var context = new Moq.Mock<HttpContextBase>();
            request.Setup(x => x.RequestContext).Returns(new RequestContext(context.Object, new RouteData()));
            ShimController.AllInstances.RequestGet = (x) => request.Object;
        }

        private static void InitializeHttpAdd()
        {
            CreateCurrentUser();
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => GroupId;
            BusinessCommunicatorFakes.ShimGroup.ExistsInt32StringInt32Int32 = (x, y, z, b) => false;
            ShimRegexUtilities.IsValidObjectNameString = (x) => true;
            ShimCustomer.ExistsInt32 = (x) => true;
            ShimUser.IsSystemAdministratorUser = (x) => true;
            KMPlatformBusinessLogicFakes.ShimUser.GetByUserIDInt32Boolean = (x, y) => new User();
            BusinessCommunicatorFakes.ShimGroup.CheckForExistingSeedlistNullableOfInt32Int32 = (x, y) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            KMCommonFakes.ShimCacheUtil.IsCacheEnabled = () => false;
            DataLayerCommunicatorFakes.ShimGroupConfig.GetListSqlCommand = (x) => new List<GroupConfig> { new GroupConfig() };
            BusinessCommunicatorFakes.ShimGroupDataFields.SaveGroupDataFieldsUser = (x, y) => GroupId;
            var request = new Moq.Mock<HttpRequestBase>();
            var context = new Moq.Mock<HttpContextBase>();
            request.Setup(x => x.RequestContext).Returns(new RequestContext(context.Object, new RouteData()));
            ShimController.AllInstances.RequestGet = (x) => request.Object;
        }

        private static void InitializeAdd()
        {
            CreateCurrentUser();
            var shimECNSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimECNSession;
            ShimECNSession.AllInstances.CustomerIDGet = (x) => GroupId;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            ShimConvenienceMethods.GetTempDataControllerBaseString = (x, y) => 
            {
                var result = new Object();
                if (x.Equals(TypeGroupName) || x.Equals(TypeGroupDisc))
                {
                    return result = FolderName;
                }
                else if (x.Equals(EcnErrors))
                {
                    return null;
                }
                else
                {
                    return result = GroupId;
                }
            };
            DataLayerCommunicatorFakes.ShimFolder.GetListSqlCommand =
                (x) => new List<Folder>
                {
                    new Folder()
                    {
                        FolderName = FolderName,
                        ParentID = 0
                    }
                };
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => true;
        }

        private static void InitializeSearch(string param)
        {
            CreateCurrentUser();
            if (param.Equals(TypeGroup))
            {
                BusinessCommunicatorFakes.ShimGroup.GetByGroupName_NoAccessCheckStringStringInt32Int32Int32Int32Int32BooleanStringNullableOfInt32StringString = 
                    (q, w, e, r, t, y, u, i, o, p, z, x) => new DataTable();
            }
            else
            {
                BusinessCommunicatorFakes.ShimGroup.GetByProfileName_NoAccessCheckStringStringInt32Int32Int32Int32Int32BooleanStringNullableOfInt32StringString =
                    (q, w, e, r, t, y, u, i, o, p, z, x) => new DataTable();
            }
            ShimConversionMethods.DataTableToListGroupsDataTable =
                    (x) => new List<EcnCommunicatorModels.Group>
                    {
                        new EcnCommunicatorModels.Group()
                        {
                            GroupID = GroupId
                        }
                    };
            BusinessCommunicatorFakes.ShimGroup.GetByGroupID_NoAccessCheckInt32 =
                (x) => new Group() { FolderID = GroupId };
            DataLayerCommunicatorFakes.ShimFolder.GetSqlCommand = (x) => new Folder() { FolderName = FolderName };
        }

        private static void InitializeEmails(bool param)
        {
            CreateCurrentUser();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            if (param)
            {
                BusinessCommunicatorFakes.ShimGroup.GetByGroupIDInt32User =
                (x, y) => new Group() { GroupID = GroupId };
            }
            else
            {
                BusinessCommunicatorFakes.ShimGroup.GetByGroupIDInt32User =
                (x, y) => new Group() { GroupID = GroupId, MasterSupression = GroupId };
            }
            DataLayerCommunicatorFakes.ShimEmail.GetListSqlCommand = 
                (x) => new List<Email> { new Email() { CustomerID = GroupId } };
            DataLayerCommunicatorFakes.ShimFolder.GetListSqlCommand =
                (x) => new List<Folder>
                {
                    new Folder()
                    {
                        FolderName = FolderName,
                        ParentID = 0
                    }
                };
        }

        private static void InitializeEdit()
        {
            var shimECNSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimECNSession;
            ShimECNSession.AllInstances.CustomerIDGet = (x) => GroupId;
            CreateCurrentUser();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            BusinessCommunicatorFakes.ShimGroup.GetByGroupIDInt32User =
                (x, y) => new Group() { GroupID = GroupId };
            DataLayerCommunicatorFakes.ShimFolder.GetListSqlCommand =
                (x) => new List<Folder>
                {
                    new Folder()
                    {
                        FolderName = FolderName,
                        ParentID = 0
                    }
                };
            ShimConvenienceMethods.GetTempDataControllerBaseString = (x, y) => Null;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => true;
        }

        private static void InitializeIndex()
        {
            CreateCurrentUser();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            DataLayerCommunicatorFakes.ShimFolder.GetListSqlCommand =
                (x) => new List<Folder>
                {
                    new Folder()
                    {
                        FolderName = FolderName,
                        ParentID = 0
                    }
                };
            ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => new Customer() { BaseChannelID = Id };
            DataLayerCommunicatorFakes.ShimGroup.GetByGroupNameInt32StringStringInt32BooleanInt32Int32StringStringInt32Int32StringNullableOfInt32 =
                (x, y, z, a, s, d, f, g, h, j, k, l, q) => new DataTable();
            ShimConvenienceMethods.GetTempDataControllerBaseString = (x, y) => Null;
        }

        private static void CreateCurrentUser()
        {
            var user = new User()
            {
                CurrentClient = new Client() { ClientID = Id },
                CustomerID = GroupId,
                UserID = Id
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
        }

        private static void CreateEcnSession()
        {
            var shimECNSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimECNSession;
            ShimECNSession.AllInstances.CustomerIDGet = (x) => GroupId;
            var fieldCurrentUser = typeof(ECNSession).GetField(CurrentUser);
            if (fieldCurrentUser == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentCustomer should not be null");
            }
            fieldCurrentUser.SetValue(shimECNSession.Instance, new User
            {
                UserID = GroupId
            });
            var fieldCurrentBaseChannel = typeof(ECNSession).GetField(CurrentBaseChannel);
            if(fieldCurrentBaseChannel == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentBaseChannel should not be null");
            }
            fieldCurrentBaseChannel.SetValue(shimECNSession.Instance, new BaseChannel
            {
                BaseChannelID = GroupId
            });
            var fieldCurrentCustomer = typeof(ECNSession).GetField(CurrentCustomer);
            if (fieldCurrentCustomer == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentCustomer should not be null");
            }
            fieldCurrentCustomer.SetValue(shimECNSession.Instance, new Customer
            {
                CustomerID = GroupId
            });
        }
    }
}
