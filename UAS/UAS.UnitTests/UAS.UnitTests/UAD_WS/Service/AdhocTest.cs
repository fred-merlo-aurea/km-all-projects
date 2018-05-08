using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityAdhoc = FrameworkUAD.Entity.Adhoc;
using ServiceAdhoc = UAD_WS.Service.Adhoc;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimAdhoc;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdhocTest : Fakes
    {
        private const int AdhocId = 100;
        private const int CategoryId = 200;
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceAdhoc _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAdhoc();
        }

        [Test]
        public void Delete_AdHoc_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.Delete_AdHocInt32ClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Delete_AdHoc(Guid.Empty, new ClientConnections(), AdhocId);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Delete_AdHoc_ByAdhocId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.Delete_AdHocInt32ClientConnections = (_, id, ___) =>
            {
                parameterId = id;
                return true;
            };

            // Act
            var result = _testEntity.Delete_AdHoc(Guid.Empty, new ClientConnections(), AdhocId);

            // Assert
            VerifySuccessResponse(result, true);
            parameterId.ShouldBe(AdhocId);
        }

        [Test]
        public void Delete_ByCategoryId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteInt32ClientConnections = (_, id, ___) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, new ClientConnections(), CategoryId);

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(CategoryId);
        }

        [Test]
        public void SelectCategoryID_ByCategoryId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdhoc>();
            ShimWorker.AllInstances.SelectCategoryIDInt32ClientConnectionsInt32Int32 = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.SelectCategoryID(Guid.Empty, new ClientConnections(), CategoryId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_ByEntityAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAdhoc();
            ShimWorker.AllInstances.SaveAdhocClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityAdhoc>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_ByEntityAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityAdhoc();
            ShimWorker.AllInstances.SaveAdhocClientConnections = (a, b, c) => AffectedCountNegative;

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void SelectAll_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdhoc>();
            ShimWorker.AllInstances.SelectAllClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.SelectAll(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
