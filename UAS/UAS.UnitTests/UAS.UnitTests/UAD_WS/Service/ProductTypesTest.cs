using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityProductTypes = FrameworkUAD.Entity.ProductTypes;
using ServiceProductTypes= UAD_WS.Service.ProductTypes;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimProductTypes;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductTypesTest : Fakes
    {
        private const int TypeId = 100;
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceProductTypes _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceProductTypes();
        }

        [Test]
        public void Delete_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Delete(Guid.Empty, TypeId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Delete_ByTypeId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, id) =>
            {
                parameterId = id;
                return true;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, TypeId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
            parameterId.ShouldBe(TypeId);
        }

        [Test]
        public void Save_WithEntityAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProductTypes();
            ShimWorker.AllInstances.SaveProductTypesClientConnections = (_, __, ___) => AffectedCountPositive;
            ShimForJsonFunction<EntityProductTypes>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WithEntityAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityProductTypes();
            ShimWorker.AllInstances.SaveProductTypesClientConnections = (_, __, ___) => AffectedCountNegative;
            ShimForJsonFunction<EntityProductTypes>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductTypes>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
