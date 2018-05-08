using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityBatch = FrameworkUAD.Entity.Batch;
using ServiceBatch = UAD_WS.Service.Batch;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimBatch;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BatchTest : Fakes
    {
        private const int UserId = 100;
        private const int AffectedCountPositive = 1;

        private ServiceBatch _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceBatch();
        }

        [Test]
        public void CloseBatches_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CloseBatchesInt32ClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.CloseBatches(Guid.Empty, UserId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void CloseBatches_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterUserId = 0;
            ShimWorker.AllInstances.CloseBatchesInt32ClientConnections = (_, id, ___) =>
            {
                parameterUserId = id;
                return true;
            };

            // Act
            var result = _testEntity.CloseBatches(Guid.Empty, UserId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
            parameterUserId.ShouldBe(UserId);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityBatch();
            ShimWorker.AllInstances.SaveBatchClientConnections = (_, __, ___) => AffectedCountPositive;
            ShimForJsonFunction<EntityBatch>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void StartNewBatch_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityBatch();
            ShimWorker.AllInstances.StartNewBatchInt32Int32ClientConnections = (a, b, c, d) => entity;

            // Act
            var result = _testEntity.StartNewBatch(Guid.Empty, UserId, UserId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatch>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatch>();
            ShimWorker.AllInstances.SelectInt32BooleanClientConnections = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, UserId, true, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void BatchCheck_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityBatch();
            ShimWorker.AllInstances.BatchCheckBatch = (a, b) => true;
            ShimForJsonFunction<EntityBatch>();

            // Act
            var result = _testEntity.BatchCheck(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
