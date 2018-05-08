using System;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using KmpPlatformService = KMPlatform.Enums.Services;
using ServiceClient = UAS_WS.Service.Client;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimClient;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityClient = KMPlatform.Entity.Client;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClientTest : Fakes
    {
        private const string FolderPath = "root/folder1";
        private const int ClientId = 100;
        private const int GroupId = 200;

        private ServiceClient _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceClient();
        }

        [Test]
        public void SelectFtpFolder_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectFtpFolderStringBoolean = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SelectFtpFolder(Guid.Empty, FolderPath, true);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void SelectFtpFolder_WithFolderPath_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterPath = string.Empty;
            var entity = new EntityClient();
            ShimWorker.AllInstances.SelectFtpFolderStringBoolean = (_, path, __) =>
            {
                parameterPath = path;
                return entity;
            };

            // Act
            var result = _testEntity.SelectFtpFolder(Guid.Empty, FolderPath, true);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterPath.ShouldBe(FolderPath);
        }

        [Test]
        public void SelectDefault_WithAccessId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterInclusive = false;
            var entity = new EntityClient();
            ShimWorker.AllInstances.SelectDefaultGuidBoolean = (_, __, inclusive) =>
            {
                parameterInclusive = inclusive;
                return entity;
            };

            // Act
            var result = _testEntity.SelectDefault(Guid.Empty, true);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterInclusive.ShouldBeTrue();
        }

        [Test]
        public void HasService_ByClientEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityClient();

            // Act
            var result = _testEntity.HasService(Guid.Empty, entity, new KmpPlatformService(), GroupId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void HasFeature_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.HasFeature(Guid.Empty, ClientId, new KmpPlatformService(), string.Empty, GroupId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void HasFulfillmentService_ByClientIdAndGroupId_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.HasFulfillmentService(Guid.Empty, ClientId, GroupId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void HasFulfillmentService_ByClientEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityClient();

            // Act
            var result = _testEntity.HasFulfillmentService(Guid.Empty, entity, GroupId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void UseUADSuppressionFeature_ByClientIdAndGroupId_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.UseUADSuppressionFeature(Guid.Empty, ClientId, GroupId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void GetClientAdditionalProperties_ByClientEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimClientAdditionalProperties.AllInstances.SetObjectsInt32Boolean = (_, id, __) =>
            {
                parameterId = id;
                return new ClientAdditionalProperties();
            };

            // Act
            var result = _testEntity.GetClientAdditionalProperties(Guid.Empty, ClientId, true);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterId.ShouldBe(ClientId);
        }
    }
}
