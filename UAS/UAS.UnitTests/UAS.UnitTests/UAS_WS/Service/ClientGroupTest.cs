using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityClientGroup = KMPlatform.Entity.ClientGroup;
using ServiceClientGroup = UAS_WS.Service.ClientGroup;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimClientGroup;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClientGroupTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceClientGroup _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceClientGroup();
        }

        [Test]
        public void SelectClient_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectClientInt32Boolean = (a, b, c) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SelectClient(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SelectClient_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityClientGroup>();
            ShimWorker.AllInstances.SelectClientInt32Boolean = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectClient(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForUserAuthorization_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityClientGroup>();
            ShimWorker.AllInstances.SelectForUserAuthorizationInt32Boolean = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectForUserAuthorization(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
