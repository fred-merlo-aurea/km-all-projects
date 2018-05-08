using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityMasterGroup = FrameworkUAD.Entity.MasterGroup;
using ServiceMasterGroup = UAD_WS.Service.MasterGroup;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimMasterGroup;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MasterGroupTest : Fakes
    {
        private const int GroupId = 100;
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceMasterGroup _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceMasterGroup();
        }

        [Test]
        public void Delete_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32ClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Delete(Guid.Empty, GroupId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Delete_ByPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteInt32ClientConnections = (_, id, ___) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, GroupId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(GroupId);
        }

        [Test]
        public void Save_WithEntityAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityMasterGroup();
            ShimWorker.AllInstances.SaveMasterGroupClientConnections = (_, __, ___) => AffectedCountPositive;
            ShimForJsonFunction<EntityMasterGroup>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WithEntityAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityMasterGroup();
            ShimWorker.AllInstances.SaveMasterGroupClientConnections = (_, __, ___) => AffectedCountNegative;
            ShimForJsonFunction<EntityMasterGroup>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityMasterGroup>();
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
