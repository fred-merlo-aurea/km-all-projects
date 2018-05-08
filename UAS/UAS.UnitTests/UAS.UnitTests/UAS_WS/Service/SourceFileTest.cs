using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntitySourceFile = FrameworkUAS.Entity.SourceFile;
using ServiceSourceFile = UAS_WS.Service.SourceFile;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimSourceFile;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SourceFileTest : Fakes
    {
        private const string SampleString = "name2";
        private const int SampleId = 100;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceSourceFile _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSourceFile();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectBooleanBoolean = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, true, true);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Select_IncludeCustomProperties_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySourceFile>();
            ShimWorker.AllInstances.SelectBooleanBoolean = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, true, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySourceFile>();
            ShimWorker.AllInstances.SelectInt32Boolean = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectSpecialFiles_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySourceFile>();
            ShimWorker.AllInstances.SelectSpecialFilesInt32Boolean = (_, __, ___) => list;

            // Act
            var result = _testEntity.SelectSpecialFiles(Guid.Empty, SampleId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Delete_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32Int32 = (_, __, ___) => AffectedCountPositive;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId, SampleId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Delete_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32Int32 = (_, __, ___) => AffectedCountNegative;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId, SampleId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_WithIncludeCustomProperties_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySourceFile>();
            ShimWorker.AllInstances.SelectBoolean = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectSpecialFiles_WithIncludeCustomProperties_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySourceFile>();
            ShimWorker.AllInstances.SelectSpecialFilesBoolean = (_, __) => list;

            // Act
            var result = _testEntity.SelectSpecialFiles(Guid.Empty, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientIdAndIncludeCustomPropertiesAndIsDeleted_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySourceFile>();
            ShimWorker.AllInstances.SelectInt32BooleanBoolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientNameAndFileName_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySourceFile();
            ShimWorker.AllInstances.SelectInt32StringBoolean = (a, b, c, d) => entity;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleString, SampleString, true);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void SelectForSourceFile_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySourceFile();
            ShimWorker.AllInstances.SelectSourceFileIDInt32Boolean = (a, b, c) => entity;

            // Act
            var result = _testEntity.SelectForSourceFile(Guid.Empty, SampleId, true);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Save_WithEntity_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            var entity = new EntitySourceFile();
            ShimWorker.AllInstances.SaveSourceFileBoolean = (a, b, c) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;
            ShimForJsonFunction<EntitySourceFile>();

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
        public void IsFileNameUnique_ByClientIdAndFileName_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.IsFileNameUniqueInt32String = (a, b, c) => true;

            // Act
            var result = _testEntity.IsFileNameUnique(Guid.Empty, SampleId, SampleString);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
