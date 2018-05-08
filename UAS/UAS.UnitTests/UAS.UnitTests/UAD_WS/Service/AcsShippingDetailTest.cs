using System;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityAcsShippingDetail = FrameworkUAD.Entity.AcsShippingDetail;
using ServiceAcsShippingDetail = UAD_WS.Service.AcsShippingDetail;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimAcsShippingDetail;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AcsShippingDetailTest : Fakes
    {
        private const int AffectedCountPositive = 1;

        private ServiceAcsShippingDetail _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAcsShippingDetail();
        }

        [Test]
        public void Save_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityAcsShippingDetail();
            ShimWorker.AllInstances.SaveAcsShippingDetailClientConnections = (a, b, c) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityAcsShippingDetail>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAcsShippingDetail();
            ShimWorker.AllInstances.SaveAcsShippingDetailClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityAcsShippingDetail>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }
    }
}
