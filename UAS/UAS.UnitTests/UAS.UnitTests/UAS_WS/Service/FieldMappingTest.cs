using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityFieldMapping = FrameworkUAS.Entity.FieldMapping;
using ServiceFieldMapping = UAS_WS.Service.FieldMapping;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimFieldMapping;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FieldMappingTest : Fakes
    {
        private const int SampleId = 100;
        private const string SampleName = "name1";
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceFieldMapping _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceFieldMapping();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntityFieldMapping>();
            ShimWorker.AllInstances.SelectStringBoolean = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleName);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Select_ByClientName_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFieldMapping>();
            ShimWorker.AllInstances.SelectStringBoolean = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientIdAndFileName_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFieldMapping>();
            ShimWorker.AllInstances.SelectInt32StringBoolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, SampleName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntityAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityFieldMapping();
            ShimWorker.AllInstances.SaveFieldMapping = (a, b) => AffectedCountPositive;
            ShimForJsonFunction<EntityFieldMapping>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WithEntityAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityFieldMapping();
            ShimWorker.AllInstances.SaveFieldMapping = (a, b) => AffectedCountNegative;
            ShimForJsonFunction<EntityFieldMapping>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void ColumnReorder_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.ColumnReorderInt32 = (a, b) => false;

            // Act
            var result = _testEntity.ColumnReorder(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Delete_BySourceFileIdAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32 = (a, b) => AffectedCountPositive;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Delete_BySourceFileIdAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32 = (a, b) => AffectedCountNegative;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void DeleteMapping_ByMappingIdAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteMappingInt32 = (a, b) => AffectedCountPositive;

            // Act
            var result = _testEntity.DeleteMapping(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void DeleteMapping_ByMappingIdAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteMappingInt32 = (a, b) => AffectedCountNegative;

            // Act
            var result = _testEntity.DeleteMapping(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFieldMapping>();
            ShimWorker.AllInstances.SelectBoolean = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFieldMapping>();
            ShimWorker.AllInstances.SelectInt32Boolean = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForFieldMapping_ByFieldMappingId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityFieldMapping();
            ShimWorker.AllInstances.SelectFieldMappingIDInt32Boolean = (a, b, c) => entity;

            // Act
            var result = _testEntity.SelectForFieldMapping(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, entity);
        }
    }
}
