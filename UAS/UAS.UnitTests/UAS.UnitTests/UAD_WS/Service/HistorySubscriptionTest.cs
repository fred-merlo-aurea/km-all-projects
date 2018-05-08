using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityHistorySubscription = FrameworkUAD.Entity.HistorySubscription;
using EntityProductSubscription = FrameworkUAD.Entity.ProductSubscription;
using ServiceHistorySubscription = UAD_WS.Service.HistorySubscription;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimHistorySubscription;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistorySubscriptionTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int SampleId = 100;

        private ServiceHistorySubscription _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceHistorySubscription();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityHistorySubscription>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityHistorySubscription();
            ShimWorker.AllInstances.SaveHistorySubscriptionClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityHistorySubscription>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SaveForSubscriber_WithEntityAndUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProductSubscription();
            ShimWorker.AllInstances.SaveProductSubscriptionInt32ClientConnections = (a, b, c, d) => AffectedCountPositive;
            ShimForJsonFunction<EntityProductSubscription>();

            // Act
            var result = _testEntity.SaveForSubscriber(Guid.Empty, entity, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }
    }
}
