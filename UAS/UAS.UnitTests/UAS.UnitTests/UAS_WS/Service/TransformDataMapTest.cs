using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityTransformDataMap = FrameworkUAS.Entity.TransformDataMap;
using ServiceTransformDataMap = UAS_WS.Service.TransformDataMap;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformDataMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TransformDataMapTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceTransformDataMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceTransformDataMap();
        }

        [Test]
        public void SelectForSourceFile_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectSourceFileIDInt32 = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SelectForSourceFile(Guid.Empty, SampleId);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SelectForSourceFile_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformDataMap>();
            ShimWorker.AllInstances.SelectSourceFileIDInt32 = (_, __) => list;

            // Act
            var result = _testEntity.SelectForSourceFile(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Delete_ByDataMapId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformDataMap>();
            ShimWorker.AllInstances.DeleteInt32 = (_, __) => list;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
