using System;
using System.Collections.Generic;
using System.Data;
using ecn.webservice;
using ecn.webservice.classes.Fakes;
using ecn.webservice.Facades.Fakes;
using ecn.webservice.Facades.Params;
using ECN.Framework.BusinessLayer.Interfaces;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using BusinessLayerCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessLayerCommunicatorFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;

namespace ECN.Webservice.Tests.Facades
{
    [TestFixture]
    public partial class BlastFacadeTest
    {
        private DataTable _dataTable;
        private const int BlastId = 10;
        private const string ActionTypeCode = "ActionTypeCode";
        private const string ActionTypeCodeValue = "send";
        private const string DistinctCount = "DistinctCount";
        private const string DistinctCountValue = "10";
        private const string Total = "total";
        private const string Success = "Success";
        private const string Fail = "Fail";
        private const string Open = "open";
        private const string Click = "click";
        private const string Bounce = "bounce";
        private const string Resend = "resend";
        private const string Refer = "refer";
        private const string Subscribe = "subscribe";
        private const string MasterSupressed = "MASTSUP_UNSUB";
        private const string AbuseComplaints = "ABUSERPT_UNSUB";
        private const string Feedback = "FEEDBACK_UNSUB";
        private const string Exist = "BLAST DOESN'T EXIST";
        private const string ErrorCreatingBlast = "UNKNOWN ERROR CREATING BLAST";
        private const string XmlSearchString = "<ISPs><ISP>name</ISP></ISPs>";

        [TearDown]
        public void TearDownGetBlast()
        {
            _dataTable?.Dispose();
        }

        [Test]
        [TestCase(ActionTypeCodeValue)]
        [TestCase(Open)]
        [TestCase(Click)]
        [TestCase(Bounce)]
        [TestCase(Resend)]
        [TestCase(Refer)]
        [TestCase(Subscribe)]
        [TestCase(MasterSupressed)]
        [TestCase(AbuseComplaints)]
        [TestCase(Feedback)]
        public void GetBlastReport_ForDifferentActionType_ReturnReport(string param)
        {
            // Arrange
            InitializeBlastReport(param);
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };

            // Act
            var result = _testEntity.GetBlastReport(context, BlastId);

            // Assert
            result.ShouldBe(Success);
        }

        [Test]
        public void GetBlastReport_ForNullDataTable_ReturnReport()
        {
            // Arrange
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => null;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Exist;
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };

            // Act
            var result = _testEntity.GetBlastReport(context, BlastId);

            // Assert
            result.ShouldBe(Exist);
        }

        [Test]
        public void AddScheduledBlast_ForNullBlastId_ReturnErrorMessage()
        {
            // Arrange
            InitializeScheduledBlast();
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };

            // Act
            var result = _testEntity.AddScheduledBlast(context, CreateDictionary());

            // Assert
            result.ShouldBe(ErrorCreatingBlast);
        }

        [Test]
        public void GetBlastReportByISP_ForDataTable_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportByISPParams() { BlastId = BlastId, XmlSearch = XmlSearchString};
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 = 
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;

            // Act
            var result = _testEntity.GetBlastReportByISP(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetBlastReportByISP_ForNullDataTable_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportByISPParams() { BlastId = BlastId, XmlSearch = XmlSearchString };
            InitializeBlastReport(Open);
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => null;
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Fail;

            // Act
            var result = _testEntity.GetBlastReportByISP(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetBlastOpensReport_ForBlastExistTrue_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = true, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => true;

            // Act
            var result = _testEntity.GetBlastOpensReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetBlastOpensReport_ForBlastExistFalse_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = true, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Fail;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _testEntity.GetBlastOpensReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetBlastClicksReport_ForBlastExistTrue_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = false, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => true;

            // Act
            var result = _testEntity.GetBlastClicksReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetBlastClicksReport_ForBlastExistFalse_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = true, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Fail;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _testEntity.GetBlastClicksReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetBlastBounceReport_ForBlastExistTrue_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = false, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => true;

            // Act
            var result = _testEntity.GetBlastBounceReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetBlastBounceReport_ForBlastExistFalse_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = true, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Fail;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _testEntity.GetBlastBounceReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetBlastUnsubscribeReport_ForBlastExistTrue_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = false, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => true;

            // Act
            var result = _testEntity.GetBlastUnsubscribeReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetBlastUnsubscribeReport_ForBlastExistFalse_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastReportParams() { BlastId = BlastId, WithDetail = false, FilterType = Success };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Fail;
            BusinessLayerCommunicatorFakes.ShimBlast.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _testEntity.GetBlastUnsubscribeReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetBlastDeliveryReport_ForContext_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId, UserID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new GetBlastDeliveryReportParams() { DateFrom = new DateTime(), DateTo = new DateTime() };
            InitializeBlastReport(Open);
            BusinessLayerCommunicatorFakes.ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;

            // Act
            var result = _testEntity.GetBlastDeliveryReport(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));

        }

        private void InitializeScheduledBlast()
        {
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 1;
            ShimDataFunctions.GetDataTableStringString = (x, y) => new DataTable();
            ShimGroupDataFields.GetListSqlCommand = (x) => new List<GroupDataFields> { };
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => new DataTable();
            BusinessLayerCommunicatorFakes.ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = 
                (x, y) => new BlastSchedule()
                {
                    BlastScheduleID = BlastId
                };
            BusinessLayerCommunicatorFakes.ShimBlastSetupInfo.GetNextScheduledBlastSetupInfoInt32Boolean = 
                (x, y) => new BlastSetupInfo();
            BusinessLayerCommunicatorFakes.ShimCampaign.HasPermissionEnumsAccessUser = (x, y) => true;
            ShimCampaign.GetSqlCommand = (x) => new Campaign() { CustomerID = BlastId, CampaignID = 0 };
            BusinessLayerCommunicatorFakes.ShimCampaign.SaveCampaignUser = (x, y) => 1;
            BusinessLayerCommunicatorFakes.ShimCampaignItem.SaveCampaignItemUser = (x, y) => 1;
            BusinessLayerCommunicatorFakes.ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (x, y, z) => 1;
            BusinessLayerCommunicatorFakes.ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (x) => 1;
            BusinessLayerCommunicatorFakes.ShimCampaignItemBlast.HasPermissionEnumsAccessUser = (x, y) => true;
            BusinessLayerCommunicatorFakes.ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = 
                (x, y, z) => new CampaignItem();
            ShimCampaignItemBlast.GetByCampaignItemIDInt32 = 
                (x) => new List<CampaignItemBlast> { new CampaignItemBlast() { BlastID = BlastId, CustomerID = BlastId } };
            ShimCampaignItem.GetSqlCommand = (x) => new CampaignItem();
            BusinessLayerCommunicatorFakes.ShimCampaignItemBlast.GetByCampaignItemID_NoAccessCheckInt32Boolean = 
                (x, y) => CreateList<CampaignItemBlast>(); ;
            BusinessLayerCommunicatorFakes.ShimCampaignItemTestBlast.GetByCampaignItemID_NoAccessCheckInt32Boolean = 
                (x, y) => CreateList<CampaignItemTestBlast>();
            BusinessLayerCommunicatorFakes.ShimCampaignItemSuppression.GetByCampaignItemID_NoAccessCheckInt32Boolean = 
                (x, y) => CreateList<CampaignItemSuppression>();
            BusinessLayerCommunicatorFakes.ShimCampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheckInt32 =
                (x) => CreateList<CampaignItemOptOutGroup>();
            var access = new BusinessLayerCommunicator.AccessCheck();
            var user = new Moq.Mock<IUser>();
            user.Setup(x => x.IsSystemAdministrator(Moq.It.IsAny<User>())).Returns(true);
            var customer = new Moq.Mock<ICustomer>();
            BusinessLayerCommunicator.AccessCheck.Initialize(user.Object, customer.Object);
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => ErrorCreatingBlast;
        }

        private List<T> CreateList<T>() where T : new()
        {
            return new List<T> { new T() };
        }

        private void InitializeBlastReport(string param)
        {
            _dataTable = new DataTable()
            {
                Columns = {ActionTypeCode, DistinctCount, Total, "ClickTime", "EmailAddress", "Link" },
                Rows = { { param, DistinctCountValue, DistinctCountValue, Success, Success, "link" } }
            };
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => _dataTable;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Success;
        }

        private Dictionary<string, object> CreateDictionary()
        {
            return new Dictionary<string, object>
            {
                { Consts.MessageIdParameter, BlastId },
                { Consts.FilterIdParameter, BlastId },
                { Consts.ListIdParameter, BlastId },
                { Consts.SubjectParameter, Open },
                { Consts.FromEmailParameter, Open },
                { Consts.FromNameParameter, Open },
                { Consts.ReplyEmailParameter, Open },
                { Consts.RefBlastsParameter, Open },
                { Consts.XmlScheduleParameter, Open }
            };
        }
    }
}
