using System;
using System.Web;
using System.Web.Fakes;
using System.Configuration.Fakes;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using ecn.communicator.mvc.Controllers;
using ecn.communicator.mvc.Infrastructure.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN.Framework.BusinessLayer.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_DataLayer.Activity.View.Fakes;
using ECN_Framework_Entities.Activity.View;
using ECN_Framework_Entities.Communicator;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Accounts = ECN_Framework_Entities.Accounts;
using AccountsFake = ECN_Framework_DataLayer.Accounts.Fakes;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using CommunicatorModels = ecn.communicator.mvc.Models;
using DataLayerFakes = ECN_Framework_DataLayer.Communicator.Fakes;
using EntityFakes = KM.Common.Entity.Fakes;

namespace ECN.Communicator.MVC.Tests
{
    [TestFixture]
    public partial class SubscriberControllerTest
    {
        private const int GroupId = 10;
        private const int EmailId = 10;
        private const int PageNumber = 10;
        private const int PageSize = 10;
        private const string Like = "like";
        private const string Starts = "starts";
        private const string Ends = "ends";
        private const string Equal = "equals";
        private const string ProfileName = "name";
        private const string EmailAddress = "dummy@dummy.com";
        private const string FromDate = "04/13/2018";
        private const string EndDate = "04/13/3018";
        private const string ToDate = "01/01/1753";
        private const string EmptyDate = "";
        private const string CurrentUser = "CurrentUser";
        private const string SelectedEmailId = "selectedEmailID";
        private const string Value = "value";
        private const string KmCommonApplication = "KMCommon_Application";
        private const string StringTen = "10";
        private const string CurrentBaseChannel = "CurrentBaseChannel";
        private const string PartialErrorNotification = "Partials/_ErrorNotification";
        private const string PartialModal = "Partials/Modals/_AddUDFData";
        private const string TempURL = "http://www.tempuri.org";
        private static DataTable _dataTable;
        private static DataTable _dataTable1;

        [TearDown]
        public void TearDown()
        {
            if (_dataTable != null || _dataTable1 != null)
            {
                _dataTable.Dispose();
                _dataTable1.Dispose();
            }
        }

        [Test]
        [TestCase(Like, false)]
        [TestCase(Starts, true)]
        [TestCase(Ends, true)]
        [TestCase(Equal, true)]
        public void Index_ForGroupNotNull_ReturnActionResult(string comparator, bool param)
        {
            // Arrange
            var controller = new SubscriberController();
            InitializeIndex(param);

            // Act
            var result = controller.Index(GroupId, comparator) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void SubLogReadToGrid_ForEmail_ReturnActionResult()
        {
            // Arrange 
            InitializeLog();
            var controller = new SubscriberController();

            // Act
            var result = controller.SubLogReadToGrid(new DataSourceRequest(), EmailId, 
                GroupId, PageNumber, PageSize) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        [TestCase(Like, true)]
        [TestCase(Equal, true)]
        [TestCase(Ends, true)]
        [TestCase(Starts, true)]
        [TestCase(ProfileName, true)]
        public void EmailsSubsReadToGrid_ForInvalidSortDescription_PartialViewResult(string param, bool activity)
        {
            // Arrange
            InitializeEmailSubs();
            var controller = new SubscriberController();
            var request = new DataSourceRequest()
            {
                Sorts = CreateSortsList()
            };

            // Act
            var result = controller.EmailsSubsReadToGrid(request, GroupId, Like, param, 
                ProfileName, FromDate, ToDate, activity, PageNumber, PageSize) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(PartialErrorNotification));
        }

        [Test]
        public void Edit_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeEmail();
            var controller = new SubscriberController();

            // Act
            var result = controller.Edit(GroupId, GroupId) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void EditUDF_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeUdf();
            var controller = new SubscriberController();

            // Act
            var result = controller.EditUDF(GroupId, GroupId) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void LoadUDFHistoryData_ForDataTable_ReturnActionResult()
        {
            // Arrange
            InitializeUdfHistory();
            var controller = new SubscriberController();

            // Act
            var result = controller.LoadUDFHistoryData(GroupId, GroupId, GroupId) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void LoadAddUDFData_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeAddUdf();
            var controller = new SubscriberController();

            // Act
            var result = controller.LoadAddUDFData(GroupId, GroupId) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(PartialModal));
        }

        [Test]
        [TestCase(Like, FromDate)]
        [TestCase(FromDate, Like)]
        [TestCase(ToDate, FromDate)]
        [TestCase(FromDate, ToDate)]
        [TestCase(EmptyDate, EmptyDate)]
        [TestCase(FromDate, EndDate)]
        public void ValidateDates_ForDates_ReturnActionResult(string startDate, string endDate)
        {
            // Arrange
            var controller = new SubscriberController();

            // Act
            var result = controller.ValidateDates(startDate, endDate, true) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void Update_ForEmail_ReturnActionResult(bool param)
        {
            // Arrange
            InitializeUpdate(param);
            var controller = new SubscriberController();
            var email = new CommunicatorModels.Email()
            {
                EmailAddress = EmailAddress,
                EmailID = GroupId,
                CurrentGroupID = GroupId,
                FormatTypeCode = EmailAddress,
                SubscribeTypeCode = EmailAddress
            };

            // Act
            var result = controller.Update(email) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        private static void InitializeUpdate(bool param)
        {
            var user = new User()
            {
                CurrentClient = new Client() { ClientID = GroupId },
                CustomerID = GroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
            ShimConversionMethods.ToEmail_InternalEmailUser = 
                (x, y) => new Email()
                {
                    EmailID = GroupId,
                    EmailAddress = EmailAddress,
                    CustomerID = GroupId
                };
            if (param)
            {
                ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => GroupId;
            }
            else
            {
                ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
                DataLayerFakes.ShimGroup.GetMasterSuppressionGroupInt32 =
                (x) => new Group() { CustomerID = GroupId, GroupID = GroupId };
                ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
                DataLayerFakes.ShimEmailGroup.GetByEmailAddressGroupIDStringInt32 = (x, y) => null;
                DataLayerFakes.ShimChannelMasterSuppressionList.GetListSqlCommand = 
                    (x) => new List<ChannelMasterSuppressionList> { };
                DataLayerFakes.ShimGlobalMasterSuppressionList.GetListSqlCommand = 
                    (x) => new List<GlobalMasterSuppressionList> { };
                ShimEmail.IsValidEmailAddressString = (x) => true;
                ShimGroup.GetByGroupIDInt32User = (x, y) => new Group();
                DataLayerFakes.ShimEmailGroup.ImportEmailsInt32Int32Int32StringStringStringStringBooleanStringString =
                    (a, s, d, f, g, h, j, k, l, z) => new DataTable();
            }
            DataLayerFakes.ShimEmail.GetSqlCommand = (x) => new Email();
            var shimECNSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimECNSession;
            ShimECNSession.AllInstances.CustomerIDGet = (x) => GroupId;
            var fieldCurrentUser = typeof(ECNSession).GetField(CurrentBaseChannel);
            if (fieldCurrentUser == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentCustomer should not be null");
            }
            fieldCurrentUser.SetValue(shimECNSession.Instance, new Accounts.BaseChannel
            {
                BaseChannelID = GroupId
            });
        }

        private static void InitializeAddUdf()
        {
            var shimECNSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimECNSession;
            ShimECNSession.AllInstances.CustomerIDGet = (x) => GroupId;
            BusinessLogicFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            var user = new User()
            {
                CurrentClient = new Client() { ClientID = GroupId },
                CustomerID = GroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
            DataLayerFakes.ShimGroupDataFields.GetListSqlCommand = 
                (x) => new List<GroupDataFields> { new GroupDataFields() { CustomerID = GroupId } };
        }

        private static void InitializeUdfHistory()
        {
            var user = new User()
            {
                CurrentClient = new Client() { ClientID = GroupId },
                CustomerID = GroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
            _dataTable = new DataTable()
            {
                Columns = {"LastModifiedDate", "EmailID" },
                Rows = { FromDate }
            };
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => _dataTable;
        }

        private static void InitializeUdf()
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
                UserID = GroupId,
                CustomerID = GroupId
            });
            BusinessLogicFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            var user = new User()
            {
                CurrentClient = new Client() { ClientID = GroupId },
                CustomerID = GroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => new DataTable();
            DataLayerFakes.ShimEmail.GetSqlCommand = (x) => new Email()
            {
                EmailID = EmailId,
                CustomerID = GroupId,
                EmailAddress = EmailAddress
            };
            var dataFieldSets = new DataFieldSets();
            DataLayerFakes.ShimDataFieldSets.GetListSqlCommand = (x) => new List<DataFieldSets> { dataFieldSets };
            ShimHttpContext.CurrentGet = () =>
            {
                var context = new HttpContext(new HttpRequest(null, TempURL, null), new HttpResponse(null));
                return context;
            };
            var httpSessionBase = new Moq.Mock<HttpSessionStateBase>();
            httpSessionBase.Setup(x => x.Add(SelectedEmailId, Value));
            ShimController.AllInstances.SessionGet = (x) => httpSessionBase.Object;
        }

        private static void InitializeEmail()
        {
            var user = new User()
            {
                CurrentClient = new Client() { ClientID = GroupId },
                CustomerID = GroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
            DataLayerFakes.ShimEmail.GetSqlCommand = (x) => new Email()
            {
                EmailID = EmailId,
                CustomerID = GroupId
            };
            BusinessLogicFakes.ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            ShimGroup.GetByGroupIDInt32User = (x, y) => new Group()
            {
                MasterSupression = GroupId,
                GroupID = GroupId
            };
            ShimBlastActivity.GetListSqlCommand = (x) => new List<BlastActivity> { new BlastActivity() };
            AccountsFake.ShimCode.GetListSqlCommand = (x) => new List<Accounts.Code> { new Accounts.Code() };
        }

        private static IList<SortDescriptor> CreateSortsList()
        { 
            var email = new Email() { EmailAddress = EmailAddress };
            var sortDescriptor = new SortDescriptor(email.EmailAddress, ListSortDirection.Ascending);
            return new List<SortDescriptor> { sortDescriptor };
        }

        private static void InitializeEmailSubs()
        {
            var user = new User()
            {
                CustomerID = GroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
            _dataTable = new DataTable()
            {
                Columns = { "DataColumn1", "DataColumn2" },
                Rows = { { "1", "1" }, { "Data21", "Data22" } }
            };
            _dataTable1 = new DataTable()
            {
                Columns = { "EmailID" },
                Rows = { GroupId }
            };
            var dataSet = new DataSet()
            {
                Tables = { _dataTable, _dataTable1 }
            };
            ShimDataFunctions.GetDataSetSqlCommandString = (x, y) => dataSet;
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(KmCommonApplication, StringTen);
            ShimConfigurationManager.AppSettingsGet = () => nameValueCollection;
            EntityFakes.ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 = (x, y, z, q, w, e) => { };
        }

        private static void InitializeLog()
        {
            var user = new User()
            {
                CustomerID = GroupId
            };
            ShimConvenienceMethods.GetCurrentUser = () => user;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            DataLayerFakes.ShimEmail.GetSqlCommand = (x) => new Email()
            {
                EmailID = EmailId,
                CustomerID = GroupId
            };
            _dataTable = new DataTable()
            {
                Columns = { "BlastID", "TotalCount", "EmailSubject", "ActionTypeCode", "ActionValue", "ActionDate" },
                Rows = { { "subscribe", "1", "\\ud000\\ud000", "subscribe", "value", "04/13/2018" } }
            };
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => _dataTable;
            var userMock = new Moq.Mock<IUser>();
            userMock.Setup(x => x.IsSystemAdministrator(user)).Returns(false);
        }

        private static void InitializeIndex(bool param)
        {
            ShimConvenienceMethods.GetCurrentUser = () => new User() { CustomerID = GroupId };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, b) => true;
            if (param)
            {
                ShimGroup.GetByGroupIDInt32User = (x, y) => new Group()
                {
                    MasterSupression = GroupId,
                    GroupID = GroupId
                };
            }
            else
            {
                ShimGroup.GetByGroupIDInt32User = (x, y) => new Group()
                {
                    GroupID = GroupId
                };
            }
            _dataTable = new DataTable()
            {
                Columns = { "DataColumn1", "DataColumn2" },
                Rows = { { "1", "1" }, { "Data21", "Data22" } }
            };
            var dataSet = new DataSet()
            {
                Tables = { _dataTable, new DataTable()}
            };
            ShimDataFunctions.GetDataSetSqlCommandString = (x, y) => dataSet;
        }
    }
}
