using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityTransformationPubMap = FrameworkUAS.Entity.TransformationPubMap;
using ServiceTransformationPubMap = UAS_WS.Service.TransformationPubMap;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformationPubMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TransformationPubMapTest : Fakes
    {
        private const int SampleId = 100;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceTransformationPubMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceTransformationPubMap();
        }

        [Test]
        public void Delete_ByPublicationIdAndWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32Int32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId, SampleId);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Delete_ByPublicationIdAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32Int32 = (_, __, ___) => AffectedCountPositive;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Delete_ByPublicationIdAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32Int32 = (_, __, ___) => AffectedCountNegative;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Delete_ByTransformationIdAndWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32 = (_, __) => AffectedCountPositive;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Delete_ByTransformationIdAndWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32 = (_, __) => AffectedCountNegative;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformationPubMap>();
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
            var list = new List<EntityTransformationPubMap>();
            ShimWorker.AllInstances.SelectInt32 = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
