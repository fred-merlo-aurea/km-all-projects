using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityClientUADUsersMap = KMPlatform.Entity.ClientUADUsersMap;
using ServiceClientUADUsersMap = UAS_WS.Service.ClientUADUsersMap;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimClientUADUsersMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClientUADUsersMapTest : Fakes
    {
        private const int ClientId = 100;
        private const int UserId = 200;

        private ServiceClientUADUsersMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceClientUADUsersMap();
        }

        [Test]
        public void Save_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveInt32Int32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Save(Guid.NewGuid(), ClientId, UserId);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_ByClientIdAndUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterClientId = 0;
            var parameterUserId = 0;
            ShimWorker.AllInstances.SaveInt32Int32 = (_, clientId, userId) =>
            {
                parameterClientId = clientId;
                parameterUserId = userId;
                return false;
            };

            // Act
            var result = _testEntity.Save(Guid.NewGuid(), ClientId, UserId);

            // Assert
            parameterClientId.ShouldBe(ClientId);
            parameterUserId.ShouldBe(UserId);
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityClientUADUsersMap>();
            ShimWorker.AllInstances.Select = _ => list;

            // Act
            var result = _testEntity.Select(Guid.NewGuid());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectClient_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityClientUADUsersMap>();
            ShimWorker.AllInstances.SelectClientInt32 = (_, __) => list;

            // Act
            var result = _testEntity.SelectClient(Guid.NewGuid(), ClientId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectUser_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityClientUADUsersMap>();
            ShimWorker.AllInstances.SelectUserInt32 = (_, __) => list;

            // Act
            var result = _testEntity.SelectUser(Guid.NewGuid(), UserId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientIdAndUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityClientUADUsersMap();
            ShimWorker.AllInstances.SelectInt32Int32 = (_, __, ___) => entity;

            // Act
            var result = _testEntity.Select(Guid.NewGuid(), ClientId, UserId);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityClientUADUsersMap();
            ShimWorker.AllInstances.SaveClientUADUsersMap = (_, __) => true;
            ShimForJsonFunction<EntityClientUADUsersMap>();

            // Act
            var result = _testEntity.Save(Guid.NewGuid(), entity);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
