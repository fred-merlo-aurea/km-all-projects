using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntitySubscriberDemographicTransformed = FrameworkUAD.Entity.SubscriberDemographicTransformed;
using ServiceSubscriberDemographicTransformed = UAD_WS.Service.SubscriberDemographicTransformed;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberDemographicTransformed;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberDemographicTransformedTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceSubscriberDemographicTransformed _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSubscriberDemographicTransformed();
        }

        [Test]
        public void SelectPublication_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectPublicationInt32ClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SelectPublication(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SelectPublication_ByPubId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberDemographicTransformed>();
            ShimWorker.AllInstances.SelectPublicationInt32ClientConnections = (_, __, ___) => list;

            // Act
            var result = _testEntity.SelectPublication(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectSubscriberTransformed_ByStRecordId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberDemographicTransformed>();
            ShimWorker.AllInstances.SelectSubscriberTransformedGuidClientConnections = (_, __, ___) => list;

            // Act
            var result = _testEntity.SelectSubscriberTransformed(Guid.Empty, Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
