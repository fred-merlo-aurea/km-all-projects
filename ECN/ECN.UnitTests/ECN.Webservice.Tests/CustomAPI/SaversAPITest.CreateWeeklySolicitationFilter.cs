using System;
using System.Collections.Generic;
using ecn.webservice.classes.Fakes;
using ecn.webservice.CustomAPI.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using FakeDataLayerComm = ECN_Framework_DataLayer.Communicator.Fakes;
using FakePlatformData = KMPlatform.DataAccess.Fakes;
using KMPlatformFakes = KM.Platform.Fakes;
using static ecn.webservice.classes.SendResponse;

namespace ECN.Webservice.Tests.CustomAPI
{
    public partial class SaversAPITest
    {
        private const string CreateWeeklySolicitationFilterWebMethodName = "CreateWeeklySolicitationFilter";
        private const string CreateWeeklySolicitationFilterInvalidAccessKeyMsg = "Invalid Access Key";
        private const string CreateWeeklySolicitationFilterNoGroupErrorMsg = "No Group set for Access Key";
        private const string CreateWeeklySolicitationFilterValidationErrorMsg = "Filter validation failed";
        private const string CreateWeeklySolicitationFilterGroupValitationErrorMsg = "FilterGroup validation failed";
        private const string CreateWeeklySolicitationFilterGroupSaveErrorMsg = "Failure creating filter group";
        private const string CreateWeeklySolicitationFilterGroupIdWebMethodName = "FilterGroupID";
        private const string CreateWeeklySolicitationFilterGroupInvalidIdErrorMsg = "Failure creating Filter Condition";
        private const string CreateWeeklySolicitationFilterSaveErrorMsg = "Failure creating filter";
        private const string CreateWeeklySolicitationFilterValidateWebMethodName = "Filter Validate";
        private const string CreateWeeklySolicitationFilterSaveWebMethodName = "Filter insert";
        private const string CreateWeeklySolicitationFilterIDWebMethodName = "FilterID";
        private const string CreateWeeklySolicitationFilterCreatedMsg = "Filter Created:";
        private const string CreateWeeklySolicitationFilterGroupFilterMethodName = "FilterGroup Validate";
        private const string CreateWeeklySolicitationFilterGroupInsertMethodName = "FilterGroup insert";
        private const string CreateWeeklySolicitationFilterCreateFilterConditionMethodName = "CreateFilterCondition";
        private const string CreateWeeklySolicitationFilterNotCreatedErrorMsg = "Filter could not be created:";
        private const int CreateWeeklySolicitationFilterUserId = 10;
        private const int CreateWeeklySolicitationFilterClientId = 10;
        private readonly string _solicitationStartDate = DateTime.Now.ToString();
        private readonly string _zipCodes = "<ZipCodes><ZipCode>55447</ZipCode><ZipCode>55388</ZipCode></ZipCodes>";
        private readonly string _solicitationEndDate = DateTime.Now.AddDays(1).ToString();
        private string _createWeeklySolicitationResponseWebMethod;
        private ResponseCode? _createWeeklySolicitationResponseCode;
        private int? _createWeeklySolicitationResponseId;
        private string _createWeeklySolicitationResponseOutput;

        [Test]
        public void CreateWeeklySolicitationFilter_NoUser_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation();
            ShimUser.GetByAccessKeyStringBoolean = (key, getChildren) => null;

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, string.Empty, string.Empty, string.Empty);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe(CreateWeeklySolicitationFilterWebMethodName),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterInvalidAccessKeyMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_InvalidUserId_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation();
            ShimUser.GetByAccessKeyStringBoolean = (key, getChildren) => new User() { UserID = -10 };

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, string.Empty, string.Empty, string.Empty);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe(CreateWeeklySolicitationFilterWebMethodName),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterInvalidAccessKeyMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_NoGroup_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation();

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe(CreateWeeklySolicitationFilterWebMethodName),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterNoGroupErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_InvaidGroupId_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = -1 });

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe(CreateWeeklySolicitationFilterWebMethodName),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterNoGroupErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_FilterValidateException_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 });
            ShimFilter.ValidateFilter = (filter) =>
            {
                var errors = new List<ECNError>() { new ECNError() };
                throw new ECNException(errors);
            };

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName} - {CreateWeeklySolicitationFilterValidateWebMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterValidationErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_FilterSavedException_Error()
        {
            // Arrange
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 });
            ShimFilter.ValidateFilter = (filter) => { };
            ShimFilter.SaveFilterUser = (filter, user) => throw new Exception();
              
            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName} - {CreateWeeklySolicitationFilterSaveWebMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterSaveErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_FilterSaveReturnInvalidId_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 }, filterId: -10);

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName} - {CreateWeeklySolicitationFilterIDWebMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterSaveErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_FilterGroupValidateException_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 }, filterId: 10);
            ShimFilterGroup.ValidateFilterGroup = (filter) =>
            {
                var errors = new List<ECNError>() { new ECNError() };
                throw new ECNException(errors);
            };

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName} - {CreateWeeklySolicitationFilterGroupFilterMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterGroupValitationErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_FilterGroupSavedException_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 }, filterId: 10);
            ShimFilterGroup.ValidateFilterGroup = (filter) => { };
            ShimFilterGroup.SaveFilterGroupUser = (filter, user) => throw new Exception();

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName} - {CreateWeeklySolicitationFilterGroupInsertMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterGroupSaveErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_FilterGroupSaveReturnInvalidId_Error()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 }, filterId: 10, filterGroupId: -10);

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName} - {CreateWeeklySolicitationFilterGroupIdWebMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterGroupInvalidIdErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_CreateFilterConditionSuccess_SucessResponse()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 }, filterId: 10, filterGroupId: 10);
            ShimSaversAPI.AllInstances.CreateFilterConditionInt32ListOfStringUserInt32 = (service, grpId, zip, user, custId) => "Success";
            ShimFilterCreator.AllInstances.CreateFilterConditionIEnumerableOfStringInt32 = (service, zip, custId) => "Success";

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Success),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterCreatedMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_CreateFilterConditionFail_FailResponse()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 }, filterId: 10, filterGroupId: 10);
            ShimSaversAPI.AllInstances.CreateFilterConditionInt32ListOfStringUserInt32 = (service, grpId, zip, user, custId) => "Fail";

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            _createWeeklySolicitationResponseOutput.ShouldSatisfyAllConditions(
                () => _createWeeklySolicitationResponseWebMethod.ShouldBe($"{CreateWeeklySolicitationFilterWebMethodName} - {CreateWeeklySolicitationFilterCreateFilterConditionMethodName}"),
                () => _createWeeklySolicitationResponseCode.ShouldBe(ResponseCode.Fail),
                () => _createWeeklySolicitationResponseId.ShouldBe(0),
                () => _createWeeklySolicitationResponseOutput.ShouldContain(CreateWeeklySolicitationFilterNotCreatedErrorMsg));
        }

        [Test]
        public void CreateWeeklySolicitationFilter_ValidFilters_AllSaveMethodsCalled()
        {
            // Arrange 
            InitTestCreateWeeklySolicitation(dataGroup: new Group() { GroupID = 10 }, filterId: 10, filterGroupId: 10);
            bool filterGroupSaved = false;
            bool filterSaved = false;
            ShimFilterGroup.SaveFilterGroupUser = (grp, user) =>
            {
                filterGroupSaved = true;
                return 10;
            };
            ShimFilter.SaveFilterUser = (filter, user) =>
            {
                filterSaved = true;
                return 10;
            };

            // Act
            _saversAPIInstance.CreateWeeklySolicitationFilter(string.Empty, 0, _solicitationStartDate, _solicitationEndDate, _zipCodes);

            // Assert
            filterGroupSaved.ShouldSatisfyAllConditions(
                () => filterGroupSaved.ShouldBeTrue(),
                () => filterSaved.ShouldBeTrue());
        }

        private void InitTestCreateWeeklySolicitation(Group dataGroup = null, int filterId = -1, int filterGroupId = -1)
        {
            _createWeeklySolicitationResponseWebMethod = string.Empty;
            _createWeeklySolicitationResponseCode = null;
            _createWeeklySolicitationResponseId = null;
            _createWeeklySolicitationResponseOutput = string.Empty;
            InitCommonShimForCreateWeeklySolicitation(filterId, filterGroupId);
            FakeDataLayerComm.ShimGroup.GetByGroupIDInt32 = (id) => dataGroup;
        }

        private void InitCommonShimForCreateWeeklySolicitation(int filterId = -1, int filterGroupId = -1)
        {
            var dummyUser = new User() { UserID = CreateWeeklySolicitationFilterUserId, IsActive = true, IsPlatformAdministrator = false };
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (webMethod, code, id, output) =>
             {
                 _createWeeklySolicitationResponseWebMethod = webMethod;
                 _createWeeklySolicitationResponseCode = code;
                 _createWeeklySolicitationResponseId = id;
                 _createWeeklySolicitationResponseOutput = output;
                 return string.Empty;
             };
            ShimUser.GetByAccessKeyStringBoolean = (key, getChildren) => dummyUser;
            KMPlatformFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, code, feature, access) => true;
            ShimAccessCheck.CanAccessByCustomerOf1M0User<Group>((grp, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<Filter>((grp, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<FilterGroup>((grp, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<List<FilterGroup>>((grps, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<List<FilterCondition>>((conditions, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<FilterCondition>((condition, user) => true);
            ShimCustomer.ExistsInt32 = (id) => true;
            ShimUser.GetByUserIDInt32Boolean = (id, getChild) => dummyUser;
            ShimUser.AllInstances.SelectUserInt32Boolean = (user, id, getChild) => dummyUser;
            FakePlatformData.ShimECN.getClientIDbyCustomerIDInt32 = (id) => CreateWeeklySolicitationFilterClientId;
            FakePlatformData.ShimUserClientSecurityGroupMap.GetListSqlCommand = (cmd) =>
             {
                 return new List<UserClientSecurityGroupMap>()
                 {
                    new UserClientSecurityGroupMap()
                    {
                        ClientID = CreateWeeklySolicitationFilterClientId
                    }
                 };
             };
            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.ExecuteNonQuerySqlCommandString = (command, conn) => true;
            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.ExecuteScalarSqlCommandString = (command, conn) =>
            {
                if (command.CommandText == "e_Group_Exists_ByID" ||
                    command.CommandText == "e_Filter_Exists_ByID" ||
                    command.CommandText == "e_FilterGroup_Exists_ByID")
                {
                    return 1;
                }
                else if (command.CommandText == "e_Filter_Save")
                {
                    return filterId;
                }
                else if (command.CommandText == "e_FilterGroup_Save")
                {
                    return filterGroupId;
                }
                return 0;
            };
            FakeDataLayerComm.ShimFilter.GetSqlCommand = (command) => 
            new Filter()
            {
                FilterID = filterId
            };
            FakeDataLayerComm.ShimFilterGroup.GetSqlCommand = (command) => 
            new FilterGroup()
            {
                FilterID = filterId, FilterGroupID = filterGroupId
            };
            FakeDataLayerComm.ShimFilterGroup.GetListSqlCommand = (command) =>
            new List<FilterGroup>()
            {
                new FilterGroup()
                {
                  FilterGroupID = filterGroupId,
                  FilterID = filterId
                }
             };
            FakeDataLayerComm.ShimFilterCondition.GetListSqlCommand = (command) =>
            new List<FilterCondition>()
            {
                new FilterCondition()
                {
                    FilterGroupID = filterGroupId
                }
            };
        }
    }
}
