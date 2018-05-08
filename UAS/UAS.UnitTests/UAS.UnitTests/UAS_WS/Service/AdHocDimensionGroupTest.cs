using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityAdHocDimensionGroup = FrameworkUAS.Entity.AdHocDimensionGroup;
using ServiceAdhocDimensionGroup = UAS_WS.Service.AdHocDimensionGroup;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimAdHocDimensionGroup;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdHocDimensionGroupTest : Fakes
    {
        private const int SampleId = 100;
        private const string SampleName = "name1";

        private ServiceAdhocDimensionGroup _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAdhocDimensionGroup();
        }

        [Test]
        public void SaveBulkSqlInsert_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroup>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfAdHocDimensionGroup = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.NewGuid(), list);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SaveBulkSqlInsert_ByClientIdAndUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroup>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfAdHocDimensionGroup = (_, __) => false;

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.NewGuid(), list);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroup>();
            ShimWorker.AllInstances.SelectBoolean = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.NewGuid(), true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroup>();
            ShimWorker.AllInstances.SelectInt32Boolean = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.NewGuid(), SampleId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientIdAndGroupName_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroup>();
            ShimWorker.AllInstances.SelectInt32StringBoolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.NewGuid(), SampleId, SampleName, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientIdAndFileIdAndGroupName_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAdHocDimensionGroup();
            ShimWorker.AllInstances.SelectInt32Int32StringBoolean = (a, b, c, d, e) => entity;

            // Act
            var result = _testEntity.Select(Guid.NewGuid(), SampleId, SampleId, SampleName, true);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Select_ByClientIdAndFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroup>();
            ShimWorker.AllInstances.SelectInt32Int32Boolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.NewGuid(), SampleId, SampleId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectByAdHocDimensionGroupId_ByGroupId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAdHocDimensionGroup();
            ShimWorker.AllInstances.SelectByAdHocDimensionGroupIdInt32Boolean = (a, b, c) => entity;

            // Act
            var result = _testEntity.SelectByAdHocDimensionGroupId(Guid.NewGuid(), SampleId, true);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAdHocDimensionGroup();
            ShimWorker.AllInstances.SaveAdHocDimensionGroup = (a, b) => true;
            ShimForJsonFunction<EntityAdHocDimensionGroup>();

            // Act
            var result = _testEntity.Save(Guid.NewGuid(), entity);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
