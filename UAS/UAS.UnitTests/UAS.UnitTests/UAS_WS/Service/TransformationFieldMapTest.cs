using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityTransformationFieldMap = FrameworkUAS.Entity.TransformationFieldMap;
using ServiceTransformationFieldMap = UAS_WS.Service.TransformationFieldMap;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformationFieldMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TransformationFieldMapTest : Fakes
    {
        private const int SampleId = 100;
        private const string SampleString = "name1";
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceTransformationFieldMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceTransformationFieldMap();
        }

        [Test]
        public void DeleteFieldMapping_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteFieldMappingIDInt32 = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.DeleteFieldMapping(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void DeleteFieldMapping_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteFieldMappingIDInt32 = (_, __) => AffectedCountPositive;

            // Act
            var result = _testEntity.DeleteFieldMapping(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void DeleteFieldMapping_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteFieldMappingIDInt32 = (_, __) => AffectedCountNegative;

            // Act
            var result = _testEntity.DeleteFieldMapping(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void DeleteSourceFile_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteSourceFileIDInt32 = (_, __) => AffectedCountPositive;

            // Act
            var result = _testEntity.DeleteSourceFile(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void DeleteSourceFile_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteSourceFileIDInt32 = (_, __) => AffectedCountNegative;

            // Act
            var result = _testEntity.DeleteSourceFile(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Delete_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteStringInt32String = (a, b, c, d) => AffectedCountPositive;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleString, SampleId, SampleString);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Delete_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteStringInt32String = (a, b, c, d) => AffectedCountNegative;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleString, SampleId, SampleString);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformationFieldMap>();
            ShimWorker.AllInstances.Select = _ => list;

            // Act
            var result = _testEntity.Select(Guid.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByTransformationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformationFieldMap>();
            ShimWorker.AllInstances.SelectTransformationIDInt32 = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Save_WithEntity_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            var entity = new EntityTransformationFieldMap();
            ShimWorker.AllInstances.SaveTransformationFieldMap = (_, __) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;
            ShimForJsonFunction<EntityTransformationFieldMap>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

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

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteTransformationFieldMapping_ByClientIdAndFiledMappingId_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            ShimWorker.AllInstances.DeleteFieldMappingIDStringInt32Int32 = (a, b, c, d) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;

            // Act
            var result = _testEntity.DeleteTransformationFieldMapping(Guid.Empty, SampleString, SampleId, SampleId);

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
