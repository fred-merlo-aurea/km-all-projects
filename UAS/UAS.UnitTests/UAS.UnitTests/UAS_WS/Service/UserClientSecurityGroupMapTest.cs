using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityUserClientSecurityGroupMap = KMPlatform.Entity.UserClientSecurityGroupMap;
using ServiceUserClientSecurityGroupMap = UAS_WS.Service.UserClientSecurityGroupMap;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimUserClientSecurityGroupMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserClientSecurityGroupMapTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceUserClientSecurityGroupMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceUserClientSecurityGroupMap();
        }

        [Test]
        public void SelectForUser_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectForUserInt32 = (a, b) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SelectForUser(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SelectForUser_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityUserClientSecurityGroupMap>();
            ShimWorker.AllInstances.SelectForUserInt32 = (a, b) => list;

            // Act
            var result = _testEntity.SelectForUser(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForClient_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityUserClientSecurityGroupMap>();
            ShimWorker.AllInstances.SelectForClientInt32 = (a, b) => list;

            // Act
            var result = _testEntity.SelectForClient(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
