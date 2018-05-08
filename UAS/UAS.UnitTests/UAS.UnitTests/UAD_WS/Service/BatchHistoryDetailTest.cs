using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityBatchHistoryDetail = FrameworkUAD.Object.BatchHistoryDetail;
using ServiceBatchHistoryDetail = UAD_WS.Service.BatchHistoryDetail;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimBatchHistoryDetail;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BatchHistoryDetailTest : Fakes
    {
        private const int SampleId = 100;
        private const string ClientName = "name1";

        private ServiceBatchHistoryDetail _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceBatchHistoryDetail();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectInt32BooleanClientConnectionsString = (a, b, c, d, e) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true, new ClientConnections(), ClientName);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Select_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatchHistoryDetail> { new EntityBatchHistoryDetail() };
            ShimWorker.AllInstances.SelectInt32BooleanClientConnectionsString = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true, new ClientConnections(), ClientName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientName_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatchHistoryDetail> { new EntityBatchHistoryDetail() };
            ShimWorker.AllInstances.SelectClientConnectionsString = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections(), ClientName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_BySubscriptionId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatchHistoryDetail> { new EntityBatchHistoryDetail() };
            ShimWorker.AllInstances.SelectInt32ClientConnectionsString = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, new ClientConnections(), ClientName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForSubscriber_BySubscriptionId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatchHistoryDetail> { new EntityBatchHistoryDetail() };
            ShimWorker.AllInstances.SelectSubscriberInt32ClientConnectionsString = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SelectForSubscriber(Guid.Empty, SampleId, new ClientConnections(), ClientName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectBatch_ByBatchId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatchHistoryDetail> { new EntityBatchHistoryDetail() };
            ShimWorker.AllInstances.SelectBatchInt32StringInt32ClientConnectionsString = (a, b, c, d, e, f) => list;

            // Act
            var result = _testEntity.SelectBatch(
                Guid.Empty,
                SampleId,
                ClientName,
                SampleId,
                new ClientConnections(),
                ClientName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectBatch_ByBatchIdAndDateRange_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityBatchHistoryDetail> { new EntityBatchHistoryDetail() };
            ShimWorker.AllInstances.SelectBatchInt32StringInt32DateTimeDateTimeClientConnectionsString = (a, b, c, d, e, f, g, h) => list;

            // Act
            var result = _testEntity.SelectBatch(
                Guid.Empty,
                SampleId,
                ClientName,
                SampleId,
                DateTime.MinValue,
                DateTime.MaxValue,
                new ClientConnections(),
                ClientName);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
