using System;
using System.Collections.Generic;
using ecn.common.classes.Fakes;
using ecn.webservice;
using ecn.webservice.Facades.Fakes;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessLayerCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessLayerAccountFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Fakes;
using EcnCommonObject = ECN_Framework_Common.Objects;

namespace ECN.Webservice.Tests.Facades
{
    [TestFixture]
    public partial class BlastFacadeTest
    {
        private const string BlastLicensed = "UNLIMITED";
        private const string LicenseNoLicense = "NO LICENSE";
        private const string MethodPerformAddBlast = "PerformAddBlast";
        private const string MethodIsLicenceAvailable = "IsLicenceAvailable";
        private const string MethodGetTestBlastCount = "GetTestBlastCount";
        private const string MethodEnsureBlastValues = "EnsureBlastValues";

        [Test]
        public void AddBlast_ForNotLicensed_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new AddBlastParams()
            {
                FromEmail = Open,
                FilterId = BlastId,
                Subject = Open
            };
            InitializeUpdateBlast();
            ShimLicenseCheck.AllInstances.AvailableString = (x, y) => LicenseNoLicense;

            // Act
            var result = _testEntity.AddBlast(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddBlast_ForLicensed_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new AddBlastParams()
            {
                ReplyEmail = Open,
                FromEmail = Open,
                FilterId = BlastId,
                Subject = Open
            };
            InitializeUpdateBlast();
            ShimBlastFacade.AllInstances.PerformAddBlastWebMethodExecutionContextAddBlastParamsInt32Boolean = 
                (q, w, e, r, t) => Success;

            // Act
            var result = _testEntity.AddBlast(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void AddBlast_ForValueError_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new AddBlastParams()
            {
                ReplyEmail = Open,
                FromEmail = Open,
                FilterId = BlastId,
                Subject = Open
            };
            InitializeUpdateBlast();
            ShimGroup.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _testEntity.AddBlast(context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void PerformAddBlast_ForGreaterEmailCount_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new AddBlastParams()
            {
                ReplyEmail = Open,
                FromEmail = Open,
                FilterId = BlastId,
                Subject = Open,
                IsTest = true
            };
            InitializeUpdateBlast();
            InitializeAddBlast();
            _privateObject = new PrivateObject(_testEntity);

            // Act
            var result = _privateObject.Invoke(MethodPerformAddBlast, 
                new object[] { context, parameters, BlastId, true }) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void IsLicenceAvailable_ForLicensedAndAvailabe_returnTrue()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            _privateObject = new PrivateObject(_testEntity);
            ShimLicenseCheck.AllInstances.CurrentString = (x, y) => LicenseNoLicense;
            ShimLicenseCheck.AllInstances.AvailableString = (x, y) => BlastLicensed;

            // Act
            var result = _privateObject.Invoke(MethodIsLicenceAvailable, new object[] { context });

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(true);
        }

        [Test]
        public void DeleteBlast_ForCampaignItemTestBlastManager_ReturnSuccessResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            ShimCampaignItemBlastManager.AllInstances.GetByBlastIdInt32UserBoolean = (x, y, z, q) => null;
            ShimCampaignItemTestBlastManager.AllInstances.GetByBlastIdInt32UserBoolean = 
                (x, y, z, p) => new CampaignItemTestBlast() { CampaignItemID = BlastId, CampaignItemTestBlastID = BlastId };
            ShimCampaignItemTestBlastManager.AllInstances.DeleteInt32Int32User = 
                (x, y, z, r) => new CampaignItemTestBlast();
            ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 =
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, q, z) => Success;

            // Act
            var result = _testEntity.DeleteBlast(context, BlastId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetTestBlastCount_ForTestBlastLimit_ReturnValue()
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            BusinessLayerAccountFakes.ShimCustomer.GetByCustomerIDInt32Boolean = 
                (x, y) => new Customer() { BaseChannelID = BlastId };
            BusinessLayerAccountFakes.ShimBaseChannel.GetByBaseChannelIDInt32 = 
                (x) => new BaseChannel() { TestBlastLimit = BlastId };

            // Act
            var result = _privateObject.Invoke(MethodGetTestBlastCount, new object[] { BlastId });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBe(BlastId));
        }

        [Test]
        public void GetTestBlastCount_ForTestBlastLimitNull_ReturnValue()
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            BusinessLayerAccountFakes.ShimCustomer.GetByCustomerIDInt32Boolean =
                (x, y) => new Customer() { BaseChannelID = BlastId };
            BusinessLayerAccountFakes.ShimBaseChannel.GetByBaseChannelIDInt32 =
                (x) => new BaseChannel();

            // Act
            var result = _privateObject.Invoke(MethodGetTestBlastCount, new object[] { BlastId });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBe(BlastId));
        }

        [Test]
        public void EnsureBlastValues_ForLayoutExistFalse_ReturnValue()
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            bool value = true;
            var param = new object[] { BlastId, BlastId, BlastId, BlastId, Open, value, new DateTime(), new User(), Open };
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimLayout.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _privateObject.Invoke(MethodEnsureBlastValues, param);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        [TestCase(Open)]
        [TestCase("")]
        public void EnsureBlastValues_ForFilterExistFalse_ReturnValue(string blast)
        {
            // Arrange
            _privateObject = new PrivateObject(_testEntity);
            bool value = true;
            var param = new object[] { BlastId, BlastId, BlastId, BlastId, Open, value, new DateTime(), new User(), blast };
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimLayout.ExistsInt32Int32 = (x, y) => true;
            ShimFilter.ExistsInt32Int32 = (x, y) => false;
            DataLayerFakes.ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => BlastId;
            DataLayerFakes.ShimDataFunctions.ExecuteScalarStringString = (x, y) => BlastId;

            // Act
            var result = _privateObject.Invoke(MethodEnsureBlastValues, param);

            // Assert
            result.ShouldNotBeNull();
        }

        private void InitializeAddBlast()
        {
            ShimLicenseCheck.AllInstances.CurrentString = (x, y) => BlastLicensed;
            BusinessLayerAccountFakes.ShimCustomer.GetByCustomerIDInt32Boolean = 
                (x, y) => new Customer() { TestBlastLimit = BlastId };
            DataLayerFakes.ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 15;
            ShimBlastFacade.AllInstances.GetOrCreateCampaignWebMethodExecutionContext = (x, y) => new Campaign();
            ShimBlastFacade.AllInstances.CreateCampaignItemBlastWebMethodExecutionContextAddBlastParamsInt32BooleanCampaignItem = 
                (x, y, z, q, e, r) => new CampaignItemBlast();
        }
    }
}
