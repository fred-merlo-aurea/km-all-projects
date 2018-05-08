using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityAdhocCategory = FrameworkUAD.Entity.AdhocCategory;
using ServiceAdhocCategory = UAD_WS.Service.AdhocCategory;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimAdhocCategory;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdhocCategoryTest : Fakes
    {
        private const int AffectedCountPositive = 1;

        private ServiceAdhocCategory _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAdhocCategory();
        }

        [Test]
        public void Save_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityAdhocCategory();
            ShimWorker.AllInstances.SaveAdhocCategoryClientConnections = (a, b, c) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityAdhocCategory>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAdhocCategory();
            ShimWorker.AllInstances.SaveAdhocCategoryClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityAdhocCategory>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SelectAll_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdhocCategory>();
            ShimWorker.AllInstances.SelectAllClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.SelectAll(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
