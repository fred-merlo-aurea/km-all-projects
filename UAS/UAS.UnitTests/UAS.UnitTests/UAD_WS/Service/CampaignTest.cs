using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityCampaign = FrameworkUAD.Entity.Campaign;
using ServiceCampaign = UAD_WS.Service.Campaign;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimCampaign;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignTest : Fakes
    {
        private ServiceCampaign _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceCampaign();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Select_ByAccessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCampaign>();
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
