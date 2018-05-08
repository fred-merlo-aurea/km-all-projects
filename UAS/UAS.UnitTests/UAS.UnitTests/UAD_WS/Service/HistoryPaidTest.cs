using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityHistoryPaid = FrameworkUAD.Entity.HistoryPaid;
using EntitySubscriptionPaid = FrameworkUAD.Entity.SubscriptionPaid;
using ServiceHistoryPaid = UAD_WS.Service.HistoryPaid;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimHistoryPaid;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryPaidTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int SampleId = 100;

        private ServiceHistoryPaid _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceHistoryPaid();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectInt32ClientConnections = (a, b, c) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityHistoryPaid>();
            ShimWorker.AllInstances.SelectInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntityAndUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySubscriptionPaid();
            ShimWorker.AllInstances.SaveSubscriptionPaidInt32ClientConnections = (a, b, c, d) => AffectedCountPositive;
            ShimForJsonFunction<EntitySubscriptionPaid>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityHistoryPaid();
            ShimWorker.AllInstances.SaveHistoryPaidClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityHistoryPaid>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }
    }
}
