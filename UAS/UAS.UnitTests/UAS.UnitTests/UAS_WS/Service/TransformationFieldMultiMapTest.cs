using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityTransformationFieldMultiMap = FrameworkUAS.Entity.TransformationFieldMultiMap;
using ServiceTransformationFieldMultiMap = UAS_WS.Service.TransformationFieldMultiMap;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformationFieldMultiMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TransformationFieldMultiMapTest : Fakes
    {
        private const int SampleId = 100;
        private const int SourceFileId = 200;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceTransformationFieldMultiMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceTransformationFieldMultiMap();
        }

        [Test]
        public void DeleteByFieldMultiMapID_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteByFieldMultiMapIDInt32 = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.DeleteByFieldMultiMapID(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void DeleteByFieldMultiMapID_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteByFieldMultiMapIDInt32 = (_, __) => AffectedCountPositive;

            // Act
            var result = _testEntity.DeleteByFieldMultiMapID(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void DeleteByFieldMultiMapID_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteByFieldMultiMapIDInt32 = (_, __) => AffectedCountNegative;

            // Act
            var result = _testEntity.DeleteByFieldMultiMapID(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void DeleteBySourceFileID_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteBySourceFileIDInt32 = (_, __) => AffectedCountPositive;

            // Act
            var result = _testEntity.DeleteBySourceFileID(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void DeleteBySourceFileIDAndFieldMultiMapID_BySourceFileIdAndMapId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteBySourceFileIDAndFieldMultiMapIDInt32Int32 = (_, __, ___) => AffectedCountPositive;

            // Act
            var result = _testEntity.DeleteBySourceFileIDAndFieldMultiMapID(Guid.Empty, SourceFileId, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void DeleteByFieldMappingID_ByMappingId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteByFieldMappingIDInt32 = (_, __) => AffectedCountPositive;

            // Act
            var result = _testEntity.DeleteByFieldMappingID(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformationFieldMultiMap>();
            ShimWorker.AllInstances.Select = _ => list;

            // Act
            var result = _testEntity.Select(Guid.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectTransformationID_ByTransformationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformationFieldMultiMap>();
            ShimWorker.AllInstances.SelectTransformationIDInt32 = (_, __) => list;

            // Act
            var result = _testEntity.SelectTransformationID(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
