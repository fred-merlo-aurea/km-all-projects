using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityAdHocDimension = FrameworkUAS.Entity.AdHocDimension;
using ServiceAdhocDimension = UAS_WS.Service.AdHocDimension;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimAdHocDimension;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdHocDimensionTest : Fakes
    {
        private const int FileId = 100;

        private ServiceAdhocDimension _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAdhocDimension();
        }

        [Test]
        public void Delete_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32 = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Delete(Guid.Empty, FileId);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Delete_ByFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteInt32 = (_, id) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, FileId);

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(FileId);
        }

        [Test]
        public void Select_ByGroupId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimension>();
            ShimWorker.AllInstances.SelectInt32 = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, FileId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SaveBulkSqlInsert_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimension>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (_, __) => true;
            ShimForJsonFunction<List<EntityAdHocDimension>>();

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, list);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
