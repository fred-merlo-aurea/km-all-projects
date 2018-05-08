using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceProduct = UAD_WS.Service.Product;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimProduct;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityProduct = FrameworkUAD.Entity.Product;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductTest : Fakes
    {
        private const int UserId = 100;
        private const int FromId = 200;
        private const int ToId = 300;
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceProduct _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceProduct();
        }

        [Test]
        public void UpdateLock_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateLockClientConnectionsInt32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.UpdateLock(Guid.Empty, new ClientConnections(), UserId);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void UpdateLock_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.UpdateLockClientConnectionsInt32 = (_, __, id) =>
            {
                parameterId = id;
                return true;
            };

            // Act
            var result = _testEntity.UpdateLock(Guid.Empty, new ClientConnections(), UserId);

            // Assert
            VerifySuccessResponse(result, true);
            parameterId.ShouldBe(UserId);
        }

        [Test]
        public void Copy_ByFromIdAndToId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterFromId = 0;
            var parameterToId = 0;
            ShimWorker.AllInstances.CopyClientConnectionsInt32Int32 = (_, __, fromId, toId) =>
            {
                parameterFromId = fromId;
                parameterToId = toId;
                return false;
            };

            // Act
            var result = _testEntity.Copy(Guid.Empty, FromId, ToId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
            parameterFromId.ShouldBe(FromId);
            parameterToId.ShouldBe(ToId);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProduct>();
            ShimWorker.AllInstances.SelectClientConnectionsBoolean = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByPubId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProduct();
            ShimWorker.AllInstances.SelectInt32ClientConnectionsBooleanBoolean = (a, b, c, d, e) => entity;

            // Act
            var result = _testEntity.Select(Guid.Empty, UserId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Save_WithEntity_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            var entity = new EntityProduct();
            ShimWorker.AllInstances.SaveProductClientConnections = (a, b, c) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;
            ShimForJsonFunction<EntityProduct>();

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
