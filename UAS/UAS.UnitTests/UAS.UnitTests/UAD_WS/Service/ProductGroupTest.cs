using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityProductGroup = FrameworkUAD.Entity.ProductGroup;
using ServiceProductGroup = UAD_WS.Service.ProductGroup;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimProductGroup;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductGroupTest : Fakes
    {
        private const int PublicationId = 100;
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceProductGroup _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceProductGroup();
        }

        [Test]
        public void Delete_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Delete(Guid.Empty, PublicationId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Delete_ByPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, id) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, PublicationId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(PublicationId);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductGroup>();
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Save_ByAccessKey_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            var entity = new EntityProductGroup();
            ShimWorker.AllInstances.SaveProductGroupClientConnections = (_, __, ___) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;
            ShimForJsonFunction<EntityProductGroup>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            if (workerSuccess)
            {
                VerifySuccessResponse(result, AffectedCountPositive);
            }
            else
            {
                VerifyErrorResponse(result, AffectedCountNegative);
            }
        }
    }
}
